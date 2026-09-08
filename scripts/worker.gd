extends CharacterBody3D

const Preferences = preload("res://scripts/preferences.gd")
const Bevel = preload("res://scripts/bevel_mesh.gd")
const Art = preload("res://scripts/postal_art.gd")

var map_layout = null
var peer_id := 0
var slot := 1
const SUITS := [Color("bd8840"), Color("538e87"), Color("857797"), Color("af6860")]
const SPAWNS := [Vector3(-1, 0.05, 3), Vector3(0.8, 0.05, 3), Vector3(-1, 0.05, 5.3), Vector3(0.8, 0.05, 5.3)]

func spawn_position() -> Vector3:
	return map_layout.worker_spawn(slot) if is_instance_valid(map_layout) else SPAWNS[clampi(slot, 1, 4) - 1]
var heading := 0.0
var look_yaw := 0.0
var look_pitch := -0.22
var local := false
var held := false
var visual: Node3D
var rig: Node3D
var camera: Camera3D
var arms: Array[MeshInstance3D] = []
var legs: Array[Node3D] = []
var nameplate: Label3D
var gait := 0.0
var walk_velocity := Vector3.ZERO
var push_velocity := Vector3.ZERO
var stagger := 0.0
var speed_scale := 1.0
var carry_blend := 0.0
var art_player: AnimationPlayer
var art_skeleton: Skeleton3D

func _ready() -> void:
	collision_layer = 2
	collision_mask = 7
	var shape := CollisionShape3D.new()
	var capsule := CapsuleShape3D.new()
	capsule.radius = 0.32
	capsule.height = 1.65
	shape.shape = capsule
	shape.position.y = 0.83
	add_child(shape)
	visual = Node3D.new()
	add_child(visual)
	var suit: Color = SUITS[clampi(slot, 1, 4) - 1]
	var character := Art.model("worker"); visual.add_child(character)
	art_player = character.find_children("*","AnimationPlayer",true,false)[0]
	art_skeleton = character.find_children("*","Skeleton3D",true,false)[0]
	art_player.callback_mode_process = AnimationMixer.ANIMATION_CALLBACK_MODE_PROCESS_MANUAL
	for clip in art_player.get_animation_list():
		if clip == "RESET": continue
		art_player.get_animation(clip).loop_mode = Animation.LOOP_LINEAR
		art_player.play(clip); art_player.advance(0); break
	# Numbered, colored back badge keeps the four crew members distinguishable.
	var badge := _part(Vector3(0.34,0.20,0.035),Vector3(0,1.10,0.24),suit)
	badge.name = "CrewBadge"
	nameplate = Label3D.new()
	nameplate.text = "%02d / %s" % [slot, "HOST" if peer_id == 1 else "CREW"]
	nameplate.font_size = 36
	nameplate.pixel_size = 0.006
	nameplate.position.y = 2.06
	nameplate.billboard = BaseMaterial3D.BILLBOARD_ENABLED
	nameplate.visibility_range_end = 18
	add_child(nameplate)

func _rounded(size: Vector3, at: Vector3, color: Color) -> MeshInstance3D:
	var part := _part(size,at,color)
	var capsule := CapsuleMesh.new(); capsule.radius = size.x/2; capsule.height = size.y; capsule.radial_segments = 12; capsule.rings = 4
	part.mesh = capsule; part.scale.z = size.z/size.x
	return part

func _part(size: Vector3, at: Vector3, color: Color) -> MeshInstance3D:
	var mesh := MeshInstance3D.new()
	mesh.mesh = Bevel.make(size,minf(0.025,minf(size.x,minf(size.y,size.z))*0.16))
	var material := StandardMaterial3D.new()
	material.albedo_color = color
	material.roughness = 0.85
	material.metallic_specular = 0.15
	mesh.material_override = material
	mesh.position = at
	visual.add_child(mesh)
	return mesh

func make_local() -> void:
	if local:
		return
	local = true
	rig = Node3D.new()
	rig.position = Vector3(0, 1.65, 0)
	add_child(rig)
	var arm := SpringArm3D.new()
	arm.spring_length = 5.8
	arm.margin = 0.25
	arm.collision_mask = 1
	var sweep := SphereShape3D.new()
	sweep.radius = 0.20
	arm.shape = sweep
	rig.add_child(arm)
	camera = Camera3D.new()
	camera.fov = 72
	arm.add_child(camera)
	camera.current = true
	nameplate.visible = false
	update_look()

func aim(relative: Vector2) -> void:
	look_yaw = wrapf(look_yaw - relative.x * 0.0025 * Preferences.sensitivity, -PI, PI)
	look_pitch = clampf(look_pitch - relative.y * 0.0025 * Preferences.sensitivity * (-1.0 if Preferences.invert else 1.0), -0.85, 0.25)
	update_look()

func update_look() -> void:
	if rig:
		rig.rotation = Vector3(look_pitch, look_yaw, 0)

func simulate(movement: Vector2, yaw: float, jump: bool, delta: float, drift: Vector3 = Vector3.ZERO) -> void:
	heading = yaw
	var wish := Vector3(movement.x, 0, movement.y).rotated(Vector3.UP, yaw) * 4.5 * speed_scale
	walk_velocity = walk_velocity.move_toward(wish, 24.0 * delta)
	var grounded_drift := drift if is_on_floor() else Vector3.ZERO
	velocity.x = walk_velocity.x + push_velocity.x + grounded_drift.x
	velocity.z = walk_velocity.z + push_velocity.z + grounded_drift.z
	push_velocity = push_velocity.move_toward(Vector3.ZERO, 14.0 * delta)
	stagger = maxf(0, stagger - delta)
	if not is_on_floor():
		velocity.y -= 18.0 * delta
	elif jump:
		velocity.y = maxf(velocity.y,6.5)
	move_and_slide()
	if position.y < -5.0:
		position = spawn_position()
		reset_motion()

func apply_push(direction: Vector3) -> void:
	push_velocity = (push_velocity + direction * 7.0).limit_length(12.0)
	velocity.y = maxf(velocity.y, 3.8)
	stagger = 0.5

func reset_motion() -> void:
	speed_scale = 1.0
	velocity = Vector3.ZERO
	walk_velocity = Vector3.ZERO
	push_velocity = Vector3.ZERO
	stagger = 0

func hand_position() -> Vector3:
	return position + Vector3(0, 1.05, 0) + forward() * 0.98

func forward() -> Vector3:
	return Vector3.FORWARD.rotated(Vector3.UP, heading)

func _process(delta: float) -> void:
	if not visual:
		return
	# Held cargo already uses authoritative heading: do not leave hands facing
	# the previous direction during a quick turn.
	visual.rotation.y = heading if held else lerp_angle(visual.rotation.y, heading, minf(delta * 16.0, 1.0))
	carry_blend = move_toward(carry_blend,1.0 if held else 0.0,delta*9.0)
	gait += delta * Vector2(velocity.x, velocity.z).length() * 2.5
	visual.position.y = absf(sin(gait)) * minf(velocity.length() * 0.008, 0.035)
	visual.rotation.z = sin(stagger * PI * 2) * 0.35
	var stride := clampf(Vector2(velocity.x,velocity.z).length()/4.5,0,1)
	if art_player:
		art_skeleton.clear_bones_global_pose_override()
		art_player.advance(delta*stride*1.5)
		if stride < 0.02:
			# A stopped courier plants both feet instead of freezing mid-stride.
			for prefix in ["Left","Right"]:
				for suffix in ["UpLeg","Leg","Foot","ToeBase"]:
					var bone := art_skeleton.find_bone(prefix+suffix)
					if bone >= 0: art_skeleton.reset_bone_pose(bone)
		if carry_blend > 0.01:
			for side in [-1.0,1.0]:
				var prefix := "Left" if side < 0 else "Right"
				_aim_arm(prefix+"Arm",Vector3(side*0.38,1.04,-0.32),carry_blend)
				_aim_arm(prefix+"ForeArm",Vector3(side*0.38,1.04,-0.70),carry_blend)
	for index in range(arms.size()):
		var arm := arms[index]
		arm.rotation.x = lerpf(sin(gait+index*PI)*0.45*stride,1.25,carry_blend)
		arm.position.z = -0.2*carry_blend
	for index in range(legs.size()): legs[index].rotation.x = -sin(gait+index*PI)*0.5*stride

func _aim_arm(bone_name: String, toward: Vector3, weight: float) -> void:
	var index := art_skeleton.find_bone(bone_name)
	if index < 0: return
	var pose := art_skeleton.get_bone_global_pose(index)
	var target := art_skeleton.to_local(visual.to_global(toward))
	var direction := (target-pose.origin).normalized()
	pose.basis = Basis(Quaternion(pose.basis.y.normalized(),direction))*pose.basis
	art_skeleton.set_bone_global_pose_override(index,pose,weight,true)

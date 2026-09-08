extends CharacterBody3D

const Preferences = preload("res://scripts/preferences.gd")
const Bevel = preload("res://scripts/bevel_mesh.gd")

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
	_rounded(Vector3(0.62,0.78,0.44),Vector3(0,0.94,0),suit)
	var head := _part(Vector3.ONE,Vector3(0,1.49,0),Color("c6b79b"))
	var sphere := SphereMesh.new(); sphere.radius = 0.245; sphere.height = 0.38; sphere.radial_segments = 12; sphere.rings = 6; head.mesh = sphere
	_part(Vector3(0.48,0.11,0.47),Vector3(0,1.65,0),suit.darkened(0.25))
	_part(Vector3(0.5,0.035,0.18),Vector3(0,1.60,-0.25),suit.darkened(0.25))
	for x in [-0.09,0.09]: _part(Vector3(0.045,0.065,0.025),Vector3(x,1.49,-0.23),Color("243333"))
	_part(Vector3(0.52,0.07,0.45),Vector3(0,0.89,0),Color("d4caa6"))
	_part(Vector3(0.25,0.2,0.045),Vector3(-0.12,1.13,-0.245),suit.darkened(0.18))
	_part(Vector3(0.025,0.46,0.02),Vector3(0,1.06,-0.265),Color("364441"))
	_part(Vector3(0.18,0.13,0.025),Vector3(0.17,1.2,-0.24),Color("d4caa6"))
	var number := Label3D.new(); number.name = "EmployeeNumber"; number.text = "%02d" % slot; number.font_size = 42; number.pixel_size = 0.004
	_part(Vector3(0.32,0.24,0.025),Vector3(0,1.13,0.215),Color("344a46"))
	number.position = Vector3(0,1.13,0.231); number.modulate = Color("e0d8bf"); number.outline_size = 0; number.visibility_range_end = 15; visual.add_child(number)
	for side in [-1.0, 1.0]:
		var arm := _rounded(Vector3(0.19,0.54,0.22),Vector3(side*0.4,0.98,0),suit)
		arms.append(arm)
		var glove := _part(Vector3(0.20,0.18,0.23),Vector3.ZERO,Color("d3c9ad")); glove.reparent(arm,false); glove.position = Vector3(0,-0.27,0)
		var leg := Node3D.new(); leg.name = "LegLeft" if side < 0 else "LegRight"; visual.add_child(leg); leg.position = Vector3(side*0.17,0.57,0); legs.append(leg)
		var trouser := _rounded(Vector3(0.23,0.47,0.24),Vector3.ZERO,Color("364644")); trouser.reparent(leg,false); trouser.position.y = -0.2
		var boot := _part(Vector3(0.25,0.16,0.35),Vector3.ZERO,Color("243330")); boot.reparent(leg,false); boot.position = Vector3(0,-0.46,-0.035)
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
	for index in range(arms.size()):
		var arm := arms[index]
		arm.rotation.x = lerpf(sin(gait+index*PI)*0.45*stride,1.25,carry_blend)
		arm.position.z = -0.2*carry_blend
	for index in range(legs.size()): legs[index].rotation.x = -sin(gait+index*PI)*0.5*stride

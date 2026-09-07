extends CharacterBody3D

const Preferences = preload("res://scripts/preferences.gd")

var peer_id := 0
var slot := 1
const SUITS := [Color("eda941"), Color("5ccbc2"), Color("b99ee7"), Color("ef8f91")]
const SPAWNS := [Vector3(-1, 0.05, 3), Vector3(0.8, 0.05, 3), Vector3(-1, 0.05, 5.3), Vector3(0.8, 0.05, 5.3)]

func spawn_position() -> Vector3:
	return SPAWNS[clampi(slot, 1, 4) - 1]
var heading := 0.0
var look_yaw := 0.0
var look_pitch := -0.22
var local := false
var held := false
var visual: Node3D
var rig: Node3D
var camera: Camera3D
var arms: Array[MeshInstance3D] = []
var nameplate: Label3D
var gait := 0.0
var walk_velocity := Vector3.ZERO
var push_velocity := Vector3.ZERO
var stagger := 0.0
var speed_scale := 1.0

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
	_part(Vector3(0.64, 0.76, 0.42), Vector3(0, 0.91, 0), suit)
	_part(Vector3(0.48, 0.36, 0.42), Vector3(0, 1.50, 0), suit.lightened(0.2))
	_part(Vector3(0.37, 0.13, 0.05), Vector3(0, 1.50, -0.225), Color("132b3a"))
	_part(Vector3(0.22, 0.48, 0.25), Vector3(-0.19, 0.30, 0), Color("273e50"))
	_part(Vector3(0.22, 0.48, 0.25), Vector3(0.19, 0.30, 0), Color("273e50"))
	for side in [-1.0, 1.0]:
		arms.append(_part(Vector3(0.18, 0.60, 0.22), Vector3(side * 0.45, 0.93, 0), suit))
	nameplate = Label3D.new()
	nameplate.text = "%02d / %s" % [slot, "HOST" if peer_id == 1 else "CREW"]
	nameplate.font_size = 36
	nameplate.pixel_size = 0.006
	nameplate.position.y = 2.06
	nameplate.billboard = BaseMaterial3D.BILLBOARD_ENABLED
	add_child(nameplate)

func _part(size: Vector3, at: Vector3, color: Color) -> MeshInstance3D:
	var mesh := MeshInstance3D.new()
	var box := BoxMesh.new()
	box.size = size
	mesh.mesh = box
	var material := StandardMaterial3D.new()
	material.albedo_color = color
	material.roughness = 0.85
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
	look_pitch = clampf(look_pitch - relative.y * 0.0025 * Preferences.sensitivity, -0.85, 0.25)
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
		velocity.y = 6.5
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
	visual.rotation.y = lerp_angle(visual.rotation.y, heading, minf(delta * 16.0, 1.0))
	gait += delta * Vector2(velocity.x, velocity.z).length() * 2.5
	visual.position.y = absf(sin(gait)) * minf(velocity.length() * 0.008, 0.035)
	visual.rotation.z = sin(stagger * PI * 2) * 0.35
	for arm in arms:
		arm.rotation.x = -1.25 if held else sin(gait) * 0.12
		arm.position.z = -0.2 if held else 0.0

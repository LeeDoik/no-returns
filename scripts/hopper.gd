extends Node3D

const Copy = preload("res://scripts/copy.gd")
const REST_SECONDS := 4.0
const WINDUP_SECONDS := 1.0
const FORWARD_SPEED := 2.8
const LIFT_SPEED := 6.5

var cargo: Node3D
var phase := "rest"
var remaining := REST_SECONDS
var left_ground := false
var paused := false
var label: Label3D
var indicator: MeshInstance3D

func setup(source: Node3D) -> void:
	cargo = source
	label = Label3D.new()
	var font := SystemFont.new()
	font.font_names = PackedStringArray(["Malgun Gothic", "Segoe UI", "Arial"])
	label.font = font
	label.font_size = 24
	label.pixel_size = 0.006
	label.visibility_range_end = 13
	label.billboard = BaseMaterial3D.BILLBOARD_ENABLED
	label.modulate = Color("ffe792")
	add_child(label)
	indicator = MeshInstance3D.new()
	var arrow := PrismMesh.new()
	arrow.size = Vector3(0.34, 0.08, 0.55)
	indicator.mesh = arrow
	var material := StandardMaterial3D.new()
	material.albedo_color = Color("ffc857")
	material.shading_mode = BaseMaterial3D.SHADING_MODE_UNSHADED
	indicator.material_override = material
	add_child(indicator)
	reset()

func reset() -> void:
	phase = "rest"
	remaining = REST_SECONDS
	left_ground = false
	paused = false
	if is_instance_valid(cargo) and is_instance_valid(cargo.visual):
		cargo.visual.scale = Vector3.ONE

func snapshot() -> Array:
	return [phase, remaining, left_ground, paused]

func apply_snapshot(data: Array) -> void:
	if data.size() != 4:
		return
	phase = str(data[0])
	remaining = float(data[1])
	left_ground = bool(data[2])
	paused = bool(data[3])
	present()

func _grounded() -> bool:
	if not is_instance_valid(cargo) or not is_instance_valid(cargo.body):
		return false
	var ray := PhysicsRayQueryParameters3D.create(
		cargo.body.global_position,
		cargo.body.global_position + Vector3.DOWN * 0.48,
		1,
		[cargo.body.get_rid()]
	)
	return not get_world_3d().direct_space_state.intersect_ray(ray).is_empty()

func step(delta: float) -> void:
	if not is_finite(delta) or delta <= 0 or not is_instance_valid(cargo):
		return
	if not cargo.active or cargo.recovery_left > 0 or not cargo.body.visible:
		paused = false
		return
	if cargo.rules.holder_id != 0:
		paused = true
		if phase == "airborne":
			left_ground = true
		return
	var grounded: bool = _grounded() and cargo.body.linear_velocity.y <= 0.2
	if phase == "airborne":
		paused = true
		if not grounded:
			left_ground = true
		elif left_ground and cargo.body.linear_velocity.y <= 0.2:
			reset()
		return
	if not grounded:
		paused = true
		return
	paused = false
	remaining = maxf(0, remaining - delta)
	if remaining > 0 and not is_zero_approx(remaining):
		return
	if phase == "rest":
		phase = "windup"
		remaining = WINDUP_SECONDS
		return
	phase = "airborne"
	remaining = 0
	left_ground = false
	cargo.body.freeze = false
	cargo.body.sleeping = false
	cargo.body.linear_velocity = Vector3.FORWARD.rotated(Vector3.UP, cargo.facing) * FORWARD_SPEED + Vector3.UP * LIFT_SPEED
	cargo.body.angular_velocity = Vector3.ZERO

func present() -> void:
	if not is_instance_valid(cargo) or not is_instance_valid(cargo.body) or not label:
		return
	var shown: bool = cargo.active and cargo.body.visible and cargo.recovery_left <= 0
	label.visible = shown
	indicator.visible = shown
	if not shown:
		return
	global_position = cargo.body.global_position
	global_rotation = Vector3.ZERO
	indicator.position = Vector3(0, 0.52, -0.28)
	indicator.rotation.y = cargo.facing
	label.position = Vector3(0, 0.85, 0)
	label.font_size = 26 if phase == "windup" else 20
	if phase == "windup":
		cargo.visual.scale = Vector3(1.12, 0.72, 1.12)
		label.text = Copy.get_text("hopper_paused") if paused else Copy.get_text("hopper_windup") % remaining
	elif phase == "airborne":
		cargo.visual.scale = Vector3(0.94, 1.12, 0.94)
		label.text = Copy.get_text("hopper_name") + "\n" + Copy.get_text("hopper_paused" if cargo.rules.holder_id != 0 else "hopper_airborne")
	else:
		cargo.visual.scale = Vector3.ONE
		label.text = Copy.get_text("hopper_paused") if paused else Copy.get_text("hopper_rest") % remaining

func _process(_delta: float) -> void:
	present()

extends Node3D

const Zone = preload("res://scripts/map_zone.gd")
const Layout = preload("res://scripts/depot_layout.gd")
const SPEED := 2.0
const USE_DISTANCE := 2.4
const COOLDOWN := 0.5

var direction := -1
var cooldown := 0.0
var active := false
var stripes: Array[MeshInstance3D] = []
var handle: MeshInstance3D
var edited := false

func _ready() -> void:
	edited = has_node("Bed")
	if edited:
		for child in get_children():
			if child is MeshInstance3D and str(child.name).begins_with("Stripe"): stripes.append(child)
		handle = get_node("Lever/Handle")
		_present()
		return
	var bed := MeshInstance3D.new()
	var bed_mesh := BoxMesh.new()
	bed_mesh.size = Vector3(2.2, 0.08, 6.0)
	bed.mesh = bed_mesh
	bed.position = Layout.BELT_CENTER + Vector3(0, 0.025, 0)
	bed.material_override = _material(Color("29343b"))
	add_child(bed)
	for index in range(7):
		var stripe := MeshInstance3D.new()
		var stripe_mesh := BoxMesh.new()
		stripe_mesh.size = Vector3(2.0, 0.025, 0.12)
		stripe.mesh = stripe_mesh
		stripe.position = Layout.BELT_CENTER + Vector3(0, 0.08, -2.7 + index * 0.9)
		stripe.material_override = _material(Color("d19a3a"))
		add_child(stripe)
		stripes.append(stripe)
	var post := MeshInstance3D.new()
	var post_mesh := BoxMesh.new()
	post_mesh.size = Vector3(0.18, 0.9, 0.18)
	post.mesh = post_mesh
	post.position = Layout.LEVER + Vector3.UP * 0.45
	post.material_override = _material(Color("5b6770"))
	add_child(post)
	handle = MeshInstance3D.new()
	var handle_mesh := BoxMesh.new()
	handle_mesh.size = Vector3(0.14, 0.65, 0.14)
	handle.mesh = handle_mesh
	handle.position = Layout.LEVER + Vector3.UP * 1.05
	handle.material_override = _material(Color("ef6f6c"))
	add_child(handle)
	_present()

func _material(color: Color) -> StandardMaterial3D:
	var material := StandardMaterial3D.new()
	material.albedo_color = color
	material.roughness = 0.82
	return material

func reset() -> void:
	direction = -1
	cooldown = 0
	active = false
	_present()

func drift_at(at: Vector3) -> Vector3:
	if edited:
		if not Zone.contains(get_node("TransportZone"),at): return Vector3.ZERO
		return global_basis.orthonormalized() * Vector3(0,0,direction*SPEED)
	if absf(at.x - Layout.BELT_CENTER.x) > 1.1 or absf(at.z - Layout.BELT_CENTER.z) > 3.0:
		return Vector3.ZERO
	return Vector3(0, 0, direction * SPEED)

func try_reverse(peer_id: int, workers: Dictionary, phase: String) -> bool:
	if phase != "playing" or cooldown > 0 or not workers.has(peer_id):
		return false
	var worker = workers[peer_id]
	if not can_use(worker):
		return false
	direction *= -1
	cooldown = COOLDOWN
	_present()
	return true

func can_use(worker: Node3D) -> bool:
	if not is_instance_valid(worker):
		return false
	var lever_position: Vector3 = get_node("Lever").global_position if edited else Layout.LEVER
	var offset: Vector3 = lever_position - worker.position
	offset.y = 0
	if offset.length() > USE_DISTANCE or offset.is_zero_approx():
		return false
	var facing: Vector3 = worker.forward()
	facing.y = 0
	if facing.normalized().dot(offset.normalized()) < 0.5:
		return false
	var ray := PhysicsRayQueryParameters3D.create(worker.position + Vector3.UP, lever_position + Vector3.UP * 0.7, 1, [worker.get_rid()])
	if not get_world_3d().direct_space_state.intersect_ray(ray).is_empty():
		return false
	return true

func apply_direction(value: int) -> void:
	if value == -1 or value == 1:
		direction = value
		_present()

func step(delta: float, cargos: Dictionary) -> void:
	if is_finite(delta) and delta > 0:
		cooldown = maxf(0, cooldown - delta)
	for cargo in cargos.values():
		if not is_instance_valid(cargo) or cargo.creature_held or not cargo.active or cargo.recovery_left > 0 or not cargo.body.visible:
			continue
		if cargo.rules.holder_id != 0 or (cargo.cling and not cargo.cling.target_kind.is_empty()):
			continue
		if cargo.hopper and cargo.hopper.phase == "airborne":
			continue
		var drift := drift_at(cargo.body.global_position)
		if drift == Vector3.ZERO or not _grounded(cargo):
			continue
		cargo.body.freeze = false
		cargo.body.sleeping = false
		# Motor force must overcome the parcel's floor friction, not stall below speed.
		var horizontal := Vector2(cargo.body.linear_velocity.x, cargo.body.linear_velocity.z).move_toward(Vector2(drift.x, drift.z), 24.0 * delta)
		cargo.body.linear_velocity.x = horizontal.x
		cargo.body.linear_velocity.z = horizontal.y

func _grounded(cargo: Node3D) -> bool:
	if cargo.body.linear_velocity.y > 0.2:
		return false
	var ray := PhysicsRayQueryParameters3D.create(cargo.body.global_position, cargo.body.global_position + Vector3.DOWN * 0.48, 1, [cargo.body.get_rid()])
	return not get_world_3d().direct_space_state.intersect_ray(ray).is_empty()

func _present() -> void:
	if handle:
		handle.rotation.x = direction * 0.55

func _process(delta: float) -> void:
	var center_z: float = 0.0 if edited else Layout.BELT_CENTER.z
	var half_length: float = get_node("TransportZone/CollisionShape3D").shape.size.z * 0.5 - 0.15 if edited else 2.7
	if not active:
		return
	for stripe in stripes:
		stripe.position.z += direction * SPEED * delta
		stripe.position.z = wrapf(stripe.position.z,center_z-half_length,center_z+half_length)

func worker_drift(worker: Node3D) -> Vector3:
	var relative: Vector3 = to_local(worker.global_position) if edited else worker.global_position
	if relative.y < -0.1 or relative.y > 0.15: return Vector3.ZERO
	return drift_at(worker.global_position)

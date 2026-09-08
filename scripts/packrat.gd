extends CharacterBody3D

const Layout = preload("res://scripts/depot_layout.gd")
const Copy = preload("res://scripts/copy.gd")
const START := Vector3(10.0, 0.05, -3.2)
const STEAL_RADIUS := 1.4
const SEARCH_RADIUS := 6.0
const STUCK_SECONDS := 1.25
const WARNING_SECONDS := 0.9
const FLEE_SECONDS := 3.0
const HORN_RANGE := 4.0
const HORN_COOLDOWN := 8.0
const PROTECTION_SECONDS := 8.0
const SPEED := 2.7
const PATROL := [Vector3(10, 0.05, -3.2), Vector3(10, 0.05, -8), Vector3(10, 0.05, -14), Vector3(13, 0.05, -17)]

var map_layout = null
var active := false
var phase := "off"
var remaining := 0.0
var target_id := 0
var carried_id := 0
var facing := 0.0
var target_position := START
var patrol_index := 0
var stuck_time := 0.0
var protections: Dictionary = {}
var horn_cooldowns: Dictionary = {}
var horn_cooldown_seconds := HORN_COOLDOWN
var known_cargos: Dictionary = {}
var carry_offset := Vector3.ZERO
var last_safe_cargo_position := Vector3.ZERO
var visual: Node3D
var label: Label3D

func _ready() -> void:
	collision_layer = 8
	collision_mask = 1
	var collision := CollisionShape3D.new()
	var capsule := CapsuleShape3D.new()
	capsule.radius = 0.42
	capsule.height = 0.9
	collision.shape = capsule
	collision.position.y = 0.45
	add_child(collision)
	visual = Node3D.new()
	add_child(visual)
	_part(SphereMesh.new(), Vector3(0, 0.43, 0), Vector3(0.75, 0.48, 1.05), Color("58463c"))
	for side in [-1.0, 1.0]:
		_part(SphereMesh.new(), Vector3(side * 0.25, 0.79, -0.22), Vector3(0.28, 0.32, 0.18), Color("b47d78"))
		_part(SphereMesh.new(), Vector3(side * 0.15, 0.61, -0.48), Vector3.ONE * 0.08, Color("fff2c7"))
		_part(SphereMesh.new(), Vector3(side * 0.15, 0.61, -0.53), Vector3.ONE * 0.035, Color("151719"))
	var tail_mesh := CylinderMesh.new()
	tail_mesh.top_radius = 0.035
	tail_mesh.bottom_radius = 0.055
	tail_mesh.height = 0.9
	var tail := _part(tail_mesh, Vector3(0, 0.35, 0.75), Vector3.ONE, Color("a56e68"))
	tail.rotation.x = PI * 0.5
	label = Label3D.new()
	var font := SystemFont.new()
	font.font_names = PackedStringArray(["Malgun Gothic", "Segoe UI", "Arial"])
	label.font = font
	label.font_size = 26
	label.pixel_size = 0.006
	label.billboard = BaseMaterial3D.BILLBOARD_ENABLED
	label.position.y = 1.25
	add_child(label)
	position = _start_point()
	target_position = position
	_present()

func _part(mesh: PrimitiveMesh, at: Vector3, scale_value: Vector3, color: Color) -> MeshInstance3D:
	var instance := MeshInstance3D.new()
	instance.mesh = mesh
	instance.position = at
	instance.scale = scale_value
	var material := StandardMaterial3D.new()
	material.albedo_color = color
	material.roughness = 0.9
	instance.material_override = material
	visual.add_child(instance)
	return instance

func reset(cargos: Dictionary) -> void:
	known_cargos = cargos
	_drop_carried(cargos, false)
	active = false
	phase = "off"
	remaining = 0
	target_id = 0
	carried_id = 0
	carry_offset = Vector3.ZERO
	last_safe_cargo_position = Vector3.ZERO
	patrol_index = 0
	stuck_time = 0.0
	protections.clear()
	horn_cooldowns.clear()
	position = _start_point()
	target_position = position
	velocity = Vector3.ZERO
	_present()

func start(stage: int) -> void:
	_drop_carried(known_cargos, false)
	if is_instance_valid(map_layout):
		position = _start_point()
		patrol_index = 0
	active = stage >= 1
	phase = "patrol" if active else "off"
	remaining = 0
	target_id = 0
	carried_id = 0
	_present()

func stop(cargos: Dictionary) -> void:
	known_cargos = cargos
	_drop_carried(cargos, false)
	active = false
	phase = "off"
	remaining = 0
	target_id = 0
	velocity = Vector3.ZERO
	_present()

func step(delta: float, workers: Dictionary, cargos: Dictionary) -> void:
	if not is_finite(delta) or delta <= 0:
		return
	known_cargos = cargos
	_tick_map(protections, delta)
	_tick_map(horn_cooldowns, delta)
	if not active:
		return
	match phase:
		"seek":
			var cargo = cargos.get(target_id)
			if not _eligible(cargo) or position.distance_to(cargo.body.global_position) > SEARCH_RADIUS:
				target_id = 0
				phase = "patrol"
				stuck_time = 0.0
			elif position.distance_to(cargo.body.global_position) <= STEAL_RADIUS:
				phase = "warning"
				remaining = WARNING_SECONDS
				stuck_time = 0.0
			else:
				var before := position
				_move_toward(cargo.body.global_position, delta)
				stuck_time = stuck_time + delta if position.distance_to(before) < SPEED * delta * 0.1 else 0.0
				if stuck_time >= STUCK_SECONDS:
					protections[target_id] = 3.0
					target_id = 0
					phase = "patrol"
					stuck_time = 0.0
					patrol_index = (patrol_index + 1) % _patrol_points().size()
		"warning":
			var cargo = cargos.get(target_id)
			if not _eligible(cargo) or position.distance_to(cargo.body.global_position) > STEAL_RADIUS:
				phase = "patrol"
				target_id = 0
				remaining = 0
			else:
				remaining = maxf(0, remaining - delta)
				if remaining <= 0:
					_claim(cargo)
		"carry":
			var cargo = cargos.get(carried_id)
			if not is_instance_valid(cargo) or not cargo.creature_held:
				carried_id = 0
				phase = "patrol"
			else:
				if not _move_carried(cargo, _nest_point(), delta):
					_drop_carried(cargos, false)
					_present()
					return
				if global_position.distance_to(_nest_point()) <= 1.2:
					_drop_carried(cargos, true)
		"flee":
			remaining = maxf(0, remaining - delta)
			_move_toward(_patrol_points()[(patrol_index + 2) % _patrol_points().size()], delta)
			if remaining <= 0:
				phase = "patrol"
		"patrol":
			var candidate = _nearest(cargos)
			if candidate:
				target_id = candidate.cargo_id
				phase = "warning" if position.distance_to(candidate.body.global_position) <= STEAL_RADIUS else "seek"
				remaining = WARNING_SECONDS if phase == "warning" else 0.0
				stuck_time = 0.0
			else:
				var goal: Vector3 = _patrol_points()[patrol_index]
				_move_toward(goal, delta)
				if position.distance_to(goal) < 0.25:
					patrol_index = (patrol_index + 1) % _patrol_points().size()
	_present()

func scare(worker: Node3D, cargos: Dictionary) -> bool:
	if not active or not is_instance_valid(worker) or float(horn_cooldowns.get(worker.peer_id, 0.0)) > 0:
		return false
	var origin := global_position + Vector3.UP * 0.55
	var target := worker.global_position + Vector3.UP
	if origin.distance_to(target) > HORN_RANGE:
		return false
	var ray := PhysicsRayQueryParameters3D.create(origin, target, 1, [get_rid(), worker.get_rid()])
	if not get_world_3d().direct_space_state.intersect_ray(ray).is_empty():
		return false
	horn_cooldowns[worker.peer_id] = horn_cooldown_seconds
	startle(cargos)
	return true

func startle(cargos: Dictionary) -> void:
	if not active: return
	known_cargos = cargos
	_drop_carried(cargos, false)
	target_id = 0
	phase = "flee"
	remaining = FLEE_SECONDS
	_present()

func snapshot() -> Array:
	return [active, phase, position, facing, remaining, target_id, carried_id]

func apply_snapshot(data: Array) -> void:
	if data.size() != 7:
		return
	active = bool(data[0])
	phase = str(data[1])
	target_position = data[2]
	if position.distance_to(target_position) > 4:
		position = target_position
	facing = float(data[3])
	remaining = float(data[4])
	target_id = int(data[5])
	carried_id = int(data[6])
	_present()

func status_key() -> String:
	return "packrat_" + phase

func protected_left(cargo_id: int) -> float:
	return float(protections.get(cargo_id, 0.0))

func _tick_map(values: Dictionary, delta: float) -> void:
	for key in values.keys():
		values[key] = maxf(0, float(values[key]) - delta)
		if values[key] <= 0:
			values.erase(key)

func _eligible(cargo) -> bool:
	if not is_instance_valid(cargo) or not cargo.active or cargo.recovery_left > 0 or not cargo.body.visible:
		return false
	if cargo.creature_held or cargo.rules.holder_id != 0 or cargo.rules.delivered:
		return false
	if cargo.cling and not cargo.cling.target_kind.is_empty():
		return false
	# An attached Clinger holds its partner too: the rat cannot split the pair.
	for partner in known_cargos.values():
		if not is_instance_valid(partner) or not partner.active or not partner.body.visible or partner.creature_held: continue
		if partner.cling and partner.cling.target_kind == "cargo" and partner.cling.target_id == cargo.cargo_id:
			return false
	if cargo.hopper and cargo.hopper.phase == "airborne":
		return false
	if protected_left(cargo.cargo_id) > 0 or absf(cargo.body.linear_velocity.y) > 0.25:
		return false
	var ray := PhysicsRayQueryParameters3D.create(cargo.body.global_position, cargo.body.global_position + Vector3.DOWN * 0.48, 1, [cargo.body.get_rid()])
	return not get_world_3d().direct_space_state.intersect_ray(ray).is_empty() and _clear_sight(cargo)

func _clear_sight(cargo) -> bool:
	var ray := PhysicsRayQueryParameters3D.create(global_position + Vector3.UP * 0.5, cargo.body.global_position, 1, [get_rid(), cargo.body.get_rid()])
	return get_world_3d().direct_space_state.intersect_ray(ray).is_empty()

func _nearest(cargos: Dictionary):
	var nearest = null
	var distance := SEARCH_RADIUS + 0.001
	for cargo in cargos.values():
		if not _eligible(cargo):
			continue
		var at: Vector3 = cargo.body.global_position
		if not _searchable(at): continue
		var candidate: float = position.distance_to(cargo.body.global_position)
		if candidate < distance:
			distance = candidate
			nearest = cargo
	return nearest

func _claim(cargo) -> void:
	if not _eligible(cargo):
		phase = "patrol"
		target_id = 0
		return
	cargo.creature_held = true
	cargo.rules.holder_id = 0
	cargo.body.freeze = true
	cargo.body.linear_velocity = Vector3.ZERO
	cargo.body.angular_velocity = Vector3.ZERO
	cargo.body.collision_layer = 0
	cargo.body.collision_mask = 1
	carried_id = cargo.cargo_id
	carry_offset = cargo.body.global_position - global_position
	last_safe_cargo_position = cargo.body.global_position
	target_id = 0
	phase = "carry"
	remaining = 0

func _drop_carried(cargos: Dictionary, protect: bool) -> void:
	if carried_id == 0:
		return
	var cargo = cargos.get(carried_id)
	if is_instance_valid(cargo):
		if not protect and _cargo_overlaps_static(cargo):
			cargo.body.global_position = last_safe_cargo_position
		cargo.creature_held = false
		cargo.rules.holder_id = 0
		cargo.body.freeze = not cargo.active
		cargo.body.sleeping = false
		cargo.body.linear_velocity = Vector3.ZERO
		cargo.body.angular_velocity = Vector3.ZERO
		cargo.body.collision_layer = 4
		cargo.body.collision_mask = 7
		if protect:
			cargo.body.global_position = _drop_point()
			protections[carried_id] = PROTECTION_SECONDS
	carried_id = 0
	carry_offset = Vector3.ZERO
	last_safe_cargo_position = Vector3.ZERO
	phase = "patrol" if active else "off"

func _move_carried(cargo, goal: Vector3, delta: float) -> bool:
	var offset := goal - position
	offset.y = 0
	if offset.length() < 0.001:
		return true
	var motion: Vector3 = offset.normalized() * minf(offset.length(), SPEED * delta)
	var shape: Shape3D
	for child in cargo.body.get_children():
		if child is CollisionShape3D and child.shape:
			shape = child.shape
			break
	if shape:
		if shape is BoxShape3D:
			var inset := BoxShape3D.new()
			# Godot's resting contact may settle a few millimetres into the floor.
			# Keep the horizontal sweep effectively full-size while tolerating it.
			inset.size = shape.size - Vector3(0.002, 0.02, 0.002)
			shape = inset
		var request := PhysicsShapeQueryParameters3D.new()
		request.shape = shape
		request.transform = Transform3D(Basis.IDENTITY, cargo.body.global_position)
		request.motion = motion
		request.margin = 0.0
		request.collision_mask = 1
		request.exclude = [cargo.body.get_rid(), get_rid()]
		var space := get_world_3d().direct_space_state
		if space.cast_motion(request)[0] < 1.0:
			return false
		request.motion = Vector3.ZERO
		if not space.intersect_shape(request, 1).is_empty():
			return false
	var previous := global_position
	_move_toward(goal, delta)
	cargo.body.global_position += global_position - previous
	last_safe_cargo_position = cargo.body.global_position
	return true

func _cargo_overlaps_static(cargo) -> bool:
	var shape: Shape3D
	for child in cargo.body.get_children():
		if child is CollisionShape3D and child.shape:
			shape = child.shape
			break
	if not shape:
		return false
	if shape is BoxShape3D:
		var inset := BoxShape3D.new()
		inset.size = shape.size - Vector3(0.002, 0.02, 0.002)
		shape = inset
	var request := PhysicsShapeQueryParameters3D.new()
	request.shape = shape
	request.transform = Transform3D(Basis.IDENTITY, cargo.body.global_position)
	request.margin = 0.0
	request.collision_mask = 1
	request.exclude = [cargo.body.get_rid(), get_rid()]
	return not get_world_3d().direct_space_state.intersect_shape(request, 1).is_empty()

func _move_toward(goal: Vector3, delta: float) -> void:
	var offset := goal - position
	offset.y = 0
	if offset.length() < 0.001:
		velocity = Vector3.ZERO
		return
	var direction := offset.normalized()
	facing = atan2(-direction.x, -direction.z)
	velocity = direction * SPEED
	move_and_collide(direction * minf(offset.length(), SPEED * delta))
	if is_instance_valid(map_layout):
		global_position = map_layout.rat_clamp(global_position)
	else:
		position.x = clampf(position.x, 8.5, 14.5)
		position.z = clampf(position.z, -19.0, 0.0)

func _process(delta: float) -> void:
	if multiplayer.has_multiplayer_peer() and not multiplayer.is_server():
		position = position.lerp(target_position, minf(1, delta * 15))
	_present()

func _present() -> void:
	visible = active
	if not label:
		return
	visual.rotation.y = facing
	label.visible = active
	label.modulate = Color("ff746c") if phase == "warning" else Color("f3dfad")
	match phase:
		"warning": label.text = _localized("STEAL IN %.1fs!", "%.1f초 뒤 훔쳐요!") % remaining
		"seek": label.text = _localized("PACKAGE SPOTTED!", "상자 발견!")
		"carry": label.text = _localized("STOLEN!", "도둑맞음!")
		"flee": label.text = _localized("FLEE %.1fs", "도망 %.1f초") % remaining
		_: label.text = _localized("PACKRAT", "포장쥐")

func _localized(english: String, korean: String) -> String:
	return korean if Copy.language == "ko" else english

func _nest_point() -> Vector3:
	return map_layout.rat_nest() if is_instance_valid(map_layout) else Layout.NEST
func _start_point() -> Vector3:
	return map_layout.rat_start() if is_instance_valid(map_layout) else START
func _drop_point() -> Vector3:
	return map_layout.rat_drop() if is_instance_valid(map_layout) else Layout.NEST + Vector3(0,0.55,1.35)
func _patrol_points() -> Array:
	return map_layout.rat_patrol() if is_instance_valid(map_layout) else PATROL
func _searchable(at: Vector3) -> bool:
	return map_layout.rat_can_search(at) if is_instance_valid(map_layout) else at.x >= 8.5 and at.x <= 14.5 and at.z >= -19 and at.z <= 0

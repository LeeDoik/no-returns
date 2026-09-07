extends Node3D

signal sneezed(cargo: Node3D)
signal notice(key: String)

const Layout = preload("res://scripts/depot_layout.gd")
const Rules = preload("res://scripts/cargo_rules.gd")
const Sneeze = preload("res://scripts/sneeze_rules.gd")
const Cues = preload("res://scripts/sneeze_cues.gd")
const Clinger = preload("res://scripts/clinger.gd")
const Hopper = preload("res://scripts/hopper.gd")
const COMMON_KEYS := ["position", "holder", "score", "delivered", "destination", "recovery", "visible", "active", "facing"]
const SNEEZE_KEYS := ["phase", "remaining", "epoch", "event", "origin", "burst_facing"]
const SIZE := 0.8
const THROW_SPEED := 9.5
const THROW_LIFT := 5.4
const MARGIN := 0.005

var cargo_id := 1
var kind := "standard"
var home := Vector3(0, 0.55, 1.1)
var rules = Rules.new()
var sneeze = Sneeze.new(randi())
var body: RigidBody3D
var shape: BoxShape3D
var visual: Node3D
var cues: Node3D
var cling: Node3D
var hopper: Node3D
var destination_labels: Array[Label3D] = []
var recovery_left := 0.0
var facing := 0.0
var active := false
var burst_origin := Vector3.ZERO
var burst_facing := 0.0
var target_position := Vector3.ZERO
var authority_epoch := ""
var carrier: PhysicsBody3D

func _ready() -> void:
	body = RigidBody3D.new()
	body.name = "Body"
	body.mass = 1.2
	body.collision_layer = 4
	body.collision_mask = 7
	body.continuous_cd = true
	body.linear_damp_mode = RigidBody3D.DAMP_MODE_REPLACE
	body.linear_damp = 0
	body.angular_damp_mode = RigidBody3D.DAMP_MODE_REPLACE
	body.angular_damp = 0
	body.lock_rotation = true
	body.physics_material_override = PhysicsMaterial.new()
	body.physics_material_override.bounce = 0.2
	body.physics_material_override.friction = 0.8
	body.freeze = true
	add_child(body)
	shape = BoxShape3D.new()
	shape.size = Vector3.ONE * SIZE
	var collision := CollisionShape3D.new()
	collision.shape = shape
	body.add_child(collision)
	visual = Node3D.new()
	body.add_child(visual)
	_part(Vector3.ONE * SIZE, Vector3.ZERO, Color("baace3") if kind == "sneezer" else (Color("f2c14e") if kind == "hopper" else Color("d5ae70")))
	_part(Vector3(0.14, 0.81, 0.81), Vector3.ZERO, Color("f09baa") if kind == "sneezer" else Color("69b5a1"))
	if kind == "clinger":
		_part(Vector3.ONE * 0.805, Vector3.ZERO, Color("b4db73"))
		for side in [-1.0, 1.0]:
			_part(Vector3(0.12, 0.40, 0.40), Vector3(side * 0.44, 0, 0), Color("6d9b42"))
			_part(Vector3(0.40, 0.40, 0.12), Vector3(0, 0, side * 0.44), Color("6d9b42"))
	for side in [-1.0, 1.0]:
		var tag := Label3D.new()
		tag.text = "A\n↑ ↑"
		tag.font_size = 65
		tag.pixel_size = 0.004
		tag.position = Vector3(side * 0.41, 0, 0)
		tag.rotation.y = side * PI / 2
		tag.modulate = Color("253d37")
		tag.outline_size = 0
		visual.add_child(tag)
		destination_labels.append(tag)
	if kind == "sneezer":
		cues = Cues.new()
		add_child(cues)
		cues.setup(body, visual)
	if kind == "clinger":
		cling = Clinger.new()
		add_child(cling)
		cling.setup(self)
	if kind == "hopper":
		hopper = Hopper.new()
		add_child(hopper)
		hopper.setup(self)
	body.position = home
	target_position = home

func _part(size: Vector3, at: Vector3, color: Color) -> void:
	var mesh := MeshInstance3D.new()
	var cube := BoxMesh.new()
	cube.size = size
	mesh.mesh = cube
	var material := StandardMaterial3D.new()
	material.albedo_color = color
	mesh.material_override = material
	mesh.position = at
	visual.add_child(mesh)

func reset_shift() -> void:
	# Each authority/shift owns a distinct event sequence namespace. Guest cargo
	# nodes survive lobby transitions, so a new host may start at a lower ID.
	authority_epoch = Crypto.new().generate_random_bytes(16).hex_encode()
	if cues:
		cues.begin_epoch()
	rules.reset_shift()
	active = true
	reset_crate()

func reset_crate() -> void:
	_set_carrier(null)
	rules.reset_crate()
	if cling:
		cling.reset()
	if hopper:
		hopper.reset()
	recovery_left = 0
	facing = 0
	sneeze.reset()
	if cues:
		cues.cancel()
	body.freeze = true
	body.position = home
	body.rotation = Vector3.ZERO
	body.linear_velocity = Vector3.ZERO
	body.angular_velocity = Vector3.ZERO
	body.visible = true
	_set_collision_enabled(true)
	body.freeze = not active
	body.sleeping = false
	target_position = home

func cancel() -> void:
	active = false
	if cling:
		cling.detach()
	if hopper:
		hopper.reset()
	sneeze.reset()
	if cues:
		cues.cancel()

func pickup(worker: Node3D) -> bool:
	if not active or recovery_left > 0 or not body.visible or (cling and not cling.target_kind.is_empty()):
		return false
	var ray := PhysicsRayQueryParameters3D.create(worker.position + Vector3.UP, body.position, 1)
	if not get_world_3d().direct_space_state.intersect_ray(ray).is_empty():
		return false
	if not rules.try_pickup(worker.peer_id, worker.position, body.position):
		return false
	_set_carrier(worker)
	body.freeze = true
	body.linear_velocity = Vector3.ZERO
	body.angular_velocity = Vector3.ZERO
	facing = worker.heading
	# Commit the validated pickup pose before the next movement tick. Otherwise
	# a moving player can leave pickup range before the first held snapshot.
	move_held(worker)
	return true

func release(worker: Node3D, throwing: bool) -> void:
	if not rules.try_release(worker.peer_id):
		return
	_set_carrier(null)
	body.freeze = false
	body.sleeping = false
	body.linear_velocity = throw_velocity(worker) if throwing else Vector3.ZERO
	body.angular_velocity = Vector3.ZERO

func throw_velocity(worker: Node3D) -> Vector3:
	return worker.forward() * THROW_SPEED + Vector3.UP * THROW_LIFT

func query(at: Vector3, motion: Vector3) -> PhysicsShapeQueryParameters3D:
	var request := PhysicsShapeQueryParameters3D.new()
	request.shape = shape
	request.transform = Transform3D(Basis.IDENTITY, at)
	request.motion = motion
	request.margin = MARGIN
	request.collision_mask = body.collision_mask
	request.exclude = [body.get_rid(), carrier.get_rid()] if is_instance_valid(carrier) else [body.get_rid()]
	return request

func _set_carrier(worker: PhysicsBody3D) -> void:
	if is_instance_valid(carrier):
		body.remove_collision_exception_with(carrier)
		carrier.remove_collision_exception_with(body)
	carrier = worker
	if is_instance_valid(carrier):
		body.add_collision_exception_with(carrier)
		carrier.add_collision_exception_with(body)

func move_held(worker: Node3D) -> void:
	var distance := body.position.distance_to(worker.position)
	if distance > Rules.PICKUP_DISTANCE and not is_equal_approx(distance, Rules.PICKUP_DISTANCE):
		release(worker, false)
		return
	var motion: Vector3 = worker.hand_position() - body.position
	var fractions := get_world_3d().direct_space_state.cast_motion(query(body.position, motion))
	body.position += motion * fractions[0]
	facing = worker.heading

func step(delta: float, workers: Dictionary) -> void:
	if not active:
		return
	if cling and not cling.target_kind.is_empty():
		return
	if recovery_left > 0:
		recovery_left -= delta
		if recovery_left <= 0:
			reset_crate()
		return
	if rules.holder_id != 0 and workers.has(rules.holder_id):
		move_held(workers[rules.holder_id])
	else:
		if Layout.outside(body.position):
			recover("recovered")
			return
		if Layout.dock_at(body.position) != 0:
			var dock := Layout.dock_at(body.position)
			if dock != 0:
				recover("shipped" if rules.try_dispatch(dock) else "wrong_bay")
				return
	if kind == "sneezer" and sneeze.advance(delta):
		burst_origin = body.position
		burst_facing = facing
		sneezed.emit(self)
	if hopper:
		hopper.step(delta)

func recover(reason: String) -> void:
	_set_carrier(null)
	if cling:
		cling.detach()
	if hopper:
		hopper.reset()
	rules.holder_id = 0
	recovery_left = 1.2
	sneeze.reset()
	if cues:
		cues.cancel()
	body.freeze = true
	body.visible = false
	_set_collision_enabled(false)
	notice.emit(reason)

func _set_collision_enabled(enabled: bool) -> void:
	body.collision_layer = 4 if enabled else 0
	body.collision_mask = 7 if enabled else 0

func push(direction: Vector3, workers: Dictionary) -> void:
	if rules.holder_id != 0 and workers.has(rules.holder_id):
		release(workers[rules.holder_id], false)
	body.freeze = false
	body.sleeping = false
	body.linear_velocity = (body.linear_velocity + direction * 8.0 + Vector3.UP * 4.0).limit_length(16.0)

func predict_contact(origin: Vector3, velocity: Vector3) -> Dictionary:
	var dt := 1.0 / Engine.physics_ticks_per_second
	var gravity: Vector3 = ProjectSettings.get_setting("physics/3d/default_gravity_vector") * float(ProjectSettings.get_setting("physics/3d/default_gravity")) * body.gravity_scale
	var space := get_world_3d().direct_space_state
	var at := origin
	for index in range(ceili(3.0 / dt)):
		velocity += gravity * dt
		var motion := velocity * dt
		var request := query(at, motion)
		var fractions := space.cast_motion(request)
		if fractions[0] < 1.0:
			var center := at + motion * fractions[0]
			request.transform.origin = at + motion * fractions[1]
			request.motion = Vector3.ZERO
			var rest := space.get_rest_info(request)
			return {"center": center, "normal": rest.normal} if not rest.is_empty() else {}
		at += motion
	return {}

func snapshot() -> Dictionary:
	# Registry keys already identify the fixed cargo types. Ordinary cargo does
	# not need a clock or event payload in every 20 Hz unreliable snapshot.
	var state := {"position": body.position, "holder": rules.holder_id,
		"score": rules.score, "delivered": rules.delivered, "destination": rules.destination, "recovery": recovery_left, "visible": body.visible,
		"active": active, "facing": facing}
	if kind == "sneezer":
		state.merge({"phase": sneeze.phase, "remaining": sneeze.remaining, "epoch": authority_epoch,
			"event": sneeze.event_id, "origin": burst_origin, "burst_facing": burst_facing})
	if cling:
		state["cling"] = cling.snapshot()
	if hopper:
		state["hopper"] = hopper.snapshot()
	return state

func wire_snapshot() -> Array:
	var state := snapshot()
	var values: Array = []
	for key in COMMON_KEYS:
		values.append(state[key])
	if kind == "sneezer":
		for key in SNEEZE_KEYS:
			values.append(state[key])
	if cling:
		values.append(state.cling)
	if hopper:
		values.append(state.hopper)
	return values

func apply_wire(values: Array) -> void:
	var keys: Array = COMMON_KEYS.duplicate()
	if kind == "sneezer":
		keys.append_array(SNEEZE_KEYS)
	if cling:
		keys.append("cling")
	if hopper:
		keys.append("hopper")
	if values.size() != keys.size():
		return
	var state := {}
	for index in range(keys.size()):
		state[keys[index]] = values[index]
	apply_snapshot(state)

func apply_snapshot(data: Dictionary) -> void:
	rules.holder_id = data.holder
	var roster = get_parent().get("workers")
	_set_carrier(roster.get(rules.holder_id) if roster is Dictionary else null)
	rules.score = data.score
	rules.delivered = data.delivered
	rules.destination = data.destination
	recovery_left = data.recovery
	active = data.active
	facing = data.facing
	body.freeze = true
	body.visible = data.visible
	_set_collision_enabled(data.visible and recovery_left <= 0)
	target_position = data.position
	if kind == "sneezer":
		if authority_epoch != data.epoch:
			authority_epoch = data.epoch
			cues.begin_epoch()
		sneeze.phase = data.phase
		sneeze.remaining = data.remaining
		sneeze.event_id = data.event
		burst_origin = data.origin
		burst_facing = data.burst_facing
	if cling:
		cling.apply_snapshot(data.cling)
	if hopper:
		hopper.apply_snapshot(data.hopper)
	_present()

func interpolate(delta: float) -> void:
	if body.position.distance_to(target_position) > 4.0 or recovery_left > 0:
		body.position = target_position
	else:
		body.position = body.position.lerp(target_position, minf(1, delta * 20))

func _process(_delta: float) -> void:
	_present()

func _present() -> void:
	# Carry/throw direction is authoritative and already replicated. Rotate the
	# model with it while keeping the stable axis-aligned physics/query shape.
	visual.rotation.y = facing
	var destination_text := "A\n↑ ↑" if rules.destination == 1 else "B\n◆"
	for tag in destination_labels:
		tag.text = destination_text
	if cues:
		cues.present(sneeze, facing, burst_origin, burst_facing, active and body.visible and recovery_left <= 0)

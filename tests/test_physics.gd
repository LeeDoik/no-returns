extends SceneTree

var game: Node
var failures := 0
var first_contact := Vector3.INF

func _initialize() -> void:
	call_deferred("_run")

func _check(ok: bool, message: String) -> void:
	if not ok:
		failures += 1
		print("FAIL physics: " + message)

func _frames(count: int) -> void:
	for i in count:
		await physics_frame
		await process_frame

func _run() -> void:
	game = load("res://scenes/main.tscn").instantiate()
	root.add_child(game)
	game.practice_game()
	# These original trajectory/carry cases isolate one crate in static geometry.
	game.cargos[2].cancel()
	game.cargos[2].body.visible = false
	game.cargos[2].body.collision_layer = 0
	game.cargos[2].body.collision_mask = 0
	game.cargos[3].cancel()
	game.cargos[3].body.visible = false
	game.cargos[3].body.collision_layer = 0
	game.cargos[3].body.collision_mask = 0
	game.set_physics_process(false)
	game.set_process(false)
	game.cargos[1].body.contact_monitor = true
	game.cargos[1].body.max_contacts_reported = 8
	game.cargos[1].body.body_entered.connect(_contact)
	await _frames(3)
	await _pickup_near_boundary("behind", Vector3(0, 0.41, 2.3))
	await _pickup_near_boundary("side", Vector3(2.3, 0.41, 0))
	await _held_wall()
	await _throw_case("floor", Vector3(-10, 0.05, 5), 0.0, Vector3.UP)
	await _throw_case("oblique floor", Vector3(-11, 0.05, 5), 0.15, Vector3.UP)
	await _throw_case("back wall", Vector3(0, 0.05, -23), 0.0, Vector3.BACK)
	await _throw_case("side wall", Vector3(13, 0.05, 7), -PI / 2.0, Vector3.LEFT)
	await _throw_case("divider", Vector3(0, 0.05, 0.18), 0.0, Vector3.BACK)
	print("PHYSICS %s: %d failure(s)" % ["PASS" if failures == 0 else "FAIL", failures])
	quit(0 if failures == 0 else 1)

func _arrange(worker_at: Vector3, yaw: float, cargo_at: Vector3) -> void:
	game.cargos[1].rules.reset_crate()
	game.cargos[1].body.freeze = true
	game.cargos[1].body.position = cargo_at
	game.cargos[1].body.rotation = Vector3.ZERO
	game.cargos[1].body.linear_velocity = Vector3.ZERO
	game.cargos[1].body.angular_velocity = Vector3.ZERO
	game.workers[1].position = worker_at
	game.workers[1].heading = yaw
	await _frames(3)
	game._receive_action(1, "interact")
	_check(game.cargos[1].rules.holder_id == 1, "fixture pickup accepted")

func _pickup_near_boundary(label: String, cargo_at: Vector3) -> void:
	# Keep this pickup fixture clear of the divider; wall blocking is tested
	# separately and is allowed to prevent a crate from reaching the hand.
	await _arrange(Vector3(0, 0.05, 4), 0, cargo_at + Vector3(0, 0, 4))
	game.cargos[1].step(1.0 / 60.0, game.workers)
	await _frames(1)
	_check(game.cargos[1].rules.holder_id == 1, label + " near-boundary pickup remains held after first carry tick")
	_check(game.cargos[1].body.position.distance_to(game.workers[1].hand_position()) < 0.01, label + " reachable pickup reaches hand")

func _held_wall() -> void:
	# Divider occupies z [-1.75, -1.25], y [0, 1]. The hand and
	# the entire crate must remain on its near side while the worker walks up.
	await _arrange(Vector3(0, 0.05, 0.8), 0, Vector3(0, 1.1, 0))
	for i in 60:
		game.workers[1].simulate(Vector2(0, -1), 0, false, 1.0 / 60.0)
		game.cargos[1].step(1.0 / 60.0, game.workers)
		await _frames(1)
		var overlap := PhysicsShapeQueryParameters3D.new()
		overlap.shape = _cargo_shape()
		overlap.transform = game.cargos[1].body.global_transform
		overlap.collision_mask = 1
		_check(game.get_world_3d().direct_space_state.intersect_shape(overlap).is_empty(), "held cargo volume never overlaps depot")
	_check(game.cargos[1].body.position.z >= -0.851, "full held box stopped before divider: %s" % game.cargos[1].body.position)
	var before: Vector3 = game.cargos[1].body.position
	game.workers[1].heading = PI
	game.cargos[1].release(game.workers[1], false)
	_check(game.cargos[1].body.position.distance_to(before) < 0.001, "drop preserves last validated position")
	await _frames(4)
	_check(game.cargos[1].body.position.z >= -0.86, "dropped cargo stayed on divider near side")
	# Also reject an arbitrarily long tether after an abrupt carrier relocation.
	await _arrange(Vector3(0, 0.05, 0.8), 0, Vector3(0, 1.1, 0))
	game.workers[1].position = Vector3(0, 0.05, -4)
	game.cargos[1].step(1.0 / 60.0, game.workers)
	_check(game.cargos[1].rules.holder_id == 0, "blocked distant carry releases instead of long-distance drag")

func _cargo_shape() -> Shape3D:
	for child in game.cargos[1].body.get_children():
		if child is CollisionShape3D:
			return child.shape
	return null

func _contact(_body: Node) -> void:
	if first_contact == Vector3.INF:
		first_contact = game.cargos[1].body.position

func _throw_case(label: String, worker_at: Vector3, yaw: float, normal: Vector3) -> void:
	var worker = game.workers[1]
	worker.position = worker_at
	worker.heading = yaw
	await _arrange(worker_at, yaw, worker.hand_position())
	game.cargos[1].step(1.0 / 60.0, game.workers)
	game._update_marker()
	_check(game.depot.marker.visible, label + " preview finds first contact")
	var marker: Vector3 = game.depot.marker.position
	first_contact = Vector3.INF
	game.cargos[1].release(worker, true)
	for i in 180:
		await _frames(1)
		if first_contact != Vector3.INF:
			break
	_check(first_contact != Vector3.INF, label + " actual body contact within 3 seconds")
	if first_contact != Vector3.INF:
		# Compare ring to center of the contacting face; 8 cm lift keeps it visible.
		var expected_marker := first_contact - normal * 0.32
		var error := marker.distance_to(expected_marker)
		print("TEST physics: %s marker error %.4f m; predicted %s; contact center %s" % [label, error, marker, first_contact])
		_check(error <= 0.20, label + " first-contact marker within 0.20 m")

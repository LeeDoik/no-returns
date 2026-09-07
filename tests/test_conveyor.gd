extends SceneTree

const Layout = preload("res://scripts/depot_layout.gd")
const Conveyor = preload("res://scripts/conveyor.gd")
const Worker = preload("res://scripts/worker.gd")
const Cargo = preload("res://scripts/cargo.gd")

var failures: Array[String] = []

func check(ok: bool, message: String) -> void:
	if not ok:
		failures.append(message)

func _initialize() -> void:
	call_deferred("run")

func run() -> void:
	var world := Node3D.new()
	root.add_child(world)
	var floor := StaticBody3D.new()
	var floor_collision := CollisionShape3D.new()
	var floor_shape := BoxShape3D.new()
	floor_shape.size = Vector3(40, 0.2, 40)
	floor_collision.shape = floor_shape
	floor.add_child(floor_collision)
	floor.position.y = -0.1
	world.add_child(floor)
	var conveyor = Conveyor.new()
	world.add_child(conveyor)
	var worker = Worker.new()
	worker.peer_id = 1
	world.add_child(worker)
	worker.position = Layout.LEVER + Vector3(0, 0, 2)
	worker.heading = 0
	var roster := {1: worker}
	check(conveyor.direction == -1, "belt starts toward the loading bays")
	check(conveyor.drift_at(Layout.BELT_CENTER) == Vector3(0, 0, -2), "belt center moves at two metres per second")
	check(conveyor.drift_at(Layout.BELT_CENTER + Vector3(1.2, 0, 0)) == Vector3.ZERO, "belt has bounded width")
	check(conveyor.drift_at(Layout.BELT_CENTER + Vector3(0, 0, -3.1)) == Vector3.ZERO, "belt has bounded length")
	check(not conveyor.try_reverse(9, roster, "playing"), "non-member cannot use lever")
	check(not conveyor.try_reverse(1, roster, "waiting"), "lever is disabled outside play")
	worker.position = Layout.LEVER + Vector3(5, 0, 0)
	check(not conveyor.try_reverse(1, roster, "playing"), "distant worker cannot use lever")
	worker.position = Layout.LEVER + Vector3(0, 0, 2)
	worker.heading = PI
	check(not conveyor.try_reverse(1, roster, "playing"), "worker must face lever")
	worker.heading = 0
	var wall := StaticBody3D.new()
	var wall_collision := CollisionShape3D.new()
	var wall_shape := BoxShape3D.new()
	wall_shape.size = Vector3(1, 2, 0.2)
	wall_collision.shape = wall_shape
	wall.add_child(wall_collision)
	wall.position = Layout.LEVER + Vector3(0, 1, 1)
	world.add_child(wall)
	await physics_frame
	check(not conveyor.try_reverse(1, roster, "playing"), "solid wall blocks lever line of sight")
	wall.queue_free()
	await physics_frame
	check(conveyor.try_reverse(1, roster, "playing") and conveyor.direction == 1, "nearby facing worker reverses belt")
	check(not conveyor.try_reverse(1, roster, "playing"), "half-second cooldown blocks repeats")
	conveyor.step(0.5, {})
	check(conveyor.try_reverse(1, roster, "playing") and conveyor.direction == -1, "lever works after cooldown")

	worker.position = Layout.BELT_CENTER
	for index in range(8):
		worker.simulate(Vector2.ZERO, 0, false, 1.0 / 60.0)
	worker.simulate(Vector2.ZERO, 0, false, 0.1, conveyor.drift_at(worker.position))
	check(worker.velocity.z < -1.9, "worker simulation accepts belt drift")

	var cargo = Cargo.new()
	cargo.kind = "standard"
	world.add_child(cargo)
	cargo.reset_shift()
	cargo.body.position = Layout.BELT_CENTER + Vector3(0, 0.4, 0)
	cargo.body.freeze = false
	conveyor.step(0.1, {1: cargo})
	check(cargo.body.linear_velocity.z < -0.75 and cargo.body.linear_velocity.z > -0.85, "grounded free cargo accelerates smoothly with belt")
	cargo.rules.holder_id = 1
	cargo.body.linear_velocity = Vector3.ZERO
	conveyor.step(0.1, {1: cargo})
	check(cargo.body.linear_velocity == Vector3.ZERO, "held cargo ignores belt")
	var clinger_cargo = Cargo.new()
	clinger_cargo.kind = "clinger"
	world.add_child(clinger_cargo)
	clinger_cargo.reset_shift()
	clinger_cargo.body.position = Layout.BELT_CENTER + Vector3(0, 0.4, 0)
	clinger_cargo.cling.target_kind = "worker"
	clinger_cargo.body.linear_velocity = Vector3.ZERO
	conveyor.step(0.1, {2: clinger_cargo})
	check(clinger_cargo.body.linear_velocity == Vector3.ZERO, "attached Clinger ignores belt")
	cargo.rules.holder_id = 0
	var hopper_cargo = Cargo.new()
	hopper_cargo.kind = "hopper"
	world.add_child(hopper_cargo)
	hopper_cargo.reset_shift()
	hopper_cargo.body.position = Layout.BELT_CENTER + Vector3(0, 0.7, 0)
	hopper_cargo.hopper.phase = "airborne"
	hopper_cargo.body.linear_velocity = Vector3.ZERO
	conveyor.step(0.1, {2: hopper_cargo})
	check(hopper_cargo.body.linear_velocity == Vector3.ZERO, "airborne Hopper ignores belt")

	var game = load("res://scenes/main.tscn").instantiate()
	root.add_child(game)
	game.practice_game()
	game.set_physics_process(false)
	check(game._snapshot().round.size() == 7 and game._snapshot().round[6] == -1, "snapshot appends only belt direction")
	var state: Dictionary = game._snapshot()
	state.round[6] = 1
	game._receive_snapshot(state)
	check(game.conveyor.direction == 1, "guest applies replicated direction")
	game.leave_game()
	game.queue_free()
	world.queue_free()
	await process_frame
	for failure in failures:
		push_error(failure)
	print("CONVEYOR %s" % ("PASS" if failures.is_empty() else "FAIL"))
	quit(0 if failures.is_empty() else 1)

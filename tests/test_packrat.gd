extends SceneTree

const Packrat = preload("res://scripts/packrat.gd")
const Worker = preload("res://scripts/worker.gd")
const Layout = preload("res://scripts/depot_layout.gd")
const Cargo = preload("res://scripts/cargo.gd")

class FakeRules extends RefCounted:
	var holder_id := 0
	var delivered := false

class FakeCargo extends Node3D:
	var cargo_id := 1
	var active := true
	var recovery_left := 0.0
	var creature_held := false
	var kind := "standard"
	var rules := FakeRules.new()
	var cling = null
	var hopper = null
	var body: RigidBody3D

	func _ready() -> void:
		body = RigidBody3D.new()
		body.collision_layer = 4
		body.collision_mask = 7
		var collision := CollisionShape3D.new()
		var shape := BoxShape3D.new()
		shape.size = Vector3.ONE * 0.8
		collision.shape = shape
		body.add_child(collision)
		add_child(body)

var failures: Array[String] = []

func check(ok: bool, message: String) -> void:
	if not ok:
		failures.append(message)

func _initialize() -> void:
	call_deferred("run")

func add_box(parent: Node3D, size: Vector3, at: Vector3) -> StaticBody3D:
	var solid := StaticBody3D.new()
	var collision := CollisionShape3D.new()
	var shape := BoxShape3D.new()
	shape.size = size
	collision.shape = shape
	solid.add_child(collision)
	solid.position = at
	parent.add_child(solid)
	return solid

func run() -> void:
	var world := Node3D.new()
	root.add_child(world)
	add_box(world, Vector3(32, 0.2, 36), Vector3(0, -0.1, -9))
	var rat = Packrat.new()
	world.add_child(rat)
	var cargo := FakeCargo.new()
	world.add_child(cargo)
	cargo.body.position = rat.position + Vector3(0.6, 0.4, 0)
	var cargos := {1: cargo}
	rat.reset(cargos)
	rat.start(0)
	rat.step(2.0, {}, cargos)
	check(not rat.active and not cargo.creature_held, "contract one has no Packrat")

	rat.start(1)
	var sight_wall := add_box(world, Vector3(0.15, 2, 1.5), rat.position + Vector3(0.3, 1, 0))
	rat.step(0.01, {}, cargos)
	check(rat.phase == "patrol", "static wall blocks cargo detection")
	sight_wall.queue_free()
	await physics_frame
	rat.step(0.01, {}, cargos)
	check(rat.phase == "warning" and rat.target_id == 1 and is_equal_approx(rat.remaining, 0.9), "eligible grounded cargo starts visible warning")
	cargo.rules.holder_id = 9
	rat.step(1.0, {}, cargos)
	check(not cargo.creature_held and rat.phase == "patrol", "pickup wins race during warning")
	cargo.rules.holder_id = 0
	rat.step(0.01, {}, cargos)
	rat.step(0.9, {}, cargos)
	check(cargo.creature_held and cargo.body.freeze and cargo.body.collision_mask == 1, "warning completion claims one cargo with static-only collision")

	var worker := Worker.new()
	worker.peer_id = 7
	world.add_child(worker)
	worker.position = rat.position + Vector3(0, 0, 2)
	worker.heading = 0
	check(rat.scare(worker, cargos), "nearby clear-sight airhorn forces a drop")
	check(not cargo.creature_held and not cargo.body.freeze and cargo.body.collision_layer == 4 and cargo.body.collision_mask == 7, "airhorn restores cargo physics")
	check(rat.phase == "flee" and not rat.scare(worker, cargos), "airhorn starts three-second flee and worker cooldown")
	rat.step(3.0, {7: worker}, cargos)
	check(rat.phase == "patrol", "flee ends after three seconds")

	rat.reset(cargos)
	rat.start(1)
	rat.position = Vector3(13, 0.05, -2.8)
	cargo.body.position = rat.position + Vector3(0, 0.4, -0.5)
	rat.step(0.01, {}, cargos)
	rat.step(0.9, {}, cargos)
	var carry_wall := add_box(world, Vector3(3, 2, 0.2), Vector3(13, 1, -3.65))
	await physics_frame
	var safe_position: Vector3 = cargo.body.position
	rat.step(0.5, {}, cargos)
	check(not cargo.creature_held and cargo.body.position.distance_to(safe_position) < 0.01, "blocked carried box drops at its last safe position")
	carry_wall.queue_free()
	await physics_frame

	rat.reset(cargos)
	rat.start(2)
	cargo.body.position = rat.position + Vector3(0.5, 0.4, 0)
	rat.step(0.01, {}, cargos)
	rat.step(0.9, {}, cargos)
	rat.position = Layout.NEST
	rat.step(0.01, {}, cargos)
	check(not cargo.creature_held and rat.protected_left(1) > 7.9, "nest drops cargo with eight-second protection")
	cargo.body.position = rat.position + Vector3(0.5, 0.4, 0)
	rat.step(1.0, {}, cargos)
	check(rat.phase != "warning", "protected cargo cannot be stolen again")

	rat.reset(cargos)
	rat.start(1)
	cargo.body.position = rat.position + Vector3(0.5, 0.4, 0)
	rat.step(0.01, {}, cargos)
	rat.step(0.9, {}, cargos)
	rat.stop(cargos)
	check(not cargo.creature_held and not cargo.body.freeze and cargo.body.collision_mask == 7, "stop restores carried cargo")
	cargo.active = false
	var wall := add_box(world, Vector3(6, 2, 0.3), Vector3(11, 1, -4.2))
	rat.reset(cargos)
	rat.start(1)
	rat.step(0.01, {}, cargos)
	rat.step(10.0, {}, cargos)
	check(rat.position.z > -4.15, "static wall blocks Packrat patrol movement")
	wall.queue_free()
	rat.stop(cargos)

	var real_cargo = Cargo.new()
	real_cargo.cargo_id = 9
	real_cargo.kind = "standard"
	world.add_child(real_cargo)
	real_cargo.reset_shift()
	var real_cargos := {9: real_cargo}
	rat.reset(real_cargos)
	real_cargo.body.position = rat.position + Vector3(1.1, 0.5, 0)
	for index in range(30):
		await physics_frame
	rat.start(1)
	rat.step(0.01, {}, real_cargos)
	rat.step(0.9, {}, real_cargos)
	check(real_cargo.creature_held, "settled real Cargo can be claimed on open floor: phase=%s remaining=%.2f cargo=%s velocity=%s" % [rat.phase, rat.remaining, real_cargo.body.position, real_cargo.body.linear_velocity])
	var carried_frames := 0
	for index in range(120):
		await physics_frame
		rat.step(1.0 / 60.0, {}, real_cargos)
		if real_cargo.creature_held:
			carried_frames += 1
		if rat.protected_left(9) > 0:
			break
	check(carried_frames > 20, "settled real Cargo remains carried across open-floor movement: frames=%d phase=%s rat=%s cargo=%s" % [carried_frames, rat.phase, rat.position, real_cargo.body.position])
	check(not real_cargo.creature_held and rat.protected_left(9) > 7.9, "real Cargo reaches nest and receives protection without teleporting rat: protected=%.2f" % rat.protected_left(9))
	rat.stop(real_cargos)

	var saved: Array = rat.snapshot()
	check(var_to_bytes(saved).size() <= 256, "Packrat snapshot stays under 256 bytes")
	var guest = Packrat.new()
	world.add_child(guest)
	guest.apply_snapshot(saved)
	check(guest.active == saved[0] and guest.phase == saved[1] and guest.target_position == saved[2] and guest.facing == saved[3], "guest applies compact state without simulation")

	world.queue_free()
	await process_frame
	await test_depot_search()
	for failure in failures:
		push_error(failure)
	print("PACKRAT %s" % ("PASS" if failures.is_empty() else "FAIL"))
	quit(0 if failures.is_empty() else 1)

func test_depot_search() -> void:
	var game = load("res://scenes/main.tscn").instantiate()
	root.add_child(game)
	game.practice_game(true)
	game.set_physics_process(false)
	game.set_process(false)
	for item in game.cargos.values(): item.cancel()
	var box = game.cargos[1]
	box.reset_shift()
	box.body.position = Vector3(2, 0.43, -16.4)
	game.packrat.reset(game.cargos)
	for frame in range(30): await physics_frame
	game.packrat.start(1)
	var start: Vector3 = game.packrat.position
	game.packrat.step(1.0 / 60.0, game.workers, game.cargos)
	check(game.packrat.phase == "seek", "visible cargo four metres away triggers active approach in real depot")
	for frame in range(300):
		await physics_frame
		game.packrat.step(1.0 / 60.0, game.workers, game.cargos)
		if box.creature_held: break
	check(box.creature_held, "rat approaches and steals distant settled cargo without teleporting")
	check(game.packrat.position.distance_to(start) > 1.0, "theft requires rat movement")
	game.packrat.reset(game.cargos)
	box.body.position = Vector3(2, 0.43, -16.4)
	box.rules.holder_id = 1
	game.packrat.start(1)
	game.packrat.step(0.01, game.workers, game.cargos)
	check(game.packrat.target_id == 0, "held cargo does not attract searching rat")
	box.rules.holder_id = 0
	for frame in range(30): await physics_frame
	game.packrat.step(0.01, game.workers, game.cargos)
	check(game.packrat.phase == "seek", "released cargo can be approached again")
	box.rules.holder_id = 1
	game.packrat.step(0.01, game.workers, game.cargos)
	check(game.packrat.phase == "patrol" and game.packrat.target_id == 0, "pickup during approach cancels pursuit")
	box.rules.holder_id = 0
	game.leave_game()
	game.queue_free()
	await process_frame

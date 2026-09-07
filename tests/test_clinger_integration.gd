extends SceneTree

const Layout = preload("res://scripts/depot_layout.gd")

var errors: Array[String] = []
func _initialize() -> void:
	call_deferred("run")

func check(ok: bool, message: String) -> void:
	if not ok:
		errors.append(message)

func run() -> void:
	var game = load("res://scenes/main.tscn").instantiate()
	root.add_child(game)
	game.practice_game()
	game.set_physics_process(false)
	var normal = game.cargos[1]
	var sticky = game.cargos[3]
	var worker = game.workers[1]
	worker.position = Vector3(0,0.05,5)
	normal.body.freeze = true
	normal.body.position = Vector3(0,1,4)
	sticky.body.freeze = true
	sticky.body.position = Vector3(0.85,1,4)
	sticky.cling.cooldown = 0
	await physics_frame
	await process_frame
	sticky.cling.step(0.01, game.workers, game.cargos)
	check(sticky.cling.target_kind == "cargo", "real Clinger attaches to Standard")
	check(not sticky.pickup(worker), "attached Clinger cannot be picked up directly")
	game._receive_action(1, "interact")
	check(normal.rules.holder_id == 1 and sticky.rules.holder_id == 0, "one held slot carries attached companion")
	var before: Vector3 = sticky.body.position
	worker.position.x += 0.2
	normal.step(0.01, game.workers)
	sticky.step(0.01, game.workers)
	sticky.cling.step(0.01, game.workers, game.cargos)
	check(sticky.body.position.x > before.x + 0.1, "attached companion follows held target")
	normal.release(worker, false)
	normal.body.freeze = true
	normal.body.position = game.depot.bay(1) - Vector3(0, 0.1, 0)
	sticky.body.position = game.depot.bay(1) + Vector3(0.85, -0.1, 0)
	normal.step(0.01, game.workers)
	sticky.step(0.01, game.workers)
	sticky.cling.step(0.01, game.workers, game.cargos)
	check(game.score == 2 and not normal.body.visible and not sticky.body.visible, "real bundle delivers both once")
	sticky.cling.step(0.01, game.workers, game.cargos)
	check(game.score == 2 and sticky.body.collision_layer == 0, "bundle recovery does not duplicate or leave hidden collision")
	game.start_shift()
	worker.position = Vector3(2,0.05,4)
	sticky.body.freeze = true
	sticky.body.position = Vector3(2.7,0.8,4)
	sticky.cling.cooldown = 0
	await physics_frame
	await process_frame
	sticky.cling.step(0.01, game.workers, game.cargos)
	check(sticky.cling.target_kind == "worker", "real Clinger attaches to worker")
	game._physics_process(1.0/60.0)
	check(is_equal_approx(worker.speed_scale,0.7), "main applies slowdown")
	sticky.recover("wrong_bay")
	game._physics_process(1.0/60.0)
	check(sticky.cling.target_kind.is_empty() and is_equal_approx(worker.speed_scale,1.0), "recovery clears attachment and slowdown")
	game.start_shift()
	check(sticky.cling.target_kind.is_empty() and sticky.cling.cooldown == 1.0, "restart resets attachment and grace")
	game.leave_game()
	check(not sticky.active and sticky.cling.target_kind.is_empty(), "lobby clears attachment")
	game.queue_free()
	await process_frame
	for message in errors:
		push_error(message)
	print("CLINGER INTEGRATION %s" % ("PASS" if errors.is_empty() else "FAIL"))
	quit(0 if errors.is_empty() else 1)

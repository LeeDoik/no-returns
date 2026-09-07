extends SceneTree

var failures: Array = []
func _initialize() -> void:
	call_deferred("run")
func check(ok: bool, message: String) -> void:
	if not ok: failures.append(message)
func run() -> void:
	var game = load("res://scenes/main.tscn").instantiate()
	root.add_child(game)
	game.host_game(27947)
	game.set_physics_process(false)
	game.start_shift()
	check(game.phase == "waiting", "host alone cannot start")
	if game.workers[1].get("slot") == null:
		push_error("Four-worker slots are not implemented")
		quit(1)
		return
	for id in [12,23,34]:
		game._add_worker(id)
		game.start_shift()
		check(game.phase == "playing", "2/3/4 workers can start")
		game.phase = "waiting"
	var slots := []
	var positions := []
	for worker in game.workers.values():
		check(not slots.has(worker.slot), "unique slot")
		check(not positions.has(worker.position), "unique spawn")
		slots.append(worker.slot)
		positions.append(worker.position)
	game.last_blast = {"source":2,"event":99,"workers":[1,12,23,34],"cargos":[1,3]}
	check(var_to_bytes(game._snapshot()).size() <= 1280, "four-player packet budget")
	var kept: int = game.workers[34].slot
	var freed: int = game.workers[12].slot
	game._player_left(12)
	await process_frame
	game._add_worker(45)
	check(game.workers[45].slot == freed and game.workers[34].slot == kept, "vacant slot reused without renumbering")
	game._add_worker(56)
	check(game.workers.size() == 4, "fifth worker refused")
	game.start_shift()
	check(game.phase == "playing", "replacement roster restarts")
	game.leave_game()
	game.queue_free()
	await process_frame
	for message in failures: push_error(message)
	print("ROSTER %s" % ("PASS" if failures.is_empty() else "FAIL"))
	quit(0 if failures.is_empty() else 1)

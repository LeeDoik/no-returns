extends "res://tests/capture_sneezer.gd"

func _run() -> void:
	game = load("res://scenes/main.tscn").instantiate()
	root.add_child(game)
	game.practice_game()
	game.set_physics_process(false)
	var worker = game.workers[1]
	worker.position = Vector3(-5, 0.05, 5)
	var cargo = game.cargos[2]
	cargo.body.position = worker.position + Vector3(0, 1.05, -0.98)
	cargo.pickup(worker)
	game.depot.overview.current = true
	game.depot.overview.position = Vector3(-1, 5, 10)
	game.depot.overview.look_at(Vector3(-5, 1, 5))
	for yaw in [0.0, 1.0]:
		worker.heading = yaw
		cargo.move_held(worker)
		await settle(8)
		await capture("cargo-facing-%d" % int(yaw))
	game.leave_game()
	game.queue_free()
	await process_frame
	quit(1 if failures else 0)

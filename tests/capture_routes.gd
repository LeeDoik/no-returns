extends "res://tests/capture_sneezer.gd"
func _run() -> void:
	game = load("res://scenes/main.tscn").instantiate(); root.add_child(game)
	game.set_physics_process(false)
	await settle(15)
	await capture("routes-menu")
	game.practice_game(true); game.ui.visible = false
	game.depot.overview.current = true
	game.depot.overview.fov = 50
	game.depot.overview.position = Vector3(0,46,27)
	game.depot.overview.look_at(Vector3(0,0,-18))
	await settle(10); await capture("routes-overhead")
	game.depot.overview.fov = 75
	game.depot.overview.position = Vector3(7,6,-16)
	game.depot.overview.look_at(Vector3(0,2,-28))
	await settle(8); await capture("routes-gate-closed")
	game.workers[1].position = game.routes.get_node("Gate/PlateFront").global_position
	game.routes.step(6.6,game.workers,game.cargos)
	await settle(8); await capture("routes-gate-open")
	game.depot.overview.position = Vector3(14,5,-18)
	game.depot.overview.look_at(Vector3(21,2,-28))
	await settle(8); await capture("routes-airmail")
	game.queue_free(); await process_frame
	print("ROUTES CAPTURE PASS"); quit(0)

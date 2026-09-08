extends "res://tests/capture_sneezer.gd"
func _run() -> void:
	game = load("res://scenes/main.tscn").instantiate(); root.add_child(game)
	game.set_physics_process(false); game.practice_game(true); game.ui.visible = false
	game.depot.overview.current = true
	game.depot.overview.fov = 55
	game.depot.overview.position = Vector3(0,46,27)
	game.depot.overview.look_at(Vector3(0,0,-18))
	await settle(20); await capture("winding-overhead")
	game.depot.overview.fov = 75
	game.depot.overview.position = Vector3(-11,8,-5)
	game.depot.overview.look_at(Vector3(-17,1,-23))
	await settle(8); await capture("winding-alley")
	game.routes.step(6.6,{}, {})
	game.depot.overview.position = Vector3(5,6,-19)
	game.depot.overview.look_at(Vector3(17,2,-25))
	await settle(8); await capture("winding-crosswind")
	game.depot.overview.position = Vector3(7,3.5,4)
	game.depot.overview.look_at(Vector3(-1,1.2,-6))
	await settle(8); await capture("winding-interior")
	game.queue_free(); await process_frame
	print("WINDING CAPTURE PASS"); quit(0)

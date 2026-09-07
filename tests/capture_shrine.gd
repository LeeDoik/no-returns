extends "res://tests/capture_sneezer.gd"

func _run() -> void:
	game = load("res://scenes/main.tscn").instantiate()
	root.add_child(game)
	game.set_physics_process(false)
	await settle(20)
	await capture("shrine-menu-ko")
	game.ui._toggle_language()
	await settle(12)
	await capture("shrine-menu-en")
	game.ui._toggle_language()
	game.practice_game(true)
	await settle(15)
	await capture("shrine-entry")
	game.ui.visible = false
	game.depot.overview.current = true
	game.depot.overview.position = Vector3(0, 25, 13)
	game.depot.overview.look_at(Vector3(0, 1, -9))
	await settle(10)
	await capture("shrine-overhead")
	game.depot.overview.position = Vector3(0, 8.5, -13)
	game.depot.overview.fov = 90
	game.depot.overview.look_at(Vector3(0, 3, -25))
	await settle(10)
	await capture("shrine-altars")
	game.depot.overview.fov = 70
	game.depot.overview.position = Vector3(9, 3.5, -1)
	game.depot.overview.look_at(Vector3(13, 1.8, -5))
	await settle(10)
	await capture("shrine-employee")
	game.queue_free()
	await process_frame
	print("SHRINE CAPTURE PASS")
	quit(0)

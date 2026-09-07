extends "res://tests/capture_sneezer.gd"

func _run() -> void:
	game = load("res://scenes/main.tscn").instantiate()
	root.add_child(game)
	game.test_mode = true
	game.set_physics_process(false)
	await settle(20)
	await capture("four-menu-ko")
	game.ui._toggle_language()
	await settle(3)
	await capture("four-menu-en")
	game.ui._toggle_language()
	game.host_game(27948)
	for id in [12,23,34]:
		game._add_worker(id)
	await settle(3)
	await capture("four-lobby")
	game.start_shift()
	game.workers[1].position = Vector3(-3,0.05,6)
	game.workers[1].look_pitch = -0.35
	game.workers[1].update_look()
	game.workers[12].position = Vector3(-1,0.05,4)
	game.workers[23].position = Vector3(1,0.05,4)
	game.workers[34].position = Vector3(3,0.05,4)
	await settle(8)
	await capture("four-workers")
	game.leave_game()
	game.queue_free()
	await process_frame
	print("VISUAL CAPTURE %s" % ("PASS" if failures == 0 else "FAIL"))
	quit(0 if failures == 0 else 1)

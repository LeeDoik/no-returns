extends "res://tests/capture_sneezer.gd"

# Real-renderer review fixture for the 0.6 depot presentation.
func _run() -> void:
	game = load("res://scenes/main.tscn").instantiate()
	root.add_child(game)
	game.test_mode = true
	game.set_physics_process(false)
	await settle(25)
	for landmark in ["SortingWall", "DispatchA", "DispatchB", "PackratNest"]:
		if game.depot.get_node_or_null(landmark) == null:
			failures += 1
			push_error("Night depot landmark missing: %s" % landmark)
	await capture("night-depot-menu-ko")
	game.ui._toggle_language()
	await settle(3)
	await capture("night-depot-menu-en")
	game.ui._toggle_language()
	game.ui.visible = false
	game.depot.overview.current = true
	game.depot.overview.position = Vector3(0, 29, 7)
	game.depot.overview.look_at(Vector3(0, 0, -9))
	await settle(4)
	await capture("night-depot-overhead")

	game.practice_game()
	var worker = game.workers[1]
	worker.position = Vector3(-10, 0.05, -15)
	worker.look_yaw = PI
	worker.look_pitch = -0.16
	worker.update_look()
	await settle(5)
	await capture("night-depot-third-person")

	game.depot.overview.current = true
	game.depot.overview.position = Vector3(14.5, 5.6, 1.5)
	game.depot.overview.look_at(Vector3(11, 0.8, -10))
	await settle(4)
	await capture("night-depot-dispatch-nest")
	game.ui.visible = true
	worker.position = Vector3(2, 0.05, -2)
	worker.look_yaw = 0
	worker.heading = 0
	worker.update_look()
	worker.camera.current = true
	await settle(4)
	await capture("night-depot-lever-ko")
	game.ui._toggle_language()
	await settle(4)
	await capture("night-depot-lever-en")
	game.queue_free()
	await process_frame
	print("NIGHT DEPOT VISUAL CAPTURE %s" % ("PASS" if failures == 0 else "FAIL"))
	quit(0 if failures == 0 else 1)

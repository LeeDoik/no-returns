extends "res://tests/capture_sneezer.gd"
func _run() -> void:
	game = load("res://scenes/main.tscn").instantiate(); root.add_child(game)
	game.set_physics_process(false); game.practice_game(true); game.ui.visible = false
	var worker = game.workers[1]
	for view in [["enclosed-intake",Vector3(0,0.05,2),0.0], ["enclosed-archive",Vector3(-21,0.05,-15),0.0], ["enclosed-wind",Vector3(16,0.05,-24),-PI/2], ["enclosed-dispatch",Vector3(-10,0.05,-41),PI/2]]:
		worker.position = view[1]; worker.look_yaw = view[2]; worker.heading = view[2]; worker.look_pitch = -0.2; worker.update_look(); worker.camera.current = true
		await settle(20); await capture(view[0])
	game.depot.overview.current = true; game.depot.overview.fov = 58
	game.depot.overview.position = Vector3(0,51,27); game.depot.overview.look_at(Vector3(0,0,-18))
	await settle(10); await capture("enclosed-overhead")
	game.queue_free(); await process_frame
	print("ENCLOSED CAPTURE %s" % ("PASS" if failures == 0 else "FAIL")); quit(1 if failures else 0)

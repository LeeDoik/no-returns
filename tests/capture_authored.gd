extends "res://tests/capture_sneezer.gd"
func _run() -> void:
	game = load("res://scenes/main.tscn").instantiate(); root.add_child(game)
	game.set_physics_process(false); game.practice_game(true); game.ui.visible = false
	var worker = game.workers[1]
	for view in [["authored-intake",Vector3(0,0.05,2),0.0], ["authored-archive",Vector3(-21,0.05,-15),0.0], ["authored-wind",Vector3(16,0.05,-24),-PI/2], ["authored-dispatch",Vector3(-10,0.05,-41),PI/2]]:
		worker.position = view[1]; worker.look_yaw = view[2]; worker.heading = view[2]; worker.look_pitch = -0.2; worker.update_look(); worker.camera.current = true
		await settle(20); await capture(view[0])
	game.depot.overview.current = true; game.depot.overview.position = Vector3(-3.5,2.8,3); game.depot.overview.look_at(Vector3(-6,1.2,1))
	await settle(20); await capture("authored-packing-desk")
	worker.position = Vector3(0,0.05,3); worker.heading = 0
	var cargo = game.cargos[1]; cargo.body.freeze = true; cargo.body.position = worker.hand_position()
	cargo.pickup(worker); worker.held = true
	game.depot.overview.current = true; game.depot.overview.position = Vector3(4,2.7,0); game.depot.overview.look_at(Vector3(0,1,2.7))
	await settle(20); await capture("authored-carry")
	for arm in worker.arms:
		var glove: MeshInstance3D = arm.get_child(0)
		if (glove.global_position-worker.global_position).dot(worker.forward()) <= 0:
			push_error("Carrying gloves must reach toward the parcel"); failures += 1
	preload("res://scripts/copy.gd").language = "en"; game.routes._present()
	worker.position = Vector3(16,0.05,-24); worker.look_yaw = -PI/2; worker.heading = -PI/2; worker.held = false; worker.update_look(); worker.camera.current = true
	await settle(20); await capture("authored-wind-en")
	print("RENDER DRAW CALLS: ",Performance.get_monitor(Performance.RENDER_TOTAL_DRAW_CALLS_IN_FRAME))
	game.depot.overview.current = true; game.depot.overview.fov = 58
	game.depot.overview.position = Vector3(0,51,27); game.depot.overview.look_at(Vector3(0,0,-18))
	await settle(10); await capture("authored-overhead")
	game.queue_free(); await process_frame
	print("AUTHORED CAPTURE %s" % ("PASS" if failures == 0 else "FAIL")); quit(1 if failures else 0)

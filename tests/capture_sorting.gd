extends "res://tests/capture_sneezer.gd"
func run_camera(at: Vector3, toward: Vector3) -> void:
	game.depot.overview.global_position = at; game.depot.overview.look_at(toward); game.depot.overview.fov = 70; game.depot.overview.current = true
func _run() -> void:
	game = load("res://scenes/main.tscn").instantiate(); root.add_child(game); game.practice_game(true)
	game.set_process(false); game.set_physics_process(false); game.ui.visible = false
	game.packrat.start(1)
	run_camera(Vector3(9,5,-21),Vector3(1,0.9,-13.8)); await settle(16); await capture("sorting-hub-ko")
	run_camera(Vector3(4.4,3.1,-5.8),Vector3(0,1,-9.7)); await settle(8); await capture("sorting-input-ko")
	for id in game.cargos:
		if id in [3,4]: game.cargos[id].reset_shift()
		game.cargos[id].active = id in [3,4]; game.cargos[id].body.visible = id in [3,4]
	var hopper = game.cargos[4]; var clinger = game.cargos[3]
	hopper.body.freeze = true; clinger.body.freeze = true; await physics_frame
	hopper.body.position = Vector3(-0.42,0.43,-13.2); hopper.facing = 0
	clinger.body.position = Vector3(0.42,0.43,-13.2); clinger.cling.cooldown = 0
	await physics_frame; hopper.body.freeze = false; clinger.body.freeze = false
	await physics_frame; clinger.cling.step(0.01,{},game.cargos)
	hopper.hopper.phase = "windup"; hopper.hopper.remaining = 0.01
	run_camera(Vector3(4,2.9,-17),Vector3(0,1,-14))
	for i in range(20):
		await physics_frame; hopper.step(1.0/60,game.workers); clinger.cling.step(1.0/60,{},game.cargos)
	print("PAIR FRAME ",hopper.body.position," ",clinger.body.position," visible ",hopper.body.visible," ",clinger.body.visible)
	await capture("sorting-paired-hop-ko")
	preload("res://scripts/copy.gd").language = "en"
	run_camera(Vector3(9,5,-21),Vector3(1,0.9,-13.8)); await settle(8); await capture("sorting-hub-en")
	game.leave_game(); game.queue_free(); await process_frame; print("SORTING CAPTURE PASS"); quit()

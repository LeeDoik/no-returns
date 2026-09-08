extends "res://tests/capture_sorting.gd"
const Art = preload("res://scripts/postal_art.gd")
func _run() -> void:
	game = load("res://scenes/main.tscn").instantiate(); root.add_child(game); game.practice_game(true)
	root.msaa_3d = Viewport.MSAA_4X
	game.set_process(false); game.set_physics_process(false); game.ui.visible = false
	for c in game.cargos.values(): c.body.freeze = true; c.body.visible = false
	await physics_frame
	for i in range(1,5):
		var c = game.cargos[i]; c.body.visible = true; c.body.position = Vector3(-3.0+(i-1)*1.3,0.43,2.0); c.facing = PI
	game.workers[1].position = Vector3(2.6,0.05,2.0); game.workers[1].heading = PI
	run_camera(Vector3(2.8,2.4,6.4),Vector3(-0.3,0.65,2)); await settle(15); await capture("art-parcels")
	game.cargos[2].sneeze.phase = "windup"; game.cargos[2].sneeze.remaining = 0.3
	run_camera(Vector3(-0.7,1.2,4),Vector3(-1.7,0.6,2)); await settle(3); await capture("art-sneeze-warning")
	game.cargos[2].sneeze.phase = "burst"; await settle(3); await capture("art-sneeze-burst")
	run_camera(Vector3(5.1,3.3,4.8),Vector3(-1.4,1,-2)); await settle(5); await capture("art-receiving")
	run_camera(Vector3(4.4,3.1,-5.8),Vector3(0,1,-9.7)); await settle(5); await capture("art-sorting")
	game.packrat.start(1); game.packrat.position = Vector3(4,0.05,-16); run_camera(Vector3(2,1.3,-18.5),Vector3(4,0.4,-16)); await settle(5); await capture("art-packrat")
	var worker = game.workers[1]; worker.position = Vector3(0,0.05,3); worker.heading = 0; worker.held = true
	for id in [2,3,4]: game.cargos[id].body.visible = false
	game.cargos[1].body.position = worker.hand_position(); game.cargos[1].facing = 0
	run_camera(Vector3(2.4,1.8,0.7),Vector3(0,1,2.6)); await settle(35); await capture("art-carry")
	for p in game.reactions.props:
		if p.kind == 0: print("CUSHION PIECES ",p.pieces.size()); break
	game.leave_game(); game.queue_free(); await process_frame; await process_frame
	Art.models.clear(); Art.materials.clear()
	print("POSTAL ART CAPTURE PASS"); quit()

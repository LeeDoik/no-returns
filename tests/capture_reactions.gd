extends "res://tests/capture_sorting.gd"
func _run() -> void:
	game = load("res://scenes/main.tscn").instantiate(); root.add_child(game); game.practice_game(true)
	game.set_process(false); game.set_physics_process(false); game.ui.visible = false
	for c in game.cargos.values(): c.body.freeze = true; c.body.visible = false
	game.workers[1].position = Vector3(-5,0.05,-14.4)
	var paper = game.reactions.get_node("Paperwork/Documents05")
	var source = game.cargos[2]; source.body.visible = true; source.body.position = paper.global_position+Vector3(0,0,-2); source.body.position.y = 1.05; source.facing = PI
	run_camera(paper.global_position+Vector3(3,1.8,-3),paper.global_position+Vector3(0,0.6,0)); await settle(15); await capture("paperwork-before")
	game._apply_sneeze(source); game.reactions.step(0.2,{},{})
	await create_timer(0.24).timeout; await capture("paperwork-bundle")
	await create_timer(0.55).timeout; await capture("paperwork-flutter")
	game.reactions.reset()
	var spring = game.reactions.get_node("Intake/ReturnSpring")
	var cargo = game.cargos[1]; cargo.body.visible = true; cargo.body.freeze = false; cargo.body.position = spring.global_position+Vector3.UP*0.43
	game.workers[1].position = spring.global_position+Vector3(-0.55,0.05,0)
	run_camera(Vector3(3,4.5,3),Vector3(-3,0.8,-1.5)); await settle(10); await capture("reactive-intake-ready")
	game.reactions.step(0.2,game.workers,{1:cargo})
	game.reactions.get_node("Intake/CartonTower").arm(); game.reactions.step(0.2,{},{})
	game.reactions.step(0.2,{},{})
	for i in range(15):
		await physics_frame; game.workers[1].simulate(Vector2.ZERO,0,false,1.0/60)
	await capture("reactive-intake-burst")
	game.leave_game(); game.queue_free(); await process_frame; print("REACTIONS CAPTURE PASS"); quit()

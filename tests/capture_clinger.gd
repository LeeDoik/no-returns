extends "res://tests/capture_sneezer.gd"

func _run() -> void:
	game = load("res://scenes/main.tscn").instantiate()
	root.add_child(game)
	game.test_mode = true
	game.set_physics_process(false)
	await settle(25)
	await capture("clinger-menu-ko")
	game.ui._toggle_language()
	await settle(3)
	await capture("clinger-menu-en")
	game.ui._toggle_language()
	game.practice_game()
	game._add_worker(2)
	game.workers[1].position = Vector3(-3, 0.05, 6)
	game.workers[1].look_pitch = -0.35
	game.workers[1].update_look()
	game.workers[2].position = Vector3(-0.7, 0.05, 4)
	game.cargos[1].body.freeze = true
	game.cargos[1].body.position = Vector3(4, 0.55, 4)
	var sticky = game.cargos[3]
	sticky.body.freeze = true
	sticky.body.position = Vector3(-1.4, 0.8, 4)
	sticky.cling.cooldown = 0
	var source = game.cargos[2]
	source.body.freeze = true
	source.body.position = Vector3(-2, 0.55, 2)
	source.facing = PI
	await settle(3)
	sticky.cling.step(0.01, game.workers, game.cargos)
	await settle(3)
	await capture("clinger-attached")
	source.sneeze.phase = "windup"
	source.sneeze.remaining = 0.8
	await settle(3)
	await capture("clinger-rescue-warning")
	source.step(0.81, game.workers)
	await settle(3)
	await capture("clinger-released")
	game.leave_game()
	game.queue_free()
	await process_frame
	print("VISUAL CAPTURE %s" % ("PASS" if failures == 0 else "FAIL"))
	quit(0 if failures == 0 else 1)

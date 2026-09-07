extends "res://tests/capture_sneezer.gd"

func _run() -> void:
	game = load("res://scenes/main.tscn").instantiate()
	root.add_child(game)
	game.test_mode = true
	game.set_physics_process(false)
	await settle(25)
	game.practice_game()
	var worker = game.workers[1]
	worker.position = Vector3(1.8, 0.05, 5.0)
	worker.look_pitch = -0.35
	worker.update_look()
	var hopper = game.cargos[4]
	hopper.body.freeze = true
	hopper.body.position = Vector3(0.8, 0.55, 3.5)
	hopper.facing = PI
	hopper.hopper.phase = "rest"
	hopper.hopper.remaining = 2.4
	await settle(3)
	await capture("hopper-rest")
	hopper.hopper.phase = "windup"
	hopper.hopper.remaining = 0.7
	await settle(3)
	await capture("hopper-windup")
	hopper.rules.holder_id = 1
	hopper.hopper.paused = true
	await settle(3)
	await capture("hopper-held-pause")
	hopper.rules.holder_id = 0
	hopper.hopper.phase = "airborne"
	hopper.hopper.paused = true
	hopper.body.position += Vector3(0, 1.4, -1.2)
	await settle(3)
	await capture("hopper-airborne")
	game.leave_game()
	game.queue_free()
	await process_frame
	print("HOPPER VISUAL CAPTURE %s" % ("PASS" if failures == 0 else "FAIL"))
	quit(0 if failures == 0 else 1)

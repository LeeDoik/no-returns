extends "res://tests/capture_sneezer.gd"

func _run() -> void:
	game = load("res://scenes/main.tscn").instantiate()
	root.add_child(game)
	game.set_physics_process(false)
	await settle(25)
	await capture("expanded-menu-ko")
	game.ui._toggle_language()
	await settle(3)
	await capture("expanded-menu-en")
	game.ui._toggle_language()
	game.practice_game()
	game.ui.paused = true
	await settle(5)
	await capture("expanded-settings-ko")
	game.ui.paused = false
	game.cargos[1].rules.score = 5
	game._finish("won")
	await settle(5)
	await capture("expanded-results-ko")
	game.ui._toggle_language()
	await settle(5)
	await capture("expanded-results-en")
	game.leave_game()
	game.queue_free()
	await process_frame
	print("EXPANDED CAPTURE %s" % ("PASS" if failures == 0 else "FAIL"))
	quit(1 if failures else 0)

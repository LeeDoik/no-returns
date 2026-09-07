extends SceneTree

# Actual-renderer inspection fixture; not part of the headless gameplay suite.
var game: Node3D
var failures := 0

func _initialize() -> void:
	call_deferred("_run")

func capture(label: String) -> void:
	await RenderingServer.frame_post_draw
	var path := "res://artifacts/%s.png" % label
	var result := root.get_texture().get_image().save_png(path)
	if result != OK:
		failures += 1
	print("CAPTURE %s: %s" % [path, error_string(result)])

func settle(count: int) -> void:
	for index in range(count):
		await process_frame

func _run() -> void:
	game = load("res://scenes/main.tscn").instantiate()
	root.add_child(game)
	game.test_mode = true
	game.set_physics_process(false)
	await settle(25)
	await capture("sneezer-menu-ko")
	game.ui._toggle_language()
	await settle(3)
	await capture("sneezer-menu-en")
	game.ui._toggle_language()
	game.practice_game()
	game._add_worker(2)
	var worker = game.workers[1]
	worker.position = Vector3(-3, 0.05, 6)
	worker.look_pitch = -0.35
	worker.update_look()
	game.workers[2].position = Vector3(-0.7, 0.05, 5.0)
	game.cargos[1].body.freeze = true
	game.cargos[1].body.position = Vector3(-0.7, 0.55, 3.8)
	var source = game.cargos[2]
	source.body.freeze = true
	source.body.position = Vector3(-2, 0.55, 2)
	source.facing = PI
	source.sneeze.phase = "windup"
	source.sneeze.remaining = 0.8
	await settle(8)
	await capture("sneezer-windup")
	source.step(0.81, game.workers)
	await settle(3)
	for member in game.workers.values():
		member.simulate(Vector2.ZERO, 0, false, 1.0 / 60.0)
	await capture("sneezer-burst")
	game.ui.paused = true
	game.ui._toggle_sound()
	await settle(3)
	await capture("sneezer-pause-muted")
	game.leave_game()
	game.queue_free()
	await process_frame
	print("VISUAL CAPTURE %s" % ("PASS" if failures == 0 else "FAIL"))
	quit(0 if failures == 0 else 1)

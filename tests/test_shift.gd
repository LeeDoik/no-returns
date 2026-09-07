extends SceneTree

var game: Node
var failures := 0

func _initialize() -> void:
	call_deferred("_run")

func _run() -> void:
	game = load("res://scenes/main.tscn").instantiate()
	root.add_child(game)
	game.practice_game()
	await _frames(4)
	check(game.phase == "playing" and game.score == 0, "practice starts a fresh shift")
	game.session.send_input(Vector2(INF, 0), 0.0, false)
	check(not game.inputs.has(1), "invalid movement is rejected before simulation")
	game.cargos[1].body.freeze = true
	game.cargos[1].body.position = Vector3(4.5, 0.65, -7.0)
	await _frames(3)
	check(game.score == 0 and not game.cargos[1].body.visible and game.notice_key == "wrong_bay", "wrong bay returns without scoring")
	await create_timer(1.4).timeout
	check(game.cargos[1].body.visible and game.cargos[1].body.position.distance_to(Vector3(0, 0.55, 1.1)) < 0.4, "returned cargo arrives at intake")
	game.cargos[1].body.freeze = true
	game.cargos[1].body.position = Vector3(0, -6, 0)
	await _frames(3)
	check(game.cargos[1].recovery_left > 0 and game.score == 0, "lost cargo enters recovery without scoring")
	await create_timer(1.4).timeout
	for index in range(5):
		game.cargos[1].body.freeze = true
		game.cargos[1].body.position = Vector3(-4.5 if game.cargos[1].rules.destination == 1 else 4.5, 0.65, -7.0)
		await _frames(3)
		check(game.score == index + 1, "physical delivery %d scores once" % (index + 1))
		if index < 4:
			await create_timer(1.4).timeout
	check(game.phase == "won", "fifth delivery ends shift successfully")
	game.session.send_action("start")
	await _frames(3)
	check(game.phase == "playing" and game.score == 0 and game.time_left > 179, "result restart clears score and restores time")
	game.time_left = 0.01
	await _frames(3)
	check(game.phase == "lost", "timer expiry ends shift")
	await create_timer(0.2).timeout
	game.session.send_action("start")
	await _frames(3)
	check(game.phase == "playing" and game.cargos[1].body.visible, "timeout can restart with usable cargo")
	game.leave_game()
	game.queue_free()
	await process_frame
	if failures == 0:
		print("PASS shift: wrong bay, lost cargo, five deliveries, success/timeout restart, invalid input")
	quit(1 if failures else 0)

func _frames(count: int) -> void:
	for index in range(count):
		await physics_frame

func check(condition: bool, description: String) -> void:
	if not condition:
		failures += 1
		push_error("SHIFT FAIL: " + description)

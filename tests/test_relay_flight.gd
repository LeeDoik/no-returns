extends SceneTree
var game: Node
func _initialize() -> void: call_deferred("run")
func run() -> void:
	game = load("res://scenes/main.tscn").instantiate()
	root.add_child(game)
	game.practice_game(true)
	game.set_physics_process(false)
	game._add_worker(22)
	for item in game.cargos.values(): item.cancel()
	var cargo = game.cargos[1]
	cargo.active = true
	cargo.reset_crate()
	var sender = game.workers[1]
	var receiver = game.workers[22]
	sender.position = Vector3(-10, 0, 3)
	sender.heading = 0
	receiver.position = Vector3(-10, 0, -1.25)
	receiver.heading = PI
	cargo.body.freeze = true
	cargo.body.position = sender.hand_position()
	await physics_frame
	await process_frame
	if not cargo.pickup(sender): return fail("sender pickup")
	cargo.release(sender, true)
	var origin: Vector3 = cargo.body.position
	for frame in range(90):
		await physics_frame
		await process_frame
		cargo.step(1.0/60.0, game.workers)
		if cargo.body.position.distance_to(origin) >= 3.1:
			if not cargo.pickup(receiver) or not cargo.relay_ready:
				return fail("actual airborne catch at %s from %s" % [cargo.body.position,origin])
			print("RELAY FLIGHT PASS: real physics throw travels >=3m before another worker catches")
			game.leave_game()
			game.queue_free()
			await process_frame
			quit(0)
			return
	fail("throw did not travel three metres")
func fail(reason: String) -> void:
	push_error(reason)
	quit(1)

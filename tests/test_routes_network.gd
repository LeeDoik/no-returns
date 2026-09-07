extends "res://tests/test_expanded_network.gd"
func begin() -> void:
	checkpoint = Checkpoint.new(); checkpoint.name = "RoutesCheckpoint"; root.add_child(checkpoint)
	game = load("res://scenes/main.tscn").instantiate(); root.add_child(game)
	deadline = Time.get_ticks_msec()+20000
	if role == "host": game.host_game(27953)
	else: game.join_game("127.0.0.1",27953)
func host_step() -> void:
	match stage:
		0:
			if checkpoint.guest_id == 0 or game.workers.size() != 2: return
			game.start_shift(); game.set_physics_process(false)
			game.workers[1].position = game.routes.get_node("Gate/PlateFront").global_position
			game.routes.step(6.7,game.workers,game.cargos)
			game.session.publish(game._snapshot()); game._publish_metadata(true)
			announce("active"); stage = 1
		1:
			if not checkpoint.acknowledged: return
			game.start_shift(); game.set_physics_process(false)
			game.session.publish(game._snapshot()); game._publish_metadata(true)
			announce("reset"); stage = 2
		2:
			if not checkpoint.acknowledged: return
			print("PASS network host: pressure gate and gust state replicate and reset")
			game.leave_game(); quit(0)
func guest_step() -> void:
	if game.session.confirmed and checkpoint.guest_id == 0:
		checkpoint.guest_id = game.multiplayer.get_unique_id(); checkpoint.identify.rpc_id(1)
	match checkpoint.phase:
		"active":
			if not game.routes.gate_open or game.routes.wind_phase() != 2: return
			if not game.routes.get_node("Gate/Door/CollisionShape3D").disabled: fail("guest door collision disagrees"); return
			ack()
		"reset":
			if game.routes.gate_open or game.routes.wind_phase() != 0: return
			if game.routes.get_node("Gate/Door/CollisionShape3D").disabled: fail("guest reset leaves door passable"); return
			if sent != "reset":
				ack()
				print("PASS network guest: same gate, gust and new-contract reset")
				# Keep the reliable acknowledgement alive until the host disconnects.
	if sent == "reset" and game.phase == "menu": quit(0)

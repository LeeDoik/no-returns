extends "res://tests/test_expanded_network.gd"

func begin() -> void:
	checkpoint = Checkpoint.new(); checkpoint.name = "WorkerAnimationCheckpoint"; root.add_child(checkpoint)
	game = load("res://scenes/main.tscn").instantiate(); root.add_child(game)
	deadline = Time.get_ticks_msec() + 20000
	if role == "host": game.host_game(27959)
	else: game.join_game("127.0.0.1",27959)

func host_step() -> void:
	match stage:
		0:
			if checkpoint.guest_id == 0 or game.workers.size() != 2: return
			game.start_shift(); game.set_physics_process(false)
			game.session.publish(game._snapshot()); game._publish_metadata(true)
			announce("ready"); stage = 1
		1:
			if not checkpoint.acknowledged: return
			game.workers[1].play_throw()
			game._publish_metadata(true)
			announce("throw"); stage = 2
		2:
			if not checkpoint.acknowledged: return
			print("PASS network host: worker throw follows authoritative sequence")
			game.leave_game(); quit(0)

func guest_step() -> void:
	if sent == "throw" and game.phase == "menu": quit(0); return
	if game.session.confirmed and checkpoint.guest_id == 0:
		checkpoint.guest_id = game.multiplayer.get_unique_id(); checkpoint.identify.rpc_id(1)
	if not game.workers.has(1): return
	var worker = game.workers[1]
	match checkpoint.phase:
		"ready":
			if worker.animation_wire_initialized: ack()
		"throw":
			if sent == "throw" or worker.throw_sequence != 1: return
			if worker.art_player.current_animation != "throw": return
			ack(); print("PASS network guest: remote throw clip actually plays")

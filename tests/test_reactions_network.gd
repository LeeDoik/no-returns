extends "res://tests/test_expanded_network.gd"
var saw_warning := false
func begin() -> void:
	checkpoint = Checkpoint.new(); checkpoint.name = "ReactionsCheckpoint"; root.add_child(checkpoint)
	game = load("res://scenes/main.tscn").instantiate(); root.add_child(game)
	deadline = Time.get_ticks_msec()+20000
	if role == "host": game.host_game(27955)
	else: game.join_game("127.0.0.1",27955)
func host_step() -> void:
	match stage:
		0:
			if checkpoint.guest_id == 0 or game.workers.size() != 2: return
			game.start_shift()
			game.workers[checkpoint.guest_id].position = game.reactions.get_node("Intake/ReturnSpring").global_position
			# Trigger just after an ordinary metadata publication. Automatic transition
			# publication must deliver the 0.18s warning before the next 0.2s tick.
			game.tick = 12; game._publish_metadata(true)
			game.reactions.get_node("Paperwork/Documents00").arm(Vector3.RIGHT)
			announce("reaction"); stage = 1
		1:
			if not checkpoint.acknowledged: return
			game.start_shift(); game.set_physics_process(false); game.session.publish(game._snapshot()); game._publish_metadata(true)
			announce("reset"); stage = 2
		2:
			if not checkpoint.acknowledged: return
			print("PASS network host: spring and directed paper event replicate and reset"); game.leave_game(); quit(0)
func guest_step() -> void:
	if game.session.confirmed and checkpoint.guest_id == 0:
		checkpoint.guest_id = game.multiplayer.get_unique_id(); checkpoint.identify.rpc_id(1)
	match checkpoint.phase:
		"reaction":
			var spring = game.reactions.get_node("Intake/ReturnSpring"); var paper = game.reactions.get_node("Paperwork/Documents00")
			if spring.phase == 1: saw_warning = true
			if spring.event_id != 1 or paper.event_id != 1: return
			if not saw_warning: fail("guest missed spring compression warning"); return
			if paper.phase != 2 or not paper.burst_direction.is_equal_approx(Vector3.RIGHT): fail("guest paper direction/state differs"); return
			var first: int = paper.event_id; game.reactions.apply_snapshot(game.reactions.snapshot())
			if paper.event_id != first: fail("duplicate metadata repeats event"); return
			ack()
		"reset":
			for p in game.reactions.props:
				if p.phase != 0 or p.event_id != 0: return
			if sent != "reset": ack(); print("PASS network guest: same spring/paper direction, duplicate and reset state")
	if sent == "reset" and game.phase == "menu": quit(0)

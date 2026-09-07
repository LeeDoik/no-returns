extends SceneTree

func _initialize() -> void:
	call_deferred("run")

func run() -> void:
	var game = load("res://scenes/main.tscn").instantiate()
	root.add_child(game)
	game.practice_game()
	game.set_physics_process(false)
	if game.cargos.size() != 4 or not game.cargos[1].has_method("wire_snapshot"):
		push_error("Four-cargo compact replication is not implemented")
		quit(1)
		return
	for cargo in game.cargos.values():
		var ghost = load("res://scripts/cargo.gd").new()
		ghost.kind = cargo.kind
		game.add_child(ghost)
		cargo.rules.score = 2
		cargo.rules.holder_id = 77
		cargo.body.position = Vector3(3, 2, 1)
		ghost.apply_wire(cargo.wire_snapshot())
		if ghost.snapshot() != cargo.snapshot():
			# Guest interpolation deliberately postpones the physical pose.
			ghost.body.position = ghost.target_position
			if ghost.snapshot() != cargo.snapshot():
				push_error("Wire state differs for " + cargo.kind)
				quit(1)
				return
		ghost.queue_free()
	game._add_worker(2)
	game.last_blast = {"source": 2, "event": 99, "workers": [1,2], "cargos": [1,3]}
	var size := var_to_bytes(game._snapshot()).size()
	if size > 1280:
		push_error("Four-cargo snapshot exceeds budget: %d" % size)
		quit(1)
		return
	game.leave_game()
	game.queue_free()
	await process_frame
	print("WIRE PASS: all four types round trip; snapshot %d bytes" % size)
	quit(0)

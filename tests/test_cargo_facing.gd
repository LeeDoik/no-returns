extends SceneTree

var failures := 0

func _initialize() -> void:
	call_deferred("run")

func check(ok: bool, message: String) -> void:
	if not ok:
		failures += 1
		push_error("FACING: " + message)

func forward_of(node: Node3D) -> Vector3:
	return -node.global_basis.z.normalized()

func run() -> void:
	var game = load("res://scenes/main.tscn").instantiate()
	root.add_child(game)
	game.practice_game()
	game.set_physics_process(false)
	var worker = game.workers[1]
	worker.position = Vector3(-6, 0.05, 6)
	for cargo in game.cargos.values():
		cargo.body.freeze = true
		cargo.body.position = worker.position + Vector3(0, 1.05, -0.98)
		worker.heading = 0
		check(cargo.pickup(worker), cargo.kind + " pickup")
		var ghost = load("res://scripts/cargo.gd").new()
		ghost.kind = cargo.kind
		game.add_child(ghost)
		for yaw in [PI / 2, -0.7, PI - 0.01, -PI + 0.01]:
			worker.heading = yaw
			cargo.move_held(worker)
			cargo._present()
			var expected: Vector3 = worker.forward()
			check(forward_of(cargo.visual).is_equal_approx(expected), cargo.kind + " held body follows worker")
			check(cargo.throw_velocity(worker).slide(Vector3.UP).normalized().is_equal_approx(expected), "throw follows visible direction")
			if cargo.cues:
				check(forward_of(cargo.cues.face).is_equal_approx(expected), "nose rotates once with body")
			ghost.apply_wire(cargo.wire_snapshot())
			check(forward_of(ghost.visual).is_equal_approx(expected), cargo.kind + " guest sees same facing")
			if ghost.cues:
				check(forward_of(ghost.cues.face).is_equal_approx(expected), "guest nose matches blast direction")
		cargo.release(worker, true)
		cargo._present()
		check(forward_of(cargo.visual).is_equal_approx(worker.forward()), "release retains facing")
		cargo.reset_crate()
		cargo._present()
		check(forward_of(cargo.visual).is_equal_approx(Vector3.FORWARD), "recovery resets facing")
		ghost.queue_free()
	game.leave_game()
	game.queue_free()
	await process_frame
	print("CARGO FACING %s" % ("PASS" if failures == 0 else "FAIL"))
	quit(1 if failures else 0)

extends SceneTree
var failures := 0
var game
func check(ok: bool, message: String) -> void:
	if not ok: failures += 1; push_error(message)
func _initialize() -> void: run.call_deferred()
func run() -> void:
	game = load("res://scenes/main.tscn").instantiate(); root.add_child(game); game.practice_game(); game.set_physics_process(false)
	var source = game.cargos[2]; source.body.freeze = true
	await physics_frame; await physics_frame
	var stacks = game.reactions.get_node("Paperwork").get_children()
	check(stacks.size() == 13,"All 13 existing document piles/forms converted")
	for paper in stacks:
		game.reactions.reset(); var found := false
		for i in range(16):
			var toward := Vector3.FORWARD.rotated(Vector3.UP,i*TAU/16)
			source.body.global_position = paper.global_position-toward*2; source.body.global_position.y = 1.05; source.facing = i*TAU/16
			if game._blast_reaches(source,paper.global_position+Vector3.UP*0.45): found = true; break
		check(found,"Held sneezer can reach existing "+paper.name)
		if not found: continue
		game._apply_sneeze(source); game.reactions.step(0.2,{},{}); paper.set_process(false)
		check(paper.event_id == 1 and paper.pieces.size() == 18,"Sneeze throws a finite paper bundle at "+paper.name)
		var start: Vector3 = paper.pieces[0].global_position
		for i in range(10): paper.burst_age += 0.016; paper._paper_step(0.016)
		var motion: Vector3 = paper.pieces[0].global_position-start
		check(motion.dot(paper.burst_direction)>0.3,"Paper bundle initially flies along sneeze direction")
	game.reactions.reset()
	var p = stacks[0]; source.body.global_position = p.global_position+Vector3(0,0,2); source.facing = PI
	game._apply_sneeze(source); check(p.phase == 0,"Backward sneeze leaves paper stacked")
	source.body.global_position = p.global_position+Vector3(0,0,6); source.facing = 0
	game._apply_sneeze(source); check(p.phase == 0,"Out-of-range sneeze leaves paper stacked")
	# A temporary wall tests the same occlusion path as all existing props.
	var wall := StaticBody3D.new(); var collision := CollisionShape3D.new(); var shape := BoxShape3D.new()
	shape.size = Vector3(2,3,0.15); collision.shape = shape; wall.add_child(collision); root.add_child(wall)
	wall.global_position = p.global_position+Vector3(0,0,1)
	source.body.global_position = p.global_position+Vector3(0,0,2)
	await physics_frame; await physics_frame
	game._apply_sneeze(source); check(p.phase == 0,"Wall blocks sneeze-driven paper flight")
	# Actual visual sheets hit the wall and settle instead of passing through it.
	p.arm(Vector3.BACK); game.reactions.step(0.2,{},{}); p.set_process(false)
	for i in range(200): p.burst_age += 0.016; p._paper_step(0.016)
	for sheet in p.pieces: check(sheet.global_position.z < wall.global_position.z,"Flying sheet stays in front of wall")
	wall.free(); game.reactions.step(9,{},{}); p._present()
	check(p.phase == 0 and p.pieces[0].position == p.origins[0],"Documents restore after cooldown")
	game.leave_game(); game.queue_free(); await process_frame
	print("PAPERWORK %s"%("PASS" if failures == 0 else "FAIL")); quit(0 if failures == 0 else 1)

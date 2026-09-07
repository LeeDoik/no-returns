extends SceneTree

const Layout = preload("res://scripts/depot_layout.gd")

var game: Node
var failures := 0

func _initialize() -> void:
	call_deferred("_run")

func check(ok: bool, message: String) -> void:
	if not ok:
		failures += 1
		push_error("SNEEZE SCENE FAIL: " + message)

func frames(count: int) -> void:
	for index in range(count):
		await physics_frame
		await process_frame

func _run() -> void:
	game = load("res://scenes/main.tscn").instantiate()
	root.add_child(game)
	if game.get("cargos") == null:
		push_error("Sneezer feature missing: scene has no multiple-cargo registry")
		quit(1)
		return
	game.practice_game()
	game.set_physics_process(false)
	game._add_worker(2)
	var ordinary = game.cargos[1]
	var source = game.cargos[2]
	check(game.cargos.size() == 4 and source.kind == "sneezer", "stable cargo identities with Hopper added")
	# A moving worker must not lose a valid boundary pickup before it replicates.
	game.cargos[3].body.freeze = true
	game.cargos[3].body.position = Vector3(8, 1, 8)
	source.body.position = Vector3(7, 1, 7)
	ordinary.body.freeze = true
	ordinary.body.position = Vector3(2.32, 0.55, 4)
	game.workers[1].position = Vector3(0, 0.05, 4)
	await frames(3)
	game._receive_action(1, "interact")
	game.workers[1].position.x -= 0.15
	ordinary.step(1.0 / 60.0, game.workers)
	check(ordinary.rules.holder_id == 1, "moving boundary pickup stays held on first tick")
	game.start_shift()
	game.workers[1].position = Vector3(-2, 0.05, 5)
	game.workers[2].position = Vector3(0, 0.05, 3)
	await frames(3)
	game._receive_action(1, "interact")
	game._receive_action(2, "interact")
	check(source.rules.holder_id == 1 and ordinary.rules.holder_id == 2, "nearest selection and simultaneous different holders")
	game._receive_action(1, "interact")
	check(source.rules.holder_id == 0 and ordinary.rules.holder_id == 2, "interact puts down current slot without taking another")
	game._receive_action(1, "interact")
	# Arrange a carrier behind the source and another worker with cargo in front.
	game.workers[1].position = Vector3(-1, 0.05, 6)
	game.workers[1].heading = 0
	game.workers[2].position = Vector3(-1, 0.05, 2.6)
	game.workers[2].heading = 0
	source.body.freeze = true
	source.body.position = game.workers[1].hand_position()
	ordinary.body.freeze = true
	ordinary.body.position = game.workers[2].hand_position()
	await frames(3)
	source.sneeze.phase = "windup"
	source.sneeze.remaining = 0.1
	var sequence: int = source.sneeze.event_id
	source.step(0.05, game.workers)
	check(source.sneeze.event_id == sequence and game.workers[2].push_velocity == Vector3.ZERO, "windup does not hit early")
	source.step(0.06, game.workers)
	check(source.sneeze.event_id == sequence + 1, "one burst event")
	check(source.rules.holder_id == 1 and game.workers[1].push_velocity == Vector3.ZERO, "source and its carrier immune")
	check(ordinary.rules.holder_id == 0 and ordinary.body.linear_velocity.z < -7.5, "other held cargo dropped and pushed")
	check(game.workers[2].push_velocity.z < -6.5, "worker receives persistent directional push")
	var before := Vector3(game.workers[2].position)
	game.workers[2].simulate(Vector2.ZERO, 0, false, 1.0 / 60.0)
	await frames(1)
	check(game.workers[2].position.z < before.z - 0.04, "normal input does not erase push")
	var push_before: Vector3 = game.workers[2].push_velocity
	source.step(0.1, game.workers)
	check(source.sneeze.event_id == sequence + 1 and game.workers[2].push_velocity == push_before, "burst is not applied repeatedly")
	# Repeated network snapshots must not replay the puff/audio.
	var ghost = load("res://scripts/cargo.gd").new()
	ghost.kind = "sneezer"
	ghost.home = Vector3(7, 1, 7)
	game.add_child(ghost)
	var state: Dictionary = source.snapshot()
	ghost.apply_snapshot(state)
	var cue_count: int = ghost.cues.bursts_played
	ghost.apply_snapshot(state)
	check(cue_count == 1 and ghost.cues.bursts_played == 1, "replicated burst presents once")
	# Reused guest nodes must accept lower event IDs from a different host.
	state["epoch"] = "previous-host"
	state.event = 10
	ghost.apply_snapshot(state)
	check(ghost.cues.bursts_played == 2, "later event in previous session presents")
	state.active = false
	ghost.apply_snapshot(state)
	state["epoch"] = "new-host"
	state.active = true
	state.event = 1
	ghost.apply_snapshot(state)
	ghost.apply_snapshot(state)
	check(ghost.cues.bursts_played == 3, "new host lower event presents exactly once after lobby")
	ghost.queue_free()
	# Walls shield both cargo and workers.
	source.rules.reset_crate()
	ordinary.rules.reset_crate()
	source.body.position = Vector3(0, 0.55, 0.7)
	source.facing = 0
	ordinary.body.freeze = true
	ordinary.body.position = Vector3(0, 0.55, -2.5)
	ordinary.body.linear_velocity = Vector3.ZERO
	game.workers[2].position = Vector3(0, 0.05, -2.5)
	game.workers[2].reset_motion()
	await frames(3)
	game._apply_sneeze(source)
	check(ordinary.body.linear_velocity == Vector3.ZERO and game.workers[2].push_velocity == Vector3.ZERO, "solid divider shields cargo and worker")
	# Delivering Sneezer must add to the shared quota without resetting the other slot.
	ordinary.rules.holder_id = 2
	var score_before: int = game.score
	source.body.position = game.depot.bay(1) - Vector3(0, 0.1, 0)
	source.step(1.0 / 60.0, game.workers)
	check(source.recovery_left > 0 and game.score == score_before + 1 and ordinary.rules.holder_id == 2, "Sneezer delivery increments shared quota and preserves other ownership")
	source.step(0.1, game.workers)
	check(game.score == score_before + 1, "Sneezer delivery does not count again while recovering")
	# Return/finish/restart cannot leak a previously armed sneeze.
	source.sneeze.phase = "windup"
	source.sneeze.remaining = 0.01
	sequence = source.sneeze.event_id
	source.recover("wrong_bay")
	await frames(3)
	var probe := PhysicsShapeQueryParameters3D.new()
	probe.shape = source.shape
	probe.transform = Transform3D(Basis.IDENTITY, source.body.position)
	probe.collision_mask = 4
	probe.exclude = [ordinary.body.get_rid()]
	check(game.get_world_3d().direct_space_state.intersect_shape(probe).is_empty(), "hidden recovering cargo does not block physics queries")
	ghost = load("res://scripts/cargo.gd").new()
	ghost.kind = "sneezer"
	game.add_child(ghost)
	ghost.apply_snapshot(source.snapshot())
	check(ghost.body.collision_layer == 0 and ghost.body.collision_mask == 0, "guest recovery disables collision too")
	source.step(1.3, game.workers)
	source.step(0.02, game.workers)
	check(source.sneeze.event_id == sequence and source.sneeze.phase == "calm", "recovery cancels armed sneeze")
	ghost.apply_snapshot(source.snapshot())
	check(source.body.collision_layer == 4 and source.body.collision_mask == 7 and ghost.body.collision_layer == 4 and ghost.body.collision_mask == 7, "host and guest collision restored after recovery")
	ghost.queue_free()
	await frames(3)
	probe.transform.origin = source.body.position
	check(not game.get_world_3d().direct_space_state.intersect_shape(probe).is_empty(), "restored cargo participates in physics queries")
	source.sneeze.phase = "windup"
	source.sneeze.remaining = 0.01
	game._finish("won")
	check(source.sneeze.phase == "calm", "shift end cancels armed sneeze")
	game.start_shift()
	check(source.sneeze.remaining == 6.0 and game.workers[2].push_velocity == Vector3.ZERO, "restart resets clocks and pushes")
	game.leave_game()
	game.queue_free()
	await process_frame
	print("SNEEZE SCENE %s: %d failures" % ["PASS" if failures == 0 else "FAIL", failures])
	quit(0 if failures == 0 else 1)

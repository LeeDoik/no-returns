extends SceneTree
var failures := 0
func _initialize() -> void: call_deferred("run")
func check(ok: bool, message: String) -> void:
	if not ok:
		failures += 1
		push_error(message)
func run() -> void:
	var game = load("res://scenes/main.tscn").instantiate()
	root.add_child(game)
	if not game.depot.has_node("Gameplay/RouteChallenges"):
		push_error("Expanded route challenges are missing")
		game.queue_free()
		quit(1)
		return
	game.practice_game(true)
	game.set_physics_process(false)
	var route = game.depot.get_node("Gameplay/RouteChallenges")
	var worker = game.workers[1]
	var cargo = game.cargos[1]
	check(game.depot.get_node("Geometry/Floor").dimensions == Vector3(48,0.5,60), "floor grows to 48 by 60 metres")
	check(game.depot.bay(1).is_equal_approx(Vector3(-15,0.65,-43)), "dispatch moves to far courtyard")
	check(not game.depot.outside(Vector3(23,0.55,-46)), "expanded corner is playable")
	check(game.depot.outside(Vector3(26,0.55,-46)), "outside expanded map recovers cargo")
	await physics_frame
	var from := Vector3(0,1,-26)
	var to := Vector3(0,1,-30)
	var query := PhysicsRayQueryParameters3D.create(from,to,1)
	check(not game.get_world_3d().direct_space_state.intersect_ray(query).is_empty(), "closed door blocks the central path")
	worker.position = route.get_node("Gate/PlateFront").global_position
	route.step(0.1, game.workers, game.cargos)
	check(route.gate_open, "worker pressure opens shortcut")
	await physics_frame
	check(game.get_world_3d().direct_space_state.intersect_ray(query).is_empty(), "open door clears physical passage")
	worker.position = Vector3(0,0,6)
	route.step(5.5, game.workers, game.cargos)
	check(route.gate_open, "solo crossing grace lasts six seconds")
	worker.position = route.get_node("Gate/Clearance").global_position
	route.step(1, game.workers, game.cargos)
	check(route.gate_open, "occupied doorway does not close on worker")
	worker.position = Vector3(0,0,6)
	route.step(1, game.workers, game.cargos)
	check(not route.gate_open, "door closes after clear grace expires")
	cargo.body.freeze = true
	cargo.body.linear_velocity = Vector3.ZERO
	cargo.body.position = route.get_node("Gate/PlateFront").global_position + Vector3.UP*0.55
	route.step(0.1, game.workers, game.cargos)
	check(route.gate_open, "free cargo weights pressure plate")
	cargo.rules.holder_id = 1
	route.reset()
	route.step(0.1, game.workers, game.cargos)
	check(not route.gate_open, "held cargo alone cannot weight plate")
	cargo.rules.holder_id = 0
	var wind = route.get_node("AirMail")
	worker.position = wind.global_position
	cargo.body.position = wind.global_position + Vector3.UP*0.55
	route.reset()
	route.step(5.1, game.workers, game.cargos)
	check(route.wind_phase() == 1 and route.worker_drift(worker) == Vector3.ZERO, "warning precedes wind with no force")
	route.step(1.5, game.workers, game.cargos)
	check(route.wind_phase() == 2 and route.worker_drift(worker).z < 0, "active wind pushes workers toward dispatch")
	check(cargo.body.linear_velocity.z < 0, "active wind moves free cargo")
	cargo.rules.holder_id = 1
	cargo.body.linear_velocity = Vector3.ZERO
	route.step(0.1, game.workers, game.cargos)
	check(cargo.body.linear_velocity == Vector3.ZERO, "wind preserves held cargo ownership")
	cargo.rules.holder_id = 0
	cargo.creature_held = true
	route.step(0.1, game.workers, game.cargos)
	check(cargo.body.linear_velocity == Vector3.ZERO, "wind preserves creature cargo ownership")
	cargo.creature_held = false
	wind.rotation.y = PI/2
	check(route.worker_drift(worker).x < 0, "wind force follows editor rotation")
	var state = route.snapshot()
	route.reset()
	route.apply_snapshot(state)
	check(route.wind_phase() == 2, "replicated phase restores wind presentation")
	route.reset()
	check(not route.gate_open and route.wind_phase() == 0, "new contract resets mechanisms")
	game.queue_free()
	await process_frame
	print("ROUTE CHALLENGES %s" % ("PASS" if failures == 0 else "FAIL"))
	quit(1 if failures else 0)

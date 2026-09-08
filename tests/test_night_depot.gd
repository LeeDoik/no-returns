extends SceneTree

const Layout = preload("res://scripts/depot_layout.gd")
var game
var failures := 0
func _initialize() -> void:
	call_deferred("run")
func check(ok: bool, message: String) -> void:
	if not ok:
		failures += 1
		push_error("NIGHT DEPOT: " + message)
func run() -> void:
	game = load("res://scenes/main.tscn").instantiate()
	root.add_child(game)
	game.practice_game()
	game.set_physics_process(false)
	await physics_frame
	await physics_frame
	var space: PhysicsDirectSpaceState3D = game.get_world_3d().direct_space_state
	for at in [Vector3(23, 2, -46), Vector3(-23, 2, -46), Vector3(23, 2, 10)]:
		check(not space.intersect_ray(PhysicsRayQueryParameters3D.create(at, at + Vector3.DOWN * 4, 1)).is_empty(), "expanded floor exists at " + str(at))
	for cargo in game.cargos.values():
		for id in [1,2]:
			var target: Vector3 = game.depot.bay(id)
			var start: Vector3 = cargo.home + Vector3.UP * 0.5
			check(start.distance_to(target) > 20, "intake is separated from dispatch")
			check(not space.intersect_ray(PhysicsRayQueryParameters3D.create(start, target, 1)).is_empty(), "central partition blocks direct dispatch sightline")
	# Current throw preview uses the same volume/gravity as the release simulation.
	# All intake-to-bay aims must make their first contact well before dispatch.
	for source in game.cargos.values():
		for id in [1, 2]:
			var start: Vector3 = source.home + Vector3.UP * 0.5
			var direction: Vector3 = game.depot.bay(id) - start
			direction.y = 0
			var hit: Dictionary = source.predict_contact(start, direction.normalized() * source.THROW_SPEED + Vector3.UP * source.THROW_LIFT)
			check(not hit.is_empty(), "intake throw finds a solid contact")
			if not hit.is_empty():
				check(Vector2(hit.center.x, hit.center.z).distance_to(Vector2(game.depot.bay(id).x, game.depot.bay(id).z)) > 3, "intake throw cannot reach dispatch in one flight")
	# The belt must transport packages across the sorting wall's depth, not into it.
	var belt_start := Layout.BELT_CENTER + Vector3(0, 0.45, 2.5)
	var belt_end := Layout.BELT_CENTER + Vector3(0, 0.45, -2.5)
	check(belt_start.z > -10 and belt_end.z < -10, "belt crosses the sorting boundary")
	var belt_query: PhysicsShapeQueryParameters3D = game.cargos[1].query(belt_start, belt_end - belt_start)
	belt_query.collision_mask = 1
	check(space.cast_motion(belt_query)[0] >= 1.0, "belt transfers cargo past sorting wall without collision")
	# Sweep both a worker capsule and held box along permanent side routes.
	var capsule := CapsuleShape3D.new()
	capsule.radius = 0.32
	capsule.height = 1.65
	for route_name in ["LeftLoop","RightLoop"]:
		var route: Array[Vector3] = []
		for point in game.depot.get_node("DesignRoutes/"+route_name).get_children():
			route.append(point.global_position)
		for index in range(route.size() - 1):
			for held in [false,true]:
				var request := PhysicsShapeQueryParameters3D.new()
				request.shape = game.cargos[1].shape if held else capsule
				request.collision_mask = 1
				request.margin = 0.001
				request.transform.origin = route[index] + Vector3.UP * (1.05 if held else 0.875)
				request.motion = route[index+1] - route[index]
				check(space.cast_motion(request)[0] >= 1.0, "side route clear for worker and held cargo")
	var cargo = game.cargos[1]
	cargo.body.freeze = true
	cargo.body.position = Vector3(14, 0.55, -24)
	cargo.step(0.01, game.workers)
	check(cargo.recovery_left == 0, "expanded playable corner does not recover")
	cargo.body.position = Vector3(26, 0.55, -24)
	cargo.step(0.01, game.workers)
	check(cargo.recovery_left > 0, "outside expanded boundary recovers")
	for id in [1,2]:
		cargo.reset_crate()
		cargo.rules.destination = id
		cargo.body.freeze = true
		cargo.body.position = game.depot.bay(id)
		var previous: int = cargo.rules.score
		cargo.step(0.01, game.workers)
		check(cargo.rules.score == previous + 1, "new dispatch scores at bay " + str(id))
	game.leave_game()
	game.queue_free()
	await process_frame
	print("NIGHT DEPOT %s" % ("PASS" if failures == 0 else "FAIL"))
	quit(1 if failures else 0)

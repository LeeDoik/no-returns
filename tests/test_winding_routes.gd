extends SceneTree
var failures := 0
func _initialize() -> void: call_deferred("run")
func check(ok: bool, message: String) -> void:
	if not ok:
		failures += 1
		push_error(message)
func run() -> void:
	var game = load("res://scenes/main.tscn").instantiate()
	root.add_child(game); game.practice_game(); game.set_physics_process(false)
	await physics_frame; await physics_frame
	var space: PhysicsDirectSpaceState3D = game.get_world_3d().direct_space_state
	for side in [-1,1]:
		var query := PhysicsRayQueryParameters3D.create(Vector3(side*21,1,6),Vector3(side*21,1,-43),1)
		check(not space.intersect_ray(query).is_empty(), "old straight outer lane must bend")
	var paths = game.depot.get_node_or_null("DesignRoutes")
	check(paths != null, "editable winding-route references exist")
	if paths:
		for path in paths.get_children():
			var points: Array = path.get_children()
			check(points.size() >= 2, "route has multiple points")
			for i in range(points.size()-1):
				var shape := BoxShape3D.new()
				# Covers a worker and held box with 3m turning clearance.
				shape.size = Vector3(3.0,1.8,3.0)
				var query := PhysicsShapeQueryParameters3D.new()
				query.shape = shape; query.collision_mask = 1; query.margin = 0.005
				query.transform.origin = points[i].global_position+Vector3.UP*0.95
				query.motion = points[i+1].global_position-points[i].global_position
				check(space.cast_motion(query)[0] >= 1.0, "%s segment %d preserves carrying clearance" % [path.name,i])
	var air = game.routes.get_node("AirMail")
	game.routes.step(6.6,{}, {})
	game.workers[1].position = air.global_position
	check(game.routes.worker_drift(game.workers[1]).x < -5, "crosswise airflow points toward central link")
	game.queue_free(); await process_frame
	print("WINDING ROUTES %s" % ("PASS" if failures == 0 else "FAIL"))
	quit(1 if failures else 0)

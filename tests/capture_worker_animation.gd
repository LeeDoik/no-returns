extends SceneTree

func _initialize() -> void: call_deferred("run")

func run() -> void:
	var stage := Node3D.new(); root.add_child(stage)
	var environment := WorldEnvironment.new()
	var settings := Environment.new()
	settings.background_mode = Environment.BG_COLOR
	settings.background_color = Color("233039")
	settings.ambient_light_source = Environment.AMBIENT_SOURCE_COLOR
	settings.ambient_light_color = Color.WHITE
	settings.ambient_light_energy = 0.7
	environment.environment = settings; stage.add_child(environment)
	var sun := DirectionalLight3D.new(); sun.rotation_degrees = Vector3(-40,-30,0)
	sun.light_energy = 1.5; sun.shadow_enabled = true; stage.add_child(sun)
	var floor_mesh := MeshInstance3D.new(); var plane := PlaneMesh.new()
	plane.size = Vector2(12,12); floor_mesh.mesh = plane; stage.add_child(floor_mesh)
	var worker = load("res://scripts/worker.gd").new(); stage.add_child(worker)
	worker.set_process(false); worker.nameplate.hide()
	var box := MeshInstance3D.new(); var cube := BoxMesh.new(); cube.size = Vector3.ONE * 0.8
	box.mesh = cube; box.position = worker.hand_position(); stage.add_child(box)
	var material := StandardMaterial3D.new(); material.albedo_color = Color("b18a51")
	box.material_override = material
	var camera := Camera3D.new(); stage.add_child(camera)
	camera.position = Vector3(3,2.3,-4); camera.look_at(Vector3(0,0.9,-0.2)); camera.current = true
	for state in ["idle", "walk", "run", "carry_idle", "carry_walk", "throw", "air_rise"]:
		box.visible = state.begins_with("carry")
		worker.art_player.play(state); worker.art_player.advance(0.45)
		await process_frame
		await RenderingServer.frame_post_draw
		var result := root.get_texture().get_image().save_png("res://artifacts/worker-%s.png" % state)
		if result != OK: quit(1); return
	stage.queue_free(); await process_frame
	var game = load("res://scenes/main.tscn").instantiate(); root.add_child(game)
	game.test_mode = true; game.practice_game(); game.set_physics_process(false)
	worker = game.workers[1]
	worker.position = Vector3(-3,0.05,6)
	game.cargos[1].body.position = worker.position + Vector3(0,0.45,-0.7)
	game.cargos[1].pickup(worker); worker.held = true
	for i in range(20): await process_frame
	await RenderingServer.frame_post_draw
	root.get_texture().get_image().save_png("res://artifacts/worker-game-carry.png")
	game.leave_game(); game.queue_free(); await process_frame
	print("WORKER CAPTURE PASS"); quit()

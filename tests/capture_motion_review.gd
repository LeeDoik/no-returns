extends SceneTree

var worker
var box: Node3D
var camera: Camera3D
var label: Label
var frame := 0
var report := {}
var names := ["forward", "strafe", "backward", "carry", "turn", "throw", "jump", "stop"]
var output := "before"

func _initialize() -> void: call_deferred("setup")

func setup() -> void:
	for arg in OS.get_cmdline_user_args():
		if arg.begins_with("--label="): output = arg.trim_prefix("--label=")
	var stage := Node3D.new(); root.add_child(stage)
	var env := WorldEnvironment.new(); var settings := Environment.new()
	settings.background_mode = Environment.BG_COLOR; settings.background_color = Color("243039")
	settings.ambient_light_source = Environment.AMBIENT_SOURCE_COLOR; settings.ambient_light_color = Color.WHITE; settings.ambient_light_energy = 0.45
	env.environment = settings; stage.add_child(env)
	var light := DirectionalLight3D.new(); light.rotation_degrees = Vector3(-45,-30,0); light.shadow_enabled = true; light.light_energy = 0.9; stage.add_child(light)
	for x in range(-10,11):
		for z in range(-25,5):
			var tile := MeshInstance3D.new(); var plane := PlaneMesh.new(); plane.size = Vector2.ONE
			tile.mesh = plane; tile.position = Vector3(x,0,z)
			var mat := StandardMaterial3D.new(); mat.albedo_color = Color("58636a") if (x+z)%2 == 0 else Color("626e76")
			tile.material_override = mat; stage.add_child(tile)
	worker = load("res://scripts/worker.gd").new(); stage.add_child(worker); worker.set_process(false); worker.nameplate.hide()
	box = load("res://scripts/postal_art.gd").model("standard"); stage.add_child(box)
	camera = Camera3D.new(); stage.add_child(camera); camera.current = true; camera.fov = 42
	label = Label.new(); label.position = Vector2(24,20); label.add_theme_font_size_override("font_size",24); root.add_child(label)
	await process_frame
	# Measure complete source clips, not just one flattering still.
	for clip in ["idle","walk","run","carry_walk","carry_idle","throw"]:
		var low := Vector3(INF,INF,INF); var high := -low
		var length: float = worker.art_player.get_animation(clip).length
		worker.art_player.play(clip)
		for index in range(121):
			worker.art_player.seek(length*index/121.0,true)
			worker.art_skeleton.force_update_all_bone_transforms()
			var point: Vector3 = worker.art_skeleton.global_transform * worker.art_skeleton.get_bone_global_pose(worker.art_skeleton.find_bone("Hip")).origin
			low = low.min(point); high = high.max(point)
		report[clip] = {"length":length,"hip_min":[low.x,low.y,low.z],"hip_max":[high.x,high.y,high.z]}
	var file := FileAccess.open("res://artifacts/motion-%s.json" % output,FileAccess.WRITE); file.store_string(JSON.stringify(report,"  "))
	worker.animation_motion.current = ""
	run_review()

func run_review() -> void:
	for index in range(960):
		frame = index
		var segment := index/120
		var phase := float(index%120)/60.0
		var direction := Vector3.FORWARD
		worker.held = segment == 3 or segment == 4
		worker.heading = 0
		worker.velocity = Vector3.ZERO
		if segment == 1: direction = Vector3.RIGHT
		if segment == 2: direction = Vector3.BACK
		if segment < 4: worker.velocity = direction * ((2.4 if worker.held else 4.5) if output=="before" else (1.6 if worker.held else 3.2))
		if segment == 4: worker.heading = sin(phase*PI)*1.2; worker.velocity = worker.forward()*(2.4 if output=="before" else 1.6)
		if segment == 5 and index%120 == 0: worker.play_throw()
		if segment == 6:
			worker.velocity.y = 4.8-9.81*phase if phase < 0.98 else 0
			worker.position.y = maxf(0,4.8*phase-4.905*phase*phase)
		else: worker.position.y = 0
		if output == "directions":
			worker.held = true; worker.heading = 0
			var angle := segment*PI/4
			worker.velocity = Vector3(sin(angle),0,-cos(angle)) * (1.6 if segment == 0 else 1.2)
		worker.position += worker.velocity.slide(Vector3.UP)/60.0
		worker._process(1.0/60.0)
		box.visible = worker.held; box.position = worker.hand_position(); box.rotation.y = worker.heading
		camera.position = worker.position + Vector3(3,2.0,4.2); camera.look_at(worker.position + Vector3(0,0.9,0))
		label.text = "%s | %s | %.2fs | %.1f m/s" % [output,str(segment*45)+" deg" if output=="directions" else names[segment],phase,worker.velocity.length()]
		await process_frame; await RenderingServer.frame_post_draw
		if index%30 == 0:
			root.get_texture().get_image().save_png("res://artifacts/motion-%s-%02d-%02d.png" % [output,segment,index%120])
	print("MOTION REVIEW CAPTURE PASS"); quit()

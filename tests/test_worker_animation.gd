extends SceneTree
var failures := 0
func check(ok: bool, message: String) -> void:
	if not ok: failures += 1; push_error(message)
func _initialize() -> void: call_deferred("run")
func run() -> void:
	var worker = load("res://scripts/worker.gd").new()
	root.add_child(worker)
	worker.set_process(false)
	await process_frame
	for clip in ["idle", "walk", "run", "carry_idle", "carry_walk", "carry_run", "air_rise", "air_fall", "throw", "hit_to_body_01"]:
		check(worker.art_player.has_animation(clip), "missing worker motion: " + clip)
	if failures:
		worker.free(); print("WORKER ANIMATION FAIL"); quit(1); return
	worker.velocity = Vector3.ZERO
	worker._process(0.3)
	check(worker.art_player.current_animation == "idle", "stationary worker plays idle")
	worker.velocity = Vector3(0,0,-4.5)
	for i in range(30): worker._process(1.0/60.0)
	check(worker.art_player.current_animation == "run", "full speed plays run")
	worker.held = true
	for i in range(30): worker._process(1.0/60.0)
	check(worker.art_player.current_animation == "carry_run", "moving carrier uses authored carry pose")
	worker.velocity = Vector3.ZERO
	for i in range(30): worker._process(1.0/60.0)
	check(worker.art_player.current_animation == "carry_idle", "stopped carrier retains hand support")
	worker.held = false
	worker.play_throw()
	worker._process(0.05)
	check(worker.art_player.current_animation == "throw", "throw release plays follow-through")
	for i in range(90): worker._process(1.0/60.0)
	check(worker.art_player.current_animation == "idle", "throw returns to idle")
	worker.velocity.y = 4.0; worker._process(0.1)
	check(worker.art_player.current_animation == "air_rise", "upward motion uses airborne pose")
	worker.velocity.y = -4.0; worker._process(0.1)
	check(worker.art_player.current_animation == "air_fall", "falling uses airborne pose")
	worker.velocity = Vector3.ZERO; worker._process(0.3)
	check(worker.visual.position.length() < 0.01, "animation never displaces gameplay root")
	worker.receive_throw_sequence(7)
	check(worker.animation_motion.throw_left == 0, "joining does not replay historic throws")
	worker.receive_throw_sequence(8)
	check(worker.animation_motion.throw_left > 0, "new remote throw starts follow-through")
	worker._process(0.2)
	var remaining: float = worker.animation_motion.throw_left
	worker.receive_throw_sequence(8)
	worker.receive_throw_sequence(6)
	check(is_equal_approx(worker.animation_motion.throw_left, remaining), "duplicate and stale snapshots do not restart throw")
	for i in range(worker.art_skeleton.get_bone_count()):
		check(worker.art_skeleton.get_bone_global_pose(i).is_finite(), "finite bone pose")
	worker.free()
	print("WORKER ANIMATION %s" % ("PASS" if failures == 0 else "FAIL")); quit(1 if failures else 0)

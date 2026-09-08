extends SceneTree
var failures := 0
func check(ok: bool, message: String) -> void:
	if not ok: failures += 1; push_error(message)
func _initialize() -> void: call_deferred("run")
func run() -> void:
	var worker = load("res://scripts/worker.gd").new(); root.add_child(worker); worker.set_process(false)
	for clip in ["idle","walk","run","carry_walk","carry_idle","throw"]:
		worker.art_player.play(clip)
		for i in range(32):
			worker.art_player.seek(worker.art_player.get_animation(clip).length*i/32,true)
			worker.art_skeleton.force_update_all_bone_transforms()
			var hip: Vector3 = worker.art_skeleton.global_transform * worker.art_skeleton.get_bone_global_pose(worker.art_skeleton.find_bone("Hip")).origin
			check(Vector2(hip.x,hip.z).length()<0.15,clip+" hips stay inside collision capsule")
	for clip in ["carry_back","carry_left","carry_right","carry_forward_left","carry_forward_right","carry_back_left","carry_back_right","land"]:
		check(worker.art_player.has_animation(clip),"dedicated motion exists: "+clip)
	worker.heading = 0; worker.velocity = Vector3.RIGHT*3.2
	for i in range(60): worker._process(1.0/60)
	check((-worker.visual.basis.z).dot(Vector3.RIGHT)>.95,"empty worker faces travel rather than moonwalking")
	worker.free(); print("MOTION QUALITY %s" % ("PASS" if failures==0 else "FAIL"));quit(0 if failures==0 else 1)

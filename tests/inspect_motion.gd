extends SceneTree
func _initialize() -> void: call_deferred("run")
func run() -> void:
	var worker = load("res://scripts/worker.gd").new(); root.add_child(worker); worker.set_process(false)
	var result := {}
	for clip in ["idle","walk","run","carry_walk"]:
		var frames := []; var duration: float = worker.art_player.get_animation(clip).length
		worker.art_player.play(clip)
		for index in range(121):
			worker.art_player.seek(duration*index/120.0,true); worker.art_skeleton.force_update_all_bone_transforms()
			var sample := {}
			for name in ["Hip","Spine01","L_Upperarm","L_Forearm","L_Hand","L_Foot","R_Foot","L_ToeBase"]:
				var p: Vector3 = worker.art_skeleton.global_transform * worker.art_skeleton.get_bone_global_pose(worker.art_skeleton.find_bone(name)).origin
				sample[name] = [p.x,p.y,p.z]
			frames.append(sample)
		result[clip] = {"duration":duration,"frames":frames}
	var file := FileAccess.open("res://artifacts/motion-samples.json",FileAccess.WRITE);file.store_string(JSON.stringify(result))
	worker.free(); quit()

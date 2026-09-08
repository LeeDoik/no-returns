extends SceneTree
var failures := 0
func _initialize() -> void: call_deferred("run")
func check(ok: bool, message: String) -> void:
	if not ok: failures += 1; push_error(message)
func run() -> void:
	var game = load("res://scenes/main.tscn").instantiate(); root.add_child(game)
	game.test_mode = true; game.practice_game(); game.set_physics_process(false)
	var worker = game.workers[1]; var cargo = game.cargos[1]
	worker.position = Vector3(-6,0.05,6); worker.velocity = Vector3.ZERO
	cargo.body.freeze = true; cargo.body.position = worker.hand_position()
	check(worker.visual.get_node_or_null("CrewBadge") == null, "back board removed")
	check(cargo.pickup(worker), "pickup accepted")
	check(worker.held, "pickup immediately sets carrying state")
	worker.held = true; worker.velocity = Vector3(0,0,-4.5)
	for i in range(60): worker._process(1.0/60)
	check(worker.art_player.current_animation == "carry_walk", "carrier never uses running torso")
	for i in range(150):
		worker._process(1.0/60)
		for bone_name in ["Hip", "Spine01", "Head", "L_Calf", "R_Calf"]:
			var bone: int = worker.art_skeleton.find_bone(bone_name)
			if bone < 0: continue
			var world: Vector3 = worker.art_skeleton.global_transform * worker.art_skeleton.get_bone_global_pose(bone).origin
			var point: Vector3 = cargo.body.global_transform.affine_inverse() * world
			check(not (absf(point.x)<0.45 and absf(point.y)<0.45 and absf(point.z)<0.45), "carry torso/leg stays outside box: " + bone_name)
	worker.velocity = Vector3(1,0,-2)
	cargo.release(worker,false)
	check(cargo.body.linear_velocity.is_equal_approx(worker.velocity), "moving drop inherits momentum")
	check(not worker.held, "release immediately clears carrying")
	check(not cargo.body.lock_rotation, "free cargo can tumble")
	var gravity: float = ProjectSettings.get_setting("physics/3d/default_gravity")
	check(absf(gravity*cargo.body.gravity_scale-9.81) < 0.01, "parcel earth gravity")
	check(cargo.body.physics_material_override.bounce <= 0.1, "cardboard does not bounce like rubber")
	cargo.body.position = Vector3(-6,6,6); cargo.body.rotation = Vector3.ZERO
	for i in range(2): await physics_frame; await process_frame
	cargo.body.linear_velocity = Vector3.ZERO
	var start_tick := Engine.get_physics_frames()
	cargo.body.apply_impulse(Vector3(3,0,0),Vector3(0,0.3,0))
	for i in range(12): await physics_frame; await process_frame
	check(cargo.body.rotation.length()>0.05, "off-center impulse causes physical rotation")
	var elapsed := float(Engine.get_physics_frames()-start_tick)/Engine.physics_ticks_per_second
	check(absf(cargo.body.linear_velocity.y+9.81*elapsed)<0.4, "free fall accelerates at earth gravity")
	var before: Quaternion = cargo.body.quaternion
	var decoded: Vector3 = cargo.unpack_rotation(cargo.pack_rotation(cargo.body.rotation))
	check(before.angle_to(Basis.from_euler(decoded).get_rotation_quaternion()) < 0.012, "compressed rotation remains accurate")
	game.leave_game(); game.queue_free(); await process_frame
	print("CARRY PHYSICS %s" % ("PASS" if failures == 0 else "FAIL")); quit(0 if failures==0 else 1)

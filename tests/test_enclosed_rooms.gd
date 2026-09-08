extends "res://tests/test_winding_routes.gd"
func run() -> void:
	var game = load("res://scenes/main.tscn").instantiate()
	root.add_child(game); game.practice_game(); game.set_physics_process(false)
	await physics_frame; await physics_frame
	var space: PhysicsDirectSpaceState3D = game.get_world_3d().direct_space_state
	for pair in [[Vector3(0,2,0),Vector3(-15,2,0)], [Vector3(-21,2,-21),Vector3(-12,2,-21)], [Vector3(18,2,-24),Vector3(18,2,-19)], [Vector3(-18,2,-32),Vector3(-18,2,-43)], [Vector3(16,2,-32),Vector3(16,2,-43)]]:
		check(not space.intersect_ray(PhysicsRayQueryParameters3D.create(pair[0],pair[1],1)).is_empty(),"room wall interrupts sightline: "+str(pair[0]))
	var box := BoxShape3D.new(); box.size = Vector3.ONE*0.8
	var query := PhysicsShapeQueryParameters3D.new(); query.shape = box; query.collision_mask = 1; query.margin = 0.005
	query.transform.origin = Vector3(-6.5,2.375,-2); query.motion = Vector3(-4,0,0)
	check(space.cast_motion(query)[0] >= 1.0,"parcel passes through hatch")
	var capsule := CapsuleShape3D.new(); capsule.radius = 0.32; capsule.height = 1.65
	query.shape = capsule
	check(space.cast_motion(query)[0] < 1.0,"worker cannot fit through parcel hatch even at opening center")
	check(not space.intersect_ray(PhysicsRayQueryParameters3D.create(Vector3(0,2,-6),Vector3(0,9,-6),1)).is_empty(),"packing roof has collision")
	# Real third-person spring arm must shorten before its camera enters a wall.
	var worker = game.workers[1]
	worker.position = Vector3(6.5,0.05,0); worker.look_yaw = PI/2; worker.look_pitch = -0.22; worker.update_look()
	for i in range(8): await physics_frame
	var arm: SpringArm3D = worker.rig.get_child(0)
	check(arm.get_hit_length() < arm.spring_length,"camera retracts inside intake room")
	check(worker.camera.global_position.x < 8.1,"camera stays on interior side of wall")
	# Exercise a real release and catch across the wall, not only a static shape.
	game._add_worker(22)
	for item in game.cargos.values(): item.cancel()
	var cargo = game.cargos[1]; cargo.active = true; cargo.reset_crate()
	worker.position = Vector3(-4,0.05,-2); worker.heading = PI/2
	var receiver = game.workers[22]; receiver.position = Vector3(-12,0.05,-2); receiver.heading = -PI/2
	cargo.body.freeze = true; cargo.body.position = worker.hand_position()
	await physics_frame; await process_frame
	check(cargo.pickup(worker),"sender picks up parcel inside room")
	cargo.release(worker,true)
	var crossed := false
	for frame in range(90):
		await physics_frame; await process_frame; cargo.step(1.0/60.0,game.workers)
		if cargo.body.position.x < -11.8 and cargo.body.position.distance_to(receiver.position) < 2.3:
			crossed = true
			print("HATCH FLIGHT: ",cargo.body.position," flight=",cargo.flight_left," origin=",cargo.throw_origin)
			check(cargo.pickup(receiver) and cargo.relay_ready,"teammate catches airborne parcel beyond hatch")
			break
	check(crossed,"real throw traverses parcel hatch")
	game.queue_free(); await process_frame
	print("ENCLOSED ROOMS %s" % ("PASS" if failures == 0 else "FAIL")); quit(1 if failures else 0)

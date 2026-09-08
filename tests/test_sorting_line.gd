extends SceneTree
var game
var failures := 0
const DT := 1.0/60.0
func check(ok: bool, message: String) -> void:
	if not ok: failures += 1; push_error(message)
func _initialize() -> void: run.call_deferred()
func park(ids: Array) -> void:
	for id in game.cargos:
		var cargo = game.cargos[id]; cargo.reset_shift(); cargo.active = id in ids
		cargo.body.visible = id in ids; cargo.body.freeze = not (id in ids)
		cargo.body.position = Vector3(0,0.43,4+id*1.5)
	game.packrat.stop(game.cargos)
func advance(frames: int, ids: Array) -> void:
	for i in range(frames):
		await physics_frame
		for id in ids: game.cargos[id].step(DT,game.workers)
		game.conveyor.step(DT,game.cargos)
		if 3 in ids: game.cargos[3].cling.step(DT,game.workers,game.cargos)
func run() -> void:
	game = load("res://scenes/main.tscn").instantiate(); root.add_child(game); game.practice_game()
	game.set_physics_process(false)
	check(game.conveyor.global_position.is_equal_approx(Vector3(0,0,-11)),"Belt connects intake to sorting")
	check(game.depot.rat_start().distance_to(game.depot.rat_nest()) < 1,"Rat emerges at visible nest")
	park([1]); var standard = game.cargos[1]
	standard.body.position = Vector3(0,0.43,-7.8)
	await advance(260,[1])
	print("BELT STOP ",standard.body.position," velocity ",standard.body.linear_velocity)
	check(standard.body.position.z < -13 and standard.body.position.z > -14.5,"Normal cargo crosses wall on belt, then waits at sorting lip")
	park([3,4]); var hopper = game.cargos[4]; var clinger = game.cargos[3]
	hopper.body.position = Vector3(-0.42,0.43,-13.2); hopper.facing = 0
	clinger.body.position = Vector3(0.42,0.43,-13.2); clinger.cling.cooldown = 0
	await physics_frame
	clinger.cling.step(DT,game.workers,game.cargos)
	check(clinger.cling.target_kind == "cargo" and clinger.cling.target_id == 4,"Clinger can pair with Hopper at sorting lip")
	hopper.hopper.phase = "windup"; hopper.hopper.remaining = 0.01
	await advance(75,[3,4])
	check(hopper.body.position.z < -15 and clinger.body.position.z < -15,"Hopper physically carries bonded cargo over lip")
	park([1,2,3]); var sneezer = game.cargos[2]
	standard.body.position = Vector3(0,0.43,-12.2)
	sneezer.body.position = Vector3(0,0.43,-10.8); sneezer.facing = 0
	await physics_frame
	game._apply_sneeze(sneezer)
	await advance(65,[1])
	print("BLAST STOP ",standard.body.position," velocity ",standard.body.linear_velocity)
	check(standard.body.position.z < -14.7,"Sneeze physically launches normal cargo over sorting lip")
	park([1,2,3]); standard.body.position = Vector3(0,0.43,-16.4)
	clinger.body.position = Vector3(0.84,0.43,-16.4); clinger.cling.cooldown = 0
	game.packrat.position = Vector3(1.2,0.05,-16.4); game.packrat.start(1)
	game.packrat.position = Vector3(1.2,0.05,-16.4)
	await physics_frame; clinger.cling.step(DT,{},game.cargos)
	game.packrat.known_cargos = game.cargos
	check(not game.packrat._eligible(standard),"Rat cannot split and steal a bonded pair")
	clinger.cling.detach(); clinger.active = false
	check(game.packrat._eligible(standard),"Detached cargo becomes stealable again")
	game.packrat._claim(standard)
	sneezer.body.position = Vector3(1.2,0.43,-14.5); sneezer.facing = 0
	game._apply_sneeze(sneezer)
	check(game.packrat.phase == "flee" and not standard.creature_held,"Sneeze rescues stolen cargo and scares rat")
	game.packrat.start(1); game.packrat.position = Vector3(1.2,0.05,-16.4)
	sneezer.facing = PI; game._apply_sneeze(sneezer)
	check(game.packrat.phase != "flee","Backward sneeze cannot scare rat")
	sneezer.body.position = Vector3(1.2,0.43,-10); sneezer.facing = 0; game._apply_sneeze(sneezer)
	check(game.packrat.phase != "flee","Out-of-range sneeze cannot scare rat")
	sneezer.body.position = Vector3(1.2,0.43,-14.5)
	var wall := StaticBody3D.new(); var collision := CollisionShape3D.new(); var shape := BoxShape3D.new()
	shape.size = Vector3(3,3,0.2); collision.shape = shape; wall.add_child(collision); root.add_child(wall); wall.position = Vector3(1.2,1.5,-15.5)
	await physics_frame; await physics_frame
	game._apply_sneeze(sneezer); check(game.packrat.phase != "flee","Wall blocks sneeze scare")
	wall.free()
	park([1]); standard.body.position = Vector3(0,0.43,-16.4)
	game.packrat.reset(game.cargos); game.packrat.start(1)
	var stolen := false
	for i in range(650):
		await physics_frame
		game.packrat.step(DT,game.workers,game.cargos)
		stolen = stolen or standard.creature_held
		if game.packrat.protected_left(1)>0: break
	print("RAT RETURN ",game.packrat.phase," ",game.packrat.position," CARGO ",standard.body.position)
	check(stolen and game.packrat.protected_left(1)>0,"Rat walks from visible nest to belt output and returns stolen cargo without jamming")
	game.leave_game(); game.queue_free(); await process_frame
	print("SORTING LINE %s" % ("PASS" if failures == 0 else "FAIL")); quit(0 if failures == 0 else 1)

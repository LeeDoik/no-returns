extends SceneTree
var game
var failures := 0
func check(ok: bool, message: String) -> void:
	if not ok: failures += 1; push_error(message)
func _initialize() -> void: run.call_deferred()
func run() -> void:
	game = load("res://scenes/main.tscn").instantiate(); root.add_child(game); game.practice_game()
	game.set_physics_process(false)
	var system = game.get("reactions")
	check(system != null,"Map has a reactive prop system")
	if system == null: game.free(); quit(1); return
	check(system.props.size() == 37,"24 floor props and 13 existing document stacks populate the map")
	var spring = system.props[0]
	for p in system.props:
		if p.kind == 1: spring = p; break
	var worker = game.workers[1]
	worker.position = spring.global_position; worker.reset_motion()
	system.step(0.01,game.workers,{})
	system.step(0.2,game.workers,{})
	check(worker.velocity.y >= 8,"Stepping on spring launches worker")
	var event: int = spring.event_id
	system.step(12,game.workers,{}); system.step(0.3,game.workers,{})
	check(spring.event_id == event,"Standing occupant cannot loop spring after cooldown")
	worker.position += Vector3(4,0,0); system.step(0.1,game.workers,{})
	worker.position = spring.global_position; system.step(0.1,game.workers,{}); system.step(0.2,game.workers,{})
	check(spring.event_id == event+1,"Leaving and returning rearms spring")
	system.reset(); worker.position = Vector3(0,0.05,5)
	var cargo = game.cargos[1]; cargo.reset_shift(); cargo.body.position = spring.global_position+Vector3.UP*0.43
	cargo.body.freeze = false; await physics_frame
	system.step(0.01,{}, {1:cargo}); system.step(0.2,{}, {1:cargo})
	check(cargo.body.linear_velocity.y >= 8,"Spring launches loose cargo")
	for i in range(15): await physics_frame
	check(cargo.body.global_position.y > 1,"Launched cargo physically rises")
	var replica = load("res://scenes/maps/shipping_shrine.tscn").instantiate(); root.add_child(replica)
	var remote = replica.get_node("Gameplay/Reactions")
	remote.apply_snapshot(system.snapshot())
	check(remote.snapshot() == system.snapshot(),"Guest presents identical phase and event state")
	remote.reset(); remote.apply_wire_snapshot(system.wire_snapshot())
	check(remote.snapshot() == system.snapshot(),"Compressed impact state round trips exactly")
	var wire_before = remote.snapshot()
	remote.apply_wire_snapshot(PackedByteArray())
	remote.apply_wire_snapshot([])
	check(remote.snapshot() == wire_before,"Invalid wire types leave state unchanged")
	remote.reset(); check(remote.props[0].phase == 0 and remote.props[0].event_id == 0,"Restart resets reactions")
	replica.free()
	system.reset(); cargo.rules.holder_id = 1
	system.step(0.01,{}, {1:cargo}); system.step(0.3,{}, {1:cargo})
	check(spring.event_id == 0,"Held cargo cannot trigger independently")
	cargo.rules.holder_id = 0; cargo.body.position = Vector3(0,0.43,5)
	var cushion = system.get_node("Intake/PackingCushion")
	var tower = system.get_node("Intake/CartonTower")
	system.reset(); worker.position = tower.global_position; worker.velocity = Vector3(2,0,0)
	system.step(0.2,game.workers,{})
	check(tower.event_id == 1 and cushion.phase == 1,"Impact topples tower and primes neighboring cushion")
	system.step(0.2,{},{}); check(cushion.event_id == 1,"Chain fires on a later step")
	system.reset(); worker.position = cushion.global_position+Vector3.UP*4
	system.step(0.3,game.workers,{})
	check(cushion.phase == 0,"Overhead worker cannot trigger floor prop")
	var wall := StaticBody3D.new(); var shape := CollisionShape3D.new(); var box := BoxShape3D.new()
	box.size = Vector3(0.15,3,3); shape.shape = box; wall.add_child(shape); root.add_child(wall)
	wall.global_position = cushion.global_position+Vector3(0.45,1.5,0)
	await physics_frame; await physics_frame
	worker.position = cushion.global_position+Vector3(0.8,0,0)
	system.step(0.3,game.workers,{})
	check(cushion.phase == 0,"Wall blocks activation")
	wall.free(); system.reset()
	var before: PackedFloat32Array = system.snapshot(); var invalid := before.duplicate(); invalid[1] = NAN
	system.apply_snapshot(invalid); check(system.snapshot() == before,"Malformed state rejected without partial mutation")
	game.leave_game(); game.queue_free(); await process_frame
	print("REACTIVE PROPS %s"%("PASS" if failures == 0 else "FAIL")); quit(0 if failures == 0 else 1)

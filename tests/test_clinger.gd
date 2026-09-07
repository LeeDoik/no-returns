extends SceneTree

const Cargo = preload("res://scripts/cargo.gd")
const Worker = preload("res://scripts/worker.gd")
var failures: Array[String] = []
var world: Node3D
var source
var target
var worker
var bond

func _init() -> void:
	call_deferred("run")

func check(value: bool, message: String) -> void:
	if not value:
		failures.append(message)

func run() -> void:
	if not ResourceLoader.exists("res://scripts/clinger.gd"):
		push_error("FAIL: attachment controller missing")
		quit(1)
		return
	world = Node3D.new()
	root.add_child(world)
	source = Cargo.new()
	target = Cargo.new()
	worker = Worker.new()
	world.add_child(source)
	world.add_child(target)
	world.add_child(worker)
	source.reset_shift()
	target.reset_shift()
	worker.peer_id = 7
	target.cargo_id = 2
	bond = load("res://scripts/clinger.gd").new()
	source.add_child(bond)
	bond.setup(source)
	source.body.freeze = true
	target.body.freeze = true
	await physics_frame
	place()
	check(bond.cooldown == 1.0, "reset has one second grace")
	bond.step(1.0, {7: worker}, {})
	bond.step(0.01, {7: worker}, {})
	check(bond.target_kind == "worker" and bond.target_id == 7, "nearby worker attaches")
	check(source.body.freeze and source.body.collision_layer == 0, "attached physics is frozen")
	worker.position.x += 0.3
	bond.step(0.1, {7: worker}, {})
	check(is_equal_approx(source.body.position.x, 0.3), "worker follows with offset")
	bond.step(5.0, {7: worker}, {})
	check(bond.target_kind == "" and bond.cooldown == 2.0, "five seconds releases with cooldown")
	bond.step(0.1, {7: worker}, {})
	check(bond.target_kind == "", "cannot immediately reattach")
	place()
	bond.cooldown = 0
	target.body.position = Vector3(0.5, 1, 0)
	bond.step(0.01, {7: worker}, {2: target})
	check(bond.target_kind == "cargo", "nearest eligible target wins across registries")
	bond.detach()
	for state in ["clinger", "hidden", "recovering", "inactive"]:
		place()
		bond.cooldown = 0
		target.kind = "clinger" if state == "clinger" else "standard"
		target.body.visible = state != "hidden"
		target.recovery_left = 1 if state == "recovering" else 0
		target.active = state != "inactive"
		bond.step(0.01, {}, {2: target})
		check(bond.target_kind == "", "ineligible target excluded: " + state)
	target.kind = "standard"
	target.reset_shift()
	place()
	bond.cooldown = 0
	source.rules.holder_id = 7
	bond.step(0.01, {7: worker}, {})
	check(bond.target_kind == "", "held source cannot attach")
	source.rules.holder_id = 0
	bond.step(0.01, {}, {2: target})
	check(bond.target_kind == "cargo", "nearby cargo attaches")
	target.body.position.z += 0.4
	bond.step(0.01, {}, {2: target})
	check(is_equal_approx(source.body.position.z, 0.4), "cargo follows with offset")
	var saved: Array = bond.snapshot()
	bond.detach()
	bond.apply_snapshot(saved)
	check(bond.snapshot() == saved, "snapshot preserves bond state")
	bond.step(0.01, {}, {})
	check(bond.target_kind == "", "missing target detaches")
	place()
	bond.cooldown = 0
	bond.step(0.01, {}, {2: target})
	target.rules.try_dispatch(1)
	target.recover("shipped")
	bond.step(0.01, {}, {2: target})
	bond.step(0.01, {}, {2: target})
	check(source.rules.score == 1 and source.recovery_left > 0, "target dispatch scores once and recovers source")
	source.reset_crate()
	target.reset_crate()
	place()
	bond.reset()
	bond.cooldown = 0
	bond.step(0.01, {}, {2: target})
	target.recover("wrong_bay")
	bond.step(0.01, {}, {2: target})
	check(bond.target_kind == "" and source.rules.score == 1, "wrong bay detaches without score")
	target.reset_crate()
	place()
	bond.cooldown = 0
	bond.step(0.01, {7: worker}, {})
	worker.position.x += 3
	bond.step(0.01, {7: worker}, {})
	check(bond.target_kind == "", "respawn-sized movement detaches")
	place()
	var wall := StaticBody3D.new()
	wall.collision_layer = 1
	var collision := CollisionShape3D.new()
	var box := BoxShape3D.new()
	box.size = Vector3(0.1, 3, 3)
	collision.shape = box
	wall.add_child(collision)
	wall.position = Vector3(0.45, 1, 0)
	world.add_child(wall)
	await physics_frame
	place()
	worker.position = Vector3(0.9, 0, 0)
	bond.cooldown = 0
	bond.step(0.01, {7: worker}, {})
	check(bond.target_kind == "", "solid wall prevents acquisition")
	worker.position = Vector3(0, 0, 0.6)
	bond.step(0.01, {7: worker}, {})
	check(bond.target_kind == "worker", "clear line permits acquisition")
	worker.position.x += 0.8
	bond.step(0.01, {7: worker}, {})
	check(bond.target_kind == "" and source.body.position.x < 0.45, "box sweep prevents wall crossing")
	wall.queue_free()
	var floor_body := StaticBody3D.new()
	var floor_collision := CollisionShape3D.new()
	var floor_box := BoxShape3D.new()
	floor_box.size = Vector3(20, 0.2, 20)
	floor_collision.shape = floor_box
	floor_body.position.y = -0.1
	floor_body.add_child(floor_collision)
	world.add_child(floor_body)
	await physics_frame
	place()
	source.body.position.y = 0.4
	bond.cooldown = 0
	bond.step(0.01, {7: worker}, {})
	worker.position.z += 0.1
	bond.step(0.01, {7: worker}, {})
	check(bond.target_kind == "worker", "resting floor contact permits horizontal follow")
	for message in failures:
		push_error(message)
	print("PASS: clinger actual-scene behavior" if failures.is_empty() else "FAIL: %d clinger assertions" % failures.size())
	quit(0 if failures.is_empty() else 1)

func place() -> void:
	source.body.freeze = true
	target.body.freeze = true
	source.body.position = Vector3(0, 1, 0)
	target.body.position = Vector3(0.8, 1, 0)
	worker.position = Vector3(0, 0, 0.6)

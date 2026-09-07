extends SceneTree

const Cargo = preload("res://scripts/cargo.gd")

var failures: Array[String] = []
var world: Node3D
var cargo: Node3D

func _initialize() -> void:
	call_deferred("run")

func check(condition: bool, message: String) -> void:
	if not condition:
		failures.append(message)

func box(parent: Node3D, size: Vector3, at: Vector3) -> void:
	var solid := StaticBody3D.new()
	var collision := CollisionShape3D.new()
	var shape := BoxShape3D.new()
	shape.size = size
	collision.shape = shape
	solid.add_child(collision)
	solid.position = at
	parent.add_child(solid)

func settle(count: int = 3) -> void:
	for index in range(count):
		await physics_frame

func run() -> void:
	world = Node3D.new()
	root.add_child(world)
	box(world, Vector3(12, 0.2, 12), Vector3(0, -0.1, 0))
	box(world, Vector3(3, 0.65, 0.25), Vector3(0, 0.325, -0.65))
	cargo = Cargo.new()
	cargo.kind = "hopper"
	cargo.home = Vector3(0, 0.55, 1)
	world.add_child(cargo)
	cargo.reset_shift()
	await settle(30)
	check(cargo.hopper != null and cargo.hopper.phase == "rest", "Hopper starts in rest")

	cargo.step(4.0, {})
	check(cargo.hopper.phase == "windup" and is_equal_approx(cargo.hopper.remaining, 1.0), "four grounded seconds begin windup")
	cargo.rules.holder_id = 9
	cargo.step(0.6, {})
	check(is_equal_approx(cargo.hopper.remaining, 1.0), "holding pauses windup")
	cargo.rules.holder_id = 0
	cargo.step(1.0, {})
	check(cargo.hopper.phase == "airborne", "released windup launches once")
	check(is_equal_approx(cargo.body.linear_velocity.y, 6.5), "hop applies lift")
	check(is_equal_approx(cargo.body.linear_velocity.z, -2.8), "hop follows cargo facing")
	var first_velocity: Vector3 = cargo.body.linear_velocity
	cargo.step(2.0, {})
	check(cargo.body.linear_velocity == first_velocity, "airborne clock does not repeat hop")

	var crossed := false
	for index in range(180):
		await physics_frame
		cargo.step(1.0 / 60.0, {})
		if cargo.body.position.z < -0.9:
			crossed = true
		if cargo.hopper.phase == "rest" and cargo.hopper.remaining > 3.9:
			break
	check(crossed, "hop crosses the low divider")
	check(cargo.hopper.phase == "rest" and cargo.hopper.remaining > 3.9, "landing starts a fresh rest")

	cargo.hopper.phase = "windup"
	cargo.hopper.remaining = 0.7
	cargo.rules.holder_id = 9
	cargo.step(0.5, {})
	cargo.body.position = Vector3(0, 1.2, 1)
	cargo.rules.holder_id = 0
	cargo.body.freeze = false
	for index in range(90):
		await physics_frame
		if cargo.hopper._grounded() and cargo.body.linear_velocity.y <= 0.2:
			break
		cargo.step(1.0 / 60.0, {})
	cargo.step(0.01, {})
	check(cargo.hopper.phase == "windup" and cargo.hopper.remaining > 0.65, "ordinary held drop pauses then resumes the same windup")

	cargo.hopper.phase = "windup"
	cargo.hopper.remaining = 0.2
	cargo.recover("recovered")
	check(cargo.hopper.phase == "rest" and is_equal_approx(cargo.hopper.remaining, 4.0), "recovery cancels pending hop")
	cargo.hopper.phase = "windup"
	cargo.hopper.remaining = 0.4
	cargo.reset_crate()
	check(cargo.hopper.phase == "rest" and is_equal_approx(cargo.hopper.remaining, 4.0), "reset cancels pending hop")

	cargo.hopper.phase = "windup"
	cargo.hopper.remaining = 0.35
	var guest := Cargo.new()
	guest.kind = "hopper"
	world.add_child(guest)
	guest.apply_wire(cargo.wire_snapshot())
	check(guest.hopper.snapshot() == cargo.hopper.snapshot(), "guest applies compact Hopper state")
	var guest_before: Vector3 = guest.body.linear_velocity
	guest.interpolate(0.05)
	check(guest.body.linear_velocity == guest_before, "guest presentation does not apply hop physics")

	world.queue_free()
	await process_frame
	for failure in failures:
		push_error(failure)
	print("HOPPER %s" % ("PASS" if failures.is_empty() else "FAIL"))
	quit(0 if failures.is_empty() else 1)

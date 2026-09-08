extends SceneTree

var failures := 0
var game

func _initialize() -> void:
	call_deferred("run")
func check(ok: bool, message: String) -> void:
	if not ok:
		failures += 1
		push_error("COLLISION: " + message)
func frames(count: int) -> void:
	for i in range(count):
		await physics_frame
func run() -> void:
	game = load("res://scenes/main.tscn").instantiate()
	root.add_child(game)
	game.practice_game()
	game.set_physics_process(false)
	var worker = game.workers[1]
	var cargo = game.cargos[1]
	worker.position = Vector3(-6, 0.05, 6)
	cargo.body.freeze = true
	cargo.body.position = Vector3(-6, 0.5, 4.5)
	await frames(3)
	for i in range(60):
		worker.simulate(Vector2(0, -1), 0, false, 1.0 / 60)
		await frames(1)
	check(worker.position.z > 5.1, "worker cannot walk through a stationary crate")
	worker.position = Vector3(-6, 0.05, 6)
	worker.reset_motion()
	cargo.body.position = Vector3(-6, 1.05, 5)
	check(cargo.pickup(worker), "pickup near worker")
	await frames(2)
	for i in range(40):
		worker.simulate(Vector2(0, -1), 0, false, 1.0 / 60)
		cargo.move_held(worker)
		await frames(1)
	# Carrying now walks at 2.4 m/s; cover over one metre without self-blocking.
	check(worker.position.z < 5.0 and cargo.rules.holder_id == 1, "own held crate does not block carrying: worker=%s cargo=%s holder=%d" % [worker.position, cargo.body.position, cargo.rules.holder_id])
	game._add_worker(22)
	var other = game.workers[22]
	other.position = cargo.body.position + Vector3(0, -1, -1.5)
	other.reset_motion()
	await frames(3)
	for i in range(45):
		other.simulate(Vector2(0, 1), 0, false, 1.0 / 60)
		await frames(1)
	check(other.position.z < cargo.body.position.z - 0.6, "coworker collides with held crate")
	cargo.release(worker, false)
	check(not cargo.body.get_collision_exceptions().has(worker), "put-down restores owner collision")
	# Real rigid body flying toward a stationary worker must contact the capsule.
	other.position = Vector3(-8, 0.05, 8)
	worker.position = Vector3(-6, 0.05, 3)
	worker.reset_motion()
	cargo.body.position = Vector3(-6, 1, 6)
	cargo.body.gravity_scale = 0
	cargo.body.contact_monitor = true
	cargo.body.max_contacts_reported = 8
	var contacts: Array = []
	cargo.body.body_entered.connect(func(body): contacts.append(body))
	cargo.body.linear_velocity = Vector3(0, 0, -8)
	await frames(35)
	check(contacts.has(worker), "moving crate physically contacts worker")
	check(cargo.body.position.z > worker.position.z, "moving crate does not pass through worker")
	cargo.reset_crate()
	check(cargo.body.collision_mask & 2, "recovery keeps worker collisions")
	game.leave_game()
	game.queue_free()
	await process_frame
	print("WORKER CARGO COLLISION %s" % ("PASS" if failures == 0 else "FAIL"))
	quit(1 if failures else 0)

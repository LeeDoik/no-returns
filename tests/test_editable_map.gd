extends SceneTree
var failures := 0
func check(ok: bool, message: String) -> void:
	if not ok:
		failures += 1
		push_error(message)
func _initialize() -> void: call_deferred("run")
func run() -> void:
	var path := "res://scenes/maps/shipping_shrine.tscn"
	if not ResourceLoader.exists(path):
		push_error("Editable map scene does not exist")
		quit(1)
		return
	var map = load(path).instantiate()
	root.add_child(map)
	check(map.map_fingerprint().length() == 64, "saved map has SHA256 identity")
	var block = load("res://scenes/pieces/solid_block.tscn").instantiate()
	var other = load("res://scenes/pieces/solid_block.tscn").instantiate()
	root.add_child(block); root.add_child(other)
	block.dimensions = Vector3(4,3,1)
	for child in block.get_children():
		if child is CollisionShape3D: check(child.shape.size == block.dimensions, "block collider follows dimensions")
		if child is MeshInstance3D: check(child.mesh.size == block.dimensions, "block mesh follows dimensions")
	check(other.dimensions == Vector3(2,2,0.5), "editing one block preserves another")
	block.queue_free(); other.queue_free()
	var bay = map.get_node("Gameplay/DispatchA")
	var old: Vector3 = map.bay(1)
	bay.position += Vector3(0,0,4)
	bay.rotation.y = PI/2
	check(map.dock_at(map.bay(1)) == 1 and map.dock_at(old) == 0, "moving bay moves delivery volume")
	check(map.dock_at(map.bay(1)+Vector3(0,0,1.45)) == 1, "rotated bay uses local volume")
	var belt = map.get_node("Gameplay/Conveyor")
	belt.position += Vector3(-4,0,0)
	belt.rotation.y = PI/2
	var drift: Vector3 = belt.drift_at(belt.global_position)
	check(drift.is_equal_approx(Vector3(-2,0,0)), "moved rotated belt carries in its own direction")
	check(belt.drift_at(Vector3(10,0,-10)) == Vector3.ZERO, "old belt location stops carrying")
	map.get_node("Gameplay/PackratTerritory").position += Vector3(-1,0,2)
	check(map.rat_nest().is_equal_approx(Vector3(12,0,-3)), "nest follows territory transform")
	map.get_node("Gameplay/WorkerSpawns/Worker1").position += Vector3(1,0,0)
	check(map.worker_spawn(1).is_equal_approx(Vector3(0,0.05,3)), "worker spawn follows marker")
	var packed := PackedScene.new()
	check(packed.pack(map) == OK, "edited map packs")
	check(ResourceSaver.save(packed,"res://artifacts/edited-map-test.tscn") == OK, "edited map saves")
	var expected: Vector3 = map.bay(1)
	map.queue_free()
	await process_frame
	var restored = load("res://artifacts/edited-map-test.tscn").instantiate()
	root.add_child(restored)
	check(restored.bay(1).is_equal_approx(expected), "saved placement survives reload")
	check(restored.dock_at(expected) == 1, "restored delivery uses edited position")
	restored.queue_free()
	await process_frame
	var game = load("res://scenes/main.tscn").instantiate()
	game.get_node("Map/Gameplay/DispatchA").position.z += 4
	game.get_node("Map/Gameplay/CargoSpawns/Cargo1").position.x += 2
	game.get_node("Map/Gameplay/WorkerSpawns/Worker1").position.x += 2
	root.add_child(game)
	game.practice_game(true)
	game.set_physics_process(false)
	var cargo = game.cargos[1]
	check(cargo.home.is_equal_approx(game.depot.cargo_spawn(1)), "game cargo home uses edited marker")
	check(game.workers[1].spawn_position().is_equal_approx(game.depot.worker_spawn(1)), "game worker uses edited marker")
	cargo.rules.destination = 1
	cargo.body.freeze = true
	cargo.body.position = game.depot.bay(1)
	cargo.step(0.01, game.workers)
	check(game.contracts.earned == 10, "actual cargo earns delivery at edited bay")
	game.queue_free()
	await process_frame
	print("EDITABLE MAP %s" % ("PASS" if failures == 0 else "FAIL"))
	quit(1 if failures else 0)

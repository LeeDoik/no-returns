extends SceneTree

var game: Node
var role := ""
var stage := 0
var deadline := 0
var guest_id := 0
var seen_warning := false
var at := 0
var cargo_before := Vector3.ZERO
var host_before := Vector3.ZERO
var max_snapshot_bytes := 0

func _initialize() -> void:
	for arg in OS.get_cmdline_user_args():
		if arg.begins_with("--role="):
			role = arg.trim_prefix("--role=")
	call_deferred("_begin")

func _begin() -> void:
	game = load("res://scenes/main.tscn").instantiate()
	root.add_child(game)
	deadline = Time.get_ticks_msec() + 18000
	if role == "host":
		game.host_game(27944)
	else:
		game.join_game("127.0.0.1", 27944)

func _process(_delta: float) -> bool:
	if not game:
		return false
	if Time.get_ticks_msec() > deadline:
		fail("Timeout: %s stage %d" % [role, stage])
		return false
	if role == "host":
		host_step()
	else:
		guest_step()
	return false

func host_step() -> void:
	var source = game.cargos[2]
	var ordinary = game.cargos[1]
	# Leave room for RPC/ENet framing below its observed 1392-byte MTU.
	var snapshot_bytes := var_to_bytes(game._snapshot()).size()
	max_snapshot_bytes = maxi(max_snapshot_bytes, snapshot_bytes)
	if snapshot_bytes > 1280:
		fail("Snapshot exceeds payload budget: %d bytes" % snapshot_bytes)
		return
	if stage == 0 and game.workers.size() == 2:
		for id in game.workers:
			if id != 1:
				guest_id = id
		game.start_shift()
		game.workers[1].position = Vector3(-1, 0.05, 2.6)
		game.workers[guest_id].position = Vector3(-1, 0.05, 6)
		source.body.freeze = true
		source.body.position = Vector3(-1, 1.1, 5.02)
		source.sneeze.remaining = 30
		ordinary.body.freeze = true
		ordinary.body.position = Vector3(-1, 1.1, 1.62)
		at = Time.get_ticks_msec()
		stage = 1
	elif stage == 1 and Time.get_ticks_msec() - at > 100:
		game.session.send_action("interact")
		stage = 2
	elif stage == 2 and source.rules.holder_id == guest_id:
		if ordinary.rules.holder_id != 1:
			fail("Host was not holding the other cargo")
			return
		source.sneeze.phase = "windup"
		source.sneeze.remaining = 1.5
		print("TEST sneeze host: two distinct held cargos; windup started")
		stage = 3
	elif stage == 3 and source.sneeze.event_id == 1:
		if ordinary.rules.holder_id != 0 or game.workers[1].push_velocity.length() < 5:
			fail("Host did not receive a shove/drop from guest-held source")
			return
		if source.rules.holder_id != guest_id or game.workers[guest_id].push_velocity != Vector3.ZERO:
			fail("Sneezer carrier was affected")
			return
		print("TEST sneeze host: one event pushed worker and cargo; carrier immune")
		stage = 4
	elif stage == 4 and game.workers.size() == 1:
		if source.sneeze.event_id != 1 or source.rules.holder_id != 0:
			fail("Duplicate event or stale ownership on disconnect")
			return
		print("PASS network host: Sneezer authority, event, worker/cargo impact, source disconnect")
		print("TEST sneeze host: largest snapshot %d bytes (budget 1280)" % max_snapshot_bytes)
		game.leave_game()
		quit(0)

func guest_step() -> void:
	if game.phase != "playing" or not game.workers.has(game.local_id):
		return
	var source = game.cargos[2]
	var ordinary = game.cargos[1]
	if stage == 0:
		if game.cargos.size() != 4 or ordinary.kind != "standard" or source.kind != "sneezer":
			fail("Cargo identities differ")
			return
		game.session.send_input(Vector2.ZERO, 0, false)
		game.session.send_action("interact")
		if source.rules.holder_id == game.local_id:
			cargo_before = ordinary.body.position
			host_before = game.workers[1].position
			stage = 1
	elif stage == 1:
		if source.sneeze.phase == "windup":
			seen_warning = true
			if ordinary.rules.holder_id != 1:
				fail("Hit occurred before warning finished")
				return
		if source.sneeze.event_id == 1:
			if not seen_warning:
				fail("Guest missed the whole warning")
				return
			if game.last_blast.event != 1 or not game.last_blast.workers.has(1) or not game.last_blast.cargos.has(1):
				fail("Authoritative hit results differ")
				return
			stage = 2
			at = Time.get_ticks_msec()
	elif stage == 2 and Time.get_ticks_msec() - at > 700:
		if ordinary.rules.holder_id != 0 or source.rules.holder_id != game.local_id:
			fail("Ownership differs after event")
			return
		if ordinary.body.position.z > cargo_before.z - 0.15 or game.workers[1].position.z > host_before.z - 0.15:
			fail("Guest did not observe actual pushed movement")
			return
		if source.sneeze.event_id != 1 or source.cues.bursts_played != 1:
			fail("Repeated snapshots replayed burst")
			return
		print("PASS network guest: same warning/event, pushed movement, ownership, one visual burst")
		game.leave_game()
		quit(0)

func fail(message: String) -> void:
	push_error(message)
	quit(1)

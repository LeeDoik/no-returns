extends SceneTree

var game: Node
var role := ""
var stage := 0
var deadline := 0
var at := 0
var guest_id := 0
var initial := Vector3.ZERO
var max_bytes := 0

func _initialize() -> void:
	for arg in OS.get_cmdline_user_args():
		if arg.begins_with("--role="):
			role = arg.trim_prefix("--role=")
	call_deferred("begin")

func begin() -> void:
	game = load("res://scenes/main.tscn").instantiate()
	root.add_child(game)
	deadline = Time.get_ticks_msec() + 20000
	if role == "host":
		game.host_game(27945)
	else:
		game.join_game("127.0.0.1", 27945)

func _process(_delta: float) -> bool:
	if not game:
		return false
	if Time.get_ticks_msec() > deadline:
		fail("Timeout %s stage %d" % [role, stage])
		return false
	if role == "host":
		host_step()
	else:
		guest_step()
	return false

func host_step() -> void:
	var sticky = game.cargos[3]
	var sneezer = game.cargos[2]
	max_bytes = maxi(max_bytes, var_to_bytes(game._snapshot()).size())
	if max_bytes > 1280:
		fail("Clinger packet budget exceeded: %d" % max_bytes)
		return
	if stage == 0 and game.workers.size() == 2:
		for id in game.workers:
			if id != 1:
				guest_id = id
		game.start_shift()
		game.workers[1].position = Vector3(-5, 0.05, 5)
		game.workers[guest_id].position = Vector3(2, 0.05, 4)
		sneezer.body.freeze = true
		sneezer.body.position = Vector3(2, 0.55, 7)
		sneezer.sneeze.remaining = 30
		sticky.body.freeze = true
		sticky.body.position = Vector3(2.7, 0.8, 4)
		sticky.cling.cooldown = 0
		stage = 1
	elif stage == 1 and sticky.cling.target_kind == "worker":
		if sticky.cling.target_id != guest_id:
			fail("Wrong worker acquired")
			return
		sneezer.sneeze.phase = "windup"
		sneezer.sneeze.remaining = 1.5
		stage = 2
	elif stage == 2 and sneezer.sneeze.event_id == 1:
		if not sticky.cling.target_kind.is_empty() or sticky.cling.cooldown <= 0:
			fail("Sneeze did not release the bond")
			return
		stage = 3
	elif stage == 3 and game.workers.size() == 1:
		print("PASS network host: Clinger target, sneeze break, disconnect; max snapshot %d bytes" % max_bytes)
		game.leave_game()
		quit(0)

func guest_step() -> void:
	if game.phase != "playing" or not game.workers.has(game.local_id):
		return
	var sticky = game.cargos[3]
	var worker = game.workers[game.local_id]
	if stage == 0 and sticky.cling.target_kind == "worker":
		if sticky.cling.target_id != game.local_id or sticky.cling.remaining <= 0:
			fail("Replicated target/countdown differs")
			return
		initial = sticky.body.position
		game.session.send_input(Vector2(1,0), 0, false)
		at = Time.get_ticks_msec()
		stage = 1
	elif stage == 1 and Time.get_ticks_msec() - at > 230:
		game.session.send_input(Vector2.ZERO, 0, false)
		if sticky.body.position.x < initial.x + 0.10 or not is_equal_approx(worker.speed_scale, 0.7):
			fail("Guest did not observe carried attachment movement/slowdown")
			return
		stage = 2
	elif stage == 2 and game.cargos[2].sneeze.event_id == 1:
		if not sticky.cling.target_kind.is_empty() or sticky.cling.cooldown <= 0:
			fail("Guest did not observe sneeze release/cooldown")
			return
		if not game.last_blast.cargos.has(3):
			fail("Detached Clinger was not pushed")
			return
		at = Time.get_ticks_msec()
		stage = 3
	elif stage == 3 and Time.get_ticks_msec() - at > 200:
		if not is_equal_approx(worker.speed_scale, 1.0):
			fail("Worker slowdown persisted after release")
			return
		print("PASS network guest: same bond/countdown, followed movement, slowdown and sneeze release")
		game.leave_game()
		quit(0)

func fail(message: String) -> void:
	push_error(message)
	quit(1)

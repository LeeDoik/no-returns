extends SceneTree

const Layout = preload("res://scripts/depot_layout.gd")

var game: Node
var role := ""
var deadline := 0
var stage := 0
var initial_x := 0.0
var stage_time := 0
var guest_id := 0
var diagnostic_at := 0

func _initialize() -> void:
	for arg in OS.get_cmdline_user_args():
		if arg.begins_with("--role="):
			role = arg.trim_prefix("--role=")
	call_deferred("_begin")

func _begin() -> void:
	if not ResourceLoader.exists("res://scenes/main.tscn"):
		_fail("Missing playable scene: network feature has not been implemented")
		return
	game = load("res://scenes/main.tscn").instantiate()
	root.add_child(game)
	deadline = Time.get_ticks_msec() + 25000
	if role == "host":
		game.host_game(27943)
	else:
		game.join_game("127.0.0.1", 27943)
	print("TEST %s: started" % role)

func _process(_delta: float) -> bool:
	if not is_instance_valid(game):
		return false
	if Time.get_ticks_msec() > deadline:
		_fail("Timeout in %s stage %d" % [role, stage])
		return false
	if role == "host":
		_run_host()
	else:
		_run_guest()
	return false

func _run_host() -> void:
	if stage == 0 and game.workers.size() == 2:
		for id in game.workers:
			if id != 1:
				guest_id = id
		game.start_shift()
		# Keep the new third crate away from the original nearest-Standard fixture.
		game.cargos[3].body.position = Vector3(7, 0.55, 7)
		initial_x = game.workers[guest_id].position.x
		stage = 1
	elif stage == 1 and game.cargos[1].rules.holder_id == guest_id:
		if game.workers[guest_id].position.x < initial_x + 0.15:
			_fail("Guest movement did not reach host")
			return
		game.session.send_action("throw")
		if game.cargos[1].rules.holder_id != guest_id:
			_fail("Non-holder host was allowed to throw guest cargo")
			return
		print("TEST host: guest movement, pickup, foreign action rejected")
		stage = 2
	elif stage == 2 and game.cargos[1].rules.holder_id == 0:
		# Arrange a physical arrival; dispatch still runs through the real world tick.
		game.cargos[1].body.linear_velocity = Vector3.ZERO
		game.cargos[1].body.position = Layout.bay(1)
		stage = 3
	elif stage == 3 and game.score == 1:
		print("TEST host: delivered exactly once")
		stage = 4
	elif stage == 4 and game.cargos[1].rules.holder_id == guest_id:
		stage = 5
	elif stage == 5 and game.workers.size() == 1:
		if game.cargos[1].rules.holder_id != 0 or game.phase != "waiting":
			_fail("Disconnect did not release cargo and return host to waiting")
			return
		print("TEST host: disconnect released held cargo and returned to lobby")
		stage = 6
	elif stage == 6 and game.workers.size() == 2:
		stage_time = Time.get_ticks_msec()
		stage = 7
	elif stage == 7 and Time.get_ticks_msec() - stage_time > 600:
		game.leave_game()
		print("PASS network host: movement, ownership, delivery, held disconnect, rejoin, host leave")
		quit(0)

func _run_guest() -> void:
	if stage == 8:
		if game.phase == "menu" and game.notice_key == "host_left":
			print("PASS network guest: movement, pickup, throw, delivery, rejoin, host departure reason")
			quit(0)
		return
	if stage == 7:
		if Time.get_ticks_msec() - stage_time > 600:
			game.join_game("127.0.0.1", 27943)
			stage = 8
		return
	if game.session.mode != "guest" or not game.workers.has(game.local_id):
		return
	if stage == 0 and game.phase == "playing":
		initial_x = game.workers[game.local_id].position.x
		stage_time = Time.get_ticks_msec()
		stage = 1
	elif stage == 1:
		game.session.send_input(Vector2(1, 0), 0.0, false)
		if Time.get_ticks_msec() - stage_time > 200:
			game.session.send_input(Vector2.ZERO, 0.0, false)
			stage_time = Time.get_ticks_msec()
			stage = 2
	elif stage == 2:
		game.session.send_input(Vector2.ZERO, 0.0, false)
		game.session.send_action("interact")
		if game.cargos[1].rules.holder_id == game.local_id:
			if game.workers[game.local_id].position.x < initial_x + 0.15:
				_fail("Guest did not observe movement")
				return
			print("TEST guest: authoritative pickup observed")
			stage_time = Time.get_ticks_msec()
			stage = 3
	elif stage == 3 and Time.get_ticks_msec() - stage_time > 500:
		game.session.send_action("throw")
		stage = 4
	elif stage == 4 and game.cargos[1].rules.holder_id == 0:
		print("TEST guest: release observed")
		stage = 5
	elif stage == 5 and game.score == 1:
		if diagnostic_at == 0:
			print("TEST guest: observed delivery; returning to intake")
			diagnostic_at = 1
		if game.cargos[1].recovery_left <= 0:
			game.session.send_input(Vector2(-1, 0) if game.workers[game.local_id].position.x > 0.7 else Vector2.ZERO, 0.0, false)
			game.session.send_action("interact")
		if game.cargos[1].rules.holder_id == game.local_id:
			game.session.send_input(Vector2.ZERO, 0.0, false)
			stage_time = Time.get_ticks_msec()
			stage = 6
	elif stage == 6 and Time.get_ticks_msec() - stage_time > 500:
		game.leave_game()
		stage_time = Time.get_ticks_msec()
		stage = 7

func _fail(message: String) -> void:
	push_error(message)
	quit(1)

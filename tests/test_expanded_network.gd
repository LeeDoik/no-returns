extends SceneTree

const Layout = preload("res://scripts/depot_layout.gd")

class Checkpoint extends Node:
	var phase := ""
	var data: Dictionary = {}
	var acknowledged := false
	var guest_id := 0

	@rpc("any_peer", "call_remote", "reliable")
	func identify() -> void:
		if multiplayer.is_server():
			guest_id = multiplayer.get_remote_sender_id()

	@rpc("authority", "call_remote", "reliable")
	func announce(next_phase: String, details: Dictionary = {}) -> void:
		phase = next_phase
		data = details

	@rpc("any_peer", "call_remote", "reliable")
	func acknowledge(which: String) -> void:
		if multiplayer.is_server() and which == phase:
			acknowledged = true

var game: Node
var checkpoint: Checkpoint
var role := ""
var stage := 0
var sent := ""
var deadline := 0
var announced_at := 0

func _initialize() -> void:
	for arg in OS.get_cmdline_user_args():
		if arg.begins_with("--role="):
			role = arg.trim_prefix("--role=")
	call_deferred("begin")

func begin() -> void:
	checkpoint = Checkpoint.new()
	checkpoint.name = "ExpandedCheckpoint"
	root.add_child(checkpoint)
	game = load("res://scenes/main.tscn").instantiate()
	root.add_child(game)
	deadline = Time.get_ticks_msec() + 30000
	if role == "host":
		game.host_game(27949)
	else:
		game.join_game("127.0.0.1", 27949)

func _process(_delta: float) -> bool:
	if Time.get_ticks_msec() > deadline:
		fail("timeout at stage %d / %s" % [stage, checkpoint.phase])
		return false
	if role == "host":
		host_step()
	else:
		guest_step()
	return false

func announce(next_phase: String, data: Dictionary = {}) -> void:
	checkpoint.phase = next_phase
	checkpoint.data = data
	checkpoint.acknowledged = false
	checkpoint.announce.rpc(next_phase, data)
	announced_at = Time.get_ticks_msec()
	print("TEST expanded host: %s" % next_phase)

func ack() -> void:
	if sent == checkpoint.phase:
		return
	checkpoint.acknowledge.rpc_id(1, checkpoint.phase)
	sent = checkpoint.phase
	print("TEST expanded guest: verified %s" % sent)

func host_step() -> void:
	var hopper = game.cargos.get(4)
	match stage:
		0:
			if checkpoint.guest_id == 0 or game.workers.size() != 2:
				return
			game.start_shift()
			if game.phase != "playing":
				fail("two-worker shift did not start")
				return
			var carrier = game.workers[checkpoint.guest_id]
			carrier.position = Vector3(4, 0.05, 3)
			carrier.heading = 0
			hopper.body.freeze = true
			hopper.body.position = carrier.hand_position()
			hopper.rules.holder_id = checkpoint.guest_id
			hopper.hopper.phase = "windup"
			hopper.hopper.remaining = 0.8
			hopper.hopper.step(0.01)
			announce("held", {"guest": checkpoint.guest_id})
			stage = 1
		1:
			if not checkpoint.acknowledged:
				return
			if hopper.hopper.remaining < 0.79:
				fail("held Hopper windup advanced on host")
				return
			hopper.rules.holder_id = 0
			hopper.body.freeze = false
			hopper.hopper.remaining = 0.15
			stage = 2
		2:
			if hopper.hopper.phase != "airborne":
				return
			announce("hop")
			stage = 3
		3:
			if not checkpoint.acknowledged:
				return
			var cargo = game.cargos[1]
			cargo.rules.destination = 2
			cargo.body.freeze = true
			cargo.body.position = Vector3(0, 0.55, 3)
			announce("destination_b")
			stage = 4
		4:
			if not checkpoint.acknowledged:
				return
			var cargo = game.cargos[1]
			cargo.body.position = Layout.bay(2)
			stage = 5
		5:
			if game.score != 1:
				return
			announce("delivery_b")
			stage = 6
		6:
			if not checkpoint.acknowledged:
				return
			var guest = game.workers[checkpoint.guest_id]
			guest.position = Vector3(0, 0.05, 5)
			guest.heading = 0
			game.cargos[2].body.freeze = true
			game.cargos[2].body.position = Vector3(0, 0.55, 2.5)
			announce("request_ping")
			stage = 7
		7:
			if game.pings.remaining <= 0 or game.pings.slot != game.workers[checkpoint.guest_id].slot:
				return
			announce("ping")
			stage = 8
		8:
			if not checkpoint.acknowledged or Time.get_ticks_msec() - announced_at < 250:
				return
			if game.conveyor.direction != -1:
				fail("conveyor did not start in its reset direction")
				return
			game.workers[checkpoint.guest_id].position = Layout.LEVER + Vector3(0, 0.05, 1)
			announce("request_conveyor")
			stage = 9
		9:
			if game.conveyor.direction != 1:
				return
			announce("conveyor")
			stage = 10
		10:
			if not checkpoint.acknowledged or Time.get_ticks_msec() - announced_at < 250:
				return
			game.phase = "won"
			game.round_state.base_won = true
			game.cargos[1].rules.score = game.round_state.quota
			announce("request_overtime")
			stage = 11
		11:
			if not game.round_state.votes.has(checkpoint.guest_id):
				return
			game._receive_action(1, "overtime")
			if not game.round_state.bonus or not game.round_state.base_won or game.score != 6 or game.round_state.quota != 10:
				fail("overtime state differs: score %d quota %d base_won %s bonus %s" % [game.score, game.round_state.quota, game.round_state.base_won, game.round_state.bonus])
				return
			announce("overtime")
			stage = 12
		12:
			if not checkpoint.acknowledged:
				return
			game.leave_game()
			print("PASS network host: Hopper pause/hop, B delivery, ping, conveyor and overtime")
			quit(0)

func guest_step() -> void:
	if sent == "overtime" and game.phase == "menu":
		print("PASS network guest: replicated Hopper, B destination, ping, conveyor and overtime")
		quit(0)
		return
	if game.session.mode != "guest" or not game.workers.has(game.local_id):
		return
	if checkpoint.guest_id == 0:
		checkpoint.identify.rpc_id(1)
	match checkpoint.phase:
		"held":
			var hopper = game.cargos[4]
			if hopper.rules.holder_id == game.local_id and hopper.hopper.phase == "windup" and hopper.hopper.paused and hopper.hopper.remaining >= 0.79:
				ack()
		"hop":
			if game.cargos[4].hopper.phase == "airborne":
				ack()
		"destination_b":
			if game.cargos[1].rules.destination == 2:
				ack()
		"delivery_b":
			if game.score == 1:
				ack()
		"request_ping":
			if sent != "request_ping":
				game.session.send_action("ping")
				sent = "request_ping"
		"ping":
			if game.pings.remaining > 0 and game.pings.slot == game.workers[game.local_id].slot:
				ack()
		"request_conveyor":
			if sent != "request_conveyor" and game.conveyor.direction == -1:
				game.session.send_action("lever")
				sent = "request_conveyor"
		"conveyor":
			if game.conveyor.direction == 1:
				ack()
		"request_overtime":
			if sent != "request_overtime":
				game.session.send_action("overtime")
				sent = "request_overtime"
		"overtime":
			if game.phase == "playing" and game.round_state.bonus and game.round_state.base_won and game.score == 6 and game.round_state.quota == 10:
				ack()

func fail(message: String) -> void:
	push_error("FAIL expanded network %s: %s" % [role, message])
	quit(1)

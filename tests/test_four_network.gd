extends SceneTree

const Layout = preload("res://scripts/depot_layout.gd")

# This node only synchronizes assertions. Fixture mutations occur on the host.
class Checkpoint extends Node:
	var phase := ""
	var data: Dictionary = {}
	var acks: Dictionary = {}
	var roles: Dictionary = {}

	@rpc("any_peer", "call_remote", "reliable")
	func identify(role_name: String) -> void:
		if multiplayer.is_server():
			roles[role_name] = multiplayer.get_remote_sender_id()

	@rpc("authority", "call_remote", "reliable")
	func announce(next_phase: String, details: Dictionary) -> void:
		phase = next_phase
		data = details

	@rpc("any_peer", "call_remote", "reliable")
	func acknowledge(which: String) -> void:
		if multiplayer.is_server() and which == phase:
			acks[multiplayer.get_remote_sender_id()] = true

var game: Node
var checkpoint: Checkpoint
var role := ""
var deadline := 0
var stage := 0
var max_bytes := 0
var registered := false
var sent := ""
var roster: Dictionary = {}
var carrier := 0
var sticky_target := 0
var other_hit := 0
var phase_at := 0

func _initialize() -> void:
	for arg in OS.get_cmdline_user_args():
		if arg.begins_with("--role="):
			role = arg.trim_prefix("--role=")
	call_deferred("begin")

func begin() -> void:
	checkpoint = Checkpoint.new()
	checkpoint.name = "FourCheckpoint"
	root.add_child(checkpoint)
	game = load("res://scenes/main.tscn").instantiate()
	root.add_child(game)
	deadline = Time.get_ticks_msec() + 40000
	if role == "host":
		game.host_game(27946)
	else:
		game.join_game("127.0.0.1", 27946)

func _process(_delta: float) -> bool:
	if not game:
		return false
	if Time.get_ticks_msec() > deadline:
		fail("Timeout stage %d checkpoint %s acknowledgements %s" % [stage, checkpoint.phase, checkpoint.acks])
		return false
	if role == "host":
		host_step()
	else:
		guest_step()
	return false

func announce(which: String, details: Dictionary = {}) -> void:
	checkpoint.phase = which
	checkpoint.data = details
	checkpoint.acks.clear()
	checkpoint.announce.rpc(which, details)
	phase_at = Time.get_ticks_msec()
	print("TEST four host: %s" % which)

func ack() -> void:
	if sent != checkpoint.phase:
		print("TEST four %s: verified %s" % [role, checkpoint.phase])
		checkpoint.acknowledge.rpc_id(1, checkpoint.phase)
		sent = checkpoint.phase

func valid_roster(expected: Dictionary) -> bool:
	if game.workers.size() != expected.size():
		return false
	var slots: Array = []
	for id in expected:
		if not game.workers.has(id) or game.workers[id].slot != expected[id]:
			return false
		var slot: int = game.workers[id].slot
		if slot < 1 or slot > 4 or slots.has(slot):
			return false
		slots.append(slot)
	return true

func host_step() -> void:
	max_bytes = maxi(max_bytes, var_to_bytes(game._snapshot()).size())
	if max_bytes > 1280:
		fail("State exceeds 1280-byte budget: %d" % max_bytes)
		return
	var source = game.cargos[2]
	var sticky = game.cargos[3]
	var standard = game.cargos[1]
	match stage:
		0:
			if game.workers.size() != 4 or checkpoint.roles.size() != 3:
				return
			for id in game.workers:
				roster[id] = game.workers[id].slot
			if not valid_roster(roster):
				fail("Host roster slots are not unique 1..4")
				return
			for id in game.workers:
				for other in game.workers:
					if id != other and game.workers[id].position.distance_to(game.workers[other].position) < 0.7:
						fail("Overlapping lobby spawns")
						return
			carrier = checkpoint.roles.guest1
			sticky_target = checkpoint.roles.guest2
			other_hit = checkpoint.roles.guest3
			announce("roster", roster)
			stage = 1
		1:
			if checkpoint.acks.size() != 3:
				return
			game.start_shift()
			if game.phase != "playing":
				fail("Host cannot start with four workers")
				return
			game.workers[1].position = Vector3(-5, 0.05, 5)
			game.workers[carrier].position = Vector3(2, 0.05, 7)
			game.workers[sticky_target].position = Vector3(1.4, 0.05, 3.8)
			game.workers[other_hit].position = Vector3(2.7, 0.05, 3.8)
			source.body.freeze = true
			source.body.position = Vector3(2, 1.05, 6.02)
			source.sneeze.remaining = 30
			standard.body.freeze = true
			standard.body.position = Vector3(2.7, 1.05, 2.82)
			sticky.body.freeze = true
			sticky.body.position = Vector3(0.8, 1.05, 3.8)
			sticky.cling.cooldown = 0
			announce("pickup", {"carrier": carrier, "target": sticky_target, "other": other_hit})
			stage = 2
		2:
			if source.rules.holder_id != carrier or standard.rules.holder_id != other_hit or sticky.cling.target_kind != "worker":
				return
			if sticky.cling.target_id != sticky_target:
				fail("Clinger attached to wrong worker")
				return
			announce("bond", {"carrier": carrier, "target": sticky_target, "other": other_hit})
			stage = 3
		3:
			if checkpoint.acks.size() != 3:
				return
			source.sneeze.phase = "windup"
			source.sneeze.remaining = 1.2
			stage = 4
		4:
			if source.sneeze.event_id != 1:
				return
			if not blast_matches(carrier, sticky_target, other_hit):
				fail("Host blast hit/ownership/attachment result differs: %s" % game.last_blast)
				return
			announce("blast", {"carrier": carrier, "target": sticky_target, "other": other_hit})
			stage = 5
		5:
			if checkpoint.acks.size() != 3:
				return
			standard.body.freeze = true
			standard.body.linear_velocity = Vector3.ZERO
			standard.body.position = game.depot.bay(1)
			stage = 6
		6:
			if game.score != 1:
				return
			announce("delivery")
			stage = 7
		7:
			if checkpoint.acks.size() != 3:
				return
			if source.rules.holder_id != carrier:
				fail("Carrier lost source before leaving")
				return
			announce("leave")
			stage = 8
		8:
			if game.workers.size() != 3:
				return
			if game.phase != "waiting" or source.rules.holder_id != 0 or game.session.in_shift:
				fail("Guest departure did not cancel shift and release cargo")
				return
			roster.erase(carrier)
			if not valid_roster(roster):
				fail("Remaining slots changed after departure")
				return
			announce("waiting", roster)
			stage = 9
		9:
			if checkpoint.acks.size() != 2:
				return
			game.start_shift()
			if game.phase != "playing" or game.score != 0:
				fail("Three-worker restart failed")
				return
			announce("restart", roster)
			stage = 10
		10:
			if checkpoint.acks.size() != 2:
				return
			game.leave_game()
			print("PASS four network host: four roster/slots, bond, two hits, ownership, delivery, three restart, host departure; max snapshot %d bytes" % max_bytes)
			quit(0)

func blast_matches(owner: int, first: int, second: int) -> bool:
	var hit: Dictionary = game.last_blast
	return hit.get("event", 0) == 1 and hit.get("source", 0) == 2 and hit.workers.size() == 2 and hit.workers.has(first) and hit.workers.has(second) and hit.cargos.has(3) and hit.cargos.has(1) and game.cargos[2].rules.holder_id == owner and game.cargos[1].rules.holder_id == 0 and game.cargos[3].cling.target_kind.is_empty()

func guest_step() -> void:
	if sent == "restart" and game.phase == "menu" and game.notice_key == "host_left":
		print("PASS four network %s: all shared assertions, three restart and host-left reason" % role)
		quit(0)
		return
	if game.session.mode != "guest" or not game.workers.has(game.local_id):
		return
	if not registered:
		checkpoint.identify.rpc_id(1, role)
		registered = true
	if sent == checkpoint.phase:
		return
	var data := checkpoint.data
	match checkpoint.phase:
		"roster":
			if game.phase == "waiting" and valid_roster(data):
				ack()
		"pickup":
			if game.phase != "playing":
				return
			if role == "guest1" and game.cargos[2].rules.holder_id == 0:
				game.session.send_action("interact")
			if role == "guest3" and game.cargos[1].rules.holder_id == 0:
				game.session.send_action("interact")
		"bond":
			if game.cargos[3].cling.target_kind == "worker" and game.cargos[3].cling.target_id == data.target and game.cargos[3].cling.remaining > 0 and is_equal_approx(game.workers[data.target].speed_scale, 0.7) and game.cargos[2].rules.holder_id == data.carrier and game.cargos[1].rules.holder_id == data.other:
				ack()
		"blast":
			if blast_matches(data.carrier, data.target, data.other) and game.cargos[2].cues.bursts_played == 1:
				ack()
		"delivery":
			if game.score == 1 and game.cargos[1].rules.score == 1:
				ack()
		"leave":
			if role == "guest1":
				game.leave_game()
				print("PASS four network guest1: all shared assertions; departed while carrying source")
				quit(0)
		"waiting":
			if game.phase == "waiting" and valid_roster(data) and game.cargos[2].rules.holder_id == 0:
				ack()
		"restart":
			if game.phase == "playing" and valid_roster(data) and game.score == 0:
				ack()

func fail(message: String) -> void:
	push_error("FAIL four network %s: %s" % [role, message])
	quit(1)

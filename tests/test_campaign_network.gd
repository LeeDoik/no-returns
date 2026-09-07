extends "res://tests/test_expanded_network.gd"

var requested_at := 0
var saw_approach := false

func begin() -> void:
	checkpoint = Checkpoint.new()
	checkpoint.name = "CampaignCheckpoint"
	root.add_child(checkpoint)
	game = load("res://scenes/main.tscn").instantiate()
	root.add_child(game)
	deadline = Time.get_ticks_msec() + 30000
	if role == "host":
		game.host_game(27950, true)
		game.session.action_received.connect(func(id, action):
			if action == "horn": print("HORN CHECK phase=%s rat=%s worker=%s cooldown=%s" % [game.packrat.phase, game.packrat.position, game.workers[id].position, game.horn_cooldowns.get(id,0)])
		)
	else: game.join_game("127.0.0.1", 27950)

func complete_contract() -> void:
	var cargo = game.cargos[1]
	for i in range(game.round_state.quota):
		cargo.reset_crate()
		game.contracts.replace_cargo(1)
		cargo.body.freeze = true
		cargo.body.position = game.depot.bay(cargo.rules.destination)
		cargo.step(0.01, game.workers)
	game._physics_process(0.001)
	print("CAMPAIGN CHECK stage=%d bank=%d earned=%d relays=%d score=%d quota=%d" % [game.contracts.stage,game.contracts.credits,game.contracts.earned,game.contracts.relays,game.score,game.round_state.quota])

func host_step() -> void:
	if game.packrat.phase == "seek": saw_approach = true
	match stage:
		0:
			if checkpoint.guest_id == 0 or game.workers.size() != 2: return
			game.start_shift()
			complete_contract()
			announce("bank")
			stage = 1
		1:
			if not checkpoint.acknowledged or Time.get_ticks_msec() - announced_at < 350: return
			if game.contracts.boots != 0 or game.contracts.credits != 80:
				fail("guest changed shared bank")
				return
			game._receive_action(1, "buy_boots")
			announce("ready")
			stage = 2
		2:
			if not game.contracts.ready.has(checkpoint.guest_id): return
			game._receive_action(1, "next_contract")
			if game.contracts.stage != 1 or not game.packrat.active:
				fail("second contract failed to start")
				return
			var cargo = game.cargos[1]
			cargo.body.freeze = false
			cargo.body.position = game.packrat.position + Vector3(3.5, 0.4, 1.7)
			cargo.body.linear_velocity = Vector3.ZERO
			game.workers[checkpoint.guest_id].position = game.packrat.position + Vector3(2, 0, 1)
			stage = 3
		3:
			if not game.cargos[1].creature_held: return
			announce("stolen")
			stage = 4
		4:
			if game.packrat.phase != "flee" or game.cargos[1].creature_held: return
			if game.cargos[1].body.collision_mask != 7:
				fail("scared rat did not restore cargo collision")
				return
			announce("rescued")
			stage = 5
		5:
			if not checkpoint.acknowledged: return
			complete_contract()
			announce("ready_final")
			stage = 6
		6:
			if not game.contracts.ready.has(checkpoint.guest_id): return
			game._receive_action(1, "next_contract")
			complete_contract()
			announce("finished")
			stage = 7
		7:
			if not checkpoint.acknowledged: return
			if not game.contracts.finished or game.contracts.credits != 280 or game.profile.runs != 1 or not saw_approach:
				fail("host completed run differs")
				return
			print("PASS network host: campaign bank, permissions, shared upgrade/readiness, theft/rescue, all contracts")
			game.leave_game()
			quit(0)

func guest_step() -> void:
	if game.packrat.phase == "seek": saw_approach = true
	if game.session.mode != "guest":
		if sent == "finished":
			print("PASS network guest: campaign metadata, shared upgrades, claimed cargo, horn rescue and completion")
			quit(0)
		return
	if checkpoint.guest_id == 0:
		checkpoint.guest_id = game.local_id
		checkpoint.identify.rpc_id(1)
	match checkpoint.phase:
		"bank":
			if game.contracts.credits != 80 or game.phase != "won": return
			if sent != "bank": game.session.send_action("buy_boots")
			ack()
		"ready":
			if game.contracts.boots != 1 or game.contracts.credits != 60: return
			request_next()
			ack()
		"stolen":
			if not game.packrat.active or not game.cargos[1].creature_held: return
			if Time.get_ticks_msec() - requested_at > 200:
				requested_at = Time.get_ticks_msec()
				print("REQUEST HORN confirmed=%s phase=%s" % [game.session.confirmed,game.phase])
				game.session.send_action("horn")
			ack()
		"rescued":
			if game.cargos[1].creature_held or game.packrat.phase != "flee" or float(game.horn_cooldowns.get(game.local_id,0)) <= 0: return
			ack()
		"ready_final":
			if game.phase != "won" or game.contracts.stage != 1 or game.contracts.credits != 160: return
			request_next()
			ack()
		"finished":
			if not game.contracts.finished or game.contracts.credits != 280 or game.profile.runs != 1 or not saw_approach: return
			ack()

func request_next() -> void:
	if Time.get_ticks_msec() - requested_at > 200:
		requested_at = Time.get_ticks_msec()
		game.session.send_action("next_contract")

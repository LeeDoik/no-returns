extends SceneTree

const Layout = preload("res://scripts/depot_layout.gd")
const Profile = preload("res://scripts/run_profile.gd")
var failures := 0
func check(ok: bool, text: String) -> void:
	if not ok:
		failures += 1
		push_error(text)
func _initialize() -> void:
	call_deferred("run")
func run() -> void:
	var game = load("res://scenes/main.tscn").instantiate()
	root.add_child(game)
	game.practice_game(true)
	game.set_physics_process(false)
	await physics_frame
	await process_frame
	check(game.contracts.enabled and game.time_left == 240 and not game.packrat.active, "first campaign contract is clear onboarding")
	game._add_worker(22)
	game.start_shift()
	game._render_ui()
	check(game.round_state.quota == 6, "campaign scales for two workers")
	var cargo = game.cargos[1]
	var sender = game.workers[1]
	var receiver = game.workers[22]
	sender.position = Vector3(-10, 0, 3)
	sender.heading = 0
	cargo.body.freeze = true
	cargo.body.position = sender.hand_position()
	check(cargo.pickup(sender), "sender owns actual package")
	cargo.release(sender, true)
	receiver.position = Vector3(-10, 0, -3)
	receiver.heading = PI
	cargo.body.position = receiver.hand_position()
	cargo.flight_left = 1.5
	check(cargo.pickup(receiver) and cargo.relay_ready, "different crew member catches valid airborne relay")
	cargo.release(receiver, false)
	cargo.body.freeze = true
	cargo.body.position = game.depot.bay(1)
	cargo.step(0.01, game.workers)
	check(game.contracts.earned == 15 and game.contracts.relays == 1, "correct delivery rewards one relay")
	check(not cargo.relay_ready, "dispatch consumes relay provenance")
	cargo.reset_crate()
	game.contracts.replace_cargo(1)
	cargo.body.position = receiver.hand_position()
	cargo.throw_peer = receiver.peer_id
	cargo.throw_origin = cargo.body.position + Vector3.BACK * 4
	cargo.flight_left = 2
	check(cargo.pickup(receiver) and not cargo.relay_ready, "self catch cannot earn relay")
	cargo.release(receiver, false)
	cargo.recover("wrong_bay")
	check(not cargo.relay_ready and cargo.flight_left == 0, "wrong bay clears reward provenance")
	game._finish("lost")
	check(game.contracts.credits == 0, "failed attempt never banks relay/dispatch rewards")
	game.start_shift()
	for stage in range(3):
		for i in range(game.round_state.quota):
			cargo.reset_crate()
			game.contracts.replace_cargo(1)
			cargo.body.freeze = true
			cargo.body.position = game.depot.bay(cargo.rules.destination)
			cargo.step(0.01, game.workers)
		game._physics_process(0.01)
		check(game.phase == "won" and game.contracts.won, "physical shipments complete contract %d" % stage)
		if stage == 0:
			game._receive_action(22, "buy_boots")
			check(game.contracts.boots == 0, "guest purchase denied in integration")
			game._receive_action(1, "buy_boots")
			check(game.contracts.boots == 1 and game.contracts.credits == 60, "shared purchase applied once")
		if stage < 2:
			game._receive_action(22, "next_contract")
			check(game.phase == "won", "host readiness still needed")
			game._receive_action(1, "next_contract")
			check(game.phase == "playing" and game.contracts.stage == stage + 1 and game.packrat.active, "ready crew advances with active creature")
	check(game.contracts.finished and game.profile.runs == 1 and game.contracts.credits == 280, "three-contract run completes and records once")
	check(not game.profile.complete(game.contracts), "duplicate completion cannot write another record")
	check(var_to_bytes(game._metadata_snapshot()).size() <= 2048, "reliable campaign metadata remains bounded")
	check(var_to_bytes(game.packrat.snapshot()).size() <= 256, "creature packet remains bounded")
	var record = Profile.new()
	record.path = "res://artifacts/run-profile-test.cfg"
	record.last_id = ""
	check(record.complete(game.contracts) and record.save_error == OK, "completed run saves")
	var loaded = Profile.new()
	loaded.path = record.path
	loaded.load_record()
	check(loaded.runs == 1 and loaded.best_credits == 300 and loaded.best_deliveries == 24 and loaded.last_id == game.contracts.run_id, "profile reloads exact completed-run record")
	var cfg := ConfigFile.new()
	cfg.set_value("record", "version", 1)
	cfg.set_value("record", "runs", INF)
	cfg.set_value("record", "best_credits", "bad")
	cfg.save(record.path)
	loaded.load_record()
	check(loaded.runs == 0 and loaded.best_credits == 0, "invalid saved numeric fields recover safely")
	var completed: Dictionary = game._metadata_snapshot()
	game.profile.last_id = ""
	game.profile.runs = 0
	game.participation = 0
	game.observed_run = ""
	game._receive_metadata(completed)
	check(game.profile.runs == 0, "late join cannot claim a completed run")
	for stage in range(3):
		var active: Dictionary = completed.duplicate(true)
		active.campaign.stage = stage
		active.campaign.settled = false
		active.campaign.finished = false
		game._receive_metadata(active)
	game._receive_metadata(completed)
	check(game.profile.runs == 1, "guest must observe all three active contracts")
	game.ui.help_open = false
	game.ui._help_pressed()
	check(game.ui.help_open, "help button opens once")
	game.ui._help_pressed()
	check(not game.ui.help_open, "help button closes once")
	game.leave_game()
	check(not game.packrat.active and not game.contracts.enabled, "leave clears campaign and creature")
	game.queue_free()
	await process_frame
	print("CAMPAIGN %s" % ("PASS" if failures == 0 else "FAIL"))
	quit(1 if failures else 0)

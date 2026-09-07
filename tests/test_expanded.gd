extends SceneTree

const Layout = preload("res://scripts/depot_layout.gd")

var game
var failures := 0
func _initialize() -> void:
	call_deferred("run")
func check(ok: bool, message: String) -> void:
	if not ok:
		failures += 1
		push_error("EXPANDED: " + message)
func frames(count: int) -> void:
	for index in range(count): await physics_frame
func run() -> void:
	game = load("res://scenes/main.tscn").instantiate()
	root.add_child(game)
	game.replay_seed = 817
	game.practice_game()
	game.set_physics_process(false)
	await frames(3)
	var cargo = game.cargos[1]
	cargo.rules.destination = 2
	cargo.body.freeze = true
	cargo.body.position = Layout.bay(1)
	cargo.step(0.01, game.workers)
	check(game.score == 0 and game.notice_key == "wrong_bay", "B label rejects A bay")
	cargo.step(1.3, game.workers)
	check(cargo.rules.destination == 2, "wrong-bay recovery keeps destination")
	cargo.body.freeze = true
	cargo.body.position = Layout.bay(2)
	cargo.step(0.01, game.workers)
	check(game.score == 1 and cargo.rules.delivered, "B label scores in B bay")
	cargo.step(0.01, game.workers)
	check(game.score == 1, "delivered cargo cannot score twice")
	var sticky = game.cargos[3]
	sticky.cling.target_kind = "cargo"
	sticky.cling.target_id = 1
	sticky.cling.remaining = 4
	sticky.cling.step(0.01, game.workers, game.cargos)
	check(sticky.rules.score == 0 and sticky.recovery_left > 0, "mismatched attached A does not score with delivered B")
	sticky.reset_crate()
	sticky.rules.destination = 2
	sticky.cling.target_kind = "cargo"
	sticky.cling.target_id = 1
	sticky.cling.remaining = 4
	sticky.cling.step(0.01, game.workers, game.cargos)
	check(sticky.rules.score == 1 and game.score == 2, "matching attached B scores once with delivered B")
	game.start_shift()
	var worker = game.workers[1]
	worker.position = Vector3(0, 0, 3)
	worker.heading = 0
	cargo.body.position = Vector3(0, 0.5, 1.1)
	game._receive_action(900, "ping")
	check(game.pings.remaining == 0, "outsider ping rejected")
	game._receive_action(1, "ping")
	check(game.pings.cargo_id == 1 and game.pings.slot == 1, "ping selects visible cargo in front")
	game.pings.step(0.5)
	game._receive_action(1, "ping")
	check(is_equal_approx(game.pings.remaining, 3.5), "spam does not extend marker")
	game.pings.step(4)
	check(game.pings.snapshot().is_empty(), "marker expires")
	game._add_worker(22)
	game.start_shift()
	check(game.round_state.quota == 6, "two-player quota applied")
	game.cargos[1].rules.score = 6
	game._finish("won")
	game._receive_action(22, "overtime")
	game._receive_action(22, "overtime")
	check(game.phase == "won" and game.round_state.votes.size() == 1, "duplicate vote cannot start bonus")
	game._receive_action(1, "overtime")
	check(game.phase == "playing" and game.round_state.bonus and game.round_state.quota == 10 and game.score == 6, "all votes begin one bonus and retain base score")
	game.time_left = 0.001
	game._physics_process(0.01)
	check(game.phase == "bonus_done" and game.round_state.base_won, "bonus timeout keeps base success")
	game._receive_action(1, "overtime")
	check(game.phase == "bonus_done", "no repeated bonus")
	game._receive_action(1, "restart")
	check(game.score == 0 and not game.round_state.bonus and game.round_state.votes.is_empty(), "restart resets bonus")
	for id in [33,44]: game._add_worker(id)
	game.start_shift()
	game.last_blast = {"source":2,"event":99,"workers":[1,22,33,44],"cargos":[1,3,4]}
	game.pings.mark(game.workers[1], game.cargos)
	game.round_state.votes = [1,22,33,44]
	game.cargos[2].sneeze.phase = "windup"
	game.cargos[3].cling.target_kind = "worker"
	game.cargos[3].cling.target_id = 22
	game.cargos[4].hopper.phase = "airborne"
	game._set_notice("sneeze_blast", 3)
	var bytes: int = var_to_bytes(game._snapshot()).size()
	check(bytes <= 1280, "full snapshot budget: %d" % bytes)
	game._render_ui()
	check(game.ui.score_label.text.contains("10"), "HUD displays dynamic target")
	check(not game.pings.snapshot().is_empty(), "marker exists before coworker leaves")
	game._player_left(22)
	check(game.phase == "waiting" and game._snapshot().ping.is_empty(), "coworker leaving clears marker in waiting snapshot")
	game.leave_game()
	check(game.pings.snapshot().is_empty(), "leave clears marker")
	game.queue_free()
	await process_frame
	print("EXPANDED %s: packet %d bytes" % ["PASS" if failures == 0 else "FAIL", bytes])
	quit(1 if failures else 0)

extends SceneTree

var failures := 0
func check(ok: bool, message: String) -> void:
	if not ok:
		failures += 1
		push_error(message)

func _initialize() -> void:
	_run.call_deferred()

func _run() -> void:
	var game = load("res://scenes/main.tscn").instantiate()
	root.add_child(game)
	await process_frame
	game.practice_game(true)
	await physics_frame
	var before: float = game.time_left
	game.ui.paused = true
	if game.has_method("_sync_pause"): game._sync_pause()
	await create_timer(0.15, true).timeout
	check(is_equal_approx(game.time_left, before), "Solo menu must freeze contract time")
	check(paused, "Solo menu must pause physics, not only timer")
	game._command("resume", "")
	await process_frame
	check(not paused, "Resume must unpause the tree")
	game._command("settings", "")
	check(game.ui.get("settings_open") == true, "Settings must open during play")
	game.ui.settings.pending = "interact"
	game._request_exit("quit")
	check(game.ui.settings.pending.is_empty() and game.ui.confirm_action == "quit", "Exit cancels pending key capture so Escape can dismiss confirmation")
	game._command("cancel_exit", "")
	check(game.phase == "playing" and game.ui.settings_open,"Cancel exit returns to current settings without losing the run")
	game._command("settings_close", "")
	game.ui.paused = false; game._sync_pause()
	game.phase = "won"; game._command("settings", "")
	game.start_shift()
	check(game.ui.paused and Input.mouse_mode == Input.MOUSE_MODE_VISIBLE,"Starting next contract preserves host settings")
	game._command("settings_close", "")
	game.phase = "won"; game._command("settings", "")
	game._begin_bonus()
	check(game.ui.paused,"Starting overtime preserves host settings")
	game._command("settings_close", "")
	game._command("resume", "")
	var snapshot: Dictionary = game._snapshot()
	game.phase = "waiting"; game.session.mode = "guest"
	game._command("settings", "")
	game._receive_snapshot(snapshot)
	check(game.ui.paused and Input.mouse_mode == Input.MOUSE_MODE_VISIBLE,"Host start preserves guest settings cursor and blocks gameplay input")
	game.ui.confirm_action = "quit"; game.exit_previous_pause = false
	game._command("cancel_exit", "")
	check(game.ui.paused and Input.mouse_mode == Input.MOUSE_MODE_VISIBLE,"Cancel nested exit cannot resume beneath settings")
	game._command("settings_close", "")
	check(not game.ui.paused and not paused,"Guest can close settings and enter running game")
	game.ui.paused = true; game._sync_pause()
	check(not paused,"Online menu must not pause the shared world")
	game.leave_game()
	check(not paused, "Leaving must restore processing")
	game._command("settings", "")
	check(game.ui.get("settings_open") == true, "Settings must open from main menu")
	game.leave_game()
	game.session.mode = "guest"
	game.session.confirmed = false
	game.session.join_deadline = Time.get_ticks_msec() - 1
	game.session._process(0)
	check(game.session.mode == "offline", "Admission must time out after transport connects")
	game.queue_free()
	await process_frame
	print("RELEASE USABILITY %s" % ("PASS" if failures == 0 else "FAIL"))
	quit(0 if failures == 0 else 1)

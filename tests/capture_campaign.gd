extends "res://tests/capture_sneezer.gd"

func campaign_state(phase: String = "playing") -> Dictionary:
	return {"phase": phase, "notice": "", "count": 3, "score": 8, "quota": 12, "time": 147.0,
		"mode": "host", "prompt": "carrying", "cargo_name": "SNEEZER / A ↑↑", "warning": "",
		"bonus": false, "base_won": true, "elapsed": 93.0, "seed": 707, "votes": 0,
		"voted": false, "progress": preload("res://scripts/copy.gd").get_text("lesson_3"), "creature": "packrat_warning",
		"horn_left": 0.0, "help": false, "lesson": 3,
		"campaign": {"enabled": true, "stage": 1, "credits": 35, "earned": 25,
			"deliveries": 19, "relays": 3, "boots": 1, "time_upgrade": 0, "horn": 0,
			"finished": false, "ready": 1, "voted": false, "best_runs": 2}}

func _run() -> void:
	game = load("res://scenes/main.tscn").instantiate(); root.add_child(game); game.test_mode = true
	game.set_process(false); game.set_physics_process(false); await settle(20)
	await capture("campaign-menu-ko")
	game.ui._toggle_language(); await settle(12); await capture("campaign-menu-en")
	game.ui._toggle_language(); game.ui.render(campaign_state()); await settle(12); await capture("campaign-hud-ko")
	game.ui.paused = true; game.ui.render(campaign_state()); await settle(12); await capture("campaign-settings-ko")
	game.ui.paused = false
	game.ui.toggle_help(); game.ui.render(campaign_state()); await settle(12); await capture("campaign-help-ko")
	game.ui._toggle_language(); game.ui.render(campaign_state()); await settle(12); await capture("campaign-help-en")
	print("HUD GEOMETRY phase=%s heading=%s row=%s panel=%s" % [game.ui.phase_label.get_global_rect(), game.ui.phase_label.get_parent().get_global_rect(), game.ui.phase_label.get_parent().get_parent().get_global_rect(), game.ui.phase_label.get_parent().get_parent().get_parent().get_global_rect()])
	game.ui.help_open = false
	game.ui.render(campaign_state("won")); await settle(12); await capture("campaign-contract-en")
	game.ui._toggle_language()
	var result := campaign_state("won"); game.ui.render(result); await settle(12); await capture("campaign-contract-ko")
	result.mode = "guest"; result.campaign.credits = 100; game.ui.render(result); await settle(12); await capture("campaign-contract-guest-ko")
	game.queue_free(); await process_frame
	print("CAMPAIGN VISUAL CAPTURE %s" % ("PASS" if failures == 0 else "FAIL")); quit(0 if failures == 0 else 1)

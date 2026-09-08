extends "res://tests/capture_campaign.gd"

func _run() -> void:
	game = load("res://scenes/main.tscn").instantiate(); root.add_child(game)
	game.set_process(false); game.set_physics_process(false)
	await settle(20)
	for language in ["ko","en"]:
		preload("res://scripts/copy.gd").language = language; game.ui._apply_copy()
		game.ui.render({"phase":"menu"}); await settle(8); await capture("release-menu-"+language)
		game.ui.settings_open = true; game.ui.render({"phase":"menu"}); await settle(8); await capture("release-settings-"+language)
		game.ui.settings_open = false; game.ui.help_open = true; game.ui.render({"phase":"menu"}); await settle(8); await capture("release-help-"+language)
		game.ui.help_open = false
		game.ui.render(campaign_state("won")); await settle(8); await capture("release-result-"+language)
		game.ui.paused = true; game.ui.render(campaign_state()); await settle(8); await capture("release-pause-"+language)
		game.ui.confirm_action = "leave"; game.ui.render(campaign_state()); await settle(8); await capture("release-exit-"+language)
		game.ui.confirm_action = ""; game.ui.paused = false
	game.ui.settings_open = true; game.ui.render({"phase":"menu"})
	game.ui.settings.get_child(0).get_child(1).scroll_vertical = 10000
	await settle(6); await capture("release-controls-en")
	game.ui.settings_open = false
	root.content_scale_size = Vector2i.ZERO
	for dimensions in [Vector2i(1024,768),Vector2i(1600,900)]:
		root.size = dimensions; await settle(12)
		game.ui.render({"phase":"menu"}); await settle(6)
		var bounds: Rect2 = game.ui.transform * game.ui.menu.get_rect()
		if not Rect2(Vector2.ZERO,Vector2(dimensions)).encloses(bounds): failures += 1; push_error("Menu exceeds resized viewport")
		await capture("release-menu-%dx%d" % [dimensions.x,dimensions.y])
	game.leave_game(); game.queue_free(); await process_frame
	print("RELEASE CAPTURE %s" % ("PASS" if failures == 0 else "FAIL")); quit(0 if failures == 0 else 1)

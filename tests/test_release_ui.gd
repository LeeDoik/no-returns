extends SceneTree
const UI = preload("res://scripts/interface.gd")
const Copy = preload("res://scripts/copy.gd")
var failures := 0
func check(ok: bool, message: String) -> void:
	if not ok: failures += 1; push_error(message)
func _initialize() -> void: _run.call_deferred()
func _run() -> void:
	var ui := UI.new(); root.add_child(ui)
	for language in ["ko","en"]:
		Copy.language = language; ui._apply_copy()
		ui.render({"phase":"menu"}); await process_frame; await process_frame
		check(ui.menu.size.x <= 498.1 and ui.menu.size.y <= 708.1,"Menu must fit designed panel in " + language)
		ui.settings_open = true; ui.render({"phase":"menu"}); await process_frame
		check(ui.menu_buttons.campaign.focus_mode == Control.FOCUS_NONE,"Background play button cannot receive keyboard focus under settings")
		check(ui.settings.back.has_focus(),"Settings has a focused return button")
		check(ui.settings.size.y <= 700.1,"Long settings remain scrollable instead of growing off screen")
		ui.settings_open = false; ui.help_open = true; ui.render({"phase":"menu"}); await process_frame
		check(ui.help_body.size.x <= 716.1,"English/Korean help wraps within manual")
		ui.help_open = false; ui.paused = true; ui.confirm_action = "leave"
		ui.render({"phase":"playing","mode":"host"}); await process_frame
		check(ui.confirm_no.has_focus(),"Exit defaults to staying in game")
		check(not ui.pause.visible and ui.shade.visible,"Exit confirmation is the single visible modal")
		ui.confirm_action = ""; ui.paused = false; ui.render({"phase":"playing","mode":"practice"})
		check(not ui.actions_panel.visible,"No accidental leave button during active gameplay")
		ui.render({"phase":"lost","mode":"practice","campaign":{"enabled":true}})
		check(not ui.campaign_buttons.buy_boots.visible,"Failed contract does not show unusable shop buttons")
		ui.render({"phase":"connecting","mode":"guest"})
		ui.render({"phase":"waiting","mode":"host","count":2})
		check(ui.action_button.focus_mode == Control.FOCUS_ALL,"Start becomes keyboard accessible when lobby is ready")
	ui.queue_free(); await process_frame
	print("RELEASE UI %s" % ("PASS" if failures == 0 else "FAIL")); quit(0 if failures == 0 else 1)

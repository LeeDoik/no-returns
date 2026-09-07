extends CanvasLayer

signal command(action: String, address: String)

const Preferences = preload("res://scripts/preferences.gd")
const Copy = preload("res://scripts/copy.gd")
const CampaignCopy = preload("res://scripts/campaign_copy.gd")
const Cues = preload("res://scripts/sneeze_cues.gd")
var menu: PanelContainer
var hud: Control
var pause: PanelContainer
var address: LineEdit
var menu_status: Label
var phase_label: Label
var score_label: Label
var timer_label: Label
var crew_label: Label
var prompt: Label
var notice: Label
var controls: Label
var action_button: Button
var menu_buttons: Dictionary = {}
var copy_labels: Dictionary = {}
var campaign_labels: Dictionary = {}
var campaign_buttons: Dictionary = {}
var language_button: Button
var menu_note: Label
var paused := false
var overtime_button: Button
var result_panel: PanelContainer
var result_label: Label
var sensitivity_button: Button
var sound_buttons: Array[Button] = []
var volume_button: Button
var fov_button: Button
var fullscreen_button: Button
var help_panel: PanelContainer
var help_body: Label
var help_open := false
var campaign_strip: Label
var progress_label: Label
var creature_label: Label
var horn_label: Label
var campaign_result: VBoxContainer
var campaign_title: Label
var campaign_bank: Label
var campaign_stats: Label

func _ready() -> void:
	var root := Control.new(); root.set_anchors_and_offsets_preset(Control.PRESET_FULL_RECT); root.mouse_filter = Control.MOUSE_FILTER_IGNORE
	var theme := Theme.new(); var font := SystemFont.new(); font.font_names = PackedStringArray(["Malgun Gothic", "Segoe UI"])
	theme.default_font = font; theme.default_font_size = 18; root.theme = theme; add_child(root)
	_build_menu(root); _build_hud(root); _build_pause(root); _build_help(root); _apply_copy()

func _build_menu(root: Control) -> void:
	menu = _panel(root, Vector2(42, 34), Vector2(512, 732)); var stack := _stack(menu, 8)
	_campaign_label(stack, "edition", 14, Color("e5b75b")); _label(stack, "NO\nRETURNS", 48, Color("f8f0db"))
	_campaign_label(stack, "menu_intro", 19, Color("d8e3e4")); _rule(stack)
	menu_buttons["campaign"] = _campaign_button(stack, "campaign", func(): command.emit("campaign", ""), true)
	menu_buttons["host"] = _campaign_button(stack, "host_campaign", func(): command.emit("host_campaign", ""))
	menu_buttons["practice"] = _campaign_button(stack, "practice_short", func(): command.emit("practice", ""))
	_copy_label(stack, "address", 13, Color("9fb3bc"))
	var row := HBoxContainer.new(); row.add_theme_constant_override("separation", 8); stack.add_child(row)
	address = LineEdit.new(); address.text = "127.0.0.1"; address.max_length = 253; address.custom_minimum_size = Vector2(300, 42); row.add_child(address)
	menu_buttons["join"] = _button(row, "join", func(): command.emit("join", address.text))
	menu_status = _label(stack, "", 13, Color("ffab82")); menu_status.autowrap_mode = TextServer.AUTOWRAP_WORD_SMART
	var utility := HBoxContainer.new(); utility.add_theme_constant_override("separation", 8); stack.add_child(utility)
	language_button = _button(utility, "", _toggle_language); sound_buttons.append(_button(utility, "", _toggle_sound))
	menu_buttons["help"] = _campaign_button(utility, "help", _help_pressed)
	_copy_label(stack, "development", 13, Color("9fb3bc"))
	_campaign_button(stack, "quit", func(): command.emit("quit", ""))
	menu_note = _label(root, "", 20, Color("ffedc4")); menu_note.position = Vector2(725, 672)

func _build_hud(root: Control) -> void:
	hud = Control.new(); hud.set_anchors_and_offsets_preset(Control.PRESET_FULL_RECT); hud.mouse_filter = Control.MOUSE_FILTER_IGNORE; root.add_child(hud)
	var top := _panel(hud, Vector2(24, 20), Vector2(1232, 84)); var top_row := HBoxContainer.new(); top_row.add_theme_constant_override("separation", 24); top.add_child(top_row)
	var heading := VBoxContainer.new(); heading.size_flags_horizontal = Control.SIZE_EXPAND_FILL; top_row.add_child(heading)
	_copy_label(heading, "prototype", 12, Color("c6ab74")); phase_label = _label(heading, "", 21, Color("f7efdc"))
	campaign_strip = _label(top_row, "", 15, Color("afcbd0")); score_label = _label(top_row, "", 22, Color("81d9c4")); timer_label = _label(top_row, "", 27, Color("f6d184")); crew_label = _label(top_row, "", 15, Color("bacbd3"))
	var bottom := _panel(hud, Vector2(24, 658), Vector2(910, 118)); var lower := _stack(bottom, 4)
	prompt = _label(lower, "", 19, Color("f6d184")); notice = _label(lower, "", 14, Color("85d9c8")); progress_label = _label(lower, "", 13, Color("b7c9cf")); controls = _label(lower, "", 13, Color("94aab3"))
	var actions := _panel(hud, Vector2(958, 658), Vector2(298, 118)); var actions_stack := _stack(actions, 6)
	action_button = _button(actions_stack, "start", func(): command.emit("start", ""), true); menu_buttons["leave"] = _button(actions_stack, "leave", func(): command.emit("leave", ""))
	creature_label = _label(hud, "", 14, Color("ffbd85")); creature_label.position = Vector2(930, 118)
	horn_label = _label(hud, "", 14, Color("f6d184")); horn_label.position = Vector2(1030, 145)
	result_panel = _panel(hud, Vector2(293, 178), Vector2(694, 438)); var result_stack := _stack(result_panel, 10)
	result_label = _label(result_stack, "", 21, Color("f6d184")); overtime_button = _button(result_stack, "overtime", func(): command.emit("overtime", ""), true)
	campaign_result = _stack(result_stack, 8); campaign_title = _label(campaign_result, "", 27, Color("f6d184")); campaign_bank = _label(campaign_result, "", 20, Color("81d9c4")); campaign_stats = _label(campaign_result, "", 15, Color("bdcdd2"))
	campaign_buttons["buy_boots"] = _campaign_button(campaign_result, "buy_boots", func(): command.emit("buy_boots", ""))
	campaign_buttons["buy_time"] = _campaign_button(campaign_result, "buy_time", func(): command.emit("buy_time", ""))
	campaign_buttons["buy_horn"] = _campaign_button(campaign_result, "buy_horn", func(): command.emit("buy_horn", ""))
	campaign_buttons["next_contract"] = _campaign_button(campaign_result, "next_contract", func(): command.emit("next_contract", ""), true)

func _build_pause(root: Control) -> void:
	pause = _panel(root, Vector2(405, 124), Vector2(470, 570)); var stack := _stack(pause, 12)
	_copy_label(stack, "pause_title", 27, Color("f6d184")); _copy_label(stack, "pause_note", 14, Color("b6c8ce"))
	menu_buttons["resume"] = _button(stack, "resume", func(): command.emit("resume", ""), true)
	_campaign_label(stack, "settings", 14, Color("e5b75b")); sound_buttons.append(_button(stack, "", _toggle_sound))
	volume_button = _button(stack, "", _toggle_volume); sensitivity_button = _button(stack, "", _toggle_sensitivity)
	fov_button = _button(stack, "", _toggle_fov); fullscreen_button = _button(stack, "", _toggle_fullscreen)
	menu_buttons["pause_help"] = _campaign_button(stack, "help", _help_pressed)
	menu_buttons["pause_leave"] = _button(stack, "leave", func(): command.emit("leave", ""))
	menu_buttons["quit"] = _campaign_button(stack, "quit", func(): command.emit("quit", ""))

func _build_help(root: Control) -> void:
	help_panel = _panel(root, Vector2(310, 120), Vector2(660, 560)); var stack := _stack(help_panel, 14)
	_campaign_label(stack, "help_title", 30, Color("f6d184")); help_body = _campaign_label(stack, "help_body", 18, Color("d9e4e5"))
	campaign_buttons["help_close"] = _campaign_button(stack, "help_close", _help_pressed, true); help_panel.visible = false

func _panel(parent: Node, at: Vector2, size: Vector2) -> PanelContainer:
	var panel := PanelContainer.new(); panel.position = at; panel.custom_minimum_size = size; panel.size = size
	var style := StyleBoxFlat.new(); style.bg_color = Color(0.035, 0.07, 0.09, 0.97); style.border_color = Color("52666b")
	style.set_border_width_all(1); style.set_corner_radius_all(5); style.content_margin_left = 22; style.content_margin_right = 22; style.content_margin_top = 16; style.content_margin_bottom = 16
	panel.add_theme_stylebox_override("panel", style); parent.add_child(panel); return panel

func _stack(parent: Node, gap: int) -> VBoxContainer:
	var stack := VBoxContainer.new(); stack.add_theme_constant_override("separation", gap); parent.add_child(stack); return stack

func _rule(parent: Node) -> void:
	var rule := HSeparator.new(); rule.modulate = Color("6f8588"); parent.add_child(rule)

func _label(parent: Node, text: String, size: int, color: Color) -> Label:
	var item := Label.new(); item.text = text; item.add_theme_font_size_override("font_size", size); item.add_theme_color_override("font_color", color); parent.add_child(item); return item

func _copy_label(parent: Node, key: String, size: int, color: Color) -> Label:
	var item := _label(parent, Copy.get_text(key), size, color); copy_labels[item] = key; return item

func _campaign_label(parent: Node, key: String, size: int, color: Color) -> Label:
	var item := _label(parent, CampaignCopy.get_text(key), size, color); campaign_labels[item] = key; return item

func _button(parent: Node, key: String, callback: Callable, primary: bool = false) -> Button:
	var button := Button.new(); button.text = Copy.get_text(key); _style_button(button, primary); button.pressed.connect(callback); parent.add_child(button); return button

func _campaign_button(parent: Node, key: String, callback: Callable, primary: bool = false) -> Button:
	var button := Button.new(); button.text = CampaignCopy.get_text(key); _style_button(button, primary); button.pressed.connect(callback); parent.add_child(button); campaign_buttons[button] = key; return button

func _style_button(button: Button, primary: bool) -> void:
	button.custom_minimum_size.y = 40; button.focus_mode = Control.FOCUS_ALL
	var normal := StyleBoxFlat.new(); normal.bg_color = Color("edb958") if primary else Color("233b49"); normal.set_corner_radius_all(4); normal.content_margin_left = 14; normal.content_margin_right = 14
	var hover: StyleBoxFlat = normal.duplicate(); hover.bg_color = normal.bg_color.lightened(0.14)
	var focus: StyleBoxFlat = normal.duplicate(); focus.border_color = Color("fff2bf"); focus.set_border_width_all(2)
	button.add_theme_stylebox_override("normal", normal); button.add_theme_stylebox_override("hover", hover); button.add_theme_stylebox_override("pressed", hover); button.add_theme_stylebox_override("focus", focus)
	button.add_theme_color_override("font_color", Color("172b36") if primary else Color("eff0e9")); button.add_theme_color_override("font_hover_color", Color("172b36") if primary else Color.WHITE)

func toggle_help() -> void:
	help_open = not help_open; help_panel.visible = help_open

func _help_pressed() -> void:
	command.emit("help", "")

func _toggle_language() -> void:
	Copy.toggle(); Preferences.save_settings(); _apply_copy()

func _toggle_sound() -> void:
	Cues.muted = not Cues.muted; Preferences.save_settings(); _apply_copy()

func _toggle_sensitivity() -> void:
	Preferences.sensitivity += 0.25
	if Preferences.sensitivity > 2: Preferences.sensitivity = 0.5
	Preferences.save_settings(); _apply_copy()

func _toggle_volume() -> void:
	Preferences.volume = snappedf(Preferences.volume - 0.2, 0.2)
	if Preferences.volume < 0: Preferences.volume = 1.0
	Preferences.apply_settings(); Preferences.save_settings(); _apply_copy()

func _toggle_fov() -> void:
	Preferences.fov += 5
	if Preferences.fov > 90: Preferences.fov = 60
	Preferences.save_settings(); _apply_copy()

func _toggle_fullscreen() -> void:
	Preferences.fullscreen = not Preferences.fullscreen; Preferences.apply_settings(); Preferences.save_settings(); _apply_copy()

func _apply_copy() -> void:
	if sensitivity_button: sensitivity_button.text = Copy.get_text("sensitivity") % Preferences.sensitivity
	for item in copy_labels: item.text = Copy.get_text(copy_labels[item])
	for item in campaign_labels: item.text = CampaignCopy.get_text(campaign_labels[item])
	for key in menu_buttons:
		if key in ["campaign", "host", "practice", "help", "pause_help", "quit"]: continue
		menu_buttons[key].text = Copy.get_text("leave" if key == "pause_leave" else key)
	for button in campaign_buttons:
		if button is Button: button.text = CampaignCopy.get_text(campaign_buttons[button])
	language_button.text = "English / 한국어"; menu_note.text = Copy.get_text("menu_note"); controls.text = CampaignCopy.get_text("help")
	for button in sound_buttons: button.text = Copy.get_text("sound_off" if Cues.muted else "sound_on")
	volume_button.text = CampaignCopy.get_text("volume") % roundi(Preferences.volume * 100)
	fov_button.text = CampaignCopy.get_text("fov") % roundi(Preferences.fov)
	fullscreen_button.text = CampaignCopy.get_text("fullscreen_on" if Preferences.fullscreen else "fullscreen_off")

func render(state: Dictionary) -> void:
	var phase: String = state.get("phase", "menu"); var campaign: Dictionary = state.get("campaign", {})
	var campaign_enabled := bool(campaign.get("enabled", false)); menu.visible = phase == "menu"; menu_note.visible = menu.visible; hud.visible = not menu.visible
	pause.visible = paused and phase == "playing"; menu_status.text = Copy.get_text(state.get("notice", "")) if not str(state.get("notice", "")).is_empty() else ""
	phase_label.text = Copy.get_text("ready" if phase == "waiting" and int(state.get("count", 0)) >= 2 else phase)
	score_label.text = Copy.get_text("score") % [state.get("score", 0), state.get("quota", 0)]
	var seconds := ceili(float(state.get("time", 0))); timer_label.text = "%02d:%02d" % [seconds / 60, seconds % 60]
	crew_label.text = Copy.get_text("solo") if state.get("mode", "practice") == "practice" else Copy.get_text("crew") % state.get("count", 0)
	campaign_strip.visible = campaign_enabled; campaign_strip.text = (CampaignCopy.get_text("contract") % (int(campaign.get("stage", 0)) + 1)) + "\n" + (CampaignCopy.get_text("bank") % int(campaign.get("credits", 0)))
	prompt.text = Copy.get_text(state.get("prompt", "far")); var cargo_name := str(state.get("cargo_name", ""))
	if not cargo_name.is_empty(): prompt.text = cargo_name + "  /  " + prompt.text
	notice.text = Copy.get_text(state.get("notice", "")) if not str(state.get("notice", "")).is_empty() else ""
	if not str(state.get("warning", "")).is_empty(): notice.text = state.warning
	progress_label.text = str(state.get("progress", "")); progress_label.visible = not progress_label.text.is_empty()
	var creature_key := str(state.get("creature", "")); creature_label.text = CampaignCopy.get_text(creature_key); creature_label.visible = campaign_enabled and phase == "playing" and not creature_label.text.is_empty()
	var horn_left := float(state.get("horn_left", -1.0)); horn_label.visible = campaign_enabled and phase == "playing" and int(campaign.get("stage", 0)) >= 1 and horn_left >= 0
	horn_label.text = CampaignCopy.get_text("horn_ready") if horn_left <= 0 else CampaignCopy.get_text("horn_wait") % horn_left
	help_panel.visible = help_open or bool(state.get("help", false)); help_panel.move_to_front()
	action_button.text = Copy.get_text("restart" if phase in ["won", "lost", "bonus_done"] else "start")
	action_button.visible = phase in ["waiting", "won", "lost", "bonus_done"] and (not campaign_enabled or phase in ["waiting", "lost"] or bool(campaign.get("finished", false)))
	action_button.disabled = state.get("mode", "") == "guest" or (state.get("mode", "") == "host" and int(state.get("count", 0)) < 2)
	result_panel.visible = phase in ["won", "lost", "bonus_done"]
	result_label.visible = not campaign_enabled; overtime_button.visible = not campaign_enabled and phase == "won" and not bool(state.get("bonus", false))
	result_label.text = Copy.get_text("result") % [state.get("score", 0), state.get("quota", 0), ceili(float(state.get("elapsed", 0))), state.get("seed", 0)]
	if bool(state.get("base_won", false)): result_label.text += "\n" + Copy.get_text("base_secured")
	overtime_button.disabled = bool(state.get("voted", false)); overtime_button.text = Copy.get_text("overtime_votes") % [state.get("votes", 0), state.get("count", 0)]
	if phase == "playing" and bool(state.get("bonus", false)): phase_label.text = Copy.get_text("bonus_playing")
	_render_campaign_result(campaign, phase, str(state.get("mode", "")), int(state.get("count", 0)))

func _render_campaign_result(campaign: Dictionary, phase: String, mode: String, count: int) -> void:
	var enabled := bool(campaign.get("enabled", false)); campaign_result.visible = enabled
	if not enabled: return
	var success := phase in ["won", "bonus_done"]; var finished := bool(campaign.get("finished", false)); var credits := int(campaign.get("credits", 0))
	campaign_title.text = CampaignCopy.get_text("run_clear" if finished else ("contract_clear" if success else "contract_failed"))
	campaign_bank.text = (CampaignCopy.get_text("bank") % credits) + ("  ·  " + CampaignCopy.get_text("earned") % int(campaign.get("earned", 0)) if success else "")
	campaign_stats.text = (CampaignCopy.get_text("run_progress") % [campaign.get("deliveries", 0), campaign.get("relays", 0)]) + "\n" + (CampaignCopy.get_text("record") % campaign.get("best_runs", 0))
	var can_buy := success and not finished and mode != "guest"
	var boots := int(campaign.get("boots", 0)); var time_upgrade := int(campaign.get("time_upgrade", 0)); var horn := int(campaign.get("horn", 0))
	_set_upgrade("buy_boots", CampaignCopy.get_text("buy_boots") % boots, not can_buy or credits < 20 or boots >= 2)
	_set_upgrade("buy_time", CampaignCopy.get_text("buy_time") % time_upgrade, not can_buy or credits < 25 or time_upgrade >= 2)
	_set_upgrade("buy_horn", CampaignCopy.get_text("buy_horn") % horn, not can_buy or credits < 20 or horn >= 1)
	var next: Button = campaign_buttons["next_contract"]; next.visible = success and not finished
	next.text = CampaignCopy.get_text("ready_wait") if bool(campaign.get("voted", false)) else CampaignCopy.get_text("next_contract") % [campaign.get("ready", 0), count]
	next.disabled = bool(campaign.get("voted", false))

func _set_upgrade(key: String, text: String, disabled: bool) -> void:
	var button: Button = campaign_buttons[key]; button.text = text; button.disabled = disabled

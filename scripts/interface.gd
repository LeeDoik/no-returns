extends CanvasLayer

signal command(action: String, address: String)

const Release = preload("res://scripts/release_copy.gd")
const Settings = preload("res://scripts/settings_panel.gd")
const Preferences = preload("res://scripts/preferences.gd")
const Copy = preload("res://scripts/copy.gd")
const CampaignCopy = preload("res://scripts/campaign_copy.gd")
const Cues = preload("res://scripts/sneeze_cues.gd")
var canvas: Control
var settings: PanelContainer
var settings_open := false
var confirm_action := ""
var confirm_panel: PanelContainer
var confirm_title: Label
var confirm_note: Label
var confirm_yes: Button
var confirm_no: Button
var shade: ColorRect
var focus_layer := ""
var last_state := {}
var pause_note: Label
var utility_panels: Array[Control] = []
var actions_panel: PanelContainer
var release_labels := {}
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
	process_mode = Node.PROCESS_MODE_ALWAYS
	var root := Control.new(); root.set_anchors_and_offsets_preset(Control.PRESET_FULL_RECT); root.mouse_filter = Control.MOUSE_FILTER_IGNORE
	var theme := Theme.new(); var font := SystemFont.new(); font.font_names = PackedStringArray(["Malgun Gothic", "Segoe UI"])
	theme.default_font = font; theme.default_font_size = 18; root.theme = theme; add_child(root)
	canvas = root
	_build_menu(root); _build_hud(root)
	shade = ColorRect.new(); shade.color = Color(0.01,0.02,0.02,0.72); shade.size = Vector2(1280,800); root.add_child(shade); shade.visible = false
	_build_pause(root); _build_help(root)
	settings = Settings.new(); root.add_child(settings); settings.visible = false
	settings.closed.connect(func(): command.emit("settings_close", ""))
	settings.changed.connect(_apply_copy)
	_build_confirmation(root); _apply_copy()
	get_viewport().size_changed.connect(_fit_canvas); _fit_canvas()

func _build_menu(root: Control) -> void:
	menu = _panel(root, Vector2(54, 46), Vector2(498, 708)); var stack := _stack(menu, 10)
	_campaign_label(stack, "edition", 14, Color("e5b75b")); _label(stack, "NO\nRETURNS", 48, Color("f8f0db"))
	_campaign_label(stack, "menu_intro", 18, Color("d8e3e4")); _rule(stack)
	menu_buttons["campaign"] = _campaign_button(stack, "campaign", func(): command.emit("campaign", ""), true)
	menu_buttons["host"] = _campaign_button(stack, "host_campaign", func(): command.emit("host_campaign", ""))
	menu_buttons["practice"] = _campaign_button(stack, "practice_short", func(): command.emit("practice", ""))
	_release_button(stack,"settings",func(): command.emit("settings", ""))
	menu_buttons["help"] = _campaign_button(stack,"help",_help_pressed)
	language_button = _button(stack,"",_toggle_language)
	_campaign_button(stack,"quit",func(): command.emit("quit", ""))
	_copy_label(stack,"development",12,Color("9fb3bc"))
	var terminal := _panel(root,Vector2(600,370),Vector2(626,384)); utility_panels.append(terminal)
	var connection := _stack(terminal,12)
	_release_label(connection,"crew_terminal",22); _release_label(connection,"join_hint",17)
	_copy_label(connection,"address",14,Color("bdc9c2"))
	var row := HBoxContainer.new(); row.add_theme_constant_override("separation",8); connection.add_child(row)
	address = LineEdit.new(); address.text = "127.0.0.1"; address.max_length = 253; address.custom_minimum_size = Vector2(270,44); address.size_flags_horizontal = Control.SIZE_EXPAND_FILL; row.add_child(address)
	address.text_submitted.connect(func(_value: String): command.emit("join",address.text))
	menu_buttons["join"] = _button(row,"join",func(): command.emit("join",address.text))
	menu_status = _label(connection,"",15,Color("ffbd91")); menu_status.autowrap_mode = TextServer.AUTOWRAP_WORD_SMART
	var brief := _panel(root,Vector2(600,46),Vector2(626,268)); utility_panels.append(brief)
	var brief_stack := _stack(brief,18); _release_label(brief_stack,"goal",23); _release_label(brief_stack,"goal_body",20)
	menu_note = _label(root,"",14,Color("ffedc4")); menu_note.visible = false

func _build_hud(root: Control) -> void:
	hud = Control.new(); hud.set_anchors_and_offsets_preset(Control.PRESET_FULL_RECT); hud.mouse_filter = Control.MOUSE_FILTER_IGNORE; root.add_child(hud)
	var top := _panel(hud, Vector2(24, 20), Vector2(1232, 84)); var top_row := HBoxContainer.new(); top_row.add_theme_constant_override("separation", 24); top.add_child(top_row)
	var heading := VBoxContainer.new(); heading.size_flags_horizontal = Control.SIZE_EXPAND_FILL; top_row.add_child(heading)
	_copy_label(heading, "prototype", 12, Color("c6ab74")); phase_label = _label(heading, "", 21, Color("f7efdc"))
	campaign_strip = _label(top_row, "", 15, Color("afcbd0")); score_label = _label(top_row, "", 22, Color("81d9c4")); timer_label = _label(top_row, "", 27, Color("f6d184")); crew_label = _label(top_row, "", 15, Color("bacbd3"))
	var bottom := _panel(hud, Vector2(24, 658), Vector2(910, 118)); var lower := _stack(bottom, 4)
	prompt = _label(lower, "", 17, Color("f6d184")); notice = _label(lower, "", 14, Color("85d9c8")); progress_label = _label(lower, "", 13, Color("b7c9cf")); controls = _label(lower, "", 13, Color("94aab3"))
	var actions := _panel(hud, Vector2(958, 658), Vector2(298, 118)); actions_panel = actions; var actions_stack := _stack(actions, 6)
	action_button = _button(actions_stack, "start", func(): command.emit("start", ""), true); menu_buttons["leave"] = _button(actions_stack, "leave", func(): command.emit("leave", ""))
	creature_label = _label(hud, "", 14, Color("ffbd85")); creature_label.position = Vector2(930, 118)
	horn_label = _label(hud, "", 14, Color("f6d184")); horn_label.position = Vector2(1030, 145)
	result_panel = _panel(hud, Vector2(293, 178), Vector2(694, 438)); var result_stack := _scroll_stack(result_panel, 10)
	result_label = _label(result_stack, "", 21, Color("f6d184")); overtime_button = _button(result_stack, "overtime", func(): command.emit("overtime", ""), true)
	campaign_result = _stack(result_stack, 8); campaign_title = _label(campaign_result, "", 27, Color("f6d184")); campaign_bank = _label(campaign_result, "", 20, Color("81d9c4")); campaign_stats = _label(campaign_result, "", 15, Color("bdcdd2"))
	campaign_buttons["buy_boots"] = _campaign_button(campaign_result, "buy_boots", func(): command.emit("buy_boots", ""))
	campaign_buttons["buy_time"] = _campaign_button(campaign_result, "buy_time", func(): command.emit("buy_time", ""))
	campaign_buttons["buy_horn"] = _campaign_button(campaign_result, "buy_horn", func(): command.emit("buy_horn", ""))
	campaign_buttons["next_contract"] = _campaign_button(campaign_result, "next_contract", func(): command.emit("next_contract", ""), true)

func _build_pause(root: Control) -> void:
	pause = _panel(root,Vector2(390,160),Vector2(500,480)); var stack := _stack(pause,16)
	_copy_label(stack,"pause_title",28,Color("f6d184"))
	pause_note = _label(stack,"",17,Color("c7d1ca")); pause_note.autowrap_mode = TextServer.AUTOWRAP_WORD_SMART
	menu_buttons["resume"] = _button(stack,"resume",func(): command.emit("resume",""),true)
	_release_button(stack,"settings",func(): command.emit("settings",""))
	menu_buttons["pause_help"] = _campaign_button(stack,"help",_help_pressed)
	menu_buttons["pause_leave"] = _button(stack,"leave",func(): command.emit("leave",""))
	menu_buttons["quit"] = _campaign_button(stack,"quit",func(): command.emit("quit",""))

func _build_help(root: Control) -> void:
	help_panel = _panel(root, Vector2(260, 70), Vector2(760, 660)); var stack := _scroll_stack(help_panel, 14)
	_campaign_label(stack, "help_title", 30, Color("f6d184")); help_body = _campaign_label(stack, "help_body", 18, Color("d9e4e5")); help_body.autowrap_mode = TextServer.AUTOWRAP_WORD_SMART
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
	button.add_theme_color_override("font_focus_color", Color("172b36") if primary else Color.WHITE)
	var disabled: StyleBoxFlat = normal.duplicate(); disabled.bg_color = Color("1b2a2f")
	button.add_theme_stylebox_override("disabled", disabled); button.add_theme_color_override("font_disabled_color", Color("78868a"))

func toggle_help() -> void:
	help_open = not help_open; help_panel.visible = help_open

func _help_pressed() -> void:
	command.emit("help", "")

func _toggle_language() -> void:
	Copy.toggle(); Preferences.save_settings(); _apply_copy()

func _toggle_sound() -> void:
	Cues.muted = not Cues.muted; Preferences.save_settings(); _apply_copy()

func _apply_copy() -> void:
	for item in copy_labels: item.text = Copy.get_text(copy_labels[item])
	for item in campaign_labels: item.text = CampaignCopy.get_text(campaign_labels[item])
	for key in menu_buttons:
		if key in ["campaign", "host", "practice", "help", "pause_help", "quit"]: continue
		menu_buttons[key].text = Copy.get_text("leave" if key == "pause_leave" else key)
	for button in campaign_buttons:
		if button is Button: button.text = CampaignCopy.get_text(campaign_buttons[button])
	language_button.text = "English / 한국어"; menu_note.text = Copy.get_text("menu_note"); controls.text = CampaignCopy.get_text("help")
	for item in release_labels: item.text = Release.text(release_labels[item])
	if settings: settings.refresh()

func render(state: Dictionary) -> void:
	var phase: String = state.get("phase", "menu"); var campaign: Dictionary = state.get("campaign", {})
	var campaign_enabled := bool(campaign.get("enabled", false)); menu.visible = phase == "menu"; menu_note.visible = false; hud.visible = not menu.visible
	last_state = state
	for panel in utility_panels: panel.visible = menu.visible
	pause.visible = paused and phase == "playing" and not help_open and not settings_open and confirm_action.is_empty(); menu_status.text = Copy.get_text(state.get("notice", "")) if not str(state.get("notice", "")).is_empty() else ""
	phase_label.text = Copy.get_text("ready" if phase == "waiting" and int(state.get("count", 0)) >= 2 else phase)
	score_label.text = Copy.get_text("score") % [state.get("score", 0), state.get("quota", 0)]
	var seconds := ceili(float(state.get("time", 0))); timer_label.text = "%02d:%02d" % [seconds / 60, seconds % 60]
	crew_label.text = (Release.text("solo") if campaign_enabled else Copy.get_text("solo")) if state.get("mode", "practice") == "practice" else Copy.get_text("crew") % state.get("count", 0)
	campaign_strip.visible = campaign_enabled; campaign_strip.text = (CampaignCopy.get_text("contract") % (int(campaign.get("stage", 0)) + 1)) + "\n" + (CampaignCopy.get_text("bank") % int(campaign.get("credits", 0)))
	prompt.text = Copy.get_text(state.get("prompt", "far")); var cargo_name := str(state.get("cargo_name", ""))
	if not cargo_name.is_empty(): prompt.text = cargo_name + "  /  " + prompt.text
	notice.text = Copy.get_text(state.get("notice", "")) if not str(state.get("notice", "")).is_empty() else ""
	if not str(state.get("warning", "")).is_empty(): notice.text = state.warning
	progress_label.text = str(state.get("progress", "")); progress_label.visible = not progress_label.text.is_empty()
	var creature_key := str(state.get("creature", "")); creature_label.text = CampaignCopy.get_text(creature_key); creature_label.visible = campaign_enabled and phase == "playing" and not creature_label.text.is_empty()
	var horn_left := float(state.get("horn_left", -1.0)); horn_label.visible = campaign_enabled and phase == "playing" and int(campaign.get("stage", 0)) >= 1 and horn_left >= 0
	horn_label.text = CampaignCopy.get_text("horn_ready") if horn_left <= 0 else CampaignCopy.get_text("horn_wait") % horn_left
	help_panel.visible = (help_open or bool(state.get("help", false))) and not settings_open and confirm_action.is_empty()
	settings.visible = settings_open and confirm_action.is_empty()
	confirm_panel.visible = not confirm_action.is_empty()
	shade.visible = pause.visible or help_panel.visible or settings.visible or confirm_panel.visible
	pause_note.text = Release.text("solo_pause" if state.get("mode", "") == "practice" else "online_pause")
	confirm_title.text = Release.text("quit_title" if confirm_action == "quit" else "leave_title")
	confirm_note.text = Release.text("host_leave" if state.get("mode", "") == "host" else "leave_note")
	confirm_yes.text = CampaignCopy.get_text("quit") if confirm_action == "quit" else Release.text("confirm")
	confirm_no.text = Release.text("keep_playing")
	actions_panel.visible = phase != "playing"
	menu_buttons["leave"].text = Copy.get_text("cancel" if phase == "connecting" else "leave")
	if phase == "waiting" and state.get("mode", "") == "guest": prompt.text = Release.text("waiting_guest")
	controls.text = Release.text("settings_hint") + "  ·  " + CampaignCopy.get_text("help")
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
	_focus_modal()

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
	button.visible = last_state.get("phase", "") == "won" and not bool(last_state.get("campaign", {}).get("finished", false))

func _fit_canvas() -> void:
	var view := get_viewport().get_visible_rect().size
	var factor := minf(view.x/1280.0,view.y/800.0)
	transform = Transform2D(0,Vector2.ONE*factor,0,(view-Vector2(1280,800)*factor)*0.5)

func _release_label(parent: Node, key: String, font_size: int) -> Label:
	var label := _label(parent,Release.text(key),font_size,Color("dedbcb")); label.autowrap_mode = TextServer.AUTOWRAP_WORD_SMART; release_labels[label] = key; return label

func _release_button(parent: Node, key: String, callback: Callable) -> Button:
	var button := _button(parent,"",callback); release_labels[button] = key; button.text = Release.text(key); return button

func _scroll_stack(panel: PanelContainer, gap: int) -> VBoxContainer:
	var scroll := ScrollContainer.new(); scroll.horizontal_scroll_mode = ScrollContainer.SCROLL_MODE_DISABLED; panel.add_child(scroll)
	var stack := _stack(scroll,gap); stack.size_flags_horizontal = Control.SIZE_EXPAND_FILL; return stack

func _build_confirmation(root: Control) -> void:
	confirm_panel = _panel(root,Vector2(365,240),Vector2(550,320)); var stack := _stack(confirm_panel,20)
	confirm_title = _label(stack,"",26,Color("f6d184"))
	confirm_note = _label(stack,"",18,Color("d5ded5")); confirm_note.autowrap_mode = TextServer.AUTOWRAP_WORD_SMART
	confirm_no = _button(stack,"",func(): command.emit("cancel_exit",""),true)
	confirm_yes = _button(stack,"",func(): command.emit("confirm_exit",""))
	confirm_panel.visible = false

func _focus_modal() -> void:
	var layer := "confirm" if confirm_panel.visible else ("settings" if settings.visible else ("help" if help_panel.visible else ("pause" if pause.visible else ("menu" if menu.visible else ("result" if result_panel.visible else "hud")))) )
	var signature := "%s/%s/%s/%s" % [layer,last_state.get("phase", ""),action_button.disabled,campaign_buttons["next_contract"].disabled]
	if focus_layer == signature: return
	var active: Control = {"confirm":confirm_panel,"settings":settings,"help":help_panel,"pause":pause,"menu":canvas,"hud":hud,"result":hud}[layer]
	for item in canvas.find_children("*","Control",true,false):
		if item is BaseButton or item is LineEdit or item is Range:
			var allowed: bool = item.is_visible_in_tree() and (layer == "menu" or active.is_ancestor_of(item))
			item.focus_mode = Control.FOCUS_ALL if allowed else Control.FOCUS_NONE
	focus_layer = signature
	var target: Control
	match layer:
		"confirm": target = confirm_no
		"settings": target = settings.back
		"help": target = campaign_buttons["help_close"]
		"pause": target = menu_buttons["resume"]
		"menu": target = menu_buttons["campaign"]
		"hud":
			if action_button.visible and not action_button.disabled: target = action_button
		"result":
			if campaign_buttons["next_contract"].is_visible_in_tree() and not campaign_buttons["next_contract"].disabled: target = campaign_buttons["next_contract"]
			elif action_button.visible and not action_button.disabled: target = action_button
			else: target = menu_buttons["leave"]
	if target: target.grab_focus()

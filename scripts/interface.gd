extends CanvasLayer

signal command(action: String, address: String)

const Preferences = preload("res://scripts/preferences.gd")
const Copy = preload("res://scripts/copy.gd")
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
var language_button: Button
var menu_note: Label
var paused := false
var overtime_button: Button
var result_panel: PanelContainer
var result_label: Label
var sensitivity_button: Button
var sound_buttons: Array[Button] = []

func _ready() -> void:
	var root := Control.new()
	root.set_anchors_and_offsets_preset(Control.PRESET_FULL_RECT)
	root.mouse_filter = Control.MOUSE_FILTER_IGNORE
	var theme := Theme.new()
	var font := SystemFont.new()
	font.font_names = PackedStringArray(["Malgun Gothic", "Segoe UI", "Arial"])
	theme.default_font = font
	theme.default_font_size = 18
	root.theme = theme
	add_child(root)
	menu = _panel(root, Vector2(42, 34), Vector2(450, 728))
	var stack := _stack(menu, 9)
	_copy_label(stack, "foundation", 15, Color("e5b75b"))
	_label(stack, "NO\nRETURNS", 50, Color("f8f0db"))
	_copy_label(stack, "subtitle", 17, Color("c5d1d5"))
	_copy_label(stack, "intro", 22, Color("f8f0db"))
	_copy_label(stack, "scope", 13, Color("a9bdc6"))
	menu_buttons["practice"] = _button(stack, "practice", func(): command.emit("practice", ""), true)
	menu_buttons["host"] = _button(stack, "host", func(): command.emit("host", ""))
	_copy_label(stack, "address", 13, Color("a9bdc6"))
	var row := HBoxContainer.new()
	stack.add_child(row)
	address = LineEdit.new()
	address.text = "127.0.0.1"
	address.max_length = 253
	address.custom_minimum_size = Vector2(238, 43)
	row.add_child(address)
	menu_buttons["join"] = _button(row, "join", func(): command.emit("join", address.text))
	menu_status = _label(stack, "", 14, Color("ffbc8d"))
	menu_status.autowrap_mode = TextServer.AUTOWRAP_WORD_SMART
	_copy_label(stack, "development", 13, Color("91a8b5"))
	language_button = _button(stack, "", _toggle_language)
	sound_buttons.append(_button(stack, "", _toggle_sound))
	menu_note = _label(root, "", 24, Color("ffedc4"))
	menu_note.position = Vector2(725, 640)
	hud = Control.new()
	hud.set_anchors_and_offsets_preset(Control.PRESET_FULL_RECT)
	hud.mouse_filter = Control.MOUSE_FILTER_IGNORE
	root.add_child(hud)
	var top := _panel(hud, Vector2(24, 22), Vector2(1232, 94))
	var top_row := HBoxContainer.new()
	top_row.add_theme_constant_override("separation", 28)
	top.add_child(top_row)
	var heading := VBoxContainer.new()
	heading.size_flags_horizontal = Control.SIZE_EXPAND_FILL
	top_row.add_child(heading)
	_copy_label(heading, "prototype", 13, Color("c6ab74"))
	phase_label = _label(heading, "", 23, Color("f7efdc"))
	score_label = _label(top_row, "", 24, Color("81d9c4"))
	timer_label = _label(top_row, "", 29, Color("f6d184"))
	crew_label = _label(top_row, "", 16, Color("bacbd3"))
	var bottom := _panel(hud, Vector2(24, 640), Vector2(910, 136))
	var lower := _stack(bottom, 7)
	prompt = _label(lower, "", 21, Color("f6d184"))
	notice = _label(lower, "", 15, Color("85d9c8"))
	controls = _label(lower, "", 15, Color("c5d1d5"))
	var actions := _panel(hud, Vector2(958, 640), Vector2(298, 136))
	var actions_stack := _stack(actions, 9)
	action_button = _button(actions_stack, "start", func(): command.emit("start", ""), true)
	menu_buttons["leave"] = _button(actions_stack, "leave", func(): command.emit("leave", ""))
	result_panel = _panel(hud, Vector2(325, 240), Vector2(630, 295))
	var result_stack := _stack(result_panel, 14)
	result_label = _label(result_stack, "", 23, Color("f6d184"))
	overtime_button = _button(result_stack, "overtime", func(): command.emit("overtime", ""), true)
	pause = _panel(root, Vector2(425, 195), Vector2(430, 420))
	var pause_stack := _stack(pause, 20)
	_copy_label(pause_stack, "pause_title", 27, Color("f6d184"))
	_copy_label(pause_stack, "pause_note", 14, Color("b6c8ce"))
	menu_buttons["resume"] = _button(pause_stack, "resume", func(): command.emit("resume", ""), true)
	sound_buttons.append(_button(pause_stack, "", _toggle_sound))
	sensitivity_button = _button(pause_stack, "", _toggle_sensitivity)
	menu_buttons["pause_leave"] = _button(pause_stack, "leave", func(): command.emit("leave", ""))
	_apply_copy()

func _panel(parent: Node, at: Vector2, size: Vector2) -> PanelContainer:
	var panel := PanelContainer.new()
	panel.position = at
	panel.custom_minimum_size = size
	panel.size = size
	var style := StyleBoxFlat.new()
	style.bg_color = Color(0.044, 0.080, 0.104, 0.96)
	style.border_color = Color("43555b")
	style.set_border_width_all(1)
	style.set_corner_radius_all(6)
	style.content_margin_left = 22
	style.content_margin_right = 22
	style.content_margin_top = 16
	style.content_margin_bottom = 16
	panel.add_theme_stylebox_override("panel", style)
	parent.add_child(panel)
	return panel

func _stack(parent: Node, gap: int) -> VBoxContainer:
	var stack := VBoxContainer.new()
	stack.add_theme_constant_override("separation", gap)
	parent.add_child(stack)
	return stack

func _label(parent: Node, text: String, size: int, color: Color) -> Label:
	var item := Label.new()
	item.text = text
	item.add_theme_font_size_override("font_size", size)
	item.add_theme_color_override("font_color", color)
	parent.add_child(item)
	return item

func _copy_label(parent: Node, key: String, size: int, color: Color) -> Label:
	var item := _label(parent, Copy.get_text(key), size, color)
	copy_labels[item] = key
	return item

func _button(parent: Node, key: String, callback: Callable, primary: bool = false) -> Button:
	var button := Button.new()
	button.text = Copy.get_text(key)
	button.custom_minimum_size.y = 43
	button.focus_mode = Control.FOCUS_NONE
	var normal := StyleBoxFlat.new()
	normal.bg_color = Color("edb958") if primary else Color("233b49")
	normal.set_corner_radius_all(4)
	normal.content_margin_left = 14
	normal.content_margin_right = 14
	var hover: StyleBoxFlat = normal.duplicate()
	hover.bg_color = normal.bg_color.lightened(0.14)
	button.add_theme_stylebox_override("normal", normal)
	button.add_theme_stylebox_override("hover", hover)
	button.add_theme_stylebox_override("pressed", hover)
	button.add_theme_color_override("font_color", Color("172b36") if primary else Color("eff0e9"))
	button.add_theme_color_override("font_hover_color", Color("172b36") if primary else Color.WHITE)
	button.pressed.connect(callback)
	parent.add_child(button)
	return button

func _toggle_language() -> void:
	Copy.toggle()
	Preferences.save_settings()
	_apply_copy()

func _toggle_sound() -> void:
	Cues.muted = not Cues.muted
	Preferences.save_settings()
	_apply_copy()

func _toggle_sensitivity() -> void:
	Preferences.sensitivity += 0.25
	if Preferences.sensitivity > 2: Preferences.sensitivity = 0.5
	Preferences.save_settings()
	_apply_copy()

func _apply_copy() -> void:
	sensitivity_button.text = Copy.get_text("sensitivity") % Preferences.sensitivity
	for item in copy_labels:
		item.text = Copy.get_text(copy_labels[item])
	for key in menu_buttons:
		menu_buttons[key].text = Copy.get_text("leave" if key == "pause_leave" else key)
	language_button.text = "English / 한국어"
	menu_note.text = Copy.get_text("menu_note")
	controls.text = Copy.get_text("controls")
	for button in sound_buttons:
		button.text = Copy.get_text("sound_off" if Cues.muted else "sound_on")

func render(state: Dictionary) -> void:
	var phase: String = state.phase
	menu.visible = phase == "menu"
	menu_note.visible = menu.visible
	hud.visible = not menu.visible
	pause.visible = paused and phase == "playing"
	menu_status.text = Copy.get_text(state.notice) if not state.notice.is_empty() else ""
	phase_label.text = Copy.get_text("ready" if phase == "waiting" and state.count >= 2 else phase)
	score_label.text = Copy.get_text("score") % [state.score, state.quota]
	var seconds := ceili(state.time)
	timer_label.text = "%02d:%02d" % [seconds / 60, seconds % 60]
	crew_label.text = Copy.get_text("solo") if state.mode == "practice" else Copy.get_text("crew") % state.count
	prompt.text = Copy.get_text(state.prompt)
	if not state.cargo_name.is_empty():
		prompt.text = state.cargo_name + "  /  " + prompt.text
	notice.text = Copy.get_text(state.notice) if not state.notice.is_empty() else ""
	if not state.warning.is_empty():
		notice.text = state.warning
	action_button.text = Copy.get_text("restart" if phase in ["won", "lost", "bonus_done"] else "start")
	action_button.visible = phase in ["waiting", "won", "lost", "bonus_done"]
	action_button.disabled = state.mode == "guest" or (state.mode == "host" and state.count < 2)

	result_panel.visible = phase in ["won", "lost", "bonus_done"]
	result_label.text = Copy.get_text("result") % [state.score, state.quota, ceili(state.elapsed), state.seed]
	if state.base_won: result_label.text += "\n" + Copy.get_text("base_secured")
	overtime_button.visible = phase == "won" and not state.bonus
	overtime_button.disabled = state.voted
	overtime_button.text = Copy.get_text("overtime_votes") % [state.votes, state.count]
	if phase == "playing" and state.bonus: phase_label.text = Copy.get_text("bonus_playing")

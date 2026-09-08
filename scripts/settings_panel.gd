extends PanelContainer

signal closed
signal changed
const Preferences = preload("res://scripts/preferences.gd")
const Bindings = preload("res://scripts/input_bindings.gd")
const Text = preload("res://scripts/release_copy.gd")
var stack: VBoxContainer
var status: Label
var pending := ""
var buttons := {}
var captions := {}
var sliders := {}
var checks := {}
var fps: OptionButton
var back: Button
var refreshing := false

func _ready() -> void:
	custom_minimum_size = Vector2(710,700); size = custom_minimum_size
	position = Vector2(285,50)
	var style := StyleBoxFlat.new(); style.bg_color = Color("152729"); style.border_color = Color("879286"); style.set_border_width_all(1)
	style.content_margin_left = 26; style.content_margin_right = 26; style.content_margin_top = 18; style.content_margin_bottom = 18
	add_theme_stylebox_override("panel",style)
	var outer := VBoxContainer.new(); outer.add_theme_constant_override("separation",12); add_child(outer)
	var title := Label.new(); title.add_theme_font_size_override("font_size",28); outer.add_child(title); captions[title] = "settings"
	var scroll := ScrollContainer.new(); scroll.size_flags_vertical = Control.SIZE_EXPAND_FILL; scroll.horizontal_scroll_mode = ScrollContainer.SCROLL_MODE_DISABLED; outer.add_child(scroll)
	stack = VBoxContainer.new(); stack.size_flags_horizontal = Control.SIZE_EXPAND_FILL; stack.add_theme_constant_override("separation",12); scroll.add_child(stack)
	caption("sound"); slider("volume",0,1,0.05)
	caption("display"); slider("sensitivity",0.5,2,0.05); slider("fov",60,90,1)
	for key in ["cues", "invert", "fullscreen", "vsync"]:
		var item := CheckButton.new(); item.custom_minimum_size.y = 42; stack.add_child(item); checks[key] = item
		item.toggled.connect(func(value: bool):
			if refreshing: return
			Preferences.set_option(key,value); persist())
	var row := HBoxContainer.new(); stack.add_child(row)
	var label := Label.new(); label.size_flags_horizontal = Control.SIZE_EXPAND_FILL; row.add_child(label); captions[label] = "fps"
	fps = OptionButton.new(); fps.custom_minimum_size = Vector2(180,42); row.add_child(fps)
	for cap in [0,60,90,120,144]: fps.add_item(str(cap),cap)
	fps.item_selected.connect(func(index: int): Preferences.frame_cap = fps.get_item_id(index); persist())
	caption("controls"); caption("rebind",15)
	for action in Bindings.DEFAULTS:
		var button := Button.new(); button.custom_minimum_size.y = 42; button.alignment = HORIZONTAL_ALIGNMENT_LEFT; stack.add_child(button); buttons[action] = button
		button.pressed.connect(func(): pending = action; status.text = Text.text("listening"))
	var reset := Button.new(); reset.custom_minimum_size.y = 42; stack.add_child(reset); captions[reset] = "reset"
	reset.pressed.connect(func(): Bindings.restore({}); persist())
	status = Label.new(); status.autowrap_mode = TextServer.AUTOWRAP_WORD_SMART; status.custom_minimum_size.y = 44; status.add_theme_font_size_override("font_size",15); outer.add_child(status)
	back = Button.new(); back.custom_minimum_size.y = 44; outer.add_child(back); captions[back] = "close"
	back.pressed.connect(func(): pending = ""; closed.emit())
	refresh()

func caption(key: String, font_size: int = 20) -> void:
	var label := Label.new(); label.autowrap_mode = TextServer.AUTOWRAP_WORD_SMART; label.add_theme_font_size_override("font_size",font_size); stack.add_child(label); captions[label] = key

func slider(key: String, low: float, high: float, step: float) -> void:
	var row := HBoxContainer.new(); row.add_theme_constant_override("separation",12); stack.add_child(row)
	var label := Label.new(); label.custom_minimum_size.x = 225; row.add_child(label)
	var item := HSlider.new(); item.min_value = low; item.max_value = high; item.step = step; item.custom_minimum_size = Vector2(220,42); item.size_flags_horizontal = Control.SIZE_EXPAND_FILL; row.add_child(item)
	sliders[key] = [item,label]
	item.value_changed.connect(func(value: float):
		if refreshing: return
		Preferences.set_option(key,value); persist())

func persist() -> void:
	Preferences.apply_settings(); Preferences.save_settings(); refresh(); changed.emit()

func refresh() -> void:
	refreshing = true
	for item in captions: item.text = Text.text(captions[item])
	for key in sliders:
		var value: float = Preferences.option(key)
		sliders[key][0].set_value_no_signal(value)
		sliders[key][1].text = Text.text(key) + "  " + ("%d%%" % roundi(value*100) if key == "volume" else ("%.2fx" % value if key == "sensitivity" else "%d°" % roundi(value)))
	for key in checks:
		checks[key].text = Text.text(key); checks[key].set_pressed_no_signal(bool(Preferences.option(key)))
	fps.set_item_text(0,Text.text("unlimited")); fps.select(fps.get_item_index(Preferences.frame_cap))
	for action in buttons: buttons[action].text = Text.text(action) + "    [ " + Bindings.label(action) + " ]"
	status.text = Text.text("saved" if Preferences.save_error == OK else "save_failed")
	refreshing = false

func _input(event: InputEvent) -> void:
	if not visible or pending.is_empty(): return
	if event is InputEventKey and event.pressed and not event.echo:
		if event.keycode == KEY_ESCAPE:
			pending = ""; refresh()
		else:
			var code: int = event.physical_keycode if event.physical_keycode else event.keycode
			assign(code)
	elif event is InputEventMouseButton and event.pressed:
		assign(-event.button_index)
	else: return
	get_viewport().set_input_as_handled()

func assign(code: int) -> void:
	var result := Bindings.bind(pending,code)
	if result.is_empty(): pending = ""; persist()
	else: status.text = Text.text(result)

extends RefCounted

const DEFAULTS := {"forward":KEY_W, "back":KEY_S, "left":KEY_A, "right":KEY_D, "jump":KEY_SPACE, "interact":KEY_E, "throw":-MOUSE_BUTTON_LEFT, "ping":KEY_Q, "lever":KEY_F, "horn":KEY_R, "help":KEY_H}
static var keys: Dictionary = DEFAULTS.duplicate()

static func configure() -> void:
	for action in keys:
		if not InputMap.has_action(action): InputMap.add_action(action)
		InputMap.action_erase_events(action)
		var code := int(keys[action])
		var event: InputEvent
		if code < 0:
			event = InputEventMouseButton.new(); event.button_index = -code
		else:
			event = InputEventKey.new(); event.physical_keycode = code
		InputMap.action_add_event(action, event)
	if not InputMap.has_action("menu"):
		InputMap.add_action("menu")
		var escape := InputEventKey.new(); escape.physical_keycode = KEY_ESCAPE
		InputMap.action_add_event("menu", escape)

static func valid(code: int) -> bool:
	return code in [-1, -2, -3] or (code > 0 and code not in [KEY_ESCAPE, KEY_TAB, KEY_ENTER, KEY_KP_ENTER, KEY_SHIFT, KEY_CTRL, KEY_ALT, KEY_META] and not OS.get_keycode_string(code).is_empty())

static func bind(action: String, code: int) -> String:
	if not DEFAULTS.has(action) or not valid(code): return "reserved"
	for other in keys:
		if other != action and keys[other] == code: return "conflict"
	keys[action] = code
	configure()
	return ""

static func restore(saved: Variant) -> void:
	keys = DEFAULTS.duplicate()
	if saved is Dictionary:
		var candidate: Dictionary = DEFAULTS.duplicate()
		for action in DEFAULTS:
			var value: Variant = saved.get(action, DEFAULTS[action])
			if not value is int or not valid(value):
				configure(); return
			candidate[action] = value
		var unique := {}
		for code in candidate.values(): unique[code] = true
		if unique.size() == candidate.size(): keys = candidate
	configure()

static func label(action: String) -> String:
	var code := int(keys.get(action, 0))
	if code < 0: return { -1:"LMB", -2:"RMB", -3:"MMB" }.get(code, "Mouse")
	return OS.get_keycode_string(code)

static func expand(text: String) -> String:
	for action in DEFAULTS: text = text.replace("{" + action + "}", label(action))
	return text

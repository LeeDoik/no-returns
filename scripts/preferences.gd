extends RefCounted

const Copy = preload("res://scripts/copy.gd")
const Cues = preload("res://scripts/sneeze_cues.gd")
const Bindings = preload("res://scripts/input_bindings.gd")
static var sensitivity := 1.0
static var volume := 0.8
static var fov := 72.0
static var fullscreen := false
static var enabled := true
static var invert := false
static var vsync := true
static var frame_cap := 120
static var save_error := OK

static func load_settings(path: String = "user://preferences.cfg") -> void:
	if not enabled: return
	var config := ConfigFile.new()
	if config.load(path) != OK:
		apply_settings()
		return
	var language: String = str(config.get_value("preferences", "language", "ko"))
	Copy.language = language if language in ["ko", "en"] else "ko"
	Cues.muted = bool(config.get_value("preferences", "muted", false))
	sensitivity = _number(config,"sensitivity",1.0,0.5,2.0)
	volume = _number(config,"volume",0.8,0.0,1.0)
	fov = _number(config,"fov",72.0,60.0,90.0)
	fullscreen = bool(config.get_value("preferences", "fullscreen", false))
	invert = bool(config.get_value("preferences", "invert", false))
	vsync = bool(config.get_value("preferences", "vsync", true))
	var cap: Variant = config.get_value("preferences", "frame_cap", 120)
	frame_cap = cap if cap is int and cap in [0,60,90,120,144] else 120
	Bindings.restore(config.get_value("preferences", "bindings", {}))
	apply_settings()

static func save_settings(path: String = "user://preferences.cfg") -> void:
	if not enabled: return
	var config := ConfigFile.new()
	config.set_value("preferences", "language", Copy.language)
	config.set_value("preferences", "muted", Cues.muted)
	config.set_value("preferences", "sensitivity", sensitivity)
	config.set_value("preferences", "volume", volume)
	config.set_value("preferences", "fov", fov)
	config.set_value("preferences", "fullscreen", fullscreen)
	config.set_value("preferences", "invert", invert)
	config.set_value("preferences", "vsync", vsync)
	config.set_value("preferences", "frame_cap", frame_cap)
	config.set_value("preferences", "bindings", Bindings.keys)
	save_error = config.save(path)

static func _number(config: ConfigFile, key: String, fallback: float, low: float, high: float) -> float:
	var value: Variant = config.get_value("preferences",key,fallback)
	if not (value is float or value is int) or not is_finite(float(value)): return fallback
	return clampf(float(value),low,high)

static func apply_settings() -> void:
	var master := AudioServer.get_bus_index("Master")
	if master >= 0:
		AudioServer.set_bus_volume_db(master, linear_to_db(maxf(volume, 0.0001)))
		AudioServer.set_bus_mute(master, volume <= 0.0)
	if DisplayServer.get_name() != "headless":
		DisplayServer.window_set_mode(DisplayServer.WINDOW_MODE_FULLSCREEN if fullscreen else DisplayServer.WINDOW_MODE_WINDOWED)
		DisplayServer.window_set_vsync_mode(DisplayServer.VSYNC_ENABLED if vsync else DisplayServer.VSYNC_DISABLED)
	Engine.max_fps = frame_cap

static func option(key: String) -> Variant:
	return {"sensitivity":sensitivity,"volume":volume,"fov":fov,"invert":invert,"fullscreen":fullscreen,"vsync":vsync,"cues":not Cues.muted}.get(key)

static func set_option(key: String, value: Variant) -> void:
	match key:
		"sensitivity": sensitivity = float(value)
		"volume": volume = float(value)
		"fov": fov = float(value)
		"invert": invert = bool(value)
		"fullscreen": fullscreen = bool(value)
		"vsync": vsync = bool(value)
		"cues": Cues.muted = not bool(value)

extends RefCounted

const Copy = preload("res://scripts/copy.gd")
const Cues = preload("res://scripts/sneeze_cues.gd")
static var sensitivity := 1.0
static var volume := 0.8
static var fov := 72.0
static var fullscreen := false
static var enabled := true

static func load_settings(path: String = "user://preferences.cfg") -> void:
	if not enabled: return
	var config := ConfigFile.new()
	if config.load(path) != OK:
		apply_settings()
		return
	var language: String = str(config.get_value("preferences", "language", "ko"))
	Copy.language = language if language in ["ko", "en"] else "ko"
	Cues.muted = bool(config.get_value("preferences", "muted", false))
	var value := float(config.get_value("preferences", "sensitivity", 1.0))
	sensitivity = clampf(value, 0.5, 2.0) if is_finite(value) else 1.0
	value = float(config.get_value("preferences", "volume", 0.8))
	volume = clampf(value, 0.0, 1.0) if is_finite(value) else 0.8
	value = float(config.get_value("preferences", "fov", 72.0))
	fov = clampf(value, 60.0, 90.0) if is_finite(value) else 72.0
	fullscreen = bool(config.get_value("preferences", "fullscreen", false))
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
	config.save(path)

static func apply_settings() -> void:
	var master := AudioServer.get_bus_index("Master")
	if master >= 0:
		AudioServer.set_bus_volume_db(master, linear_to_db(maxf(volume, 0.0001)))
		AudioServer.set_bus_mute(master, volume <= 0.0)
	if DisplayServer.get_name() != "headless":
		DisplayServer.window_set_mode(DisplayServer.WINDOW_MODE_EXCLUSIVE_FULLSCREEN if fullscreen else DisplayServer.WINDOW_MODE_WINDOWED)

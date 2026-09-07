extends RefCounted

const Copy = preload("res://scripts/copy.gd")
const Cues = preload("res://scripts/sneeze_cues.gd")
static var sensitivity := 1.0
static var enabled := true

static func load_settings(path: String = "user://preferences.cfg") -> void:
	if not enabled: return
	var config := ConfigFile.new()
	if config.load(path) != OK: return
	var language: String = str(config.get_value("preferences", "language", "ko"))
	Copy.language = language if language in ["ko", "en"] else "ko"
	Cues.muted = bool(config.get_value("preferences", "muted", false))
	var value := float(config.get_value("preferences", "sensitivity", 1.0))
	sensitivity = clampf(value, 0.5, 2.0) if is_finite(value) else 1.0

static func save_settings(path: String = "user://preferences.cfg") -> void:
	if not enabled: return
	var config := ConfigFile.new()
	config.set_value("preferences", "language", Copy.language)
	config.set_value("preferences", "muted", Cues.muted)
	config.set_value("preferences", "sensitivity", sensitivity)
	config.save(path)

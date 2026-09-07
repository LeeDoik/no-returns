extends SceneTree

const Preferences = preload("res://scripts/preferences.gd")
const Copy = preload("res://scripts/copy.gd")
const Cues = preload("res://scripts/sneeze_cues.gd")
func _initialize() -> void:
	var path := "res://artifacts/preferences-test.cfg"
	Preferences.enabled = true
	Copy.language = "en"
	Cues.muted = true
	Preferences.sensitivity = 1.75
	Preferences.volume = 0.35
	Preferences.fov = 83.0
	Preferences.fullscreen = true
	Preferences.save_settings(path)
	Copy.language = "ko"
	Cues.muted = false
	Preferences.sensitivity = 1
	Preferences.volume = 1.0
	Preferences.fov = 72.0
	Preferences.fullscreen = false
	Preferences.load_settings(path)
	if Copy.language != "en" or not Cues.muted or Preferences.sensitivity != 1.75 or not is_equal_approx(Preferences.volume, 0.35) or not is_equal_approx(Preferences.fov, 83.0) or not Preferences.fullscreen:
		push_error("Saved preferences did not reload")
		quit(1)
		return
	var config := ConfigFile.new()
	config.set_value("preferences", "language", "invalid")
	config.set_value("preferences", "sensitivity", INF)
	config.set_value("preferences", "volume", -4.0)
	config.set_value("preferences", "fov", 120.0)
	config.save(path)
	Preferences.load_settings(path)
	if Copy.language != "ko" or Preferences.sensitivity != 1 or Preferences.volume != 0.0 or Preferences.fov != 90.0:
		push_error("Invalid preference fallback failed")
		quit(1)
		return
	Preferences.volume = 0.8
	AudioServer.set_bus_volume_db(0, 0.0)
	Preferences.load_settings("res://artifacts/no-first-launch-config.cfg")
	if not is_equal_approx(db_to_linear(AudioServer.get_bus_volume_db(0)), 0.8):
		push_error("First launch volume was not applied")
		quit(1)
		return
	print("PREFERENCES PASS")
	quit(0)

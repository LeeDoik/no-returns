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
	Preferences.save_settings(path)
	Copy.language = "ko"
	Cues.muted = false
	Preferences.sensitivity = 1
	Preferences.load_settings(path)
	if Copy.language != "en" or not Cues.muted or Preferences.sensitivity != 1.75:
		push_error("Saved preferences did not reload")
		quit(1)
		return
	var config := ConfigFile.new()
	config.set_value("preferences", "language", "invalid")
	config.set_value("preferences", "sensitivity", INF)
	config.save(path)
	Preferences.load_settings(path)
	if Copy.language != "ko" or Preferences.sensitivity != 1:
		push_error("Invalid preference fallback failed")
		quit(1)
		return
	print("PREFERENCES PASS")
	quit(0)

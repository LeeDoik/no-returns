extends SceneTree
const Bindings = preload("res://scripts/input_bindings.gd")
const Preferences = preload("res://scripts/preferences.gd")
const Copy = preload("res://scripts/copy.gd")
const Profile = preload("res://scripts/run_profile.gd")
const Contracts = preload("res://scripts/contracts.gd")
var failures := 0
func check(value: bool, message: String) -> void:
	if not value: failures += 1; push_error(message)
func _initialize() -> void:
	Bindings.restore({})
	check(Bindings.bind("interact",KEY_Z).is_empty(),"Can rebind interaction")
	check(Bindings.bind("throw",KEY_Z) == "conflict","Duplicate controls rejected")
	check(Bindings.bind("throw",KEY_ESCAPE) == "reserved","Escape stays available")
	Copy.language = "en"
	check(Copy.get_text("near").begins_with("Z "),"Prompt must use rebound key")
	Preferences.enabled = true; Preferences.invert = true; Preferences.frame_cap = 90; Preferences.vsync = false
	Preferences.save_settings("res://artifacts/release-controls.cfg")
	Bindings.restore({}); Preferences.invert = false; Preferences.frame_cap = 120; Preferences.vsync = true
	Preferences.load_settings("res://artifacts/release-controls.cfg")
	check(Bindings.keys.interact == KEY_Z and Preferences.invert and Preferences.frame_cap == 90 and not Preferences.vsync,"Extended settings survive reload")
	Bindings.restore({"interact":KEY_W})
	check(Bindings.keys.interact == KEY_E,"Invalid duplicate config falls back safely")
	var profile := Profile.new()
	var run := Contracts.new(); run.begin(); run.finished = true; run.total_deliveries = 12
	# A missing directory creates a deterministic write failure without touching user data.
	profile.path = "res://artifacts/no-such-record-folder/run.cfg"
	profile.complete(run)
	check(profile.save_error != OK,"Record write failure observable")
	profile.path = "res://artifacts/release-record.cfg"
	profile.complete(run)
	var reloaded := Profile.new(); reloaded.path = profile.path; reloaded.load_record()
	check(reloaded.runs == 1 and reloaded.best_deliveries == 12,"Retry same completion after write failure must save exactly once")
	var bad := ConfigFile.new()
	bad.set_value("preferences","sensitivity",{})
	bad.set_value("preferences","fov",Vector2.ONE)
	bad.set_value("preferences","volume",[])
	bad.save("res://artifacts/release-invalid.cfg")
	Preferences.sensitivity = 1.9; Preferences.fov = 85.0; Preferences.volume = 0.3
	Preferences.load_settings("res://artifacts/release-invalid.cfg")
	check(Preferences.sensitivity == 1.0 and Preferences.fov == 72.0 and Preferences.volume == 0.8,"Damaged preference types fall back without script errors")
	print("RELEASE CONTROLS %s" % ("PASS" if failures == 0 else "FAIL"))
	quit(0 if failures == 0 else 1)

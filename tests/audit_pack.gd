extends SceneTree

func _initialize() -> void:
	var path := ProjectSettings.globalize_path("res://build/NO_RETURNS_0.7/NO_RETURNS.pck")
	if not ProjectSettings.load_resource_pack(path):
		push_error("Cannot mount exported pack")
		quit(1)
		return
	for script in ["main", "cargo", "cargo_rules", "session", "depot", "worker", "interface", "copy", "clinger", "hopper", "round_rules", "pings", "preferences", "sneeze_rules", "sneeze_cues", "depot_layout", "conveyor", "contracts", "run_profile", "feedback", "packrat", "campaign_copy", "shrine_decor"]:
		if not FileAccess.file_exists("res://scripts/%s.gdc" % script):
			push_error("Missing packed script: " + script)
			quit(1)
			return
	for script in ["editable_map", "editable_block", "map_sign", "map_zone", "route_challenges", "bevel_mesh"]:
		if not FileAccess.file_exists("res://scripts/%s.gdc" % script):
			push_error("Missing packed editor runtime: " + script)
			quit(1)
			return
	var scene_path := "res://scenes/maps/shipping_shrine.tscn"
	var remap := ConfigFile.new()
	if remap.load(scene_path + ".remap") != OK:
		push_error("Saved map missing from exported pack")
		quit(1)
		return
	var scene = load(scene_path)
	if not scene is PackedScene:
		quit(1)
		return
	var map = scene.instantiate()
	var identity: String = map.map_fingerprint()
	if identity.length() != 64 or not FileAccess.file_exists(str(remap.get_value("remap", "path", ""))):
		push_error("Exported map identity failed")
		quit(1)
		return
	map.free()
	print("PACK AUDIT PASS: 29 compiled gameplay scripts and saved map identity available: " + identity)
	quit(0)

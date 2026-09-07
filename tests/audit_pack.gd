extends SceneTree

func _initialize() -> void:
	var path := ProjectSettings.globalize_path("res://build/NO_RETURNS_0.7/NO_RETURNS.pck")
	if not ProjectSettings.load_resource_pack(path):
		push_error("Cannot mount exported pack")
		quit(1)
		return
	for script in ["main", "cargo", "cargo_rules", "session", "depot", "worker", "interface", "copy", "clinger", "hopper", "round_rules", "pings", "preferences", "sneeze_rules", "sneeze_cues", "depot_layout", "conveyor", "contracts", "run_profile", "feedback", "packrat", "campaign_copy"]:
		if not FileAccess.file_exists("res://scripts/%s.gdc" % script):
			push_error("Missing packed script: " + script)
			quit(1)
			return
	print("PACK AUDIT PASS: 22 compiled gameplay scripts available")
	quit(0)

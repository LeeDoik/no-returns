extends RefCounted

const FILE := "user://run_profile.cfg"
var runs := 0
var best_credits := 0
var best_deliveries := 0
var best_relays := 0
var last_id := ""
var path := FILE
var enabled := true
var save_error := OK
var dirty := false

func load_record() -> void:
	if not enabled: return
	var cfg := ConfigFile.new()
	if cfg.load(path) != OK or cfg.get_value("record", "version", 0) != 1: return
	runs = _number(cfg.get_value("record", "runs", 0))
	best_credits = _number(cfg.get_value("record", "best_credits", 0))
	best_deliveries = _number(cfg.get_value("record", "best_deliveries", 0))
	best_relays = _number(cfg.get_value("record", "best_relays", 0))
	last_id = str(cfg.get_value("record", "last_id", "")).left(64)

func _number(value: Variant) -> int:
	if not (value is int or value is float) or not is_finite(float(value)): return 0
	return clampi(int(value), 0, 1000000)

func complete(run: RefCounted) -> bool:
	if not run.enabled or not run.finished or run.run_id.is_empty(): return false
	if run.run_id == last_id:
		if dirty: return retry_save()
		return false
	last_id = run.run_id
	runs += 1
	best_credits = maxi(best_credits, run.total_earned)
	best_deliveries = maxi(best_deliveries, run.total_deliveries)
	best_relays = maxi(best_relays, run.total_relays)
	dirty = enabled
	return retry_save() if enabled else true

func retry_save() -> bool:
	if not enabled or not dirty: return false
	var cfg := ConfigFile.new()
	for key in ["runs", "best_credits", "best_deliveries", "best_relays", "last_id"]: cfg.set_value("record", key, get(key))
	cfg.set_value("record", "version", 1)
	# Write alongside the old record, then replace only after a complete write.
	var temporary := path + ".tmp"
	save_error = cfg.save(temporary)
	if save_error == OK:
		save_error = DirAccess.rename_absolute(temporary, path)
	if save_error == OK: dirty = false
	return save_error == OK

extends RefCounted

var enabled := false
var stage := 0
var credits := 0
var earned := 0
var deliveries := 0
var relays := 0
var boots := 0
var time_upgrade := 0
var horn := 0
var finished := false
var won := false
var settled := false
var ready: Array = []
var claimed: Dictionary = {}
var total_deliveries := 0
var total_relays := 0
var total_earned := 0
var run_id := ""

func begin() -> void:
	enabled = true
	stage = 0
	credits = 0
	boots = 0
	time_upgrade = 0
	horn = 0
	finished = false
	total_deliveries = 0
	total_relays = 0
	total_earned = 0
	run_id = Crypto.new().generate_random_bytes(12).hex_encode()
	prepare()

func prepare() -> void:
	earned = 0
	deliveries = 0
	relays = 0
	won = false
	settled = false
	ready.clear()
	claimed.clear()

func quota(crew: int) -> int:
	return [5, 6, 8, 10][clampi(crew, 1, 4) - 1] + stage * 2

func duration() -> float:
	return 240.0 + time_upgrade * 20.0

func ship(id: int, relay: bool) -> void:
	if not enabled or settled or finished or claimed.has(id): return
	claimed[id] = true
	deliveries += 1
	if relay: relays += 1
	earned += 10 + (5 if relay else 0)

func replace_cargo(id: int) -> void:
	claimed.erase(id)

func finish(success: bool) -> void:
	if not enabled or settled: return
	settled = true
	won = success
	ready.clear()
	if success:
		credits += earned + 20
		total_earned += earned + 20
		total_deliveries += deliveries
		total_relays += relays
		finished = stage == 2

func buy(item: String, peer_id: int) -> bool:
	if not enabled or not won or finished or peer_id != 1: return false
	var cost := {"boots": 20, "time": 25, "horn": 20}
	if not cost.has(item) or credits < int(cost[item]): return false
	if item == "boots":
		if boots >= 2: return false
		boots += 1
	elif item == "time":
		if time_upgrade >= 2: return false
		time_upgrade += 1
	else:
		if horn >= 1: return false
		horn = 1
	credits -= int(cost[item])
	ready.clear()
	return true

func vote(peer_id: int, roster: Array) -> bool:
	if not enabled or not won or finished or not roster.has(peer_id): return false
	if not ready.has(peer_id): ready.append(peer_id)
	for id in roster:
		if not ready.has(id): return false
	return true

func advance() -> bool:
	if not enabled or not won or finished: return false
	stage += 1
	prepare()
	return true

func snapshot() -> Dictionary:
	return {"enabled":enabled, "stage":stage, "credits":credits, "earned":earned,
		"deliveries":deliveries, "relays":relays, "boots":boots, "time_upgrade":time_upgrade,
		"horn":horn, "finished":finished, "won":won, "settled":settled,
		"ready":ready.duplicate(), "total_deliveries":total_deliveries,
		"total_relays":total_relays, "total_earned":total_earned, "run_id":run_id}

func apply_snapshot(data: Dictionary) -> void:
	for key in ["enabled","stage","credits","earned","deliveries","relays","boots","time_upgrade","horn","finished","won","settled","total_deliveries","total_relays","total_earned","run_id"]:
		if data.has(key): set(key, data[key])
	ready = data.get("ready", []).duplicate()

extends RefCounted

var quota := 5
var duration := 180.0
var crew := 1
var bonus := false
var base_won := false
var votes: Array = []
var seed_value := 0
var random := RandomNumberGenerator.new()
var batch: Array = []

func start(count: int, replay_seed: int = -1) -> void:
	crew = clampi(count, 1, 4)
	quota = [5, 6, 8, 10][crew - 1]
	duration = 180
	bonus = false
	base_won = false
	votes.clear()
	batch.clear()
	seed_value = replay_seed if replay_seed >= 0 else int(Time.get_unix_time_from_system() * 1000) % 2147483647
	random.seed = seed_value

func next_destination() -> int:
	if batch.is_empty():
		batch = [1, 1, 2, 2]
		for index in range(batch.size() - 1, 0, -1):
			var other := random.randi_range(0, index)
			var swap: int = batch[index]
			batch[index] = batch[other]
			batch[other] = swap
	return int(batch.pop_back())

func vote(peer_id: int, roster: Array) -> bool:
	if bonus or not roster.has(peer_id):
		return false
	if not votes.has(peer_id):
		votes.append(peer_id)
	for member in roster:
		if not votes.has(member):
			return false
	return true

func begin_bonus(current_score: int) -> void:
	base_won = true
	bonus = true
	duration = 60
	quota = current_score + [3, 4, 5, 6][crew - 1]
	votes.clear()

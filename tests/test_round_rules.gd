extends SceneTree

var failures := 0
func check(value: bool, message: String) -> void:
	if not value:
		failures += 1
		push_error(message)

func _initialize() -> void:
	var script = load("res://scripts/round_rules.gd")
	if script == null:
		quit(1)
		return
	var a = script.new()
	var b = script.new()
	for crew in range(1, 5):
		a.start(crew, 123)
		check(a.quota == [5,6,8,10][crew-1], "quota follows crew size")
	a.start(4, 9182)
	b.start(4, 9182)
	var total := 0
	for index in range(40):
		var destination: int = a.next_destination()
		check(destination == b.next_destination(), "seed replays shipment sequence")
		check(destination in [1,2], "valid destination")
		total += 1 if destination == 1 else 0
	check(total == 20, "bounded balanced batches")
	check(not a.vote(99, [1,2]), "outsider cannot vote")
	check(not a.vote(1, [1,2]), "one worker cannot choose for crew")
	check(not a.vote(1, [1,2]), "duplicate vote does not count twice")
	check(a.vote(2, [1,2]), "unanimous crew accepts overtime")
	a.begin_bonus(10)
	check(a.bonus and a.base_won and a.quota == 16 and a.duration == 60, "bonus is separate from secured base success")
	check(not a.vote(1, [1]), "second overtime refused")
	a.start(1, 42)
	check(not a.bonus and not a.base_won and a.votes.is_empty() and a.quota == 5, "new shift clears result and votes")
	print("ROUND RULES %s" % ("PASS" if failures == 0 else "FAIL"))
	quit(1 if failures else 0)

extends SceneTree

var game: Node
var role := ""
var seen_connection := false
var deadline := 0
func _initialize() -> void:
	for arg in OS.get_cmdline_user_args():
		if arg.begins_with("--role="): role = arg.trim_prefix("--role=")
	call_deferred("begin")
func begin() -> void:
	game = load("res://scenes/main.tscn").instantiate()
	root.add_child(game)
	deadline = Time.get_ticks_msec() + 12000
	if role == "host":
		game.multiplayer.peer_connected.connect(func(_id): seen_connection = true)
		game.host_game(27952, true)
	else:
		game.session.map_id = "different-map"
		game.join_game("127.0.0.1", 27952)
func _process(_delta: float) -> bool:
	if not game: return false
	if Time.get_ticks_msec() > deadline:
		push_error("Map mismatch test timed out")
		quit(1)
	elif role == "guest" and game.notice_key == "map_mismatch":
		if game.session.confirmed or not game.workers.is_empty():
			push_error("Mismatched guest admitted")
			quit(1)
		else:
			print("PASS network guest: different map rejected before admission")
			quit(0)
	elif role == "host" and seen_connection and game.multiplayer.get_peers().is_empty():
		if game.workers.size() != 1 or not game.session.admitted.is_empty() or not game.session.pending.is_empty():
			push_error("Mismatch polluted host roster")
			quit(1)
		else:
			print("PASS network host: rejected peer never joins roster")
			game.leave_game()
			quit(0)
	return false

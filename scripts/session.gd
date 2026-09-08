extends Node

signal joined
signal player_joined(peer_id: int)
signal player_left(peer_id: int)
signal stopped(reason: String)
signal input_received(peer_id: int, movement: Vector2, yaw: float, jump: bool)
signal action_received(peer_id: int, action: String)
signal snapshot_received(snapshot: Dictionary)
signal metadata_received(state: Dictionary)
signal creature_received(state: Array)

const DEFAULT_PORT := 27842
const MAX_WORKERS := 4
const PROTOCOL := 9
var protocol := PROTOCOL
var map_id := ""
var pending: Dictionary = {}
var admitted: Dictionary = {}
var confirmed := false
var mode := "offline"
var in_shift := false
var join_deadline := 0
var last_action: Dictionary = {}

func _ready() -> void:
	multiplayer.peer_connected.connect(_on_peer_connected)
	multiplayer.peer_disconnected.connect(_on_peer_disconnected)
	multiplayer.connected_to_server.connect(_on_connected)
	multiplayer.connection_failed.connect(func(): close("connection_failed"))
	multiplayer.server_disconnected.connect(func(): close("host_left"))

func host(port: int = DEFAULT_PORT) -> Error:
	close()
	var peer := ENetMultiplayerPeer.new()
	var error := peer.create_server(port, MAX_WORKERS - 1, 2)
	if error != OK:
		return error
	multiplayer.multiplayer_peer = peer
	mode = "host"
	return OK

func practice() -> void:
	close()
	mode = "practice"

func join_host(address: String, port: int = DEFAULT_PORT) -> Error:
	close()
	var peer := ENetMultiplayerPeer.new()
	var error := peer.create_client(address.strip_edges(), port, 2)
	if error != OK:
		return error
	multiplayer.multiplayer_peer = peer
	mode = "connecting"
	join_deadline = Time.get_ticks_msec() + 10000
	return OK

func close(reason: String = "") -> void:
	mode = "offline"
	in_shift = false
	join_deadline = 0
	last_action.clear()
	pending.clear()
	admitted.clear()
	confirmed = false
	if multiplayer.multiplayer_peer:
		multiplayer.multiplayer_peer.close()
	multiplayer.multiplayer_peer = OfflineMultiplayerPeer.new()
	if not reason.is_empty():
		stopped.emit(reason)

func _process(_delta: float) -> void:
	if mode == "host":
		for id in pending.keys():
			if Time.get_ticks_msec() > int(pending[id]):
				pending.erase(id)
				multiplayer.multiplayer_peer.disconnect_peer(id)
	if (mode == "connecting" or (mode == "guest" and not confirmed)) and join_deadline > 0 and Time.get_ticks_msec() > join_deadline:
		close("connection_failed")

func _on_connected() -> void:
	mode = "guest"


func _on_peer_connected(peer_id: int) -> void:
	if mode != "host":
		return
	if in_shift:
		_reject.rpc_id(peer_id)
		# Let the reliable rejection arrive; the rejected guest closes its connection.
		return
	pending[peer_id] = Time.get_ticks_msec() + 6000
	_hello.rpc_id(peer_id, protocol, map_id)

func _on_peer_disconnected(peer_id: int) -> void:
	last_action.erase(peer_id)
	pending.erase(peer_id)
	admitted.erase(peer_id)
	if mode == "host":
		player_left.emit(peer_id)

@rpc("authority", "call_remote", "reliable")
func _reject() -> void:
	close("shift_running")

func send_input(movement: Vector2, yaw: float, jump: bool) -> void:
	if mode == "guest" and confirmed:
		_receive_input.rpc_id(1, movement, yaw, jump)
	elif mode == "host" or mode == "practice":
		_accept_input(1, movement, yaw, jump)

@rpc("any_peer", "call_remote", "unreliable_ordered", 0)
func _receive_input(movement: Vector2, yaw: float, jump: bool) -> void:
	if mode == "host":
		_accept_input(multiplayer.get_remote_sender_id(), movement, yaw, jump)

func _accept_input(peer_id: int, movement: Vector2, yaw: float, jump: bool) -> void:
	if peer_id <= 0 or not movement.is_finite() or not is_finite(yaw):
		return
	input_received.emit(peer_id, movement.limit_length(), wrapf(yaw, -PI, PI), jump)

func send_action(action: String) -> void:
	if mode == "guest" and confirmed:
		_receive_action.rpc_id(1, action)
	elif mode == "host" or mode == "practice":
		_accept_action(1, action)

@rpc("any_peer", "call_remote", "reliable", 0)
func _receive_action(action: String) -> void:
	if mode == "host":
		_accept_action(multiplayer.get_remote_sender_id(), action)

func _accept_action(peer_id: int, action: String) -> void:
	if action not in ["interact", "throw", "start", "restart", "ping", "overtime", "lever", "horn", "next_contract", "buy_boots", "buy_time", "buy_horn"]:
		return
	var now := Time.get_ticks_msec()
	if now - int(last_action.get(peer_id, -1000)) < 150:
		return
	last_action[peer_id] = now
	action_received.emit(peer_id, action)

func publish(snapshot: Dictionary) -> void:
	if mode == "host":
		for id in admitted: _receive_snapshot.rpc_id(id, snapshot)

@rpc("authority", "call_remote", "unreliable_ordered", 1)
func _receive_snapshot(snapshot: Dictionary) -> void:
	if mode == "guest" and confirmed:
		snapshot_received.emit(snapshot)

@rpc("authority", "call_remote", "reliable")
func _hello(expected: int, expected_map: String) -> void:
	if mode != "guest": return
	if expected != protocol:
		close("version_mismatch")
		return
	if expected_map != map_id:
		close("map_mismatch")
		return
	_identify.rpc_id(1, protocol, map_id)

@rpc("any_peer", "call_remote", "reliable")
func _identify(version: int, guest_map: String) -> void:
	if mode != "host": return
	var id := multiplayer.get_remote_sender_id()
	if not pending.has(id): return
	pending.erase(id)
	if version != protocol:
		_version_reject.rpc_id(id)
		return
	if in_shift:
		_reject.rpc_id(id)
		return
	if guest_map != map_id:
		_map_reject.rpc_id(id)
		return
	admitted[id] = true
	_accepted.rpc_id(id, protocol)
	player_joined.emit(id)

@rpc("authority", "call_remote", "reliable")
func _version_reject() -> void:
	close("version_mismatch")

func publish_metadata(state: Dictionary) -> void:
	if mode == "host":
		for id in admitted: _metadata.rpc_id(id, state)

@rpc("authority", "call_remote", "reliable", 0)
func _metadata(state: Dictionary) -> void:
	if mode == "guest" and confirmed: metadata_received.emit(state)

func publish_creature(state: Array) -> void:
	if mode == "host":
		for id in admitted: _creature.rpc_id(id, state)

@rpc("authority", "call_remote", "unreliable_ordered", 1)
func _creature(state: Array) -> void:
	if mode == "guest" and confirmed: creature_received.emit(state)

@rpc("authority", "call_remote", "reliable")
func _accepted(version: int) -> void:
	if mode != "guest" or version != protocol: return
	confirmed = true
	join_deadline = 0
	joined.emit()

@rpc("authority", "call_remote", "reliable")
func _map_reject() -> void:
	close("map_mismatch")

extends RefCounted

const PICKUP_DISTANCE: float = 2.4

var holder_id: int = 0
var delivered: bool = false
var score: int = 0
var destination: int = 1


func try_pickup(peer_id: int, player_position: Vector3, cargo_position: Vector3) -> bool:
	if peer_id <= 0 or holder_id != 0 or delivered:
		return false
	if not player_position.is_finite() or not cargo_position.is_finite():
		return false
	var distance := player_position.distance_to(cargo_position)
	if distance > PICKUP_DISTANCE and not is_equal_approx(distance, PICKUP_DISTANCE):
		return false

	holder_id = peer_id
	return true


func try_release(peer_id: int) -> bool:
	if peer_id <= 0 or peer_id != holder_id:
		return false

	holder_id = 0
	return true


func try_dispatch(dock_id: int) -> bool:
	if dock_id != destination or holder_id != 0 or delivered:
		return false

	delivered = true
	score += 1
	return true


func remove_player(peer_id: int) -> void:
	if peer_id > 0 and peer_id == holder_id:
		holder_id = 0


func reset_crate() -> void:
	holder_id = 0
	delivered = false


func reset_shift() -> void:
	reset_crate()
	score = 0
	destination = 1

extends RefCounted

const WINDUP_SECONDS := 1.5
const BURST_SECONDS := 0.35
const RANGE := 4.5
const HALF_ANGLE_DEGREES := 50.0
const HEIGHT := 1.6

var phase := "calm"
var remaining := 6.0
var event_id := 0

var _rng := RandomNumberGenerator.new()


func _init(seed_value: int = 1) -> void:
	_rng.seed = seed_value


func reset() -> void:
	phase = "calm"
	remaining = 6.0


func advance(delta: float) -> bool:
	if not is_finite(delta) or delta <= 0.0:
		return false

	remaining -= delta
	if remaining > 0.0 and not is_zero_approx(remaining):
		return false

	match phase:
		"calm":
			phase = "windup"
			remaining = WINDUP_SECONDS
		"windup":
			phase = "burst"
			remaining = BURST_SECONDS
			event_id += 1
			return true
		"burst":
			phase = "calm"
			remaining = _rng.randf_range(6.0, 9.0)

	return false


static func contains(origin: Vector3, forward: Vector3, target: Vector3) -> bool:
	if not origin.is_finite() or not forward.is_finite() or not target.is_finite():
		return false

	var horizontal_forward := Vector2(forward.x, forward.z)
	if horizontal_forward.is_zero_approx():
		return false

	var offset := target - origin
	if absf(offset.y) > HEIGHT and not is_equal_approx(absf(offset.y), HEIGHT):
		return false

	var horizontal_offset := Vector2(offset.x, offset.z)
	var horizontal_distance := horizontal_offset.length()
	if horizontal_offset.is_zero_approx():
		return false
	if horizontal_distance > RANGE and not is_equal_approx(horizontal_distance, RANGE):
		return false

	var alignment := horizontal_forward.normalized().dot(horizontal_offset / horizontal_distance)
	var minimum_alignment := cos(deg_to_rad(HALF_ANGLE_DEGREES))
	return alignment > minimum_alignment or is_equal_approx(alignment, minimum_alignment)

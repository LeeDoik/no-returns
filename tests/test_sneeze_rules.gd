extends SceneTree

const SneezeRules = preload("res://scripts/sneeze_rules.gd")

var _failures: Array[String] = []


func _init() -> void:
	_test_initial_state_and_constants()
	_test_clock_transitions_and_single_event()
	_test_long_frames_cross_one_transition_only()
	_test_invalid_deltas_do_not_change_state()
	_test_reset_cancels_phase_and_preserves_event_id()
	_test_seeded_calm_intervals_are_bounded_and_repeatable()
	_test_cone_forward_and_inclusive_boundaries()
	_test_cone_rejections()
	_test_cone_rejects_invalid_samples()

	if _failures.is_empty():
		print("PASS: sneeze rules behavioral tests")
		quit(0)
		return

	for failure in _failures:
		push_error(failure)
	print("FAIL: %d sneeze rules assertion(s)" % _failures.size())
	quit(1)


func _test_initial_state_and_constants() -> void:
	var rules = SneezeRules.new()
	_assert_equal(rules.phase, "calm", "a new clock begins calm")
	_assert_approx(rules.remaining, 6.0, "the initial calm lasts exactly six seconds")
	_assert_equal(rules.event_id, 0, "a new clock has no sneeze event")
	_assert_approx(rules.WINDUP_SECONDS, 1.5, "windup duration matches the contract")
	_assert_approx(rules.BURST_SECONDS, 0.35, "burst duration matches the contract")
	_assert_approx(rules.RANGE, 4.5, "horizontal range matches the contract")
	_assert_approx(rules.HALF_ANGLE_DEGREES, 50.0, "cone half-angle matches the contract")
	_assert_approx(rules.HEIGHT, 1.6, "height tolerance matches the contract")


func _test_clock_transitions_and_single_event() -> void:
	var rules = SneezeRules.new()
	_assert_false(rules.advance(5.999), "calm does not end before its boundary")
	_assert_equal(rules.phase, "calm", "the clock stays calm before its boundary")
	_assert_approx(rules.remaining, 0.001, "calm time is reduced by delta", 0.00001)
	_assert_false(rules.advance(0.001), "calm to windup does not emit a burst")
	_assert_equal(rules.phase, "windup", "calm changes to windup at zero")
	_assert_approx(rules.remaining, 1.5, "windup receives its full warning duration")
	_assert_equal(rules.event_id, 0, "windup does not increment the event ID")
	_assert_false(rules.advance(1.499), "windup does not burst early")
	_assert_equal(rules.phase, "windup", "phase remains windup before its boundary")
	_assert_true(rules.advance(0.001), "windup to burst emits exactly one event")
	_assert_equal(rules.phase, "burst", "windup changes to burst at zero")
	_assert_approx(rules.remaining, 0.35, "burst receives its full visible duration")
	_assert_equal(rules.event_id, 1, "the first burst increments the event ID once")
	_assert_false(rules.advance(0.0), "zero delta cannot repeat a burst event")
	_assert_equal(rules.event_id, 1, "the burst event remains single-shot")
	_assert_false(rules.advance(0.35), "burst to calm does not emit a new event")
	_assert_equal(rules.phase, "calm", "burst returns to calm")
	_assert_between(rules.remaining, 6.0, 9.0, "the next calm interval is bounded")


func _test_long_frames_cross_one_transition_only() -> void:
	var rules = SneezeRules.new()
	_assert_false(rules.advance(100.0), "a long calm frame only starts windup")
	_assert_equal(rules.phase, "windup", "a long calm frame cannot erase the warning")
	_assert_approx(rules.remaining, 1.5, "a long calm frame resets to full windup")
	_assert_true(rules.advance(100.0), "the next long frame starts one burst")
	_assert_equal(rules.phase, "burst", "a long windup frame stops at burst")
	_assert_approx(rules.remaining, 0.35, "a long windup frame keeps full burst visibility")
	_assert_equal(rules.event_id, 1, "one long windup frame emits one event")
	_assert_false(rules.advance(100.0), "a long burst frame only returns to calm")
	_assert_equal(rules.phase, "calm", "a long burst frame stops at calm")
	_assert_equal(rules.event_id, 1, "returning to calm does not emit another event")


func _test_invalid_deltas_do_not_change_state() -> void:
	for bad_delta in [-1.0, NAN, INF, -INF]:
		var rules = SneezeRules.new()
		_assert_false(rules.advance(bad_delta), "an invalid delta does not emit an event")
		_assert_equal(rules.phase, "calm", "an invalid delta preserves phase")
		_assert_approx(rules.remaining, 6.0, "an invalid delta preserves remaining time")
		_assert_equal(rules.event_id, 0, "an invalid delta preserves the event ID")


func _test_reset_cancels_phase_and_preserves_event_id() -> void:
	var rules = SneezeRules.new()
	rules.advance(6.0)
	_assert_equal(rules.phase, "windup", "setup enters windup")
	rules.reset()
	_assert_equal(rules.phase, "calm", "reset cancels windup")
	_assert_approx(rules.remaining, 6.0, "reset restores the initial calm duration")
	_assert_equal(rules.event_id, 0, "reset before a burst preserves zero event ID")
	rules.advance(6.0)
	_assert_true(rules.advance(1.5), "setup emits a burst")
	_assert_equal(rules.event_id, 1, "setup increments the event ID")
	rules.reset()
	_assert_equal(rules.phase, "calm", "reset cancels burst")
	_assert_approx(rules.remaining, 6.0, "reset after burst restores initial calm")
	_assert_equal(rules.event_id, 1, "reset never rewinds the monotonic event ID")


func _test_seeded_calm_intervals_are_bounded_and_repeatable() -> void:
	var first = SneezeRules.new(37)
	var second = SneezeRules.new(37)
	var different = SneezeRules.new(38)
	var first_values: Array[float] = []
	var different_values: Array[float] = []
	for index in range(4):
		_advance_full_cycle(first)
		_advance_full_cycle(second)
		_advance_full_cycle(different)
		first_values.append(first.remaining)
		different_values.append(different.remaining)
		_assert_between(first.remaining, 6.0, 9.0, "seeded calm interval %d is bounded" % index)
		_assert_approx(first.remaining, second.remaining, "equal seeds produce equal interval %d" % index)
	_assert_true(first_values != different_values, "different seeds produce a different sequence")

	var default_first = SneezeRules.new()
	var default_second = SneezeRules.new()
	_advance_full_cycle(default_first)
	_advance_full_cycle(default_second)
	_assert_approx(default_first.remaining, default_second.remaining, "the default seed is deterministic")


func _test_cone_forward_and_inclusive_boundaries() -> void:
	var origin := Vector3(3.0, 2.0, -4.0)
	var forward := Vector3(0.0, 0.0, -8.0)
	_assert_true(SneezeRules.contains(origin, forward, origin + Vector3(0.0, 0.0, -2.0)), "a target directly forward is inside")
	_assert_true(SneezeRules.contains(origin, Vector3(0.0, 4.0, -8.0), origin + Vector3(0.0, 0.0, -2.0)), "vertical aim does not alter the horizontal cone")
	_assert_true(SneezeRules.contains(origin, forward, origin + Vector3(0.0, 1.6, -4.5)), "range and height boundaries are inclusive")
	var edge_radians := deg_to_rad(50.0)
	var edge_target := origin + Vector3(sin(edge_radians) * 4.0, 0.0, -cos(edge_radians) * 4.0)
	_assert_true(SneezeRules.contains(origin, forward, edge_target), "the cone angle boundary is inclusive within float tolerance")


func _test_cone_rejections() -> void:
	var origin := Vector3.ZERO
	var forward := Vector3.FORWARD
	_assert_false(SneezeRules.contains(origin, forward, Vector3(0.0, 0.0, 1.0)), "a target behind is outside")
	_assert_false(SneezeRules.contains(origin, forward, Vector3(4.0, 0.0, 0.0)), "a target ninety degrees to the side is outside")
	var outside_radians := deg_to_rad(50.1)
	var outside_target := Vector3(sin(outside_radians) * 4.0, 0.0, -cos(outside_radians) * 4.0)
	_assert_false(SneezeRules.contains(origin, forward, outside_target), "a target just outside the cone angle is rejected")
	_assert_false(SneezeRules.contains(origin, forward, Vector3(0.0, 0.0, -4.5001)), "horizontal range beyond 4.5 meters is rejected")
	_assert_false(SneezeRules.contains(origin, forward, Vector3(0.0, 1.6001, -2.0)), "height beyond 1.6 meters is rejected")
	_assert_false(SneezeRules.contains(origin, forward, Vector3(0.0, 100.0, 0.0)), "overlapping horizontal positions have no target direction")


func _test_cone_rejects_invalid_samples() -> void:
	_assert_false(SneezeRules.contains(Vector3(NAN, 0.0, 0.0), Vector3.FORWARD, Vector3.FORWARD), "a nonfinite origin is rejected")
	_assert_false(SneezeRules.contains(Vector3.ZERO, Vector3(INF, 0.0, 0.0), Vector3.FORWARD), "a nonfinite forward vector is rejected")
	_assert_false(SneezeRules.contains(Vector3.ZERO, Vector3.UP, Vector3.FORWARD), "a zero horizontal forward vector is rejected")
	_assert_false(SneezeRules.contains(Vector3.ZERO, Vector3.FORWARD, Vector3(0.0, 0.0, NAN)), "a nonfinite target is rejected")


func _advance_full_cycle(rules: RefCounted) -> void:
	rules.advance(rules.remaining)
	rules.advance(rules.remaining)
	rules.advance(rules.remaining)


func _assert_true(actual: bool, message: String) -> void:
	if not actual:
		_failures.append("Expected true: %s" % message)


func _assert_false(actual: bool, message: String) -> void:
	if actual:
		_failures.append("Expected false: %s" % message)


func _assert_equal(actual: Variant, expected: Variant, message: String) -> void:
	if actual != expected:
		_failures.append("%s (expected %s, got %s)" % [message, expected, actual])


func _assert_approx(actual: float, expected: float, message: String, tolerance: float = 0.000001) -> void:
	if absf(actual - expected) > tolerance:
		_failures.append("%s (expected %s +/- %s, got %s)" % [message, expected, tolerance, actual])


func _assert_between(actual: float, minimum: float, maximum: float, message: String) -> void:
	if actual < minimum or actual > maximum:
		_failures.append("%s (expected %s..%s, got %s)" % [message, minimum, maximum, actual])

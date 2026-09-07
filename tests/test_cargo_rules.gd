extends SceneTree

const CargoRules = preload("res://scripts/cargo_rules.gd")

var _failures: Array[String] = []


func _init() -> void:
	_test_initial_state()
	_test_pickup_identity_distance_and_finite_positions()
	_test_contested_pickup_and_release_ownership()
	_test_dispatch_boundaries_and_one_time_score()
	_test_disconnect_boundaries()
	_test_reset_boundaries()

	if _failures.is_empty():
		print("PASS: cargo rules behavioral tests")
		quit(0)
		return

	for failure in _failures:
		push_error(failure)
	print("FAIL: %d cargo rules assertion(s)" % _failures.size())
	quit(1)


func _test_initial_state() -> void:
	var rules = CargoRules.new()
	_assert_equal(rules.holder_id, 0, "a new crate has no holder")
	_assert_equal(rules.delivered, false, "a new crate is not delivered")
	_assert_equal(rules.score, 0, "a new shift starts at zero")
	_assert_equal(rules.PICKUP_DISTANCE, 2.4, "pickup distance matches the contract")


func _test_pickup_identity_distance_and_finite_positions() -> void:
	var rules = CargoRules.new()
	var origin := Vector3.ZERO

	_assert_false(rules.try_pickup(0, origin, origin), "peer zero cannot pick up")
	_assert_false(rules.try_pickup(-4, origin, origin), "negative peer IDs cannot pick up")
	_assert_false(
		rules.try_pickup(1, Vector3(NAN, 0.0, 0.0), origin),
		"a nonfinite player position is rejected"
	)
	_assert_false(
		rules.try_pickup(1, origin, Vector3(0.0, INF, 0.0)),
		"a nonfinite cargo position is rejected"
	)
	_assert_false(
		rules.try_pickup(1, origin, Vector3(2.4001, 0.0, 0.0)),
		"a pickup beyond the distance limit is rejected"
	)
	_assert_equal(rules.holder_id, 0, "failed pickups do not claim the crate")
	_assert_true(
		rules.try_pickup(1, origin, Vector3(2.4, 0.0, 0.0)),
		"a pickup exactly at the distance limit succeeds"
	)


func _test_contested_pickup_and_release_ownership() -> void:
	var rules = CargoRules.new()
	_assert_true(rules.try_pickup(7, Vector3.ZERO, Vector3.ZERO), "the first peer claims a free crate")
	_assert_false(rules.try_pickup(8, Vector3.ZERO, Vector3.ZERO), "a second peer cannot steal a held crate")
	_assert_equal(rules.holder_id, 7, "a contested pickup preserves the original holder")
	_assert_false(rules.try_release(8), "a non-holder cannot release the crate")
	_assert_false(rules.try_release(0), "an invalid peer cannot release the crate")
	_assert_equal(rules.holder_id, 7, "failed releases preserve ownership")
	_assert_true(rules.try_release(7), "the holder can release the crate")
	_assert_equal(rules.holder_id, 0, "a successful release clears ownership")


func _test_dispatch_boundaries_and_one_time_score() -> void:
	var rules = CargoRules.new()
	_assert_true(rules.try_pickup(2, Vector3.ZERO, Vector3.ZERO), "setup pickup succeeds")
	_assert_false(rules.try_dispatch(1), "a held crate cannot be dispatched")
	_assert_true(rules.try_release(2), "setup release succeeds")
	_assert_false(rules.try_dispatch(0), "an invalid dock cannot receive the crate")
	_assert_false(rules.try_dispatch(2), "the wrong dock cannot receive the crate")
	_assert_equal(rules.score, 0, "rejected dispatches do not score")
	_assert_true(rules.try_dispatch(1), "dock one accepts a free undelivered crate")
	_assert_equal(rules.delivered, true, "a successful dispatch marks the crate delivered")
	_assert_equal(rules.score, 1, "a successful dispatch scores exactly once")
	_assert_false(rules.try_dispatch(1), "the same crate cannot be dispatched twice")
	_assert_equal(rules.score, 1, "duplicate dispatch does not increment the score")
	_assert_false(rules.try_pickup(3, Vector3.ZERO, Vector3.ZERO), "a delivered crate cannot be picked up")


func _test_disconnect_boundaries() -> void:
	var rules = CargoRules.new()
	_assert_true(rules.try_pickup(11, Vector3.ZERO, Vector3.ZERO), "setup pickup succeeds")
	rules.remove_player(12)
	_assert_equal(rules.holder_id, 11, "removing a non-holder preserves ownership")
	rules.remove_player(0)
	_assert_equal(rules.holder_id, 11, "removing an invalid peer preserves ownership")
	rules.remove_player(11)
	_assert_equal(rules.holder_id, 0, "removing the holder releases the crate")


func _test_reset_boundaries() -> void:
	var rules = CargoRules.new()
	_assert_true(rules.try_dispatch(1), "the first crate scores")
	rules.reset_crate()
	_assert_equal(rules.holder_id, 0, "crate reset clears the holder")
	_assert_equal(rules.delivered, false, "crate reset clears delivered state")
	_assert_equal(rules.score, 1, "crate reset preserves the shift score")
	_assert_true(rules.try_dispatch(1), "the reset crate can score once again")
	_assert_equal(rules.score, 2, "the new crate adds one score")

	rules.reset_shift()
	_assert_equal(rules.holder_id, 0, "shift reset clears the holder")
	_assert_equal(rules.delivered, false, "shift reset clears delivered state")
	_assert_equal(rules.score, 0, "shift reset clears the score")
	_assert_true(rules.try_pickup(5, Vector3.ZERO, Vector3.ZERO), "a new shift accepts pickup")


func _assert_true(actual: bool, message: String) -> void:
	if not actual:
		_failures.append("Expected true: %s" % message)


func _assert_false(actual: bool, message: String) -> void:
	if actual:
		_failures.append("Expected false: %s" % message)


func _assert_equal(actual: Variant, expected: Variant, message: String) -> void:
	if actual != expected:
		_failures.append("%s (expected %s, got %s)" % [message, expected, actual])

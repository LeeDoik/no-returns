extends RefCounted

const FLOOR_SIZE := Vector2(32, 36)
const FLOOR_CENTER := Vector3(0, -0.25, -9)
const BAY_A := Vector3(-10, 0.65, -22)
const BAY_B := Vector3(10, 0.65, -22)
const BELT_CENTER := Vector3(0, 0, -5)
const LEVER := Vector3(2, 0, -4)
const NEST := Vector3(13, 0, -5)

static func bay(id: int) -> Vector3:
	return BAY_A if id == 1 else BAY_B

static func dock_at(at: Vector3) -> int:
	if at.y >= 1.7: return 0
	for id in [1, 2]:
		var center := bay(id)
		if absf(at.x - center.x) < 1.5 and absf(at.z - center.z) < 1.3:
			return id
	return 0

static func outside(at: Vector3) -> bool:
	return at.y < -4 or absf(at.x) > 17 or at.z < -28 or at.z > 10

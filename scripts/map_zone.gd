extends RefCounted

static func contains(zone: Node3D, point: Vector3) -> bool:
	if not is_instance_valid(zone): return false
	var collision := zone.get_node_or_null("CollisionShape3D") as CollisionShape3D
	if not collision or collision.disabled or not collision.shape is BoxShape3D: return false
	var local := collision.global_transform.affine_inverse() * point
	var half: Vector3 = collision.shape.size * 0.5
	return absf(local.x) <= half.x and absf(local.y) <= half.y and absf(local.z) <= half.z

static func clamp_point(zone: Node3D, point: Vector3) -> Vector3:
	var collision := zone.get_node("CollisionShape3D") as CollisionShape3D
	var local := collision.global_transform.affine_inverse() * point
	var half: Vector3 = collision.shape.size * 0.5
	local.x = clampf(local.x, -half.x, half.x)
	local.z = clampf(local.z, -half.z, half.z)
	return collision.global_transform * local

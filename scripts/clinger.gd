extends Node3D

const Copy = preload("res://scripts/copy.gd")
const REACH := 0.95
const DURATION := 5.0
var target_kind := ""
var target_id := 0
var remaining := 0.0
var cooldown := 1.0
var cargo
var offset := Vector3.ZERO
var label: Label3D
var link: MeshInstance3D

func setup(source: Node3D) -> void:
	cargo = source
	label = Label3D.new()
	var font := SystemFont.new()
	font.font_names = PackedStringArray(["Malgun Gothic", "Segoe UI", "Arial"])
	label.font = font
	label.font_size = 30
	label.pixel_size = 0.006
	label.billboard = BaseMaterial3D.BILLBOARD_ENABLED
	label.modulate = Color("d8ffa0")
	add_child(label)
	link = MeshInstance3D.new()
	var cylinder := CylinderMesh.new()
	cylinder.top_radius = 0.035
	cylinder.bottom_radius = 0.035
	cylinder.height = 1
	link.mesh = cylinder
	var material := StandardMaterial3D.new()
	material.albedo_color = Color("b1ef52")
	material.shading_mode = BaseMaterial3D.SHADING_MODE_UNSHADED
	link.material_override = material
	add_child(link)
	reset()

func detach() -> void:
	target_kind = ""
	target_id = 0
	remaining = 0
	cooldown = 2
	offset = Vector3.ZERO
	if is_instance_valid(cargo) and is_instance_valid(cargo.body):
		if cargo.body.visible:
			cargo.body.collision_layer = 4
			cargo.body.collision_mask = 7
		cargo.body.freeze = not (cargo.active and cargo.rules.holder_id == 0 and cargo.recovery_left <= 0)
		cargo.body.sleeping = false

func reset() -> void:
	detach()
	cooldown = 1

func snapshot() -> Array:
	return [target_kind, target_id, remaining, cooldown]

func apply_snapshot(data: Array) -> void:
	if data.size() != 4:
		return
	target_kind = str(data[0])
	target_id = int(data[1])
	remaining = float(data[2])
	cooldown = float(data[3])
	# Guests only present replicated state; Cargo owns their frozen body pose.
	if target_kind != "" and is_instance_valid(cargo):
		cargo.body.collision_layer = 0
		cargo.body.collision_mask = 1

func _available(item) -> bool:
	return is_instance_valid(item) and not item.creature_held and item.active and item.recovery_left <= 0 and item.body.visible

func _anchor(item, kind: String) -> Vector3:
	return item.global_position + Vector3.UP if kind == "worker" else item.body.global_position

func step(delta: float, workers: Dictionary, cargos: Dictionary) -> void:
	if not _available(cargo) or cargo.rules.holder_id != 0:
		if target_kind != "":
			detach()
		return
	if target_kind != "":
		var targets: Dictionary = workers if target_kind == "worker" else cargos
		var target = targets.get(target_id)
		if not is_instance_valid(target):
			detach()
			return
		if target_kind == "cargo":
			if target.rules.delivered:
				detach()
				if cargo.rules.try_dispatch(target.rules.destination):
					cargo.recover("shipped")
				else:
					cargo.recover("wrong_bay")
				return
			if not _available(target):
				detach()
				return
		remaining = maxf(0, remaining - delta)
		var wanted: Vector3 = _anchor(target, target_kind) + offset
		var motion: Vector3 = wanted - cargo.body.global_position
		if remaining <= 0 or motion.length() > 2.0:
			detach()
			return
		var request: PhysicsShapeQueryParameters3D = cargo.query(cargo.body.global_position, motion)
		request.collision_mask = 1
		# A 1 mm inset tolerates resting contact without treating the floor as
		# penetration. The full swept box still blocks meaningful wall crossings.
		var follow_shape := BoxShape3D.new()
		follow_shape.size = cargo.shape.size - Vector3.ONE * 0.002
		request.shape = follow_shape
		request.margin = 0.0
		var space := get_world_3d().direct_space_state
		var fractions := space.cast_motion(request)
		# cast_motion ignores initial overlap, so explicitly reject that too.
		request.motion = Vector3.ZERO
		if fractions[0] < 1.0 or not space.intersect_shape(request, 1).is_empty():
			detach()
			return
		cargo.body.global_position = wanted
		return
	if cooldown > 0:
		cooldown = maxf(0, cooldown - delta)
		return
	var nearest := REACH + 0.00001
	var chosen = null
	var chosen_kind := ""
	var chosen_id := 0
	for kind in ["worker", "cargo"]:
		var candidates: Dictionary = workers if kind == "worker" else cargos
		for id in candidates:
			var candidate = candidates[id]
			if not is_instance_valid(candidate) or candidate == cargo:
				continue
			if kind == "cargo" and (not _available(candidate) or candidate.kind == "clinger" or candidate.rules.delivered):
				continue
			var at := _anchor(candidate, kind)
			var distance: float = cargo.body.global_position.distance_to(at)
			if distance > REACH or distance >= nearest:
				continue
			var ray := PhysicsRayQueryParameters3D.create(cargo.body.global_position, at, 1, [cargo.body.get_rid()])
			if not get_world_3d().direct_space_state.intersect_ray(ray).is_empty():
				continue
			nearest = distance
			chosen = candidate
			chosen_kind = kind
			chosen_id = id
	if chosen != null:
		target_kind = chosen_kind
		target_id = chosen_id
		remaining = DURATION
		offset = cargo.body.global_position - _anchor(chosen, chosen_kind)
		cargo.body.freeze = true
		cargo.body.linear_velocity = Vector3.ZERO
		cargo.body.angular_velocity = Vector3.ZERO
		cargo.body.collision_layer = 0
		cargo.body.collision_mask = 1

func _process(_delta: float) -> void:
	if not is_instance_valid(cargo) or not label:
		return
	label.visible = cargo.active and cargo.body.visible
	link.visible = false
	if not label.visible:
		return
	label.global_position = cargo.body.global_position + Vector3.UP * 0.8
	var status: String
	if target_kind != "":
		status = Copy.get_text("clinger_tag_attached") % remaining
	elif cooldown > 0:
		status = Copy.get_text("clinger_tag_cooldown") % cooldown
	else:
		status = Copy.get_text("clinger_tag_ready")
	label.text = Copy.get_text("clinger_name") + "\n" + status
	var game = cargo.get_parent()
	if target_kind == "" or game == null:
		return
	var registry = game.get("workers" if target_kind == "worker" else "cargos")
	if not registry is Dictionary:
		return
	var target = registry.get(target_id)
	if not is_instance_valid(target):
		return
	var start: Vector3 = cargo.body.global_position
	var end := _anchor(target, target_kind)
	var direction := end - start
	if direction.length() < 0.001:
		return
	link.visible = true
	var axis := direction.normalized()
	var side := axis.cross(Vector3.FORWARD if absf(axis.dot(Vector3.UP)) > 0.99 else Vector3.UP).normalized()
	link.global_transform = Transform3D(Basis(side, axis * direction.length(), side.cross(axis)), (start + end) * 0.5)

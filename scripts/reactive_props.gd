extends Node3D
signal state_changed
const Prop = preload("res://scripts/reactive_prop.gd")
var props: Array[Node3D] = []
var last_phases := PackedInt32Array()
func _ready() -> void:
	_collect(self)
func _collect(node: Node) -> void:
	for child in node.get_children():
		if child.get_script() == Prop: props.append(child)
		else: _collect(child)
func reset() -> void:
	for p in props: p.reset()
	last_phases.clear()
func free_cargo(cargo) -> bool:
	return cargo.active and cargo.body.visible and cargo.recovery_left <= 0 and not cargo.rules.delivered and cargo.rules.holder_id == 0 and not cargo.creature_held and (not cargo.cling or cargo.cling.target_kind.is_empty())
func reaches(p: Node3D, at: Vector3, radius: float) -> bool:
	var local: Vector3 = p.to_local(at)
	if local.y < -0.1 or local.y > (1.9 if p.kind == 2 else 1.2): return false
	if Vector2(local.x,local.z).length() > radius: return false
	var origin: Vector3 = p.global_position+Vector3.UP*0.5
	return get_world_3d().direct_space_state.intersect_ray(PhysicsRayQueryParameters3D.create(origin,at,1)).is_empty()

# Host only; guests only apply state and animate. Occupancy survives cooldown.
func step(delta: float, workers: Dictionary, cargos: Dictionary) -> void:
	if not is_finite(delta) or delta <= 0: return
	for p in props:
		if p.kind == 3: continue
		var occupants: Dictionary = {}
		for id in workers:
			var w = workers[id]
			if not reaches(p,w.global_position+Vector3.UP*0.5,p.trigger_radius): continue
			var key := "w%d"%id; occupants[key] = true
			if not p.occupied.has(key) and (p.kind != 2 or w.velocity.length() >= 1.8): p.arm()
		for id in cargos:
			var c = cargos[id]
			if not free_cargo(c) or not reaches(p,c.body.global_position,p.trigger_radius): continue
			var key := "c%d"%id; occupants[key] = true
			if not p.occupied.has(key) and (p.kind != 2 or c.body.linear_velocity.length() >= 1.8): p.arm()
		p.occupied = occupants
	# Separate loops prevent tree order from changing chain timing.
	var fired: Array[Node3D] = []
	for p in props:
		if p.phase == 0: continue
		p.remaining = maxf(0,p.remaining-delta)
		if p.remaining > 0: continue
		if p.phase == 1: p.fire(); fired.append(p)
		else: p.phase = 0
	for p in fired:
		if p.kind == 3: continue
		_effect(p,workers,cargos)
		if p.kind == 1: continue
		for other in props:
			if other == p or other.kind in [1,3]: continue
			if reaches(p,other.global_position+Vector3.UP*0.5,2.65): other.arm()
	_notify_changes()

func _notify_changes() -> void:
	var phases := PackedInt32Array()
	for p in props: phases.append(p.phase); phases.append(p.event_id)
	if phases == last_phases: return
	last_phases = phases; state_changed.emit()

func _effect(p: Node3D, workers: Dictionary, cargos: Dictionary) -> void:
	for w in workers.values():
		if not reaches(p,w.global_position+Vector3.UP*0.5,p.effect_radius): continue
		var direction := _direction(p,w.global_position)
		w.push_velocity = (w.push_velocity+direction*(2.5 if p.kind == 1 else 4.0)).limit_length(10)
		w.velocity.y = maxf(w.velocity.y,p.lift_speed); w.stagger = 0.35
	for c in cargos.values():
		if not free_cargo(c) or not reaches(p,c.body.global_position,p.effect_radius): continue
		c.body.freeze = false; c.body.sleeping = false
		var direction := _direction(p,c.body.global_position)
		var velocity: Vector3 = c.body.linear_velocity+direction*(3.0 if p.kind == 1 else 5.0)
		velocity.y = maxf(velocity.y,p.lift_speed); c.body.linear_velocity = velocity.limit_length(14)
func _direction(p: Node3D, at: Vector3) -> Vector3:
	if p.kind == 1: return -p.global_basis.z.normalized()
	var offset := at-p.global_position; offset.y = 0
	return offset.normalized() if offset.length() > 0.1 else -p.global_basis.z.normalized()

func blast(source: Node3D, can_reach: Callable) -> void:
	for p in props:
		if can_reach.call(source,p.global_position+Vector3.UP*0.45): p.arm(Vector3.FORWARD.rotated(Vector3.UP,source.facing))
	_notify_changes()
func snapshot() -> PackedFloat32Array:
	var state := PackedFloat32Array()
	for p in props: state.append_array(PackedFloat32Array([p.phase,p.remaining,p.event_id,p.burst_direction.x,p.burst_direction.y,p.burst_direction.z]))
	return state
func apply_snapshot(state: Variant) -> void:
	if not state is PackedFloat32Array or state.size() != props.size()*6: return
	for value in state:
		if not is_finite(value): return
	for i in range(props.size()):
		var n := i*6
		if state[n] != floorf(state[n]) or state[n] < 0 or state[n] > 2: return
		if state[n+1] < 0 or state[n+1] > 20: return
		if state[n+2] != floorf(state[n+2]) or state[n+2] < 0 or state[n+2] > 16777215: return
		if Vector3(state[n+3],state[n+4],state[n+5]).length() > 1.01: return
	for i in range(props.size()):
		var n := i*6
		props[i].apply_state([int(state[n]),state[n+1],int(state[n+2]),Vector3(state[n+3],state[n+4],state[n+5])])

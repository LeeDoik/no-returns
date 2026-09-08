extends "res://tools/expand_shrine.gd"
const PropScene = preload("res://scenes/pieces/reactive_prop.tscn")
const Manager = preload("res://scripts/reactive_props.gd")
var used: Array[Vector3] = []
func own_reactions(node: Node) -> void:
	for child in node.get_children():
		child.owner = map
		if child.get_script() != preload("res://scripts/reactive_prop.gd"): own_reactions(child)
func clear_at(at: Vector3) -> bool:
	for taken in used:
		if taken.distance_to(at) < 1.85: return false
	var q := PhysicsShapeQueryParameters3D.new(); var shape := BoxShape3D.new(); shape.size = Vector3(1.8,3,1.8)
	q.shape = shape; q.transform.origin = at+Vector3.UP*1.6; q.collision_mask = 1; q.margin = 0.02
	return map.get_world_3d().direct_space_state.intersect_shape(q,1).is_empty()
func place_near(at: Vector3) -> Vector3:
	if clear_at(at): used.append(at); return at
	for radius in [1.0,2.0,3.0,4.0]:
		for i in range(16):
			var candidate: Vector3 = at+Vector3(cos(i*TAU/16),0,sin(i*TAU/16))*radius
			if clear_at(candidate): used.append(candidate); return candidate
	push_error("No clear prop placement near "+str(at)); return Vector3.INF
func run() -> void:
	map = load(TARGET).instantiate(); root.add_child(map); map.set_process(false)
	if map.has_meta("reactive_delivery"):
		push_error("Reactive props already applied; edit saved nodes instead."); quit(1); return
	await physics_frame; await physics_frame
	var system := group(map.get_node("Gameplay"),"Reactions")
	for row in [["Intake",Vector3(-4,0,-3)],["WestEntry",Vector3(-15,0,-12)],["WestBend",Vector3(-21,0,-22)],["WestDispatch",Vector3(-12,0,-35)],["CentralSort",Vector3(3,0,-24)],["EastBend",Vector3(15,0,-21)],["EastDispatch",Vector3(12,0,-35)],["FinalHandoff",Vector3(3,0,-40)]]:
		var cluster := group(system,row[0]); cluster.set_meta("_edit_group_",true)
		for kind in range(3):
			var p = PropScene.instantiate(); p.name = ["PackingCushion","ReturnSpring","CartonTower"][kind]
			p.kind = kind; p.reset_seconds = [8.0,3.0,10.0][kind]; p.lift_speed = [4.5,8.5,3.5][kind]
			p.trigger_radius = 1.15 if kind == 2 else 0.85; p.effect_radius = 1.25 if kind == 1 else 2.2
			var target: Vector3 = row[1]+[Vector3.ZERO,Vector3(0,0,3),Vector3(2.1,0,0)][kind]
			var point := place_near(target)
			if not point.is_finite(): p.free(); quit(1); return
			cluster.add_child(p); p.global_position = point
			print(row[0]," ",p.name," ",point)
	system.set_script(Manager)
	map.set_meta("reactive_delivery",1)
	own_reactions(map)
	var packed := PackedScene.new()
	if packed.pack(map) != OK or ResourceSaver.save(packed,TARGET) != OK: quit(1); return
	print("24 REACTIVE PROPS SAVED"); map.queue_free(); await process_frame; quit(0)

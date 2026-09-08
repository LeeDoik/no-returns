extends "res://tools/apply_postal_art.gd"
func run() -> void:
	map = load(TARGET).instantiate(); root.add_child(map); map.set_process(false)
	for node in map.find_children("Art_*","Node3D",true,false):
		var id := ""
		for key in ["workbench","shelf","conveyor_module","dispatch","pendant"]:
			if node.has_node(key): id = key; break
		if id.is_empty(): continue
		var parent := node.get_parent(); var pose: Transform3D = node.transform
		var replacement := instance(parent,id); replacement.transform = pose
		# Keep authored address labels attached to dispatch art.
		for child in node.get_children():
			if child is Label3D: child.reparent(replacement,false)
		parent.remove_child(node); node.free()
	for mesh in map.find_children("*","MeshInstance3D",true,false):
		var size: Vector3 = mesh.mesh.get_aabb().size
		if minf(size.x,minf(size.y,size.z)) < 0.025: mesh.cast_shadow = GeometryInstance3D.SHADOW_CASTING_SETTING_OFF
	for p in map.get_node("Gameplay/Reactions").props:
		for child in p.get_children(): child.free()
	own(map); var packed := PackedScene.new()
	if packed.pack(map) != OK or ResourceSaver.save(packed,TARGET) != OK: quit(1); return
	map.free(); await process_frame; print("POSTAL BATCHING SAVED"); quit()

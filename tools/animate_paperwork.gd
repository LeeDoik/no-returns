extends "res://tools/populate_reactive_props.gd"
func run() -> void:
	map = load(TARGET).instantiate(); root.add_child(map); map.set_process(false)
	if map.has_meta("reactive_paperwork"):
		push_error("Paperwork already converted; edit saved props instead."); quit(1); return
	var system = map.get_node("Gameplay/Reactions")
	var documents := group(system,"Paperwork")
	var count := 0
	for mesh in map.get_node("Geometry").find_children("*","MeshInstance3D",true,false):
		if not mesh.is_visible_in_tree() or str(mesh.name) not in ["Paperwork","PaperStack","UnfiledForms"]: continue
		var prop = PropScene.instantiate(); prop.kind = 3; prop.name = "Documents%02d"%count; prop.reset_seconds = 8
		documents.add_child(prop); prop.global_position = mesh.global_position-Vector3.UP*0.06
		prop.set_meta("replaces",str(map.get_path_to(mesh))); mesh.visible = false; count += 1
		if mesh.name == "UnfiledForms":
			for other in mesh.get_parent().get_children():
				if other is MeshInstance3D and other.mesh.get_aabb().size.is_equal_approx(Vector3(0.62,0.025,0.52)): other.visible = false
			prop.global_position = mesh.global_position+Vector3.UP*0.245
		print(prop.name," ",prop.global_position)
	for invoice in map.get_node("Geometry").find_children("Invoice","Node3D",true,false):
		if not invoice.is_visible_in_tree(): continue
		var prop = PropScene.instantiate(); prop.kind = 3; prop.reset_seconds = 8; prop.name = "Documents%02d"%count
		documents.add_child(prop); prop.global_position = invoice.global_position+Vector3.UP*0.01
		prop.set_meta("replaces",str(map.get_path_to(invoice))); invoice.visible = false; count += 1
	map.set_meta("reactive_paperwork",count); own_reactions(map)
	var packed := PackedScene.new()
	if packed.pack(map) != OK or ResourceSaver.save(packed,TARGET) != OK: quit(1); return
	print("PAPERWORK SAVED ",count); map.queue_free(); await process_frame; quit(0)

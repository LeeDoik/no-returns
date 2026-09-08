extends "res://tools/expand_shrine.gd"
# Explicit, one-time saved-scene art migration. Never called by play or export.
const Art = preload("res://scripts/postal_art.gd")
func instance(parent: Node, id: String, at: Vector3 = Vector3.ZERO) -> Node3D:
	var node := Art.model(id); node.name = "Art_"+id; parent.add_child(node,true); node.position = at
	# Bake material overrides and editable part hierarchy into the saved map.
	node.scene_file_path = ""
	return node
func own(node: Node) -> void:
	# Prop visuals are editor previews, never authored scene state.
	if node.get_script() == preload("res://scripts/reactive_prop.gd"): return
	super.own(node)
func run() -> void:
	map = load(TARGET).instantiate(); root.add_child(map); map.set_process(false)
	if map.has_meta("postal_art_01"): push_error("Postal art already applied; preserving editor work."); quit(1); return
	map.set_meta("postal_art_01",1)
	var benches := 0
	for furniture in map.get_node("Geometry/InteriorDressing").get_children():
		if not furniture.has_node("Desk"): continue
		for child in furniture.get_node("Desk").get_children():
			if child is MeshInstance3D: child.visible = false
		instance(furniture,"workbench").rotation.y = PI
		benches += 1
		if furniture.has_node("AuthoredWorktop"):
			for child in furniture.get_node("AuthoredWorktop").get_children():
				if child is MeshInstance3D and child.position.y < 1.24: child.visible = false
	var conveyor = map.get_node("Gameplay/Conveyor")
	for rack in map.get_node("Geometry/Shelves").get_children():
		for child in rack.get_children():
			if str(child.name).begins_with("Detail"): child.visible = false
		var shelf := instance(rack,"shelf"); shelf.rotation.y = PI/2; shelf.scale = Vector3(1.55,1.0,1.0)
	conveyor.get_node("Bed").visible = false
	for i in range(8): instance(conveyor,"conveyor_module",Vector3(0,-0.15,-3.5+i))
	for bay_name in ["DispatchA","DispatchB"]:
		var bay = map.get_node("Gameplay/"+bay_name)
		# Leave the existing delivery volume and expressive destination facade intact.
		var port := instance(bay,"dispatch",Vector3(0,-0.65,-1.55)); port.rotation.y = PI
		var title := add_sign(port,"Destination",Vector3(0,2.21,-0.412),"A" if bay_name == "DispatchA" else "B","A" if bay_name == "DispatchA" else "B",38)
		title.pixel_size = 0.005; title.outline_size = 0; title.modulate = Color("273c33")
	for light in map.find_children("*","OmniLight3D",true,false):
		if light.position.y < 3.0: continue
		instance(light,"pendant",Vector3(0,0.20,0))
		light.light_color = Color("ffe0b0")
		light.light_energy = minf(light.light_energy,1.2)
	var floor_body = map.get_node("Geometry/Floor")
	for child in floor_body.get_children():
		if not child is MeshInstance3D: continue
		var material: StandardMaterial3D = child.material_override.duplicate()
		material.albedo_texture = load(Art.DIRECTORY+"textures/floor_basecolor.png")
		material.normal_enabled = true; material.normal_texture = load(Art.DIRECTORY+"textures/floor_normal.png"); material.normal_scale = 0.25
		material.uv1_triplanar = true; material.uv1_world_triplanar = true; material.uv1_scale = Vector3.ONE*0.3
		material.albedo_color = Color.WHITE; floor_body.color = Color.WHITE; child.material_override = material
	# Surface grain on existing structural solids keeps all their editable dimensions.
	for solid in map.find_children("*","StaticBody3D",true,false):
		if not solid.get_script() == preload("res://scripts/editable_block.gd") or solid == floor_body: continue
		if solid.dimensions.y < 0.8: continue
		for child in solid.get_children():
			if not child is MeshInstance3D or not child.visible: continue
			var material: StandardMaterial3D = child.material_override.duplicate()
			material.normal_enabled = true; material.normal_texture = load(Art.DIRECTORY+"textures/paint_normal.png"); material.normal_scale = 0.14
			material.uv1_triplanar = true; material.uv1_world_triplanar = true; material.uv1_scale = Vector3.ONE*0.8
			child.material_override = material
	for p in map.get_node("Gameplay/Reactions").props:
		for child in p.get_children(): child.free()
	own(map)
	var packed := PackedScene.new()
	if packed.pack(map) != OK or ResourceSaver.save(packed,TARGET) != OK: quit(1); return
	print("POSTAL ART MAP SAVED benches=",benches," conveyors=8")
	map.queue_free(); await process_frame; quit()

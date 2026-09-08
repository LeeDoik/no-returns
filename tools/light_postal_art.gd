extends "res://tools/apply_postal_art.gd"
func run() -> void:
	map = load(TARGET).instantiate(); root.add_child(map); map.set_process(false)
	var env: Environment = map.get_node("Environment/WorldEnvironment1").environment
	env.ambient_light_color = Color("a7b8c9"); env.ambient_light_energy = 0.28
	var sun: DirectionalLight3D = map.get_node("Environment/Sun1")
	sun.light_color = Color("c3d7ef"); sun.light_energy = 0.36
	for light in map.find_children("*","OmniLight3D",true,false):
		light.light_energy = minf(light.light_energy,0.7)
		light.light_color = Color("ffdcb0")
	# Preview children are reconstructed by their owning props.
	for p in map.get_node("Gameplay/Reactions").props:
		for child in p.get_children(): child.free()
	own(map); var packed := PackedScene.new()
	if packed.pack(map) != OK or ResourceSaver.save(packed,TARGET) != OK: quit(1); return
	map.free(); await process_frame; print("POSTAL LIGHTING SAVED"); quit()

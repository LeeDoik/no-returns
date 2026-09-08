extends "res://tools/expand_shrine.gd"
# Explicit saved-scene art pass. Never called by normal play or builds.
var grain: NoiseTexture2D
func finish_material(mesh: MeshInstance3D, tint: Color, textured := false) -> void:
	if not mesh.material_override is StandardMaterial3D: return
	var mat: StandardMaterial3D = mesh.material_override.duplicate()
	mat.albedo_color = tint; mat.roughness = 0.94; mat.metallic_specular = 0.15
	if textured:
		mat.albedo_texture = grain; mat.uv1_triplanar = true; mat.uv1_world_triplanar = true; mat.uv1_scale = Vector3.ONE*0.65
	mesh.material_override = mat
func trim_wall(wall: Node3D) -> void:
	var size: Vector3 = wall.dimensions
	if size.y < 2.3 or minf(size.x,size.z) > 1 or maxf(size.x,size.z) < 3: return
	var along_x := size.x > size.z
	var length := maxf(size.x,size.z); var depth := minf(size.x,size.z)
	var finish := group(wall,"SurfaceFinish")
	for side in [-1,1]:
		var panel_size := Vector3(length,1.2,0.025) if along_x else Vector3(0.025,1.2,length)
		var at := Vector3(0,-size.y/2+0.65,side*(depth/2+0.015)) if along_x else Vector3(side*(depth/2+0.015),-size.y/2+0.65,0)
		visual(finish,"KickPlate",at,panel_size,"455b59")
		var rail_size := Vector3(length,0.06,0.045) if along_x else Vector3(0.045,0.06,length)
		at.y = -size.y/2+1.3; visual(finish,"ChairRail",at,rail_size,"89928b")
		for i in range(int(length/3)+1):
			var offset := -length/2+0.1+i*(length-0.2)/maxi(1,int(length/3))
			at = Vector3(offset,0,side*(depth/2+0.04)) if along_x else Vector3(side*(depth/2+0.04),0,offset)
			visual(finish,"PanelJoint",at,Vector3(0.065,size.y,0.06) if along_x else Vector3(0.06,size.y,0.065),"7f8982")
func mounted(label: Label3D, en: String, ko: String, font := 28) -> void:
	label.visible = true
	if label.get_script() == Sign: label.english = en; label.korean = ko
	label.font_size = font; label.pixel_size = 0.01; label.outline_size = 0; label.modulate = Color("e5dfcc")
	label.visibility_range_end = 19
	var width := 1.2
	for line in en.split("\n"): width = maxf(width,line.length()*font*0.01*0.57)
	for line in ko.split("\n"): width = maxf(width,line.length()*font*0.01)
	var height := maxf(0.5,maxi(en.split("\n").size(),ko.split("\n").size())*font*0.013)
	visual(label,"SignBacking",Vector3(0,0,-0.035),Vector3(width+0.3,height+0.2,0.06),"273c3d")
func run() -> void:
	map = load(TARGET).instantiate()
	if map.has_meta("art_direction"):
		push_error("Art direction already saved; refusing overwrite."); quit(1); return
	root.add_child(map); map.set_process(false); map.set_meta("art_direction",1)
	grain = NoiseTexture2D.new(); grain.width = 128; grain.height = 128; grain.seamless = true
	var noise := FastNoiseLite.new(); noise.seed = 2407; noise.frequency = 0.18; grain.noise = noise
	var ramp := Gradient.new(); ramp.colors = PackedColorArray([Color(0.92,0.92,0.92),Color(1,1,1)]); grain.color_ramp = ramp
	var original_nodes := map.find_children("*","Node3D",true,false)
	for node in original_nodes:
		var path := str(map.get_path_to(node))
		if node is Label3D:
			node.visible = false; node.visibility_range_end = 19
			if path.begins_with("Decoration/WindingWayfinding") and node.text == "↑": node.visible = true; node.font_size = 44; node.modulate = Color("b1b5a4")
		if node is MeshInstance3D and node.material_override is StandardMaterial3D:
			var tint: Color = node.material_override.albedo_color
			# Ground the palette while retaining functional A/B and parcel colors.
			if path.begins_with("Geometry") or path.begins_with("Decoration"):
				tint = tint.lerp(Color(tint.v,tint.v,tint.v,tint.a),0.45).darkened(0.12)
				finish_material(node,tint)
			if path.begins_with("Decoration/") and not path.contains("WindingWayfinding") and node.mesh is BoxMesh:
				var size: Vector3 = node.mesh.size
				if node.global_position.y < 0.035 and minf(size.x,size.z) < 0.04: node.visible = false
		if node.get_script() == preload("res://scripts/editable_block.gd"):
			var dims: Vector3 = node.dimensions
			var wall := dims.y > 2.3 and minf(dims.x,dims.z) <= 1 and maxf(dims.x,dims.z) >= 3
			if path.begins_with("Geometry"):
				if wall: node.color = Color("b7b6a3")
				elif path == "Geometry/Floor": node.color = Color("606966")
				elif dims.y < 0.5 and node.global_position.y > 4: node.color = Color("3c4b49")
				else: node.color = node.color.lerp(Color("8b8a76"),0.32)
				for mesh in node.get_children():
					if mesh is MeshInstance3D: finish_material(mesh,node.color,wall or path == "Geometry/Floor")
				if wall: trim_wall(node)
		if node is Light3D:
			node.light_color = Color("ece3cf") if node.global_position.x < 4 else Color("d4e1df")
			if node is DirectionalLight3D: node.light_energy = 0.32
			elif path.contains("RoofLight"): node.light_energy = 0.65; node.omni_range = 10
	var env: Environment = map.get_node("Environment/WorldEnvironment1").environment
	env.background_color = Color("263537"); env.ambient_light_color = Color("aab7b6"); env.ambient_light_energy = 0.5
	# Keep the landmark but remove the repeated floating punchlines around it.
	var signs := {
		"Geometry/EnclosedRooms/IntakeRoom/Title":["NO RETURNS  /  INTAKE 01","반품 불가  /  접수 01"],
		"Geometry/EnclosedRooms/IntakeRoom/HatchLabel":["PARCEL TRANSFER","화물 인계 창구"],
		"Geometry/EnclosedRooms/IntakeRoom/HatchLabelOutside":["RECEIVING SIDE","화물 받는 쪽"],
		"Geometry/EnclosedRooms/LostArchive/Title":["02  /  LOST PROPERTY","02  /  분실물 서고"],
		"Geometry/EnclosedRooms/AirMailRoom/Title":["04  /  AIR MAIL","04  /  송풍 구역"],
		"Geometry/EnclosedRooms/DispatchA/Title":["A  /  DISPATCH","A  /  배송구"],
		"Geometry/EnclosedRooms/DispatchB/Title":["B  /  DISPATCH","B  /  배송구"],
		"Gameplay/DispatchA/Sign1":["A","A"],
		"Gameplay/DispatchB/Sign1":["B","B"],
		"Gameplay/PackratTerritory/Sign1":["EMPLOYEE OF THE MONTH","이달의 직원"],
		"Gameplay/RouteChallenges/Gate/Title":["03  /  UNPAID DOORMAN","03  /  무급 문지기"],
		"Gameplay/RouteChallenges/Gate/Instructions":["WEIGHT OPENS DOOR\n6s AFTER RELEASE","발판을 누르면 열립니다\n떠난 뒤 6초 유지"],
		"Gameplay/RouteChallenges/AirMail/Title":["AIR MAIL","송풍 배송"],
		"Gameplay/RouteChallenges/AirMail/Instructions":["AMBER: WAIT\nTEAL: GUST","노란불: 바람 예고\n청록불: 송풍 중"]
	}
	for path in signs:
		var label: Label3D = map.get_node(path); mounted(label,signs[path][0],signs[path][1],58 if path.ends_with("/Sign1") and path.contains("Dispatch") else 28)
	# Main intake plaque lives on the wall, below the boss display.
	map.get_node("Geometry/EnclosedRooms/IntakeRoom/Title").position = Vector3(0,2.5,-9.62)
	for path in ["Gameplay/RouteChallenges/Gate/Status","Gameplay/RouteChallenges/AirMail/Status"]:
		var label: Label3D = map.get_node(path)
		mounted(label,"WEIGH PLATE WITH WORKER OR PARCEL" if path.contains("Gate") else "EXPRESS AIR MAIL!",label.text,24)
	for path in ["Gameplay/RouteChallenges/Gate/PlateFront/Label","Gameplay/RouteChallenges/Gate/PlateBack/Label","Geometry/EnclosedRooms/IntakeRoom/RelaySpotLabel","Geometry/EnclosedRooms/IntakeRoom/Label3D20"]:
		var label: Label3D = map.get_node(path); label.visible = true; label.outline_size = 0; label.font_size = 25; label.modulate = Color("e0d6b6")
	# Roof battens connect the flat slab to a readable structural rhythm.
	for room in map.get_node("Geometry/EnclosedRooms").get_children():
		for child in room.get_children():
			if child.get_script() != preload("res://scripts/editable_block.gd") or not str(child.name).ends_with("Roof"): continue
			var beams := group(child,"RoofBattens")
			for z in range(-int(child.dimensions.z/2)+1,int(child.dimensions.z/2),2):
				visual(beams,"Batten",Vector3(0,-0.24,z),Vector3(child.dimensions.x,0.16,0.14),"65716a")
	own(map)
	var packed := PackedScene.new()
	if packed.pack(map) != OK or ResourceSaver.save(packed,TARGET) != OK: quit(1); return
	var visible_signs := 0
	for label in map.find_children("*","Label3D",true,false):
		if label.visible: visible_signs += 1
	print("ART DIRECTION SAVED: ",visible_signs," visible map labels (including floor arrows)")
	map.queue_free(); await process_frame; quit(0)

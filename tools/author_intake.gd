extends "res://tools/expand_shrine.gd"
# One-time composition pass. All details are baked, owned scene nodes.
const Bevel = preload("res://scripts/bevel_mesh.gd")
func piece(parent: Node, label: String, at: Vector3, size: Vector3, tint: String, bevel := 0.015) -> MeshInstance3D:
	var node := visual(parent,label,at,size,tint)
	node.mesh = Bevel.make(size,bevel); node.visibility_range_end = 22
	return node
func cylinder(parent: Node, label: String, at: Vector3, radius: float, height: float, tint: String) -> MeshInstance3D:
	var node := visual(parent,label,at,Vector3.ONE,tint)
	var mesh := CylinderMesh.new(); mesh.top_radius = radius; mesh.bottom_radius = radius; mesh.height = height; mesh.radial_segments = 20
	node.mesh = mesh; node.visibility_range_end = 22; return node
func ink(parent: Node, label: String, at: Vector3, en: String, ko: String, font := 20) -> Label3D:
	var node := add_sign(parent,label,at,en,ko,font); node.outline_size = 0; node.pixel_size = 0.006; node.modulate = Color("37443d"); node.visibility_range_end = 11; return node
func paper(parent: Node, at: Vector3, angle: float, code: String) -> void:
	var form := group(parent,"Invoice",at); form.rotation.y = angle
	piece(form,"Sheet",Vector3.ZERO,Vector3(0.36,0.012,0.48),"ddd7be",0.001)
	for row in range(4):
		visual(form,"PrintedLine",Vector3(-0.01,0.008,-0.1+row*0.048),Vector3(0.24 if row != 2 else 0.15,0.002,0.004),"7c8274")
	var text := ink(form,"Reference",Vector3(0,0.009,-0.18),code,code,13); text.rotation.x = -PI/2
	var mark := cylinder(form,"InkStamp",Vector3(0.09,0.008,0.15),0.043,0.002,"985b4d"); mark.cast_shadow = GeometryInstance3D.SHADOW_CASTING_SETTING_OFF
func tape(parent: Node, at: Vector3) -> void:
	var ring := visual(parent,"TapeRoll",at,Vector3.ONE,"c3ae75")
	var mesh := TorusMesh.new(); mesh.inner_radius = 0.06; mesh.outer_radius = 0.115; mesh.rings = 20; mesh.ring_segments = 8; ring.mesh = mesh; ring.visibility_range_end = 15
	piece(parent,"TapeTail",at+Vector3(0.17,-0.025,0),Vector3(0.19,0.012,0.065),"c3ae75",0.002)
func stamp(parent: Node, at: Vector3) -> void:
	piece(parent,"StampFoot",at,Vector3(0.24,0.055,0.15),"45483b")
	cylinder(parent,"StampGrip",at+Vector3.UP*0.14,0.055,0.23,"8e493b")
	piece(parent,"InkPad",at+Vector3(0.3,-0.005,0.04),Vector3(0.27,0.045,0.2),"6f3c35")
func worktop(desk: Node3D, role: String) -> void:
	for child in desk.get_children():
		if child is MeshInstance3D or child is Label3D: child.visible = false
	var body = desk.get_node("Desk"); body.edge_bevel = 0.045; body.color = Color("697165")
	var details := group(desk,"AuthoredWorktop")
	piece(details,"RubbedTimberTop",Vector3(0,1.215,0),Vector3(2.44,0.03,1.04),"9b8760")
	for x in [-0.56,0.56]:
		piece(details,"Drawer",Vector3(x,0.77,0.505),Vector3(1.03,0.29,0.025),"7b8273",0.007)
		piece(details,"RecessedPull",Vector3(x,0.8,0.527),Vector3(0.24,0.045,0.02),"35463d",0.005)
	for x in [-0.95,-0.5,0.4]: visual(details,"HandledEdge",Vector3(x,1.233,0.47),Vector3(0.11,0.002,0.025),"c6ad80")
	if role == "packing":
		piece(details,"ScaleBase",Vector3(-0.58,1.3,-0.04),Vector3(0.66,0.15,0.58),"495b55")
		piece(details,"ScalePlate",Vector3(-0.58,1.39,-0.04),Vector3(0.6,0.035,0.51),"9ca89a")
		piece(details,"BlankDisplay",Vector3(-0.58,1.29,0.258),Vector3(0.29,0.055,0.015),"1f302c",0.003)
		tape(details,Vector3(0.52,1.28,-0.22)); paper(details,Vector3(0.51,1.242,0.16),0.12,"NR / 014")
	elif role == "records":
		for i in range(3):
			piece(details,"FileTray",Vector3(-0.62,1.26+i*0.12,-0.07),Vector3(0.72,0.055,0.62),"4d635b")
			piece(details,"UnfiledForms",Vector3(-0.62,1.3+i*0.12,-0.07),Vector3(0.62,0.025,0.52),"d2cdb3")
		paper(details,Vector3(0.23,1.242,0.09),-0.09,"FILE / 07"); stamp(details,Vector3(0.7,1.26,-0.2))
	else:
		paper(details,Vector3(-0.64,1.242,0.02),-0.13,"NR / 001"); stamp(details,Vector3(0,1.26,0))
		piece(details,"ReceiptPrinter",Vector3(0.77,1.4,-0.11),Vector3(0.47,0.34,0.44),"495a50",0.045)
		piece(details,"ReceiptSlot",Vector3(0.77,1.43,0.12),Vector3(0.3,0.025,0.012),"25362e",0.002)
		piece(details,"Receipt",Vector3(0.77,1.31,0.18),Vector3(0.22,0.012,0.14),"d9d1b7",0.002)
func clock_face(parent: Node) -> void:
	var clock := group(parent,"StoppedClock",Vector3(4.6,3,-9.6))
	var rim := cylinder(clock,"Rim",Vector3.ZERO,0.5,0.1,"42554b"); rim.rotation.x = PI/2
	var face := cylinder(clock,"Face",Vector3(0,0,0.063),0.455,0.018,"d4ccb1"); face.rotation.x = PI/2
	for i in range(12):
		var a := i*TAU/12; var tick := piece(clock,"HourTick",Vector3(sin(a)*0.37,cos(a)*0.37,0.081),Vector3(0.025,0.072,0.009),"3b4c42",0.002); tick.rotation.z = -a
	var minute := piece(clock,"MinuteHand",Vector3(0.018,0.17,0.096),Vector3(0.018,0.34,0.01),"394739",0.002); minute.rotation.z = -0.10
	var hour := piece(clock,"HourHand",Vector3(0.11,0.059,0.108),Vector3(0.25,0.027,0.012),"394739",0.002); hour.rotation.z = 0.5
func portrait(parent: Node) -> void:
	var frame := group(parent,"FounderPortrait",Vector3(-4.7,2.8,-9.62)); frame.rotation.z = -0.045
	piece(frame,"Frame",Vector3.ZERO,Vector3(1.25,1.6,0.12),"6a5539",0.025)
	piece(frame,"Paper",Vector3(0,0,0.071),Vector3(1.07,1.42,0.015),"bfb598",0.001)
	piece(frame,"Shoulders",Vector3(0,-0.33,0.085),Vector3(0.75,0.45,0.012),"52645b",0.06)
	piece(frame,"BoxHead",Vector3(0,0.18,0.09),Vector3(0.6,0.61,0.014),"9c855b",0.025)
	for x in [-0.13,0.13]: piece(frame,"Eye",Vector3(x,0.22,0.105),Vector3(0.047,0.067,0.01),"374238",0.006)
	var label := ink(frame,"Plaque",Vector3(0,-0.66,0.095),"FOUNDER / STILL IN CHARGE","창업주 / 아직도 결재 중",15); label.visibility_range_end = 14
func run() -> void:
	map = load(TARGET).instantiate()
	if map.has_meta("authored_intake"):
		push_error("Authored intake already saved; refusing overwrite."); quit(1); return
	root.add_child(map); map.set_process(false); map.set_meta("authored_intake",1)
	map.get_node("Decoration/CardboardBoss").visible = false
	var detailing := group(map.get_node("Decoration"),"AuthoredIntake")
	portrait(detailing); clock_face(detailing)
	var title: Label3D = map.get_node("Geometry/EnclosedRooms/IntakeRoom/Title")
	title.english = "NO RETURNS\n01 / RECEIVING"; title.korean = "반품 불가\n01 / 접수실"; title.font_size = 45; title.position.y = 2.5
	title.get_node("SignBacking").mesh.size = Vector3(3.7,1.22,0.06)
	var divider := map.get_node("Geometry/LowDivider")
	for child in divider.get_children():
		if child is Node3D and child.name != "Solid1": child.visible = false
	divider.get_node("Solid1").edge_bevel = 0.045; divider.get_node("Solid1").color = Color("647060")
	var counter := group(divider,"ReceptionCounter")
	piece(counter,"Countertop",Vector3(0,1.015,0),Vector3(7.04,0.03,0.59),"a18e69",0.025)
	for x in [-2.6,-0.9,0.9,2.6]:
		piece(counter,"FrontPanel",Vector3(x,0.56,0.258),Vector3(1.52,0.73,0.025),"727c69",0.012)
		visual(counter,"RubbedEdge",Vector3(x+0.13,1.032,0.25),Vector3(0.22,0.003,0.033),"c8ad7b")
	paper(counter,Vector3(-2.7,1.043,0),-0.15,"INTAKE / 04"); stamp(counter,Vector3(-1.9,1.06,-0.03))
	# A single service opening is implied by the worn work surface and papers.
	var plaque := ink(counter,"CounterNumber",Vector3(0,0.64,0.28),"01","01",62); plaque.modulate = Color("d0c6a5")
	var interior := map.get_node("Geometry/InteriorDressing")
	for furniture in interior.get_children():
		if furniture.has_node("Desk"):
			var role := "packing" if str(furniture.name) in ["PackingDesk","CentralPacking"] else ("records" if str(furniture.name).contains("Archive") or str(furniture.name) == "LostRegistry" else "receiving")
			worktop(furniture,role)
		for node in furniture.get_children():
			if node.get_script() == preload("res://scripts/editable_block.gd"): node.edge_bevel = 0.025
	# Remove showroom mats only; route paint and functional zones are retained.
	for node in map.get_node("Decoration").find_children("*","MeshInstance3D",true,false):
		if node is MeshInstance3D and node.mesh is BoxMesh:
			var size: Vector3 = node.mesh.size
			var at: Vector3 = node.global_position
			if at.y >= 0 and at.y < 0.1 and absf(at.x) < 8 and absf(at.z) < 9 and size.x > 1 and size.z > 1: node.visible = false
	# Keep the tested throw/catch locations, but paint their edges instead of mats.
	for node in map.get_node("Geometry/EnclosedRooms/IntakeRoom").get_children():
		if node is MeshInstance3D and node.mesh is BoxMesh and node.mesh.size.is_equal_approx(Vector3(1.4,0.025,1.4)):
			node.visible = false
			for side in [-1,1]:
				visual(detailing,"RelayEdge",node.global_position+Vector3(side*0.68,0.005,0),Vector3(0.05,0.008,1.4),"b3b399")
				visual(detailing,"RelayEdge",node.global_position+Vector3(0,0.005,side*0.68),Vector3(1.4,0.008,0.05),"b3b399")
	for marker in map.get_node("Gameplay/CargoSpawns").get_children():
		var at: Vector3 = marker.global_position; at.y = 0.015
		var slot := group(detailing,str(marker.name)+"LoadingMark",at)
		for side in [-1,1]:
			for end in [-1,1]:
				visual(slot,"PaintedCorner",Vector3(side*0.64,0,end*0.5),Vector3(0.055,0.008,0.3),"b8b79c")
				visual(slot,"PaintedCorner",Vector3(side*0.52,0,end*0.64),Vector3(0.29,0.008,0.055),"b8b79c")
	# Reserve one empty shelf bay per rack: visible capacity, not random clutter.
	for rack in map.get_node("Geometry/Shelves").get_children():
		for stock in rack.get_children():
			if not str(stock.name).ends_with("Stock"): continue
			for item in stock.get_children():
				if item is MeshInstance3D and item.position.y > 2.3 and absf(item.position.z-0.4) < 0.01: item.visible = false
	own(map)
	var packed := PackedScene.new()
	if packed.pack(map) != OK or ResourceSaver.save(packed,TARGET) != OK: quit(1); return
	print("AUTHORED INTAKE SAVED")
	map.queue_free(); await process_frame; quit(0)

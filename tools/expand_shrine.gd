extends SceneTree
# One-time, explicitly invoked migration. Normal play/build never calls it.
const TARGET := "res://scenes/maps/shipping_shrine.tscn"
const Block = preload("res://scenes/pieces/solid_block.tscn")
const Sign = preload("res://scripts/map_sign.gd")
const Challenges = preload("res://scripts/route_challenges.gd")
var map: Node3D
func _initialize() -> void: call_deferred("run")
func group(parent: Node, label: String, at := Vector3.ZERO) -> Node3D:
	var node := Node3D.new(); node.name = label; parent.add_child(node); node.position = at; return node
func block(parent: Node, label: String, at: Vector3, size: Vector3, tint: String) -> Node3D:
	var node = Block.instantiate(); node.scene_file_path = ""; node.name = label
	parent.add_child(node); node.position = at; node.dimensions = size; node.color = Color(tint)
	node.get_node("Collision1").name = "CollisionShape3D"
	return node
func visual(parent: Node, label: String, at: Vector3, size: Vector3, tint: String) -> MeshInstance3D:
	var node := MeshInstance3D.new(); node.name = label; node.position = at
	var mesh := BoxMesh.new(); mesh.size = size; node.mesh = mesh
	var material := StandardMaterial3D.new(); material.albedo_color = Color(tint); material.roughness = 0.8
	node.material_override = material; parent.add_child(node); return node
func zone(parent: Node, label: String, at: Vector3, size: Vector3) -> Area3D:
	var node := Area3D.new(); node.name = label; node.position = at
	node.collision_layer = 0; node.collision_mask = 0; node.monitoring = false; node.monitorable = false
	parent.add_child(node)
	var shape := CollisionShape3D.new(); shape.name = "CollisionShape3D"; var box := BoxShape3D.new(); box.size = size; shape.shape = box; node.add_child(shape)
	return node
func add_sign(parent: Node, label: String, at: Vector3, en: String, ko: String, size := 64) -> Label3D:
	var node := Label3D.new(); node.name = label; node.position = at; node.font_size = size; node.pixel_size = 0.01
	node.outline_size = 10; node.modulate = Color("fff1cc"); node.no_depth_test = false
	var font := SystemFont.new(); font.font_names = PackedStringArray(["Malgun Gothic","Arial"]); node.font = font
	node.set_script(Sign); node.english = en; node.korean = ko; parent.add_child(node); return node
func own(node: Node) -> void:
	for child in node.get_children():
		if str(child.name).begins_with("@"):
			child.name = child.get_class()+str(child.get_index()+1)
		child.owner = map
		own(child)
func run() -> void:
	map = load(TARGET).instantiate()
	if map.has_meta("expanded_routes"):
		push_error("Map already expanded; refusing to overwrite editor changes."); quit(1); return
	root.add_child(map); map.set_process(false)
	map.set_meta("expanded_routes",1)
	var geometry := map.get_node("Geometry")
	var floor_node = geometry.get_node("Floor"); floor_node.dimensions = Vector3(48,0.5,60); floor_node.position = Vector3(0,-0.25,-18)
	for side in [-1,1]:
		var wall = geometry.get_node("OuterWalls/LeftWall" if side < 0 else "OuterWalls/RightWall")
		wall.position = Vector3(side*24,1.9,-18); wall.dimensions = Vector3(0.45,3.8,60)
	var back = geometry.get_node("OuterWalls/BackWall"); back.position.z = -48; back.dimensions.x = 48
	var front = geometry.get_node("OuterWalls/FrontWall"); front.position.z = 12; front.dimensions.x = 48
	map.get_node("Gameplay/DispatchA").position = Vector3(-15,0.65,-43)
	map.get_node("Gameplay/DispatchB").position = Vector3(15,0.65,-43)
	var bounds := map.get_node("Gameplay/PlayableBounds")
	bounds.position = Vector3(0,23,-18); bounds.get_node("CollisionShape3D").shape.size = Vector3(50,54,62)
	var courtyard := group(geometry,"PromotionCourtyard")
	for side in [-1,1]:
		block(courtyard,"SortingWingLeft" if side < 0 else "SortingWingRight",Vector3(side*10,1.7,-28),Vector3(16,3.4,0.6),"69526f")
		block(courtyard,"RelayTableLeft" if side < 0 else "RelayTableRight",Vector3(side*6,0.55,-18),Vector3(3,1.1,2),"947249")
		block(courtyard,"RestIslandLeft" if side < 0 else "RestIslandRight",Vector3(side*7,0.4,-37),Vector3(3,0.8,2),"54435f")
	var decor := group(map.get_node("Decoration"),"RouteSigns")
	add_sign(decor,"Choices",Vector3(0,4.5,-17),"CHOOSE YOUR CAREER PATH","당신의 승진 경로를 고르세요",72)
	add_sign(decor,"Relay",Vector3(0,1.8,-18),"RELAY LOUNGE\nSET DOWN. PASS ON. BLAME NEXT SHIFT.","인수인계 쉼터\n내려놓고, 넘기고, 다음 근무 탓하기",40)
	add_sign(decor,"SafeRoute",Vector3(-20.5,3,-25),"SCENIC OVERTIME\nALWAYS OPEN", "안전한 야근길\n항상 열려 있습니다",48)
	for x in [-21,0,21]:
		for z in [-19,-24,-33,-38]:
			var arrow := add_sign(decor,"Arrow",Vector3(x,0.025,z),"↑","↑",90)
			arrow.rotation.x = -PI/2; arrow.modulate = Color("73d5c5") if x != 0 else Color("efbe54")
	for x in [-21,21]:
		visual(decor,"LaneStripe",Vector3(x,0.008,-30),Vector3(3.5,0.012,22),"3d4858" if x < 0 else "315561")
	for x in range(-24,25,4): visual(decor,"NorthGrid",Vector3(x,0.011,-37),Vector3(0.022,0.01,21),"716979")
	for z in range(-46,-26,4): visual(decor,"NorthGrid",Vector3(0,0.011,z),Vector3(48,0.01,0.022),"716979")
	var routes := group(map.get_node("Gameplay"),"RouteChallenges")
	var gate := group(routes,"Gate",Vector3(0,0,-28)); gate.set_meta("_edit_group_",true)
	block(gate,"Door",Vector3(0,1.7,0),Vector3(4,3.4,0.6),"dca741")
	for side in [-1,1]: block(gate,"Frame",Vector3(side*2.2,2,0),Vector3(0.35,4,0.8),"b39a6d")
	block(gate,"Header",Vector3(0,4,0),Vector3(4.75,0.3,0.8),"b39a6d")
	for data in [["PlateFront",5.0],["PlateBack",-5.0]]:
		var plate := zone(gate,data[0],Vector3(0,0,data[1]),Vector3(2.4,1.4,2.4))
		block(plate,"Surface",Vector3(0,0.025,0),Vector3(2.4,0.05,2.4),"edb947")
		var plate_text := add_sign(plate,"Label",Vector3(0,0.06,0),"STAND HERE","여기 서세요",36); plate_text.rotation.x = -PI/2
	zone(gate,"Clearance",Vector3(0,1,0),Vector3(4.5,4,3))
	add_sign(gate,"Title",Vector3(0,5.2,0.1),"UNPAID DOORMAN","무급 문지기",64)
	var gate_status := add_sign(gate,"Status",Vector3(0,4.55,0.2),"WEIGH PLATE WITH WORKER OR PARCEL","직원 또는 상자로 발판을 누르세요",30); gate_status.set_script(null)
	add_sign(gate,"Instructions",Vector3(4.5,2,5),"WEIGHT OPENS DOOR\n6s AFTER YOU LEAVE\nPARCELS COUNT AS STAFF", "발판을 누르면 열립니다\n떠난 뒤 6초 유지\n상자도 직원으로 인정",32)
	var air := group(routes,"AirMail",Vector3(21,0,-29)); air.set_meta("_edit_group_",true)
	zone(air,"WindZone",Vector3(0,1,0),Vector3(4.5,3.2,12))
	block(air,"Indicator",Vector3(-2.6,1.1,5),Vector3(0.4,2.2,0.5),"37636d")
	visual(air,"FanFace",Vector3(0,3.8,7),Vector3(3,2,0.8),"6ca5b0")
	visual(air,"FanMouth",Vector3(0,3.4,7.45),Vector3(2,0.8,0.05),"183341")
	for x in [-0.75,0.75]:
		visual(air,"FanEye",Vector3(x,4.3,7.45),Vector3(0.5,0.5,0.05),"fbf4ce")
	add_sign(air,"Title",Vector3(0,5.5,6.5),"EXPRESS AIR MAIL","특급 바람 배송",52)
	var air_status := add_sign(air,"Status",Vector3(0,2.6,8),"TAKING A BREATH","잠시 숨 고르는 중",38); air_status.set_script(null)
	add_sign(air,"Instructions",Vector3(-4.5,1.8,8),"AMBER: BRACE\nCYAN: FREE SHIPPING", "노란불: 바람 준비\n청록불: 무료 특급 배송",32)
	for side in [-1,1]: block(air,"FanSupport",Vector3(side*2.5,1.8,7),Vector3(0.2,3.6,0.3),"769397")
	routes.set_script(Challenges)
	for x in [-16,0,16]:
		var light := OmniLight3D.new(); light.name = "CourtyardLight"; light.position = Vector3(x,8,-38)
		light.light_color = Color("a8d9ed"); light.light_energy = 0.6; light.omni_range = 17
		map.get_node("Environment").add_child(light)
	var camera: Camera3D = map.get_node("Environment/Overview")
	camera.position = Vector3(23,31,30); camera.look_at(Vector3(0,0,-18)); camera.far = 160
	own(map)
	var packed := PackedScene.new()
	if packed.pack(map) != OK or ResourceSaver.save(packed,TARGET) != OK: quit(1); return
	print("SHRINE EXPANSION SAVED: 48x60m, pressure shortcut and timed air mail")
	map.queue_free(); await process_frame; quit(0)

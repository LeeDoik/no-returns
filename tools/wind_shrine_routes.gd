extends "res://tools/expand_shrine.gd"
# Explicit one-time layout migration; never invoked by the game or build.
func route(parent: Node3D, label: String, points: Array, paint: Node3D, tint: String) -> void:
	var path := group(parent,label)
	for i in range(points.size()):
		var marker := Marker3D.new(); marker.name = "Turn%d" % (i+1); marker.position = points[i]; path.add_child(marker)
	for i in range(points.size()-1):
		var start: Vector3 = points[i]; var finish: Vector3 = points[i+1]
		var direction := finish-start
		var strip := visual(paint,label+"Line",(start+finish)*0.5+Vector3.UP*0.015,Vector3(0.2,0.012,direction.length()),tint)
		strip.rotation.y = atan2(direction.x,direction.z)
		if direction.length() > 5:
			var arrow := add_sign(paint,label+"Arrow",start.lerp(finish,0.5)+Vector3.UP*0.03,"↑","↑",72)
			arrow.rotation = Vector3(-PI/2,atan2(-direction.x,-direction.z),0); arrow.modulate = Color(tint)
func run() -> void:
	map = load(TARGET).instantiate()
	if map.has_meta("winding_routes"):
		push_error("Winding layout already applied; refusing overwrite."); quit(1); return
	root.add_child(map); map.set_process(false); map.set_meta("winding_routes",1)
	var geometry := map.get_node("Geometry")
	var alley := group(geometry,"WindingAlleys")
	var south_left := group(alley,"LeftEntryBend",Vector3(-19.5,0,-4))
	south_left.rotation.y = deg_to_rad(-12)
	block(south_left,"Screen",Vector3(0,1.3,0),Vector3(10,2.6,0.8),"616f86")
	var left_middle := group(alley,"LostPropertyBend",Vector3(-11.5,0,-18))
	block(left_middle,"Screen",Vector3(0,1.4,0),Vector3(13,2.8,0.8),"597e80")
	var tip := block(left_middle,"AngledTip",Vector3(-6.7,1.4,0.6),Vector3(2,2.8,0.8),"597e80"); tip.rotation.y = PI/4
	var left_north := group(alley,"LeftArrivalBend",Vector3(-18.5,0,-35))
	block(left_north,"Screen",Vector3(0,1.2,0),Vector3(13,2.4,0.8),"79627d")
	tip = block(left_north,"AngledTip",Vector3(6.4,1.2,0.7),Vector3(2,2.4,0.8),"79627d"); tip.rotation.y = -PI/4
	var right_south := group(alley,"RightEntryBend",Vector3(19.5,0,-18.5))
	right_south.rotation.y = deg_to_rad(12)
	block(right_south,"Screen",Vector3(0,1.3,0),Vector3(9,2.6,0.8),"8b6d57")
	var right_north := group(alley,"RightArrivalBend",Vector3(19.5,0,-36))
	block(right_north,"Screen",Vector3(0,1.3,0),Vector3(11,2.6,0.8),"79627d")
	tip = block(right_north,"AngledTip",Vector3(-5.4,1.3,0.7),Vector3(2,2.6,0.8),"79627d"); tip.rotation.y = PI/4
	for node in alley.get_children(): node.set_meta("_edit_group_",true)
	# Existing racks now furnish alcoves rather than intersect the new turns.
	geometry.get_node("Shelves/LeftRack0").position = Vector3(-7,0,-16)
	geometry.get_node("Shelves/LeftRack1").position = Vector3(-22,0,-8)
	geometry.get_node("Shelves/LeftRack2").position = Vector3(-21,0,2)
	geometry.get_node("Shelves/RightRack4").position = Vector3(19,0,-16)
	geometry.get_node("PromotionCourtyard/RelayTableLeft").position = Vector3(-3,0.55,-20)
	geometry.get_node("PromotionCourtyard/RestIslandLeft").position = Vector3(-3,0.4,-38)
	geometry.get_node("PromotionCourtyard/RestIslandRight").position = Vector3(3,0.4,-38)
	var air := map.get_node("Gameplay/RouteChallenges/AirMail")
	air.position = Vector3(15,0,-24); air.rotation.y = PI/2
	var decor := map.get_node("Decoration")
	var old_signs := decor.get_node("RouteSigns")
	for child in old_signs.get_children():
		if child is MeshInstance3D or (child is Label3D and child.get("english") == "↑"):
			child.free()
	old_signs.get_node("Choices").position = Vector3(1,4,-19)
	old_signs.get_node("Relay").position = Vector3(0,1.8,-20)
	old_signs.get_node("SafeRoute").position = Vector3(-20.8,3,-21)
	var paint := group(decor,"WindingWayfinding")
	add_sign(paint,"LostProperty",Vector3(-11.5,3.7,-17.5),"LOST & FOUND\nMOSTLY LOST", "분실물 보관소\n대부분 아직 분실 중",48)
	add_sign(paint,"Crossroads",Vector3(0,2,-24),"CHANGE ROUTES HERE\nA ←   ↑ GATE   → B", "여기서 경로를 바꿀 수 있습니다\nA ←   ↑ 문   → B",34)
	add_sign(paint,"Arrival",Vector3(0,3,-34),"DELIVERY IS AROUND THE CORNER", "배송은 모퉁이를 돌면 됩니다",44)
	var paths := group(map,"DesignRoutes")
	route(paths,"LeftLoop",[Vector3(0,0,6.5),Vector3(-11,0,6.5),Vector3(-11,0,-12),Vector3(-21,0,-12),Vector3(-21,0,-30),Vector3(-9,0,-30),Vector3(-9,0,-40),Vector3(-15,0,-40),Vector3(-15,0,-43)],paint,"94d2cb")
	route(paths,"RightLoop",[Vector3(0,0,6.5),Vector3(10,0,6.5),Vector3(10,0,-16),Vector3(12,0,-16),Vector3(12,0,-24),Vector3(20,0,-24),Vector3(20,0,-30),Vector3(11,0,-30),Vector3(11,0,-41),Vector3(15,0,-41),Vector3(15,0,-43)],paint,"f0b973")
	route(paths,"FrontCrossLink",[Vector3(-21,0,-26),Vector3(0,0,-26),Vector3(20,0,-26)],paint,"b19bd2")
	route(paths,"RearCrossLink",[Vector3(-21,0,-30),Vector3(0,0,-30),Vector3(20,0,-30)],paint,"b19bd2")
	own(map)
	var packed := PackedScene.new()
	if packed.pack(map) != OK or ResourceSaver.save(packed,TARGET) != OK: quit(1); return
	print("WINDING MAP SAVED")
	map.queue_free(); await process_frame; quit(0)

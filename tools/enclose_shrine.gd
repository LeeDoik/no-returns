extends "res://tools/expand_shrine.gd"
# Explicit one-time scene migration, never invoked during play or export.
func wall(parent: Node3D, label: String, x: float, z: float, width: float, depth: float, tint: String) -> void:
	block(parent,label,Vector3(x,2.6,z),Vector3(width,5.2,depth),tint)
func roof(parent: Node3D, label: String, at: Vector3, size: Vector3) -> void:
	block(parent,label,at,size,"354555")
	var lamp := OmniLight3D.new(); lamp.name = label+"Light"; lamp.position = at-Vector3.UP*0.7
	lamp.light_color = Color("ffe2b0"); lamp.light_energy = 0.7; lamp.omni_range = 12
	parent.add_child(lamp)
	visual(parent,label+"Lamp",at-Vector3.UP*0.2,Vector3(2,0.08,0.3),"fff1bc")
func run() -> void:
	map = load(TARGET).instantiate()
	if map.has_meta("enclosed_rooms"):
		push_error("Enclosed rooms already applied; refusing to overwrite edits."); quit(1); return
	root.add_child(map); map.set_process(false); map.set_meta("enclosed_rooms",1)
	var rooms := group(map.get_node("Geometry"),"EnclosedRooms")
	var intake := group(rooms,"IntakeRoom")
	wall(intake,"SouthWall",0,9.5,16.6,0.5,"687c86")
	wall(intake,"EastWall",8.1,-2.35,0.5,13.3,"687c86")
	wall(intake,"WestLowerWing",-8.3,1.9,0.5,4.8,"687c86")
	wall(intake,"WestUpperWing",-8.3,-6.25,0.5,5.5,"687c86")
	for x in [-8.3,8.3]: wall(intake,"DoorEnd",x,9.1,0.5,0.8,"687c86")
	# The hatch is 3m across, with vertical opening y=1.80..2.95 (1.15m).
	block(intake,"HatchSill",Vector3(-8.3,0.9,-2),Vector3(0.5,1.8,3),"bd8b49")
	block(intake,"HatchHeader",Vector3(-8.3,4.075,-2),Vector3(0.5,2.25,3),"687c86")
	for x in [-8.3,8.3]: block(intake,"ExitLintel",Vector3(x,4.65,6.5),Vector3(0.5,1.1,4.4),"bd8b49")
	roof(intake,"PackingRoof",Vector3(0,5.7,-5.5),Vector3(17.1,0.3,7.5))
	add_sign(intake,"Title",Vector3(0,4.4,-9.6),"INTAKE / PLEASE PANIC IN ORDER","접수실 / 순서대로 당황하세요",42)
	var hatch := add_sign(intake,"HatchLabel",Vector3(-7.98,3.2,-2),"PARCELS ONLY\nSTAFF: USE DOOR","상자 전용 창구\n직원은 문으로",25); hatch.rotation.y = PI/2
	hatch = add_sign(intake,"HatchLabelOutside",Vector3(-8.62,3.2,-2),"CATCH HERE\nPARCELS ONLY","여기서 받으세요\n상자 전용",25); hatch.rotation.y = -PI/2
	for point in [Vector3(-4,0,-2),Vector3(-12,0,-2)]:
		visual(intake,"RelaySpot",point+Vector3.UP*0.016,Vector3(1.4,0.025,1.4),"7f698c")
		var spot := add_sign(intake,"RelaySpotLabel",point+Vector3.UP*0.04,"THROW" if point.x > -8 else "CATCH","던지기" if point.x > -8 else "받기",25); spot.rotation.x = -PI/2
	var archive := group(rooms,"LostArchive")
	wall(archive,"InnerWall",-17.7,-19.25,0.5,9.5,"537a79")
	wall(archive,"EntryReturn",-20.7,-9.8,6,0.5,"537a79")
	wall(archive,"ExitReturn",-17.95,-24,0.5,0.5,"537a79")
	roof(archive,"ArchiveRoof",Vector3(-20.7,5.7,-19),Vector3(6.5,0.3,8))
	add_sign(archive,"Title",Vector3(-20.7,4,-14.4),"LOST PROPERTY\nYOU ARE NOW PROPERTY","분실물 서고\n직원도 보관 대상",28)
	var gate := group(rooms,"DoormanRoom")
	for x in [-7.0,7.0]:
		wall(gate,"FrontSide",x,-22,0.5,4,"77607d")
		wall(gate,"RearSide",x,-34.1,0.5,4,"77607d")
		block(gate,"CrossLinkLintel",Vector3(x,4.75,-28),Vector3(0.5,0.9,8.2),"a28d9b")
	roof(gate,"GateRoof",Vector3(0,8.2,-28),Vector3(15.5,0.3,12))
	# Lower visual beams frame the room, leaving the raised central door clear.
	for x in [-5.5,5.5]: block(gate,"CeilingBeam",Vector3(x,5.5,-28),Vector3(0.3,0.4,14),"a28d9b")
	var wind := group(rooms,"AirMailRoom")
	wall(wind,"EntrySide",9.4,-21,0.6,0.5,"8c6b4f")
	wall(wind,"EntryWall",18.9,-21,9.6,0.5,"8c6b4f")
	block(wind,"EntryLintel",Vector3(12,4.7,-21),Vector3(4.2,1,0.5),"c49d61")
	roof(wind,"WindRoof",Vector3(16.5,5.7,-24),Vector3(14.5,0.3,6.5))
	add_sign(wind,"Title",Vector3(18,3.5,-20.68),"AIR MAIL\nHOLD YOUR COLLEAGUE","특급 바람실\n동료를 꼭 붙잡으세요",35)
	for side in [-1,1]:
		var dispatch := group(rooms,"DispatchA" if side < 0 else "DispatchB")
		var opening := -9.0 if side < 0 else 11.0
		var left := -23.7 if side < 0 else 7.5
		var right := -6.5 if side < 0 else 23.7
		var color := "5f8192" if side < 0 else "998050"
		var end := opening-2.1
		wall(dispatch,"FrontLeft",(left+end)/2,-38,end-left,0.5,color)
		var start := opening+2.1
		wall(dispatch,"FrontRight",(start+right)/2,-38,right-start,0.5,color)
		wall(dispatch,"InsideWall",-6.5 if side < 0 else 7.5,-43,0.5,10,color)
		block(dispatch,"DoorLintel",Vector3(opening,4.7,-38),Vector3(4.2,1,0.5),"c6ae82")
		roof(dispatch,"DispatchRoof",Vector3(side*15.5,5.7,-45.5),Vector3(16.2,0.3,5))
		add_sign(dispatch,"Title",Vector3(opening,3.7,-37.68),"A / FEED THE DESK" if side < 0 else "B / FEED THE DESK","A / 창구에 먹이 주기" if side < 0 else "B / 창구에 먹이 주기",28)
	for room in rooms.get_children(): room.set_meta("_edit_group_",true)
	# Keep furniture clear of new interior walls; these are still editable clusters.
	map.get_node("Geometry/InteriorDressing/ArchiveDeskLeft").position.x = -3.5
	map.get_node("Geometry/InteriorDressing/ArchiveDeskRight").position.x = 4.5
	map.get_node("Geometry/InteriorDressing/ArchiveLeft").position.x = -3
	map.get_node("Geometry/InteriorDressing/ArchiveRight").position.x = 3
	map.get_node("Gameplay/RouteChallenges/AirMail/Title").position.y = 4.9
	map.get_node("Geometry/InteriorDressing/Reception").position = Vector3(-5,0,8.7)
	map.get_node("Geometry/InteriorDressing/LostParcels").position.x = -15.7
	for plant in map.get_node("Geometry/InteriorDressing").get_children():
		if plant.position.is_equal_approx(Vector3(6.5,0,-34)): plant.position = Vector3(5.8,0,-35.2)
		if plant.position.is_equal_approx(Vector3(-4,0,10)): plant.position.z = 10.5
	# The fan blows local -Z: its face and signs must face the same passage.
	var air := map.get_node("Gameplay/RouteChallenges/AirMail")
	for child in air.get_children():
		if child is Label3D: child.rotation.y = PI
		if child is MeshInstance3D and is_equal_approx(child.position.z,7.45): child.position.z = 6.55
	air.get_node("Status").position.z = 6.35
	air.get_node("Instructions").position = Vector3(0,1.3,6.3)
	own(map)
	var packed := PackedScene.new()
	if packed.pack(map) != OK or ResourceSaver.save(packed,TARGET) != OK: quit(1); return
	print("ENCLOSED ROOMS SAVED")
	map.queue_free(); await process_frame; quit(0)

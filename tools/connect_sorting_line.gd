extends "res://tools/expand_shrine.gd"

func sign_at(parent: Node, label: String, at: Vector3, en: String, ko: String, width := 3.5) -> void:
	visual(parent,label+"Board",at-Vector3(0,0,0.045),Vector3(width,0.85,0.08),"314945")
	var label_node := add_sign(parent,label,at,en,ko,34)
	label_node.pixel_size = 0.006; label_node.outline_size = 0; label_node.visibility_range_end = 15

func run() -> void:
	map = load(TARGET).instantiate(); root.add_child(map); map.set_process(false)
	if map.has_meta("connected_sorting"):
		push_error("Sorting line already applied; edit saved nodes instead."); quit(1); return
	map.set_meta("connected_sorting",1)
	var line := group(map.get_node("Geometry"),"SortingLine")
	var old_wall = map.get_node("Geometry/SortingWall/SortingWall")
	old_wall.visible = false; old_wall.collision_layer = 0
	for shape in old_wall.find_children("*","CollisionShape3D",true,false): shape.disabled = true
	for side in [-1,1]:
		block(line,"IntakeWallLeft" if side<0 else "IntakeWallRight",Vector3(side*4.75,1.7,-10),Vector3(6.5,3.4,0.5),"bac0a8")
	block(line,"ParcelHeader",Vector3(0,2.4,-10),Vector3(3,2,0.5),"60736b")
	var belt = map.get_node("Gameplay/Conveyor")
	belt.position = Vector3(0,0,-11)
	belt.get_node("Bed").mesh = belt.get_node("Bed").mesh.duplicate(); belt.get_node("Bed").mesh.size.z = 8
	var area = belt.get_node("TransportZone")
	area.get_node("CollisionShape3D").shape = area.get_node("CollisionShape3D").shape.duplicate()
	area.get_node("CollisionShape3D").shape.size.z = 8
	for child in belt.get_children():
		if str(child.name).begins_with("Stripe"): child.free()
	for i in range(10): visual(belt,"Stripe%d"%i,Vector3(0,0.08,-3.6+i*0.8),Vector3(2,0.025,0.12),"c9ae69")
	belt.get_node("Lever").position = Vector3(-2,0,3.3)
	for side in [-1,1]:
		block(line,"RailLeft" if side<0 else "RailRight",Vector3(side*1.18,0.15,-11),Vector3(0.12,0.3,8),"6c7b73")
	block(line,"SortingLip",Vector3(0,0.3,-14.45),Vector3(2.2,0.6,0.28),"b6a15e")
	visual(line,"LipTop",Vector3(0,0.615,-14.45),Vector3(2.2,0.025,0.29),"ebd7a0")
	map.get_node("Geometry/EnclosedRooms/IntakeRoom/Title").visible = false
	sign_at(line,"Input",Vector3(0,2.45,-9.69),"PARCELS THROUGH. STAFF AROUND.","택배는 직진 / 직원은 돌아가세요",4.4)
	sign_at(line,"Output",Vector3(-2.4,1.9,-14.2),"SORTING LIP\nLIFT, HOP OR SNEEZE","분류 턱\n들거나, 뛰거나, 재채기",2.8)
	for z in [-7,-12.8,-16.2]:
		var arrow := add_sign(line,"FlowArrow",Vector3(0,0.095,z),"↑","↑",48); arrow.rotation.x = -PI/2; arrow.outline_size = 0
	visual(line,"SneezeLaunchLine",Vector3(0,0.098,-12.2),Vector3(2,0.01,0.09),"d1b3a2")
	# The stock moves into the former rat corner; the sorting exit stays clear.
	map.get_node("Geometry/InteriorDressing/ReadyCrates").position = Vector3(13,0,-5)
	var table = map.get_node("Geometry/PromotionCourtyard/RelayTableRight")
	table.visible = false; table.collision_layer = 0
	for shape in table.find_children("*","CollisionShape3D",true,false): shape.disabled = true
	var nest = map.get_node("Gameplay/PackratTerritory")
	for child in nest.get_children():
		if child is GeometryInstance3D or child is Label3D: child.visible = false
		if child is PhysicsBody3D:
			child.visible = false; child.collision_layer = 0
			for shape in child.find_children("*","CollisionShape3D",true,false): shape.disabled = true
	nest.position = Vector3(6,0,-18)
	nest.get_node("StartPoint").position = Vector3(-0.3,0.05,0)
	nest.get_node("ReturnPoint").position = Vector3(-0.2,0.45,1.3)
	nest.get_node("ActivityZone").position = Vector3(-2.75,2,1.75)
	nest.get_node("ActivityZone/CollisionShape3D").shape = nest.get_node("ActivityZone/CollisionShape3D").shape.duplicate()
	nest.get_node("ActivityZone/CollisionShape3D").shape.size = Vector3(8.5,5,8.5)
	var patrol = nest.get_node("PatrolPoints")
	for child in patrol.get_children(): child.free()
	for point in [Vector3(4,0.05,-18),Vector3(4,0.05,-16.4),Vector3(0,0.05,-16.4),Vector3(0,0.05,-18.5)]:
		var marker := Marker3D.new(); marker.name = "Point%d"%patrol.get_child_count(); patrol.add_child(marker); marker.global_position = point
	var furniture := group(nest,"PaperNest")
	block(furniture,"DeskTop",Vector3(0,1.5,0),Vector3(3,0.16,1.7),"aa9670")
	# Rear supports leave an honest open mouth for the rat and dragged parcels.
	for x in [-1.2,1.2]:
		block(furniture,"Leg",Vector3(x,0.7,-0.65),Vector3(0.15,1.4,0.15),"536d61")
	visual(furniture,"Hole",Vector3(0,0.5,-0.7),Vector3(1.7,1,0.035),"253330")
	for i in range(9):
		var paper := visual(furniture,"Paper",Vector3(-0.7+(i%3)*0.5,0.025+i*0.004,-0.45+(i/3)*0.4),Vector3(0.55,0.018,0.4),"bba77b")
		paper.rotation.y = i*0.57
	sign_at(furniture,"NestSign",Vector3(0,1.95,-0.62),"RETURNS DEPARTMENT\nSHREDDING YOUR PAPERWORK","반송 부서\n서류는 씹어서 처리합니다",3.2)
	for point in [Vector3(4.7,0.025,-17),Vector3(3.5,0.025,-16.6),Vector3(2,0.025,-16.5)]:
		visual(line,"PaperTrail",point,Vector3(0.3,0.015,0.18),"c3b897")
	for item in line.get_children():
		if item.get("edge_bevel") != null: item.edge_bevel = 0.025
	line.set_meta("_edit_group_",true); furniture.set_meta("_edit_group_",true)
	own(map); var packed := PackedScene.new()
	if packed.pack(map) != OK or ResourceSaver.save(packed,TARGET) != OK: quit(1); return
	print("SORTING LINE SAVED"); map.queue_free(); await process_frame; quit(0)

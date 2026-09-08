extends "res://tools/dress_shrine.gd"
func run() -> void:
	map = load(TARGET).instantiate()
	if map.has_meta("interior_stocked"):
		push_error("Interior already stocked; refusing overwrite."); quit(1); return
	root.add_child(map); map.set_process(false); map.set_meta("interior_stocked",1)
	var interior := map.get_node("Geometry/InteriorDressing")
	cluster(interior,"CentralPacking",Vector3(-3,0,-6),"desk","PACKING DEPARTMENT","포장 담당 부서")
	cluster(interior,"PackingSupplies",Vector3(3,0,-6),"piles","EXTRA RESPONSIBILITY","추가 책임 재고")
	cluster(interior,"LateCoffee",Vector3(7,0,-6),"machine","COFFEE IS A PRIVILEGE","커피는 복지입니다")
	cluster(interior,"ReturnsFiles",Vector3(-3,0,-15.5),"cabinet","UNREAD REPORTS","아무도 안 읽은 보고서")
	cluster(interior,"ReadyCrates",Vector3(3,0,-15.5),"piles","URGENT SINCE LAST YEAR","작년부터 긴급 배송")
	cluster(interior,"ArchiveDeskLeft",Vector3(-6,0,-41.5),"desk","APPROVAL DESK A","승인 담당 A")
	cluster(interior,"ArchiveDeskRight",Vector3(6,0,-41.5),"desk","APPROVAL DESK B","승인 담당 B")
	cluster(interior,"WaitingSupplies",Vector3(-16.5,0,-8),"piles","WAITING TO WAIT","대기 순번 대기 중")
	var stock := group(map.get_node("Decoration"),"ShelfStock")
	var index := 0
	for rack in map.get_node("Geometry/Shelves").get_children():
		var contents := group(stock,str(rack.name)+"Stock",rack.position)
		contents.rotation = rack.rotation
		for y in [0.76,1.66,2.56]:
			for z in [-1.15,-0.4,0.4,1.15]:
				var at := Vector3(0,y,z)
				visual(contents,"StoredParcel",at,Vector3(0.7,0.5,0.58),["c4a174","9e7e62","acb299","c48d82"][index%4])
				visual(contents,"Label",at+Vector3(0.365,0,0),Vector3(0.025,0.2,0.3),"e9ddbf")
				index += 1
	# Keep each rack and its stock together when moved in the editor.
	for contents in stock.get_children():
		var rack_name := str(contents.name).trim_suffix("Stock")
		contents.reparent(map.get_node("Geometry/Shelves/"+rack_name),true)
	stock.free()
	var posters := group(map.get_node("Decoration"),"WallPosters")
	var copies := [["SMILE\nON DUTY","근무 중\n웃으세요"],["NO NAPS\nNO RETURNS","낮잠 금지\n반품 금지"],["BOSS IS\nWATCHING","사장님이\n보고 계십니다"],["HAPPY\nOVERTIME","행복한\n추가 근무"]]
	for side in [-1,1]:
		for z in [-43,-37,-31,-25,-19,-13,-7,-1,6]:
			var frame := group(posters,"Poster",Vector3(side*23.65,2.25,z)); frame.rotation.y = -side*PI/2
			visual(frame,"Frame",Vector3.ZERO,Vector3(2.2,1.65,0.08),"a28c6b")
			visual(frame,"Paper",Vector3(0,0,0.05),Vector3(2,1.45,0.02),"e0ce9e")
			var pair: Array = copies[(index+z)%copies.size()]
			var label := add_sign(frame,"Copy",Vector3(0,0,0.08),pair[0],pair[1],24)
			label.modulate = Color("352f3e"); label.outline_size = 0
	for table in map.get_node("Geometry/WorkTables").get_children():
		for i in range(4):
			visual(table,"Paperwork",Vector3(-1+i*0.55,1.16,0),Vector3(0.4,0.16,0.55),"d7c8a6" if i%2 else "a4bca4")
	own(map)
	var packed := PackedScene.new()
	if packed.pack(map) != OK or ResourceSaver.save(packed,TARGET) != OK: quit(1); return
	print("INTERIOR STOCKED: 29 clusters, 96 shelf parcels, 18 wall posters")
	map.queue_free(); await process_frame; quit(0)

extends "res://tools/expand_shrine.gd"
# Baked interior dressing. Every cluster remains editable in the saved scene.
func parcel(parent: Node, label: String, at: Vector3, size: Vector3, tint: String) -> void:
	block(parent,label,at,size,tint)
	visual(parent,"PackingTape",at+Vector3.UP*(size.y/2+0.01),Vector3(0.12,0.02,size.z+0.015),"e2bc70")
func piles(parent: Node3D) -> void:
	for i in range(6):
		var layer := i/3; var x := (i%3-1)*0.75
		parcel(parent,"Parcel",Vector3(x,0.42+layer*0.8,0),Vector3(0.68,0.8,0.75),["ac7e55","ba9b6b","937350"][i%3])
	block(parent,"Pallet",Vector3(0,0.055,0),Vector3(2.5,0.11,1.1),"67533e")
func cabinet(parent: Node3D) -> void:
	block(parent,"Cabinet",Vector3(0,1.35,0),Vector3(2.3,2.7,0.9),"55797e")
	for i in range(3):
		visual(parent,"Door",Vector3((i-1)*0.75,1.35,0.465),Vector3(0.65,2.5,0.025),"86a19c")
		visual(parent,"Handle",Vector3((i-1)*0.75+0.2,1.3,0.51),Vector3(0.055,0.25,0.075),"cfb670")
func desk(parent: Node3D) -> void:
	block(parent,"Desk",Vector3(0,0.6,0),Vector3(2.4,1.2,1.0),"876c58")
	visual(parent,"PaperStack",Vector3(-0.6,1.28,0),Vector3(0.7,0.16,0.5),"f2e8cf")
	visual(parent,"Stamp",Vector3(0.7,1.45,0),Vector3(0.25,0.5,0.25),"cb6d6b")
	visual(parent,"Monitor",Vector3(0.1,1.65,-0.12),Vector3(0.75,0.65,0.12),"253e4a")
	visual(parent,"Screen",Vector3(0.1,1.65,-0.04),Vector3(0.65,0.5,0.02),"94c9a1")
func lounge(parent: Node3D) -> void:
	block(parent,"Bench",Vector3(0,0.42,0),Vector3(2.5,0.84,0.95),"966b87")
	block(parent,"Backrest",Vector3(0,1.05,-0.45),Vector3(2.5,0.7,0.2),"b281a2")
	visual(parent,"Cushion",Vector3(-0.65,0.91,0),Vector3(0.6,0.14,0.7),"d6b176")
	visual(parent,"Cup",Vector3(0.8,1.01,0),Vector3(0.16,0.25,0.16),"e5e4d2")
func machine(parent: Node3D) -> void:
	block(parent,"VendingCabinet",Vector3(0,1.35,0),Vector3(1.6,2.7,1.0),"c89856")
	visual(parent,"Window",Vector3(-0.22,1.65,0.51),Vector3(0.85,1.4,0.025),"284453")
	for i in range(6): visual(parent,"Snack",Vector3(-0.45+(i%2)*0.4,1.25+(i/2)*0.35,0.55),Vector3(0.23,0.22,0.08),"afc685" if i%2 else "e3a189")
	visual(parent,"Button",Vector3(0.55,1.5,0.54),Vector3(0.13,0.2,0.06),"edcf74")
	visual(parent,"DeliverySlot",Vector3(0,0.5,0.51),Vector3(1,0.3,0.04),"392f3d")
func plant(parent: Node3D) -> void:
	block(parent,"Pot",Vector3(0,0.4,0),Vector3(0.8,0.8,0.8),"a87965")
	visual(parent,"Stem",Vector3(0,1.25,0),Vector3(0.12,1.2,0.12),"567558")
	for i in range(4):
		var leaf := visual(parent,"Leaf",Vector3(0,1.5+i*0.13,0),Vector3(1.3,0.14,0.4),"859760")
		leaf.rotation.y = i*0.8; leaf.rotation.z = 0.3
	visual(parent,"Eye",Vector3(0,1.8,0.24),Vector3(0.25,0.25,0.06),"f3e4ae")
func cluster(parent: Node, label: String, at: Vector3, kind: String, en: String, ko: String, angle := 0.0) -> void:
	var node := group(parent,label,at); node.rotation.y = angle; node.set_meta("_edit_group_",true)
	match kind:
		"piles": piles(node)
		"cabinet": cabinet(node)
		"desk": desk(node)
		"lounge": lounge(node)
		"machine": machine(node)
		"plant": plant(node)
	if not en.is_empty(): add_sign(node,"Sign",Vector3(0,3.1,0.1),en,ko,24)
func erase_legacy_paint() -> void:
	for mesh in map.find_children("*","MeshInstance3D",true,false):
		if not mesh.mesh is BoxMesh: continue
		var at: Vector3 = mesh.global_position
		if at.y < 0 or at.y > 0.1 or absf(at.x) < 9 or absf(at.x) > 11: continue
		var size: Vector3 = mesh.mesh.size
		if size.is_equal_approx(Vector3(2.8,0.025,27)) or size.is_equal_approx(Vector3(0.18,0.025,1.1)) or size.is_equal_approx(Vector3(0.18,0.025,0.75)):
			mesh.free()
func run() -> void:
	map = load(TARGET).instantiate()
	if map.has_meta("interior_dressed"):
		push_error("Interior already dressed; refusing overwrite."); quit(1); return
	root.add_child(map); map.set_process(false); map.set_meta("interior_dressed",1)
	erase_legacy_paint()
	var interior := group(map.get_node("Geometry"),"InteriorDressing")
	cluster(interior,"Reception",Vector3(-7,0,10),"desk","COMPLAINTS OPEN NEVER","민원 접수: 영원히 준비 중")
	cluster(interior,"IntakeParcels",Vector3(-19,0,8.7),"piles","RETURN TO SENDER? NO.","반송? 안 됩니다")
	cluster(interior,"StaffLockers",Vector3(18,0,9),"cabinet","TEMPORARY STAFF FOREVER","영원한 임시직")
	cluster(interior,"EntrySnacks",Vector3(22,0,4),"machine","LUNCH SOLD SEPARATELY","점심은 별도 판매",-PI/2)
	cluster(interior,"PackingDesk",Vector3(-6,0,1),"desk","STAMP FIRST. THINK LATER.","도장 먼저, 생각은 나중에")
	cluster(interior,"WaitingBench",Vector3(-20,0,-7.5),"lounge","PLEASE WAIT ANOTHER YEAR","일 년만 더 기다리세요")
	cluster(interior,"LostParcels",Vector3(-16.5,0,-22),"piles","LOST SINCE TUESDAY","화요일부터 분실 중")
	cluster(interior,"LostRegistry",Vector3(-11,0,-22),"desk","WE LOST THE LOST LIST","분실 목록도 분실")
	cluster(interior,"RestBench",Vector3(5,0,-22),"lounge","BREAK TIME: IMAGINARY","휴게 시간: 상상 속")
	cluster(interior,"PromotionFiles",Vector3(-17.5,0,-32.7),"cabinet","PROMOTION PENDING","승진 심사 중")
	cluster(interior,"NightOffice",Vector3(4.5,0,-33.6),"desk","NIGHT SHIFT DAYDREAMS","야근 중 낮잠 금지")
	cluster(interior,"ArchiveLeft",Vector3(-7,0,-45.8),"cabinet","A: ALMOST DELIVERED","A: 거의 배송됨")
	cluster(interior,"ArchiveRight",Vector3(7,0,-45.8),"cabinet","B: BARELY DELIVERED","B: 간신히 배송됨")
	cluster(interior,"NorthParcels",Vector3(-21,0,-43.5),"piles","FRAGILE EGOS","자존심 취급 주의")
	cluster(interior,"NorthSnacks",Vector3(21,0,-43.5),"machine","EMPLOYEE OF THE MEAL","이달의 식사")
	for point in [Vector3(-4,0,10),Vector3(22,0,9),Vector3(-5,0,-23),Vector3(6.5,0,-34),Vector3(-10,0,-46),Vector3(10,0,-46)]:
		cluster(interior,"OfficePlant",point,"plant","","")
	var overhead := group(map.get_node("Decoration"),"InteriorOverhead")
	# Decorative pipes and hangers stay above player/cargo clearance.
	for x in [-22.5,22.5]:
		visual(overhead,"MainPipe",Vector3(x,4.8,-18),Vector3(0.18,0.18,55),"937557")
		for z in [-40,-30,-20,-10,0,9]:
			visual(overhead,"PipeBracket",Vector3(x,4.65,z),Vector3(0.55,0.15,0.15),"b69b71")
	for z in [-7,-20,-37]:
		visual(overhead,"CrossPipe",Vector3(0,5.6,z),Vector3(45,0.14,0.14),"75627e")
		for x in [-18,-9,9,18]:
			visual(overhead,"Pennant",Vector3(x,5,z),Vector3(0.7,0.9,0.06),"b09261" if x < 0 else "638e8c")
	own(map)
	var packed := PackedScene.new()
	if packed.pack(map) != OK or ResourceSaver.save(packed,TARGET) != OK: quit(1); return
	print("INTERIOR DRESSING SAVED: 21 clusters and overhead details")
	map.queue_free(); await process_frame; quit(0)

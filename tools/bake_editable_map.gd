extends SceneTree
const Depot = preload("res://scripts/depot.gd")
const Belt = preload("res://scripts/conveyor.gd")
const Sign = preload("res://scripts/map_sign.gd")
const Block = preload("res://scripts/editable_block.gd")
const Layout = preload("res://scripts/depot_layout.gd")
const TARGET := "res://scenes/maps/shipping_shrine.tscn"
var map: Node3D
var geometry: Node3D
var decoration: Node3D
var environment: Node3D
var gameplay: Node3D
var bay_a: Node3D
var bay_b: Node3D
var territory: Node3D
func _initialize() -> void: call_deferred("bake")
func group(parent: Node, label: String, at: Vector3 = Vector3.ZERO) -> Node3D:
	var node := Node3D.new(); node.name = label; parent.add_child(node); node.position = at; return node
func marker(parent: Node3D, label: String, world: Vector3) -> Marker3D:
	var node := Marker3D.new(); node.name = label; parent.add_child(node); node.global_position = world; return node
func area(parent: Node3D, label: String, world: Vector3, size: Vector3) -> Area3D:
	var node := Area3D.new(); node.name = label; node.collision_layer = 0; node.collision_mask = 0; node.monitoring = false; node.monitorable = false
	parent.add_child(node); node.global_position = world
	var collision := CollisionShape3D.new(); collision.name = "CollisionShape3D"; var shape := BoxShape3D.new(); shape.size = size; collision.shape = shape; node.add_child(collision)
	return node
func move(node: Node, parent: Node, label: String = "") -> void:
	node.reparent(parent,true)
	if not label.is_empty(): node.name = label
func own(node: Node) -> void:
	for child in node.get_children(): child.owner = map; own(child)
func localize(label: Label3D, pair: Array) -> void:
	label.set_script(Sign); label.english = pair[0]; label.korean = pair[1]
func bake() -> void:
	if FileAccess.file_exists(TARGET):
		push_error("Refusing to overwrite the hand-edited map. This migration runs once.")
		quit(1); return
	map = Node3D.new(); map.name = "ShippingShrine"; root.add_child(map)
	environment = group(map,"Environment")
	geometry = group(map,"Geometry")
	decoration = group(map,"Decoration")
	gameplay = group(map,"Gameplay")
	bay_a = group(gameplay,"DispatchA",Layout.BAY_A)
	bay_b = group(gameplay,"DispatchB",Layout.BAY_B)
	for bay in [bay_a,bay_b]: area(bay,"DeliveryZone",bay.global_position+Vector3(0,0.2,0),Vector3(3,1.7,2.6))
	territory = group(gameplay,"PackratTerritory",Layout.NEST)
	marker(territory,"StartPoint",Vector3(10,0.05,-3.2))
	marker(territory,"ReturnPoint",Layout.NEST+Vector3(0,0.55,1.35))
	area(territory,"ActivityZone",Vector3(11.5,2,-9.5),Vector3(6,12,19))
	var patrol := group(territory,"PatrolPoints")
	var points := [Vector3(10,0.05,-3.2),Vector3(10,0.05,-8),Vector3(10,0.05,-14),Vector3(13,0.05,-17)]
	for index in range(4): marker(patrol,"Point%d" % (index+1),points[index])
	area(gameplay,"PlayableBounds",Vector3(0,23,-9),Vector3(34,54,38))
	var workers := group(gameplay,"WorkerSpawns")
	points = [Vector3(-1,0.05,3),Vector3(0.8,0.05,3),Vector3(-1,0.05,5.3),Vector3(0.8,0.05,5.3)]
	for index in range(4): marker(workers,"Worker%d"%(index+1),points[index])
	var cargos := group(gameplay,"CargoSpawns")
	points = [Vector3(0,0.55,1.1),Vector3(-2,0.55,4),Vector3(2,0.55,4),Vector3(4,0.55,1)]
	for index in range(4): marker(cargos,"Cargo%d"%(index+1),points[index])
	var original := Depot.new(); root.add_child(original)
	for sign in original.localized_labels: localize(sign,Depot.TEXT[original.localized_labels[sign]])
	var shrine = original.get_node("ShippingShrine")
	for sign in shrine.signs: localize(sign,shrine.TEXT[shrine.signs[sign]])
	for eye in shrine.eyes: eye.set_meta("shrine_motion","eye")
	for jaw in shrine.jaws: jaw.set_meta("shrine_motion","jaw")
	shrine.stamp.set_meta("shrine_motion","stamp")
	move(shrine.get_node("CardboardBoss"),decoration,"CardboardBoss")
	move(shrine.get_node("AltarA"),bay_a,"HungryFace")
	move(shrine.get_node("AltarB"),bay_b,"HungryFace")
	for node in shrine.get_children():
		if node is Node3D:
			var at: Vector3 = node.global_position
			var parent := territory if at.x > 12 and at.z > -7 and at.z < -3 else decoration
			if at.z < -19: parent = bay_a if at.x < 0 else bay_b
			move(node,parent)
	shrine.free()
	var walls := group(geometry,"OuterWalls")
	var shelving := group(geometry,"Shelves")
	var racks: Array[Node3D] = []
	for side in [-1,1]:
		for z in [-18.0,-13.0,-2.0,3.0]: racks.append(group(shelving,("Left" if side<0 else "Right")+"Rack%d"%racks.size(),Vector3(side*15.25,0,z)))
	var sorting := group(geometry,"SortingWall",Vector3(0,0,-10))
	var divider := group(geometry,"LowDivider",Vector3(1,0,-1.5))
	var tables := group(geometry,"WorkTables")
	var left_table := group(tables,"LeftTable",Vector3(-5.4,0,-12.1))
	var right_table := group(tables,"RightTable",Vector3(5.4,0,-12.1))
	for node in original.get_children():
		if node is Camera3D: move(node,environment,"Overview"); continue
		if node == original.marker: move(node,map,"ThrowMarker"); continue
		if node is WorldEnvironment or node is Light3D: move(node,environment); continue
		if not node is Node3D: move(node,decoration); continue
		var at: Vector3 = node.global_position
		var destination: Node3D = decoration
		if at.y > 6.2: destination = environment
		elif at.y < -0.1:
			move(node,geometry,"Floor"); continue
		elif absf(at.x)>=15.9 or at.z < -26.8 or at.z > 8.9: destination = walls
		elif absf(at.x)>14.6:
			for rack in racks:
				if absf(at.x-rack.position.x)<1 and absf(at.z-rack.position.z)<1.8: destination = rack; break
		elif at.z < -19: destination = bay_a if at.x<0 else bay_b
		elif at.x>11 and at.z>-7 and at.z<-3: destination = territory
		elif absf(at.z+10)<0.5 and absf(at.x)<8.3: destination = sorting
		elif absf(at.z+1.5)<0.7 and at.x>-3 and at.x<5: destination = divider
		elif absf(at.z+12.1)<1.2 and absf(at.x)<8: destination = left_table if at.x<0 else right_table
		move(node,destination)
	original.free()
	# Each editable solid box exposes matching visual/collision size and tint.
	for solid in geometry.find_children("*","StaticBody3D",true,false):
		var mesh: MeshInstance3D
		for child in solid.get_children():
			if child is MeshInstance3D and child.mesh is BoxMesh: mesh = child
		if mesh:
			var size: Vector3 = mesh.mesh.size; var tint: Color = mesh.material_override.albedo_color
			solid.set_script(Block); solid.dimensions = size; solid.color = tint
	var belt := Belt.new(); root.add_child(belt)
	var lever := group(gameplay,"Lever",Layout.LEVER)
	var belt_children := belt.get_children()
	for i in range(belt_children.size()):
		var node = belt_children[i]
		if node == belt.handle: move(node,lever,"Handle")
		elif node is MeshInstance3D and node.position.distance_to(Layout.LEVER+Vector3.UP*0.45)<0.01: move(node,lever,"Post")
		elif i==0: node.name = "Bed"
		else: node.name = "Stripe%d"%i
	for node in belt.get_children(): node.position -= Layout.BELT_CENTER
	move(belt,gameplay,"Conveyor"); belt.position = Layout.BELT_CENTER
	area(belt,"TransportZone",Layout.BELT_CENTER+Vector3.UP*0.8,Vector3(2.2,1.6,6))
	# Lever is grouped with the belt so moving the whole machine moves both.
	move(lever,belt,"Lever")
	map.set_script(load("res://scripts/editable_map.gd"))
	own(map)
	DirAccess.make_dir_recursive_absolute(ProjectSettings.globalize_path("res://scenes/maps"))
	var packed := PackedScene.new()
	if packed.pack(map)!=OK or ResourceSaver.save(packed,TARGET)!=OK:
		push_error("Could not save editable map"); quit(1); return
	print("EDITABLE MAP BAKED: ",TARGET)
	quit(0)

@tool
extends Node3D
const Zone = preload("res://scripts/map_zone.gd")
var overview: Camera3D
var marker: MeshInstance3D
var motions: Array[Dictionary] = []
var elapsed := 0.0
func _ready() -> void:
	if Engine.is_editor_hint(): return
	overview = get_node("Environment/Overview")
	marker = get_node("ThrowMarker")
	_collect_motion(self)
func _collect_motion(node: Node) -> void:
	if node.has_meta("shrine_motion"):
		motions.append({"node":node,"kind":node.get_meta("shrine_motion"),"position":node.position,"rotation":node.rotation})
	for child in node.get_children(): _collect_motion(child)
func _process(delta: float) -> void:
	if Engine.is_editor_hint(): return
	elapsed += delta
	for i in range(motions.size()):
		var item: Dictionary = motions[i]
		var target = item.node
		match item.kind:
			"eye": target.rotation.y = item.rotation.y + sin(elapsed*0.6+i)*0.15
			"jaw": target.position.y = item.position.y + sin(elapsed*1.3+i*1.9)*0.08
			"stamp": target.rotation.z = item.rotation.z + sin(elapsed*0.8)*0.13
func bay(id: int) -> Vector3:
	return get_node("Gameplay/DispatchA" if id == 1 else "Gameplay/DispatchB").global_position
func dock_at(at: Vector3) -> int:
	for id in [1,2]:
		var dock := get_node("Gameplay/DispatchA" if id == 1 else "Gameplay/DispatchB")
		if Zone.contains(dock.get_node("DeliveryZone"),at): return id
	return 0
func outside(at: Vector3) -> bool:
	return not Zone.contains(get_node("Gameplay/PlayableBounds"),at)
func worker_spawn(slot: int) -> Vector3:
	return get_node("Gameplay/WorkerSpawns/Worker%d" % clampi(slot,1,4)).global_position
func cargo_spawn(id: int) -> Vector3:
	return get_node("Gameplay/CargoSpawns/Cargo%d" % id).global_position
func rat_nest() -> Vector3:
	return get_node("Gameplay/PackratTerritory").global_position
func rat_drop() -> Vector3:
	return get_node("Gameplay/PackratTerritory/ReturnPoint").global_position
func rat_start() -> Vector3:
	return get_node("Gameplay/PackratTerritory/StartPoint").global_position
func rat_patrol() -> Array[Vector3]:
	var result: Array[Vector3] = []
	for point in get_node("Gameplay/PackratTerritory/PatrolPoints").get_children():
		if point is Node3D: result.append(point.global_position)
	if result.is_empty(): result.append(rat_start())
	return result
func rat_can_search(at: Vector3) -> bool:
	return Zone.contains(get_node("Gameplay/PackratTerritory/ActivityZone"),at)
func rat_clamp(at: Vector3) -> Vector3:
	return Zone.clamp_point(get_node("Gameplay/PackratTerritory/ActivityZone"),at)
func map_fingerprint() -> String:
	# The map and nested scene/resource dependencies are sent as hashes, never file paths.
	var context := HashingContext.new(); context.start(HashingContext.HASH_SHA256)
	var paths: Array[String] = [scene_file_path]
	var seen: Dictionary = {}
	while not paths.is_empty():
		var path: String = paths.pop_back()
		if seen.has(path): continue
		seen[path] = true
		context.update(path.to_utf8_buffer())
		var stored_path := path
		if FileAccess.file_exists(path+".remap"):
			var remap := ConfigFile.new()
			if remap.load(path+".remap") == OK: stored_path = str(remap.get_value("remap","path",path))
		elif FileAccess.file_exists(path+".import"):
			var imported := ConfigFile.new()
			if imported.load(path+".import") == OK: stored_path = str(imported.get_value("remap","path",path))
		if FileAccess.file_exists(stored_path): context.update(FileAccess.get_file_as_bytes(stored_path))
		var dependencies := ResourceLoader.get_dependencies(path)
		dependencies.sort()
		for dependency in dependencies:
			var resolved: String = dependency.get_slice("::",2) if dependency.contains("::") else dependency
			if resolved.ends_with(".gd") or resolved.ends_with(".gdc"): continue
			if resolved.begins_with("res://"): paths.append(resolved)
	return context.finish().hex_encode()

func _get_configuration_warnings() -> PackedStringArray:
	var warnings := PackedStringArray()
	for path in ["Environment/Overview","ThrowMarker","Gameplay/DispatchA/DeliveryZone/CollisionShape3D","Gameplay/DispatchB/DeliveryZone/CollisionShape3D","Gameplay/Conveyor/TransportZone/CollisionShape3D","Gameplay/Conveyor/Lever/Handle","Gameplay/PackratTerritory/StartPoint","Gameplay/PackratTerritory/ReturnPoint","Gameplay/PackratTerritory/ActivityZone/CollisionShape3D","Gameplay/PlayableBounds/CollisionShape3D","Gameplay/RouteChallenges/Gate/Door/CollisionShape3D","Gameplay/RouteChallenges/Gate/PlateFront/CollisionShape3D","Gameplay/RouteChallenges/Gate/PlateBack/CollisionShape3D","Gameplay/RouteChallenges/AirMail/WindZone/CollisionShape3D"]:
		if not has_node(path): warnings.append("필수 맵 항목 / Required map node: " + path)
	for id in range(1,5):
		for path in ["Gameplay/WorkerSpawns/Worker%d"%id,"Gameplay/CargoSpawns/Cargo%d"%id]:
			if not has_node(path): warnings.append("시작점 누락 / Missing spawn: " + path)
	return warnings

extends SceneTree
var failures := 0
func check(value: bool, message: String) -> void:
	if not value: failures += 1; push_error(message)
func _initialize() -> void: call_deferred("run")
func key(v: Vector3) -> String: return "%.5f,%.5f,%.5f" % [v.x,v.y,v.z]
func run() -> void:
	if not ResourceLoader.exists("res://scripts/bevel_mesh.gd"):
		push_error("Missing beveled geometry implementation"); quit(1); return
	var factory = load("res://scripts/bevel_mesh.gd")
	for size in [Vector3.ONE,Vector3(2.4,1.2,1),Vector3(0.5,0.035,0.18)]:
		var mesh: ArrayMesh = factory.make(size,0.04)
		var arrays := mesh.surface_get_arrays(0)
		var vertices: PackedVector3Array = arrays[Mesh.ARRAY_VERTEX]
		var normals: PackedVector3Array = arrays[Mesh.ARRAY_NORMAL]
		var edges := {}
		check(mesh.get_aabb().size.is_equal_approx(size),"bevel preserves source dimensions")
		for i in range(0,vertices.size(),3):
			var area := (vertices[i+1]-vertices[i]).cross(vertices[i+2]-vertices[i])
			check(area.length() > 0.000001,"no degenerate triangles")
			var center := (vertices[i]+vertices[i+1]+vertices[i+2])/3
			check(normals[i].dot(center) > 0,"surface normals face outward")
			check(area.dot(normals[i]) < 0,"Godot clockwise winding agrees with normals")
			for j in range(3):
				var ends := [key(vertices[i+j]),key(vertices[i+(j+1)%3])]; ends.sort()
				var edge: String = ends[0]+"|"+ends[1]; edges[edge] = int(edges.get(edge,0))+1
		for count in edges.values(): check(count == 2,"closed manifold has two triangles per edge")
	var block = load("res://scenes/pieces/solid_block.tscn").instantiate(); root.add_child(block)
	block.edge_bevel = 0.04; block.dimensions = Vector3(3,2,0.6); block.color = Color("837152")
	for child in block.get_children():
		if child is MeshInstance3D:
			check(child.mesh.get_aabb().size.is_equal_approx(block.dimensions),"editor bevel follows dimension edit")
			check(child.material_override.albedo_color.is_equal_approx(block.color),"editor bevel follows color edit")
		if child is CollisionShape3D: check(child.shape.size == block.dimensions,"editor collision stays synchronized")
	block.queue_free(); await process_frame
	print("BEVEL MESH %s" % ("PASS" if failures == 0 else "FAIL")); quit(1 if failures else 0)

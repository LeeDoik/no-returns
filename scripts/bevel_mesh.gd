@tool
extends RefCounted
# Six inset faces, twelve chamfer strips and eight corner triangles.
static var cache: Dictionary = {}
static func _face(surface: SurfaceTool, points: Array[Vector3], normal: Vector3) -> void:
	var n := normal.normalized()
	if (points[1]-points[0]).cross(points[2]-points[0]).dot(n) > 0: points.reverse()
	var tangent := n.cross(Vector3.UP).normalized() if absf(n.y) < 0.99 else Vector3.RIGHT
	for i in range(1,points.size()-1):
		for point in [points[0],points[i],points[i+1]]:
			surface.set_normal(n); surface.set_uv(Vector2(point.dot(tangent),point.dot(n.cross(tangent)))); surface.add_vertex(point)
static func make(size: Vector3, bevel: float) -> ArrayMesh:
	var half := size*0.5
	var cut := clampf(bevel,0.0001,minf(half.x,minf(half.y,half.z))*0.45)
	var id := str(size)+":"+str(cut)
	if cache.has(id): return cache[id]
	var surface := SurfaceTool.new(); surface.begin(Mesh.PRIMITIVE_TRIANGLES)
	for axis in range(3):
		var u := (axis+1)%3; var v := (axis+2)%3
		for side in [-1,1]:
			var points: Array[Vector3] = []; var normal := Vector3.ZERO; normal[axis] = side
			for corner in [Vector2(-1,-1),Vector2(1,-1),Vector2(1,1),Vector2(-1,1)]:
				var p := Vector3.ZERO; p[axis] = half[axis]*side; p[u] = (half[u]-cut)*corner.x; p[v] = (half[v]-cut)*corner.y; points.append(p)
			_face(surface,points,normal)
	for along in range(3):
		var a := (along+1)%3; var b := (along+2)%3
		for sa in [-1,1]:
			for sb in [-1,1]:
				var p := Vector3.ZERO; p[a] = sa*half[a]; p[b] = sb*(half[b]-cut); p[along] = -(half[along]-cut)
				var q := p; q[a] = sa*(half[a]-cut); q[b] = sb*half[b]
				var r := q; r[along] *= -1
				var s := p; s[along] *= -1
				var normal := Vector3.ZERO; normal[a] = sa; normal[b] = sb
				_face(surface,[p,q,r,s],normal)
	for x in [-1,1]:
		for y in [-1,1]:
			for z in [-1,1]:
				var direction := Vector3(x,y,z); var points: Array[Vector3] = []
				for axis in range(3):
					var p := direction*(half-Vector3.ONE*cut); p[axis] = direction[axis]*half[axis]; points.append(p)
				_face(surface,points,direction)
	var mesh := surface.commit()
	if cache.size() >= 128: cache.clear()
	cache[id] = mesh
	return mesh

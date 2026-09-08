@tool
@static_unload
extends RefCounted
# Shared visual assets. Physics and replicated state remain in their owners.
const DIRECTORY := "res://assets/art/release-01/"
static var models: Dictionary = {}
static var materials: Dictionary = {}

static func model(id: String) -> Node3D:
	if not models.has(id): models[id] = load(DIRECTORY + id + ".glb")
	var instance: Node3D = models[id].instantiate()
	finish(instance)
	return instance

static func finish(node: Node) -> void:
	if node is MeshInstance3D:
		var size: Vector3 = node.mesh.get_aabb().size
		if minf(size.x,minf(size.y,size.z)) < 0.012: node.cast_shadow = GeometryInstance3D.SHADOW_CASTING_SETTING_OFF
		for i in range(node.mesh.get_surface_count()):
			var source = node.mesh.surface_get_material(i)
			if not source is StandardMaterial3D: continue
			var key: String = source.resource_name
			var texture := "cardboard" if key == "Kraft cardboard" else ("paint" if key == "Sage enamel" else "")
			if texture.is_empty(): continue
			if not materials.has(key):
				var material: StandardMaterial3D = source.duplicate()
				material.albedo_color = Color.WHITE
				material.albedo_texture = load(DIRECTORY+"textures/"+texture+"_basecolor.png")
				material.normal_enabled = true
				material.normal_texture = load(DIRECTORY+"textures/"+texture+"_normal.png")
				material.normal_scale = 0.025
				material.uv1_triplanar = true
				material.uv1_scale = Vector3.ONE * 1.4
				materials[key] = material
			node.set_surface_override_material(i,materials[key])
	for child in node.get_children(): finish(child)

static func find_part(node: Node, prefix: String) -> Node3D:
	for child in node.get_children():
		if str(child.name).begins_with(prefix): return child
		var found := find_part(child,prefix)
		if found: return found
	return null

static func expression(node: Node3D, phase: String, opening: float) -> void:
	for face in ["Idle","Warning","Burst"]:
		var part := find_part(node,"Face"+face)
		if part: part.visible = face == phase
	var front := find_part(node,"LidFrontPivot")
	var back := find_part(node,"LidBackPivot")
	if front: front.rotation.x = -opening
	if back: back.rotation.x = opening

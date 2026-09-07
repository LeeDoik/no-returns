extends Node3D

var overview: Camera3D
var marker: MeshInstance3D

func _ready() -> void:
	var environment := WorldEnvironment.new()
	var settings := Environment.new()
	settings.background_mode = Environment.BG_COLOR
	settings.background_color = Color("14232e")
	settings.ambient_light_source = Environment.AMBIENT_SOURCE_COLOR
	settings.ambient_light_color = Color("c4dded")
	settings.ambient_light_energy = 0.28
	settings.tonemap_mode = Environment.TONE_MAPPER_FILMIC
	environment.environment = settings
	add_child(environment)
	var light := DirectionalLight3D.new()
	light.rotation_degrees = Vector3(-55, -28, 0)
	light.light_color = Color("fff0d9")
	light.light_energy = 0.65
	light.shadow_enabled = true
	add_child(light)
	box(Vector3(20, 0.5, 23), Vector3(0, -0.25, -1), Color("35454e"), true)
	box(Vector3(20, 3, 0.5), Vector3(0, 1.5, -12.5), Color("22313d"), true)
	box(Vector3(0.5, 3, 23), Vector3(-10, 1.5, -1), Color("22313d"), true)
	box(Vector3(0.5, 3, 23), Vector3(10, 1.5, -1), Color("22313d"), true)
	box(Vector3(20, 1.2, 0.5), Vector3(0, 0.6, 10.5), Color("22313d"), true)
	for x in range(-8, 10, 2):
		box(Vector3(0.025, 0.015, 21), Vector3(x, 0.01, -1), Color("4c5a60"))
	for z in range(-10, 10, 2):
		box(Vector3(18, 0.015, 0.025), Vector3(0, 0.01, z), Color("4c5a60"))
	box(Vector3(7, 1.0, 0.5), Vector3(1.0, 0.5, -1.5), Color("b7a473"), true)
	box(Vector3(7.1, 0.08, 0.6), Vector3(1.0, 1.04, -1.5), Color("edaf43"))
	for x in range(-2, 5):
		box(Vector3(0.35, 0.06, 0.65), Vector3(x, 1.09, -1.5), Color("343e41"))
	for x in [-4.5, 4.5]:
		var correct: bool = x < 0
		var color := Color("54c4b2") if correct else Color("df9366")
		box(Vector3(3, 0.035, 2.6), Vector3(x, 0.02, -7), color.darkened(0.25))
		for edge in [-1.55, 1.55]:
			box(Vector3(0.10, 0.045, 2.8), Vector3(x + edge, 0.04, -7), color)
		box(Vector3(3.2, 0.045, 0.1), Vector3(x, 0.04, -5.6), color)
		box(Vector3(3.2, 0.045, 0.1), Vector3(x, 0.04, -8.4), color)
		box(Vector3(3.2, 0.5, 0.4), Vector3(x, 0.25, -8.6), Color("263e47"), true)
		label("A / DISPATCH" if correct else "B / DISPATCH", Vector3(x, 1.0, -8.6), color, 56)
		label("A" if correct else "B", Vector3(x, 0.14, -7), Color.WHITE, 120, -90)
	box(Vector3(3.2, 0.03, 2.4), Vector3(0, 0.02, 1.1), Color("635c43"))
	label("INTAKE / RECOVERY", Vector3(0, 0.08, 2), Color("f6cd78"), 38, -90)
	label("NO RETURNS", Vector3(0, 2.0, -12.18), Color("efc780"), 110)
	label("SHIP FIRST. ASK LATER.", Vector3(0, 1.15, -12.18), Color("b4c5cc"), 40)
	overview = Camera3D.new()
	add_child(overview)
	overview.position = Vector3(12, 13, 17)
	overview.look_at(Vector3(0, 0, -2))
	overview.current = true
	marker = MeshInstance3D.new()
	var ring := TorusMesh.new()
	ring.inner_radius = 0.32
	ring.outer_radius = 0.39
	marker.mesh = ring
	var mat := StandardMaterial3D.new()
	mat.albedo_color = Color("ffdc7d")
	mat.shading_mode = BaseMaterial3D.SHADING_MODE_UNSHADED
	marker.material_override = mat
	marker.visible = false
	add_child(marker)
	box(Vector3(2, 0.03, 1.8), Vector3(-2, 0.02, 4), Color("534762"))
	label("SNEEZER", Vector3(-2, 0.08, 4.6), Color("e6c2f4"), 30, -90)
	box(Vector3(1.7, 0.03, 1.7), Vector3(2, 0.02, 4), Color("4c6440"))
	label("CLINGER", Vector3(2, 0.08, 4.6), Color("d6efa6"), 30, -90)

	box(Vector3(1.7, 0.03, 1.7), Vector3(4, 0.02, 1), Color("715235"))
	label("HOPPER", Vector3(4, 0.08, 1.6), Color("ffd394"), 30, -90)

func box(size: Vector3, at: Vector3, color: Color, solid: bool = false) -> Node3D:
	var parent: Node3D = StaticBody3D.new() if solid else Node3D.new()
	add_child(parent)
	parent.position = at
	var mesh := MeshInstance3D.new()
	var cube := BoxMesh.new()
	cube.size = size
	mesh.mesh = cube
	var material := StandardMaterial3D.new()
	material.albedo_color = color
	material.roughness = 0.9
	mesh.material_override = material
	parent.add_child(mesh)
	if solid:
		var collision := CollisionShape3D.new()
		var shape := BoxShape3D.new()
		shape.size = size
		collision.shape = shape
		parent.add_child(collision)
	return parent

func label(text: String, at: Vector3, color: Color, size: int, angle: float = 0) -> Label3D:
	var sign := Label3D.new()
	sign.text = text
	sign.font_size = size
	sign.pixel_size = 0.01
	sign.modulate = color
	sign.position = at
	sign.rotation_degrees.x = angle
	sign.outline_size = 0
	add_child(sign)
	return sign

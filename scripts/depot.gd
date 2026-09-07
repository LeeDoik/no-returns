extends Node3D

const Copy = preload("res://scripts/copy.gd")
const Shrine = preload("res://scripts/shrine_decor.gd")
const Layout = preload("res://scripts/depot_layout.gd")
const TEXT := {
	"sort": ["SACRED PAPERWORK", "신성한 서류 작업"], "route": ["WALK WITH PURPOSE", "경건하게 뛰세요"],
	"intake": ["OFFERING RECEPTION", "제물 접수처"], "dispatch_a": ["01  DISPATCH A", "01  출고 A"],
	"dispatch_b": ["02  DISPATCH B", "02  출고 B"], "nest": ["EXECUTIVE SUITE", "임원 전용실"],
	"table_a": ["A / ↑↑", "A / ↑↑"], "table_b": ["B / ◆", "B / ◆"]}

var overview: Camera3D
var marker: MeshInstance3D
var localized_labels: Dictionary = {}
var shown_language := ""

func _ready() -> void:
	_build_environment(); _build_shell(); _build_routes(); _build_sorting_area()
	_build_dispatch(); _build_intake(); _build_nest(); _build_lighting(); _build_camera_and_marker(); _refresh_labels()
	add_child(Shrine.new())

func _process(_delta: float) -> void:
	if shown_language != Copy.language: _refresh_labels()

func _build_environment() -> void:
	var world := WorldEnvironment.new(); var settings := Environment.new()
	settings.background_mode = Environment.BG_COLOR; settings.background_color = Color("2e2341")
	settings.ambient_light_source = Environment.AMBIENT_SOURCE_COLOR; settings.ambient_light_color = Color("bac5d2")
	settings.ambient_light_energy = 0.34; settings.reflected_light_source = Environment.REFLECTION_SOURCE_DISABLED
	settings.tonemap_mode = Environment.TONE_MAPPER_FILMIC; world.environment = settings; add_child(world)
	var moon := DirectionalLight3D.new(); moon.rotation_degrees = Vector3(-58, -32, 0)
	moon.light_color = Color("ffe6be"); moon.light_energy = 0.52; moon.shadow_enabled = true; add_child(moon)

func _build_shell() -> void:
	box(Vector3(Layout.FLOOR_SIZE.x, 0.5, Layout.FLOOR_SIZE.y), Layout.FLOOR_CENTER, Color("51465f"), true)
	box(Vector3(32, 3.8, 0.45), Vector3(0, 1.9, -27), Color("493452"), true)
	box(Vector3(0.45, 3.8, 36), Vector3(-16, 1.9, -9), Color("574362"), true)
	box(Vector3(0.45, 3.8, 36), Vector3(16, 1.9, -9), Color("574362"), true)
	box(Vector3(32, 1.2, 0.45), Vector3(0, 0.6, 9), Color("574362"), true)
	for side in [-1, 1]:
		for z in [-18.0, -13.0, -2.0, 3.0]: _shelf(Vector3(side * 15.25, 0, z), side)

func _build_routes() -> void:
	for x in [-12, -8, -4, 4, 8, 12]: box(Vector3(0.035, 0.012, 34), Vector3(x, 0.012, -9), Color("8c7c98"))
	for z in range(-24, 9, 4): box(Vector3(30, 0.012, 0.035), Vector3(0, 0.013, z), Color("8c7c98"))
	for x in [-10.0, 10.0]:
		box(Vector3(2.8, 0.025, 27), Vector3(x, 0.025, -8.5), Color("746983"))
		for z in [-19.0, -14.0, -9.0, -4.0, 1.0, 6.0]: _floor_arrow(Vector3(x, 0.05, z), Color("8fc6bf"))
		_local_label("route", Vector3(x, 0.065, 6.4), Color("b8ddd7"), 24, -90)

func _build_sorting_area() -> void:
	var wall := box(Vector3(16, 3.4, 0.5), Vector3(0, 1.7, -10), Color("65516e"), true); wall.name = "SortingWall"
	box(Vector3(16.2, 0.12, 0.68), Vector3(0, 3.43, -10), Color("d5963e"))
	for x in [-6.0, -3.0, 0.0, 3.0, 6.0]: box(Vector3(0.09, 2.7, 0.03), Vector3(x, 1.7, -9.73), Color("40525b"))
	_local_label("sort", Vector3(0, 1.65, -9.72), Color("f5c96b"), 34)
	_worktable(Vector3(-5.4, 0, -12.1), "table_a", Color("55b6ac")); _worktable(Vector3(5.4, 0, -12.1), "table_b", Color("d7865f"))
	box(Vector3(7, 1.0, 0.5), Vector3(1.0, 0.5, -1.5), Color("9e8b5d"), true)
	box(Vector3(7.1, 0.08, 0.6), Vector3(1.0, 1.04, -1.5), Color("e4a542"))
	for x in range(-2, 5): box(Vector3(0.35, 0.06, 0.65), Vector3(x, 1.09, -1.5), Color("29373c"))

func _build_dispatch() -> void:
	_dispatch_bay(1, Layout.BAY_A, Color("55c7b7")); _dispatch_bay(2, Layout.BAY_B, Color("e28e62"))

func _dispatch_bay(id: int, center: Vector3, accent: Color) -> void:
	var landmark := Node3D.new(); landmark.name = "DispatchA" if id == 1 else "DispatchB"; landmark.position = center; add_child(landmark)
	box(Vector3(3, 0.035, 2.6), Vector3(center.x, 0.02, center.z), accent.darkened(0.42))
	for edge in [-1.55, 1.55]: box(Vector3(0.11, 0.05, 2.8), Vector3(center.x + edge, 0.05, center.z), accent)
	box(Vector3(3.2, 0.05, 0.11), Vector3(center.x, 0.05, center.z + 1.4), accent); box(Vector3(3.2, 0.05, 0.11), Vector3(center.x, 0.05, center.z - 1.4), accent)
	label("A / ↑↑" if id == 1 else "B / ◆", Vector3(center.x, 0.11, center.z), Color.WHITE, 88, -90)

func _build_intake() -> void:
	box(Vector3(5.2, 0.03, 3.0), Vector3(0, 0.02, 2.3), Color("5d563f")); _local_label("intake", Vector3(0, 0.07, 2.25), Color("f5cf75"), 34, -90)
	box(Vector3(2, 0.03, 1.8), Vector3(-2, 0.02, 4), Color("51415d")); label("SNEEZER", Vector3(-2, 0.08, 4.6), Color("e5c1f2"), 28, -90)
	box(Vector3(1.7, 0.03, 1.7), Vector3(2, 0.02, 4), Color("465c3a")); label("CLINGER", Vector3(2, 0.08, 4.6), Color("d5efa7"), 28, -90)
	box(Vector3(1.7, 0.03, 1.7), Vector3(4, 0.02, 1), Color("674a30")); label("HOPPER", Vector3(4, 0.08, 1.6), Color("ffd393"), 28, -90)

func _build_nest() -> void:
	var nest := Node3D.new(); nest.name = "PackratNest"; nest.position = Layout.NEST; add_child(nest)
	box(Vector3(3.2, 0.16, 3.0), Vector3(13, 0.08, -5), Color("493c30"))
	for offset in [Vector3(-0.9, 0.27, -0.65), Vector3(0.75, 0.26, -0.8), Vector3(-0.55, 0.23, 0.75), Vector3(0.85, 0.22, 0.65)]: box(Vector3(1.05, 0.34, 0.55), Layout.NEST + offset, Color("7a6042"))
	box(Vector3(1.2, 0.8, 0.9), Vector3(13, 0.48, -5), Color("263a40")); _local_label("nest", Vector3(13, 1.25, -5), Color("e4b970"), 27)

func _build_lighting() -> void:
	for data in [[Vector3(-10, 6.5, -21), Color("9fe8dc"), 0.55], [Vector3(10, 6.5, -21), Color("ffd0a3"), 0.55], [Vector3(-7, 6.5, -5), Color("b8dced"), 0.45], [Vector3(7, 6.5, -5), Color("b8dced"), 0.45], [Vector3(0, 6.5, 3), Color("ffe0ad"), 0.4]]:
		var light := OmniLight3D.new(); light.position = data[0]; light.light_color = data[1]; light.light_energy = data[2]
		light.omni_range = 11.0; light.shadow_enabled = true; add_child(light); box(Vector3(0.8, 0.08, 0.8), data[0], data[1])

func _build_camera_and_marker() -> void:
	overview = Camera3D.new(); add_child(overview); overview.position = Vector3(15, 18, 18); overview.look_at(Vector3(0, 0, -9)); overview.current = true
	marker = MeshInstance3D.new(); var ring := TorusMesh.new(); ring.inner_radius = 0.32; ring.outer_radius = 0.39; marker.mesh = ring
	var mat := StandardMaterial3D.new(); mat.albedo_color = Color("ffdc7d"); mat.shading_mode = BaseMaterial3D.SHADING_MODE_UNSHADED
	marker.material_override = mat; marker.visible = false; add_child(marker)

func _shelf(at: Vector3, side: int) -> void:
	# One simple rack collision keeps the shelving physical without filling the scene tree with shelf colliders.
	_collider(Vector3(0.82, 2.75, 3.42), Vector3(at.x, 1.38, at.z))
	for y in [0.45, 1.35, 2.25]: box(Vector3(1.0, 0.09, 3.5), Vector3(at.x, y, at.z), Color("3f515a"))
	for z_offset in [-1.65, 1.65]: box(Vector3(0.11, 2.8, 0.11), Vector3(at.x + side * 0.35, 1.4, at.z + z_offset), Color("202f36"))

func _worktable(at: Vector3, key: String, accent: Color) -> void:
	box(Vector3(3.8, 0.16, 1.45), at + Vector3(0, 1.0, 0), Color("52616a"), true)
	for offset in [Vector3(-1.55, 0.48, -0.5), Vector3(1.55, 0.48, -0.5), Vector3(-1.55, 0.48, 0.5), Vector3(1.55, 0.48, 0.5)]: box(Vector3(0.16, 0.96, 0.16), at + offset, Color("26373f"), true)
	_local_label(key, at + Vector3(0, 1.1, 0), accent, 30, -90)

func _floor_arrow(at: Vector3, color: Color) -> void:
	box(Vector3(0.18, 0.025, 1.1), at, color)
	box(Vector3(0.18, 0.025, 0.75), at + Vector3(-0.25, 0, -0.38), color).rotation_degrees.y = -38
	box(Vector3(0.18, 0.025, 0.75), at + Vector3(0.25, 0, -0.38), color).rotation_degrees.y = 38

func _local_label(key: String, at: Vector3, color: Color, size: int, angle: float = 0) -> Label3D:
	var sign := label("", at, color, size, angle); localized_labels[sign] = key; return sign

func _refresh_labels() -> void:
	shown_language = Copy.language; var index := 1 if Copy.language == "ko" else 0
	for sign in localized_labels:
		if is_instance_valid(sign): sign.text = TEXT[localized_labels[sign]][index]

func box(size: Vector3, at: Vector3, color: Color, solid: bool = false) -> Node3D:
	var parent: Node3D = StaticBody3D.new() if solid else Node3D.new(); add_child(parent); parent.position = at
	var mesh := MeshInstance3D.new(); var cube := BoxMesh.new(); cube.size = size; mesh.mesh = cube
	var material := StandardMaterial3D.new(); material.albedo_color = color; material.roughness = 0.82; mesh.material_override = material; parent.add_child(mesh)
	if solid:
		var collision := CollisionShape3D.new(); var shape := BoxShape3D.new(); shape.size = size; collision.shape = shape; parent.add_child(collision)
	return parent

func _collider(size: Vector3, at: Vector3) -> StaticBody3D:
	var body := StaticBody3D.new(); body.position = at; add_child(body)
	var collision := CollisionShape3D.new(); var shape := BoxShape3D.new(); shape.size = size
	collision.shape = shape; body.add_child(collision); return body

func label(text: String, at: Vector3, color: Color, size: int, angle: float = 0) -> Label3D:
	var sign := Label3D.new(); sign.text = text; sign.font_size = size; sign.pixel_size = 0.01; sign.modulate = color
	sign.position = at; sign.rotation_degrees.x = angle; sign.outline_size = 5; sign.outline_modulate = Color(0.02, 0.04, 0.05, 0.85); add_child(sign); return sign

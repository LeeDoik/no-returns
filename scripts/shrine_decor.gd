extends Node3D

const Copy = preload("res://scripts/copy.gd")
const TEXT := {
	"boss": ["THE BOX IS ALWAYS RIGHT", "상자는 언제나 옳다"],
	"a": ["A / FEED ME", "A / 밥 줘"],
	"b": ["B / STILL HUNGRY", "B / 아직 배고파"],
	"employee": ["EMPLOYEE OF THE MONTH", "이달의 직원"],
	"rat": ["STOLE 100% OF THE CREDIT", "실적을 전부 훔쳤습니다"],
	"offering": ["OFFERINGS IN. COMPLAINTS OUT.", "제물은 접수. 불만은 반송."],
	"promotion": ["THE PATH TO PROMOTION", "승진의 길"],
	"kneel": ["PLEASE DO NOT LICK THE ALTAR", "제단을 핥지 마세요"],
	"approved": ["APPROVED-ish", "대충 승인"]
}
var signs: Dictionary = {}
var language := ""
var eyes: Array[Node3D] = []
var jaws: Array[Node3D] = []
var stamp: Node3D
var elapsed := 0.0

func _ready() -> void:
	name = "ShippingShrine"
	_boss()
	_face(-10, Color("61d0bf"), "a", false)
	_face(10, Color("ed8fad"), "b", true)
	_employee()
	_sign("offering", Vector3(0, 1.3, 8.7), 31, Color("f6df91"), PI)
	_sign("promotion", Vector3(12.5, 2.6, -9), 21, Color("f6df91"))
	_refresh()

func _cube(parent: Node3D, at: Vector3, size: Vector3, tint: Color) -> Node3D:
	var shape := BoxMesh.new(); shape.size = size
	return _mesh(parent, at, shape, tint)

func _ball(parent: Node3D, at: Vector3, size: Vector3, tint: Color) -> Node3D:
	var shape := SphereMesh.new(); shape.radius = 0.5; shape.height = 1.0
	var result := _mesh(parent, at, shape, tint); result.scale = size
	return result

func _mesh(parent: Node3D, at: Vector3, shape: Mesh, tint: Color) -> MeshInstance3D:
	var node := MeshInstance3D.new(); node.mesh = shape; node.position = at
	var material := StandardMaterial3D.new(); material.albedo_color = tint; material.roughness = 0.85
	node.material_override = material; parent.add_child(node); return node

func _eye(parent: Node3D, at: Vector3, size: float, wonky: float = 0) -> void:
	_ball(parent, at, Vector3(size, size, size * 0.42), Color("fff6d6"))
	var pupil := _ball(parent, at + Vector3(wonky, 0, size * 0.22), Vector3(size * 0.32, size * 0.42, 0.13), Color("242038"))
	eyes.append(pupil)

func _boss() -> void:
	var boss := Node3D.new(); boss.name = "CardboardBoss"; add_child(boss); boss.position = Vector3(0, 5.0, -10.8)
	_cube(boss, Vector3.ZERO, Vector3(5.0, 3.8, 2.9), Color("ba9457"))
	_cube(boss, Vector3(0, 0, 1.47), Vector3(0.65, 3.8, 0.025), Color("f2dca0"))
	_eye(boss, Vector3(-1.15, 0.75, 1.55), 1.45, 0.18)
	_eye(boss, Vector3(1.15, 0.55, 1.55), 1.15, -0.18)
	_cube(boss, Vector3(0.2, -0.63, 1.51), Vector3(1.45, 0.48, 0.1), Color("392b45"))
	_cube(boss, Vector3(0.4, -0.75, 1.59), Vector3(0.44, 0.26, 0.08), Color("e28d95"))
	_cube(boss, Vector3(0, -2.0, 1.5), Vector3(0.5, 0.9, 0.15), Color("b44d85")).rotation.z = 0.22
	_cube(boss, Vector3(0, 2.12, 0), Vector3(5.4, 0.38, 3.0), Color("efb84f"))
	for x in [-2.2, -1.1, 0.0, 1.1, 2.2]:
		_cube(boss, Vector3(x, 2.7, 0), Vector3(0.5, 0.9 + absf(x)*0.18, 0.55), Color("efb84f"))
	for side in [-1, 1]:
		_cube(boss, Vector3(side * 4, -0.7, 0.3), Vector3(3, 0.65, 0.8), Color("ba9457")).rotation.z = side * 0.22
		_ball(boss, Vector3(side * 5.2, -0.35, 0.3), Vector3(1.2, 1.0, 1.0), Color("fff1c7"))
	stamp = Node3D.new(); boss.add_child(stamp); stamp.position = Vector3(5.3, 0.4, 0.3)
	_cube(stamp, Vector3(0, 0.7, 0), Vector3(0.45, 1.4, 0.5), Color("61384e"))
	_cube(stamp, Vector3.ZERO, Vector3(2.4, 0.55, 1.0), Color("d77591"))
	_sign("approved", Vector3(5.3, 5.4, -9.95), 22, Color("fff1c7"))
	_sign("boss", Vector3(0, 2.65, -9.65), 42, Color("fff1c7"))

func _face(x: float, tint: Color, key: String, sleepy: bool) -> void:
	var head := Node3D.new(); head.name = "Altar" + key.to_upper(); add_child(head); head.position = Vector3(x, 3.5, -25.3)
	_cube(head, Vector3.ZERO, Vector3(5.3, 7.0, 2.8), tint)
	_cube(head, Vector3(0, -1.55, 1.43), Vector3(4.1, 3.0, 0.08), Color("251d37"))
	for side in [-1,1]:
		_eye(head, Vector3(side*1.28, 1.5, 1.5), 1.65, side * (-0.18 if sleepy else 0.12))
		_cube(head, Vector3(side*1.25, 2.38, 1.68), Vector3(1.65, 0.28, 0.18), Color("59405b")).rotation.z = side * (0.18 if sleepy else -0.22)
		_ball(head, Vector3(side*2.2, -0.4, 1.48), Vector3(0.65, 0.6, 0.1), Color("e7989d"))
	for tooth_x in [-1.35,-0.45,0.45,1.35]:
		_cube(head, Vector3(tooth_x, -0.27, 1.55), Vector3(0.58, 0.4, 0.16), Color("fff6d6"))
	var jaw := _cube(head, Vector3(0, -3.13, 1.55), Vector3(4.25, 0.22, 0.34), tint.lightened(0.18))
	jaws.append(jaw)
	_cube(self, Vector3(x, 0.045, -22.6), Vector3(2.85, 0.06, 3.6), Color("c97494"))
	_cube(self, Vector3(x, 0.079, -22.6), Vector3(0.045, 0.012, 3.1), Color("934767"))
	_sign(key, Vector3(x, 3.98, -23.7), 34, Color("fff6d6"))
	var halo := TorusMesh.new(); halo.inner_radius = 1.3; halo.outer_radius = 1.47
	var ring := _mesh(head, Vector3(0, 4.15, 0), halo, Color("f6d278")); ring.rotation.z = -0.12 if sleepy else 0.12
	_sign("kneel", Vector3(x, 0.13, -20.2), 20, Color("fff1c7"), 0, -PI/2)

func _employee() -> void:
	_cube(self, Vector3(14.55, 2.1, -5), Vector3(0.18, 3.6, 3.6), Color("684b74"))
	_sign("employee", Vector3(14.4, 3.5, -5), 23, Color("f6df91"), -PI/2)
	_sign("rat", Vector3(14.4, 1.0, -5), 17, Color("fce7d6"), -PI/2)
	_ball(self, Vector3(14.25, 2.25, -5), Vector3(0.35, 1.1, 1.4), Color("ba966d"))
	for z in [-5.45,-4.55]: _ball(self, Vector3(14.15, 2.9, z), Vector3(0.24, 0.48, 0.45), Color("e6b8a0"))
	for z in [-5.24,-4.76]:
		_ball(self, Vector3(14.02, 2.4, z), Vector3(0.1, 0.19, 0.17), Color("302438"))
	_ball(self, Vector3(13.96, 2.12, -5), Vector3(0.16, 0.15, 0.18), Color("674057"))
	for side in [-1,1]:
		for dy in [-0.12,0.0,0.12]:
			_cube(self, Vector3(14.0, 2.12+dy, -5+side*0.55), Vector3(0.03,0.025,0.65), Color("e7c8a9"))
	_cube(self, Vector3(13, 1.7, -5), Vector3(1.1, 0.2, 0.9), Color("f4c556"))
	for x in [12.6,13.0,13.4]: _cube(self, Vector3(x, 2.0, -5), Vector3(0.2, 0.55, 0.35), Color("f4c556"))

func _sign(key: String, at: Vector3, size: int, tint: Color, yaw: float = 0, pitch: float = 0) -> void:
	var sign := Label3D.new(); add_child(sign); sign.position = at; sign.rotation = Vector3(pitch, yaw, 0)
	var font := SystemFont.new(); font.font_names = PackedStringArray(["Malgun Gothic", "Segoe UI"])
	sign.font = font; sign.font_size = size; sign.pixel_size = 0.01; sign.modulate = tint; sign.outline_size = 5
	signs[sign] = key

func _refresh() -> void:
	language = Copy.language
	for sign in signs: sign.text = TEXT[signs[sign]][1 if language == "ko" else 0]

func _process(delta: float) -> void:
	elapsed += delta
	if language != Copy.language: _refresh()
	for i in range(eyes.size()): eyes[i].rotation.y = sin(elapsed * 0.6 + i) * 0.15
	for i in range(jaws.size()): jaws[i].position.y = -3.13 + sin(elapsed * 1.3 + i * 1.9) * 0.08
	if stamp: stamp.rotation.z = sin(elapsed * 0.8) * 0.13

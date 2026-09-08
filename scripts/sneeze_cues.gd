extends Node3D

const Copy = preload("res://scripts/copy.gd")
const Sneeze = preload("res://scripts/sneeze_rules.gd")
static var muted := false
static var warning_sound: AudioStreamWAV
static var sneeze_sound: AudioStreamWAV

var body: RigidBody3D
var visual: Node3D
var face: Node3D
var nose: MeshInstance3D
var eyes: Array[MeshInstance3D] = []
var caption: Label3D
var zone: MeshInstance3D
var burst: Node3D
var burst_text: Label3D
var puffs: Array[MeshInstance3D] = []
var puff_material: StandardMaterial3D
var player: AudioStreamPlayer3D
var seen_event := 0
var last_phase := "calm"
var burst_left := 0.0
var bursts_played := 0

func setup(cargo_body: RigidBody3D, cargo_visual: Node3D) -> void:
	body = cargo_body
	visual = cargo_visual
	face = Node3D.new()
	visual.add_child(face)
	for x in [-0.2, 0.2]:
		var eye := _sphere(face, 0.065, Color("22303e"))
		eye.position = Vector3(x, 0.19, -0.415)
		eyes.append(eye)
	nose = _sphere(face, 0.19, Color("f37e96"))
	nose.position = Vector3(0, -0.02, -0.49)
	caption = _label(36)
	add_child(caption)
	zone = MeshInstance3D.new()
	var lines := ImmediateMesh.new()
	lines.surface_begin(Mesh.PRIMITIVE_LINES)
	var previous := Vector3.ZERO
	for index in range(21):
		var angle := deg_to_rad(-Sneeze.HALF_ANGLE_DEGREES + index * Sneeze.HALF_ANGLE_DEGREES / 10.0)
		var point := Vector3(sin(angle), 0, -cos(angle)) * Sneeze.RANGE
		lines.surface_add_vertex(previous)
		lines.surface_add_vertex(point)
		previous = point
	lines.surface_add_vertex(previous)
	lines.surface_add_vertex(Vector3.ZERO)
	lines.surface_add_vertex(Vector3.ZERO)
	lines.surface_add_vertex(Vector3(0, 0, -2.8))
	lines.surface_add_vertex(Vector3(0, 0, -2.8))
	lines.surface_add_vertex(Vector3(-0.4, 0, -2.3))
	lines.surface_add_vertex(Vector3(0, 0, -2.8))
	lines.surface_add_vertex(Vector3(0.4, 0, -2.3))
	lines.surface_end()
	zone.mesh = lines
	zone.material_override = _material(Color("ffcf60"), true)
	add_child(zone)
	burst = Node3D.new()
	add_child(burst)
	puff_material = _material(Color(0.9, 1, 0.96, 0.8), true)
	puff_material.transparency = BaseMaterial3D.TRANSPARENCY_ALPHA
	for index in range(7):
		var puff := _sphere(burst, 0.14 + index * 0.025, Color.WHITE)
		puff.material_override = puff_material
		puffs.append(puff)
	burst_text = _label(62)
	burst_text.position = Vector3(0, 0.8, -1.2)
	burst.add_child(burst_text)
	player = AudioStreamPlayer3D.new()
	player.volume_db = -13
	player.max_distance = 18
	add_child(player)
	if warning_sound == null:
		warning_sound = _sound(false)
		sneeze_sound = _sound(true)
	cancel()

func _label(font_size: int) -> Label3D:
	var label := Label3D.new()
	var font := SystemFont.new()
	font.font_names = PackedStringArray(["Malgun Gothic", "Segoe UI", "Arial"])
	label.font = font
	label.font_size = font_size
	label.pixel_size = 0.006
	label.visibility_range_end = 16
	label.billboard = BaseMaterial3D.BILLBOARD_ENABLED
	label.outline_size = 6
	label.no_depth_test = false
	return label

func _material(color: Color, unshaded: bool = false) -> StandardMaterial3D:
	var material := StandardMaterial3D.new()
	material.albedo_color = color
	if unshaded:
		material.shading_mode = BaseMaterial3D.SHADING_MODE_UNSHADED
	return material

func _sphere(parent: Node, radius: float, color: Color) -> MeshInstance3D:
	var mesh := MeshInstance3D.new()
	var sphere := SphereMesh.new()
	sphere.radius = radius
	sphere.height = radius * 2
	mesh.mesh = sphere
	mesh.material_override = _material(color)
	mesh.cast_shadow = GeometryInstance3D.SHADOW_CASTING_SETTING_OFF
	parent.add_child(mesh)
	return mesh

func present(clock: RefCounted, yaw: float, origin: Vector3, burst_yaw: float, enabled: bool) -> void:
	if not enabled:
		seen_event = maxi(seen_event, clock.event_id)
		cancel()
		return
	caption.visible = clock.phase == "windup"
	caption.position = body.position + Vector3(0, 1.0, 0)
	caption.text = ""
	# The face inherits the cargo model's facing; world-space cues use yaw below.
	face.rotation.y = 0
	var winding: bool = clock.phase == "windup"
	var progress: float = clampf(1 - clock.remaining / Sneeze.WINDUP_SECONDS, 0, 1) if winding else 0.0
	visual.scale = Vector3.ONE * (1 + progress * 0.18)
	nose.scale = Vector3.ONE * (1 + progress * 0.6)
	for eye in eyes:
		eye.scale.y = 1 - progress * 0.8
	zone.visible = winding
	zone.position = Vector3(body.position.x, 0.07, body.position.z)
	zone.rotation.y = yaw
	if winding:
		caption.font_size = 36
		caption.text = Copy.get_text("sneeze_warning") % maxf(0, clock.remaining)
		caption.modulate = Color("ffd86f")
		if last_phase != "windup":
			_play(warning_sound, body.position)
	else:
		caption.font_size = 26
		caption.modulate = Color.WHITE
	if clock.event_id > seen_event:
		seen_event = clock.event_id
		if clock.phase == "burst":
			bursts_played += 1
			burst_left = Sneeze.BURST_SECONDS
			burst.position = origin
			burst.rotation.y = burst_yaw
			_play(sneeze_sound, origin)
	last_phase = clock.phase

func _process(delta: float) -> void:
	if muted:
		player.stop()
	burst_left = maxf(0, burst_left - delta)
	burst.visible = burst_left > 0
	if not burst.visible:
		return
	var progress := 1.0 - burst_left / Sneeze.BURST_SECONDS
	burst_text.text = Copy.get_text("achoo")
	puff_material.albedo_color.a = (1 - progress) * 0.85
	for index in range(puffs.size()):
		puffs[index].position = Vector3(sin(index * 2.4) * (0.2 + progress * 0.65), progress * 0.4, -0.65 - progress * (1.8 + index * 0.3))
		puffs[index].scale = Vector3.ONE * (0.6 + progress * 2.2)

func begin_epoch() -> void:
	seen_event = 0
	cancel()

func cancel() -> void:
	burst_left = 0
	last_phase = "calm"
	if zone:
		zone.visible = false
		burst.visible = false
		caption.visible = false
		visual.scale = Vector3.ONE
		player.stop()

func _play(stream: AudioStreamWAV, at: Vector3) -> void:
	if muted or DisplayServer.get_name() == "headless":
		return
	player.position = at
	player.stream = stream
	player.play()

static func _sound(is_burst: bool) -> AudioStreamWAV:
	var rate := 22050
	var duration := 0.28 if is_burst else 0.18
	var count := int(rate * duration)
	var data := PackedByteArray()
	data.resize(count * 2)
	var rng := RandomNumberGenerator.new()
	rng.seed = 53
	for index in range(count):
		var t := float(index) / rate
		var envelope := sin(PI * t / duration)
		var value := (rng.randf_range(-1, 1) * 0.65 + sin(TAU * 110 * t) * 0.35) if is_burst else sin(TAU * (440 * t + 500 * t * t))
		data.encode_s16(index * 2, int(value * envelope * 14000))
	var sound := AudioStreamWAV.new()
	sound.format = AudioStreamWAV.FORMAT_16_BITS
	sound.mix_rate = rate
	sound.data = data
	return sound

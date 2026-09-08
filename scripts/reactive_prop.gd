@tool
extends Node3D

const Art = preload("res://scripts/postal_art.gd")
const Bevel = preload("res://scripts/bevel_mesh.gd")
const Cues = preload("res://scripts/sneeze_cues.gd")
@export_enum("Packing cushion", "Return spring", "Carton tower", "Paper stack") var kind := 0:
	set(value):
		kind = clampi(value,0,3)
		if is_inside_tree(): _build.call_deferred()
@export_range(0.6,2.0,0.1) var trigger_radius := 1.0
@export_range(1.0,4.0,0.1) var effect_radius := 2.2
@export_range(3.0,10.0,0.5) var lift_speed := 4.5
@export_range(2.0,20.0,0.5) var reset_seconds := 8.0
var phase := 0 # ready, compressing, spent
var remaining := 0.0
var event_id := 0
var occupied: Dictionary = {}
var visual: Node3D
var pieces: Array[Node3D] = []
var origins: Array[Vector3] = []
var burst_age := 9.0
var sound: AudioStreamPlayer3D
var burst_direction := Vector3.FORWARD
var paper_velocity: Array[Vector3] = []
var paper_resting: Array[bool] = []
var paper_fanned := false
static var sounds: Dictionary = {}

func _ready() -> void: _build()
func _build() -> void:
	if is_instance_valid(visual): visual.free()
	pieces.clear(); origins.clear()
	visual = Node3D.new(); visual.name = "Preview"; add_child(visual)
	if kind != 3: _part(Vector3(1.65,0.04,1.65),Vector3(0,0.025,0),Color("354b48"))
	match kind:
		0:
			var model := Art.model("packing_cushion"); visual.add_child(model)
			for cell in model.find_children("Heat sealed cell*","MeshInstance3D",true,false):
				pieces.append(cell); origins.append(cell.position)
		1:
			var model := Art.model("return_spring"); visual.add_child(model)
			var top := Art.find_part(model,"Top"); pieces.append(top); origins.append(top.position)
		2:
			for i in range(3):
				var pivot := Node3D.new(); visual.add_child(pivot); pivot.position = Vector3(0,0.3+i*0.47,0)
				var model := Art.model("standard"); pivot.add_child(model); model.scale = Vector3(1.08,0.57,0.97)
				pieces.append(pivot); origins.append(pivot.position)
		3:
			for i in range(18):
				var p := _part(Vector3(0.56,0.009,0.39),Vector3(sin(i*2.3)*0.025,0.015+i*0.01,cos(i)*0.02),Color("e2d8b8"))
				pieces.append(p); origins.append(p.position)
				for j in range(3):
					var ink := _part(Vector3(0.25,0.002,0.008),Vector3.ZERO,Color("8c9385")); ink.reparent(p,false); ink.position = Vector3(-0.04,0.006,-0.09+j*0.035)
				var stamp := _part(Vector3(0.09,0.002,0.04),Vector3.ZERO,Color("b37462")); stamp.reparent(p,false); stamp.position = Vector3(0.12,0.006,0.12)
	if kind != 3:
		var label := Label3D.new(); label.text = ["POP","UP ↑","TILT"][kind]; label.font_size = 38; label.pixel_size = 0.004; label.outline_size = 0
		label.position = Vector3(0,0.055,0.60); label.rotation.x = -PI/2; label.modulate = Color("f1deb0"); visual.add_child(label)
	if not Engine.is_editor_hint() and not sound:
		sound = AudioStreamPlayer3D.new(); sound.max_distance = 16; sound.unit_size = 3; sound.volume_db = -15; add_child(sound)
	_present()

func _part(size: Vector3, at: Vector3, tint: Color) -> MeshInstance3D:
	var mesh := MeshInstance3D.new(); mesh.mesh = Bevel.make(size,0.018); mesh.position = at
	var mat := StandardMaterial3D.new(); mat.albedo_color = tint; mat.roughness = 0.88; mesh.material_override = mat; visual.add_child(mesh); return mesh

func reset() -> void:
	phase = 0; remaining = 0; event_id = 0; occupied.clear(); burst_age = 9; burst_direction = Vector3.FORWARD; _present()
	if sound: sound.stop()

func arm(direction: Vector3 = Vector3.ZERO) -> void:
	if phase != 0: return
	if direction.length_squared() > 0.1: burst_direction = direction.normalized()
	phase = 1; remaining = 0.18

func fire() -> void:
	phase = 2; remaining = reset_seconds; event_id += 1; burst_age = 0; _begin_paper(); _play(); _present()

func _process(delta: float) -> void:
	if Engine.is_editor_hint(): return
	burst_age += delta
	if sound and Cues.muted: sound.stop()
	if kind == 3 and phase == 2: _paper_step(minf(delta,0.05))
	_present()

func _present() -> void:
	if not is_instance_valid(visual): return
	if kind == 3 and phase == 2: return
	for i in range(pieces.size()):
		var p := pieces[i]; p.position = origins[i]; p.rotation = Vector3.ZERO; p.scale = Vector3.ONE
		if phase == 1: p.scale.y = 0.7
		if phase != 2: continue
		var t := minf(burst_age,1.0)
		match kind:
			0:
				p.scale.y = 0.16
				p.position += Vector3(sin(i*2.4),0,cos(i*2.4))*t*0.7 + Vector3.UP*maxf(0,2*t-3*t*t)
				p.rotation.y = t*(i-2)*0.5
			1:
				p.position.y += maxf(0,sin(minf(burst_age*5,PI)))*0.48
				p.scale.y = 0.55
			2:
				p.position = origins[i].lerp(Vector3((i-1)*0.45,0.25,-0.4-i*0.38),minf(1,t*3))
				p.rotation.x = minf(1,t*3)*(-1.1-i*0.15)

func apply_state(state: Array) -> void:
	var old_event := event_id
	phase = int(state[0]); remaining = float(state[1]); event_id = int(state[2])
	burst_direction = state[3]
	if event_id > old_event: burst_age = 0; _begin_paper(); _play()
	if event_id < old_event or phase == 0: burst_age = 9
	_present()

func _begin_paper() -> void:
	if kind != 3: return
	paper_velocity.clear(); paper_resting.clear(); paper_fanned = false
	for i in range(pieces.size()):
		pieces[i].position = origins[i]; pieces[i].rotation = Vector3.ZERO; pieces[i].scale = Vector3.ONE
		paper_velocity.append(burst_direction*(6.0+i*0.035)+Vector3.UP*(2.8+i*0.018))
		paper_resting.append(false)

func _paper_step(delta: float) -> void:
	if paper_velocity.size() != pieces.size(): return
	if burst_age > 0.14 and not paper_fanned:
		var across := Vector3(-burst_direction.z,0,burst_direction.x)
		for i in range(pieces.size()):
			paper_velocity[i] += across*sin(i*2.4)*3.6 + Vector3.UP*cos(i*1.7)*1.5 + burst_direction*sin(i*0.9)*1.8
		paper_fanned = true
	for i in range(pieces.size()):
		if paper_resting[i]: continue
		var sheet := pieces[i]
		var v := paper_velocity[i]
		if burst_age > 0.14:
			v.x += (sin(i*2.1+burst_age*5)*2.3-v.x*1.8)*delta
			v.z += (cos(i*1.7+burst_age*4)*2.3-v.z*1.8)*delta
			v.y = maxf(-0.85,v.y-5.0*delta)
		var from := sheet.global_position; var motion := v*delta
		var ray := PhysicsRayQueryParameters3D.create(from,from+motion+motion.normalized()*0.04,1)
		var hit := get_world_3d().direct_space_state.intersect_ray(ray)
		if not hit.is_empty():
			sheet.global_position = hit.position+hit.normal*0.035
			v = v.slide(hit.normal)*0.2
			if hit.normal.y > 0.5: paper_resting[i] = true; sheet.rotation = Vector3(0,i*0.6,0)
		else: sheet.global_position += motion
		if not paper_resting[i] and burst_age > 0.14:
			sheet.rotation = Vector3(sin(burst_age*8+i)*0.6,burst_age*(i%3-1)*1.2,cos(burst_age*7+i)*0.5)
		paper_velocity[i] = v

func _play() -> void:
	if not sound or Cues.muted or DisplayServer.get_name() == "headless": return
	if not sounds.has(kind): sounds[kind] = _make_sound()
	sound.stream = sounds[kind]; sound.play()

func _make_sound() -> AudioStreamWAV:
	var rng := RandomNumberGenerator.new(); rng.seed = 832+kind
	var bytes := PackedByteArray(); var count := 8820; bytes.resize(count*2)
	for i in range(count):
		var t := float(i)/22050
		var noise := rng.randf_range(-1,1)
		var wave := noise*exp(-t*28)*0.5 + sin(TAU*85*t)*exp(-t*18)*0.3
		if kind == 1: wave = sin(TAU*(230*t-180*t*t))*exp(-t*8)*0.55
		if kind == 2: wave = noise*(exp(-t*45)+0.5*exp(-absf(t-0.11)*55)+0.3*exp(-absf(t-0.22)*60))*0.45
		if kind == 3: wave = noise*sin(PI*t/0.4)*exp(-t*4)*0.3
		bytes.encode_s16(i*2,int(clampf(wave,-0.9,0.9)*23000))
	var result := AudioStreamWAV.new(); result.format = AudioStreamWAV.FORMAT_16_BITS; result.mix_rate = 22050; result.data = bytes; return result

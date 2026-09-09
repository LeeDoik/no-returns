extends Node3D
const Cargo = preload("res://scripts/cargo.gd")
const Worker = preload("res://scripts/worker.gd")
const DEFAULTS = {"mass":3.0,"friction":0.65,"bounce":0.06,"gravity":9.81,"damping":0.6,"throw":9.5}
var settings = DEFAULTS.duplicate()
var parcels: Array = []
var homes: Array[Vector3] = []
var worker
var panel: PanelContainer
var status: Label
var fields := {}
var held = null
var jump_requested := false
var elapsed := 0.0
var message := ""
var ground_material := PhysicsMaterial.new()

func _ready():
 var environment := WorldEnvironment.new()
 environment.environment = Environment.new()
 environment.environment.background_mode = Environment.BG_COLOR
 environment.environment.background_color = Color("233346")
 environment.environment.ambient_light_source = Environment.AMBIENT_SOURCE_COLOR
 environment.environment.ambient_light_color = Color("dce8fa")
 environment.environment.ambient_light_energy = 0.7
 add_child(environment)
 var light := DirectionalLight3D.new()
 light.rotation_degrees = Vector3(-55,-25,0); light.shadow_enabled = true
 add_child(light)
 ground_material.friction = 0.65
 block(Vector3(34,0.4,28),Vector3(0,-0.2,0),Color("455667"))
 for x in range(-16,17,2):
  block(Vector3(0.025,0.005,28),Vector3(x,0.004,0),Color("637384"),false)
 for z in range(-14,15,2):
  block(Vector3(34,0.005,0.025),Vector3(0,0.004,z),Color("637384"),false)
 for x in [-17,17]: block(Vector3(0.3,1,28),Vector3(x,0.5,0),Color("263848"))
 for z in [-14,14]: block(Vector3(34,1,0.3),Vector3(0,0.5,z),Color("263848"))
 label("01 / DROP 4 m",Vector3(-10,2,-5))
 label("02 / RAMP 20 deg",Vector3(-3,3,-7))
 label("03 / STACK x6",Vector3(5,3,-6))
 label("04 / CARRY & WALL",Vector3(11,3,-2))
 var ramp = block(Vector3(4,0.25,7),Vector3(-3,1.32,-7),Color("457f88"))
 ramp.rotation_degrees.x = 20
 block(Vector3(0.3,2.5,7),Vector3(10,1.25,-1),Color("b77949"))
 block(Vector3(0.3,2.5,7),Vector3(11.8,1.25,-1),Color("b77949"))
 block(Vector3(2.1,2.5,0.3),Vector3(10.9,1.25,-4.5),Color("b77949"))
 worker = Worker.new(); worker.peer_id = 1; add_child(worker)
 worker.position = Vector3(0,0.05,8); worker.make_local()
 homes = [Vector3(-10,4.4,-4),Vector3(-3,3.6,-9)]
 for i in range(6): homes.append(Vector3(5,0.41+i*0.81,-5))
 homes.append(Vector3(0,0.41,5)); homes.append(Vector3(10.9,0.41,3.5))
 for home in homes:
  var parcel = Cargo.new(); parcel.home = home; add_child(parcel)
  parcel.active = true; parcel.body.freeze = false; parcels.append(parcel)
 make_ui()
 apply_settings()
 Input.mouse_mode = Input.MOUSE_MODE_VISIBLE

func block(size: Vector3, at: Vector3, color: Color, collision := true):
 var body := StaticBody3D.new(); body.position = at; add_child(body)
 body.physics_material_override = ground_material
 var mesh := MeshInstance3D.new(); var box := BoxMesh.new(); box.size = size; mesh.mesh = box
 var material := StandardMaterial3D.new(); material.albedo_color = color; material.roughness = 0.85
 mesh.material_override = material; body.add_child(mesh)
 if collision:
  var shape := CollisionShape3D.new(); var box_shape := BoxShape3D.new(); box_shape.size = size
  shape.shape = box_shape; body.add_child(shape)
 return body

func label(text: String, at: Vector3):
 var node := Label3D.new(); node.text = text; node.position = at
 node.font_size = 64; node.pixel_size = 0.01; node.billboard = BaseMaterial3D.BILLBOARD_ENABLED
 add_child(node)

func make_ui():
 var canvas := CanvasLayer.new(); add_child(canvas)
 panel = PanelContainer.new(); panel.position = Vector2(12,12); panel.custom_minimum_size.x = 345
 canvas.add_child(panel)
 var column := VBoxContainer.new(); panel.add_child(column)
 var title := Label.new(); title.text = "물리 실험실 / PHYSICS LAB"; column.add_child(title)
 var engine := Label.new()
 # Read the effective setting; do not change project settings from the laboratory.
 engine.text = "Backend: %s | %d Hz" % [ProjectSettings.get_setting("physics/3d/physics_engine","DEFAULT"),Engine.physics_ticks_per_second]
 column.add_child(engine)
 for spec in [["mass","상자 질량 / kg",0.2,30,0.1],["friction","상자·바닥 마찰",0,1,0.01],["bounce","상자 반발",0,1,0.01],["gravity","상자 중력 / m/s²",0.1,25,0.1],["damping","회전 감쇠",0,5,0.05],["throw","던지기 수평 속도 / m/s",1,20,0.1]]:
  var row := HBoxContainer.new(); column.add_child(row)
  var text := Label.new(); text.text = spec[1]; text.custom_minimum_size.x = 225; row.add_child(text)
  var value := SpinBox.new(); value.min_value = spec[2]; value.max_value = spec[3]; value.step = spec[4]; value.value = settings[spec[0]]
  row.add_child(value); fields[spec[0]] = value
  value.value_changed.connect(func(number): settings[spec[0]] = number; apply_settings())
 for spec in [["시험 재시작 / R",reset_trials],["기본값 복원",restore_defaults],["조정값 저장",save_values],["저장값 불러오기",load_values]]:
  var button := Button.new(); button.text = spec[0]; button.pressed.connect(spec[1]); column.add_child(button)
 var controls := Label.new()
 controls.text = "Tab: 조작 ↔ 패널 / WASD: 이동\n마우스: 시점 / Space: 점프\nE: 들기·내려놓기 / 왼쪽 클릭: 던지기\n1~4: 시험 구역 이동 / R: 전체 초기화\n상자 중력만 변경 · 본편 설정에 영향 없음"
 column.add_child(controls)
 status = Label.new(); column.add_child(status)

func apply_settings():
 ground_material.friction = settings.friction
 for parcel in parcels:
  parcel.body.mass = settings.mass
  parcel.body.physics_material_override.friction = settings.friction
  parcel.body.physics_material_override.bounce = settings.bounce
  parcel.body.gravity_scale = settings.gravity / float(ProjectSettings.get_setting("physics/3d/default_gravity"))
  parcel.body.angular_damp = settings.damping
  parcel.body.sleeping = false

func reset_trials():
 if held != null: held.release(worker,false); held = null
 for i in range(parcels.size()):
  var body = parcels[i].body
  body.freeze = false; body.position = homes[i]; body.rotation = Vector3.ZERO
  body.linear_velocity = Vector3.ZERO; body.angular_velocity = Vector3.ZERO; body.sleeping = false
 worker.position = Vector3(0,0.05,8); worker.reset_motion()
 elapsed = 0; apply_settings(); message = "초기 상태로 복원됨"

func restore_defaults():
 settings = DEFAULTS.duplicate(); sync_fields(); reset_trials()

func sync_fields():
 for key in fields:
  fields[key].set_value_no_signal(settings[key])
 apply_settings()

func save_values():
 var file := FileAccess.open("user://physics-lab.json",FileAccess.WRITE)
 if file == null: message = "저장 실패"; return
 file.store_string(JSON.stringify(settings,"  ")); message = "저장됨: user://physics-lab.json"

func load_values():
 if not FileAccess.file_exists("user://physics-lab.json"): message = "저장된 값 없음"; return
 var data = JSON.parse_string(FileAccess.get_file_as_string("user://physics-lab.json"))
 if not data is Dictionary: message = "파일 형식 오류"; return
 for key in DEFAULTS:
  if data.has(key) and (data[key] is float or data[key] is int) and is_finite(float(data[key])):
   settings[key] = clampf(float(data[key]),fields[key].min_value,fields[key].max_value)
 sync_fields(); reset_trials(); message = "저장값 적용됨"

func _input(event):
 if event is InputEventKey and event.pressed and not event.echo and event.keycode in [KEY_TAB,KEY_ESCAPE]:
  var capture := Input.mouse_mode != Input.MOUSE_MODE_CAPTURED
  Input.mouse_mode = Input.MOUSE_MODE_CAPTURED if capture else Input.MOUSE_MODE_VISIBLE
  panel.visible = not capture
  get_viewport().set_input_as_handled()

func _unhandled_input(event):
 if event is InputEventKey and event.pressed and not event.echo:
  if Input.mouse_mode != Input.MOUSE_MODE_CAPTURED: return
  if event.keycode == KEY_R: reset_trials()
  if event.keycode == KEY_SPACE: jump_requested = true
  if event.keycode == KEY_E:
   if held != null: held.release(worker,false); held = null
   else:
    var nearest = null; var distance := 3.0
    for parcel in parcels:
     var d: float = parcel.body.position.distance_to(worker.position)
     if d < distance: nearest = parcel; distance = d
    if nearest != null and nearest.pickup(worker): held = nearest
  if event.keycode >= KEY_1 and event.keycode <= KEY_4:
   if held != null: held.release(worker,false); held = null
   worker.position = [Vector3(-10,0.05,1),Vector3(-3,0.05,1),Vector3(5,0.05,1),Vector3(10.9,0.05,6)][event.keycode-KEY_1]
   worker.reset_motion()
 if Input.mouse_mode == Input.MOUSE_MODE_CAPTURED:
  if event is InputEventMouseMotion: worker.aim(event.relative)
  if event is InputEventMouseButton and event.pressed and event.button_index == MOUSE_BUTTON_LEFT and held != null:
   var parcel = held; parcel.release(worker,true)
   parcel.body.linear_velocity = worker.forward()*settings.throw + Vector3.UP*Cargo.THROW_LIFT + worker.velocity
   held = null

func _physics_process(delta):
 elapsed += delta
 var movement := Vector2.ZERO
 if Input.mouse_mode == Input.MOUSE_MODE_CAPTURED:
  movement = Vector2(float(Input.is_physical_key_pressed(KEY_D))-float(Input.is_physical_key_pressed(KEY_A)),float(Input.is_physical_key_pressed(KEY_S))-float(Input.is_physical_key_pressed(KEY_W))).limit_length()
 worker.simulate(movement,worker.look_yaw,jump_requested,delta); jump_requested = false
 if held != null:
  held.move_held(worker)
  if held.carrier == null: held = null
 var speed: float = parcels[0].body.linear_velocity.length()
 status.text = "%.1f s | 낙하 상자 %.2f m/s\n%s" % [elapsed,speed,message]

func _exit_tree():
 Input.mouse_mode = Input.MOUSE_MODE_VISIBLE

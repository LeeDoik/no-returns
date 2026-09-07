extends Node3D

const Copy = preload("res://scripts/copy.gd")
var remaining := 0.0
var at := Vector3.ZERO
var slot := 0
var cargo_id := 0
var cooldowns: Dictionary = {}
var sign: Label3D

func _ready() -> void:
	sign = Label3D.new()
	var font := SystemFont.new()
	font.font_names = PackedStringArray(["Malgun Gothic", "Segoe UI", "Arial"])
	sign.font = font
	sign.font_size = 38
	sign.pixel_size = 0.007
	sign.billboard = BaseMaterial3D.BILLBOARD_ENABLED
	sign.modulate = Color("fff0a3")
	add_child(sign)

func clear() -> void:
	remaining = 0
	cooldowns.clear()

func mark(worker: Node3D, cargos: Dictionary) -> void:
	if float(cooldowns.get(worker.peer_id, 0)) > 0:
		return
	cooldowns[worker.peer_id] = 1.0
	var origin: Vector3 = worker.position + Vector3.UP
	var direction: Vector3 = worker.forward()
	var endpoint := origin + direction * 6
	var space := get_world_3d().direct_space_state
	var hit := space.intersect_ray(PhysicsRayQueryParameters3D.create(origin, endpoint, 1))
	if not hit.is_empty(): endpoint = hit.position
	var best := 6.0
	cargo_id = 0
	at = endpoint
	for id in cargos:
		var cargo = cargos[id]
		if not cargo.active or not cargo.body.visible: continue
		var offset: Vector3 = cargo.body.position - origin
		if offset.length() > best or offset.normalized().dot(direction) < 0.65: continue
		if not space.intersect_ray(PhysicsRayQueryParameters3D.create(origin, cargo.body.position, 1)).is_empty(): continue
		best = offset.length()
		cargo_id = id
		at = cargo.body.position
	slot = worker.slot
	remaining = 4

func step(delta: float) -> void:
	remaining = maxf(0, remaining - delta)
	for peer in cooldowns:
		cooldowns[peer] = maxf(0, cooldowns[peer] - delta)

func snapshot() -> Array:
	return [at, slot, cargo_id, remaining] if remaining > 0 else []

func apply_snapshot(data: Array) -> void:
	if data.is_empty():
		remaining = 0
		return
	at = data[0]
	slot = data[1]
	cargo_id = data[2]
	remaining = data[3]

func _process(_delta: float) -> void:
	sign.visible = remaining > 0
	if not sign.visible: return
	var position_to_mark := at
	var game = get_parent()
	var cargo = game.cargos.get(cargo_id)
	if cargo and cargo.body.visible:
		position_to_mark = cargo.body.position
	sign.position = position_to_mark + Vector3.UP * 1.4
	sign.text = (Copy.get_text("ping_cargo") if cargo_id else Copy.get_text("ping_here")) % slot

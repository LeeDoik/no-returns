extends Node3D

const Zone = preload("res://scripts/map_zone.gd")
const Copy = preload("res://scripts/copy.gd")
@export_range(2.0, 20.0, 0.5) var gate_hold_seconds := 6.0
@export_range(2.0, 20.0, 0.5) var wind_idle_seconds := 5.0
@export_range(0.5, 5.0, 0.1) var wind_warning_seconds := 1.5
@export_range(1.0, 8.0, 0.5) var wind_burst_seconds := 2.5
@export_range(1.0, 10.0, 0.5) var worker_wind_speed := 6.0
@export_range(2.0, 18.0, 0.5) var cargo_wind_speed := 12.0
var gate_left := 0.0
var wind_clock := 0.0
var gate_open := false
var door_origin := Vector3.ZERO
var presented_gate := -1
var presented_wind := -1

func _ready() -> void:
	door_origin = get_node("Gate/Door").position
	_present()

func reset() -> void:
	gate_left = 0
	wind_clock = 0
	gate_open = false
	_present()

func _free_cargo(cargo: Node3D) -> bool:
	return cargo.active and cargo.body.visible and cargo.recovery_left <= 0 and not cargo.creature_held and cargo.rules.holder_id == 0 and (not cargo.cling or cargo.cling.target_kind.is_empty())

func _occupied(zone: Node3D, workers: Dictionary, cargos: Dictionary, pressure: bool) -> bool:
	for worker in workers.values():
		if Zone.contains(zone, worker.global_position): return true
	for cargo in cargos.values():
		if not cargo.active or not cargo.body.visible or cargo.recovery_left > 0: continue
		if pressure and (not _free_cargo(cargo) or absf(cargo.body.linear_velocity.y) > 0.3): continue
		if Zone.contains(zone, cargo.body.global_position): return true
	return false

# Called by the host/solo simulation only. Guests receive presentation state.
func step(delta: float, workers: Dictionary, cargos: Dictionary) -> void:
	if not is_finite(delta) or delta <= 0: return
	var pressed := _occupied(get_node("Gate/PlateFront"), workers, cargos, true) or _occupied(get_node("Gate/PlateBack"), workers, cargos, true)
	gate_left = gate_hold_seconds if pressed else maxf(0, gate_left-delta)
	if gate_left <= 0 and _occupied(get_node("Gate/Clearance"), workers, cargos, false): gate_left = 0.25
	gate_open = gate_left > 0
	wind_clock = fmod(wind_clock+delta, wind_idle_seconds+wind_warning_seconds+wind_burst_seconds)
	if wind_phase() == 2:
		var airflow := _wind_direction()*cargo_wind_speed
		for cargo in cargos.values():
			if not _free_cargo(cargo) or not Zone.contains(get_node("AirMail/WindZone"), cargo.body.global_position): continue
			cargo.body.freeze = false
			cargo.body.sleeping = false
			var velocity: Vector2 = Vector2(cargo.body.linear_velocity.x,cargo.body.linear_velocity.z).move_toward(Vector2(airflow.x,airflow.z), 18*delta)
			cargo.body.linear_velocity.x = velocity.x
			cargo.body.linear_velocity.z = velocity.y
	_present()

func wind_phase() -> int:
	if wind_clock < wind_idle_seconds: return 0
	return 1 if wind_clock < wind_idle_seconds+wind_warning_seconds else 2

func _wind_direction() -> Vector3:
	return get_node("AirMail").global_basis.orthonormalized()*Vector3.FORWARD

func worker_drift(worker: Node3D) -> Vector3:
	if wind_phase() != 2 or not Zone.contains(get_node("AirMail/WindZone"),worker.global_position): return Vector3.ZERO
	return _wind_direction()*worker_wind_speed

func snapshot() -> PackedFloat32Array:
	return PackedFloat32Array([gate_left,wind_clock])

func apply_snapshot(state: Variant) -> void:
	if not state is PackedFloat32Array or state.size() != 2: return
	if not is_finite(state[0]) or not is_finite(state[1]): return
	gate_left = maxf(0,state[0])
	wind_clock = maxf(0,state[1])
	gate_open = gate_left > 0
	_present()

func _present() -> void:
	if presented_gate != int(gate_open):
		get_node("Gate/Door").position = door_origin + Vector3.UP*(4.0 if gate_open else 0.0)
		get_node("Gate/Door/CollisionShape3D").disabled = gate_open
		for plate in ["PlateFront","PlateBack"]:
			get_node("Gate/"+plate+"/Surface").color = Color("63e5ad") if gate_open else Color("edb947")
		presented_gate = int(gate_open)
	var ko: bool = Copy.language == "ko"
	get_node("Gate/Status").text = ("통과!  %d초" if ko else "GO!  %ds") % ceili(gate_left) if gate_open else ("직원 또는 상자로 발판을 누르세요" if ko else "WEIGH PLATE WITH WORKER OR PARCEL")
	var phase := wind_phase()
	get_node("AirMail/Status").text = ["잠시 숨 고르는 중", "곧 송풍!", "특급 배송 중!"][phase] if ko else ["TAKING A BREATH", "GUST INCOMING!", "EXPRESS AIR MAIL!"][phase]
	if presented_wind != phase:
		get_node("AirMail/Indicator").color = [Color("37636d"),Color("f6be42"),Color("63efdd")][phase]
		presented_wind = phase

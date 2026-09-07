extends Node3D

const Rules = preload("res://scripts/cargo_rules.gd")
const Session = preload("res://scripts/session.gd")
const Depot = preload("res://scripts/depot.gd")
const Worker = preload("res://scripts/worker.gd")
const Interface = preload("res://scripts/interface.gd")
const Copy = preload("res://scripts/copy.gd")
const Cargo = preload("res://scripts/cargo.gd")
const Cues = preload("res://scripts/sneeze_cues.gd")
const SHIFT_SECONDS := 180.0
const Preferences = preload("res://scripts/preferences.gd")
const Round = preload("res://scripts/round_rules.gd")
const Pings = preload("res://scripts/pings.gd")
const Conveyor = preload("res://scripts/conveyor.gd")
var round_state = Round.new()
var pings: Node3D
var conveyor: Node3D
var replacements: Dictionary = {}
var replay_seed := -1

var session: Node
var depot: Node3D
var ui: CanvasLayer
var worker_root: Node3D
var workers: Dictionary = {}
var inputs: Dictionary = {}
var targets: Dictionary = {}
var local_id := 1
var phase := "menu"
var time_left := SHIFT_SECONDS
var notice_key := ""
var notice_until := 0
var tick := 0
var test_mode := false
var capture_mode := ""
var capture_frames := 0
var cargos: Dictionary = {}
var last_blast: Dictionary = {}
var score: int:
	get:
		var total := 0
		for cargo in cargos.values():
			total += cargo.rules.score
		return total

func _ready() -> void:
	_configure_input()
	for argument in OS.get_cmdline_user_args():
		if argument.begins_with("--seed="): replay_seed = argument.trim_prefix("--seed=").to_int()
	for arg in OS.get_cmdline_user_args():
		if arg.begins_with("--role="):
			test_mode = true
		if arg.begins_with("--capture="):
			capture_mode = arg.trim_prefix("--capture=")
	Preferences.enabled = not test_mode and capture_mode.is_empty()
	Preferences.load_settings()
	depot = Depot.new()
	add_child(depot)
	conveyor = Conveyor.new()
	add_child(conveyor)
	for id in [1, 2, 3, 4]:
		var cargo = Cargo.new()
		cargo.cargo_id = id
		cargo.kind = {1: "standard", 2: "sneezer", 3: "clinger", 4: "hopper"}[id]
		cargo.home = {1: Vector3(0, 0.55, 1.1), 2: Vector3(-2, 0.55, 4), 3: Vector3(2, 0.55, 4), 4: Vector3(4, 0.55, 1)}[id]
		cargo.name = "Cargo_%d" % id
		add_child(cargo)
		cargo.sneezed.connect(_apply_sneeze)
		cargo.notice.connect(func(key: String):
			_set_notice(key, 3)
			if key == "shipped": replacements[id] = true)
		cargos[id] = cargo
	pings = Pings.new()
	add_child(pings)
	worker_root = Node3D.new()
	worker_root.name = "Workers"
	add_child(worker_root)
	session = Session.new()
	session.name = "Session"
	add_child(session)
	session.joined.connect(_joined)
	session.player_joined.connect(_player_joined)
	session.player_left.connect(_player_left)
	session.stopped.connect(_stopped)
	session.input_received.connect(_receive_input)
	session.action_received.connect(_receive_action)
	session.snapshot_received.connect(_receive_snapshot)
	ui = Interface.new()
	add_child(ui)
	ui.command.connect(_command)
	for arg in OS.get_cmdline_user_args():
		if arg == "--host":
			host_game()
		elif arg.begins_with("--join="):
			join_game(arg.trim_prefix("--join="))
	if capture_mode == "world":
		practice_game()
		workers[1].look_yaw = -0.18
		workers[1].update_look()
	_render_ui()

func _configure_input() -> void:
	for key in {"left": KEY_A, "right": KEY_D, "forward": KEY_W, "back": KEY_S, "jump": KEY_SPACE, "interact": KEY_E, "menu": KEY_ESCAPE, "ping": KEY_Q, "lever": KEY_F}:
		if not InputMap.has_action(key):
			InputMap.add_action(key)
			var event := InputEventKey.new()
			event.physical_keycode = {"left": KEY_A, "right": KEY_D, "forward": KEY_W, "back": KEY_S, "jump": KEY_SPACE, "interact": KEY_E, "menu": KEY_ESCAPE, "ping": KEY_Q, "lever": KEY_F}[key]
			InputMap.action_add_event(key, event)
	if not InputMap.has_action("throw"):
		InputMap.add_action("throw")
		var mouse := InputEventMouseButton.new()
		mouse.button_index = MOUSE_BUTTON_LEFT
		InputMap.action_add_event("throw", mouse)

func practice_game() -> void:
	leave_game()
	session.practice()
	local_id = 1
	_add_worker(1)
	start_shift()

func host_game(port: int = Session.DEFAULT_PORT) -> void:
	leave_game()
	if session.host(port) != OK:
		_set_notice("host_failed", 10)
		return
	local_id = 1
	_add_worker(1)
	phase = "waiting"

func join_game(address: String, port: int = Session.DEFAULT_PORT) -> void:
	leave_game()
	if address.strip_edges().is_empty() or session.join_host(address, port) != OK:
		_set_notice("connection_failed", 10)
		return
	phase = "connecting"

func leave_game() -> void:
	if session:
		session.close()
	for worker in workers.values():
		worker.queue_free()
	workers.clear()
	inputs.clear()
	targets.clear()
	last_blast.clear()
	replacements.clear()
	if pings: pings.clear()
	if conveyor: conveyor.reset()
	phase = "menu"
	time_left = SHIFT_SECONDS
	notice_key = ""
	if ui:
		ui.paused = false
	for cargo in cargos.values():
		cargo.cancel()
		cargo.rules.reset_shift()
		cargo.reset_crate()
	if depot:
		depot.overview.current = true
		depot.marker.visible = false
	Input.mouse_mode = Input.MOUSE_MODE_VISIBLE

func _stopped(reason: String) -> void:
	leave_game()
	_set_notice(reason, 15)

func _joined() -> void:
	local_id = multiplayer.get_unique_id()

func _player_joined(peer_id: int) -> void:
	if phase != "waiting":
		return
	_add_worker(peer_id)

func _player_left(peer_id: int) -> void:
	if not workers.has(peer_id):
		return
	for cargo in cargos.values():
		if cargo.rules.holder_id == peer_id:
			cargo.release(workers[peer_id], false)
		cargo.cancel()
	workers[peer_id].queue_free()
	workers.erase(peer_id)
	inputs.clear()
	pings.clear()
	phase = "waiting"
	conveyor.active = false
	session.in_shift = false
	Input.mouse_mode = Input.MOUSE_MODE_VISIBLE
	_set_notice("guest_left", 8)

func _add_worker(peer_id: int, assigned_slot: int = -1) -> void:
	if workers.has(peer_id) or workers.size() >= Session.MAX_WORKERS:
		return
	if assigned_slot < 0:
		var occupied: Array = []
		for member in workers.values():
			occupied.append(member.slot)
		for candidate in range(1, Session.MAX_WORKERS + 1):
			if not occupied.has(candidate):
				assigned_slot = candidate
				break
	var worker = Worker.new()
	worker.peer_id = peer_id
	worker.slot = assigned_slot
	worker.name = "Worker_%d" % peer_id
	worker_root.add_child(worker)
	worker.position = worker.spawn_position()
	workers[peer_id] = worker
	if peer_id == local_id:
		worker.make_local()

func start_shift() -> void:
	if session.mode not in ["host", "practice"]:
		return
	if session.mode == "host" and (workers.size() < 2 or workers.size() > Session.MAX_WORKERS):
		return
	inputs.clear()
	last_blast.clear()
	replacements.clear()
	if pings: pings.clear()
	if conveyor: conveyor.reset()
	round_state.start(workers.size(), replay_seed)
	time_left = round_state.duration
	for cargo in cargos.values():
		cargo.reset_shift()
	for id in workers:
		workers[id].position = workers[id].spawn_position()
		workers[id].reset_motion()
	phase = "playing"
	conveyor.active = true
	session.in_shift = true
	notice_key = ""
	ui.paused = false
	if not test_mode and capture_mode.is_empty():
		Input.mouse_mode = Input.MOUSE_MODE_CAPTURED

func _command(action: String, address: String) -> void:
	match action:
		"practice": practice_game()
		"host": host_game()
		"join": join_game(address)
		"leave": leave_game()
		"start": session.send_action("start")
		"overtime": session.send_action("overtime")
		"resume":
			ui.paused = false
			Input.mouse_mode = Input.MOUSE_MODE_CAPTURED

func _unhandled_input(event: InputEvent) -> void:
	if phase != "playing" or test_mode:
		return
	if event.is_action_pressed("menu"):
		ui.paused = not ui.paused
		Input.mouse_mode = Input.MOUSE_MODE_VISIBLE if ui.paused else Input.MOUSE_MODE_CAPTURED
		get_viewport().set_input_as_handled()
		return
	if ui.paused or Input.mouse_mode != Input.MOUSE_MODE_CAPTURED:
		return
	if event is InputEventMouseMotion and workers.has(local_id):
		workers[local_id].aim(event.relative)
	if event.is_action_pressed("interact") and not event.is_echo():
		session.send_action("interact")
	if event.is_action_pressed("ping") and not event.is_echo():
		session.send_action("ping")
	if event.is_action_pressed("lever") and not event.is_echo():
		session.send_action("lever")
	if event.is_action_pressed("throw"):
		session.send_action("throw")

func _notification(what: int) -> void:
	if what == NOTIFICATION_APPLICATION_FOCUS_OUT and ui and phase == "playing":
		ui.paused = true
		Input.mouse_mode = Input.MOUSE_MODE_VISIBLE

func _receive_input(peer_id: int, movement: Vector2, yaw: float, jump: bool) -> void:
	if not workers.has(peer_id) or phase != "playing":
		return
	var pending_jump: bool = inputs.get(peer_id, {}).get("jump", false)
	inputs[peer_id] = {"move": movement, "yaw": yaw, "jump": jump or pending_jump, "at": Time.get_ticks_msec()}

func _receive_action(peer_id: int, action: String) -> void:
	if not workers.has(peer_id):
		return
	if action == "overtime":
		if phase == "won" and not round_state.bonus and round_state.vote(peer_id, workers.keys()):
			_begin_bonus()
		return
	if action in ["start", "restart"]:
		if peer_id == 1 and phase in ["waiting", "won", "lost", "bonus_done"]:
			start_shift()
		return
	if phase != "playing":
		return
	var worker = workers[peer_id]
	var held = held_cargo(peer_id)
	if action == "ping":
		pings.mark(worker, cargos)
		return
	if action == "lever":
		conveyor.try_reverse(peer_id, workers, phase)
		return
	if action == "interact":
		if held:
			held.release(worker, false)
		else:
			var candidates: Array = cargos.values()
			candidates.sort_custom(func(a, b): return worker.position.distance_squared_to(a.body.position) < worker.position.distance_squared_to(b.body.position))
			for candidate in candidates:
				if candidate.pickup(worker):
					break
	elif action == "throw" and held:
		held.release(worker, true)

func held_cargo(peer_id: int) -> Node3D:
	for cargo in cargos.values():
		if cargo.rules.holder_id == peer_id:
			return cargo
	return null

func _blast_reaches(source: Node3D, target: Vector3) -> bool:
	var direction := Vector3.FORWARD.rotated(Vector3.UP, source.facing)
	if not Cargo.Sneeze.contains(source.body.position, direction, target):
		return false
	var ray := PhysicsRayQueryParameters3D.create(source.body.position, target, 1)
	return get_world_3d().direct_space_state.intersect_ray(ray).is_empty()

func _apply_sneeze(source: Node3D) -> void:
	if session.mode not in ["host", "practice"] or phase != "playing":
		return
	var direction := Vector3.FORWARD.rotated(Vector3.UP, source.facing)
	var hit_workers: Array = []
	var hit_cargos: Array = []
	var sticky = cargos[3]
	if not sticky.cling.target_kind.is_empty() and _blast_reaches(source, sticky.body.position):
		sticky.cling.detach()
	for id in workers:
		if id == source.rules.holder_id:
			continue
		var worker = workers[id]
		if _blast_reaches(source, worker.position + Vector3.UP):
			var held = held_cargo(id)
			if held:
				held.release(worker, false)
			worker.apply_push(direction)
			hit_workers.append(id)
	for cargo in cargos.values():
		if cargo == source or not cargo.body.visible or cargo.recovery_left > 0:
			continue
		if _blast_reaches(source, cargo.body.position):
			cargo.push(direction, workers)
			hit_cargos.append(cargo.cargo_id)
	last_blast = {"source": source.cargo_id, "event": source.sneeze.event_id, "workers": hit_workers, "cargos": hit_cargos}
	_set_notice("sneeze_blast", 1.5)

func _physics_process(delta: float) -> void:
	if not session or phase == "menu":
		return
	if phase == "playing" and not test_mode and workers.has(local_id):
		var movement := Vector2.ZERO
		var jump := false
		if not ui.paused and Input.mouse_mode == Input.MOUSE_MODE_CAPTURED:
			movement = Input.get_vector("left", "right", "forward", "back")
			jump = Input.is_action_just_pressed("jump")
		session.send_input(movement, workers[local_id].look_yaw, jump)
	if session.mode not in ["host", "practice"]:
		return
	if phase == "playing":
		time_left = maxf(0, time_left - delta)
		for id in workers:
			workers[id].speed_scale = 0.7 if cargos[3].cling.target_kind == "worker" and cargos[3].cling.target_id == id else 1.0
			var sample: Dictionary = inputs.get(id, {})
			var fresh: bool = Time.get_ticks_msec() - int(sample.get("at", -10000)) < 300
			var belt_drift: Vector3 = conveyor.drift_at(workers[id].position) if workers[id].position.y >= -0.1 and workers[id].position.y <= 0.15 else Vector3.ZERO
			workers[id].simulate(sample.get("move", Vector2.ZERO) if fresh else Vector2.ZERO, float(sample.get("yaw", workers[id].heading)), bool(sample.get("jump", false)) if fresh else false, delta, belt_drift)
			if inputs.has(id):
				inputs[id]["jump"] = false
		pings.step(delta)
		for cargo in cargos.values():
			cargo.step(delta, workers)
			if replacements.has(cargo.cargo_id) and cargo.recovery_left <= 0:
				cargo.rules.destination = round_state.next_destination()
				replacements.erase(cargo.cargo_id)
		conveyor.step(delta, cargos)
		cargos[3].cling.step(delta, workers, cargos)
		if score >= round_state.quota:
			_finish("won")
		elif time_left <= 0:
			_finish("bonus_done" if round_state.bonus else "lost")
	for id in workers:
		workers[id].held = held_cargo(id) != null
	tick += 1
	if tick % 3 == 0:
		session.publish(_snapshot())

func _begin_bonus() -> void:
	round_state.begin_bonus(score)
	time_left = round_state.duration
	replacements.clear()
	pings.clear()
	for cargo in cargos.values():
		cargo.active = true
		cargo.reset_crate()
		cargo.rules.destination = round_state.next_destination()
	phase = "playing"
	conveyor.active = true
	ui.paused = false
	if not test_mode: Input.mouse_mode = Input.MOUSE_MODE_CAPTURED

func _finish(result: String) -> void:
	if result == "won":
		round_state.base_won = true
		if round_state.bonus: result = "bonus_done"
	pings.clear()
	for cargo in cargos.values():
		cargo.cancel()
	phase = result
	conveyor.active = false
	inputs.clear()
	ui.paused = false
	Input.mouse_mode = Input.MOUSE_MODE_VISIBLE

func _snapshot() -> Dictionary:
	var poses: Dictionary = {}
	for id in workers:
		poses[id] = [workers[id].position, workers[id].heading, workers[id].velocity, workers[id].stagger, workers[id].speed_scale, workers[id].slot]
	var cargo_states: Dictionary = {}
	for id in cargos:
		cargo_states[id] = cargos[id].wire_snapshot()
	return {"workers": poses, "phase": phase, "time": time_left, "cargos": cargo_states,
		"blast": [] if last_blast.is_empty() else [last_blast.source, last_blast.event, PackedInt32Array(last_blast.workers), PackedInt32Array(last_blast.cargos)],
		"round": [round_state.quota, round_state.duration, round_state.bonus, round_state.base_won, PackedInt32Array(round_state.votes), round_state.seed_value, conveyor.direction],
		"ping": pings.snapshot(), "notice": notice_key if Time.get_ticks_msec() < notice_until else ""}

func _receive_snapshot(state: Dictionary) -> void:
	var old_phase := phase
	phase = state.phase
	conveyor.active = phase == "playing"
	time_left = state.time
	last_blast = {} if state.blast.is_empty() else {"source": state.blast[0], "event": state.blast[1], "workers": Array(state.blast[2]), "cargos": Array(state.blast[3])}
	round_state.quota = state.round[0]
	round_state.duration = state.round[1]
	round_state.bonus = state.round[2]
	round_state.base_won = state.round[3]
	round_state.votes = Array(state.round[4])
	round_state.seed_value = state.round[5]
	conveyor.apply_direction(state.round[6])
	pings.apply_snapshot(state.ping)
	for id in cargos:
		cargos[id].apply_wire(state.cargos[id])
	notice_key = state.notice
	notice_until = Time.get_ticks_msec() + 250
	var poses: Dictionary = state.workers
	for id in workers.keys():
		if not poses.has(id):
			workers[id].queue_free()
			workers.erase(id)
			targets.erase(id)
	for id in poses:
		var values: Array = poses[id]
		var pose := {"position": values[0], "yaw": values[1], "velocity": values[2], "stagger": values[3], "speed": values[4]}
		if not workers.has(id):
			_add_worker(id, values[5])
			workers[id].position = pose.position
		targets[id] = pose
		workers[id].held = held_cargo(id) != null
		workers[id].stagger = pose.stagger
		workers[id].speed_scale = pose.speed
	if old_phase != phase:
		ui.paused = false
		if phase == "playing" and not test_mode:
			Input.mouse_mode = Input.MOUSE_MODE_CAPTURED
		else:
			Input.mouse_mode = Input.MOUSE_MODE_VISIBLE

func _process(delta: float) -> void:
	if not ui:
		return
	if session.mode == "guest":
		var weight := minf(1, delta * 20)
		for id in targets:
			workers[id].position = workers[id].position.lerp(targets[id].position, weight)
			workers[id].heading = targets[id].yaw
			workers[id].velocity = targets[id].velocity
		for cargo in cargos.values():
			cargo.interpolate(delta)
	_update_marker()
	_render_ui()
	if not capture_mode.is_empty():
		capture_frames += 1
		if capture_frames == 80:
			_capture.call_deferred()

func _update_marker() -> void:
	var held = held_cargo(local_id)
	depot.marker.visible = phase == "playing" and held != null and workers.has(local_id)
	if not depot.marker.visible:
		return
	var contact: Dictionary = held.predict_contact(held.body.position, held.throw_velocity(workers[local_id]))
	depot.marker.visible = not contact.is_empty()
	if contact.is_empty():
		return
	var normal: Vector3 = contact.normal
	depot.marker.position = contact.center - normal * (Cargo.SIZE * 0.5 - 0.08)
	depot.marker.quaternion = Quaternion(Vector3.UP, normal)

func _set_notice(key: String, seconds: float) -> void:
	notice_key = key
	notice_until = Time.get_ticks_msec() + int(seconds * 1000)

func _render_ui() -> void:
	var prompt_key := "far"
	var cargo_name := ""
	var held = held_cargo(local_id)
	if held:
		prompt_key = "carrying"
		cargo_name = Copy.get_text(held.kind + "_name") + (" / A ↑↑" if held.rules.destination == 1 else " / B ◆")
	elif workers.has(local_id):
		var nearest: Node3D = null
		var distance := Rules.PICKUP_DISTANCE
		for cargo in cargos.values():
			if not cargo.body.visible or cargo.rules.holder_id != 0:
				continue
			if cargo.cling and not cargo.cling.target_kind.is_empty():
				continue
			var candidate: float = workers[local_id].position.distance_to(cargo.body.position)
			if candidate <= distance:
				nearest = cargo
				distance = candidate
		if nearest:
			prompt_key = "near"
			cargo_name = Copy.get_text(nearest.kind + "_name") + (" / A ↑↑" if nearest.rules.destination == 1 else " / B ◆")
		elif conveyor.can_use(workers[local_id]):
			prompt_key = "lever"
	var warning := ""
	var source = cargos[2]
	if phase == "playing" and source.sneeze.phase == "windup":
		warning = Copy.get_text("sneeze_warning") % maxf(0, source.sneeze.remaining)
	elif phase == "playing" and not cargos[3].cling.target_kind.is_empty():
		warning = Copy.get_text("clinger_attached") % maxf(0, cargos[3].cling.remaining)
	elif phase == "playing" and cargos[4].hopper.phase == "windup":
		warning = Copy.get_text("hopper_paused") if cargos[4].rules.holder_id else Copy.get_text("hopper_windup") % cargos[4].hopper.remaining
	ui.render({"quota": round_state.quota, "bonus": round_state.bonus, "base_won": round_state.base_won,
		"elapsed": round_state.duration - time_left, "votes": round_state.votes.size(), "voted": round_state.votes.has(local_id), "seed": round_state.seed_value,
		"phase": phase, "mode": session.mode, "count": workers.size(), "score": score,
		"time": time_left, "prompt": prompt_key, "cargo_name": cargo_name, "warning": warning,
		"notice": notice_key if Time.get_ticks_msec() < notice_until else ""})

func _capture() -> void:
	await RenderingServer.frame_post_draw
	var screenshot := get_viewport().get_texture().get_image()
	var path := "res://artifacts/%s.png" % capture_mode
	var error := screenshot.save_png(path)
	print("CAPTURE %s: %s" % [path, error_string(error)])
	get_tree().quit(0 if error == OK else 1)

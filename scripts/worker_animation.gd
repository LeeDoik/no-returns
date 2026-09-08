extends RefCounted
# Clips share one centered skeleton; direction selects a real carrying gait.
var player: AnimationPlayer
var throw_left := 0.0
var land_left := 0.0
var airborne := false
var running := false
var current := ""

func _init(animation_player: AnimationPlayer) -> void:
	player = animation_player
	player.callback_mode_process = AnimationMixer.ANIMATION_CALLBACK_MODE_PROCESS_MANUAL
	for clip in player.get_animation_list():
		if clip not in ["RESET","throw","land","hit_to_body_01"]:
			player.get_animation(clip).loop_mode = Animation.LOOP_LINEAR
	player.play("idle"); player.advance(0); current = "idle"

func release_throw() -> void:
	throw_left = player.get_animation("throw").length
	current = ""

func update(delta: float, velocity: Vector3, carrying: bool, stagger: float, heading: float = 0.0) -> void:
	var speed := Vector2(velocity.x,velocity.z).length()
	var was_airborne := airborne
	if absf(velocity.y) > 0.05: airborne = true
	elif absf(velocity.y) < 0.0001: airborne = false
	if was_airborne and not airborne: land_left = 0.3
	if speed > 2.0: running = true
	elif speed < 1.65: running = false
	var clip := "idle" if speed < 0.08 else ("run" if running else "walk")
	var rate := 1.0 if clip == "idle" else clampf(speed/(3.0 if running else 1.1),0.3,1.8)
	if carrying:
		throw_left = 0
		clip = "carry_air" if airborne else "carry_idle"
		rate = 1.0
		if not airborne and speed > 0.08:
			var local := velocity.rotated(Vector3.UP,-heading)
			var angle := atan2(local.x,-local.z)
			var sector := posmod(roundi(angle/(PI/4)),8)
			clip = ["carry_walk","carry_forward_right","carry_right","carry_back_right","carry_back","carry_back_left","carry_left","carry_forward_left"][sector]
			var reference: float = 0.72 if sector in [2,6] else (0.93 if sector in [1,3,5,7] else 1.1)
			rate = clampf(speed/reference,0.3,1.8)
	elif throw_left > 0: clip = "throw"; rate = 1.0
	elif airborne: clip = "air_rise" if velocity.y >= 0 else "air_fall"; rate = 1.0
	elif stagger > 0.05: clip = "hit_to_body_01"; rate = 1.0
	elif land_left > 0 and speed < 0.4: clip = "land"; rate = 1.0
	if clip != current:
		var phase := 0.0
		var locomotion := current in ["walk","run"] or (current.begins_with("carry_") and current not in ["carry_idle","carry_air"])
		if locomotion and player.current_animation_length > 0:
			phase = fmod(player.current_animation_position/player.current_animation_length,1.0)
		player.play(clip,0.14)
		if locomotion and (clip in ["walk","run"] or (clip.begins_with("carry_") and clip not in ["carry_idle","carry_air"])):
			player.seek(player.get_animation(clip).length*phase,false)
		current = clip
	player.speed_scale = rate
	player.advance(delta)
	throw_left = maxf(0,throw_left-delta)
	land_left = maxf(0,land_left-delta)

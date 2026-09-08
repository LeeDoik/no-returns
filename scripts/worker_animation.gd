extends RefCounted
# Presentation only: movement, releases and collision remain authoritative gameplay.
var player: AnimationPlayer
var throw_left := 0.0
var airborne_left := 0.0
var running := false
var current := ""

func _init(animation_player: AnimationPlayer) -> void:
	player = animation_player
	player.callback_mode_process = AnimationMixer.ANIMATION_CALLBACK_MODE_PROCESS_MANUAL
	for clip in ["idle", "walk", "run", "carry_idle", "carry_walk", "carry_run", "air_rise", "air_fall"]:
		player.get_animation(clip).loop_mode = Animation.LOOP_LINEAR
	player.play("idle"); player.advance(0); current = "idle"

func release_throw() -> void:
	throw_left = player.get_animation("throw").length
	current = "" # Retrigger even when a second release follows immediately.

func update(delta: float, velocity: Vector3, carrying: bool, stagger: float) -> void:
	var speed := Vector2(velocity.x, velocity.z).length()
	if speed > 2.4: running = true
	elif speed < 1.9: running = false
	airborne_left = 0.16 if absf(velocity.y) > 0.9 else maxf(0, airborne_left - delta)
	var clip := "idle" if speed < 0.12 else ("run" if running else "walk")
	var rate := 1.0 if clip == "idle" else clampf(speed / (3.0 if running else 1.2), 0.35, 2.2)
	if throw_left > 0:
		clip = "throw"; rate = 1.0
	elif carrying:
		clip = "carry_" + ("idle" if airborne_left > 0 else clip)
		if airborne_left > 0: rate = 1.0
	elif stagger > 0.05:
		clip = "hit_to_body_01"; rate = 1.4
	elif airborne_left > 0:
		clip = "air_rise" if velocity.y >= 0 else "air_fall"; rate = 1.0
	if clip != current:
		player.play(clip, 0.12 if clip == "throw" else 0.18)
		current = clip
	# Match the step cadence to the displayed movement speed.
	player.speed_scale = rate
	player.advance(delta)
	throw_left = maxf(0, throw_left - delta)

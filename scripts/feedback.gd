extends Node

const Cues = preload("res://scripts/sneeze_cues.gd")
var player: AudioStreamPlayer
var sounds: Dictionary = {}

func _ready() -> void:
	player = AudioStreamPlayer.new()
	player.volume_db = -12
	add_child(player)
	for kind in ["ship", "relay", "horn", "win", "fail"]: sounds[kind] = _tone(kind)

func play(kind: String) -> void:
	if Cues.muted or DisplayServer.get_name() == "headless" or not sounds.has(kind): return
	player.stream = sounds[kind]
	player.play()

func _process(_delta: float) -> void:
	if Cues.muted: player.stop()

func _tone(kind: String) -> AudioStreamWAV:
	if kind == "ship": return _stamp()
	var rate := 22050
	var duration := 0.32 if kind != "win" else 0.6
	var count := int(rate * duration)
	var bytes := PackedByteArray()
	bytes.resize(count * 2)
	var frequencies := {"ship":660.0,"relay":880.0,"horn":220.0,"win":523.25,"fail":164.81}
	for i in count:
		var t := float(i) / rate
		var freq: float = frequencies[kind]
		if kind == "win": freq *= [1.0,1.25,1.5][mini(2, int(t / 0.2))]
		var wave := sin(TAU * freq * t) + (0.3 * sin(TAU * freq * 1.5 * t) if kind == "horn" else 0.0)
		bytes.encode_s16(i * 2, int(wave * sin(PI * t / duration) * 10000))
	var sound := AudioStreamWAV.new()
	sound.format = AudioStreamWAV.FORMAT_16_BITS
	sound.mix_rate = rate
	sound.data = bytes
	return sound

func _stamp() -> AudioStreamWAV:
	# A low impact, short paper scrape, and lighter return of the mechanism.
	# Dedicated seeded noise must never consume gameplay's random sequence.
	var rng := RandomNumberGenerator.new(); rng.seed = 7142
	var rate := 22050; var count := int(rate*0.32)
	var bytes := PackedByteArray(); bytes.resize(count*2)
	var filtered := 0.0
	for i in range(count):
		var t := float(i)/rate
		filtered = lerpf(filtered,rng.randf_range(-1,1),0.38)
		var thud := sin(TAU*92*t)*exp(-t*35)*0.58
		var contact := filtered*exp(-t*82)*0.5
		var scrape := filtered*exp(-absf(t-0.065)*40)*0.18
		var rebound := sin(TAU*185*t)*exp(-maxf(0,t-0.13)*55)*0.16 if t >= 0.13 else 0.0
		var envelope := minf(1,t*1500)*minf(1,(0.32-t)*80)
		bytes.encode_s16(i*2,int(clampf((thud+contact+scrape+rebound)*envelope,-0.95,0.95)*25000))
	var sound := AudioStreamWAV.new(); sound.format = AudioStreamWAV.FORMAT_16_BITS; sound.mix_rate = rate; sound.data = bytes
	return sound

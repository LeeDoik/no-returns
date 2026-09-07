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

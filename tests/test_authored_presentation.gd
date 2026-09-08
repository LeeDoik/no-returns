extends SceneTree
var failures := 0
func check(ok: bool, message: String) -> void:
	if not ok: failures += 1; push_error(message)
func _initialize() -> void: call_deferred("run")
func run() -> void:
	var game = load("res://scenes/main.tscn").instantiate(); root.add_child(game); game.practice_game(true); game.set_physics_process(false)
	var worker = game.workers[1]; worker.held = true
	for yaw in [0.0,PI/2,-PI+0.01]:
		worker.heading = yaw; worker._process(1.0/60.0)
		check((-worker.visual.basis.z).is_equal_approx(worker.forward()),"carrying body follows parcel on first turn frame")
	worker._process(0.2)
	check(is_equal_approx(worker.carry_blend,1.0),"pickup pose reaches grip within 0.2s")
	worker.held = false; worker._process(0.04)
	check(worker.carry_blend > 0 and worker.carry_blend < 1,"release blends instead of snapping")
	worker._process(0.2); check(is_zero_approx(worker.carry_blend),"release settles")
	var sneezer = game.cargos[2]
	sneezer.sneeze.phase = "calm"; sneezer._present(); check(not sneezer.cues.caption.visible,"safe parcel does not show a floating name")
	sneezer.sneeze.phase = "windup"; sneezer.sneeze.remaining = 0.6; sneezer._present()
	check(sneezer.cues.caption.visible and not sneezer.cues.caption.text.is_empty(),"imminent sneeze still warns")
	var clip: AudioStreamWAV = game.feedback.sounds.ship
	var peak := 0
	for i in range(0,clip.data.size(),2): peak = maxi(peak,absi(clip.data.decode_s16(i)))
	check(peak > 1000 and peak < 32760,"delivery sound is audible without clipping")
	check(clip.get_length() < 0.4,"confirmation remains short")
	game.queue_free(); await process_frame
	print("AUTHORED PRESENTATION %s" % ("PASS" if failures == 0 else "FAIL")); quit(1 if failures else 0)

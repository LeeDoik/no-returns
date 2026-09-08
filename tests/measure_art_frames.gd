extends "res://tests/capture_sorting.gd"
func _run() -> void:
	game = load("res://scenes/main.tscn").instantiate(); root.add_child(game); game.practice_game(true)
	game._add_worker(2); game._add_worker(3); game._add_worker(4)
	run_camera(Vector3(3.2,2.5,6),Vector3(-0.5,0.8,1)); await settle(60)
	var samples: Array[float] = []; var last := Time.get_ticks_usec()
	for i in range(180):
		await process_frame; var now := Time.get_ticks_usec(); samples.append((now-last)/1000.0); last = now
	samples.sort()
	print("ART FRAME SAMPLE 1152x720 4 crew / ms median=",samples[90]," p95=",samples[171]," draw_calls=",Performance.get_monitor(Performance.RENDER_TOTAL_DRAW_CALLS_IN_FRAME)," primitives=",Performance.get_monitor(Performance.RENDER_TOTAL_PRIMITIVES_IN_FRAME))
	game.leave_game(); game.free(); await process_frame; quit()

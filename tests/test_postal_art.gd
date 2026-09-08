extends SceneTree
const Art = preload("res://scripts/postal_art.gd")
var failures := 0
func check(ok: bool, message: String) -> void:
	if not ok: failures += 1; push_error(message)
func _initialize() -> void: call_deferred("run")
func run() -> void:
	var before := int(Performance.get_monitor(Performance.OBJECT_ORPHAN_NODE_COUNT))
	var game = load("res://scenes/main.tscn").instantiate(); root.add_child(game); game.practice_game(true); game.set_physics_process(false)
	await process_frame
	var source = game.cargos[2]
	for phase in ["calm","windup","burst"]:
		source.sneeze.phase = phase; source._present()
		var expected: String = {"calm":"Idle","windup":"Warning","burst":"Burst"}[phase]
		for face in ["Idle","Warning","Burst"]:
			check(Art.find_part(source.art_model,"Face"+face).visible == (face == expected),"one matching face during "+phase)
		check(is_zero_approx(Art.find_part(source.art_model,"LidFrontPivot").rotation.x) == (phase == "calm"),"sneeze opens hinged lid")
	for prop in game.reactions.props:
		if prop.kind == 0:
			check(prop.pieces.size() == 6,"six animated cushion cells")
			prop.fire(); check(prop.pieces[0].scale.y < 0.2,"cushion art deflates")
		elif prop.kind == 1:
			prop.fire(); prop.burst_age = 0.2; prop._present(); check(prop.pieces[0].position.y > prop.origins[0].y,"spring top launches")
	var worker = game.workers[1]
	check(worker.art_skeleton.get_bone_count() == 24,"worker rig preserved")
	worker.velocity = Vector3(4.5,0,0); worker._process(0.2)
	worker.velocity = Vector3.ZERO; worker._process(0.1)
	var foot: int = worker.art_skeleton.find_bone("LeftFoot")
	check(worker.art_skeleton.get_bone_pose(foot).is_equal_approx(worker.art_skeleton.get_bone_rest(foot)),"idle restores planted foot")
	check(game.packrat.art_skeleton.get_bone_count() == 6,"rat paw and tail rig preserved")
	game.leave_game(); game.free(); await process_frame; await process_frame
	check(int(Performance.get_monitor(Performance.OBJECT_ORPHAN_NODE_COUNT)) <= before,"art leaves no orphan nodes")
	print("POSTAL ART %s" % ("PASS" if failures == 0 else "FAIL")); quit(1 if failures else 0)

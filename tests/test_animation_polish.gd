extends SceneTree
func _initialize(): call_deferred("run")
func run():
 var w = load("res://scripts/worker.gd").new(); root.add_child(w); w.set_process(false)
 var a = w.animation_motion
 a.update(0.016,Vector3(0,2,0),false,0)
 a.update(0.016,Vector3.ZERO,false,0)
 assert(a.current == "air_rise", "apex must not start landing")
 a.update(0.016,Vector3(0,-1,0),false,0)
 a.update(0.016,Vector3.ZERO,false,0)
 a.update(0.016,Vector3.ZERO,false,0)
 for i in range(30): a.update(1.0/60,Vector3.FORWARD, true,0,0)
 a.update(0.016,Vector3.FORWARD.rotated(Vector3.UP,-deg_to_rad(24)),true,0,0)
 assert(a.current == "carry_walk", "direction boundary must not flicker")
 for i in range(30): a.update(1.0/60,Vector3.ZERO,true,0,float(i)*0.03)
 assert(a.current in ["carry_left","carry_right"],"turning carrier needs footwork")
 w.free(); print("ANIMATION POLISH PASS"); quit()

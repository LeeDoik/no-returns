extends SceneTree
func _initialize(): call_deferred("run")
func run():
 var lab = load("res://scenes/physics_lab.tscn").instantiate()
 root.add_child(lab)
 await physics_frame
 assert(lab.parcels.size() == 10)
 lab.settings.mass = 7.0
 lab.apply_settings()
 assert(lab.parcels[0].body.mass == 7.0)
 var height: float = lab.parcels[0].body.position.y
 for i in range(45): await physics_frame
 assert(lab.parcels[0].body.position.y < height - 0.5)
 lab.reset_trials()
 assert(lab.parcels[0].body.linear_velocity == Vector3.ZERO)
 assert(lab.parcels[0].body.position.is_equal_approx(lab.homes[0]))
 print("PHYSICS LAB PASS")
 quit()

extends SceneTree
func _initialize(): call_deferred("run")
func run():
 var lab = load("res://scenes/physics_lab.tscn").instantiate(); root.add_child(lab)
 lab.set_physics_process(false)
 lab.worker.position = Vector3(0,0.05,6)
 var parcel = lab.parcels[8]
 parcel.body.position = Vector3(0,0.41,4.8)
 var start: Vector3 = parcel.body.position
 lab.grip.attach(parcel.body,0); lab.grip.attach(parcel.body,1)
 assert(parcel.body.position == start and not parcel.body.freeze)
 for i in range(180):
  lab.grip.step(Vector3(0,1.3,4.8),0,Vector3.ZERO,1.0/60)
  await physics_frame
 assert(parcel.body.position.y > 0.8)
 assert(parcel.body.position.is_finite() and parcel.body.linear_velocity.length() < 3)
 var velocity: Vector3 = parcel.body.linear_velocity
 lab.grip.release_all(); assert(parcel.body.linear_velocity == velocity)
 # A 30 kg box cannot be suspended by two 90 N hands under 9.81 m/s².
 parcel.body.position = Vector3(0,0.41,4.8); parcel.body.linear_velocity = Vector3.ZERO; parcel.body.angular_velocity = Vector3.ZERO; parcel.body.rotation = Vector3.ZERO; parcel.body.mass = 30
 lab.grip.attach(parcel.body,0); lab.grip.attach(parcel.body,1)
 for i in range(180):
  lab.grip.step(Vector3(0,1.3,4.8),0,Vector3.ZERO,1.0/60)
  await physics_frame
 assert(parcel.body.position.y < 0.65)
 # Pull through the end wall. Real collision must stop the dynamic box.
 parcel.body.mass = 3; parcel.body.position = Vector3(10.9,1,-3.6); parcel.body.linear_velocity = Vector3.ZERO; parcel.body.angular_velocity = Vector3.ZERO
 for i in range(180):
  lab.grip.step(Vector3(10.9,1,-5.1),0,Vector3.ZERO,1.0/60)
  await physics_frame
 assert(parcel.body.position.z > -4.4)
 assert(parcel.body.position.is_finite())
 print("FORCE GRAB INTEGRATION PASS"); quit()

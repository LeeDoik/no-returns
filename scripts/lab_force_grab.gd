extends RefCounted
# Lab-only compliant grip. All movement is integrated by the physics engine.
var bodies: Array = [null,null]
var anchors: Array[Vector3] = [Vector3.ZERO,Vector3.ZERO]
var spring := 180.0
var damping := 18.0
var max_force := 90.0
var total_force := Vector3.ZERO
var max_error := 0.0
var targets: Array[Vector3] = [Vector3.ZERO,Vector3.ZERO]
var points: Array[Vector3] = [Vector3.ZERO,Vector3.ZERO]

func attach(body: RigidBody3D, hand: int):
 bodies[hand] = body
 anchors[hand] = Vector3(-0.28 if hand == 0 else 0.28,0,0.38)
 body.freeze = false; body.sleeping = false

func release(hand: int):
 bodies[hand] = null

func release_all():
 release(0); release(1); total_force = Vector3.ZERO; max_error = 0

func is_holding() -> bool:
 return is_instance_valid(bodies[0]) or is_instance_valid(bodies[1])

func step(center: Vector3, heading: float, target_velocity: Vector3, delta: float):
 total_force = Vector3.ZERO; max_error = 0
 for hand in range(2):
  if not is_instance_valid(bodies[hand]): continue
  var body: RigidBody3D = bodies[hand]
  var offset := body.global_basis * anchors[hand]
  points[hand] = body.global_position + offset
  targets[hand] = center + anchors[hand].rotated(Vector3.UP,heading)
  var error := targets[hand]-points[hand]
  max_error = maxf(max_error,error.length())
  if error.length() > 2.8:
   release(hand); continue
  var point_velocity := body.linear_velocity + body.angular_velocity.cross(offset)
  # Implicit spring coefficient stabilizes low-mass bodies at the fixed tick rate.
  var effective_mass := maxf(0.1,body.mass*0.5)
  var divisor := 1.0+damping*delta/effective_mass+spring*delta*delta/effective_mass
  var force := ((error*spring+(target_velocity-point_velocity)*damping)/divisor).limit_length(max_force)
  body.apply_force(force,offset)
  total_force += force

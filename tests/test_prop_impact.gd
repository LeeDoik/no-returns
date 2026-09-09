extends SceneTree
func _initialize(): call_deferred("run")
func run():
 var script = load("res://scripts/reactive_prop.gd")
 var paper = script.new(); paper.kind=3; root.add_child(paper); paper.set_process(false)
 paper.arm(Vector3.RIGHT,2.0,Vector3.ZERO); paper.fire()
 var low: float = paper.paper_velocity[0].length()
 paper.reset(); paper.arm(Vector3.RIGHT,10.0,Vector3.ZERO); paper.fire()
 assert(paper.paper_velocity[0].length()>low*1.5,"stronger impact produces faster paper")
 paper.reset(); paper.rotation.y=PI/2; paper.arm(Vector3.BACK,6.0,Vector3.ZERO); paper.fire()
 assert(paper.paper_velocity[0].dot(Vector3.BACK)>4,"world impact direction survives rotated parent")
 var tower=script.new();tower.kind=2;root.add_child(tower);tower.set_process(false)
 assert(tower.pieces[0] is RigidBody3D,"tower pieces use physical motion")
 tower.arm(Vector3.RIGHT,8.0,Vector3(0,0.3,0));tower.fire()
 for i in range(10):await physics_frame
 assert(tower.pieces[0].linear_velocity.x>0,"tower follows incoming impact")
 tower.reset();assert(tower.pieces[0].freeze and tower.pieces[0].linear_velocity==Vector3.ZERO,"reset clears physics")
 paper.free();tower.free();print("PROP IMPACT PASS");quit()

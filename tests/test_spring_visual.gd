extends SceneTree
var failures := 0
func check(ok: bool, message: String):
 if not ok: failures += 1; push_error(message)
func bounds(mesh: MeshInstance3D) -> AABB:
 return mesh.global_transform * mesh.get_aabb()
func _initialize(): call_deferred("run")
func run():
 var prop = load("res://scripts/reactive_prop.gd").new(); prop.kind = 1; root.add_child(prop); prop.set_process(false)
 var art = load("res://scripts/postal_art.gd")
 var plate = art.find_part(prop.visual,"Launch plate")
 var supports: Array = []
 for node in prop.visual.find_children("*","MeshInstance3D",true,false):
  if str(node.name).begins_with("Compression spring") or str(node.name).begins_with("Guide pin") or str(node.name).begins_with("Surface Brushed dark steel"): supports.append(node)
 check(supports.size()>=1,"spring and guide-pin support geometry exists")
 var bottoms: Array = []
 for mesh in supports: bottoms.append(bounds(mesh).position.y)
 var thickness: float = bounds(plate).size.y
 for phase in [0,1,2]:
  for frame in range(61):
   prop.phase = phase; prop.remaining = 0.18*(1.0-frame/60.0); prop.burst_age = frame/60.0; prop._present()
   var floor_y: float = bounds(plate).position.y
   check(absf(bounds(plate).size.y-thickness)<0.001,"plate remains rigid")
   for i in range(supports.size()):
    var box := bounds(supports[i])
    check(absf(box.position.y-bottoms[i])<0.002,"support foot stays fixed")
    check(absf(box.end.y-floor_y)<0.006,"support meets underside without penetration or gap")
 check(absf(prop.pieces[0].position.y-prop.origins[0].y)<0.001,"launch returns continuously to resting height")
 prop.reset(); prop.free()
 print("SPRING VISUAL %s" % ("PASS" if failures==0 else "FAIL")); quit(0 if failures==0 else 1)

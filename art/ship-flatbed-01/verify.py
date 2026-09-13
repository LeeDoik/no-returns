import bpy,json,math
from pathlib import Path
from mathutils import Vector
R=Path(__file__).resolve().parent
bpy.ops.wm.open_mainfile(filepath=str(R/'Flatbed_Integrated.blend'))
s=bpy.context.scene;s.frame_set(40)
meshes=[o for o in s.objects if o.type=='MESH' and not o.name.startswith('REF_')]
for o in meshes:o.data.calc_loop_triangles()
expected=sum(len(o.data.loop_triangles) for o in meshes)
points=[o.matrix_world@v.co for o in meshes for v in o.data.vertices]
lo=[min(v[i] for v in points) for i in range(3)];hi=[max(v[i] for v in points) for i in range(3)]
report={'export_expected_triangles':expected,'open_bounds_min':lo,'open_bounds_max':hi,'seats':len([o for o in meshes if o.name.startswith('SeatBack')]),'corridor_rays':[]}
deps=bpy.context.evaluated_depsgraph_get()
for x in [-.6,0,.6]:
 for z in [1.0,2.2]:
  hit,loc,normal,face,obj,matrix=s.ray_cast(deps,Vector((x,-3.7,z)),Vector((0,1,0)),distance=5.3)
  report['corridor_rays'].append({'x':x,'z':z,'clear':not hit,'hit':obj.name if hit else None,'point':list(loc) if hit else None})
bpy.ops.object.select_all(action='SELECT');bpy.ops.object.delete(use_global=False)
bpy.ops.import_scene.gltf(filepath=str(R/'Flatbed_Integrated.glb'))
loaded=[o for o in bpy.context.scene.objects if o.type=='MESH']
for o in loaded:o.data.calc_loop_triangles()
report['roundtrip_triangles']=sum(len(o.data.loop_triangles) for o in loaded)
report['roundtrip_geometry_match']=report['roundtrip_triangles']==expected
report['uv_missing']=[o.name for o in loaded if not o.data.uv_layers]
report['invalid_coordinates']=sum(not all(math.isfinite(a) for a in v.co) for o in loaded for v in o.data.vertices)
report['unity_manual_validation']=False
(R/'roundtrip-validation.json').write_text(json.dumps(report,indent=2))
print(json.dumps(report,indent=2))
assert report['roundtrip_geometry_match'] and report['seats']==4 and not report['uv_missing'] and not report['invalid_coordinates']

import bpy,json
from pathlib import Path
from mathutils import Vector
R=Path(__file__).resolve().parent
bpy.ops.object.select_all(action='SELECT');bpy.ops.object.delete(use_global=False)
bpy.ops.import_scene.fbx(filepath=str(next((R/'source').rglob('*.fbx'))))
obs=[o for o in bpy.context.scene.objects if o.type=='MESH']
print('SOURCE',[(o.name,list(o.dimensions),list(o.rotation_euler)) for o in obs])
pts=[o.matrix_world@v.co for o in obs for v in o.data.vertices];lo=Vector([min(p[i] for p in pts) for i in range(3)]);hi=Vector([max(p[i] for p in pts) for i in range(3)]);c=(lo+hi)/2;d=max(hi-lo)
print('BOUNDS',list(lo),list(hi))
s=bpy.context.scene;s.render.engine='CYCLES';s.cycles.samples=12;s.render.resolution_x=900;s.render.resolution_y=700;s.render.resolution_percentage=100;s.world.color=(.3,.3,.3)
for offset in [(2,-3,4),(-3,1,2)]:
 bpy.ops.object.light_add(type='AREA',location=c+Vector(offset)*d);o=bpy.context.object;o.data.energy=700*d*d;o.data.size=d*3;o.rotation_euler=(c-o.location).to_track_quat('-Z','Y').to_euler()
bpy.ops.object.camera_add();cam=bpy.context.object;s.camera=cam;cam.data.type='ORTHO';cam.data.ortho_scale=d*1.3
for name,offset in [('source_front',(1,-2,1)),('source_back',(-1,2,1))]:
 cam.location=c+Vector(offset)*d;cam.rotation_euler=(c-cam.location).to_track_quat('-Z','Y').to_euler();s.render.filepath=str(R/'review'/f'{name}.png');bpy.ops.render.render(write_still=True)
(R/'source-bounds.json').write_text(json.dumps({'lo':list(lo),'hi':list(hi)}))

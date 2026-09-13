"""Validate downloaded Smart Mesh FBXs and render actual candidate geometry."""
import bpy, json, zipfile, hashlib, math
from pathlib import Path
from mathutils import Vector, Matrix
root=Path(__file__).resolve().parent/'smart'
root.mkdir(exist_ok=True)
files=[]
for module in ['Wall','Corner','Door','Floor','Lamp','Rack']:
 for variant in 'ABC':
    name=f'NR_{module}_Smart_{variant}'
    archive=Path('C:/Users/LeeDoik/Downloads')/(name+'_Textured.zip')
    if not archive.exists():archive=archive.with_name(name+'.zip')
    dest=(root/archive.stem).resolve();dest.mkdir(exist_ok=True)
    with zipfile.ZipFile(archive) as z:
        assert z.testzip() is None
        for item in z.infolist():assert (dest/item.filename).resolve().is_relative_to(dest)
        z.extractall(dest)
    files.append(next(dest.glob('*.fbx')))
bpy.ops.object.select_all(action='SELECT');bpy.ops.object.delete(use_global=False)
report=[]
for i,p in enumerate(files):
    before=set(bpy.data.objects);bpy.ops.import_scene.fbx(filepath=str(p))
    meshes=[o for o in set(bpy.data.objects)-before if o.type=='MESH']
    print('INSPECT_SMART',p.name,len(meshes),[len(o.data.uv_layers) for o in meshes])
    if not meshes or not all(o.data.uv_layers for o in meshes):
        report.append(dict(file=str(p.relative_to(root)),error='missing mesh or UV'))
        for o in meshes: bpy.data.objects.remove(o,do_unlink=True)
        continue
    points=[o.matrix_world@Vector(v) for o in meshes for v in o.bound_box]
    lo=Vector([min(v[k] for v in points) for k in range(3)]);hi=Vector([max(v[k] for v in points) for k in range(3)])
    center=Vector(((lo.x+hi.x)/2,(lo.y+hi.y)/2,lo.z));scale=1/max(hi-lo)
    for o in meshes:
        world=o.matrix_world.copy();o.parent=None;o.matrix_world=Matrix.Identity(4)
        for v in o.data.vertices:v.co=(world@v.co-center)*scale
        o.location=((i%3)*1.6,-(i//3)*1.6,0)
    report.append(dict(file=str(p.relative_to(root)),polygons=sum(len(o.data.polygons) for o in meshes),uv=True,sha256=hashlib.sha256(p.read_bytes()).hexdigest()))
    bpy.ops.object.text_add(location=((i%3)*1.6-0.55,-(i//3)*1.6-0.55,0))
    label=bpy.context.object;label.data.body=p.stem.replace('NR_','').replace('_Smart_',' ').replace('_Textured','');label.data.size=0.14
scene=bpy.context.scene;scene.render.engine='CYCLES';scene.cycles.samples=24;scene.world.color=(0.35,)*3
bpy.ops.object.light_add(type='AREA',location=(3,-3,8));bpy.context.object.data.energy=1800;bpy.context.object.data.size=8
rows=6;target=Vector((1.6,-4,0.3))
bpy.ops.object.camera_add(location=target+Vector((1,-5,11)));camera=bpy.context.object;camera.rotation_euler=(target-camera.location).to_track_quat('-Z','Y').to_euler();camera.data.type='ORTHO';camera.data.ortho_scale=max(10,rows*2);scene.camera=camera
scene.render.resolution_x=1400;scene.render.resolution_y=2000;scene.render.resolution_percentage=100;scene.render.filepath=str(root/'candidate-review.png')
bpy.ops.wm.save_as_mainfile(filepath=str(root/'candidate-review.blend'));bpy.ops.render.render(write_still=True)
(root/'validation.json').write_text(json.dumps(report,indent=2),encoding='utf-8');print('SMART_FBX_CHECKED',len(report),'PASS',sum('error' not in r for r in report),'FAIL',sum('error' in r for r in report))

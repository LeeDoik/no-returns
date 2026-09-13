import bpy,json
from pathlib import Path
from mathutils import Vector,Matrix
root=Path(__file__).resolve().parent
out=root/'selected';out.mkdir(exist_ok=True)
items=[('Wall','B',0,3.0),('Corner','C',2,3.0),('Door','B',2,3.2),('Floor','B',0,3.0),('Lamp','C',0,1.2),('Rack','A',2,2.4)]
records=json.loads((root/'smart/validation.json').read_text()); report=[]
bpy.ops.object.select_all(action='SELECT');bpy.ops.object.delete(use_global=False)
for i,(name,variant,axis,size) in enumerate(items):
 r=next(r for r in records if Path(r['file']).stem.replace('_Textured','')==f'NR_{name}_Smart_{variant}')
 before=set(bpy.data.objects);bpy.ops.import_scene.fbx(filepath=str(root/'smart'/r['file']))
 meshes=[o for o in set(bpy.data.objects)-before if o.type=='MESH']
 bpy.ops.object.select_all(action='DESELECT')
 for o in meshes:o.select_set(True)
 bpy.context.view_layer.objects.active=meshes[0]
 if len(meshes)>1:bpy.ops.object.join()
 o=bpy.context.object;world=o.matrix_world.copy();o.parent=None
 pts=[world@Vector(v) for v in o.bound_box];lo=Vector([min(v[k] for v in pts) for k in range(3)]);hi=Vector([max(v[k] for v in pts) for k in range(3)])
 center=Vector(((lo.x+hi.x)/2,(lo.y+hi.y)/2,lo.z));scale=size/(hi-lo)[axis]
 for v in o.data.vertices:v.co=(world@v.co-center)*scale
 o.matrix_world=Matrix.Identity(4);o.name=f'NR_{name}_Selected'
 bpy.ops.object.mode_set(mode='EDIT');bpy.ops.mesh.select_all(action='SELECT');bpy.ops.mesh.delete_loose();bpy.ops.mesh.normals_make_consistent(inside=False);bpy.ops.object.mode_set(mode='OBJECT')
 textures=[]
 for mat in o.data.materials:
  mat.name=f'NR_{name}_Selected_Mat'
  for node in mat.node_tree.nodes:
   if node.type=='TEX_IMAGE' and node.image:
    img=node.image.copy();img.scale(512,512);img.filepath_raw=str(out/f'NR_{name}_BaseColor_512.png');img.file_format='PNG';img.save();node.image=img;node.interpolation='Closest';textures.append(Path(img.filepath_raw).name)
 bpy.context.view_layer.update();o.data.calc_loop_triangles()
 report.append(dict(module=name,variant=variant,source=r['file'],triangles=len(o.data.loop_triangles),polygons=len(o.data.polygons),dimensions_m=list(o.dimensions),textures=textures,pivot='bottom center',status='offline review; scale proposal; no collision or Unity validation'))
 bpy.ops.export_scene.fbx(filepath=str(out/f'NR_{name}_Selected.fbx'),use_selection=True,object_types={'MESH'},axis_forward='-Z',axis_up='Y',path_mode='COPY',embed_textures=True,add_leaf_bones=False)
 o.scale=(1/max(o.dimensions),)*3;o.location=((i%3)*1.6,-(i//3)*1.8,0)
 bpy.ops.object.text_add(location=((i%3)*1.6-0.5,-(i//3)*1.8-0.55,0));bpy.context.object.data.body=f'{name} {variant}';bpy.context.object.data.size=0.13
scene=bpy.context.scene;scene.render.engine='CYCLES';scene.cycles.samples=24;scene.world.color=(0.35,)*3
bpy.ops.object.light_add(type='AREA',location=(1,-2,6));bpy.context.object.data.energy=1200;bpy.context.object.data.size=6
bpy.ops.object.camera_add(location=(3,-6,6));cam=bpy.context.object;cam.rotation_euler=(Vector((1.6,-0.9,0.2))-cam.location).to_track_quat('-Z','Y').to_euler();cam.data.type='ORTHO';cam.data.ortho_scale=5.5;scene.camera=cam
scene.render.resolution_x=1400;scene.render.resolution_y=1100;scene.render.resolution_percentage=100;scene.render.filepath=str(out/'selected-review.png')
bpy.ops.wm.save_as_mainfile(filepath=str(out/'selected-review.blend'));bpy.ops.render.render(write_still=True)
(out/'selection.json').write_text(json.dumps(report,indent=2))
checks=[]
for r in report:
 bpy.ops.object.select_all(action='SELECT');bpy.ops.object.delete(use_global=False)
 bpy.ops.import_scene.fbx(filepath=str(out/f"NR_{r['module']}_Selected.fbx"))
 meshes=[o for o in bpy.context.scene.objects if o.type=='MESH'];assert meshes and all(o.data.uv_layers for o in meshes)
 total=0
 for o in meshes:o.data.calc_loop_triangles();total+=len(o.data.loop_triangles)
 assert total==r['triangles'];assert all(Path(out/t).exists() for t in r['textures'])
 checks.append(dict(module=r['module'],triangles=total,uv=True,texture_files=True))
(out/'roundtrip-validation.json').write_text(json.dumps(checks,indent=2));print('SELECTED_ROUNDTRIP_PASS',len(checks))

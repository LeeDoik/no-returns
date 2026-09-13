import bpy,json
from pathlib import Path
from mathutils import Vector,Matrix
root=Path(__file__).resolve().parent
out=root/'selected';out.mkdir(exist_ok=True)
items=[('Parcel','Approved',2,.65),('Receipt','Approved',2,1.6)]
import zipfile,hashlib
records=[]
for name,variant,axis,size in items:
 archive=Path('C:/Users/LeeDoik/Downloads')/f'NR_{name}_Approved.zip'
 dest=(root/'source'/name).resolve();dest.mkdir(parents=True,exist_ok=True)
 with zipfile.ZipFile(archive) as z:
  assert z.testzip() is None
  for item in z.infolist():assert (dest/item.filename).resolve().is_relative_to(dest)
  z.extractall(dest)
 records.append(dict(file=str(next(dest.glob('*.fbx'))),sha256=hashlib.sha256(archive.read_bytes()).hexdigest()))
(root/'sources.json').write_text(json.dumps(records,indent=2))
report=[]
bpy.ops.object.select_all(action='SELECT');bpy.ops.object.delete(use_global=False)
for i,(name,variant,axis,size) in enumerate(items):
 r=next(r for r in records if Path(r['file']).stem==f'NR_{name}_{variant}')
 before=set(bpy.data.objects);bpy.ops.import_scene.fbx(filepath=r['file'])
 meshes=[o for o in set(bpy.data.objects)-before if o.type=='MESH'];assert meshes and all(o.data.uv_layers for o in meshes)
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
 if name=='Parcel':
  # The generated unseen rear had an unapproved circular mechanism. Repair only that surface.
  rear=max(v.co.y for v in o.data.vertices);front=min(v.co.y for v in o.data.vertices);plane=front+(rear-front)*.78
  backmat=bpy.data.materials.new('Parcel_RepairedRear');backmat.diffuse_color=(.43,.35,.24,1);backmat.use_nodes=True
  bs=backmat.node_tree.nodes.get('Principled BSDF');bs.inputs['Base Color'].default_value=(.43,.35,.24,1);bs.inputs['Roughness'].default_value=1
  o.data.materials.append(backmat);slot=len(o.data.materials)-1
  import bmesh
  bm=bmesh.new();bm.from_mesh(o.data)
  plane=front+(rear-front)*.55
  cut=bmesh.ops.bisect_plane(bm,geom=list(bm.verts)+list(bm.edges)+list(bm.faces),dist=.00001,plane_co=(0,plane,0),plane_no=(0,1,0),clear_outer=True,clear_inner=False)
  # Reuse the approved textured front half, rotated, to close the rear without inventing machinery.
  duplicated=bmesh.ops.duplicate(bm,geom=list(bm.verts)+list(bm.edges)+list(bm.faces))['geom']
  for v in duplicated:
   if isinstance(v,bmesh.types.BMVert):v.co=Vector((-v.co.x,2*plane-v.co.y,v.co.z))
  bmesh.ops.remove_doubles(bm,verts=list(bm.verts),dist=.00001)
  bmesh.ops.recalc_face_normals(bm,faces=list(bm.faces));bm.to_mesh(o.data);bm.free()
  o.data.update()
 bpy.context.view_layer.update();o.data.calc_loop_triangles()
 report.append(dict(module=name,variant=variant,source=r['file'],triangles=len(o.data.loop_triangles),polygons=len(o.data.polygons),dimensions_m=list(o.dimensions),textures=textures,pivot='bottom center',status='offline review; scale proposal; no collision or Unity validation'))
 bpy.ops.export_scene.fbx(filepath=str(out/f'NR_{name}_Selected.fbx'),use_selection=True,object_types={'MESH'},axis_forward='-Z',axis_up='Y',path_mode='COPY',embed_textures=True,add_leaf_bones=False)
 o.scale=(1/max(o.dimensions),)*3;o.location=((i%3)*1.6,-(i//3)*1.8,0)
 bpy.ops.object.text_add(location=((i%3)*1.6-0.5,-(i//3)*1.8-0.55,0));bpy.context.object.data.body=f'{name} {variant}';bpy.context.object.data.size=0.13
scene=bpy.context.scene;scene.render.engine='CYCLES';scene.cycles.samples=24;scene.world.color=(0.35,)*3
bpy.ops.object.light_add(type='AREA',location=(1,-2,6));bpy.context.object.data.energy=1200;bpy.context.object.data.size=6
bpy.ops.object.camera_add(location=(2,-5,3));cam=bpy.context.object;cam.rotation_euler=(Vector((.8,0,.3))-cam.location).to_track_quat('-Z','Y').to_euler();cam.data.type='ORTHO';cam.data.ortho_scale=3.5;scene.camera=cam
scene.render.resolution_x=1400;scene.render.resolution_y=1100;scene.render.resolution_percentage=100;scene.render.filepath=str(out/'selected-review.png')
bpy.ops.wm.save_as_mainfile(filepath=str(out/'selected-review.blend'));bpy.ops.render.render(write_still=True)
(out/'selection.json').write_text(json.dumps(report,indent=2))
cam.location=Vector((2,5,3));cam.rotation_euler=(Vector((.8,0,.3))-cam.location).to_track_quat('-Z','Y').to_euler();scene.render.filepath=str(out/'rear-review.png');bpy.ops.render.render(write_still=True)
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

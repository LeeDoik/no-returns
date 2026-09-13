import bpy,json,math
from pathlib import Path
from mathutils import Vector,Matrix
root=Path(__file__).resolve().parent;out=root/'selected';out.mkdir(exist_ok=True)
bpy.ops.object.select_all(action='SELECT');bpy.ops.object.delete(use_global=False)
source=next((root/'source').rglob('*.fbx'));bpy.ops.import_scene.fbx(filepath=str(source))
meshes=[o for o in bpy.context.scene.objects if o.type=='MESH'];assert meshes and all(o.data.uv_layers for o in meshes)
bpy.ops.object.select_all(action='DESELECT')
for o in meshes:o.select_set(True)
bpy.context.view_layer.objects.active=meshes[0]
if len(meshes)>1:bpy.ops.object.join()
o=bpy.context.object;world=o.matrix_world.copy();o.parent=None
pts=[world@Vector(v) for v in o.bound_box];lo=Vector([min(v[k] for v in pts) for k in range(3)]);hi=Vector([max(v[k] for v in pts) for k in range(3)])
center=Vector(((lo.x+hi.x)/2,(lo.y+hi.y)/2,lo.z));scale=.44/(hi.z-lo.z)
for v in o.data.vertices:v.co=(world@v.co-center)*scale
o.matrix_world=Matrix.Identity(4);o.name='NR_Beacon_Selected'
for mat in o.data.materials:
 mat.name='NR_Beacon_Mat'
 for node in mat.node_tree.nodes:
  if node.type=='TEX_IMAGE' and node.image:
   img=node.image.copy();img.scale(512,512);img.filepath_raw=str(out/'NR_Beacon_BaseColor_512.png');img.file_format='PNG';img.save();node.image=img;node.interpolation='Closest'
bpy.ops.object.mode_set(mode='EDIT');bpy.ops.mesh.select_all(action='SELECT');bpy.ops.mesh.delete_loose();bpy.ops.mesh.normals_make_consistent(inside=False);bpy.ops.object.mode_set(mode='OBJECT')
o.data.calc_loop_triangles();report=dict(triangles=len(o.data.loop_triangles),vertices=len(o.data.vertices),uv_layers=len(o.data.uv_layers),dimensions=list(o.dimensions),source=str(source))
bpy.ops.export_scene.fbx(filepath=str(out/'NR_Beacon_Selected.fbx'),use_selection=True,add_leaf_bones=False,bake_anim=False,axis_forward='-Z',axis_up='Y',path_mode='AUTO')
(out/'validation.json').write_text(json.dumps(report,indent=2))
scene=bpy.context.scene;scene.render.engine='CYCLES';scene.cycles.samples=24;scene.render.resolution_x=900;scene.render.resolution_y=900;scene.render.resolution_percentage=100
scene.world.color=(.16,.16,.16)
for loc,power,size in [((1,-2,3),180,2),((-2,-1,1),80,2),((0,2,2),120,1.5)]:
 bpy.ops.object.light_add(type='AREA',location=loc);bpy.context.object.data.energy=power;bpy.context.object.data.shape='DISK';bpy.context.object.data.size=size
bpy.ops.object.camera_add(location=(.7,-1.3,.65));cam=bpy.context.object;scene.camera=cam;cam.data.type='ORTHO';cam.data.ortho_scale=.75
for name,pos in [('front',(.7,-1.3,.65)),('rear',(-.7,1.3,.65))]:
 cam.location=pos;cam.rotation_euler=(Vector((0,0,.22))-cam.location).to_track_quat('-Z','Y').to_euler();scene.render.filepath=str(out/(name+'.png'));bpy.ops.render.render(write_still=True)
bpy.ops.wm.save_as_mainfile(filepath=str(out/'beacon-review.blend'))
# Reimport exported FBX independently to verify geometry and UV survived.
bpy.ops.wm.read_factory_settings(use_empty=True);bpy.ops.import_scene.fbx(filepath=str(out/'NR_Beacon_Selected.fbx'))
mm=[x for x in bpy.context.scene.objects if x.type=='MESH'];assert mm and all(x.data.uv_layers for x in mm)
(out/'roundtrip.json').write_text(json.dumps(dict(meshes=len(mm),uv=True)))
print('BEACON_PREPARED',report)

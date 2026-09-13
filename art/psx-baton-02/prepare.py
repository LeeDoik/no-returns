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
# Align the principal shaft axis, keeping the fork at positive Z.
import numpy as np
coords=np.array([list(world@v.co) for v in o.data.vertices]);mean=coords.mean(axis=0)
values,vectors=np.linalg.eigh(np.cov((coords-mean).T));axis=Vector(vectors[:,-1]);axis=axis if axis.z>0 else -axis
rotation=axis.rotation_difference(Vector((0,0,1))).to_matrix()
pts=[rotation@(world@v.co) for v in o.data.vertices]
lo=Vector([min(v[k] for v in pts) for k in range(3)]);hi=Vector([max(v[k] for v in pts) for k in range(3)])
center=Vector(((lo.x+hi.x)/2,(lo.y+hi.y)/2,lo.z));scale=.68/(hi.z-lo.z)
for v,p in zip(o.data.vertices,pts):v.co=(p-center)*scale
o.matrix_world=Matrix.Identity(4);o.name='NR_Baton_Selected'
for mat in o.data.materials:
 mat.name='NR_Baton_Mat'
 for node in mat.node_tree.nodes:
  if node.type=='TEX_IMAGE' and node.image:
   img=node.image.copy();img.scale(512,512);img.filepath_raw=str(out/'NR_Baton_BaseColor_512.png');img.file_format='PNG';img.save();node.image=img;node.interpolation='Closest'
bpy.ops.object.mode_set(mode='EDIT');bpy.ops.mesh.select_all(action='SELECT');bpy.ops.mesh.delete_loose();bpy.ops.mesh.normals_make_consistent(inside=False);bpy.ops.object.mode_set(mode='OBJECT')
o.data.calc_loop_triangles();report=dict(triangles=len(o.data.loop_triangles),vertices=len(o.data.vertices),uv_layers=len(o.data.uv_layers),dimensions=list(o.dimensions),source=str(source))
# Locate green charge pixels on the generated mesh for screen fitting.
img=bpy.data.images.load(str(out/'NR_Baton_BaseColor_512.png'),check_existing=False)
pixels=list(img.pixels);w,h=img.size
matches=[]
for poly in o.data.polygons:
 uv=sum((o.data.uv_layers.active.data[i].uv for i in poly.loop_indices),Vector((0,0)))/len(poly.loop_indices)
 x,y=int(uv.x*w)%w,int(uv.y*h)%h;rr,gg,bb=pixels[(y*w+x)*4:(y*w+x)*4+3]
 if gg>.25 and gg>rr*1.35 and gg>bb*1.3:
  matches.append(dict(p=list(poly.center),n=list(poly.normal),index=poly.index))
(out/'green-faces.json').write_text(json.dumps(matches))
# Replace only the inner glass region; preserve the generated recessed frame.
import bmesh
bm=bmesh.new();bm.from_mesh(o.data)
remove=[f for f in bm.faces if -.046<f.calc_center_median().x<-.026 and -.047<f.calc_center_median().y<-.032 and .328<f.calc_center_median().z<.444]
bmesh.ops.delete(bm,geom=remove,context='FACES');bm.to_mesh(o.data);bm.free()
mesh=bpy.data.meshes.new('BatonScreen');mesh.from_pydata([(-.046,-.0426,.328),(-.026,-.0426,.328),(-.026,-.0426,.444),(-.046,-.0426,.444)],[],[(0,1,2,3)]);mesh.update()
glass=bpy.data.objects.new('BatonScreen',mesh);bpy.context.collection.objects.link(glass)
uv=mesh.uv_layers.new(name='ScreenUV')
for i,xy in enumerate([(0,0),(1,0),(1,1),(0,1)]):uv.data[i].uv=xy
mat=bpy.data.materials.new('BatonScreen');mat.diffuse_color=(.01,.02,.01,1);mesh.materials.append(mat)
glass.select_set(True);o.select_set(True)
report['screen_faces_removed']=len(remove);report['screen_separate_uv']=True
bpy.ops.export_scene.fbx(filepath=str(out/'NR_Baton_Selected.fbx'),use_selection=True,add_leaf_bones=False,bake_anim=False,axis_forward='-Z',axis_up='Y',path_mode='AUTO')
(out/'validation.json').write_text(json.dumps(report,indent=2))
scene=bpy.context.scene;scene.render.engine='CYCLES';scene.cycles.samples=24;scene.render.resolution_x=900;scene.render.resolution_y=900;scene.render.resolution_percentage=100
scene.world.color=(.16,.16,.16)
for loc,power,size in [((1,-2,3),180,2),((-2,-1,1),80,2),((0,2,2),120,1.5)]:
 bpy.ops.object.light_add(type='AREA',location=loc);bpy.context.object.data.energy=power;bpy.context.object.data.shape='DISK';bpy.context.object.data.size=size
bpy.ops.object.camera_add(location=(.7,-1.3,.65));cam=bpy.context.object;scene.camera=cam;cam.data.type='ORTHO';cam.data.ortho_scale=1.05
for name,pos in [('front',(.7,-1.3,.65)),('rear',(-.7,1.3,.65))]:
 cam.location=pos;cam.rotation_euler=(Vector((0,0,.34))-cam.location).to_track_quat('-Z','Y').to_euler();scene.render.filepath=str(out/(name+'.png'));bpy.ops.render.render(write_still=True)
bpy.ops.wm.save_as_mainfile(filepath=str(out/'baton-review.blend'))
# Reimport exported FBX independently to verify geometry and UV survived.
bpy.ops.wm.read_factory_settings(use_empty=True);bpy.ops.import_scene.fbx(filepath=str(out/'NR_Baton_Selected.fbx'))
mm=[x for x in bpy.context.scene.objects if x.type=='MESH'];assert mm and all(x.data.uv_layers for x in mm)
(out/'roundtrip.json').write_text(json.dumps(dict(meshes=len(mm),uv=True)))
print('BATON_PREPARED',report)

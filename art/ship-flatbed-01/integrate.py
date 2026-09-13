import bpy,bmesh,math,json
from pathlib import Path
from mathutils import Vector,Matrix
R=Path(__file__).resolve().parent;O=R/'review'
bpy.ops.wm.open_mainfile(filepath=str(R/'Flatbed_Structural.blend'))
# Keep the coherent cabin and mechanisms; replace provisional outer styling.
remove=('Engine','VentSlat','OrangeBand','Shoulder','Landing','NoseLower','NoseCheek')
for o in list(bpy.context.scene.objects):
 if o.name.startswith(remove):bpy.data.objects.remove(o,do_unlink=True)
# Fit the cabin inside the generated pressure hull: 4.352m clear width.
for o in list(bpy.context.scene.objects):
 if o.type=='MESH' and not o.name.startswith('REF_'):
  if o.name.startswith(('CockpitGlass','Window','TaperWall')):bpy.data.objects.remove(o,do_unlink=True);continue
  if not o.name.startswith(('Door_','Ramp','Rear')):
   o.location.x*=.68;o.scale.x*=.68
for o in bpy.context.scene.objects:
 if o.type=='MESH' and o.name.startswith(('Roof','CabinWall')):
  for v in o.data.vertices:
   world=o.matrix_world@v.co
   if world.z>2.9:v.co.z-=.25
before=set(bpy.context.scene.objects)
bpy.ops.import_scene.fbx(filepath=str(next((R/'source').rglob('*.fbx'))))
src=[o for o in bpy.context.scene.objects if o not in before and o.type=='MESH'][0]
src.name='Tripo_Hull_Reworked';world=src.matrix_world.copy()
for v in src.data.vertices:
 p=world@v.co;v.co=(-p.x*10.8/.672363162,-p.y*11.2/.999511719,p.z*4.45/.411621094)
src.parent=None;src.matrix_world=Matrix.Identity(4)
for m in src.data.materials:
 if m and m.use_nodes:
  for n in m.node_tree.nodes:
   if n.type=='TEX_IMAGE' and n.image:
    n.image.scale(2048,2048);n.image.filepath_raw=str(R/'Hull_BaseColor_2048.png');n.image.file_format='PNG';n.image.save();n.interpolation='Closest'
# Split the rear opening; keep the original generated exterior everywhere else.
bm=bmesh.new();bm.from_mesh(src.data)
for axis,values in [(0,[-2.18,-1.53,1.53,2.18]),(1,[-3.8,3.8]),(2,[.58,2.88])]:
 for value in values:
  co=Vector((0,0,0));co[axis]=value;no=Vector((0,0,0));no[axis]=1
  bmesh.ops.bisect_plane(bm,geom=list(bm.verts)+list(bm.edges)+list(bm.faces),dist=.00001,plane_co=co,plane_no=no)
remove=[]
for f in bm.faces:
 x,y,z=f.calc_center_median()
 if .58<z<2.88 and ((abs(x)<1.53 and y<-3.8) or (abs(x)<2.18 and -3.8<y<3.8)):remove.append(f)
bmesh.ops.delete(bm,geom=remove,context='FACES');bm.to_mesh(src.data);bm.free()
# Replace actual blue glazing faces with glass in place. No detached replacement windshield.
source_mat=src.data.materials[0];image=next(n.image for n in source_mat.node_tree.nodes if n.type=='TEX_IMAGE' and n.image)
pixels=list(image.pixels);w,h=image.size;uvdata=src.data.uv_layers.active.data
src.data.materials.append(bpy.data.materials.get('Glazing'));glassidx=len(src.data.materials)-1;glassfaces=0
for f in src.data.polygons:
 if f.center.y<4.0 or not (2.55<f.center.z<3.25) or not (-3.1<f.center.x<2.0):continue
 uv=sum((uvdata[i].uv for i in f.loop_indices),Vector((0,0)))/len(f.loop_indices)
 xx=int(uv.x*w)%w;yy=int(uv.y*h)%h;r,g,b=pixels[(yy*w+xx)*4:(yy*w+xx)*4+3]
 if b>r*1.025 and g>r*1.015 and max(r,g,b)<.8:f.material_index=glassidx;glassfaces+=1
print('GLASS_FACES',glassfaces)
# Use the same textured metal finish for interior panel faces; UVs remain editable.
wallmat=bpy.data.materials.get('Paint_Ivory');nodes=wallmat.node_tree.nodes
tex=nodes.new('ShaderNodeTexImage');tex.image=bpy.data.images.load(str(R/'Hull_BaseColor_2048.png'),check_existing=True);tex.interpolation='Closest'
# Keep an editable simple paint material for the fabricated cabin; full atlas would stamp unrelated labels.
wallmat.node_tree.nodes.get('Principled BSDF').inputs['Base Color'].default_value=(.29,.255,.20,1)
glassmat=bpy.data.materials.get('Glazing');gp=glassmat.node_tree.nodes.get('Principled BSDF');gp.inputs['Base Color'].default_value=(.65,.8,.82,1);gp.inputs['IOR'].default_value=1.05;gp.inputs['Roughness'].default_value=.035
# Reuse the approved Tripo rack family for interior continuity.
for o in list(bpy.context.scene.objects):
 if o.name.startswith(('CargoRack','Shelf')):bpy.data.objects.remove(o,do_unlink=True)
prior=set(bpy.context.scene.objects)
bpy.ops.import_scene.fbx(filepath=str(R.parents[1]/'NoReturns/Assets/_NoReturns/Art/PSXKit01/Selected/NR_Rack_Selected.fbx'))
rackparts=[o for o in bpy.context.scene.objects if o not in prior and o.type=='MESH']
for j,o in enumerate(rackparts):
 pts=[o.matrix_world@v.co for v in o.data.vertices];lo=Vector([min(p[i] for p in pts) for i in range(3)]);hi=Vector([max(p[i] for p in pts) for i in range(3)])
 for v,p in zip(o.data.vertices,pts):v.co=((p.x-(lo.x+hi.x)/2)*.54/(hi.x-lo.x),(p.y-(lo.y+hi.y)/2)*1.15/(hi.y-lo.y),(p.z-lo.z)*1.2/(hi.z-lo.z))
 o.parent=None;o.matrix_world=Matrix.Identity(4);o.name='InteriorRack_0_'+str(j);o.location=(-1.904,-2.2,.6)
 for k,(x,y) in enumerate([(-1.904,-.9),(1.904,-2.2),(1.904,-.9)],1):
  c=o.copy();c.data=o.data.copy();bpy.context.collection.objects.link(c);c.name='InteriorRack_'+str(k)+'_'+str(j);c.location=(x,y,.6)
for o in bpy.context.scene.objects:
 if o.type=='LIGHT' and abs(o.location.x)<.1 and o.location.z<4:o.location.z=2.65
# The exterior window sill requires a raised cockpit, not an artificially lowered windshield.
for o in bpy.context.scene.objects:
 if o.name=='Roof':
  for v in o.data.vertices:
   if v.co.y>1.8:v.co.y=1.8
 if o.name.startswith(('Console','ConsoleScreen')):o.location.z+=.6
for n,loc,dim in [('CockpitDeck',(0,3.25,.9),(4.3,1.1,.6)),('Step01',(0,1.95,.7),(2.5,.3,.2)),('Step02',(0,2.25,.8),(2.5,.3,.4)),('Step03',(0,2.55,.9),(2.5,.3,.6))]:
 bpy.ops.mesh.primitive_cube_add(size=1,location=loc);o=bpy.context.object;o.name=n;o.dimensions=dim;bpy.ops.object.transform_apply(location=False,rotation=False,scale=True);o.data.materials.append(bpy.data.materials.get('Floor'))
 bpy.ops.object.mode_set(mode='EDIT');bpy.ops.mesh.select_all(action='SELECT');bpy.ops.uv.smart_project();bpy.ops.object.mode_set(mode='OBJECT')
# Preserve all references but exclude them from game exports.
s=bpy.context.scene;s.frame_set(40)
for o in s.objects:
 if o.name.startswith('RearJamb'):o.location.x=1.88 if o.location.x>0 else -1.88;o.dimensions.x=.76
 if o.name.startswith('RearLintel'):o.dimensions.x=4.5
meshes=[o for o in s.objects if o.type=='MESH' and not o.name.startswith('REF_')]
bpy.ops.object.select_all(action='DESELECT')
for o in s.objects:
 if o.name.startswith('REF_'):o.select_set(False)
for o in meshes:o.select_set(True)
bpy.ops.export_scene.gltf(filepath=str(R/'Flatbed_Integrated.glb'),use_selection=True,export_animations=True)
bpy.ops.export_scene.fbx(filepath=str(R/'Flatbed_Integrated.fbx'),use_selection=True,add_leaf_bones=False,bake_anim=False,axis_forward='-Z',axis_up='Y',path_mode='COPY',embed_textures=True)
for o in meshes:o.data.calc_loop_triangles()
report={'meshes':len(meshes),'triangles':sum(len(o.data.loop_triangles) for o in meshes),'source_id':'c1864948-b96d-483d-b31d-c00dfca0bba9','source_untouched':True,'uv_missing':[o.name for o in meshes if not o.data.uv_layers],'nonfinite_vertices':sum(not all(math.isfinite(a) for a in v.co) for o in meshes for v in o.data.vertices),'unity_playtest':False}
(R/'integrated-validation.json').write_text(json.dumps(report,indent=2))
cam=s.camera
for name,pos,target,ortho,frame in [('integrated-front',(14,17,11),(0,0,1.5),19,1),('integrated-rear',(12,-17,10),(0,-1,1.5),20,40),('integrated-interior',(0,-3.3,2.17),(0,3.4,1.8),0,40),('integrated-cockpit',(0,3.1,2.77),(0,5.5,2.9),0,40),('integrated-closed',(10,-15,8),(0,-1,1.5),19,1)]:
 s.frame_set(frame);cam.location=pos;cam.rotation_euler=(Vector(target)-cam.location).to_track_quat('-Z','Y').to_euler();cam.data.type='ORTHO' if ortho else 'PERSP';cam.data.ortho_scale=ortho or 16;cam.data.lens=20;s.render.filepath=str(O/(name+'.png'));bpy.ops.render.render(write_still=True)
s.frame_set(40);bpy.ops.file.pack_all();bpy.ops.wm.save_as_mainfile(filepath=str(R/'Flatbed_Integrated.blend'))

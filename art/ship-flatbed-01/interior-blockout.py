"""Build an isolated, flat-deck cabin candidate inside the preserved angular hull."""
import bpy, json, math, hashlib
from pathlib import Path
from mathutils import Vector
from mathutils.bvhtree import BVHTree
R=Path(__file__).resolve().parent
O=R/'interior-blockout';O.mkdir(exist_ok=True)
bpy.ops.wm.open_mainfile(filepath=str(R/'Flatbed_Angular.blend'))
s=bpy.context.scene;s.frame_set(40)
hull=bpy.data.objects['Tripo_Hull_Reworked']
def fingerprint(o):
 return hashlib.sha256(repr(([(tuple(v.co)) for v in o.data.vertices],[(tuple(p.vertices),p.material_index) for p in o.data.polygons])).encode()).hexdigest()
original=fingerprint(hull)
for o in list(s.objects):
 if o!=hull:bpy.data.objects.remove(o,do_unlink=True)
def mat(n,c,emission=0):
 m=bpy.data.materials.new(n);m.diffuse_color=(*c,1);m.use_nodes=True
 p=m.node_tree.nodes.get('Principled BSDF');p.inputs['Base Color'].default_value=(*c,1);p.inputs['Roughness'].default_value=.8
 if emission:p.inputs['Emission Color'].default_value=(*c,1);p.inputs['Emission Strength'].default_value=emission
 return m
ivory=mat('Trial_Ivory',(.46,.43,.35));dark=mat('Trial_Graphite',(.075,.085,.09));orange=mat('Trial_Orange',(.6,.19,.055));screen=mat('Trial_Screen',(.025,.18,.15),.5);cyan=mat('Trial_Cyan',(.08,.65,.5),1);lamp=mat('Trial_Lamp',(.8,.65,.4),2)
boxes=[]
def box(n,loc,dim,m,collision=True):
 bpy.ops.mesh.primitive_cube_add(size=1,location=loc);o=bpy.context.object;o.name=n;o.dimensions=dim;bpy.ops.object.transform_apply(location=False,rotation=False,scale=True);o.data.materials.append(m)
 if collision:boxes.append({'name':n,'position':list(loc),'size':list(dim)})
 return o
floor=1.0
box('Trial_Deck',(0,0,.9),(4.352,7.6,.2),dark)
box('Trial_Threshold',(0,-3.9,.9),(3,.2,.2),dark)
box('Trial_Ceiling',(0,0,3.18),(4.35,7.6,.12),ivory)
for side in [-1,1]:
 box('Trial_Wall_'+str(side),(side*2.19,0,2.04),(.12,7.6,2.08),ivory)
 for j,y in enumerate([.35,1.4]):
  box('Trial_SeatBack_'+str(side)+'_'+str(j),(side*2.02,y,1.91),(.16,.72,.86),orange)
  box('Trial_SeatPan_'+str(side)+'_'+str(j),(side*1.80,y,1.43),(.58,.72,.14),dark)
 for j,y in enumerate([-2.6,-1.3]):
  box('Trial_Rack_'+str(side)+'_'+str(j),(side*1.86,y,1.61),(.54,1.05,1.22),ivory)
 box('Trial_Dock_'+str(side),(side*2.01,.6,2.63),(.16,.44,.38),orange)
 box('Trial_Jamb_'+str(side),(side*1.64,-3.97,1.93),(.28,.16,1.86),ivory)
 box('Trial_OpenDoor_'+str(side),(side*2.3,-4.10,1.91),(1.48,.10,1.82),dark)
 for y in [-2,0,2]:
  box('Trial_Light_'+str(side)+'_'+str(y),(side*1.15,y,3.10),(.62,.14,.035),lamp,False)
  bpy.ops.object.light_add(type='AREA',location=(side*1.1,y,3.07));bpy.context.object.data.energy=24;bpy.context.object.data.size=1.1
box('Trial_Lintel',(0,-3.97,2.91),(3.56,.16,.10),ivory)
box('Trial_ConsoleBase',(0,3.24,1.50),(2.12,.55,1.0),dark)
box('Trial_ConsoleHousing',(0,3.43,2.15),(2.12,.22,.70),ivory)
box('Trial_DisplaySurface',(0,3.309,2.15),(1.92,.015,.53),screen,False)
for side in [-1,1]:
 box('Trial_AuxConsole_'+str(side),(side*1.66,3.12,1.71),(.62,.5,1.42),dark)
 box('Trial_AuxDisplay_'+str(side),(side*1.66,2.857,2.16),(.5,.015,.30),screen,False)
for n,z,text in [('Title',2.28,'NO RETURNS'),('Route',2.12,'CINDER DEPOT'),('Note',1.98,'STRUCTURE TRIAL')]:
 bpy.ops.object.text_add(location=(-.82,3.292,z),rotation=(math.pi/2,0,0));o=bpy.context.object;o.name='Trial_Label_'+n;o.data.body=text;o.data.size=.115;o.data.extrude=0;o.data.materials.append(cyan)
 bpy.ops.object.convert(target='MESH')
# Rear ramp top endpoints: y=-4,z=1 and y=-7,z=0. Separate object.
verts=[(x,y,z-d) for d in [0,.09] for x,y,z in [(-1.5,-4,1),(1.5,-4,1),(1.5,-7,0),(-1.5,-7,0)]]
mesh=bpy.data.meshes.new('Trial_Ramp');mesh.from_pydata(verts,[],[(0,3,2,1),(4,5,6,7),(0,1,5,4),(1,2,6,5),(2,3,7,6),(3,0,4,7)]);mesh.update();ramp=bpy.data.objects.new('Trial_ExteriorRamp',mesh);s.collection.objects.link(ramp);mesh.materials.append(dark)
# UVs for new trial parts; preserve the original exterior UVs.
meshes=[o for o in s.objects if o.type=='MESH']
for o in meshes:
 if o==hull:continue
 bpy.ops.object.select_all(action='DESELECT');o.select_set(True);bpy.context.view_layer.objects.active=o
 bpy.ops.object.mode_set(mode='EDIT');bpy.ops.mesh.select_all(action='SELECT');bpy.ops.uv.smart_project();bpy.ops.object.mode_set(mode='OBJECT')
bpy.context.view_layer.update()
# Collision checks deliberately include the original hull and all solid trial furniture.
solid_names={b['name'] for b in boxes}
verts=[];faces=[];owners=[]
for o in meshes:
 if o!=hull and o.name not in solid_names:continue
 if o.name in ['Trial_Deck','Trial_Threshold']:continue
 offset=len(verts);verts.extend(o.matrix_world@v.co for v in o.data.vertices)
 for p in o.data.polygons:
  if o==hull and p.material_index==1:continue
  faces.append([offset+i for i in p.vertices]);owners.append(o.name)
tree=BVHTree.FromPolygons(verts,faces)
hits=[]
for lane in [-.55,0,.55]:
 for step in range(95):
  y=-6.8+step*.1
  if y>2.5:break
  base=max(0,min(1,(y+7)/3))
  for k in range(13):
   c=Vector((lane,y,base+.34+k*(1.8-.68)/12));near=tree.find_nearest(c)
   if near[0] is not None and near[3]<.34-.002:hits.append({'lane':lane,'y':round(y,2),'object':owners[near[2]]});break
# Cargo: conservative circumscribed sphere for the runtime .8 x .65 x .65 box.
cargo_hits=[];cargo_radius=math.sqrt(.4**2+.325**2+.325**2)
for step in range(85):
 y=-6.5+step*.1;base=max(0,min(1,(y+7)/3));c=Vector((0,y,base+1.03));near=tree.find_nearest(c)
 if near[0] is not None and near[3]<cargo_radius:cargo_hits.append({'y':round(y,2),'object':owners[near[2]]})
eyes=[]
for x in [-.45,0,.45]:
 origin=Vector((x,2.5,floor+1.57));hit=tree.ray_cast(origin,Vector((0,1,0)),10)
 eyes.append({'x':x,'opaque_obstacle':owners[hit[2]] if hit[0] is not None else None})
report={'status':'structural candidate; sampled geometry checks only','floor_height_m':floor,'ceiling_underside_m':3.12,'headroom_m':2.12,'eye_height_above_floor_m':1.57,'employee_height_m':1.8,'employee_radius_m':.34,'cargo_size_m':[.8,.65,.65],'ramp_horizontal_run_m':3,'hull_fingerprint_before':original,'hull_fingerprint_after':fingerprint(hull),'employee_sample_collisions':hits,'cargo_conservative_sphere_collisions':cargo_hits,'forward_eye_rays':eyes,'unity_playtest':False}
bpy.ops.object.select_all(action='DESELECT')
for o in meshes:o.select_set(True)
bpy.ops.export_scene.fbx(filepath=str(O/'Flatbed_InteriorTrial.fbx'),use_selection=True,add_leaf_bones=False,bake_anim=False,axis_forward='-Z',axis_up='Y',path_mode='COPY',embed_textures=True)
bpy.ops.export_scene.gltf(filepath=str(O/'Flatbed_InteriorTrial.glb'),use_selection=True,export_animations=False)
for o in meshes:o.data.calc_loop_triangles()
report['triangles']=sum(len(o.data.loop_triangles) for o in meshes)
report['missing_uvs']=[o.name for o in meshes if not o.data.uv_layers]
(O/'validation.json').write_text(json.dumps(report,indent=2));(O/'collider-layout.json').write_text(json.dumps(boxes,indent=2))
bpy.ops.object.camera_add();cam=bpy.context.object;s.camera=cam;s.render.engine='CYCLES';s.cycles.samples=20;s.render.resolution_x=1200;s.render.resolution_y=800;s.render.resolution_percentage=100;s.world.color=(.16,.16,.16)
for pos in [(0,-9,9),(6,6,10)]:
 bpy.ops.object.light_add(type='AREA',location=pos);l=bpy.context.object;l.data.energy=1800;l.data.size=7;l.rotation_euler=(Vector((0,0,1.5))-l.location).to_track_quat('-Z','Y').to_euler()
for name,pos,target,ortho in [('forward',(0,-3.3,2.57),(0,3.4,2.15),0),('rear',(0,2.5,2.57),(0,-4.5,1.9),0),('window',(0,2.4,2.57),(0,6,2.57),0),('exterior-rear',(11,-16,10),(0,-1,1.7),19)]:
 cam.location=pos;cam.rotation_euler=(Vector(target)-cam.location).to_track_quat('-Z','Y').to_euler();cam.data.type='ORTHO' if ortho else 'PERSP';cam.data.ortho_scale=ortho or 15;cam.data.lens=20;s.render.filepath=str(O/(name+'.png'));bpy.ops.render.render(write_still=True)
bpy.ops.file.pack_all();bpy.ops.wm.save_as_mainfile(filepath=str(O/'Flatbed_InteriorTrial.blend'))
print('TRIAL_REPORT',json.dumps(report))

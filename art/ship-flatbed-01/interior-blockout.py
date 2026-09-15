"""Build an isolated, flat-deck cabin candidate inside the preserved angular hull."""
import bpy, bmesh, json, math, hashlib
from pathlib import Path
from mathutils import Vector
from mathutils.bvhtree import BVHTree
R=Path(__file__).resolve().parent
O=R/'interior-blockout-05';O.mkdir(exist_ok=True)
# Interior first: no generated exterior is loaded or used as a dimensional constraint.
bpy.ops.wm.read_factory_settings(use_empty=True)
s=bpy.context.scene;s.world=bpy.data.worlds.new('Trial_World');hull=None;original=None

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
facade=[]
def wind_panel(name,x0,x1,z0,z1,material,solid=True):
 pts=[(x,5.12-(z-2.3)*1.34/1.04,z) for x,z in [(x0,z0),(x1,z0),(x1,z1),(x0,z1)]]
 mesh=bpy.data.meshes.new(name);mesh.from_pydata(pts,[],[(0,1,2,3)]);mesh.update();o=bpy.data.objects.new(name,mesh);s.collection.objects.link(o);mesh.materials.append(material)
 if solid:facade.append(name)
 return o
glass=mat('Trial_Glazing',(.22,.32,.32));glass.node_tree.nodes.get('Principled BSDF').inputs['Transmission Weight'].default_value=.9
glass.node_tree.nodes.get('Principled BSDF').inputs['Roughness'].default_value=.05
wind_panel('Trial_WindowSill',-3.3,3.3,2.30,2.42,ivory)
wind_panel('Trial_WindowHeader',-3.3,3.3,3.17,3.34,ivory)
wind_panel('Trial_WindowCheekL',-3.3,-2.02,2.42,3.17,ivory)
wind_panel('Trial_WindowCheekR',2.02,3.3,2.42,3.17,ivory)
for j,(a,b) in enumerate([(-2.02,-.78),(-.72,.72),(.78,2.02)]):wind_panel('Trial_WindowGlass_'+str(j),a,b,2.42,3.17,glass,False)
for x in [-.75,.75]:wind_panel('Trial_WindowMullion_'+str(x),x-.03,x+.03,2.42,3.17,dark)
box('Trial_Deck',(0,0,.9),(4.352,7.6,.2),dark)
box('Trial_Threshold',(0,-3.9,.9),(3,.2,.2),dark)
box('Trial_Ceiling',(0,0,3.18),(4.35,7.6,.12),ivory)
for side in [-1,1]:
 box('Trial_Wall_'+str(side),(side*2.19,0,2.06),(.12,7.6,2.12),ivory)
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
box('Trial_ConsoleBase',(0,3.24,1.37),(2.12,.55,.74),dark)
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
# Lift the upper shell and cabin together, preserving deck, furniture and eye height.
# Blend above the rear doorway so lower openings and the display stay fixed.
def raised_z(z):
 return z+1.0*max(0,min(1,(z-2.86)/.26))
for o in meshes:
 inv=o.matrix_world.inverted()
 for v in o.data.vertices:
  world=o.matrix_world@v.co;world.z=raised_z(world.z);v.co=inv@world
 o.data.update()
for o in s.objects:
 if o.type=='LIGHT':o.location.z=raised_z(o.location.z)
for b in boxes:
 low=b['position'][2]-b['size'][2]/2;high=b['position'][2]+b['size'][2]/2
 b['position'][2]=(raised_z(low)+raised_z(high))/2;b['size'][2]=raised_z(high)-raised_z(low)
# Replace truck-like rows with asymmetric orbital postal work zones.
removed={o.name for o in s.objects if any(o.name.startswith(p) for p in ['Trial_Seat','Trial_Rack','Trial_Dock'])}
for name in removed:bpy.data.objects.remove(bpy.data.objects[name],do_unlink=True)
boxes[:]=[b for b in boxes if b['name'] not in removed]
burgundy=mat('Trial_Burgundy',(.19,.045,.055))
# Left: sealed return lockers, unequal heights and recessed latch strips.
for j,y in enumerate([-1.1,.05,1.2]):
 box('Trial_SealedLocker_'+str(j),(-1.87,y,2.06),(.48,.96,2.12),burgundy)
 box('Trial_LockerDoor_'+str(j),(-1.614,y,2.06),(.028,.79,1.84),ivory)
 box('Trial_LockerLatch_'+str(j),(-1.585,y,2.02),(.04,.28,.08),dark)
 box('Trial_LockerStatus_'+str(j),(-1.56,y,2.66),(.015,.22,.04),cyan,False)
# Right: a continuous dispatch desk, parcel cubbies and overhead paperwork machine.
box('Trial_DispatchDesk',(1.87,.35,1.63),(.48,3.6,.16),ivory)
for y in [-1.25,1.95]:box('Trial_DeskLeg_'+str(y),(1.9,y,1.28),(.34,.18,.56),dark)
box('Trial_ReceiptPrinter',(1.84,.95,1.98),(.38,.76,.54),burgundy)
box('Trial_ReceiptSlot',(1.637,.95,2.03),(.018,.55,.055),dark,False)
for j,y in enumerate([-.85,-.27]):box('Trial_SupplyCubby_'+str(j),(1.9,y,2.55),(.36,.46,.6),dark)
box('Trial_DeskDisplay',(1.63,.08,2.12),(.025,.64,.43),screen,False)
# Central scan gate leaves a broad ground-level route; no raised cargo pedestal.
for side in [-1,1]:
 box('Trial_ScannerUpright_'+str(side),(side*1.59,-.65,2.48),(.14,.3,2.96),burgundy)
 box('Trial_ScannerStrip_'+str(side),(side*1.505,-.65,2.8),(.018,.18,1.28),cyan,False)
box('Trial_ScannerOverhead',(0,-.65,3.9),(3.32,.46,.3),ivory)
box('Trial_ScannerLens',(0,-.65,3.73),(.72,.28,.035),cyan,False)
for x in [-.6,.6]:box('Trial_FloorMark_'+str(x),(x,-.65,1.004),(.035,1.25,.008),orange,False)
for y in [-1.26,-.04]:box('Trial_FloorMark_'+str(y),(0,y,1.004),(1.23,.035,.008),orange,False)
# Four folded launch seats live beside the rear airlock rather than lining the saloon.
for side in [-1,1]:
 for j,y in enumerate([-3.25,-2.55]):
  box('Trial_FoldedSeat_'+str(side)+'_'+str(j),(side*2.04,y,1.95),(.14,.52,1.25),orange)
  box('Trial_SeatHarness_'+str(side)+'_'+str(j),(side*1.95,y,2.06),(.025,.07,.8),dark,False)
# Overhead utility trunk gives the workstations a connected mechanical purpose.
box('Trial_UtilityTrunk',(1.9,.2,3.86),(.22,5.6,.25),burgundy)
# Label planes are illustrative; no unimplemented controls are presented as functional.
def label(n,text,loc,rot,size=.13):
 bpy.ops.object.text_add(location=loc,rotation=rot);o=bpy.context.object;o.name='Trial_Label_'+n;o.data.body=text;o.data.size=size;o.data.materials.append(cyan);bpy.ops.object.convert(target='MESH')
label('Scan','PARCEL SCREENING',( -.95,-.891,3.85),(math.pi/2,0,0),.14)
label('Return','SEALED RETURNS',(-1.59,-1.42,3.19),(math.pi/2,0,math.pi/2),.12)
label('Dispatch','DISPATCH',(1.615,-.62,2.82),(math.pi/2,0,-math.pi/2),.12)
# The envelope below is a temporary cover derived around the interior, not final ship art.
# The previous generated hull did not contain the rectangular cabin.
# Build a fitted structural envelope; keep original hull source files untouched.
if hull:bpy.data.objects.remove(hull,do_unlink=True)
hull=None
for name in ['Trial_Lintel','Trial_Jamb_-1','Trial_Jamb_1','Trial_OpenDoor_-1','Trial_OpenDoor_1']:
 o=bpy.data.objects.get(name)
 if o:bpy.data.objects.remove(o,do_unlink=True)
boxes[:]=[b for b in boxes if b['name'] not in ['Trial_Lintel','Trial_Jamb_-1','Trial_Jamb_1','Trial_OpenDoor_-1','Trial_OpenDoor_1']]
for o in s.objects:
 if o.type=='MESH' and o.name.startswith('Trial_Window'):
  for v in o.data.vertices:v.co.x=max(-2.55,min(2.55,v.co.x))
for side in [-1,1]:
 box('Shell_Side_'+str(side),(side*2.4,-.16,2.44),(.3,8.08,3.88),ivory)
 box('Trial_Jamb_'+str(side),(side*2.075,-3.98,2.56),(.95,.24,3.12),ivory)
 box('Trial_OpenDoor_'+str(side),(side*3.4,-4.12,2.56),(1.6,.12,3.12),dark)
 # Forward side cheek closes the sloping windshield to the side shell.
 pts=[(side*2.55,y,z) for y,z in [(3.78,.5),(5.12,.5),(5.12,2.3),(3.78,4.34)]]
 me=bpy.data.meshes.new('Shell_NoseCheek');me.from_pydata(pts,[],[(0,1,2,3)]);me.update()
 o=bpy.data.objects.new('Shell_NoseCheek_'+str(side),me);s.collection.objects.link(o);me.materials.append(ivory);facade.append(o.name)
box('Shell_Roof',(0,-.16,4.36),(5.1,8.08,.24),ivory)
box('Shell_Underfloor',(0,.46,.64),(5.1,9.32,.28),dark)
box('Shell_NoseLower',(0,5.12,1.4),(5.1,.16,1.8),ivory)
box('Trial_Lintel',(0,-3.98,4.24),(5.1,.24,.24),ivory)
# Bridge the old 0.2 m gap between cabin and rear doorway.
box('Shell_RearRoofBridge',(0,-3.98,4.18),(5.1,.44,.12),ivory)
box('Trial_EntryDeck',(0,-3.89,.9),(3.2,.22,.2),dark)
meshes=[o for o in s.objects if o.type=='MESH']
for o in meshes:
 if o.data.uv_layers:continue
 bpy.ops.object.select_all(action='DESELECT');o.select_set(True);bpy.context.view_layer.objects.active=o
 bpy.ops.object.mode_set(mode='EDIT');bpy.ops.mesh.select_all(action='SELECT');bpy.ops.uv.smart_project();bpy.ops.object.mode_set(mode='OBJECT')
bpy.context.view_layer.update()
# Verify cabin fixture vertices against the fitted envelope (not just outer bounds).
protrusions=[]
for o in meshes:
 if not o.name.startswith('Trial_') or any(k in o.name for k in ['OpenDoor','ExteriorRamp','Window','Jamb','Lintel']):continue
 for v in o.data.vertices:
  w=o.matrix_world@v.co
  roof=4.48 if w.y<=3.78 else 4.34-(w.y-3.78)*2.04/1.34
  if abs(w.x)>2.55+.001 or w.y < -4.2-.001 or w.y>5.2+.001 or w.z<.5-.001 or w.z>roof+.001:protrusions.append(o.name);break
assert not protrusions,protrusions
# Collision checks deliberately include the original hull and all solid trial furniture.
solid_names={b['name'] for b in boxes}|set(facade)
verts=[];faces=[];owners=[]
for o in meshes:
 if o!=hull and o.name not in solid_names:continue
 if o.name in ['Trial_Deck','Trial_Threshold','Trial_EntryDeck','Shell_Underfloor']:continue
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
report={'status':'structural candidate; sampled geometry checks only','floor_height_m':floor,'ceiling_underside_m':4.12,'headroom_m':3.12,'eye_height_above_floor_m':1.57,'employee_height_m':1.8,'employee_radius_m':.34,'cargo_size_m':[.8,.65,.65],'ramp_horizontal_run_m':3,'hull_fingerprint_before':original,'hull_fingerprint_after':None,'employee_sample_collisions':hits,'cargo_conservative_sphere_collisions':cargo_hits,'forward_eye_rays':eyes,'unity_playtest':False}
report['localized_windshield_replacement']=True
report['upper_shell_lift_m']=1.0
report['fitted_structural_envelope']=True
report['interior_first']=True
report['layout']='orbital postal screening / sealed returns / dispatch / folded launch seats'
report['scanner_clear_width_m']=3.04
report['scanner_clear_height_m']=2.75
report['interior_protrusions']=protrusions
report['entry_clear_width_m']=3.2
report['entry_clear_height_m']=3.12
report['central_window_and_display_axis_x_m']=0
report['console_base_top_m']=1.74
report['display_bottom_m']=1.885
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
for name,pos,target,ortho in [('forward',(0,-3.3,2.57),(0,3.4,2.15),0),('rear',(0,2.5,2.57),(0,-4.5,1.9),0),('window',(0,2.4,2.57),(0,6,2.57),0),('exterior-rear',(11,-16,10),(0,-1,1.7),19),('exterior-front',(-11,16,10),(0,0,2),19)]:
 cam.location=pos;cam.rotation_euler=(Vector(target)-cam.location).to_track_quat('-Z','Y').to_euler();cam.data.type='ORTHO' if ortho else 'PERSP';cam.data.ortho_scale=ortho or 15;cam.data.lens=20;s.render.filepath=str(O/(name+'.png'));bpy.ops.render.render(write_still=True)
bpy.context.preferences.filepaths.save_version=0
bpy.ops.file.pack_all();bpy.ops.wm.save_as_mainfile(filepath=str(O/'Flatbed_InteriorTrial.blend'))
print('TRIAL_REPORT',json.dumps(report))

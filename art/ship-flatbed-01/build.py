"""Common-coordinate FLATBED cabin, mechanisms and review renders. Blender 5.2."""
import bpy, math, json
from pathlib import Path
from mathutils import Vector
R=Path(__file__).resolve().parent
O=R/'review';O.mkdir(exist_ok=True)
bpy.ops.object.select_all(action='SELECT');bpy.ops.object.delete(use_global=False)
def mat(name,color,emit=0):
 m=bpy.data.materials.new(name);m.diffuse_color=(*color,1);m.use_nodes=True
 p=m.node_tree.nodes.get('Principled BSDF');p.inputs['Base Color'].default_value=(*color,1);p.inputs['Roughness'].default_value=.8
 if emit:p.inputs['Emission Color'].default_value=(*color,1);p.inputs['Emission Strength'].default_value=emit
 return m
ivory=mat('Paint_Ivory',(.48,.43,.32));orange=mat('Safety_Orange',(.48,.12,.035));dark=mat('Graphite',(.055,.07,.07));floor=mat('Floor',(.12,.14,.13));cyan=mat('Display_Cyan',(.03,.55,.49),1);amber=mat('Lamp_Amber',(.9,.39,.055),2)
glass=mat('Glazing',(.08,.19,.2));p=glass.node_tree.nodes.get('Principled BSDF');p.inputs['Transmission Weight'].default_value=.85;p.inputs['Roughness'].default_value=.12
def box(n,loc,size,m,bev=.025):
 bpy.ops.mesh.primitive_cube_add(size=1,location=loc);o=bpy.context.object;o.name=n;o.dimensions=size;bpy.ops.object.transform_apply(location=False,rotation=False,scale=True);o.data.materials.append(m)
 if bev:
  mod=o.modifiers.new('Machined edges','BEVEL');mod.width=bev;mod.segments=1;bpy.context.view_layer.objects.active=o;bpy.ops.object.modifier_apply(modifier=mod.name)
 return o
def mesh(n,verts,faces,m):
 d=bpy.data.meshes.new(n);d.from_pydata(verts,[],faces);d.update();o=bpy.data.objects.new(n,d);bpy.context.collection.objects.link(o);d.materials.append(m);return o
def panel(n,points,m,th=.06):
 o=mesh(n,points,[tuple(range(len(points)))],m);mod=o.modifiers.new('Thickness','SOLIDIFY');mod.thickness=th;bpy.context.view_layer.objects.active=o;o.select_set(True);bpy.ops.object.modifier_apply(modifier=mod.name);o.select_set(False);return o
# X right, Y forward. Clear deck y=-3.8..3.8. Floor z=.6.
deck=[(-3.2,-3.8,.6),(3.2,-3.8,.6),(3.2,1.8,.6),(2.4,3.8,.6),(-2.4,3.8,.6),(-3.2,1.8,.6)]
panel('Deck',deck,floor,.2)
roof=panel('Roof',[(x,y,3.2) for x,y,z in deck],ivory,.2)
for s in [-1,1]:
 box('CabinWall', (s*3.4,-1,1.85),(.4,5.6,2.5),ivory)
 panel('TaperWall',[(s*3.2,1.8,.6),(s*2.4,3.8,.6),(s*2.4,3.8,3.1),(s*3.2,1.8,3.1)],ivory,.22)
 # Low exterior shoulder skirts stay outside the clear cabin.
 panel('Shoulder',[(s*3.6,-4,.7),(s*4.45,-4,1.2),(s*4.45,2.7,1.2),(s*3.6,3.5,2.8)],ivory,.12)
 box('EnginePod',(s*4.5,-.9,1.65),(1.8,9.4,1.65),dark,.16)
 for y in [-4.5,-2.6,-.7,1.2,3.1]:
  box('EngineArmor',(s*4.5,y,2.48),(1.72,1.65,.16),ivory,.07)
  box('OrangeBand',(s*4.5,y,2.58),(.42,1.62,.055),orange,.01)
 for y in [-5.62,3.82]:
  box('EngineGrille',(s*4.5,y,1.7),(1.45,.1,1.1),dark)
  for z in [1.3,1.5,1.7,1.9,2.1]:box('VentSlat',(s*4.5,y-.065,z),(1.28,.07,.065),floor,.01)
 for y in [-3.2,2.6]:
  box('LandingFoot',(s*3.75,y,.09),(1,.8,.18),dark,.07);box('LandingStrut',(s*3.75,y,.49),(.3,.35,.65),floor)
  box('LandingCuff',(s*3.75,y,.73),(.6,.55,.3),orange)
# front lower nose plus three genuine glazing panels on the same forward plane
box('NoseLower',(0,4.35,1.08),(4.8,1.1,.96),ivory,.09)
for i in range(3):
 x0=-2.4+i*1.6+.065;x1=x0+1.47
 panel('CockpitGlass_'+str(i),[(x0,4.9,1.56),(x1,4.9,1.56),(x1,3.85,3.1),(x0,3.85,3.1)],glass,.025)
for x in [-2.4,-.8,.8,2.4]:
 o=box('WindowMullion',(x,4.375,2.33),(.085,1.865,.085),ivory);o.rotation_euler.x=math.atan2(1.54,-1.05)
box('WindowHeader',(0,3.85,3.12),(4.95,.14,.15),ivory)
box('WindowSill',(0,4.9,1.56),(4.95,.15,.14),dark)
for s in [-1,1]:
 panel('NoseCheek',[(s*2.4,3.8,.6),(s*2.4,4.9,.6),(s*2.4,4.9,1.56),(s*2.4,3.85,3.1),(s*2.4,3.8,3.1)],ivory,.1)
# Rear jambs, sliding split door parks laterally. Ramp retracts UNDER deck, never becomes door.
for s in [-1,1]:box('RearJamb',(s*2.4,-3.92,1.8),(1.8,.24,2.4),ivory)
box('RearLintel',(0,-3.92,3.06),(6.4,.24,.12),ivory)
doors=[]
for s in [-1,1]:
 o=box('Door_L' if s<0 else 'Door_R',(s*2.29,-4.08,1.8),(1.48,.13,2.38),dark);doors.append(o)
 o.location.x=s*.75;o.keyframe_insert(data_path='location',frame=1);o.location.x=s*2.29;o.keyframe_insert(data_path='location',frame=40)
ramp=box('Ramp_Deploy',(0,-5.35,.3),(3,3,.09),floor);ramp.rotation_euler.x=math.asin(.6/3)
ramp.location=(0,-2.25,.35);ramp.rotation_euler.x=0;ramp.keyframe_insert(data_path='location',frame=1);ramp.keyframe_insert(data_path='rotation_euler',frame=1)
ramp.location=(0,-5.35,.3);ramp.rotation_euler.x=math.asin(.6/3);ramp.keyframe_insert(data_path='location',frame=40);ramp.keyframe_insert(data_path='rotation_euler',frame=40)
for x in [-1.35,1.35]:
 o=box('RampEdge',(x,0,.06),(.09,2.85,.04),orange,.005);o.parent=ramp
# Racks and four separate fold seats: longitudinal pitch 1m, outside central aisle.
for s in [-1,1]:
 for y in [-2.2,-.9]:
  box('CargoRack',(s*2.8,y,1.2),(.72,1.15,1.2),dark)
  for z in [.68,1.3,1.88]:box('Shelf',(s*2.8,y,z),(.8,1.2,.065),ivory)
 for y in [.45,1.45]:
  box('SeatBack',(s*2.83,y,1.48),(.16,.74,.72),orange)
  box('SeatCushion',(s*2.43,y,.99),(.65,.74,.15),dark)
 box('EquipmentLocker',(s*2.8,-3.18,1.46),(.65,.6,1.7),ivory)
 box('LockerStripe',(s*2.43,-3.18,1.5),(.04,.45,.14),orange)
 box('Console',(s*1.25,3.25,1.04),(1.95,.7,.86),dark,.08)
 box('ConsoleScreen',(s*1.25,3.18,1.483),(.95,.42,.02),cyan,.01)
 for y in [-3.1,1.9]:box('GuideLight',(s*1.52,y,.66),(.08,.38,.035),amber,.005)
 for y in [-2.7,0,2]:box('RoofRib',(s*2,y,3.03),(2.25,.11,.13),dark)
for y in [-2.9,2.3]:box('CeilingLight',(0,y,3.06),(1.1,.19,.055),amber)
# Four reference capsules in a separate hidden collection; not export assets.
refs=[]
for x,y in [(-.6,-2),(.6,-2),(-.6,.6),(.6,.6)]:
 bpy.ops.mesh.primitive_uv_sphere_add(segments=12,ring_count=8,location=(x,y,1.5));o=bpy.context.object;o.name='REF_Player';o.scale=(.34,.34,.9);refs.append(o);o.hide_render=True
for o in refs:o.hide_set(True)
scene=bpy.context.scene;scene.frame_set(40);scene.unit_settings.system='METRIC'
assets=[o for o in scene.objects if o.type=='MESH' and o not in refs]
for o in assets:
 bpy.ops.object.select_all(action='DESELECT');o.select_set(True);bpy.context.view_layer.objects.active=o
 bpy.ops.object.mode_set(mode='EDIT');bpy.ops.mesh.select_all(action='SELECT');bpy.ops.mesh.normals_make_consistent(inside=False);bpy.ops.uv.smart_project(island_margin=.02);bpy.ops.object.mode_set(mode='OBJECT')
bpy.ops.object.select_all(action='DESELECT')
for o in assets:o.select_set(True)
bpy.ops.export_scene.gltf(filepath=str(R/'Flatbed_Structural.glb'),use_selection=True,export_animations=True)
bpy.ops.export_scene.fbx(filepath=str(R/'Flatbed_Structural.fbx'),use_selection=True,add_leaf_bones=False,bake_anim=False,axis_forward='-Z',axis_up='Y')
report={'status':'structural model; Tripo shell integration pending','clear_floor_width_m':6.4,'clear_floor_length_m':7.6,'seat_count':4,'door_width_m':3,'door_height_m':2.4,'ramp_length_m':3,'floor_height_m':.6,'ramp_angle_deg':math.degrees(math.asin(.2)),'meshes':len(assets),'triangles':sum(len(o.data.polygons)*2 for o in assets),'unity_playtest':False}
(R/'validation.json').write_text(json.dumps(report,indent=2))
for y in [-2,1.5]:
 bpy.ops.object.light_add(type='AREA',location=(0,y,2.96));bpy.context.object.data.energy=85;bpy.context.object.data.size=2
scene.render.engine='CYCLES';scene.cycles.samples=24;scene.render.resolution_x=1200;scene.render.resolution_y=850;scene.render.resolution_percentage=100;scene.world.color=(.2,.2,.2)
for pos,power,size in [((3,-7,12),2400,8),((-8,0,7),1800,7),((0,8,10),2200,6)]:
 bpy.ops.object.light_add(type='AREA',location=pos);l=bpy.context.object;l.data.energy=power;l.data.shape='DISK';l.data.size=size;l.rotation_euler=(Vector((0,0,1))-l.location).to_track_quat('-Z','Y').to_euler()
bpy.ops.object.camera_add();cam=bpy.context.object;scene.camera=cam
for name,pos,target,ortho in [('exterior',(14,17,11),(0,0,1),19),('rear',(12,-17,10),(0,-1,1),20),('cutaway',(10,-12,16),(0,0,1),17),('interior',(0,-3.3,2.17),(0,3.4,1.8),0)]:
 roof.hide_render=name=='cutaway';cam.location=pos;cam.rotation_euler=(Vector(target)-cam.location).to_track_quat('-Z','Y').to_euler();cam.data.type='ORTHO' if ortho else 'PERSP';cam.data.ortho_scale=ortho or 16;cam.data.lens=20
 scene.render.filepath=str(O/(name+'.png'));bpy.ops.render.render(write_still=True)
roof.hide_render=False
bpy.ops.wm.save_as_mainfile(filepath=str(R/'Flatbed_Structural.blend'))

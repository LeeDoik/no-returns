"""Authored metre-scale postal kit. Run in Blender; emits editable source and GLBs."""
import bpy, math, json
from pathlib import Path
from mathutils import Vector

ROOT=Path(__file__).resolve().parents[3]
OUT=ROOT/'assets/art/release-01'
SOURCE=Path(__file__).resolve().parent
OUT.mkdir(parents=True,exist_ok=True)
bpy.ops.object.select_all(action='SELECT');bpy.ops.object.delete(use_global=False)
def V(p): return Vector((p[0],-p[2],p[1]))
def mat(name,color,rough=.7,metal=0):
    m=bpy.data.materials.new(name);m.diffuse_color=(*color,1);m.use_nodes=True
    p=m.node_tree.nodes.get('Principled BSDF');p.inputs['Base Color'].default_value=(*color,1);p.inputs['Roughness'].default_value=rough;p.inputs['Metallic'].default_value=metal
    return m
sage=mat('Sage enamel',(0.25,.37,.32),.38,.25)
dark=mat('Charcoal rubber',(.055,.075,.068),.85)
steel=mat('Brushed dark steel',(.26,.31,.29),.32,.7)
brass=mat('Worn brass',(.55,.39,.16),.35,.72)
wood=mat('Warm timber',(.48,.31,.14),.72)
card=mat('Kraft cardboard',(.65,.43,.22),.88)
cream=mat('Ivory paper',(.82,.77,.61),.84)
tape=mat('Parcel tape',(.76,.61,.34),.43)
pink=mat('Sneeze coral',(.68,.28,.30),.62)
green=mat('Adhesive lime',(.42,.58,.12),.46)
amber=mat('Hopper ochre',(.78,.49,.10),.5)
white=mat('Eye ivory',(.9,.87,.73),.3)
ink=mat('Printed ink',(.035,.053,.045),.72)
mesh_assets=[];active_root=None

def group(name,at=(0,0,0),parent=None):
    o=bpy.data.objects.new(name,None);bpy.context.collection.objects.link(o)
    o.parent=parent or active_root;o.location=V(at);return o
def finish(o,name,at,material,parent=None):
    o.name=name;o.location=V(at);o.parent=parent or active_root
    if material:o.data.materials.append(material)
    return o
def cube(name,at,size,material,bevel=.02,parent=None):
    bpy.ops.mesh.primitive_cube_add();o=bpy.context.object;o.dimensions=(size[0],size[2],size[1]);bpy.ops.object.transform_apply(location=False,rotation=False,scale=True)
    finish(o,name,at,material,parent)
    if bevel:
        b=o.modifiers.new('Rounded manufactured edge','BEVEL');b.width=min(bevel,min(size)*.25);b.segments=3
        n=o.modifiers.new('Weighted corner normals','WEIGHTED_NORMAL')
    return o
def sphere(name,at,size,material,parent=None):
    bpy.ops.mesh.primitive_uv_sphere_add(segments=20,ring_count=12);o=bpy.context.object;o.scale=(size[0]/2,size[2]/2,size[1]/2);bpy.ops.object.transform_apply(location=False,rotation=False,scale=True)
    for p in o.data.polygons:p.use_smooth=True
    return finish(o,name,at,material,parent)
def cylinder(name,at,radius,length,material,axis=(0,1,0),parent=None):
    bpy.ops.mesh.primitive_cylinder_add(vertices=24,radius=radius,depth=length);o=bpy.context.object
    finish(o,name,at,material,parent);o.rotation_mode='QUATERNION';o.rotation_quaternion=V(axis).to_track_quat('Z','Y')
    b=o.modifiers.new('Rolled rim','BEVEL');b.width=min(.008,radius*.12);b.segments=2
    o.modifiers.new('Weighted normals','WEIGHTED_NORMAL');return o
def tube(name,points,radius,material,parent=None):
    curve=bpy.data.curves.new(name,'CURVE');curve.dimensions='3D';curve.resolution_u=2 if 'spring' in name.lower() else 8;curve.bevel_depth=radius;curve.bevel_resolution=1 if 'spring' in name.lower() else 2
    sp=curve.splines.new('BEZIER');sp.bezier_points.add(len(points)-1)
    for b,p in zip(sp.bezier_points,points):b.co=V(p);b.handle_left_type='AUTO';b.handle_right_type='AUTO'
    o=bpy.data.objects.new(name,curve);bpy.context.collection.objects.link(o);o.parent=parent or active_root;curve.materials.append(material)
    bpy.ops.object.select_all(action='DESELECT');bpy.context.view_layer.objects.active=o;o.select_set(True);bpy.ops.object.convert(target='MESH');o.select_set(False);return o
def bolt(at,parent=None):
    cylinder('Hex fastener',at,.021,.018,brass,(0,0,-1),parent)
def barcode(at,parent=None):
    for i in range(15):cube('Barcode', (at[0]-.12+i*.017,at[1],at[2]),(.008 if i%3 else .013,.13,.0015),ink,0,parent)
def root(name):
    global active_root
    active_root=None;active_root=group(name);return active_root
def export(name):
    # Batch rigid parts by material under the same pivot; expressions/lids stay separate.
    batches={}
    for o in list(active_root.children_recursive):
        if o.type!='MESH' or (name=='packing_cushion' and o.name.startswith('Heat sealed cell')):continue
        bpy.ops.object.select_all(action='DESELECT');o.select_set(True);bpy.context.view_layer.objects.active=o
        for modifier in list(o.modifiers):bpy.ops.object.modifier_apply(modifier=modifier.name)
        key=(o.parent,o.data.materials[0] if o.data.materials else None)
        batches.setdefault(key,[]).append(o)
    for (parent,material),objects in batches.items():
        if len(objects)<2:continue
        bpy.ops.object.select_all(action='DESELECT')
        for o in objects:o.select_set(True)
        bpy.context.view_layer.objects.active=objects[0];bpy.ops.object.join();objects[0].name='Surface '+material.name
    bpy.ops.object.select_all(action='DESELECT')
    objects=[active_root]+list(active_root.children_recursive)
    for o in objects:o.select_set(True)
    bpy.context.view_layer.objects.active=active_root
    bpy.ops.export_scene.gltf(filepath=str(OUT/(name+'.glb')),export_format='GLB',use_selection=True,export_apply=True,export_animations=False,export_cameras=False,export_lights=False)
    deps=bpy.context.evaluated_depsgraph_get();tris=0
    for o in objects:
        if o.type=='MESH':
            ev=o.evaluated_get(deps);mesh=ev.to_mesh();tris+=sum(len(p.vertices)-2 for p in mesh.polygons);ev.to_mesh_clear()
    mesh_assets.append({'id':name,'triangles':tris,'objects':len(objects)})
    active_root.hide_render=True;active_root.hide_viewport=True

def cargo(kind):
    root(kind);accent={'standard':sage,'sneezer':pink,'clinger':green,'hopper':amber}[kind]
    # Four thin walls and a bottom form an actual hollow carton beneath two flaps.
    for x in [-.385,.385]:cube('Side panel',(x,0,0),(.03,.74,.77),card,.006)
    cube('Rear panel',(0,0,.385),(.77,.74,.03),card,.006)
    cube('Front panel',(0,0,-.385),(.77,.74,.03),card,.006)
    cube('Bottom',(0,-.37,0),(.77,.03,.77),card,.006)
    for side in [-1,1]:
        hinge=group('LidFrontPivot' if side<0 else 'LidBackPivot',(0,.375,side*.39))
        cube('Folded lid',(0,0,-side*.194),(.795,.018,.39),card,.006,hinge)
        cube('Lid tape',(0,.012,-side*.194),(.15,.004,.395),accent,.001,hinge)
    for z in [-.404,.404]:cube('Tape strip',(0,.23,z),(.15,.29,.004),accent,.001)
    for x in [-.403,.403]:
        patch=cube('Shipping label',(x,0,.02),(.004,.28,.32),cream,.001)
        # Embossed corner protectors and folded board lines remain readable at play distance.
        for z in [-.34,.34]:cube('Corner fold',(x,.0,z),(.006,.67,.011),tape,.001)
    if kind=='standard':
        cube('Front manifest',(0,0,-.408),(.47,.36,.007),cream,.004);barcode((0,-.07,-.413))
        for x in [-.13,.13]:tube('Up arrow',[(x,.10,-.416),(x,.20,-.416),(x-.035,.16,-.416),(x,.20,-.416),(x+.035,.16,-.416)],.008,ink)
    else:
        for phase in ['Idle','Warning','Burst']:
            face=group('Face'+phase)
            for side in [-1,1]:
                x=side*.17
                if phase=='Burst':
                    tube('Squeezed eyelid',[(x-.085,.13,-.418),(x,.18,-.45),(x+.085,.13,-.418)],.019,ink,face)
                else:
                    sphere('Eye socket',(x,.11,-.406),(.25,.26,.068),tape,face)
                    sphere('Ivory eyeball',(x,.12,-.44),(.20,.22,.09),white,face)
                    sphere('Pupil',(x-side*.025,.10 if phase=='Idle' else .17,-.488),(.074,.096,.027),ink,face)
                    sphere('Eye glint',(x-side*.025-.015,.13,-.504),(.021,.027,.009),white,face)
                tube('Raised brow',[(x-.09,.28,-.434),(x,.30+(.04 if phase=='Warning' else 0),-.454),(x+.09,.27,-.434)],.018,accent,face)
            if phase=='Burst':
                sphere('Dark open mouth',(0,-.17,-.411),(.24,.26,.06),ink,face)
                tube('Mouth lip',[(-.12,-.17,-.44),(0,-.035,-.45),(.12,-.17,-.44),(0,-.30,-.45),(-.12,-.17,-.44)],.012,card,face)
            else:
                tube('Mouth',[(-.11,-.15,-.417),(0,-.19 if phase=='Idle' else -.11,-.433),(.11,-.15,-.417)],.014,ink,face)
            if phase=='Warning':
                for side in [-1,1]:sphere('Pressure cheek',(side*.28,-.13,-.42),(.15,.14,.10),accent,face)
            for side in [-1,1]:cube('Front tooth',(side*.035,-.18,-.447),(.06,.073,.023),white,.006,face)
    if kind=='clinger':
        for side in [-1,1]:
            for z in [-.18,.18]:
                cylinder('Adhesive cup',(side*.414,-.11,z),.095,.047,green,(side,0,0))
                cylinder('Cup recess',(side*.443,-.11,z),.057,.009,dark,(side,0,0))
    if kind=='hopper':
        for x in [-.22,.22]:
            cube('Hop shoe',(x,-.40,0),(.30,.09,.48),dark,.035)
            cube('Spring mount',(x,-.345,0),(.18,.045,.23),brass,.012)
    export(kind)

def desk(name,counter=False):
    root(name);width=2.4
    cube('Carcass',(0,.57,0),(width,1.10,.94),sage,.055)
    cube('Timber worktop',(0,1.15,0),(width+.10,.09,1.04),wood,.035)
    cube('Worktop front inlay',(0,1.198,-.43),(width-.08,.006,.05),brass,.001)
    cube('Kick recess',(0,.065,-.47),(width-.14,.13,.06),dark,.01)
    for x in [-.61,.61]:
        cube('Drawer face',(x,.87,-.495),(1.10,.34,.035),sage,.018)
        cube('Drawer keyline',(x,.678,-.499),(1.12,.012,.012),dark,.001)
        tube('Pull handle',[(x-.14,.88,-.52),(x-.14,.88,-.59),(x+.14,.88,-.59),(x+.14,.88,-.52)],.017,steel)
        for dx in [-.41,.41]:bolt((x+dx,.89,-.52))
        cube('Lower cupboard',(x,.36,-.49),(1.1,.56,.022),sage,.016)
    for x in [-1.13,1.13]:
        for z in [-.37,.37]:cylinder('Rubber foot',(x,.02,z),.065,.045,dark)
    export(name)

def belt():
    root('conveyor_module')
    for x in [-.98,.98]:
        cube('Pressed rail',(x,.22,0),(.12,.18,1.0),sage,.025)
        cube('Rail highlight',(x,.32,0),(.11,.025,1),steel,.005)
        for z in [-.35,.35]:bolt((x,.22,z))
    for i in range(6):
        z=-.416+i*.166
        cylinder('Transport roller',(0,.12,z),.083,1.83,dark,(1,0,0))
        for x in [-.92,.92]:cylinder('Roller bearing',(x,.12,z),.105,.045,brass,(1,0,0))
    export('conveyor_module')

def shelf():
    root('shelf')
    for x in [-1.1,1.1]:
        for z in [-.36,.36]:
            cube('Folded upright',(x,1.35,z),(.06,2.7,.065),sage,.009)
            cube('Foot plate',(x,.02,z),(.18,.04,.18),steel,.012)
    for y in [.12,.86,1.60,2.34]:
        cube('Shelf tray',(0,y,0),(2.26,.055,.81),sage,.016)
        cube('Rolled front edge',(0,y-.04,-.4),(2.26,.085,.035),steel,.01)
        for x in [-.75,0,.75]:cube('Label holder',(x,y-.04,-.424),(.26,.066,.008),cream,.003)
    for x in [-1.1,1.1]:tube('Back diagonal',[(x,.14,.39),(-x,2.35,.39)],.018,steel)
    export('shelf')

def dispatch():
    root('dispatch')
    for x in [-1.44,1.44]:
        cube('Cast side column',(x,1.1,0),(.32,2.2,.66),sage,.08)
        for y in [.2,1,1.9]:bolt((x,y,-.35))
    cube('Arch header',(0,2.18,0),(3.20,.38,.70),sage,.085)
    cube('Inner header',(0,1.95,-.09),(2.66,.095,.20),brass,.018)
    cube('Deep mail slot',(0,.99,.27),(2.55,1.90,.12),dark,.03)
    for i in range(8):cube('Rubber curtain',(-1.11+i*.32,1.13,.18),(.30,1.62,.035),dark,.02)
    cube('Receiving lip',(0,.23,-.36),(2.65,.18,.82),steel,.055)
    for x in [-1.34,1.34]:
        cylinder('Status lamp bezel',(x,1.71,-.37),.12,.065,brass,(0,0,-1))
        sphere('Status glass',(x,1.71,-.42),(.18,.18,.07),green)
    cube('Address sign',(0,2.20,-.38),(1.40,.27,.035),cream,.012)
    export('dispatch')

def spring():
    root('return_spring')
    cube('Base casting',(0,.065,0),(1.55,.13,1.55),sage,.055)
    for x in [-.56,.56]:
        for z in [-.56,.56]:
            cylinder('Guide pin',(x,.24,z),.038,.32,steel)
            points=[]
            for i in range(81):
                a=i/80*math.tau*4;points.append((x+math.cos(a)*.085,.13+i/80*.24,z+math.sin(a)*.085))
            tube('Compression spring',points,.015,steel)
    top=group('Top');cube('Launch plate',(0,.43,0),(1.48,.11,1.48),green,.06,top)
    for x in [-.50,.50]:cube('Warning stripe',(x,.49,0),(.06,.004,1.2),cream,.001,top)
    for x in [-.13,.13]:tube('Up marking',[(x,.492,.20),(x,.492,-.20),(x-.08,.492,-.1),(x,.492,-.20),(x+.08,.492,-.1)],.014,cream,top)
    export('return_spring')

def cushion():
    root('packing_cushion')
    cube('Inflator tray',(0,.04,0),(1.5,.08,1.25),sage,.035)
    for i in range(6):
        at=((i%3-1)*.45,.16,(i//3-.5)*.58)
        cube('Heat sealed cell',at,(.43,.28,.55),tape,.10)
        cube('Heat seal',(at[0],.08,at[2]),(.44,.025,.56),cream,.012)
    cylinder('Air valve',(.70,.11,.38),.05,.10,brass,(1,0,0));export('packing_cushion')

def lamp():
    root('pendant')
    cylinder('Cable',(0,.40,0),.014,.80,dark)
    bpy.ops.mesh.primitive_cone_add(vertices=32,radius1=.32,radius2=.09,depth=.20)
    o=bpy.context.object;finish(o,'Spun enamel shade',(0,-.09,0),sage)
    sphere('Frosted diffuser',(0,-.18,0),(.48,.04,.48),cream)
    cylinder('Brass cap',(0,.035,0),.09,.085,brass);export('pendant')

for name in ['standard','sneezer','clinger','hopper']:cargo(name)
desk('workbench');belt();shelf();dispatch();spring();cushion();lamp()
# Gallery layout lives in the editable source only; individual exports retain origins.
for i,a in enumerate(mesh_assets):
    r=bpy.data.objects[a['id']];r.hide_render=False;r.hide_viewport=False;r.location=V(((i%4)*3.5,0,(i//4)*4))
# The source scene opens in a useful neutral expression with its actual textures.
for o in bpy.data.objects:
    if o.name.startswith(('FaceWarning','FaceBurst')):
        for child in o.children_recursive: child.hide_render=True; child.hide_viewport=True
for material,texture in [(card,'cardboard'),(sage,'paint')]:
    nodes=material.node_tree.nodes;links=material.node_tree.links
    image=nodes.new('ShaderNodeTexImage');image.image=bpy.data.images.load(str(OUT/'textures'/f'{texture}_basecolor.png'));image.projection='BOX';image.projection_blend=.2
    coords=nodes.new('ShaderNodeTexCoord');links.new(coords.outputs['Object'],image.inputs['Vector']);links.new(image.outputs['Color'],nodes.get('Principled BSDF').inputs['Base Color'])
bpy.ops.wm.save_as_mainfile(filepath=str(SOURCE/'postal-kit.blend'))
(SOURCE.parent/'reports/kit.json').write_text(json.dumps(mesh_assets,indent=2),encoding='utf-8')
print('POSTAL_KIT_COMPLETE',json.dumps(mesh_assets))

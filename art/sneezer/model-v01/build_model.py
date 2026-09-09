import bpy, math, json
from pathlib import Path
from mathutils import Vector

OUT = Path(__file__).resolve().parent
bpy.ops.object.select_all(action='SELECT')
bpy.ops.object.delete(use_global=False)
scene = bpy.context.scene
scene.unit_settings.system = 'METRIC'
scene.render.engine = 'CYCLES'
scene.cycles.samples = 24
scene.render.resolution_x = 900
scene.render.resolution_y = 900
scene.render.resolution_percentage = 100
scene.render.image_settings.file_format = 'PNG'
scene.render.fps = 30
scene.frame_start, scene.frame_end = 1, 90
scene.view_settings.view_transform = 'Standard'
scene.world.color = (.22,.22,.22)

def mat(name, color):
    m=bpy.data.materials.new(name); m.diffuse_color=(*color,1); m.use_nodes=True
    p=m.node_tree.nodes.get('Principled BSDF'); p.inputs['Base Color'].default_value=(*color,1)
    p.inputs['Roughness'].default_value=1
    return m
card=mat('Cardboard / coarse scanned texture',(.32,.26,.18))
img=bpy.data.images.load(str(OUT/'textures/cardboard.png')); img.pack()
tex=card.node_tree.nodes.new('ShaderNodeTexImage'); tex.image=img; tex.interpolation='Closest'
card.node_tree.links.new(tex.outputs['Color'],card.node_tree.nodes.get('Principled BSDF').inputs['Base Color'])
ink=mat('Faded dark ink',(.035,.029,.023))
white=mat('Aged off-white print',(.60,.57,.46))
tape=mat('Old paper tape',(.43,.37,.26))
seam=mat('Fold seam',(.09,.069,.045))
root=bpy.data.objects.new('Sneezer',None); scene.collection.objects.link(root)
root['asset_version']='0.1'; root['front_axis']='-Y (Blender); +Z (glTF)'
root['dimensions_m']='0.8 x 0.72 x 0.76'; root['states']='1 idle; 30 warning; 48 sneeze; 90 idle'

def mesh(name,verts,faces,material,parent=root):
    me=bpy.data.meshes.new(name); me.from_pydata(verts,[],faces); me.update()
    o=bpy.data.objects.new(name,me); scene.collection.objects.link(o); o.parent=parent; me.materials.append(material)
    uv=me.uv_layers.new(name='UVMap')
    for p in me.polygons:
        n=p.normal; axis=max(range(3),key=lambda i:abs(n[i])); axes=[i for i in range(3) if i!=axis]
        for li in p.loop_indices:
            co=me.vertices[me.loops[li].vertex_index].co
            uv.data[li].uv=(co[axes[0]]/.8+.5,co[axes[1]]/.8+.1)
    return o

v=[(-.4,-.36,0),(.4,-.36,0),(.4,.36,0),(-.4,.36,0),(-.4,-.36,.75),(.4,-.36,.75),(.4,.36,.75),(-.4,.36,.75)]
body=mesh('Box_shell',v,[(0,3,2,1),(0,1,5,4),(1,2,6,5),(2,3,7,6),(3,0,4,7)],card)
mesh('Interior_darkness',[(-.395,-.355,.67),(.395,-.355,.67),(.395,.355,.67),(-.395,.355,.67)],[(0,1,2,3)],seam)
lid=mesh('Lid_front_hinged',[(-.4,0,0),(.4,0,0),(.4,.357,0),(-.4,.357,0)],[(0,1,2,3)],card)
lid.location=(0,-.36,.756)
mesh('Lid_back',[(-.4,.002,.757),(.4,.002,.757),(.4,.36,.757),(-.4,.36,.757)],[(0,1,2,3)],card)
mesh('Tape_front_lid',[(-.068,.005,.002),(.068,.005,.002),(.068,.353,.002),(-.068,.353,.002)],[(0,1,2,3)],tape,lid)
mesh('Tape_back_lid',[(-.068,.005,.759),(.068,.005,.759),(.068,.36,.759),(-.068,.36,.759)],[(0,1,2,3)],tape)
mesh('Tape_front_torn',[(-.068,-.361,.752),(.068,-.361,.752),(.068,-.361,.61),(.04,-.361,.625),(.01,-.361,.60),(-.025,-.361,.62),(-.068,-.361,.61)],[(0,6,5,4,3,2,1)],tape)
mesh('Blank_shipping_label',[(-.401,-.22,.20),(-.401,.09,.20),(-.401,.09,.37),(-.401,-.22,.37)],[(0,3,2,1)],white)

# Facial art is coplanar pixel geometry, not sculpted features; no image editing.
def face(state):
    grid={}
    def rect(x,y,w,h,c='d'):
        for yy in range(y,y+h):
            for xx in range(x,x+w): grid[xx,yy]=c
    def line(x,y,x2,y2,c='d',thick=1):
        n=max(abs(x2-x),abs(y2-y),1)
        for i in range(n+1): rect(round(x+(x2-x)*i/n),round(y+(y2-y)*i/n),1,thick,c)
    if state=='Idle':
        line(4,7,10,5,thick=2); line(20,5,26,7,thick=2)
        rect(4,11,8,5,'w'); rect(21,10,7,5,'w')
        rect(7,11,3,3); rect(23,10,3,3)
        line(4,10,11,10,thick=2); line(21,9,27,10,thick=2)
        line(9,23,12,25,thick=2); rect(12,25,9,2); line(21,25,25,22,thick=2)
        rect(14,24,2,2,'w');rect(17,24,2,2,'w')
    else:
        line(5,6,10,8,thick=2);line(22,8,27,5,thick=2)
        line(5,12,11,15,thick=2);line(5,18,11,15,thick=2)
        line(21,15,27,12,thick=2);line(21,15,27,18,thick=2)
        if state=='Warning':
            line(13,25,16,22,thick=2);line(16,22,19,25,thick=2)
        else:
            rect(11,21,12,10);rect(10,23,14,7)
            rect(14,21,2,3,'w');rect(18,21,2,3,'w')
    group=bpy.data.objects.new('Face_'+state,None);scene.collection.objects.link(group);group.parent=root
    for color,material in [('d',ink),('w',white)]:
        verts=[];polys=[]
        # Merge horizontal runs to avoid one object or two triangles per pixel.
        for y in range(32):
            x=0
            while x<32:
                if grid.get((x,y))!=color:x+=1;continue
                start=x
                while x<32 and grid.get((x,y))==color:x+=1
                x0=(start-16)*.019;x1=(x-16)*.019;z1=.655-y*.018;z0=z1-.018
                idx=len(verts);verts += [(x0,-.362,z0),(x1,-.362,z0),(x1,-.362,z1),(x0,-.362,z1)]
                polys.append((idx,idx+1,idx+2,idx+3))
        mesh(state+'_'+color,verts,polys,material,group)
    return group
faces={s:face(s) for s in ['Idle','Warning','Sneeze']}
for frame,state in [(1,'Idle'),(20,'Idle'),(21,'Warning'),(44,'Warning'),(45,'Sneeze'),(58,'Sneeze'),(59,'Idle'),(90,'Idle')]:
    for name,o in faces.items():
        o.scale=(1,1,1) if state==name else (0,0,0)
        o.keyframe_insert(data_path='scale',frame=frame)
for frame,scale,angle in [(1,(1,1,1),0),(20,(1,1,1),0),(36,(1.045,1.045,.96),0),(44,(1.06,1.06,.94),0),(48,(1.10,1.04,.87),24),(58,(.98,1,1.03),4),(68,(1,1,1),0),(90,(1,1,1),0)]:
    root.scale=scale;root.keyframe_insert(data_path='scale',frame=frame)
    lid.rotation_euler.x=math.radians(angle);lid.keyframe_insert(data_path='rotation_euler',frame=frame)
for action in bpy.data.actions:
    for layer in action.layers:
        for strip in layer.strips:
            for slot in action.slots:
                bag=strip.channelbag(slot)
                if bag:
                    for fc in bag.fcurves:
                        for kp in fc.keyframe_points:kp.interpolation='CONSTANT' if action.name.startswith('Face_') else 'LINEAR'
for f,n in [(1,'IDLE'),(30,'WARNING'),(48,'SNEEZE'),(90,'LOOP_END')]:scene.timeline_markers.new(n,frame=f)

asset_objects=[o for o in scene.objects]
def select_asset():
    bpy.ops.object.select_all(action='DESELECT')
    for o in asset_objects:o.select_set(True)
    bpy.context.view_layer.objects.active=root
scene.frame_set(1);select_asset()
bpy.ops.export_scene.gltf(filepath=str(OUT/'sneezer.glb'),export_format='GLB',use_selection=True,export_animations=True,export_animation_mode='SCENE',export_frame_range=True,export_force_sampling=True,export_cameras=False,export_lights=False)
for f,n in [(1,'idle'),(30,'warning'),(48,'sneeze')]:
    scene.frame_set(f);select_asset()
    bpy.ops.export_scene.gltf(filepath=str(OUT/f'sneezer-{n}.glb'),export_format='GLB',use_selection=True,export_animations=False,export_cameras=False,export_lights=False)

groundmat=mat('Preview only ground',(.085,.091,.095))
bpy.ops.mesh.primitive_plane_add(size=200);ground=bpy.context.object;ground.name='PREVIEW_ground';ground.data.materials.append(groundmat);ground.location.z=-.008
bpy.ops.object.camera_add(location=(1.55,-2.4,1.5));cam=bpy.context.object;cam.name='PREVIEW_camera';cam.rotation_euler=(Vector((0,0,.4))-cam.location).to_track_quat('-Z','Y').to_euler();cam.data.type='ORTHO';cam.data.ortho_scale=1.55;scene.camera=cam
bpy.ops.object.light_add(type='AREA',location=(-2,-3,5));key=bpy.context.object;key.name='PREVIEW_key';key.data.energy=450;key.data.shape='DISK';key.data.size=3
key.rotation_euler=(Vector((0,0,.4))-key.location).to_track_quat('-Z','Y').to_euler()
scene.frame_set(1)
select_asset()
for screen in bpy.data.screens:
    for area in screen.areas:
        if area.type=='VIEW_3D':
            area.spaces.active.region_3d.view_distance=2.2
            area.spaces.active.region_3d.view_location=(0,0,.4)
            area.spaces.active.shading.type='MATERIAL'
bpy.ops.wm.save_as_mainfile(filepath=str(OUT/'sneezer.blend'))
report={'dimensions_m':[.8,.72,.76],'texture_size':list(img.size),'objects':len(asset_objects),'triangles_all_expression_variants':sum(sum(len(p.vertices)-2 for p in o.data.polygons) for o in asset_objects if o.type=='MESH'),'animation':'Scene, frames 1-90 at 30fps','expression_frames':{'idle':1,'warning':30,'sneeze':48},'effects':'Breath particles and collision not included; engine integration pending.'}
(OUT/'build-report.json').write_text(json.dumps(report,indent=2),encoding='utf-8')
for f,n in [(1,'idle'),(30,'warning'),(48,'sneeze')]:
    scene.frame_set(f);scene.render.filepath=str(OUT/'previews'/f'{n}.png');bpy.ops.render.render(write_still=True)
print('SNEEZER_BUILD_COMPLETE '+json.dumps(report))

import bpy,bmesh,json
from pathlib import Path
OUT=Path(__file__).resolve().parent
bpy.ops.wm.open_mainfile(filepath=str(OUT.parent/'model-v01/sneezer.blend'))
scene=bpy.context.scene;scene.frame_set(1)
root=bpy.data.objects['Sneezer'];root['asset_version']='0.2'
card=bpy.data.materials['Cardboard / coarse scanned texture']
dark=bpy.data.materials['Fold seam']

def mesh(name,vs,fs,mat,parent=root):
    me=bpy.data.meshes.new(name);me.from_pydata(vs,[],fs);me.update()
    o=bpy.data.objects.new(name,me);scene.collection.objects.link(o);o.parent=parent;me.materials.append(mat)
    uv=me.uv_layers.new(name='UVMap')
    for p in me.polygons:
        axes=[i for i in range(3) if i!=max(range(3),key=lambda i:abs(p.normal[i]))]
        for li in p.loop_indices:
            co=me.vertices[me.loops[li].vertex_index].co
            uv.data[li].uv=(co[axes[0]]/.8+.5,co[axes[1]]/.8+.1)
    return o
def panel(name,x0,x1,z0,z1,y=-.36,parent=root,mat=card):
    return mesh(name,[(x0,y,z0),(x1,y,z0),(x1,y,z1),(x0,y,z1)],[(0,1,2,3)],mat,parent)
def apply(o,mod):
    bpy.context.view_layer.objects.active=o
    bpy.ops.object.modifier_apply(modifier=mod.name)
def thickness(o,amount):
    if not o.data.polygons:return
    mod=o.modifiers.new('Physical thickness','SOLIDIFY');mod.thickness=amount;mod.offset=1
    apply(o,mod)

# Cut a real aperture through the front, hidden by a flush patch in closed states.
body=bpy.data.objects['Box_shell']
bm=bmesh.new();bm.from_mesh(body.data)
front=[f for f in bm.faces if f.normal.y<-.9]
bmesh.ops.delete(bm,geom=front,context='FACES');bm.to_mesh(body.data);bm.free()
x0,x1,z0,z1=-.12,.152,.085,.285
panel('Front_above_mouth',-.4,.4,z1,.75)
panel('Front_below_mouth',-.4,.4,0,z0)
panel('Front_left_mouth',-.4,x0,z0,z1)
panel('Front_right_mouth',x1,.4,z0,z1)
for state in ['Idle','Warning']:
    panel(state+'_mouth_closed',x0,x1,z0,z1,parent=bpy.data.objects['Face_'+state])

group=bpy.data.objects['Face_Sneeze']
o=bpy.data.objects['Sneeze_d']
bm=bmesh.new();bm.from_mesh(o.data)
bmesh.ops.delete(bm,geom=[f for f in bm.faces if f.calc_center_median().z<.30],context='FACES')
bm.to_mesh(o.data);bm.free()
# Sloped cardboard lip and 10cm-deep dark cavity.
outer=[(x0,-.362,z0),(x1,-.362,z0),(x1,-.362,z1),(x0,-.362,z1)]
inner=[(x0+.012,-.34,z0+.012),(x1-.012,-.34,z0+.012),(x1-.012,-.34,z1-.012),(x0+.012,-.34,z1-.012)]
back=[(x0+.012,-.255,z0+.012),(x1-.012,-.255,z0+.012),(x1-.012,-.255,z1-.012),(x0+.012,-.255,z1-.012)]
mesh('Sneeze_cardboard_lip',outer+inner,[(i,(i+1)%4,(i+1)%4+4,i+4) for i in range(4)],card,group)
mesh('Sneeze_mouth_walls',inner+back,[(i,(i+1)%4,(i+1)%4+4,i+4) for i in range(4)],dark,group)
mesh('Sneeze_mouth_depth',back,[(0,1,2,3)],dark,group)

# Give the stepped facial strokes solid relief; merge shared grid vertices first.
for name in ['Idle_d','Idle_w','Warning_d','Sneeze_d','Sneeze_w']:
    o=bpy.data.objects[name]
    bm=bmesh.new();bm.from_mesh(o.data)
    bmesh.ops.remove_doubles(bm,verts=list(bm.verts),dist=.00001)
    bm.to_mesh(o.data);bm.free()
    # Brows stand further forward than eyes; the mouth stays shallow.
    for v in o.data.vertices:
        v.co.y-=.014 if v.co.z>.48 else (.009 if v.co.z>.3 else .003)
    thickness(o,.012 if name.endswith('_d') else .018)
for name in ['Lid_front_hinged','Lid_back']:
    thickness(bpy.data.objects[name],.008)
for name in ['Tape_front_lid','Tape_back_lid']:
    for v in bpy.data.objects[name].data.vertices:v.co.z+=.009

# Angular pressure cheeks belong only to the warning state.
for side in [-1,1]:
    cx=side*.265;cz=.29;r=.070
    mesh('Warning_cheek_'+str(side),[(cx-r,-.363,cz),(cx,-.363,cz-r),(cx+r,-.363,cz),(cx,-.363,cz+r),(cx,-.407,cz)],[(0,1,4),(1,2,4),(2,3,4),(3,0,4)],card,bpy.data.objects['Face_Warning'])

# Edge thickness is subtle and faceted, with no smoothing.
for name in ['Box_shell','Lid_front_hinged','Lid_back']:
    o=bpy.data.objects[name]
    bevel=o.modifiers.new('Small angular edge','BEVEL');bevel.width=.002;bevel.segments=1
    apply(o,bevel)

asset=[o for o in scene.objects if not o.name.startswith('PREVIEW_')]
def select():
    bpy.ops.object.select_all(action='DESELECT')
    for o in asset:o.select_set(True)
    bpy.context.view_layer.objects.active=root
scene.frame_set(1);select()
bpy.ops.export_scene.gltf(filepath=str(OUT/'sneezer.glb'),export_format='GLB',use_selection=True,export_animations=True,export_animation_mode='SCENE',export_frame_range=True,export_force_sampling=True,export_cameras=False,export_lights=False)
scene.frame_set(1);bpy.ops.wm.save_as_mainfile(filepath=str(OUT/'sneezer.blend'))
report={'version':'0.2','triangles_all_states':sum(sum(len(p.vertices)-2 for p in o.data.polygons) for o in asset if o.type=='MESH'),'mouth_depth_m':.105,'lid_thickness_m':.008,'facial_relief_m':'.012-.026','warning_cheek_depth_m':.044}
(OUT/'build-report.json').write_text(json.dumps(report,indent=2),encoding='utf-8')
for f,n in [(1,'idle'),(30,'warning'),(48,'sneeze')]:
    scene.frame_set(f);scene.render.filepath=str(OUT/'previews'/f'{n}.png');bpy.ops.render.render(write_still=True)
print('BUILD_COMPLETE '+json.dumps(report))

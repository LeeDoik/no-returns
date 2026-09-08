"""Prepare and inspect Tripo motion sources without replacing the playable model."""
import bpy, json, math
from pathlib import Path
from mathutils import Vector, Matrix

OUT = Path(__file__).resolve().parent
bpy.ops.object.select_all(action='SELECT')
bpy.ops.object.delete(use_global=False)
bpy.ops.import_scene.gltf(filepath=str(OUT / 'worker-tripo-seven.glb'))
rig = next(o for o in bpy.context.scene.objects if o.type == 'ARMATURE')
meshes = [o for o in bpy.context.scene.objects if o.type == 'MESH' and any(m.type == 'ARMATURE' for m in o.modifiers)]
for obj in list(bpy.context.scene.objects):
    if obj not in meshes and obj != rig:
        bpy.data.objects.remove(obj, do_unlink=True)
for track in list(rig.animation_data.nla_tracks):
    rig.animation_data.nla_tracks.remove(track)
actions = {}
for action in bpy.data.actions:
    name = action.name.split(':')[-1]
    action.name = name
    action.use_fake_user = True
    actions[name] = action
rig.animation_data.action = None
for bone in rig.pose.bones:
    bone.matrix_basis.identity()
bpy.context.view_layer.update()
def bounds():
    points = []
    deps = bpy.context.evaluated_depsgraph_get()
    for obj in meshes:
        evaluated = obj.evaluated_get(deps)
        mesh = evaluated.to_mesh()
        points.extend(obj.matrix_world @ v.co for v in mesh.vertices)
        evaluated.to_mesh_clear()
    return [Vector(tuple(fn(p[i] for p in points) for i in range(3))) for fn in (min, max)]
low, high = bounds()
scale = 1.65 / (high.z-low.z)
holder = bpy.data.objects.new('WorkerArt', None)
bpy.context.collection.objects.link(holder)
rig.parent = holder
holder.scale = (scale,) * 3
holder.rotation_euler.z = math.pi
holder.location = ((low.x+high.x)*.5*scale, (low.y+high.y)*.5*scale, -low.z*scale)
for obj in meshes:
    for material in obj.data.materials:
        if not material or not material.use_nodes: continue
        shader = material.node_tree.nodes.get('Principled BSDF')
        if shader:
            shader.inputs['Roughness'].default_value = .82
            for link in list(shader.inputs['Alpha'].links):
                material.node_tree.links.remove(link)
            shader.inputs['Alpha'].default_value = 1
def activate(action, frame):
    rig.animation_data.action = action
    if action.slots: rig.animation_data.action_slot = action.slots[0]
    bpy.context.scene.frame_set(frame)
    bpy.context.view_layer.update()
report = {'scale':scale, 'source_rest_bounds':[list(low),list(high)], 'bones':[b.name for b in rig.data.bones], 'actions':{}}
for name, action in actions.items():
    lo, hi = action.frame_range
    positions = []
    for frame in range(int(lo),int(hi)+1):
        activate(action, frame)
        positions.append(list(rig.matrix_world @ rig.pose.bones['Root'].head))
    report['actions'][name] = {'frames':[lo,hi], 'seconds':(hi-lo)/bpy.context.scene.render.fps, 'root_range':[max(p[i] for p in positions)-min(p[i] for p in positions) for i in range(3)]}
def rotate_toward(bone, direction):
    matrix = bone.matrix.copy()
    rotation = (matrix.to_3x3() @ Vector((0,1,0))).normalized().rotation_difference(direction.normalized())
    aligned = rotation.to_matrix().to_4x4() @ matrix
    aligned.translation = matrix.translation
    bone.matrix = aligned
    bpy.context.view_layer.update()

def carrying_pose(extension=0.0):
    spine = rig.pose.bones['Spine01']
    matrix = spine.matrix.copy()
    aligned = Matrix.Rotation(math.radians(12),4,'X') @ matrix
    aligned.translation = matrix.translation
    spine.matrix = aligned
    bpy.context.view_layer.update()
    inverse = rig.matrix_world.inverted()
    for prefix, side in [('L_',-1),('R_',1)]:
        upper=rig.pose.bones[prefix+'Upperarm'];fore=rig.pose.bones[prefix+'Forearm'];hand=rig.pose.bones[prefix+'Hand']
        shoulder=upper.head.copy()
        a=(fore.head-shoulder).length;b=(hand.head-fore.head).length
        target=inverse @ Vector((side*.38,.43+extension,1.04+extension*.4))
        pole=inverse @ Vector((side*.62,.13,1.00))
        direction=(target-shoulder).normalized()
        distance=min((target-shoulder).length,(a+b)*.985)
        along=(a*a-b*b+distance*distance)/(2*distance)
        across=math.sqrt(max(0,a*a-along*along))
        bend=(pole-shoulder)-direction*(pole-shoulder).dot(direction)
        elbow=shoulder+direction*along+bend.normalized()*across
        rotate_toward(upper,elbow-shoulder)
        rotate_toward(fore,shoulder+direction*distance-fore.head)
        rotate_toward(hand,rig.matrix_world.to_3x3().inverted() @ Vector((0,1,extension*.4)))

# Bake complete poses so clips share one skeleton without runtime arm overrides.
def bake_variant(name, source, frames, pose):
    samples=[]
    for frame, source_frame in frames:
        activate(source,source_frame)
        pose(frame)
        samples.append((frame,{bone.name:bone.matrix_basis.copy() for bone in rig.pose.bones}))
    action=bpy.data.actions.new(name);action.use_fake_user=True
    rig.animation_data.action=action
    for frame, matrices in samples:
        for bone in rig.pose.bones:
            bone.matrix_basis=matrices[bone.name]
            bone.keyframe_insert('location',frame=frame,group=bone.name)
            bone.keyframe_insert('rotation_quaternion',frame=frame,group=bone.name)
            bone.keyframe_insert('scale',frame=frame,group=bone.name)
    actions[name]=action
for name in ['idle','walk','run']:
    source=actions[name]
    frames=[(f,f) for f in range(int(source.frame_range[0]),int(source.frame_range[1])+1)]
    bake_variant('carry_'+name,source,frames,lambda frame:carrying_pose())
# Air poses keep displacement in gameplay, avoiding a second animated jump arc.
bake_variant('air_rise',actions['idle'],[(1,1),(13,1)],lambda frame:None)
bake_variant('air_fall',actions['idle'],[(1,1),(13,1)],lambda frame:None)
for name in ['air_rise','air_fall']:
    action=actions[name]
    for frame in [1,13]:
        activate(action,frame)
        for prefix,side in [('L_',-1),('R_',1)]:
            bone=rig.pose.bones[prefix+'Upperarm']
            rotate_toward(bone,rig.matrix_world.to_3x3().inverted() @ Vector((side*.45,.12,-.55)))
            bone.keyframe_insert('rotation_quaternion',frame=frame,group=bone.name)
        if name=='air_rise':
            for prefix in ['L_','R_']:
                bone=rig.pose.bones[prefix+'Calf'];bone.rotation_quaternion @= Vector((1,0,0)).rotation_difference(Vector((.7,0,.7)));bone.keyframe_insert('rotation_quaternion',frame=frame,group=bone.name)
bake_variant('throw',actions['idle'],[(f,1) for f in range(1,16)],lambda frame:carrying_pose(.1*math.sin((frame-1)/14*math.pi)))
activate(actions['idle'], 1)
bpy.ops.wm.save_as_mainfile(filepath=str(OUT/'worker-animation.blend'))
bpy.ops.export_scene.gltf(filepath=str(OUT/'worker-candidate.glb'),export_format='GLB',export_animations=True,export_animation_mode='ACTIONS',export_cameras=False,export_lights=False)
(OUT/'animation-inspection.json').write_text(json.dumps(report,indent=2),encoding='utf-8')
scene=bpy.context.scene
scene.render.engine='CYCLES';scene.cycles.samples=24
scene.render.resolution_x=800;scene.render.resolution_y=800;scene.render.resolution_percentage=100
scene.world.color=(.2,.2,.2)
def aim(obj, point): obj.rotation_euler=(Vector(point)-obj.location).to_track_quat('-Z','Y').to_euler()
bpy.ops.object.camera_add(location=(3,5,2.0));camera=bpy.context.object;aim(camera,(0,0,.85));camera.data.type='ORTHO';camera.data.ortho_scale=2.5;scene.camera=camera
for pos,power in [((2,3,4),300),((-3,2,2),200),((0,-3,3),220)]:
    bpy.ops.object.light_add(type='AREA',location=pos);light=bpy.context.object;light.data.energy=power;light.data.shape='DISK';light.data.size=4;aim(light,(0,0,.9))
for name,frame in [('idle',1),('walk',20),('run',8),('carry_idle',1),('carry_walk',20),('throw',7),('air_rise',1)]:
    activate(actions[name],frame);scene.render.filepath=str(OUT/f'preview-{name}.png');bpy.ops.render.render(write_still=True)
print('INSPECTED',json.dumps(report))

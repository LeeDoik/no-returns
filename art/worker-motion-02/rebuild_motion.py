"""Rebake centered, floor-corrected motions and separate carrying upper bodies."""
import bpy, math, json
from pathlib import Path
from mathutils import Vector, Matrix

OUT=Path(__file__).resolve().parent
ROOT=OUT.parents[1]
bpy.ops.wm.open_mainfile(filepath=str(ROOT/'art/tripo-01/worker-animation.blend'))
rig=next(o for o in bpy.context.scene.objects if o.type=='ARMATURE')
meshes=[o for o in bpy.context.scene.objects if o.type=='MESH']
source={a.name:a for a in bpy.data.actions}
for name,action in source.items(): action.name='source_'+name
scene=bpy.context.scene; scene.render.fps=60
inverse=rig.matrix_world.inverted()
directions=rig.matrix_world.to_3x3().inverted()

def activate(name,phase=0):
 a=source[name];rig.animation_data.action=a;rig.animation_data.action_slot=a.slots[0]
 value=a.frame_range[0]+phase*(a.frame_range[1]-a.frame_range[0])
 scene.frame_set(int(value),subframe=value-int(value));bpy.context.view_layer.update()

activate('idle')
neutral={b.name:b.matrix_basis.copy() for b in rig.pose.bones}
upper=[b.name for b in rig.pose.bones if b.name not in ['Root','Hip','Pelvis'] and not any(word in b.name for word in ['Thigh','Calf','Foot','Toe'])]

def translate_hip(delta):
 b=rig.pose.bones['Hip'];m=b.matrix.copy();m.translation+=directions@delta;b.matrix=m;bpy.context.view_layer.update()

def center():
 p=rig.matrix_world@rig.pose.bones['Hip'].head
 translate_hip(Vector((-p.x,-p.y,0)))

def rotate_toward(bone,direction):
 matrix=bone.matrix.copy()
 q=(matrix.to_3x3()@Vector((0,1,0))).normalized().rotation_difference(direction.normalized())
 aligned=q.to_matrix().to_4x4()@matrix;aligned.translation=matrix.translation;bone.matrix=aligned
 bpy.context.view_layer.update()

def solve(upper,lower,end,target,pole):
 a=rig.pose.bones[upper];b=rig.pose.bones[lower];c=rig.pose.bones[end]
 start=a.head.copy();length_a=(b.head-start).length;length_b=(c.head-b.head).length
 target=inverse@Vector(target);pole=inverse@Vector(pole)
 vector=target-start;distance=min(vector.length,(length_a+length_b)*.97)
 direction=vector.normalized();along=(length_a**2-length_b**2+distance**2)/(2*distance)
 across=math.sqrt(max(0,length_a**2-along**2))
 bend=pole-start-direction*(pole-start).dot(direction)
 elbow=start+direction*along+bend.normalized()*across
 rotate_toward(a,elbow-start);rotate_toward(b,start+direction*distance-b.head)

def upper_neutral(lean=0):
 for name in upper: rig.pose.bones[name].matrix_basis=neutral[name]
 bpy.context.view_layer.update()
 bone=rig.pose.bones['Spine01'];matrix=bone.matrix.copy()
 turn=Matrix.Rotation(math.radians(lean),4,'X')@matrix;turn.translation=matrix.translation;bone.matrix=turn
 bpy.context.view_layer.update()

def carry(extension=0):
 upper_neutral(7)
 for prefix,side in [('L_',-1),('R_',1)]:
  solve(prefix+'Upperarm',prefix+'Forearm',prefix+'Hand',(side*.40,.44+extension,1.0+extension*.2),(side*.55,.13,.91))
  rotate_toward(rig.pose.bones[prefix+'Hand'],directions@Vector((0,1,.10)))

def floor_correct():
 deps=bpy.context.evaluated_depsgraph_get()
 low=min((o.matrix_world@v.co).z for o in meshes for v in o.evaluated_get(deps).data.vertices)
 translate_hip(Vector((0,0,max(0,.004-low))))

def stride_side(angle):
 positions={p:rig.matrix_world@rig.pose.bones[p+'Foot'].head for p in ['L_','R_']}
 for prefix,side in [('L_',-1),('R_',1)]:
  foot=rig.pose.bones[prefix+'Foot'];rotation=foot.matrix.to_3x3().copy()
  at=positions[prefix]
  target=Vector((side*.13+math.sin(angle)*at.y*.65, math.cos(angle)*at.y+.04+(.09 if at.z>.08 else 0),at.z))
  solve(prefix+'Thigh',prefix+'Calf',prefix+'Foot',target,(side*.3,.65,.4))
  matrix=rotation.to_4x4();matrix.translation=foot.head;foot.matrix=matrix
  bpy.context.view_layer.update()

def pose(name,t):
 if name in ['walk','run','carry_walk','carry_back','carry_left','carry_right','carry_forward_left','carry_forward_right','carry_back_left','carry_back_right']:
  phase=(1-t)%1 if name=='carry_back' else t
  activate('run' if name=='run' else 'walk',phase)
 else: activate('idle',0)
 center()
 angles={'carry_left':-90,'carry_right':90,'carry_forward_left':-45,'carry_forward_right':45,'carry_back_left':-135,'carry_back_right':135}
 if name in angles: stride_side(math.radians(angles[name]))
 if name.startswith('carry') and name!='carry_air': carry()
 elif name=='idle':
  upper_neutral(.5*math.sin(t*2*math.pi))
 elif name in ['air_rise','air_fall','carry_air']:
  upper_neutral(3)
  for prefix,side in [('L_',-1),('R_',1)]:
   rotate_toward(rig.pose.bones[prefix+'Upperarm'],directions@Vector((side*.35,.10,-.65)))
   foot=rig.pose.bones[prefix+'Foot'];rotation=foot.matrix.to_3x3().copy()
   solve(prefix+'Thigh',prefix+'Calf',prefix+'Foot',(side*.13,-.10,.23 if name!='air_fall' else .10),(side*.3,.65,.4))
   matrix=rotation.to_4x4();matrix.translation=foot.head;foot.matrix=matrix;bpy.context.view_layer.update()
 elif name in ['land','hit_to_body_01']:
  pulse=math.sin(math.pi*t)**2
  feet={p: (rig.matrix_world@rig.pose.bones[p+'Foot'].head,rig.pose.bones[p+'Foot'].matrix.to_3x3().copy()) for p in ['L_','R_']}
  translate_hip(Vector((0,0,-.075*pulse)))
  upper_neutral(9*pulse)
  for prefix,side in [('L_',-1),('R_',1)]:
   solve(prefix+'Thigh',prefix+'Calf',prefix+'Foot',feet[prefix][0],(side*.3,.65,.4))
   foot=rig.pose.bones[prefix+'Foot'];matrix=feet[prefix][1].to_4x4();matrix.translation=foot.head;foot.matrix=matrix;bpy.context.view_layer.update()
 elif name=='throw':
  # Release at frame zero, outward follow-through, then recover to neutral.
  carry(.065*math.sin(math.pi*t))
  if t>.3:
   blend=(t-.3)/.7;blend=blend*blend*(3-2*blend)
   for name_bone in upper:
    bone=rig.pose.bones[name_bone]
    bone.matrix_basis=bone.matrix_basis.lerp(neutral[name_bone],blend)
   bpy.context.view_layer.update()
 floor_correct()
 # Solve the hands last so floor correction never makes the box contact bob.
 if name.startswith('carry'): carry()

durations={'idle':2.4,'walk':2.0,'run':1.0,'carry_idle':2.4,'carry_walk':2.0,'carry_back':2.0,'carry_left':2.0,'carry_right':2.0,'carry_forward_left':2.0,'carry_forward_right':2.0,'carry_back_left':2.0,'carry_back_right':2.0,'carry_air':.4,'air_rise':.4,'air_fall':.4,'land':.3,'throw':.48,'hit_to_body_01':.5}
loops=set(durations)-{'land','throw','hit_to_body_01'}
samples={}
for name,duration in durations.items():
 count=round(duration*60);sequence=[]
 for frame in range(count+1):
  pose(name,frame/count if frame<count or name not in loops else 0)
  sequence.append({b.name:b.matrix_basis.copy() for b in rig.pose.bones})
 samples[name]=sequence
 print('SAMPLED',name,flush=True)

rig.animation_data.action=None
for action in list(bpy.data.actions): bpy.data.actions.remove(action)
for name,sequence in samples.items():
 action=bpy.data.actions.new(name);action.use_fake_user=True;rig.animation_data.action=action
 previous={}
 for frame,matrices in enumerate(sequence):
  for name_bone,matrix in matrices.items():
   bone=rig.pose.bones[name_bone];bone.rotation_mode='QUATERNION';bone.matrix_basis=matrix
   if name_bone in previous and bone.rotation_quaternion.dot(previous[name_bone])<0: bone.rotation_quaternion.negate()
   previous[name_bone]=bone.rotation_quaternion.copy()
   for channel in ['location','rotation_quaternion','scale']: bone.keyframe_insert(channel,frame=frame,group=name_bone)
 # Linear sampled curves cannot overshoot authored joint poses between keys.
 for layer in action.layers:
  for strip in layer.strips:
   for bag in strip.channelbags:
    for curve in bag.fcurves:
     for key in curve.keyframe_points: key.interpolation='LINEAR'

rig.animation_data.action=bpy.data.actions['idle'];rig.animation_data.action_slot=rig.animation_data.action.slots[0]
scene.frame_set(0)
bpy.ops.wm.save_as_mainfile(filepath=str(OUT/'worker-motion.blend'))
bpy.ops.export_scene.gltf(filepath=str(OUT/'worker.glb'),export_format='GLB',export_animations=True,export_animation_mode='ACTIONS',export_cameras=False,export_lights=False)
(OUT/'motion-report.json').write_text(json.dumps({'fps':60,'durations':durations,'source':'art/tripo-01/worker-animation.blend','corrections':['center Hip XY per frame','evaluated sole floor correction','independent carry upper body','directional carry lower body','explicit throw recovery','linear sampled keys']},indent=2))
print('MOTION REBUILD COMPLETE',flush=True)

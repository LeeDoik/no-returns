"""Normalize generated characters in Blender, retain armatures and animation."""
import bpy, json, math, sys
from pathlib import Path
from mathutils import Vector
root=Path(__file__).resolve().parents[3];source=Path(__file__).resolve().parent
name=sys.argv[sys.argv.index('--')+1] if '--' in sys.argv else 'packrat'
bpy.ops.object.select_all(action='SELECT');bpy.ops.object.delete(use_global=False)
bpy.ops.import_scene.gltf(filepath=str(source/f'{name}-generated.glb'))
bpy.context.view_layer.update()
for o in bpy.context.scene.objects: print('IMPORTED',o.name,o.type,list(o.dimensions),list(o.location))
meshes=[o for o in bpy.context.scene.objects if o.type=='MESH'];rigs=[o for o in bpy.context.scene.objects if o.type=='ARMATURE']
if rigs:
    # Meshy embeds a hidden controller shape; it is not the character's body.
    for o in list(meshes):
        if not any(m.type=='ARMATURE' for m in o.modifiers):
            meshes.remove(o);bpy.data.objects.remove(o,do_unlink=True)
points=[]
deps=bpy.context.evaluated_depsgraph_get()
for o in meshes:
    ev=o.evaluated_get(deps);me=ev.to_mesh()
    points.extend(o.matrix_world@v.co for v in me.vertices);ev.to_mesh_clear()
lo=Vector(tuple(min(p[i] for p in points) for i in range(3)));hi=Vector(tuple(max(p[i] for p in points) for i in range(3)))
holder=bpy.data.objects.new(name,None);bpy.context.collection.objects.link(holder)
for o in list(bpy.context.scene.objects):
    if o!=holder and o.parent is None:o.parent=holder
scale=(1.65 if name=='worker' else .9)/(hi.z-lo.z)
holder.scale=(scale,)*3;holder.location=(-(lo.x+hi.x)/2*scale,-(lo.y+hi.y)/2*scale,-lo.z*scale)
# Generated glTF faces +Z (Blender -Y); Godot's locomotion faces -Z.
holder.rotation_euler.z=math.pi
if name=='packrat':
    bpy.context.view_layer.update()
    for o in meshes:
        transform=o.matrix_world.copy();o.parent=None;o.matrix_world.identity()
        o.data.transform(transform)
    bpy.data.objects.remove(holder,do_unlink=True)
    arm=bpy.data.armatures.new('PackratRig');rig=bpy.data.objects.new('PackratRig',arm);bpy.context.collection.objects.link(rig)
    bpy.context.view_layer.objects.active=rig;rig.select_set(True);bpy.ops.object.mode_set(mode='EDIT')
    body=arm.edit_bones.new('Body');body.head=(0,0,.3);body.tail=(0,0,.6)
    feet=[]
    for x in [-.25,.25]:
        for y in [-.28,.28]:
            label=('L' if x<0 else 'R')+('Front' if y>0 else 'Rear')
            b=arm.edit_bones.new(label);b.head=(x,y,.28);b.tail=(x,y,.06);b.parent=body;feet.append((label,x,y))
    tail=arm.edit_bones.new('Tail');tail.head=(0,-.30,.25);tail.tail=(0,-.60,.25);tail.parent=body
    bpy.ops.object.mode_set(mode='OBJECT')
    for o in meshes:
        groups={label:o.vertex_groups.new(name=label) for label in ['Body','Tail']+[f[0] for f in feet]}
        for v in o.data.vertices:
            p=v.co;label,x,y=min(feet,key=lambda f:(p.x-f[1])**2+(p.y-f[2])**2)
            weight=max(0,min(1,(.30-p.z)/.15))*max(0,min(1,(abs(p.x)-.05)/.12))
            tail_weight=max(0,min(1,(-p.y-.36)/.2)) if p.z>.13 else 0
            groups[label].add([v.index],weight*(1-tail_weight),'REPLACE');groups['Tail'].add([v.index],tail_weight,'REPLACE');groups['Body'].add([v.index],(1-weight)*(1-tail_weight),'REPLACE')
        mod=o.modifiers.new('Soft paw and tail deformation','ARMATURE');mod.object=rig;o.parent=rig
    rigs=[rig]
for o in meshes:
    for m in o.data.materials:
        if not m:continue
        m.diffuse_color=(*m.diffuse_color[:3],1)
        if m.use_nodes:
            p=m.node_tree.nodes.get('Principled BSDF')
            if p:
                p.inputs['Alpha'].default_value=1
                for link in list(p.inputs['Alpha'].links):m.node_tree.links.remove(link)
                p.inputs['Roughness'].default_value=.82
    for p in o.data.polygons:p.use_smooth=True
report={'id':name,'source_bounds':[list(lo),list(hi)],'scale':scale,'triangles':sum(sum(len(p.vertices)-2 for p in o.data.polygons) for o in meshes),'bones':{o.name:[b.name for b in o.data.bones] for o in rigs},'actions':[a.name for a in bpy.data.actions]}
(source.parent/'reports'/f'{name}-blender.json').write_text(json.dumps(report,indent=2))
bpy.ops.wm.save_as_mainfile(filepath=str(source/f'{name}.blend'))
bpy.ops.export_scene.gltf(filepath=str(root/'assets/art/release-01'/f'{name}.glb'),export_format='GLB',export_animations=True,export_animation_mode='ACTIONS',export_cameras=False,export_lights=False)
print('CHARACTER_READY',json.dumps(report))

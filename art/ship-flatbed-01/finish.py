"""Bake a shared cabin paint finish and publish the reviewed export without scale dummies."""
import bpy
from pathlib import Path
from mathutils import Vector
R=Path(__file__).resolve().parent
bpy.ops.wm.open_mainfile(filepath=str(R/'Flatbed_Integrated.blend'))
s=bpy.context.scene;s.frame_set(40)
for o in list(s.objects):
 if o.name.startswith('REF_'):bpy.data.objects.remove(o,do_unlink=True)
# Bake a procedural 3D material to a portable 512px texture. No external imagery is edited.
bpy.ops.mesh.primitive_plane_add(size=2,location=(0,0,-20));plane=bpy.context.object
m=bpy.data.materials.new('CabinPaintBake');m.use_nodes=True;n=m.node_tree.nodes;n.clear();links=m.node_tree.links
out=n.new('ShaderNodeOutputMaterial');em=n.new('ShaderNodeEmission');noise=n.new('ShaderNodeTexNoise');noise.inputs['Scale'].default_value=85;noise.inputs['Detail'].default_value=1
uv=n.new('ShaderNodeTexCoord');ramp=n.new('ShaderNodeValToRGB');ramp.color_ramp.elements[0].position=.28;ramp.color_ramp.elements[0].color=(.285,.25,.195,1);ramp.color_ramp.elements[1].position=.65;ramp.color_ramp.elements[1].color=(.34,.30,.24,1)
links.new(uv.outputs['UV'],noise.inputs['Vector']);links.new(noise.outputs['Fac'],ramp.inputs[0]);links.new(ramp.outputs[0],em.inputs[0]);links.new(em.outputs[0],out.inputs[0])
img=bpy.data.images.new('Cabin_Paint_512',512,512);target=n.new('ShaderNodeTexImage');target.image=img;n.active=target;plane.data.materials.append(m)
bpy.ops.object.select_all(action='DESELECT');plane.select_set(True);bpy.context.view_layer.objects.active=plane;s.cycles.samples=1;bpy.ops.object.bake(type='EMIT')
img.filepath_raw=str(R/'Cabin_Paint_512.png');img.file_format='PNG';img.save();bpy.data.objects.remove(plane,do_unlink=True)
paint=bpy.data.materials.get('Paint_Ivory');node=paint.node_tree.nodes.new('ShaderNodeTexImage');node.image=img;node.interpolation='Closest';paint.node_tree.links.new(node.outputs['Color'],paint.node_tree.nodes.get('Principled BSDF').inputs['Base Color'])
meshes=[o for o in s.objects if o.type=='MESH'];bpy.ops.object.select_all(action='DESELECT')
for o in meshes:o.select_set(True)
bpy.ops.export_scene.gltf(filepath=str(R/'Flatbed_Integrated.glb'),use_selection=True,export_animations=True)
bpy.ops.export_scene.fbx(filepath=str(R/'Flatbed_Integrated.fbx'),use_selection=True,add_leaf_bones=False,bake_anim=False,axis_forward='-Z',axis_up='Y',path_mode='COPY',embed_textures=True)
s.cycles.samples=24;cam=s.camera
for name,pos,target,ortho,frame in [('integrated-front',(14,17,11),(0,0,1.5),19,1),('integrated-rear',(12,-17,10),(0,-1,1.5),20,40),('integrated-interior',(0,-3.3,2.17),(0,3.4,1.8),0,40),('integrated-cockpit',(0,3.1,2.77),(0,5.5,2.9),0,40),('integrated-closed',(10,-15,8),(0,-1,1.5),19,1)]:
 s.frame_set(frame);cam.location=pos;cam.rotation_euler=(Vector(target)-cam.location).to_track_quat('-Z','Y').to_euler();cam.data.type='ORTHO' if ortho else 'PERSP';cam.data.ortho_scale=ortho or 16;cam.data.lens=20;s.render.filepath=str(R/'review'/(name+'.png'));bpy.ops.render.render(write_still=True)
s.frame_set(40);bpy.ops.file.pack_all();bpy.ops.wm.save_as_mainfile(filepath=str(R/'Flatbed_Integrated.blend'))

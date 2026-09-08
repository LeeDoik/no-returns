import bpy
from pathlib import Path
root=Path(__file__).resolve().parents[3]
bpy.ops.wm.open_mainfile(filepath=str(root/'art/release-01/source/postal-kit.blend'))
for o in bpy.data.objects:
    if o.name.startswith(('FaceWarning','FaceBurst')):
        for child in o.children_recursive: child.hide_render=True; child.hide_viewport=True
for name,texture in [('Kraft cardboard','cardboard'),('Sage enamel','paint')]:
    m=bpy.data.materials[name];nodes=m.node_tree.nodes;links=m.node_tree.links
    image=nodes.new('ShaderNodeTexImage');image.image=bpy.data.images.load(str(root/'assets/art/release-01/textures'/f'{texture}_basecolor.png'));image.projection='BOX';image.projection_blend=.2
    coords=nodes.new('ShaderNodeTexCoord');links.new(coords.outputs['Object'],image.inputs['Vector']);links.new(image.outputs['Color'],nodes.get('Principled BSDF').inputs['Base Color'])
bpy.ops.file.make_paths_relative()
bpy.ops.wm.save_as_mainfile(filepath=str(root/'art/release-01/source/postal-kit.blend'))

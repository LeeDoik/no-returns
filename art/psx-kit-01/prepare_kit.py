"""Import Tripo reduced sources, export review FBXs and render the real meshes."""
import bpy, json
from pathlib import Path
from mathutils import Vector

ROOT = Path(__file__).resolve().parents[2]
SRC = ROOT / 'art/psx-kit-01/tripo-source'
OUT = ROOT / 'art/psx-kit-01/prepared'
OUT.mkdir(exist_ok=True)
bpy.ops.object.select_all(action='SELECT')
bpy.ops.object.delete(use_global=False)
items = [('Wall',1500,3.0),('Corner',2500,3.0),('Door',3000,3.2),('Floor',1000,0.25),('Lamp',800,0.65),('Rack',4000,2.4)]
report=[]
for i,(name,budget,height) in enumerate(items):
    before=set(bpy.data.objects)
    bpy.ops.import_scene.gltf(filepath=str(SRC/f'NR_{name}_Tripo_{budget}.glb'))
    imported=set(bpy.data.objects)-before
    meshes=[o for o in imported if o.type=='MESH']
    bpy.ops.object.select_all(action='DESELECT')
    for o in meshes:
        o.parent=None
        o.select_set(True)
    bpy.context.view_layer.objects.active=meshes[0]
    bpy.ops.object.join()
    o=bpy.context.object
    o.name=f'NR_{name}_Review'
    bpy.ops.object.transform_apply(location=False,rotation=True,scale=True)
    bounds=[o.matrix_world@Vector(v) for v in o.bound_box]
    lo=Vector(tuple(min(v[k] for v in bounds) for k in range(3)))
    hi=Vector(tuple(max(v[k] for v in bounds) for k in range(3)))
    factor=height/(hi.z-lo.z)
    center=Vector(((lo.x+hi.x)/2,(lo.y+hi.y)/2,lo.z))
    for v in o.data.vertices: v.co=(o.matrix_world@v.co-center)*factor
    o.matrix_world.identity()
    for mat in o.data.materials:
        mat.name=f'NR_{name}_Material'
        for node in mat.node_tree.nodes:
            if node.type=='TEX_IMAGE' and node.image:
                img=node.image
                img.filepath_raw=str(OUT/f'{name}_{img.name}.png')
                img.file_format='PNG'
                img.save()
                node.interpolation='Closest'
    bpy.ops.export_scene.fbx(filepath=str(OUT/f'NR_{name}_Review.fbx'),use_selection=True,object_types={'MESH'},axis_forward='-Z',axis_up='Y',path_mode='COPY',embed_textures=True,add_leaf_bones=False)
    report.append({'name':name,'triangles':len(o.data.polygons),'dimensions_m':list(o.dimensions),'status':'review only; proportions and collision fit unverified'})
    # A comparison display uses a common apparent size, independent of gameplay dimensions.
    o.scale=(1/max(o.dimensions),)*3
    o.location=((i%3)*1.6, -(i//3)*1.7,0)
scene=bpy.context.scene
scene.render.engine='CYCLES'
scene.cycles.samples=24
scene.world.color=(0.35,0.35,0.35)
bpy.ops.object.light_add(type='AREA',location=(1,-2,6))
bpy.context.object.data.energy=1200
bpy.context.object.data.shape='DISK'
bpy.context.object.data.size=6
bpy.ops.object.camera_add(location=(5,-8,6))
camera=bpy.context.object
camera.rotation_euler=(Vector((1.6,-0.8,0.4))-camera.location).to_track_quat('-Z','Y').to_euler()
camera.data.type='ORTHO';camera.data.ortho_scale=6
scene.camera=camera
scene.render.resolution_x=1400;scene.render.resolution_y=1000;scene.render.resolution_percentage=100
scene.render.filepath=str(OUT/'mesh-review.png')
bpy.ops.wm.save_as_mainfile(filepath=str(OUT/'kit-review.blend'))
bpy.ops.render.render(write_still=True)
(OUT/'prepare-report.json').write_text(json.dumps(report,indent=2),encoding='utf-8')
print('TRIPO_KIT_PREPARED',len(report))

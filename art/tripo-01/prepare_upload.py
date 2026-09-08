"""Bake the existing worker into a textured, unrigged T-pose for Tripo."""
import bpy, json, math, struct
from pathlib import Path
from mathutils import Vector, Matrix

OUT = Path(__file__).resolve().parent
ROOT = OUT.parents[1]
bpy.ops.wm.open_mainfile(filepath=str(ROOT / 'art/release-01/source/worker.blend'))
rig = next(o for o in bpy.context.scene.objects if o.type == 'ARMATURE')
rig.animation_data_clear()
for bone in rig.pose.bones:
    for constraint in list(bone.constraints):
        bone.constraints.remove(constraint)
    bone.matrix_basis = Matrix.Identity(4)
bpy.context.view_layer.update()
# Remove isolated weight spikes before baking the old rig into the upload mesh.
for obj in [o for o in bpy.context.scene.objects if o.type == 'MESH']:
    bpy.ops.object.select_all(action='DESELECT')
    obj.select_set(True)
    bpy.context.view_layer.objects.active = obj
    import numpy as np
    count = len(obj.data.vertices)
    weights = np.zeros((count, len(obj.vertex_groups)))
    for vertex in obj.data.vertices:
        for group in vertex.groups:
            weights[vertex.index, group.group] = group.weight
    # glTF duplicates vertices at UV seams; share smoothing across coincident points.
    _, inverse = np.unique(np.round([tuple(v.co) for v in obj.data.vertices], 3), axis=0, return_inverse=True)
    welded = np.zeros((int(inverse.max()) + 1, weights.shape[1]))
    np.add.at(welded, inverse, weights)
    welded /= np.bincount(inverse)[:, None]
    weights = welded
    edges = np.unique(np.sort(inverse[np.array([tuple(edge.vertices) for edge in obj.data.edges])], axis=1), axis=0)
    count = len(weights)
    a, b = edges[:, 0], edges[:, 1]
    degree = np.bincount(np.concatenate((a, b)), minlength=count).clip(1)[:, None]
    for _ in range(16):
        neighbours = np.zeros_like(weights)
        np.add.at(neighbours, a, weights[b])
        np.add.at(neighbours, b, weights[a])
        weights = weights * .2 + neighbours / degree * .8
    weights /= weights.sum(axis=1).clip(.00001)[:, None]
    weights = weights[inverse]
    for group in obj.vertex_groups:
        group.remove(list(range(len(obj.data.vertices))))
    for index, row in enumerate(weights):
        for group_index in np.flatnonzero(row > .00001):
            obj.vertex_groups[int(group_index)].add([index], float(row[group_index]), 'REPLACE')
# Align upper arm, forearm and mitten in armature space, retaining bone scale.
for side, sign in [('Left', 1), ('Right', -1)]:
    for suffix in ['Arm', 'ForeArm', 'Hand']:
        bone = rig.pose.bones[side + suffix]
        matrix = bone.matrix.copy()
        rotation = (matrix.to_3x3() @ Vector((0, 1, 0))).normalized().rotation_difference(Vector((sign, 0, 0)))
        aligned = rotation.to_matrix().to_4x4() @ matrix
        aligned.translation = matrix.translation
        bone.matrix = aligned
        bpy.context.view_layer.update()
meshes = [o for o in bpy.context.scene.objects if o.type == 'MESH']
for obj in meshes:
    bpy.ops.object.select_all(action='DESELECT')
    obj.select_set(True)
    bpy.context.view_layer.objects.active = obj
    for modifier in list(obj.modifiers):
        if modifier.type == 'ARMATURE':
            bpy.ops.object.modifier_apply(modifier=modifier.name)
    transform = obj.matrix_world.copy()
    obj.parent = None
    obj.matrix_world = Matrix.Identity(4)
    obj.data.transform(transform)
    obj.vertex_groups.clear()
    obj.animation_data_clear()
for obj in list(bpy.context.scene.objects):
    if obj.type != 'MESH':
        bpy.data.objects.remove(obj, do_unlink=True)
points = [v.co for obj in meshes for v in obj.data.vertices]
low = Vector(tuple(min(p[i] for p in points) for i in range(3)))
high = Vector(tuple(max(p[i] for p in points) for i in range(3)))
offset = Vector(((low.x + high.x) / 2, (low.y + high.y) / 2, low.z))
scale = 1.65 / (high.z - low.z)
for obj in meshes:
    for vertex in obj.data.vertices:
        vertex.co = (vertex.co - offset) * scale
for action in list(bpy.data.actions):
    bpy.data.actions.remove(action)
bpy.ops.wm.save_as_mainfile(filepath=str(OUT / 'worker-tripo-input.blend'))
glb = OUT / 'worker-tripo-input.glb'
bpy.ops.export_scene.gltf(filepath=str(glb), export_format='GLB', export_animations=False, export_skins=False, export_cameras=False, export_lights=False)
data = glb.read_bytes()
length = struct.unpack_from('<I', data, 12)[0]
document = json.loads(data[20:20 + length])
assert not document.get('skins') and not document.get('animations')
assert document.get('images') and len(data) < 150_000_000
report = {'source': 'art/release-01/source/worker.blend', 'file': glb.name, 'bytes': len(data), 'height_m': 1.65, 'skins': len(document.get('skins', [])), 'animations': len(document.get('animations', [])), 'images': len(document.get('images', [])), 'triangles': sum(len(p.vertices)-2 for obj in meshes for p in obj.data.polygons)}
(OUT / 'upload-validation.json').write_text(json.dumps(report, indent=2), encoding='utf-8')
# Preview only: saved upload source contains no cameras, lights or scenery.
scene = bpy.context.scene
scene.render.engine = 'CYCLES'
scene.cycles.samples = 32
scene.render.resolution_x = 1000
scene.render.resolution_y = 1000
scene.render.resolution_percentage = 100
scene.world.color = (0.18, 0.18, 0.18)
def aim(obj, point):
    obj.rotation_euler = (Vector(point) - obj.location).to_track_quat('-Z', 'Y').to_euler()
bpy.ops.object.camera_add(location=(0, 5, 1.0))
camera = bpy.context.object
aim(camera, (0, 0, 0.85))
camera.data.type = 'ORTHO'
camera.data.ortho_scale = 2.5
scene.camera = camera
for location, energy, size in [((2, 3, 4), 250, 4), ((-3, 1, 2), 160, 3), ((0, -3, 3), 200, 3)]:
    bpy.ops.object.light_add(type='AREA', location=location)
    lamp = bpy.context.object
    lamp.data.energy = energy
    lamp.data.shape = 'DISK'
    lamp.data.size = size
    aim(lamp, (0, 0, .9))
scene.render.filepath = str(OUT / 'worker-tpose-front.png')
bpy.ops.render.render(write_still=True)
camera.location = (4, 3, 1.3)
aim(camera, (0, 0, .85))
scene.render.filepath = str(OUT / 'worker-tpose-angle.png')
bpy.ops.render.render(write_still=True)
print('TRIPO_UPLOAD_READY', json.dumps(report))

"""Inspect the preserved FBX and build a non-destructive Blender review file.

Run: blender --background --factory-startup --disable-autoexec --python-exit-code 1 --python art/player-employee-01/inspect.py
"""
import hashlib
import json
import math
from collections import Counter
from pathlib import Path

import bmesh
import bpy
from mathutils import Vector

ROOT = Path(__file__).resolve().parent
SOURCE = ROOT / 'source' / 'spacesuit+3d+model.fbx'
REVIEW = ROOT / 'review'
PREPARED = ROOT / 'prepared'


def bounds(objects):
    points = [o.matrix_world @ v.co for o in objects for v in o.data.vertices]
    assert points and all(math.isfinite(x) for p in points for x in p)
    low = Vector([min(p[i] for p in points) for i in range(3)])
    high = Vector([max(p[i] for p in points) for i in range(3)])
    return low, high


def inspect(obj):
    mesh = obj.data
    mesh.calc_loop_triangles()
    bm = bmesh.new()
    bm.from_mesh(mesh)
    bm.verts.ensure_lookup_table()
    seen, parts = set(), []
    for vertex in bm.verts:
        if vertex.index in seen:
            continue
        stack = [vertex]
        seen.add(vertex.index)
        vertices = []
        while stack:
            current = stack.pop()
            vertices.append(current)
            for edge in current.link_edges:
                other = edge.other_vert(current)
                if other.index not in seen:
                    seen.add(other.index)
                    stack.append(other)
        points = [obj.matrix_world @ v.co for v in vertices]
        parts.append({'vertices': len(vertices),
                      'world_min': [min(p[i] for p in points) for i in range(3)],
                      'world_max': [max(p[i] for p in points) for i in range(3)]})
    uv = mesh.uv_layers.active
    result = {
        'name': obj.name, 'vertices': len(mesh.vertices), 'edges': len(mesh.edges),
        'faces': len(mesh.polygons), 'face_sizes': dict(Counter(str(len(p.vertices)) for p in mesh.polygons)),
        'triangles': len(mesh.loop_triangles), 'uv_layers': len(mesh.uv_layers),
        'uv_coordinates_finite': bool(uv) and all(math.isfinite(x) for d in uv.data for x in d.uv),
        'uv_min': [min(d.uv[i] for d in uv.data) for i in range(2)] if uv else None,
        'uv_max': [max(d.uv[i] for d in uv.data) for i in range(2)] if uv else None,
        'boundary_edges': sum(e.is_boundary for e in bm.edges),
        'overconnected_edges': sum(len(e.link_faces) > 2 for e in bm.edges),
        'inconsistent_winding_edges': sum(e.is_manifold and not e.is_contiguous for e in bm.edges),
        'exception_edges': [
            {'kind': 'overconnected' if len(e.link_faces) > 2 else 'winding',
             'vertices': [v.index for v in e.verts],
             'world_points': [list(obj.matrix_world @ v.co) for v in e.verts],
             'linked_faces': [f.index for f in e.link_faces]}
            for e in bm.edges
            if len(e.link_faces) > 2 or (e.is_manifold and not e.is_contiguous)],
        'wire_edges': sum(e.is_wire for e in bm.edges),
        'loose_vertices': sum(not v.link_edges for v in bm.verts),
        'zero_area_faces': sum(p.area < 1e-12 for p in mesh.polygons),
        'components': sorted(parts, key=lambda p: p['vertices'], reverse=True),
        'materials': [m.name if m else None for m in mesh.materials],
        'vertex_groups': len(obj.vertex_groups),
        'modifiers': [m.type for m in obj.modifiers],
    }
    bm.free()
    return result


def main():
    (ROOT / 'validation.json').unlink(missing_ok=True)
    provenance = json.loads((ROOT / 'provenance.json').read_text())
    for record in provenance['files']:
        assert hashlib.sha256((ROOT / record['path']).read_bytes()).hexdigest() == record['sha256']
    REVIEW.mkdir(exist_ok=True)
    PREPARED.mkdir(exist_ok=True)
    bpy.ops.wm.read_factory_settings(use_empty=True)
    bpy.ops.import_scene.fbx(filepath=str(SOURCE))
    objects = [o for o in bpy.context.scene.objects if o.type == 'MESH']
    assert len(objects) == 1, 'Review script expects this single supplied mesh'
    model = objects[0]
    source_info = inspect(model)
    low, high = bounds(objects)
    source_low = low.copy()
    source_size = high - low
    assert source_size.z > 0
    images = [i for i in bpy.data.images if i.source == 'FILE']
    assert images and all(i.has_data and min(i.size) > 0 for i in images), 'Missing source textures'
    image_info = [{'name': i.name, 'size': list(i.size),
                   'path': str(Path(bpy.path.abspath(i.filepath)).relative_to(ROOT)),
                   'colorspace': i.colorspace_settings.name} for i in images]
    # Bake only import orientation, uniform scale and placement into the working copy.
    # Preserve every face, seam, material and UV for the next topology review.
    factor = provenance['target_height_m'] / source_size.z
    origin = Vector(((low.x + high.x) / 2, (low.y + high.y) / 2, low.z))
    matrix = model.matrix_world.copy()
    for vertex in model.data.vertices:
        vertex.co = (matrix @ vertex.co - origin) * factor
    model.matrix_world.identity()
    model.name = 'NR_Employee_01'
    model.data.name = 'NR_Employee_01_Mesh'
    model.data.update()
    bpy.context.view_layer.update()
    prepared_info = inspect(model)
    for key in ('vertices', 'edges', 'faces', 'face_sizes', 'triangles', 'uv_layers',
                'boundary_edges', 'overconnected_edges', 'inconsistent_winding_edges'):
        assert prepared_info[key] == source_info[key], f'Unexpected topology change: {key}'
    low, high = bounds(objects)
    assert abs((high - low).z - 1.8) < 1e-5 and abs(low.z) < 1e-5
    assert source_info['uv_coordinates_finite'] and not source_info['zero_area_faces']
    assert all(abs(x - 1) < 1e-6 for x in model.scale)
    for img in images:
        img.pack()
    scene = bpy.context.scene
    scene.unit_settings.system = 'METRIC'
    scene.render.engine = 'CYCLES'
    scene.cycles.samples = 24
    scene.cycles.use_denoising = True
    scene.render.resolution_x = scene.render.resolution_y = 1000
    scene.render.resolution_percentage = 100
    scene.render.image_settings.file_format = 'PNG'
    scene.view_settings.view_transform = 'Standard'
    world = bpy.data.worlds.new('Review Studio')
    world.use_nodes = True
    world.node_tree.nodes['Background'].inputs['Color'].default_value = (.6, .6, .6, 1)
    world.node_tree.nodes['Background'].inputs['Strength'].default_value = .6
    scene.world = world
    center = (low + high) / 2
    for index, offset in enumerate(((2, -3, 3), (-2, 1, 2))):
        bpy.ops.object.light_add(type='AREA', location=center + Vector(offset))
        light = bpy.context.object
        light.name = f'Review Light {index + 1}'
        light.data.energy = 300 if index == 0 else 180
        light.data.shape = 'DISK'
        light.data.size = 4
        light.rotation_euler = (center - light.location).to_track_quat('-Z', 'Y').to_euler()
    bpy.ops.object.camera_add()
    camera = bpy.context.object
    camera.name = 'Review Camera'
    camera.data.type = 'ORTHO'
    camera.data.ortho_scale = max(high - low) * 1.15
    scene.camera = camera
    full_scale = camera.data.ortho_scale
    hand_center = Vector((.85, .02, 1.33))
    for name, focus, offset, scale in (
        ('front', center, (0, -4, 0), full_scale),
        ('back', center, (0, 4, 0), full_scale),
        ('three-quarter', center, (2, -4, .5), full_scale),
        ('hand-top', hand_center, (0, 0, 3), .42),
    ):
        camera.location = focus + Vector(offset)
        camera.rotation_euler = (focus - camera.location).to_track_quat('-Z', 'Y').to_euler()
        camera.data.ortho_scale = scale
        scene.render.filepath = str(REVIEW / (name + '.png'))
        bpy.ops.render.render(write_still=True)
    # Temporary diagnostic copy: actual polygon edges, not shader triangulation.
    diagnostic = model.copy()
    diagnostic.data = model.data.copy()
    scene.collection.objects.link(diagnostic)
    diagnostic.data.materials.clear()
    for name, color in (('Review Clay', (.7, .7, .7, 1)), ('Review Edges', (.01, .01, .01, 1))):
        material = bpy.data.materials.new(name)
        material.use_nodes = True
        material.node_tree.nodes['Principled BSDF'].inputs['Base Color'].default_value = color
        material.node_tree.nodes['Principled BSDF'].inputs['Roughness'].default_value = 1
        diagnostic.data.materials.append(material)
    for polygon in diagnostic.data.polygons:
        polygon.material_index = 0
    wire = diagnostic.modifiers.new('Actual polygon edges', 'WIREFRAME')
    wire.use_replace = False
    wire.thickness = .0008
    wire.offset = 1
    wire.material_offset = 1
    model.hide_render = True
    for name, focus, offset, scale in (
        ('wire-front', center, (0, -4, 0), full_scale),
        ('wire-hand', hand_center, (0, 0, 3), .42),
    ):
        camera.location = focus + Vector(offset)
        camera.rotation_euler = (focus - camera.location).to_track_quat('-Z', 'Y').to_euler()
        camera.data.ortho_scale = scale
        scene.render.filepath = str(REVIEW / (name + '.png'))
        bpy.ops.render.render(write_still=True)
    diagnostic_mesh = diagnostic.data
    diagnostic_materials = list(diagnostic_mesh.materials)
    bpy.data.objects.remove(diagnostic, do_unlink=True)
    bpy.data.meshes.remove(diagnostic_mesh)
    for material in diagnostic_materials:
        bpy.data.materials.remove(material)
    model.hide_render = False
    camera.location = center + Vector((2, -4, .5))
    camera.rotation_euler = (center - camera.location).to_track_quat('-Z', 'Y').to_euler()
    camera.data.ortho_scale = full_scale
    scene.render.filepath = str(REVIEW / 'three-quarter.png')
    bpy.ops.object.select_all(action='DESELECT')
    model.select_set(True)
    bpy.context.view_layer.objects.active = model
    for screen in bpy.data.screens:
        for area in screen.areas:
            if area.type == 'VIEW_3D':
                area.spaces.active.shading.type = 'MATERIAL'
                area.spaces.active.overlay.show_extras = False
                area.spaces.active.region_3d.view_distance = 3.5
                area.spaces.active.region_3d.view_location = center
                area.spaces.active.region_3d.view_rotation = camera.rotation_euler.to_quaternion()
    # Relative image paths plus packed pixels keep the editable copy portable.
    for img in images:
        img.filepath = '//../source/spacesuit+3d+model.fbm/spacesuit+3d+model_basecolor.jpg'
    bpy.ops.wm.save_as_mainfile(filepath=str(PREPARED / 'NR_Employee_01.blend'))
    report = {'blender': bpy.app.version_string, 'source': source_info,
              'source_bounds_m': {'min': list(source_low), 'size': list(source_size)},
              'uniform_scale_factor': factor,
              'prepared': prepared_info,
              'prepared_bounds_m': {'min': list(low), 'max': list(high), 'size': list(high - low)},
              'images': image_info,
              'armatures': sum(o.type == 'ARMATURE' for o in scene.objects),
              'checks': {'source_hashes_match': True, 'topology_counts_unchanged': True,
                         'finite_geometry_and_uvs': True, 'zero_area_faces_absent': True,
                         'target_height_1_8m': True, 'floor_centered': True, 'textures_loaded_and_packed': True},
              'pending': ['User appearance approval and topology repair decisions', 'Joint/hand deformation', 'Rigging',
                          'Unity import/game checks', 'Provider generation parameters and license evidence']}
    bpy.ops.wm.open_mainfile(filepath=str(PREPARED / 'NR_Employee_01.blend'), load_ui=False, use_scripts=False)
    reopened = bpy.data.objects['NR_Employee_01']
    assert inspect(reopened) == prepared_info, 'Saved mesh changed on reload'
    reloaded_low, reloaded_high = bounds([reopened])
    assert (reloaded_low - low).length < 1e-6 and (reloaded_high - high).length < 1e-6
    packed = [i for i in bpy.data.images if i.source == 'FILE']
    # Reopened images decode lazily: read dimensions/pixel count before has_data.
    assert len(packed) == len(image_info)
    assert all(i.packed_file and min(i.size) > 0 and len(i.pixels) > 0 and i.has_data for i in packed)
    assert len([o for o in bpy.context.scene.objects if o.type == 'MESH']) == 1
    report['checks']['saved_blend_reopens_with_mesh_and_packed_texture'] = True
    (ROOT / 'validation.json').write_text(json.dumps(report, indent=2) + '\n')
    print('EMPLOYEE_CHECKS', json.dumps(report['checks']))


if __name__ == '__main__':
    main()

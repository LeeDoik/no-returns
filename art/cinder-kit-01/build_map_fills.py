"""blender --background --python art/cinder-kit-01/build_map_fills.py

Crop approved module geometry/UVs for existing building dimensions; never scale scene instances.
"""
import hashlib
import json
from pathlib import Path

import bmesh
import bpy
from mathutils import Vector

ROOT = Path(__file__).resolve().parent
GAME = ROOT.parents[1] / "NoReturns/Assets/_NoReturns/Art/CinderMapFills01"
GAME.mkdir(parents=True, exist_ok=True)
bpy.ops.wm.open_mainfile(filepath=str(ROOT / "Cinder_Kit_Appearance.blend"))
scene = bpy.context.scene
specs = [("Floor_Half", "Floor_A", (.6, 1.2)), ("Floor_Quarter", "Floor_A", (.6, .6)),
         ("Ceiling_Half", "Ceiling_A", (.6, 1.2)), ("Ceiling_Quarter", "Ceiling_A", (.6, .6)),
         ("Ceiling_Edge_Half", "Ceiling_Edge", (.15, .6))]
specs += [("Wall_Fill" + suffix, "Wall_A", (width, .3))
          for suffix, width in (("005", .05), ("030", .3), ("065", .65))]
records, copies = [], []
for name, source, dimensions in specs:
    original = bpy.data.objects["NR_Cinder_" + source]
    obj = original.copy(); obj.data = original.data.copy()
    scene.collection.objects.link(obj); obj.name = "NR_Cinder_" + name
    ratios = [dimensions[i] / original.dimensions[i] for i in (0, 1)]
    layer = obj.data.uv_layers.active
    for face in obj.data.polygons:
        dominant = max(range(3), key=lambda i: abs(face.normal[i]))
        axes = [i for i in range(3) if i != dominant]
        if source == "Wall_A" and dominant != 2:
            axes = [0 if dominant == 1 else 1, 2]
        for channel, axis in enumerate(axes):
            if axis >= 2: continue
            anchor = min(layer.data[i].uv[channel] for i in face.loop_indices)
            for loop in face.loop_indices:
                old = layer.data[loop].uv[channel]
                layer.data[loop].uv[channel] = anchor + (old - anchor) * ratios[axis]
    for vertex in obj.data.vertices:
        for axis in (0, 1): vertex.co[axis] *= ratios[axis]
    bpy.context.view_layer.update()
    obj.data.calc_loop_triangles()
    bm = bmesh.new(); bm.from_mesh(obj.data)
    assert all(e.is_manifold for e in bm.edges) and bm.calc_volume() > 0, name
    bm.free()
    assert len(obj.data.loop_triangles) == 12 and len(obj.data.materials) == 1, name
    assert all(0 <= value <= 1 for loop in layer.data for value in loop.uv), name
    bounds = [(v.co.x, v.co.z, v.co.y) for v in obj.data.vertices]
    low = [round(min(v[i] for v in bounds), 6) for i in range(3)]
    high = [round(max(v[i] for v in bounds), 6) for i in range(3)]
    bpy.ops.object.select_all(action="DESELECT"); obj.select_set(True)
    bpy.context.view_layer.objects.active = obj
    path = GAME / (obj.name + ".fbx")
    bpy.ops.export_scene.fbx(filepath=str(path), use_selection=True, add_leaf_bones=False,
                             bake_anim=False, axis_forward="-Z", axis_up="Y", apply_scale_options="FBX_SCALE_ALL")
    before = set(scene.objects); bpy.ops.import_scene.fbx(filepath=str(path))
    imported = [o for o in scene.objects if o not in before]
    meshes = [o for o in imported if o.type == "MESH"]
    assert len(meshes) == 1
    copy = meshes[0]; bpy.context.view_layer.update(); copy.data.calc_loop_triangles()
    assert Vector(copy.dimensions - obj.dimensions).length < 1e-5 and copy.location.length < 1e-5, name
    assert copy.data.uv_layers and len(copy.data.loop_triangles) == 12 and len(copy.data.materials) == 1
    assert sorted(tuple(round(v, 5) for v in loop.uv) for loop in copy.data.uv_layers.active.data) == sorted(
        tuple(round(v, 5) for v in loop.uv) for loop in layer.data), name
    for copy in imported: bpy.data.objects.remove(copy, do_unlink=True)
    records.append({"name": obj.name, "source": original.name, "size_m": [round(high[i] - low[i], 6) for i in range(3)],
                    "min_m": low, "triangles": 12, "uv_density_preserved": True, "closed_outward_mesh": True,
                    "fbx_roundtrip": "pass", "sha256": hashlib.sha256(path.read_bytes()).hexdigest()})
    copies.append(obj)
assert len(records) == 8
for obj in list(scene.objects):
    if obj not in copies: bpy.data.objects.remove(obj, do_unlink=True)
bpy.context.preferences.filepaths.save_version = 0
bpy.ops.wm.save_as_mainfile(filepath=str(ROOT / "Cinder_Map_Fills.blend"))
(ROOT / "map-fill-validation.json").write_text(json.dumps({"date": "2026-10-01", "blender_version": bpy.app.version_string,
    "origin": "Cropped approved local geometry and UVs; existing aged atlas, no new generated imagery", "parts": records}, indent=2) + "\n")
print("CINDER MAP FILLS PASS: 8 closed modules, unit pivots, UV density, FBX roundtrip")

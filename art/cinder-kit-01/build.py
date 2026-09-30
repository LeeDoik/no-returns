"""Blender --background --python art/cinder-kit-01/build.py; gray structure only."""
import json
from pathlib import Path

import bmesh
import bpy
from mathutils import Vector

ROOT = Path(__file__).resolve().parent
GAME = ROOT.parents[1] / "NoReturns/Assets/_NoReturns/Art/CinderKit01"
GAME.mkdir(parents=True, exist_ok=True)
bpy.ops.wm.read_factory_settings(use_empty=True)
scene = bpy.context.scene
scene.unit_settings.system = "METRIC"
gray = bpy.data.materials.new("Cinder_Structure_Gray")
gray.diffuse_color = (.48, .48, .48, 1)
gray.use_nodes = True
gray.node_tree.nodes["Principled BSDF"].inputs["Base Color"].default_value = gray.diffuse_color
gray.node_tree.nodes["Principled BSDF"].inputs["Roughness"].default_value = .85


def part(name, boxes, budget):
    # Source coordinates are Unity X,Y,Z metres, stored as Blender X,-Z,Y.
    objects = []
    for center, size in boxes:
        bpy.ops.mesh.primitive_cube_add(size=1, location=(center[0], -center[2], center[1]))
        obj = bpy.context.object
        obj.dimensions = (size[0], size[2], size[1])
        bpy.ops.object.transform_apply(location=False, rotation=False, scale=True)
        objects.append(obj)
    bpy.ops.object.select_all(action="DESELECT")
    for obj in objects:
        obj.select_set(True)
    bpy.context.view_layer.objects.active = objects[0]
    bpy.ops.object.join()
    obj = bpy.context.object
    obj.name = name
    scene.cursor.location = (0, 0, 0)
    bpy.ops.object.origin_set(type="ORIGIN_CURSOR")
    obj.data.materials.append(gray)
    bpy.ops.object.mode_set(mode="EDIT")
    bpy.ops.mesh.select_all(action="SELECT")
    bpy.ops.uv.smart_project(island_margin=.02)
    bpy.ops.object.mode_set(mode="OBJECT")
    obj.data.calc_loop_triangles()
    bm = bmesh.new()
    bm.from_mesh(obj.data)
    assert all(edge.is_manifold for edge in bm.edges), name
    assert all(face.calc_area() > 0 for face in bm.faces), name
    assert bm.calc_volume() > 0, name
    bm.free()
    assert obj.data.uv_layers and len(obj.data.loop_triangles) <= budget, name
    bpy.context.view_layer.update()
    bounds = [(v.co.x, v.co.z, -v.co.y) for v in obj.data.vertices]
    low = [round(min(v[i] for v in bounds), 6) for i in range(3)]
    high = [round(max(v[i] for v in bounds), 6) for i in range(3)]
    record = {"name": name, "min_m": low, "max_m": high,
              "size_m": [round(high[i] - low[i], 6) for i in range(3)],
              "triangles": len(obj.data.loop_triangles), "budget": budget,
              "uv_layers": len(obj.data.uv_layers), "material_slots": len(obj.data.materials),
              "closed_outward_mesh": True, "pivot_m": [0, 0, 0]}
    bpy.ops.export_scene.fbx(filepath=str(GAME / (name + ".fbx")), use_selection=True,
                             add_leaf_bones=False, bake_anim=False,
                             axis_forward="-Z", axis_up="Y",
                             apply_scale_options="FBX_SCALE_ALL")
    return obj, record


specs = [
    ("NR_Cinder_Floor_A", [((0, -.1, 0), (1.2, .2, 1.2))], 200),
    ("NR_Cinder_Wall_A", [((0, 2, 0), (1.2, 4, .3))], 400),
    ("NR_Cinder_Wall_Fill035", [((0, 2, 0), (.35, 4, .3))], 400),
    ("NR_Cinder_Wall_Fill090", [((0, 2, 0), (.9, 4, .3))], 400),
    ("NR_Cinder_DoorFrame_A", [((-1.75, 1.65, 0), (.3, 3.3, .3)),
                                ((1.75, 1.65, 0), (.3, 3.3, .3)),
                                ((0, 3.65, 0), (3.8, .7, .3))], 1200),
    ("NR_Cinder_Ceiling_A", [((0, .15, 0), (1.2, .3, 1.2))], 600),
    ("NR_Cinder_Ceiling_Edge", [((0, .15, 0), (.15, .3, 1.2))], 600),
    ("NR_Cinder_Ceiling_Corner", [((0, .15, 0), (.15, .3, .15))], 600),
    ("NR_Cinder_Beam_A", [((0, .15, 0), (1.2, .3, .3))], 600),
    ("NR_Cinder_Corner_A", [((0, 2, 0), (.3, 4, .3))], 600),
    ("NR_Cinder_End_A", [((0, 2, 0), (.15, 4, .3))], 600),
]
models, records = [], []
for name, boxes, budget in specs:
    obj, record = part(name, boxes, budget)
    models.append(obj)
    records.append(record)
assert records[4]["size_m"] == [3.8, 4.0, .3]
assert specs[4][1][0][0][0] + .15 == -1.6
assert specs[4][1][1][0][0] - .15 == 1.6
assert 3.65 - .35 == 3.3

# Independently reimport every FBX and check size, pivot, UVs and triangle count.
for obj, record in zip(models, records):
    before = set(scene.objects)
    bpy.ops.import_scene.fbx(filepath=str(GAME / (record["name"] + ".fbx")))
    imported = [o for o in scene.objects if o not in before]
    meshes = [o for o in imported if o.type == "MESH"]
    assert len(meshes) == 1
    copy = meshes[0]
    bpy.context.view_layer.update()
    assert all(abs(a - b) < 1e-5 for a, b in zip(copy.dimensions, obj.dimensions)), record["name"]
    assert copy.location.length < 1e-5 and copy.data.uv_layers, record["name"]
    assert len(copy.data.materials) == record["material_slots"]
    copy.data.calc_loop_triangles()
    assert len(copy.data.loop_triangles) == record["triangles"]
    record["fbx_roundtrip"] = "pass"
    for copy in imported:
        bpy.data.objects.remove(copy, do_unlink=True)

report = {"status": "gray structural candidate; user approval pending", "units": "metres",
          "origin": "locally authored Blender primitives; no external model or generated texture",
          "blender_version": bpy.app.version_string, "production_units": 5,
          "fbx_count": len(records), "door_clear_m": [3.2, 3.3], "parts": records}
(ROOT / "validation.json").write_text(json.dumps(report, indent=2) + "\n")
bpy.context.preferences.filepaths.save_version = 0
bpy.ops.wm.save_as_mainfile(filepath=str(ROOT / "Cinder_Kit_Structure.blend"))

# Model sheet is rendered from the same source geometry, not a concept image.
for i, obj in enumerate(models):
    obj.location = ((i % 4) * 4.5, -(i // 4) * 4.5, 0)
scene.world = bpy.data.worlds.new("Review world")
scene.world.color = (.25, .25, .25)
for position in [(5, 3, 12), (10, -10, 10)]:
    bpy.ops.object.light_add(type="AREA", location=position)
    light = bpy.context.object
    light.data.energy = 2400
    light.data.size = 10
    light.rotation_euler = (Vector((6, -4, 1)) - light.location).to_track_quat("-Z", "Y").to_euler()
bpy.ops.object.camera_add(location=(20, 12, 15))
camera = bpy.context.object
camera.rotation_euler = (Vector((6.5, -4, 1.5)) - camera.location).to_track_quat("-Z", "Y").to_euler()
camera.data.type = "ORTHO"
camera.data.ortho_scale = 23
scene.camera = camera
scene.render.engine = "CYCLES"
scene.cycles.samples = 16
scene.render.resolution_x = 1400
scene.render.resolution_y = 1000
scene.render.resolution_percentage = 100
scene.render.filepath = str(ROOT / "structure-sheet.png")
bpy.ops.render.render(write_still=True)
print("CINDER STRUCTURE PASS: 11 FBX, dimensions, closed normals, UVs and roundtrip")

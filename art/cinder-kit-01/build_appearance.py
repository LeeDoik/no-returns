"""blender --background --python art/cinder-kit-01/build_appearance.py

Direct geometry and imagegen-aged texture sources; provenance in aged-texture-provenance.json.
Append -- --surfaces-only to preserve existing verified FBX bytes during a surface revision.
The gray source and exports are never overwritten.
"""
import hashlib
import json
import sys
from pathlib import Path

import bmesh
import bpy
from mathutils import Vector

ROOT = Path(__file__).resolve().parent
GAME = ROOT.parents[1] / "NoReturns/Assets/_NoReturns/Art/CinderAppearance01"
GAME.mkdir(parents=True, exist_ok=True)


# Native Blender resampling changes resolution only; the source edits are imagegen outputs.
for source, name, size in (("aged-atlas-source.png", "Cinder_Surface_Atlas.png", (512, 512)),
                            ("aged-sign-source.png", "Cinder_Warehouse_Sign.png", (256, 128))):
    image = bpy.data.images.load(str(ROOT / source))
    assert abs(image.size[0] / image.size[1] - size[0] / size[1]) < .01, source
    image.scale(*size)
    image.filepath_raw = str(GAME / name)
    image.file_format = "PNG"
    image.save()
    assert tuple(image.size) == size, source

# UV regions remain identical to the verified first appearance pass.
regions = {"wall": (0, 0, 256, 512), "floor": (256, 0, 256, 256),
           "ceiling": (256, 256, 128, 128), "graphite": (384, 256, 128, 128),
           "rust": (256, 384, 128, 128)}

bpy.ops.wm.open_mainfile(filepath=str(ROOT / "Cinder_Kit_Structure.blend"))
scene = bpy.context.scene
surface = bpy.data.materials.new("Cinder_Surface")
surface.use_nodes = True
node = surface.node_tree.nodes.new("ShaderNodeTexImage")
node.image = bpy.data.images.load(str(GAME / "Cinder_Surface_Atlas.png"))
node.interpolation = "Closest"
surface.node_tree.links.new(node.outputs["Color"], surface.node_tree.nodes["Principled BSDF"].inputs["Base Color"])
surface.node_tree.nodes["Principled BSDF"].inputs["Roughness"].default_value = .95
signmat = surface.copy(); signmat.name = "Cinder_Sign"
signmat.node_tree.nodes.get(node.name).image = bpy.data.images.load(str(GAME / "Cinder_Warehouse_Sign.png"))
emission = bpy.data.materials.new("Cinder_Lamp_Emission"); emission.use_nodes = True
bsdf = emission.node_tree.nodes["Principled BSDF"]
bsdf.inputs["Base Color"].default_value = (1, .65, .18, 1)
bsdf.inputs["Emission Color"].default_value = (1, .52, .12, 1)
bsdf.inputs["Emission Strength"].default_value = 2
models = list(o for o in scene.objects if o.type == "MESH")


def uv(obj, kind):
    mesh = obj.data
    layer = mesh.uv_layers.active or mesh.uv_layers.new()
    low = [min(v.co[i] for v in mesh.vertices) for i in range(3)]
    high = [max(v.co[i] for v in mesh.vertices) for i in range(3)]
    for face in mesh.polygons:
        dominant = max(range(3), key=lambda i: abs(face.normal[i]))
        axes = [i for i in range(3) if i != dominant]
        for loop in face.loop_indices:
            p = mesh.vertices[mesh.loops[loop].vertex_index].co
            if kind == "wall" and dominant != 2:
                horizontal = 0 if dominant == 1 else 1
                u = (p[horizontal] - low[horizontal]) / 1.2
                v = p.z / 4
            elif kind == "sign" and dominant == 1 and face.normal.y > 0:
                layer.data[loop].uv = (max(0, min(1, .5 - p.x / 1.2)), max(0, min(1, .5 + p.z / .6)))
                continue
            else:
                u, v = [(p[a] - low[a]) / max(.00001, high[a] - low[a]) for a in axes]
            region = regions[kind if kind != "sign" else "graphite"]
            x, y, w, h = region
            layer.data[loop].uv = ((x + 1 + u * (w - 2)) / 512,
                                   1 - (y + h - 1 - v * (h - 2)) / 512)


for obj in models:
    obj.data.materials.clear(); obj.data.materials.append(surface)
    kind = "wall" if "Wall" in obj.name else "floor" if "Floor" in obj.name else "ceiling" if "Ceiling" in obj.name else "graphite"
    uv(obj, kind)


def prop(name, boxes):
    pieces = []
    for center, size, kind in boxes:
        # Unity's imported handedness maps Blender Y to Unity +Z in this pipeline.
        bpy.ops.mesh.primitive_cube_add(size=1, location=(center[0], center[2], center[1]))
        obj = bpy.context.object; obj.dimensions = (size[0], size[2], size[1])
        bpy.ops.object.transform_apply(location=False, rotation=False, scale=True)
        obj.data.materials.append(emission if kind == "emission" else surface)
        if kind == "sign":
            obj.data.materials.append(signmat)
            for face in obj.data.polygons:
                if face.normal.y > 0: face.material_index = 1
        uv(obj, "graphite" if kind == "emission" else kind)
        pieces.append(obj)
    bpy.ops.object.select_all(action="DESELECT")
    for obj in pieces: obj.select_set(True)
    bpy.context.view_layer.objects.active = pieces[0]
    if len(pieces) > 1: bpy.ops.object.join()
    obj = bpy.context.object; obj.name = name
    scene.cursor.location = (0, 0, 0); bpy.ops.object.origin_set(type="ORIGIN_CURSOR")
    models.append(obj)


prop("NR_Cinder_Lamp_A", [((0, 0, .08), (.6, .2, .16), "graphite"),
                            ((0, 0, .17), (.5, .12, .02), "emission")])
prop("NR_Cinder_Sign_A", [((0, 0, .01), (1.2, .6, .02), "sign")])
rack = [((x, 1.2, z), (.09, 2.4, .09), "rust") for x in (-1.155, 1.155) for z in (-.255, .255)]
rack += [((0, y, 0), (2.22, .08, .6), "graphite") for y in (.22, 1.32)]
rack += [((0, 2.34, -.255), (2.22, .08, .06), "rust")]
prop("NR_Cinder_Rack_A", rack)

records = []
for obj in models:
    bpy.ops.object.select_all(action="DESELECT"); obj.select_set(True)
    mesh = obj.data; mesh.calc_loop_triangles()
    bm = bmesh.new(); bm.from_mesh(mesh)
    assert all(e.is_manifold for e in bm.edges) and all(f.calc_area() > 0 for f in bm.faces), obj.name
    assert bm.calc_volume() > 0; bm.free()
    assert all(0 <= t <= 1 for loop in mesh.uv_layers.active.data for t in loop.uv), obj.name
    bounds = [(v.co.x, v.co.z, v.co.y) for v in mesh.vertices]
    low = [round(min(v[i] for v in bounds), 6) for i in range(3)]
    high = [round(max(v[i] for v in bounds), 6) for i in range(3)]
    path = GAME / (obj.name + ".fbx")
    if "--surfaces-only" not in sys.argv:
        bpy.ops.export_scene.fbx(filepath=str(path), use_selection=True, add_leaf_bones=False, bake_anim=False,
                                 axis_forward="-Z", axis_up="Y", apply_scale_options="FBX_SCALE_ALL")
    before = set(scene.objects); bpy.ops.import_scene.fbx(filepath=str(path))
    imported = [o for o in scene.objects if o not in before]
    copies = [o for o in imported if o.type == "MESH"]
    assert len(copies) == 1
    copy = copies[0]; copy.data.calc_loop_triangles(); bpy.context.view_layer.update()
    assert Vector(copy.dimensions - obj.dimensions).length < 1e-5 and copy.location.length < 1e-5
    assert copy.data.uv_layers and len(copy.data.materials) == len(mesh.materials)
    assert len(copy.data.loop_triangles) == len(mesh.loop_triangles)
    for copy in imported: bpy.data.objects.remove(copy, do_unlink=True)
    records.append({"name": obj.name, "min_m": low, "max_m": high,
                    "size_m": [round(high[i] - low[i], 6) for i in range(3)], "triangles": len(mesh.loop_triangles),
                    "materials": [m.name for m in mesh.materials], "closed_outward_mesh": True, "fbx_roundtrip": "pass"})
assert len(records) == 14
for name, size in (("Lamp", [.6, .2, .18]), ("Sign", [1.2, .6, .02]), ("Rack", [2.4, 2.4, .6])):
    assert next(r for r in records if name in r["name"])["size_m"] == size
for image in (node.image, signmat.node_tree.nodes.get(node.name).image): image.pack()
bpy.context.preferences.filepaths.save_version = 0
bpy.ops.wm.save_as_mainfile(filepath=str(ROOT / "Cinder_Kit_Appearance.blend"))
report = {"date": "2026-10-01", "origin": "direct local Blender geometry; built-in imagegen aged texture edits, native Blender resolution normalization; no concept cropping",
          "texture_provenance": "art/cinder-kit-01/aged-texture-provenance.json",
          "blender_version": bpy.app.version_string, "user_concept_approved": True, "user_final_appearance_review": False,
          "wall_stripe_target_y_m": [1.10, 1.75], "rack_shelf_center_y_m": [.22, 1.32], "parts": records,
          "files": [{"path": str(p.relative_to(ROOT.parents[1])), "sha256": hashlib.sha256(p.read_bytes()).hexdigest(), "bytes": p.stat().st_size}
                    for p in sorted(GAME.glob("*.fbx")) + sorted(GAME.glob("*.png")) + [ROOT / "Cinder_Kit_Appearance.blend"]]}
(ROOT / "production-validation.json").write_text(json.dumps(report, indent=2) + "\n")
print("CINDER APPEARANCE PASS: 14 FBX, 2 imagegen-aged textures, size/pivot/UV/closed normals/FBX roundtrip")

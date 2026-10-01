"""blender --background --python art/cinder-kit-01/build_appearance.py

Directly authored geometry/paint; approved concepts are references, not textures.
The gray source and exports are never overwritten.
"""
import hashlib
import json
import random
import struct
import zlib
from pathlib import Path

import bmesh
import bpy
from mathutils import Vector

ROOT = Path(__file__).resolve().parent
GAME = ROOT.parents[1] / "NoReturns/Assets/_NoReturns/Art/CinderAppearance01"
GAME.mkdir(parents=True, exist_ok=True)


def png(path, width, height, pixels):
    def chunk(kind, data):
        return struct.pack(">I", len(data)) + kind + data + struct.pack(">I", zlib.crc32(kind + data))
    rows = b"".join(b"\0" + bytes(pixels[y * width * 3:(y + 1) * width * 3]) for y in range(height))
    path.write_bytes(b"\x89PNG\r\n\x1a\n" + chunk(b"IHDR", struct.pack(">IIBBBBB", width, height, 8, 2, 0, 0, 0))
                     + chunk(b"IDAT", zlib.compress(rows, 9)) + chunk(b"IEND", b""))


def paint(width, height, color):
    pixels = list(color) * (width * height)
    def rect(x, y, w, h, rgb):
        for row in range(max(0, y), min(height, y + h)):
            for col in range(max(0, x), min(width, x + w)):
                offset = (row * width + col) * 3
                pixels[offset:offset + 3] = rgb
    return pixels, rect


atlas, rect = paint(512, 512, (57, 56, 53))
rng = random.Random(41)
# Atlas regions use image coordinates: wall, floor, ceiling, graphite, rust.
regions = {"wall": (0, 0, 256, 512), "floor": (256, 0, 256, 256),
           "ceiling": (256, 256, 128, 128), "graphite": (384, 256, 128, 128),
           "rust": (256, 384, 128, 128)}
colors = {"wall": (181, 172, 151), "floor": (88, 86, 79),
          "ceiling": (162, 155, 137), "graphite": (57, 56, 53), "rust": (130, 58, 37)}
for kind, (x, y, w, h) in regions.items():
    for row in range(y, y + h, 8):
        for col in range(x, x + w, 8):
            noise = rng.choice((-2, -1, 0, 0, 0, 1, 2))
            rect(col, row, 8, 8, tuple(c + noise for c in colors[kind]))
# Wall stripe Y=1.10..1.75m, apron Y=0..0.22m; shared by every wall width.
rect(0, 288, 256, 83, colors["rust"])
rect(0, 484, 256, 28, (66, 64, 57))
rect(0, 152, 256, 2, (115, 108, 94))
rect(0, 0, 2, 512, (119, 113, 100))
rect(254, 0, 2, 512, (119, 113, 100))
for _ in range(70):
    rect(rng.randrange(3, 250), rng.randrange(473, 484), rng.randrange(2, 6), rng.randrange(2, 6), (135, 112, 83))
for y in (158, 470):
    for x in (8, 245):
        rect(x, y, 3, 3, (94, 88, 77))
for x, y, w, h in (regions["floor"], regions["ceiling"]):
    rect(x, y, w, 2, (55, 53, 48))
    rect(x, y, 2, h, (55, 53, 48))
    rect(x + w - 2, y, 2, h, (100, 96, 86))
    rect(x, y + h - 2, w, 2, (100, 96, 86))
rect(386, 258, 124, 2, (94, 85, 68))
rect(386, 380, 124, 2, (81, 74, 62))
png(GAME / "Cinder_Surface_Atlas.png", 512, 512, atlas)

sign, rect = paint(256, 128, (181, 172, 151))
rect(0, 0, 256, 3, (106, 93, 73)); rect(0, 125, 256, 3, (106, 93, 73))
rect(0, 0, 3, 128, (106, 93, 73)); rect(253, 0, 3, 128, (106, 93, 73))
for x in (8, 244):
    for y in (8, 117):
        rect(x, y, 4, 4, (63, 60, 53))
# Original 5x7 pixel lettering avoids an external font/license dependency.
glyphs = {
    "W": ("10001", "10001", "10001", "10101", "10101", "11011", "10001"),
    "A": ("01110", "10001", "10001", "11111", "10001", "10001", "10001"),
    "R": ("11110", "10001", "10001", "11110", "10100", "10010", "10001"),
    "E": ("11111", "10000", "10000", "11110", "10000", "10000", "11111"),
    "H": ("10001", "10001", "10001", "11111", "10001", "10001", "10001"),
    "O": ("01110", "10001", "10001", "10001", "10001", "10001", "01110"),
    "U": ("10001", "10001", "10001", "10001", "10001", "10001", "01110"),
    "S": ("01111", "10000", "10000", "01110", "00001", "00001", "11110"),
}
for index, letter in enumerate("WAREHOUSE"):
    for y, row in enumerate(glyphs[letter]):
        for x, value in enumerate(row):
            if value == "1": rect(48 + index * 18 + x * 3, 29 + y * 3, 3, 3, (45, 44, 41))
rect(48, 81, 145, 10, (45, 44, 41))
for y in range(66, 106):
    rect(189, y, 21 - abs(y - 86), 1, (45, 44, 41))
png(GAME / "Cinder_Warehouse_Sign.png", 256, 128, sign)

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
report = {"date": "2026-10-01", "origin": "direct local Blender geometry and deterministic original pixel paint; no concept cropping or external font",
          "blender_version": bpy.app.version_string, "user_concept_approved": True, "user_final_appearance_review": False,
          "wall_stripe_y_m": [1.10, 1.75], "rack_shelf_center_y_m": [.22, 1.32], "parts": records,
          "files": [{"path": str(p.relative_to(ROOT.parents[1])), "sha256": hashlib.sha256(p.read_bytes()).hexdigest(), "bytes": p.stat().st_size}
                    for p in sorted(GAME.glob("*.fbx")) + sorted(GAME.glob("*.png")) + [ROOT / "Cinder_Kit_Appearance.blend"]]}
(ROOT / "production-validation.json").write_text(json.dumps(report, indent=2) + "\n")
print("CINDER APPEARANCE PASS: 14 FBX, 2 original textures, size/pivot/UV/closed normals/FBX roundtrip")

"""Round-trip verification, independent of the generation scene."""
import bpy,json,math
from pathlib import Path
R=Path(__file__).resolve().parent/'interior-blockout-05'
expected=json.loads((R/'validation.json').read_text())
bpy.ops.object.select_all(action='SELECT');bpy.ops.object.delete(use_global=False)
bpy.ops.import_scene.gltf(filepath=str(R/'Flatbed_InteriorTrial.glb'))
meshes=[o for o in bpy.context.scene.objects if o.type=='MESH']
for o in meshes:o.data.calc_loop_triangles()
result={'expected_triangles':expected['triangles'],'reimport_triangles':sum(len(o.data.loop_triangles) for o in meshes),'missing_uvs':[o.name for o in meshes if not o.data.uv_layers],'nonfinite_vertices':sum(not all(math.isfinite(c) for c in v.co) for o in meshes for v in o.data.vertices),'separate_screen':any(o.name=='Trial_DisplaySurface' for o in meshes),'separate_ramp':any(o.name=='Trial_ExteriorRamp' for o in meshes)}
assert result['expected_triangles']==result['reimport_triangles'] and not result['missing_uvs'] and not result['nonfinite_vertices'] and result['separate_screen'] and result['separate_ramp']
(R/'roundtrip.json').write_text(json.dumps(result,indent=2))
print('ROUNDTRIP PASS',json.dumps(result))

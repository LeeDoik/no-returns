import bpy, json
from pathlib import Path
root=Path(__file__).resolve().parent/'prepared'
results=[]
expected={'Wall':1500,'Corner':2500,'Door':3000,'Floor':1000,'Lamp':800,'Rack':4000}
for p in sorted(root.glob('*.fbx')):
    bpy.ops.object.select_all(action='SELECT')
    bpy.ops.object.delete(use_global=False)
    bpy.ops.import_scene.fbx(filepath=str(p))
    meshes=[o for o in bpy.context.scene.objects if o.type=='MESH']
    faces=sum(len(o.data.polygons) for o in meshes)
    assert faces==expected[p.stem.split('_')[1]]
    assert all(o.data.uv_layers for o in meshes)
    results.append(dict(file=p.name,meshes=len(meshes),faces=faces,uv=True))
(root/'fbx-import-validation.json').write_text(json.dumps(results,indent=2),encoding='utf-8')
print('FBX_ROUNDTRIP_PASS',len(results))

import bpy,json
from pathlib import Path
OUT=Path(__file__).resolve().parent
bpy.ops.wm.open_mainfile(filepath=str(OUT/'sneezer.blend'))
for o in list(bpy.data.objects):
    if not o.name.startswith('PREVIEW_'):bpy.data.objects.remove(o,do_unlink=True)
bpy.ops.import_scene.gltf(filepath=str(OUT/'sneezer.glb'))
scene=bpy.context.scene
result={}
for frame,state in [(1,'idle'),(30,'warning'),(48,'sneeze')]:
    scene.frame_set(frame)
    scales={n:list(bpy.data.objects['Face_'+n].scale) for n in ['Idle','Warning','Sneeze']}
    assert scales[state.capitalize()][0]>.99,(frame,scales)
    assert sum(s[0]>.5 for s in scales.values())==1,(frame,scales)
    result[state]={'face_scales':scales,'lid_angle':list(bpy.data.objects['Lid_front_hinged'].matrix_basis.to_euler())}
assert result['sneeze']['lid_angle'][0]>.2
scene.render.filepath=str(OUT/'previews/glb-reimport-sneeze.png')
bpy.ops.render.render(write_still=True)
(OUT/'import-validation.json').write_text(json.dumps(result,indent=2),encoding='utf-8')
print('IMPORT_VALIDATED: three exclusive facial states and lid animation.')

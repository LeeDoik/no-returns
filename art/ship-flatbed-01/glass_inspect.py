import bpy,json
from pathlib import Path
R=Path(__file__).resolve().parent
bpy.ops.wm.open_mainfile(filepath=str(R/'Flatbed_Integrated.blend'))
o=bpy.data.objects['Tripo_Hull_Reworked']
fs=[f for f in o.data.polygons if f.material_index==1]
print('GLASSBOUNDS',len(fs),[[min(o.data.vertices[i].co[k] for f in fs for i in f.vertices),max(o.data.vertices[i].co[k] for f in fs for i in f.vertices)] for k in range(3)])
print([(f.index, [round(a,2) for a in f.center]) for f in fs])

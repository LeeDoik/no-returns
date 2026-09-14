import bpy,json
from pathlib import Path
from mathutils import Vector
r=Path(__file__).resolve().parent
bpy.ops.wm.open_mainfile(filepath=str(r/'Flatbed_Angular.blend'))
out={}
for o in bpy.context.scene.objects:
 if o.type!='MESH':continue
 pts=[o.matrix_world@v.co for v in o.data.vertices]
 if not pts:continue
 out[o.name]={'min':[min(p[i] for p in pts) for i in range(3)],'max':[max(p[i] for p in pts) for i in range(3)]}
 if o.name=='Tripo_Hull_Reworked':
  vs={i for f in o.data.polygons if f.material_index==1 for i in f.vertices}
  p=[o.matrix_world@o.data.vertices[i].co for i in vs]
  out['GLASS']={'min':[min(v[i] for v in p) for i in range(3)],'max':[max(v[i] for v in p) for i in range(3)]}
(r/'interior-probe.json').write_text(json.dumps(out,indent=2))
from mathutils.bvhtree import BVHTree
h=bpy.data.objects['Tripo_Hull_Reworked'];tree=BVHTree.FromPolygons([h.matrix_world@v.co for v in h.data.vertices],[list(f.vertices) for f in h.data.polygons])
print('ROOF',[(x,y,tuple(tree.ray_cast(Vector((x,y,2.6)),Vector((0,0,1)))[0] or Vector((0,0,0)))) for y in [-3,-1,1,3] for x in [-1.6,0,1.6]])

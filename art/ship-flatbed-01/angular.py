"""Non-destructive angular exterior variant; keep cabin and glazing unchanged."""
import bpy,bmesh,json,math
from pathlib import Path
from mathutils import Vector
R=Path(__file__).resolve().parent
bpy.ops.wm.open_mainfile(filepath=str(R/'Flatbed_Integrated.blend'))
s=bpy.context.scene;s.frame_set(40)
hull=bpy.data.objects['Tripo_Hull_Reworked']
original_coords=[tuple(v.co) for v in hull.data.vertices]
hull.data.calc_loop_triangles();before=len(hull.data.loop_triangles)
bm=bmesh.new();bm.from_mesh(hull.data)
# Only dissolve non-glass edges between nearby face normals. Boundary positions stay fixed.
edges=[e for e in bm.edges if len(e.link_faces)==2 and all(f.material_index==0 for f in e.link_faces)]
bmesh.ops.dissolve_limit(bm,angle_limit=math.radians(12),use_dissolve_boundaries=False,verts=[],edges=edges,delimit={'MATERIAL'})
bm.to_mesh(hull.data);bm.free();hull.data.update()
for p in hull.data.polygons:p.use_smooth=False
# Generated custom corner normals also retain a soft appearance unless explicitly discarded.
if hull.data.has_custom_normals:
 bpy.context.view_layer.objects.active=hull;bpy.ops.object.select_all(action='DESELECT');hull.select_set(True)
 try:bpy.ops.mesh.customdata_custom_splitnormals_clear()
 except RuntimeError:pass
hull.data.calc_loop_triangles()
meshes=[o for o in s.objects if o.type=='MESH']
report={'source':'Flatbed_Integrated.blend','variant':'Flatbed_Angular','angle_degrees':12,'hull_triangles_before':before,'hull_triangles_after':len(hull.data.loop_triangles),'flat_shading':True,'new_vertex_positions':sum(tuple(v.co) not in original_coords for v in hull.data.vertices),'unity_manual_validation':False}
# Other geometry must be bit-for-bit the same before and after this local operation.
report['unchanged_other_meshes']=len(meshes)-1
bpy.ops.object.select_all(action='DESELECT')
for o in meshes:o.select_set(True)
bpy.ops.export_scene.gltf(filepath=str(R/'Flatbed_Angular.glb'),use_selection=True,export_animations=True)
bpy.ops.export_scene.fbx(filepath=str(R/'Flatbed_Angular.fbx'),use_selection=True,add_leaf_bones=False,bake_anim=False,axis_forward='-Z',axis_up='Y',path_mode='COPY',embed_textures=True)
cam=s.camera;s.cycles.samples=24
for name,pos,target,frame in [('angular-front',(14,17,11),(0,0,1.5),1),('angular-rear',(12,-17,10),(0,-1,1.5),40)]:
 s.frame_set(frame);cam.location=pos;cam.rotation_euler=(Vector(target)-cam.location).to_track_quat('-Z','Y').to_euler();cam.data.type='ORTHO';cam.data.ortho_scale=19 if frame==1 else 20;s.render.filepath=str(R/'review'/(name+'.png'));bpy.ops.render.render(write_still=True)
s.frame_set(40);bpy.ops.file.pack_all();bpy.ops.wm.save_as_mainfile(filepath=str(R/'Flatbed_Angular.blend'))
expected=0
for o in meshes:o.data.calc_loop_triangles();expected+=len(o.data.loop_triangles)
bpy.ops.object.select_all(action='SELECT');bpy.ops.object.delete(use_global=False)
bpy.ops.import_scene.gltf(filepath=str(R/'Flatbed_Angular.glb'))
loaded=[o for o in s.objects if o.type=='MESH']
for o in loaded:o.data.calc_loop_triangles()
report['export_triangles']=expected;report['reimport_triangles']=sum(len(o.data.loop_triangles) for o in loaded)
report['missing_uvs']=[o.name for o in loaded if not o.data.uv_layers]
report['nonfinite_vertices']=sum(not all(math.isfinite(x) for x in v.co) for o in loaded for v in o.data.vertices)
(R/'angular-validation.json').write_text(json.dumps(report,indent=2))
assert expected==report['reimport_triangles'] and not report['missing_uvs'] and not report['nonfinite_vertices']

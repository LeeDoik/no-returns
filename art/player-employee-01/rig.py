"""Local FK deformation trial; preserve the supplied mesh and edited review file.

blender --background --factory-startup --disable-autoexec --python-exit-code 1 --python art/player-employee-01/rig.py
"""
import hashlib
import importlib.util
import json
import math
from pathlib import Path

import bmesh
import bpy
from mathutils import Quaternion, Vector, geometry
from mathutils.bvhtree import BVHTree

ROOT = Path(__file__).resolve().parent
OUT = ROOT / 'rigged'
REVIEW = OUT / 'review'
spec = importlib.util.spec_from_file_location('employee_inspection', ROOT / 'inspect.py')
inspection = importlib.util.module_from_spec(spec)
spec.loader.exec_module(inspection)


def components(mesh):
    adjacent = {v.index: set() for v in mesh.vertices}
    for edge in mesh.edges:
        a, b = edge.vertices
        adjacent[a].add(b)
        adjacent[b].add(a)
    unseen = set(adjacent)
    result = []
    while unseen:
        start = unseen.pop()
        part, stack = [start], [start]
        while stack:
            for other in adjacent[stack.pop()]:
                if other in unseen:
                    unseen.remove(other)
                    part.append(other)
                    stack.append(other)
        result.append(part)
    return sorted(result, key=len, reverse=True)


def main():
    OUT.mkdir(exist_ok=True)
    REVIEW.mkdir(exist_ok=True)
    (OUT / 'validation.json').unlink(missing_ok=True)
    source = ROOT / 'prepared/NR_Employee_01.blend'
    source_hash = hashlib.sha256(source.read_bytes()).hexdigest()
    bpy.ops.wm.open_mainfile(filepath=str(source), use_scripts=False)
    model = bpy.data.objects['NR_Employee_01']
    assert not model.vertex_groups and not any(o.type == 'ARMATURE' for o in bpy.context.scene.objects)
    before = inspection.inspect(model)
    assert (before['vertices'], before['faces']) == (4759, 4888)
    assert model.matrix_world.is_identity
    # The raised strap and underlying boot share two invalid junction edges.
    # Split just those junctions: retain every surface and UV, including the underlay.
    bm = bmesh.new()
    bm.from_mesh(model.data)
    original_faces = [(tuple(tuple(model.data.vertices[v].co) for v in p.vertices),
                       tuple(tuple(model.data.uv_layers.active.data[i].uv) for i in p.loop_indices))
                      for p in model.data.polygons]
    junctions = [e for e in bm.edges if len(e.link_faces)>2 or (e.is_manifold and not e.is_contiguous)]
    assert len(junctions)==2
    junction_vertices = [sorted(v.index for v in e.verts) for e in junctions]
    bmesh.ops.split_edges(bm, edges=junctions)
    bm.to_mesh(model.data)
    bm.free()
    model.data.update()
    cleaned = inspection.inspect(model)
    assert cleaned['overconnected_edges'] == cleaned['inconsistent_winding_edges'] == 0
    assert (cleaned['vertices'], cleaned['faces'], cleaned['triangles']) == (4765,4888,9118)
    for polygon, original in zip(model.data.polygons,original_faces):
        assert original == (tuple(tuple(model.data.vertices[v].co) for v in polygon.vertices),
                            tuple(tuple(model.data.uv_layers.active.data[i].uv) for i in polygon.loop_indices))
    parts = components(model.data)
    assert len(parts) == 21
    bpy.ops.object.select_all(action='DESELECT')
    bpy.ops.object.armature_add(enter_editmode=True)
    rig = bpy.context.object
    rig.name = 'NR_Employee_Rig'
    rig.data.name = 'NR_Employee_Skeleton'
    rig.data.edit_bones.remove(rig.data.edit_bones[0])
    def bone(name, head, tail, parent=None, deform=True):
        b = rig.data.edit_bones.new(name)
        b.head, b.tail = head, tail
        b.use_deform = deform
        b.align_roll(Vector((0, -1, 0)) if abs((b.tail-b.head).normalized().z) > .9 else Vector((0, 0, 1)))
        if parent:
            b.parent = rig.data.edit_bones[parent]
        return b
    bone('Root', (0, 0, 0), (0, 0, .12), deform=False)
    bone('Hips', (0, .015, .91), (0, .015, 1.055), 'Root')
    bone('Spine', (0, .015, 1.055), (0, .015, 1.20), 'Hips')
    bone('Chest', (0, .015, 1.20), (0, .015, 1.33), 'Spine')
    bone('UpperChest', (0, .015, 1.33), (0, .015, 1.45), 'Chest')
    bone('Neck', (0, .015, 1.45), (0, .015, 1.54), 'UpperChest')
    bone('Head', (0, .015, 1.54), (0, .015, 1.78), 'Neck')
    finger_points = {
        'Index': [( .838,-.035,1.332),(.899,-.035,1.328),(.944,-.035,1.31),(.977,-.032,1.296)],
        'Middle':[( .85,.012,1.337),(.91,.012,1.33),(.955,.012,1.307),(.985,.012,1.297)],
        'Ring':[( .85,.057,1.334),(.91,.057,1.326),(.951,.057,1.305),(.979,.057,1.297)],
        'Little':[( .838,.097,1.336),(.893,.097,1.316),(.931,.097,1.299),(.955,.097,1.295)],
        'Thumb':[( .794,-.021,1.326),(.822,-.053,1.307),(.866,-.065,1.28),(.902,-.059,1.278)],
    }
    for side, sign in (('Left', 1), ('Right', -1)):
        def p(x,y,z): return (sign*x,y,z)
        bone(side+'Shoulder',p(.075,.015,1.411),p(.255,.015,1.375),'UpperChest')
        bone(side+'UpperArm',p(.255,.015,1.375),p(.482,.015,1.349),side+'Shoulder')
        bone(side+'LowerArm',p(.482,.015,1.349),p(.73,.015,1.365),side+'UpperArm')
        bone(side+'Hand',p(.73,.015,1.365),p(.838,.025,1.337),side+'LowerArm')
        for finger, points in finger_points.items():
            for index in range(3):
                bone(f'{side}{finger}{index+1}', p(*points[index]),p(*points[index+1]),
                     side+'Hand' if index==0 else f'{side}{finger}{index}')
        bone(side+'UpperLeg',p(.143,.022,.929),p(.218,-.006,.523),'Hips')
        bone(side+'LowerLeg',p(.218,-.006,.523),p(.282,.045,.17),side+'UpperLeg')
        bone(side+'Foot',p(.282,.045,.17),p(.285,-.12,.075),side+'LowerLeg')
        bone(side+'Toes',p(.285,-.12,.075),p(.285,-.19,.065),side+'Foot')
    bpy.ops.object.mode_set(mode='OBJECT')
    assert len(rig.data.bones) == 53
    bpy.ops.object.select_all(action='DESELECT')
    model.select_set(True)
    rig.select_set(True)
    bpy.context.view_layer.objects.active = rig
    bpy.ops.object.parent_set(type='ARMATURE_AUTO')
    # Rigid equipment should not melt or borrow nearby limb/finger weights.
    rigid_parts = []
    for part in parts[1:]:
        center = sum((model.data.vertices[i].co for i in part), Vector()) / len(part)
        side = 'Left' if center.x > 0 else 'Right'
        if len(part) in (454,82,54):  # gloves and padded guards remain articulated
            continue
        if len(part) in (506,512): target = side+'Foot'
        elif len(part)==90: target = 'Neck'
        elif center.z>1.5: target = 'Head'
        else: target = 'Hips'
        for group in model.vertex_groups:
            group.remove(part)
        model.vertex_groups[target].add(part, 1, 'REPLACE')
        rigid_parts.append({'vertices':len(part),'bone':target})
    # Bind sewn pad rims to the adjacent cloth surface, retaining the raised front.
    # Surface barycentrics transfer the same bone weights to the attachment rim.
    body=set(parts[0])
    model.data.calc_loop_triangles()
    triangles=[tuple(t.vertices) for t in model.data.loop_triangles if set(t.vertices)<=body]
    coords=[v.co.copy() for v in model.data.vertices]
    tree=BVHTree.FromPolygons(coords,triangles,all_triangles=True)
    bm=bmesh.new();bm.from_mesh(model.data)
    boundary={v.index for e in bm.edges if e.is_boundary for v in e.verts}
    bm.free()
    guard_vertices=0
    guard_moves=[]
    guard_rims=[]
    for part in parts:
        if len(part) not in (82,54): continue
        for group in model.vertex_groups: group.remove(part)
        for index in part:
            point,normal,face,distance=tree.find_nearest(coords[index])
            ids=triangles[face]
            mix=geometry.barycentric_transform(point,*(coords[i] for i in ids),
                Vector((1,0,0)),Vector((0,1,0)),Vector((0,0,1)))
            if index in boundary:
                model.data.vertices[index].co=point+normal*.001
                guard_moves.append((model.data.vertices[index].co-coords[index]).length)
                guard_rims.append((index,ids,tuple(mix)))
            weights={}
            for vid,amount in zip(ids,mix):
                for group in model.data.vertices[vid].groups:
                    weights[group.group]=weights.get(group.group,0)+max(0,amount)*group.weight
            for group,weight in weights.items():
                if weight>0: model.vertex_groups[group].add([index],weight,'REPLACE')
            guard_vertices+=1
    model.data.update()
    bpy.context.view_layer.objects.active = model
    bpy.ops.object.select_all(action='DESELECT')
    model.select_set(True)
    bpy.ops.object.vertex_group_limit_total(limit=4)
    bpy.ops.object.vertex_group_normalize_all(lock_active=False)
    unweighted = [v.index for v in model.data.vertices if sum(g.weight for g in v.groups)<.999]
    assert not unweighted, f'Unweighted vertices: {unweighted[:20]}'
    assert all(len([g for g in v.groups if g.weight>0])<=4 for v in model.data.vertices)
    assert all(abs(sum(g.weight for g in v.groups)-1)<1e-5 for v in model.data.vertices)
    assert len(model.modifiers)==1 and model.modifiers[0].type=='ARMATURE'
    # Linear skinning matches the ordinary FBX/Unity path; no preserve-volume shortcut.
    model.modifiers[0].use_deform_preserve_volume = False
    scene = bpy.context.scene
    scene.render.fps=24
    scene.frame_start=1
    scene.frame_end=145
    for pb in rig.pose.bones:
        pb.rotation_mode = 'QUATERNION'
    def rotate(name, axis, degrees):
        pb=rig.pose.bones[name]
        basis=pb.bone.matrix_local.to_quaternion()
        pb.rotation_quaternion=basis.inverted() @ Quaternion(Vector(axis),math.radians(degrees)) @ basis
    def fingers(side,sign,amount):
        for finger in finger_points:
            for index in range(1,4):
                degrees=amount*(.55 if finger=='Thumb' else 1)
                rotate(f'{side}{finger}{index}',(0,1,0),sign*degrees)
    tests=[('rest',1),('shoulder-raise',25),('elbow-90',49),('knee-90',73),('grip',97),('carry',121),('rest-return',145)]
    frame_records=[]
    pose_coordinates={}
    for name, frame in tests:
        scene.frame_set(frame)
        for pb in rig.pose.bones:
            pb.rotation_quaternion=Quaternion()
        for side,sign in (('Left',1),('Right',-1)):
            if name=='shoulder-raise': rotate(side+'UpperArm',(0,1,0),-sign*45)
            if name=='elbow-90': rotate(side+'LowerArm',(0,0,1),-sign*90)
            if name=='knee-90': rotate(side+'LowerLeg',(1,0,0),90)
            if name=='grip': fingers(side,sign,55)
            if name=='carry':
                rotate(side+'UpperArm',(0,1,0),sign*65)
                rotate(side+'LowerArm',(0,0,1),-sign*95)
                fingers(side,sign,25)
        for pb in rig.pose.bones:
            pb.keyframe_insert(data_path='rotation_quaternion',frame=frame,group=pb.name)
        scene.timeline_markers.new(name,frame=frame)
        bpy.context.view_layer.update()
        evaluated=model.evaluated_get(bpy.context.evaluated_depsgraph_get())
        coords=[v.co.copy() for v in evaluated.data.vertices]
        assert len(coords)==4765 and all(math.isfinite(a) for p in coords for a in p)
        pose_coordinates[frame]=coords
        ratios=[]
        for edge in model.data.edges:
            a,b=edge.vertices
            old=(model.data.vertices[a].co-model.data.vertices[b].co).length
            if old>.002: ratios.append((coords[a]-coords[b]).length/old)
        rim_gaps=[(coords[i]-sum((coords[v]*w for v,w in zip(ids,mix)),Vector())).length for i,ids,mix in guard_rims]
        frame_records.append({'pose':name,'frame':frame,'max_edge_stretch':max(ratios),'min_edge_ratio':min(ratios),
            'max_guard_rim_gap_m':max(rim_gaps)})
    rig.animation_data.action.name='NR_Deformation_Check_NOT_GAMEPLAY'
    rig.animation_data.action.use_fake_user=True
    rig.show_in_front=True
    rig.data.display_type='STICK'
    # Reuse the inspection studio, without changing the source material.
    scene.render.resolution_x=scene.render.resolution_y=1000
    scene.cycles.samples=24
    cam=scene.camera
    def render(name,frame,focus,offset,scale):
        scene.frame_set(frame)
        focus=Vector(focus)
        cam.location=focus+Vector(offset)
        cam.rotation_euler=(focus-cam.location).to_track_quat('-Z','Y').to_euler()
        cam.data.ortho_scale=scale
        scene.render.filepath=str(REVIEW/(name+'.png'))
        bpy.ops.render.render(write_still=True)
    render('boot-repaired',1,(.285,-.08,.11),(0,-1,.65),.4)
    for name,frame in tests[1:4]: render(name,frame,(0,0,.95),(2,-4,.6),2.35)
    render('grip',97,(.82,.015,1.335),(.5,-1,.8),.43)
    render('grip-right',97,(-.82,.015,1.335),(-.5,-1,.8),.43)
    render('knee-side',73,(.22,0,.52),(3,-1,.4),.95)
    render('carry',121,(0,-.1,.95),(2,-4,.6),2.05)
    scene.frame_set(1)
    cam.location=Vector((2,-4,1.4));cam.rotation_euler=(Vector((0,0,.9))-cam.location).to_track_quat('-Z','Y').to_euler();cam.data.ortho_scale=2.27
    bpy.ops.object.select_all(action='DESELECT')
    rig.select_set(True)
    bpy.context.view_layer.objects.active=rig
    for screen in bpy.data.screens:
        for area in screen.areas:
            if area.type=='VIEW_3D':
                area.spaces.active.overlay.show_extras=False
                area.spaces.active.region_3d.view_location=Vector((0,0,.9))
                area.spaces.active.region_3d.view_distance=3.5
                area.spaces.active.region_3d.view_rotation=cam.rotation_euler.to_quaternion()
    for img in bpy.data.images:
        if img.source=='FILE':
            img.pack()
            img.filepath='//../source/spacesuit+3d+model.fbm/spacesuit+3d+model_basecolor.jpg'
    blend=OUT/'NR_Employee_01_Rigged.blend'
    bpy.ops.wm.save_as_mainfile(filepath=str(blend))
    model.select_set(True)
    fbx=OUT/'NR_Employee_01_Rigged.fbx'
    bpy.ops.export_scene.fbx(filepath=str(fbx),use_selection=True,object_types={'MESH','ARMATURE'},
        add_leaf_bones=False,bake_anim=False,use_mesh_modifiers=False,path_mode='COPY',embed_textures=True,
        axis_forward='-Z',axis_up='Y')
    report={'blender':bpy.app.version_string,'source':str(source.relative_to(ROOT)),
        'source_blend_sha256':source_hash,'method':'Local Blender FK bones, native heat weights, rigid-part reassignment; no provider rigging',
        'repair':{'split_original_junction_vertices':junction_vertices,'added_vertices':6,'removed_faces':0,'unchanged_surface_coordinates_and_uvs':True},
        'mesh_after_boot_repair':cleaned,'bones':53,'deform_bones':52,'rigid_parts':rigid_parts,'guard_vertices_with_surface_weights':guard_vertices,
        'guard_rim_adjustment':{'vertices':len(guard_moves),'max_displacement_m':max(guard_moves),'surface_offset_m':.001},
        'poses':frame_records,'max_weights_per_vertex':4,
        'checks':{'source_preserved':hashlib.sha256(source.read_bytes()).hexdigest()==source_hash,
            'boot_exception_edges_zero':True,'boot_surfaces_and_all_uvs_unchanged':True,'all_vertices_normalized_and_weighted':True,
            'finite_evaluated_pose_vertices':True},
        'pending':['Visual deformation review','Unity Humanoid and game validation','Production animation clips','User quality approval']}
    assert report['checks']['source_preserved']
    expected_bones=set(rig.data.bones.keys())
    bpy.ops.wm.open_mainfile(filepath=str(blend),use_scripts=False)
    reloaded=bpy.data.objects['NR_Employee_01']
    loaded_rig=bpy.data.objects['NR_Employee_Rig']
    assert set(loaded_rig.data.bones.keys())==expected_bones
    assert loaded_rig.animation_data.action.name=='NR_Deformation_Check_NOT_GAMEPLAY'
    for frame,expected in pose_coordinates.items():
        bpy.context.scene.frame_set(frame)
        bpy.context.view_layer.update()
        actual=reloaded.evaluated_get(bpy.context.evaluated_depsgraph_get()).data.vertices
        assert max((v.co-e).length for v,e in zip(actual,expected))<1e-5
    report['checks']['blend_reopens_with_all_test_poses']=True
    bpy.ops.wm.read_factory_settings(use_empty=True)
    bpy.ops.import_scene.fbx(filepath=str(fbx),use_image_search=False)
    meshes=[o for o in bpy.context.scene.objects if o.type=='MESH']
    rigs=[o for o in bpy.context.scene.objects if o.type=='ARMATURE']
    assert len(meshes)==len(rigs)==1
    imported=meshes[0]
    assert set(rigs[0].data.bones.keys())==expected_bones
    imported_info=inspection.inspect(imported)
    low,high=inspection.bounds(meshes)
    assert abs((high-low).z-1.8)<1e-4
    assert imported_info['triangles']==9118
    assert imported_info['overconnected_edges']==imported_info['inconsistent_winding_edges']==0
    assert all(abs(sum(g.weight for g in v.groups)-1)<1e-4 for v in imported.data.vertices)
    assert all(len([g for g in v.groups if g.weight>0])<=4 for v in imported.data.vertices)
    assert len(bpy.data.actions)==0, 'Diagnostic action must not be exported as a game clip'
    images=[i for i in bpy.data.images if i.source=='FILE']
    assert images and all(list(i.size)==[4096,4096] and len(i.pixels)>0 for i in images)
    report['fbx_roundtrip']={'bones':len(rigs[0].data.bones),'vertices':imported_info['vertices'],
        'triangles':imported_info['triangles'],'height_m':(high-low).z,'actions':len(bpy.data.actions)}
    report['checks']['fbx_reimports_skeleton_geometry_and_weights']=True
    report['checks']['fbx_texture_loads_and_diagnostic_action_excluded']=True
    report['output_sha256']={p.name:hashlib.sha256(p.read_bytes()).hexdigest() for p in (blend,fbx)}
    (OUT/'validation.json').write_text(json.dumps(report,indent=2)+'\n')
    print('RIG_RESULT',json.dumps({k:report[k] for k in ('bones','poses','checks')}))

if __name__=='__main__': main()

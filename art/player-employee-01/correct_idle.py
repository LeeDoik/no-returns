"""Preserve a supplied Mixamo Idle and bake a small upper-body posture offset.

Run with Blender --background --factory-startup --disable-autoexec
--python-exit-code 1 --python art/player-employee-01/correct_idle.py --
--source /path/to/Idle.fbx --output /path/to/local-review [--walk /path/to/Walking.fbx]
"""
import argparse
import hashlib
import json
import math
import sys
from pathlib import Path

import bpy
from mathutils import Quaternion, Vector

ROOT = Path(__file__).resolve().parent
# Local bone-X offsets, in degrees. Small artistic adjustments, not a rest-rig edit.
OFFSETS = {'Chest': -1.5, 'UpperChest': -1.5, 'Neck': -4.0}
SAMPLES = (1, 63, 126, 188, 251)


def digest(path):
    return hashlib.sha256(path.read_bytes()).hexdigest()


def curves(action):
    return [f for layer in action.layers for strip in layer.strips
            for bag in strip.channelbags for f in bag.fcurves]


def channel_signature(action, excluded=()):
    return [(f.data_path, f.array_index,
             [(tuple(k.co), tuple(k.handle_left), tuple(k.handle_right), k.interpolation)
              for k in f.keyframe_points]) for f in curves(action)
            if f.data_path not in excluded]


def pose(rig):
    return {p.name: rig.matrix_world @ p.head for p in rig.pose.bones}


def lean(rig, bone):
    p = rig.pose.bones[bone]
    v = rig.matrix_world.to_3x3() @ (p.tail - p.head)
    return math.degrees(math.atan2(-v.y, v.z))


def geometry_signature(mesh):
    return ([tuple(v.co) for v in mesh.data.vertices],
            [tuple(p.vertices) for p in mesh.data.polygons],
            [tuple(v.uv) for v in mesh.data.uv_layers.active.data],
            [[(g.group, g.weight) for g in v.groups] for v in mesh.data.vertices])


def render(scene, out, name, frame, offset, scale=2.15):
    scene.frame_set(frame)
    focus = Vector((0, 0, .95))
    scene.camera.location = focus + Vector(offset)
    scene.camera.rotation_euler = (focus - scene.camera.location).to_track_quat('-Z', 'Y').to_euler()
    scene.camera.data.ortho_scale = scale
    scene.render.filepath = str(out / (name + '.png'))
    bpy.ops.render.render(write_still=True)


def render_comparison(blend, out):
    # Work in an unsaved review scene; leave the single-character deliverable intact.
    bpy.ops.wm.open_mainfile(filepath=str(blend), use_scripts=False)
    scene = bpy.context.scene
    rig, mesh = bpy.data.objects['NR_Employee_Rig'], bpy.data.objects['NR_Employee_01']
    before = rig.copy()
    before.data = rig.data.copy()
    scene.collection.objects.link(before)
    before.animation_data.action = bpy.data.actions['NR_Idle_Mixamo_Original']
    copy = mesh.copy()
    scene.collection.objects.link(copy)
    copy.parent = before
    for modifier in copy.modifiers:
        if modifier.type == 'ARMATURE':
            modifier.object = before
    for obj, y in ((before, -.62), (rig, .62)):
        group = bpy.data.objects.new('Preview offset', None)
        scene.collection.objects.link(group)
        obj.parent = group
        group.location.y = y
    scene.frame_set(188)
    focus = Vector((0, 0, 1.0))
    scene.camera.location = focus + Vector((4, 0, 0))
    scene.camera.rotation_euler = (focus - scene.camera.location).to_track_quat('-Z', 'Y').to_euler()
    scene.camera.data.ortho_scale = 3.1
    scene.render.resolution_x, scene.render.resolution_y = 1200, 850
    for label, y in (('BEFORE', -.62), ('AFTER', .62)):
        bpy.ops.object.text_add(location=(.35, y, 1.99))
        text = bpy.context.object
        text.data.body, text.data.align_x, text.data.size = label, 'CENTER', .09
        text.rotation_euler = scene.camera.rotation_euler
    scene.render.filepath = str(out / 'idle-comparison.png')
    bpy.ops.render.render(write_still=True)


def main():
    parser = argparse.ArgumentParser()
    parser.add_argument('--source', type=Path, required=True)
    parser.add_argument('--output', type=Path, required=True)
    parser.add_argument('--walk', type=Path)
    args = parser.parse_args(sys.argv[sys.argv.index('--') + 1:])
    source, out = args.source.resolve(), args.output.resolve()
    assert source.is_file() and source.suffix.lower() == '.fbx'
    assert source.parent != out, 'Keep generated files separate from supplied files'
    out.mkdir(parents=True, exist_ok=True)
    source_hash = digest(source)
    studio = ROOT / 'rigged/NR_Employee_01_Rigged.blend'
    studio_hash = digest(studio)
    bpy.ops.wm.open_mainfile(filepath=str(studio), use_scripts=False)
    for obj in list(bpy.context.scene.objects):
        if obj.type in {'MESH', 'ARMATURE'}:
            bpy.data.objects.remove(obj, do_unlink=True)
    for action in list(bpy.data.actions):
        bpy.data.actions.remove(action)
    bpy.ops.import_scene.fbx(filepath=str(source), use_image_search=False)
    rigs = [o for o in bpy.context.scene.objects if o.type == 'ARMATURE']
    meshes = [o for o in bpy.context.scene.objects if o.type == 'MESH']
    assert len(rigs) == len(meshes) == 1, 'Expected the supplied With Skin Idle'
    rig, mesh = rigs[0], meshes[0]
    assert len(rig.data.bones) == 53 and len(mesh.data.vertices) == 4765
    before_geometry = geometry_signature(mesh)
    rest = {b.name: b.matrix_local.copy() for b in rig.data.bones}
    original = rig.animation_data.action
    assert tuple(original.frame_range) == (1., 251.), 'Reinspect a different source duration'
    original.name = 'NR_Idle_Mixamo_Original'
    original.use_fake_user = True
    original_signature = channel_signature(original)
    scene = bpy.context.scene
    assert scene.render.fps == 30
    scene.frame_start, scene.frame_end = 1, 251
    scene.render.resolution_x = scene.render.resolution_y = 800
    scene.render.resolution_percentage = 100
    old_poses, rotations, old_head, old_chest = [], [], [], []
    for frame in range(1, 252):
        scene.frame_set(frame)
        old_poses.append(pose(rig))
        rotations.append({n: rig.pose.bones[n].rotation_quaternion.copy() for n in OFFSETS})
        old_head.append(lean(rig, 'Head'))
        old_chest.append(lean(rig, 'UpperChest'))
    render(scene, out, 'idle-before-side', 188, (4, 0, 0))
    fixed = original.copy()
    fixed.name = 'NR_Idle_Upright'
    fixed.use_fake_user = True
    rig.animation_data.action = fixed
    for frame in range(1, 252):
        scene.frame_set(frame)
        for name, angle in OFFSETS.items():
            pb = rig.pose.bones[name]
            pb.rotation_quaternion = Quaternion((1, 0, 0), math.radians(angle)) @ rotations[frame - 1][name]
            pb.keyframe_insert('rotation_quaternion', frame=frame, group=name)
    paths = [rig.pose.bones[n].path_from_id('rotation_quaternion') for n in OFFSETS]
    assert channel_signature(original) == original_signature
    assert channel_signature(original, paths) == channel_signature(fixed, paths)
    assert geometry_signature(mesh) == before_geometry
    assert all(b.matrix_local == rest[b.name] for b in rig.data.bones)
    lower = ['Root', 'Hips'] + [side + part for side in ('Left', 'Right')
                                for part in ('UpperLeg', 'LowerLeg', 'Foot', 'Toes')]
    new_head, new_chest, saved_poses = [], [], {}
    lower_error = 0.
    for frame in range(1, 252):
        scene.frame_set(frame)
        current = pose(rig)
        lower_error = max(lower_error, max((current[n] - old_poses[frame - 1][n]).length for n in lower))
        evaluated = mesh.evaluated_get(bpy.context.evaluated_depsgraph_get())
        assert all(math.isfinite(c) for v in evaluated.data.vertices for c in v.co)
        new_head.append(lean(rig, 'Head'))
        new_chest.append(lean(rig, 'UpperChest'))
        if frame in SAMPLES:
            saved_poses[frame] = current
    assert lower_error < 1e-6
    assert max(abs(x) for x in new_head) < max(abs(x) for x in old_head)
    loop_error = max((saved_poses[1][n] - saved_poses[251][n]).length for n in saved_poses[1])
    assert loop_error < 1e-6
    render(scene, out, 'idle-after-side', 188, (4, 0, 0))
    render(scene, out, 'idle-after-front', 188, (0, -4, 0))
    render(scene, out, 'idle-after-three-quarter', 188, (2, -4, .2))
    for frame in SAMPLES:
        render(scene, out, f'idle-after-{frame:03d}', frame, (4, 0, 0))
    scene.frame_set(1)
    bpy.ops.object.select_all(action='DESELECT')
    rig.select_set(True)
    bpy.context.view_layer.objects.active = rig
    rig.show_in_front = True
    for img in bpy.data.images:
        if img.source == 'FILE' and img.has_data:
            img.pack()
    for screen in bpy.data.screens:
        for area in screen.areas:
            if area.type == 'VIEW_3D':
                area.spaces.active.shading.type = 'MATERIAL'
                area.spaces.active.region_3d.view_distance = 3.0
                area.spaces.active.region_3d.view_location = (0, 0, .95)
                area.spaces.active.region_3d.view_rotation = scene.camera.rotation_euler.to_quaternion()
    blend, fbx = out / 'NR_Employee_Idle_Upright.blend', out / 'NR_Employee_Idle_Upright.fbx'
    bpy.ops.wm.save_as_mainfile(filepath=str(blend))
    mesh.select_set(True)
    # FBX uses a zero-based time origin; Blender's default FBX import adds frame 1.
    # Shift only the export copy in memory; the saved editable file remains 1..251.
    for curve in curves(fixed):
        for key in curve.keyframe_points:
            key.co.x -= 1
            key.handle_left.x -= 1
            key.handle_right.x -= 1
    scene.frame_start, scene.frame_end = 0, 250
    bpy.ops.export_scene.fbx(filepath=str(fbx), use_selection=True, object_types={'MESH', 'ARMATURE'},
        add_leaf_bones=False, use_mesh_modifiers=False, bake_anim=True, bake_anim_use_all_actions=False,
        bake_anim_use_nla_strips=False, bake_anim_simplify_factor=0, path_mode='COPY', embed_textures=True,
        axis_forward='-Z', axis_up='Y')
    rig_name = rig.name
    bpy.ops.wm.open_mainfile(filepath=str(blend), use_scripts=False)
    rig = bpy.data.objects[rig_name]
    assert rig.animation_data.action.name == 'NR_Idle_Upright'
    assert bpy.data.actions.get('NR_Idle_Mixamo_Original')
    for frame in SAMPLES:
        bpy.context.scene.frame_set(frame)
        assert max((pose(rig)[n] - p).length for n, p in saved_poses[frame].items()) < 1e-6
    bpy.ops.wm.read_factory_settings(use_empty=True)
    bpy.ops.import_scene.fbx(filepath=str(fbx), use_image_search=False)
    rig = next(o for o in bpy.context.scene.objects if o.type == 'ARMATURE')
    mesh = next(o for o in bpy.context.scene.objects if o.type == 'MESH')
    assert len(rig.data.bones) == 53 and len(mesh.data.vertices) == 4765
    assert len(bpy.data.actions) == 1 and tuple(rig.animation_data.action.frame_range) == (1., 251.)
    assert bpy.context.scene.render.fps == 30
    assert any(i.has_data and tuple(i.size) == (4096, 4096) for i in bpy.data.images)
    roundtrip_error = 0.
    for frame in SAMPLES:
        bpy.context.scene.frame_set(frame)
        roundtrip_error = max(roundtrip_error, max((pose(rig)[n] - p).length for n, p in saved_poses[frame].items()))
    assert roundtrip_error < 1e-4
    report = {'blender': bpy.app.version_string, 'source': {'filename': source.name, 'sha256': source_hash},
        'provider': 'Mixamo, user supplied', 'source_clip_id_and_download_settings': 'not supplied',
        'publication': 'motion-bearing source and outputs kept local; source redistribution terms not established',
        'frames': [1, 251], 'fps': 30, 'bones': 53, 'vertices': 4765, 'offset_degrees_local_x': OFFSETS,
        'forward_lean_degrees': {'head_before': [min(old_head), max(old_head)], 'head_after': [min(new_head), max(new_head)],
                                 'upper_chest_before': [min(old_chest), max(old_chest)], 'upper_chest_after': [min(new_chest), max(new_chest)]},
        'max_lower_body_head_displacement_m': lower_error, 'loop_endpoint_head_error_m': loop_error,
        'fbx_roundtrip_sample_head_error_m': roundtrip_error,
        'checks': {'all_251_frames_finite': True, 'original_action_and_other_channels_preserved': True,
                   'mesh_uv_weights_and_rest_skeleton_unchanged': True, 'lower_body_unchanged': True,
                   'loop_endpoint_positions_match': True, 'blend_reopens_with_both_actions': True,
                   'fbx_reimports_one_clip_53_bones_and_texture': True, 'sampled_fbx_pose_positions_match': True},
        'pending': ['User posture approval', 'Walking rest-pose conversion and game integration',
                    'Unity Humanoid/playback/transitions', 'Public redistribution terms for motion source files'],
        'output_sha256': {p.name: digest(p) for p in (blend, fbx)}}
    if args.walk:
        walk = args.walk.resolve()
        assert walk.is_file()
        walk_hash = digest(walk)
        bpy.ops.wm.read_factory_settings(use_empty=True)
        bpy.ops.import_scene.fbx(filepath=str(walk), use_image_search=False)
        wr = next(o for o in bpy.context.scene.objects if o.type == 'ARMATURE')
        assert set(wr.data.bones.keys()) == set(rest)
        distances = {b.name: (b.head_local - rest[b.name].translation).length for b in wr.data.bones}
        report['walking_inspection_only'] = {'filename': walk.name, 'sha256': walk_hash,
            'bones': len(wr.data.bones), 'meshes': sum(o.type == 'MESH' for o in bpy.context.scene.objects),
            'frame_range': list(wr.animation_data.action.frame_range), 'fps': bpy.context.scene.render.fps,
            'max_rest_joint_difference_from_idle_m': max(distances.values()),
            'joints_differing_more_than_1mm': [n for n, d in distances.items() if d > .001]}
        assert digest(walk) == walk_hash
    assert digest(source) == source_hash and digest(studio) == studio_hash
    report['checks']['source_fbx_and_canonical_blend_preserved'] = True
    (out / 'validation.json').write_text(json.dumps(report, indent=2) + '\n')
    render_comparison(blend, out)
    print('IDLE_RESULT', json.dumps(report))


if __name__ == '__main__':
    main()

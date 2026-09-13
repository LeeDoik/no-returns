# 3. Map, art and technical guide

[한국어](03-guides.ko.md)

> **Historical record · retired/superseded 2026-09-12.** The [PSX space-delivery design](../../../../current/01-overview.en.md) is current. Implementation, launch, art and values below are historical, not current status. Deleted files remain path records only.


Main development is now NO RETURNS Unity. Godot specifications and checks below describe the preserved game, not completed Unity functionality. 현재 기준 / Current baseline (`18-unity-mainline.en.md`; retired file).

Reviewed: 2026-09-09 · Game 0.9.4 · Code baseline: this reactive-prop revision.


## Launch and editing entry points

- `00_게임 실행.cmd`: run the last Windows package; source edits are not automatically included.
- `PLAY.cmd`: run current sources. `01_맵 편집.cmd` or `EDIT_MAP.cmd`: edit the map.
- `03_물리 실험실.cmd`: separate test map. `BUILD.cmd`: refresh the package. `02_배포 파일 찾기.cmd`: locate distribution files.

## Map production

The source is shipping_shrine.tscn (`../../scenes/maps/shipping_shrine.tscn`; retired file). Geometry holds static space, Gameplay holds docks/belt/packrat/spawns, Environment holds lighting and Decoration holds dressing. F6 runs the scene alone; F5 runs the full game.

1. Open the source map and select the relevant parent. Moving only device visuals can separate them from gameplay detection.
2. Use Transform for placement/rotation and SolidBlock Dimensions for wall sizes. Plain meshes do not automatically provide collision.
3. Align worker/parcel spawns to the floor without overlap. Expansion requires checking floor, boundaries, PlayableBounds, lighting and device zones together.
4. Keep packrat waypoints connected by traversable routes. Automatic detours around complex obstacles are not guaranteed.
5. Save and check empty-handed travel, carrying, throwing and camera occlusion with the actual character. Distribute the same build to friends after map changes.

See detailed map editing (`../development/map-editing.en.md`; retired file) and reactive-device records (`../prototype/08-reactive-delivery.en.md`; retired file). Do not assume old example coordinates describe current placement.

## Art and animation production

`art/` contains authoring sources; `assets/art/release-01/` contains runtime assets. The current worker source is `art/worker-motion-02/worker-motion.blend`, reproducible builder `rebuild_motion.py`, runtime model `assets/art/release-01/worker.glb`. Clip selection/transitions live in `scripts/worker_animation.gd`, presentation poses in `scripts/worker.gd`, and asset binding in `scripts/postal_art.gd`.

Preserve originals and create separate candidates to check size, orientation, materials, bone names and clip names. Do not casually replace only the exported model. After Godot import, combine joint measurements with continuous movement/carry/turn/jump/land/throw capture. A single screenshot cannot establish natural motion. Promote only accepted results and update provenance/rights records. Production history (`../art/01-production.en.md`; retired file), motion rebuild (`../art/06-motion-rebuild.en.md`; retired file).

## Technical structure and change locations

| Feature | Reference code |
|---|---|
| Match flow, input and authority integration | main.gd |
| Worker movement, collision and visuals | worker.gd |
| Parcel physics, carrying and compressed state | cargo.gd, cargo_rules.gd |
| Contracts, upgrades and rewards | contracts.gd |
| Connections and protocol | session.gd |
| Map references/editable elements | editable_map.gd, editable_block.gd, map_zone.gd |
| UI and English source/Korean copy | interface.gd, copy.gd, campaign_copy.gd |
| Preferences and records | preferences.gd, run_profile.gd |

All files above are under `scripts/`. The host owns decisions; guests receive state. Wire-format changes require considering a protocol bump and testing all participants on the same build. Do not confuse authoritative state with visual presentation.

## Physics laboratory

This is a single-user test reusing production workers/parcels. Tab/Escape toggles the panel; WASD moves, Space jumps, E picks up/drops, left click throws, 1–4 changes stations and R resets. Tune mass, friction, restitution, parcel gravity, angular damping and throw speed one at a time with resets. Lab details (`../art/07-physics-lab.en.md`; retired file). Force-driven grabbing is currently removed.

## Completion workflow

Review relevant code/scenes and this guide → change → appropriate checks → update current specification/backlog/validation in both languages → log the change → save to local Git. Documentation-only work checks links, language parity and values rather than rerunning the entire game suite. Do not revert or commit unrelated user changes.

## Editing reactive-device art

Presentation code locates Top/Launch plate and metal support meshes by name. Rerun support-contact tests after re-exporting the model. Decorative carton debris is not delivery cargo. 0.9.4 production/validation details (`../art/09-reactive-impact.en.md`; retired file).

## Resource inventory

See the complete resource inventory (`09-resource-inventory.en.md`; retired file) for 13 runtime-folder GLBs, 21 external images, authoring sources, scenes, shaders and procedural elements. This is a static audit dated 2026-09-09, not final quality or rights approval.

## Asset generation pipeline review · 2026-09-09

Concept/style review → asset-specific draft → Blender cleanup and animation → GLB export → Godot integration → in-game visual and motion validation. Not every asset passes through every tool.

- GPT Image: early sneezer image concepts. Higgsfield: release-01 style images, materials and character mesh production.
- Blender: generated mesh cleanup and direct box/facility modeling, size, axes, materials and animation. The early PS1 sneezer was also built with Blender scripts.
- Tripo: worker rigging and animation history. Current worker motion was rebuilt in Blender; box/device reactions also use Godot procedural animation.
- Sources are in art/ and runtime outputs in assets/art/release-01/. Early art/sneezer assets are separate from the runtime sneezer.
- Reimport GLBs and check size, materials and motion, then inspect readability, collision and continuous movement in game. Run online checks for relevant changes. Distinguish generation success from integration and quality acceptance.

Evidence: production history (`../art/01-production.en.md`; retired file), motion rebuild (`../art/06-motion-rebuild.en.md`; retired file), resource inventory (`09-resource-inventory.en.md`; retired file). This review inspected records, sources and code; no service connection checks, new renders or play tests were performed.

## Quality-first pipeline proposal · 2026-09-09

Status: recommendation for the user, not adopted or implemented. Assumes GPT Image, Higgsfield and Tripo are available. Actual account balances, available models and connections were not checked in this review.

1. Style baseline: compare a representative worker, box and facility under the same game camera and lighting. Do not automatically mix the earlier PS1 concepts with the current release-01 direction. Align palette, silhouette, texture density, surface roughness and shadows first.
2. Concepts: compare 3–5 candidates for key assets using GPT Image and Higgsfield image tools. Derive front, side and rear views from one selected design, then compare part placement, proportions and colors. A consistent single view is preferable to contradictory multiple views. Generated turntable videos are not evidence of accurate 3D structure.
3. Characters: generate 2–3 key model candidates with Tripo and select silhouettes and structure using neutral-material 360-degree inspection. Separate necessary parts and clean the game mesh before final materials and rigging. Correct auto-rig joint deformation and carrying hand contact in Blender. Candidate counts are production suggestions, not measured optima.
4. Boxes and facilities: directly build moving lids, mouths, launch plates, belts and repeatable modules in Blender. Use GPT Image/Higgsfield design and material images. Do not force simple parts through 3D generation.
5. Materials: treat generated images as candidate material sources. In Blender, clean UVs, palette, seams and painted-in lighting, and bake to the game mesh where needed. Preserve high-resolution sources and select runtime resolution for the actual screen. If the PS1 direction is adopted, deliberately retain low-resolution textures and rough shading.
6. Motion and validation: use Tripo motion as a draft and refine game-specific carrying, throwing and sneezing in Blender/Godot. Reimport GLBs, then check readability, floor/hand contact, transitions and frame times with multiple characters and props through the game camera. Finish a representative worker, sneezer and facility module before scaling production.

Concentrate credits on comparing design/structure candidates and refining materials for selected results. Passing an asset through several tools does not guarantee higher quality. No project-specific comparative generations have been performed yet.

Capability references: [Tripo multiview](https://developers.tripo3d.ai/en/docs/generation-multiview-to-model), [Tripo production, cleanup and rigging](https://www.tripo3d.ai/blog/tripo-studio-tutorial-english), [Higgsfield Blender tools](https://higgsfield.ai/blog/higgsfield-blender-plugin). Stage assignments above are production judgments based on these capabilities and project history.

## All-asset coherence and image approval — confirmed user rules · 2026-09-09

Every in-game asset follows shared color roles, shape language, texture density, roughness and lighting. Prioritize coherence within the same game screen over isolated detail. Present concept images to the user first and create 3D models only from explicitly approved images. Generation completion or silence is not approval. The representative worker/sneezer/conveyor concepts (`../../art/quality-trio-01/README.en.md`; retired file) have an approved D selection that is now integrated.


The D trio is in production after user approval. See the representative art production record (`../../art/quality-trio-01/README.en.md`; retired file) for current files, validation and API/Studio status. See the current D integration specification (`../art/10-approved-d.en.md`; retired file) for game integration and validation status.


D production and reimport guide (`../art/10-approved-d.en.md`; retired file) — export CrewMask as COLOR_0 for the new worker. Earlier approval-pending/integration-pending statements are historical; the current D integration specification supersedes them.

## Expedition content authoring guidance · 2026-09-10

Use the zone, contract and tool tables in the expansion design (`08-expansion-directions.en.md`; retired file) for future authoring. Preserve the Shipping Shrine source and validate a separate expedition map. Identify safe/optional connections, receipt/recovery sockets, designated facility faces, packrat nests and safe areas per zone. The 160×160m envelope is a proposal, not an area to fill completely. Separate decorative paper effects from passage blocking, and verify turning/camera clearance while carrying a 0.8m parcel. Obtain user approval for new design images before 3D production, preserving D colors, materials and proportions. No authoring nodes or expedition map have been added yet.

## EXP-01 implementation · 2026-09-10

Edit Ground/Truck/zone objects in the expedition scene (`../../scenes/maps/toypost_village.tscn`; retired file) using Godot. Preserve Gameplay WorkerSpawns/CargoSpawns/Receipts/TruckBase names and contract IDs 1–5. Review the 40m half-extent, 1.8m receipt radius and 6m truck radius with expedition_map.gd. Initial authoring tool build_expedition_map.gd overwrites edits if rerun; do not use it for normal editing. No runtime terrain regeneration. After changes run python tools/run_expedition_tests.py and actual carrying/camera checks.

Play guide and limits (`../prototype/10-expedition.en.md`; retired file).


Unity project and CLI environment (`14-unity-environment.en.md`; retired file) · 2026-09-11 · Unity 6000.6.0f1 / CLI 1.0.0-beta.9


Unity movement and carrying lab (`15-unity-controls.en.md`; retired file) · UNITY-02 / Unity 0.2.0 / 2026-09-11


2026-09-12 · New Unity delivery experiment: NR-LOOP-01 launch and validation (`22-slapstick-loop.en.md`; retired file) — use that document for implementation, automated evidence and outstanding human validation.

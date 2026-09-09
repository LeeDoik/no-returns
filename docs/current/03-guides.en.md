# 3. Map, art and technical guide

[한국어](03-guides.ko.md)

Reviewed: 2026-09-09 · Game 0.9.3 · Code baseline `1d8fbde` (update when behavior changes).


## Launch and editing entry points

- `00_게임 실행.cmd`: run the last Windows package; source edits are not automatically included.
- `PLAY.cmd`: run current sources. `01_맵 편집.cmd` or `EDIT_MAP.cmd`: edit the map.
- `03_물리 실험실.cmd`: separate test map. `BUILD.cmd`: refresh the package. `02_배포 파일 찾기.cmd`: locate distribution files.

## Map production

The source is [shipping_shrine.tscn](../../scenes/maps/shipping_shrine.tscn). Geometry holds static space, Gameplay holds docks/belt/packrat/spawns, Environment holds lighting and Decoration holds dressing. F6 runs the scene alone; F5 runs the full game.

1. Open the source map and select the relevant parent. Moving only device visuals can separate them from gameplay detection.
2. Use Transform for placement/rotation and SolidBlock Dimensions for wall sizes. Plain meshes do not automatically provide collision.
3. Align worker/parcel spawns to the floor without overlap. Expansion requires checking floor, boundaries, PlayableBounds, lighting and device zones together.
4. Keep packrat waypoints connected by traversable routes. Automatic detours around complex obstacles are not guaranteed.
5. Save and check empty-handed travel, carrying, throwing and camera occlusion with the actual character. Distribute the same build to friends after map changes.

See [detailed map editing](../development/map-editing.en.md) and [reactive-device records](../prototype/08-reactive-delivery.en.md). Do not assume old example coordinates describe current placement.

## Art and animation production

`art/` contains authoring sources; `assets/art/release-01/` contains runtime assets. The current worker source is `art/worker-motion-02/worker-motion.blend`, reproducible builder `rebuild_motion.py`, runtime model `assets/art/release-01/worker.glb`. Clip selection/transitions live in `scripts/worker_animation.gd`, presentation poses in `scripts/worker.gd`, and asset binding in `scripts/postal_art.gd`.

Preserve originals and create separate candidates to check size, orientation, materials, bone names and clip names. Do not casually replace only the exported model. After Godot import, combine joint measurements with continuous movement/carry/turn/jump/land/throw capture. A single screenshot cannot establish natural motion. Promote only accepted results and update provenance/rights records. [Production history](../art/01-production.en.md), [motion rebuild](../art/06-motion-rebuild.en.md).

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

This is a single-user test reusing production workers/parcels. Tab/Escape toggles the panel; WASD moves, Space jumps, E picks up/drops, left click throws, 1–4 changes stations and R resets. Tune mass, friction, restitution, parcel gravity, angular damping and throw speed one at a time with resets. [Lab details](../art/07-physics-lab.en.md). Force-driven grabbing is currently removed.

## Completion workflow

Review relevant code/scenes and this guide → change → appropriate checks → update current specification/backlog/validation in both languages → log the change → save to local Git. Documentation-only work checks links, language parity and values rather than rerunning the entire game suite. Do not revert or commit unrelated user changes.

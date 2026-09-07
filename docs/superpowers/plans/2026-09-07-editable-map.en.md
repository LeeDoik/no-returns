# No-code map editing conversion plan

[한국어](2026-09-07-editable-map.ko.md)

## Authorized goal and structure

The user requested direct map editing in the editor. Convert the procedural map once into `scenes/maps/shipping_shrine.tscn`, then use that saved file as the source of truth. Ordinary play/build never regenerates or overwrites it. Geometry, lighting and decoration are real nodes. Edit dispatch visuals and zones together, belt/lever, rat nest/activity/patrol, and worker/cargo spawns.

## Implementation sequence

1. Add checks for moved/rotated bays and belt, relocated nest/spawns, and layout persistence across save/reload.
2. Implement map runtime, bilingual signs and zone containment helpers. Retain the old generator only for one-time conversion.
3. Group the current map into functional nodes, save it and reference it from the main scene. Workers, cargo, belt and rat read scene transforms.
4. Reject host/guest map-file mismatches before admission. Run existing local/network tests plus edited-layout tests.
5. Provide an editor launcher, reusable wall example and complete bilingual beginner guide. Inspect editor/game rendering, Windows export and pack; record in local Git.

## Limits and preservation

Preserve the user's project.godot editor save and concurrent art work. Default layout, dimensions, checks and rules remain. Rat AI does not gain general pathfinding: place its waypoints along open routes. Multiplayer participants must use builds of the same map. Live edit synchronization, map downloading and Steam Workshop are out of scope. Use F5 for the full game; exported executables require rebuilding.


## Validation results · September 8, 2026

Twenty-two behavior checks, seven real two-process scenarios and the four-process scenario passed. New coverage includes save/reload, translated/rotated bays and belt, actual delivery earnings, spawn integration, simultaneous block mesh/collision resizing and different-map rejection. The full log is `artifacts/editable-full-tests.log`; additional integration evidence is `artifacts/editable-final-test.log`.

Reviewed the actual-renderer Shipping Shrine capture. Windows export and packaged startup checks passed. The pack audit confirmed 27 compiled runtime scripts, the saved map and map fingerprint. Stable 0.7 EXE/ZIP paths now contain 0.7.3. Desktop capture returned another screen instead of the selected Godot window, so visual verification of mouse-driven editor interaction was not completed. Engine checks verified saved-scene edits, reload and gameplay integration.

Provided the [English editing guide](../../development/map-editing.en.md) and full Korean counterpart, EDIT_MAP.cmd, BUILD.cmd and a SolidBlock prefab with collision. Existing user project.godot edits and separate art files are excluded from this commit.

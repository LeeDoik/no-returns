# CINDER DEPOT primitive blockout

[한국어](cinder-blockout.ko.md)

2026-09-17 · CINDER-BLOCKOUT-01 · Separate spatial-validation scene

## Launch and scope

Run [Play_Cinder_Blockout.cmd](../../Play_Cinder_Blockout.cmd). The executable is `builds/CinderBlockout/NoReturns-CinderBlockout.exe`. The existing Listener game and ship trial scenes are preserved. This is a single-player experiment for reviewing space and cargo clearance. Contracts, delivery acceptance, receipts, economy, networking and enemy pursuit are not connected. Three red capsules mark proposed Listener positions.

WASD moves, mouse looks, E picks up the parcel, Q drops it, Shift sprints empty-handed, Space jumps empty-handed, F1 toggles Korean/English, Esc releases the cursor, and clicking resumes mouse look. Carrying prevents sprinting and jumping as in the existing interior trial. Language choice lasts only for this running session.

## Layout and implemented dimensions

Building positions, the outdoor detour, central shortcut and A/B/C loops from [concept map 02](concept-zoning.en.md) have been translated into primitives. One concept unit is interpreted as 0.12m: this is an **initial validation scale**, not a final map-size decision. The service building's complex outline is simplified to a rectangle. Each building has open north and south entrances. Detailed interior rooms, door interactions and clues are not present yet.

| Item | Implemented value |
|---|---|
| Entire ground | 108×86.4m |
| Warehouse | 14.4×20.4m |
| Office | 14.4×9m |
| BAY 04 | 13.8×16.8m |
| Service building | 12.6×22.2m |
| Storage | 28.8×7.2m |
| Building ceiling underside / doorway | 4m / 3.2m wide, 3.3m high |
| Covered walkway floor marking / roof underside | 3.2m wide / 4.2m |
| Loop and shortcut floor markings | 2.4m wide; outdoor ground beyond the markings is also walkable |
| Employee / camera | 1.8m high, 0.34m radius / 1.57m eye height |
| Walk / empty-hand sprint | 3m/s / 5m/s |
| Jump speed / gravity | 5m/s / 18m/s² |
| Carried parcel | 0.8×0.65×0.65m |

The ship reuses the model, collision and lighting from [interior trial 05](ship-interior-trial.en.md). It is placed southwest, with its entrance rotated east toward the departure route. No new exterior was produced. The ship was not scaled to fit the buildings. Covered walkways are simple slabs for checking overhead clearance and routing; supports and final architectural details are omitted.

## Unity editing and reproduction

Scene: [CinderDepotBlockout.unity](../../NoReturns/Assets/_NoReturns/Scenes/CinderDepotBlockout.unity). Move or resize buildings, obstacles and floor markings under `Cinder Depot editable primitive blockout`. `Reused Ship Interior Trial 05` groups the existing cabin. Primitive meshes and BoxColliders allow direct Unity editing.

Use `NO RETURNS > Trials > Create/Validate/Build Cinder Depot Blockout` in Unity. **Create and Build regenerate the scene and overwrite manual scene edits.** Save an edited variant under another name or incorporate it into the [generator](../../NoReturns/Assets/_NoReturns/Editor/CinderBlockoutBuild.cs). Standalone Validate checks the current scene. Output uses the existing `CarryWorkspace.txt` path when available.

Dimension sources: the generator above and [movement code](../../NoReturns/Assets/_NoReturns/Runtime/CinderBlockoutWalk.cs). Eight pre-existing changes to ship materials are excluded from this task's changes.

## Validation and next play review

- [x] Compilation and scene creation through Unity MCP.
- [x] 41 actual CharacterController movement checks passed: 5 building through-passages, 12 loop edges, 2 ship entry/exit paths, 22 detour/shortcut segments.
- [x] Unity scene overview and Play Mode spawn rendering inspected. One camera and parcel dimensions confirmed.
- [ ] Human E/Q carrying feel, jump clearance, travel fatigue and lure-loop enjoyment.
- [ ] Enemy pursuit, networking and a complete delivery cycle on this map.

Automated passage checks use an empty-hand employee's physical movement. They do not certify cargo rotation and carrying collisions in every direction or actual cooperative play. Traversability does not demonstrate that building roles are fun.

Local evidence: `artifacts/cinder-blockout/passage.txt`, `overview.png`, `spawn.png`. Automated checks and human evaluation remain separate.

- [x] Separate Windows build succeeded. Evidence: artifacts/cinder-blockout/build-success.txt and a successful Unity BuildReport.


The standalone Windows window also displayed the Korean HUD, parcel and buildings. No startup exceptions occurred, but existing URP warnings about stripped DepthOfField/Panini post-processing shaders remain. Human validation of language switching and the full E/Q controls remains outstanding.

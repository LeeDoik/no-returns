# CINDER DEPOT primitive blockout

[한국어](cinder-blockout.ko.md)

The current environment is [CinderCompactSiteReview with the whole map and props](cinder-compact-site.en.md). After user approval of the whole-map direction, place 47 prop groups in the same scene. Retain 94 movement segments and 17 four-body lanes; pass 5,904 prop-adjacent carrying poses and actual carrying checks. Preserve original, gray, appearance and earlier maze scenes. Values and 41 original straight routes below are historical, not current acceptance criteria. Prop quality, human passing, receipt facilities and gameplay integration remain.

## 2026-10-01 — Gray structure review scene

[First-asset production/review](cinder-asset-prep.en.md): assembled the warehouse exterior from prefabs for 5 structural units/11 variants in separate `CinderStructureReview`. Original scene unchanged. The review disables 10 original warehouse Renderers, retains 10 Colliders and adds no Colliders. Start 3m before the south entrance; carry cargo through the first 6m and return backwards. Passed Blender/FBX and Unity import, passage/jump/cargo-pose and Mac Editor Play E/Q checks. The user found warehouse size acceptable; retain the current 14.4×20.4m. Gray automated checks are not final-quality approval. Follow the latest record above for approved concepts applied in a separate scene. User carrying/joint and applied-appearance quality, standalone builds and networking/AI/delivery remain unverified. The existing blockout-generation menus below do not build this review scene.

## 2026-10-01 — Pre-production preparation record

The [first 8 warehouse-unit brief](cinder-asset-prep.en.md) specifies trial dimensions, pivots and budgets for 5 structural units (floor, wall, door frame, ceiling and corner/end finish), followed by 3 presentation units (light, sign and empty rack). Compared the saved south entrance center (-18.6,0,-8.4)m, existing 3.2×3.3m opening and 4m ceiling underside against code. The first 6m inside is a proposed review segment, not a new corridor or route change. That task prepared documents only. Use the entry above and its linked production guide for the current structural-production/review state.

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

2026-09-26 correction: this scene is the current development target. On Mac, open `CinderDepotBlockout.unity` above in Unity instead of using the Windows `.cmd`. See the [Mac CLI guide](macos-development.en.md). Only opening the existing scene and checking its hierarchy have been verified; Mac Play mode/builds remain unverified. The initial Mac environment checks of `CarryRoom` do not validate this scene.

## 2026-09-26 — Create new art, then complete the scene

User correction: the required art resources still need to be created. Existing trial models and reuse candidates in older inventories are not finished resources for the current Cinder scene. The goal is to create and apply art suited to this scene and complete one environment. The initial area and production batch below are proposed sequencing, not approval of new designs or completed production.

1. **Production baseline:** check wall, doorway, passage and ceiling dimensions and cargo clearance in the current blockout. The existing 3.2m door width, 3.3m door height and 4m ceiling underside are structural trial values, not newly approved release specifications. Use the ivory panels, rust-red bands, dark frames and warm work lights of the [existing PSX environment concept](../art/space-concepts/environment-kit-01.png) as style references.
2. **Proposed first batch:** walls, corners, door frames, floors, ceilings/beams, work lights, cargo racks and signs for the warehouse entrance and connecting passage. Prepare production concepts with dimensions, joining faces and pivots, following the established [structure-first sequence](art-structure-first.en.md).
3. **Model production and a small area:** build dimension-critical structures in Blender and align shared materials and low-resolution textures. Produce individual Tripo appearances where needed after structure/concept review. Check assembly, lighting, first-person views and cargo clearance in one Unity area.
4. **Expand across the scene:** apply the same component family to all 5 buildings, then produce required receiving-area, suppression-facility, outdoor-ground, rock and ship-finish assets in sequence. Preserve existing routes and collision criteria and tune dressing density per area.

Completion means consistent environment art, intentional treatment of exposed placeholder structures, readable first-person routes/objectives, preserved carrying/access and the user's review of actual rendered views. Enemy AI, online play and delivery logic require separate completion checks. This task only reviewed records/concepts and documented the production sequence; no new images, models or materials, spending, code/scene changes or play verification.

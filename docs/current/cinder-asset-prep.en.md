# CINDER DEPOT — First asset-production preparation

[한국어](cinder-asset-prep.ko.md)

**Follow the [four-abreast interior maze](cinder-maze-layout.en.md) for current added structure and next work.** Reuse aged appearance and add 24 partitions defining 3m passages. Direct narrow-layout review takes priority over terminal production.

2026-10-01 · CINDER-ASSET-PREP-01 · **User approval of aged warehouse style, shared appearance expanded to 5 buildings and automated checks passed / user review of the expansion incomplete.** Follow [current map scope, review scene and next production](cinder-map-appearance.en.md). Below are specifications, production and checks for the first warehouse batch.

## What to make first and why

**Use the warehouse's south entrance and the first 6m inside as the review area.** This exposes repeated walls, floor, ceiling and an entrance together, allowing joints, color and carrying clearance to be checked before extending the kit to other buildings. The warehouse currently has no separate interior corridor. The 6m describes a proposed review length; it does not authorize new corridor walls or narrower existing routes.

1. **5 structural units:** floor → straight wall → door frame → ceiling/beam → corner/end finish. Establish the shared cross-section and entrance first.
2. **3 presentation units:** work light → sign → empty cargo rack. Add entrance identification and signs of use while keeping racks away from the doorway.
3. Review structural play and appearance in the reference area before extending to the 5 buildings. The next functional assets are the receipt terminal/receipt floor marking, then suppression hardware/warning signals. Employee, hands, Listener and outer-creature rigs remain separate preparation tasks.

The first batch contains **8 production units**. Corner/end and ceiling/beam variants mean this is not an FBX, mesh or placement count. R reuse labels in the [older 44-unit demo list](demo-art-list.en.md) do not establish completed assets for current Cinder. This batch proposes newly made candidates; old Selected models serve only as appearance/defect references.

## References from the current structure

Evidence: [saved scene](../../NoReturns/Assets/_NoReturns/Scenes/CinderDepotBlockout.unity), [builder](../../NoReturns/Assets/_NoReturns/Editor/CinderBlockoutBuild.cs), [movement/carrying code](../../NoReturns/Assets/_NoReturns/Runtime/CinderBlockoutWalk.cs). Compared warehouse child Transforms in the saved scene with builder values. Preparation used static comparison; production validation below now includes Mac Editor Play in a separate review scene.

| Item | Current trial value | Preserve during production |
|---|---|---|
| Warehouse floor | 14.4×20.4m, upper surface Y=0m, thickness 0.2m | Finishes must not introduce thresholds or steps. |
| Warehouse wall | Height 4m, thickness 0.3m | Preserve existing collision regions and assembly centerlines. |
| South entrance center | Unity (X,Y,Z)=(-18.6,0,-8.4)m | +Z faces into the warehouse. Position describes the current scene. |
| Door opening | Clear width 3.2m, height 3.3m | Decorations and corners must not intrude into the opening. |
| Ceiling | Underside Y=4m, roof thickness 0.3m | New beams must not reduce overhead clearance. |
| Employee | Height 1.8m, radius 0.34m | Use the actual CharacterController for structural checks. |
| View | Eye height 1.57m, FOV 80° | Compare empty-handed and cargo-carrying views. |
| Carried cargo | 0.8×0.65×0.65m | Test camera-front carrying, rotation and wall approach through actual controls. |

2026-10-01 user feedback: “I checked the warehouse. This size looks fine.” Record positive feedback on warehouse size and retain the current 14.4×20.4m floor. No code, scene or asset dimensions changed. This does not extend approval to cargo visibility, edge rotation, wall approach, joint quality or new appearances.

Door, ceiling, employee and cargo values are existing trial values, not final dimensions approved by the user. The historical 41 passage checks do not validate new visuals, cargo rotation or cooperation.

## Per-component production instructions

Units are meters; dimensions below use **Unity X×Y×Z**. Blender uses Z-up and Unity Y-up, so check axes, rotation and units through export/reimport. The [Blender production script](../../art/cinder-kit-01/build.py) creates 5 gray structural units with 11 variants. It uses the names and trial dimensions below. After approval, the [appearance producer](../../art/cinder-kit-01/build_appearance.py) added shared surfaces, light, sign and an empty 2-tier rack.

| Order / related ID | Source name | Trial dimensions/pivot | Shape and acceptance condition | Proposed triangle ceiling |
|---|---|---|---|---|
| 1 / ENV04 | `NR_Cinder_Floor_A` | 1.2×0.2×1.2. Pivot at walking-surface center; geometry occupies Y=-0.2~0. | Flat surface. Join 5 tiles into a 6m repetition sample and inspect seams/pattern scale. | 200 |
| 2 / ENV01 | `NR_Cinder_Wall_A` | 1.2×4×0.3. Bottom-center pivot. | Close front, back and top. Flat mating faces; shared stripe height and UV density. | 400 |
| 3 / ENV03 | `NR_Cinder_DoorFrame_A` | Outside 3.8×4×0.3; clear opening 3.2×3.3. Pivot at opening-floor center. | Frame without a threshold. Decorations stay within 0.3m sides and 0.7m header. Door leaf/movement are separate. | 1,200 |
| 4 / ENV07 | `NR_Cinder_Ceiling_A` | 1.2×0.3×1.2. Underside-center pivot, placed at Y=4. | Finish the underside. First test beam variants within existing roof thickness, retaining underside Y=4. | 600 |
| 5 / ENV02·05 | `NR_Cinder_Corner_A` / `NR_Cinder_End_A` | Thickness 0.3, height 4. Corner pivot at bottom of wall-centerline intersection. | Close a 0.3×4×0.3 corner joint and a 0.15×4×0.3 end finish. After subtracting frame/corner widths, front/rear wall spans are 5.15m=4×1.2+0.35 and side spans are 20.1m=16×1.2+0.9. Do not stretch UVs with placement scale. | 600 |
| 6 / FAC01 | `NR_Cinder_Lamp_A` | 0.6×0.2×0.18. Pivot at wall-mounting-plane center. | Separate warm emissive face and housing. Unity owns the actual Light. | 400 |
| 7 / new surface unit | `NR_Cinder_Sign_A` | 1.2×0.6×0.02. Pivot at rear mounting-plane center. | Separate plate and lettering surface. Use `WAREHOUSE` / 창고 and a delivery-direction arrow as review copy. | 12 |
| 8 / FAC02 | `NR_Cinder_Rack_A` | 2.4×2.4×0.6. Bottom-center pivot. | Empty 2-tier rack. Boxes stay separate; do not bake delivery-cargo visuals into decoration. Place outside the passage. | 1,600 |

The 1.2m grid is an initial module proposal fitting the warehouse's 14.4/20.4m floor. Do not assume every building or the 3.2m opening fits that grid. Corners, ends and beam sections now use the implementation values below; sign direction awaits review. Triangle ceilings are **proposed working budgets per source/variant**, not a performance guarantee. Current measurements are 36 triangles for the frame and 12 for every other source. Record evidence and adjust if the first sample needs more silhouette detail.

## Shared surfaces and visual references

Use the [environment sheet](../art/space-concepts/environment-kit-01.png) and [sheet review](../art/space-concepts/environment-kit-01.en.md) for style. Align ivory panels, burgundy stripes, dark frames and warm work lights; following the aging request, show discoloration, rust drips and peeling paint on broad surfaces, concentrating more grime/rust at seams, bases and contact areas. Entrances and cargo silhouettes must read first. Selecting this reference is separate from approving new appearances.

The first comparison budget is **1 shared 512×512 BaseColor**, **1 separate 256×128 sign texture**, and a solid emissive light face. Start with Point filtering as a candidate and compare mipmaps, distant shimmer and text readability in actual views. Use a shared structural surface material and a separate emissive material, retaining broad simple faces and angular silhouettes. Texture sizes, pixel density and emission intensity are trial proposals; finalize after reviewing reference-area screens.

Prepare the first 8 units for **direct local Blender authoring**. Tripo calls and paid generation are not required for this batch. Test new structures in Unity and incorporate user structural feedback, then review new appearances using front, rear, side, top and first-person views from the same model. Check dimensions in the model rather than image lettering or proportions.

## Deliverables and destinations

Preserve the gray sources and review scene below. Separate appearance, texture and 3-unit deliverables are linked in the latest application record below.

- `art/cinder-kit-01/`: `build.py`, structural source `Cinder_Kit_Structure.blend`, UV/reimport/dimension/Play results and review images from the same model. Per-component FBXs are output directly to the Unity asset path below. Do not overwrite ship or Selected sources.
- `NoReturns/Assets/_NoReturns/Art/CinderKit01/`: 11 gray structural FBXs, 1 shared solid-color material and Unity-generated `.meta` files. No textures.
- `NoReturns/Assets/_NoReturns/Prefabs/CinderKit01/`: 11 review prefabs with visual models as children. These prefabs own visuals only; the separate review scene retains 10 original warehouse Colliders. No new Colliders, lights or display functionality. Assemble through Editor/CLI; do not hand-edit YAML.
- Bind dimensions, pivots, triangles, UVs, material slots, provenance, check results and user feedback to each source name. Record direct-authoring provenance for new local geometry/textures. Check usage rights and license before introducing external files. Existing concepts are references.
- Models, images, textures and `.blend` use existing `.gitattributes` Git LFS rules; production scripts, JSON and bilingual documents use ordinary Git. Exclude builds, logs, caches, credentials and personal settings.

Current tool check: local `blender --version` reports **5.2.2 LTS**. Unity target **6000.6.0f1 / URP 17.6.0** comes from the [version file](../../NoReturns/ProjectSettings/ProjectVersion.txt) and [packages](../../NoReturns/Packages/manifest.json). `game-dev` is absent from the current PATH, so that CLI's inspection, normalization and packaging route is unavailable. Preparation uses the existing local authoring route without installing tools or substituting a generation service. Verified Blender production, FBX roundtrip and Mac Editor Play in the separate structure review scene. Standalone Mac/Windows builds were not tested.

## Next task and acceptance gates

Preserve the first warehouse batch's **gray sources/assembly sample for 5 structural units** and south-entrance/first-6m records. Following aged-style approval, expanded [shared appearance across 5 buildings](cinder-map-appearance.en.md), then built the requested [narrow interior maze](cinder-maze-layout.en.md) in a separate scene. Next: maze control/passing-feel review, then BAY 04 receipt terminal/floor marking. Existing Create/Build Cinder menus regenerate scenes; do not run them on manually authored art-review scenes.

| Review view | What to inspect |
|---|---|
| 3m in front of the south entrance, eye height 1.57m | Entrance recognition; signs/lights explain the route. |
| Enter carrying cargo; rotate left/right/up/down | Frame, ceiling and corners do not intrude into the 0.8×0.65×0.65m cargo or view. |
| Look back at the entrance from 6m inside | Wall backs, ceiling underside, end finish and repeated floor seams. |
| Repeat these views after adding rack/light | Storage and carrying routes remain distinct; bright surfaces do not hide cargo outlines. |

- [x] Compared current scene, builder/controls, environment concept and production guides.
- [x] Specified 8 priority units and names, trial dimensions, pivots, surface budgets and review positions for the 5 structure-first units.
- [x] Checked units, axes, dimensions, openings, pivots, UVs, closed surfaces, normals and reimport for 11 structural sources/FBXs.
- [x] Checked reimported size/material slots. Disabled 10 original warehouse Renderers and retained 10 Colliders; the new assembly has 0 Colliders.
- [x] Verified automated passage/jump/cargo-pose checks and E/Q, carrying passage/return and empty-handed jumping in Mac Editor Play.
- [x] The user inspected the warehouse size and found it acceptable. Retain the current 14.4×20.4m.
- [ ] Obtain user feedback on cargo visibility, edge carrying-rotation, wall approach and joint quality.
- [x] Generated, inspected and saved an entrance appearance proposal and 8-unit multi-view component sheet using actual structure references.
- [x] User approved the concept direction for new colors, textures, light, sign and rack.
- [x] Applied shared surfaces and the 3 presentation units, repeated empty-handed/carrying views and automated checks.
- [x] Expanded shared appearance across 5 buildings after user approval of the aged map style. User review of the expansion remains.

Networking, enemy AI and delivery judgement are not connected to the current Cinder trial. Passing this structural area cannot complete the whole game, cooperation, enjoyment or release quality.

## 2026-10-01 — Carrying edge fix and appearance proposals

Produced and inspected the [entrance proposal and 8-unit sheet](../art/cinder-appearance-01.en.md). Connected exact built-in imagegen prompts, provenance and hashes; approval and application had not yet occurred at proposal generation. Follow the later approval/production record below for current status. Do not use generated perspective/opening proportions as dimensional validation.

Expanded carrying checks to 14 positions×8 yaw angles×3 pitch angles. The [pre-fix result](../../art/cinder-kit-01/carry-edge-before.json) found door-edge/wall overlaps in 38 of 336 samples. The original handling started BoxCast from an overlapping eye position and forced a minimum 0.15m displacement. `TrialCargoPose.Position` in the [shared carrying calculation](../../NoReturns/Assets/_NoReturns/Runtime/CinderBlockoutWalk.cs) separates a cargo-height start point for up to 4 passes before moving toward obstacles. Temporarily enable the query shape and exclude it on layer 2; restore its original Collider/layer state in `finally`. The [ship interior trial](../../NoReturns/Assets/_NoReturns/Runtime/ShipInteriorTrial.cs) and [structure checker](../../NoReturns/Assets/_NoReturns/Editor/CinderStructureBuild.cs), which used the same original handling, now share this calculation. Removed the copied old formula from actual carrying-position validation.

[Final Play check](../../art/cinder-kit-01/carry-edge-validation.json): zero overlaps in 336 samples; passed E pickup, W passage, S backward return, Q drop and empty-handed Space jump. The [runnable check](../../tools/unity_checks/CinderCarryEdgeCheck.cs) queues Input System key states and invokes the actual `Update`, with API-set poses. This is not real-time human manual control. Repassed 11 model imports, 6 forward/backward passages, 3 jump positions and 48 center-lane cargo poses. [Compilation/console evidence](../../art/cinder-kit-01/appearance-checks.json): zero compilation and console errors; existing deprecated warnings remain.

Actual gray views produced by the check: [door-edge carrying](../../art/cinder-kit-01/carry-edge-door.png) · [carrying directly before a wall](../../art/cinder-kit-01/carry-edge-wall.png). Inspected joints and partial cargo visibility. Cargo falls below the frame when directly against a wall, so user evaluation of this close view remains outstanding. Saved warehouse/interior scenes and the 8 existing materials were not changed. Ship-scene Play revalidation, standalone Mac/Windows builds, performance and networking/AI/delivery were not tested in this task.

Rerun: enter Play in `CinderStructureReview`, then run `unity command run_script --project-path NoReturns --file ../tools/unity_checks/CinderCarryEdgeCheck.cs --caller plugin --skill unity-cli` from the repository root. Relative `--file` resolves against the Unity project `NoReturns/`; stop Play afterward. Shared surfaces and the 3 presentation units were produced after approval; rerun the separate appearance-scene checks below.

## Location of existing gameplay systems

| Scene/code | Current role |
|---|---|
| [CarryRoom](../../NoReturns/Assets/_NoReturns/Scenes/CarryRoom.unity) / [CarryRoom code](../../NoReturns/Assets/_NoReturns/Runtime/CarryLab/CarryRoom.cs) | Existing carrying, delivery, Listener, baton and cooperative gameplay. The existing Windows [launcher](../../06_Play_Listener_Test.cmd) enables Listener mode with `--hazard`; ordinary Editor Play uses basic carrying mode. |
| [CarryThreat](../../NoReturns/Assets/_NoReturns/Runtime/CarryLab/CarryThreat.cs) / [BatonVisual](../../NoReturns/Assets/_NoReturns/Runtime/CarryLab/BatonVisual.cs) / [BatonFeedback](../../NoReturns/Assets/_NoReturns/Runtime/CarryLab/BatonFeedback.cs) | Preserved Listener movement, pursuit, attack and suppression plus baton appearance, charge and feedback code. |
| [CarryMission](../../NoReturns/Assets/_NoReturns/Runtime/CarryLab/CarryMission.cs) / [ShipInteriorTrial](../../NoReturns/Assets/_NoReturns/Scenes/ShipInteriorTrial.unity) | Existing ship route, return and settlement logic plus a separate interior structural trial. That interior model also remains in Cinder's layout. |
| `CinderDepotBlockout` / `CinderStructureReview` | Map-space trial / new gray-asset review. Previous `CarryRoom` enemy, baton and delivery logic is not connected yet. |

This change does not delete or replace existing runtime code. Listener navigation grids and ship/delivery judgement coordinates target the previous trial map; connecting them to Cinder remains separate work. Opening an asset scene or simply adding a `CarryRoom` component does not complete integration. Existing Listener-mode Mac execution was not rechecked in this task.

## 2026-10-01 — Gray production and review instructions

Open the [review scene](../../NoReturns/Assets/_NoReturns/Scenes/CinderStructureReview.unity) and press Play. Start 3m before the south entrance with cargo ahead-right. WASD moves, mouse looks, E picks up, Q drops, Space jumps empty-handed, F1 switches languages and Esc releases the cursor. Enter 6m, look back, or return backwards carrying cargo. The byte hash of original `CinderDepotBlockout` matches task start, and all 8 pre-existing ship-material changes were preserved.

The [Unity builder/checker](../../NoReturns/Assets/_NoReturns/Editor/CinderStructureBuild.cs) exposes `NO RETURNS/Trials/Create Cinder Structure Review`; save the current scene and stop Play before running it. It **regenerates the review scene**, so preserve manual edits in a separate copy first. `Validate Cinder Structure Review` reruns unit/pivot/UV/material, passage, jump and cargo-pose checks. Regenerate Blender sources with `blender --background --python art/cinder-kit-01/build.py`.

Implemented 5 production units as **11 sources/FBXs/prefabs**: Floor_A, Wall_A, Wall_Fill035, Wall_Fill090, DoorFrame_A, Ceiling_A, Ceiling_Edge, Ceiling_Corner, Beam_A, Corner_A and End_A. The corner is a 0.3m square joint at intersecting wall centerlines, the end is 0.15m wide, and the beam is 1.2×0.3×0.3m. Beam/end sources and prefabs are not placed in this closed warehouse. The 0.15m ceiling edge variants close the original 14.7×20.7m roof outline. All assembly instances use placement scale 1.

[Blender results](../../art/cinder-kit-01/validation.json): passed dimensions, pivots, UVs, closed meshes, positive volume, FBX reimport and triangle/material-slot agreement for 11 parts. The frame has 36 triangles; every other source has 12. [Unity assembly measurements](../../art/cinder-kit-01/unity-assembly.json): 530 visual instances, 6,408 triangles, 10 retained warehouse Colliders, 0 original active Renderers and 0 new Colliders. These are mesh measurements, not FPS/final-performance validation.

[Unity structural checks](../../art/cinder-kit-01/unity-validation.txt): 11 models, 6 forward/backward passages across 3 lanes, 3 empty-handed jump positions and 48 center-lane cargo poses passed. Cargo checks use the actual carrying BoxCast position across 4 locations×4 yaw angles×3 pitches (-80/0/80°). They do not establish user-controlled rotation along every edge or wall approach. The initial passage check hit the dropped cargo Collider; the checker now disables/restores it during the carrying test, matching actual carrying, and rebuild/revalidation passed.

[Mac Editor Play results](../../art/cinder-kit-01/play-validation.json): API-injected Input System key states exercised actual Update handling of E pickup, W carrying passage, S backwards return, Q drop and empty-handed Space jump. Jump rise was 0.634135962m. Reference camera positions/angles were set by API; this was not a human manual playtest. Zero compile errors, 7 existing deprecated-API warnings and zero new Play errors/warnings. Standalone builds, networking, AI, delivery, final appearance and user approval remain unverified.

Review images: [same-source component sheet](../../art/cinder-kit-01/structure-sheet.png), [empty-handed entrance](../../art/cinder-kit-01/entry-empty.png), [carrying entrance](../../art/cinder-kit-01/entry-carry.png), [carrying rear view from 6m inside](../../art/cinder-kit-01/inside-rear-carry.png). Unity images render the actual Play camera without IMGUI instructions. Solid gray structures are not final-art concepts.

Check bilingual links, values, names, budgets, checkbox states and Git LFS. Full documentation retains **142** evidence links to locally absent `artifacts/` from task start, with no new failures. Do not commit builds, logs, caches or personal settings. [Validation checklist](05-validation.en.md) · [Git operating rules](version-control.en.md).

Raw staged `git diff --check` reports 147 trailing-space locations in empty fields of native Unity-generated YAML/meta. Scoped code/document whitespace checks and the full staged check with only end-of-line whitespace excluded pass. Native generated files were not hand-edited solely to satisfy whitespace checking.

## 2026-10-01 — Applying the approved appearance

The user approved the [appearance concepts](../art/cinder-appearance-01.en.md): “Yes, let's go with this feel” (original: “어 이 느낌으로 가자”). Recorded in [approval provenance](../../art/cinder-kit-01/appearance-provenance.json). This approves the concept direction for color, texture, lights, sign and rack. User quality review of the applied result remains separate.

The [appearance review scene](../../NoReturns/Assets/_NoReturns/Scenes/CinderAppearanceReview.unity) is a separate copy derived from the gray scene. It retains the 14.4×20.4m warehouse, 3.2×3.3m opening, ceiling underside at 4m and 10 original warehouse Colliders. Applied surfaces to all 530 warehouse visual instances and placed the 3 presentation units at the south entrance/first 6m inside. No other buildings, new corridor or gameplay-system expansion. Only in the appearance scene, disabled Renderers on 12 existing blockout TextMeshes that showed through walls.

- Source/reproduction: [Blender producer](../../art/cinder-kit-01/build_appearance.py), [appearance source](../../art/cinder-kit-01/Cinder_Kit_Appearance.blend), [Unity import/assembly/checks](../../NoReturns/Assets/_NoReturns/Editor/CinderAppearanceBuild.cs). Run `blender --background --python art/cinder-kit-01/build_appearance.py`, then Unity's `NO RETURNS/Trials/Create Cinder Appearance Review`. The menu stops if the appearance scene exists. Preserve manual edits in a separate scene before explicitly invoking `CinderAppearanceBuild.Create(true)` to regenerate. No manual YAML edits.
- `NoReturns/Assets/_NoReturns/Art/CinderAppearance01/`: 11 structural and 3 presentation FBXs, 14 total; initially directly painted shared 512×512 BaseColor and 256×128 `WAREHOUSE`/arrow texture; 3 URP Lit materials. No concept cropping, external fonts/models or paid generation. Texture import uses sRGB, Point, mipmaps, Clamp and no compression; metallic 0, Smoothness 0.05. Initial wall stripe Y=1.10..1.75m; base wear at Y≈0.22..0.30m. Current textures were replaced by the revision below; the stripe range is a generation target rather than an exact measured paint boundary. These trial values come from the [producer](../../art/cinder-kit-01/build_appearance.py) and [Unity check](../../art/cinder-kit-01/production-unity-validation.json).
- `NoReturns/Assets/_NoReturns/Prefabs/CinderAppearance01/`: 14 prefabs. Light 0.6×0.2×0.18m, sign 1.2×0.6×0.02m, rack 2.4×2.4×0.6m. Unity owns 5 work-light Point Lights: color (1,0.67,0.3), intensity 0.65, range 5m, no shadows. Sign arrow points to the entrance. Rack center (-25.2,0,-5.4)m, yaw 90°, shelf-center heights 0.22/1.32m; 2 levels without cargo.
- Only the rack owns 1 BoxCollider covering its reserved storage volume. Rack interaction is absent; use individual shelf/post collisions when needed. Lamps, sign and structural visuals have no new Colliders. [Measured](../../art/cinder-kit-01/production-unity-validation.json): 6,408 structural and 216 presentation triangles, 7 prop placements. This does not establish FPS or final performance.

[Blender checks](../../art/cinder-kit-01/production-validation.json): passed units, dimensions, pivots, UVs, closed surfaces, positive volume, FBX roundtrip and material slots for 14 FBXs. The first Unity import projected the lamp in the wrong direction; corrected prop-axis mapping. Verified sign text/arrow direction in actual views. [Unity checks](../../art/cinder-kit-01/production-unity-validation.json): 14 imports, texture settings and placement/collision/light counts passed. [Structural rerun](../../art/cinder-kit-01/production-structure-validation.txt): baseline checks on 11 gray prefabs and 6 passages, 3 jumps and 48 center-lane cargo poses in the new appearance scene passed.

[Mac Editor Play checks](../../art/cinder-kit-01/production-carry-edge-validation.json): 18 entrance/wall/rack approach positions×8 yaw×3 pitch, **0 actual penetrations across 432 poses**; E/W/S/Q and empty-handed jump passed. 6 OverlapBox candidates were exactly touching boundaries with no positive ComputePenetration depth. The [runnable check](../../tools/unity_checks/CinderCarryEdgeCheck.cs) refines candidates with native penetration checks, tolerance 0.00001m. Existing carrying runtime code is unchanged. Automated keys/API poses are not real-time human manual play.

Actual Play camera: [empty-handed entry](../../art/cinder-kit-01/production-entry-empty.png) · [carrying entry](../../art/cinder-kit-01/production-entry-carry.png) · [carrying back toward entrance from 6m inside](../../art/cinder-kit-01/production-inside-rear-carry.png) · [empty rack](../../art/cinder-kit-01/production-rack-empty.png) · [door edge](../../art/cinder-kit-01/production-carry-edge-door.png) · [near wall](../../art/cinder-kit-01/production-carry-edge-wall.png). Waited at least a frame before capture to prevent images using the previous cargo transform. IMGUI instructions are excluded. Reduced Point intensity from 1.8→0.65 after inspecting overbright initial views. Checked bright surfaces, cargo silhouette, sign and empty 2-tier rack; user assessment, front/rear/side/top quality review of every part, distant shimmer and performance remain outstanding.

Rerun: open `CinderAppearanceReview`, invoke `CinderAppearanceBuild.Validate()` and `CinderStructureBuild.Validate()`. Enter Play and run `unity command run_script --project-path NoReturns --file ../tools/unity_checks/CinderCarryEdgeCheck.cs --caller plugin --skill unity-cli`, then stop Play. Appearance receipts use the `production-` prefix without overwriting gray evidence. [Compilation/console/preservation hashes](../../art/cinder-kit-01/production-checks.json): 0 compile errors, 0 errors/warnings in the final Play console. Original blockout/gray scenes and the 8 pre-existing ship-material edits match starting hashes. Standalone Mac/Windows builds, ship Play, networking/AI/delivery and whole-game quality were not checked in this task.

## 2026-10-01 — Aged texture revision

The user found the applied colors too clean (original: “너무 깔끔”) and requested a dirty, aged appearance like the approved concept. This is corrective feedback on the first application, distinct from quality approval of the revision. Added yellowed walls/rust drips, peeling red paint, grime/rust at seams and bases, floor wear/dirt, ceiling stains and sign-border corrosion. Retained lighting and geometry.

Edited the existing atlas/sign with built-in imagegen using the approved entrance concept as a texture reference. Preserved the 1254×1254 [atlas source](../../art/cinder-kit-01/aged-atlas-source.png), 1774×887 [sign source](../../art/cinder-kit-01/aged-sign-source.png) and [actual prompts/reference/output hashes](../../art/cinder-kit-01/aged-texture-provenance.json). Used only native Blender image resizing to normalize runtime textures to 512×512 / 256×128. Retained UV regions/texture GUIDs; no concept cropping, external generation service or Tripo calls.

Reproduce surfaces only: `blender --background --python art/cinder-kit-01/build_appearance.py -- --surfaces-only`. Updates textures and the packed appearance Blender source without re-exporting existing FBXs. Run existing validation menus after Unity `AssetDatabase.Refresh()`. Did not run the scene-generation menu. [Preservation/check receipt](../../art/cinder-kit-01/aged-checks.json): 14 FBXs, 14 prefabs, 3 materials, 3 scenes and 8 pre-existing ship-material edits, 44 files total, match starting hashes. No changes to dimensions, opening, collision, lights, placement or carrying runtime.

With current textures, passed 14 Blender reimports, Unity imports/surface settings, 6 passages, 3 jumps and 48 center-lane cargo poses. Mac Editor Play automated checks passed 432 poses with 0 penetrations, E/W/S/Q and empty-handed jumping. Current compilation status reports 0 errors; Play console has 0 errors/warnings. No forced new compilation or real-time human input test. Inspected 6 actual camera views. [Current entry](../../art/cinder-kit-01/production-entry-empty.png) and [current rack](../../art/cinder-kit-01/production-rack-empty.png); first-application comparisons are preserved as [entry](../../art/cinder-kit-01/clean-entry-before.png) and [rack](../../art/cinder-kit-01/clean-rack-before.png).

User quality feedback on the revision, all-part multi-view review, distant patterns, performance, standalone Mac/Windows builds, ship Play and networking/AI/delivery remain unverified. Documentation retains 142 existing missing artifact links with no new failures.

2026-10-01 user feedback: “Let's build the map with this feel and move on” (original: “그래 이런 느낌으로 맵을 구성하자. 다음으로 넘어가자”). Approved the aged revision as the map style reference. Subsequently [expanded shared appearances to 5 buildings](cinder-map-appearance.en.md); user review of the expansion is separate. Earlier incomplete approval records retain their historical status.

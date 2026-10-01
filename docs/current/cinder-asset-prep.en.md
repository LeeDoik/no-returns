# CINDER DEPOT — First asset-production preparation

[한국어](cinder-asset-prep.ko.md)

2026-10-01 · CINDER-ASSET-PREP-01 · **Gray structures, automated checks and user size review complete / carrying and joint-quality review and appearance production incomplete.** The current target is the [Cinder blockout](cinder-blockout.en.md). Follow the [structure-first production guide](art-structure-first.en.md). New module dimensions and budgets below are initial trial proposals, not release specifications.

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

Units are meters; dimensions below use **Unity X×Y×Z**. Blender uses Z-up and Unity Y-up, so check axes, rotation and units through export/reimport. The [Blender production script](../../art/cinder-kit-01/build.py) creates 5 gray structural units with 11 variants. It uses the names and trial dimensions below; light, sign and rack remain unproduced.

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

Use the [environment sheet](../art/space-concepts/environment-kit-01.png) and [sheet review](../art/space-concepts/environment-kit-01.en.md) for style. Align ivory panels, burgundy stripes, dark frames and warm work lights; concentrate rust/wear at lower edges and contact areas. Reduce the dense stains remaining across broad floors/walls in the old sheet. Entrances and cargo silhouettes must read first. Selecting this reference is separate from approving new appearances.

The first comparison budget is **1 shared 512×512 BaseColor**, **1 separate 256×128 sign texture**, and a solid emissive light face. Start with Point filtering as a candidate and compare mipmaps, distant shimmer and text readability in actual views. Use a shared structural surface material and a separate emissive material, retaining broad simple faces and angular silhouettes. Texture sizes, pixel density and emission intensity are trial proposals; finalize after reviewing reference-area screens.

Prepare the first 8 units for **direct local Blender authoring**. Tripo calls and paid generation are not required for this batch. Test new structures in Unity and incorporate user structural feedback, then review new appearances using front, rear, side, top and first-person views from the same model. Check dimensions in the model rather than image lettering or proportions.

## Deliverables and destinations

Structural sources and a separate review scene were created in these locations. Appearance, textures and the 3 presentation units remain future work.

- `art/cinder-kit-01/`: `build.py`, structural source `Cinder_Kit_Structure.blend`, UV/reimport/dimension/Play results and review images from the same model. Per-component FBXs are output directly to the Unity asset path below. Do not overwrite ship or Selected sources.
- `NoReturns/Assets/_NoReturns/Art/CinderKit01/`: 11 gray structural FBXs, 1 shared solid-color material and Unity-generated `.meta` files. No textures.
- `NoReturns/Assets/_NoReturns/Prefabs/CinderKit01/`: 11 review prefabs with visual models as children. These prefabs own visuals only; the separate review scene retains 10 original warehouse Colliders. No new Colliders, lights or display functionality. Assemble through Editor/CLI; do not hand-edit YAML.
- Bind dimensions, pivots, triangles, UVs, material slots, provenance, check results and user feedback to each source name. Record direct-authoring provenance for new local geometry/textures. Check usage rights and license before introducing external files. Existing concepts are references.
- Models, images, textures and `.blend` use existing `.gitattributes` Git LFS rules; production scripts, JSON and bilingual documents use ordinary Git. Exclude builds, logs, caches, credentials and personal settings.

Current tool check: local `blender --version` reports **5.2.2 LTS**. Unity target **6000.6.0f1 / URP 17.6.0** comes from the [version file](../../NoReturns/ProjectSettings/ProjectVersion.txt) and [packages](../../NoReturns/Packages/manifest.json). `game-dev` is absent from the current PATH, so that CLI's inspection, normalization and packaging route is unavailable. Preparation uses the existing local authoring route without installing tools or substituting a generation service. Verified Blender production, FBX roundtrip and Mac Editor Play in the separate structure review scene. Standalone Mac/Windows builds were not tested.

## Next task and acceptance gates

Produced **gray sources and an assembly sample for the 5 structural units**. `CinderStructureReview` preserves original Cinder and assembles the entire warehouse exterior; the south entrance and first 6m inside are user review points. No new corridor or building expansion. Next are shared surfaces and the 3 presentation units after user structural review. Existing Create/Build Cinder menus regenerate the scene; do not run them on a manually authored art-review scene.

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
- [ ] After user structural review, review new appearances in production-oriented multi-view/gameplay images and record approval status.
- [ ] Recheck the reference area with shared surfaces and 3 presentation units before extending to other buildings.

Networking, enemy AI and delivery judgement are not connected to the current Cinder trial. Passing this structural area cannot complete the whole game, cooperation, enjoyment or release quality.

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

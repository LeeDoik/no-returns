# CINDER — Whole-site layout inside the suppression field

[한국어](cinder-compact-site.ko.md)

## 2026-10-01 — Open sky and zone boundaries

The user found the map too maze-like, so open **82.8m²** of west/north perimeter-route cover: west 3×14.4m and north 13.2×3m. Split the original 2 ceiling BoxColliders into 4 matching the retained covered pieces, removing invisible ceilings in the openings. Preserve 3/3.15m ground passages, 4 loops, buildings/ship/props and lighting. No game-version change.

Distinguish paving within the 53.55×65.4m field from rough mineral ground outside, adding flat rust-colored boundary bands and 2 physical labels. English source strings are `FIELD / INTERIOR` and `OUTER / BASIN`. Add **4 service pads measuring 2.4×2.4m** around the corner suppressors, retaining approach space. Boundaries/pads are visual markings and add no ground colliders or suppression gameplay.

Review also found regular rock rows and blue suppressor blocks. Apply fixed seed 137 to irregular positions/yaw and distant peak/ridge selection for the existing 59 rocks. Hide only the original 4 suppressor Renderers, adding aged masts, cabinets and small green signal visuals inside their original 1×5×1m collision envelopes. Signals reuse the existing Unlit material and are not new Lights. Produce 4 native meshes (mast/signal/open-cover yard surfaces/boundary and pads), reusing existing meshes/materials/label tooling. Retain 69 background placements/4,624 triangles; add 8 suppressor placements/864 triangles and 3 boundary/label placements/48 triangles. Preserve original ground collision, 47 prop groups, 26 architecture placements, 40 local lights, sky, 35–115m fog and runtime.

Validation: remove roof triangles in the openings, confirm upward Raycasts hit no invisible ceiling, constrain suppressor visuals to existing collision envelopes and keep all background triangles outside the field. Pass 94 movement segments, 17 four-body lanes, 286 valid positions, 6,864 carrying poses and 94 actual carrying segments. 274,484 penetration checks, 0 overlaps, 103 contacts; E/W/S/Q and empty-hand jumping pass. Native terrain rendering changes 71,212 pixels (640×360; 10,000 threshold); reimport/reopen 4 meshes and the scene. Capture 30 views, inspect 9 key views and restore cutaway hiding. 0 compilation errors, 3 existing Editor warning types, 0 new-source warnings and final Play 0 errors/warnings. Preserve 608 of 609 starting file hashes, excluding the current scene; exclude 8 existing ship-material edits. Remove 0 original native scene IDs.

Two initial automated carrying runs reported pickup/jump input failures. Add Physics.SyncTransforms for teleported smoke-test body/cargo and stable-frame waiting, then pass reruns without changing runtime controls. Correct the first service-pad capture position that fell inside the ship. One immediate console-status query failed to connect during domain reload; the subsequent query confirmed successful compilation. Preserve historical `background-`/`architecture-`/`sky-`/`props-`/`site-` evidence; current checks write `polish-`. Documentation checking retains only 142 existing missing links, with 0 new failures. Raw staged whitespace checking fails at 74 native-generated trailing-space locations; code/document/evidence and overall checking excluding those generated spaces pass.

User appearance quality, boundary readability, human four-player passing/simultaneous carrying, performance, standalone builds, exterior creatures/suppression stages and Cinder gameplay integration remain pending. This request does not establish overall quality approval.

Both modification stages are saved in the current scene. Reproduce in order: `Polish Cinder Scenery and Suppressor Visuals` → `Open Cinder Sky and Define Zones`; each stops with unsaved edits, Play or its existing root. Manage native assets through the [production source](../../NoReturns/Assets/_NoReturns/Editor/CinderBackgroundBuild.cs). Use `Validate Cinder Exterior Background`; prop/architecture/compact-site validation menus also route to current background validation. Preserve original `ArchitectureYard.asset`, using a new mesh with only selected cover triangles removed. Retain both original background state evidence and the current state after ceiling adjustment. No new ground colliders, lights, textures, external assets, dependencies or paid generation. Run checks from the repository root after saving/stopping Play.

```bash
source ~/.unity/env
unity command eval --code 'NoReturns.Editor.CinderBackgroundBuild.Validate(); return true;' --project-path "$PWD/NoReturns" --caller plugin --skill unity-cli --format json
unity command run_script --file "$PWD/tools/unity_checks/CinderBackgroundRenderCheck.cs" --entry CinderBackgroundRenderCheck.Main --project-path "$PWD/NoReturns" --caller plugin --skill unity-cli --format json
unity command editor_play --project-path "$PWD/NoReturns" --caller plugin --skill unity-cli --format json
unity command run_script --file "$PWD/tools/unity_checks/CinderCarryEdgeCheck.cs" --entry CinderCarryEdgeCheck.Main --timeout_ms 180000 --project-path "$PWD/NoReturns" --caller plugin --skill unity-cli --format json
unity command editor_stop --project-path "$PWD/NoReturns" --caller plugin --skill unity-cli --format json
```

[Layout](../../art/cinder-kit-01/polish-layout-validation.json) · [carrying](../../art/cinder-kit-01/polish-carry-edge-validation.json) · [rendering](../../art/cinder-kit-01/polish-render-validation.json) · [preservation/check record](../../art/cinder-kit-01/polish-checks.json).

![Opened north passage](../../art/cinder-kit-01/polish-open-sky-north.png)

![Suppressor service pad](../../art/cinder-kit-01/polish-suppressor-pad.png)

2026-10-01 · CINDER-COMPACT-SITE-01 / CINDER-ARCHITECTURE-01 / CINDER-BACKGROUND-01 / CINDER-SCENERY-POLISH-01 · **Whole-site direction approved; varied architecture/background applied and automated checks passed / current appearance, visibility and human four-player review incomplete.** No game-version change.

## 2026-10-01 — Rocky territory and industrial background outside the field

The user checked the preceding building change and requested the background. Apply the barren exterior of the [suppression-field concept](../art/space-concepts/cinder-depot-suppression-01.png) in the same `CinderCompactSiteReview`. **59 rocks, 9 industrial visuals and 1 exterior surface** total 69 placements and 4,624 triangles. Layer lower nearby crags, distant peaks and northeastern silos/refinery/gantry, using retained 35–115m fog for distance. User background-quality feedback remains pending; the preceding confirmation does not establish human controls, performance or overall quality approval. No game-version change.

| Native asset | Production basis/placements |
|---|---|
| Basalt crag / Split ridge / Distant peak / Loose boulder | 4 types, 59 rocks. Nominal heights 8.5/13/23/2.1m, with irregular vertices and buried bases; check actual mesh bounds |
| Barren basin | 1 exterior surface, 300×280m. Leave the 53.55×65.4m suppression footprint empty, retaining the original floor |
| Abandoned silo / Distant refinery / Derelict gantry | 3 new types: 3 silos, 2 refineries and 1 gantry |
| Refinery exhaust | 3 reused Industrial stack meshes |

Produce 8 new native meshes, 2 materials and 1 native 128×128 RGBA32 mineral texture with the [production source](../../NoReturns/Assets/_NoReturns/Editor/CinderBackgroundBuild.cs). The fixed-seed rough mineral/gravel texture uses Repeat, Point, mipmaps and face-oriented UVs at 0.35 repeats/m. Industry reuses the aged atlas and existing mesh tool. Unit placement scale 1, 0 new Colliders/Lights, background shadow casting disabled. Hide only the Renderers of 4 gray boundary guards, retaining their fall-prevention Colliders. Preserve existing buildings, ship, 47 prop groups, 26 architectural placements, 40 local lights, sky/fog and runtime. The exterior is static scenery; no new traversal area, outer creatures or suppression behavior. The existing field line remains a visual marker rather than an implemented gameplay boundary.

Validation: all background triangles' horizontal bounds remain outside the field; finite UVs, nondegenerate triangles, upward terrain winding, scale/supported shader and retained state pass. Pass 94 movement segments, 17 four-body lanes, 286 body positions, 6,864 carrying poses and 94 actual carrying segments. Final 26,902 penetration checks yielded 0 overlaps and 103 contacts; E/W/S/Q and empty-hand jumping pass. After reimporting 8 meshes and reopening the scene, terrain visibility changes 55,914 pixels of an actual 640×360 framebuffer. The [render regression check](../../tools/unity_checks/CinderBackgroundRenderCheck.cs) fails below 10,000 changed pixels when toggling terrain visibility. Capture 25 final views, inspect 8 key views and restore cutaway hiding. 0 compile/shader errors, 3 observed existing Editor warning types, 0 new-source warnings and final Play/render-check console 0 errors/warnings.

Fix initial terrain winding. Reproduce mesh updates reaching serialized data while leaving stale rendered geometry: terrain enabled/disabled captures have identical hashes. Replace `CopySerialized` in shared `Shape.Save(replace=true)` with Clear/SetVertices/SetUVs/SetTriangles and normal/bounds recalculation on the existing Mesh, retaining its GUID and updating render buffers. Reimport alone did not resolve this; the render regression check confirms the fix. Fix inspection-camera targetTexture cleanup order. Remove 41 URP light-data components automatically added by inspection rendering; rerun and confirm the original 0-component state and saved scene. An optional temporary Python image comparison failed because PIL was unavailable; replace it with native pixel comparison. Preserve 484 of 485 starting file hashes, excluding the current scene, and exclude 8 pre-existing ship-material edits from the commit.

![Current exterior background](../../art/cinder-kit-01/background-background-panorama.png)

[Layout checks](../../art/cinder-kit-01/background-layout-validation.json) · [carrying checks](../../art/cinder-kit-01/background-carry-edge-validation.json) · [render check](../../art/cinder-kit-01/background-render-validation.json) · [preservation/check record](../../art/cinder-kit-01/background-checks.json).

### Production and checks

The `Cinder exterior background` root is already saved. `NO RETURNS/Trials/Add Cinder Exterior Background` stops in another scene, with unsaved edits, during Play or with an existing background root. Use `Validate Cinder Exterior Background`; `Validate Cinder Site Props` also routes to current background validation. Save and stop Play, then run the following from the repository root. Run the existing [shared carrying check](../../tools/unity_checks/CinderCarryEdgeCheck.cs) in Play; an existing background writes checks/captures to `background-`. Preserve earlier `architecture-`/`sky-`/`props-`/`site-` evidence.

```bash
source ~/.unity/env
unity command eval --code 'NoReturns.Editor.CinderBackgroundBuild.Validate(); return true;' --project-path "$PWD/NoReturns" --caller plugin --skill unity-cli --format json
unity command run_script --file "$PWD/tools/unity_checks/CinderBackgroundRenderCheck.cs" --entry CinderBackgroundRenderCheck.Main --project-path "$PWD/NoReturns" --caller plugin --skill unity-cli --format json
```

Full documentation fails on 142 existing missing links with no new failures. Raw staged whitespace fails on 163 trailing-space locations in Unity-generated files. Code/document/evidence checks and the overall check excluding only those generated trailing spaces pass. User background density/repetition/facility visibility, final danger-signal readability, human four-player play, performance, standalone builds, exterior gameplay and Cinder gameplay integration remain unverified. Structure/sky/prop numbers below record earlier stages; current checks use `background-`.

## 2026-10-01 — Varied building outlines and structural modules

The user requested a broader asset kit and varied building forms like the concepts instead of the current rectangular-wall layout. Produce and place **9 structural module types** in the same `CinderCompactSiteReview`. Follow the clipped corners, bent outlines and height changes in the [suppression-field concept](../art/space-concepts/cinder-depot-suppression-01.png) and [overhead concept](../art/space-concepts/cinder-depot-overhead-01.png). Change actual ground visuals/collision of Sorting island to an 8-sided chamfered outline and North control annex to a 6-vertex stepped outline with a recessed corner. Preserve interior floors, doors and wall collision of the 5 main buildings and the ship, adding distinct upper silhouettes and entry depth. No game-version change.

| Module | X×Y×Z size (m) | Placements/purpose |
|---|---|---|
| Chamfer utility | 6.3×4.3×7.2 | 1 · central 8-sided utility building |
| Stepped annex | 6.3×4.3×17.1 | 1 · northern stepped annex |
| Sawtooth roof | 7.2×1.5×6.8 | 6 · warehouse sawtooth roof |
| Vault roof | 7.2×1.9×7.2 | 4 · storage arched roof |
| L upper annex | 8×2.1×6 | 1 · office L-shaped upper room |
| Octagonal control tower | 4.8×3.4×4.8 | 1 · BAY 04 octagonal control room |
| Raised plant room | 3.9×2.2×5.4 | 3 · elevated north/service plant rooms |
| Entry hood | 4.6×0.79×1.2 | 6 · entrance canopies, minimum height 3.36m |
| Industrial stack | 1.66×4×1.66 | 2 · service exhaust stacks |

Add floor-infill and a copy of the existing yard/retained roofs excluding only replaced roofs: **11 native mesh assets** in total. The new `Cinder varied architecture` root has 26 placements including 1 floor infill, 4,021 triangles and 25 static MeshColliders. The existing surface object separately uses the yard-surface copy. Maximum height 8.3m, unit placement scale 1. Preserve the 2 original annex groups inactive in the scene, replacing rectangular collision with static MeshColliders matching the new polygons. Fill newly exposed corners/recesses; retain 47 prop groups, 6 physical labels, 40 local lights and sky/fog. Position the northern upper plant room clear of existing roof equipment.

[Producer/placement/checks](../../NoReturns/Assets/_NoReturns/Editor/CinderArchitectureBuild.cs), [reused mesh tool](../../NoReturns/Assets/_NoReturns/Editor/CinderSitePropsBuild.cs), [native asset folder](../../NoReturns/Assets/_NoReturns/Art/CinderArchitecture01/), [measurements](../../art/cinder-kit-01/architecture-layout-validation.json). Reuse the aged atlas/material. No new textures, external models, dependencies, shader or paid generation. Clip polygon tops into 1.2m tiles, fixing stretched large-triangle patterns; pass UV-density calculations on 140 Chamfer utility cap triangles. Pass reimport of all 11 new meshes and saved-scene reopening. Save native assets through Unity APIs without hand-editing YAML.

Already applied in the current scene. Save/stop Play, then run `NO RETURNS/Trials/Validate Varied Cinder Architecture`. `Validate Cinder Site Props` routes into current architecture checks without overwriting earlier props- evidence. In Play, execute the [shared carrying check](../../tools/unity_checks/CinderCarryEdgeCheck.cs) using Unity CLI `run_script`. Current results/captures use `architecture-`; preserve earlier `sky-`, `props-` and `site-`. `Add Varied Cinder Architecture` stops in another scene, with unsaved edits, during Play or with an existing architecture root. Preserve manual edits before regenerating the current layout. Sky reapplication checks preservation against the current valid-pose count rather than the previous fixed 246.

Validation: UV bounds, nondegenerate triangles, positive signed volume of static collision meshes and scale. Retain 94 ground-movement segments, 17 four-body lanes, 3/3.15m minimum widths, 4 loops and both delivery routes. Including new corner/recess samples: 286 body positions×24 directions/pitches = 6,864 carrying poses. Mac Editor Play: 94 actual carrying segments and 340,268 penetration checks with 0 overlaps, 103 contacts, tolerance 0.00001m. E/W/S/Q and empty-handed jumping passed. API poses, actual Update and key events, not real-time human controls. Capture 21 actual cameras and review 8 key views. Restore capture-only roof/upper-room hiding. 0 compile/sky-shader errors; 3 existing obsolete warning messages in this Editor recompilation, 0 new-source warnings and final Play 0 errors/warnings. Match 434 of 435 starting file hashes, excluding the current scene; exclude 8 pre-existing ship-material edits from the commit.

1 preservation check failed after Play because same-named lights enumerated in a different order. Verify all original row values as a multiset, then fix the false positive with ordinal ordering by path+position. A Python/native ordering difference required 1 further retry; final reopened-scene check passed. No actual light/collision changes. Correct the initial CLI timeout spelling to `--timeout_ms`; not a game-compilation error.

[Movement/four-body checks](../../art/cinder-kit-01/architecture-passage-validation.txt) · [carrying checks](../../art/cinder-kit-01/architecture-carry-edge-validation.json) · [preservation/check record](../../art/cinder-kit-01/architecture-checks.json). Full documentation fails on 142 existing missing links with no new failures. User quality review of building forms, canopy visibility, wayfinding and cargo rotation, human four-player play, performance, standalone builds, background quality and Cinder gameplay integration remain unverified. Upper rooms/stacks/canopies are static environment assets; no new floor access, stairs or interaction. Sky/prop/structure numbers below record earlier stages.

![Varied building silhouettes](../../art/cinder-kit-01/architecture-exterior-machinery.png)

[Chamfered utility building](../../art/cinder-kit-01/architecture-sorting-chamfer.png) · [stepped outline](../../art/cinder-kit-01/architecture-annex-step.png) · [entry canopies](../../art/cinder-kit-01/architecture-office-link.png) · [current ground-outline cutaway](../../art/cinder-kit-01/architecture-field-cutaway.png).

Raw staged whitespace fails on 201 trailing blanks in Unity-generated mesh/meta empty fields. Code/document/evidence whitespace and the full staged check ignoring only these generated trailing blanks passed. Verify 21 PNG LFS pointers and `git lfs fsck --pointers`. Did not hand-edit native YAML solely for whitespace checks.

## 2026-10-01 — Sky and distant atmosphere

At the user's request to set up the sky (original: “그래 그럼 이제 스카이 설정하자.”), apply mauve dusk, subtle clouds and distant haze in the same `CinderCompactSiteReview`. Follow the mauve-background/warm-work-light pairing in the [suppression-field concept](../art/space-concepts/cinder-depot-suppression-01.png) and [concept 02](../art/space-concepts/concept-02.png). Reject the initial builtin `Skybox/Procedural` preview because its rendered yellow horizon does not fit. The final sky uses Unity's builtin `Skybox/Cubemap` with a **64×64 pixel, 6-face RGBA32 cubemap**. Native code creates and saves static direction-space noise/color gradients without a custom shader, external imagery, dependency or weather system.

| Setting | Current value |
|---|---|
| Sky material/texture | `Cinder_Dusk_Sky.mat` / `Cinder_Dusk_Cube.asset` |
| Sky exposure/rotation | 1 / 0° |
| Cubemap top RGB / horizon RGB / bottom RGB | (0.12,0.09,0.18) / (0.34,0.27,0.38) / (0.24,0.19,0.29) |
| Directional intensity/rotation X/Y/Z | 0.55 / (24,-30,0)°; Inspector Y=330° |
| Directional RGB | (0.84,0.76,0.91) |
| Ambient | Flat, RGB (0.45,0.40,0.45) |
| Fog | Linear, RGB (0.34,0.27,0.38), start 35m/end 115m |

Keep nearby alleys, freight and labels within the fog start distance; distant facilities/background blend toward the horizon color. Preserve settings/placement of all 40 existing local lights, including interior/cabin lights. Lower directional/ambient illumination to increase warm-work-light contrast. No changes to building, ship, prop placement, collision or carrying runtime. The static sky is not connected to suppression stages or a day/night cycle. This does not complete outer terrain/creature models or light baking.

Evidence/reapplication: [sky settings, producer and checks](../../NoReturns/Assets/_NoReturns/Editor/CinderSkyBuild.cs), [sky material](../../NoReturns/Assets/_NoReturns/Art/CinderCompactSite01/Cinder_Dusk_Sky.mat), [cubemap](../../NoReturns/Assets/_NoReturns/Art/CinderCompactSite01/Cinder_Dusk_Cube.asset), [settings measurements](../../art/cinder-kit-01/sky-settings-validation.json). Save the current scene and stop Play, then run `NO RETURNS/Trials/Apply Cinder Dusk Sky` to reapply this preset/cubemap. Adjust manually in Lighting's Environment, the material and `Blockout daylight`. The menu resets manual sky tuning to the preset, so preserve required values first. It stops in another scene, with unsaved edits or during Play.

Validation: preserve original collision, 246 occupiable prop-adjacent positions and local lights; pass 94 movement segments and 17 four-body lanes. Mac Editor Play: 5,904 carrying poses, 94 actual carrying segments and 25,942 penetration checks with 0 overlaps, 57 contacts; E/W/S/Q and empty-handed jumping passed. API poses/key events, not real-time human controls. Review 18 actual camera captures of sky, alleys, carrying visibility, labels and interiors; restore inspection-only roof hiding. 0 compile/sky-shader errors, 6 existing obsolete warning types and final Play 0 errors/warnings. Match 312 of 313 starting file hashes, excluding the current scene; exclude 8 pre-existing ship-material edits from the commit. Preserve earlier `props-` evidence; route current carrying/captures into `sky-` using the saved sky-material path.

[Movement/four-body checks](../../art/cinder-kit-01/sky-passage-validation.txt) · [carrying checks](../../art/cinder-kit-01/sky-carry-edge-validation.json) · [preservation/check record](../../art/cinder-kit-01/sky-checks.json). Full documentation fails on 142 existing missing links with no new failures. User sky/fog-quality review, final danger-signal readability, human four-player play, performance, standalone builds and gameplay integration remain unverified. Prop/structure counts below record earlier work before sky setup.

![Current sky and alley](../../art/cinder-kit-01/sky-upward.png)

[Alley labels](../../art/cinder-kit-01/sky-office-link.png) · [Carrying visibility](../../art/cinder-kit-01/sky-central-carry.png) · [Interior](../../art/cinder-kit-01/sky-warehouse-racks.png) · [Exterior/distant haze](../../art/cinder-kit-01/sky-exterior-machinery.png).

Raw staged whitespace fails on 10 trailing blanks in Unity-generated cubemap/material/meta empty fields. Code/document/evidence whitespace and the full staged check ignoring only these generated trailing blanks passed. Verify 18 Git LFS PNG pointers and `git lfs fsck --pointers`. Did not hand-edit native YAML solely for whitespace checks.

## 2026-10-01 — Area-specific prop placement

The user approved the whole-site direction and asked to improve props/object placement (original: “그래 느낌 괜찮네 그러면 이제 이 맵을 더 개선해보자. 소품이나 오브젝트 배치같은거.”). Add **47 prop groups** to the same `CinderCompactSiteReview`. Preserve building positions/sizes, 3/3.15m alleys, four loops, the ship, existing movement segments and carrying runtime. User assessment of these props and real human four-player passing remain outstanding.

| Group | Count | Placement/purpose |
|---|---|---|
| Rack | 5 | Wall-side freight shelves in warehouse, BAY 04, service and storage |
| Pallet | 7 | Freight stacks in warehouse, BAY 04, storage and landing area |
| Workbench | 5 | Packing, office, dispatch and service desks |
| Cabinet | 4 | Wall-mounted power cabinets in office, BAY 04 and service |
| Drums | 3 | Warehouse, service and storage, 3 drums per group |
| Roof machinery | 6 | Equipment on closed auxiliary-building roofs |
| Pipes | 6 | High wall pipes, 2 pipes of 3m per group |
| High wall vent | 5 | High wall ventilation |
| Legend | 6 | Physical facility labels at entries and north loop |

Reuse 46 existing freight visuals and 3 CRT visuals (2 office, 1 dispatch desk). All are static props; no receipt judgement, receipt collection or E/Q interaction is connected. Retain the existing actual trial carried parcel. English production strings are `A / WAREHOUSE`, `SIDE / OFFICE`, `BAY 04 / DISPATCH`, `C SERVICE / POWER`, `STORAGE / FREIGHT` and `BAY 04 / NORTH LOOP`; `/` denotes a line break.

The [prop producer, placement and checks](../../NoReturns/Assets/_NoReturns/Editor/CinderSitePropsBuild.cs) creates 8 native meshes: pallet, cabinet, workbench, drum, vent, roof unit, pipe and label board. Save them in [CinderSiteProps01](../../NoReturns/Assets/_NoReturns/Art/CinderSiteProps01/), reusing the approved aged atlas, rack and Parcel/Receipt models. No new textures, external assets, dependencies or fonts. Labels use builtin `LegacyRuntime.ttf` and world-space UGUI without accepting clicks. The added prop group has 108 mesh placements, 205,055 triangles and 56 reserved-volume BoxColliders. Counts include instanced existing models; they are not FPS measurements or final optimization budgets. Pipe centers at 3.15/3.55m, vent bases at 2.9m and roof equipment bases at 4.3m leave alley body space clear.

A refinement check found cargo overlap in 8 poses behind/beside cabinets. Face cabinet fronts into rooms and mount their backs against walls, removing gaps that fit a body but not rotating cargo. No carrying-runtime change. Final 246 body positions×24 directions/pitches = **5,904 carrying poses**, 94 actual movement segments and **407,130 native penetration checks with 0 overlaps**, 57 boundary contacts and tolerance 0.00001m. E/W/S/Q and empty-handed jumping passed. Additional prop-end, wall and shelf samples preserve 159 original occupiable positions, 94 bidirectional movement segments and 17 four-body lanes. API-set poses and key events through actual `CinderBlockoutWalk.Update`, not real-time human controls.

Compare all original scene BoxCollider world settings outside the new prop group. Match 158 of 159 starting file hashes, excluding the 1 intentionally modified current scene. Exclude 8 pre-existing ship-material edits from the commit. Update the shared collision-snapshot helper to handle scene-root Colliders without parents. Review 17 actual camera captures, hiding roofs/roof machinery only for the cutaway and restoring them. 0 compile errors, 6 existing obsolete warning types and final Play 0 errors/warnings. A TMP-settings probe auto-imported unnecessary resources; delete only that task-created folder through native APIs. Final props do not use TMP.

Evidence: [prop measurements](../../art/cinder-kit-01/props-layout-validation.json), [structure measurements](../../art/cinder-kit-01/props-unity-validation.json), [movement/four-body checks](../../art/cinder-kit-01/props-passage-validation.txt), [carrying checks](../../art/cinder-kit-01/props-carry-edge-validation.json), [preservation, previous failure, captures and checks](../../art/cinder-kit-01/props-checks.json). Full documentation fails on 142 existing missing links with no new failures. The `site-` results below record the earlier structure task before prop placement.

![Freight shelves and stored cargo](../../art/cinder-kit-01/props-warehouse-racks.png)

[Office desks](../../art/cinder-kit-01/props-office-workbenches.png) · [Dispatch desk](../../art/cinder-kit-01/props-bay-dispatch.png) · [Facility label](../../art/cinder-kit-01/props-facility-sign.png) · [Roof/wall equipment](../../art/cinder-kit-01/props-exterior-machinery.png) · [Prop layout cutaway](../../art/cinder-kit-01/props-field-cutaway.png).

Props are already placed in the current scene. `NO RETURNS/Trials/Add Cinder Site Props` stops during Play, with unsaved changes, in another scene or when the prop group already exists, protecting manual edits. Run `Validate Cinder Site Props`, then the [shared carrying check](../../tools/unity_checks/CinderCarryEdgeCheck.cs) in Play: `props-` without the sky, `sky-` in the current sky-enabled scene. Preserve existing `site-` receipts. Next: direct prop-density/carrying-visibility assessment, BAY 04 receipt facilities and gameplay-coordinate integration. Human four-player play, AI/delivery/suppression/baton/networking, background quality, performance and standalone builds remain unverified.

Raw staged whitespace fails on 153 trailing blanks in Unity-generated scene/mesh/meta files. Code/document/evidence and the full check ignoring only these generated blanks passed. Did not hand-edit native YAML solely for whitespace checks.

## Corrected scope

The user clarified that the request concerned the entire map inside the suppression field, compact like the concept image. The previous task incorrectly narrowed only building interiors. Inspect the [suppression-field concept](../art/space-concepts/cinder-depot-suppression-01.png) and [overhead site concept](../art/space-concepts/cinder-depot-overhead-01.png), then **rearrange buildings, outdoor alleys, utility/storage masses and the ship landing area together inside the field**. The current target is [CinderCompactSiteReview](../../NoReturns/Assets/_NoReturns/Scenes/CinderCompactSiteReview.unity). The incorrectly scoped [interior maze](cinder-maze-layout.en.md) is excluded from this scene and preserved only as historical evidence.

The visual suppression field measures 53.55×65.4m; the basic perimeter-route footprint is 47.55×59.4m. Retain the original 108×86.4m review ground for surrounding context and fall prevention. Move buildings closer without reducing their individual dimensions. Move the existing ship into the southwest apron, ramp facing north. The green field line and four pylons are **boundary/location markers**; suppression operation and outer-creature exclusion are unconnected.

| Building | X×Z size (m), retained | New X,Z center (m) |
|---|---|---|
| A WAREHOUSE | 14.4×20.4 | -14.1, 1.8 |
| SIDE OFFICE | 14.4×9 | -14.1, 19.8 |
| BAY 04 | 13.8×16.8 | 12.75, 15.9 |
| C SERVICE | 12.6×22.2 | 13.35, -6.9 |
| STORAGE | 28.8×7.2 | 5.25, -24.9 |

Divide empty yards with a north control annex, L-shaped power area, west storage bays, central sorting island and eastern service housings. These comprise 6 closed auxiliary volumes; **they do not implement new enterable interiors or interactive facilities**. Standard alley width is 3m, some offset alleys 3.15m, and existing/new perimeter openings 3.2×3.3m. Connect four circulation loops around the warehouse, sorting island, eastern service building and southern storage building. The long west/north sheltered route measures 94m and the turning central shortcut 79.1m; both reach the same BAY 04 center. Actual receipt judgement is unconnected.

## Production and collision

Reuse the [approved aged appearance](cinder-map-appearance.en.md). Add 383 existing kit mesh placements for walls, corners, ends, frames and work lights, including 16 new work lights. Retain wall height/ceiling underside 4m, thickness 0.3m and placement scale 1. Yard/auxiliary roof surfaces are a native Unity mesh sampling actual `Floor_A`/`Ceiling_A` UVs at 1.2m tile density. Subtract building floors and crop edge tiles to retain atlas density and dimensions. Saved scene floors, auxiliary roofs and sheltered-route roofs remain. No external models, newly generated images or dependencies. Store one boundary-marker Unlit material and the reproducible mesh asset separately.

Preserve **local geometry/state** of 50 original building BoxColliders and ship collision while moving their groups and rotating the ship. Replace obsolete route floors/roofs and freight obstacles. The original blockout group retains 59 BoxColliders: 50 building, 4 suppressor markers, 1 review ground and 4 outer fall guards. The new site group has 22 Colliders. Earlier preservation of all 109 world coordinates does not apply to this relocation. Preserve the rack, ship interior and original scene files. Rechecking found 0.025m cargo penetration at a service corner: native BoxCast missed a grazing corner. Apply the existing penetration separation to the final pose in [shared carrying source](../../NoReturns/Assets/_NoReturns/Runtime/CinderBlockoutWalk.cs) `TrialCargoPose.Position`, used by Cinder and ship carrying. Add this exact pose to the static samples as a reproducible regression check. Retain speed, keys and pickup/drop rules.

Values/reproduction: [layout/check code](../../NoReturns/Assets/_NoReturns/Editor/CinderCompactSiteBuild.cs), [native surface mesh](../../NoReturns/Assets/_NoReturns/Art/CinderCompactSite01/CompactTileSurfaces.asset), [measurements](../../art/cinder-kit-01/site-unity-validation.json), [preserved local collision baseline](../../art/cinder-kit-01/site-local-collision-baseline.json). `NO RETURNS/Trials/Create Compact Cinder Site Review` stops if the file exists. `CreateScene(true)` is intentional regeneration, allowed only after saving the scene and stopping Play. Preserve manual edits first. No manual Unity YAML editing.

## Checks, running and remaining work

- [x] Passed 94 bidirectional movement segments: two delivery routes, four loops, north/south building entries, connecting lanes and ship ramp. Confirmed both routes reach the same BAY 04.
- [x] Measured minimum 3/3.15m widths and passed four simultaneous CharacterControllers in 17 alleys. Height 1.8m, radius 0.34m, skin 0.035m; four-body formation spans 2.81m. Open doorways widen some cross-sections, so measure the minimum opposed wall faces at three positions.
- [x] 159 occupiable positions×8 yaw×3 pitch = 3,816 static carrying poses plus 94 actual carrying segments via `CinderBlockoutWalk.Update`. 237,148 native penetration checks with 0 overlaps; 57 boundary contacts, tolerance 0.00001m. E/W/S/Q and empty-handed jumping passed. Key-event checks with API-set starting poses per segment, not human controls.
- [x] Preserved building/ship local collision, checked surface UVs/scale and matched 74 of 75 starting hashes, excluding the 1 intentionally corrected shared carrying source. Exclude 8 pre-existing ship-material edits from the commit. 0 compile errors; 8 existing obsolete warning emissions (6 unique); final Play console 0 errors/warnings.
- [x] Captured/reviewed 10 actual camera views. Temporarily hid ceilings/auxiliary roofs only for the whole-site cutaway and restored them. Saved roofs and floors remain intact.
- [x] User approval of whole-site direction.
- [ ] Prop quality, navigation, long-alley/junction sight lines, human four-player passing and cargo rotation feel.
- [ ] Final facility/cargo appearance for auxiliary volumes, background quality, receipt terminal and new-coordinate integration of AI/delivery/suppression/baton/networking, performance and standalone builds.

[Movement/four-body checks](../../art/cinder-kit-01/site-passage-validation.txt), [carrying checks](../../art/cinder-kit-01/site-carry-edge-validation.json), [preservation/check record](../../art/cinder-kit-01/site-checks.json). Full documentation fails on 142 existing missing artifact links with no new failures.

Raw staged whitespace fails on 30 trailing blank fields in Unity-generated scene, mesh, material and meta files. Code/document/evidence checks and the full staged check ignoring only generated trailing whitespace passed. Did not hand-edit Unity YAML solely for whitespace checks.

![Whole layout inside the field — roof removal is inspection-only](../../art/cinder-kit-01/site-field-cutaway.png)

[West sheltered route](../../art/cinder-kit-01/site-west-covered.png) · [Central junction](../../art/cinder-kit-01/site-central-junction.png) · [Central carrying alley](../../art/cinder-kit-01/site-central-carry.png) · [Overview with roofs](../../art/cinder-kit-01/site-overview.png).

Open `CinderCompactSiteReview` in Unity and enter Play. Start in front of the ship ramp; WASD movement, mouse view, E pickup, Q drop and empty-handed Space jump. Recheck with `Validate Compact Cinder Site Review`; in Play run the [carrying check](../../tools/unity_checks/CinderCarryEdgeCheck.cs) via Unity CLI `run_script`, then stop Play. The undressed structure uses `site-`; the dressed scene without the sky uses `props-`; the current sky-enabled scene uses `sky-`. Follow the prop checks above. Next: direct prop review, then receipt facilities and gameplay-coordinate integration.

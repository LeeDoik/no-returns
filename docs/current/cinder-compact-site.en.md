# CINDER — Whole-site layout inside the suppression field

[한국어](cinder-compact-site.ko.md)

2026-10-01 · CINDER-COMPACT-SITE-01 · **Whole-site direction approved by user; props placed and automated checks passed / prop quality and human four-player review incomplete.** No game-version change.

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

Props are already placed in the current scene. `NO RETURNS/Trials/Add Cinder Site Props` stops during Play, with unsaved changes, in another scene or when the prop group already exists, protecting manual edits. Run `Validate Cinder Site Props`, then the [shared carrying check](../../tools/unity_checks/CinderCarryEdgeCheck.cs) in Play for `props-` results. Preserve existing `site-` receipts. Next: direct prop-density/carrying-visibility assessment, BAY 04 receipt facilities and gameplay-coordinate integration. Human four-player play, AI/delivery/suppression/baton/networking, context beyond the field, performance and standalone builds remain unverified.

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
- [ ] Final facility/cargo appearance for auxiliary volumes, rocks/context beyond the field, receipt terminal and new-coordinate integration of AI/delivery/suppression/baton/networking, performance and standalone builds.

[Movement/four-body checks](../../art/cinder-kit-01/site-passage-validation.txt), [carrying checks](../../art/cinder-kit-01/site-carry-edge-validation.json), [preservation/check record](../../art/cinder-kit-01/site-checks.json). Full documentation fails on 142 existing missing artifact links with no new failures.

Raw staged whitespace fails on 30 trailing blank fields in Unity-generated scene, mesh, material and meta files. Code/document/evidence checks and the full staged check ignoring only generated trailing whitespace passed. Did not hand-edit Unity YAML solely for whitespace checks.

![Whole layout inside the field — roof removal is inspection-only](../../art/cinder-kit-01/site-field-cutaway.png)

[West sheltered route](../../art/cinder-kit-01/site-west-covered.png) · [Central junction](../../art/cinder-kit-01/site-central-junction.png) · [Central carrying alley](../../art/cinder-kit-01/site-central-carry.png) · [Overview with roofs](../../art/cinder-kit-01/site-overview.png).

Open `CinderCompactSiteReview` in Unity and enter Play. Start in front of the ship ramp; WASD movement, mouse view, E pickup, Q drop and empty-handed Space jump. Recheck with `Validate Compact Cinder Site Review`; in Play run the [carrying check](../../tools/unity_checks/CinderCarryEdgeCheck.cs) via Unity CLI `run_script`, then stop Play. The undressed structure uses `site-`; the current dressed scene uses `props-`. Follow the prop checks above. Next: direct prop review, then receipt facilities and gameplay-coordinate integration.

# CINDER — Whole-site layout inside the suppression field

[한국어](cinder-compact-site.ko.md)

2026-10-01 · CINDER-COMPACT-SITE-01 · **Whole-site density revised and automated movement/carrying checks passed / user quality and human four-player review incomplete.** No game-version change.

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
- [ ] User review of whole-site density, navigation, long-alley/junction sight lines and concept direction; human four-player passing and cargo rotation feel.
- [ ] Final facility/cargo appearance for auxiliary volumes, rocks/context beyond the field, receipt terminal and new-coordinate integration of AI/delivery/suppression/baton/networking, performance and standalone builds.

[Movement/four-body checks](../../art/cinder-kit-01/site-passage-validation.txt), [carrying checks](../../art/cinder-kit-01/site-carry-edge-validation.json), [preservation/check record](../../art/cinder-kit-01/site-checks.json). Full documentation fails on 142 existing missing artifact links with no new failures.

Raw staged whitespace fails on 30 trailing blank fields in Unity-generated scene, mesh, material and meta files. Code/document/evidence checks and the full staged check ignoring only generated trailing whitespace passed. Did not hand-edit Unity YAML solely for whitespace checks.

![Whole layout inside the field — roof removal is inspection-only](../../art/cinder-kit-01/site-field-cutaway.png)

[West sheltered route](../../art/cinder-kit-01/site-west-covered.png) · [Central junction](../../art/cinder-kit-01/site-central-junction.png) · [Central carrying alley](../../art/cinder-kit-01/site-central-carry.png) · [Overview with roofs](../../art/cinder-kit-01/site-overview.png).

Open `CinderCompactSiteReview` in Unity and enter Play. Start in front of the ship ramp; WASD movement, mouse view, E pickup, Q drop and empty-handed Space jump. Recheck with `Validate Compact Cinder Site Review`; in Play run the [carrying check](../../tools/unity_checks/CinderCarryEdgeCheck.cs) via Unity CLI `run_script`, then stop Play. Results use the `site-` prefix. Next: direct whole-layout review, then receipt facilities and gameplay-coordinate integration.

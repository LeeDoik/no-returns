# CINDER — Narrow interior maze for four people across

[한국어](cinder-maze-layout.ko.md)

2026-10-01 · CINDER-MAZE-01 · **Requested interior layout implemented and automated movement/carrying checks passed / user control feel and human four-player review incomplete.** No game-version change.

## Current layout

The user requested a compact, slightly maze-like layout with enough width for roughly four people to barely pass. Interpret this as four people abreast and add partitions inside five buildings in [separate CinderMazeReview](../../NoReturns/Assets/_NoReturns/Scenes/CinderMazeReview.unity). Standard clear width is **3.0m**, wall-center pitch 3.3m, wall thickness 0.3m and height 4m. Alternating side passages require turns; one partition in each building opens both ends to provide a circulation loop, keeping the entire interior from becoming one mandatory out-and-back path.

Retain the [approved aged style and existing appearance](cinder-map-appearance.en.md), building placement/outer dimensions, 3.2×3.3m north/south openings, ceilings, lights, rack, ship and carrying rules. **Outdoor yards and distances between buildings were not reduced in this task.** Small entrance pockets are wider than standard corridors. Preserve original, gray, warehouse appearance and five-building appearance scenes. Delivery, Listener, baton and networking remain unconnected to Cinder.

| Building | Partitions | Partition progression axis | Layout |
|---|---|---|---|
| A WAREHOUSE | 5 | Z | Alternating turns with a central two-sided detour |
| SIDE OFFICE | 3 | X | Small zones and central circulation |
| BAY 04 | 4 | Z | Alternating turns and a two-sided junction |
| C SERVICE | 5 | Z | Turns/circulation starting from the opposite side |
| STORAGE | 7 | X | Short zones across the elongated storage building |

Add 24 partition Colliders, 157 existing wall-module placements and 1,884 triangles. Reuse `Wall_A`, `Wall_Fill030` and `Wall_Fill090` at scale 1. No new models, textures, materials or gameplay systems. Use one BoxCollider per continuous partition and match its actual visual bounds. Compare and preserve settings/coordinates of the original 109 blockout BoxColliders. Values/routes: [producer/check code](../../NoReturns/Assets/_NoReturns/Editor/CinderMazeBuild.cs), [Unity measurements](../../art/cinder-kit-01/maze-unity-validation.json).

## Validation and limits

- [x] Passed 156 bidirectional movement segments and four simultaneous CharacterControllers through all 19 standard 3m interior straight lanes. Bodies use the current code's height 1.8m, radius 0.34m and skin 0.035m. The four-body formation spans 2.81m, leaving 0.19m total wall clearance. Five bodies need at least 3.4m without gaps: this is a dimensional comparison, not a five-player test.
- [x] 388 traversable body positions×8 yaw×3 pitch = 9,312 static carrying poses. Adjusted edge samples that entered the existing rack's reserved volume toward the aisle and validated positions with actual Capsule queries.
- [x] Passed carrying through 156 bidirectional segments via actual `CinderBlockoutWalk.Update`. Static poses and movement produced 22,196 native cargo penetration checks with 0 penetrations; tolerance 0.00001m. E/W/S/Q and empty-handed jumping passed. Mac Editor key events with an API-set starting pose for each segment, not human controls.
- [x] Captured 8 eye-level and 5 building cutaway views, 13 actual Play-camera images. Disabled ceiling Renderers only during inspection captures and restored them. Saved scene ceilings remain intact.
- [x] 0 compile errors; 3 existing obsolete warnings in ShipInteriorTrialBuild. 0 errors/warnings in the final Play console. All 73 starting file hashes match, including existing scenes, surfaces, carrying code and ship materials. Exclude 8 pre-existing ship-material edits from the commit.
- [ ] User review of narrow routes, junctions, carrying rotation, repeated walls/light quality. Human four-player cargo passing, simultaneous cornering feel and fun.
- [ ] Outdoor connection density, receipt/suppression facilities, Cinder gameplay integration, performance measurements and standalone Mac/Windows builds.

[Movement/four-body lanes](../../art/cinder-kit-01/maze-passage-validation.txt), [carrying poses/movement](../../art/cinder-kit-01/maze-carry-edge-validation.json), [preservation/check record](../../art/cinder-kit-01/maze-checks.json). Full documentation fails on 142 existing missing artifact links with no new failures.

![Warehouse interior layout — ceiling removed only for the inspection capture](../../art/cinder-kit-01/maze-warehouse-cutaway.png)

[Carrying at a turn](../../art/cinder-kit-01/maze-turn-carry.png) · [Junction view](../../art/cinder-kit-01/maze-junction-empty.png) · [Office](../../art/cinder-kit-01/maze-side-office-cutaway.png) · [BAY 04](../../art/cinder-kit-01/maze-bay-04-cutaway.png) · [Service](../../art/cinder-kit-01/maze-c-service-cutaway.png) · [Storage](../../art/cinder-kit-01/maze-storage-cutaway.png).

## Running and production order

Open `CinderMazeReview` in Unity and enter Play. Start at the south warehouse entrance; WASD movement, mouse view, E pickup, Q drop and empty-handed Space jump. Run `NO RETURNS/Trials/Validate Cinder Maze Review` for structure/passage checks. In Play, run the [existing carrying-check script](../../tools/unity_checks/CinderCarryEdgeCheck.cs) through Unity CLI `run_script`, then stop Play. Results use the `maze-` prefix. `Create Cinder Maze Review` stops when the file exists, preserving manual edits.

Next is direct review of route density and navigation feel. Once suitable, continue with the BAY 04 receipt terminal/floor marking, connecting routes and outdoor finishes. This layout request takes priority over the earlier appearance document's terminal-first order.

Raw staged whitespace checks fail on 4 trailing blank fields in Unity-generated scene/meta files. Code/document/evidence checks and the full staged check ignoring only generated trailing whitespace passed. No manual Unity YAML edits.

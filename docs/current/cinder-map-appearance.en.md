# CINDER — Building expansion of the approved aged appearance

[한국어](cinder-map-appearance.ko.md)

**[CinderCompactSiteReview with whole-site layout and props](cinder-compact-site.en.md) is current.** After user approval of the whole-map direction, place 47 prop groups reusing the aged atlas, rack and freight/CRT visuals. Below records shared appearance before relocation/props; do not inherit its 0 new Colliders, preservation of 109 world coordinates or earlier route passes for the current dressed scene. Follow the linked current structure/prop checks and outstanding quality status.

2026-10-01 · CINDER-MAP-APPEARANCE-01 · **User approval of aged warehouse style / shared appearance applied to 5 buildings and automated checks passed / user review of the expansion incomplete.** This does not change the game version or declare the entire map finished.

## Scope and reference

The user approved the [aged warehouse appearance](cinder-asset-prep.en.md#2026-10-01--aged-texture-revision) as the map style: “Let's build the map with this feel and move on” (original: “그래 이런 느낌으로 맵을 구성하자. 다음으로 넘어가자”). Extended yellowed panels, peeling burgundy stripes, rust/grime, dark door frames and warm work lights to the existing office, BAY 04, service and storage buildings. Used separate [CinderMapAppearanceReview](../../NoReturns/Assets/_NoReturns/Scenes/CinderMapAppearanceReview.unity), preserving the warehouse review scene. Style approval does not approve all routes, visibility, fun or release quality.

Retain building placement/dimensions, openings, collision, ship and carrying runtime. Outdoor ground, connecting-route floors/roofs, obstacles, suppressor markers and ship finish retain their trial appearance. Receipt/delivery, Listener, baton and networking are not connected to this map. This expansion covers building floors, walls, frames, ceilings and work lights.

| Building | Existing floor X×Z (m) | Added placements (including lights) | Added triangles |
|---|---|---|---|
| A WAREHOUSE | 14.4×20.4 | Retain 530 structures and 7 props | Retain 6,408+216 |
| SIDE OFFICE | 14.4×9 | 282 | 3,480 |
| BAY 04 | 13.8×16.8 | 450 | 5,496 |
| C SERVICE | 12.6×22.2 | 546 | 6,648 |
| STORAGE | 28.8×7.2 | 418 | 5,112 |

Added total: 1,696 placements and 20,736 triangles. Combined with the warehouse, the kit uses 27,360 triangles; this is not a whole-scene/FPS measurement. Retain opposite 3.2×3.3m openings and ceiling underside at 4m. Each added building receives 2 exterior entrance lights and 2 interior side-wall lights, 16 total. With the existing 5, there are 21; color (1,0.67,0.3), intensity 0.65, range 5m and no shadows retain existing prefab values.

Evidence: [original layout code](../../NoReturns/Assets/_NoReturns/Editor/CinderBlockoutBuild.cs), [expansion assembly/check code](../../NoReturns/Assets/_NoReturns/Editor/CinderMapAppearanceBuild.cs), [Unity measurements](../../art/cinder-kit-01/map-unity-validation.json). Placement derives from original floor Bounds in the current scene without arbitrarily resizing buildings.

## Fill modules and production

Reuse the existing shared 512×512 atlas and material. Crop the following fill geometry/UVs from source modules instead of stretching placement scale. No new generated imagery, Tripo or external models.

| Name (`NR_Cinder_` prefix) | Unity X×Y×Z (m) | Pivot |
|---|---|---|
| Floor_Half | 0.6×0.2×1.2 | Upper-surface center |
| Floor_Quarter | 0.6×0.2×0.6 | Upper-surface center |
| Ceiling_Half | 0.6×0.3×1.2 | Underside center |
| Ceiling_Quarter | 0.6×0.3×0.6 | Underside center |
| Ceiling_Edge_Half | 0.15×0.3×0.6 | Underside center |
| Wall_Fill005 | 0.05×4×0.3 | Bottom center |
| Wall_Fill030 | 0.3×4×0.3 | Bottom center |
| Wall_Fill065 | 0.65×4×0.3 | Bottom center |

Each has 12 triangles, 1 material slot and closed surfaces. Store 8 new FBXs/prefabs in `Art/CinderMapFills01` / `Prefabs/CinderMapFills01`; with the existing 14, there are 22 models/prefabs. Preserve the [Blender producer](../../art/cinder-kit-01/build_map_fills.py), [source](../../art/cinder-kit-01/Cinder_Map_Fills.blend) and [roundtrip receipt](../../art/cinder-kit-01/map-fill-validation.json). Run `blender --background --python art/cinder-kit-01/build_map_fills.py` to reproduce fill modules only. Unity's `NO RETURNS/Trials/Create Cinder Map Appearance Review` stops if the file exists and does not overwrite manual edits to regenerate. No manual YAML edits.

The existing work-light material stored `_EMISSION` alongside the `EmissiveIsBlack` flag, allowing URP reimport to disable emission. Corrected the shared producer/material to `BakedEmissive` and added an emission-preservation check after reimport. Colors, emission color and Point Light values are unchanged; no lightmaps were baked. Of 82 baseline files, only this 1 material changed; the other 81 hashes match. Preserve the 8 pre-existing ship-material edits and exclude them from the commit.

## Validation and direct review

- [x] Checked 8 Blender/FBX fills for dimensions, pivots, closed surfaces, UV range and reimported UV/triangle/material agreement.
- [x] Checked 8 new and 14 existing Unity imports, placement scale 1, floor/roof area and 0 new Colliders. Compared preserved settings/placement for 50 building Colliders and 109 BoxColliders across the blockout.
- [x] Passed 41 existing movement routes, 6 warehouse passages, 3 jump positions and 48 center-lane cargo poses.
- [x] Mac Editor Play passed 98 positions×8 yaw×3 pitch = 2,352 carrying poses with 0 penetrations, E/W/S/Q and empty-handed jumping. 6 boundary contacts; tolerance 0.00001m. Actual keys/API poses, not real-time human input.
- [x] Captured 15 Play-camera views and reviewed building empty-handed/carrying views, warehouse entry and overall placement. 0 compile errors; 0 errors/warnings in the final Play console.
- [ ] User review of expanded-building visibility, joints, repeated texture and lighting density; distant shimmer/performance measurements.
- [ ] Outdoor/connecting-route finishes, receipt facilities, building-specific interiors and signs. Standalone Mac/Windows builds, ship Play and networking/AI/delivery checks.

[Check/preservation hashes](../../art/cinder-kit-01/map-checks.json), [movement routes](../../art/cinder-kit-01/map-passage-validation.txt), [Play poses/input](../../art/cinder-kit-01/map-carry-edge-validation.json). Full documentation fails on 142 existing missing artifact links with no new failures in this task.

![5 building appearances and existing outdoor trial layout](../../art/cinder-kit-01/map-overview.png)

[Office empty-handed](../../art/cinder-kit-01/map-side-office-empty.png), [BAY 04 carrying](../../art/cinder-kit-01/map-bay-04-carry.png), [service empty-handed](../../art/cinder-kit-01/map-c-service-empty.png), [storage carrying](../../art/cinder-kit-01/map-storage-carry.png). Actual Play camera without IMGUI instructions. Only the overall view uses an elevated API pose and pitch 40°.

Open the map scene in Unity and enter Play, starting at the south warehouse entrance. WASD movement, mouse view, E pickup, Q drop, empty-handed Space jump, F1 language toggle and Esc cursor release. Recheck with `NoReturns.Editor.CinderMapAppearanceBuild.Validate()`; in Play run the existing [carrying check](../../tools/unity_checks/CinderCarryEdgeCheck.cs), then stop Play. New receipts/views use the `map-` prefix without overwriting warehouse evidence.

Current priority is user review of the [entire layout inside the suppression field](cinder-compact-site.en.md). Correct the interior-only interpretation by composing building spacing, yards and utility masses together. Review whole-site density/navigation/carrying feel, then continue BAY 04 receipt terminal/floor marking and coordinate integration of existing CarryRoom systems. Distinguish visual layout from actual delivery integration.

Raw staged whitespace checks fail on 106 trailing blank fields in Unity-generated scene/prefab/meta files. Code/document/evidence checks passed, as did the full staged check ignoring only generated trailing whitespace. Did not hand-edit Unity YAML solely for whitespace checks.

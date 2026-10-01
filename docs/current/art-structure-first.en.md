# Structure-first asset production

[한국어](art-structure-first.ko.md)

2026-09-14 · New default pipeline requested by the user, applying to all new/rebuilt assets. This task proceeds only through the raised-ceiling structure trial; no Tripo generation or credits are used.

## Sequence and approval

1. **Blender structure:** Use real meters for bounds, passages, doors, screen surfaces, grips, pivots and attachment points. Include actual character/cargo collision dimensions. Do not collapse the entire interior into one generated mesh.
2. **Unity placement:** Test with the actual first-person camera and controls. Keep structural collision/interaction references separate from the visual model.
3. **Human play review:** The user reviews scale, atmosphere and routes; revise accordingly. Automated collision checks support this but cannot approve fun or spaciousness. Do not begin Tripo production for that part before the user confirms satisfaction.
4. **Lock art references:** Render front, rear, side, top and gameplay views from the approved structure. Record dimensions, openings, screen surfaces, pivots, material regions and required silhouette. Preserve the existing image-approval rule and obtain confirmation for new appearances before generation.
5. **Tripo visual candidate:** Generate individual parts using the tested structure as reference. Verify supported input modes in the current tool when executing. Do not assume lossless automatic enhancement of an uploaded mesh. Preserve angular PSX silhouettes, restrained textures and a common palette; polygon count alone is not quality.
6. **Blender fitting/review:** Overlay results on the approved structure and check bounds, passages, screen openings, moving parts, UVs and materials. Repair deviations and preserve approved geometry for dimension-critical walls, door frames and screens. Simple structural assets do not all need Tripo regeneration.
7. **Unity visual replacement/retest:** Replace visuals while preserving collision, interaction and online connections. Recheck entry, jumping, cargo handling, screen occlusion, doors and reimport. Complete after human visual and handling review.

## Per-part records and gates

Link the structural source, approved-view images, dimensions/pivots, raw Tripo result, cleaned Blender source, runtime FBX/textures/Unity .meta, automated checks and user feedback. Preserve earlier approvals. Store source models/images in Git LFS and production scripts/bilingual documents in regular Git. Exclude builds, logs and credentials.

- [ ] Structure matches actual camera, character and cargo dimensions.
- [ ] The user confirms satisfactory structural play and progression to art production.
- [ ] Visual production follows approved images and geometry references.
- [ ] Passage, jumping, grips, screens and pivots survive visual replacement.
- [ ] Automated checks and human quality review are recorded separately.

The ship is currently iterating before the second gate. [Structure trial](ship-interior-trial.en.md) · [Interior component guide](ship-interior-pipeline.en.md).

## 2026-09-15 — Build the ship interior first

Build interior work zones, routes and actual play first. After user review, derive the exterior dimensions by adding structure thickness and equipment clearance around that layout; obtain exterior art approval before production. Do not compress the interior to fit an exterior concept. Use only a temporary cover for visibility and entry checks during interior testing. The generation script now starts with an empty Blender scene and does not load the old exterior file.

The new candidate arranges inspection, sealed storage and dispatch work asymmetrically around the space-delivery role. Automated checks and human satisfaction remain separate; scanner, lockers and receipt equipment are structural mockups in this task. This does not approve new economy or cargo rules.

## 2026-09-30 — Recommended Cinder production workflow

For quality and production efficiency, the recommendation is **modular environment production with Blender + Unity URP, completing a small reference area before expansion**. This is a judgment based on the current PSX direction and Cinder structure, not a comparative quality/time benchmark or approval of new appearances. The need to create new art remains unchanged.

| Role | Recommended approach |
|---|---|
| Visual baseline | Extract color, silhouette and surface-density rules from the existing concepts and generate required production images. |
| Structural walls, frames, floors and ceilings | Use Blender Python to control dimensions, pivots and joining faces. Retain editable values when regenerating the same part. |
| Surfaces | Share materials and texture sets to align color and pattern scale across repeated parts. Individual image-generation results are not automatically final materials. |
| Distinct props and organic forms | Use Tripo candidates when useful, then check dimensions, UVs and silhouettes in Blender. Follow the existing appearance and spending checks. |
| Scene assembly and review | Repeat components as Unity prefabs and use Unity CLI to assist import, assembly and checks. Inspect actual first-person views, lighting and cargo clearance in the warehouse entrance/passage before expanding to all 5 buildings and outdoors. |

Separate quality judgment from successful file generation. In the reference area, human review covers consistent colors/textures, readable silhouettes/entrances, seams, lighting and cargo visibility; automatic checks support units, pivots, UVs, missing textures and collision. Compare before/after views from matching reference cameras. Set performance targets and texture/mesh budgets after measuring the actual area rather than promising arbitrary numbers.

This investigation verified local execution of Blender **5.2.2 LTS** and Unity CLI **1.0.0-beta.11**. Game Development Studio skills are present, but `game-dev` was not found on the current PATH, so its production/validation CLI integration remains unverified. The recommended workflow does not establish successful execution of that plugin. No new installation, model production, scene changes or spending.

Official capability references: [Blender command-line automation](https://docs.blender.org/manual/en/latest/advanced/command_line/index.html), [Unity prefabs](https://docs.unity.com/en-us/engine/6000.6/manual/working-with-gameobjects/prefabs/creating), [Unity CLI](https://docs.unity.com/en-us/unity-cli/unity-cli-reference).

2026-10-01 follow-up: generated/inspected the [entrance proposal and 8-unit sheet](../art/cinder-appearance-01.en.md) and completed the [close-carrying checks and fix](cinder-asset-prep.en.md#2026-10-01--carrying-edge-fix-and-appearance-proposals). The user subsequently approved the concept direction; [actual shared-surface/3-unit application and checks](cinder-asset-prep.en.md#2026-10-01--applying-the-approved-appearance) are complete. User quality review of the applied result is next. Images do not establish completed models/textures or dimension validation.

## 2026-10-01 — Cinder gray structure production

The [5-unit production/review guide](cinder-asset-prep.en.md) connects 11 Blender sources/FBXs/visual prefabs, a separate Unity scene retaining original collision, actual Play-camera views and validation evidence. This is structural production. The user found warehouse size acceptable; retain the current 14.4×20.4m. Concept approval, the 3 presentation units and shared surfaces were completed after this gray-production record. User carrying/joint and applied-result quality review remain outstanding. The review menu regenerates the review scene; preserve manual edits in a separate copy. Gray checks do not establish final-art or game quality.

2026-10-01 surface revision: addressed the user request that the first actual appearance was too clean with [aged textures](cinder-asset-prep.en.md#2026-10-01--aged-texture-revision). Normalize built-in imagegen surface sources with native Blender resizing while retaining existing UVs, FBXs, prefabs, materials and scenes. Actual-camera comparisons and automated checks are complete; user quality feedback on the revision remains outstanding.

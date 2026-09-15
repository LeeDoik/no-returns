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

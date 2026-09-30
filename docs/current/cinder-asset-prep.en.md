# CINDER DEPOT — First asset-production preparation

[한국어](cinder-asset-prep.ko.md)

2026-10-01 · CINDER-ASSET-PREP-01 · **Preparation complete / structural and visual production and user quality approval incomplete.** The current target is the [Cinder blockout](cinder-blockout.en.md). Follow the [structure-first production guide](art-structure-first.en.md). New module dimensions and budgets below are initial trial proposals, not release specifications.

## What to make first and why

**Propose the warehouse's south entrance and the first 6m inside as the review area.** This exposes repeated walls, floor, ceiling and an entrance together, allowing joints, color and carrying clearance to be checked before extending the kit to other buildings. The warehouse currently has no separate interior corridor. The 6m describes a proposed review length; it does not authorize new corridor walls or narrower existing routes.

1. **5 structural units:** floor → straight wall → door frame → ceiling/beam → corner/end finish. Establish the shared cross-section and entrance first.
2. **3 presentation units:** work light → sign → empty cargo rack. Add entrance identification and signs of use while keeping racks away from the doorway.
3. Review structural play and appearance in the reference area before extending to the 5 buildings. The next functional assets are the receipt terminal/receipt floor marking, then suppression hardware/warning signals. Employee, hands, Listener and outer-creature rigs remain separate preparation tasks.

The first batch contains **8 production units**. Corner/end and ceiling/beam variants mean this is not an FBX, mesh or placement count. R reuse labels in the [older 44-unit demo list](demo-art-list.en.md) do not establish completed assets for current Cinder. This batch proposes newly made candidates; old Selected models serve only as appearance/defect references.

## References from the current structure

Evidence: [saved scene](../../NoReturns/Assets/_NoReturns/Scenes/CinderDepotBlockout.unity), [builder](../../NoReturns/Assets/_NoReturns/Editor/CinderBlockoutBuild.cs), [movement/carrying code](../../NoReturns/Assets/_NoReturns/Runtime/CinderBlockoutWalk.cs). Compared warehouse child Transforms in the saved scene with builder values. No actual Play test was performed in this task.

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

Door, ceiling, employee and cargo values are existing trial values, not final dimensions approved by the user. The historical 41 passage checks do not validate new visuals, cargo rotation or cooperation.

## Per-component production instructions

Units are meters; dimensions below use **Unity X×Y×Z**. Blender uses Z-up and Unity Y-up, so check axes, rotation and units through export/reimport. Author reproducible sources in Blender Python; no script or model was created in this task. Names below propose stable names for new sources.

| Order / related ID | Source name | Trial dimensions/pivot | Shape and acceptance condition | Proposed triangle ceiling |
|---|---|---|---|---|
| 1 / ENV04 | `NR_Cinder_Floor_A` | 1.2×0.2×1.2. Pivot at walking-surface center; geometry occupies Y=-0.2~0. | Flat surface. Join 5 tiles into a 6m repetition sample and inspect seams/pattern scale. | 200 |
| 2 / ENV01 | `NR_Cinder_Wall_A` | 1.2×4×0.3. Bottom-center pivot. | Close front, back and top. Flat mating faces; shared stripe height and UV density. | 400 |
| 3 / ENV03 | `NR_Cinder_DoorFrame_A` | Outside 3.8×4×0.3; clear opening 3.2×3.3. Pivot at opening-floor center. | Frame without a threshold. Decorations stay within 0.3m sides and 0.7m header. Door leaf/movement are separate. | 1,200 |
| 4 / ENV07 | `NR_Cinder_Ceiling_A` | 1.2×0.3×1.2. Underside-center pivot, placed at Y=4. | Finish the underside. First test beam variants within existing roof thickness, retaining underside Y=4. | 600 |
| 5 / ENV02·05 | `NR_Cinder_Corner_A` / `NR_Cinder_End_A` | Thickness 0.3, height 4. Corner pivot at bottom of wall-centerline intersection. | Close inside/outside corners and exposed ends. Fit 5.6m wall lengths with 4×1.2m + a 0.8m infill variant without stretching UVs. | 600 |
| 6 / FAC01 | `NR_Cinder_Lamp_A` | 0.6×0.2×0.18. Pivot at wall-mounting-plane center. | Separate warm emissive face and housing. Unity owns the actual Light. | 400 |
| 7 / new surface unit | `NR_Cinder_Sign_A` | 1.2×0.6×0.02. Pivot at rear mounting-plane center. | Separate plate and lettering surface. Use `WAREHOUSE` / 창고 and a delivery-direction arrow as review copy. | 12 |
| 8 / FAC02 | `NR_Cinder_Rack_A` | 2.4×2.4×0.6. Bottom-center pivot. | Empty 2-tier rack. Boxes stay separate; do not bake delivery-cargo visuals into decoration. Place outside the passage. | 1,600 |

The 1.2m grid is an initial module proposal fitting the warehouse's 14.4/20.4m floor. Do not assume every building or the 3.2m opening fits that grid. Resolve corner extents, beam cross-section and sign direction in the structural source. Triangle ceilings are **proposed working budgets per source/variant**, not measured current meshes or a performance guarantee. Record evidence and adjust if the first sample needs more silhouette detail.

## Shared surfaces and visual references

Use the [environment sheet](../art/space-concepts/environment-kit-01.png) and [sheet review](../art/space-concepts/environment-kit-01.en.md) for style. Align ivory panels, burgundy stripes, dark frames and warm work lights; concentrate rust/wear at lower edges and contact areas. Reduce the dense stains remaining across broad floors/walls in the old sheet. Entrances and cargo silhouettes must read first. Selecting this reference is separate from approving new appearances.

The first comparison budget is **1 shared 512×512 BaseColor**, **1 separate 256×128 sign texture**, and a solid emissive light face. Start with Point filtering as a candidate and compare mipmaps, distant shimmer and text readability in actual views. Use a shared structural surface material and a separate emissive material, retaining broad simple faces and angular silhouettes. Texture sizes, pixel density and emission intensity are trial proposals; finalize after reviewing reference-area screens.

Prepare the first 8 units for **direct local Blender authoring**. Tripo calls and paid generation are not required for this batch. Test new structures in Unity and incorporate user structural feedback, then review new appearances using front, rear, side, top and first-person views from the same model. Check dimensions in the model rather than image lettering or proportions.

## Deliverables and destinations

These are **future production destinations**, not files/folders created in this task or an executable production script.

- `art/cinder-kit-01/`: `build.py`, structural source `Cinder_Kit_Structure.blend`, per-component FBXs, UV/reimport/dimension-check JSON and review images from the same model. Do not overwrite ship or Selected sources.
- `NoReturns/Assets/_NoReturns/Art/CinderKit01/`: reviewed runtime FBXs, textures, shared materials and Unity-generated `.meta` files.
- `NoReturns/Assets/_NoReturns/Prefabs/CinderKit01/`: prefabs owning collision, lights and display connections with visual models as children. Assemble through Editor/CLI; do not hand-edit YAML.
- Bind dimensions, pivots, triangles, UVs, material slots, provenance, check results and user feedback to each source name. Record direct-authoring provenance for new local geometry/textures. Check usage rights and license before introducing external files. Existing concepts are references.
- Models, images, textures and `.blend` use existing `.gitattributes` Git LFS rules; production scripts, JSON and bilingual documents use ordinary Git. Exclude builds, logs, caches, credentials and personal settings.

Current tool check: local `blender --version` reports **5.2.2 LTS**. Unity target **6000.6.0f1 / URP 17.6.0** comes from the [version file](../../NoReturns/ProjectSettings/ProjectVersion.txt) and [packages](../../NoReturns/Packages/manifest.json). `game-dev` is absent from the current PATH, so that CLI's inspection, normalization and packaging route is unavailable. Preparation uses the existing local authoring route without installing tools or substituting a generation service. New Blender-model production, FBX roundtrip and Unity Cinder Play/builds remain outside this task's verified scope.

## Next task and acceptance gates

The next production task is **gray sources and an assembly sample for the 5 structural units**. Fit floor, wall, door frame and ceiling first, then close corners/ends. Place them through Editor/CLI in a separate review scene preserving original Cinder. Existing Create/Build Cinder menus regenerate the scene; do not run them on a manually authored art-review scene.

| Review view | What to inspect |
|---|---|
| 3m in front of the south entrance, eye height 1.57m | Entrance recognition; signs/lights explain the route. |
| Enter carrying cargo; rotate left/right/up/down | Frame, ceiling and corners do not intrude into the 0.8×0.65×0.65m cargo or view. |
| Look back at the entrance from 6m inside | Wall backs, ceiling underside, end finish and repeated floor seams. |
| Repeat these views after adding rack/light | Storage and carrying routes remain distinct; bright surfaces do not hide cargo outlines. |

- [x] Compared current scene, builder/controls, environment concept and production guides.
- [x] Specified 8 priority units and names, trial dimensions, pivots, surface budgets and review positions for the 5 structure-first units.
- [ ] Produce structural sources/FBXs and check units, axes, dimensions, openings, pivots, UVs and normals.
- [ ] Reimport preserves size/material slots and overlapping old Renderers do not cause seam flicker. Do not arbitrarily duplicate/remove existing Colliders.
- [ ] Test E/Q, door passage, carrying rotation, backwards movement and empty-handed jumping in Unity; record user spatial feedback.
- [ ] After user structural review, review new appearances in production-oriented multi-view/gameplay images and record approval status.
- [ ] Recheck the reference area with shared surfaces and 3 presentation units before extending to other buildings.

Networking, enemy AI and delivery judgement are not connected to the current Cinder trial. Passing this structural area cannot complete the whole game, cooperation, enjoyment or release quality.

## Verification of this preparation

Documentation work only. No new code, scenes, models, images, materials, builds or paid-generation changes. Check bilingual links, dimensions, names, budgets and checkbox states. Full documentation checks already had **142** evidence links to locally absent `artifacts/` at task start. Do not fabricate completed evidence to fill them; compare separately for new missing links introduced by this change. [Validation checklist](05-validation.en.md) · [Git operating rules](version-control.en.md).

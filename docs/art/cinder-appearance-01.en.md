# Cinder warehouse appearance proposal 01

[한국어](cinder-appearance-01.ko.md)

2026-10-01 · **Concept approval, first application and aged texture revision complete / user quality review of the revision incomplete.** This proposes appearances for the 14.4×20.4m warehouse in the [current production brief](../current/cinder-asset-prep.en.md). Retain the user-reviewed size. PNGs below are approved references; actual model/material/texture application is linked in the [latest production record](../current/cinder-asset-prep.en.md#2026-10-01--aged-texture-revision).

![Warehouse entrance appearance proposal](../../art/cinder-kit-01/appearance-entry-01.png)

![8-unit appearance sheet](../../art/cinder-kit-01/appearance-components-01.png)

## What to review now

Continue ivory panels, rust-red bands, dark framing and warm work lights from the [existing environment proposal](space-concepts/environment-kit-01.en.md). Reduce broad-surface mottling and floor contrast to prioritize entrances and cargo. Generated perspective and opening proportions may differ from the actual camera; do not use these images for dimensional validation. The [gray entrance measurement view](../../art/cinder-kit-01/entry-empty.png) and existing FBXs remain the structural references.

| Production unit | Appearance proposal and constraints |
|---|---|
| Floor | Quiet large gray surfaces and broad seams. No new threshold or step. |
| Straight wall | Ivory panels and rust-red band. Align band heights and UV density on both faces and adjacent modules in the model. |
| Door frame | Dark framing with a separate work light. Preserve the existing 3.2×3.3m clear opening. |
| Ceiling/beam | Simple underside finish. Do not lower new beams below the 4m ceiling underside. |
| Corner/end | Narrow closed joints. Retain source sections and pivots. |
| Work light | Compact housing and separate warm luminous face. Unity owns the actual light. |
| Sign | Proposed English source text `WAREHOUSE` and an entrance-direction arrow. Thin mounting face. |
| Empty cargo rack | Empty 2-level rack, placed outside the entrance and carrying route. |

## Pre-production status and provenance

Generated with built-in imagegen using the actual gray entrance/component sheet and existing environment proposal as references. The initial component sheet showed 3 rack levels; corrected it to 2 and inspected the final large and secondary views. Reviewed 8 entries, the `WAREHOUSE` text, an empty rack and front/back/side/top reference views. The sheet also illustrates a lamp mounted on the frame, but production keeps it a separate part. Images do not validate orthographic projection, exact dimensions, UVs, repeat connections, lighting intensity or performance.

[Actual prompt set](../../art/cinder-kit-01/appearance-prompts.json) · [Generation provenance and file hashes](../../art/cinder-kit-01/appearance-provenance.json). No API/CLI fallback generation or Tripo invocation. Retain the existing trial budgets of one shared 512×512 BaseColor and one 256×128 sign image; actual textures at these sizes were directly authored after approval; generated concepts were not used as final textures.

- [x] Generated, inspected and saved the entrance proposal and 8-unit sheet in the project.
- [x] Corrected the rack to 2 levels and connected exact prompts, references and final PNG hashes.
- [x] User approved the concept direction for colors, texture density, light, sign and rack: “Yes, let's go with this feel.”
- [x] Applied approved appearances to a separate Unity scene and repeated empty-handed/carrying views and automated checks.
- [ ] Obtain user feedback on visibility, joints and quality of the applied result.

This task's carrying checks and fix are recorded separately in the [production/review guide](../current/cinder-asset-prep.en.md). Automated passes do not replace human visibility/control evaluation or approval of these appearances.

## Actual Unity application views

The user found the first actual application too clean and requested dirty, aged textures. Actual views below show the current revision. [Aged texture production/checks](../current/cinder-asset-prep.en.md#2026-10-01--aged-texture-revision), [first-application entry](../../art/cinder-kit-01/clean-entry-before.png) and [first-application rack](../../art/cinder-kit-01/clean-rack-before.png). Concept-direction approval is distinct from quality approval of the revision.

![Empty-handed warehouse entrance](../../art/cinder-kit-01/production-entry-empty.png)

![Empty 2-tier rack](../../art/cinder-kit-01/production-rack-empty.png)

[Carrying entrance view](../../art/cinder-kit-01/production-entry-carry.png) · [Review scene](../../NoReturns/Assets/_NoReturns/Scenes/CinderAppearanceReview.unity). Captured with the actual Play camera, separate from the reference concepts.

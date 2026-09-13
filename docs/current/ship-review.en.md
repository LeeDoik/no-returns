# Ship exterior/interior consistency review

[한국어](ship-review.ko.md)

2026-09-13 · Reviewed glazing exterior 02, interior 02 and proposed dimensions. Conclusion: retain art direction but hold production-structure approval. This does not revoke the selected exterior direction. No regenerated images, models or game-code changes in this task.

## Evidence and findings

| ID | Priority/type | Observation/evidence | Required action |
|---|---|---|---|
| S01 | High / inconsistent drawings | Exterior front/rear/profiles do not resolve roof blocks, engines and supports as same-scale orthographic projections. The interior plan mixes in a frontal glazing elevation. | Export all elevations, plan and sections from one 3D block at matching scale; do not combine independently drawn proportions. |
| S02 | High / incomplete dimensions | Exterior 11.2m - interior 7.6m = 3.6m of length is not allocated in section. Removing the old forward non-habitable compartment requires a new allocation. | Allocate nose slope, bulkheads, hatch and external engine projection in actual hull coordinates. Do not assume remaining length is usable machinery space. |
| S03 | High / potential interference | Exterior glazing is on a high sloped front; interior shows large glazing directly over an eye-level console. Floor/sill/ceiling coordinates are missing, so alignment cannot be verified. | Section floor, eye height 1.57m, glass top/bottom, console top and nose slope together. Validate sightlines against exterior form. |
| S04 | High / undefined mechanism | Hatch height is 2.4m while ramp length is 3.0m. If a single ramp panel doubles as the door, it is 0.6m longer. Overlap, folding or a separate shutter is unspecified. | Separate closing shutter from loading ramp, or design folding tip; check both open and closed states. Current values alone do not prove impossibility. |
| S05 | Medium / plan-to-number mismatch | Width label is corrected to 6.4m, but the 4.8m nose dimension does not span its actual inner walls. Rack 0.8m + access 1.4m each side + central 2.0m regions are not independently readable. | Dimension actual inner walls; color furniture, access and carrying zones separately. Do not validate by measuring generated-image pixels. |
| S06 | Medium / use-space unverified | Preparation length 1.2m contains 2 seats per side plus equipment access. Consecutive longitudinal seats imply pitch 0.6m each, without frames, arms or equipment operating space. | Place unfolded seats, people, cargo and equipment working envelopes; redistribute seats or zone lengths if needed. Do not certify 4-person usability. |
| S07 | Low / concept labeling | Plan shows 5 employee silhouettes despite a target of 4. | Use 4 reference bodies in the final block; do not interpret this as 5-player support. |
| S08 | Medium / art judgment | Rust, orange stripes and strong amber lights repeat on almost every surface, weakening console/entrance hierarchy. | Reduce cargo-zone lighting, focus on window/console/ramp, localize wear to joints/lower/contact surfaces. This is art judgment, not measured validation. |

## Keep

A’s low broad hull, rear cargo entry, forward glass/console, single-deck center aisle and side equipment remain sound directions. Beacon/baton left-right reversal in the aft-looking view is correct. Resolve consistency of this form instead of discarding it for a new ship.

## Recommended next work

Assemble simple exterior shell, floor, nose, glass, door, ramp and furniture blocks before generating more images. Export every view from one undetailed model. Establish length/height allocation and mechanisms, then refine concepts against that block. This review did not execute that step.

- [ ] S01~S04: validate exterior/interior sections and closed ramp state.
- [ ] S05~S07: validate 4 reference bodies, seats, cargo and access spaces.
- [ ] S08: distinguish lighting and wear-density roles.

47.04 square meters remains the unfurnished arithmetic area of a proposed tapered plan, not proof it fits the hull. The 0.64m difference between a 2.0m aisle and two collision-body widths does not cover rotation, cargo, hands or equipment. Earlier alignment claims mean visual direction only.

[Exterior sheet](../../art/ship-concepts-01/flatbed-four-views-windows-02.png) · [Interior sheet](../../art/ship-concepts-01/interior-glazing-aligned-02.png) · [Dimensional proposal](ship-interior.en.md).

Validation: direct visual comparison and document arithmetic/status checks. No 3D mesh, collision or actual-control validation.


2026-09-14: [FLATBED integrated 3D model and review status](ship-production.en.md). Integrated-model changes supersede earlier interior dimensional proposals. Unity integration and 4-player control validation remain incomplete.

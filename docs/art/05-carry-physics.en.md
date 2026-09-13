# Carry clipping fix and physics refinement

[한국어](05-carry-physics.ko.md)

> **Historical record · retired/superseded 2026-09-12.** The [PSX space-delivery design](../current/01-overview.en.md) is current. Implementation, launch, art and values below are historical, not current status. Deleted files remain path records only.


> Documentation update: versions and values here belong to this file's original context. Check the [documentation home](../README.en.md) first for current rules and outstanding work.

## Design

September 9, 2026 · 0.9.1 / protocol 11. Apply consistent gravity and rigid-body responses while retaining existing co-op controls, rather than building a complete reality simulator. No custom physics engine or engine replacement. Remove the back-mounted CrewBadge. As requested, crew identification uses yellow, teal, purple and coral workwear for slots 1–4. Preserve face, glove and cap colors; existing nameplates remain secondary labels.

Reduce carrying speed and use carry_walk. Set carrying state at pickup, preventing running and carrying poses from mixing. If a wall blocks a parcel and it encroaches on the carrier, release it to prevent penetration. Inspect torso and leg intrusion separately from intended hand contact.

Normal falls use 9.81 m/s². Allow parcel rotation and friction; dropping retains carrier velocity, and throwing adds carrier velocity to existing throw strength. Type-specific masses are gameplay tuning values, not measurements. Keep the controllable CharacterBody, and retain the intentionally unrealistic special parcel jumps and sneezes.

The host computes rotation and sends a compact orientation through existing state data. Preserve the 1,280-byte pose packet limit; increment protocol for wire-format changes. The landing marker predicts initial contact before collision-induced rolling, not the eventual resting position.

## Implementation and validation plan

Tuning: carrying speed 2.4 m/s, empty-handed speed 4.5 m/s; acceleration 12 and 24 m/s² respectively; jump launch speed 4.8 m/s. Parcel masses are Standard 3 kg, Sneezer 4 kg, Clinger 5 kg and Hopper 2.5 kg. Restitution is 0.06, friction 0.65 and angular damping 0.6. Character pushing force is capped at 80 N. Rotation uses 10 bits per Euler axis, with approximately 0.18 degrees maximum quantization error per axis.

An additional parcel-sized character collider makes a carrier stop with the box at walls. Release blocked cargo if its forward gap from the carrier falls below 0.9 m. Carrying never selects a running clip. Special parcel reactions and control assistance retain gameplay tuning; this is not a simulation calibrated from measured material properties for every object.

- [x] Confirmed previous failures for carrying state, back decoration, rotation and moving drops. Parcel gravity was already close to earth gravity; character gravity was aligned.
- [x] worker.gd / worker_animation.gd: removed decoration, implemented crew colors, carry gait and speed, gravity and pickup-state updates.
- [x] cargo.gd: mass, rotation, friction, inherited velocity, carry collision, replicated rotation and prediction.
- [x] Passed torso/leg joint clearance across 150 carry frames, wall collisions, off-center impact rotation, free fall, compressed rotation and remote rotation checks. Inspected actual renderer views of all four colors from front/back and carrying.
- [x] Passed regressions, built and launched Windows package, updated both language documents.

Regressions passed 36 behavior tests, 10 two-peer network scenarios and one four-peer scenario. Initial-contact prediction was within 20 cm in floor, wall and oblique throw cases. The floor test accounts for fixed-step contact-notification delay and compares the first floor crossing. Logs are in `artifacts/carry-full.log`; color captures are `artifacts/crew-colors-front.png` and `crew-colors-back.png`. Automated checks do not replace extended human playtesting or testing on separate PCs.

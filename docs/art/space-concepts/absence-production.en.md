# THE ABSENCE production reference

[한국어](absence-production.ko.md)

> Discarded design: the user cancelled white THE ABSENCE. Do not use for production. Follow the [current THE STRIDER](strider-production.en.md).

2026-09-12 · SPACE-ART-13

![Production sheet](absence-production-01.png)

## Decision and scope

The user selected outer-threat candidate A and requested a simplified production sheet. Selection of A is confirmed; the revised appearance and rig approach are for review. Model production is not treated as requested or approved yet. Previous B/C alternatives remain comparison history.

## Form and parts

- 2 weightbearing legs and 2 shorter auxiliary arms that do not touch the ground.
- One arch torso; omit an external head, independent fingers and extra limbs.
- Propose a recessed opaque dark backing for the central void, rather than a through-hole or portal effect.
- A simple small inner body hangs from a short connection. It rotates/swings independently while staying inside the outer shell.
- Exclude thin cord bundles and real-time cloth simulation from the first build. Retain pale-shell/dark-interior contrast.

## Rig reference

Consider a generic rig with torso and paired leg/arm chains under a motion ROOT, and the smaller body attached to an INNER PIVOT inside the torso. Do not assume automatic humanoid rigging will succeed. Image dots show only selected conceptual joints, not a final hierarchy, joint count or bind pose. Allow deformation space at leg attachments and elbows. Limit inner-body amplitude to avoid contact with the shell.

## Initial motion proposal

| Motion | Intent | Review concern |
|---|---|---|
| IDLE | Outer body nearly still, inner body moves slowly | Excessive swinging and shell clipping |
| WALK | Short heavy alternating steps | Foot sliding, contact and lower-torso collision |
| TURN | Small steps to change heading | Planted-foot rotation and leg crossing |
| WARN | Forward lean and raised arms | Readable pre-attack pose |
| ATTACK | Simple single auxiliary-arm sweep | Shell clipping and visual/hit agreement |

Control inner motion independently. The unkillable outer-threat direction remains, but attack, pursuit, hiding and survival rules need separate design.

## Validation and outstanding work

Supplied the preceding A sheet to the built-in image tool and requested simplified anatomy, multiple views, parts/joints and basic poses. Visually checked paired legs/arms, inner body, directional views and motions. Some proportions, joint positions and surfaces differ across views; do not derive precise dimensions from the image. Scale, polygon/texture budgets, joint count and motion durations remain undecided.

- [ ] Review simplified appearance and approve model production.
- [ ] Check side/rear forms and joint layout in a grey model.
- [ ] Check ground contact and clipping during rigged walking, turning and attacking.
- [ ] Check dark backing across lighting and viewing angles.
- [ ] Evaluate silhouette and fear at gameplay distances, in fog and after suppression shutdown.

No code, model, rig or animation files have been produced.

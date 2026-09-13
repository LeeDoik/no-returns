# 1. Game overview

[한국어](01-overview.ko.md)

> **Historical record · retired/superseded 2026-09-12.** The [PSX space-delivery design](../../../../current/01-overview.en.md) is current. Implementation, launch, art and values below are historical, not current status. Deleted files remain path records only.


Main development is now NO RETURNS Unity. Godot specifications and checks below describe the preserved game, not completed Unity functionality. 현재 기준 / Current baseline (`18-unity-mainline.en.md`; retired file).

Reviewed: 2026-09-09 · Game 0.9.4 · Code baseline: this reactive-prop revision.


## Confirmed direction

NO RETURNS is a third-person 3D cooperative game about delivering living parcels. One to four players pursue shared goals. Stream-friendly situations should let viewers see accidents and teammates' responses together. Deliberate PvP betrayal is not the central premise. Keep English source copy and Korean UI.

The core loop is identify parcel traits → choose a route → carry/throw/catch and use devices → deliver to A/B → share rewards → choose upgrades → start the next contract. Sneezing, clinging and hopping parcels, packrats and environmental devices create cooperative accidents and recovery. Implementing this loop is distinct from validating its fun.

## Production scope

The current game has one Shipping Shrine map and a separate physics laboratory. Development uses Godot 4.7.2, targeting Windows/Steam. The current 0.9.4 executable is a development build, not a release candidate. Production retains legacy carrying. The Human: Fall Flat-inspired force-grab experiment was reverted at the user's request and is not a current feature.

Priorities are core controls/animation, meaningful map choices and situations, real four-player fun testing, and connection/release usability. More maps, full-body ragdolls and Unity migration are not committed scope. Do not promise completed Steam integration or store approval.

## Approved expansion direction · 2026-09-10

The user selected a shared truck base with on-foot expeditions, repeated 30–40-minute shifts, preserved completed earnings and accident recovery, and progression through tools/truck configurations. Expansion design revision 2 (`08-expansion-directions.en.md`; retired file) details Toypost Village connections, 5 contract types, 3 tools, economy, recovery, online requirements and validation criteria. The direction is approved; detailed values are experimental proposals and are not implemented. This task changes documentation only and neither fixes release scope nor replaces the existing map.

## Quality judgment criteria

The MDA analysis (`07-mda.en.md`; retired file) interprets core fun as cooperative recovery (Fellowship), transport mastery (Challenge), and discovering route/device solutions (Discovery). It separates parcel, contract and campaign loops; these are analytical hypotheses, not completed fun validation.

Success means new players understand the objective, intentionally cooperate, understand failures and want to replay. Automated test counts and subjective completion percentages are not release approval criteria. Read the [current specification](02-spec.en.md), [backlog](04-backlog.en.md) and [validation criteria](05-validation.en.md) together.

## EXP-01 implementation · 2026-09-10

Added separate EXP-01 playable: 80×80m fixed map, 3/5 contract selection, immediately saved payment of 30, unanimous return and next shift. Preserves the campaign; not the complete expansion.

Play guide and limits (`../prototype/10-expedition.en.md`; retired file).


2026-09-12 · New Unity delivery experiment: NR-LOOP-01 launch and validation (`22-slapstick-loop.en.md`; retired file) — use that document for implementation, automated evidence and outstanding human validation.

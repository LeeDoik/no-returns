# 1. Game overview

[한국어](01-overview.ko.md)

Reviewed: 2026-09-09 · Game 0.9.3 · Code baseline `1d8fbde` (update when behavior changes).


## Confirmed direction

NO RETURNS is a third-person 3D cooperative game about delivering living parcels. One to four players pursue shared goals. Stream-friendly situations should let viewers see accidents and teammates' responses together. Deliberate PvP betrayal is not the central premise. Keep English source copy and Korean UI.

The core loop is identify parcel traits → choose a route → carry/throw/catch and use devices → deliver to A/B → share rewards → choose upgrades → start the next contract. Sneezing, clinging and hopping parcels, packrats and environmental devices create cooperative accidents and recovery. Implementing this loop is distinct from validating its fun.

## Production scope

The current game has one Shipping Shrine map and a separate physics laboratory. Development uses Godot 4.7.2, targeting Windows/Steam. The current 0.9.3 executable is a development build, not a release candidate. Production retains legacy carrying. The Human: Fall Flat-inspired force-grab experiment was reverted at the user's request and is not a current feature.

Priorities are core controls/animation, meaningful map choices and situations, real four-player fun testing, and connection/release usability. More maps, full-body ragdolls and Unity migration are not committed scope. Do not promise completed Steam integration or store approval.

## Quality judgment

Success means new players understand the objective, intentionally cooperate, understand failures and want to replay. Automated test counts and subjective completion percentages are not release approval criteria. Read the [current specification](02-spec.en.md), [backlog](04-backlog.en.md) and [validation criteria](05-validation.en.md) together.

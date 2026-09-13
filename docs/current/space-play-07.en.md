# SPACE-PLAY-07 — Expanded map and suppression

[한국어](space-play-07.ko.md)

Current 0.8.2: fixed downed-host departure recovery, low-step routing/foot height, simultaneous F transitions, terminal collision, ship attack protection, the west rack slit and join-refusal feedback. Unreproduced target oscillation and the standing-host departure-abort policy remain design reviews. [Findings and evidence](review-fixes.en.md).

2026-09-13 · Build 0.8.1 global-pursuit change. The 0.8.0 validation record is preserved separately below. Expanded the map together with suppression and the outer creature at the user's request. Values below are experimental, not release commitments.

## Space and risk flow

Expand the floor from 18×26m to 32×44m. Preserve the ship, reception bench and clues, connecting an eastern service corridor, northern sorting detour and western storage area. Dividing walls, stacks and turning passages separate sight and movement routes. The 0.8.1 outer creature senses crew behind walls, so breaking sight alone no longer ends pursuit. This is not a new destination or final map art.

From arrival, progress through 4 stages: stable → unstable → imminent failure → off. Test transitions are 90, 135 and 180 seconds. Show no numeric countdown; communicate through device color, lighting intensity, signals and short status text. Continue after delivery, while waiting aboard and while reading the journal. Stop consumption at report and reset on next arrival.

The outer creature first appears at the east gate (13, 0, 28) during imminent failure. It enters 8 seconds after shutdown. The regular listener remains. This temporary outer creature cannot be killed or stopped with G and, after entry, tracks crew across the map without distance, visibility or noise requirements. Use patrol 2.2m/s, investigation 3.3m/s and attack warning 0.9 seconds. Empty-hand movement at 4m/s can escape; employees aboard are excluded from attacks. Beacons can distract it. It navigates around barriers rather than spawning beside a player without warning.

Both creatures share down state and post-rescue protection. All-down continues through existing emergency recovery, retaining secured pay. Final outer-creature models, dedicated hiding actions and new equipment are out of scope.

## Validation order

Use real time in two processes to check stage transitions, entry grace, positions and down states. Verify ship safety, ineffective outer-creature shove, all-down recovery, stage reset and traversability of the enlarged east/north/west passages. Regress existing delivery and rescue. Automated checks do not prove human fear, cue readability or map fun.

## Authoring and online contract

Edit map geometry in [CarryWorld.Build](../../NoReturns/Assets/_NoReturns/Runtime/CarryLab/CarryWorld.cs), suppression stages in [CarrySuppression](../../NoReturns/Assets/_NoReturns/Runtime/CarryLab/CarrySuppression.cs), and creature navigation/attacks in [CarryThreat](../../NoReturns/Assets/_NoReturns/Runtime/CarryLab/CarryThreat.cs). Preserve the east gate, corridor bend, connections around both ends of the north wall, and passage beside the west racks. Props currently use colliding placeholder boxes; they are not final PSX art.

[CarryRoom](../../NoReturns/Assets/_NoReturns/Runtime/CarryLab/CarryRoom.cs) advances field time and decides both creatures on the host. [CarryWire](../../NoReturns/Assets/_NoReturns/Runtime/CarryLab/CarryWire.cs) protocol 6 transmits elapsed time, stage, outer position and results. Elapsed time is synchronization/test data only; the play HUD and log show indirect status cues. Client light pulses use received time. Audio cue playback can differ slightly with each process's display cycle.

The safe ship area remains the blue floor. The entire expanded southern floor is not safe. Beacons can still only be placed in the original central delivery yard; deployment across every new corridor was not added. The outer creature does not change pursuit targets for footsteps or Q calls. Beacon attraction depends on its 12m range and current action, so it does not always stop the creature. Normal listener sound responses remain intact.

Launch [06_Play_Listener_Test](../../06_Play_Listener_Test.cmd), and join on the same PC with [07_Join_Local_Listener](../../07_Join_Local_Listener.cmd). Existing carry/delivery modes also share the expanded terrain. Reproduce with the [expanded map/suppression test](../../tools/test_suppression_build.py) and [player capture tool](../../tools/capture_expanded_map.py). Tests use isolated save folders.

## Screen inspection and remaining evaluation

Captured the [east corridor](../../artifacts/space-play-07-ui/run-20260913-121023/east-corridor.png) and [north detour](../../artifacts/space-play-07-ui/run-20260913-121023/north-detour.png) from an actual Windows window and inspected the walls, passages and stacks. These are preparation-phase 960×600 screens, not visual validation of every suppression stage. Human wayfinding, new route choices while carrying cargo, light/sound danger communication and replay appeal require separate play evaluation. Box props, creatures and the existing temporary HUD are not approved release art.


## Global automatic pursuit — 0.8.1

After entry, the host checks connected employees who are not down and are outside the ship every 0.8 seconds. It selects the closest by straight-line distance and recalculates a route to their current position. Visibility, noise and a maximum detection range are not required. Movement, down or boarding updates the target on the next scan. During attack warning and post-attack recovery, scanning resumes after that action finishes.

The outer creature's navigation now covers the full inner x=-15~15, z=-13~29 range. It follows an obstacle-avoiding grid route and excludes cells inside the ship. Sensing through walls does not allow walking through them or remote attacks. Hits still require the existing distance, visibility and warning conditions. Staying at the south edge or walking quietly no longer provides permanent safety.

A beacon pulse within 12m distracts the outer creature for 1.2 seconds after the last valid signal. When that time ends, the next scan reacquires crew. Beacons do not interrupt attack warning or post-attack recovery. The normal listener's sound search, G shove and R rescue rules remain. The diagnostic snapshot also transmits the pursued employee index as `pursuedPlayer`; this does not display crew locations on screen. Protocol 6 remains unchanged.

Validation order: use the [Unity rules fixture](../../tools/check_global_hunt.py) for distant occluded targets, southwest reach, target changes, down/ship exclusions, beacon expiry, solo and pre-entry dormancy. Use the [actual two-player test](../../tools/test_global_hunt_build.py) to wait through the real suppression time, then verify distant acquisition, moving destination updates, southern attacks, synchronization, all-down recovery and next-arrival reset. Accelerated fixture steps and real player elapsed time are distinct evidence.

## Automated results — 0.8.0

- [x] Passed [41 expanded map/suppression checks](../../artifacts/space-play-07/run-20260913-121121/report.json). Two actual Windows processes use ordinary input and real elapsed time. Includes 16 walking waypoints, preparation freeze, matching stages, gate waiting, entry grace, corridor movement, attack warning, ineffective shove, shared down state, ship safety, all-down recovery, report freeze and next-arrival reset.
- [x] Unity MCP compilation and Windows build succeeded. Protocol 6, build 0.8.0.
- [x] Inspected expanded walls, passages and stacks in actual preparation-phase player captures.
- [ ] Human wayfinding, cue readability and cooperative fun evaluation.

The first launch exposed a pre-construction creature-state initialization error, which was fixed. The next run exposed an automation driver continuing to its waypoint after emergency recovery. Stopping input at a waiting position corrected the driver, and the complete test passed again. Game reward/failure rules were not relaxed to satisfy the test.

Existing regression checks also passed on the same 0.8.0 build: [Carrying 17](../../artifacts/space-play-01/run-20260913-121606/report.json) · [Rescue/creature 21](../../artifacts/space-play-03/run-20260913-121626/report.json) · [Existing delivery 6](../../artifacts/space-play-03-delivery/run-20260913-121705/report.json).

The temporary batch editor was closed normally after the work. Its exit log reports a JobTempAlloc allocation warning; this remains an investigation item and is not confirmed as a gameplay leak in the player.

## Global pursuit validation — 0.8.1

- [x] Passed [12 Unity rules checks](../../artifacts/global-hunt/rules-latest.json). Accelerated steps verified occluded acquisition, north/southwest reach, nearest-crew selection, down/ship/disconnected exclusions, beacon distraction/reacquisition and pre-entry dormancy. Static wall/rack intersections were also checked along the northern pursuit path.
- [x] Passed [23 actual two-player checks](../../artifacts/global-hunt/run-20260913-124548/report.json). Waited through real suppression time to verify acquisition over 35m away, moving destination updates, southern-edge attacks, target changes, down/recovery and next-arrival reset.
- [ ] Human pursuit pressure, difficulty and beacon usefulness. Other PCs, network latency and 4 players were not covered.

Building exceeded the MCP response limit of 60 seconds, but the Unity build log reported Success and the new executable passed validation. The old implementation failed silent distant acquisition behind walls; the change passed it. The southwest fixture was adjusted to inspect the down event rather than the final position after returning to patrol. Automated input does not substitute for a human fun assessment.

The same 0.8.1 build also passed [21 normal-listener/shove/rescue regression checks](../../artifacts/space-play-03/run-20260913-125039/report.json).

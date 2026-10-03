# Cinder Listener, baton and rescue — 0.9.4

[한국어](cinder-listener.ko.md)

## 2026-10-04 — First-person rescue hands 0.9.16

Reach with both hands and show a small assisting movement during valid rescue. Hide the baton, restore the right-hand baton on cancellation/completion and hide hands when down. Retain the 25° wrist limit. [Current production, values and remaining scope](first-person-arms.en.md), [validation record](../validation/employee-rescue-0.9.16.json). Game **0.9.16**, protocol **13**, TCP **27842**. Preserve existing rescue adjudication. This supersedes older statements below that rescue hands are missing or hidden, within this scope. Beacon hands, full-body rescue/down, actual body contact and human quality review remain pending. Open manual test windows only on request.

## 2026-10-03 — Straight baton thrust 0.9.10

Retain immediate baton contact, range/facing/sight, Listener stun, 6s cooldown and rescue priority; change presentation to a straight thrust only. Supersede the arm/chest swing in 0.9.9 below. [Current behavior and validation scope](employee-baton.en.md).

## 2026-10-03 — Baton readiness, attack and recovery 0.9.9

Retain immediate baton contact, existing range/stun and 6s cooldown while connecting arm/chest attack posing. Prioritize simultaneous valid rescue over attack and hide weapon/attack posing. [Implementation, production and validation scope](employee-baton.en.md).

2026-10-02 · Implemented. Connect existing hazard rules to default Cinder delivery. This document supersedes older unconnected-Cinder-Listener/baton/rescue statements below. It is not human control, fun or fear quality approval.

## Running and controls

[07_Play_Cinder_4P.command](../../07_Play_Cinder_4P.command) launches 1 host + 3 clients. Gather aboard and use the E terminal buttons to select/depart. The Listener moves during field phases. In Editor: `Assets/_NoReturns/Scenes/CinderFourPlayerTest.unity` → Play → HOST/JOIN. Default build **0.9.4**, shared network protocol **13**, Cinder TCP **27842**. Connect matching versions.

- **Shift**: quiet walk. 1.5m/s empty-handed, 1m/s carrying cargo, no footstep noise. Normal movement is 4m/s empty-handed and 2.5m/s carrying cargo.
- **C**: call attracts investigation of the last position within 12m. Normal footsteps reach 6m; Q parcel release reaches 10m. Ignore calls inside the safe ship.
- **Left click**: empty-hand baton. 2.5m, forward 65° and sight clear of static obstacles; 3-second stun, 6-second cooldown. Carrying parcel/beacon uses placement instead. Two seconds of stun resistance follow the stun.
- **Hold E**: rescue a down teammate within 2m, clear sight and empty hands for 2.5 seconds. Release, distance, wall, changed target or rescuer down cancels progress. Four seconds of attack protection after rescue.

Field empty-hand prompts use current Shift/C/left-click bindings and baton cooldown. Nearby rescue/progress takes priority over E parcel/ship UI. Down blocks movement, pickup, baton and departure, releasing cargo/beacon. Menus do not pause the world.

## Current-map integration

[CarryThreat](../../NoReturns/Assets/_NoReturns/Runtime/CarryLab/CarryThreat.cs) reuses the existing 1m static grid and BFS. Sample x=-27~25, z=-34~30m, ground height changes up to 0.32m; check supporting floor, 0.84×1.6×0.84m clearance and swept boxes between cells. Stairs, dynamic geometry and routing around employees/moving cargo are outside scope. Current map uses ground routes only.

Patrol x/z sequence: **(-23,2) → (-23,-10) → (-5,-10) → (-5,14) → (-23,14)**m. Patrol 1.4m/s, last-noise investigation 2.5m/s, interest 5 seconds. Within 1.3m and clear sight, warn for 1.2 seconds, then down a crew member remaining within 1.5m; recover from attack for 4 seconds. Do not attack directly through walls. Reuse warning body color/placeholder audio and existing baton model/integrated display. No final Listener model or animation is produced.

Use Cinder coordinates from [CarryMission.Aboard](../../NoReturns/Assets/_NoReturns/Runtime/CarryLab/CarryMission.cs) for shared ship safety. Exclude ramp/deck from patrol grid. Connect one Listener and one outer creature at current east coordinates. Follow the [suppression guide](cinder-suppression.en.md) for 4 suppressor stages and outer entry/distraction. Do not instantiate old clue/save coordinates; clues and Cinder progression saving remain next steps. Preserve the existing legacy hazard mode.

After all crew remain down for 3 seconds, abort the shift to an unpaid report and recover four employees inside the actual ship at **x=-21.65/-19.75, y=1.035, z=-25.5/-26.9m**. Preserve existing wallet, pay no new delivery reward. Ship buttons prepare the next shift, resetting employees, parcel, down and cooldown state. Preserve normal receipt return payment of 300+120=420 CR. Preserve disconnect-aborts-shift/down-crew-recovery rules.

Existing beacon pulses also reach Listener `Hear`, so they can attract investigation within 12m. Follow [controls/purchase guide](controls-ui.en.md) for the 120 CR license and two 8-second uses. Beacon distraction also affects the outer creature after entry. The baton cannot stun the outer creature. Wallet/license remain session-only and do not restore after exit.

## Production and prior 0.9.3 validation record

Prioritize the [0.9.4 record](../validation/cinder-suppression-0.9.4.json) for current execution checks.

Preserve source art scene and reuse derived-scene/build tooling. No new models, textures or packages. `--map-only` runs movement/carrying; `--delivery-only` runs peaceful delivery UI regression. Default launch is delivery + Listener + suppression/outer. Automated input/evidence files remain restricted to the existing test build.

```sh
source ~/.unity/env
python3 tools/cinder_four_player.py build
python3 tools/test_cinder_threat.py
python3 tools/test_cinder_delivery.py
python3 tools/cinder_four_player.py check
python3 tools/cinder_four_player.py start --delivery-only
python3 tools/cinder_four_player.py stop
```

[Grid/patrol/ship-safety/wall-rescue check](../../tools/unity_checks/CinderThreatCheck.cs) uses native physics in the current derived scene. Distinguish explicit state setup in isolated rule checks from actual four-player checks. The [actual four-process check](../../tools/test_cinder_threat.py) uses movement/use/call/baton/rescue inputs only, without teleport or down injection. Delivery regression disables hazard and checks UI/ledger separately.

Prior Listener integration validation follows the [0.9.3 record](../validation/cinder-listener-0.9.3.json). Four-human carrying/evasion/rescue fun, warning-audio readability, Listener appearance/HUD readability, other-PC/LAN, latest Windows, performance/extended stability remain unverified.

Mac build: 0 errors/7 existing warnings. Pass 92 map-route endpoints, 2,400 patrol collision steps, 5 rescue conditions, 7 existing four-crew rescue conditions, 17 controls/UI, 35 ledger, 13 HUD behavior/100 text-height conditions. Pass **31** actual four-process hazard checks and inspect native 800×500 bilingual screens. [Korean baton/down teammate](../../art/cinder-kit-01/listener-baton-ko.png) · [English down prompt](../../art/cinder-kit-01/listener-down-en.png). At that time, receipt/normal return under an active Listener and purchased beacon distraction were not separately executed. Follow the [0.9.4 guide](cinder-suppression.en.md) for current integration/execution verification.

Also pass **48** actual four-process peaceful delivery/UI regression checks and **4** legacy-map patrol endpoints. The rescuer receives no revival protection and must keep evading. The placeholder creature body has no player/cargo physics collider; distance and sight judge attacks.

Pass **13** final actual four-process movement/connection/fifth-peer rejection/disconnect-rejoin regression checks. Stop all automated sessions after validation. At that time, the full docs check failed on 142 pre-existing missing links (0 new errors), resolved by subsequent [repository cleanup](repo-hygiene.en.md); the raw diff check also failed on 4 native scene trailing spaces. Code/docs/evidence and whitespace checks ignoring blank-at-eol pass.

At that time, four manual windows were launched from the final rebuild without test-input folders and 3 host TCP connections were verified. They were left running for user review; this did not verify human input/quality. Stop with [08_Stop_Cinder_4P.command](../../08_Stop_Cinder_4P.command).

# SPACE-PLAY-03 — Noise, down and rescue experiment

[한국어](space-play-03.ko.md)

2026-09-13 · Proposed test values. Apply only to `--hazard`, preserving carrying/delivery modes. This is not final art or the full release scope.

## Implementation plan and acceptance criteria

1. LISTENER patrols the northern work yard (z≥3). Normal steps within 6m, Q call within 12m and cargo set-down within 10m cause investigation of the last sound position. Shift quiet walk moves at 1.5m/s, or 1m/s while carrying, without step noise. Staying still, quiet movement and teammate distraction are baseline solutions.
2. The creature navigates a yard grid excluding obstacles. Patrol speed 1.4m/s; investigate speed 2.5m/s. At close range it warns for 1.2 seconds, then downs only employees within 1.5m and line of sight. Recovery lasts 4 seconds. G empty-hand shove requires 2.5m, a 65-degree view cone and line of sight; it stops the creature for 3 seconds with a 6-second cooldown. Killing and final equipment are excluded.
3. Down blocks movement, pickup and departure and releases cargo. Hold R empty-handed within 2m of a teammate for 2.5 seconds to revive. Leaving range, releasing the key, obstruction or the rescuer going down cancels progress. Grant 4 seconds of protection after revival. If everyone stays down for 3 seconds, return to the ship report, preserving secured pay but paying nothing for undelivered cargo. Death, carrying employees and persistence are follow-up work.
4. The host decides noise, pursuit, attack, shove, down, rescue and settlement; replicate to two executables. New HUD/controls retain Korean default and English toggle. Creature color and temporary warning sounds provide cues; actual audio readability requires human evaluation.
5. Automated checks cover quiet approach, noise response, warning before attack, down/cargo release/movement restriction, remote rescue rejection, rescue cancellation/completion, shove/cooldown and all-down recovery. Regress existing delivery pay. Keep actual fun, fear and route understanding separate from automated checks.

Files: CarryThreat (behavior/recovery/placeholder presentation), CarryRoom (input/carry integration), CarryWire (replicated state), CarryLanguage (bilingual HUD), test_hazard_build.py (real executables). The runtime grid is specific to the placeholder yard and does not finalize navigation for the actual CINDER DEPOT map.

## Launch and current scope

Create a room from [06_Play_Listener_Test.cmd](../../06_Play_Listener_Test.cmd); join on the same PC with [07_Join_Local_Listener.cmd](../../07_Join_Local_Listener.cmd). Use F twice to select the route/arrive, then enter the northern passage. When the creature stops and turns red, step away or use empty-hand G. If a teammate goes down, set cargo down with E and hold R nearby. Korean default and the menu language toggle remain.

This is an ordinary creature inside the work yard. Outer STRIDER, suppression and indirect time pressure remain absent. Sound and body are placeholders. The creature has no employee/cargo physics collider; close attack distance determines contact risk. The yard grid handles static geometry; moving cargo/employees are not path obstacles. Line of sight uses static obstacles. Ship noise is ignored; outside sounds can draw it to the yard boundary.

Normal return requires all standing employees aboard. Down/rescue use movement restrictions and placeholder poses, not final animation. Emergency recovery ends this shift at the report; F prepares another. Secured pay remains but no return bonus is awarded. The wallet remains session memory without persistence.

Protocol is 3. All connected executables must use the same version. The host decides inputs, down, rescue progress, noise positions, creature state and payment; clients display results.

## Validation evidence

The [two-executable checks](../../artifacts/space-play-03/latest.json) use actual movement/key input. The [test runner](../../tools/test_hazard_build.py) does not inject teleportation or direct down states. Distinguish the Unity mission-rule check of secured-pay retention from actual delivery/rescue execution. Human fear, controls, warning-audio readability and cooperative fun remain unverified.

## Confirmed results

- [x] Windows 0.4.0 build and Unity compilation.
- [x] 21 hazard checks passed across two real processes: noise, warning, down, cargo release, shove, rescue cancellation/completion and emergency recovery.
- [x] Existing carrying 17 and delivery 21 regression checks passed.
- [x] Unity mission-rule check: received 300 CR retained after abort, return pay 0, no extra payment from repeated abort.
- [x] Game-camera rendering inspected for the placeholder creature and downed teammate. This differs from full HUD/audio-readability validation.
- [ ] Human cooperative fun, fear, controls, bilingual HUD readability and warning-audio evaluation.

Testing exposed an out-of-range shove attempt and boundary stopping tolerance. Strengthened the test to shove within range after an attack warning, and fixed outside sound disappearing just before the yard boundary. Automated state checks are not human fun evaluation.

Additional executable validation: 6 [quiet alternate-route delivery checks with an active creature](../../artifacts/space-play-03-delivery/latest.json) passed; 6 [solo down/automatic recovery checks](../../artifacts/space-play-03-solo/latest.json) passed. [Build/payment-rule evidence](../../artifacts/space-play-03/build-and-ledger.json). Quiet-route validation covers receipt, not return while pursued.

# Independent code review integration — 0.8.2

[한국어](review-fixes.ko.md)

2026-09-13 · Rechecked the Claude Code review supplied by the user against current sources and executable builds. Not every reported item is treated as a reproduced defect. Applied fixes and defensive changes that preserve the design, retaining unverified and design issues below.

## Findings and changes

| Item | Evidence | 0.8.2 disposition |
|---|---|---|
| 1. Downed host and partner departure | Reproduced permanent down with 2 actual Windows players on the original 0.8.1 build | Emergency recovery returns a downed host to the ship on departure. Movement, F preparation and rejoining work from the report phase. Secured pay follows the existing abort preservation rule. |
| 2. Low-step safe spot | Before modification, neither creature reached the employee at its center in the Unity physics fixture | Build navigation cells from floor support and low steps up to 0.32m. Probe the footprint during movement to keep the body above the surface. Retain the 0.3m step employees can climb. |
| 3. Simultaneous F skips phases | Reproduced duplicate transitions from preparation, loading and delivery-complete phases through actual FixedUpdate calls | Process F only while the current phase matches the phase at the start of the physics tick. After a transition, consume other F actions in that tick. Duplicate payment was not reproduced; existing settlement rules remain. |
| 4. Missing terminal navigation obstruction on first arrival | Recorder cells (-8,10), (-8,11) were incorrectly open in the Unity fixture | Apply clue activation before creatures build routes. Discard navigation cells on room reset and rebuild on the next route request. |
| 5. Missing ship protection for the normal listener | Missing defensive condition confirmed in source; no attack reproduced through its natural route | Both listener and outer creature exclude aboard employees at attack start and hit resolution. Verify the guard in a Unity fixture that explicitly places the creature nearby. |
| 6. West rack slit | The original Unity CharacterController could enter between the rack and wall | Move rack center x from -14 to -14.75, flush with the west wall. Preserve the inner carrying aisle. Check that the actual player cannot enter the former slit. |
| 7. Possible nearest-target oscillation | No specific oscillation or stall reproduced | Open design review. Retain Euclidean nearest-target choice and the 0.8-second rescan. Human testing with employees moving on opposite sides of a wall remains necessary. |
| 8. Missing refusal reason during a shift | Source inspection and post-fix actual rejoin test | Send and localize reasons for a shift in progress or no free slot in Korean and English. Rejoining is allowed after F returns the report to preparation. |
| 9. Creatures freeze after a standing host loses the partner | Matches the existing abort rule | Retained for design review. A standing host walks back after the shift aborts. Intentional disconnect abuse and future cooperative failure policy require a separate decision. |

## Production and online limits

[CarryRoom](../../NoReturns/Assets/_NoReturns/Runtime/CarryLab/CarryRoom.cs) owns departure recovery, F transitions and clue activation order. The footprint box probe and adjacent height limit in [CarryThreat](../../NoReturns/Assets/_NoReturns/Runtime/CarryLab/CarryThreat.cs) support the current flat floor and low steps. This is not general navigation for multiple floors, arbitrary slopes or moving platforms. Tall obstacles and walls still require detours.

Rack placement lives in [CarryWorld](../../NoReturns/Assets/_NoReturns/Runtime/CarryLab/CarryWorld.cs); refusal messages use the optional rejection field in [CarryWire](../../NoReturns/Assets/_NoReturns/Runtime/CarryLab/CarryWire.cs) and [CarryLanguage](../../NoReturns/Assets/_NoReturns/Runtime/CarryLab/CarryLanguage.cs). Protocol remains 6. Tests use matching 0.8.2 players; mixed-version connections are unverified. Save format and economic values are unchanged.

## Validation evidence

- Before: the new Unity regression fixture returned PASS 1 / FAIL 8. The actual 0.8.1 player test also failed downed-host departure recovery. Dated failures remain under `artifacts/review-fixes/`.
- After: [Unity rules/physics fixture](../../tools/check_review_fixes.py) passed 11 checks. It uses actual classes/colliders with accelerated time calls. Coverage includes both creatures reaching the step, simultaneous F, settlement 1 time, terminal collision, defensive ship exclusion and slit closure. [Result](../../artifacts/review-fixes/rules-report.txt).
- After: [actual Windows regression](../../tools/test_review_fixes_build.py) passed 17 checks. It runs 2 executables with isolated saves and ordinary scripted input. Coverage includes down/departure recovery, movement/next shift, Korean rejection feedback, rejoin, synchronized down on the step and physical exclusion from the slit. [Fixed run record](../../artifacts/review-fixes/run-20260913-133845/report.json).
- Unity MCP build succeeded. [Play](../../06_Play_Listener_Test.cmd) · [Join locally](../../07_Join_Local_Listener.cmd).

Scripted input and accelerated physics checks are distinct from human judgments of feel, fear and fun. This completion claim excludes 4-player play, Steam connectivity, external internet conditions and actual reproduction of target oscillation.


## Additional regressions — 0.8.2

Passed 12 Unity global-hunt checks and 17 Unity payment/authority/reset checks. Actual players passed [23 global-hunt checks](../../artifacts/global-hunt/run-20260913-133956/report.json), [21 rescue/shove checks](../../artifacts/review-fixes/rescue-0.8.2.json) and [6 hazardous-delivery checks](../../artifacts/review-fixes/delivery-0.8.2.json). Including the new suite, totals are 67 actual-player checks and 40 Unity rules/physics checks. Compared both players and did not use personal save files.

The rules fixture constructs game objects directly in edit mode. Edit-mode error logs from the runtime Visual Destroy call remain a fixture limitation. They are not classified as player runtime errors; the tested physics queries exclude employee child colliders. Actual player logs did not contain that error, and compilation did not fail. This validation does not measure rendering quality or performance.

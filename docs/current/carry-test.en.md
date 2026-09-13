# First-person carrying experiment — launch and validation

[한국어](carry-test.ko.md)

Current 0.8.2: fixed downed-host departure recovery, low-step routing/foot height, simultaneous F transitions, terminal collision, ship attack protection, the west rack slit and join-refusal feedback. Unreproduced target oscillation and the standing-host departure-abort policy remain design reviews. [Findings and evidence](review-fixes.en.md).

Added in 0.8.1: after entry, the outer creature automatically pursues crew across the entire map. Downed/aboard employees are excluded; pursuit resumes after beacon distraction. Walls require detours but do not block detection. [Rules/validation](space-play-07.en.md).

Added in 0.8.0: expanded the map to 32×44m with an east corridor, north detour and west storage wing. Four suppression stages and delayed outer-creature entry are implemented. Values, geometry and creature visuals remain experimental, not final art. [SPACE-PLAY-07](space-play-07.en.md).

Added in 0.7.0: I clue inspection and the Tab shared field log. Records reset on next arrival and are not restored after exit; wallet/license saving remains. [Clue specification/validation](space-play-06.en.md).

As of 0.6.0, host progression saves restore wallet, beacon license and successful-delivery count; shifts restart at ship preparation. Earlier session-only/no-save notes describe the state through 0.5.0. [Save rules and validation](space-play-05.en.md).

2026-09-12 · SPACE-PLAY-01 · Internal experiment build 0.4.0

## Launch

Open [02_Play_Carry_Test.cmd](../../02_Play_Carry_Test.cmd) and select HOST. Open [03_Join_Local_Carry_Test.cmd](../../03_Join_Local_Carry_Test.cmd) to join on the same PC. The executable is `builds/CarryTest/NoReturns.exe`; retain its adjacent data folders. JOIN also accepts another host PC's LAN address, but this validation used two processes on the same PC. Transport is direct TCP 27841; Steam invitations and internet relay are absent.

WASD moves, mouse looks, E picks up/sets down, Space jumps empty-handed, Esc opens the menu, and R resets on the host. Adjust FOV in the menu. Click each window before controlling it. Host departure reports disconnection without host migration. Client departure releases held cargo.

## Current rules

Source: [CarryRoom.cs](../../NoReturns/Assets/_NoReturns/Runtime/CarryLab/CarryRoom.cs). Capacity 2 players; movement speed 4, carrying speed 2.5, pickup reach 2.4, view cone within 65 degrees plus line-of-sight check. Cargo mass is 8; the hold target adds the local offset (0, -0.54, 1.1), rotated by the full view, to eye height 1.57. At level view this is 1.03 above the feet; vertical look changes cargo position and tilt. Reject a new rotation that overlaps walls/floor. A box sweep matching cargo dimensions stops it before walls. Default FOV 80, range 65~100. The host decides ownership, movement and cargo outcomes. Clients interpolate received positions without prediction or latency compensation.

[CarryWire.cs](../../NoReturns/Assets/_NoReturns/Runtime/CarryLab/CarryWire.cs) handles TCP transport; [CarryWorld.cs](../../NoReturns/Assets/_NoReturns/Runtime/CarryLab/CarryWorld.cs) generates the placeholder room. Props are generated at runtime; direct scene editing is not available yet. Bootstrap is preserved. Final PSX art, hands/employee animation, CINDER DEPOT, Creature/rescue experiments are implemented in [separate hazard mode](space-play-03.en.md). Persistence remains absent. A separate [delivery mode](space-play-02.en.md) connects route selection, receipt, return and temporary pay.

## Development environment and build

Unity was running under an isolated account, making it invisible to the user; launching under the user's account resolved this. The earlier instruction to find an invisible terms dialog was an inaccurate diagnosis. Mono assembly-path conversion errors on the Korean path were resolved by opening the `C:/Users/LeeDoik/Documents/NoReturnsUnity` junction without moving the source project. `tools/unity.ps1` and `tools/unity_mcp.py` use the junction after verifying its target matches the original project.

Build through `NO RETURNS/Build Carry Test` in [CarryBuild.cs](../../NoReturns/Assets/_NoReturns/Editor/CarryBuild.cs). Local `NoReturns/CarryWorkspace.txt` identifies the original workspace and is excluded from Git. Resources/CarrySurface.mat is included to prevent stripped shaders. Test input `--test-dir` is enabled only under `CARRY_TEST_AUTOMATION` or `UNITY_EDITOR`. The current internal build includes the former and must not be distributed as the general release build.

## Results and remaining evaluation

- [x] Unity MCP connection, editor compilation and Windows build succeeded.
- [x] All 17 [two-process automated checks](../../tools/test_carry_build.py) passed. [Latest result](../../artifacts/space-play-01/latest.json).
- [x] Actual player-camera rendering inspected. Automated captures use URP camera output, not a complete menu/HUD screen capture.
- [ ] Two humans evaluate visibility, latency, controls, discomfort and fun.
- [ ] Separate PCs, WAN, Steam connectivity and 4-player validation.

Checks cover connection/movement across two executables, pickup/height/ownership contention/rotation/drop/client pickup, release on departure, player-wall collision, re-pickup and cargo movement followed by stopping before a wall. Strengthened height and travel assertions reproduced cargo failing to lift from the floor before the fix. Earlier failed logs remain. Automated passes do not replace human control-feel evaluation or validate a complete delivery game.

## 2026-09-13 — Guidance language (0.3.1)

First launch defaults to Korean. Use **한국어 / English** at the bottom of the start or Esc menu to switch immediately; the choice survives restart. This is a local per-player setting and does not propagate to teammates. English remains source copy; menus, controls, delivery objectives, payment, connection status and errors have Korean counterparts. Delivery-mode controls show F ship actions instead of R reset.

Maintain English source and Korean entries together in [CarryLanguage.cs](../../NoReturns/Assets/_NoReturns/Runtime/CarryLab/CarryLanguage.cs). Uses Windows Malgun Gothic; other operating systems or environments without the font remain unverified. No font file is copied or redistributed with the project. PlayerPrefs NoReturns.Language stores this preference separately from game-wallet persistence. Automated tests use separate preference keys.

Language validation: 9 automated checks passed across two Windows processes (Korean default, Korean glyph, independent toggles and restart persistence). Window capture did not reliably capture the game, so visual validation of wrapping/button readability remains incomplete.


## SPACE-PLAY-04 — Supply/risk contracts (0.5.0)

[Purchase/use specification](space-play-04.en.md). Listener mode now implements session beacon-license purchase, shared deployment and risk-contract selection. Persistence, the full shop, more destinations and final art remain. Use existing launchers 06/07. Two-process progression checks passed 24, existing rescue checks 21, and language checks 9. Human feel, economy balance and rendered readability of the new supply panel are unverified.

0.8.16: The black rectangle in the startup menu was the local character visor. Update returned early for inactive sessions, skipping body visibility. Moved visibility into LateUpdate so it runs before rendering in both menus and gameplay. A temporary Unity scene regression recorded hidden=False before and True after recompilation. Windows build verification is separate from human startup-screen confirmation, which remains pending.

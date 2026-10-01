# Four-player cooperation — current rules and validation

[한국어](four-player.ko.md)

## 2026-10-01 — Cinder four-player map test environment

**CINDER-4P-01**: derive a separate `CinderFourPlayerTest` from the current `CinderCompactSiteReview`, connecting the existing host-authoritative `CarryRoom` crew of 4, movement, shared-parcel E/Q and reset. Preserve the source art scene and regenerate the test scene on each build. Regular experiments use TCP **27841**, Cinder uses **27842**, and clients reject snapshots with a different `cinderReview` value. Retain protocol **10** and game version **0.9.1**. The test caps rendering at **30fps**, runs physics at **50Hz** and sets camera distance to **250m**; these are settings, not measured performance. [Runtime](../../NoReturns/Assets/_NoReturns/Runtime/CarryLab/CarryRoom.cs) · [scene generation/build](../../NoReturns/Assets/_NoReturns/Editor/CinderFourPlayerBuild.cs).

On Mac, double-click the [launch file](../../07_Play_Cinder_4P.command) to start 1 host and 3 clients at same-PC `127.0.0.1`. The [stop file](../../08_Stop_Cinder_4P.command) terminates only recorded processes whose executable and session identifier both match. Game windows run independently after closing the terminal; persisted session restoration is unsupported. Restarting after stopping creates a new room. One computer's keyboard/mouse controls its selected window, so evaluating four human players requires separate PCs.

```sh
# Repository root. Open this project's Unity Editor, stop Play and save the scene.
source ~/.unity/env
python3 tools/cinder_four_player.py build
python3 tools/cinder_four_player.py start
python3 tools/cinder_four_player.py stop
# Stop manual windows before checking four actual processes
python3 tools/cinder_four_player.py check
```

The Mac player is `builds/CinderFourPlayer/NoReturns.app`. Build on the Editor's next update, confirming completion/errors through `artifacts/cinder-four-player/build.json`. Avoid directly invoking a synchronous build exceeding the CLI request's 5-second limit. Builds, personal logs and session PIDs are Git-excluded; preserve only the review [validation record](../validation/cinder-four-player-01.json). Use Python's standard library. Windows generation menu/tool paths are provided, but Windows build/execution is unverified in this task.

Controls are WASD movement, mouse look, E shared-parcel pickup, Q release owned parcel, Space empty-hand jump, Esc menu and R host reset. Orange/cyan/purple/yellow helmets and `CREW {0}/4` identify crew. Spawn outside the ship at x=-21.7/-19.7m, z=-20.05/-18.65m, y=0.035m. Parcel starts at (-20.7, 0.55, -18.5)m. Peers cannot release another player's parcel. Reject a fifth client and reuse vacant slots after disconnect.

For four human players on the same LAN, distribute the same test player to each device and open the app directly: one HOST, three JOIN using the host's LAN address. Allow TCP 27842 to the host. Other-PC/internet/Steam connections are unverified in this task. The automatic launcher targets one PC.

**Implementation/validation status: Mac build: 0 errors/7 existing warnings; pass 13 actual four-process checks, 7 existing rescue-rule conditions and 7 regular-mode defaults. Inspect 2 native 800×500 Korean HUD captures for crew 4/4, E/Q controls, reticle and team colors; also verify 4 manual processes/3 connections and shutdown.** Cinder delivery, receipt collection/settlement, Listener, baton, suppression stages, beacon and progression save remain unconnected. `--hazard`/`--delivery` do not activate these systems in test mode; hide existing static Listener markers only in the test scene. Do not inherit full Cinder gameplay validation from the older Windows cooperation results below. Four-human fun/passing/simultaneous parcel rotation, performance and extended stability remain pending.

![Cinder host / crew 4/4](../../art/cinder-kit-01/crew-player-0.png)

![Cinder client / crew 4/4](../../art/cinder-kit-01/crew-player-3.png)

2026-09-13 · 0.9.0 implemented and automated checks complete. Retain Unity and direct LAN host authority. Steam and operator servers are outside scope.

- [x] CarryRoom/CarryWire: host slot 0, client slots 1~3, per-connection input/timeouts and recipient slot assignment. Do not trust input-supplied slot IDs. Protocol 10 prevents mixing old versions.
- [x] Employees/equipment/HUD: 4 fixed slots with an occupancy mask and distinct team colors. Empty slots are invisible and non-colliding.
- [x] CarryThreat: attacks/cooldowns/down states for all connected crew. Rescue the nearest down teammate within 2m; changing targets resets progress. Count only connected crew for all-down recovery.
- [x] Carry/delivery: retain single cargo/beacon ownership and identical E/Q behavior for slots 2~3. Return requires all connected crew aboard. Recover carried items on disconnect and retain the existing shift-abort rule.
- [x] Use 4 real Windows processes to verify assignments, carrying, equipment, receipt and settlement. Check overflow rejection and disconnect/rejoin. Run existing 2-player regressions and rescue logic checks.
- [x] Update bilingual specifications/checklists/history, executable and Git.

Separate scripted tests from fun and control-feel assessment. Do not mark other-PC/WAN/Steam/human 4-player evaluation complete. Retain p0/p1 and similar evidence aliases while gameplay reads player-state arrays.

## Current usage and rules

Use the same 0.9.0 executable. Launch with 06_Play_Listener_Test.cmd; one player hosts and up to 3 join using the host LAN address. Use 127.0.0.1 for same-PC testing. Maximum crew is 4 including the host; new joins are allowed only during shift preparation.

Orange, cyan, purple and yellow helmets identify slots. Empty slots have no visible employee or active collider. HUD shows current crew/4. Controls remain E pickup/interaction/hold rescue, Q put down, LMB baton and Shift quiet walk. Rescue progress belongs to the rescuer. Accumulated time restarts when the nearest rescue target changes.

Normal return requires every connected employee aboard and not down. A departing participant still aborts the shift and releases their cargo/beacon. If any remaining employee is down, recover aboard. Otherwise preserve their field positions. After returning to preparation, assign empty slots to new arrivals. Host migration, mid-shift joining and operator servers are not implemented.

Protocol is now 10. Only the slot assigned to an input socket is updated; clients cannot select their own slot. Input inactivity and timeouts are per connection. Retain p0/p1 as evidence aliases, while actual replication/display uses positions/yaws, down/rescue/cooldown arrays and occupiedMask.

## Running validation

- python tools/test_four_player.py: 4-player equipment/delivery/settlement, overflow and rejoin.
- python tools/test_four_player_rescue.py: 4 actual processes with slot 2 down and slot 3 stun/rescue.
- Unity MCP run_script with tools/unity_checks/FourCrewRules.cs, entry FourCrewRules.Run: 7 rescue targeting/progress/all-down logic conditions.

All executable checks use dedicated test-dir saves, separate from user progression. Preserve failed run records. Early routes were corrected for crew collision, the ship E return action and actual receipt location; state-file replacement reads now retry.

## Evidence and outstanding validation

Real 4-process Windows tests passed 31 equipment/delivery/settlement/connection checks and 15 down/stun/rescue checks. Existing real 2-process tests passed 24 baton/rescue/all-down recovery checks and 30 delivery/receipt/settlement checks. Unity MCP passed 7 rescue-target/progress/occupancy-mask logic checks. Multiple resolutions, human four-player control feel, long-session stability, latency and bandwidth need separate validation.

A camera render capture confirmed slot 3 first-person baton and world rendering. Hidden-window screen.png captures are black and are not HUD visual evidence. HUD crew count is checked in source and replicated state; human readability remains untested. Final review retained 2 journal clues, independent of the employee slot count.

Source evidence: [network](../../NoReturns/Assets/_NoReturns/Runtime/CarryLab/CarryRoom.Network.cs), [threat](../../NoReturns/Assets/_NoReturns/Runtime/CarryLab/CarryThreat.cs), [room/HUD](../../NoReturns/Assets/_NoReturns/Runtime/CarryLab/CarryRoom.cs), [four-process test](../../tools/test_four_player.py), [rescue test](../../tools/test_four_player_rescue.py), [Unity rules](../../tools/unity_checks/FourCrewRules.cs).

[Retained automated results](../validation/four-player-0.9.0.json). Four-player rescue and two-player regressions ran on 0.9.0 before the final journal-only UI correction. After that isolated correction, rebuilt Windows and reran the full four-player session; all 31 checks passed.

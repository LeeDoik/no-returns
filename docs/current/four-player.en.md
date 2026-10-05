# Four-player cooperation — current rules and validation

[한국어](four-player.ko.md)

## 2026-10-05 — visit ledger, revival and departure 0.9.23

Cinder receipts record 420 CR as unbanked earnings, paid only on departure. A living occupant spends 100 unbanked CR to revive one eliminated teammate after 10 seconds. Anyone can start/cancel a 12-second departure; deduct 100 CR per abandoned teammate with a zero floor. Integrate health, repeat downs/elimination and all-eliminated/liftoff during revival. [Implementation, values and limits](visit-settlement.en.md), [validation](../validation/visit-settlement-0.9.23.json). Game 0.9.23, protocol 14, TCP 27842. Supersedes historical Cinder all-aboard return, immediate settlement and one-hit down rules in this scope. New tools, multiple deliveries, Cinder saves/reconnects, final danger timing/audio and human review remain.

## 2026-10-04 — First-person wrist correction 0.9.15

Fix the wrist bend reported by the user in the 0.9.14 attack preview. Correct the relationship between the palm and cylindrical grip axes; move the elbow outward so the forearm and hand align. Limit the hand/forearm direction angle to 25° and solve grip contact again. Keep the baton upright during forward movement and retract the shoulder origin near walls. [Current pose/production](first-person-arms.en.md), [validation record](../validation/employee-wrist-0.9.15.json). Game **0.9.15**, protocol **13**, TCP **27842**. The previous pose has a user-confirmed quality issue; the revised pose has not received user approval. Manual play stays stopped.

## 2026-10-04 — Dynamic baton and first-person hands/arms 0.9.14

At the user's request, implement short anticipation, fast extension and 0.42s recovery through baton arm/chest motion, local right glove/sleeve and two-hand parcel carrying. Preserve immediate contact, 6s cooldown, carrying/down/valid-rescue blocking and existing movement. [Current baton](employee-baton.en.md), [arm production/local reproduction](first-person-arms.en.md), [validation evidence](../validation/employee-first-person-0.9.14.json). Game **0.9.14**, protocol **13**, TCP **27842**. Earlier 0.9.13 original-motion restoration and first-person-arms-not-implemented/proposed states below are historical and superseded within this implementation scope. Dedicated beacon/rescue hands, human quality, actual game GPU composition, all-corner penetration, Windows/LAN and performance remain pending. Manual play stays stopped; use the existing two-window helper only on request.

## 2026-10-03 — Original baton restored; backward/sidestep movement 0.9.13

At the user's request, restore the initial right-hand attachment version (0.9.6) of baton readiness/use. Remove the arm/chest attack correction introduced in 0.9.9; retain hand attachment, the weapon's 0.5s return, immediate contact, 6s cooldown and carrying/down/rescue hiding. Redirect backward, lateral and diagonal foot trajectories from actual body-relative movement. Reuse alternating foot timing/heights from the forward Walk while preserving torso facing, root, physics and network rules. Retain jump/landing. No new Mixamo FBX or Blender editing. [Baton](employee-baton.en.md), [movement](employee-locomotion.en.md). Revised human quality, dedicated clips, first-person arms and Windows/LAN/performance remain unverified.

## 2026-10-03 — Automatic test-window tiling 0.9.11

Two-player practice places the human host on the left and the automatic companion on the right at equal widths in landscape 16:9 viewports. Four-player tests place slots 0/1/2/3 in top-left/top-right/bottom-left/bottom-right quarters, preserving 16:9 inside each tile. Calculate sizes from the display work area with title-bar and menu-bar clearance. Allow manual resizing. Apply placement only at startup, never to windowless automated checks. Open manual windows only on request; this task relaunches the already requested two-window session with the new build. [Launch, production and validation guide](companion-play.en.md).

## 2026-10-03 — On-demand human + automatic companion 0.9.7

[Current launch, behavior and validation guide](companion-play.en.md). Open one human host and one automatic companion client only when the user says **“직접 테스트 해볼게”** (I will test it myself). Default to safe practice in the current map: repeat idle, walk/sidestep, slow movement, jump, baton and nearby parcel carry/drop on a 32-second cycle; follow the human when distant. Game 0.9.7, protocol 13, TCP 27842. Mac build with zero errors/9 warnings and 5 grouped checks in two actual windowless processes passed. Do not open manual windows this task; inspect focus/mouse and human visual quality after the request. Complex maze/multilevel routes, Windows/performance and rerunning the full 110-check regression are outside this validation scope.

## 2026-10-03 — Right-hand baton 0.9.6

See the [current employee/baton guide](employee-animation.en.md) and [tools/Blender MCP setup](macos-development.en.md). Attach the baton to the actual right hand and apply a finger grip. Hide it on every peer while carrying a parcel/beacon, restore it after dropping/placing, and retain down/rescue hiding. Version 0.9.6, protocol 13, TCP 27842, Unity 6000.6.4f1. [Actual validation scope](../validation/baton-hand-0.9.6.json). Carrying contact, dedicated full-body attacks, first-person arms, user quality/all-frame penetration, other PCs/Windows/performance remain incomplete.

## 2026-10-03 — Employee full body and idle/walk 0.9.5

[Current integration, local reproduction and validation scope](employee-animation.en.md). Import corrected Idle and Walking through separate Humanoid Avatars, connect the full employee body and switch motions from actual movement speed. Each window hides its own body and shows three teammates with team tints. Game 0.9.5, protocol 13, TCP 27842. Keep source motions/generated Unity assets local; publish reproduction code, validation records and static images. Supersede earlier Unity/idle-walk-not-integrated statements within this scope. Carrying hand contact, dedicated additional motions, first-person arms and user quality/other-environment review remain pending.

## 2026-10-02 — Cinder suppression and outer creature 0.9.4

[Current rules, production and running](cinder-suppression.en.md). Connect existing 90/135/180-second stages, signal audio and relative work-light dimming to 4 current suppressors and the boundary. Connect east entry, obstacle routing and pursuit for the outer creature 8 seconds after shutdown. Preserve sky/fog/sun, source art and collision. Default launch is delivery + Listener + suppression/outer; retain `--delivery-only`/`--map-only` regression. Version 0.9.4, protocol 13, TCP 27842. Pass 60 actual four-process checks for purchased-beacon distraction, hazardous return, shared down/ship safety, emergency recovery and next shift. [Validation record](../validation/cinder-suppression-0.9.4.json). Supersede older unconnected-suppression/outer statements below within this scope. Human quality, final creatures/audio, clues/progression saving and other-environment/performance checks remain pending.

## 2026-10-02 — Cinder Listener, baton and rescue 0.9.3

[Current rules, running and production checks](cinder-listener.en.md). Connect one Listener's actual floor/obstacle grid movement, noise investigation, warning/attack, existing empty-hand left-click baton, hold-E rescue and all-down ship recovery to default Cinder delivery. Preserve normal 420 CR payment and source art scene. Judge safety using current Cinder ship coordinates; do not instantiate old suppression/outer/clue/save objects. Existing beacon pulses also attract Listener investigation. Keep peaceful delivery regression via `--delivery-only` and movement tests via `--map-only`. Version 0.9.3, protocol 12, TCP 27842. Supersede older unconnected-Cinder-Listener/baton/rescue statements below only within this scope. Suppression/outer creature, clues, progression saving and human quality remain pending. [Actual validation scope](../validation/cinder-listener-0.9.3.json).

## 2026-10-01 — Controls, ship and purchase UI 0.9.2

[Current controls and running](controls-ui.en.md). E targeted use/ship terminal, left-click ground placement, hold right click to rotate parcel, wheel 0.75–1.6m reach, Q immediate release. Separate departure/return/purchase into native buttons showing boarding count, wallet, disabled reasons and zero-pay return confirmation. Esc settings provide 13 button rebindings, sensitivity, FOV, language/defaults. Menus block gameplay inputs while the world continues. Connect Cinder 120 CR beacon purchase/shared carrying/two 8-second signals. Version 0.9.2, protocol 11, TCP 27842. Preserve source map/delivery pay. Supersede E automatic progression/Esc shop and unconnected-Cinder-beacon statements below within this scope. Listener/baton/suppression/save integration and human quality assessment remain. [Actual validation scope](../validation/controls-ui-0.9.2.json).

## 2026-10-01 — Cinder delivery, receipt and return settlement

**CINDER-DELIVERY-01 / see evidence below for implementation and automated scope.** Connect the delivery ledger and CRT in separate `CinderFourPlayerTest`. Preserve source `CinderCompactSiteReview` art/buildings/sky/physics. Rebuilding copies the source and adds 1 existing receipt model, 1 BoxCollider, feedback and 2 physical labels. Do not turn the original 46 static freight/3 CRTs into gameplay cargo. No new models/textures/packages. Retain version **0.9.1**, protocol **10**, TCP **27842**, render cap **30fps**, physics **50Hz** and camera **250m**. [Build source](../../NoReturns/Assets/_NoReturns/Editor/CinderFourPlayerBuild.cs) · [ledger](../../NoReturns/Assets/_NoReturns/Runtime/CarryLab/CarryMission.cs).

1. Run [07_Play_Cinder_4P.command](../../07_Play_Cinder_4P.command) to start 1 host and 3 clients in delivery preparation. Walk up the ship ramp and press E to select the route. After everyone physically boards, press E again to arrive. No flight presentation.
2. E carry the sealed parcel outside to BAY 04. Floor center **(17,0,12.4)m**, outline **3×2m**. Acceptance requires an unheld parcel center strictly within **x=15.95~18.05, z=11.8~13, y=0.2~0.65m**, speed below **0.2m/s**, stable for **0.75 seconds**. Q sets it down. Held parcels cannot be accepted.
3. After **0.75 seconds** printing, look at terminal center **(14.9,0.8,12.4)m** within **2.4m** and press E to collect. Any crew member can collect this shared state; it occupies no hand slot. Delivery/collection do not pay immediately.
4. Everyone returns aboard, then E returns the ship. Standard receipt **300 + return 120 = 420 CR** pays once; the HUD shows the breakdown. Returning without collection/mid-shift disconnect pays 0 new credits and retains the secured balance. E prepares the next shift, resetting parcel/employee spawn and receipt state. This mode does not restore balance after exit.

Boarding bounds are **|x+20.7|<1.5, -30.9<z<-24.8, 0.8<y<3m**. The physical deck is 1m high; E on the ramp/outside does not advance departure/return. Keep the receipt device outside the central movement axis, retaining BAY 04 south/north traversal. The source art scene retains its solo review. For Editor delivery testing, open derived `CinderFourPlayerTest`, Play and choose HOST/JOIN.

```sh
source ~/.unity/env
python3 tools/cinder_four_player.py build
python3 tools/cinder_four_player.py start
python3 tools/cinder_four_player.py stop
python3 tools/test_cinder_delivery.py
# Retained movement/carrying, fifth-client rejection and reconnect regression
python3 tools/cinder_four_player.py check
python3 tools/cinder_four_player.py start --map-only
```

Manual launch takes actual keyboard/mouse without `--test-dir`. Four LAN humans use the same executable, 1 HOST/3 JOIN, host address and TCP 27842. Four windows on one PC accept keyboard/mouse only in the selected window. [08_Stop_Cinder_4P.command](../../08_Stop_Cinder_4P.command) stops only this test's processes.

The CRT applies only the feedback root's translation `_ScreenOffset` to the measured glass mask. Support the current terminal at unchanged size/orientation; resizing/rotation requires new glass/paper measurements. Old experiments retain offset 0. [Presentation source](../../NoReturns/Assets/_NoReturns/Runtime/CarryLab/ReceiptFeedback.cs) · [shader](../../NoReturns/Assets/_NoReturns/Resources/ReceiptUI/ReceiptCRT.shader).

[Automated/preservation evidence](../validation/cinder-delivery-01.json). `DeliveryRules.Run` checks **35** legacy/Cinder ledger conditions; `ReceiptSurfaceCheck.Run` checks **6** front/rear/wall conditions at both coordinates. At 480×320, the legacy front changes **1,597 pixels** and the Cinder front **1,596 pixels**, rear/wall views **0 pixels**. The real four-process delivery check covers physical boarding by all four, one employee carrying through the west/north detour and back, shared receipt/empty return/all-aboard/one payment/next-shift reset. Distinguish old source-scene checks from this derived-scene validation. Human controls/fun, other-PC/WAN, Windows execution, performance and Listener/baton/suppression/beacon/progression saving remain unverified.

**Verified:** Mac build **0 errors/7 existing warnings**, **23** real four-process delivery checks, **35** legacy/Cinder ledger conditions, **6** CRT surface/occlusion conditions, **132** BAY 04 four-body lane positions and **7** existing rescue rules pass. Inspect KO/EN receipt, paper ejection/collection and 420 CR report captures. Retained movement/carrying/rejection/reconnect regression also passes **13** checks on the final build.

![BAY 04 terminal](../../art/cinder-kit-01/delivery-terminal-ko.png)

![Ship settlement](../../art/cinder-kit-01/delivery-ship-report.png)

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

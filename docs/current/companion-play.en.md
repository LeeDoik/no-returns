# Hands-on player + automatic companion — 0.9.12

[한국어](companion-play.ko.md)

## 2026-10-03 — Manual test ended; baton/jump improvements 0.9.12

On the user’s “테스트 끝” request, use the existing stop helper to clean up both manual windows and session record for `run-20261003-222202-345af2`. Running-session statements below are historical. Build 0.9.12 with upright readiness, a short forward pulse and jump/landing, using windowless checks only. Retain two-player left/right 16:9 and four-player 2×2 placement. Open no new manual windows or Editor Play. The current 6-group companion check observes rising/apex/falling/landing/grounded on both peers, with about 0.644800m jump-height range. Revised human-perceived naturalness remains unverified. [Baton](employee-baton.en.md), [jump/landing](employee-locomotion.en.md), [validation record](../validation/employee-pulse-locomotion-0.9.12.json).

## 2026-10-03 — Landscape 16:9 test-window layout 0.9.11

The current two-window session is `run-20261003-222202-345af2`; previous 0.9.10 startup records describe terminated historical sessions. Both actual game viewports are 1702×958px (about 851×479pt); outer windows are 851×511pt. The title bar is outside the 16:9 game area. Pixel rounding tolerance is at most 1px. Visually inspected the human window and measured both AppKit positions/sizes.

Implement equal 16:9 left/right viewports for two players and 16:9 viewports inside a 2×2 grid for four players. Address user feedback about the small companion window and vertically stretched viewports. Mac build passed with zero errors/7 warnings; 18 checks covered two/four-player cells, 16:9, overlap and boundaries, and actual two-window sizes/AppKit positions and host/companion connectivity were checked. Actual four-window placement, Windows/Intel Mac, external monitors/display-scale changes and user handling remain unverified. [validation record](../validation/client-window-layout-0.9.11.json).

## 2026-10-03 — Automatic test-window tiling 0.9.11

Two-player practice places the human host on the left and the automatic companion on the right at equal widths in landscape 16:9 viewports. Four-player tests place slots 0/1/2/3 in top-left/top-right/bottom-left/bottom-right quarters, preserving 16:9 inside each tile. Calculate sizes from the display work area with title-bar and menu-bar clearance. Allow manual resizing. Apply placement only at startup, never to windowless automated checks. Open manual windows only on request; this task relaunches the already requested two-window session with the new build. [Layout code](../../NoReturns/Assets/_NoReturns/Runtime/CarryLab/CinderWindowLayout.cs), [launcher](../../tools/cinder_four_player.py), [settings](../../NoReturns/ProjectSettings/ProjectSettings.asset).

Use Unity work-area and resolution APIs. On macOS, Unity reports (0,0) for the inactive companion window, so [place the process’s AppKit window directly](https://developer.apple.com/documentation/appkit/nswindow/setframetopleftpoint(_:)). Read the Retina [display scale](https://developer.apple.com/documentation/appkit/nsscreen/backingscalefactor) to convert pixels and points. Windows uses the [Unity window movement API](https://docs.unity3d.com/6000.6/Documentation/ScriptReference/Screen.MoveMainWindowTo.html). Reserve 38pt for the menu bar, 32pt for the title bar and an 8px gap between clients. Retain normal input, protocol 13 and TCP 27842. Apply four-player tiling when a separate four-player test is requested; this task does not open four manual windows.

## 2026-10-03 — Requested hands-on straight-thrust play started at 0.9.10

On the user's “직접 테스트 해볼게” request, ran the existing `python3 tools/cinder_four_player.py start --companion` to open one human host and one automatic companion window. Confirmed app version 0.9.10, host listening, client slot 1, protocol 13, occupied mask 3, companion readiness and safe practice mode. Leave session `run-20261003-221113-a8137a` running for inspection of new baton thrust/retraction and hiding during carrying. No code changes, rebuild or new quality approval. Feedback on revised hit-feel/appearance and actual mouse/focus remains pending. Checked document links, language counterparts and whitespace.

## 2026-10-03 — Straight baton thrust 0.9.10

The current 0.9.10 companion uses the fast straight thrust during its baton phase. Prior manual sessions are stopped; this revision opens no manual windows. Open the current build only on “직접 테스트 해볼게”. Start/stop and validation records for 0.9.9 and earlier below are historical. [Current behavior and validation scope](employee-baton.en.md).

## 2026-10-03 — Hands-on play stopped and session cleaned at 0.9.9

On the user's “플레이 테스트 끝” request, ran the existing `python3 tools/cinder_four_player.py stop`. Confirmed both game processes in session `run-20261003-215537-82e2bd` stopped, its session file was removed and no process listened on TCP 27842. The start record below is historical; this test is now stopped. No code changes, new build or game behavior revalidation. Completion of play is not visual quality/hit-feel approval; specific user quality feedback remains pending. Checked document links, language counterparts and whitespace for this change.

## 2026-10-03 — Requested hands-on play started at 0.9.9

On the user's “직접 테스트 해볼게” request, ran the existing `python3 tools/cinder_four_player.py start --companion` to open one human host and one automatic companion window. Confirmed app version 0.9.9, host listening, client slot 1, protocol 13, occupied mask 3 and companion readiness. Leave session `run-20261003-215537-82e2bd` running in safe practice for hands-on inspection of new baton readiness/arm-chest swing/recovery and two-hand carrying. No code changes or rebuild. User quality feedback on actual mouse/focus, hit-feel and appearance remains pending. Check document links, language counterparts and whitespace only; previous game checks do not constitute quality approval of this session.

## 2026-10-03 — Baton readiness, attack and recovery 0.9.9

The current 0.9.9 companion uses new arm/chest attack posing during the baton part of its existing 32s cycle. Open the two manual windows only on request. The 0.9.7/0.9.8 results below are historical. [Implementation, production and validation scope](employee-baton.en.md).

You may close both game windows manually. Saying “테스트 끝” is an optional way to ask Codex to stop them for you. Clean remaining session metadata with the existing stop helper.

## 2026-10-03 — Start requested hands-on play 0.9.8

On the user’s “직접 테스트 해볼게” request, run the existing `python3 tools/cinder_four_player.py start --companion` to open one human host and one automatic companion window. Confirm player version 0.9.8, host listening, client slot 1 assignment and companion readiness. Leave session `run-20261003-202722-0ef980` running for hands-on play. No code changes. User feedback on actual mouse/focus, appearance and handling is still pending; do not record quality approval.

## 2026-10-03 — Two-handed parcel carrying 0.9.8

Current game 0.9.8 retains the 32-second companion cycle and two-window on-demand policy. The bot also uses 0.8m reach and the two-hand carrying pose. Pass 5 grouped two-process windowless regressions this task. The 0.9.7 build results below are historical. [Implementation, evidence and limits](employee-animation.en.md).

2026-10-03. Run [09_Play_Companion.command](../../09_Play_Companion.command) only when the user says **“직접 테스트 해볼게”** (I will test it myself). Do not open manual game windows after every task. This task validates two windowless processes and does not open manual windows.

## Launch and controls

- Open one human host window on the left and one automatic companion client on the right at equal widths in landscape 16:9. Select the human window labelled `YOU + BOT` and use existing keyboard/mouse controls. The bot neither reads keyboard/mouse input nor locks the cursor. Existing control settings and Esc menus remain available.
- Start safe observation mode in the current Cinder map. Delivery progression, departure, purchases, creatures, suppression timing and progression saves are inactive. Parcel handling and baton presentation/6-second cooldown remain testable. Actual delivery/combat validation uses the separate existing four-player mode.
- Repeat an approximately **32-second** cycle: idle (0–3s), walk/sidestep (3–11s), slow movement (11–16s), jump (16–19s), baton (19–22s), approach/carry a nearby parcel (22–30s), put down (30–32s). Reuse the employee's current idle/walk clips; this does not create dedicated jump/attack animation assets.
- Prioritize following when more than **6m** from the human. Usually target roughly **2.7m** in front of the human. Only attempt an unheld parcel within **7m** of the human. Never forcibly take a parcel held by the user.
- Check ground support and short collision sweeps to steer around nearby obstacles. Do not perform full maze/multilevel pathfinding; move into nearby open space if it gets stuck at a dead end. Do not teleport a distant bot.
- Stop using the [existing stop shortcut](../../08_Stop_Cinder_4P.command) or the stop command below. Refuse to start while another test session is active; do not arbitrarily close existing windows.

```sh
python3 tools/cinder_four_player.py start --companion
python3 tools/cinder_four_player.py stop
# Preparation after code changes: build and automated checks without opening game windows
python3 tools/cinder_four_player.py build
python3 tools/test_cinder_companion.py
```

The [launcher](../../tools/cinder_four_player.py) starts exactly two players. The host takes real input; the client uses [companion input code](../../NoReturns/Assets/_NoReturns/Runtime/CarryLab/CarryRoom.Companion.cs). Movement/jump/pickup/drop requests pass through existing networking and host adjudication. Practice baton actions test presentation without damage. If bot readiness is not confirmed, clean up both processes and report failure. Rebuild first when the player is outdated.

Enable practice options only under `CARRY_TEST_AUTOMATION` or Unity Editor. Ordinary launches retain existing behavior. The initial implementation record uses game **0.9.7**, protocol **13**, TCP **27842**, sourced from [version settings](../../NoReturns/ProjectSettings/ProjectSettings.asset). Existing [local employee asset prerequisites](employee-animation.en.md) still apply.

## Actual validation scope

[Validation record](../validation/companion-play-0.9.7.json). Unity **6000.6.4f1** Mac build: **0 errors**, **9 warnings**. Two actual host/client processes using `-batchmode -nographics` pass 5 grouped checks: separate roles/safe mode/7 actions; actual walking/jump height (approximately **0.645m**); slower movement/baton cooldown/parcel pickup/drop/baton hiding and restoration on both peers; following a moved human; two logs without runtime errors or placeholder fallback. Close both processes and remove the session marker after validation.

The first build fails because the compilation cache retains earlier Pipeline file paths. Package reimport and compilation-cache refresh alone do not resolve it. Verify no unsaved scene, restart the Editor, then pass compilation/rebuild. Confirm CLI **1.0.0-beta.12**, Pipeline **0.8.0-exp.1** and Editor **6000.6.4f1** match current official releases.

- [x] Build, two-process windowless checks, Python syntax and bilingual document links/checkbox states.
- [ ] After the user's request, inspect actual window size/focus/mouse behavior and bot appearance with the user. No game windows or Editor Play were opened in this task.
- [ ] Complex dead ends/multilevel routes, other PCs/LAN/Windows and performance. Do not rerun the previous full 110-check regression in this task.

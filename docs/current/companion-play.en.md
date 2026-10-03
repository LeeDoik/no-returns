# Hands-on player + automatic companion — 0.9.8

[한국어](companion-play.ko.md)

You may close both game windows manually. Saying “테스트 끝” is an optional way to ask Codex to stop them for you. Clean remaining session metadata with the existing stop helper.

## 2026-10-03 — Start requested hands-on play 0.9.8

On the user’s “직접 테스트 해볼게” request, run the existing `python3 tools/cinder_four_player.py start --companion` to open one human host and one automatic companion window. Confirm player version 0.9.8, host listening, client slot 1 assignment and companion readiness. Leave session `run-20261003-202722-0ef980` running for hands-on play. No code changes. User feedback on actual mouse/focus, appearance and handling is still pending; do not record quality approval.

## 2026-10-03 — Two-handed parcel carrying 0.9.8

Current game 0.9.8 retains the 32-second companion cycle and two-window on-demand policy. The bot also uses 0.8m reach and the two-hand carrying pose. Pass 5 grouped two-process windowless regressions this task. The 0.9.7 build results below are historical. [Implementation, evidence and limits](employee-animation.en.md).

2026-10-03. Run [09_Play_Companion.command](../../09_Play_Companion.command) only when the user says **“직접 테스트 해볼게”** (I will test it myself). Do not open manual game windows after every task. This task validates two windowless processes and does not open manual windows.

## Launch and controls

- Open one human host window (1280×800) and one automatic companion client (800×500). Select the human window labelled `YOU + BOT` and use existing keyboard/mouse controls. The bot neither reads keyboard/mouse input nor locks the cursor. Existing control settings and Esc menus remain available.
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

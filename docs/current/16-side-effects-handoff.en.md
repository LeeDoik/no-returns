# SIDE EFFECTS — Lead Development Agent Handoff

[한국어](16-side-effects-handoff.ko.md)

> **Historical record · retired/superseded 2026-09-12.** The [PSX space-delivery design](01-overview.en.md) is current. Implementation, launch, art and values below are historical, not current status. Deleted files remain path records only.


> Archived record · Discontinued 2026-09-12. Not a current launch guide or implementation specification. Links to removed files point to the archive containing their originals. [Unity 메인 / Mainline](18-unity-mainline.en.md).

2026-09-11 · SE-PROT-01 · Revision 2 · First prototype implemented / human validation pending

This is a work request for the user to give the lead development agent. Current implementation and evidence are in the [implementation record](17-side-effects-implementation.en.md). When the user requests implementation under this brief, proceed within the first-prototype scope below. Numeric settings are experimental, not final release specifications.

## Work request to deliver

Implement the first cooperative prototype of SIDE EFFECTS. Validate **turning a healing spell that accidentally launches a teammate into a deliberate traversal or rescue technique** through actual play. Prioritize reliable controls, understandable drawbacks and re-entry after mishaps over random rewards or content volume.

The source is [art, Core Fun and Core Loop](14-side-effects.en.md). Use the executing-plans skill to execute the stages sequentially and update their checkboxes. This handoff does not replace a detailed code-level implementation plan. Inspect current code, specify the files, interfaces and test commands needed for the first stage, then implement. Do not stop at submitting a plan; deliver a runnable result within the authorized scope.

## Current state and starting point

- First read `AGENTS.md`, the [documentation home](../README.en.md), the concept, [Unity environment](14-unity-environment.en.md) and [Unity controls specification](15-unity-controls.en.md).
- The observed foundation is `unity/NoReturns`, Unity 6000.6.0f1, URP 17.6.0 and Input System 1.19.0. Recheck `ProjectSettings/ProjectVersion.txt` and `Packages/manifest.json`. This handoff recommends using the existing Unity project.
- `Assets/_Project/Scenes/CarryLab.unity` is a solo movement/carrying lab. No networking library was present at handoff; this separate scene now uses official Unity Transport 6.6.0.
- `Assets/_Project/Runtime/WorkerController.cs` depends on CarryMotor and directly reads input. Reference its movement/camera patterns rather than attaching it unchanged to the new player. Preserve external displacement velocity separately from input velocity; input must not erase recoil every frame.
- Inspect modified files and ownership of other work first. Preserve the Godot game, Development, CarryLab, existing executables and saves. Do not revert or commit all existing uncommitted changes.

## First deliverable and exclusions

Deliver a separate SideEffectsLab scene and Windows executable. Include 2–4-player connectivity, 1 practice area, 1 combat room, 1 traversal room, 3 tools, 2 enemy types, 1 crosswind modifier, downing, dragging, revival, wiping and restarting. The 20–30-minute expedition is the full-game direction; do not pad this short lab to that duration.

Complete combat by defeating enemies, and traversal by activating the far switch and reaching the exit. Every mandatory objective needs a detour accessible through basic movement. Limit room rewards to keeping the current tool or choosing an offered replacement. A temporary results screen shows rescues/shared actions and restart.

Bosses, permanent progression/saves, safe-room reward economy, large procedural generation, matchmaking, Steam, built-in voice, viewer voting, automatic video editing and production art at scale are follow-up scope. Do not mark exclusions implemented.

## Structure and technical principles

Paths below are proposed additions relative to `unity/NoReturns`. If names collide, preserve the responsibilities and document path changes.

| Path | Responsibility |
|---|---|
| `Assets/_Project/SideEffects/Scenes/SideEffectsLab.unity` | Separate playable scene |
| `Assets/_Project/SideEffects/Runtime/Player/` | Input, movement/external velocity, health/down/revival |
| `Assets/_Project/SideEffects/Runtime/Tools/` | Definitions, use requests, targeting, healing/pushing/pulling |
| `Assets/_Project/SideEffects/Runtime/Network/` | Connectivity, authority, replication, duplicate suppression |
| `Assets/_Project/SideEffects/Runtime/Rooms/` | Objectives, enemies, transitions, seed and modifiers |
| `Assets/_Project/SideEffects/Runtime/UI/` | HUD, previews, tool choices and results |
| `Assets/_Project/SideEffects/Data/` | Tunable tool/combat/movement settings |
| `Assets/_Project/SideEffects/Tests/` | Rule, scene and networking checks |
| `artifacts/side-effects/` | Logs and footage/screenshots from both clients |

Separate input → action request → authoritative simulation → resulting state → presentation. Requests carry sender player, tool ID, sequence number and aim direction. Server/host validates ownership, cooldown, downed state, distance and occlusion, then resolves actual targets. Apply health, displacement and rewards once. Do not trust client-submitted hit results directly.

Recommend host authority initially. Check existing compatible packages first; if absent, consult current official Unity networking documentation and Editor compatibility before recording the selection rationale and pinned version. Do not automatically purchase services or add accounts. Establish the first connection through a direct address or local session. Host departure ends the session with an explanation; host migration is excluded.

Give CharacterController an external-velocity path integrated with collision movement. Apply physical forces to metallic Rigidbody props. A fully ragdolled player is unnecessary. Separate input responsiveness from authoritative correction and measure feel and positional error under latency.

## Stages and acceptance conditions

- [x] **A. Independent scene and controls:** Create movement, jump, dodge, basic attack and camera in the new scene. Initially bind WASD, mouse, Space, Shift, left-click attack, right-click tool and E interaction. Check walls, slopes and edges, and that releasing the cursor prevents attacks. Pass CarryLab regression checks.
- [x] **B. Smallest cooperative proof:** Connect 2 actual processes and implement the healing staff's heal/push plus down/revival. Test host→client and client→host use, duplicate and out-of-range requests. Health/down outcomes must agree and impulses must not duplicate. Local dummy characters do not establish multiplayer completion.
- [x] **C. Tools and combat:** Add magnetic shield, recoil hammer, metallic melee knight and a ranged enemy with telegraphed attacks. Magnet attracts metal only and never through walls. Recoil survives movement input without wall penetration. Check healing preview against actual range. Repeated-friendly-effect protection blocks displacement without accidentally suppressing valid healing.
- [x] **D. Short loop, randomness and recovery:** Connect practice→combat→traversal→results→restart. Host chooses seed, room configuration and reward candidates. Compare the same space with/without crosswinds and allow progress when retaining or replacing tools. Falls reach a rescue ledge or the last safe position while downed. Check wipes, client departure and restart for stale health, effects and object ownership.
- [ ] **E. Readable art/HUD and scale verification:** Use placeholder large gloves, capes and hat silhouettes with navy/teal environments, mint healing rings and amber warning patterns. Do not rely on color alone. HUD shows objective, teammate down state, health, tool benefit/drawback and cooldown. Check 4 processes and a Windows build, then observe whether human players progress from mishaps to deliberate exploitation.

Each stage proceeds through a rule/integration check reproducing failure → minimal implementation → passing that check → direct scene inspection → documentation/evidence update. Avoid tests that merely copy implementation. If networking is blocked, record the cause and useful local progress without advancing the multiplayer completion claim.

## Tuning and validation scenarios

Expose movement speed, tool force/range/cooldown, external-velocity damping, repeated-effect protection, revival duration and restored health in settings assets. The developer chooses minimum experimental values and records rationale and units. Build a space where healing displacement enables a shortcut while normal jumping permits a detour, then tune force. Do not freeze the carrying speed as final action movement speed.

Required checks: intended push still occurs at full health; protection permits healing but blocks repeated displacement; downed players cannot attack but can call for help; leaving revival range cancels revival; walls block healing/magnetism; transitions/restarts clear residual forces; fixed seeds reproduce room/reward choices. Do not claim a fixed seed guarantees identical physical trajectories.

Compare normal networking with round-trip latency of 100ms and packet loss of 1%. These are experiment conditions, not release thresholds. Record final health/state mismatches, duplicated effects and correction magnitude per condition. Distinguish separate-PC testing from multiple local processes.

In human play, observe changed plans on the second attempt, explanations of accident causes and whether rescue becomes repetitive labor. Automated checks cannot prove fun. Without human participation, deliver a playtest build and procedure and report fun validation incomplete.

## Completion report format

Include completed/incomplete stages; executable/scene and exact launch instructions; actual controls; tuning values and source files; automated results; 2–4-player process count/network conditions/separate-PC status; separate screen inspection from human play results; log/footage paths; known issues; and one next task.

Existing regression command from repository root: `05_Unity.cmd test`. Documentation check: `python tools/check_docs.py`. Implement separate entry points for new-scene testing/builds and report their exact commands. Do not assume the existing build includes the new scene. Do not close the user's Editor without saving to run batch tasks.

Update affected current specifications, guides, backlog, validation lists and `docs/archive/change-log.ko.md` and `.en.md` in the same task. Human documents require full Korean/English counterparts, reciprocal links and matching numbers/check states. Distinguish proposals, implementation, automated verification and observed fun.

## Execution status — 2026-09-11

A–D automated checks and E Windows build, 4-process runs and rendered samples have been performed. E as a whole stays incomplete because no humans participated. Automated passage does not replace control-feel, fun or separate-PC validation. [Results and launch guide](17-side-effects-implementation.en.md).

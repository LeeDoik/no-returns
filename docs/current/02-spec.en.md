# Current game specification — SPACE-01

0.9.1: [Cooperative HUD and evidence](crew-hud.en.md). Reorganized gameplay instructions and four-player states. Human readability assessment remains outstanding.

## Current 0.9.0 — four-player cooperation

[Current rules, launch and validation](four-player.en.md). Supports 1 host and up to 3 clients joining during preparation. This supersedes historical two-player limits and unimplemented four-player statements below. Existing E/Q, baton, delivery and receipt collection rules remain. Steam registration is deferred at user request; other-PC, internet and human four-player fun validation remain outstanding.


0.8.14: [Display implementation policy and current audit](display-systems.en.md).

0.8.13: [Integrated display / 내장 화면](next-equipment.en.md).

0.8.12: [Continuous arc; integrated-display concept pending](next-equipment.en.md).

0.8.11: [LMB + charge display](next-equipment.en.md).

0.8.10: [G 충격봉 / G baton](next-equipment.en.md).

0.8.9: [Game integration — 0.8.9](psx-beacon-01.en.md).

0.8.8: [E/Q controls and carried beacon](controls-beacon.en.md) supersedes previous keys and remote deployment.

0.8.7: replaced floating text with UI textures on the actual CRT surface. [Receipt terminal and occlusion checks](receipt-terminal.en.md).

0.8.6: original CRT/slot alignment, duplicate recorder removed, payment requires receipt collection and return. [Current receipt rules](receipt-terminal.en.md) supersede immediate-payment descriptions.

0.8.5: replaced the reception bench with floor markings and connected terminal reactions. See [Receipt terminal](receipt-terminal.en.md) for current rules and verification status; this supersedes raised-bench descriptions.

[한국어](02-spec.ko.md)

0.8.4: two approved parcel/receipt models produced and visually integrated. Dynamic terminal state remains follow-up work. [Record](psx-props-01.en.md).


0.8.3: six selected facilities integrated and Windows build completed. MCP bounds/collider checks passed for six modules; actual east-corridor and north-detour screens inspected. Existing wall/rack colliders and carrying rules remain. Full floor finish, lighting, pattern repetition, frame performance and human feel remain open. [Record](psx-tripo-kit-01.en.md).


Current 0.8.2: fixed downed-host departure recovery, low-step routing/foot height, simultaneous F transitions, terminal collision, ship attack protection, the west rack slit and join-refusal feedback. Unreproduced target oscillation and the standing-host departure-abort policy remain design reviews. [Findings and evidence](review-fixes.en.md).

Added in 0.8.1: after entry, the outer creature automatically pursues crew across the entire map. Downed/aboard employees are excluded; pursuit resumes after beacon distraction. Walls require detours but do not block detection. [Rules/validation](space-play-07.en.md).

Added in 0.8.0: expanded the map to 32×44m with an east corridor, north detour and west storage wing. Four suppression stages and delayed outer-creature entry are implemented. Values, geometry and creature visuals remain experimental, not final art. [SPACE-PLAY-07](space-play-07.en.md).

Added in 0.7.0: I clue inspection and the Tab shared field log. Records reset on next arrival and are not restored after exit; wallet/license saving remains. [Clue specification/validation](space-play-06.en.md).

As of 0.6.0, host progression saves restore wallet, beacon license and successful-delivery count; shifts restart at ship preparation. Earlier session-only/no-save notes describe the state through 0.5.0. [Save rules and validation](space-play-05.en.md).

2026-09-13 · Project foundation 0.1.0 / Play experiment 0.8.1

## Actual implementation

[NoReturns](../../NoReturns) is a newly created project. It uses Unity 6000.6.0f1, the 3D URP template, URP 17.6.0, Input System 1.20.0 and Pipeline MCP 0.7.0-exp.1. Evidence: [editor version](../../NoReturns/ProjectSettings/ProjectVersion.txt) and [packages](../../NoReturns/Packages/manifest.json).

[ProjectBootstrap](../../NoReturns/Assets/_NoReturns/Editor/ProjectBootstrap.cs) configures product name NO RETURNS, version 0.1.0, a default 1280×720 window and an empty Bootstrap scene. Folders are Scenes, Runtime, Editor, Art, Audio, Data, Prefabs and Tests. The starting scene contains only the default camera and light. The separate CarryRoom experiment below implements gameplay; the production PSX renderer remains absent.

## Approved rules and implementation boundary

The [product design](01-overview.en.md) establishes automatic travel/landing after route selection, delivery through dangerous locations, and reinvested pay for equipment and harder contracts. Carrying and two-player direct connection are implemented in the experiment below. Actual spacecraft, multiple destinations and the complete mystery remain absent. A separate experiment implements 2 clues and the shared field log. Creature, beacon and risk-selection experiments are described below. Delivery mode implements a placeholder route and a subset of pay. Previous games' reward tests and physics values are not evidence for this new game.

## Removed implementation

Deleted the previous Godot source/cache, temporary Unity project, CarryLab/SIDE EFFECTS/NR-LOOP-01, former art, builds, launchers and game-specific tools. Downloads and system-installed applications such as Unity or Blender were not removed. Git history remains, but this does not mean all deleted uncommitted/untracked files were stored in Git. The [deletion manifest](../archive/space-reset-deletion-manifest.json) records paths and sizes; it is not a file backup.

[Next work](04-backlog.en.md) · [Validation](05-validation.en.md)

## SPACE-ECO-01 — Implementation boundary

[Pay, failure and equipment economy draft](economy.en.md) and shop/settlement images are design artifacts. Only intact-cargo receipt and full-return payment are implemented in delivery-mode session memory. The hazard experiment adds session beacon purchase/effects and emergency recovery. Local host progression saving is implemented; mid-shift resume and cargo damage remain absent. Proposal arithmetic checks are not gameplay tests.

## First-person implementation boundary

First person is now the user-approved default camera. Eye-level camera and carrying are implemented in the experiment below. Hands and equipment animations remain absent.

## SPACE-PLAY-01 — Executable carrying experiment

[Carrying guide and current values](carry-test.en.md), [plan](space-play-01.en.md). Created a separate CarryRoom scene and Windows 0.3.0 executable. Unity MCP connection, compilation, build and 17 automated checks across two real processes passed. Correcting the earlier invisible terms-dialog diagnosis: isolated-account execution and Korean assembly-path errors were resolved. Human control feel and a complete delivery loop remain unverified.

## SPACE-PLAY-02

[View-relative carrying and delivery-mode specification/validation](space-play-02.en.md). Added vertical-look carrying, route selection, receipt, full-crew return and session pay. Actual map, persistence and the full shop remain. The hazard experiment is separated in SPACE-PLAY-03 below. User feedback: the previous carrying build runs, but cargo does not follow vertical view.

## Guidance language — 0.3.1

Added Korean by default, the 한국어 / English menu toggle and persistence of the local preference. [Usage and production guide](carry-test.en.md). Language selection is separate from online game state. Results are recorded in the [language checks](../../artifacts/language/latest.json).

## SPACE-PLAY-03 — LISTENER/rescue experiment (0.4.0)

[Listener specification, launch and validation](space-play-03.en.md). A separate hazard mode implements sound investigation, attack warning, shove, down, hold-R revival and all-down emergency recovery. Carrying/delivery modes remain. Version 0.8.0 adds expanded routes and suppression experiments; host progression saving is also supported. Next work includes final CINDER DEPOT spaces/art, deeper equipment reinvestment and human fear/cooperation evaluation. Final models, carrying employees, death, 4 players and Steam connectivity are not complete.


## SPACE-PLAY-04 — Supply/risk contracts (0.5.0)

[Purchase/use specification](space-play-04.en.md). Listener mode now implements session beacon-license purchase, shared deployment and risk-contract selection. Persistence, the full shop, more destinations and final art remain. Use existing launchers 06/07. Two-process progression checks passed 24, existing rescue checks 21, and language checks 9. Human feel, economy balance and rendered readability of the new supply panel are unverified.

0.8.15: [Baton shell restoration](baton-mesh-fix.en.md).

0.8.16: The black rectangle in the startup menu was the local character visor. Update returned early for inactive sessions, skipping body visibility. Moved visibility into LateUpdate so it runs before rendering in both menus and gameplay. A temporary Unity scene regression recorded hidden=False before and True after recompilation. Windows build verification is separate from human startup-screen confirmation, which remains pending.


2026-09-17: [CINDER-BLOCKOUT-01 — Unity primitive layout trial](cinder-blockout.en.md). A 108×86.4m validation scale, 5 enterable buildings, 3 loops and reused ship interior trial 05. Separate single-player spatial experiment; enemy pursuit, networking and delivery settlement are not connected. 41 physical passage checks and Editor rendering inspected; Windows build succeeded. Human controls, cargo clearance and cooperative enjoyment remain unverified.

# Current game specification — SPACE-01

[한국어](02-spec.ko.md)

## 2026-10-01 — Open sky and zone boundaries

The user found the map too maze-like, so open **82.8m²** of west/north perimeter-route cover: west 3×14.4m and north 13.2×3m. Split the original 2 ceiling BoxColliders into 4 matching the retained covered pieces, removing invisible ceilings in the openings. Preserve 3/3.15m ground passages, 4 loops, buildings/ship/props and lighting. No game-version change.

Distinguish paving within the 53.55×65.4m field from rough mineral ground outside, adding flat rust-colored boundary bands and 2 physical labels. English source strings are `FIELD / INTERIOR` and `OUTER / BASIN`. Add **4 service pads measuring 2.4×2.4m** around the corner suppressors, retaining approach space. Boundaries/pads are visual markings and add no ground colliders or suppression gameplay.

Review also found regular rock rows and blue suppressor blocks. Apply fixed seed 137 to irregular positions/yaw and distant peak/ridge selection for the existing 59 rocks. Hide only the original 4 suppressor Renderers, adding aged masts, cabinets and small green signal visuals inside their original 1×5×1m collision envelopes. Signals reuse the existing Unlit material and are not new Lights. Produce 4 native meshes (mast/signal/open-cover yard surfaces/boundary and pads), reusing existing meshes/materials/label tooling. Retain 69 background placements/4,624 triangles; add 8 suppressor placements/864 triangles and 3 boundary/label placements/48 triangles. Preserve original ground collision, 47 prop groups, 26 architecture placements, 40 local lights, sky, 35–115m fog and runtime.

[Current checks and production](cinder-compact-site.en.md#2026-10-01--open-sky-and-zone-boundaries). This modifies the visual environment; suppression/exterior creatures/delivery/Listener/baton/network integration remain incomplete. User quality and human four-player/performance review remain.

## 2026-10-01 — Current Cinder exterior background

Apply [current background, dimensions and checks](cinder-compact-site.en.md#2026-10-01--rocky-territory-and-industrial-background-outside-the-field). 59 rocks, 9 industrial visuals and 1 exterior surface total 69 placements and 4,624 triangles. Produce 8 native meshes, 2 materials and 1 native 128×128 mineral texture; reuse the industrial stack. Leave the 53.55×65.4m field footprint empty, retaining layout, collision, sky, 35–115m fog, 40 local lights and runtime. Hide only Renderers of 4 gray guards, retaining existing fall-prevention Colliders. 0 new background Colliders/Lights. The exterior is static scenery; new traversal areas, outer creatures and suppression/delivery behavior remain unimplemented. User background quality, human four-player play and performance checks remain. Distinguish the user's confirmation of preceding buildings from overall quality approval.

## 2026-10-01 — Varied Cinder building structure

[Current building forms/measurements](cinder-compact-site.en.md#2026-10-01--varied-building-outlines-and-structural-modules) are the environment baseline. Apply 9 structural module types and 11 native meshes; replace 2 actual ground outlines/colliders of the central utility/northern annex with 8-sided/stepped shapes. Place warehouse sawtooth roofs, storage vault roofs, office L-shaped upper room, octagonal control room, plant rooms, stacks and entry canopies. Retain the 5 main-building interiors, ship, 3/3.15m alleys, 4 loops, 47 prop groups, 40 lights, sky and runtime. Upper rooms are static visuals; new floor access/interaction and Cinder gameplay integration remain incomplete. User appearance-quality review is pending.

## 2026-10-01 — Current whole-site Cinder layout

[Current whole map, props and checks](cinder-compact-site.en.md) defines the environment. The user approved the whole-site direction. Retain 5 buildings, the ship, 6 auxiliary volumes, 3/3.15m alleys and 4 loops; add 47 prop groups to the same scene. The 46 freight visuals and 3 CRT visuals are static props without receipt/E/Q functionality. Exclude the 24 interior-only partitions from the current target. CarryRoom delivery, Listener, baton, suppression mechanics and networking below remain unconnected to Cinder. Prop quality, human four-player passing, receipt facilities and new-coordinate integration remain.

The same scene now uses [dusk sky, lighting and distant haze](cinder-compact-site.en.md#2026-10-01--sky-and-distant-atmosphere). Apply a static cubemap and 35–115m fog; preserve 40 local lights, placement, collision and runtime. Day/night/weather/suppression-stage integration and user sky-quality review remain incomplete.

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

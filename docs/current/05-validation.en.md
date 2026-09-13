# Testing and release checklist

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

[한국어](05-validation.ko.md)

0.8.4: two approved parcel/receipt models produced and visually integrated. Dynamic terminal state remains follow-up work. [Record](psx-props-01.en.md).


0.8.3: six selected facilities integrated and Windows build completed. MCP bounds/collider checks passed for six modules; actual east-corridor and north-detour screens inspected. Existing wall/rack colliders and carrying rules remain. Full floor finish, lighting, pattern repetition, frame performance and human feel remain open. [Record](psx-tripo-kit-01.en.md).


Current 0.8.2: fixed downed-host departure recovery, low-step routing/foot height, simultaneous F transitions, terminal collision, ship attack protection, the west rack slit and join-refusal feedback. Unreproduced target oscillation and the standing-host departure-abort policy remain design reviews. [Findings and evidence](review-fixes.en.md).

Added in 0.8.1: after entry, the outer creature automatically pursues crew across the entire map. Downed/aboard employees are excluded; pursuit resumes after beacon distraction. Walls require detours but do not block detection. [Rules/validation](space-play-07.en.md).

Added in 0.8.0: expanded the map to 32×44m with an east corridor, north detour and west storage wing. Four suppression stages and delayed outer-creature entry are implemented. Values, geometry and creature visuals remain experimental, not final art. [SPACE-PLAY-07](space-play-07.en.md).

Added in 0.7.0: I clue inspection and the Tab shared field log. Records reset on next arrival and are not restored after exit; wallet/license saving remains. [Clue specification/validation](space-play-06.en.md).

As of 0.6.0, host progression saves restore wallet, beacon license and successful-delivery count; shifts restart at ship preparation. Earlier session-only/no-save notes describe the state through 0.5.0. [Save rules and validation](space-play-05.en.md).

2026-09-12 · SPACE-01

## Foundation checks

- [x] Fresh Unity project created; not copied from the previous project.
- [x] Removed previous Godot/temporary Unity games and launchers; retained Git/document history.
- [x] Verify foundation settings, empty scene, compilation and fresh-project MCP connection.
- [x] Validate current links, bilingual numbers and checkbox parity.

Record evidence only for the new project. Do not inherit old prototype test passes. Store fresh logs in `artifacts/space-foundation/`.

## After gameplay implementation

- [ ] Solo delivery, return, settlement and recovery from failure.
- [ ] Ownership, payment, rescue, restart and departure agreement across actual processes.
- [ ] Cooperation across separate PCs and latency/loss conditions.
- [ ] 4-player roles, readability, waiting time and performance.
- [ ] Perceived equipment benefit and reasons to choose harder work.
- [ ] Fair naturally occurring accidents and creature cues.
- [ ] Voluntary questions and desire to investigate during human mystery playtests.
- [ ] PSX art consistency, UI clarity, motion discomfort and audio.
- [ ] Saves, settings, accessibility, Steam integration, store, rights, distribution and built-player validation.

There is currently no releasable game player. Compiling an empty project does not validate control feel, fun or release readiness.

## Evidence from this task

- [Foundation setup log](../../artifacts/space-foundation/setup.log): exit code 0, `SPACE-01 FOUNDATION PASS`.
- Official MCP `editor_status`: fresh `NoReturns` path, Unity 6000.6.0f1, `ready`, no compilation or domain reload in progress.
- Documentation check: local links, language counterparts and checkbox parity passed across 174 Markdown documents. Numeric parity passed for current documents 01–05.
- [Build scene settings](../../NoReturns/ProjectSettings/EditorBuildSettings.asset): only the empty `Bootstrap.unity` is enabled. No human playtest or game-player validation was performed.

## SPACE-ECO-01 — Proposal calculation checks

- [x] [Calculation check](../../tools/check_economy_proposal.py): passed 126 delivered and 126 undelivered combinations, 5 examples, image wallet flow and bilingual numeric/checkbox parity. [Result](../../artifacts/space-economy/check.json).
- [ ] Actual human balance checks and online payment/save/duplicate-processing checks in [Pay, failure and equipment economy draft](economy.en.md).

Arithmetic passing does not validate economic balance or Unity implementation.

## Pending first-person validation

- [ ] Understand forward/floor/corner visibility and set-down placement while carrying.
- [ ] Weapon switching, effects, hands, wall clipping and online teammate presentation consistency.
- [ ] Human evaluation of FOV, camera shake, discomfort and HUD readability.

Concept image review does not replace these game checks.

## SPACE-PLAY-01 verification boundary

[Carrying guide](carry-test.en.md) and [automated results](../../artifacts/space-play-01/latest.json). Unity MCP, compilation, Windows build and 17 checks across two actual processes passed. URP camera rendering was visually inspected, distinct from complete menu/HUD screen verification. Human controls/fun, separate PCs, 4 players, WAN and Steam remain unverified. Earlier failure evidence is preserved.

## SPACE-PLAY-02

[View-relative carrying and delivery-mode specification/validation](space-play-02.en.md). Added vertical-look carrying, route selection, receipt, full-crew return and session pay. Actual map, persistence and the full shop remain. The hazard experiment is separated in SPACE-PLAY-03 below. User feedback: the previous carrying build runs, but cargo does not follow vertical view.

## Guidance language — 0.3.1

Added Korean by default, the 한국어 / English menu toggle and persistence of the local preference. [Usage and production guide](carry-test.en.md). Language selection is separate from online game state. Results are recorded in the [language checks](../../artifacts/language/latest.json).

Language validation: 9 automated checks passed across two Windows processes (Korean default, Korean glyph, independent toggles and restart persistence). Window capture did not reliably capture the game, so visual validation of wrapping/button readability remains incomplete.

## SPACE-PLAY-03 — LISTENER/rescue experiment (0.4.0)

[Listener specification, launch and validation](space-play-03.en.md). A separate hazard mode implements sound investigation, attack warning, shove, down, hold-R revival and all-down emergency recovery. Carrying/delivery modes remain. Next work includes the actual CINDER DEPOT map and alternate routes, suppression, deeper equipment reinvestment and human fear/cooperation evaluation. Final models, carrying employees, death, persistence, 4 players and Steam connectivity are not complete.


## SPACE-PLAY-04 — Supply/risk contracts (0.5.0)

[Purchase/use specification](space-play-04.en.md). Listener mode now implements session beacon-license purchase, shared deployment and risk-contract selection. Persistence, the full shop, more destinations and final art remain. Use existing launchers 06/07. Two-process progression checks passed 24, existing rescue checks 21, and language checks 9. Human feel, economy balance and rendered readability of the new supply panel are unverified.

[Save/restart validation](space-play-05.en.md): passed 35 executable checks, 14 file rules, 17 economy rules and 21 rescue regressions. Human UI/feel and power loss are unverified.

[Clue/log validation](space-play-06.en.md): passed 23 two-process checks, 11 Unity rules and 21 existing rescue checks. Visually inspected the Korean journal in a visible Windows player. Human curiosity, fear and final-art approval are unverified.

## SPACE-PLAY-07

- [x] Expanded routes, suppression stages, outer creature, emergency recovery and reset in two real processes: 41 checks passed.
- [x] Same-build regressions: 17 carrying, 21 rescue and 6 delivery checks passed.
- [x] Inspected east corridor/north detour rendering in actual 960×600 Windows captures.
- [ ] Human route choices, cue readability, fear and cooperative fun.
- [ ] Investigate editor-exit JobTempAlloc warning; player leak status is unconfirmed.

[Build 0.8.0 and evidence](space-play-07.en.md). This limited experiment does not complete the broader release checklist.

## 0.8.1 global pursuit

- [x] Passed 12 Unity rules checks and 23 actual two-player checks.
- [ ] Human difficulty/cooperative response evaluation.

[Evidence](space-play-07.en.md).


## 0.8.2 independent review regressions

- [x] 11 Unity rules/physics checks: simultaneous F, terminal collision, both creatures reaching the low step, ship attack exclusion and rack slit.
- [x] 17 checks with 2 actual Windows players: downed-host departure recovery, next shift/rejoin, Korean refusal feedback, synchronized down on the step and slit exclusion.
- [x] 12 Unity global-hunt checks and 23 actual-player checks passed.
- [x] 17 Unity payment, authority and reset checks passed.
- [ ] Human feel, fear/fun, target oscillation across walls and deliberate departure-abuse evaluation.

[Review decisions and fixed run evidence](review-fixes.en.md). Automated passes are not release approval or human quality judgment.


## SPACE-ART-25 — Tripo PSX kit


2026-09-13 Unity startup recovery: after restarting the licensing helper and clearing stalled startup instances, verified editor ready, compiling=false and the actual Windows editor screen. Individual PSX import quality, play and build validation remain incomplete. [Recovery evidence](psx-tripo-kit-01.en.md).


The final 0.8.3 build passed 17 automated carrying regressions using two Windows host/client processes, including ownership contention, view tracking, drop, disconnect release and wall collision. Human feel/fun and Internet latency were not tested. Documentation checks passed for 204 entries.


Final 0.8.4 Windows build succeeded. Two real Windows processes passed 14 automated checks including delivery, receipt, single reward payment, return, wallet retention and disconnect settlement. Blender front/rear and Unity prop rendering were reviewed. After the final textured-rear repair, build and delivery tests were rerun; older runtime captures show the earlier rear, so rear-review.png is the final rear appearance evidence. Human feel/fun and performance measurement were not performed. Documentation checks passed for 206 entries.

[Steam private testing registration and checklist](steam-testing.en.md) — 2026-09-13

0.8.15: [Baton shell restoration](baton-mesh-fix.en.md).

0.8.16: The black rectangle in the startup menu was the local character visor. Update returned early for inactive sessions, skipping body visibility. Moved visibility into LateUpdate so it runs before rendering in both menus and gameplay. A temporary Unity scene regression recorded hidden=False before and True after recompilation. Windows build verification is separate from human startup-screen confirmation, which remains pending.

2026-09-13: [Complete demo cycle and 44 art production units](demo-art-list.en.md). Production proposal for the user goal, not approval of new appearances/timing or completed production.

2026-09-13 review: [Exterior/interior consistency S01~S08](ship-review.en.md). Retain appearance direction; production-structure validation remains incomplete. Dimensional sums do not prove assemblability.

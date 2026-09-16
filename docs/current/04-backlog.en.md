# Open decisions and production order

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

[한국어](04-backlog.ko.md)

0.8.4: two approved parcel/receipt models produced and visually integrated. Dynamic terminal state remains follow-up work. [Record](psx-props-01.en.md).


0.8.3: six selected facilities integrated and Windows build completed. MCP bounds/collider checks passed for six modules; actual east-corridor and north-detour screens inspected. Existing wall/rack colliders and carrying rules remain. Full floor finish, lighting, pattern repetition, frame performance and human feel remain open. [Record](psx-tripo-kit-01.en.md).


Current 0.8.2: fixed downed-host departure recovery, low-step routing/foot height, simultaneous F transitions, terminal collision, ship attack protection, the west rack slit and join-refusal feedback. Unreproduced target oscillation and the standing-host departure-abort policy remain design reviews. [Findings and evidence](review-fixes.en.md).

Added in 0.8.1: after entry, the outer creature automatically pursues crew across the entire map. Downed/aboard employees are excluded; pursuit resumes after beacon distraction. Walls require detours but do not block detection. [Rules/validation](space-play-07.en.md).

Added in 0.8.0: expanded the map to 32×44m with an east corridor, north detour and west storage wing. Four suppression stages and delayed outer-creature entry are implemented. Values, geometry and creature visuals remain experimental, not final art. [SPACE-PLAY-07](space-play-07.en.md).

Added in 0.7.0: I clue inspection and the Tab shared field log. Records reset on next arrival and are not restored after exit; wallet/license saving remains. [Clue specification/validation](space-play-06.en.md).

As of 0.6.0, host progression saves restore wallet, beacon license and successful-delivery count; shifts restart at ship preparation. Earlier session-only/no-save notes describe the state through 0.5.0. [Save rules and validation](space-play-05.en.md).

2026-09-12 · SPACE-01

## Decisions to resolve first

- [ ] Review 7: reproduce whether employees moving on opposite sides of a wall cause outer-creature target oscillation; if reproduced, compare target retention or path-distance selection. Currently unreproduced, with no behavior change.
- [ ] Review 9: decide the abuse risk and alternatives for the existing rule that partner departure aborts the shift and freezes creatures for a standing host. Distinguish this from the downed-host recovery defect. [Review record](review-fixes.en.md).

- [ ] Failure losses and recovery for workers, cargo, equipment and pay.
- [ ] Emphasis on evasion/distraction and permitted direct combat.
- [ ] First delivery site's spatial character and cargo-driven carrying choices.
- [ ] Equipment purchasing/upgrading, consumables, persistence and cooperative save ownership.
- [ ] Recurring mystery motif and connected initial clues; retain unknown identity/ending.
- [ ] Shift length, release content quantity and economic values.

Manual flight is excluded. Automatic travel and landing after route selection are confirmed. Do not reuse old prototype contract counts, currency values or art approvals.

## Proposed implementation order

1. Movement, camera, physical carrying and 2-player input/ownership foundations.
2. Connect spacecraft preparation → route selection → automatic site arrival → receipt → return/settlement.
3. Connect a creature with readable observation/counters and cargo recovery/worker rescue.
4. Connect reinvestment and harder contract choices.
5. Validate mystery clues and observable site changes.
6. Expand into approved art production and 4-player, saving and release features.

Before each stage, write bilingual detailed rules and pass criteria. After implementation, distinguish automated and human validation. Creating this foundation does not complete these gameplay items.

[Design](01-overview.en.md) · [Validation](05-validation.en.md)

## SPACE-ECO-01 — Work remaining after drafting

[Pay, failure and equipment economy draft](economy.en.md) develops the failure, purchasing and persistence decisions above as proposals. Keep checkboxes incomplete until actual play/online validation. Priorities are earnings per time from deliberate wipes, rescue incentives, solo burden, optional-supply choices and economy after finite unlocks.

## First-person follow-up

- [ ] Validate eye-level camera, hands, equipment, carrying visibility and wall clipping.
- [ ] Review FOV, head bob, discomfort options and teammate full-body animation.

Default camera and carrying experiment are implemented. Hands, equipment, teammate animation, human evaluation and new image appearance approval remain pending.

## SPACE-PLAY-01 progress

- [x] Author movement, carrying, transport, build and automated input tools.
- [x] Reconnect MCP and compile/build under the user account through the junction.
- [x] Check ownership, rotation, walls and departure in two players; fix failures.
- [ ] Human control-feel evaluation.

[Plan](space-play-01.en.md) · [Guide](carry-test.en.md). Source authoring does not complete feature validation.

## SPACE-PLAY-02

[View-relative carrying and delivery-mode specification/validation](space-play-02.en.md). Added vertical-look carrying, route selection, receipt, full-crew return and session pay. Actual map, persistence and the full shop remain. The hazard experiment is separated in SPACE-PLAY-03 below. User feedback: the previous carrying build runs, but cargo does not follow vertical view.

## Guidance language — 0.3.1

Added Korean by default, the 한국어 / English menu toggle and persistence of the local preference. [Usage and production guide](carry-test.en.md). Language selection is separate from online game state. Results are recorded in the [language checks](../../artifacts/language/latest.json).

## SPACE-PLAY-03 — LISTENER/rescue experiment (0.4.0)

[Listener specification, launch and validation](space-play-03.en.md). A separate hazard mode implements sound investigation, attack warning, shove, down, hold-R revival and all-down emergency recovery. Carrying/delivery modes remain. Next work includes the actual CINDER DEPOT map and alternate routes, suppression, deeper equipment reinvestment and human fear/cooperation evaluation. Final models, carrying employees, death, persistence, 4 players and Steam connectivity are not complete.


## SPACE-PLAY-04 — Supply/risk contracts (0.5.0)

[Purchase/use specification](space-play-04.en.md). Listener mode now implements session beacon-license purchase, shared deployment and risk-contract selection. Persistence, the full shop, more destinations and final art remain. Use existing launchers 06/07. Two-process progression checks passed 24, existing rescue checks 21, and language checks 9. Human feel, economy balance and rendered readability of the new supply panel are unverified.

## SPACE-PLAY-07 — expanded map follow-up

- [ ] Human play review of orientation, detour choice and return pressure in new wings.
- [ ] Tune the 32×44m test map against the 90/135/180-second transitions; these are not release values.
- [ ] Verify people notice the east-gate silhouette, device signals and darkness through actual audio and visuals.
- [ ] Apply approved PSX looks and final outer-creature model; current primitives are placeholders.
- [ ] Validate expanded movement areas with 4 players, other PCs and latency.

[Current implementation and evidence](space-play-07.en.md).

## Evaluation after global pursuit

- [ ] Human review of pursuit pressure and room to escape to the ship.
- [ ] Review cues and cooperative use of beacon distraction followed by reacquisition.


## Recommended order after 0.8.2 — proposal

The next objective is to refine the first CINDER DEPOT delivery to demo quality. Implemented features and automated passes do not replace human cooperative-fun validation.

1. Have 2 actual people play the current build. Attempt it without additional explanation first, then exchange carrying/luring roles for comparison. Record where they get lost, recognition of suppression stages, reasons to use the beacon, causes of down, ability to return and willingness to retry.
2. Adjust routes, creature pressure and cue communication from observed problems. Review target oscillation and intentional departure alongside existing open items. Do not finalize reward, timing or speed values for release.
3. Apply approved PSX art, lighting and sound to routes whose readability is established. Retain image approval before model production. Validate the style in a small section from the ship exit to the first danger area.
4. Once that delivery flow is stable, expand to 4-player play, other PCs/latency, Steam connectivity and distribution preparation. Validate cooperative 4-player issues before producing large amounts of content.

Initial acceptance question: can the team choose routes and roles, recognize danger cues, explain failure and try a different response next time? This is a proposed human evaluation criterion, not a current pass. This prioritization investigation changes no code, build or assets.


## SPACE-ART-25 — Tripo PSX kit



## Next art step — decoy beacon

2026-09-13: user confirmed the 0.8.7 CRT fix and requested the next step. The next candidate is 03 / DECOY BEACON from the existing equipment sheet. Re-presented the [existing concept](../art/space-concepts/equipment-01.png), awaiting appearance approval. Proposed reference: ivory metal housing, yellow speaker, dark red handle and amber warning light. After approval, produce, inspect and integrate only this beacon. Employee and other equipment are not treated as approved. This task reviews the concept and records sequencing; no code, build or 3D changes.

2026-09-13 follow-up: user approved 3D production of equipment-sheet beacon 03. The earlier pending approval is resolved. Controls and physical carrying were implemented first at user request; model production remains incomplete.
## 2026-09-13 — Next asset: shock baton

The user checked the 0.8.9 beacon and requested continuation. The next candidate is 01 / SHOCK BATON from the existing equipment-01 sheet. Preserve the ivory industrial body, yellow electrodes and dark red grip. After appearance approval, the proposal is Tripo production, Blender inspection, Unity first-person placement and a strike motion. Specify how it relates to the existing shove and its carrying/use controls before implementation. New damage, killing and pricing are not decided. This task reviews a candidate and updates documentation; no code, model or build changes. Keep 0.8.9. Baton appearance approval is pending.

[Details](next-equipment.en.md).

## 2026-09-13 — Steam cooperative service requirements

User requirements: convenient joining through Steam friends/invitations, saved session information, and protection against arbitrary game-state changes through client tampering. Requirements are confirmed; engine migration, server adoption, spending and implementation are not decided.

Recommended architecture: Steam authentication/lobbies/invitations + operator-controlled authoritative game servers + server-side persistent storage. Player PCs, including the party leader, request actions; the server validates movement, carrying, attacks, deliveries and rewards. Only trusted servers write session outcomes to storage. Lobbies organize parties and joining; they do not replace simulation servers. A permanently running server per saved session is unnecessary; consider saving empty sessions and releasing their server resources. This is an unimplemented proposal.

Alternatives: player hosting with local/Steam Cloud saves reduces operating burden but cannot place host tampering outside the trust boundary. Player hosting with server storage alone is also insufficient if fabricated host results are trusted. Steam Cloud synchronizes files rather than verifying session outcomes. Client modification itself cannot be completely prevented; server validation limits acceptance of manipulated requests into game state. This does not solve every other cheat, such as aim automation.

Existing [host saves](space-play-05.en.md) provide local progression persistence, not mid-shift resumption or tamper prevention. The new requirements do not mark existing functionality complete. Save scope (exact mid-shift restoration versus ship/end-of-shift checkpoints), session ownership/access, resumption without the party leader and operating budget remain undecided. Both Unity and Unreal can support this architecture; engine choice remains separate.

Future validation: invitations/joining across different Steam accounts and networks, reconnection and save recovery, duplicate reward prevention, rejection of invalid movement/carry/reward requests, and unauthorized session access rejection. This task only reviewed documentation and official sources.

Sources: [Steam lobbies](https://partner.steamgames.com/doc/features/multiplayer/matchmaking), [authentication](https://partner.steamgames.com/doc/features/auth), [Cloud](https://partner.steamgames.com/doc/features/cloud), [anti-cheat](https://partner.steamgames.com/doc/features/anticheat).

### 2026-09-13 — Save scope confirmed and recommendation

The user confirmed that resuming money, equipment and progression from return to the ship is sufficient. Exact restoration of mid-shift cargo positions or combat state is not required. This decision supersedes the undecided save granularity above. Checkpoint failure/retry and disconnect handling before return require follow-up specification.

Recommend retaining Unity with Steam invitations/lobbies, operator-authoritative game servers and server checkpoint persistence. The requirements justify replacing the direct TCP prototype's networking, authority and persistence structure for service use rather than an engine port. This judgment considers potential reuse of existing game logic and the scope of a full engine migration; it is not a comparative performance result. It does not recommend shipping the current networking unchanged. Engine retention and server adoption remain recommendations, not user-confirmed decisions or completed implementation. Cost must be estimated from concurrent active sessions, regions and measured server resources; no quote was produced.

Minimum validation candidate: invitation joining across different Steam accounts/networks, delivery and ship-return checkpoint save, resume after everyone exits, duplicate payout rejection, and invalid movement/reward request rejection. Session ownership and access without the party leader remain undecided. No code changes or actual server deployment.

### 2026-09-13 — Prerelease Steam invitation testing prerequisites

Friends invitations/joining can be tested before release with Steamworks integration and accounts granted test access. Requirements include an owned AppID and Steamworks configuration, compatible test builds and app access for each account; consider Release State Override keys for a small external test. A lobby invitation does not grant a game license. Valve's official SpaceWar sample AppID 480 is an initial API proof-of-concept candidate, not a substitute for testing distribution and entitlements under the game's own app. This project's Steamworks registration, owned AppID and test-key availability remain unverified. The preceding invitation-development recommendation did not establish readiness to connect.

[Prerelease testing](https://partner.steamgames.com/doc/store/testing) · [Official SpaceWar sample](https://partner.steamgames.com/doc/sdk/api/example). Official documentation review only; no Steam configuration, code or distribution changes and no actual invitation test.

[Steam private testing registration and checklist](steam-testing.en.md) — 2026-09-13

0.8.15: [Baton shell restoration](baton-mesh-fix.en.md).

0.8.16: The black rectangle in the startup menu was the local character visor. Update returned early for inactive sessions, skipping body visibility. Moved visibility into LateUpdate so it runs before rendering in both menus and gameplay. A temporary Unity scene regression recorded hidden=False before and True after recompilation. Windows build verification is separate from human startup-screen confirmation, which remains pending.

2026-09-13: [Complete demo cycle and 44 art production units](demo-art-list.en.md). Production proposal for the user goal, not approval of new appearances/timing or completed production.

2026-09-13 review: [Exterior/interior consistency S01~S08](ship-review.en.md). Retain appearance direction; production-structure validation remains incomplete. Dimensional sums do not prove assemblability.


2026-09-14: [FLATBED integrated 3D model and review status](ship-production.en.md). Integrated-model changes supersede earlier interior dimensional proposals. Unity integration and 4-player control validation remain incomplete.

2026-09-14: [FLATBED interior production preparation](ship-interior-pipeline.en.md). Resolve flat-floor/window/headroom consistency and cargo passage below the rear engine, then validate a representative section before full assembly. Modeling and Unity validation remain incomplete.

2026-09-15 latest: [Structure trial 05](ship-interior-trial.en.md). Interior-first orbital post office: central inspection, left sealed lockers, right dispatch desk and rear folded seats. Devices are structural mockups; human spatial review and exterior art remain pending.


2026-09-16: [HTML layout study: 18 spaces, loops and emergency exit](map-study.en.md). Unity integration and human feel testing remain pending. Ship exterior production is deferred.


2026-09-16 / MAP-STUDY-03: Added west loading yard, east service yard, southern outdoor route and inside-only exit to HTML. Connectivity, delivery cycle and outdoor return automation passed; browser visuals inspected. Unity, online and human fun testing not performed. [MAP-STUDY-03](map-study.en.md).


2026-09-16: [MAP-STUDY-04](map-study.en.md) — Exterior expansion 04: enlarged the proposed overall extent to 120×96m. Preserved interior coordinates and widened the west antenna area, east service yard, south freight yard and fuel equipment area. Exterior obstacles allow movement around multiple sides. Antenna/fuel areas are currently labels and collision obstacles, with no new interactions. Automated travel to 5 additional exterior destinations and existing delivery/exit/return checks passed. Browser rendering confirmed with zero console errors. First-person feel, danger balance and Unity integration remain unverified.


2026-09-16 [MAP-STUDY-05](map-study.en.md): Added the northern exterior maintenance area to complete a four-sided perimeter loop. Proposed extent: 120×112m. Preserve separation between interior and exterior, connected only through the existing west entrance and east emergency door. Full exterior circuit and existing delivery/door automated checks passed; browser rendering and zero console errors confirmed. Unity integration and human feel remain unverified.


2026-09-16: [EXIT-RELOCATION](map-study.en.md). Moved the emergency exit to the right wall of receiving at the user-marked location. Closed the former cooling exit. Inside-only E unlock remains. Automated delivery/receipt/return, new gate unlock, old exit blockage and perimeter circuit checks passed. No Unity changes; human feel unverified.

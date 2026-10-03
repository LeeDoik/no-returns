# Open decisions and production order

[한국어](04-backlog.ko.md)

## 2026-10-04 — First-person rescue hands 0.9.16

Reach with both hands and show a small assisting movement during valid rescue. Hide the baton, restore the right-hand baton on cancellation/completion and hide hands when down. Retain the 25° wrist limit. [Current production, values and remaining scope](first-person-arms.en.md), [validation record](../validation/employee-rescue-0.9.16.json). Game **0.9.16**, protocol **13**, TCP **27842**. Preserve existing rescue adjudication. This supersedes older statements below that rescue hands are missing or hidden, within this scope. Beacon hands, full-body rescue/down, actual body contact and human quality review remain pending. Open manual test windows only on request.

## 2026-10-04 — First-person wrist correction 0.9.15

Fix the wrist bend reported by the user in the 0.9.14 attack preview. Correct the relationship between the palm and cylindrical grip axes; move the elbow outward so the forearm and hand align. Limit the hand/forearm direction angle to 25° and solve grip contact again. Keep the baton upright during forward movement and retract the shoulder origin near walls. [Current pose/production](first-person-arms.en.md), [validation record](../validation/employee-wrist-0.9.15.json). Game **0.9.15**, protocol **13**, TCP **27842**. The previous pose has a user-confirmed quality issue; the revised pose has not received user approval. Manual play stays stopped.

## 2026-10-04 — Dynamic baton and first-person hands/arms 0.9.14

At the user's request, implement short anticipation, fast extension and 0.42s recovery through baton arm/chest motion, local right glove/sleeve and two-hand parcel carrying. Preserve immediate contact, 6s cooldown, carrying/down/valid-rescue blocking and existing movement. [Current baton](employee-baton.en.md), [arm production/local reproduction](first-person-arms.en.md), [validation evidence](../validation/employee-first-person-0.9.14.json). Game **0.9.14**, protocol **13**, TCP **27842**. Earlier 0.9.13 original-motion restoration and first-person-arms-not-implemented/proposed states below are historical and superseded within this implementation scope. Dedicated beacon/rescue hands, human quality, actual game GPU composition, all-corner penetration, Windows/LAN and performance remain pending. Manual play stays stopped; use the existing two-window helper only on request.

## 2026-10-03 — Original baton restored; backward/sidestep movement 0.9.13

At the user's request, restore the initial right-hand attachment version (0.9.6) of baton readiness/use. Remove the arm/chest attack correction introduced in 0.9.9; retain hand attachment, the weapon's 0.5s return, immediate contact, 6s cooldown and carrying/down/rescue hiding. Redirect backward, lateral and diagonal foot trajectories from actual body-relative movement. Reuse alternating foot timing/heights from the forward Walk while preserving torso facing, root, physics and network rules. Retain jump/landing. No new Mixamo FBX or Blender editing. [Baton](employee-baton.en.md), [movement](employee-locomotion.en.md). Revised human quality, dedicated clips, first-person arms and Windows/LAN/performance remain unverified.

## 2026-10-03 — Landscape 16:9 test-window layout 0.9.11

Implement equal 16:9 left/right viewports for two players and 16:9 viewports inside a 2×2 grid for four players. Address user feedback about the small companion window and vertically stretched viewports. Mac build passed with zero errors/7 warnings; 18 checks covered two/four-player cells, 16:9, overlap and boundaries, and actual two-window sizes/AppKit positions and host/companion connectivity were checked. Actual four-window placement, Windows/Intel Mac, external monitors/display-scale changes and user handling remain unverified. [Current guide](companion-play.en.md), [validation record](../validation/client-window-layout-0.9.11.json).



## 2026-10-03 — Historical character proposal (current status is at 0.9.14 above)

At that time, inspect code and local employee assets and propose baton attack motion → jump/landing plus backward/sideways locomotion → first-person arms. At 0.9.8, attacks changed only the baton object rotation/position and had no dedicated full-body clip. The first task would add a brief preparation/swing/recovery moving the arm, shoulder and weapon together, aligned with existing contact/cooldown and retaining carry/down/rescue blocking. Extra locomotion would complement the reused forward Walk. First-person arms are a separate production task completing local baton/carrying presentation. This order is a proposal, not implementation authorization, new-motion validation or play-quality approval. Specific quality feedback from the recent hands-on session has not yet been received.

## 2026-10-03 — Two-handed parcel carrying 0.9.8

Implement and validate near-parcel two-hand posing and release/down clearing. Distant/extreme-view/rotating contact gaps, finger penetration, first-person occlusion and human quality remain. Dedicated beacon/attack/jump/rescue motions and first-person arms remain incomplete. [Implementation, evidence and limits](employee-animation.en.md).

## 2026-10-03 — On-demand human + automatic companion 0.9.7

[Current launch, behavior and validation guide](companion-play.en.md). Open one human host and one automatic companion client only when the user says **“직접 테스트 해볼게”** (I will test it myself). Default to safe practice in the current map: repeat idle, walk/sidestep, slow movement, jump, baton and nearby parcel carry/drop on a 32-second cycle; follow the human when distant. Game 0.9.7, protocol 13, TCP 27842. Mac build with zero errors/9 warnings and 5 grouped checks in two actual windowless processes passed. Do not open manual windows this task; inspect focus/mouse and human visual quality after the request. Complex maze/multilevel routes, Windows/performance and rerunning the full 110-check regression are outside this validation scope.

## 2026-10-03 — Right-hand baton 0.9.6

See the [current employee/baton guide](employee-animation.en.md) and [tools/Blender MCP setup](macos-development.en.md). Attach the baton to the actual right hand and apply a finger grip. Hide it on every peer while carrying a parcel/beacon, restore it after dropping/placing, and retain down/rescue hiding. Version 0.9.6, protocol 13, TCP 27842, Unity 6000.6.4f1. [Actual validation scope](../validation/baton-hand-0.9.6.json). Carrying contact, dedicated full-body attacks, first-person arms, user quality/all-frame penetration, other PCs/Windows/performance remain incomplete.

- [x] Unity 6000.6.4f1 Mac build: zero errors, 14 warnings. Four actual processes pass 69 baton/suppression/delivery + 10 visual/transition + 31 Listener/hit/down/rescue checks = **110**. Review 4 idle/walk hand/body samples and a game capture from the new build.
- [x] Actual MCP stdio tool calls read Blender's 5 objects, 53 bones and right-hand information. Preserve unsaved scene/existing user edits. Apply current official Unity/CLI/Pipeline/uv/Codex CLI releases and confirm Blender 5.2.2 LTS matches the latest release.
- [ ] Codex desktop update/restart and default MCP tool exposure, the new Unity's Windows support/execution, human quality/all-frame penetration and performance. Observe a Burst error resolving a Pipeline DLL during upgrade and 5 Burst entry-point warnings in the final build. Build/110 game checks pass, but warning cause/performance impact remains unverified. Remaining warnings concern mesh collision pre-baking, absent Pipeline runtime config, obsolete search APIs and stripped debug shaders.

## 2026-10-02 — Player asset preparation

[Production recommendation](demo-art-list.en.md#2026-10-02--proposed-player-model-and-animation-workflow). Research the player appearance/animation pipeline at the user's request. These are new pending production items; 0.9.4 behavior is unchanged.

- [x] On 2026-10-03, generate [employee concept 02](03-guides.en.md#2026-10-03--employee-image-concept-02) with the built-in image tool and visually inspect front/side/back views.
- [x] On 2026-10-03, generate [3 separate front/side/back PNGs](03-guides.en.md#2026-10-03--separate-employee-view-images), checking full-body framing and removed text. The front is a single-character T pose.
- [x] On 2026-10-03, preserve FBX/JPG originals, complete [Blender inspection and a 1.8m working copy](03-guides.en.md#2026-10-03--employee-blender-inspection-and-working-copy), inspect 6 renders and verify reopening. Measure 4,888 faces/9,118 triangles, a 4K texture and no skeleton.
- [x] [Repair 2 boot exceptions and build a 53-bone candidate](03-guides.en.md#2026-10-03--employee-boot-repair-and-first-deformation-rig), adjust pad rims/weights, test both grips, shoulders, 90-degree elbows/knees, carry-ready pose and FBX reimport.
- [x] [Correct supplied Mixamo Idle posture](03-guides.en.md#2026-10-03--correct-the-supplied-idle-upper-body-posture): source preservation, 251-frame checks, Blender/FBX reopening and static render review. Walking received/skeleton-inspected only.
- [x] [Unity employee full body and Idle/Walk integration](employee-animation.en.md): separate Humanoid Avatar retargeting, speed-driven transitions, four team tints and four actual process checks.
- [ ] User approval of corrected Idle and public source-redistribution terms for motions.
- [ ] User appearance approval, cross-view consistency and extreme-bend pad/cloth polish. Do not blanket-weld 442 boundary edges/21 components.
- [ ] Validate actual parcel contact. Decide presentation for cargo controls beyond arm reach.
- [ ] Integrate first-person arms and dedicated baton/down/rescue/jump motions with existing adjudication; review human visual quality. Four actual process checks for full body/four tints/Idle·Walk are complete. Finalize exact clip count and completion only after rig validation.

## 2026-10-02 — Current development status and next order

Prioritize current 0.9.4 [suppression/outer rules](cinder-suppression.en.md). Connect suppression decay/outer creature while preserving delivery, Listener, baton, rescue, current sky and source environment. Record actual execution in the [validation record](../validation/cinder-suppression-0.9.4.json).

- [ ] Play a delivery for about 5 minutes, checking parcel controls, departure/return UI, warnings and rescue for friction. Distinguish automated four-process checks from human assessment.
- [x] Connect existing stages/audio/relative dimming to 4 suppressors, the boundary and 40 work lights while preserving sky/fog/sun.
- [x] Connect east advance cues, 8-second entry grace, current floor/obstacle routing and ship protection for the outer creature. 90/135/180 seconds remain validation values, not final release difficulty.
- [x] Pass 60 actual four-process checks for purchased-beacon Listener/outer attraction, hazardous receipt/normal return, emergency recovery/next shift and stage agreement.
- [ ] Connect Cinder progression saving after human cycle-quality review. Keep clues/final creatures/audio, four-human, other-PC/LAN, Windows and performance checks separate.

The historical `artifacts/` paths identify local evidence from the original run. These files are no longer retained here and are not included in the public repository. Historical passes are distinct from current revalidation.

## 2026-10-02 — Repository and local file cleanup

[Cleanup scope and retention policy](repo-hygiene.en.md). Remove 51 unreferenced Unity review copies/duplicate Smart Wall files (44,900,041 bytes), 115,418,532 bytes of stale local tests/logs/caches and 246 LFS cached objects (approximately 122MB reported). Preserve production sources, current assets, historical documents/commits and 8 existing ship material edits. Retain game 0.9.3 / protocol 12. Pass dependencies across all Unity Assets, LFS HEAD integrity and post-cleanup Mac build with 0 errors / 4 warnings. Mark 142 historical local-evidence links as unretained provenance; all 272 documents pass link/language/checkbox checks. [Per-file evidence](../validation/repo-cleanup-2026-10-02.json).

## 2026-10-02 — Cinder Listener, baton and rescue 0.9.3

[Current rules, running and production checks](cinder-listener.en.md). Connect one Listener's actual floor/obstacle grid movement, noise investigation, warning/attack, existing empty-hand left-click baton, hold-E rescue and all-down ship recovery to default Cinder delivery. Preserve normal 420 CR payment and source art scene. Judge safety using current Cinder ship coordinates; do not instantiate old suppression/outer/clue/save objects. Existing beacon pulses also attract Listener investigation. Keep peaceful delivery regression via `--delivery-only` and movement tests via `--map-only`. Version 0.9.3, protocol 12, TCP 27842. Supersede older unconnected-Cinder-Listener/baton/rescue statements below only within this scope. Suppression/outer creature, clues, progression saving and human quality remain pending. [Actual validation scope](../validation/cinder-listener-0.9.3.json).

- [x] Implement current Cinder noise/path/attack/baton/down/rescue/emergency aboard recovery and pass 31 actual four-process checks.
- [x] Verify 92 route endpoints, 2,400 patrol collision steps, 5 rescue conditions, 17 controls/UI, 35 ledger, 13/100 HUD conditions, 48 delivery/UI and 13 movement/network regression checks, 4 legacy patrol endpoints and bilingual native screens.
- [ ] About 5 minutes of human controls and four-human cooperation/warning-audio/appearance quality review, distinct from automated passing.
- [ ] Suppression/outer/clues/progression saving, separate active-Listener normal receipt return/purchased-beacon distraction checks, other-PC/LAN/Windows/extended stability/performance.

## 2026-10-02 — Previous 0.9.2 investigation and recommendation

After current 0.9.2, recommend **brief human control review → Cinder Listener/baton/rescue integration → suppression/exterior and beacon distraction → progression saving**. This is not implementation of new gameplay or user quality approval.

- [ ] Play a delivery for about 5 minutes, checking awkward/confusing parcel rotation, reach, ground placement and ship departure/return/purchase buttons. Four-process automated checks do not replace this human assessment.
- [ ] Connect existing Listener sound response/movement/attacks, empty-hand baton defense and teammate down/rescue to current Cinder corridors/collision. First validate a delivery segment where evasion, defense and rescue are available choices.
- [ ] Then connect risks inside/outside suppression and actual Listener distraction by the purchased beacon. Do not treat current purchase/carrying/signal/audio as completed distraction.
- [ ] Review combined delivery/purchase/return behavior before Cinder progression saving and other-PC/LAN, Windows, performance/extended stability checks.

[Current controls](controls-ui.en.md) · [Existing Listener/rescue rules](space-play-03.en.md). During this 2026-10-02 investigation, the previous four manual processes were no longer running. Verify from [code](../../NoReturns/Assets/_NoReturns/Runtime/CarryLab/CarryRoom.Network.cs) that all-aboard checks include only current participants, including a one-person room; do not claim game execution or human input verification in this investigation.

## 2026-10-01 — Controls, ship and purchase UI 0.9.2

[Current controls and running](controls-ui.en.md). E targeted use/ship terminal, left-click ground placement, hold right click to rotate parcel, wheel 0.75–1.6m reach, Q immediate release. Separate departure/return/purchase into native buttons showing boarding count, wallet, disabled reasons and zero-pay return confirmation. Esc settings provide 13 button rebindings, sensitivity, FOV, language/defaults. Menus block gameplay inputs while the world continues. Connect Cinder 120 CR beacon purchase/shared carrying/two 8-second signals. Version 0.9.2, protocol 11, TCP 27842. Preserve source map/delivery pay. Supersede E automatic progression/Esc shop and unconnected-Cinder-beacon statements below within this scope. Listener/baton/suppression/save integration and human quality assessment remain. [Actual validation scope](../validation/controls-ui-0.9.2.json).

- [x] Targeted use, rotation/reach, collision-checked placement, menu input blocking and 13 saved rebindings.
- [x] Native connection/settings/ship/shop/log UI, all-aboard/host authority/zero-pay confirmation/duplicate-purchase guards.
- [x] Cinder beacon purchase/shared physical carrying/aboard placement preserving charges. Listener distraction remains incomplete.
- [ ] Four-human control preference/readability/fun, other-PC/LAN, latest Windows execution, performance/extended stability.
- [ ] Cinder Listener/baton/suppression/progression-save integration.


## 2026-10-01 — Cinder delivery, receipt and return settlement

[Current delivery test](four-player.en.md#2026-10-01--cinder-delivery-receipt-and-return-settlement).

- [x] Implement BAY 04 terminal/floor marking, physical ship E progression, all-aboard requirement, receipt collection, one settlement and next-shift reset in the derived Cinder scene.
- [x] Reuse the delivery ledger/CRT/model/screens, preserve the source art scene and separate default delivery from retained `--map-only` testing.
- [x] Mac build 0 errors/7 existing warnings; 23 four-process delivery, 35 ledger, 6 CRT surface/occlusion, 132 four-body lane positions and 7 existing rescue rules; inspect KO/EN terminal/receipt/report captures. Preserve 647 of 648 source hashes, changing only the derived scene.
- [ ] Four-human carrying/passing/collection/controls/fun, LAN/other-PC, Windows, extended stability and performance.
- [ ] Integrate Listener/baton → suppression/exterior → beacon/progression saving in the current map. See the [validation record](../validation/cinder-delivery-01.json) for completed automated scope.

Supersede older unconnected-receipt/delivery/settlement statuses below within this implementation scope. This is not full demo completion or quality approval.

## 2026-10-01 — Four-player test environment and remaining integration

[Current test scope](four-player.en.md#2026-10-01--cinder-four-player-map-test-environment).

- [x] Build separate Cinder test scene/Mac player and start/stop tools for 1 host/3 clients.
- [x] Mac build: 0 errors/7 existing warnings; pass 13 actual four-process checks, 7 existing rescue-rule conditions and 7 regular-mode defaults. Inspect 2 native 800×500 Korean HUD captures for crew 4/4, E/Q controls, reticle and team colors; also verify 4 manual processes/3 connections and shutdown.
- [ ] Four-human controls/passing/simultaneous parcel rotation/navigation/fun, extended stability, performance, other-PC/LAN and Windows execution.
- [x] Integrate BAY 04 receipt facilities and delivery/receipt/settlement in derived Cinder coordinates; see CINDER-DELIVERY-01 above.
- [ ] Integrate Listener/baton, suppression/exterior, beacon and saving at current Cinder coordinates.

Do not treat the movement/carrying test as complete cooperative gameplay or quality approval.

## 2026-10-01 — Open sky and zone boundaries

[Current environment](cinder-compact-site.en.md#2026-10-01--open-sky-and-zone-boundaries).

- [x] Remove 82.8m² of cover and matching invisible ceilings; apply boundary bands, 2 signs and 4 suppressor pads measuring 2.4×2.4m.
- [x] Reduce repetitive placement of 59 rocks, replace 4 suppressor visuals and retain ground collision/40 lights/runtime. Pass 6,864 carrying poses, 94 actual carrying segments and terrain rendering.
- [ ] User review of sky opening, navigation, boundary readability and pad appearance.
- [ ] Human four-player passing/simultaneous carrying, performance/standalone builds and exterior/suppression/delivery/Listener/baton/network integration.

Distinguish earlier overall direction approval from new appearance-quality approval.

## 2026-10-01 — Work after applying the background

Save the [current exterior background](cinder-compact-site.en.md#2026-10-01--rocky-territory-and-industrial-background-outside-the-field) in the same scene. Distinguish implemented scenery from quality/gameplay validation.

- [x] Place 59 rocks, 9 industrial visuals and 1 terrain surface: 69 placements and 4,624 triangles. Pass retained 94 movement segments, 17 four-body lanes, 6,864 carrying poses and actual-terrain render regression.
- [ ] User review of background density, rock repetition, distant-facility visibility and existing danger-signal readability.
- [ ] Human four-player passing/simultaneous carrying, performance and standalone builds.
- [ ] Exterior access/creatures, suppression-stage changes and Cinder receipt/Listener/baton/ship/delivery/network integration. Current scenery adds 0 Colliders/Lights and retains the existing 4 fall guards.

Next production candidates are BAY 04 receipt facilities/marking and Cinder-coordinate integration of existing gameplay. Building confirmation does not establish detailed quality approval.

## 2026-10-01 — Work after varied building structure

[Current structure/checks](cinder-compact-site.en.md#2026-10-01--varied-building-outlines-and-structural-modules): apply 9 module types, 11 meshes, 26 placements and 2 replaced ground outlines. Pass 94 movement segments, 17 four-body lanes and 6,864 carrying poses. Next: user review of building silhouettes, canopy visibility, new-corner wayfinding/cargo rotation. Upper-room access/stairs/interaction, human four-player play, performance, standalone builds, background quality and Cinder receipt/AI/suppression/baton/networking coordinate integration remain incomplete. Earlier whole-map direction approval does not approve this appearance.

## 2026-10-01 — Next asset task

The user approved the feel of the [current whole map](cinder-compact-site.en.md). Place 47 prop groups, 46 static freight visuals, 3 CRTs and 6 labels in the same scene, retaining 94 movement segments and 17 four-body lanes. 5,904 carrying poses, 94 actual carrying segments and 407,130 penetration checks yielded 0 overlaps. Next: direct prop density/navigation/carrying visibility and human four-player passing review → BAY 04 receipt facilities/marking and AI/delivery coordinate integration → finer auxiliary-equipment appearance and background quality. Preserve existing scenes, building sizes and carrying code. Props have no delivery/receipt functionality; distant repetition, performance and standalone builds remain incomplete.

Apply the current [dusk sky/distant haze](cinder-compact-site.en.md#2026-10-01--sky-and-distant-atmosphere) in the same scene; verify 5,904 carrying poses, 94 movement segments and 25,942 penetration checks with 0 overlaps. Include sky/fog, work-light contrast and final danger-signal readability in the next quality review. Distinguish applying the environment direction from user-quality approval/performance verification. Weather/day-night changes are outside this implementation scope.

Cinder gameplay integration also remains. Review, connect and revalidate existing `CarryRoom` Listener navigation, baton and ship/delivery coordinates against current Cinder space. Their absence from the gray scene reflects missing integration, not deleted code.

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

Added Korean by default, the 한국어 / English menu toggle and persistence of the local preference. [Usage and production guide](carry-test.en.md). Language selection is separate from online game state. Results are recorded in the language checks (`artifacts/language/latest.json`).

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


2026-09-17: [CONCEPT-ZONING-02](concept-zoning.en.md). Added three proposed loops and connecting paths: A warehouse, B central freight obstacles, C east service block. Proposed one Listener per zone (three total), with separate lure-player markers. Each loop has at least two escape connections; purple dashes represent proposed traversable routes. Evaluate one employee drawing pursuit while others carry along the opposite side. Infinite kiting, player-count scaling, hearing/pursuit reset/speed and cargo clearance remain undecided. Only HTML visualization changed; no AI, Unity or existing playable-study integration. Browser rendering of loops and zone markers inspected. Cooperative fun and actual pursuit remain unverified.


2026-09-17: [CINDER-BLOCKOUT-01 — Unity primitive layout trial](cinder-blockout.en.md). A 108×86.4m validation scale, 5 enterable buildings, 3 loops and reused ship interior trial 05. Separate single-player spatial experiment; enemy pursuit, networking and delivery settlement are not connected. 41 physical passage checks and Editor rendering inspected; Windows build succeeded. Human controls, cargo clearance and cooperative enjoyment remain unverified.

2026-09-17 / WORLD-01: [Setting expansion proposal](world-setting.en.md). Outstanding decisions: approve the corporate-logistics focus; define Earth's cause of destruction, creature origins and concealed company information; select first-delivery clues. Underground cities, new NPCs, factions and resonator cooling are not implemented. Narrative alone does not change existing delivery, rescue or reward rules.

2026-09-17 / TRAILER-01: [Trailer production backlog](trailer-recruitment.en.md). Review 75-second proposal → temporary-voice animatic → keyframe approval → actual cooperative capture → final sound/subtitles/publication review. Do not advertise pursuit, networking or delivery as implemented in the new Cinder blockout where they are not connected. Omit wishlist messaging until the store page is ready.

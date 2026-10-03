# Testing and release checklist

[한국어](05-validation.ko.md)

## 2026-10-04 — Dynamic baton and first-person hands/arms 0.9.14

At the user's request, implement short anticipation, fast extension and 0.42s recovery through baton arm/chest motion, local right glove/sleeve and two-hand parcel carrying. Preserve immediate contact, 6s cooldown, carrying/down/valid-rescue blocking and existing movement. [Current baton](employee-baton.en.md), [arm production/local reproduction](first-person-arms.en.md), [validation evidence](../validation/employee-first-person-0.9.14.json). Game **0.9.14**, protocol **13**, TCP **27842**. Earlier 0.9.13 original-motion restoration and first-person-arms-not-implemented/proposed states below are historical and superseded within this implementation scope. Dedicated beacon/rescue hands, human quality, actual game GPU composition, all-corner penetration, Windows/LAN and performance remain pending. Manual play stays stopped; use the existing two-window helper only on request.

- [x] Mac build: zero errors/7 existing warnings. Baton 930 + first-person 560 = 1,490 static samples; 1 actual URP offscreen stack render; inspect 5 images.
- [x] Actual windowless two-client baton/hands/movement 20 checks, companion 6 groups and four-client hazard/down/rescue 32 checks: 58 total. Include local hand hiding during down/valid rescue.
- [ ] Human naturalness/impact, actual game GPU composition, all-distance/corner penetration, dedicated beacon/rescue hands, Windows/LAN/performance.

## 2026-10-03 — Original baton/backward-sidestep 0.9.13

- [x] Restore original 0.9.6 baton readiness/use, remove attack arm/chest correction and retain hand attachment, 0.5s return, immediate contact and 6s cooldown.
- [x] Redirect backward/left/right/diagonal feet; maintain separate lanes, torso/root/bone length and release in air/down/stop.
- [x] Mac build zero errors/7 existing warnings; 930 baton, 270 jump/landing and 225 direction static samples passed.
- [x] Actual two-client baton/direction 19 checks, companion 6 groups and four-client Listener/down/rescue 30 checks passed: 55 total. [Validation record](../validation/employee-direction-0.9.13.json).
- [ ] Revised human quality, dedicated movement clips, first-person arms, all-frame penetration, Windows/LAN/performance and full delivery/suppression regression.

The first native direction test was blocked by the dropped parcel. Without changing collision/game rules, move to the existing access lane away from cargo and match outbound/return times; the same check then passed. Initial regressions could not start while prior TCP connections were closing; rerun successfully after port release. [Current baton](employee-baton.en.md), [current movement](employee-locomotion.en.md). The 0.9.12/0.9.10/0.9.9 sections below are historical checks of cancelled attack presentation, not current behavior.

## 2026-10-03 — Restored baton grip, forward pulse and jump/landing 0.9.12

The user requested ending manual play, fixing awkward baton holding and proceeding to the next task. Stop manual session `run-20261003-222202-345af2` with the existing helper. Restore upright 0.9.9 readiness and first-person offset/orientation, extending the hand and baton once by 0.20m and returning within 0.30s. Cancel always-forward readiness from 0.9.10. Retain immediate contact, 6s cooldown, hand attachment and carry/down/rescue exclusion; add no magic projectile. As the next task, apply actual position/floor-based rise/apex/fall, 0.22s landing and up to 0.065m hip compression/planted-foot correction over the existing Humanoid. Preserve root, bone lengths, physics/jump/network rules and two-player 16:9/four-player 2×2 tiling. Game 0.9.12, protocol 13, TCP 27842. [Baton](employee-baton.en.md), [jump/landing](employee-locomotion.en.md), [validation record](../validation/employee-pulse-locomotion-0.9.12.json).

Mac build passes with zero errors/7 existing warnings, 120 static baton and 270 jump/landing samples, 12 actual windowless two-client baton checks, 6 companion groups and 30 four-client Listener/hit/down/rescue checks: 48 native checks total. Both peers observe rise/apex/fall/landing/grounded, with about 0.644800m jump-height range. Review 8 static images and track them with LFS. Initial 0.24m extension failed with 0.037240m path deviation at the arm reach limit; reduce it to 0.20m. The initial companion collector missed landing weights after the Jump label ended; inspect observed Landing states, span the label boundary and pass. Unity CLI 1.0.0-beta.12, Pipeline 0.8.0-exp.1 and Editor 6000.6.4f1 match official latest releases; no further updates. Add no FBXs or Blender source/MCP edits.

Update affected Korean/English specifications, production guidance, backlog, checklist and employee/launch guides; remove cancelled 0.9.10 readiness instructions from current specifications. Preserve historical checks/records. Open no new manual windows or Editor Play and terminate automated processes/session. Revised human quality, dedicated backward/sidestep movement, first-person arms, all-frame penetration, full delivery/suppression regression, Windows/LAN/performance remain unverified. Preserve eight preexisting material and two Blender modifications and exclude them from the commit.

## 2026-10-03 — Landscape 16:9 test-window layout 0.9.11

Implement equal 16:9 left/right viewports for two players and 16:9 viewports inside a 2×2 grid for four players. Address user feedback about the small companion window and vertically stretched viewports. Mac build passed with zero errors/7 warnings; 18 checks covered two/four-player cells, 16:9, overlap and boundaries, and actual two-window sizes/AppKit positions and host/companion connectivity were checked. Actual four-window placement, Windows/Intel Mac, external monitors/display-scale changes and user handling remain unverified. [Current guide](companion-play.en.md), [validation record](../validation/client-window-layout-0.9.11.json).

## 2026-10-03 — Straight baton thrust 0.9.10

Passed the Mac build with 0 errors/7 warnings, 120 attack poses, 12 actual two-client thrust/carry-exclusion checks and 30 four-client Listener/contact/down/rescue checks (42 total). Verified forward direction, path/bone lengths and exclusion/return. Fixed near-wall first-person wobble found by the initial check and passed the same assertions. Open no manual play windows; human hit-feel/all penetration, Windows/LAN and performance remain unverified. [Current behavior and validation scope](employee-baton.en.md).

## 2026-10-03 — Baton readiness, attack and recovery 0.9.9

Passed the Mac build with zero errors/7 warnings, 210 attack/120 carry static samples and actual windowless baton 12/carry 10/Listener-contact-down-rescue 30 checks (52 total). Verified interruption/exclusion/recovery and rescue priority, then stopped test processes and removed session records. Human hit-feel/appearance, all locomotion phases/clothing penetration, Windows/LAN and performance remain separately unverified. [Implementation, production and validation scope](employee-baton.en.md).

## 2026-10-03 — Two-handed parcel carrying 0.9.8

Pass Mac build with zero errors/7 warnings, 120 pose samples, 10 windowless four-client checks and 5 grouped human+companion regressions. Open no manual windows; human quality, full delivery/hazard regression, other OS/PCs and performance remain unverified. [Implementation, evidence and limits](employee-animation.en.md).

## 2026-10-03 — On-demand human + automatic companion 0.9.7

[Current launch, behavior and validation guide](companion-play.en.md). Open one human host and one automatic companion client only when the user says **“직접 테스트 해볼게”** (I will test it myself). Default to safe practice in the current map: repeat idle, walk/sidestep, slow movement, jump, baton and nearby parcel carry/drop on a 32-second cycle; follow the human when distant. Game 0.9.7, protocol 13, TCP 27842. Mac build with zero errors/9 warnings and 5 grouped checks in two actual windowless processes passed. Do not open manual windows this task; inspect focus/mouse and human visual quality after the request. Complex maze/multilevel routes, Windows/performance and rerunning the full 110-check regression are outside this validation scope.

## 2026-10-03 — Right-hand baton 0.9.6

See the [current employee/baton guide](employee-animation.en.md) and [tools/Blender MCP setup](macos-development.en.md). Attach the baton to the actual right hand and apply a finger grip. Hide it on every peer while carrying a parcel/beacon, restore it after dropping/placing, and retain down/rescue hiding. Version 0.9.6, protocol 13, TCP 27842, Unity 6000.6.4f1. [Actual validation scope](../validation/baton-hand-0.9.6.json). Carrying contact, dedicated full-body attacks, first-person arms, user quality/all-frame penetration, other PCs/Windows/performance remain incomplete.

- [x] Unity 6000.6.4f1 Mac build: zero errors, 14 warnings. Four actual processes pass 69 baton/suppression/delivery + 10 visual/transition + 31 Listener/hit/down/rescue checks = **110**. Review 4 idle/walk hand/body samples and a game capture from the new build.
- [x] Actual MCP stdio tool calls read Blender's 5 objects, 53 bones and right-hand information. Preserve unsaved scene/existing user edits. Apply current official Unity/CLI/Pipeline/uv/Codex CLI releases and confirm Blender 5.2.2 LTS matches the latest release.
- [ ] Codex desktop update/restart and default MCP tool exposure, the new Unity's Windows support/execution, human quality/all-frame penetration and performance. Observe a Burst error resolving a Pipeline DLL during upgrade and 5 Burst entry-point warnings in the final build. Build/110 game checks pass, but warning cause/performance impact remains unverified. Remaining warnings concern mesh collision pre-baking, absent Pipeline runtime config, obsolete search APIs and stripped debug shaders.

## 2026-10-03 — Employee full body and Idle/Walk 0.9.5

[Production/launch guide](employee-animation.en.md) · [Validation JSON](../validation/employee-unity-0.9.5.json).

- [x] Two separate Humanoid Avatars, 53 bones/10,612 Unity vertices, looping clips, Root Motion disabled, 10 finite-deformation/size/root pose samples and static pose image review.
- [x] Unity 6000.6.0f1 Mac build: zero errors and 7 existing warnings. Four actual processes pass 10 visual/transition, 13 carrying/reconnection and 31 Listener/baton/down/rescue checks, 54 total. Actual game captures confirm employee appearance, team tint, existing baton and lying employee display.
- [ ] User posture/color approval, foot sliding/loop seams, full-frame penetration/cargo contact, dedicated additional motions/first-person arms, four humans, other PCs, Windows and performance. Motion source redistribution remains unresolved; keep files local. The complete 60-check suppression and delivery/payout UI suites were not rerun.

Supersede earlier Unity/Walking/four-player-unverified statements below within the scope above.

## 2026-10-03 — Mixamo Idle upper-body correction

[Production/local-file guidance](03-guides.en.md#2026-10-03--correct-the-supplied-idle-upper-body-posture) · [Validation JSON](../validation/employee-idle-posture-2026-10-03.json).

- [x] Preserve source FBX/canonical Blender, mesh/UV/weights/rest skeleton and original Action; finite deformation and unchanged lower-body positions across 251 frames; matching loop endpoint positions.
- [x] Reopen Blender with both Actions; reimport a single corrected FBX clip with 53 bones, 4K texture and approximately 0.0023mm maximum position difference at 5 samples. Nine automated checks plus 5 side frames/front/three-quarter/comparison render review.
- [ ] User posture approval, velocity continuity/all surface intersections, Walking rest-pose conversion, Unity Humanoid/transitions, carrying contacts, first-person/four-player/performance. Walking only passes structural inspection: 53 bones, no mesh, 30fps and frames 1–32.

## 2026-10-03 — Employee rig candidate and deformation trial

[Files, reproduction and frames](03-guides.en.md#2026-10-03--employee-boot-repair-and-first-deformation-rig) · [Inspection JSON](../../art/player-employee-01/rigged/validation.json).

- [x] Zero boot exception edges of each kind, preserved boot surfaces/all UVs, 53 bones and every vertex normalized with at most 4 influences. Adjust 20 pad-rim vertices by at most 8.53mm; verify at most 3.24mm from corresponding cloth points in sampled poses.
- [x] Review 7 sampled frames and 8 actual renders, reproduce poses after `.blend` reopening and reimport FBX skeleton/mesh/weights/4K texture. Pass 8 file/numerical checks. Verify the diagnostic action is excluded from FBX.
- [ ] Extreme-bend angular compression/small-edge stretching and user quality approval, Unity Humanoid, idle/walk, actual cargo contact, first-person arms and four-player/performance. Static FBX and sampled poses do not replace game/release validation.

## 2026-10-03 — Employee Blender handoff checks

Follow the [working copy, sources and reproduction method](03-guides.en.md#2026-10-03--employee-blender-inspection-and-working-copy) and [measured JSON](../../art/player-employee-01/validation.json).

- [x] Preserve FBX/JPG hashes, import in Blender and inspect counts/UVs/textures/skeleton. 4,230 quads + 658 triangles = 4,888 faces, 9,118 after triangulation. Verify the linked 4096×4096 JPG and absence of a skeleton.
- [x] Save/reopen a 1.8m floor-centered `.blend` at rotation 0/scale 1, with packed texture and unchanged source topology counts/connectivity. Pass 8 reproducible script checks, visually inspect 6 actual renders and verify the working copy displayed in the Blender app.
- [ ] Unity/performance and user appearance approval. Supersede receipt-stage boot/rig pending status within the candidate checks above; do not classify all 437 original boundary edges and 21 components as defects.

## 2026-10-03 — Employee topology validation preparation

[Settings, production order and official evidence](03-guides.en.md#2026-10-03--initial-employee-topology-settings). The initial settings research ran no model/joint checks. Subsequent static mesh inspection is recorded above; deformation checks remain pending.

- [ ] Inspect actual Faces/Tris, face flow, separated fingers, normals and UVs/textures on a mesh generated with the proposed Quad/5,000-face settings. Distinguish the approximately 10,000-triangle estimate from measured output.
- [ ] With a temporary rig, check raised shoulders, 90-degree elbow/knee bends and parcel carrying, then final triangulated mesh/first-person hands in Unity. Finalize the budget using deformation and performance results.

## 2026-10-03 — Individual view image checks

- [x] Check full-body framing/orientation/original poses and removal of text/swatches in [3 individual images](03-guides.en.md#2026-10-03--separate-employee-view-images), each 1254×1254 PNG, exact generation records and bilingual documentation.
- [ ] User appearance approval, same-pose multi-view consistency, finger structure and Tripo model/rig/game validation. The current side view is a lowered-arm reference pose.

## 2026-10-03 — Employee image concept 02 review

[Concept, review scope and exact generation record](03-guides.en.md#2026-10-03--employee-image-concept-02).

- [x] Generate/view 1 PNG at 1774×887: inspect front/back T poses, lowered-arm side view, empty hands, workwear abrasion, a back without protruding equipment and 4 team-color swatches. Check bilingual document links/checkbox states.
- [ ] User appearance approval, separate Tripo inputs, cross-view proportions/fingers/seams, model/rig deformation and first-person/remote gameplay validation. Distinguish these from successful image generation.

## 2026-10-02 — Player rig/animation validation preparation

[Proposal with code/official documentation evidence](demo-art-list.en.md#2026-10-02--proposed-player-model-and-animation-workflow). This task performs only source/official-capability research and documentation checks. No new model/clip import, Unity execution/build or human quality validation was performed.

- [ ] Check FBX units/orientation, valid Humanoid Avatar, shoulder/elbow/knee deformation and textures.
- [ ] Check in-place locomotion, disabled Root Motion, preserved collision/movement, speed-dependent foot sliding and transitions.
- [ ] Check arm reach/contact transitions across cargo distance 0.75–1.6m, rotation and floor placement; first-person visibility/wall clipping, remote full body and 4 team colors.
- [ ] Check carrying/baton mutual exclusion, down/2.5-second rescue/recovery and consistent state in four actual processes. Review visual quality separately with people.

## 2026-10-02 — Cinder suppression and outer creature 0.9.4

[Current rules, production and running](cinder-suppression.en.md). Connect existing 90/135/180-second stages, signal audio and relative work-light dimming to 4 current suppressors and the boundary. Connect east entry, obstacle routing and pursuit for the outer creature 8 seconds after shutdown. Preserve sky/fog/sun, source art and collision. Default launch is delivery + Listener + suppression/outer; retain `--delivery-only`/`--map-only` regression. Version 0.9.4, protocol 13, TCP 27842. Pass 60 actual four-process checks for purchased-beacon distraction, hazardous return, shared down/ship safety, emergency recovery and next shift. [Validation record](../validation/cinder-suppression-0.9.4.json). Supersede older unconnected-suppression/outer statements below within this scope. Human quality, final creatures/audio, clues/progression saving and other-environment/performance checks remain pending.

- [x] Verify 9 time boundaries, 5 signals, 40 work lights, 7 outer routes/1,600 collision steps and preserved sky/fog/sun/source art/scenes.
- [x] Pass 60 actual four-process checks: hazardous delivery/420 CR return, 120 CR purchase/300 CR retention, both creature attractions, stages/grace, baton immunity, shared down/ship safety, all-down recovery/next shift.
- [x] Mac 0.9.4 build with 0 errors/7 existing warnings, 2 native 800×500 bilingual warning PNGs and document links/language/checkbox checks. Pass 31 existing Listener/rescue, 48 delivery/UI and 13 movement/network regression checks and all 274 document checks. Total actual player checks: 152.
- [ ] Four-human controls/warning interpretation/fun/appearance/audio/dim-floor readability, clues/progression saving and other-PC/LAN/Windows/performance/extended stability.


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

[Current evidence](cinder-compact-site.en.md#2026-10-01--open-sky-and-zone-boundaries).

- [x] Remove 82.8m² of cover, pass upward Raycasts and adjust 2 ceiling colliders into 4. Preserve ground collision and suppressor envelopes; 0 new ground colliders/Lights. Apply 2 signs, 4 pads and 4 native meshes.
- [x] Pass 94 movement segments, 17 four-body lanes, 286 valid positions, 6,864 carrying poses, 94 actual carrying segments and 274,484 penetration checks: 0 overlaps, 103 contacts, E/W/S/Q/empty-hand jumping.
- [x] Terrain rendering changes 71,212 pixels/640×360/10,000 threshold; reimport 4 meshes/reopen the scene, capture 30 views/inspect 9 key views. 0 compilation errors, 3 existing warning types, 0 new-source warnings and final Play 0 errors/warnings.
- [x] After 2 initial automated input failures, fix test teleport synchronization/frame waiting and pass reruns. Preserve 608 of 609 starting files, remove 0 original scene IDs, exclude 8 pre-existing ship-material edits and preserve historical evidence.
- [x] Align paired documents; 142 existing missing links/0 new failures. Record raw whitespace failure at 74 native-generated locations; pass overall checking excluding generated trailing spaces and code/document/evidence whitespace checking.
- [ ] User appearance/boundary readability, human four-player controls, performance/standalone builds and Cinder gameplay integration.

## 2026-10-01 — Exterior background and actual rendering

[Current background, running and evidence](cinder-compact-site.en.md#2026-10-01--rocky-territory-and-industrial-background-outside-the-field).

- [x] 8 native meshes, 2 materials, 1 native 128×128 texture, 69 placements, 4,624 triangles, unit scale 1. All triangles' horizontal bounds remain outside the field; 0 new Colliders/Lights. Retain 4 fall guards, 40 local lights, buildings/props/sky.
- [x] Pass 94 movement segments, 17 four-body lanes, 286 body positions, 6,864 carrying poses and 94 actual carrying segments. 26,902 penetration checks, 0 overlaps, 103 contacts; E/W/S/Q and empty-hand jumping pass.
- [x] Terrain winding, finite UVs, nondegenerate triangles and supported shader checks; reimport 8 meshes/reopen scene. Terrain enabled/disabled difference: 55,914 pixels in actual 640×360 native rendering, above the 10,000 threshold. Fix mesh render-buffer updates and inspection-camera cleanup. Capture 25 views, inspect 8 key views and restore cutaway hiding.
- [x] 0 compile/shader errors, 3 observed existing Editor warning types, 0 new-source warnings and final Play/render console 0 errors/warnings. Preserve 484 of 485 starting file hashes, excluding the current scene; exclude 8 existing ship-material edits.
- [x] Update paired documentation; no new failures beyond 142 existing missing links. Record 163 raw staged generated trailing-space locations; verify code/document/evidence checks and overall check excluding those generated spaces.
- [ ] User background quality, danger-signal readability, human four-player play, performance, standalone builds, exterior gameplay and Cinder gameplay integration.

## 2026-10-01 — Varied building forms

[Current structure, running and evidence](cinder-compact-site.en.md#2026-10-01--varied-building-outlines-and-structural-modules).

- [x] Verify 9 structural module types, 11 meshes, 26 placements, 4,021 triangles, 25 static MeshColliders and scale 1. Replace 2 ground outlines; preserve main interiors/ship, 47 prop groups, 6 labels, 40 local lights and sky.
- [x] Pass 94 movement segments, 17 four-body lanes, 3/3.15m widths and 4 loops. 286 body positions, 6,864 carrying poses, 94 actual carrying segments and 340,268 penetration checks with 0 overlaps, 103 contacts; E/W/S/Q and empty-handed jumping passed.
- [x] Pass UV/nondegenerate triangles/positive signed volume, cap-tile density on 140 triangles, reimport of 11 meshes and scene reopening. Capture 21 cameras, review 8 key views, restore upper-structure hiding. 0 compile/sky-shader errors, 3 observed existing Editor warning messages, 0 new-source warnings and final Play 0 errors/warnings.
- [x] Preserve 434 of 435 starting hashes, excluding the current scene. No new failures beyond 142 existing missing document links. Fix light-record ordering false positive without data changes.
- [ ] User appearance/visibility/wayfinding, human four-player play, performance, standalone builds, background quality and gameplay integration. Upper-room access/interaction is unimplemented.

## 2026-10-01 — Dusk sky and distant haze

[Current settings, running and evidence](cinder-compact-site.en.md#2026-10-01--sky-and-distant-atmosphere).

- [x] Check builtin Skybox/Cubemap, 64×64 pixel 6-face RGBA32 cubemap, 35–115m Linear fog and lighting values. 0 sky-shader errors; preserve 40 local lights, collision and 246 occupiable positions.
- [x] Pass 94 movement segments, 17 four-body lanes, 5,904 carrying poses and 94 actual carrying segments. 25,942 penetration checks with 0 overlaps; E/W/S/Q and empty-handed jumping passed.
- [x] Review 18 actual cameras, label/carrying visibility, interior contrast and roof restoration. Match 312 of 313 starting hashes, excluding the current scene. 0 compile errors, 6 existing obsolete warning types and final Play 0 errors/warnings.
- [x] Preserve earlier props- evidence; verify current sky- check/capture paths. No new failures beyond 142 existing missing documentation links.
- [ ] User sky/fog quality, final danger-signal readability, human four-player play, performance, standalone builds and gameplay integration.

## 2026-10-01 — Current whole-site prop placement

[Props, running and evidence](cinder-compact-site.en.md). The `site-` counts below record the structure before props.

- [x] Record user approval of whole-site direction. Check 47 prop groups, 46 freight visuals, 3 CRTs, 6 labels, 8 native meshes, 108 placements, 205,055 triangles and 56 reserved BoxColliders.
- [x] Preserve original scene BoxCollider world settings, 159 occupiable positions, 94 movement segments and 17 four-body lanes. Match 158 of 159 starting file hashes, excluding the current scene.
- [x] Resolve 8 failing cabinet-gap poses by correcting placement. Pass 5,904 carrying poses, 94 actual carrying segments and 407,130 penetration checks with 0 overlaps; E/W/S/Q and empty-handed jumping passed.
- [x] Review 17 actual camera views, label readability and restoration of hidden roofs. 0 compile errors, 6 existing obsolete warning types, final Play 0 errors/warnings. Retain 142 existing missing documentation links with 0 new failures.
- [ ] Prop quality, human four-player passing/simultaneous cargo rotation, distant patterns, performance and standalone builds.
- [ ] Receipt facilities, AI/delivery/suppression/baton/networking integration at current Cinder coordinates and background quality.

## 2026-10-01 — Relocating the entire suppression-field site

[Current whole layout, running and evidence](cinder-compact-site.en.md). The interior maze below records the earlier incorrect scope.

- [x] Relocated 5 buildings/the ship; checked 6 auxiliary volumes, 3/3.15m alleys, 4 loops and 94/79.1m routes reaching the same BAY 04.
- [x] Preserved local collision on 50 building BoxColliders/the ship; checked 59 Colliders in the original group, 22 in the new group, native tile UVs, placement scale 1 and 74 of 75 starting file hashes, excluding the corrected shared carrying source.
- [x] Passed 94 bidirectional movement segments, 17 four-body lanes, 3,816 static carrying poses and 94 actual carrying segments. 237,148 penetration checks with 0 overlaps; E/W/S/Q and empty-handed jumping passed.
- [x] Reviewed 10 actual camera views and restored inspection-only roof hiding. 0 compile errors, 8 existing obsolete warning emissions (6 unique), final Play 0 errors/warnings. Full documentation retains 142 existing missing links with 0 new failures.
- [x] User approval of whole-site direction.
- [ ] Prop quality, junction/long-alley sight lines, navigation, human four-player passing/cargo rotation review.
- [ ] Final auxiliary facilities/cargo, background quality, receipt facilities, new-coordinate Cinder AI/delivery/suppression/baton/networking integration, performance and standalone builds.

## 2026-10-01 — Earlier 3m interior-maze record

[Current layout, scope and evidence](cinder-maze-layout.en.md).

- [x] Verified 157 existing wall modules, 24 partition Colliders, visual/collision bounds, 3m width, scale 1 and preservation of 109 original BoxColliders.
- [x] Passed 156 bidirectional movement segments and four simultaneous bodies through 19 interior straight lanes.
- [x] Passed 9,312 static carrying poses and 156 actual carrying segments, 22,196 native penetration checks total with 0 overlaps. E/W/S/Q and empty-handed jumping passed.
- [x] Reviewed 13 actual Play-camera views, retained saved ceilings and preserved 73 starting files. 0 compile errors; 3 existing obsolete warnings; final Play 0 errors/warnings. No new documentation failures beyond 142 existing missing links.
- [ ] User navigation/carrying rotation/repeated-wall quality and human four-player passing/simultaneous cornering feel.
- [ ] Earlier-scene human quality review remains incomplete; follow the whole-site entry above for current outdoor layout/spacing.

## 2026-10-01 — Shared Cinder building appearance expansion

[Current scope, measurements and review scene](cinder-map-appearance.en.md).

- [x] Recorded user approval of the aged warehouse as the map style reference.
- [x] Checked 8 Blender/FBX fills, Unity imports/UVs, floor/roof area, placement scale 1 and emission reimport. 0 new Colliders; preserved settings/placement of 109 existing BoxColliders.
- [x] Passed 41 existing movement routes, 6 warehouse passages, 3 jumps, 48 center-lane cargo poses, Mac Editor Play 2,352 poses with 0 penetrations, E/W/S/Q and empty-handed jumping. 0 compile errors; 0 errors/warnings in the final Play console.
- [x] Captured 15 actual Play-camera views; reviewed building empty-handed/carrying views and overall placement. Bilingual/link checks have 0 new failures beyond 142 existing missing links.
- [ ] User review of expansion visibility, joints, repeated patterns and lighting density; distant shimmer/performance, standalone Mac/Windows builds and ship Play.
- [ ] Receipt facilities, outdoor/connecting routes, suppression facilities and Cinder delivery/AI/networking integration checks.

## 2026-10-01 — First Cinder asset batch

[Specifications, review views and evidence boundary](cinder-asset-prep.en.md). This records gray production and separate review-scene checks; historical game/build passes do not validate the new batch.

- [x] Compared saved warehouse geometry against builder/carrying-code values.
- [x] Checked bilingual names, dimensions, budgets, checkbox states and local links in the first 8-unit preparation documents. Full documentation retains 142 existing missing artifact links with no new failures.
- [x] Checked units, axes, openings, pivots, UVs, closed surfaces, normals, reimport and Unity import for 5 structural units/11 variants.
- [x] Passed 6 forward/backward passages, 3 jumps and 48 cargo-pose samples in the separate review scene; verified E/Q, carrying passage/return and empty-handed jumping in Mac Editor Play. Zero compile errors, 7 existing warnings, zero new Play errors/warnings.
- [x] 2026-10-01 user warehouse-size review: found acceptable; retain the current 14.4×20.4m. No dimension changes.
- [x] Fixed 38 → 0 overlaps in 336 door/wall proximity poses; rechecked E/W/S/Q and empty-handed jump. [Evidence](cinder-asset-prep.en.md#2026-10-01--carrying-edge-fix-and-appearance-proposals).
- [x] Generated/inspected the [entrance proposal and 8-unit sheet](../art/cinder-appearance-01.en.md), recorded provenance/hashes and corrected 3 rack levels to 2.
- [ ] Obtain user cargo-visibility, edge carrying-rotation, wall-approach and joint-quality feedback. Standalone Mac/Windows builds were not tested in this task.
- [x] User concept-direction approval and shared surfaces, work lights, sign and empty 2-tier rack in separate `CinderAppearanceReview`.
- [x] Checked 14 new FBXs, 2 textures, 0 penetrations in 432 carrying poses, E/W/S/Q, empty-handed jump and matching empty-handed/carrying views. 0 compile errors and 0 errors/warnings in the final Play console. [Evidence](cinder-asset-prep.en.md#2026-10-01--applying-the-approved-appearance).
- [x] Recorded feedback that the first application was too clean; applied 2 imagegen-aged surfaces, normalized to 512×512 / 256×128, rechecked 14 imports/432 poses, inspected 6 actual camera views and matched hashes on 44 preserved files. [Revision evidence](cinder-asset-prep.en.md#2026-10-01--aged-texture-revision).
- [x] User approved the aged texture revision as the map style reference.
- [ ] User appearance, visibility and joint-quality review of the actual result; all-part multi-view review, distant shimmer and performance. Ship-scene Play revalidation remains separate.
- [ ] After expanding the reference area, validate actual Cinder delivery, enemy AI, networking and human cooperation. Do not mark currently unconnected systems complete.

2026-09-26: [Mac development environment](macos-development.en.md) — passed Unity 6000.6.0f1 arm64/Windows Mono installation, CLI/PATH, LFS restoration, Personal activation, compilation and actual CLI/MCP calls. Discovered 151 commands; zero compilation errors and 7 existing deprecated-API warnings. Passed bundled Korean-font checks and starting/stopping the `CarryRoom` menu after recompilation. The new run recorded zero Console errors/warnings. No scene changes. Automatic MCP exposure in a new Codex session, gameplay progression and game builds on both platforms remain unverified. Help text still overlaps the quit button in the small Game view.

2026-09-26 correction: the current development target is `CinderDepotBlockout` from [CINDER-BLOCKOUT-01](cinder-blockout.en.md). Only opening the existing scene and checking its hierarchy have been verified; Mac Play mode/builds for this scene remain unverified. The `CarryRoom` results above do not validate the latest map.

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

- Foundation setup log (`artifacts/space-foundation/setup.log`): exit code 0, `SPACE-01 FOUNDATION PASS`.
- Official MCP `editor_status`: fresh `NoReturns` path, Unity 6000.6.0f1, `ready`, no compilation or domain reload in progress.
- Documentation check: local links, language counterparts and checkbox parity passed across 174 Markdown documents. Numeric parity passed for current documents 01–05.
- [Build scene settings](../../NoReturns/ProjectSettings/EditorBuildSettings.asset): only the empty `Bootstrap.unity` is enabled. No human playtest or game-player validation was performed.

## SPACE-ECO-01 — Proposal calculation checks

- [x] [Calculation check](../../tools/check_economy_proposal.py): passed 126 delivered and 126 undelivered combinations, 5 examples, image wallet flow and bilingual numeric/checkbox parity. Result (`artifacts/space-economy/check.json`).
- [ ] Actual human balance checks and online payment/save/duplicate-processing checks in [Pay, failure and equipment economy draft](economy.en.md).

Arithmetic passing does not validate economic balance or Unity implementation.

## Pending first-person validation

- [ ] Understand forward/floor/corner visibility and set-down placement while carrying.
- [ ] Weapon switching, effects, hands, wall clipping and online teammate presentation consistency.
- [ ] Human evaluation of FOV, camera shake, discomfort and HUD readability.

Concept image review does not replace these game checks.

## SPACE-PLAY-01 verification boundary

[Carrying guide](carry-test.en.md) and automated results (`artifacts/space-play-01/latest.json`). Unity MCP, compilation, Windows build and 17 checks across two actual processes passed. URP camera rendering was visually inspected, distinct from complete menu/HUD screen verification. Human controls/fun, separate PCs, 4 players, WAN and Steam remain unverified. Earlier failure evidence is preserved.

## SPACE-PLAY-02

[View-relative carrying and delivery-mode specification/validation](space-play-02.en.md). Added vertical-look carrying, route selection, receipt, full-crew return and session pay. Actual map, persistence and the full shop remain. The hazard experiment is separated in SPACE-PLAY-03 below. User feedback: the previous carrying build runs, but cargo does not follow vertical view.

## Guidance language — 0.3.1

Added Korean by default, the 한국어 / English menu toggle and persistence of the local preference. [Usage and production guide](carry-test.en.md). Language selection is separate from online game state. Results are recorded in the language checks (`artifacts/language/latest.json`).

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


2026-09-14: [FLATBED integrated 3D model and review status](ship-production.en.md). Integrated-model changes supersede earlier interior dimensional proposals. Unity integration and 4-player control validation remain incomplete.

2026-09-14: [FLATBED interior validation](ship-interior-pipeline.en.md#sequence-and-gates) — actual employee/cargo passage, door/ramp interference, first-person legibility, wiring after reimport, multiple processes and 4-player passing. All remain future checks; documentation checks do not substitute for them.

2026-09-15 latest: [Structure trial 05](ship-interior-trial.en.md). Interior-first orbital post office: central inspection, left sealed lockers, right dispatch desk and rear folded seats. Devices are structural mockups; human spatial review and exterior art remain pending.


2026-09-16: [HTML layout study: 18 spaces, loops and emergency exit](map-study.en.md). Unity integration and human feel testing remain pending. Ship exterior production is deferred.


2026-09-16 / MAP-STUDY-03: Added west loading yard, east service yard, southern outdoor route and inside-only exit to HTML. Connectivity, delivery cycle and outdoor return automation passed; browser visuals inspected. Unity, online and human fun testing not performed. [MAP-STUDY-03](map-study.en.md).


2026-09-16: [MAP-STUDY-04](map-study.en.md) — Exterior expansion 04: enlarged the proposed overall extent to 120×96m. Preserved interior coordinates and widened the west antenna area, east service yard, south freight yard and fuel equipment area. Exterior obstacles allow movement around multiple sides. Antenna/fuel areas are currently labels and collision obstacles, with no new interactions. Automated travel to 5 additional exterior destinations and existing delivery/exit/return checks passed. Browser rendering confirmed with zero console errors. First-person feel, danger balance and Unity integration remain unverified.


2026-09-16 [MAP-STUDY-05](map-study.en.md): Added the northern exterior maintenance area to complete a four-sided perimeter loop. Proposed extent: 120×112m. Preserve separation between interior and exterior, connected only through the existing west entrance and east emergency door. Full exterior circuit and existing delivery/door automated checks passed; browser rendering and zero console errors confirmed. Unity integration and human feel remain unverified.


2026-09-16: [EXIT-RELOCATION](map-study.en.md). Moved the emergency exit to the right wall of receiving at the user-marked location. Closed the former cooling exit. Inside-only E unlock remains. Automated delivery/receipt/return, new gate unlock, old exit blockage and perimeter circuit checks passed. No Unity changes; human feel unverified.


2026-09-16: [Concept-based interior/exterior zoning](concept-zoning.en.md) / CONCEPT-ZONING-01.


2026-09-17: [CONCEPT-ZONING-02](concept-zoning.en.md). Added three proposed loops and connecting paths: A warehouse, B central freight obstacles, C east service block. Proposed one Listener per zone (three total), with separate lure-player markers. Each loop has at least two escape connections; purple dashes represent proposed traversable routes. Evaluate one employee drawing pursuit while others carry along the opposite side. Infinite kiting, player-count scaling, hearing/pursuit reset/speed and cargo clearance remain undecided. Only HTML visualization changed; no AI, Unity or existing playable-study integration. Browser rendering of loops and zone markers inspected. Cooperative fun and actual pursuit remain unverified.


2026-09-17: [CINDER-BLOCKOUT-01 — Unity primitive layout trial](cinder-blockout.en.md). A 108×86.4m validation scale, 5 enterable buildings, 3 loops and reused ship interior trial 05. Separate single-player spatial experiment; enemy pursuit, networking and delivery settlement are not connected. 41 physical passage checks and Editor rendering inspected; Windows build succeeded. Human controls, cargo clearance and cooperative enjoyment remain unverified.

2026-09-17 / WORLD-01: [Narrative consistency review](world-setting.en.md). Reviewed bilingual documents, links, checkbox parity and compatibility with existing delivery/suppression rules. User approval and human curiosity remain unverified. Future playtests should examine whether deliveries remain completable without clues, helping residents feels meaningful, and players can distinguish ordinary deliveries from anomalies. No code/scene changes or game tests.

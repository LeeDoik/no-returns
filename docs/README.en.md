# NO RETURNS documentation

[한국어](README.md)

## Current implementation — personal slots and medicine 0.9.25

Implemented 1 co-op/2 solo slots, pre-departure 40 CR medicine purchase/equipping, field swapping and self/teammate full healing. New Cinder sessions start with 400 CR. Retain unused equipped doses within the session; treatment does not reset downs. Game 0.9.25 / protocol 16 / TCP 27842. [Current specification, production and validation scope](current/personal-equipment.en.md).

## Current implementation — multiple deliveries 0.9.24

Implemented 3–6 physical parcels by crew count, independent carrying, verification pause/resume, receipts and district bundles. Game 0.9.24 / protocol 15 / TCP 27842. This supersedes the single 420 CR delivery for normal Cinder sessions. [Current rules, production evidence and remaining work](current/multiple-deliveries.en.md).

## 2026-10-05 — visit ledger, revival and departure 0.9.23

Cinder receipts record 420 CR as unbanked earnings, paid only on departure. A living occupant spends 100 unbanked CR to revive one eliminated teammate after 10 seconds. Anyone can start/cancel a 12-second departure; deduct 100 CR per abandoned teammate with a zero floor. Integrate health, repeat downs/elimination and all-eliminated/liftoff during revival. [Implementation, values and limits](current/visit-settlement.en.md), [validation](validation/visit-settlement-0.9.23.json). Game 0.9.23, protocol 14, TCP 27842. Supersedes historical Cinder all-aboard return, immediate settlement and one-hit down rules in this scope. New tools, multiple deliveries, Cinder saves/reconnects, final danger timing/audio and human review remain.

## 2026-10-05 — consolidated design NR-COOP-01

[Cooperative design v1](current/coop-design-v1.en.md) governs target behavior for new work. It separates user selections from delegated completions and initial tuning. It overrides conflicting historical design below without promoting code or validation to implemented status. Documentation only; game version unchanged.

## 2026-10-04 — Companion rescue demonstration and replay 0.9.22

The manual-test companion first approaches and falls nearby; hold use (default **E**) for **2.5s** to revive it. It then runs the existing **32s** movement/jump/baton/parcel sequence before returning to rescue practice. In practice mode only, the host's reset-lab binding (default **R**) prepares another rescue demonstration without resetting the room/player position. [Usage and limits](current/companion-play.en.md), [validation](validation/companion-rescue-0.9.22.json). Game **0.9.22**, protocol **13**, TCP **27842**. Supersedes previous default-companion down/rescue-not-included statements. Stop the previous manual session; open new manual windows only upon a hands-on test request.

## 2026-10-04 — Fall and get-up motion 0.9.21

Replace the teammate's instant 90° tilt with a **0.65s** knee buckle/side collapse and **0.95s** roll upright/knee extension. Layer the pose over the existing Humanoid; preserve rescue, movement, collision and input rules. [Production and remaining scope](current/employee-animation.en.md), [validation](validation/employee-down-0.9.21.json). Game **0.9.21**, protocol **13**, TCP **27842**. This supersedes earlier missing-down/get-up statements within this procedural presentation scope. Dedicated motion-capture clips, ragdolls, actual rescue contact, human naturalness and slope/wall intersection correction remain pending.

## 2026-10-04 — Remote full-body rescue pose 0.9.20

Show rescuing teammates bending their knees, leaning forward and extending both hands. Blend out in about 0.167s after cancellation/completion; clear on carrying, down or inactivity. Preserve existing 2m/line-of-sight, 2.5s rescue and 4s protection rules. [Production and limits](current/first-person-arms.en.md), [validation](validation/employee-rescue-remote-0.9.20.json). Game **0.9.20**, protocol **13**, TCP **27842**. This supersedes earlier missing-full-body-rescue statements within this presentation scope. Dedicated down/get-up animation, target auto-facing/actual body contact and human quality remain pending.

## 2026-10-04 — Targeted quick tests 0.9.19

Finish visual changes with compilation, relevant Editor poses/render and docs. For carry/rescue/attack changes, run only the affected windowless two-client scenario from a prepared state. Reserve full delivery/four-player checks for large changes, related failures, pre-release or explicit requests. Three scenarios reuse the same two processes: this quick suite passed **18 checks in 8.869 seconds**, including startup/shutdown; a build-inclusive run took **17.735 seconds**. These are measurements on this Mac and do not replace full regression. Reject stale builds before launch. [Usage and selection](current/quick-testing.en.md), [validation](validation/quick-testing-0.9.19.json). No manual windows opened.

## 2026-10-04 — Teammate two-hand beacon carry 0.9.18

Show teammates supporting the beacon with both hands during idle/walk. Follow body orientation and limit wrist bend to 25°. Clear the pose on release, down or rescue. Preserve first-person presentation and host purchasing/position/placement/charge rules. [Current production and limits](current/first-person-arms.en.md), [validation](validation/employee-beacon-remote-0.9.18.json). Game **0.9.18**, protocol **13**, TCP **27842**. This supersedes previous missing-remote-beacon-pose statements within this scope. Full-body rescue/down, actual teammate contact and human quality remain pending. Open manual test windows only on request.

## 2026-10-04 — Two-hand beacon carry 0.9.17

Implement a first-person pose supporting the existing beacon with both hands and following view rotation. Limit wrist bend to 25°. Releasing restores world presentation/collision and, in normal play, the baton. Preserve host purchasing/position/placement/charge adjudication. [Current production and limits](current/first-person-arms.en.md), [validation record](validation/employee-beacon-0.9.17.json). Game **0.9.17**, protocol **13**, TCP **27842**. This supersedes older missing/hidden beacon-hand statements below within this scope. Remote beacon carry, full-body rescue/down, actual teammate body contact and human quality review remain pending. Open manual test windows only on request.

## 2026-10-04 — First-person rescue hands 0.9.16

Reach with both hands and show a small assisting movement during valid rescue. Hide the baton, restore the right-hand baton on cancellation/completion and hide hands when down. Retain the 25° wrist limit. [Current production, values and remaining scope](current/first-person-arms.en.md), [validation record](validation/employee-rescue-0.9.16.json). Game **0.9.16**, protocol **13**, TCP **27842**. Preserve existing rescue adjudication. This supersedes older statements below that rescue hands are missing or hidden, within this scope. Beacon hands, full-body rescue/down, actual body contact and human quality review remain pending. Open manual test windows only on request.

## 2026-10-04 — First-person wrist correction 0.9.15

Fix the wrist bend reported by the user in the 0.9.14 attack preview. Correct the relationship between the palm and cylindrical grip axes; move the elbow outward so the forearm and hand align. Limit the hand/forearm direction angle to 25° and solve grip contact again. Keep the baton upright during forward movement and retract the shoulder origin near walls. [Current pose/production](current/first-person-arms.en.md), [validation record](validation/employee-wrist-0.9.15.json). Game **0.9.15**, protocol **13**, TCP **27842**. The previous pose has a user-confirmed quality issue; the revised pose has not received user approval. Manual play stays stopped.

## 2026-10-04 — Dynamic baton and first-person hands/arms 0.9.14

At the user's request, implement short anticipation, fast extension and 0.42s recovery through baton arm/chest motion, local right glove/sleeve and two-hand parcel carrying. Preserve immediate contact, 6s cooldown, carrying/down/valid-rescue blocking and existing movement. [Current baton](current/employee-baton.en.md), [arm production/local reproduction](current/first-person-arms.en.md), [validation evidence](validation/employee-first-person-0.9.14.json). Game **0.9.14**, protocol **13**, TCP **27842**. Earlier 0.9.13 original-motion restoration and first-person-arms-not-implemented/proposed states below are historical and superseded within this implementation scope. Dedicated beacon/rescue hands, human quality, actual game GPU composition, all-corner penetration, Windows/LAN and performance remain pending. Manual play stays stopped; use the existing two-window helper only on request.

[Test-window layout 0.9.11](current/companion-play.en.md): automatic two-player left/right 16:9 and four-player 2×2 tiling. Allow manual resizing.

[Original baton restored at 0.9.13](current/employee-baton.en.md), [backward/sidestep and jump/landing](current/employee-locomotion.en.md). Separate implementation/automated validation from human quality approval. Open manual windows only on request.

[Two-handed parcel carrying 0.9.8](current/employee-animation.en.md): apply 0.8m default reach and hand-contact posing. Pass Mac build, 120 static pose samples and windowless carry/companion checks. Distant contact and human quality remain incomplete. Open manual windows only on request.

## 2026-10-03 — On-demand human + automatic companion 0.9.7

[Current launch, behavior and validation guide](current/companion-play.en.md). Open one human host and one automatic companion client only when the user says **“직접 테스트 해볼게”** (I will test it myself). Default to safe practice in the current map: repeat idle, walk/sidestep, slow movement, jump, baton and nearby parcel carry/drop on a 32-second cycle; follow the human when distant. Game 0.9.7, protocol 13, TCP 27842. Mac build with zero errors/9 warnings and 5 grouped checks in two actual windowless processes passed. Do not open manual windows this task; inspect focus/mouse and human visual quality after the request. Complex maze/multilevel routes, Windows/performance and rerunning the full 110-check regression are outside this validation scope.

## 2026-10-03 — Right-hand baton 0.9.6

See the [current employee/baton guide](current/employee-animation.en.md) and [tools/Blender MCP setup](current/macos-development.en.md). Attach the baton to the actual right hand and apply a finger grip. Hide it on every peer while carrying a parcel/beacon, restore it after dropping/placing, and retain down/rescue hiding. Version 0.9.6, protocol 13, TCP 27842, Unity 6000.6.4f1. [Actual validation scope](validation/baton-hand-0.9.6.json). Carrying contact, dedicated full-body attacks, first-person arms, user quality/all-frame penetration, other PCs/Windows/performance remain incomplete.

## 2026-10-03 — Employee full body and idle/walk 0.9.5

[Current integration, local reproduction and validation scope](current/employee-animation.en.md). Import corrected Idle and Walking through separate Humanoid Avatars, connect the full employee body and switch motions from actual movement speed. Each window hides its own body and shows three teammates with team tints. Game 0.9.5, protocol 13, TCP 27842. Keep source motions/generated Unity assets local; publish reproduction code, validation records and static images. Supersede earlier Unity/idle-walk-not-integrated statements within this scope. Carrying hand contact, dedicated additional motions, first-person arms and user quality/other-environment review remain pending.

## 2026-10-02 — Cinder suppression and outer creature 0.9.4

[Current rules, production and running](current/cinder-suppression.en.md). Connect existing 90/135/180-second stages, signal audio and relative work-light dimming to 4 current suppressors and the boundary. Connect east entry, obstacle routing and pursuit for the outer creature 8 seconds after shutdown. Preserve sky/fog/sun, source art and collision. Default launch is delivery + Listener + suppression/outer; retain `--delivery-only`/`--map-only` regression. Version 0.9.4, protocol 13, TCP 27842. Pass 60 actual four-process checks for purchased-beacon distraction, hazardous return, shared down/ship safety, emergency recovery and next shift. [Validation record](validation/cinder-suppression-0.9.4.json). Supersede older unconnected-suppression/outer statements below within this scope. Human quality, final creatures/audio, clues/progression saving and other-environment/performance checks remain pending.

## 2026-10-02 — Repository and local file cleanup

[Cleanup scope and retention policy](current/repo-hygiene.en.md). Remove 51 unreferenced Unity review copies/duplicate Smart Wall files (44,900,041 bytes), 115,418,532 bytes of stale local tests/logs/caches and 246 LFS cached objects (approximately 122MB reported). Preserve production sources, current assets, historical documents/commits and 8 existing ship material edits. Retain game 0.9.3 / protocol 12. Pass dependencies across all Unity Assets, LFS HEAD integrity and post-cleanup Mac build with 0 errors / 4 warnings. Mark 142 historical local-evidence links as unretained provenance; all 272 documents pass link/language/checkbox checks. [Per-file evidence](validation/repo-cleanup-2026-10-02.json).

## 2026-10-02 — Cinder Listener, baton and rescue 0.9.3

[Current rules, running and production checks](current/cinder-listener.en.md). Connect one Listener's actual floor/obstacle grid movement, noise investigation, warning/attack, existing empty-hand left-click baton, hold-E rescue and all-down ship recovery to default Cinder delivery. Preserve normal 420 CR payment and source art scene. Judge safety using current Cinder ship coordinates; do not instantiate old suppression/outer/clue/save objects. Existing beacon pulses also attract Listener investigation. Keep peaceful delivery regression via `--delivery-only` and movement tests via `--map-only`. Version 0.9.3, protocol 12, TCP 27842. Supersede older unconnected-Cinder-Listener/baton/rescue statements below only within this scope. Suppression/outer creature, clues, progression saving and human quality remain pending. [Actual validation scope](validation/cinder-listener-0.9.3.json).

## 2026-10-01 — Controls, ship and purchase UI 0.9.2

[Current controls and running](current/controls-ui.en.md). E targeted use/ship terminal, left-click ground placement, hold right click to rotate parcel, wheel 0.75–1.6m reach, Q immediate release. Separate departure/return/purchase into native buttons showing boarding count, wallet, disabled reasons and zero-pay return confirmation. Esc settings provide 13 button rebindings, sensitivity, FOV, language/defaults. Menus block gameplay inputs while the world continues. Connect Cinder 120 CR beacon purchase/shared carrying/two 8-second signals. Version 0.9.2, protocol 11, TCP 27842. Preserve source map/delivery pay. Supersede E automatic progression/Esc shop and unconnected-Cinder-beacon statements below within this scope. Listener/baton/suppression/save integration and human quality assessment remain. [Actual validation scope](validation/controls-ui-0.9.2.json).

[Current Cinder delivery, receipt and return settlement](current/four-player.en.md#2026-10-01--cinder-delivery-receipt-and-return-settlement): connect default four-window launch to delivery testing. Ship E preparation/arrival → BAY 04 floor acceptance → receipt E collection → all-aboard return/420 CR settlement → next shift. Preserve the source art scene. Retain movement testing with `start --map-only`/`check`. Listener/baton/suppression/beacon/save and four-human/other-PC/performance validation remain. [Actual validation scope](validation/cinder-delivery-01.json).

[Current Cinder four-player test environment](current/four-player.en.md#2026-10-01--cinder-four-player-map-test-environment): start/stop 1 host and 3 Mac client windows for movement/shared-parcel carrying in the current map. **Mac build: 0 errors/7 existing warnings; pass 13 actual four-process checks, 7 existing rescue-rule conditions and 7 regular-mode defaults. Inspect 2 native 800×500 Korean HUD captures for crew 4/4, E/Q controls, reticle and team colors; also verify 4 manual processes/3 connections and shutdown.** Full Cinder delivery/Listener/baton integration and four-human/other-PC/performance validation remain pending.

[Current open sky and zone boundaries](current/cinder-compact-site.en.md#2026-10-01--open-sky-and-zone-boundaries): open 82.8m² of west/north cover and adjust matching ceiling collision. Distinguish interior paving/exterior rocks with boundary bands/signs, adding 4 suppressor service pads, aged visuals and irregular rock placement. Pass 6,864 carrying poses, 94 actual carrying segments and rendering. User appearance, human four-player/performance and gameplay integration remain pending.

[Current rocky territory/industrial background](current/cinder-compact-site.en.md#2026-10-01--rocky-territory-and-industrial-background-outside-the-field): place 59 rocks, 9 industrial visuals and 1 terrain surface outside the same scene. Hide gray guard visuals while retaining fall-prevention collision, map and lighting. Native terrain rendering, 6,864 carrying poses and 94 actual carrying segments pass. Background quality, gameplay integration and human four-player/performance checks remain.

[Current varied building forms](current/cinder-compact-site.en.md#2026-10-01--varied-building-outlines-and-structural-modules): produce/place 9 structural module types and 11 native meshes. Change silhouettes with a central 8-sided utility building, northern stepped outline, warehouse sawtooth/storage vault roofs, upper rooms, control tower, stacks and canopies. Pass 94 movement segments, 17 four-body lanes and 6,864 carrying poses. Appearance quality and human four-player review remain.

[Current sky and distant atmosphere](current/cinder-compact-site.en.md#2026-10-01--sky-and-distant-atmosphere): apply a static mauve sky, 35–115m fog and reduced ambient/directional light in the same scene. Retain 40 local lights, map placement, collision and runtime. Inspect 18 actual captures and carrying checks; user sky-quality/final danger-signal readability review remains pending.

[Current compact map and prop placement](current/cinder-compact-site.en.md). The user approved the whole-site direction. Add 47 groups of racks, pallets, desks, cabinets, drums, pipes, roof equipment and labels in the same `CinderCompactSiteReview`. Preserve 3/3.15m alleys, 4 loops, 94 movement segments and 17 four-body lanes. Passed 5,904 carrying poses and actual movement. Prop quality and human four-player review remain.

Existing Listener, baton and ship gameplay code/scenes are preserved. See [existing gameplay versus Cinder review scenes](current/cinder-asset-prep.en.md#location-of-existing-gameplay-systems). Gameplay integration into Cinder remains incomplete.

2026-09-12 · SPACE-01. The current direction is PSX space-delivery mystery. These documents supersede previous plans.

1. [Overview and product design](current/01-overview.en.md)
2. [Current game specification](current/02-spec.en.md)
3. [Map, art and technical guide](current/03-guides.en.md)
4. [Open decisions, work and priorities](current/04-backlog.en.md)
5. [Testing and release checklist](current/05-validation.en.md)
6. [Historical archive](archive/README.en.md)

[Open project](../01_Open_Project.cmd) · [Documentation rules](../AGENTS.md)

[macOS development environment and Unity CLI](current/macos-development.en.md)

[Pay, failure and shop design — test proposal](current/economy.en.md)

[Executable carrying experiment — controls and current validation](current/carry-test.en.md)

[Listener/rescue experiment — launch/current validation](current/space-play-03.en.md)

[Supply/risk contract experiment](current/space-play-04.en.md)

[Host progression save](current/space-play-05.en.md)

[Site clues/shared log](current/space-play-06.en.md)

[SPACE-PLAY-07 — expanded map/suppression](current/space-play-07.en.md)

[Independent review integration — 0.8.2](current/review-fixes.en.md)

[Current 0.9.0 — four-player launch and validation](current/four-player.en.md)

[0.9.2 — cooperative HUD and menus](current/crew-hud.en.md)

[One-cycle demo and 3D asset list](current/demo-art-list.en.md)

- [FLATBED 3D production/review candidate](current/ship-production.en.md): Tripo source, Blender integration, exports and validation status.

- [CINDER DEPOT HTML / MAP-STUDY-05](current/map-study.en.md)

- [Concept-based interior/exterior zoning](current/concept-zoning.en.md)

[Unity primitive map trial — launch, scale and validation](current/cinder-blockout.en.md)
[World-setting proposal — Earth, underground settlements and survival logistics](current/world-setting.en.md)
[Recruitment-ad trailer — reference analysis and proposed 75-second script](current/trailer-recruitment.en.md)

[HyperFrames video setup, commands and validation](current/hyperframes.en.md)

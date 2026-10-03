# NO RETURNS documentation

[한국어](README.md)

[Test-window layout 0.9.11](current/companion-play.en.md): automatic two-player left/right 16:9 and four-player 2×2 tiling. Allow manual resizing.

[Upright baton grip/forward pulse 0.9.12](current/employee-baton.en.md): restore the earlier upright grip and extend/retract once. Add [jump/landing](current/employee-locomotion.en.md). Revised human quality remains unverified. Open manual windows only on request.

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

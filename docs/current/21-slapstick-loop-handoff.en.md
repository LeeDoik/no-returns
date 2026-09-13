# NO RETURNS — NR-LOOP-01 Small Delivery Loop Development Handoff

[한국어](21-slapstick-loop-handoff.ko.md)

> **Historical record · retired/superseded 2026-09-12.** The [PSX space-delivery design](01-overview.en.md) is current. Implementation, launch, art and values below are historical, not current status. Deleted files remain path records only.


[Actual implementation and validation](22-slapstick-loop.en.md). The following is the original handoff requirements record. Consult the result document for implementation status and remaining human validation.

2026-09-12 · Revision 1 · Experimental implementation handoff requested by user / implementation and fun validation pending

## Goal and baseline

Test “take on cargo you cannot handle because the money looks tempting” through play. Connect cheap deliveries → trolley purchase → expensive sneezing deliveries → settlement/retry in an 8–12-minute experiment. Duration is an observation target, not a deadline. Values are initial experiment settings, not release balance.

The user selected valuable-cargo progression, reinvestment and absurd comedy, and requested continuation through this small scope. Consult [discussion](20-player-goals.en.md) and comedy concept (`../../artifacts/concepts/no-returns-slapstick-v2.png`; retired file), but do not implement its giant walking cargo, scanner or final art. Where the older Manyfast PRD conflicts, use this document for this experiment and distinguish that from a full-product specification change.

## Scope

Implement one fixed map, ordinary/sneezing cargo, one trolley, one shop, two destinations, shared balance, settlement and restart. Connect the entire loop with 1 player, then complete 2-player direct-connect cooperation. Solo completion does not validate cooperative fun. Retain the online product direction, but exclude Steam invitations, lobbies, matchmaking, host migration, joining mid-run and persistent saving.

Exclude region unlocks, procedural maps, additional equipment, cargo damage/quality bonuses, combat, health/death, mandatory quotas, fines, full ragdolls and final art. Use temporary worker shapes with deadpan faces and parcel faces, warning motion and sound to communicate causes. Existing approved assets may be reused; new art production must not block a playable loop.

## Play sequence and economy

| Stage | Action and condition | Money and next state |
|---|---|---|
| Start | Two ordinary parcels beside the truck; show trolley price as goal | Balance 0, trolley 60 |
| Basic deliveries | Deliver one ordinary parcel each to A and B | 30 each, total 60 |
| Investment | Purchase trolley at truck shop; unlock previewed premium orders | Spend 60, balance 0; spawn trolley immediately |
| Premium deliveries | Dispatch sneezers individually; deliver to A then B | 90 each, balance 180 after both |
| End | Return to truck for settlement after both premium deliveries | Revenue 240, investment 60, balance 180; retry/exit |

Dispatch the next premium parcel only after confirming the previous delivery. Preview premium payout and sneeze behavior before purchase. Purchase once; insufficient funds and duplicate input do not deduct money. Delivery requires matching cargo ID/destination and an unheld parcel inside the receiving area for 1 second, then confirms exactly once. Cargo loaded on the trolley must also be deliverable by entering the area. Wrong deliveries retain the item and show its correct destination. After payment, deactivate cargo and clean up ownership and attachment.

Early settlement lists unfinished deliveries and ends the run without revoking confirmed earnings within that run. Restart begins a new experiment, resetting both players, cargo, equipment, balance and timers. There is no persistence after process exit; state this in the menu. Do not describe this as implementing the previously discussed permanent company save.

## Map and choices

Place the truck centrally, connecting a broad detour and narrow alley to A. Put B in a small yard with a low step. The trolley must actually traverse the detour and reach both destinations. Avoid deep drops, mazes and lengthy round trips. Tune widths and distances against actual trolley dimensions, turning and handling speed.

The alley makes trolley turning awkward but permits hand-carrying and handoffs. The broad path is reliable. Let players choose where the sneeze faces, whether to set cargo down and dodge, or unload for a shortcut handoff. Premium cargo remains hand-carryable so an overturned trolley cannot block progress. Use dropping and catching rather than adding simultaneous lifting.

## Trolley and sneeze

The trolley holds one parcel. Approach its handle and use E to push/release; use E at the loading point to load/unload. Context prompts must resolve conflicts with grabbing. Use a simple Rigidbody chassis and bounded connection forces, excluding wheel-suspension simulation. Keep loaded cargo attached with world collisions intact. The trolley should move more reliably than hand carrying; measure actual time savings. It does not prevent sneezes.

Activate sneezing on the first pickup or loading after dispatch. Select subsequent intervals of 8–12 seconds from a seed; precede each forward blast with 1.2 seconds of face inflation and inhaling. Continue while placed on the floor before delivery; dropping during the warning cannot reset the timer. Undispatched and completed parcels do not react. Use a fixed seed first; retries offer same/new seed.

Apply one impulse per worker, dynamic cargo or trolley within a forward 3m cone with half-angle 45 degrees, checking wall occlusion. Initial target velocity change is horizontal 3m/s and upward 1.5m/s per target, adjustable in Inspector. Workers accumulate and damp separate external velocity while respecting collisions; do not fully lock input. Hit workers release hand-held cargo; trolley attachments remain. Do not repeatedly hit the sneezer itself. Avoid excessive camera shake or unlimited chain firing.

Recover out-of-bounds or stuck cargo/trolleys from the truck. Restore the existing object to its initial location, clean velocities/ownership/attachments and preserve payment/contract state. Worker rescue returns only the worker and releases carried cargo at the scene. Never automatically deliver cargo or grant money.

## Cooperation and UI

Use 2-player host/client direct connection with participation complete before starting. Host resolves contracts, wallet, purchase, physics, sneeze seed and deliveries; clients request actions. Each parcel has one owner. Concurrent purchases deduct/spawn once. Clients may request purchases and recovery. Host announces and confirms early settlement/restart for both. On client departure, release ownership and continue solo on host; on host departure, explain and return to menu. Use a new run instead of reconnection in this experiment.

The main developer selects and documents the smallest direct-connect implementation after checking current Unity support and the existing environment. Do not duplicate solo physics/payment rules. If networking is blocked, provide the solo build while leaving the cooperation gate incomplete.

HUD contains current goal, shared balance, next purchase cost, completed premium deliveries and interaction hints. Parcels show destination, payout and sneeze warning. English is the game-copy source with Korean counterparts in documentation: `Buy trolley — 60` / 수레 구매 — 60, `Premium delivery — 90` / 고가 배송 — 90, `Wrong address` / 잘못된 목적지, `Delivered +90` / 배송 완료 +90, `Restart clears this run` / 재시작 시 이번 실행 초기화. Connect goals to visible cargo rather than large explanatory popups.

## Code evidence and development sequence

Read [baseline](18-unity-mainline.en.md) and [controls](15-unity-controls.en.md). Unity project: `unity/NoReturns`. Confirmed code: `Assets/_Project/Runtime/WorkerController.cs`, `CarryMotor.cs`, `CargoBody.cs`, `ThirdPersonCamera.cs`, `CarryHud.cs`; tests: `Assets/_Project/Editor/CarryLabTests.cs`.

WorkerController directly polls keyboard/mouse, overwrites horizontal velocity and resets all cargo with R. Separate input ownership, external forces and experiment restart. CargoBody fall recovery also needs delivery-state integration. Preserve the original CarryLab scene and behavior; avoid broad refactoring or restoring SIDE EFFECTS.

Proposed new paths: `Assets/_Project/SlapstickLoop/Scenes/SlapstickLoop.unity`, `Runtime/LoopRules.cs` (phase/wallet/deduplication), `DeliveryPoint.cs` (receipt), `SneezeCargo.cs` (warning/blast), `TrolleyController.cs` (pushing/loading), `LoopHud.cs`, `LoopSession.cs` (participation/authority), `Editor/LoopBuilder.cs`, `Editor/LoopTests.cs`. These are intended implementation targets, not existing files. The sequence below is a development handoff; detailed code and network-package selection belong in the main task using current APIs.

- [ ] A: Connect two ordinary deliveries, wallet, purchase, two premium deliveries, settlement and restart in a separate scene. Automatically check duplicate delivery/purchase and the balance sequence.
- [ ] B: Connect trolley, warnings, blasts and recovery. Check wall occlusion, dropping during warnings, no post-completion firing, and external forces surviving movement input.
- [ ] C: Connect host/client ownership and shared payment. Check concurrent grabbing/purchasing/receipt, departures and leftover timers/objects after restart. Separate solo and 2-player results.
- [ ] D: Run CarryLab regressions and new checks; provide a separate Windows build, controls guide and launcher. Preserve `06_Unity_Play.cmd` and add a separate entry. Verify delivery→purchase→premium delivery→settlement in the built executable.
- [ ] E: Observe human play and revise or hold using the criteria below. Update experimental values in both languages together.

Existing regression command: `powershell -NoProfile -File tools/unity.ps1 test`. Documentation check: `python tools/check_docs.py`. Add a separate test entry for this experiment and record exact commands, logs and exit codes in results. Existing tests do not validate the new economy/cooperation. Touch only relevant files and preserve other working-tree changes.

## Fun validation and completion

When available, use two different 2-player teams. Minimize explanation on the first run, then repeat the same seed to observe learning. A new seed is also available. This small sample does not establish release-quality fun.

Record time to first purchase, travel/recovery time and payout per delivery, explanation of the first incident, changed plans/roles due to a teammate, control recovery time, and voluntary reasons to replay. Do not judge only by laughter counts.

Proceed when the team understands the next purchase goal, understands and recovers from at least one incident using another approach, and describes the purchase benefit and new premium-delivery problem. Hold when warnings feel unreadable/unfair, the trolley has no perceived benefit, one player repeatedly waits, or premium delivery feels like slower cheap delivery. Adjust warnings, impulses, trolley and routes before adding maps/equipment.

Report automated rules, built-executable completion, separate-PC connection and human fun validation independently. If people or hardware are unavailable, leave those checks pending and hand off the playable build with an observation form. All implementation/play items are currently incomplete; writing this document does not validate them.

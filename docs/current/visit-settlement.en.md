# Cinder visit ledger, revival and extraction — 0.9.23

[한국어](visit-settlement.ko.md)

## Current implementation — multiple deliveries 0.9.24

Implemented 3–6 physical parcels by crew count, independent carrying, verification pause/resume, receipts and district bundles. Game 0.9.24 / protocol 15 / TCP 27842. This supersedes the single 420 CR delivery for normal Cinder sessions. [Current rules, production evidence and remaining work](multiple-deliveries.en.md).

2026-10-05 · Implementation baseline: game 0.9.23 / protocol 14 / TCP 27842

## Implemented scope

The first implementation step of the [consolidated design](coop-design-v1.en.md). New visit state applies to Cinder delivery/hazard modes only. Preserve legacy laboratory behavior and safe companion practice rescue. This does not complete the full game design.

- Collecting a receipt records **420 CR** as unbanked earnings. Preserve the previous 300+120 total without a separate return bonus. Duplicate collection cannot increase the ledger. Existing wallet funds stay unchanged until departure settlement.
- Any living occupant selects an eliminated teammate at the ship terminal, immediately spending **100 unbanked CR** for a **10-second** revival aboard. Reject insufficient balance, concurrent revival and invalid targets. Anyone may operate it; one revival runs at a time. Existing savings cannot fund revival.
- Anyone can start/cancel manual departure. After **12 seconds**, settle using occupancy at liftoff. Keep the existing two-click confirmation for leaving without a receipt. Deduct **100 CR** per abandoned teammate, with a **0 CR** floor. No occupants or valid pending revival means failure and **0 CR**.
- A running revival survives elimination of the last living player and counts as an occupant at departure. Do not charge both revival and return fees for that person. Downed occupants count aboard and are rescued at low health. Move employees aboard after departure.
- Maximum health **100**, current creature hit **60**. First down allows rescue for **45 seconds**, restoring **30** health and **4 seconds** protection. Second down or bleedout eliminates. Keep the existing **2.5-second** rescue hold for this change (different from the design's initial 3 seconds). Ship revival resets health and down count. Survivor health persists next landing; only down count resets.
- Solo has **2** self-revives per landing: after **3 seconds** down, hold use for **2 seconds**. Restore **30** health and **4 seconds** protection; another down after exhaustion fails the visit. In co-op, all down without a running revival fails the visit.
- The ledger also implements an automatic warning at **17 minutes** and forced departure at **18 minutes**. Currently communicated with HUD copy without exact remaining departure seconds. Broadcast/horn/engine presentation and fog/controller retiming remain **unimplemented**. Existing danger stages at 90/135/180 seconds and outer entry at 188 seconds remain. This is therefore not a balanced 15–20-minute visit yet.

## Presentation and use

Open the terminal with use (default E) aboard ship. While landed, the right side shows revival buttons per employee and insufficient-funds/progress feedback. The left side shows occupancy, wallet, unbanked earnings and departure/cancel. After return, display gross rewards, revival deductions, return deductions and actual payment. Add health/unbanked earnings to the HUD and distinguish eliminated/rescuable states. English and Korean copy are provided. Safe companion practice does not validate delivery/elimination/paid revival.

## Source evidence

- [Visit state and values](../../NoReturns/Assets/_NoReturns/Runtime/CarryLab/CarryVisit.cs): health/downs/revival, reserved fees, departure and payout.
- [Mission ledger](../../NoReturns/Assets/_NoReturns/Runtime/CarryLab/CarryMission.cs): one-time receipt credit and wallet settlement.
- [Room integration](../../NoReturns/Assets/_NoReturns/Runtime/CarryLab/CarryRoom.cs), [threat/rescue](../../NoReturns/Assets/_NoReturns/Runtime/CarryLab/CarryThreat.cs), [terminal](../../NoReturns/Assets/_NoReturns/Runtime/CarryLab/CarryRoom.Controls.cs), [wire state](../../NoReturns/Assets/_NoReturns/Runtime/CarryLab/CarryWire.cs).
- [Editor rules review](../../NoReturns/Assets/_NoReturns/Editor/VisitRulesReview.cs), [real two-player visit test](../../tools/test_cinder_visit.py), [existing rescue test](../../tools/cinder_quick_test.py), [four-player physical delivery test](../../tools/test_cinder_delivery.py).

## Validation and limits

Results are stored in the [receipt](../validation/visit-settlement-0.9.23.json). The first rescue run exposed a HUD exception reading an empty visit snapshot immediately after joining. Add array validity checks and rerun. Consecutive client launches also required waiting for TCP port release; no unrelated active session was forcibly terminated.

Version 0.9.23 integrated the single delivery. Multiple contracts and district bonuses are superseded by the current 0.9.24 guide. Five tools/medicine/cart/dragging/spectating/voice/ship intrusion remain design work. Consumable retention will be implemented with personal inventory. Existing shared beacon ownership, purchasing authority and charges remain unchanged. Cinder progression is still session memory; save/resume/reconnect/migration are unimplemented. Preserve existing visit failure on participant disconnection. Human handling/readability/fun, real-time 18-minute departure, Windows/LAN/performance remain unverified.

Official CLI queries confirmed stable Unity 6000.6.4f1, CLI 1.0.0-beta.12 and Pipeline 0.8.0-exp.1 as current. Reconnect after the initial Editor instance exited, then run checks. No model/material/Blender changes. No manual play windows opened.

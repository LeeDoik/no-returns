# Multiple deliveries and district bundles — 0.9.24

[한국어](multiple-deliveries.ko.md)

2026-10-05 · Game 0.9.24 / protocol 15 / TCP 27842.

## Implemented rules

This is the next implementation slice of [NR-COOP-01](coop-design-v1.en.md), integrated with the [visit ledger](visit-settlement.en.md). Normal Cinder sessions use several physical parcels instead of the previous single 420 CR delivery. Safe companion practice and legacy quick fixtures keep their single training parcel.

Load 3/4/5/6 parcels aboard for 1/2/3/4 crew. Update the manifest while preparing; lock it when landing. Each employee carries at most one parcel, and different employees can carry different parcels simultaneously. Use the same pickup, rotation, reach, placement and drop controls. Read the physical address label or the ship terminal's DELIVERY MANIFEST; choose any subset and order. No exact receiver HUD marker is added. Unfinished parcels incur no extra penalty on departure; teammate return fees still apply.

| Parcel | Address | Base reward |
| --- | --- | ---: |
| NR-01 | WEST / BAY 02 | 100 CR |
| NR-02 | WEST / BAY 02 | 120 CR |
| NR-03 | EAST / BAY 04 | 180 CR |
| NR-04 | NORTH / BAY 07 | 160 CR |
| NR-05 | EAST / BAY 04 | 200 CR |
| NR-06 | NORTH / BAY 07 | 180 CR |

Use the first N entries for the crew count. These are initial implementation values, not validated economy balance. WEST has a 55 CR bundle; EAST has 95 CR when both contracts exist; NORTH has 85 CR when both exist. A district with only one contract has no bonus. Award 25% of that district's base total, rounded down, once with its last receipt. Reveal the bonus only at the ship terminal when 1 or 0 contracts remain. No global bonus announcement.

Place an unheld, settled parcel inside its matching receiver's marked floor area. Verify for 25 seconds, one parcel per receiver at a time; different receivers may operate concurrently. Walking away does not pause verification. Picking the parcel up or moving it outside the area pauses progress and scanner noise without clearing progress. Return it to resume. A paused parcel does not reserve the receiver. Verification emits local looping sound and periodically attracts the Listener. Completion shows a local green light and receipt/chime. Verified parcels stay locked until receipt collection; collecting instantly shares the record and removes that physical parcel. Collecting again cannot pay twice.

All base rewards and bundle rewards enter unbanked earnings. They can fund paid revival and are deposited in the wallet only at departure. Existing failure/abandoned teammate/automatic departure rules remain. Completing two WEST deliveries yields 275 CR before revival/return deductions. Next landing resets parcel state and retains settled wallet funds.

## Source and reproduction

- [Manifest and rules](../../NoReturns/Assets/_NoReturns/Runtime/CarryLab/CarryDeliveries.cs): count, fixed addresses, coordinates, rewards, 25-second verification and bonus.
- [Physical parcels and receiver presentation](../../NoReturns/Assets/_NoReturns/Runtime/CarryLab/CarryRoom.Deliveries.cs): runtime receiver placement, ship cargo, labels, per-player selection and replication.
- [Mission](../../NoReturns/Assets/_NoReturns/Runtime/CarryLab/CarryMission.cs), [host integration](../../NoReturns/Assets/_NoReturns/Runtime/CarryLab/CarryRoom.cs), [terminal UI](../../NoReturns/Assets/_NoReturns/Runtime/CarryLab/CarryRoom.Controls.cs), [feedback](../../NoReturns/Assets/_NoReturns/Runtime/CarryLab/ReceiptFeedback.cs).
- [Editor review](../../NoReturns/Assets/_NoReturns/Editor/DeliveryRulesReview.cs), [physical 4-player test](../../tools/test_cinder_delivery.py): the current test replaces the obsolete single-parcel 420 CR journey. Require a current build, then run `python3 tools/test_cinder_delivery.py --headless`. Do not use historical single-delivery results as evidence for this feature.

## Validation and remaining work

See the [validation receipt](../validation/multiple-deliveries-0.9.24.json). Automated checks and human quality approval are separate. No manual play windows opened. The physical journey isolates delivery with hazard AI disabled; it does not validate combat while scanning, district difficulty or sound readability. Dedicated signed approaches, randomized contracts, extra deliveries, all receiver routes, all six simultaneous cargo arrangements, persistence/reconnects and 1–4-player balance remain incomplete or unverified. Existing danger timing at 90/135/180/188 seconds has not yet been aligned to the 17/18-minute departure schedule. New equipment is not implemented in this change.

Official checks this work session found Unity 6000.6.4f1, CLI 1.0.0-beta.12 and Pipeline 0.8.0-exp.1 current. Preserve unrelated existing material and Blender edits.


Results: 27 Editor rules, 23 actual 2-player beacon/rescue/baton checks, and 24 actual 4-player delivery checks passed. The 17 existing actual 2-player paid-revival/departure checks also passed. Final Mac build: 0 errors and 7 existing warnings. The initial WEST terminal overlapped the perimeter wall; moved it inside the corridor and revalidated physical access. Fixed a test automation wait that clicked before returning from the manifest to the terminal. Two historical full hazard scripts depend on old rules and now stop before launching clients. Do not treat them as passing full hazard regression until migrated.

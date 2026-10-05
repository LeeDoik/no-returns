# Personal equipment slots and medicine — 0.9.25

[한국어](personal-equipment.ko.md)

2026-10-06 · Game 0.9.25 / protocol 16 / TCP 27842.

## Implemented slice

The next [co-op design](coop-design-v1.en.md) slice gives each co-op employee 1 special slot and solo 2 slots. Cargo, basic baton and radio remain separate. Medicine is the first available personal item; scanner, decoy stack, cutters, plasma, cart and reusable-item storage are not implemented by this change. The old shared beacon remains its existing separate prototype system.

New normal Cinder sessions start with the planned shared wallet of 400 CR instead of 0 CR. At the ship terminal, choose PERSONAL EQUIPMENT, select a usable slot, then BUY & EQUIP MEDICINE / 40 CR. Any living crew member can spend the shared settled wallet on their own empty slot during preparation, selected route or report (phases 0/1/4). Host serializes transactions, rejects full slots and insufficient balance, and never spends unbanked delivery money. Buying directly equips 1 dose; no bulk consumable storage. On-site ship visits do not allow restocking.

Default bindings are F to use medicine, X to switch solo slots, G to put selected medicine down on clear nearby floor, and E to pick a dropped dose into the selected empty slot. All bindings can be changed in settings. G is available only while landed, preventing pre-departure stockpiling of unmounted spare doses. Turn toward a nearby teammate to heal them; otherwise F heals self. Teammate targeting requires the first visible employee collider within 2.2 m; pickup uses a 2.4 m ray. Cargo/beacon carriers, downed and eliminated actors cannot use or exchange medicine. Carrying a parcel keeps its existing Q release behavior.

1 dose restores an injured living target to 100 health immediately. Keep down count and existing protection unchanged. Healthy, downed, eliminated or obstructed targets cannot receive medicine; a downed person must first be rescued. Target validity and dose consumption occur together on the host, so duplicate/simultaneous treatment of a healthy target does not spend a second dose. No free ship treatment or passive regeneration. Treatment during an active visit is allowed aboard and outside; surviving injuries can also be treated during subsequent preparation.

Unused equipped medicine persists through revival, departure, visit failure and subsequent landings within the same session. Consumed doses do not return. Directly dropped doses left at departure/failure are lost; field exchange uses one physical medicine box and 1 authoritative pickup. Ground stock is bounded at 32 boxes. If solo becomes co-op during preparation, an already equipped second dose remains reserved and unavailable in the 1-slot loadout; it becomes accessible when solo again. This does not add account ownership, company storage, save/resume/reconnection or cross-session retention.

## Production and source

- [Personal dose ledger](../../NoReturns/Assets/_NoReturns/Runtime/CarryLab/CarryInventory.cs), [room targeting and physical box](../../NoReturns/Assets/_NoReturns/Runtime/CarryLab/CarryRoom.Inventory.cs).
- [Wallet purchase](../../NoReturns/Assets/_NoReturns/Runtime/CarryLab/CarryMission.cs), [health guard](../../NoReturns/Assets/_NoReturns/Runtime/CarryLab/CarryVisit.cs), [input](../../NoReturns/Assets/_NoReturns/Runtime/CarryLab/CarryControls.cs), [menus](../../NoReturns/Assets/_NoReturns/Runtime/CarryLab/CarryRoom.Controls.cs), [wire](../../NoReturns/Assets/_NoReturns/Runtime/CarryLab/CarryWire.cs).
- [Editor checks](../../NoReturns/Assets/_NoReturns/Editor/InventoryRulesReview.cs), [2-client purchase/healing/exchange](../../tools/test_cinder_inventory.py). Run against a current build: `python3 tools/test_cinder_inventory.py`. Initial test fixtures prepare injuries, location and funds only; purchases, treatments, drops and pickups use ordinary controls and UI.

The dropped box uses a small cream cube with a MED label as temporary presentation. Medicine-specific hands, use animation, final mesh, sound and human readability remain pending. Reuse existing carry/rescue/body presentation rather than claiming a new medical animation. The inventory hint and personal menu show slot, item, controls and wallet in Korean and English. Expanded settings remain two columns.

## Validation and limits

See [validation](../validation/personal-equipment-0.9.25.json) for actual results. No manual player/companion windows opened. GUI Editor startup exited without a reported compile error; a persistent windowless Editor connected and compiled. An initial native launch found a duplicate EMPTY translation key; removed the duplicate and reran. Initial build from the headless Editor's unnamed scene was rejected; opened the authored review scene and rebuilt. GUI/Metal quality is not verified by this fallback.

Official release checks: Unity 6000.6.4f1, CLI 1.0.0-beta.12, Pipeline 0.8.0-exp.1 current on 2026-10-06. Session state remains slot-based rather than account-based. Persistence, reconnects, full hazard regression migration, remaining tools and human play quality are still pending. No new dependency or final asset production.


Related regression fix: existing ship parcels moved after landing through employee collisions, breaking address-specific selection. Keep them fixed until first pickup; released parcels use existing physics. Update the delivery check for the new starting wallet of 400 CR and wallet of 675 CR after both WEST contracts.

Validation: 21 Editor rules, 23 actual 2-player medicine checks, 12 existing rescue checks and 25 actual 4-player delivery regression checks passed. Final Mac build: 0 errors, 7 existing warnings. Passed 296 document checks, bilingual numeric alignment, Python syntax and changed-file whitespace checks.

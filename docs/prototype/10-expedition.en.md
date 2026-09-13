# Expedition play guide

[한국어](10-expedition.ko.md)

> **Historical record · retired/superseded 2026-09-12.** The [PSX space-delivery design](../current/01-overview.en.md) is current. Implementation, launch, art and values below are historical, not current status. Deleted files remain path records only.


2026-09-10 · EXP-01 first development playable, not the finished expansion.

## Launch and controls

Select **DELIVERY EXPEDITION · PLAYTEST** on the existing executable's main-menu right side. Choose solo, host expedition (LAN), or join expedition. Use the same latest build and join through the expedition menu, separately from campaign joining. Steam invitations and external-internet validation remain separate work.

The host selects 3 of 5 contracts. Everyone readies, then parcels appear ahead of the truck. Carry C01–C05 parcels to matching numbered recipients. Defaults: WASD movement, mouse look, E pickup/catch/set-down, left click throw, Space jump and Q ping. Existing remapped controls apply and the HUD shows actual bindings.

Set a parcel down at its recipient and let it stabilize for 0.4 seconds to secure 30 immediately. Held or fast-flying parcels are not accepted. Wrong recipients pay nothing; retrieve cargo yourself. Completed parcels never respawn. Maximum shift payout is 90.

Tab/Esc opens/closes the board. Ready within 6m of the truck and review the undelivered abandonment list. Everyone readying settles the shift; pressing again cancels readiness. Leaving the truck clears a ready employee's agreement. The world continues while the board is open. Secured earnings survive unfinished-contract abandonment. The host starts the next shift.

## Scope and limitations

The 80×80m fixed editable map contains 3 zones, 5 ground-level recipients and loop streets. The rooftop greenhouse contract currently uses its ground counter. This is a rules-validation blockout, not final art. Sneezer/clinger/hopper and carrying reuse existing behavior. Adhesion targets employees/cargo only; hopping pauses while held.

The truck is fixed. Driving, physical truck-loading puzzles, tools/purchases, quality bonuses, optional jobs, random zones and new rescue/safe sockets are absent. Actual timing and the 30–40-minute fun target remain unverified. Out-of-bounds cargo uses existing recovery to its original position ahead of the truck; employees use existing fall recovery.

## Persistence and online

Host company balance saves as expedition_company.json in Godot user data, separately from existing run_profile.cfg. Guests see the shared balance without copying it into their company. Switching hosts changes company balance. Delivery commits only after successful saving; failure shows an alert and leaves cargo for retry. Mid-shift resume and host migration are unsupported.

Late joiners receive current state and spawn at the truck. Departing employees drop cargo on site and clear readiness. Host departure ends the session while saved earnings remain. Expedition protocol is 13; campaign remains 12.

## Validation

Passed 31 state/storage checks, map carrying-clearance connectivity, physical receipt/wrong destination/early return/next shift, and actual local ENet two/four-player checks. Reviewed actual GPU menu/board/play/map samples. Newcomer fun, external networking and endurance remain unverified.

Direct launch: double-click `04_배송 원정.cmd` in the project folder to open the expedition menu.

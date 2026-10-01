# Unified controls and carried beacon

[한국어](controls-beacon.ko.md)

The historical `artifacts/` paths identify local evidence from the original run. These files are no longer retained here and are not included in the public repository. Historical passes are distinct from current revalidation.

## 2026-10-01 — Controls, ship and purchase UI 0.9.2

[Current controls and running](controls-ui.en.md). E targeted use/ship terminal, left-click ground placement, hold right click to rotate parcel, wheel 0.75–1.6m reach, Q immediate release. Separate departure/return/purchase into native buttons showing boarding count, wallet, disabled reasons and zero-pay return confirmation. Esc settings provide 13 button rebindings, sensitivity, FOV, language/defaults. Menus block gameplay inputs while the world continues. Connect Cinder 120 CR beacon purchase/shared carrying/two 8-second signals. Version 0.9.2, protocol 11, TCP 27842. Preserve source map/delivery pay. Supersede E automatic progression/Esc shop and unconnected-Cinder-beacon statements below within this scope. Listener/baton/suppression/save integration and human quality assessment remain. [Actual validation scope](../validation/controls-ui-0.9.2.json).

## Earlier 0.8.8 implementation and validation

2026-09-13 · 0.8.8 · Implemented · see validation scope below

E: pick up targeted cargo/beacon, collect receipt, inspect clue, select route/return aboard. Hold E beside a downed partner to rescue. Q: set down the held item. E never drops it. Calling moves from Q to C. G shove, Tab journal and Esc menu remain. Supply menu: up/down selects purchase or contract, E confirms; mouse remains supported. Separate F/I/B/T/V shortcuts are removed. Rescue, targeted use, pickup and ship actions do not execute together from one press.

Buying the 120 CR beacon license spawns one shared device aboard at (-2,0.22,-7). Carry it with E; cargo and beacon cannot be held together. Q aboard stores it without a charge. Setting it on field ground activates an 8-second signal and consumes 1 charge. There are 2 uses per shift; it cannot be collected while active. Afterward E carries it again. Next shift resupplies the device aboard and 2 charges; departure alone does not reset its position. Remote creation/deployment is removed. Host owns carrier, position and stock, shared through protocol 9. Blocked placement keeps it held.

The user approved concept 03 / DECOY BEACON for production. They requested control changes and physical carrying first, so this build uses the existing placeholder. Approved final 3D production/integration remains subsequent art work; it is not marked complete.

Validation: two real processes for E/Q carrying, receipt and return; a separate seeded-wallet session for purchase, ship spawn, client carry, activation and recovery. Human handling and new art quality require separate validation.


Verification: Windows 0.8.8 build, 30 delivery/E/Q checks in two real processes and 14 seeded-wallet beacon purchase/carry checks passed. Final recovery testing used the ship-exit approach. Earlier attempts waiting at a creature patrol point failed from downing; combat and dangerous-area recovery difficulty remain for human evaluation. Documentation check passed for 210 entries.

Build (`artifacts/space-foundation/controls-build-result.json`) · E/Q delivery (`artifacts/space-play-02/latest.json`) · Physical beacon (`artifacts/physical-beacon/latest.json`)

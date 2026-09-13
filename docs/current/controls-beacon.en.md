# Unified controls and carried beacon

[한국어](controls-beacon.ko.md)

2026-09-13 · 0.8.8 · Implemented · see validation scope below

E: pick up targeted cargo/beacon, collect receipt, inspect clue, select route/return aboard. Hold E beside a downed partner to rescue. Q: set down the held item. E never drops it. Calling moves from Q to C. G shove, Tab journal and Esc menu remain. Supply menu: up/down selects purchase or contract, E confirms; mouse remains supported. Separate F/I/B/T/V shortcuts are removed. Rescue, targeted use, pickup and ship actions do not execute together from one press.

Buying the 120 CR beacon license spawns one shared device aboard at (-2,0.22,-7). Carry it with E; cargo and beacon cannot be held together. Q aboard stores it without a charge. Setting it on field ground activates an 8-second signal and consumes 1 charge. There are 2 uses per shift; it cannot be collected while active. Afterward E carries it again. Next shift resupplies the device aboard and 2 charges; departure alone does not reset its position. Remote creation/deployment is removed. Host owns carrier, position and stock, shared through protocol 9. Blocked placement keeps it held.

The user approved concept 03 / DECOY BEACON for production. They requested control changes and physical carrying first, so this build uses the existing placeholder. Approved final 3D production/integration remains subsequent art work; it is not marked complete.

Validation: two real processes for E/Q carrying, receipt and return; a separate seeded-wallet session for purchase, ship spawn, client carry, activation and recovery. Human handling and new art quality require separate validation.


Verification: Windows 0.8.8 build, 30 delivery/E/Q checks in two real processes and 14 seeded-wallet beacon purchase/carry checks passed. Final recovery testing used the ship-exit approach. Earlier attempts waiting at a creature patrol point failed from downing; combat and dangerous-area recovery difficulty remain for human evaluation. Documentation check passed for 210 entries.

[Build](../../artifacts/space-foundation/controls-build-result.json) · [E/Q delivery](../../artifacts/space-play-02/latest.json) · [Physical beacon](../../artifacts/physical-beacon/latest.json)

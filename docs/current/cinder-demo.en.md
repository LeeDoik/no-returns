# CINDER DEPOT delivery demo

[한국어](cinder-demo.ko.md)

2026-09-18 · CINDER-DEMO-01 · Connecting the primitive map to existing game rules

## Launch and one cycle

Run [Play_Cinder_Demo.cmd](../../Play_Cinder_Demo.cmd). The Windows executable is `builds/CinderDemo/NoReturns-CinderDemo.exe`. Create a room to play alone, or let up to three peers using the same build join the host's LAN address before departure. Additional processes on the same PC use `127.0.0.1`. The existing connection uses TCP 27841, not Steam matchmaking.

Press E aboard to select the contract and depart. Pick up cargo with E, carry it to BAY 04 in the northeast, and place it inside the floor outline beside the terminal with Q. Stable cargo is scanned before a receipt prints. Look at the terminal and press E to collect the receipt, then bring everyone aboard and press E to return and settle. A standard contract pays 300CR delivery plus 120CR return, totaling 420CR. Printing alone, or returning without collecting the receipt, does not pay. After settlement, purchasing the 120CR beacon unlock spawns physical equipment aboard. The next shift retains the unlock and remaining balance and prepares two uses.

Controls: WASD movement, mouse look, E interaction/hold near a teammate to rescue, Q set down, Space empty-handed jump, Shift quiet walking, C call, left click baton. Use supply selection and language switching in the Esc menu. Unlike Shift sprint in the blockout trial, this demo uses the existing cooperative game's quiet walking.

## Scope and values

- Separate [CinderDeliveryDemo.unity](../../NoReturns/Assets/_NoReturns/Scenes/CinderDeliveryDemo.unity). The source blockout and ship trial remain. The map covers 108×86.4m and retains the existing ship dimensions and entrance. No new exterior was made.
- [CinderDemoLayout.cs](../../NoReturns/Assets/_NoReturns/Runtime/CarryLab/CinderDemoLayout.cs): four ship spawns, cargo/equipment loading, BAY 04 reception center `(32.7,0,15)`, and aboard return volume. The reception device's original screen material and receipt slot move with the same offset.
- [CarryMission.cs](../../NoReturns/Assets/_NoReturns/Runtime/CarryLab/CarryMission.cs): released cargo stable for 0.75 seconds is received, print readiness takes 0.75 seconds, and settlement requires receipt collection and everyone aboard. Prices, payouts and failure rules reuse existing behavior, not a newly finalized economy. The existing limitation that a departing peer aborts the current shift remains.
- [CarryThreat.cs](../../NoReturns/Assets/_NoReturns/Runtime/CarryLab/CarryThreat.cs): one B-zone listener and one outer creature. Wall-aware navigation covers the larger map and is prepared before connection. Subsequent shifts reuse the navigation graph for this static map. Additional A/C listeners are unimplemented.
- [CarrySuppression.cs](../../NoReturns/Assets/_NoReturns/Runtime/CarryLab/CarrySuppression.cs): stable at 0–360 seconds, irregular at 360–480, critical at 480–600, off from 600, outer intrusion from 608. These are initial field-time test values. Signals, lighting and copy replace a numeric countdown. Not every blockout suppression pillar is individually functional.
- [CarryRoom.Network.cs](../../NoReturns/Assets/_NoReturns/Runtime/CarryLab/CarryRoom.Network.cs): host authority, preparation-only joining, Cinder protocol 11 versus existing trial protocol 10. Builds for different maps cannot join each other.
- The maintenance clue moved to the office. Detailed interior rooms, new monsters, final art and additional cargo types are outside this change.

## Authoring and reproduction

Unity `NO RETURNS > Demo > Create Cinder Delivery Demo` recopies the source blockout to the demo path and therefore overwrites direct demo edits. Save a separate version or incorporate changes into generation source first. **Build Cinder Delivery Demo builds the saved demo scene without regenerating the map.** Validate checks reception, receipt, all-crew return and 420CR rules. [CinderDemoBuild.cs](../../NoReturns/Assets/_NoReturns/Editor/CinderDemoBuild.cs) is the reproduction source.

Automation uses a local Windows build with test-input support. The [delivery test](../../tools/test_cinder_demo.py) supports `--crew 1`, `--crew 2`, and `--crew 4`; the [hazard test](../../tools/test_cinder_hazards.py) uses two processes. Run sequentially because they share a port. Ordinary movement inputs traverse the west/north detour both ways, without teleportation or seeded money. In multiplayer tests only the host delivers while peers remain aboard to check replication and the all-aboard rule; this is distinct from simultaneous cargo handling by the whole crew.

## Validation and remaining work

See CINDER-DEMO-01 in the [validation checklist](05-validation.en.md) for results. Local raw evidence is in `artifacts/cinder-demo/`, `artifacts/cinder-demo-loop/`, and `artifacts/cinder-demo-hazards/`. Builds, logs and captures are excluded from Git.

Human checks remain: finding reception on a first visit, cargo visibility/carry fatigue, reasons to distract enemies for a courier, understanding suppression signals, tension after rescue, and the 10–15-minute target duration. That duration is not a measured result. Render inspection showed the existing baton obscuring the center of the terminal screen. Weapon placement and terminal readability are follow-up work. The outer creature's complete pursuit routes, cargo collisions at every doorway, other PCs/internet/Steam and actual four-person enjoyment are unverified. Existing URP DepthOfField/Panini shader warnings and deprecated Unity API warnings remain. This is a playable-cycle test, not release sign-off.

[Running and connecting on another PC](cinder-portable.en.md)

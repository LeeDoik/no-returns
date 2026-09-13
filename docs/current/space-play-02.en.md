# SPACE-PLAY-02 — View-relative carrying and first delivery loop

[한국어](space-play-02.ko.md)

2026-09-12. Internal validation plan for the approved next step. Scope and acceptance conditions follow writing-plans.

1. Add a two-player failing check for cargo following only horizontal facing. Rotate an eye-relative local offset by full look rotation while retaining wall/floor collision. Test vertical view, ownership and collision for both host and client.
2. Preserve CarryRoom carrying mode; add `--delivery`. Use the back of the placeholder room as the ship area and the existing bench as reception. This does not build the actual CINDER DEPOT map or final art.
3. F inside the ship selects CINDER DEPOT; F again checks everyone aboard and transitions to automatic arrival. Travel is represented by a state change only. Set cargo down on reception and let it settle to confirm receipt. F with everyone aboard returns/settles; F prepares the next shift. Reject F outside the ship, held-cargo acceptance and duplicate payment.
4. Use only intact cargo from the economy proposal: receipt 300 CR, full return 120 CR, total 420 CR. The shared wallet is session memory only; damage, death, purchasing and persistence remain absent. Block new joins during a shift; disconnection aborts that shift without settlement to prevent paying a missing employee bonus. Already secured receipt pay remains. This is a temporary restriction, not the final reconnection rule.
5. Verify replicated phase, receipt and wallet across two processes, unpaid empty returns, duplicate payment prevention and next-shift reset. Separate automated validation from human controls/fun evaluation. Update Windows build and bilingual specifications, guide, validation and history together.

Files: CarryRoom integrates input/carrying; CarryMission owns shift state/payment; CarryWire replicates state; CarryWorld supplies placeholder markings; CarryBuild builds. Automated input uses normal movement/interactions without adding teleportation.

## Launch and production

[Launch delivery mode](../../04_Play_Delivery_Loop.cmd), select HOST, then [join on the same PC](../../05_Join_Local_Delivery.cmd). Use the same build in both windows. WASD/mouse/E remain unchanged; F performs ship actions. R reset is disabled in delivery mode. The original [carrying mode](carry-test.en.md) remains available.

The teal floor is the ship zone; the gold bench is reception. Walk around the wall via (x=1, z=5) toward the bench at (x=-6, z=9). Look at cargo and press E to pick up, then E to set it down over the bench. Receipt requires 0.75 seconds settled and released. Adjust cargo height by looking up/down. Return with F inside the ship after all connected crew gather. Returning without delivery pays 0 CR.

[CarryMission.cs](../../NoReturns/Assets/_NoReturns/Runtime/CarryLab/CarryMission.cs) owns shift state and duplicate-payment prevention. Ship bounds: |x|<3, -10<z<-3.8, y<2. Reception checks cargo center -7.05<x<-4.95, 8.6<z<9.4, 1.15<y<1.5 and squared speed<0.04. These exact bounds apply only to the placeholder geometry. Only the host decides pay; protocol 3 replicates results. Do not connect different versions.

States: preparation, route selected, field, receipt confirmed, report. The field permits an unpaid return. F at the report starts the next shift, keeping the wallet while resetting cargo/employees. Departure/arrival are immediate state changes, not flight, actual spacecraft or site loading. There is no persistence, so quitting loses the balance.

## Current validation results

- [x] Compilation and Windows 0.3.0 build through Unity MCP.
- [x] 17 [automated carrying checks](../../artifacts/space-play-01/latest.json): vertical view, host/client synchronization, floor, walls, ownership and departure.
- [x] 21 [automated delivery checks](../../artifacts/space-play-02/latest.json): actual movement in two executables, receipt/return/420 CR agreement, unpaid empty return, blocked return with partner outside, blocked reclaimed receipt, next shift and disconnection.
- [ ] Human controls, visibility, discomfort, route understanding and fun evaluation.
- [ ] Actual CINDER DEPOT map, threats, rescue, persistence, shop, 4 players and separate-PC validation.

Both modes ship in the existing `builds/CarryTest/NoReturns.exe`. Automated inputs move real executables but differ from human keyboard/mouse evaluation. Camera-render captures do not validate the entire HUD. The user's confirmation that the previous build runs is not approval of the new build's controls.

## Guidance language — 0.3.1

Added Korean by default, the 한국어 / English menu toggle and persistence of the local preference. [Usage and production guide](carry-test.en.md). Language selection is separate from online game state. Results are recorded in the [language checks](../../artifacts/language/latest.json).

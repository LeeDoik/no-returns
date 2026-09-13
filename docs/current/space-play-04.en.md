# SPACE-PLAY-04 — Supply and risk contract experiment

[한국어](space-play-04.ko.md)

As of 0.6.0, host progression saves restore wallet, beacon license and successful-delivery count; shifts restart at ship preparation. Earlier session-only/no-save notes describe the state through 0.5.0. [Save rules and validation](space-play-05.en.md).

2026-09-13 · Implementation of proposed test values. This does not finalize the release economy or include persistence.

## Plan and acceptance criteria

Connect delivery → return → purchase → equipment use next shift → risk contract selection inside the existing listener experiment. Preserve carrying, rescue and existing launch paths.

- B: the host buys a beacon license for 120 CR aboard the ship during preparation or report. Reject duplicates, insufficient funds and client purchases. License and wallet last for the session.
- F: existing route selection and arrival. Arrival refills purchased beacons to 2 shared uses. V: aim empty-handed at yard floor within 8m to deploy. Only 1 device may be active; for 8 seconds it sends sound from its position to the listener within 12m every 1 second. Wall, sky, out-of-range, carrying and downed attempts do not consume stock.
- The yard bounds are x=-8~3, z=3~13, floor height at most 0.4m and upward normal at least 0.65. These limits are specific to the placeholder map. A temporary cylinder displays the beacon. End active signals at report.
- After the first successful delivery, the host can press T aboard with a selected route to toggle standard/risk. Changes are blocked in the field. Standard receipt/return pay is 300+120 CR; risk pays 450+180 CR. A new shift resets selection to standard.
- Risk uses the same region with patrol 1.8m/s, investigation 3.2m/s, step hearing 8m and attack warning 0.9 seconds. Standard retains 1.4m/s, 2.5m/s, 6m and 1.2 seconds. Preserve shove, rescue and secured-pay rules.
- The Esc supply panel shows price and contract pay. All instructions support Korean default and English toggle. The host decides purchases, deployment, contract and rewards and replicates them to two processes.

## Sources and validation

[CarryMission](../../NoReturns/Assets/_NoReturns/Runtime/CarryLab/CarryMission.cs), [CarryEquipment](../../NoReturns/Assets/_NoReturns/Runtime/CarryLab/CarryEquipment.cs), [CarryRoom](../../NoReturns/Assets/_NoReturns/Runtime/CarryLab/CarryRoom.cs) and [CarryThreat](../../NoReturns/Assets/_NoReturns/Runtime/CarryLab/CarryThreat.cs) define the rules. Build 0.5.0, protocol 4.

Automated checks verify delivery, purchase and deployment agreement and rejected requests in two real executables. Separate compilation/rule checks from executable checks. Feel, sound readability, equipment pricing and risk-contract fun require human play. Record results at completion.

Launch: [listener experiment](../../06_Play_Listener_Test.cmd), [same-PC partner](../../07_Join_Local_Listener.cmd). Persistence, final shop art, new destinations, 4 players and suppression are excluded.

## 2026-09-13 validation results

- Unity MCP compilation and Windows 0.5.0 build completed. Passed 17 rule checks covering standard/risk pay, duplicate payment/purchase rejection, authority/location/phase gates, retained license and shift reset. [Rule result](../../artifacts/space-play-04/rules-result.json).
- Passed 24 [progression checks](../../artifacts/space-play-04/latest.json) in two actual Windows processes: normal delivery and fast return earn 420 CR → purchase for 120 CR → shared 300 CR wallet → risk contract → client beacon deployment, remote attraction and expiry.
- Existing [rescue checks](../../artifacts/space-play-03/latest.json) passed 21 and [language checks](../../artifacts/language/latest.json) passed 9. These counts describe automated checks, not human evaluations.
- The first progression attempt was caught during a slow return by the creature investigating cargo set-down. The 300 CR receipt remained secured. Changed the test return path to fast movement around the back of the yard without weakening game rules. Preserved the failed run evidence.
- Added a temporary spatial beep and pulsating cylinder to the beacon. Full HUD/supply-panel capture and actual listening evaluation were not completed. The automated camera capture request was overwritten by subsequent movement input and produced no image; this is not visual approval.
- Successful risk delivery and the full 630 CR settlement were not verified through the complete playable route. The 450+180 calculation was verified separately through Unity rule checks. Human fear, fun, pricing and beep readability remain unverified.

[Carrying regression checks](../../artifacts/space-play-01/latest.json) also passed 17. Documentation links/counterparts passed across 190 documents.

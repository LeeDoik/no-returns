# Pay, failure and equipment economy draft

[한국어](economy.ko.md)

As of 0.6.0, host progression saves restore wallet, beacon license and successful-delivery count; shifts restart at ship preparation. Earlier session-only/no-save notes describe the state through 0.5.0. [Save rules and validation](space-play-05.en.md).

0.8.6 current rule: delivery confirmation does not pay. Collect the receipt and return normally to earn standard 420 / risk 630. Draft provisions below for 300 after post-delivery wipe and immediate secured pay are retired. [Current specification](receipt-terminal.en.md).

2026-09-12 · SPACE-ECO-01 · Test proposal; not implemented or balance-validated

## Purpose and scope

Delivery should enable the next equipment choice while failure still permits departure with basic gear. This is the requested economic design draft, not final values. The initial test uses 1 contract, 1 parcel and 1 site per shift. CR is shared team currency, not duplicated or divided per player. Starting funds are 0 CR. The pictured previous balance of 180 CR is a mid-campaign example.

## Pay

| Tier | Base B | Maximum condition Q | Maximum return R | Maximum total |
|---|---:|---:|---:|---:|
| Standard | 240 | 60 | 120 | 420 |
| Hazardous | 360 | 90 | 180 | 630 |
| Deep | 480 | 120 | 240 | 840 |

Cargo-condition multiplier q is 1 for intact, 0.5 for damaged and 0 for heavily damaged. Even heavily damaged cargo is accepted initially; complete destruction during transport is excluded. Fall/impact thresholds require separate physics validation. Labels/terminal show condition before acceptance.

- Acceptance records B + Q × q once in the shared account. Death or a wipe does not revoke it.
- Return adds floor(R × r / n) only when delivery was completed. n is the crew roster fixed at landing; r is returned crew.
- Without delivery, all pay is 0. Empty departures and immediate returns earn no bonus.
- Settlement must not add base/condition pay again. It displays recorded payments and adds only the return bonus.
- Suppression expiry does not reduce pay. It increases delivery/return danger; no explicit time bonus or exact countdown is introduced.

## Incidents, failure and recovery

Propose downed as rescuable by teammates and dead as unable to act for the remaining shift. A downed employee carried aboard alive counts as returned. Rescue controls and death transitions remain undecided. If everyone becomes incapacitated, end the shift and settle at base. All employees return next shift; no permanent character loss, debt or mandatory death fee.

Retain accepted-cargo pay, equipment licenses and upgrades. Undelivered cargo closes the contract without pay; optional supplies left on site are lost. Return unused supplies only if safely stored aboard or brought back by crew. Supplies stored aboard are treated as recovered even after a wipe in this proposal. Basic and licensed tool bodies are reissued next shift. Failure mainly costs return bonus, optional supplies and time spent.

Do not automatically end while someone remains active. Propose departure after ready confirmation from living crew aboard, clearly showing who will be left behind. Outside survivors must not permanently block departure. Intentional abandonment, host authority and reconnect fairness require playtesting.

## Purchases, upgrades and loadout space

| Item | Price CR | Result |
|---|---:|---|
| Basic flashlight/baton | 0 | Issued to every employee; free recharge/reissue |
| Decoy Beacon license | 120 | Permanent campaign unlock |
| Pressure Caster license | 300 | Permanent campaign unlock |
| Baton insulated grip | 180 | Candidate upgrade reduces own post-use recovery |
| Caster efficient valve | 240 | Candidate upgrade uses less gas for the same push |
| Beacon directional horn | 180 | Candidate narrow, longer-reaching direction mode |
| Cargo securing rack | 300 | Candidate reduces cargo movement aboard |
| Extra supply pack | 40 | Optional pre-departure purchase; 1 use |

Each upgrade is purchased once and requires its base tool license. Exact improvement magnitudes remain undecided. No upgrade kills STRIDER or extends suppression in this draft.

Licenses belong to the team. Duplicate tool selection is allowed; prepare 1 body per slot in a shared 4-slot deployment rack. Basic flashlights/batons are personal issue outside this rack. Each employee can carry 1 additional tool. No unlimited in-mission body spawning. All 1–4-player crews use the same shared balance, prices and pay table; cargo/route difficulty for smaller crews is adjusted separately.

Selected tool bodies receive 1 free standard charge at shift start, with no in-shift onboard recharge. Normal next-shift issue remains available after spending the charge. Charge cannot be sold or transferred between tools. Load at most 2 optional packs; each uses 1 slot, competing with tools in the same 4-slot rack. Packs refill only up to tool capacity; amounts require tool validation. Returned unused packs go back into storage and are not issued free. No resale or salvage refunds in the first version.

No mandatory fuel, repair or redeployment cost. A balance of 0 still permits standard work. Every required delivery needs a route solvable with basic tools.

## Progression and persistence

Propose hazardous work after 2 successful standard delivery/return shifts, and deep work after 2 successful hazardous shifts. Success means acceptance followed by at least 1 crew member returning. No currency entry fee. Higher difficulty is optional and standard work remains available. Higher pay compensates greater carrying, routing and information burden rather than merely more creature health.

A first intact full-return shift yields 420 CR, enough for caster 300 + beacon 120 together. Later choose supplies versus upgrade order. Instead of mandatory upkeep draining currency, acknowledge that this finite equipment list eventually loses spending opportunities. Long-term economy expansion follows equipment-use data.

Shared funds, licenses, upgrades and contracts belong to the host campaign save. Guests share campaign equipment but do not duplicate funds/items into other rooms. Personal guest progression is outside initial scope and must be explained on joining. Propose purchases confirmed by the host at base, with requests from other crew.

## Duplicate-payment and departure requirements

The host must use runId/contractId/receiptId for once-only acceptance/payment, saving receipts and wallet together. Reconnects, collecting paper again or repeated interaction cannot pay again. Do not settle completed run IDs twice. Shop checks funds, subtracts and unlocks atomically; duplicate purchases fail. No purchases during missions.

Fix n at landing; new players participate next shift. Disconnect is not immediate return; reconnect as the same employee. Unrecovered crew do not count toward return bonus. Host departure pauses the campaign for saved-state resume. Automatic migration and protection against local save editing are outside initial scope.

## Examples and validation

| Standard contract outcome | Total CR |
|---|---:|
| Intact cargo, full return | 420 |
| Intact cargo, 2 of 4 return | 360 |
| Intact cargo, wipe after acceptance | 300 |
| Damaged cargo, full return | 390 |
| Heavily damaged cargo, full return | 360 |
| No delivery, full return | 0 |
| No delivery, wipe | 0 |

The [settlement screen](../art/space-concepts/shift-report-01.png) shows previous 180 + earned 360 = 540 CR. The subsequent [shop screen](../art/space-concepts/equipment-shop-01.png) shows 540 - 300 = 240 CR. The shop previews the base purchase and locked later upgrade without charging both together.

Automated calculations check arithmetic in formulas/tables, monotonic returns by crew count, no empty-return pay and first-purchase access only. They are not Unity payment/save/online tests. Visually checked image amounts and selected names. No claim of validated balance is made.

- [ ] Record shifts to first purchase and optional-supply burden in play.
- [ ] Compare deliberate death settlement with normal return using CR/minute and fun.
- [ ] Evaluate time saved versus bonus lost by abandoning crew, and rescue willingness.
- [ ] Compare solo/multiplayer earnings, carrying burden and loadout choices.
- [ ] Check whether extra supplies are always bought or always ignored.
- [ ] Validate payment/purchase duplication, reconnect and host resume across actual processes.

Keeping 300 after a post-delivery wipe can motivate intentional death if it settles faster. Measure and adjust return bonus, return distance and redeployment flow together, without patching it through confiscated secured pay or long forced waits. The criterion is that normal return is preferable while failed teams still want another attempt.

## SPACE-PLAY-02 implementation subset

[Delivery mode](space-play-02.en.md) implements only intact-cargo receipt and full-return pay in session memory. Damage, death, persistence, purchasing and economic balance in this document remain proposed/unverified.


## SPACE-PLAY-04 — Supply/risk contracts (0.5.0)

[Purchase/use specification](space-play-04.en.md). Listener mode now implements session beacon-license purchase, shared deployment and risk-contract selection. Persistence, the full shop, more destinations and final art remain. Use existing launchers 06/07. Two-process progression checks passed 24, existing rescue checks 21, and language checks 9. Human feel, economy balance and rendered readability of the new supply panel are unverified.

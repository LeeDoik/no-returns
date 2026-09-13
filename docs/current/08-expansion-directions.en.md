# 8. NO RETURNS expansion design — cooperative delivery expeditions

[한국어](08-expansion-directions.ko.md)

> **Historical record · retired/superseded 2026-09-12.** The [PSX space-delivery design](01-overview.en.md) is current. Implementation, launch, art and values below are historical, not current status. Deleted files remain path records only.


Revised: 2026-09-10 · Design revision 2 · Current game 0.9.4. **Direction approved / detailed rules are experimental proposals / expansion features are not implemented.** This deliverable changes documentation, not code, maps or models. See the [current specification](02-spec.en.md) for implemented behavior and the [history](../archive/change-log.en.md) for earlier exploration.

Implementation update 2026-09-10: added EXP-01 fixed map, contracts, base payouts and return. Below remains the full expansion target; unimplemented labels refer to the design-time baseline. For actual scope/values use the [current specification](02-spec.en.md) and [play guide](../prototype/10-expedition.en.md).

## 1. Vision and decisions

**Load living parcels into a truck, head into a different delivery district each shift, and complete the day's deliveries by recovering from accidents together.**

The user selected 1–4-player third-person 3D cooperation, a shared truck base and on-foot deliveries, repeated 30–40-minute shifts, preserved completed earnings with accident recovery, and progression through tools and truck configurations. Steam is the release target. Preserve the cute tactile toy materials and shapes of the [approved D art](../art/10-approved-d.en.md). Identify teams through workwear colors. Maintain English source game copy with complete Korean counterparts.

Direct truck driving and city reputation/life simulation are outside this focus. Freeform terrain generation, comprehensive weather and a replacement physics-grab system are outside the first experiment. Preserve the Shipping Shrine, existing campaign and physics lab. One validation map does not cap the final game's scale.

Fun hypothesis: teams discuss delivery order, use parcels as both problems and tools, exchange roles after accidents, and try different equipment and routes next shift. This is not a human-playtest finding.

## 2. Example shift

The crew starts work in Toypost Village. They select the paper shop's sneezer, the rooftop greenhouse's hopper and the equipment yard's clinger. They pack a trolley using 2 shared equipment slots and straps using 1, leaving 1 of the 4 slots empty. Filling every slot is optional.

They initially head for the nearby paper shop, then discover a sign explaining that a sneeze can clear the alley's paper obstruction. They postpone delivery to use the parcel as a tool. Another parcel rolls away during the sneeze; one employee retrieves it while another marks the shortcut. The opened route stays open for this shift.

On the roof, they align the hopper before its warning completes. A teammate moves into a catching position; a solo player could instead use the guarded stairs. They then try attaching the clinger to the equipment platform's designated face, needing to collect it before adhesion expires. If they miss, a recovery balcony below lets them continue.

After the basic deliveries, a new request asks them to collect another parcel from the village counter. Secured earnings remain safe. The crew can deliver more or return to the truck for settlement. Next shift they swap the trolley for a ramp and try another route.

## 3. Shift and contract rules

All durations, distances and prices below are experimental proposals. Moment loop: observe → transport → react → recover. Delivery loop: destination → route → solution → receipt. Shift loop: preparation → basic deliveries → optional job → settlement → equipment choice.

| Stage | Target duration | Rules |
|---|---:|---|
| Preparation | About 3 minutes | Select 3 of 5 contracts; load their 3 parcels and equipment |
| Basic deliveries | 20–25 minutes | Any order; return to the truck to exchange loaded items |
| Optional job | Optional 5–8 minutes | Reveal 1 after all 3 basic contracts; declining is normal completion |
| Settlement | About 2 minutes | Show secured earnings, bonuses, recovery record and next purchases |

These add up to 25–30 minutes without the optional job and 30–38 with it. The representative shift target is 30–40 minutes; do not make early-returning crews wait. Validate whether routes and on-site problems give the 3 deliveries sufficient substance. Do not inflate parcel counts to fill time.

- A contract normally has 1 destination and 1 parcel. Before acceptance, show destination, parcel type, receipt conditions, base payment, quality bonus and known passage conditions.
- Basic contracts have no deadline. Timed contracts appear only as optional jobs, and their timer begins at acceptance. No automatic acceptance or extension.
- The host proposes the selection; all connected players must ready before departure. Contract/equipment changes clear readiness. Solo departure occurs immediately upon readiness.
- Return also requires everyone to ready. Show the abandonment list first if contracts remain. Until return is confirmed, players may continue transporting items during readiness.
- The truck remains parked at the same location throughout a shift. Departure and return use short transitions, without driver/passenger waiting roles.
- Spawn the optional contract's parcel at a counter near the truck. Do not award it remotely or duplicate an already delivered parcel.
- Receipt requires the designated parcel to be placed in its receipt area, no longer held, and satisfy the contract condition. Pay once per contract. Confirmed deliveries remove the parcel from further tool use.

### Five contract candidates

English names below are draft game copy. Each has base payment 30 and maximum quality bonus 10. The first shift offers each type once, and the crew selects 3. The optional job uses 1 unselected type at a different receipt location.

| ID / Name | Parcel and receipt condition | Safe route / optional route | Decision |
|---|---|---|---|
| C01 Paper Please | Sneezer, place on marked stand inside shop | Rear passage / paper-obstructed alley | Deliver first or use its sneeze to open a route for other contracts |
| C02 Rooftop Greens | Hopper, elevated stand inside railing | Parcel-width stairs / timed hop and catch | Carry alone via stairs or exploit height with a teammate |
| C03 Hold Tight | Clinger, yard maintenance stand | Ground detour / designated face on moving platform | Carry around safely or cross before adhesion expires |
| C04 Quiet Delivery | Standard, stand inside warehouse | Wide ramp / narrow route beside unstable shelves | Use a trolley safely or hand-carry through the short route |
| C05 Counter Service | Standard, stand behind shutter | Side-door manual latch / employee-or-parcel pressure plate | Have someone hold the door or use the solo side entrance |

The timed optional variant starts at 8 minutes. Expiry removes only its time bonus of 10; delivery still earns base payment 30. Including quality, this additional contract pays at most 50. Timer expiry never forces a return.

## 4. First district — Toypost Village

The reception square is the central hub, with shopping alleys, rooftop connections and the postal equipment yard forming 3 delivery zones. Park the truck at the square's edge. Every zone has a return connection to the square and a connection to an adjacent zone.

```text
Truck — Reception square — Shopping alleys
             |   \               |
             |    Rooftop connections
             |          |
             +— Postal equipment yard
```

Start with an approximately 160×160m envelope and roofs 3–6m above ground. Do not fill the entire envelope with walkable ground: buildings, courtyards, interiors and empty boundary space are included. Alley corners, indoor stands, roof railings and yard equipment prevent direct truck-to-destination throws. Check camera and doorway collisions together.

Current speeds are 3.2m/s empty-handed and 1.6m/s carrying forward (worker.gd (`../../scripts/worker.gd`; retired file)). The target of no more than 20 seconds of uneventful walking corresponds to 64m empty-handed or 32m carrying on flat ground. Measure real carrying routes including stops, turns and visibility. Do not estimate shift duration from length alone.

Each recipient has a fixed safe route and an optional route with a different solution. Start testing ground carrying paths at 1.8m wide and narrow optional passages at 1.2m, but check the 0.8m parcel, employee carrying collider and turning clearance in practice. Doorway width alone does not prove traversability.

### Zone situations and creature placement

- Alleys: a waiting area before the paper barrier, a shallow slope where a nearby parcel can roll during a sneeze, and a safe rear route. Separate decorative paper from passage-blocking state.
- Roofs: a short connection with mutually visible throwing/catching positions, a retrieval balcony below and guarded stairs. A long throw must not be the only solution.
- Yard: a shuttling platform, latching lever and manual detour. Distinguish the adhesive face by color and shape; prevent parcels becoming trapped underneath.
- Telegraph the packrat with a paper nest and drag marks in the yard. Stolen cargo goes to a visible nest and must be recoverable by horn or direct retrieval. No stealing in truck preparation or receipt areas. Add no new creature species in the first experiment.

### Random assembly and fairness

Author 2 variants per zone and validate the 8 combinations of 3 zones first. Keep safe connections fixed while varying optional passage states, recipient sockets, parcel combinations and events. Share the host's final layout; do not send only a seed and expect peers to independently reproduce physics.

Reject combinations without validated required paths and recovery points. If runtime validation fails, use a verified default layout and log the failed seed/configuration. Remaining destinations must be accessible even after all useful special parcels have been delivered. Events may change optional passages but never close the sole safe route.

## 5. Parcel problems, solutions and cues

Preserve the implemented sneezer, clinger and hopper values in the [current specification](02-spec.en.md). Environmental uses and recovery connections below are expansion proposals.

| Type | Trigger, cue and extent | Solution and recovery | Replicated outcome |
|---|---|---|---|
| Sneezer | Initial wait 6 seconds, later 6–9; warning 1.5, burst 0.35; forward range 4.5m, half-angle 50 degrees, vertical extent 1.6m. Expression, motion, sound and direction cue | One valid burst opens a paper barrier for the shift. A windmill opens an optional shutter for 8 seconds. On failure, wait for another sneeze or detour | Burst event ID, origin/direction, barrier opening and shutter expiry; final decorative debris positions are non-authoritative |
| Clinger | Currently targets employees/parcels within 0.95m for 5 seconds. Link and remaining-time cue. No current wall/equipment adhesion | Add designated equipment faces only, retaining 5 seconds. Do not enable arbitrary wall adhesion. Pickup detaches it; expiry inherits platform velocity and drops it toward a recovery surface | Facility ID, local attachment position, expiry and release velocity; safely detach if target disappears |
| Hopper | Currently runs only when unheld and grounded: rest 4 seconds, warning 1, forward speed 2.8m/s and lift 6.5m/s. Pauses while held | Align and set down, then catch using its warning. Add no jumping out of the employee's hands. Start testing optional single-hop stands at height differences no greater than 1.2m. Recover via stairs/balcony | Phase, remaining time, direction, authoritative position and velocity |

The hopper's theoretical free rise is 6.5²/(2×9.81) ≈ 2.15m. This excludes collider, rotation and stand clearance; it does not mean a single hop climbs an entire 3–6m roof. No special parcel becomes a mandatory unique key.

## 6. Tools, truck configurations and economy

Initial permanent unlocks belong to the host's local expedition profile and are shared by the crew. Guests can use team equipment without buying it themselves. Explain that switching hosts switches company progression. Do not duplicate earnings across companies or grant personal statistical advantages. Keep expedition saves separate from existing completion records.

| Tool | Price / equipment slots | Controls and limits | Concrete choice |
|---|---|---|---|
| Cargo Straps | 60 / 1 | 2 reusable straps per pack; secure cargo to designated trolley/truck anchors, detach to recover. Restraint suppresses movement/hopping but not sneezing | Stable loading versus preparation time; separate sneezer cargo |
| Folding Ramp | 120 / 2 | Designated placement surfaces; length 3m, width 1.4m, maximum height difference 0.8m. Solo placement/retrieval; cannot retrieve while occupied by employee/cargo | Cross a low ledge or use the wide detour |
| Parcel Trolley | 180 / 2 | At most 2 parcels, pushed by 1 employee. Target flat speed 1.8m/s; cannot climb stairs directly, ramp allowed. Brakes when released | Loading work saves trips; hand-carry individually in tight alleys |

All tools are new proposals. The first shift is completable using free basic actions, horn and pings. The shared 4-slot equipment budget is separate from space for 3 delivery parcels. No duplicate tool loading in the first experiment. Crews can choose ramp+trolley, ramp+straps or trolley+straps; filling unused slots is optional.

Test truck facilities as a free layout choice initially. Its single rear facility position accepts either a fixed rack, quickly securing 1 parcel, or an equipment shelf, moving tool pickup/storage to the side door. Both preserve the 4-slot budget. Paid facility progression and further types need a later design after usage/choice testing; they are not hidden purchases in this price table.

### Existing versus proposed economy

Current contracts.gd (`../../scripts/contracts.gd`; retired file) pays 10 per delivery, 5 extra per relay and 20 on contract success; all maximum upgrades total 110. A 4-player first success yields at least 120, allowing every upgrade at once. Do not change those current values in this task.

Expansion base earnings for 3 contracts are 3×30 = 90, up to 120 including quality. With the optional contract, earnings range from 120–170. Proposed quality bonuses are pristine 10 / scuffed 5 / damaged 0, affecting bonuses only. Normal pickup/set-down and intended tool use do not cause damage. Do not invent impact-damage thresholds before measuring them in the physics experiment.

| Example purchase path | Balance and choice |
|---|---|
| First basic shift 90, buy straps for 60 | 30 remains; another basic shift of 90 enables the ramp at 120 |
| Best first basic shift 120 | Buy ramp at 120 or straps at 60 and save |
| Save two base payouts of 90 | Buy trolley at 180 |
| Maximum first shift 170 | Below total tool price 360; cannot unlock everything |

Do not multiply rewards, parcel mass or basic contract count by player count. The 4-slot loadout choice remains after full unlock. This small tool list does not promise an entire long-term progression system; the table tests early economic choices.

## 7. Accidents, rescue and recovery

Record completed base earnings once in the company balance at receipt. Secured quality/time bonuses also survive later accidents. Abandoning undelivered work forfeits only its potential reward, without fines or permanent debt. A shift with an abandoned basic contract cannot reveal the optional job.

- Retrieve accessible dropped cargo manually. On hazard exit, out-of-bounds or verified irretrievability, restore it to the last safe point. Safe points are authored sockets on fixed ground, with parcel clearance and safe-route connections. Moving platforms and roof edges are not eligible. If none is suitable, use the truck cargo space.
- The host updates safe points only after actual traversal. Recovery cannot advance cargo toward an unreached destination or reset contract/cargo condition. Remove the pre-recovery instance to prevent duplicates and repeat payment.
- Employee incapacitation is new. A nearby teammate rescues with a 3-second interaction; dropped cargo remains on site. After 20 seconds the employee may request truck rescue. If everyone is incapacitated, show a 5-second notice and resume at the truck. Solo uses the same all-incapacitated rule.
- Rescue does not teleport all cargo to the truck. Accessible parcels still need retrieval; only inaccessible ones receive safe recovery. Completed base earnings survive rescue.
- Repeated accidents do not trigger a forced game over. The crew may continue recovery or ready together to return and abandon remaining contracts.

## 8. Online and persistence requirements

Reuse the ENet host authority, lobby and validated carrying requests in session.gd (`../../scripts/session.gd`; retired file). Do not imply Steam invitations already exist. Connect new state at an expedition-mode boundary instead of overwriting the fixed campaign.

- Host-owned state: shift ID/phase, final zone layout/seed/content version, contract-to-parcel mapping, receipt/payment ledger, parcel safe points/condition, facility states/tool ownership and ready roster.
- Client requests: proposed contract selection, readiness, pickup, tool use and rescue. The host validates membership, distance, ownership, contract phase and already processed events. Clients never decide payment amounts or final cargo poses.
- Send layout at join/shift start; important contract/payment/facility transitions use reliable events with recoverable state; continuous motion follows existing interpolation. A seed is not a physics synchronization mechanism.
- On guest departure, the host safely releases held items and updates readiness. Rejoining/late-joining players receive current state before spawning at the truck. Do not recreate cargo or rewards.
- Host migration is outside the first experiment. Host departure ends the session with a clear notice while saved company earnings/unlocks remain. Mid-shift resume is outside scope; unfinished contracts end.
- Use a separate versioned expedition save. Atomically record balance, unlocks and delivery payment IDs to prevent duplicate payouts on replay/restart. Do not change or overwrite run_profile.cfg. Bump the protocol and reject older clients when actually implementing wire changes; current protocol 12 stays unchanged in this documentation task.

## 9. Differences from current implementation

| Feature | Classification | Verified basis and expansion work |
|---|---|---|
| Movement, pickup, throw, catch, pings | Reuse | worker.gd (`../../scripts/worker.gd`; retired file), cargo.gd (`../../scripts/cargo.gd`; retired file), pings.gd (`../../scripts/pings.gd`; retired file). Keep fixed carrying |
| Directional sneeze, paper response | Reuse + modify | sneeze_rules.gd (`../../scripts/sneeze_rules.gd`; retired file), reactive_props.gd (`../../scripts/reactive_props.gd`; retired file). Add barrier gameplay state and windmill connection |
| Adhesion | Modify | Extend employee/cargo targets in clinger.gd (`../../scripts/clinger.gd`; retired file) with designated facility faces and detachment; not arbitrary wall adhesion |
| Hopping | Reuse + map authoring | hopper.gd (`../../scripts/hopper.gd`; retired file) pauses while held. Add reachable stands and recovery routes |
| Doors, conveyor, packrat | Reuse + modify | route_challenges.gd (`../../scripts/route_challenges.gd`; retired file), conveyor.gd (`../../scripts/conveyor.gd`; retired file), packrat.gd (`../../scripts/packrat.gd`; retired file). Add expedition activation, nest and safe-zone connections |
| Delivery and economy | Modify + new | cargo_rules.gd (`../../scripts/cargo_rules.gd`; retired file), contracts.gd (`../../scripts/contracts.gd`; retired file), main.gd (`../../scripts/main.gd`; retired file). Add recipient contracts, immediate payment, quality and expedition phases |
| Cargo recovery | Modify | cargo.gd currently resets a crate after recovery delay. Add preserved contracts and last-safe-socket recovery |
| Truck, tools, permanent unlocks, rescue | New | Not confirmed as current features. Require preparation, placement, ownership, persistence and rescue rules |
| Map assembly and invitations | New / separate | Add assembly/content-version checks. Internet/Steam invitations remain the separate NET-01 workstream |

## 10. Production order and validation

1. This documentation task: rules, content, values, differences and validation criteria; no game changes.
2. Proposed next implementation: connect truck preparation → 3 contracts → immediate rewards → return in 1 fixed layout; preserve existing modes.
3. Connect parcel tool uses, routes, 3 tools and rescue; verify solutions for solo and 2–4 players.
4. After the fixed layout works, add 2 variants per zone and all 8 combinations, late join and save/recovery regression checks.
5. Reassess economy, travel time and content scale after newcomer cooperative testing. Automated passes do not establish release quality.

### Future automated checks — not executed

- [ ] Check safe routes, parcel clearance, receipt and recovery sockets across 8 zone combinations × 10 basic contract selections × 4 player counts = 320 base configurations. Separately check event/location variants with 100 fixed seeds.
- [ ] Remaining required contracts remain possible after each special parcel is delivered first or lost/recovered.
- [ ] Verify basic earnings 90–120, additional-job totals 120–170, tool prices, 4-slot loadouts and player-count-independent income.
- [ ] No duplicate payout/cargo on repeated receipt, event retransmission, reconnect or restart after saving.
- [ ] Safe handling of solo/all-crew rescue, guest/host departure, readiness changes, deleted facility targets and occupied recovery sockets.
- [ ] Contract, facility and cargo results match across peers, excluding decorative debris; reject protocol/map-version mismatches.

### Future human playtests — not executed

Check solvability with 1 solo run, 1 two-player run and 1 three-player run, then have 2 newcomer groups of 4 each play 2 shifts. External-network and endurance testing remain separate items in the [release checklist](05-validation.en.md).

| Observation | Initial revision criterion |
|---|---|
| Delivery-order/route discussion | Record at least 2 distinct choice reasons per four-player group |
| Parcel tool use | Each group attempts at least 1 use without explicit facilitator instruction |
| Accident recovery | Record causal explanations and retrieval/detour examples for actual accidents; 0 progression deadlocks |
| Travel/waiting | For every video segment exceeding 20 seconds with no meaningful activity, record cause and proposed fix |
| Equipment/optional job | Each group explains selection/decline reasons; 0 required-progress failures due to missing a tool |
| Timing/retry | Compare actual timing against basic 25–30 and optional-inclusive 30–40 minutes; ask what they would change next shift |

Human-test numbers guide iteration; they are neither statistical proof of fun nor Steam release gates. Do not lengthen a short run if its choices are already meaningful.

## 11. Unknowns and maintenance

Impact-damage thresholds, large-map camera/performance, adhesive moving-platform stability, trolley feel, actual shift duration and long-term content consumption remain unverified. Determine detailed physics values through implementation experiments; this plan is not evidence. Paid facility progression and later regions need design after the first experiment.

Link the approved future direction from the overview while preserving current specification values. Track EXP-01–EXP-05 in the [backlog](04-backlog.en.md), updating related guides, validation and both change logs together. Distinguish design agreement from implementation and actual release approval.

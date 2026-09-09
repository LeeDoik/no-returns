# Exploring a larger NO RETURNS design

[한국어](08-expansion-directions.ko.md)

Written 2026-09-09. Status: broad exploration and recommendations requested by the user, not an approved implementation specification, production scale or release scope. Current behavior follows the [specification](02-spec.en.md).

## Revised perspective

The previous analysis focused on improving the existing Shipping Shrine and equipment values. At the user's request, this exploration includes travel to destinations, randomized maps and larger game structures. One implemented map does not set a ceiling on future content. A small validation segment tests a larger vision; it does not define the finished game's size.

The missing layer at this scale is a journey of choosing destinations, cargo, routes and risk. Existing pickup/throw/catch actions can solve obstacles within that journey. The 120-versus-110 equipment economy remains a diagnosis of current rules, but does not require price tuning before choosing a long-term direction.

## Three directions to compare

| Direction | Central experience | Main choices | Appeal and design demands |
|---|---|---|---|
| Cooperative delivery expeditions — initial recommendation | Overcoming a different delivery district with the team each run | Contracts, loading, delivery order, routes, optional jobs | Living cargo can create and solve journey problems. Generated routes must be fair, traversable with parcels and built around worthwhile recurring situations |
| Delivery truck road trip | A shared mobile base and incidents across multiple stops | Driving route, stops, loading, vehicle condition, on-foot deliveries | Events connect road travel and cargo management. Non-drivers need meaningful roles; driving and carrying both need attention |
| City delivery life | Learning a city and developing a personal delivery practice | Clients, local reputation, time of day, contract combinations, business investment | Builds attachment to places and customers. Districts, clients and world state must respond sufficiently, avoiding empty back-and-forth journeys |

Expeditions are recommended because existing parcel behavior and cooperative carrying can remain central to the larger structure. If combined with a road trip, the vehicle handles inter-district travel, loading and preparation while on-foot segments handle precise transport problems. This is not a specification combining all three directions immediately. City life instead centers a persistent world and long-term relationships.

## Recommended vision

**A cooperative adventure about loading living parcels, heading to a different delivery district each shift, and completing the day's work through team-chosen routes and inventive transport methods.**

Beyond comic accidents, create anticipation before departure, curiosity about routes, difficult choices between remaining cargo and risk, and relief after the final delivery. Fellowship, Challenge and Discovery remain central in MDA terms, supported by the Fantasy of being delivery workers and the sequence of incidents within a shift. These are desired experiences, not validated outcomes.

## Expanded loop

**Review jobs → choose cargo and equipment → load and plan routes → travel and explore → approach recipients and transport parcels → solve parcel/environment problems → satisfy receipt conditions → gain rewards and information → choose another delivery or return → prepare for the next shift.**

Three loops should exchange information. A held parcel's traits change which alley to take; resources remaining after that route change whether to accept another job; the day's rewards and discoveries change tools and districts chosen next time.

- Moment loop: observe → grab/move/throw/catch → read the response → adjust the next action.
- Delivery loop: review destination information → choose an approach → position cargo and teammates → solve delivery → assess the next destination.
- Shift loop: choose jobs/loading → make several deliveries → optional extra work or return → settle/prepare → start a shift under different conditions.

Instead of escalating the same timer every time, consider selectable pressure through individual deadlines, risky shortcuts and optional jobs. A successful delivery could provide information or access for the next destination, reducing repetition that only increments a score.

## Making travel playable

Do not merely increase walking distance. Routes need transport advantages and drawbacks. A rooftop path might be short but require throw/catch coordination; an alley might be stable but awkward for a clinging parcel in narrow passages; a lift might be convenient but require timing. These are proposed situations, not claims about the existing map.

Prefer changing situational roles over fixed classes. A scout marks a route, becomes the receiver in the next segment, and another worker checks remaining cargo. If direct vehicle driving is chosen, design activity for driving, navigation, calming cargo and repairs together. Passengers merely waiting for the next stop would weaken travel as play.

## Units of map randomization

The recommendation is to combine districts with designed purposes/rules and vary their connections and conditions. Harbors, rooftops, markets and factories can pose different transport problems. Route networks, destination order, passage states, weather, cargo combinations and incidents should provoke different decisions. Both free terrain generation and combinations of authored areas remain candidates; the recommendation favors control over cooperative situations and deliverability, rather than cost savings alone.

Randomness should change solutions, not just appearance. The same bridge should admit different choices depending on cargo, time and tools. Hiding all important information would turn planning into guessing. Consider revealing destinations and known road conditions before departure while leaving detours and local incidents to exploration.

Generation checks should cover spawn-to-destination connectivity, parcel dimensions and passage widths, recipient access and recovery, solutions for solo and each crew size, and readable clues/risks. Online players must share the same generated layout and incident state. Unreachable destinations or permanent loss of required cargo are not automatically comic randomness.

## Expanding the role of living parcels

Current parcels primarily complicate transport. The expansion could let their traits function as tools: a sneeze activates a device or pushes an obstruction, adhesion carries a parcel on a moving mechanism, or a timed hop helps reach an elevated recipient. All are proposed new interactions, not existing supported features.

Using cargo as a tool should involve delivery delay or recovery cost to create a choice. Delivery order could pose the question: delivering this parcel now pays sooner, but removes a useful tool for the next obstacle. A special parcel serving as the only key would create a dead end when lost, so alternative routes or recovery mechanisms are needed.

## Example shift

This is illustrative; duration, district count and included elements are not fixed. The team loads parcels for several recipients around a harbor. Flooded passages force a choice between a rooftop shortcut and a longer detour. A scouting teammate takes a receiving position while another throws. A hopping parcel disrupts the plan, and the team recovers scattered cargo while reconsidering which parcel to use as a tool and which to deliver first.

The first recipient provides access to the next area. After delivery, a new job arrives, prompting a choice between continuing with remaining resources and returning. Back at base, the team considers tools with different transport capabilities, vehicle loading arrangements or new job types alongside simple speed boosts. Even failure should leave something learned and a reason to change the next attempt.

## Progression and risk rewards

Do not expand spending only through higher prices. Consider tools, loading methods, district access and contract types that change next-shift choices. Permanent progression is a candidate, not current implementation; numerical accumulation should not undermine new teammates' participation or cooperative roles.

Optional jobs compare additional rewards with risks to remaining cargo, time and equipment. Completed-delivery earnings, losses affecting cargo still aboard and return bonuses can be designed separately. One accident erasing all progress, or resource shortages creating an unrecoverable failure, are separate choices rather than assumptions.

## Vision scale and validation order

This concept can support large areas and many shifts. Its first experiment should test branching routes, different receipt conditions, usable cargo traits and an extra-delivery-or-return decision connected within one journey. That experiment's size is not the ceiling on the finished game.

Observe whether players discuss delivery order, change plans because of map conditions, solve problems through teammates and parcel traits, remain meaningfully active during travel, and want to try another method next shift. Record completion time, help requests, waiting segments and behavior changes after accidents. Metrics and pass thresholds are not yet agreed.

## Status and next decision

The current direction question is whether expeditions, truck road trips or city life should provide the central feeling. Pending an answer, expeditions anchor the comparison. This task changes planning documents only, not gameplay code, current release features or the existing Word worksheet's description of current implementation. Read the [MDA analysis](07-mda.en.md) and [backlog](04-backlog.en.md) as current-state analysis, and this document as future possibilities.

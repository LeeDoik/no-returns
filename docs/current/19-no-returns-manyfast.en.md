# NO RETURNS — Manyfast detailed concept

[한국어](19-no-returns-manyfast.ko.md)

> **Historical record · retired/superseded 2026-09-12.** The [PSX space-delivery design](01-overview.en.md) is current. Implementation, launch, art and values below are historical, not current status. Deleted files remain path records only.


2026-09-12 · Revision 3 · Direction confirmed / detailed design proposed

[Manyfast](https://manyfast.io/editor/427d2942-d820-4899-abbf-664df084d5ed?map=prdMap) · [Current Unity state](18-unity-mainline.en.md) · [Expedition detail](08-expansion-directions.en.md)

## Product definition

NO RETURNS — a 1–4-player 3D third-person cooperative delivery expedition: arrive with living parcels, choose routes with friends, recover from mishaps and complete deliveries. Target PC/Steam, with Unity as the main development platform.

## Decisions and current state

Revision 3, 2026-09-12. User decision: discard SIDE EFFECTS and deepen NO RETURNS. Its magic tools, combat, bosses and previous implementation brief are not active requirements. Continue the existing NO RETURNS expedition direction and approved D art.
Current Unity work is CarryLab movement, camera, physical carrying and throwing. Godot campaign and EXP-01 delivery/contracts/saves/networking are reference implementations, not completed Unity ports. Details below are proposals, not finalized release specifications or validated fun.

## Core Fun

Core fun is inventing delivery methods together while handling parcels with distinct behaviors.
Handling mastery: coordinate grip direction, throwing strength and receiving position.
Cooperative judgment: rotate opening routes, carrying and recovery rather than assigning a permanent driver or waiting role.
Delivery order: deliver the sneezing parcel now or first use it to open a shortcut for another delivery.
Preserved achievement: completed deliveries remain earned; mishaps create recovery, detour and rearrangement problems. The game must be enjoyable with friends without an audience.

## Experience problems

Hypotheses: repetitive walking makes delivery feel like labor. Unclear grabbing and camera occlusion frustrate rather than amuse. Losing completed achievements after one mishap discourages retries. One expert doing everything leaves others waiting. Establish actual frequency and impact through human play.

## Product design

Art: retain approved D-style toy postal village, uniformed workers, expressive cardboard parcels and recessed conveyors. Combine team colors with names/patterns; communicate sneeze buildup, adhesive state and jump telegraphs through motion and sound. Do not switch to fantasy combat art.
Truck: a stationary shared preparation, equipment exchange and return base. Direct driving is excluded.
Contracts: propose selecting 3 from the existing 5 candidates. Reveal destination, parcel, acceptance condition, reward and known route conditions before acceptance. Basic contracts have no deadline.
Contracts: C01 Paper Please delivers a sneezer to a shop stand, comparing a paper-barrier shortcut with a detour. C02 Rooftop Greens delivers a hopper to a rooftop stand, comparing stairs with teammate catches. C03 Hold Tight delivers a clinger to a maintenance bench, comparing a ground detour with designated attachment surfaces. C04 Quiet Delivery takes a normal parcel to a warehouse via a broad route or narrow shelf passage. C05 Counter Service takes a normal parcel behind a shutter via teammate pressure-plate use or a solo side door. Facility attachment and barrier clearing are new designs, not current Unity features.
Map: connect the reception plaza, shopping alley, rooftops and service yard. Each destination has a safe route, optional route and recovery route. Delivering one special parcel first must not block other contracts.
Randomness: select validated room connections, contract combinations and selected device states at shift start, sharing the result with everyone. Never randomize control rules while carrying. Exclude disconnected mandatory paths and unrecoverable placements. Validate a fixed layout first.
Equipment: straps at 60/1 slot, ramp at 120/2 slots, trolley at 180/2 slots and a shared 4-slot budget are existing experimental proposals. Loadout choices remain after all unlocks. The first shift must be completable without purchases. Tune prices and physical performance from play.
Rewards: begin testing at 30 per basic delivery and 90 for 3 contracts. Pay once at delivery confirmation and retain earnings after later mishaps. Wrong deliveries leave the parcel in place and explain the mismatch. Validate quality and timed-extra bonuses after the basic loop.
Recovery: retrieve reachable parcels manually. Restore unreachable parcels to previously reached safe positions or the truck while preserving contracts. Rescuing workers must not teleport their cargo. Avoid long spectating and permanent debt as primary penalties.
UI: show grab target/unavailable reason centrally, destination and reaction telegraphs on parcels, and selected contracts/completion/earned rewards in the HUD. Preparation holds contract details; travel emphasizes current objectives and teammate pings. List unfinished contracts before return.
Networking/saves: host resolves contracts, parcel ownership, delivery and payment. Duplicate requests/reconnection must not duplicate parcels or money. Guests use shared company equipment. Host departure ends the session with an explanation and retained confirmed achievements. Unity networking selection and resuming unfinished shifts require separate decisions.

## Differentiation hypothesis

Parcels are both delivery objectives and environmental tools that change delivery methods. Delivery order affects map use; the same parcel can travel safely or enable a risky shortcut. Replay variation comes from the team's order, routes and loadout, not merely the number of random mishaps.

## Audience

Players and streamers who enjoy talking, cooperating and light physical mishaps with friends. Solo players use safe routes; groups of 2–4 create alternatives through task sharing and handoffs. Maintain English source game copy and Korean counterparts. Steam is a target, not evidence of registration, invitation integration or release.

## Core Loop and first playable

Moment: observe parcel telegraphs → grab/place/throw/catch → reaction → recovery or combination.
Delivery: identify destination → agree order/route → carry/use facilities → confirm receipt → retain reward.
Shift: select contracts/loadout → basic deliveries → optional extra or return → settlement → next loadout. The full-game 30–40-minute duration is a target, not a reason to pad walking.
Example: use the sneezer to open the paper alley while a teammate retrieves another parcel knocked away. Carry the hopper through the shortcut, then choose a rooftop throw/catch or stairs. Discuss another delivery or return after completing the work.
Next Unity validation order: connect normal-parcel pickup→destination receipt→single payment→truck return first. Add a sneezer and safe/optional routes, then test handoffs and recovery with 2 players. Expand remaining contracts, equipment and randomized areas afterward. Do not include the entire progression system in the first sample.

## Validation criteria

Automated checks: no duplicate payments after wrong/repeated receipt, reconnection or save reload; contracts survive recovery; mandatory paths remain traversable; declining extras permits normal return.
Human play: record understanding of destinations and failed grabs without explanation, alternative uses of the same parcel, and reasons for route/equipment choices. Analyze idle walking/waiting intervals over 20 seconds. Players should explain mishaps and continue deliveries.
Start solo and with 2 players, then test 3–4. Report local multiple processes, separate PCs, external networking and human enjoyment separately. Numbers guide experiments, not release approval.

## Risks and boundaries

Risks: repetitive carrying labor, camera/wall collision frustration, uneven workload, special parcels becoming mandatory keys that block progress, early economic exhaustion and networked physics costs. Observe a short fixed delivery loop first.
Open decisions: impact damage thresholds, final movement/trolley speeds, bonus/economy balance, actual shift duration, Unity networking and final-art performance. Godot values/tests do not prove Unity completion. This documentation task neither changes code nor sends execution instructions to a development agent.



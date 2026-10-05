# NO RETURNS — consolidated cooperative design v1

[한국어](coop-design-v1.ko.md)

## Current implementation — multiple deliveries 0.9.24

Implemented 3–6 physical parcels by crew count, independent carrying, verification pause/resume, receipts and district bundles. Game 0.9.24 / protocol 15 / TCP 27842. This supersedes the single 420 CR delivery for normal Cinder sessions. [Current rules, production evidence and remaining work](multiple-deliveries.en.md).

2026-10-05 · Design baseline NR-COOP-01 · Not an implementation completion claim

## Status and precedence

Consolidates the user's 2026-10-04–05 answers and delegation to finish planning without further questions. **[Selected]** means a direct user decision; **[Completed]** means an initial design supplied under that delegation. Completed decisions are not separate user approvals or proof of fun. All numerical tuning below is **[Completed · initial tuning]**, not values extracted from current code.

This document governs target behavior for new work. It overrides conflicting overview, economy drafts and creature experiments, but historical results in the [implementation specification](02-spec.en.md) do not validate these new rules. This change is documentation only and does not increment the game version. Repeated questionnaires end here; subsequent revisions should come from implementation findings and actual play.

Implementation transition: [0.9.23 visit ledger, revival and departure](visit-settlement.en.md). Some rules now connect to one delivery; the full design below is not implemented. Total reward 420 CR, rescue hold 2.5 seconds and existing danger stages still retain earlier values.

## 1. Product promise

[Selected] A first-person space-delivery cooperative game for 1–4 players. Fear and tension lead; humor emerges from teammates' mistakes and responses. Cargo controls stay simple; danger comes from the environment, creatures and crew actions. Employees and creatures are treated seriously. Avoidance, distraction and some combat coexist, but shooting does not clear all powerful creatures.

[Selected] Equipment determines roles instead of fixed classes. Crew generally travel together and divide hauling, scouting, distraction and facility operation. Money buys equipment choices and ship decoration, without a numerical upgrade tree. Mystery is discoverable background fiction, not a required investigation or ending.

[Completed] Success is **extracting some earnings after judging risk**, rather than finishing every parcel. Target site duration is 15–20 minutes. Multiple destinations form a session without daily quotas, debt or forced campaign resets. Keep the dirty, aged PSX industrial look, open sky with compact alleys, and readable inner/outer boundaries.

## 2. Visit and contract flow

[Selected] Prepare aboard ship → choose destination → automatic travel/landing → choose deliveries and explore → receiver verification/receipt → more delivery or extraction → settlement/purchases. No manual piloting. All destinations are initially available; distant ones cost money to reach. A free destination always remains available. Before travel show environment, approximate risk, base delivery count and individual rewards, but not creature types or hidden bonuses.

[Selected] All base cargo starts aboard ship. Reveal the destination district, then require building numbers and signs to locate receivers. Deliver any subset without unfinished-contract penalties. Longer, riskier deliveries pay more. When a district has one base delivery left, the ship terminal reveals that delivery and its bundle bonus. No global notification or company radio announcement.

[Completed] District bonuses count only that visit's base delivery bundle. Hidden extra deliveries never change the requirement. All users see the same terminal state. Provide a district map and address list aboard ship without exact receiver/creature HUD markers. Address signs, return routes and cart detours must be readable.

[Selected] Major geography persists while deliveries, creatures and some passages/entrances vary per visit. Local controls and cutters open routes. Every ordinary delivery is possible without a specific tool; some bonus areas require one. Bonus areas contain extra delivery cargo, narrative clues and decoration collectibles.

[Completed] Generation must validate a walkable route to every base receiver. Not every route supports carts, but unloading locations must exist. Field-discovered deliveries reveal destination/reward before hauling and have no abandonment penalty. Discovered clues immediately persist in the team archive. Decoration collectibles occupy a small quest-item record and must leave on a successful extraction to unlock. A wipe loses unextracted collectibles but preserves discovered narrative records.

## 3. Cargo, receivers and carts

[Selected] Carry one parcel by hand. Movement and simple doors/buttons remain available, but baton use, special equipment and rescue require putting cargo down. Cargo cannot break from impact or attacks. Placement at a receiver automatically starts verification and attracting noise. It continues without nearby crew. Picking cargo up pauses progress and noise; replacing it at the same receiver resumes progress. Completion is communicated only by local sound/light. Creatures cannot erase verification or receipts. One button press collects the receipt immediately into the shared delivery record, with no physical receipt hauling. Collector incapacitation does not erase it, but a wipe loses the earnings.

[Completed] Cargo ID must match the receiver; a mismatch displays an address error without starting noise. Creature presence does not automatically interrupt verification. Completion sounds are audible to nearby creatures too. Receipt/reward delivery is idempotent across duplicate interaction and reconnects. Progress belongs to the cargo/receiver pair and visit.

[Selected] Carts are separately purchased shared equipment, not personal-slot items. They are noisy, cannot pass stairs/narrow routes, and pushing blocks weapons/tools. Downed crew can be loaded. Carts left outside are lost even on a wipe; return them to the ship loading area to retain them.

[Completed] Initially allow one cart per team with three cargo spaces. A downed employee uses two spaces; multiple patients cannot ride simultaneously. Interaction snaps cargo into secure positions instead of making fiddly physics/balancing the challenge. Receipts need no cart space. Falling structures can obstruct a cart but do not instantly delete it; unload and detour. Platforms/debris must never permanently block the only return path.

## 4. Escalation and departure

[Selected] Fog thickens with time and controller stages advance until outer creatures enter delivery districts. Crew cannot slow or reverse progression, including while regrouping aboard ship. Read fog, machinery sound and creature movement rather than a numerical timer/stage HUD. Noise, combat and facility operation create additional danger.

[Completed] Noise stimulates local AI but never accelerates the controller's world clock. Separate environmental reaction from global time pressure so causes remain learnable. Stages are stable → weakening → incursion → final departure. Losing teammates does not reset or slow stages. Creatures enter through designated outer routes, never spawn beside players.

[Selected] At the final stage, radio announcements, horn and engine startup warn of automatic departure without exact remaining seconds. Automatic departure cannot be canceled. Anyone aboard may start/cancel manual departure and leave teammates behind. Pursuing creatures do not prevent departure.

[Completed] Automatic departure overrides manual cancellation. Manual departure also uses sound and an occupancy display, but cannot extend the final deadline. Resolve occupancy once at liftoff. Living occupants, downed crew loaded inside, and a paid revival currently running are valid occupants. Downed occupants enter the rescued state after departure and retain low health. Downed/eliminated people outside do not count.

[Selected] If all living crew are eliminated while a paid revival is running, it still completes. A pending revival alone is sufficient for successful extraction. With neither occupants nor a pending revival, the visit fails and loses all unbanked earnings. On a valid extraction, abandoned teammates rejoin after return-fee deductions and retain their held personal equipment.

## 5. Health, rescue, revival and settlement

[Selected] In co-op the first down allows rescue; the second causes immediate elimination. Bleedout also eliminates. Dragging/cart transport does not pause the rescue deadline. Dragging blocks baton/tools. Rescue restores normal actions with low health and brief invulnerability. Solo grants two self-revives per landing, with low health and the same protection. Co-op has no self-revive.

[Selected] A living crew member selects one teammate at the ship revival device. Revival runs automatically while the operator may move freely; only one runs at a time. Insufficient unbanked earnings block revival; existing savings cannot pay. Completion restores full health and resets down count. Consumed medicine/decoys are not restored. No passive healing or free ship healing. Survivors retain injuries after extraction. Medicine instantly restores full health without resetting down count.

[Completed] Reserve/deduct the revival fee when starting and reject duplicate selection. No discretionary cancellation/refund. A revival pending at liftoff completes and is not charged twice. Eliminated crew rejoining after failure/extraction return at full health, retaining only unconsumed held supplies. Free failure recovery generates no money. Down count resets on the next landing too, but surviving crew health persists. Solo self-revival spends its charges and cannot remotely start ship revival alone.

[Selected] Rewards bank only on departure. Boarding to regroup/revive is not settlement. A wipe loses all current unbanked earnings while preserving existing savings/held personal gear. No debt; settlement floor is zero. Unfinished deliveries are returned without penalties.

[Completed] Settlement = max(0, receipted base/extra rewards + earned district bonuses − started revival fees − abandoned-person return fees). Revival spending power includes only bonuses already earned. Wipes instead pay zero without refunding earlier purchases/travel. Failure starts a new visit rather than resuming the failed one. Waiting downed players do not postpone a wipe; a running revival does. Post-rescue protection ignores damage and is not canceled by acting.

[Completed] On down, special gear remains locked in inventory and only hand cargo drops. Rescue restores its use; elimination reserves it for return. Deliberately dropped equipment has no protection. Cargo falling out of bounds returns to the nearest accessible drop location without automatically awarding delivery credit.

## 6. Equipment and loss

[Selected] One special slot per co-op player, two in solo. Basic baton, radio and cargo are separate. Field swapping is allowed. Deliberately dropped gear left behind is lost; gear held at elimination is retained. Purchases are allowed only after leaving the site. Bring only equipped quantities with no spare ship storage/restocking while landed. Remove the folding barricade and ammunition-based bolt weapon.

[Completed] Allow duplicate types both per person and across the team. Solo may bring two medicines but forgo detection/cutting. No forced classes or duplicate bans. Spare owned reusable tools remain in company storage that never lands. Equip from it before travel; unavailable items cannot be accessed on site. Each reusable copy requires a purchase; remaining consumable quantities persist. Consuming all charges frees the slot for field pickups. Solo still uses only one of its two tools at a time.

| Equipment | Selected function | Completed operating rule |
|---|---|---|
| Basic baton | Brief stun/push, no kills, regenerating energy; some creatures immune | Short melee range; misses spend energy. Creatures gain repeat-stun resistance to prevent permanent lockdown. No friendly health damage or action interruption |
| Life scanner | Periodic untyped dots for all creatures in range, visible only to user; signal/noise attract danger, screen/static distract; battery regenerates while off | Detect through walls but not precise elevation. Screen occupies peripheral space without forced central blackout. All creatures show as dots but signal response varies; Listener reacts only to operating sound |
| Noise decoy | Three per slot, activates on landing then is consumed; chase diversion depends on species | Show throw trajectory before use; active decoys cannot be recovered. Species-specific sound priority; Listener prioritizes an audible active decoy and picks nearest among multiple |
| Portable cutter | Locks/routes only; unlimited use, time and loud noise costs; interruption resets; opened routes stay open for visit | No creature attack. Movement, damage or release interrupts. Ordinary delivery routes have manual-power/detour alternatives; bonus-only entrances are exceptions |
| Plasma cutter | Regenerating energy, paced shots, multiple hits even for weak creatures; friendly fire; pressure-pipe and hanging-support interactions | Brief regeneration delay after firing prevents sustained infinite fire. Pipes/supports have recognizable materials/shapes. Does not replace the portable cutter's lock opening |
| Medicine | One use per slot; instant full heal for self/ally; no down-count reset | Living injured targets only, short range and line of sight. Rescue downed targets first. Invalid/duplicate simultaneous healing consumes only one valid dose |

[Selected] Explain basic usage but leave most side effects and creature interactions to discovery. Outcomes follow stable rules with environmental/species differences, not random malfunctions. No automatic solution encyclopedia.

[Completed] Disclose controls, prices, remaining charges and friendly fire before purchase. Couple light, sound and creature reaction consistently so consequences can be learned. Build tension through spatial judgment and risky use rather than obscuring basic equipment operation.

## 7. Creatures and environmental traps

[Selected] Mix common and location-specific creatures. Listener senses only sound. Crouched footsteps are inaudible, but voice/radio/tools are not. It reaches the last sound location, gives a brief sound/pose warning, then attacks that point. If the player moved, it misses and immediately searches; without new sound it eventually leaves. Contact triggers search rather than an instant attack. Chase speed exceeds player sprint. Periodic calls/clicks become more frequent in pursuit. One hit is survivable; a second before healing downs the player.

[Selected] Baton briefly stuns Listener. Direct plasma cannot kill it, environmental traps can. Audible decoys divert it even during pursuit. Scanner signals themselves do not attract it (the earlier signal-response answer was explicitly corrected). It enters open ship doors but cannot pass closed ones. Through closed doors it hears loud activity, not normal voice/radio.

[Completed] Being shot does not magically reveal the attacker; investigate impact/firing sounds only. Return to patrol after search. When a decoy ends, search its location and follow new sound. Lock the attack point when windup starts so players can dodge sideways on the cue.

[Selected] Pressure-pipe releases and falling structures can kill some strong creatures and damage teammates. Outer creatures enter delivery districts over time. Some creatures that actually follow players to the ship can threaten crew through closed doors.

[Completed] Limit the initial roster to the following three species. Names/appearance are not new approved art.

| Creature | Role and sensing | Counterplay and ship behavior |
|---|---|---|
| LISTENER | Selected common sound hunter described above | Decoys, quiet movement, baton; environmental kills; closed doors block |
| SCAVENGER (working name) | Common weak visual creature patrolling cargo routes; investigates sound then confirms line of sight | Baton control/plasma kills; cover breaks pursuit. Decoy works after losing sight; scanner signal ignored. Cannot pass closed doors |
| STRIDER | Strong Cinder-specific outer threat entering during incursion; senses sight, loud sounds and scanner signals | Immune to baton/direct plasma; traps only interrupt briefly. Can break a closed ship door after warning if it pursued crew there. Decoys work before chase/after losing sight. Departure remains possible |

[Completed] STRIDER has no permanent wall-penetrating player knowledge. Ship attacks require actual tracking, not automatic placement at the ship as time passes. Door breaking has sound/shaking warnings; restore door function on the next landing. No ship health or persistent repair expense. Pipes release once then stop, dropped structures cannot be reset during the visit; restore both on a new visit. Cargo/receipts cannot be destroyed by this damage.

## 8. Voice, spectating and controls

[Selected] Proximity voice plus a basic radio. Both voice and received radio audio are creature-detectable. Reception always stays on. Automatic transmission is configurable; disabling it uses push-to-talk. Eliminated players speak only to other eliminated players and freely switch between living teammates' first/third-person views.

[Completed] Default radio to push-to-talk; proximity voice may use voice activation or push-to-talk. Radio transmission creates local voice at the sender and sound at receiving radios. Avoid playing the same voice twice through nearby/radio paths. World radio loudness is fixed while user playback volume, captions and hearing-accessibility controls remain available. Local muting does not remove AI noise. Spectator audio/radios emit no world noise. Do not attempt to police Discord or other external chat.

[Completed] Voice-driven AI uses microphone level/activity only, without semantic analysis or recording. Provide noise-floor calibration, input meter and transmission indicator. Server validates state/range/rate. For players without microphones, offer proximity/radio text and short calls with the same spatial/noise rules. No automatic enemy-location ping. Accessibility captions expose audible cues/direction without revealing previously undetected creatures.

[Completed] Core inputs are movement, look, sprint, crouch, interact, drop, equipment switch, primary use and radio. Block tool use while carrying with a put-down hint instead of automatically dropping cargo. Departure/purchase use explicit buttons without voting. Offer full rebinding, toggle/hold options, sensitivity/FOV and reduced shake/flashes. Medicine target is shown clearly: aim at a valid ally or explicitly self-heal when targeting empty space.

## 9. Economy, saves and party changes

[Selected] Team owns money, purchases and decorations; anyone may buy. Persist in the host's save with changing participants allowed. All destinations are selectable immediately but paid routes cost money; a free route always exists. No numerical upgrades, personal wallets or debt.

[Completed] Server atomically validates payment and grants items; share recent purchases. Store unbanked ledger, settlement IDs and item IDs to prevent duplicate grants. Save safely in preparation; on-site saves restore the same visit checkpoint. Host exit suspends the session and resuming restores visit, danger clock, injuries, consumption and revival progress. Journal consumption/settlement events so force-quitting cannot restore supplies/time. Shared cloud ownership and host migration are outside initial scope.

[Completed] Lock contracts/creature counts to party size at landing. Mid-visit newcomers spectate and equip/join next landing. Disconnected participants can reconnect to their existing character; the body remains for 60 seconds, then becomes eliminated. Down/disconnect must not regenerate equipment or rescale the visit. Distinguish host suspension from participant disconnect.

[Completed] Solo scaling changes delivery count, simultaneous creature count and placement, not sensing rules/damage. Prevent one spawn permanently occupying the only passage. Two-player routes must support a carrier and a responder. Intentional wipes for free recovery cost unbanked earnings, travel and time; do not add debt or equipment confiscation to prevent them.

## 10. Initial tuning — all completed, not play-validated

These are starting values to make implementation actionable, not current executable values or validated balance. Preserve selected targets: 15–20-minute visits, equipment slots, 3 decoys, 1 medicine and 2 solo self-revives.

| Item | Starting value |
|---|---|
| Visit stages | Stable 0–7 min, weakening 7–12 min, incursion 12–17 min, final warning at 17 min, automatic departure at 18 min |
| Manual departure | 12 seconds, cancelable by anyone; cannot exceed the 18-minute automatic deadline |
| Base deliveries by players | 1/2/3/4 players: 3/4/5/6 parcels, up to 3 districts; bonuses only for districts with at least 2 parcels |
| Verification | Standard 25 seconds, difficult receiver 40 seconds |
| Rewards/fees | Easy 100 / normal 150 / risky 220 CR; district bonus 25% of its base total, rounded down. Revival 100 CR, return fee 100 CR per abandoned person |
| Routes/starting wallet | Free Cinder, later routes 200/400 CR; starting shared wallet 400 CR |
| Prices | Scanner 160, 3 decoys 60, portable cutter 180, plasma 220, 1 medicine 40, cart 250 CR |
| Health/rescue | Max 100; Listener damage 60; rescue/self-revive health 30; protection 4 seconds; rescue hold 3 seconds; bleedout 45 seconds; ship revival 10 seconds |
| Solo self-revive | Available after 3 seconds down, hold 2 seconds; cancellation spends nothing, completion spends 1 charge; use before bleedout |
| Baton | Energy 100, 1 use spends 40; after 2 seconds without use regenerate 20/second; stun 2 seconds with 4-second restun immunity afterward |
| Scanner | Range 20 m, interval 3 seconds, continuous use 30 seconds, full recharge 20 seconds; operating sound 8 m |
| Decoy/cutter | Decoy 12 seconds, attraction range 25 m. Cutting 6 seconds, noise 20 m |
| Plasma | Energy 100, cost for 1 shot 25, interval 0.8 seconds; after 3 seconds idle regenerate 12.5/second; weak creature requires 3 hits, friendly damage 35 |
| Movement/attack cues | Listener chase 1.2× baseline sprint, windup 0.8 seconds, search 8 seconds. STRIDER ship-door warning 8 seconds |
| Environmental traps | Pipe release 3 seconds, lethal to Listener in zone, 60 friendly damage. Direct falling hit lethal to Listener, 100 friendly damage. STRIDER interruption 3 seconds |

[Completed] Only basic baton/radio are initially free. Handle existing saves through a separate implementation migration without arbitrarily resetting money/purchases. Even with no cash and injuries, close free-route deliveries and basic tools must allow earning recovery money. Do not make higher-paying destinations difficult solely by increasing enemy health.

## 11. Production scope and sequence

[Completed] Instead of promising launch quantities, cap the first complete production scope at **one Cinder destination, two common creatures plus one outer creature, five special tools, one cart, three receiver districts, up to six base deliveries, one bonus area, three clues and three decorations**. Reuse current models/map first; new creature/equipment appearance needs separate production. Add destinations only after evidence that this loop is enjoyable.

1. **Rule foundation:** visit ledger, state transitions, inventory/health/downs/liftoff, existing-save migration. Explain costs and failure reasons in UI.
2. **One delivery:** connect base cargo, address search, automatic verification/pause/resume, receipt, ship-only bonus reveal and extraction settlement.
3. **Cooperative survival:** finish Listener/noise, baton, rescue/dragging/self-revive/ship revival first. Compare current behavior against target rules per task.
4. **Choices and risk:** scanner/decoy/portable cutter/plasma/medicine, cart, traps, Scavenger, outer escalation/automatic departure, voice noise and spectating.
5. **Continuous sessions:** purchases/storage/destinations/decorations/clue persistence, reconnect/host-suspension recovery, voice accessibility and 1–4-player tuning.

Passing automated checks is not human approval of fun or quality. Open manual game windows only when explicitly requested. Before that, use relevant Editor/windowless checks. This planning-only change does not edit executable code/scenes/assets, build Unity, run clients or update development tools.

## 12. Acceptance and remaining validation

- [ ] Two players alternate hauling/responding and complete a delivery plus voluntary extraction.
- [ ] Four players may duplicate equipment but benefit from mixing roles. Solo can complete ordinary deliveries with two slots.
- [ ] Sound origins, door attenuation and decoy/signal priorities agree across server/clients; spectators create no creature-audible sound.
- [ ] Deterministic handling of verification pickup/replacement, duplicate receipts, simultaneous healing/purchases/revival and elimination at liftoff.
- [ ] Pass settlement cases for last survivor eliminated during revival, liftoff during revival, nobody aboard, manual/automatic departure races and downed occupants.
- [ ] Validate insufficient unbanked revival funds, zero floor, preserved savings, nonrefundable travel, consumed supplies and abandoned gear/cart loss.
- [ ] Players learn responses from cues and read addresses/departure warnings in fog. Hearing-accessibility settings remain playable.
- [ ] Geometry variation, debris and carts never block required deliveries or the sole return route. Free destinations avoid bankruptcy/injury deadlocks.
- [ ] Disconnect/reconnect/host exit/interrupted saves cannot duplicate rewards, restore consumed items or lose progress.
- [ ] Humans actually weigh one more delivery against leaving during 15–20-minute visits. Tune if one tool/route dominates every decision.

Only document checks were performed in this task. All items above remain unexecuted and are not launch completion claims. Platform/Steam integration, latency, performance, final art/audio and voice-service selection require implementation-stage technical review. This closes an actionable v1 rule design; revise tuning from play evidence.

[Overview](01-overview.en.md) · [Current implementation](02-spec.en.md) · [Work order](04-backlog.en.md) · [Test selection](quick-testing.en.md)

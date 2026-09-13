# 7. MDA analysis — Core Fun and Core Loop

[한국어](07-mda.ko.md)

> **Historical record · retired/superseded 2026-09-12.** The [PSX space-delivery design](01-overview.en.md) is current. Implementation, launch, art and values below are historical, not current status. Deleted files remain path records only.


Analysis date: 2026-09-09 · Game 0.9.4 · Static review of current specifications and code. No gameplay code changes. Fun and player behavior below are hypotheses inferred from implementation, not validated through human play in this task.

## Method

Update 2026-09-09: this analysis and its Word worksheet describe current implementation. The user's request for travel, randomized maps and broader scale is addressed in [expansion exploration](08-expansion-directions.en.md). Existing segment improvements and economy findings do not cap the future vision or mandate what must be implemented first.

MDA connects Mechanics (rules and actions), Dynamics (patterns emerging from rules and player choices), and Aesthetics (the player's experience). Aesthetics does not mean visual quality alone. Core Fun and Core Loop are analytical distinctions for this project, not additional MDA components. Source: Hunicke, LeBlanc, Zubek, [original MDA paper](https://www.cs.northwestern.edu/~hunicke/MDA.pdf).

## Core Fun — Why play again?

**The strongest core-fun hypothesis: carrying unruly living parcels, having the plan disrupted, and improvising with teammates to recover and complete the delivery.** Accidents are conditional disruptions, not a mandatory step in every delivery.

| Priority | Experience (A) | Expected play patterns (D) | Implemented support (M) |
|---|---|---|---|
| Core 1 | Fellowship — shared accomplishment | Coordinating throw/catch timing and spontaneously switching carrying, device-operation and rat-response roles | Pick up/throw/catch, pings, shared objectives/balance, relay bonus, horn |
| Core 2 | Challenge — mastering readable risks | Choosing safe carrying or throwing, responding to parcel warnings and meeting the deadline | Carry slowdown, sneezing/clinging/hopping parcels, time limit, A/B destination checks |
| Core 3 | Discovery — finding better solutions | Choosing a worker or parcel for a pressure plate and combining routes with belts/airflow | Worker/parcel plate detection, gate hold time, belt reversal, airflow device |
| Supporting | Sensation and Fantasy — physical feedback and an unusual delivery-worker role | Enjoying the visible causes and consequences of throws and reactions | Worker animation, special-parcel warnings, flying paperwork and collapsing decorative cartons |

Comic mishaps and recovery stories may emerge from these experiences together. They are not a separate standard MDA category, nor grounds to classify Narrative as core as though a separate story campaign had been established. Evidence for Expression or Submission as central pleasures is also weak in the current implementation.

Evidence: carrying and relays (`../../scripts/cargo.gd`; retired file), delivery checks (`../../scripts/cargo_rules.gd`; retired file), worker (`../../scripts/worker.gd`; retired file), shared rewards (`../../scripts/contracts.gd`; retired file), environment (`../../scripts/route_challenges.gd`; retired file), belt (`../../scripts/conveyor.gd`; retired file), rat (`../../scripts/packrat.gd`; retired file), pings (`../../scripts/pings.gd`; retired file), reactive props (`../../scripts/reactive_props.gd`; retired file). See the [current specification](02-spec.en.md) for special-parcel values and sources.

## Core Loop — What choices and actions repeat?

### Per-parcel loop

**Read parcel traits and A/B destination → choose route, transport method and roles → carry or throw/catch and use devices → release at the matching delivery area → receive score/feedback → select the next parcel.**

When an accident or interference occurs, branch into **assess → recover, reroute or seek teammate help → resume transport**. The hypothesis is that this recovery branch turns delivery work into cooperative situations. Not every accident is funny: control errors and inexplicable collisions can cause frustration.

| Loop element | Role in the current implementation |
|---|---|
| Goal | Each parcel's A/B destination and the contract quota |
| Choice | Which parcel first, which route, solo carrying or relay |
| Action | Move, pick up, release, throw, catch, ping, operate devices, sound horn |
| Resistance | Slow carrying, parcel behavior, environmental timing, packrats from contract 2, remaining time |
| Feedback/reward | Delivery score, relay notice, earnings accumulated during a contract |
| Repeat incentive | Next parcel/destination, remaining quota/time, improvements learned from the previous failure |

Delivery requires the matching destination, no holder and no previous delivery. A relay requires another worker to catch a thrown parcel within 3 seconds of the throw, at least 3m from its origin, while passing the floor-proximity check. Not every short handoff counts. Evidence: cargo.gd pickup/release (`../../scripts/cargo.gd`; retired file), cargo_rules.gd try_dispatch (`../../scripts/cargo_rules.gd`; retired file).

### Within-contract loop

**Repeat deliveries → reassess time/quota → adjust priorities and roles → repeat deliveries → meet quota or run out of time.**

Campaign duration starts at 240 seconds plus 20 seconds per time-upgrade level. Quotas for 1/2/3/4 players are 5/6/8/10 plus contract stage (0/1/2)×2. Solo retains transport and route mastery, but cannot provide fellowship with other players or a relay.

### Campaign progression loop

**Win contract → settle shared credits → host buys upgrades → everyone readies → next contract → finish by winning the third contract.**

Each delivery earns 10 credits plus 5 for a relay. On success, earnings and a 20-credit completion bonus enter the shared balance. Failure does not pay the same rewards or automatically enter the successful upgrade/next-contract path. Boots, time and horn purchases change the next contract's conditions. These upgrades are progression within the current campaign, not permanent character growth. Evidence: contracts.gd (`../../scripts/contracts.gd`; retired file), flow control (`../../scripts/main.gd`; retired file).

Practice has a separate flow: after a successful base 180-second shift, unanimous agreement enables a 60-second extra target. Keep it distinct from campaign upgrades. Evidence: round_rules.gd (`../../scripts/round_rules.gd`; retired file), main.gd (`../../scripts/main.gd`; retired file).

## Weak points and questions to validate

| Current evidence/constraint | Implication for fun | Human-play question |
|---|---|---|
| Solo supported; workers or parcels activate plates | Sharing a session does not guarantee cooperation | Do players spontaneously request help and relays? |
| Relay delivery bonus implemented | May incentivize accepting failure/recovery costs | Is it actually worth choosing over safe carrying? |
| Held parcels use fixed carrying; force-based grabbing was reverted | Do not promise jointly supporting weight as the core physical interaction | Can players understand and control carrying, throwing and recovery outcomes? |
| Decorative carton debris does not directly push workers/delivery parcels | Visible chaos does not necessarily create new choices | Do devices change routes, timing or roles? |
| User concerns about map quality; real four-player validation outstanding | Discovery/cooperation have potential but remain unconfirmed | Do players try alternatives beyond repetitive carrying and want to replay? |

These are neither confirmed bugs nor approved new features. They provide observation criteria for existing [MAP-01, PHY-01, QA-01 and UX-01](04-backlog.en.md). Record examples of help requests, role switches, rerouting, explanations of failure and voluntary retries. No pass thresholds or actual results have been established; do not mark human-play items in the [validation criteria](05-validation.en.md) complete.

## Reward-loop weakness identified while completing the worksheet

On 2026-09-09, filled all 7 response areas in the supplied Word worksheet and appended the repeating structure and improvement priorities. [English completed copy](../deliverables/core_loop_No_Returns.filled.en.docx), [Korean counterpart](../deliverables/core_loop_No_Returns.filled.ko.docx). The source remains unchanged.

Under current contracts.gd (`../../scripts/contracts.gd`; retired file) rules, every maximum upgrade costs boots 20×2 + time 25×2 + horn 20 = 110. A four-player first contract meeting its quota of 10 without relays pays 10×10 + success bonus 20 = 120. Purchases have no count limit, so all upgrades can be maximized after the first contract. Subsequent purchase choices and equipment uses for additional relay income can therefore run out early. Code and arithmetic confirm the structure; reduced fun remains a hypothesis. This does not negate records, transport efficiency or cooperation itself.

Improvement priorities are ① sustaining reward→next purchase choices ② transport-method choice value ③ recovery→learning/satisfaction ④ variation in the next contract. Price/availability adjustments and map-segment comparisons remain proposals, not approved implementation changes. Follow [ECO-01 and the existing backlog](04-backlog.en.md).

Checked all 7 response areas, source hash, page settings, preservation of 7 untouched package parts, and bilingual content/values. Page rendering failed with `LibreOffice soffice.exe was not found on PATH`; actual pagination, fonts and clipping remain unverified. No gameplay changes or game-test reruns.

## MDA investigation verification scope

Compared current documents with relevant code for rules, rewards and progression conditions. Review document links, language correspondence, numbers and checkbox states, and run `python tools/check_docs.py`. Game execution, rendering, actual cooperative play, experienced fun and rerunning existing game tests are outside this investigation's verification scope.

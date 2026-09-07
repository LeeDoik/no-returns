# NO RETURNS — Detailed cooperative campaign design

[한국어](2026-09-07-release-design.ko.md)

Version 0.7 design, September 7, 2026. Korean is the default documentation entry point. [Implementation plan](../plans/2026-09-07-release-polish.en.md).

## 01. Status and authority

This is a detailed design for a playable development build, not a statement that the game is ready for commercial release. **Committed** means the implementation plan specifies the behavior. **Locally verified** means the specified development behavior passed the checks recorded in section 29; it does not certify external release gates. **Proposed** means a product or tuning decision requiring validation. **External/unverified** means a test or account-dependent requirement has not been established locally.

The implementation plan controls numerical conflicts. The 0.6 depot is the foundation; 0.7 adds a complete contract run, shared purchases, relay rewards, Packrat, onboarding, settings and protocol admission. This document does not certify the current checkout or ZIP. No price, release date, sales expectation or external multiplayer reliability is approved here.

**0.7.2 map theme update:** The earlier night-warehouse presentation in this document is superseded by The Shipping Shrine: a giant cardboard boss, hungry A/B face altars, Packrat employee-of-the-month display and bilingual company slogans. Dimensions, carrying, dispatch and contract rules are unchanged. [Concept and implementation record](../plans/2026-09-07-shipping-shrine.en.md).

**0.7.3 map editing conversion:** `scenes/maps/shipping_shrine.tscn` is the source of map placement and gameplay volumes. Legacy code coordinates are no longer the normal game map source. Protocol 8 rejects guests with different saved map/dependency fingerprints. [Complete editing guide](../../development/map-editing.en.md).

**0.7.4 map expansion:** The current default map is 48×60 m with farther dispatch, a pressure gate, periodic airflow and a relay lounge, using protocol 9. The [expansion design](../plans/2026-09-08-shrine-expansion.en.md) supersedes older dimensions/protocol values below.

## 02. Player promise and intended audience

The player is a worker in a night depot. Living packages interfere with ordinary delivery, and workers learn to turn those behaviors into useful routes and catches. A successful run should leave the crew able to describe who threw, who caught, what caused a mishap and how they recovered. The target audience is friends who enjoy physical coordination and short cooperative sessions; solo campaign provides a complete alternative with lower quotas.

**Proposed, unmeasured session target:** 10–15 minutes for three successful contracts plus decisions between them. Three full base timers total 12 minutes; early success shortens this, upgrades and retries extend it. This is a pacing hypothesis, not a measured completion time or a promised playtime.

## 03. Reference facts: How to Fish

The following five facts come from the [official Steam description](https://store.steampowered.com/app/4001890/How_to_Fish/), checked September 7, 2026. They establish reference features only.

| Officially described feature | Scope of the fact |
| --- | --- |
| Physics-based fishing for 1–4 players | Player count and activity |
| Selling fish and purchasing better gear | Earn-and-spend progression |
| Quests and bosses lead to later islands | Staged progression |
| Rare fish variants can be collected | Collection goal |
| Trick shots earn additional money | Reward for skilled execution |

No sales, concurrent-player counts, success causes, networking implementation or exact balance values are inferred from this description. No reference assets, characters, wording or level layouts are copied.

## 04. Our interpretation and independent design

**Our design inference:** visible short-term work, a shared spending decision and a more demanding next task may give physical mishaps a purpose beyond a single gag. That hypothesis motivates three depot contracts and temporary shared upgrades. It does not establish why the reference game performs as it does.

Our skilled action is a deliberate throw-and-catch relay, our complication is living freight, and our progression stays inside one depot. Collection, bosses and island travel are not 0.7 scope. A useful design test is whether removing relay credit still leaves an understandable cooperative action; rewards should reinforce teamwork rather than create arbitrary chores.

## 05. Scope and explicit exclusions

**Implemented; locally verified:** one depot; Standard, Sneezer, Clinger and Hopper; solo and 2–4-worker campaign; three contracts; A/B dispatch; conveyor; one Packrat in contracts 2–3; shared run credits; three upgrade categories; local completed-run records; Korean and English UI; Windows portable development package.

**External/unverified or future:** Steam accounts and App IDs, Steam friend invitations and transport, WAN quality, host migration, public matchmaking, voice chat, controller certification, permanent gameplay progression, additional maps and final commercial assets. Do not describe any of these as supported in public materials until implemented and tested.

## 06. Loops at three time scales

| Scale | Action sequence | Observable outcome |
| --- | --- | --- |
| Seconds | Read destination → pick up → move/throw → catch or recover | Package location and owner stay understandable |
| One contract | Choose routes → coordinate hazards → dispatch quota | Success banks rewards; timeout banks none |
| One run | Complete contract → discuss purchases → all ready → harder contract | Three successes produce a completed-run record |

A mishap must return the crew to a decision, not permanently remove a package. With four fixed package objects, recovery reuses an identity instead of growing the world indefinitely. Keep the ordinary walking route viable so advanced throws are an option, not the only way to finish.

## 07. Modes and entry flow

The title menu offers solo campaign, cooperative campaign hosting, joining a host and short practice. Existing practice and host APIs retain their legacy behavior unless explicitly invoked as campaign. This protects earlier mechanic tests and provides a quick environment for learning throws.

Flow: title → choose mode → lobby when cooperative → first contract → success settlement → shared shop/readiness → next contract → final record. A connecting state must offer a way back and explain failure. Joining a campaign does not silently start a personal independent run. The host determines the active session mode.

## 08. Run and contract state transitions

| State/event | Required next behavior |
| --- | --- |
| Campaign creation | New run credits/upgrades; stage index 0 |
| First start | Spawn/reset four packages; use current crew quota and contract timer |
| Quota reached | Stop active interaction, settle the successful attempt once |
| Successful contract 1 or 2 | Show ledger and shared purchases; collect readiness |
| Every current worker ready | Begin the next contract with purchased upgrades |
| Third successful contract | Show completed run; save local record; no fourth contract |
| Timeout before quota | Show failure; bank nothing from this attempt |
| Restart/run exit/disconnection | Release worker/creature claims and clear transient actions |

**Committed:** failed-contract retry stays on the same stage and preserves credits/upgrades already earned from earlier successes, while clearing current-attempt earnings, deliveries, relays and lifecycle claims. A new run clears all run upgrades. Final completion disables the shop. The campaign uses next-contract readiness instead of legacy overtime. Legacy practice success may retain its existing one-off bonus.

## 09. Contract numbers and escalation

Each contract starts with **240 seconds**, plus **20 seconds per time-upgrade level**, maximum two levels. Base quotas by crew size are **5/6/8/10**; stages add **0/2/4**. Stage index is 0-based internally and 1-based in UI.

| Workers | Contract 1 | Contract 2 | Contract 3 |
| --- | --- | --- | --- |
| 1 | 5 | 7 | 9 |
| 2 | 6 | 8 | 10 |
| 3 | 8 | 10 | 12 |
| 4 | 10 | 12 | 14 |

Contract 1 teaches routing without Packrat. Contract 2 introduces one Packrat without adding package identities. Contract 3 raises delivery demand while retaining the same learnable rules. These are committed initial tuning values, not validated difficulty. Current-run upgrades only affect applicable later contract starts; buying time does not retroactively alter a finished result.

## 10. Depot zones and spatial readability

Current layout coordinates are an implementation reference, not a promise of additional content. The floor is **32 × 36 m**, centered at **(0, -0.25, -9)**. Dispatch A is **(-10, 0.65, -22)**; B is **(10, 0.65, -22)**. Conveyor center is **(10, 0, -10)**, lever **(12, 0, -9)**, Packrat nest **(13, 0, -5)**.

| Zone | Purpose | Reading rule |
| --- | --- | --- |
| Intake/front | Find the four packages and learn controls | Separate type silhouettes and destination tags |
| Left route | Direct access to A | A and arrows repeated at decision points |
| Right route | B, conveyor and Packrat encounters | Belt direction and nest must be visible before committing |
| Divider/obstacles | Throw, Hopper and cooperative route choices | Always provide an ordinary carry alternative |
| Dispatch frontage | Final destination decision | Letters and shapes supplement color |
| Nest | Recover stolen freight without damage/death | Reachable from the public walking route |

**Proposed QA target:** a new player should identify A, B and intake from their signs within the first contract. Darkness provides atmosphere; it must not conceal destination text or the creature tell. Verify both supported UI languages against real world backgrounds.

## 11. Basic interaction and ownership

WASD moves, mouse looks, Space jumps, E picks up/catches/puts down, left click throws, Q pings, F operates the conveyor lever, R sounds the airhorn and Esc opens the menu. Opening a menu does not pause an online shift. No damage or death system is introduced.

The host accepts interaction only for admitted workers in an active phase. One worker owns at most one carried package; a package has at most one worker or creature claim. Standard pickup reach remains **2.4 m** with clear static line of sight. A guest requests an action rather than assigning ownership or money. Clear stale movement/action state on run transitions and disconnection.

## 12. Destination, dispatch and recovery

Start with A destinations. Successful replacement assignments use the host-seeded shuffled bag containing two A and two B entries. A seed reproduces assignment order, not physics or every behavior interval. Correct unheld dispatch scores once; wrong bay and out-of-bounds recovery score nothing and preserve the destination. Ordinary recovery takes **1.2 seconds**.

The destination letter is present on cargo and in contextual guidance. Dispatch validity uses the host's actual bay and ownership state, never a client claim. After recovery, erase relay provenance, release attachments/creature ownership, restore collisions and return to intake. A newly spawned replacement cannot inherit a prior shipment's bonus eligibility.

## 13. Four cargo behaviors

| Type | Committed foundation | Intended cooperative use |
| --- | --- | --- |
| Standard | Ordinary carry and throw | Establish timing and reliable relay practice |
| Sneezer | 1.5-second tell; later calm intervals 6–9 seconds; forward blast | Release sticky links or push cargo; carrier is exempt from its own blast |
| Clinger | Attach within 0.95 m for up to 5 seconds; worker speed 70%; 2-second detach cooldown | Move a matching pair, or use a sneeze to separate a mismatch |
| Hopper | 4 grounded seconds of rest; 1-second windup; forward 2.8 m/s and upward 6.5 m/s | Aim while held, release near a low obstacle, catch or guide after jumping |

Holding pauses Hopper's pending cycle; ordinary airborne travel pauses the clock, and its own hop landing begins fresh rest. All behavior cues distinguish waiting, imminent action and actual action. Clinger chains are excluded. A linked pair earns two dispatches only when each destination matches the actual bay; a mismatched partner recovers without scoring.

## 14. Conveyor and pings

The conveyor moves at **2 m/s**. It affects grounded workers and free grounded cargo inside its bounds; held, creature-claimed, attached and airborne cargo must not be transported as ordinary floor freight. Reversal uses F, **2.4 m** reach, a facing test, clear sight and **0.5-second** shared cooldown. The UI uses the same geometric eligibility as the server. Motion cues stop outside active play and resume in a new active contract.

Q maintains one shared marker for **4 seconds**, with a **1-second sender cooldown**. Select visible nearby cargo in front or a point ahead. Show the sender's slot; newer valid pings replace older ones. A departure or round reset clears the marker. This remains a coordination aid, not a replacement for implemented voice communication.

## 15. Team relay qualification

Committed sequence: A deliberately throws → B catches while airborne → the package reaches its correct bay. B must differ from A, catch within **3 seconds** and be at least **3 m from the release position**. Qualifying relay credit is **5**, once for that delivered package, in addition to ordinary delivery credit.

Track authoritative thrower, release location/time, catch eligibility and whether the dispatch reward was consumed. Self-catch, repeated pickup, ground pickup, an expired catch, too-short travel, wrong delivery, recovery and duplicate dispatch cannot create credit. Recovery clears provenance. A successful catch announces the relay but does not bank money until a successful contract settlement.

**Proposed clarity rule:** use distinct catch and settlement wording so a promising relay is not confused with banked credit. If a later action invalidates eligibility, the ledger must show the final valid count, not a cumulative animation count.

## 16. Cooperative examples and failure recovery

**Two workers:** A reads B, throws across an open aisle, B catches 3.2 m from release after 1 second and carries to B. Correct dispatch counts one delivery and one relay. The same throw caught after 3.1 seconds counts no relay, though a subsequent ordinary delivery is valid.

**Three workers:** A carries Sneezer, B transports Hopper with a matching Clinger attached, C signals the B route. A's blast can separate the pair before a wrong destination. This is a choice with readable consequences, not a guaranteed shortcut.

**Four workers:** two handle intake and dispatch while one watches the right lane and one retrieves Packrat cargo. Roles are informal and interchangeable. A horn on cooldown must not make recovery impossible: the nest remains reachable and the creature drops cargo there. No class lock, mandatory specialist or permanent punishment is required.

## 17. Packrat state machine

Packrat appears only in campaign contracts **2–3**. There is exactly **one** creature, on a fixed right-lane patrol with a reachable nest. It targets free grounded cargo and never steals worker-held or attached cargo.

| State/event | Result/invariant |
| --- | --- |
| Disabled: practice/contract 1 | No creature claim or active theft |
| Patrol with valid nearby candidate | Begin a visible 0.9-second steal tell |
| Candidate becomes held, attached or invalid during tell | Cancel/re-evaluate; do not override ownership |
| Tell completes with valid target | Claim one package; carry it visibly toward nest |
| Valid horn during approach/carry | Drop if carrying; flee for 3 seconds |
| Nest reached | Automatically drop; protect dropped cargo from re-theft for 8 seconds |
| End/restart/disconnection | Drop and restore cargo state before reset |

While claimed, ordinary pickup, cargo update, conveyor transport, Clinger acquisition and sneeze cargo push exclude that package. The claim cannot strand a frozen invisible object. Every drop restores layer, mask and freeze behavior consistently. Final 0.7 tuning: patrol/carry speed 2.7 m/s, theft radius 1.4 m; waypoints (X,Z) (10,-3.2), (10,-8), (10,-14), (13,-17), nest (13,-5). These remain provisional balance values. Carried-cargo obstacle checks allow 6 mm of vertical contact tolerance per side to accommodate settled rigid bodies; they retain near-full horizontal dimensions and block wall passage.

**0.7.1 correction:** Search for visible free grounded cargo within 6 m in the right-side activity bounds (X 8.5–14.5, Z -19–0), approach, then start the existing 0.9-second warning within 1.4 m. Pickup, attachment, lost sight or range cancels pursuit. After 1.25 seconds without progress, ignore the target for three seconds and resume patrol. The host simulates `seek`, replicates it in the existing creature packet, and both UIs show approach status. This does not add pursuit into the left lane or intake area.

## 18. Airhorn decisions and readability

R affects Packrat within **4 m** and clear sight. A valid scare forces a drop and **3-second flee**. Each worker has an **8-second cooldown**, reduced to **6 seconds** with the shared horn kit. One player's use must not consume another's cooldown. The host validates the request and broadcasts its outcome.

Show remaining cooldown near the relevant action and a short readable response to success. **Proposed UX:** distinguish out of range/blocked sight from cooldown without repetitive large messages. Mute suppresses synthesized audio but retains visual tells and feedback. The horn is a recovery tool; there is no damage, kill reward or requirement to harm the creature.

## 19. Shared credits and transaction rules

On a successful contract, bank **10 × valid deliveries + 5 × valid relays + 20 completion credits**. A failed attempt banks **0**, including its successful individual deliveries. Settlement occurs once per attempt. Credits are shared by the run, not split or awarded separately to each peer.

Example: two-worker contract 1 finishes with 6 deliveries and 2 qualifying relays: **60 + 10 + 20 = 90 credits**. Buying boots for 20 and a time permit for 25 leaves **45**. This is a rules illustration, not a measured expected reward. Host-only purchasing checks phase, balance and cap before subtracting funds. Duplicate messages cannot create repeated settlement or over-cap upgrades.

## 20. Upgrade shop and balance questions

| Shared upgrade | Cost | Effect | Cap |
| --- | --- | --- | --- |
| Boots | 20 per purchase | +8% movement per level | 2 |
| Overtime permit | 25 per purchase | +20 seconds per contract per level | 2 |
| Airhorn kit | 20 | Horn cooldown 8 → 6 seconds | 1 |

Purchases are available only to the host between successful contracts. Each worker sees the same balance, levels and next-contract consequences. **Committed arithmetic:** boots are additive, giving +16% at level 2, a final movement multiplier of 1.16. Time reaches 260/280 seconds at levels 1/2. No purchase creates a permanent account advantage.

**Balance hypothesis:** the first contract may fund several upgrades immediately. Test whether crews still discuss a meaningful choice or simply buy everything. Do not silently raise costs to create artificial scarcity; change the table, tests and both languages together after observations.

## 21. Readiness, retries and group consent

All current workers must opt in before the next contract starts. One worker's repeated vote counts once. A disconnected worker is not a ghost required for unanimity. Show ready count, own vote and host purchase authority. The last vote cannot skip settlement or start two contracts.

**Committed:** a purchase clears readiness so the crew can review the changed setup. Retries preserve earlier successful savings and upgrades, remain on the current stage and reset attempt counters/claims. A full run restart resets credits/upgrades. After contract 3 the run is finished and purchasing is disabled. Host departure ends the session; host migration is outside scope.

## 22. Onboarding and help

Provide five short lesson states **0–4**, with visible actions rather than long prose. **Proposed teaching sequence:** 0 move/look and locate intake; 1 read A/B and pick up; 2 throw/catch and recover; 3 operate conveyor and communicate with Q; 4 read behavior tells and use R against Packrat when available. Do not require Packrat completion in contract 1, where it is absent.

Players can open help again. Explain that Esc does not stop online time, crew goals change with size, and failed contracts do not bank attempt credit. **Proposed experiment threshold:** four of five new players complete an ordinary delivery without verbal coaching within 2 minutes. Failure triggers tutorial revision, not a claim that players misunderstood.

## 23. HUD, results and menu hierarchy

Active HUD prioritizes contract index, deliveries/target, remaining time and the next contextual action. Creature status and horn cooldown are visible without covering cargo. Holding a package should expose its destination. Use slot numbers, letters and shapes in addition to color.

Success view shows deliveries, relays, reward breakdown, shared balance, upgrades and readiness. Failure explicitly says the attempt banked no credits. Final view distinguishes a completed three-contract run from one successful contract. Campaign screens replace legacy overtime controls while preserving practice behavior. Help, quit, join failure and connection cancellation must remain accessible.

**Proposed layout acceptance:** no clipped essential text at 1280 × 800 logical resolution and the supported scaled window; verify Korean/English title, active HUD, help, settings, success, failure, shop and completed-run views. Actual captures, not source inspection alone, establish readability.

## 24. Settings, accessibility and localization

Persist language, cue mute and mouse sensitivity. Extend preferences with master volume **0–1**, field of view **60–90 degrees** and fullscreen. Retain compatible defaults when older files lack new fields; clamp invalid values and keep tests isolated from personal preferences. Existing sensitivity range is **0.50–2.00**, in **0.25** steps. No camera shake is used.

English remains the production-copy source; Korean supports full review and is the default UI. Both versions must match numbers, states and warnings. Sound cannot be the only theft/windup signal. **Proposed future acceptance:** separate subtitle/text-size options and remappable keys require their own implementation and validation; they are not current advertised features.

## 25. Art and animation direction

Retain the night-depot geography while improving readable silhouettes: rectangular ordinary freight, a clear Sneezer face/direction, visible Clinger attachment, Hopper squash/arrow and an unmistakable Packrat carrying pose. Warm working lights separate usable routes from cool background structure. Readability outranks decorative clutter.

**In-progress development presentation:** primitive geometry and existing scene assets. **Commercial art gate:** confirm original work or documented licenses for every model, texture, font and promotional asset. No copied reference art. Required animations are idle, warning, action and recovery for each relevant behavior; do not imply final animation polish merely because a state exists.

## 26. Audio and feedback budget

Use short locally synthesized cues for relay and correct dispatch; obey mute and master volume. Creature tell, horn success, purchase and contract settlement should have distinct visual responses. **Proposed mix:** prevent repeated dispatch/relay events from stacking into an excessively loud cue; prioritize imminent behavior warnings over ambient machinery.

Each audio event needs authoritative event identity or equivalent deduplication across snapshots. A reconnect or a stale repeated state must not replay a whole history of successes. Ambient conveyor visuals should agree with active simulation. Final sound design, loudness evaluation and target-device listening tests remain external production work.

## 27. Network authority and replication

Host simulates workers, cargo claims, Packrat, timers, dispatch, relay provenance and money. Guests send bounded input/actions and present received state. Session-version handshake rejects an incompatible protocol **before roster admission**. Do not let rejected clients occupy a worker slot or submit purchases.

Motion snapshot serialized budget remains **≤1280 bytes**. Campaign metadata is a separate authoritative reliable message sent on change. Packrat uses a separate compact unreliable state **≤256 bytes**. Keep reset/end events reliable or otherwise convergent so a lost creature packet cannot leave a permanent claim. Verify maximum field values and longest state names, not only idle packets.

Current ENet direct-IP/local-LAN transport is distinct from Steam networking. [Steam Datagram Relay documentation](https://partner.steamgames.com/doc/features/multiplayer/steamdatagramrelay) is an integration reference, not evidence that this build has relay or invites. Actual Steam transport and separate-network operation remain **external/unverified**.

## 28. Persistence, recovery and integrity

Save only completed-run records locally; run credits and upgrades never become permanent advantages. On a completed three-contract run, every participating worker stores a local completion, increments completed runs by one and updates best run delivery/relay totals. Failed or abandoned runs do not produce a completion record. Gross credits earned across the run may be recorded separately from the remaining bank balance. Validate loaded records and recover gracefully from malformed or missing data. Implemented record fields are format version 1, completed-run count, best deliveries, best relays, best gross credits and the last completed run ID. Guests must observe all three active stages before recording completion; joining an already finished run cannot earn a record. **Proposed additional fields:** completed timestamp, crew size and elapsed run time. Do not promise cloud synchronization, leaderboards or cross-device continuation.

End, restart and disconnect tests must cover worker-held, Clinger-attached, recovering, Hopper-airborne and creature-held cargo. A run transition clears readiness, temporary action cooldowns and pending claims as specified by the implementation. No save failure should prevent returning to the title menu. Only aggregate best values/count and the last ID are retained, not a run history. Missing/invalid numeric data falls back safely; version 1 is accepted. User data is scoped to the application name, so older version folders are not automatically migrated. There is no mid-run save or cross-device continuation.

## 29. QA matrix and evidence requirements

| Area | Cases | Pass condition |
| --- | --- | --- |
| Contract quotas | 1/2/3/4 workers × all three stages | Exact 5–14 table values |
| Timers | 0/1/2 permit levels | 240/260/280 seconds at contract start |
| Settlement | success, failure, duplicate result | Correct single award; failure adds zero |
| Purchases | guest, insufficient funds, cap, repeated request | No unauthorized spend or negative balance |
| Relay | valid catch, self, floor, 3-second boundary, 3-m boundary | Only eligible correct dispatch earns one bonus |
| Relay reset | wrong bay, recovery, reuse, duplicate dispatch | No inherited or duplicate provenance reward |
| Readiness | duplicate vote, last vote, departure, new attempt | Current roster only; single transition |
| Packrat eligibility | held, attached, airborne, recovering, free grounded | Only allowed candidate can be claimed |
| Packrat tells | candidate changes during 0.9 seconds | No stolen ownership race |
| Packrat recovery | horn, nest, restart, disconnect | Visible usable cargo; masks/layers/freeze restored |
| Horn | 4-m boundary, obstacle, cooldown, two workers | Authoritative range and independent cooldowns |
| Conveyor | grounded, elevated, held, creature-held, idle/active | Correct exclusions and visual state |
| Protocol | matching and mismatched clients | Incompatible peer rejected before roster |
| Replication | host/guest, four workers, packet loss simulation | Consistent claims, rewards and next contract |
| Payloads | worst motion and creature states | ≤1280 and ≤256 serialized bytes |
| Settings/save | old file, corrupt data, restart, isolated test | Safe defaults and correct persistence |
| UI/languages | title/help/settings/play/result/shop/final | Essential content visible in both languages |
| Distribution | export, unpack elsewhere, launch/capture | No editor dependency or missing scripts/resources |

**Local verification, September 7, 2026, 0.7:** `python tools/run_tests.py` passed 21 behavior scripts, six real two-process scenarios and a four-process scenario. Evidence: `artifacts/full-07-tests.log`. New checks cover real-flight relay, three-contract settlement/upgrades/readiness, theft/horn rescue, actual grounded-cargo nest transport, mismatched protocol and record/settings edge cases. `python -m unittest discover -s tests -p test_steampipe.py` passed. `python tools/build_windows.py` exported and smoke-tested the package; `tests/audit_pack.gd` found all 22 compiled gameplay modules. Korean/English captures are under `artifacts/campaign-*.png`; the isolated executable and pack rendered an actual solo campaign at `build/pack-probe-0.7/artifacts/world.png`. Final UI edits received another campaign regression and visual inspection. The QA table also includes planned packet-loss/newcomer/hardware checks that were **not** performed; local passes do not certify every row or external conditions.

## 30. Independent tester experiments

**Proposed first round:** five new solo testers and three independent pairs; obtain consent for observation/recording. Use the same build and avoid coaching until the initial task window ends. These are recruitment targets, not completed studies.

Measure time to first correct dispatch, wrong-bay count, recognized tell count, attempted/qualified relays, Packrat recovery time, contract completion, upgrade choices and replay preference. Ask each pair to recount one mishap and each member's contribution. Separate a laugh, confusion and frustration in notes rather than counting all reactions as enjoyment.

Investigate if ordinary first delivery exceeds 2 minutes for most newcomers, Packrat cargo is repeatedly described as lost permanently, one worker does all transactions without discussion, or upgrades are always purchased identically. These are revision triggers, not automatic commercial thresholds. Repeat with distinct testers after changes; do not generalize a localhost developer session into market demand.

## 31. Performance and release gates

**Proposed profiling target:** stable 60 fps at 1280 × 800 on a declared target Windows machine, with all four workers, active cargo behaviors and Packrat. The machine specification, frame-time percentiles and network conditions must be recorded before advertising requirements. No reference game's requirements are inherited.

| Gate | Required evidence | Status at drafting |
| --- | --- | --- |
| Development mechanics | Campaign and regression tests; no known open P1/P2 in reviewed scope | Locally verified |
| Portable build | Exported scripts/resources, smoke launch, clean-folder rendering, checksums | Locally verified |
| Readability | Actual Korean/English captures and newcomer observations | Captures verified; newcomers unverified |
| Network release | Real accounts, separate PCs/networks, measured latency/loss, invitations | External/unverified |
| Content rights | Asset/license register, original store images/trailer | External/unverified |
| Platform setup | Creator's Steamworks enrollment and actual application/depot IDs | External/unverified |
| Store/build approval | Current feature claims and submitted playable build | External/unverified |

Steam requires store-presence and build review before release, and advertised supported features must be implemented in the current build; consult the [official review process](https://partner.steamgames.com/doc/store/review_process). This document does not authorize payment, submission or publication. Price and release date remain undecided.

## 32. English store copy source and Korean review

**Proposed copy; not published. Use only after the matching behavior is verified.**

**Short description — English source:**

Run a night depot where the packages refuse to behave. Carry, throw and relay four kinds of living cargo, outsmart a thieving Packrat, and spend shared earnings between three increasingly demanding contracts. Play solo or coordinate a crew of up to four.

**Feature text — English source:**

- Read the label, choose your route, and get each package to the correct bay.
- Catch a coworker's deliberate throw to earn a relay bonus on delivery.
- Put sneezes, sticky cargo, jumps and a reversible conveyor to work.
- Recover stolen freight with a well-timed airhorn or a trip to the nest.
- Complete three contracts and choose temporary upgrades for the whole crew.

**Development-build disclosure — English source:**

This Windows development build uses direct-IP ENet connections. Steam friend invitations and Steam relay transport are not implemented. Reliability across separate networks has not yet been verified. Art, balance and onboarding remain in development.

The Korean counterpart contains a full translation and preserves these English production strings. Do not add claims about endless content, controller support, voice chat, matchmaking, Steam invites or guaranteed session length.

## 33. Content expansion and change control

**Future proposals, outside 0.7:** destination-specific contracts, a second depot with a distinct transport problem, cosmetic completed-run rewards, additional cooperative tools and configurable challenge rules. Each addition must preserve an ordinary recoverable route and fit a demonstrated player need. More hazards alone do not establish more depth.

Before accepting a feature, specify its teaching moment, effect on ownership, network budget, solo viability, failure recovery and QA cases. Prototype one change at a time; compare behavior with the existing three-contract run. Update both documents and implementation numbers together. Record final evidence in the [implementation ledger](../plans/2026-09-07-release-polish.en.md); a completed development checklist never silently completes external release gates.

# NO RETURNS — definitive product direction

[한국어](01-overview.ko.md)

0.8.4: two approved parcel/receipt models produced and visually integrated. Dynamic terminal state remains follow-up work. [Record](psx-props-01.en.md).


0.8.3: six selected facilities integrated and Windows build completed. MCP bounds/collider checks passed for six modules; actual east-corridor and north-detour screens inspected. Existing wall/rack colliders and carrying rules remain. Full floor finish, lighting, pattern repetition, frame performance and human feel remain open. [Record](psx-tripo-kit-01.en.md).


Current 0.8.2: fixed downed-host departure recovery, low-step routing/foot height, simultaneous F transitions, terminal collision, ship attack protection, the west rack slit and join-refusal feedback. Unreproduced target oscillation and the standing-host departure-abort policy remain design reviews. [Findings and evidence](review-fixes.en.md).

Added in 0.8.1: after entry, the outer creature automatically pursues crew across the entire map. Downed/aboard employees are excluded; pursuit resumes after beacon distraction. Walls require detours but do not block detection. [Rules/validation](space-play-07.en.md).

Added in 0.8.0: expanded the map to 32×44m with an east corridor, north detour and west storage wing. Four suppression stages and delayed outer-creature entry are implemented. Values, geometry and creature visuals remain experimental, not final art. [SPACE-PLAY-07](space-play-07.en.md).

Added in 0.7.0: I clue inspection and the Tab shared field log. Records reset on next arrival and are not restored after exit; wallet/license saving remains. [Clue specification/validation](space-play-06.en.md).

As of 0.6.0, host progression saves restore wallet, beacon license and successful-delivery count; shifts restart at ship preparation. Earlier session-only/no-save notes describe the state through 0.5.0. [Save rules and validation](space-play-05.en.md).

2026-09-12 · SPACE-01 · Product direction approved / detailed design in progress / carrying, delivery and hazard experiments implemented

## One sentence

Employees of a space delivery company travel aboard a spacecraft to unfamiliar destinations, overcome local creatures and hazards to deliver cargo, and reinvest their pay in equipment and more dangerous delivery routes in an online cooperative game.

This document is the highest-level design authority for the new NO RETURNS. Code, art, prices, maps and completion claims from Godot, CarryLab, SIDE EFFECTS, toy delivery and NR-LOOP-01 do not carry forward. Previous records are archival, not current specifications.

## Confirmed and undecided

| Status | Content |
|---|---|
| Approved now | NO RETURNS name, first-person default camera, PSX style, space mystery, employment by a space delivery company, route selection followed by automatic spacecraft travel and landing, delivery through regions with monsters or creatures, reinvested pay and harder jobs |
| Retained as the working direction | Unity, Windows/Steam, 1–4-player online cooperation, English game copy and complete Korean documentation |
| Undecided | failure losses, combat emphasis, shift length, release content quantity, prices/rewards, mystery identity and ending |

“Definitive design” records commitment to this product direction. It does not finalize undecided details or imply implementation. This task establishes the fresh project and production baseline.

## Core experience

1. A destination and payment appear. The team judges whether its equipment is adequate.
2. The team prepares and arrives aboard the spacecraft. Sounds and visible traces suggest local threats.
3. Players decide who carries, scouts or distracts creatures to move cargo safely.
4. They obtain delivery confirmation and return to the spacecraft. Discovered information changes the next decision.
5. They reinvest pay and take a harder destination.

Comedy should emerge from serious workplace procedure colliding with unexpected incidents, rather than exaggerated employee jokes. Mystery should change how players see contracts and locations, rather than becoming a separate mode requiring extensive reading.

## Core loop

Review contract → prepare equipment/cargo → spacecraft travel and arrival → investigate hazards and deliver → obtain confirmation and extract → settle, improve equipment and choose the next contract.

Contracts show destination, cargo handling conditions, receipt method, pay and known hazards. Explicitly mark unknown information so established rules do not appear to change arbitrarily. New economy design will set amounts and contract counts. The former 30/60/90 economy is a retired experimental value set.

The spacecraft is the shared hub for preparation, contracts, loading and return. The user selected route choice followed by automatic travel and landing. Manual flight control is outside scope; production focuses on site exploration, delivery and creature responses. Team readiness rules for departure and return remain a later detail.

## Creatures and delivery — proposals

Creatures must change carrying methods and route choices. The working proposal emphasizes observation, evasion, distraction and tools; the need for direct combat is a separate decision. Avoid repeating pursuit through narrow corridors as the sole challenge.

| Role to test | Observable clue | Delivery decision |
|---|---|---|
| Noise-reactive creature | Nearby sounds subside when movement stops | Put cargo down and wait quietly, or have a partner distract it elsewhere |
| Creature attracted to particular light | Traces of following illumination | Keep visibility or switch equipment off and detour |
| Creature occupying a delivery route | Recurring movement and rest patterns | Move during a pause or seek another receipt approach |

These are candidate roles, not committed species counts, appearances or implementations. Specify warning clues, detection, pursuit termination, counters, recovery and online authority before production. Solo play requires an alternative solution.

## Progression and harder jobs — proposals

Pay funds equipment improvements and preparation for harder work. Prioritize carrying assistance, detection, distraction/barriers and spacecraft loading/recovery support that change how players act. Do not remove danger solely through numerical power growth.

Combine uncertainty, cargo handling burden and delivery/extraction conditions instead of merely increasing creature counts and health. Better equipment should expand choices; separately examine designs that block progress behind one purchased solution. Prices, unlock requirements, consumable/permanent ownership, save ownership and failure losses remain undecided.

## Mystery — unapproved proposals

The immediate goal is to give players a reason to ask “Why are they sending us here?” rather than locking a hidden explanation in advance. Candidates include:

- A deserted facility issues an ordinary delivery confirmation.
- A mark discovered on site appears later in a contract's sender record.
- A destination photograph differs slightly from the site, with a subsequent delivery allowing players to investigate.

Do not label these events a corporate conspiracy, supernatural event or AI explanation yet. The first test should contain connected clues and an observable change without finalizing identity or ending. Separate essential delivery instructions from narrative deception so players can trust gameplay rules. Basic delivery must remain possible without noticing clues, and teammates must be able to verify the same evidence. Completing a collectible list and feeling curious are different outcomes.

## PSX space mystery art direction

Use clear low-poly forms, coarse low-resolution textures, limited palettes, silhouettes in fog/darkness and worn space-industrial facilities. Exact hardware emulation is not the goal. Employees, cargo, spacecraft, facilities, creatures and UI must share material density and period character.

Delivery targets, entrances and creature cues must remain readable in dark environments. Tune or make optional camera shake, dithering, vertex wobble and CRT effects based on readability and discomfort. Obtain approval for consistent concept images before final model production. Approval of the old toy-themed D direction does not apply to new art.

## First production unit — proposal, not release scope

Test a complete delivery with a spacecraft preparation space, one destination, one cargo type, one creature with a learnable counter, equipment with an observable purchase benefit, receipt/extraction/settlement and an intriguing site clue. Finalize detailed counts and stages in the work plan around automatic travel and landing.

Ask whether players understand the destination, read and counter creature cues, change actions because of cargo, want to reinvest, and want to investigate together. Automated tests validate contracts, rewards, recovery and synchronization; human play validates readability, feel, fun and curiosity.

[Current implementation](02-spec.en.md) · [Production guide](03-guides.en.md) · [Open decisions and work order](04-backlog.en.md) · [Validation](05-validation.en.md)

## Suppression and indirect time pressure — additional agreed direction

The user selected unkillable dangerous creatures outside the delivery area, suppression shutdown and creature entry when the time limit expires, and indirect warning cues instead of exact time display. Version 0.8.0 implements this direction with experimental suppression stages and a placeholder creature. Final spaces/models and human cue readability remain incomplete. Ordinary creatures remain inside even during suppression. Continued field decay after delivery is proposed so exploration and return decisions use the remaining operational margin.

Stable, unstable, critical and offline stages; lamp, sound and interior-light cues; perimeter entry; and survival through observation, hiding and detours are detailed design proposals. Exact operating duration, stage lengths, perimeter detection, entry, pursuit, escape and recovery rules remain undecided. Unwarned instant death or spawning directly beside players is not established as a default rule.

## SPACE-ECO-01 — Proposed economy

[Pay, failure and equipment economy draft](economy.en.md) defines delivery pay, return bonus, failure recovery and permanent equipment unlocks. Values, save ownership and loss rules are proposals pending play validation, not implemented commitments.

## Visualization inventory

These images belong to the current PSX space-delivery direction. **Image creation does not mean appearance approval, completed 3D production, implemented gameplay or validated play.** Only explicit selections/replacements are recorded below; model production follows image approval. See the [production guide](03-guides.en.md) for provenance and limitations.

| Item | Created material | Current purpose/status |
|---|---|---|
| Overall art direction | [PSX space mystery](../art/space-concepts/concept-02.png) | Positively received style reference; MOUTHWASHING-inspired direction |
| Countermeasure equipment | [Equipment sheet](../art/space-concepts/equipment-01.png) | Baton, Pressure Caster and Decoy Beacon appearance proposals; effects not implemented |
| Gameplay atmosphere | [Exploration](../art/space-concepts/gameplay-01.png) · [Encounter](../art/space-concepts/gameplay-02.png) · [Receipt](../art/space-concepts/gameplay-03.png) | Existing scene concepts; do not complete the HUD/continuous-play reviews below |
| Employee | [Character sheet](../art/space-concepts/employee-01.png) | Employee appearance and team-color reference, not an actual model or animation |
| Face LED | [Expression sheet](../art/space-concepts/expressions-02.png) | Retain the HELP-to-FLIP OFF version; thicker-finger revision cancelled |
| Interior creature | [THE LISTENER behavior sheet](../art/space-concepts/creature-01-behavior.png) | Proposed sound-responsive behavior and poses |
| Spacecraft | [Interior layout](../art/space-concepts/ship-interior-01.png) | Preparation, loading and access-space concept |
| Route selection | [PSX route-terminal UI](../art/space-concepts/route-terminal-02.png) | Style reference replacing the cleaner screen |
| First destination | [CINDER DEPOT routes/perspectives](../art/space-concepts/cinder-depot-layout-01.png) | Provisional first site: landing, detour, shortcut, receipt and clue locations |
| Time pressure/perimeter | [Suppression integration sheet](../art/space-concepts/cinder-depot-suppression-01.png) | Outer threats and indirect time-cue proposal; matched-view stage comparison remains future work |
| Outer creature | [Initial A THE STRIDER](../art/space-concepts/outer-threats-01.png) · [Production sheet](../art/space-concepts/strider-production-01.png) · [Production guide](../art/space-concepts/strider-production.en.md) | User selected the initial brown A; current outer-creature production reference, no model/rig produced |
| Delivery props/mystery | [Cargo, receipt device and clues](../art/space-concepts/cargo-receipt-clues-01.png) | Sealed crate, receipt equipment, paperwork and recent-use traces; mystery explanation undecided |
| Equipment purchase/upgrades | [Shop UI](../art/space-concepts/equipment-shop-01.png) | Purchase and locked later-upgrade preview; linked to the [economy draft](economy.en.md) |
| Return settlement | [Settlement UI](../art/space-concepts/shift-report-01.png) | Proposed delivery, condition, return bonus and team-balance display; payments not implemented |

### Replaced/discarded concepts — excluded from current production references

- [Initial overall concept](../art/space-concepts/concept-01.png) records the earlier style.
- [Initial LED expressions](../art/space-concepts/expressions-01.png) precede the HELP replacement. The subsequent thicker-middle-finger revision was deleted at the user's request and has no link.
- [Initial route UI](../art/space-concepts/route-terminal-01.png) records the cleaner UI before the PSX revision.
- [Later outer-creature comparison](../art/space-concepts/outer-threats-02.png) and [more grotesque comparison](../art/space-concepts/outer-threats-03.png) record the selection process. Do not confuse A in those sheets with the current STRIDER.
- [White THE ABSENCE sheet](../art/space-concepts/absence-production-01.png) and its [former production proposal](../art/space-concepts/absence-production.en.md) are discarded and retained only as history.

## Next visualizations — queued

The order below is review priority. Existing scene images do not complete these objectives. Following the inventory task, gameplay framing/HUD, suppression-stage, rescue/extraction and equipment before/after concepts were produced. The continuous first-delivery concept is also created. User approval and actual play validation remain separate for all five items.

| Priority | Material | Scenes/content | Review questions |
|---|---|---|---|
| 1 | Actual gameplay framing/HUD | CINDER DEPOT cargo carrying and pausing before a creature; teammate colors, interaction and equipment charge | How much does cargo obscure the view? Is essential information readable within the PSX style? |
| 2 | 4 suppression stages at the same location | Matched framing for stable → unstable → critical → offline; lighting, machinery, creatures and sound annotations | Can players estimate when to retreat without an exact timer? |
| 3 | Downed, rescue and emergency return | Find teammate → put cargo down → rescue/carry → return aboard | Are the rescuable state and required actions clear? Is the cargo-versus-teammate choice understandable? |
| 4 | Before/after equipment use | Beacon, baton and compressed gas: signals before use, effect area, creature reaction and aftermath | Are each tool's role and limits distinct? Does it avoid implying that outer creatures can be killed? |
| 5 | Continuous first-delivery sequence | Landing → exploration/carrying → encounter/counter → receipt → clue discovery → return | Do scenes connect into one delivery experience? Where do tension and curiosity arise? |

- [x] [Create first-person gameplay framing/HUD concept](../art/space-concepts/gameplay-first-person-01.png).
- [ ] User approval and in-game readability validation of gameplay framing/HUD.
- [x] [Create matched-view suppression-stage concept](../art/space-concepts/suppression-stages-01.png).
- [ ] User approval and actual cue-readability validation of suppression stages.
- [x] [Create downed, rescue and emergency-return concept](../art/space-concepts/rescue-extraction-01.png).
- [ ] User approval and actual rescue transport/return validation.
- [x] [Create before/after equipment-use concept](../art/space-concepts/equipment-before-after-01.png).
- [ ] User approval and actual equipment-effect/cooperation validation.
- [x] [Create continuous first-delivery concept](../art/space-concepts/first-delivery-01.png).
- [ ] User approval and actual first-delivery flow/fun validation.

Start with gameplay framing/HUD and reuse that composition for suppression-stage comparisons. Images review information placement and intended expression. Sound communication, actual controls, time pressure, fear and cooperative fun require separate subsequent play validation.

## Default camera change — first person approved

The user selected first-person gameplay. The [new sheet](../art/space-concepts/gameplay-first-person-01.png) compares empty-handed exploration, two-handed carrying, baton readiness and compressed-air response. The [third-person HUD sheet](../art/space-concepts/gameplay-hud-01.png) and cameras in earlier scenes are historical comparisons, not current camera references. Employee, equipment and facility appearance references remain. Camera direction is approved; new image appearance awaits approval; the gameplay camera is not implemented.

## SPACE-PLAY-03 — LISTENER/rescue experiment (0.4.0)

[Listener specification, launch and validation](space-play-03.en.md). A separate hazard mode implements sound investigation, attack warning, shove, down, hold-R revival and all-down emergency recovery. Carrying/delivery modes remain. Version 0.8.0 adds expanded routes and suppression experiments; host progression saving is also supported. Next work includes final CINDER DEPOT spaces/art, deeper equipment reinvestment and human fear/cooperation evaluation. Final models, carrying employees, death, 4 players and Steam connectivity are not complete.


## SPACE-PLAY-04 — Supply/risk contracts (0.5.0)

[Purchase/use specification](space-play-04.en.md). Listener mode now implements session beacon-license purchase, shared deployment and risk-contract selection. Persistence, the full shop, more destinations and final art remain. Use existing launchers 06/07. Two-process progression checks passed 24, existing rescue checks 21, and language checks 9. Human feel, economy balance and rendered readability of the new supply panel are unverified.


## SPACE-ART-23 — First-area PSX environment modules

Added the [CINDER DEPOT entrance and 6-asset sheet](../art/space-concepts/environment-kit-01.png) and [production/review record](../art/space-concepts/environment-kit-01.en.md). Generated with the built-in image tool using the existing overall concept, ship and first-person sheets. This pending-approval reference compares consistency across wall, corner, door frame, floor, lamp and cargo rack. It is not the exact current map layout or a game capture. No models, materials or game integration were produced; the build remains 0.8.2. The production record preserves the actual prompt and limitations in floor mottling density and connection dimensions.


## SPACE-ART-24 — CINDER DEPOT overall overhead concept

[Overhead image](../art/space-concepts/cinder-depot-overhead-01.png) · [Generation/edit prompts](../art/space-concepts/cinder-depot-overhead-01.prompt.txt)

Following the user's request, review the whole layout before producing individual modules. Used the recent environment kit and earlier site layout as references for a built-in generated overhead roof-cutaway concept. Labels identify 01 ship/return, 02 west storage detour, 03 central sorting yard, 04 receiving bay, 05 optional office clue, 06 east suppression equipment and 07 outer entry. Solid teal marks the longer sheltered detour; ochre dashes mark the shorter route past the normal creature. Both converge on the same receiving bay. The teal route does not guarantee safety after outer-creature entry.

An additional image edit clarified the initially ambiguous east maintenance lane and outer bridge connections. Visually checked the arrangement and palette, but detailed start-arrow continuity, threshold traversal, exact carrying widths and physical reachability of every corridor remain unverified. Check a planar connectivity layout before constructing the actual map. Floor mottling density also requires adjustment in the real-time view.

This is a layout proposal rather than an exact reconstruction or measured drawing of the current 32×44m test map. No photogrammetry, mesh, model or game integration was performed. Preserve the 0.8.2 executable and existing images. Overall layout and individual module approvals remain separately pending. Updated Korean/English documentation and checked links.


## SPACE-ART-25 — Tripo PSX kit


2026-09-13: [Complete demo cycle and 44 art production units](demo-art-list.en.md). Production proposal for the user goal, not approval of new appearances/timing or completed production.

[Ship exterior — A selected](ship-concepts.en.md). Comparison concepts before 3D production.


2026-09-16: [HTML layout study: 18 spaces, loops and emergency exit](map-study.en.md). Unity integration and human feel testing remain pending. Ship exterior production is deferred.


2026-09-16 / MAP-STUDY-03: Added west loading yard, east service yard, southern outdoor route and inside-only exit to HTML. Connectivity, delivery cycle and outdoor return automation passed; browser visuals inspected. Unity, online and human fun testing not performed. [MAP-STUDY-03](map-study.en.md).

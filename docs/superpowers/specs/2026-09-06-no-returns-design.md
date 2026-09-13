# NO RETURNS

[한국어](2026-09-06-no-returns-design.ko.md)

> **Historical record · retired/superseded 2026-09-12.** The [PSX space-delivery design](../../current/01-overview.en.md) is current. Implementation, launch, art and values below are historical, not current status. Deleted files remain path records only.


> Documentation update: versions and values here belong to this file's original context. Check the [documentation home](../../README.en.md) first for current rules and outstanding work.

> The packages are alive. Ship them anyway.

**Document:** Concept design v0.2  
**Created:** September 6, 2026; updated September 7, 2026  
**Status:** The concept, English authoring, 3D third-person perspective, and Steam release target are confirmed. Detailed rules remain proposals for playtesting.  
**Canonical design language:** English, with complete Korean documentation maintained alongside it.

## 1. Direction

### Confirmed with the creator

- Develop the living-package delivery concept, originally discussed as "반품 불가."
- Focus on cooperation: everyone wants the team to succeed, but helpful actions can cause accidents.
- Online multiplayer is a requirement.
- Keep development time and spending small.
- Use absurd situations and randomness to create opportunities for playful improvisation.
- Plan for a global release, with English as the original language for design and game text. Always provide complete Korean document counterparts.
- Godot is the intended engine. Art and asset selection come later.
- Use a 3D third-person camera so players can see their own character and nearby coworkers.
- Prepare for a Steam release on Windows PC. The creator has not registered for Steamworks yet.

### Working assumptions

- **Working title:** NO RETURNS. Final naming is a later release decision.
- **Initial release target:** Windows PC on Steam.
- **Players:** 2–4 friends in a private online session.
- **Session:** One self-contained shift, targeting 10–15 minutes including results and a quick restart.
- **Environment:** One compact shipping depot with a readable layout.
- **Camera:** 3D third-person, with a player-controlled view and a clear view of carried cargo.
- **Business format:** A small, complete game. Pricing and commercial scope will be decided after the core playtest.

## 2. High concept

NO RETURNS is an online cooperative shipping game for 2–4 players. Work a shift at a depot that accepts living packages. Carry, aim, and throw unpredictable cargo into the correct dispatch bays before time runs out. A sneezing box can clear a jam or launch your coworker across the room. A sticky parcel can bundle a delivery or attach itself to the person trying to help.

Learn each package's behavior, combine its effects with other cargo, and turn a disastrous shift into a successful delivery run.

**Player promise:** Make a plan with your friends, watch it go wrong, and find a ridiculous way to save it.

## 3. What makes the game its own

**Cargo is also equipment.** Every special package has an inconvenient behavior that can be used deliberately to solve a delivery problem.

The central design question for a new package is: "What trouble does it cause, and what useful thing can a player do with that same behavior?"

The game should produce moments such as:

1. A player gets attached to a sticky package while reaching for another delivery.
2. A coworker aims a sneezing package to break the attachment.
3. The sneeze pushes the freed package toward a dispatch bay.
4. Another player catches or redirects it, rescuing the delivery.

This is a desired interaction sequence, not a scripted event. The first prototype must establish whether players discover and enjoy it.

## 4. Design principles

### Clear jobs, visible consequences

Players and spectators should quickly understand which package needs to go where. Package warnings must connect visibly to their effects. A player should be able to explain why an accident happened.

### Reliable controls, troublesome cargo

Movement, pickup, release, and throwing should respond consistently. Difficulty comes from coordinating around package behavior, shared space, and timing.

### Cooperation creates opportunities

All players share the shipment goal. Roles are informal and can change during a shift. Players can receive incoming cargo, transport it, catch a throw, or clear a jam without selecting a class.

### Recoverable accidents

Mistakes cost time and create new situations. Players return to useful play quickly. There is no permanent elimination or extended spectator period.

### Familiar rules in changing combinations

Randomness changes the shipment order and available combinations. Each package type behaves consistently enough to learn and use intentionally.

## 5. The shift loop

1. **Join:** Friends enter a private lobby and ready up.
2. **Briefing:** Show the shared quota, shift timer, and dispatch symbols.
3. **Receive:** A small, capped queue introduces packages into the depot.
4. **Read:** Identify the destination symbol and recognize the package's behavior cues.
5. **Coordinate:** Choose a safe route, pass cargo to a coworker, or use a package effect as a shortcut.
6. **Dispatch:** Send the package into the matching bay. Correct deliveries increase the shared count.
7. **Finish:** Meet the quota before the timer expires to win. Show a compact result and offer immediate replay.

For the first playtest, correct dispatches are worth one delivery each. Wrong-bay deliveries return to an accessible recovery tray after a short delay. Cargo leaving the playable space also returns there. No required package can become permanently unreachable.

Use a finite shift timer and tune the quota from actual playtests. The 10–15 minute session length is a design target, not a measured result.

### Optional overtime

After the team meets the quota, players may agree to accept a harder bonus batch. Base success is secured; the team is pursuing a separate bonus result. This feature belongs to the expanded prototype, after the main loop is enjoyable.

## 6. Player actions

- Move relative to a third-person camera and aim by turning the view.
- Pick up one package, put it down, or throw it.
- Pick up or catch an incoming package when hands are free.
- Point out a destination or hazard with a simple contextual ping in the expanded prototype.

The first version has no dedicated combat, manual finger controls, or character classes. Picking up a package should not require precise hand placement. Recovery from a knockback must be quick enough to keep players involved.

### Third-person camera requirements

Keep the whole character readable while moving and the held package visible while aiming. Use camera collision to avoid views through walls. Bring the camera closer while aiming and preview the intended throw landing area. The preview should explain an ordinary throw; a package's own behavior can still change the outcome.

Character falls and reactions can use short authored motions. Fully simulated ragdolls and physically articulated hands are outside the first prototype. Offer camera sensitivity and reduced camera shake settings.

## 7. Initial package roster

| Package | Predictable behavior | Problem it creates | Useful application | Readable cue |
| --- | --- | --- | --- | --- |
| Standard | Behaves as ordinary cargo | Requires transport and correct routing | Baseline for learning and testing other effects | Plain body and destination symbol |
| Sneezer | After a visible windup, pushes characters and packages in a short forward area; rests before another sneeze | Knocks cargo off its route and interrupts coworkers | Sends cargo across a gap or separates a sticky attachment | Expanding face/body, directional cue, and a matching sound |
| Clinger | Temporarily attaches to one character or one other package on contact | Tethers a carrier or joins cargo that should be separated | Carries two packages together for a short interval | Stretching adhesive and a visible link |
| Hopper | Jumps at a regular, signaled interval while unheld; holding pauses the jump cycle | Escapes an unattended staging area | Crosses a low obstacle when positioned and released carefully | Squash motion and a landing/jump indicator |

### Interaction boundaries

- Begin with Standard, Sneezer, and Clinger. Add Hopper only after the first combination is fun and understandable.
- A Clinger can have only one attachment at a time. Attached cargo cannot form longer chains.
- Adhesion expires automatically; a sneeze can break it early. Both outcomes need clear feedback.
- A sneeze may affect multiple nearby objects, but every shift has a small active-cargo cap.
- Held packages use a stable carry position. Their behavior should be readable while carried.
- Every required shipment must have a conventional carrying route. A specific random package combination is never required to finish.
- Package timing, throw strength, adhesion duration, and cargo cap are playtest tuning values. Introduce them one at a time and record the tested settings.

## 8. Depot design

The first depot contains one intake area, a central work area, a recovery tray, and two dispatch bays. Bay labels use distinct symbols and colors together.

A low divider creates a safe walking route around it and a shorter route for coordinated throws or package effects. Players can see the consequence of a shortcut from the shared work area.

Keep sightlines open and avoid hidden required rooms. A small map should still provide space to recover dropped cargo and step away from an imminent sneeze.

## 9. Randomness and replay

The first prototype randomizes package order and destination assignment within controlled limits. Introduce each special behavior clearly before combining several active hazards.

- Keep early shipments manageable.
- Maintain a recovery interval after a disruptive shipment.
- Ensure the quota remains achievable for the selected player count.
- Preserve recognizable package rules between sessions.
- Allow a fixed sequence during testing so a confusing or broken interaction can be reproduced.

Environmental modifiers and additional package types are expansion candidates. The first playtest does not depend on a large content pool.

## 10. Global audience and presentation

### English as the source language

Write the original UI copy, package names, tutorial prompts, design documents, and store pitch in English. Always maintain complete Korean documentation alongside it for the creator's review. Use short, direct sentences and consistent terminology: **package**, **dispatch bay**, **shift**, and **quota**.

Example interface copy:

- "Deliver 6 packages."
- "Wrong bay!"
- "Sneeze incoming!"
- "Quota met!"
- "Accept overtime?"
- "Shift complete."

### Humor that travels

Build humor around anticipation, motion, timing, facial reactions, overconfidence, and visible consequences. Optional workplace copy can add character, but reading jokes is never required to understand a hazard.

The player's microphone and spoken language do not drive game rules. Friends may use their existing voice-chat service during early tests.

### Localization and accessibility requirements

- Keep player-facing strings separate from gameplay logic when implementation begins.
- Use symbols or shapes alongside color for destinations and warnings.
- Pair important audio cues with visible cues.
- Allow translated text to expand without breaking the interface.
- Localize language labels and input prompts consistently when additional languages are added.
- Choose launch languages after playtesting and audience research; global intent does not commit the project to every language at launch.

### Watching the game

Keep the shared goal and current progress easy to see. Players should be identifiable at a glance, and package windups must remain visible during busy moments. Test whether a new viewer can understand a short accident clip without a narrated explanation.

Built-in streaming integrations, automated clip capture, and an in-game voice system are outside the initial scope.

## 11. Online requirements

Online play is part of the first functional prototype. Start with two players connecting from separate networks, then expand to four.

The proposed model has one player host the session and decide delivery results, package ownership, attachments, and random events. Other players receive the resulting state. This is a direction to validate, not a selected network integration.

The proposed connection path is a private Steam lobby plus modern Steam P2P networking, accessed through a Godot-compatible integration. A lobby gathers players; a separate transport carries gameplay state. Validate this combination with the selected Godot version before treating it as implemented. A direct-IP local test does not establish a usable global-release join flow. See the [online and build plan](../../steam/04-online-and-build-plan.md).

Host departure may end an early prototype session, with a clear message for the remaining players. Automatic host migration is outside the initial scope.

Test contention over the same package, disconnects while holding cargo, delayed actions, and recovery after an interrupted delivery. Players must agree on whether a package is held, attached, or shipped.

Godot's official networking documentation describes the available high-level networking facilities and the internet-hosting considerations: [High-level multiplayer](https://docs.godotengine.org/en/stable/tutorials/networking/high_level_multiplayer.html).

## 12. Smallest useful prototype

### First playable experiment

- One graybox depot and two dispatch bays.
- Two online players, tested across separate networks.
- Standard, Sneezer, and Clinger packages.
- Movement, pickup, put-down, throw, and a forgiving catch interaction.
- A fixed shift, shared quota, cargo recovery, and restart.
- A deliberately small number of simultaneously active packages.
- English prompts and visible package cues using placeholder assets.

### Next experiment, if the first one works

- Four online players.
- Hopper packages.
- Controlled shipment variations and player-count tuning.
- A contextual ping.
- Optional overtime and a compact results screen.
- A usable friend-join flow appropriate to the selected distribution platform.

### Deferred scope

Additional depots, progression, cosmetics, procedural levels, workshop support, public matchmaking, console versions, integrated voice, and spectator integrations are deferred. Art production begins once the core interaction is worth keeping.

No release date or cost estimate is established by this concept. Estimate the next development slice after confirming developer availability and testing the connection approach. The [release roadmap](../../steam/01-release-roadmap.md) proposes an effort cap for the first experiment. Expand scope only after the smallest prototype meets the playtest goals.

## 13. Playtest questions and decision gates

| Question | Evidence to collect | Design response if it fails |
| --- | --- | --- |
| Can new players understand the job? | Players identify the destination and make a delivery after a brief introduction | Simplify labels and depot layout |
| Do packages create intentional teamwork? | Players voluntarily coordinate a pass or use a package effect to help a coworker | Improve the useful application before adding more types |
| Are accidents understandable? | Players can explain the cause of a recent mistake | Strengthen cues and reduce overlapping effects |
| Can players recover quickly? | A disrupted player returns to meaningful participation without a long wait | Shorten recovery and remove unreachable cargo states |
| Is the interaction enjoyable online? | Both players see consistent ownership, attachments, and shipment results | Fix synchronization and simplify object behavior |
| Does the game make sense to a viewer? | A viewer unfamiliar with the game can describe the goal and cause of an accident in a short clip | Improve framing and visual signals |
| Is there a reason to replay? | Players propose a new tactic and choose another shift | Rework the core interaction before expanding content |

These are validation goals, not claims about an existing game. Test with people outside the design conversation before treating the concept as validated.

## 14. Decisions to make next

1. Review the core package interactions and the first shift layout.
2. Confirm developer availability and the proposed effort cap for the first experiment.
3. Complete Steamworks onboarding using the creator's actual account and details.
4. Validate a compatible Godot and Steam networking integration before committing to production scope.

The next design revision should refine a representative minute of play. The [Steam preparation pack](../../../README.en.md) tracks release work and distinguishes prepared documents from actual completed release requirements.

## 15. Reference observations

- [How to Fish](https://store.steampowered.com/app/4001890/How_to_Fish/) combines a catch-and-sell progression loop with unusual tools and trick-shot rewards. The useful reference is the relationship between a clear shared activity and opportunities for improvisation.
- [MECCHA CHAMELEON](https://store.steampowered.com/app/4704690/MECCHA_CHAMELEON/) centers on painting the character to blend into the environment. The useful reference is how a compact rule can make player-created decisions visible and entertaining.

These references inform design hypotheses. They do not demonstrate that NO RETURNS will be enjoyable or commercially successful.

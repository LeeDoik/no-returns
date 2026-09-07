# Depot equipment and creatures — Design notes v0.1

[한국어](2026-09-07-depot-creature-design.ko.md)

**Follow-up:** This document records the original proposals. Version 0.6 subsequently implemented the larger night map, reversing conveyor and nest placeholder only. Packrat AI, pressure-plate doors and launchers remain unimplemented. See the [0.6 guide](../../prototype/06-night-depot.en.md) for current status.

**Created:** September 7, 2026  
**Status:** Design proposals collected from the conversation. Not implemented; values, enjoyment and difficulty are unvalidated.  
**Current request:** Record the design only. Do not change playtest 0.5 or its distribution files.

## 1. Direction

Keep shipping central while adding equipment that is useful but can cause accidents, and warehouse creatures that play with packages. Build cooperation through carrying, luring, recovery and cargo effects rather than a separate combat activity. Accidents should have visible causes and allow a quick return to shipping.

We compared equipment-focused expansion, moving obstacles, and multiple floors/rooms. Prioritize equipment that preserves visibility in a small depot, adding a creature to test whether lure and delivery roles emerge naturally. Multiple floors/rooms are deferred because of routing, camera and online-validation costs.

## 2. First proposed experiment

Propose **rearranging the existing depot + one reversible conveyor + one Packrat** as the first combination to test. Adding many devices together would make the sources of enjoyment and frustration harder to identify, so defer the pressure-plate door and shipping launcher.

Reuse Standard, Sneezer, Clinger and Hopper cargo. Start with primitive visuals; buying commercial assets or producing final characters is outside this slice.

## 3. Map size and layout

**Follow-up feedback:** The creator noted that one throw currently reaches dispatch and requested a larger map. The size-retention option below remains an earlier proposal; the first map's theme, expanded dimensions and intake-to-dispatch route will be reconsidered in the next discussion. Final map size is undecided.

The current floor is approximately **20 × 23 m**. Keep that size initially; consider expanding to around **24 × 28 m** only if play evidence shows persistent congestion with four workers. These are initial proposals, not locked dimensions.

| Space | Initial proposal | Purpose |
| --- | --- | --- |
| Main routes | 2.5–3 m wide | Room for workers carrying packages to pass |
| Shortcuts | 1.5–2 m wide | Optional routes requiring timing and yielding |
| Central work area | Preserve open sightlines | See coworkers' actions and accidents |
| Main flow | Intake → central work area → A/B dispatch | Make the objective and travel direction readable |
| Side routes | Bypasses that do not require equipment | Avoid mandatory cargo combinations or device use |
| Packrat nest | One near a wall, accessible to workers | Keep pursuit and recovery short and clear |

Use low sorting tables as temporary work surfaces. Put tall shelves along walls, and add empty pallets and dispatch beacons for warehouse context. Do not give every background prop physics or its own interaction initially.

## 4. Equipment candidates

| Element | Useful action | Expected accident | Priority |
| --- | --- | --- | --- |
| Reversible conveyor | Send packages to a coworker receiving at the far end | Another worker flips the lever, reversing cargo and workers | First experiment |
| Pressure-plate shortcut door | One worker holds the plate while another carries cargo through | A Hopper used as a weight jumps and the door closes | Later candidate |
| Button-operated shipping launcher | Launch two attached packages together | Launch the worker still standing on it while loading | Later candidate |

Propose light/sound warnings before the door closes, reopening if a worker or package is caught. The launcher also needs visible anticipation before activation. Conveyor speed/length, door warning time, launcher force and cooldown remain undecided.

Equipment offers an alternative to safe carrying. Walking around and using equipment for faster delivery must both remain viable. Prefer accidents caused by recognizable cargo behavior and worker actions over unexplained random breakdowns.

## 5. First creature: Packrat / 포장쥐

**Packrat** is the English working name; **포장쥐** is its Korean working name. This warehouse creature grabs unattended packages and drags them to its nest. Start with one type and one creature; instantly stealing a held package is outside the first slice.

### Basic behavior

1. Select a package resting on the floor.
2. Telegraph the attempt visibly, such as perking up its ears. Give workers a chance to pick up the package first.
3. Grab and drag it toward an accessible nest. Workers should be able to catch up.
4. Drop it and flee when hit by a sneeze or when a nearby worker retrieves it.
5. Rest briefly instead of immediately pursuing the recovered package or worker again.

The first experiment includes **stealing, worker retrieval and sneeze-driven retreat**. Also observe using a bait package to lure it. No separate health, combat or killing system is proposed.

### Cargo combinations

| Combination | Desired situation | Status |
| --- | --- | --- |
| Sneezer | The stolen package sneezes and the Packrat drops it and flees | First experiment |
| Hopper | The creature loses its grip when the package jumps | Later combination candidate |
| Clinger | A trap package intended to stop the thief sticks to the coworker coming to help | First observe situations created by existing rules |

A new rule allowing attachment to the Packrat is not included yet. How the Hopper clock behaves while being dragged also needs later design work.

### Preventing blocked progress

- Never permanently remove cargo or take it somewhere inaccessible.
- Workers must be able to retrieve cargo easily even at the nest.
- Design it to give up rather than persist when its path is blocked or its cargo is invalid.
- Add a recovery interval that limits immediate repeated theft.
- Do not let the creature make the delivery quota impossible to complete.

Movement speed, detection distance, anticipation duration, rest duration and the exact retrieval input remain undecided. Specify them in the implementation plan, then tune from play evidence.

## 6. Other creature candidates

| Candidate | Disruption | Useful counterplay | Status |
| --- | --- | --- | --- |
| Sleepy gatekeeper | Sleeps across a shortcut | Wake it with a sneeze or lure it using a thrown package | Alternative candidate |
| Package follower | Follows workers and pesters them for cargo | An empty-handed coworker lures it away during shipping | Alternative candidate |

Neither is added to the first Packrat experiment. Their names, appearance and detailed behavior are not finalized.

## 7. Validation questions and next steps

- Can a newcomer understand the creature's target from its warning alone?
- Does voluntary cooperation emerge, with one worker luring while another ships?
- Is recovery short and understandable, or does it feel like repetitive work?
- Can the conveyor and creature also be used to help shipping?
- Are four workers constantly blocked, or is one person stuck with the same role?
- Can a viewer explain an accident, and do players try a different tactic next shift?

If implementation is requested later, specify conveyor and Packrat rules first. Online decisions should fit the existing host-authoritative design, and creature cargo ownership must not conflict with worker pickup. Adding device/creature state also requires rechecking the existing packet budget. This document does not represent completed implementation or a new release.

Related: [Core design](2026-09-06-no-returns-design.md), [current 0.5 play guide](../../prototype/05-expanded.en.md).

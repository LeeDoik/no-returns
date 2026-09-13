# Reactive delivery areas · Playtest 0.8.0

[한국어](08-reactive-delivery.ko.md)

> **Historical record · retired/superseded 2026-09-12.** The [PSX space-delivery design](../current/01-overview.en.md) is current. Implementation, launch, art and values below are historical, not current status. Deleted files remain path records only.


> Documentation update: versions and values here belong to this file's original context. Check the [documentation home](../README.en.md) first for current rules and outstanding work.

September 8, 2026. This development build connects existing mechanisms to delivery and adds reactive props. It is not a completed Steam release or final art version.

## Try this first

Hold the Sneezer toward the paperwork on the intake counter. When it sneezes in front of the stack, 18 sheets initially travel together for 0.14 seconds, then fan apart. They hit nearby walls, desks and floors and restore to their original stack after eight seconds. This applies to 13 existing desk, counter and document-tray locations. Paper is visual: it does not change delivery scores or create physical obstacles. Sneezes have a 4.5 m range, a 50-degree half-angle and a 1.6 m height difference limit; walls block them.

## The sorting line as a delivery tool

The central eight-metre intake conveyor passes through a low parcel opening into sorting. Workers use the side doors. A 0.6 m lip stops normal cargo at the end, where a coworker can lift it across. The reverse-belt binding also permits return transport.

- **Hopper:** jumps over the lip when left in front of it.
- **Clinger:** attach it to the Hopper to cross together. During its five-second bond, the rat cannot split or steal either partner. Detached parcels become targets again.
- **Sneezer:** put normal cargo near the pale launch line and sneeze from behind it to clear the lip. Cargo placed right against the lip can hit it before gaining height. A sneeze also breaks adhesion and scares a nearby rat for three seconds, making it drop stolen cargo.

The paper nest is under the right-hand sorting desk. Contract one reveals the nest; from contract two, the rat patrols the conveyor exit. Retrieve stolen cargo at the front of the nest. The existing horn still works. A floor-contact tolerance issue that caused repeated claiming and dropping of resting cargo was also fixed.

## 24 props throughout the map

Eight clusters each contain one of the three types: intake; west entrance, bend and dispatch approach; central sorting; east bend and dispatch approach; and the final handoff area. Step on them, throw parcels at them or sneeze at them. They do not deduct points.

| Mark | Reaction | Reuse |
|---|---|---|
| POP | A packing cushion compresses and bursts, pushing nearby workers and loose cargo upward/outward. | 8 seconds |
| UP ↑ | A spring lifts workers/cargo and adds a small push in its facing direction. Workers retain held parcels. | 3 seconds, after leaving and re-entering |
| TILT | A moving worker or thrown parcel tips an empty-carton tower. Nearby towers/cushions can continue the chain. | 10 seconds |

A 0.18-second compression precedes activation. Chains reach visible props within 2.65 m and respect walls. Fallen pieces are decorative and never permanently accumulate in passages. Workers overhead or behind walls cannot activate them. Held, recovering, delivered or rat-held cargo receives no independent push. Existing carrying routes, cross-links, pressure gate and gust area remain.

## Editing, multiplayer and validation scope

In Godot, open `Gameplay/Reactions` in `scenes/maps/shipping_shrine.tscn` to find the eight clusters and 13 `Paperwork` locations. Duplicate, move or rotate a prop and change its type, radius, launch strength or recovery time in the Inspector. See the [full editing guide](../development/map-editing.en.md). Running or building does not overwrite the saved map.

The host decides reactions and impulses; guests receive matching event numbers, directions and state. Compression and firing transmit immediately, and duplicate state does not replay sound. Each PC computes visual paper trajectories, so reception timing can cause minor differences. Everyone uses **the same 0.8.0 ZIP, protocol 10**. Existing EXE/ZIP paths remain unchanged.

Automated physics, behavior and local multiplayer checks verify implementation. Friend-group difficulty/fun, low-end performance, separate PCs, Steam invitations/relay and review remain separately incomplete.

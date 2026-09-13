# 2. Current game specification

[한국어](02-spec.ko.md)

> **Historical record · retired/superseded 2026-09-12.** The [PSX space-delivery design](../../../../current/01-overview.en.md) is current. Implementation, launch, art and values below are historical, not current status. Deleted files remain path records only.


Main development is now NO RETURNS Unity. Godot specifications and checks below describe the preserved game, not completed Unity functionality. 현재 기준 / Current baseline (`18-unity-mainline.en.md`; retired file).

Reviewed: 2026-09-09 · Game 0.9.4 · Code baseline: this reactive-prop revision.


This document is the current behavior reference. If historical plans conflict, check this document and its linked code. If new code differs, update this document rather than reverting code to historical numbers. Values below are defaults unless stated otherwise.

## Modes, progression and rewards

- Solo or 2–4 players. There is a three-contract campaign and 180-second practice.
- Campaign duration is 240 seconds plus 20 per time-upgrade level. Quotas for 1/2/3/4 players are 5/6/8/10 plus contract stage (0/1/2) × 2.
- Each delivery earns 10 credits; a relay adds 5. Successful contracts add earnings plus a 20-credit success bonus to shared funds. Failure does not award funds through that settlement path.
- Only the host purchases after success and before the final contract. Boots cost 20 (maximum two levels, +8% movement multiplier each), time costs 25 (maximum two levels), horn costs 20 (maximum one level, cooldown 8→6 seconds). All players ready up to advance.
- Practice quotas are 5/6/8/10. The 60-second bonus sets a target of current score plus 3/4/5/6.

Sources: contracts.gd (`../../scripts/contracts.gd`; retired file), round_rules.gd (`../../scripts/round_rules.gd`; retired file), main.gd (`../../scripts/main.gd`; retired file).

## Input and pause

WASD moves, mouse looks, Space jumps, E picks up/catches/drops, left click throws, Q pings, F operates the belt lever, R sounds the horn, H shows help and Esc opens menus. Production bindings are configurable. Solo menus pause play; online shifts continue. The lab has separate controls in the [production guide](03-guides.en.md).

## Worker and parcel values

| Parameter | Current value |
|---|---|
| Empty-handed maximum speed / acceleration | 3.2 m/s / 24 m/s² |
| Forward / side-back carrying speed | 1.6 / 1.2 m/s (diagonals follow input-axis scaling) |
| Carry acceleration / jump initial speed | 12 m/s² / 4.8 m/s |
| Gravity | Worker and ordinary falls: 9.81 m/s² |
| Base parcel collider | 0.8 m cube |
| Mass: standard / sneezer / clinger / hopper | 3 / 4 / 5 / 2.5 kg |
| Parcel friction / restitution / angular damping | 0.65 / 0.06 / 0.6 |
| Pickup range | Within 2.4 m, plus occlusion, ownership and state checks |
| Throw | Forward 9.5 m/s + up 5.4 m/s + worker velocity |
| Drop | Inherits worker velocity |
| Worker push force cap | 80 N |

Held parcels freeze free simulation, check translation/rotation collisions and move toward the hand position. Carrier-parcel collisions are excepted while a carrying collider is enabled on the worker. If an obstructed parcel's forward gap falls below 0.9 m, carrying ends. This is not weight-driven compliant grabbing. Delivery requires matching destination, no holder and not already delivered.

Sources: worker.gd (`../../scripts/worker.gd`; retired file), cargo.gd (`../../scripts/cargo.gd`; retired file), cargo_rules.gd (`../../scripts/cargo_rules.gd`; retired file).

## Special parcels and devices

| Element | Code-defined behavior |
|---|---|
| Sneezer | Initial wait 6 s, subsequent wait 6–9 s, warning 1.5 s, burst 0.35 s; range 4.5 m, half-angle 50°, height check 1.6 m |
| Clinger | Reach 0.95 m, attachment 5 s; attached workers receive a 0.7 speed multiplier |
| Hopper | Rest 4 s, warning 1 s, forward 2.8 m/s and up 6.5 m/s |
| Conveyor | Speed 2 m/s, lever reach 2.4 m, cooldown 0.5 s |
| Packrat | Active from contract 2; search 6 m, steal reach 1.4 m, warning 0.9 s; horn range 4 m |
| Environment | Plates, doors, gusts, POP/UP/TILT and flying paper are connected. Individual placements/settings follow the saved scene and relevant scripts. |

Sources: sneezer (`../../scripts/sneeze_rules.gd`; retired file), clinger (`../../scripts/clinger.gd`; retired file), hopper (`../../scripts/hopper.gd`; retired file), belt (`../../scripts/conveyor.gd`; retired file), packrat (`../../scripts/packrat.gd`; retired file), route devices (`../../scripts/route_challenges.gd`; retired file), reactive props (`../../scripts/reactive_props.gd`; retired file).

## Animation, networking and persistence

Workers use 41 bones and 18 clips, including eight carry directions, air, landing and throwing. Version 0.9.3 refines transitions and breathing. Transition details (`../art/08-animation-polish.en.md`; retired file). Fully planted hand/foot IK and full-body ragdolls are absent.

Networking uses ENet with host authority, up to four players, default UDP port 27842 and protocol 12. Protocol/map mismatches are checked on admission. At 60 Hz, general states publish every three ticks (20 Hz), packrats every six (10 Hz), and metadata every twelve (5 Hz). Steam friend invites are not connected. Session code (`../../scripts/session.gd`; retired file).

Local settings and completed-run records are saved. Records use `user://run_profile.cfg`; mid-campaign saves, Steam Cloud and online leaderboards are absent. Lab values use `user://physics-lab.json` only and never automatically change production.

## 0.9.4 reactive-device revision

The spring plate retains its thickness while spring/guide height follows its underside. Paper reacts to sneezes and fast worker/cargo contacts; decorative carton rigid bodies topple from impact direction, strength and position. Impact information replicates online. Values and presentation/gameplay boundaries (`../art/09-reactive-impact.en.md`; retired file).


## D representative art integration · 2026-09-10

Applied the 41-joint/18-clip worker, workwear team-color mask, three-expression/two-lid sneezer and four recessed conveyor modules. Game rules and protocol remain unchanged. See the D integration specification (`../art/10-approved-d.en.md`; retired file) for values, sources and validation limits.

## Boundary between expansion and current behavior · 2026-09-10

Expansion design revision 2 (`08-expansion-directions.en.md`; retired file) describes an approved future direction. Current 240-second contracts, success settlement, A/B receipt, crate reset and employee/cargo adhesion rules remain unchanged. Truck, permanent tools, immediate rewards, facility adhesion, employee rescue, safe-socket recovery and map assembly are not current features. This revision changes neither code nor game version.

## EXP-01 implementation · 2026-09-10

Campaign/practice values above remain separate. Expedition receipt requires unheld cargo within radius 1.8m and relative height -0.1–1.0m, below 1m/s for 0.4 seconds. Base payout 30, shift maximum 90, no additional/quality bonuses. Everyone readies within 6m of the truck to return. Wrong deliveries remain on site; completed parcels do not respawn. Company save is expedition_company.json, ENet protocol 13. Share spatial hashes, contract state and authoritative cargo/worker poses. Supports late join/departure, not host migration/mid-shift resume. Out-of-bounds parcels return to original positions, not last-safe sockets. Sources: runner (`../../scripts/expedition.gd`; retired file), contracts/storage (`../../scripts/expedition_rules.gd`; retired file), map (`../../scripts/expedition_map.gd`; retired file).

Play guide and limits (`../prototype/10-expedition.en.md`; retired file).


Unity movement and carrying lab (`15-unity-controls.en.md`; retired file) · UNITY-02 / Unity 0.2.0 / 2026-09-11


2026-09-12 · New Unity delivery experiment: NR-LOOP-01 launch and validation (`22-slapstick-loop.en.md`; retired file) — use that document for implementation, automated evidence and outstanding human validation.

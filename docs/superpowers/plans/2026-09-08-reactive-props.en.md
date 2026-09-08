# Reactive delivery props · 0.8.0

[한국어](2026-09-08-reactive-props.ko.md)

## Design

Use authored clusters throughout intake, both winding routes, central sorting and dispatch. Prefer three reusable, recognizable reactions over unrelated random traps or dozens of simulated debris bodies. Preserve existing carrying clearances and a walk-around option. No score damage or mandatory cargo type. These complement the connected sorting line in the same release.

- **POP / inflated packing cushions:** stepping on or hitting the cushion primes a short compression, then bursts it. A small local upward/outward push moves loose parcels and workers. It reinflates after eight seconds.
- **UP / return spring:** entering the plate launches workers or loose cargo upward and slightly in its facing direction. It resets after three seconds, but a stationary occupant cannot repeatedly fire it. Position it as an optional passing aid.
- **TILT / empty carton towers:** a moving worker, thrown parcel or sneeze tips the stack. Nearby towers/cushions can react after a short delay, making a visible chain. They stand back up after ten seconds. Falling cartons are bounded visual pieces, not unbounded networked debris.

Readable silhouettes, compression before release, finite debris motion and local sound convey cause and effect. Reactions respect walls and floor height. Held, delivered, recovering and rat-held cargo cannot trigger or receive independent cargo impulses. Spring-assisted workers retain what they carry. Only the host computes effects; guests receive phase, remaining time and event sequence. Reset all prop state on restart/contract changes, reject incompatible protocol versions, and avoid replaying sound for duplicate snapshots.

## Work and acceptance

- [x] Implement reusable editor-visible prop and host manager, local effects/audio, bounded chain and replication.
- [x] Place 24 props in named editable clusters with clear ground/ceiling and permanent route clearance.
- [x] Verify worker/cargo launch, impact chain, sneeze trigger, wall/height protection, cooldown/re-entry, state reset and two-process replication.
- [x] Re-run carrying routes and existing multiplayer checks; inspect actual renders; update complete EN/KO guides and Windows package.

No automatic map migration on play/build. Human group playtesting is still required to judge density and timing.

## Added request: sneeze at documents

Convert the 13 existing visible paperwork/form locations into Paper stack props, in addition to the 24 floor props. A sneeze throws 18 sheets in its direction as a tight bundle for 0.14 seconds; the sheets then fan apart, flutter, collide visually with static walls/desks/floors and restore after eight seconds. Other entry/chain triggers do not activate paper. Reuse the same nodes for every burst. Paper does not score or push cargo.

Store phase, timer, event number and direction in a compact float buffer. Keep the existing whole-campaign metadata budget of 2,048 bytes. Send phase changes immediately so the 0.18-second compression cannot disappear between regular 0.2-second updates. Validate warning reception with a real host/guest run, not only a manually published fired state.

## Validation results

33 behavior checks, nine two-process network scenarios and one four-process scenario passed. Tests using old rat/lever coordinates were updated to saved-map positions; those cases and the remaining network tests were rerun after the initial suite. Evidence is in `artifacts/sorting-suite.log`, `artifacts/remaining-network.log` and `artifacts/paperwork-final.log`. The existing 2,048-byte campaign metadata budget remains intact. Actual renders checked the intake passage, prop launch and flying paperwork. Windows export, packed launch, 34 compiled scripts/saved-map audit, and SHA256 verification of the eight ZIP files passed. Package identity is recorded in `artifacts/sorting-pack-audit.log` and `build/NO_RETURNS_0.7/SHA256.json`.

Review identified and resolved rat floor-contact rejection and missing guest compression warnings. External playtests and Steam integration/review are not marked complete.

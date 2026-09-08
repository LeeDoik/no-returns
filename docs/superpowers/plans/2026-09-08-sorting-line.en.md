# Connected sorting line · 0.8.0

[한국어](2026-09-08-sorting-line.ko.md)

## Decision

Bring separate devices into one delivery situation. An 8 m conveyor sends parcels from intake to central sorting through a floor-level cargo passage in the north wall. Workers and held cargo use the existing doors. A 0.6 m sorting lip ends the line. A coworker can lift normal cargo over it, a sneeze can launch it, and a Hopper can jump over on its own. Preserve walking detours and the western throwing hatch.

The Clinger's existing five-second bond carries it with another parcel or lets it ride a Hopper over the lip. Rats cannot split and steal a bonded pair; both become vulnerable after detaching. A sneeze scares the rat for three seconds and makes it drop stolen cargo, subject to existing direction/range/wall-occlusion checks. The existing airhorn remains available.

The rat starts at a paper nest under the right-hand sorting desk and patrols the conveyor exit. It activates from contract 2; the nest remains visible in contract 1 to establish its later appearance. Move decorative stock away from patrol/retrieval paths. Do not add mandatory type-locked puzzles or routes requiring a particular cargo.

## Implementation and validation

- [x] Apply a one-time saved-map migration, preserving editable conveyor/lip/wall passage/nest/spawn/patrol/retrieval nodes.
- [x] Match stripe animation to transport-zone length; implement bonded-pair theft protection and sneeze-driven rat retreat.
- [x] Verify actual physics for passage transport, normal cargo stopping, Hopper/Clinger paired crossing, sneeze cargo launch/rat rescue and nest retrieval. Reject through-wall/out-of-range scare effects.
- [x] Run existing carrying-route/contract/two/four-player checks; update actual bilingual views, docs, Windows package and local Git.

Map fingerprints reject the old layout. The host owns decisions; direct map editing remains available. External friend-group choice/fun validation is separately unfinished.

## Validation results

33 behavior checks, nine two-process network scenarios and one four-process scenario passed. Tests using old rat/lever coordinates were updated to saved-map positions; those cases and the remaining network tests were rerun after the initial suite. Evidence is in `artifacts/sorting-suite.log`, `artifacts/remaining-network.log` and `artifacts/paperwork-final.log`. The existing 2,048-byte campaign metadata budget remains intact. Actual renders checked the intake passage, prop launch and flying paperwork. Windows export, packed launch, 34 compiled scripts/saved-map audit, and SHA256 verification of the eight ZIP files passed. Package identity is recorded in `artifacts/sorting-pack-audit.log` and `build/NO_RETURNS_0.7/SHA256.json`.

Review identified and resolved rat floor-contact rejection and missing guest compression warnings. External playtests and Steam integration/review are not marked complete.

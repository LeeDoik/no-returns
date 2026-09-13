# Art direction pass · 0.7.7

[한국어](2026-09-08-art-direction.ko.md)

> **Historical record · retired/superseded 2026-09-12.** The [PSX space-delivery design](../../current/01-overview.en.md) is current. Implementation, launch, art and values below are historical, not current status. Deleted files remain path records only.


> Documentation update: versions and values here belong to this file's original context. Check the [documentation home](../../README.en.md) first for current rules and outstanding work.

## Goal and diagnosis

The current view asks players to notice large primitives, saturated colors, floating jokes and repeated props simultaneously. Make deliberate selection and finish visible instead of the quantity added. The direction is **an aging delivery depot pretending to be a temple**. Ground the facility so living parcels and delivery mouths feel strange. This pass cannot guarantee that nobody will perceive an AI-made aesthetic.

## Production rules

- Limit backgrounds to concrete gray, worn cream walls and dark green-gray metal. Use teal/ochre to distinguish A/B routes and bays. Cargo and workers should read clearly against the background.
- Reduce floor grids and repeated jokes. Mount signs to physical walls or backing panels; prioritize destinations, controls and hazards. Concentrate humor on the boss display and delivery machinery. Hide small distant text.
- Add wall kick plates, structural trims, roof battens and subtle material variation between metal and concrete. Add no new passage-blocking colliders. Keep furniture and rooms editable in the saved scene.
- Give workers rounded workwear, a cap, gloves, boots, reflective bands and employee numbers. Alternate arms and legs while walking. Preserve colliders, movement speeds, hand positions and network state.
- Distinguish deliverable cargo with cardboard colors and type-specific tape. Preserve sneeze, attachment and jump warnings while limiting small distant text.

## Work and verification

- [x] Use `tools/direct_shrine_art.gd` to revise saved-map colors, materials, finishes, signs and lights. Do not regenerate during play or builds.
- [x] Refine `scripts/worker.gd`, `cargo.gd` and cargo presentation code. Add no new gameplay rules.
- [x] Compare before/after views from identical player cameras. Check Korean/English signs, actual cargo warnings, map editing, carrying, relay and online behavior.
- [x] Update Korean/English guides; verify Windows export and package; record local milestone `v0.7.7`.

Review whether space purpose and destinations read without jokes, workers stand apart from backgrounds, and finishes align with walls and openings. Continue refining fun and visual impression through human playtests.

## Validation record

- Passed 25 behavior checks, eight two-process scenarios and one four-process scenario.
- Fixed carrying-glove rotation identified in read-only review. After the final change, re-ran cargo-facing checks and verified in an actual render that gloves point toward the parcel.
- Compared intake, archive, air-mail, dispatch and overview from matching cameras. Also inspected carrying pose and English air-mail signs.
- Visible map text decreased from 104 to 38, including floor arrows. Actual cargo-state warnings remain separate.
- Reduced wall/floor grain to a low-contrast 0.92–1.0 brightness range. Added no paid external assets or services.

- Windows export, packaged launch, and audit of 28 compiled scripts plus map identity passed. Updated the existing EXE/ZIP paths to 0.7.7.

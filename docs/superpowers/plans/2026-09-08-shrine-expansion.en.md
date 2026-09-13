# Shipping Shrine expansion design and implementation plan

[한국어](2026-09-08-shrine-expansion.ko.md)

> **Historical record · retired/superseded 2026-09-12.** The [PSX space-delivery design](../../current/01-overview.en.md) is current. Implementation, launch, art and values below are historical, not current status. Deleted files remain path records only.


> Documentation update: versions and values here belong to this file's original context. Check the [documentation home](../../README.en.md) first for current rules and outstanding work.

Goal: version 0.7.4 expands the saved map from 32×36 m to 48×60 m (2.5 times the area), adding route choice, cooperation and timing challenges. Following the user's standing request to skip incremental approvals, implement and validate in this session.

Simple enlargement adds empty travel; additional floors increase camera and rat pathfinding work. Choose three horizontal routes. Keep intake and the existing southern section, moving dispatch to X±15, Z-43. Floor bounds: X±24, Z-48…12. A second sorting wall at Z-28 has permanently open bypasses at X±21 and a 4 m central door. Add a relay staging area at Z-18 for setting cargo down.

Central 'UNPAID DOORMAN': workers or free packages on either pressure plate open the door. Hold it open for 6 seconds after leaving the plate so solo players can run through. Detect workers/packages inside the doorway to prevent closure on them. Holding a package outside the plate is not an additional activation condition. The left bypass always stays open.

Right 'EXPRESS AIR MAIL': repeat 5 seconds idle, 1.5 seconds amber warning, 2.5 seconds gust. Push workers and free cargo in the floor-arrow direction. Use the wind to send packages or take the safe adjacent path. Do not independently push held, creature-claimed or attached cargo. Mechanism clocks stop in lobbies/results and reset each contract. Settings and placement remain editable in Godot.

Technology: Godot 4.7.2. Modify the saved PackedScene once, never regenerate at runtime. `scripts/route_challenges.gd` manages plate, gate and gust state/effects. Main calls step only on host/solo. Carry mechanism state in the existing separate metadata; guests update presentation and gate collision from it. Preserve the 1,280-byte motion packet budget. Protocol 9 prevents mixing older behavior. Preserve current rat territory, 240-second contracts and cargo types; tune after human play.

- [x] Write and observe failing tests for map size, new bays, worker/cargo plate activation, anti-crush door, gust warning/direction/ownership protection and reset.
- [x] Expand the saved map, place editable mechanism nodes and signs, implement runtime and online wiring.
- [x] Verify door/gust state and restart reset in two real processes. Update existing tests to read current map locations and run the suite.
- [x] Inspect real-renderer captures for layout/readability, rebuild Windows and audit the pack, update complete bilingual guides/README and local Git.

Exclude existing user settings and separate art work. Enlarged-map difficulty and fun require human play; automated checks cannot certify fun.


## Completion and validation · September 8, 2026

Twenty-three behavior checks, eight real two-process scenarios and the four-process scenario passed. Worker/cargo shape sweeps validated the side bypasses, and intake throws cannot reach dispatch in one flight. New mechanism tests cover physical door clearance, worker/package pressure, six-second grace, anti-crush occupancy, gust warning/direction/ownership protection, replicated appearance/collision and restart.

An initial full run failed the guest movement observation's timing condition once. The isolated scenario passed on recheck, and the final complete suite passed. Final log: `artifacts/expansion-final-tests.log`; initial run: `artifacts/expansion-full-tests.log`; isolated recheck: `artifacts/network-expansion-recheck.log`. Adding physics frames to the new pressure test exposed residual falling velocity in its frozen cargo fixture; explicitly setting stationary velocity fixed the arrangement without relaxing the resting-package condition.

Door collision and material updates now run only when the displayed door/color state changes. The worst tested motion snapshot remains 1,280 bytes; separate campaign/mechanism metadata passed its 2,048-byte limit. Inspected actual-renderer captures of the full layout, closed/open gate and fan presentation. Windows ZIP creation, packaged startup, file hashes and the audit of 28 compiled scripts/map fingerprint passed. Evidence: `artifacts/routes-capture.log`, `artifacts/routes-pack-audit.log`, `build/NO_RETURNS_0.7/SHA256.json`.

Existing executable paths now contain 0.7.4, with complete Korean/English guide updates. Human validation of travel distance, contract difficulty and practical shortcut value remains. This change does not constitute a Steam release.

An independent code review checked host authority, replication, reset, ownership protection and route placement and found no material issues.

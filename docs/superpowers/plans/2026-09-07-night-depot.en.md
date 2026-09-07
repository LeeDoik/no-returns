# Night depot implementation plan

[한국어](2026-09-07-night-depot.ko.md)

September 7, 2026. Target: prototype 0.6. Use subagent-driven-development for the bounded depot presentation task and a final review. The creator approved the larger night depot; the Packrat scope is a nest placeholder only, with AI deferred.

## Constraints and layout

Keep 2–4 host-authoritative workers and the existing cargo rules. Preserve full Korean/English documentation. No paid art. Build one open-roof graybox depot with readable lighting, wall shelving, sorting tables, marked routes, dispatch shutters and an accessible nest placeholder.

Floor: 32 × 36 m centered at (0, -9) in X/Z; walls X ±16, Z -27 and +9. Keep the existing intake cargo positions and worker spawns near Z 1–5.3. Dispatch centers: A (-10, -22), B (+10, -22); bay half extents remain 1.5 × 1.3 m, center Y .65 for fixtures. Recovery margin: X ±17, Z -28..10, below Y -4. A 16 m wide, 3.4 m tall central sorting partition at Z -10 prevents direct intake-to-dispatch throws. Both sides have permanent walkable bypasses near X ±10. Retain the low practice divider at (1, .5, -1.5) for Hopper/throw combinations. Avoid blocking existing intake approach lanes.

Conveyor: flush with floor, centered (0, -5), width 2.2 m, length 6 m. Forward points toward negative Z. Speed 2 m/s, lever at (2, 0, -4), interaction range 2.4 m. F reverses it, with a .5-second shared cooldown, server distance/facing/line-of-sight checks and no requirement to release held cargo. Workers receive external belt velocity; free grounded cargo approaches belt velocity, excluding held, recovering and attached cargo. Guests only present replicated state. Reset forward each shift and clear movement outside play.

## Work

- [x] Centralize map bounds/bays in `scripts/depot_layout.gd`; failing layout/route/delivery tests first; update cargo recovery and migrate old bay fixtures to shared positions.
- [x] Replace `scripts/depot.gd` visuals/geometry: industrial night setting, open sightlines, side routes, high blocker, shelves/lights/shutters, intake and nest. Keep `box`, `label`, `overview`, `marker` interfaces. Add conveyor visual arrow methods owned by the conveyor controller.
- [x] Add `scripts/conveyor.gd`, F action/copy, authoritative movement and compact replicated direction. Test reversal validation, worker/cargo movement, inactive/held exclusions and two-process agreement.
- [x] Validate actual paths to both bays carrying cargo, direct-throw prevention, recovery at expanded edges, all regressions, worst snapshot <=1280 bytes and actual rendered Korean/English views.
- [x] Publish paired 0.6 play guide, update entry points/build paths, export Windows package, verify clean-folder launch, review and commit to main. Preserve v0.5.0 and create v0.6.0 only after validation.

## Evidence

Seventeen behavior tests, four real two-process scenarios, the four-process scenario and the SteamPipe generator test passed. Checks cover worker/held-cargo routes to both bay centers, intake-throw first contact, expanded-boundary recovery, conveyor range/facing/line-of-sight/cooldown and guest reversal. The worst tested snapshot is 1,280 bytes, equal to the current limit. Actual rendering verified Korean/English menus and lever prompts, plus overhead, third-person and dispatch/nest views. Windows export and executable smoke checks passed; all 17 gameplay scripts are packed, and practice rendered in a separate folder containing only the executable and game pack.

Review findings for bonus/departure belt presentation and mismatched F-prompt conditions were fixed. Route sweeps now reach bay centers at Z -22. Initial old-layout bounds/delivery failures, floor-only fixtures hitting new sorting furniture, and a type-inference error during the height-guard edit were resolved before the full suite passed again. Certificate-store and cache/user-directory environment warnings were distinguished from script failures. Keep 180 seconds and existing quotas until real fun/difficulty testing. Packrat AI is not implemented. Record local Git 0.6 history and tag while preserving v0.5.0 and its build.

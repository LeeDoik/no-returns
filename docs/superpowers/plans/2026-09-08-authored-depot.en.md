# Focused intake and presentation refinement · 0.7.8

[한국어](2026-09-08-authored-depot.ko.md)

> Documentation update: versions and values here belong to this file's original context. Check the [documentation home](../../README.en.md) first for current rules and outstanding work.

## Direction

Compose the first intake room as a place people worked in. Avoid identical furniture duplication or random prop scattering. Differentiate forms and objects around receiving, packing and record keeping. Put wear only on handled edges and work surfaces. The goal is coherent, deliberate craft, not a guarantee of mistaken beliefs about authorship.

## Implementation

- [x] Implement reusable beveled geometry and verify bounds, surface orientation and closed topology. Apply it to furniture, parcels and worker details while preserving collision sizes. Support Godot Dimensions, Color and a new Edge Bevel setting.
- [x] Replace the giant intake boss display with a small office portrait and individually compose reception, packing and records furniture. Add invoices, tape, stamps and scales on work surfaces; replace display mats with loading-location marks. Mount the room clock and signs to real walls. Differentiate reused furniture in other zones by role.
- [x] Keep the worker body and hands aligned with the parcel while carrying. Briefly blend pickup/put-down poses. Reduce constant names on safe parcels while retaining hazard warnings. Distinguish delivery confirmation with a short stamp/mechanism sound.
- [x] Validate routes, hatch relay, cargo behavior and online play; inspect actual Korean/English views and carrying poses. Refresh the Windows build/package and record local `v0.7.8`.

## Scope and acceptance

Preserve map dimensions, spawn points, dispatch bays, rat territory, physics and network protocol. Keep the new layout directly editable in the saved map. Decoration adds no passage-blocking colliders. Differentiate repeated props while limiting the visibility distance of small details. Prioritize the first view, turning corners and consistent carrying over new features or zone counts.

## Validation results

- Full suite passed: 27 behavior checks, 8 two-player network scenarios and 1 four-player network scenario. Network checks used multiple processes on this PC, not an external network.
- Bevel bounds, surface orientation, closed topology and editor size/color/collision synchronization passed. The initial check failed before implementation and passed after implementation.
- Carrying direction, pose transitions, sneeze warnings and delivery sound duration/amplitude checks passed. Audio validation checked the waveform; it does not replace listening evaluation.
- Inspected actual Korean/English renders, worktops and carrying poses. After the final display-mat cleanup, route checks and capture checks passed again.
- Windows export and packed launch passed. Verified 29 compiled runtime scripts and saved-map identity in the package. The ZIP includes both language guides, license notices and SHA256 manifest.
- Independent code review reported no additional defects. Real friend-group fun, long sessions and target-PC performance remain unverified.

Executable: `build/NO_RETURNS_0.7/NO_RETURNS.exe`. Package: `build/NO_RETURNS_0.7_Windows.zip`. Local baseline: `v0.7.8`.

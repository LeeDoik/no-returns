# Packrat search and approach fix — 0.7.1

[한국어](2026-09-07-packrat-search.ko.md)

> Documentation update: versions and values here belong to this file's original context. Check the [documentation home](../README.en.md) first for current rules and outstanding work.

## Cause

Previously Packrat searched only within its 1.4 m steal range and had no approach state. A grounded package about 4 m away in the real depot reproduced passing by without theft. Prior tests placed cargo beside the rat and missed this gap.

## Change

Find visible free grounded cargo within 6 m in the right-side activity area, approach, then warn for 0.9 seconds within 1.4 m. Held, attached, airborne, protected and wall-hidden cargo remain excluded. Pickup during pursuit cancels it. No movement progress for 1.25 seconds ignores that target for three seconds. Patrol waypoints (10,-3.2), (10,-8), (10,-14), (13,-17) avoid the nest. Localized approach status and PACKAGE SPOTTED! are shared online.

Contract 1 and short practice have no rat. It remains in the right lane in contracts 2–3; the left carrying route stays available. Existing 0.7 executable/ZIP paths are refreshed and the in-game version reads 0.7.1. The application name remains stable to preserve settings/record locations. Git v0.7.0 is preserved; the fix baseline is v0.7.1.

## Validation

`tests/test_packrat.gd` now checks approaching and stealing a package about 4 m away in the real depot without teleportation. It failed before and passed after the fix. Pickup cancellation and existing settled carry/nest/horn/wall/recovery checks remain. `tests/test_campaign_network.gd` now uses distant cargo and verifies approach on host and guest before theft/horn rescue/completion. Logs: `artifacts/packrat-seek-red.log`, `packrat-seek-green.log`, `packrat-seek-network-run.log`. These do not replace separate-PC or real-friend playtesting.

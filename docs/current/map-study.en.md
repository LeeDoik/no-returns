# CINDER DEPOT playable layout study

[한국어](map-study.ko.md)

2026-09-16 · MAP-STUDY-02 · HTML implemented / proposal before Unity integration

## Scope and direction

Keep the user-selected ship interior trial 05. Do not produce the exterior. The [standalone HTML](../../prototypes/cinder-depot/index.html) studies layout; no Unity scene, model or online game was changed.

The abandoned industrial exploration described on the [official Lethal Company page](https://store.steampowered.com/app/1966720/Lethal_Company/) is a reference. These rooms and connections are hand-authored for NO RETURNS, not copied maps or an implementation of its generation algorithm.

## Structure and choices

18 named spaces including landing connect through narrow corridors and branches. West entrance, unloading, sorting, storage, power, control, inspection and receiving form the primary approach. Records, security, pumps, turbines, suppressor core, workshop, cooling and waste storage form central and eastern loops. The break room is an optional dead end.

Unlock the west entrance emergency connector with E from inside the facility. Mandatory delivery and all other passages remain connected while it is closed. Once opened it serves the return trip. Route overlay shows the shortest connection from your position to the objective and can be disabled for free exploration. All passages allow cargo. Employee-only passages, additional lock puzzles, vertical traversal, fog of war and procedural generation are not implemented.

## Launch and rules

Open root Play_Map_Overview.cmd or the HTML in a browser. No external downloads or server required. Korean is default; switch with English.

- WASD/arrows or floor click: move. Space: start/pause.
- E: pick up cargo → deliver at receiving → collect receipt → finish at ship. Also inspect records and unlock the emergency exit from inside.
- Q: set cargo down. F: an 8-second lure in danger mode. Beacons are unlimited in this experiment.
- Danger defaults off. When enabled, suppression advances through 4 stages at 45-second intervals with patrol/hunt behavior. Colors and descriptions replace a numeric timer. A 0.9-second attack warning precedes recovery to ship, leaving cargo on site. Ship interior is safe.
- Proposed extent 100×80m is represented at 10 pixels/m. Accelerated review movement is 88 pixels/s empty-handed and 62 carrying. This does not validate Unity scale, physics or final difficulty.

## Evidence and next gates

[Automated check](../../prototypes/cinder-depot/check.cjs): connectivity of every movement cell except the closed gate, full carry/delivery/receipt/return loop, outside gate denial/inside unlock, drop/recovery passed. Node syntax check passed. Browser rendering and read-only connectivity diagnostics confirmed 879 floor cells with receiving, clue and patrol points connected.

Automation executes movement logic in a mocked DOM; it does not validate human feel or networking. A full manual browser return run, danger balance, human navigation/fun, first-person visibility, elevation and four-player passing remain unverified. Test whether players can infer destinations at junctions without the overlay and whether revisiting remains interesting before Unity blockout.

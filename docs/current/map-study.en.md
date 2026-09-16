# CINDER DEPOT playable layout study

[한국어](map-study.ko.md)

2026-09-16 · MAP-STUDY-05 · HTML implemented / proposal before Unity integration

## Scope and direction

Keep the user-selected ship interior trial 05. Do not produce the exterior. The [standalone HTML](../../prototypes/cinder-depot/index.html) studies layout; no Unity scene, model or online game was changed.

The abandoned industrial exploration described on the [official Lethal Company page](https://store.steampowered.com/app/1966720/Lethal_Company/) is a reference. These rooms and connections are hand-authored for NO RETURNS, not copied maps or an implementation of its generation algorithm.

## Structure and choices

18 named spaces including landing connect through narrow corridors and branches. West entrance, unloading, sorting, storage, power, control, inspection and receiving form the primary approach. Records, security, pumps, turbines, suppressor core, workshop, cooling and waste storage form central and eastern loops. The break room is an optional dead end.

Added the west loading yard, southwest landing area, east service yard and southern outdoor return route. Blue-grey identifies the interior and ochre identifies exterior space. Enter through the west main entrance, then leave through the emergency exit directly to the right of receiving. E unlocks the east door only from inside. While closed, the west entrance still connects every area and the delivery destination. Containers and equipment bend the exterior route and its straight sightlines. Existing four-stage suppression/hunting behavior remains; dedicated exterior creatures, visibility and cover simulation are not implemented.

The route overlay shows the shortest connection to the objective and can be disabled. Interior rooms were rearranged to fit the complete map; this is not a scale enlargement preserving previous room dimensions. All passages allow cargo. Employee-only passages, additional lock puzzles, vertical traversal, fog of war and procedural generation are not implemented.

## Launch and rules

Open root Play_Map_Overview.cmd or the HTML in a browser. No external downloads or server required. Korean is default; switch with English.

- WASD/arrows or floor click: move. Space: start/pause.
- E: pick up cargo → deliver at receiving → collect receipt → finish at ship. Also inspect records and unlock the emergency exit from inside.
- Q: set cargo down. F: an 8-second lure in danger mode. Beacons are unlimited in this experiment.
- Danger defaults off. When enabled, suppression advances through 4 stages at 45-second intervals with patrol/hunt behavior. Colors and descriptions replace a numeric timer. A 0.9-second attack warning precedes recovery to ship, leaving cargo on site. Ship interior is safe.
- Proposed extent 120×112m is represented at 10 pixels/m. Accelerated review movement is 88 pixels/s empty-handed and 62 carrying. This does not validate Unity scale, physics or final difficulty.

## Evidence and next gates

[Automated check](../../prototypes/cinder-depot/check.cjs): connectivity of every movement cell except the closed gate, full carry/delivery/receipt/return loop, outside gate denial/inside unlock, drop/recovery and travel through the unlocked east exit, service yard, southern exterior and back to ship passed. Node syntax check passed. Browser rendering and read-only connectivity diagnostics confirmed receiving, clue and patrol points connected.

Automation executes movement logic in a mocked DOM; it does not validate human feel or networking. A full manual browser return run, danger balance, human navigation/fun, first-person visibility, elevation and four-player passing remain unverified. Test whether players can infer destinations at junctions without the overlay and whether revisiting remains interesting before Unity blockout.


## MAP-STUDY-04

Exterior expansion 04: enlarged the proposed overall extent to 120×112m. Preserved interior coordinates and widened the west antenna area, east service yard, south freight yard and fuel equipment area. Exterior obstacles allow movement around multiple sides. Antenna/fuel areas are currently labels and collision obstacles, with no new interactions. Automated travel to 5 additional exterior destinations and existing delivery/exit/return checks passed. Browser rendering confirmed with zero console errors. First-person feel, danger balance and Unity integration remain unverified.


## MAP-STUDY-05

Added the northern exterior maintenance area to complete a four-sided perimeter loop. Proposed extent: 120×112m. Preserve separation between interior and exterior, connected only through the existing west entrance and east emergency door. Full exterior circuit and existing delivery/door automated checks passed; browser rendering and zero console errors confirmed. Unity integration and human feel remain unverified.


## 2026-09-16 / EXIT-RELOCATION

Moved the emergency exit to the right wall of receiving at the user-marked location. Closed the former cooling exit. Inside-only E unlock remains. Automated delivery/receipt/return, new gate unlock, old exit blockage and perimeter circuit checks passed. No Unity changes; human feel unverified.

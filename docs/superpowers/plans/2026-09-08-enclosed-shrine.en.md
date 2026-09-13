# Enclosed rooms implementation plan · 0.7.6

[한국어](2026-09-08-enclosed-shrine.ko.md)

> **Historical record · retired/superseded 2026-09-12.** The [PSX space-delivery design](../../current/01-overview.en.md) is current. Implementation, launch, art and values below are historical, not current status. Deleted files remain path records only.


> Documentation update: versions and values here belong to this file's original context. Check the [documentation home](../../README.en.md) first for current rules and outstanding work.

Goal: divide the 48×60m Shipping Shrine into spaces with doorways, interrupted sightlines and roofs to vary delivery situations. Preserve the winding main routes and both cross-links. Modify the saved Godot 4.7.2 map scene without regenerating it at runtime.

## Spaces and situations

- Intake / packing room: tall walls and side exits enclose the starting area. A low west-wall parcel hatch has a 3×1.15m opening that admits boxes only. A teammate can receive on the other side or walk around through the doorway.
- Lost-property archive: long walls and a partial roof surround the existing shelves. Players turn after entering before the next space becomes visible.
- Unpaid doorman waiting room: walls and a high roof surround the central gate and plates. Cross-links before/behind the gate and both bypasses remain open.
- Express air-mail passage: a doorway after the conveyor leads to transverse wind under a roof. Players can wait for the amber warning or carry parcels through.
- A/B dispatch rooms: offset entrances hide the delivery bays behind walls. Destinations are revealed around the last corner.

New walls are normally 5.2m high; partial roofs are normally at 5.7m, with the doorman roof at 8.2m to clear the raised door. Add fill lighting under roofs. Preserve fixed furniture and shelves. Supply matching English/Korean signs. Add no network state or interaction keys. Keep the existing rat activity area unobstructed.

## Implementation and validation

- [x] `tests/test_enclosed_rooms.gd`: verify sightline occlusion, box passage / worker blockage at the parcel hatch, and ceiling collision. First confirm failure on the old map.
- [x] `tools/enclose_shrine.gd`: explicitly run a one-time migration to save editable `Geometry/EnclosedRooms` in `scenes/maps/shipping_shrine.tscn`.
- [x] Run carrying-route, rat, gate, wind and new tests; capture interiors using the actual player camera.
- [x] Update editing guides and README in both languages; run the full suite, Windows export and package audit.
- [x] Review, record in local Git and create the `v0.7.6` milestone.

Acceptance: preserve existing 3m carrying routes; actual player camera respects walls; a physical box can traverse the hatch while a worker capsule cannot; online regressions pass. Final fun and long-session readability require user playtesting.

## Placement review adjustments

Adjusted walls at three doorway edges to preserve the existing 3m carrying-route checks. Raised the hatch opening to 1.8–2.95m above ground and marked throwing/catching spots to match an actual relay throw. Moved the reception desk, lost-parcel pile, plants and dispatch furniture clear of walls. Oriented the fan face and signs toward players inside the air-mail room.

## Validation record

- New room checks: confirmed sightline, hatch and camera failures on the old map, then passes on the enclosed map. A real physics parcel crosses the hatch, travels approximately 7m and is caught airborne by a teammate for a relay.
- Full suite: 25 behavior checks, eight two-process scenarios and one four-process scenario passed. Re-ran carrying-route checks successfully after the final plant move.
- Actual rendering: captured intake, archive, air-mail and dispatch through the worker third-person camera, plus an overview, to inspect walls, roofs, sightlines and signs.
- Fixed wall overlaps in four furniture/plant clusters identified by read-only code review.
- Outputs: `build/NO_RETURNS_0.7/NO_RETURNS.exe`, `build/NO_RETURNS_0.7_Windows.zip`, including English and Korean play guides.

- Windows export, packaged launch, and package audit of 28 compiled scripts plus saved map identity passed.

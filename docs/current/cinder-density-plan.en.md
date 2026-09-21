# CINDER facility density expansion plan

[한국어](cinder-density-plan.ko.md)

2026-09-21 · CINDER-DENSE-02 · User approved: populate a real facility without shrinking the map.

Keep the existing 108×86.4m ground and ship dimensions. Preserve the separate source blockout while rebuilding buildings/routes in the delivery demo. Divide interiors into functional rooms and corridors; connect entrances, alleys and passages into indoor/outdoor loops. Preserve cargo clearance and delivery/terminal coordinates. Spatial and sightline segmentation, rather than prop count, is the objective.

- [x] Build warehouse, checkpoint, sorting hall, office, reception, service and storage buildings plus perimeter utilities, rooms, corridors and work courts in `CinderDenseBuild`. Check original size, entrance links, cargo clearance, long sightline obstruction and ship return.
- [x] Extend CarryThreat/CarryRoom with three host-simulated A/B/C listeners replicated to peers. Process down/rescue/baton cooldown once; preserve outer creature and legacy trial. Bump Cinder protocol to 12.
- [x] Generate, validate, render and build Windows through the Unity plugin CLI. Test delivery, receipt, return, multiple-enemy consistency and rescue in real processes.
- [x] Update paired specifications, guides, backlog, validation and changelog; commit/push only task changes.

Separate automated routes/state checks from human tension, wayfinding and distraction cooperation. Suppression timing/economy remain unchanged. Final art, operable doors and multi-floor AI are separate scope. Preserve eight pre-existing material edits and the recovery scene.

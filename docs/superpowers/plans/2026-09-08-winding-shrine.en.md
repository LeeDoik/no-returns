# Winding Shipping Shrine layout plan

[한국어](2026-09-08-winding-shrine.ko.md)

Goal: version 0.7.5 replaces three parallel straight lanes within the existing 48×60 m map with winding alleys and cross-links. Follow the user's standing instruction to proceed through layout, validation and build without incremental approval. Improve horizontal routing and changing sightlines before adding an elevation system.

Left: alternate an angled screen extending from the southern outer wall, an inner lost-property wall and a northern outer-wall screen to create an S route. Right: enter inside a southern screen, travel sideways through the airflow and turn inward again at the northern end. Move AirMail to (15,0,-24), Y rotation 90 degrees, blowing toward the central cross-link. Keep the pressure gate. At Z-26 before and Z-30 behind the gate, cross-links connect left and right so players can change routes midway.

Screens are 2.4–2.8 m high with some 45-degree tips. Use editable/duplicable SolidBlocks under Geometry/WindingAlleys. Keep intake, dispatch, rat territory, contract time and mechanism cycles. Save reference routes as Marker3D waypoints visible in the editor. These are design/test references, not forced navigation paths.

- [x] Add a failing check that the old outer straight lanes are interrupted.
- [x] Save winding walls, angled tips, transverse airflow, directional arrows, bilingual signs and reference waypoints.
- [x] Sweep worker capsules and held-cargo clearance along the S route, right route and cross-links. Check moved/rotated mechanisms and existing regressions.
- [x] Inspect actual renders, build Windows, update complete bilingual guidance/history and record local Git v0.7.5.

Exclude user project.godot and art work. Human play must validate navigation interest and detour length. Keep protocol 9; map fingerprints prevent mixing different saved maps/builds.


## Additional request: interior density

The user requested a densely furnished interior. In the same change, add 29 grouped reception/packing desks, lost-property piles, lockers, benches, vending cabinets, archives and plants; 96 shelf parcels; 18 wall posters; desk paperwork; and overhead pipes/pennants. Furniture and floor parcels have collision; shelf stock stays inside existing rack collision. Small papers/signs/pipes add no independent collision. New vending cabinets and stamps are decorative, not new usable mechanics.

Group furniture under Geometry/InteriorDressing, overhead dressing under Decoration/InteriorOverhead and posters under Decoration/WallPosters. Stock is parented to each Geometry/Shelves rack so it moves with it. Repeat 3 m turning-clearance sweeps after furnishing. Remove old straight-lane paint so it does not conflict with winding guidance.


## Completion and validation

Twenty-four behavior checks, eight real two-process scenarios and the four-process scenario passed. Final evidence is in `artifacts/winding-final-tests.log`. Swept 3×1.8×3 m clearance boxes plus actual worker capsule/cargo shapes along the routes. Adjusted initial tight spots beside fan supports and pressure plates; routes passed again after furnishing. Confirmed that screens interrupt the old outer straight lanes while the new connected paths remain open.

Inspected actual renders of the full layout, alley, transverse airflow and packing interior. Windows export, packaged startup, 28 compiled scripts/map fingerprint audit and package hashes passed. Independent code review found no material traversal, mechanism or editing hierarchy issues. Stable 0.7 EXE/ZIP paths now contain 0.7.5. Updated complete bilingual documents; exclude existing user settings and separate art work from the commit.

Evidence: `artifacts/winding-capture.log`, `artifacts/winding-pack.log`, `artifacts/interior-route.log`. Target-device performance with dense props and actual navigation/carrying enjoyment still require human playtesting.

# CINDER DEPOT environment art 01 — image approval

[한국어](environment-kit-01.ko.md)

2026-09-13 · First-area art proposal for the PSX space mystery. References the existing overall concept, ship interior and first-person sheet. Do not produce 3D models or replace executable visuals until the user approves the new sheet.

## Image scope

Propose one sheet with a first-person environment view from the ship ramp toward the first delivery entrance and 6 matching modular assets: wall, corner, door frame, floor, lamp and cargo rack. This is not the exact layout of the current validation map or a gameplay capture. No new creature design or equipment is introduced.

Connect facilities and ship through ivory industrial panels, oxidized orange/rust-red bands, dark metal frames and the triangular courier mark. Warm ship lighting anchors return; doorway lamps and the BAY 04 sign emphasize direction. Reserve teal primarily for teammate identification. Keep floor contrast below entrances and make carrying space readable through the bend.

Use large angular shapes, simple edges, flat shading and low-resolution point-filtered textures. The 128/256-pixel source-texture appearance is a visual prompt target, not a validated texture-file specification. Concentrate rust, touch wear and scratches near panel bottoms and contact areas rather than applying uniform noise everywhere. Use restrained shadow dithering. Exclude reflective surfaces, bloom, depth of field and excessive fog from the treatment.

## Checks after approval and during production

- Do panel spacing and color-band height match across repeated modules?
- Do door frames and racks preserve carrying routes and colliders?
- Are entrances, cargo and teammates distinguishable in dark scenes?
- Does the low-resolution treatment avoid becoming dirty visual noise?

Created with the built-in image generator. The actual prompt is preserved through the link in this English counterpart. The asset breakdown is visual reference, not production assets with validated dimensions, UVs, meshes, materials, repeat connections or performance. No code, scene, model or build changes. Post-generation visual findings follow below.


## Visual review and saved result

[Environment sheet PNG](environment-kit-01.png) · [Actual generation prompt](environment-kit-01.prompt.txt)

Confirmed the first-person ship-threshold composition, bent entrance, teal teammate, orange bands and 6 isolated modules. Lamps, signs and door frames stand out from dark surfaces. However, broad floor and wall areas still have relatively dense mottling. Production should further simplify small floor marks to prioritize cargo and teammate silhouettes. Exact dimensions and band continuity between modules and the perspective view were not validated through this image. The new sheet awaits approval and is not applied to the 0.8.2 executable.

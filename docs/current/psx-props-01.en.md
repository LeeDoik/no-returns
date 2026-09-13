# Delivery prop production

[한국어](psx-props-01.ko.md)

0.8.5 removes the raised bench and connects display, indicators, sound and receipt reactions. The 0.8.4 status below is production history. [Current receipt terminal](receipt-terminal.en.md).

2026-09-13 · SPACE-ART-27 · In production.

The user approved the sealed parcel and receipt device from cargo-receipt-clues-01 for faithful production. Mystery props, employee, equipment and Listener are outside this approval. The existing six facilities will not be regenerated.

Isolated parcel and standalone receipt-terminal inputs were reconstructed from the approved sheet; the source sheet is preserved. Terminal copy will be displayed separately in Unity and the receiving platform will reuse existing facilities. Generate one of each with private Tripo Smart Mesh P2.0. Regenerate only for confirmed model defects; fix units, pivots, materials and placement locally.

Completion gates: archive/mesh/UV checks, actual-model render comparison, a root preserving unit conversion, Unity import and carry/receipt regression checks. Generation alone does not establish game integration or final user visual approval.

## Production and integration — 0.8.4

Parcel: 3722 triangles; terminal: 9777 triangles. One generation each, zero regenerations. Each used 65 for geometry and 20 for texture, spending 170 from 1315; remaining 1145 observed in the UI. Both passed ZIP/UV/mesh checks and Blender FBX reimport. After the front render, in-game/rear review revealed an unwanted circular mechanism on the parcel rear. Only that rear was cut and replaced with a rotated copy of the approved front portion, preserving textures and corner guards; no Tripo regeneration was used. Terminal pedestal details differ from the input; runtime copy is not baked into the texture.

Imported into Unity with 512 textures, bottom-center pivots and parent roots preserving FBX transforms. Parcel visuals fit the existing 0.8×0.65×0.65m collision shape; mass, ownership and carrying rules are unchanged. The terminal sits beside the original receiving bench at (-8.1,0.8,9), sized 0.75×1.6×0.65m with a simple collider. Existing six facilities, receipt conditions and rewards remain unchanged. Dynamic screen/indicator state and receipt-ejection animation are not implemented; the terminal is currently visual. The existing HUD communicates delivery state.

[Model render](../../art/psx-props-01/selected/selected-review.png) · [Mesh metrics](../../art/psx-props-01/selected/selection.json) · [FBX checks](../../art/psx-props-01/selected/roundtrip-validation.json) · [Generation record](../../art/psx-props-01/generation-manifest.json)

The plain rear-panel attempt was superseded; the final model reuses front textures and corner guards. Both front and rear renders were inspected. Source files and failed-review run history are preserved.


Final 0.8.4 Windows build succeeded. Two real Windows processes passed 14 automated checks including delivery, receipt, single reward payment, return, wallet retention and disconnect settlement. Blender front/rear and Unity prop rendering were reviewed. After the final textured-rear repair, build and delivery tests were rerun; older runtime captures show the earlier rear, so rear-review.png is the final rear appearance evidence. Human feel/fun and performance measurement were not performed. Documentation checks passed for 206 entries.

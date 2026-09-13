# Decoy beacon art production

[한국어](psx-beacon-01.ko.md)

2026-09-13 · In production

Isolate approved equipment-sheet beacon 03 and generate privately with Tripo Smart Mesh P2.0. One mesh and texture cost 65+20=85 credits, with confirmed balance 1145→1060. Preserve input and generated source; inspect size, UV, 512 texture and FBX roundtrip in Blender before replacing the Unity cylinder appearance. Preserve E/Q carrying, purchase, signal duration and stock. Generation does not count as game integration or user quality approval.

## Game integration — 0.8.9

Applied the Tripo model of approved beacon 03 as the game appearance. Inspected Blender front/rear renders and FBX roundtrip. The model has 9052 triangles, 1 UV layer and a 512 texture, displayed at 0.42×0.44×0.34m. Existing collision and E/Q purchase, carry and placement rules remain. The rear circular assembly and connector are generated geometry with no separate function. Human feel and final appearance approval remain unverified.

[Source and validation](../../art/psx-beacon-01/selected/validation.json) · [Front](../../art/psx-beacon-01/selected/front.png) · [Rear](../../art/psx-beacon-01/selected/rear.png)

Validation: Windows 0.8.9 build succeeded. Two real processes passed 17 beacon checks (seeded test wallet) and 30 delivery/receipt regression checks. Reviewed aboard, carried and deployed captures. An initial capture immediately after camera rotation preceded network propagation; added a 0.5-second settling wait to the test and recaptured. Documentation checks passed for 212 documents. Human feel, fun and overall performance were not tested. Existing signal audio and orientation handling remain.

[Beacon results](../../artifacts/physical-beacon/latest.json) · [Carried](../../artifacts/physical-beacon/run-20260913-190229/carried.png) · [Delivery run](../../artifacts/space-play-02/run-20260913-190139/)

User check: on 2026-09-13 the user confirmed the integration and requested continuation. This does not establish comprehensive feel or fun validation.

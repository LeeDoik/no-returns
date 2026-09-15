# FLATBED interior structure trial 04

[한국어](ship-interior-trial.ko.md)

2026-09-15 · Refit the structure after the user reported interior protrusion and a low entrance. Applied to the isolated trial scene/Windows build; main game and earlier art sources are preserved. [Previous state](../archive/ship-interior-trial-03.en.md).

## Cause and correction

Trial 03 raised the cabin ceiling but left the rear door approximately 1.86m high. Stretching an irregular generated hull did not guarantee containment of the rectangular cabin. Earlier central passage checks did not establish exterior fit or comfortable entry.

Following the new pipeline, replaced this trial's visual exterior with a simple structure fitted to the cabin. Preserved the original Tripo hull; this does not complete final engines, decoration or ship appearance. Kept furniture, display and viewpoint while building outer walls, roof, nose connections and rear frame from shared dimensions.

- Cabin floor 1.00m, ceiling underside 4.12m, clear height 3.12m.
- Rear opening clear width 3.2m and height 3.12m. Door panels match that height and are moved aside in their open positions.
- Exterior width 5.1m. Filled floor/ceiling connections between doorway and cabin. Exterior ramp has a horizontal run of 3m and rise of 1m.
- Preserved employee height 1.8m, radius 0.34m, jump speed 5m/s and gravity 18m/s².

## Deliverables and reproduction

[Blender](../../art/ship-flatbed-01/interior-blockout-04/Flatbed_InteriorTrial.blend) · [FBX](../../art/ship-flatbed-01/interior-blockout-04/Flatbed_InteriorTrial.fbx) · [GLB](../../art/ship-flatbed-01/interior-blockout-04/Flatbed_InteriorTrial.glb)

[Interior forward](../../art/ship-flatbed-01/interior-blockout-04/forward.png) · [Interior rear](../../art/ship-flatbed-01/interior-blockout-04/rear.png) · [Exterior rear](../../art/ship-flatbed-01/interior-blockout-04/exterior-rear.png) · [Exterior front](../../art/ship-flatbed-01/interior-blockout-04/exterior-front.png)

[Structure generation/dimensions](../../art/ship-flatbed-01/interior-blockout.py) · [Geometry evidence](../../art/ship-flatbed-01/interior-blockout-04/validation.json) · [Round-trip](../../art/ship-flatbed-01/interior-blockout-04/roundtrip.json) · [Unity checks/build](../../NoReturns/Assets/_NoReturns/Editor/ShipInteriorTrialBuild.cs) · [Controls](../../NoReturns/Assets/_NoReturns/Runtime/ShipInteriorTrial.cs).

Executable: `builds/ShipInteriorTrial/NoReturns-InteriorTrial.exe`. WASD/mouse, Space empty-hand jump, E pick up, Q drop, F1 language switch, Esc release cursor. Retain adjacent data files. Build through the existing Editor using `powershell -NoProfile -File tools/unity.ps1 trial`. Close any previous trial instance and relaunch.

## Verification scope

- [x] Compared fixed interior vertices against the fitted exterior bounds and sloping nose roof: 0 protrusions. Excludes intentionally external doors, ramp and shell. This is not a general arbitrary mesh-intersection checker.
- [x] Reviewed same-model interior/exterior renders; GLB round-trip passed: 2,255 triangles, missing UVs 0, nonfinite coordinates 0.
- [x] Actual CharacterController passed 5 entry and 3 return routes.
- [x] No head collision at 12 jump positions including beneath the doorway. Actual rise approximately 0.645m.
- [x] Unity compilation/import and Windows build succeeded; fresh build-success.txt verified.
- [ ] Human spatial comfort, entry clearance and cargo-handling review.
- [ ] Final art, moving doors, exterior finishing, main game and 4-player online integration.

Local evidence: `artifacts/ship-interior-trial/passage.txt`, `build-success.txt`, `envelope-blender.log`, `envelope-roundtrip.log`. Automated checks do not establish comfort. Proceed to art through the [structure-first pipeline](art-structure-first.en.md) after human structural approval. No Tripo generation or credits used.

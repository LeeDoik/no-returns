# FLATBED interior structure trial 05

[한국어](ship-interior-trial.ko.md)

2026-09-15 · Interior-first orbital post-office structural candidate. Applied to the isolated trial scene/Windows build; main game and earlier art sources are preserved. [Previous state](../archive/ship-interior-trial-03.en.md).

## Orbital post-office interior

The user requested a space distinct from a truck. Removed repeating rows of seats/racks and arranged separate inspection, sealed storage and dispatch zones.

- Center: floor inspection markings and an overhead scanner instead of a raised platform. Clear passage width 3.04m and overhead clearance 2.75m. Cargo can travel from the entrance toward the front.
- Left: sealed return lockers with latches/status strips. Opaque storage offers a place to explore mystery, but sounds, creatures and events are not implemented.
- Right: receipt-printer form, workbench, equipment cubbies and wall display. Heights/depths differ from the tall lockers on the left.
- Rear: 4 wall-folded employee seats and clear entry space. Folding/sitting behavior is not implemented.
- Front: retained centered observation windows and shared route screen. An overhead service trunk connects the work zones.

All new work devices are structural mockups. Do not mark inspection, settlement, storage, purchasing or dynamic UI as implemented. Current play functions are movement, empty-hand jumping and cargo pickup/drop.

Preserved floor 1.00m, ceiling underside 4.12m, cabin height 3.12m, entrance width 3.2m/height 3.12m. Employee height 1.8m/radius 0.34m and jump speed 5m/s/gravity 18m/s² are unchanged. Jump checks now include beneath the scanner.

The generation script starts with an empty Blender scene and builds the interior without loading the previous hull. The exterior is a temporary entry/occlusion cover. Sequence: interior play approval → surrounding exterior structure → exterior concept approval → art production. Earlier models/main game are preserved; no Tripo generation or spending.

## Deliverables and reproduction

[Blender](../../art/ship-flatbed-01/interior-blockout-05/Flatbed_InteriorTrial.blend) · [FBX](../../art/ship-flatbed-01/interior-blockout-05/Flatbed_InteriorTrial.fbx) · [GLB](../../art/ship-flatbed-01/interior-blockout-05/Flatbed_InteriorTrial.glb)

[Interior forward](../../art/ship-flatbed-01/interior-blockout-05/forward.png) · [Interior rear](../../art/ship-flatbed-01/interior-blockout-05/rear.png) · [Exterior rear](../../art/ship-flatbed-01/interior-blockout-05/exterior-rear.png) · [Exterior front](../../art/ship-flatbed-01/interior-blockout-05/exterior-front.png)

[Structure generation/dimensions](../../art/ship-flatbed-01/interior-blockout.py) · [Geometry evidence](../../art/ship-flatbed-01/interior-blockout-05/validation.json) · [Round-trip](../../art/ship-flatbed-01/interior-blockout-05/roundtrip.json) · [Unity checks/build](../../NoReturns/Assets/_NoReturns/Editor/ShipInteriorTrialBuild.cs) · [Controls](../../NoReturns/Assets/_NoReturns/Runtime/ShipInteriorTrial.cs).

Executable: `builds/ShipInteriorTrial/NoReturns-InteriorTrial.exe`. WASD/mouse, Space empty-hand jump, E pick up, Q drop, F1 language switch, Esc release cursor. Retain adjacent data files. Build through the existing Editor using `powershell -NoProfile -File tools/unity.ps1 trial`. Close any previous trial instance and relaunch.

## Verification scope

- [x] Compared fixed interior vertices against the fitted exterior bounds and sloping nose roof: 0 protrusions. Excludes intentionally external doors, ramp and shell. This is not a general arbitrary mesh-intersection checker.
- [x] Reviewed same-model interior/exterior renders; GLB round-trip passed: 4,246 triangles, missing UVs 0, nonfinite coordinates 0.
- [x] Actual CharacterController passed 5 entry and 3 return routes.
- [x] No head collision at 15 jump positions including beneath the doorway. Actual rise approximately 0.645m.
- [x] Unity compilation/import and Windows build succeeded; fresh build-success.txt verified.
- [ ] Human spatial comfort, entry clearance and cargo-handling review.
- [ ] Final art, moving doors, exterior finishing, main game and 4-player online integration.

Local evidence: `artifacts/ship-interior-trial/passage.txt`, `build-success.txt`, `envelope-blender.log`, `envelope-roundtrip.log`. Automated checks do not establish comfort. Proceed to art through the [structure-first pipeline](art-structure-first.en.md) after human structural approval. No Tripo generation or credits used.

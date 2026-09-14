# FLATBED interior structure trial 03

[한국어](ship-interior-trial.ko.md)

2026-09-14 · Central window/display corrected; Unity import, automated passage and Windows build completed. This is an isolated spatial trial, not final art. Existing Angular/Integrated models, trial 01 sources and main game scene/build are preserved.

## Placement and deliverables

Locally rebuilt the new candidate's windshield surround to correct the off-center window. Aligned central glazing and display to X=0 with symmetric side panes and mullions. Console base top is 1.74m and display bottom 1.885m, leaving 0.145m clearance and removing screen obstruction. Center and side eye rays found no opaque hull obstacle. The new candidate's hull fingerprint changes; earlier sources remain intact.

- [Blender](../../art/ship-flatbed-01/interior-blockout-03/Flatbed_InteriorTrial.blend) · [FBX](../../art/ship-flatbed-01/interior-blockout-03/Flatbed_InteriorTrial.fbx) · [GLB](../../art/ship-flatbed-01/interior-blockout-03/Flatbed_InteriorTrial.glb)
- [Forward](../../art/ship-flatbed-01/interior-blockout-03/forward.png) · [Rear](../../art/ship-flatbed-01/interior-blockout-03/rear.png) · [Windshield](../../art/ship-flatbed-01/interior-blockout-03/window.png)
- [Generation code](../../art/ship-flatbed-01/interior-blockout.py) · [Geometry checks](../../art/ship-flatbed-01/interior-blockout-03/validation.json) · [Round-trip checks](../../art/ship-flatbed-01/interior-blockout-03/roundtrip.json)
- [Unity scene](../../NoReturns/Assets/_NoReturns/Scenes/ShipInteriorTrial.unity) · [Generation/check/build code](../../NoReturns/Assets/_NoReturns/Editor/ShipInteriorTrialBuild.cs) · [Trial controls](../../NoReturns/Assets/_NoReturns/Runtime/ShipInteriorTrial.cs)

Local executable: `builds/ShipInteriorTrial/NoReturns-InteriorTrial.exe`. Keep it with the other data in its folder. Builds/logs are excluded from Git. Controls: WASD/mouse, E pick up, Q drop, Space empty-hand jump, F1 language switch, Esc release cursor. This does not replace main game carrying code or build settings.

## Dimensions and passage verification

Floor is 1.00m, ceiling underside 4.12m, cabin height 3.12m and eye elevation 2.57m. Used employee height 1.8m, radius 0.34m, eye height 1.57m and cargo size 0.8×0.65×0.65m from [current carrying code](../../NoReturns/Assets/_NoReturns/Runtime/CarryLab/CarryRoom.cs). Rear frame clearance is approximately 1.86m; exterior ramp horizontal run is 3m and rise 1m. These are trial values, not release decisions.

Corrected FBX forward-axis differences with a 180-degree Unity Y rotation. Raw generated hull triangles used as walking collision snagged at the entrance. Separated visual hull and collision, using boxes for cabin/frame and a dedicated inclined box for the ramp. This does not replace main game physics.

Actual CharacterController.Move tests at X=-0.55, 0 and 0.55m all reached Z=2.259m. Employee samples and a conservative enclosing cargo sphere also found no collisions. GLB round-trip matches 13,925 triangles, missing UVs 0 and nonfinite coordinates 0. These checks do not establish human carrying feel while rotating cargo or guarantee passage at every position.

## Unity recovery and execution

The sandbox attempt failed with exit code 198 and `No valid Unity Editor license found`. After read-only inspection in the normal user environment confirmed Unity Personal Assigned, opened the Editor once and attached to the existing instance. No global sandbox disabling, license deletion or forced license-process termination was used. Hub IPC warnings remain; this does not mean all warnings disappeared.

Registered MCP was official `unity mcp --project-path ...`, already an attach configuration. The [actual MCP probe](../../tools/unity_mcp_probe.py) passed initialize, tools/list and tools/call editor_status, confirming ready, compiling=false and domainReloadInProgress=false. The [task helper](../../tools/unity.ps1) now attaches only for setup/check/trial, failing when no Editor is connected or compilation/reload/Play is active. It does not silently spawn batch Editors. Registered menus avoid observed string-conversion errors with dynamic eval.

~~~powershell
powershell -NoProfile -File tools/unity.ps1 open
powershell -NoProfile -File tools/unity.ps1 trial
python tools/unity_mcp_probe.py
~~~

Run open once in the normal user environment when needed. The first build succeeded in approximately 111 seconds but exceeded the CLI's default 30-second response timeout. The helper now waits up to 600 seconds and checks a fresh build-success.txt. A menu response alone does not establish build success.

## Complete and unverified

- [x] Same-model renders confirm centered windshield and unobscured display.
- [x] Unity compilation/import and 3 actual passage lines passed.
- [x] Windows BuildPipeline success, fresh executable and success record verified.
- [x] Editor Play entry and game-view rendering verified.
- [ ] Human review of E pick-up, Q drop and carrying through the low entrance.
- [ ] Final exterior texture retention, glazing, ceiling seams, excessive lighting and PSX finishing.
- [ ] Door animation, dynamic display, main game and 4-player online integration.

Seats, walls and racks are plain structural parts and display text is illustrative. Local evidence: `artifacts/ship-interior-trial/passage.txt`, `build-success.txt`, `unity-play.png`, `unity-interior.png` and `NoReturns/Logs/Editor.log`. Geometry JSON unity_playtest=false describes the Blender check's own scope, distinct from the separate Unity results. Automated checks/render review are not human handling or fun validation.

## 2026-09-14 — Jump clearance and production sequence

The user's actual play feedback was that the ceiling felt low, with a request for enough height to jump without touching it. Increased cabin height from 2.12m to 3.12m, a 1.00m lift. Kept floor, display and player viewpoint fixed while raising upper walls, ceiling, roof and upper front together. The low rear door remains; the guarantee does not cover jumping directly beneath it or on furniture. Exterior proportions and upper-front finishing after the roof lift still need user visual review.

Added Space empty-hand jumping to the trial. Uses production CarryRoom initial speed 5m/s and gravity 18m/s²; no jumping while carrying cargo. Theoretical rise is approximately 0.694m, leaving approximately 0.60m above a 1.8m employee at the apex. Actual CharacterController checks at 9 positions recorded approximately 0.645m rise with no Above collision, and 3 original passage routes passed. Discrete integration and ground-contact clearance explain the difference from theory. New Windows build succeeded. These are automated checks; human satisfaction with the new height remains unverified.

Apply the [new asset pipeline](art-structure-first.en.md): structure → actual Unity play → user confirmation → Tripo visuals based on approved structure → Blender dimensional review → game retest. Art production for this model waits for structural confirmation; no Tripo generation or spending occurred. Earlier renders/play captures document the previous height; use the new structural renders and passage.txt for current height evidence.

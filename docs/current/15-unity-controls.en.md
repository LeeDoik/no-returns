# Unity movement and carrying prototype

[한국어](15-unity-controls.ko.md)

> **Historical record · retired/superseded 2026-09-12.** The [PSX space-delivery design](01-overview.en.md) is current. Implementation, launch, art and values below are historical, not current status. Deleted files remain path records only.


2026-09-11 · UNITY-02 · Approved scope: movement, 3rd-person camera, pickup and throwing. Implementation and validation results will be updated below.

## Design and implementation sequence

1. Preserve the development scene and create a separate CarryLab scene. Compare controls using ground, walls, a ramp and boxes with different masses.
2. Separate CharacterController movement from the collision-aware camera. Use WASD, mouse and Space. Start with walk speed 3.2m/s, carrying speed 1.6m/s and jump launch speed 4.8m/s from Godot `scripts/worker.gd`.
3. Keep boxes as Rigidbody objects. Track the carrying target with limited force and rotation torque; release when too far away. E picks up/drops; hold and release left click to throw. Do not teleport boxes through walls to reach the hands.
4. Automated physics checks cover pickup range, occlusion, orientation tracking, wall collision, release and mass-dependent throwing. Assess actual feel and visual quality separately.
5. Update the Windows build and bilingual specification, guide, backlog and validation history.

Artwork uses temporary primitives. Do not produce an unapproved character model. This is a solo control experiment; networking, delivery rules, final animation and the full map port are outside this step.


## Current implementation · Unity 0.2.0

CarryLab (`../../unity/NoReturns/Assets/_Project/Scenes/CarryLab.unity`; retired file) is a solo carrying lab separate from Development. The initial screen shows English controls, the nearby box name and throw charge. 06_Unity_Play.cmd (`../../06_Unity_Play.cmd`; retired file) launches the built lab. 05_Unity.cmd (`../../05_Unity.cmd`; retired file) opens the Editor. If the scene is empty, open CarryLab from the Project window or use `NO RETURNS > Create Carry Lab`. An existing CarryLab file is opened without being overwritten.

| Control | Action |
|---|---|
| WASD | Move relative to the camera's horizontal heading |
| Mouse | Rotate the 3rd-person view |
| Space | Jump from the ground |
| E | Pick up a nearby box in front / drop |
| Hold and release left click | Charge and throw |
| R | Restore worker and boxes to their initial positions |
| Esc / click | Release cursor / resume controls |

## Rules and tuning locations

Values are for initial control validation, not final balance. Adjust the corresponding components in the Inspector.

| Item | Current value | Source |
|---|---|---|
| Walk / carry speed | 3.2 / 1.6m/s | WorkerController (`../../unity/NoReturns/Assets/_Project/Runtime/WorkerController.cs`; retired file) |
| Jump launch speed | 4.8m/s | WorkerController |
| Pickup center distance | Within 2.1m, in front, unobstructed | CarryMotor (`../../unity/NoReturns/Assets/_Project/Runtime/CarryMotor.cs`; retired file) |
| Carry force limit | 110N | CarryMotor |
| Grip separation release | Above 2.6m | CarryMotor |
| Charge duration / throw impulse | 0.85 seconds / 4–11N·s | WorkerController / CarryMotor |
| Camera distance / cast radius | 4.2m / 0.2m | ThirdPersonCamera (`../../unity/NoReturns/Assets/_Project/Runtime/ThirdPersonCamera.cs`; retired file) |
| Box size / mass | 0.8m / 1·2·4kg | CarryLabBuilder (`../../unity/NoReturns/Assets/_Project/Editor/CarryLabBuilder.cs`; retired file) |
| Lab floor | 24×24m | CarryLabBuilder |
| Fall recovery height | Below -8m | CargoBody (`../../unity/NoReturns/Assets/_Project/Runtime/CargoBody.cs`; retired file), WorkerController |

Carrying does not freeze the Rigidbody or move its Transform. Limited forces and rotation torque track grip position and velocity while preserving gravity, worker contact and world collisions. Equal throw impulses give heavier boxes smaller velocity changes. E and throwing release ownership. Pickup checks range and occlusion and permits one held box. Camera obstruction shortens its distance.

CarryHud (`../../unity/NoReturns/Assets/_Project/Runtime/CarryHud.cs`; retired file) uses uGUI/TMP. Keyboard and mouse are supported; gamepad support and control rebinding UI are not implemented. The temporary capsule has no final arms, hands or animation, so hand contact and animation quality must not be marked validated.

## Validation results and remaining review

Run CarryLabTests (`../../unity/NoReturns/Assets/_Project/Editor/CarryLabTests.cs`; retired file) with `05_Unity.cmd test`. All 24 checks passed using actual physics in a separate batch Editor. Coverage includes range, occlusion, ownership, falling, rotation, wall collisions, mass-dependent impulse, camera obstruction, walking, jumping and carrying movement. Reduced carrying travel was reproduced and fixed by compensating for grip velocity. Evidence: `artifacts/unity/test.log`.

MCP performed scene creation, saving, read-back, play start/stop and screenshots. Evidence: `artifacts/unity/carry-hierarchy.json`, `carry-lab.png`. The captured sample validates rendering and HUD visibility, not a complete human control test. Rapid turns, corners, extended ramp carrying, hand/foot animation, gamepad, separate PCs and online consistency remain unverified. Character motion uses CharacterController; this is not claimed to be a fully physical character or Human Fall Flat implementation.

The default TMP import API timed out. With the Editor closed, 37 original resources from the installed package were restored with their GUIDs preserved. Include these originals and their license in Git. Subsequent scene creation does not launch an automatic font import dialog.

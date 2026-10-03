# Employee baton readiness, swing and recovery — 0.9.9

[한국어](employee-baton.ko.md)

2026-10-03. The user authorizes baton attack motion as the next character task. Current game **0.9.9**, protocol **13**, TCP **27842**. Sources: [version settings](../../NoReturns/ProjectSettings/ProjectSettings.asset) and [current employee visuals](employee-animation.en.md).

## Current behavior

- With empty hands, bend the right arm and hold the baton upright in a ready stance. Apply right-arm/chest rotations over Idle/Walk while retaining lower-body locomotion. Enter readiness over approximately **0.1 seconds**.
- Existing empty-hand left-click contact remains **immediate**. Move directly from readiness to impact, then **0–0.12s** follow-through → **0.12–0.25s** arm recovery → **0.25–0.55s** return to readiness. Add no separate post-click wind-up delaying contact. Retain the **6-second** cooldown.
- Use the actual hand reference point and orientation for the baton. Remove the separate weapon attack rotation that could separate it from the hand. Local first-person weapon motion uses the same timeline and retains wall pull-in. First-person full arm assets remain absent.
- Hide the weapon and attack pose during parcel/beacon carrying, down state, valid rescue progress, and for inactive/absent employees. An interrupted swing does not replay immediately after dropping an item or finishing rescue; return to readiness.
- Inspection finds that simultaneous valid rescue and attack requests can allow an attack in the existing adjudication code. Prioritize rescue over attacking when a visible rescue target is within 2m. Retain empty-hand range/facing/sight checks, Listener stun duration and movement/cargo collision.

## Implementation and production

[Attack pose/timeline](../../NoReturns/Assets/_NoReturns/Runtime/CarryLab/EmployeeVisual.Baton.cs) · [shared arm rotation solver](../../NoReturns/Assets/_NoReturns/Runtime/CarryLab/EmployeeVisual.Carry.cs) · [weapon presentation](../../NoReturns/Assets/_NoReturns/Runtime/CarryLab/BatonVisual.cs) · [rescue priority](../../NoReturns/Assets/_NoReturns/Runtime/CarryLab/CarryThreat.cs).

Queried current employee chest/right shoulder/arm/hand locations and save state through Blender MCP. The Blender file was not modified or saved. Runtime motion evaluates existing Humanoid Idle/Walk, then rotates arm, wrist and chest in Unity. Import no additional Mixamo clips or FBXs; preserve sources, bone lengths/scales, employee root, Root Motion and colliders. Clamp unreachable targets rather than stretching arms; the weapon remains attached to the actual hand. Replacing this with a dedicated animation clip is a separate task.

## Validation and reproduction

[Validation record](../validation/employee-baton-0.9.9.json). Confirmed latest official Unity **6000.6.4f1**, CLI **1.0.0-beta.12** and Pipeline **0.8.0-exp.1**. Confirmed Blender **5.2.2 LTS**, MCP package **2.1.3**, add-on **1.8/protocol 13** compatibility and disabled telemetry.

Run `NoReturns.Editor.EmployeeBatonReview.Review()` in the Unity Editor. The [sample validation code](../../NoReturns/Assets/_NoReturns/Editor/EmployeeBatonReview.cs) writes records and static renders to `artifacts/employee-baton/`. Test two Idle/Walk states at normalized time 0.2, employee directions 0°/90°/135°, and **35** samples covering readiness and 0–0.55s at 60fps: **210** poses total. Maximum hand-target error **0.067738m**, maximum bone-length difference **0.000000358m**, static hand travel approximately **0.437047m**, chest rotation approximately **14.993174°**. Hand-target error is reach clamping, not separation between weapon and hand. Verified finite meshes, unchanged root/bone lengths, blocking/interruption cancellation and recovery. Reran and passed **120** [existing carry samples](../../NoReturns/Assets/_NoReturns/Editor/EmployeeCarryReview.cs).

Passed the Mac build with **zero errors/7 warnings**, **12** actual windowless baton checks, **10** carry checks and **30** Listener/contact/down/rescue checks: **52** total. Upper-arm rotation measures approximately **55.00°/53.26°** across peers for idle attacks and **59.25°/57.59°** for walking attacks, distinguishing arm motion from character translation. The immediately chained Listener run was refused before launch while its port briefly remained unavailable; retry after port release passed 30 checks. Recorded the 7 existing build warnings in the receipt. Stopped all test processes and removed session records.

Static renders: [ready](../../art/player-employee-01/unity-review/Baton-Ready.png) · [impact](../../art/player-employee-01/unity-review/Baton-0.png) · [follow-through](../../art/player-employee-01/unity-review/Baton-7.png) · [recovery](../../art/player-employee-01/unity-review/Baton-15.png) · [returned](../../art/player-employee-01/unity-review/Baton-33.png). These are neither actual game captures nor user quality approval.

Run windowless checks sequentially as follows. Follow the [companion launch policy](companion-play.en.md): manual game windows open only on “직접 테스트 해볼게”.

```sh
python3 tools/cinder_four_player.py build
python3 tools/test_employee_baton.py
python3 tools/test_employee_carry.py
python3 tools/test_cinder_threat.py --headless
```

The `--headless` Listener check uses actual input/network/adjudication and omits screen captures only. No manual game windows were opened or Editor Play entered. Human hit feel/visual quality, all locomotion phases/clothing penetration/first-person occlusion, full delivery/suppression regression, Windows/LAN and performance remain unverified. Dedicated jump/landing, backward/sideways, rescue/down motions and first-person arms remain incomplete.

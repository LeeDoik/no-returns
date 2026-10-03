# Employee baton straight thrust and retraction — 0.9.10

[한국어](employee-baton.ko.md)

2026-10-03. After hands-on play, the user requested a precise sharp thrust instead of the flailing attack. Current game **0.9.10**, protocol **13**, TCP **27842**, sourced from [version settings](../../NoReturns/ProjectSettings/ProjectSettings.asset). This supersedes the sideways swing, chest twisting and 0.55s return in 0.9.9. Human hit-feel approval of this revision remains pending.

## Current behavior

- Point the tip forward from the empty-hand ready stance. Keep wrist orientation and chest twist fixed while retaining lower-body Idle/Walk locomotion. Enter readiness over approximately **0.1s**.
- Advance the hand straight by **0.30m** during **0–0.06s** after left click. Use a fast initial push, hold during **0.06–0.10s**, then retract along the same path during **0.10–0.30s**. Total presentation lasts **0.30s**. Existing contact remains **immediate** on click and cooldown remains **6s**. Visual full extension and contact occur at different times; do not delay damage adjudication.
- Attach the weapon to the actual hand reference/orientation without independent sideways weapon rotation or lateral chest twist. Clamp at arm reach without stretching bones. This can introduce small deviations in actual hand travel while retaining the forward weapon axis.
- Keep the first-person tip aligned with the aim forward direction. Visual extension is **0.27m**; near walls, check current tip clearance over **0.92–1.19m** (including **0.04m** clearance) and retract depth only. Keep lateral/vertical grip coordinates fixed. Retain the **0.35** minimum scale, so this is not a claim of eliminating all penetration at extreme wall proximity/corners. Full first-person arms remain absent.
- Block weapon/attack posing during parcel/beacon carrying, down state, valid rescue, and for inactive/absent employees. Interrupted attacks do not replay; return to readiness. Simultaneous valid rescue/attack input prioritizes rescue. Attack range/facing/sight, Listener stun and movement/cargo collision remain unchanged.

## Implementation and production

[Thrust timeline](../../NoReturns/Assets/_NoReturns/Runtime/CarryLab/EmployeeVisual.Baton.cs) · [hand attachment/first-person presentation](../../NoReturns/Assets/_NoReturns/Runtime/CarryLab/BatonVisual.cs) · [shared arm solver](../../NoReturns/Assets/_NoReturns/Runtime/CarryLab/EmployeeVisual.Carry.cs). Apply arm/wrist rotations over existing Humanoid poses while preserving bone lengths, employee root and colliders. No new FBX/clip, Blender source changes or MCP edits.

## Validation and reproduction

[Current receipt](../validation/employee-baton-thrust-0.9.10.json) · [previous 0.9.9 receipt](../validation/employee-baton-0.9.9.json). Reconfirmed latest official Unity **6000.6.4f1**, CLI **1.0.0-beta.12** and Pipeline **0.8.0-exp.1**; no updates required this task.

The [pose review](../../NoReturns/Assets/_NoReturns/Editor/EmployeeBatonReview.cs) evaluates Idle/Walk at normalized time 0.2 × employee directions 0°/90°/135° × **20** samples including readiness and 0–0.30s at 60fps: **120** total. It writes results/static renders into `artifacts/employee-baton-thrust/`. Verified maximum actual hand extension **0.300000m**, lateral/vertical path deviation **0.019451m**, forward weapon-axis error **0°**, chest twist **0°**, hand-target error **0.038461m** and bone-length change **0.000000298m**. Hand-target error measures reach clamping, not separation between weapon and hand. Finite meshes, unchanged root/bone lengths, exclusion/interruption cancellation and return passed.

Passed the Mac build with **0 errors/7 warnings**, **12** actual windowless two-client thrust/carry-exclusion checks and **30** four-client Listener/contact/down/rescue checks: **42** total. Actual body-relative weapon extension measures approximately **0.243484–0.300001m**, maximum lateral/vertical deviation **0.000004m** and forward-axis error **0.019783°**. The first native run failed with **0.065237m** deviation because near-wall correction also moved lateral/vertical first-person grip coordinates while walking. Corrected depth only, rebuilt and passed the same checks. Stopped test processes and removed session records. Preserve the 7 existing build warnings in the receipt.

Static renders: [ready](../../art/player-employee-01/unity-review/Thrust-Ready.png) · [extension](../../art/player-employee-01/unity-review/Thrust-3.png) · [hold](../../art/player-employee-01/unity-review/Thrust-6.png) · [retraction](../../art/player-employee-01/unity-review/Thrust-12.png) · [returned](../../art/player-employee-01/unity-review/Thrust-18.png). These are not actual game captures or human quality approval.

```sh
python3 tools/cinder_four_player.py build
python3 tools/test_employee_baton.py
python3 tools/test_cinder_threat.py --headless
```

No manual windows or Editor Play this task. Follow the [companion policy](companion-play.en.md): open two windows only on “직접 테스트 해볼게”. Human hit-feel, clothing/corner penetration and first-person occlusion, all locomotion phases/rapid view changes, full delivery/suppression regression, Windows/LAN and performance remain unverified. Dedicated jump/landing, backward/sideways, rescue/down motions and first-person arms remain incomplete.

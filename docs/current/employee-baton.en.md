# Employee upright baton grip and short forward pulse — 0.9.12

[한국어](employee-baton.ko.md)

2026-10-03. The user ended the 0.9.11 hands-on test and reported an awkward holding pose. Restore the upright 0.9.9 ready grip and extend the hand and baton once, like casting a spell. Current game **0.9.12**, protocol **13**, TCP **27842**. [Version settings](../../NoReturns/ProjectSettings/ProjectSettings.asset). Cancel the always-forward aiming stance from 0.9.10. Naturalness and hit-feel approval of this revision remain pending.

## Current behavior

- Empty-hand grip position is **(0.30, 1.21, 0.20)m** relative to the employee, with baton rotation **(15, -10, -15)°**, matching 0.9.9 readiness. Enter readiness over approximately **0.1s**. Do not hold the baton horizontally aimed at rest.
- Extend the hand and upright baton **0.20m** forward: fast extension during **0–0.06s**, hold during **0.06–0.10s**, retract during **0.10–0.30s**. Add no lateral swing, baton reorientation or chest twist. Contact remains **immediate** at click, with a **6s** cooldown. This adds no magic/projectile mechanic.
- Retain actual hand attachment, finger grip and arm reach limits. Walking poses can leave a target-to-hand gap at the reach limit; do not stretch bones.
- First person restores the 0.9.9 grip offset **(0.27, -0.34, 0.46)m** and rotation **(-12, 180, -18)°**, extending only **0.18m**. Check the baton axis’s forward projection with **0.04m** clearance and pull depth only near walls. Keep the **0.35** minimum scale. Full-body first-person arms and all-corner penetration fixes remain absent.
- Block baton and attack posing during carrying, down, valid rescue, and inactive/unoccupied states. Cancel interrupted attacks without replay; return to readiness. Valid rescue takes priority over simultaneous attack. Retain range, facing, visibility, Listener stun and physics adjudication.

## Production and validation

[Arm motion](../../NoReturns/Assets/_NoReturns/Runtime/CarryLab/EmployeeVisual.Baton.cs), [attachment/first person](../../NoReturns/Assets/_NoReturns/Runtime/CarryLab/BatonVisual.cs), [sample review](../../NoReturns/Assets/_NoReturns/Editor/EmployeeBatonReview.cs), [current validation record](../validation/employee-pulse-locomotion-0.9.12.json). Apply over the existing Unity Humanoid without a new FBX/clip, Blender source changes or MCP editing. See the [separate current jump/landing guide](employee-locomotion.en.md).

Pass **120** samples: Idle/Walk × headings 0°/90°/135° × readiness and 20 times spanning 0–0.30s at 60fps. Maximum hand/baton extension **0.200000m**, lateral/vertical path deviation **0.021758m**, baton-axis change **0°**, chest twist **0°**, target error **0.041788m**, bone-length change **0.000000358m**. Target error measures reach limits, not weapon separation from the hand. The initial 0.24m extension failed with 0.037240m path deviation from the reach limit. Reducing extension to 0.20m passed the same review.

Static review: [readiness](../../art/player-employee-01/unity-review/Pulse-Ready.png), [extension](../../art/player-employee-01/unity-review/Pulse-3.png), [hold](../../art/player-employee-01/unity-review/Pulse-6.png), [retraction](../../art/player-employee-01/unity-review/Pulse-12.png), [return](../../art/player-employee-01/unity-review/Pulse-18.png). These are neither gameplay captures nor user quality approval.

Mac build passes with **zero errors/7 existing warnings**; two actual windowless clients pass **12** pulse/carry-exclusion checks. Actual body-relative baton extension is **0.153486–0.200003m**, maximum path deviation **0.005647m**, axis change **0.001645°**. World hand travel during walking includes employee movement and is not attack reach. See the validation record for additional companion/Listener scope. Confirm CLI **1.0.0-beta.12**, Pipeline **0.8.0-exp.1** and Editor **6000.6.4f1** match official latest releases.

```sh
python3 tools/cinder_four_player.py build
python3 tools/test_employee_baton.py
python3 tools/test_cinder_companion.py
python3 tools/test_cinder_threat.py --headless
```

After ending manual play, open no new manual windows or Editor Play. Only launch two windows on “직접 테스트 해볼게” under the [current launch rules](companion-play.en.md). Revised user quality, abrupt look/corners, full delivery/suppression regression, Windows/LAN/performance remain unverified. Backward/sidestep, dedicated rescue/down clips and first-person arms remain outstanding. Preserve [0.9.10 validation](../validation/employee-baton-thrust-0.9.10.json) and [0.9.9 validation](../validation/employee-baton-0.9.9.json) as historical records.

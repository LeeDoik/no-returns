# Employee baton — original motion restored 0.9.13

[한국어](employee-baton.ko.md)

2026-10-03. The user requested the “very first version”; restore readiness/use from the original right-hand attachment at **0.9.6 / 8cd97aa**. Restore the version without attack arm/chest correction. Current game **0.9.13**, protocol **13**, TCP **27842**. [Version settings](../../NoReturns/ProjectSettings/ProjectSettings.asset). Cancel 0.9.9 readiness/swing, 0.9.10 forward aim/thrust and 0.9.12 upright forward pulse in the current specification. Preserve historical evidence in the [change log](../archive/change-log.en.md).

## Current behavior and production

- Remote batons attach to the actual right hand/fingers and follow Idle/Walk unchanged. No separate attack arm/chest IK. Retain finger grip and two-hand parcel posing.
- Weapon rotation peaks immediately on click and returns over **0.5s** with squared decay. Keep the hand attachment position; rotate the weapon **-65°** around hand-relative z. Contact is **immediate**, cooldown **6s**.
- First-person rest position **(0.27,-0.34,0.46)m**, rotation **(-12,180,-18)°**. Apply **(-0.13,+0.12,+0.18)m** position and **(-65,0,+35)°** rotation deltas multiplied by the decaying strike amount. Restore original **0.95m** wall clearance and minimum **0.35** scale. Complete wall/corner penetration prevention remains unverified.
- Hide during parcel/beacon carrying, down, valid rescue, inactivity or disconnected slots. Preserve rescue priority, range, visibility and Listener stun rules. No dedicated attack FBX or first-person arms were added.

[Attachment/original motion](../../NoReturns/Assets/_NoReturns/Runtime/CarryLab/BatonVisual.cs), [employee](../../NoReturns/Assets/_NoReturns/Runtime/CarryLab/EmployeeVisual.cs), [static review](../../NoReturns/Assets/_NoReturns/Editor/EmployeeBatonReview.cs), [native checks](../../tools/test_employee_baton.py), [current movement](employee-locomotion.en.md).

## Validation and unknowns

[Validation record](../validation/employee-direction-0.9.13.json). Idle/Walk × 3 yaws × 5 walk phases × 31 attack/return times: **930** samples. Maximum attachment distance **0.119736m**, added arm/chest rotation **0°**, return by 0.5s. Mac build: **zero errors/7 existing warnings**. See the record for actual two-client weapon rotation/attachment, cooldown, carrying hiding/restoration and four-client Listener/down/rescue results.

Official releases match Unity CLI **1.0.0-beta.12**, Pipeline **0.8.0-exp.1**, Editor **6000.6.4f1**; no pending updates. No Blender/MCP editing. Stop the prior manual session and open no new manual windows. Revised naturalness/impact, all-frame penetration, Windows/LAN/performance and the full delivery/suppression regression remain unverified.

# Employee jump and landing presentation — 0.9.13

[한국어](employee-locomotion.ko.md)

2026-10-03. Current game **0.9.13**, protocol **13**, TCP **27842**. [Settings](../../NoReturns/ProjectSettings/ProjectSettings.asset). Retain jump/landing and add backward/sidestep presentation. No new dedicated Mixamo clips: redirect alternating foot trajectories from the existing Walk toward actual movement through Humanoid leg IK. Human quality approval is pending.
## Backward/sidestep production

Compute body-relative horizontal velocity from actual position changes, smoothing direction exponentially at **12** per second. Below **0.12m/s** is Idle; x exceeding **1.2×** absolute z is left/right strafe; otherwise label forward/backward. Diagonals use a continuous direction vector. Preserve forward Walk; blend correction as the forward component changes from **0.85 to 0.50**.

Redirect the existing Walk's fore/aft foot displacement by **0.4×** laterally and **0.65×** longitudinally, retaining foot height/alternating timing. Keep left/right trajectories outside body-center x **-0.045/+0.045m**. Blend foot orientation toward rest by up to **0.8**, preserving bone lengths through two-joint leg IK. Do not rotate root/hips. Airborne, down and stopped states release correction; retain landing compression and carry-arm order. [Direction code](../../NoReturns/Assets/_NoReturns/Runtime/CarryLab/EmployeeVisual.Direction.cs).

[Validation record](../validation/employee-direction-0.9.13.json). 3 yaws × backward/left/right/rear-diagonal 5 directions × 1/2.2/4m/s × 5 walk phases: **225** samples verify separate foot lanes, torso/root/bone length, finite meshes and air/down release. Maximum bone length change **0.000000507m**; the same foot’s trajectory difference across two phases projects at least **0.332020m** along actual movement. [Backward](../../art/player-employee-01/unity-review/Backward.png), [left](../../art/player-employee-01/unity-review/Strafe-Left.png), [right](../../art/player-employee-01/unity-review/Strafe-Right.png) are static samples, not human quality approval/game footage. See the record for actual per-direction network checks.

## Current behavior and production

- Calculate vertical speed from actual employee position differences and probe **0.30m** below an origin **0.20m** above the employee’s ground reference. Exclude CharacterControllers, Rigidbodies and triggers; accept floor normals with y above **0.5**. Treat absent ground or upward speed above **1m/s** as airborne.
- Show rising/apex/falling while airborne. Rising requires vertical speed above **0.3m/s**, falling below **-0.3m/s**, with apex between them. Tuck the legs slightly and adjust ankles. Transition from Walk to Idle when air weight exceeds **0.5**. Change leg weight at **12** per second.
- After more than **0.12s** airborne, touching the floor starts a **0.22s** landing presentation. Lower visual hips by up to **0.065m**, using two-joint leg posing to preserve that frame’s foot positions/orientations while flexing and recovering the knees. Clear air/landing state on down or position changes of **2m or more**.
- Preserve employee root, bone lengths/scales, colliders, networking, existing jump speed and gravity. Evaluate the same presentation from actual host/remote interpolated positions, including aboard the ship/on slopes. Add no jump input, landing sound or landing damage.

[Presentation code](../../NoReturns/Assets/_NoReturns/Runtime/CarryLab/EmployeeVisual.Locomotion.cs), [position-based speed](../../NoReturns/Assets/_NoReturns/Runtime/CarryLab/EmployeeVisual.cs), [runtime connection](../../NoReturns/Assets/_NoReturns/Runtime/CarryLab/CarryRoom.cs), [static review](../../NoReturns/Assets/_NoReturns/Editor/EmployeeLocomotionReview.cs), [actual companion check](../../tools/test_cinder_companion.py).

## Validation and remaining work

[Validation record](../validation/employee-pulse-locomotion-0.9.12.json). Check **270** samples: Idle/Walk × 3 headings × 5 walk times × 9 ground/rise/apex/fall/landing/return times. Verify finite meshes, unchanged root/bone lengths, foot contact, state return and down cancellation. Maximum bone-length change **0.000000507m**, landing foot error **0.000001566m**, hip compression **0.065000m**. [Rise](../../art/player-employee-01/unity-review/Jump-Rise.png), [fall](../../art/player-employee-01/unity-review/Jump-Fall.png), [landing](../../art/player-employee-01/unity-review/Landing.png) are static review samples, not gameplay captures/human approval.

Mac build passes with **zero errors/7 existing warnings**. Actual windowless human/companion checks include the existing seven actions, movement/jump height, following, carry hiding/restoration and rise/fall/landing/ground return on both peers. The first check failed to collect landing weights that occurred after the bot’s Jump label ended. Both peers had observed Landing states; correct the collector to span the label boundary through landing. Use the validation record for actual passing numbers and additional Listener scope.

End manual play and open no new manual windows or Editor Play. [On-demand launch guide](companion-play.en.md). Backward/sidestep correction is implemented above. Next production is **first-person arms**; dedicated motion clips remain separate outstanding work. Dedicated jump FBX/rescue/down clips, steep slopes/stairs, abrupt turns, all-frame clothing penetration during air/landing, human quality, Windows/LAN and performance remain unverified. Distinguish implemented presentation from final dedicated clip production.

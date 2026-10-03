# Employee jump and landing presentation — 0.9.12

[한국어](employee-locomotion.ko.md)

2026-10-03. Implement jump/landing as the next character task after revising the baton. Current game **0.9.12**, protocol **13**, TCP **27842**. [Settings source](../../NoReturns/ProjectSettings/ProjectSettings.asset). Keep the two Idle/Walk FBXs; do not download or author a dedicated Mixamo jump clip. Adjust existing Humanoid leg rotations and landing hip position for presentation.

## Current behavior and production

- Calculate vertical speed from actual employee position differences and probe **0.30m** below an origin **0.20m** above the employee’s ground reference. Exclude CharacterControllers, Rigidbodies and triggers; accept floor normals with y above **0.5**. Treat absent ground or upward speed above **1m/s** as airborne.
- Show rising/apex/falling while airborne. Rising requires vertical speed above **0.3m/s**, falling below **-0.3m/s**, with apex between them. Tuck the legs slightly and adjust ankles. Transition from Walk to Idle when air weight exceeds **0.5**. Change leg weight at **12** per second.
- After more than **0.12s** airborne, touching the floor starts a **0.22s** landing presentation. Lower visual hips by up to **0.065m**, using two-joint leg posing to preserve that frame’s foot positions/orientations while flexing and recovering the knees. Clear air/landing state on down or position changes of **2m or more**.
- Preserve employee root, bone lengths/scales, colliders, networking, existing jump speed and gravity. Evaluate the same presentation from actual host/remote interpolated positions, including aboard the ship/on slopes. Add no jump input, landing sound or landing damage.

[Presentation code](../../NoReturns/Assets/_NoReturns/Runtime/CarryLab/EmployeeVisual.Locomotion.cs), [position-based speed](../../NoReturns/Assets/_NoReturns/Runtime/CarryLab/EmployeeVisual.cs), [runtime connection](../../NoReturns/Assets/_NoReturns/Runtime/CarryLab/CarryRoom.cs), [static review](../../NoReturns/Assets/_NoReturns/Editor/EmployeeLocomotionReview.cs), [actual companion check](../../tools/test_cinder_companion.py).

## Validation and remaining work

[Validation record](../validation/employee-pulse-locomotion-0.9.12.json). Check **270** samples: Idle/Walk × 3 headings × 5 walk times × 9 ground/rise/apex/fall/landing/return times. Verify finite meshes, unchanged root/bone lengths, foot contact, state return and down cancellation. Maximum bone-length change **0.000000507m**, landing foot error **0.000001566m**, hip compression **0.065000m**. [Rise](../../art/player-employee-01/unity-review/Jump-Rise.png), [fall](../../art/player-employee-01/unity-review/Jump-Fall.png), [landing](../../art/player-employee-01/unity-review/Landing.png) are static review samples, not gameplay captures/human approval.

Mac build passes with **zero errors/7 existing warnings**. Actual windowless human/companion checks include the existing seven actions, movement/jump height, following, carry hiding/restoration and rise/fall/landing/ground return on both peers. The first check failed to collect landing weights that occurred after the bot’s Jump label ended. Both peers had observed Landing states; correct the collector to span the label boundary through landing. Use the validation record for actual passing numbers and additional Listener scope.

End manual play and open no new manual windows or Editor Play. [On-demand launch guide](companion-play.en.md). Next production is **dedicated backward/sidestep movement**, then **first-person arms**. Dedicated jump FBX/rescue/down clips, steep slopes/stairs, abrupt turns, all-frame clothing penetration during air/landing, human quality, Windows/LAN and performance remain unverified. Distinguish implemented presentation from final dedicated clip production.

# First-person hand/arm production and integration — 0.9.14

[한국어](first-person-arms.ko.md)

2026-10-04. Reuse the employee spacesuit's gloves and sleeves for the local right arm and two-hand parcel carry. [Baton motion](employee-baton.en.md), [validation record](../validation/employee-first-person-0.9.14.json). Game **0.9.14**, protocol **13**, TCP **27842**. [Version settings](../../NoReturns/ProjectSettings/ProjectSettings.asset). No new Mixamo motion, Blender/MCP editing or image/model generation service calls.

## Local reproduction

1. Prepare `EmployeeLocal/Employee.prefab` and local Idle/Walk through the existing [employee import](employee-animation.en.md).
2. Run Unity menu `NO RETURNS/Art/Prepare Local First Person Arms`. The [extractor](../../NoReturns/Assets/_NoReturns/Editor/FirstPersonArmsBuild.cs) retains triangles whose vertices each have a combined skin weight of at least 0.6 from bones beneath the corresponding upper arm. From 9,118 source triangles, generate **1,414 vertices/1,240 triangles** on the left and **1,452 vertices/1,250 triangles** on the right. Preserve source bones, bind poses, material and texture; add no colliders.
3. Generate `Assets/_NoReturns/Resources/EmployeeLocal/FirstPersonArms.prefab` and two meshes locally. Redistribution review of the existing Mixamo source remains incomplete, so this folder stays excluded from public Git. Commit reproduction code, Unity .meta, validation records and review PNGs.
4. The [builder](../../NoReturns/Assets/_NoReturns/Editor/CinderFourPlayerBuild.cs) validates the arm prefab/two meshes and absence of colliders before building the existing Mac test app. Rerun extraction after reimporting the employee model.

## Presentation

[Runtime integration](../../NoReturns/Assets/_NoReturns/Runtime/CarryLab/FirstPersonArms.cs). Continue hiding the local full body and display only two extracted arm meshes. Show the right glove/sleeve gripping the baton when empty-handed; show both hands when carrying a parcel. Restore the right-hand baton after release. Hide this presentation during beacon carry, down, valid rescue or inactivity. Dedicated beacon/rescue hand poses remain incomplete.

Exclude **layer 31** from the URP Base camera and render arms/local baton through an Overlay camera. The Overlay clears depth; arms neither cast nor receive shadows. Follow the local FOV, with near **0.015m** and far **3m**. Place the model origin at **(0,-1.75,0.20)m** relative to the eye. [Official Unity camera stacking guidance](https://docs.unity3d.com/6000.0/Documentation/Manual/urp/camera-stacking.html). Null-graphics automated clients skip GPU renderer/stack initialization while performing the same bone/grip calculations. Verify actual stack configuration through Editor graphics checks.

Drive right-hand IK from the baton position/rotation, then attach the final weapon to the actual grip. Preserve arm bone lengths. Near a wall, retract only eye-relative weapon depth to a minimum factor of **0.35**; do not shrink the whole arm/weapon. A central forward ray does not guarantee prevention of all lateral corner penetration. Parcel hands reuse existing cargo contact goals; distant carrying can retain contact error due to arm reach limits.

## Validation and remaining work

[Editor review](../../NoReturns/Assets/_NoReturns/Editor/FirstPersonArmsReview.cs): FOV 65/80/100° × pitch -45/0/45° × depth 0.35/1 × 31 times, plus carrying/hiding: **560** samples. Maximum grip-target error **0.022269m**, bone-length change **0.000000477m**. Verify two-hand carry, hiding, layer/depth stack and finite meshes. [Ready](../../art/player-employee-01/unity-review/FirstPerson-Ready.png), [strike](../../art/player-employee-01/unity-review/FirstPerson-Strike.png), [return](../../art/player-employee-01/unity-review/FirstPerson-Return.png), [two-hand carry](../../art/player-employee-01/unity-review/FirstPerson-Carry.png). These are static Editor reviews, not game screenshots or human quality approval.

Also pass an offscreen render of the actual URP Base/Overlay stack: **49,977** visible arm/baton pixels against black, and inspect the [stack output](../../art/player-employee-01/unity-review/FirstPerson-Stack-Ready.png). This checks the actual Editor camera stack, not native full-map/HUD composition. Inspect **5** review images total.

See the validation record for actual Mac build and windowless client results. Naturalness/impact, actual game GPU stack/lighting/HUD composition, all-distance/rotation/wall-corner penetration, Windows/LAN, performance and dedicated rescue/beacon motions remain unverified. Open no manual windows and retain the [on-demand play](companion-play.en.md) rule.

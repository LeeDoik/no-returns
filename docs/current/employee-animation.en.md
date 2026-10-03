# Employee full body and Idle/Walk — 0.9.8

[한국어](employee-animation.ko.md)

## 2026-10-03 — Two-handed parcel carrying pose 0.9.8

Bring default carrying reach from **1.1m to 0.8m** and align both hands with the upper part of the parcel face nearest the employee. Retain the **0.75–1.6m** wheel range and rotation, placement, ownership and collision rules. Apply arm posing over Idle/Walk, blend in/out over about **0.167 seconds**, and clear immediately when down. Change wrist/arm rotations only, preserving bone lengths/scales and employee root. Distant or heavily tilted parcels can remain beyond hand reach. This does not establish perfect contact at every distance or eliminate finger penetration. Preserve baton hiding while carrying/restoration after release and local-body hiding. Dedicated beacon grip, attack/jump/rescue motions and first-person arms remain outside this change.

Implementation: [two-hand pose](../../NoReturns/Assets/_NoReturns/Runtime/CarryLab/EmployeeVisual.Carry.cs), [runtime integration](../../NoReturns/Assets/_NoReturns/Runtime/CarryLab/CarryRoom.cs), [default control reach](../../NoReturns/Assets/_NoReturns/Runtime/CarryLab/CarryRoom.Controls.cs). Query the saved employee rig through Blender MCP: each upper arm is approximately 0.228m and forearm 0.249m; do not modify/save the Blender source. The game evaluates its existing Mixamo Humanoid in Unity and adjusts hand contact. Add no animation FBX or packages. Game **0.9.8**, protocol **13**, TCP **27842**. [Version source](../../NoReturns/ProjectSettings/ProjectSettings.asset).

[Validation record](../validation/employee-carry-0.9.8.json): Unity **6000.6.4f1** Mac build, **zero errors/7 warnings**. Check **120** samples across Idle/Walk × 3 reaches × 4 rotations × 5 times for finite meshes and unchanged bone lengths/root; also check release/down pose clearing. Maximum hand reference-point error for nearby forward-facing 0.75/0.8m samples: **0.039603m**; maximum bone-length difference: **0.000000388m**. [Sample validation code](../../NoReturns/Assets/_NoReturns/Editor/EmployeeCarryReview.cs). Four renders are static Unity pose reviews, not actual game captures: [Idle](../../art/player-employee-01/unity-review/Carry-Idle.png), [Walk](../../art/player-employee-01/unity-review/Carry-Walk.png), [hands close-up](../../art/player-employee-01/unity-review/Carry-Hands.png), [reach limit](../../art/player-employee-01/unity-review/Carry-ReachLimit.png).

Pass **10** [carry checks](../../tools/test_employee_carry.py) across four actual windowless clients: ownership/default contact, non-owner drop rejection, ±90°/180° rotation, maximum-reach clamp/near-contact restoration, release pose clearing and clean runtime logs. The first attempt fails because landing props block parcel rotation; move through the existing corridor to clear space and pass. Do not weaken collision checks. Also pass **5 grouped** windowless human+automatic companion regression checks. Open no manual game windows or Editor Play. Open [human+companion](companion-play.en.md) only on “직접 테스트 해볼게”. All-frame/extreme-view/rotation transitions, first-person occlusion/human quality, full delivery/hazard regression, Windows/LAN and performance are not validated this task.

## Historical body integration and baton records 0.9.5–0.9.6

2026-10-03. Integrate the supplied employee model, corrected Idle and Walking into the current Cinder player. Replace primitive visuals while retaining existing movement, collision, carrying and network adjudication. Initial integration was 0.9.5; game **0.9.6** at the time of this record, protocol **13**, TCP **27842**. Version source: [PlayerSettings](../../NoReturns/ProjectSettings/ProjectSettings.asset); implementation: [employee visuals](../../NoReturns/Assets/_NoReturns/Runtime/CarryLab/EmployeeVisual.cs) and [CarryRoom](../../NoReturns/Assets/_NoReturns/Runtime/CarryLab/CarryRoom.cs).

## 2026-10-03 — Right-hand baton attachment 0.9.6

The version at the time was **0.9.6**, protocol **13**, TCP **27842**. Move teammates' batons from a fixed torso offset to their actual right-hand/finger bones. Follow idle/walk motion and curl fingers around the handle. Retain the local first-person position, wall pull-in and existing hit adjudication. Carrying a parcel or beacon hides that employee's baton on every peer; dropping or placing the object restores it. Down/rescue hiding also remains.

Sources: [baton presentation](../../NoReturns/Assets/_NoReturns/Runtime/CarryLab/BatonVisual.cs) and the employee visual code above. Avoid inheriting the imported bones' 100x scale: use world metres, 95% from wrist to middle proximal joint plus 0.025m toward the palm. Apply a fixed grip of 55° for fingers and 30° for the thumb around the index-to-little-finger axis only while showing the baton. Add no dedicated full-body strike motion or carrying-contact IK.

[Development tools and Blender MCP setup](macos-development.en.md#2026-10-03--current-tools-and-blender-mcp) · [This validation record](../validation/baton-hand-0.9.6.json). The 0.9.5 results below are historical; use the linked record for this task's scope. Carrying contact, dedicated attack motion, all-frame penetration/user quality and first-person arms remain incomplete.

- [x] Unity 6000.6.4f1 Mac build: zero errors, 14 warnings. Four actual processes pass 69 baton/suppression/delivery + 10 visual/transition + 31 Listener/hit/down/rescue checks = **110**. Review 4 idle/walk hand/body samples and a game capture from the new build.
- [x] Actual MCP stdio tool calls read Blender's 5 objects, 53 bones and right-hand information. Preserve unsaved scene/existing user edits. Apply current official Unity/CLI/Pipeline/uv/Codex CLI releases and confirm Blender 5.2.2 LTS matches the latest release.
- [ ] Codex desktop update/restart and default MCP tool exposure, the new Unity's Windows support/execution, human quality/all-frame penetration and performance. Observe a Burst error resolving a Pipeline DLL during upgrade and 5 Burst entry-point warnings in the final build. Build/110 game checks pass, but warning cause/performance impact remains unverified. Remaining warnings concern mesh collision pre-baking, absent Pipeline runtime config, obsolete search APIs and stripped debug shaders.

## Integration and production

- The [import script](../../NoReturns/Assets/_NoReturns/Editor/EmployeeAnimationBuild.cs) imports each FBX as `Humanoid / Create From This Model`. Both use 53 bone names but different rest joint positions, so retarget through separate Avatars. Both are valid Humanoids; Idle lasts 8.333334 seconds and Walk 1.033333 seconds.
- Apply the original dirty color texture and URP/Lit with Smoothness 0.18. The source has 4,765 vertices, 9,118 triangles and a 1.8m height; Unity splits UVs/normals into 10,612 vertices. Blend team colors into the full material at 32%.
- Actual horizontal movement drives Idle→Walk above 0.12m/s and Walk→Idle below 0.08m/s, with 0.15-second transitions. Stride playback rate is speed/2.2 clamped to 0.35–2.2. Disable Root Motion; the existing CharacterController owns position. Remote players derive the same transitions from received/interpolated positions.
- Each window hides its own body and shows the other three employees. Down state freezes animation and retains the existing whole-body rotation. Add no gameplay colliders.
- Reuse the single forward Walk for sideways/backward movement for now. Dedicated backward/strafe, carrying hand contact, full-body baton attacks, down/get-up, rescue, jump motions and first-person arms remain subsequent production work.

## Local reproduction and launch

Public source-redistribution terms for original/corrected Mixamo motions remain unresolved. Keep those FBXs and generated Prefab/Controller locally under `NoReturns/Assets/_NoReturns/Resources/EmployeeLocal/`, excluded from Git. Publish only import/check code, hashes and static images. Game builds include these local assets, so this validation build also remains local.

1. Use the [preceding correction step](03-guides.en.md#2026-10-03--correct-the-supplied-idle-upper-body-posture) to prepare `artifacts/employee-idle/corrected/NR_Employee_Idle_Upright.fbx`. Also provide `~/Downloads/Walking.fbx` and the repository's original employee texture. Input hashes are recorded in the [validation record](../validation/employee-unity-0.9.5.json).
2. Run Unity menu `NO RETURNS > Art > Prepare Local Employee Animations`. Alternative input paths can be supplied through `NoReturns.Editor.EmployeeAnimationBuild.Prepare(idlePath, walkPath)` in the Editor. Existing generated local assets are updated; separate source files remain intact.
3. Run `NO RETURNS > Art > Review Local Employee Animations` to check 10 poses and create 4 static images. Local evidence lives in `artifacts/employee-unity/`. Static images draw Animator-evaluated baked meshes and are distinct from actual game captures. Correct the rotation axis for height measurement according to the [Unity BakeMesh definition](https://docs.unity.com/en-us/engine/6000.3/script-reference/unityengine/skinnedmeshrenderer/bakemesh).
4. Build with `python3 tools/cinder_four_player.py build`, then launch [07_Play_Cinder_4P.command](../../07_Play_Cinder_4P.command). Observe teammates while moving in another window. A fresh checkout without prepared local assets stops the Cinder build with a preparation error. Direct Editor Play retains the primitive visual fallback.
5. Recheck with `python3 tools/test_employee_animation.py`, `python3 tools/cinder_four_player.py check` and `python3 tools/test_cinder_threat.py`. Run sequentially because they share the same port.

## Validation and remaining quality review

[Validation record](../validation/employee-unity-0.9.5.json). Verify two Humanoids, finite meshes across 10 sampled poses, heights of 1.768–1.838m, zero root displacement and foot movement. Fix incorrect preview-axis measurement/static skinning refresh in the review tool without arbitrarily rescaling the model. Native Mac build: zero errors and 7 existing warnings. Pass 10 visual/transition, 13 carrying/reconnection and 31 Listener/baton/down/rescue checks across four actual processes: 54 total. The complete 60-check suppression and delivery/payout UI suites were not rerun. Warnings concern existing map collision prebaking, missing Pipeline runtime configuration, obsolete search APIs and stripped unused debug shaders.

User posture/color approval, human assessment of sliding/loop seams, full-frame cloth penetration/cargo contact, four human players, other PCs, Windows and measured performance remain pending. This document supersedes earlier employee Unity-not-integrated statements within its implemented/verified scope.

[Unity idle pose](../../art/player-employee-01/unity-review/Idle-1.png) · [Walking pose](../../art/player-employee-01/unity-review/Walk-3.png) · [Actual game capture](../../art/player-employee-01/unity-review/employee-in-game.png).

Static baton poses: [idle body](../../art/player-employee-01/unity-review/Baton-Idle-body.png) · [idle hand](../../art/player-employee-01/unity-review/Baton-Idle-hand.png) · [walk body](../../art/player-employee-01/unity-review/Baton-Walk-body.png) · [walk hand](../../art/player-employee-01/unity-review/Baton-Walk-hand.png). These are Animator-evaluated samples from Unity 6000.6.0f1, not all-frame penetration checks or human approval.

[Actual game capture in the new Unity build](../../art/player-employee-01/unity-review/Baton-in-game.png).

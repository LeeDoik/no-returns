# Employee baton — dynamic motion 0.9.15

[한국어](employee-baton.ko.md)

## 2026-10-04 — First-person rescue hands 0.9.16

Reach with both hands and show a small assisting movement during valid rescue. Hide the baton, restore the right-hand baton on cancellation/completion and hide hands when down. Retain the 25° wrist limit. [Current production, values and remaining scope](first-person-arms.en.md), [validation record](../validation/employee-rescue-0.9.16.json). Game **0.9.16**, protocol **13**, TCP **27842**. Preserve existing rescue adjudication. This supersedes older statements below that rescue hands are missing or hidden, within this scope. Beacon hands, full-body rescue/down, actual body contact and human quality review remain pending. Open manual test windows only on request.

## 2026-10-04 — First-person wrist correction 0.9.15

Fix the wrist bend reported by the user in the 0.9.14 attack preview. Correct the relationship between the palm and cylindrical grip axes; move the elbow outward so the forearm and hand align. Limit the hand/forearm direction angle to 25° and solve grip contact again. Keep the baton upright during forward movement and retract the shoulder origin near walls. [Current pose/production](first-person-arms.en.md), [validation record](../validation/employee-wrist-0.9.15.json). Game **0.9.15**, protocol **13**, TCP **27842**. The previous pose has a user-confirmed quality issue; the revised pose has not received user approval. Manual play stays stopped.

2026-10-04. At the user's request, replace the restored 0.9.13 weapon-only rotation with short anticipation, fast forward arm/weapon movement and recovery. Integrate [first-person hands/arms](first-person-arms.en.md). Game **0.9.15**, protocol **13**, TCP **27842**. [Version settings](../../NoReturns/ProjectSettings/ProjectSettings.asset). Preserve the previous motion in the [change log](../archive/change-log.en.md) and 0.9.13 validation record; the current behavior is specified below.

## Current behavior

- The [shared curve](../../NoReturns/Assets/_NoReturns/Runtime/CarryLab/BatonMotion.cs) uses 0–0.025s small anticipation, 0.025–0.085s fast extension, 0.085–0.14s peak hold and 0.14–0.42s recovery. Preserve **immediate** contact, **6s** cooldown, existing range, visibility, Listener stun and rescue priority. Do not delay adjudication to match the visual motion.
- Teammates use right-arm IK and up to **4°** chest rotation, attaching the weapon to the actual grip. Preserve Idle/Walk resting arms; return to existing motion after attacking. Retain finger grip, two-hand parcel pose, backward/sidestep and jump/landing.
- Move the local right glove/sleeve together with the baton. Rest grip target **(0.26,-0.33,0.46)m**, position delta **(-0.065,+0.10,+0.18)m**, rotation **(-12-18a,180,-18+8a)°**, using curve amount `a`. Anticipation reaches -0.18. If arm reach is insufficient, attach the baton to the actual grip without stretching bones.
- Hide the baton during parcel/beacon carrying, down, valid rescue, inactivity or disconnection. Blocking an ongoing attack cancels its remaining motion. Switch parcel carrying to local two-hand presentation. Retract depth to a minimum **0.35** factor based on a central wall ray. [Presentation](../../NoReturns/Assets/_NoReturns/Runtime/CarryLab/BatonVisual.cs), [arm/chest motion](../../NoReturns/Assets/_NoReturns/Runtime/CarryLab/EmployeeVisual.Baton.cs).

## Previous 0.9.14 validation and unknowns

[Validation record](../validation/employee-first-person-0.9.14.json). **930** [static baton samples](../../NoReturns/Assets/_NoReturns/Editor/EmployeeBatonReview.cs) verify attachment distance at most **0.119736m**, attack arm rotation up to **90.273743°**, chest rotation up to **4.000152°**, finite state and recovery. See the record for **560** first-person samples and actual two/four-client motion, carrying, movement and down/rescue checks. Mac build: **zero errors/7 existing warnings**.

Official releases match Unity CLI **1.0.0-beta.12**, Pipeline **0.8.0-exp.1**, Editor **6000.6.4f1**; no pending updates. No new FBX or Blender/MCP editing. Open no manual windows or Editor Play. Human naturalness/impact, actual game GPU stack/lighting/HUD composition, all-frame/wall-corner penetration, dedicated FBX/beacon/rescue motion, Windows/LAN, performance and full delivery/suppression regression remain unverified.

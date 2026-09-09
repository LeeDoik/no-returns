# Worker rig and animation revision

[한국어](03-tripo-animation.ko.md)

> Documentation update: versions and values here belong to this file's original context. Check the [documentation home](../README.en.md) first for current rules and outstanding work.

## Latest progress — runtime integration

On September 9, 2026, the supplied GLB was verified to contain 41 bones and seven Tripo motions. Three carrying clips, two airborne poses and a throw follow-through were authored in Blender and integrated into Godot. Original and editable files are preserved. [Integration result and validation scope](04-animation-integration.en.md).

The paid asset is `ba747b8c-0476-4108-9a87-6823869224c1`. Recorded rigging cost remains 20 credits. The browser download timeout was resolved by the user supplying the local file. The following sections retain the original problems, targets and Free-plan history; use the integration result above for current completion status.

## Scope and observed problem

The player character is the first target. The rat and parcel animations are outside this revision. The existing worker uses one `Casual_Walk` clip, resets its legs when stationary, and aims arm bones toward the held parcel. It has no dedicated idle or carrying clips. Replacing the rig alone cannot resolve abrupt transitions, unsuitable movement cadence, or hand contact.

## Upload source

`art/tripo-01/worker-tripo-input.glb` is a textured, unrigged T-pose baked from `art/release-01/source/worker.blend`. Blender preparation smooths existing skin weights across coincident UV-seam vertices before raising the arms, then removes the old rig and animation. The original source remains intact. The companion `.blend` is editable; front and angled PNGs document the upload pose.

The mesh is 1.65 m tall and contains 20,664 triangles. Validation checks that the GLB includes an image, contains no skins or animations, and is below the 150 MB upload limit. Exact file size is recorded in `art/tripo-01/upload-validation.json`. The front preview was inspected for tearing and long underarm spikes. This does not validate the forthcoming rig's deformation.

Tripo upload asset: `b22f0221-706e-4a90-afc8-433458e5b6eb`. Original UV is enabled. The upload orientation dialog uses Y = -90 degrees so the face is visible. `v1.0 - Good for Humanoid` produced a humanoid rig. The credit balance changed from 200 to 180. The `walk` animation plays in the browser; its displayed duration is about 2.38 seconds. A single preview does not establish acceptable deformation throughout the whole cycle.

### Current export blocker

The account is Free. `Export Skeleton` is disabled for both GLB and Blender FBX. Its adjacent upgrade control opens the pricing dialog. No rig or animation file has been downloaded. On September 9, 2026, the account's monthly Pro offer displays $8 for the first month, renewing at $20 per month. Annual billing is a separate option. No subscription or payment was submitted by this task; the user was asked to handle the new recurring purchase.

The pricing UI labels Free as non-commercial and Pro as including commercial use. Treat this Free-plan rig as an evaluation result. Before Steam production integration, confirm rights for the output or regenerate the rig while a qualifying paid plan is active; do not assume upgrading retroactively licenses an earlier output. The source mesh itself was prepared locally from the pre-existing Blender file.

## Motion requirements

These are project requirements, not a claim that Tripo supplies every clip. The observed library includes `idle`, `standing_relax`, `walk`, `run`, `lift_heavy`, `jump`, `jump_down`, `fall`, and `hit_to_body_01`. Each candidate needs a visual review on this character.

| Game clip | Intended behavior | Production route |
|---|---|---|
| Idle | Relaxed breathing, planted feet, quiet hands | Compare idle and standing_relax |
| Walk / Run | Readable foot contact at slow and full movement speeds | Review walk and run, calibrate cadence |
| CarryIdle / CarryWalk | Both hands support the 0.8 m parcel, elbows remain bent | Author a carrying upper-body layer in Blender if no suitable library motion exists |
| Pickup / Throw | Lean and arm follow-through matched to existing grab/release timing | Review lift_heavy for pickup; author the two-handed throw if needed |
| Jump / Fall / Land | Takeoff, airborne pose and landing follow actual grounded state | Review jump, fall and jump_down; split or author phases |
| Stagger | Brief readable loss of balance, then recover | Review hit_to_body_01 and adjust amplitude |

Use one accepted skeleton for all clips. Preserve shoulders, elbows, wrists, hips, knees and ankles. The mitten model does not need individually animated fingers. Download rigged GLB or FBX with the required motions. Keep untouched Tripo outputs under `art/tripo-01` and record their source IDs and available export options.

## Godot integration requirements

Character movement remains controlled by the game. Prefer in-place clips; if the export has root displacement, remove or extract it deliberately so animation does not move the character twice. Match the final rest height to 1.65 m and forward direction to the existing character convention. Retain source transforms until conversion is verified.

Replace the single-clip controller with explicit idle, movement, carrying, airborne and reaction states. Blend pose transitions rather than resetting leg bones. Drive cadence from actual horizontal displacement, including replicated characters. Use a dedicated carrying layer with limited hand-contact correction; do not fully straighten the elbows every frame. Keep grab, throw and collision authority in the existing gameplay code.

## Acceptance checklist

- [x] Preserve the current source and runtime model.
- [x] Prepare and inspect an unrigged, textured T-pose.
- [x] Upload to Tripo and submit humanoid Auto Rig.
- [x] Confirm humanoid rig completion and browser playback of walk.
- [ ] Verify the generated skeleton and shoulder/knee deformation.
- [ ] Obtain and inspect the required animation exports.
- [ ] Integrate state transitions and parcel hand contact in Godot.
- [ ] Visually check start/stop, turns, carrying, throwing and landing from gameplay cameras.
- [ ] Check local and remote players, regression tests and the packaged build.

No runtime model replacement or animation fix is claimed by this preparation document. The currently playable art build remains 0.9.0 until integration is validated.

## Source

[Tripo: uploading an existing model for animation](https://www.tripo3d.ai/help/features/is-it-possible-to-upload-my-existing-model-for-animation) documents supported formats, the 150 MB limit and T-pose humanoid preparation. Account UI and library names were inspected on September 9, 2026, Korea time; plan and export availability depend on the account.

[Tripo commercial-use guidance](https://www.tripo3d.ai/help/privacy-policy/how-to-use-tripo-models-commercially) distinguishes Free and paid use. It does not establish retroactive commercial rights for this specific rig.

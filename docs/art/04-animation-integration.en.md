# Animation integration work plan

[한국어](04-animation-integration.ko.md)

## Target

Continue the [worker revision](03-tripo-animation.en.md) with a rig generated after the user's Tripo upgrade. Use one skeleton and in-place motion. Keep the previous playable model until a replacement passes import and visual checks. Stop work and report when the Codex account's remaining usage reaches 1% or less, as requested by the user. Do not redeem usage resets.

## Execution

- [x] Confirm the upgraded account balance: 3,180 credits.
- [x] Upload the validated local T-pose as a new asset: `ba747b8c-0476-4108-9a87-6823869224c1`.
- [x] Submit humanoid v1.0 Auto Rig, displayed cost 20 credits.
- [x] Generate all seven candidates and confirm their presence in Choose Animations. The rig cost 20 credits; observed balance after generation is 3,160.
- [x] Request GLB export with skeleton, seven motions and Animation stay in Place. Tripo reports export success; local receipt is still unverified.
- [ ] Export a rigged GLB with idle, walk and run first; record filenames, skeleton, frame ranges and source asset in `art/tripo-01`.
- [ ] Obtain jump, fall, lift_heavy and hit_to_body_01 candidates on the same rig. Check each before assigning it to gameplay. These names refer to the observed Tripo library, not a promise of suitable motion.
- [ ] In Blender, check scale, facing, root displacement, missing textures, arm and leg deformation. Store untouched downloads separately from edited output. Author CarryIdle and CarryWalk upper-body poses with bent elbows and hands at the parcel sides. Create two-handed throw follow-through if the candidate library is unsuitable.
- [ ] Add `tests/test_worker_animation.gd`: check required clips, idle after stopping, carry state while holding, no visual root drift and finite bone transforms. Run with the local Godot console and require a meaningful failure before changing the controller.
- [ ] Add `scripts/worker_animation.gd` for explicit clip choice and blending; connect it from `scripts/worker.gd`. Feed existing velocity, held state, vertical motion and stagger. Keep movement and cargo authority unchanged. Derive remote airborne presentation from available replicated motion rather than assuming a client's floor state is authoritative.
- [ ] Update `tests/test_postal_art.gd` to check required skeletal joints and actual idle playback instead of the previous 24-bone count and rest-pose leg reset.
- [ ] Run the animation test, postal art test, then `tools/run_tests.py`. Inspect recorded gameplay at start/stop, fast turning, holding and release. Export with `tools/build_windows.py` only after validation.
- [ ] Update the paired art guides and READMEs with actual completed status. Commit only this task's changes to local Git; exclude the user's `project.godot` change and `art/sneezer` work.

## Delivery boundary

A successful browser preview proves neither a usable downloaded skeleton nor natural movement in the game. If usage reaches the requested stop threshold, preserve completed assets, record remaining steps here, and leave the playable version intact unless the replacement has passed validation.

Current blocker: the download event timed out and no local GLB arrived. The page asset bundler does not support GLB. Resume from local file receipt, verify the seven clips in Blender, then continue the unchecked steps. No gameplay files have been changed in this revision.

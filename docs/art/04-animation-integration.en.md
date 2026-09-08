# Worker animation integration result

[한국어](04-animation-integration.ko.md)

September 9, 2026 · Worker animation revision to the 0.9.0 art build. Every participant must use the same latest ZIP. Protocol remains 10.

## Source and authored result

The user-supplied `Downloads/worker-tripo-input.glb` contains 41 bones and seven motions: idle, walk, run, jump, fall, lift_heavy and hit_to_body_01. It is preserved separately as `art/tripo-01/worker-tripo-seven.glb` to distinguish it from the unrigged upload source with the same original name. Its size is 9,405,548 bytes. The paid Tripo asset is `ba747b8c-0476-4108-9a87-6823869224c1`; this local integration made no further generation requests beyond the previously recorded 20-credit rigging job.

Blender 5.2.1 LTS normalizes rest height to 1.65 m, forward orientation and floor origin. Root translation range is zero on all three axes in the seven source clips. The 19.5-second lift_heavy includes turns unsuitable for sustained carrying, so it is not directly connected. Six same-skeleton clips were authored: carry_idle, carry_walk, carry_run, air_rise, air_fall and throw, giving 13 exported clips. Airborne clips emphasize pose rather than adding another jump over the physics arc.

The editable source is `art/tripo-01/worker-animation.blend`, the reproducible preparation tool is `prepare_animation.py` in that folder, and the runtime model is `assets/art/release-01/worker.glb`. The previous model remains in Git history. Map, parcel and rat sources are unchanged in this revision.

## Runtime behavior

Idle, walk, run, carrying, airborne, hit and throw states select and blend motions. Movement speed controls step cadence; walking/running use separate entry and exit thresholds. Abrupt leg rest-pose resets and per-frame arm aiming are removed. Carrying hand and elbow adjustments are baked into the Blender clips.

Throw follow-through starts at the existing release decision, adding no input delay. Each worker's throw sequence uses the existing reliable metadata channel. Pose packet size is unchanged; metadata changes are checked at intervals of up to 0.2 seconds. Duplicate and stale snapshots do not restart playback; late joiners do not replay past throws. Older metadata without throw information can still be read. Existing host authority retains movement, collision and delivery decisions.

## Validation record

- [x] Confirmed the new animation test fails on the old model because required clips are missing.
- [x] Replacement passes idle, movement, carrying, throw recovery, airborne transitions, finite bone transforms and fixed visual-root checks.
- [x] Initial, duplicate and stale throw-sequence reception checks pass.
- [x] Inspected real Godot-renderer stills of idle, walk, run, carry, throw and airborne poses. Reproduce with `tests/capture_worker_animation.gd`.
- [x] Four-worker snapshot remains within 1,280 bytes after adding throw information.
- [x] Passed 35 behavior tests, 10 two-peer network scenarios and one four-peer network scenario. Two processes verify actual remote throw playback. Windows export and packaged launch checks passed.
- [ ] Human evaluation of carrying, sharp turns, landings and remote motion on separate PCs.

Logs and captures are stored in `artifacts`. Still images and automated tests do not guarantee natural motion in every gameplay situation. Dedicated full-body pickup and landing clips, directional strafing/backpedaling, and exact hand contact on varied parcels require further review. Pickup currently blends into carrying; landing returns to locomotion or idle. This does not constitute completed Steam release readiness.

## Usage stop rule

Stop work and notify the user when Codex has 1% or less remaining. The usage reset was verified at resumption; no reset credit was redeemed.

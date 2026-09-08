# Worker motion rebuild and continuous review

[한국어](06-motion-rebuild.ko.md)

## Confirmed causes

Recorded a continuous 16-second scenario in 0.9.1 and measured hips and feet across entire clips. Run Hip Z was approximately -0.913 m; idle/walk were approximately -0.086 m. A stationary Root concealed an 83 cm Hip-origin mismatch. The evaluated shoe mesh extended approximately 3–10 cm below the floor in idle, walk and run. Sideways/backward travel reused forward motion, and carrying wrists inherited walking-arm roll. Earlier clip-existence, finite-transform and still-image checks missed these defects.

## Rebuild scope

Preserve the original Tripo motions as references and rebake 18 clips on the same 41-bone skeleton. Center horizontal Hip position on the collision capsule at every frame, and correct floor penetration using the evaluated shoe mesh. Author carrying upper bodies from idle with fixed hand targets and bent elbows. Provide eight cardinal/diagonal carry directions, airborne carrying and carry idle. Bend airborne knees, use planted-foot knee compression for landing, and recover the arms after throw follow-through. Bake all output as linear 60 fps keys.

Empty-handed visuals face actual travel; carriers retain existing parcel aim direction. Match travel to measured stride: maximum empty-handed speed 3.2 m/s, forward carrying 1.6 m/s, sideways/backward carrying 1.2 m/s. Existing equipment/status multipliers remain. Preserve progress when switching loops and avoid interpreting the airborne apex as landing. Retain four crew colors and physical/delivery rules.

The editable source is `art/worker-motion-02/worker-motion.blend`; its reproducible builder is `rebuild_motion.py` in the same folder. Earlier rigging sources are not overwritten.

## Review sequence

- [x] Capture the previous 16-second sequence, frame samples and hip/foot measurements.
- [x] Reproduce old run-origin and missing directional-motion failures in a new test.
- [x] Import rebuilt model into Godot; test hips, hands, feet and direction changes.
- [x] Record the same continuous sequence and eight carry directions with the actual renderer; inspect frame sequences.
- [x] Run behavior/network regressions, launch the package and update both language guides.

Passing still-image or automated checks alone does not establish natural movement. Keep before/after footage and joint measurements together, distinguishing measured findings from judgments requiring human review.

## 0.9.2 verification results

Passed 37 behavior checks, 10 two-peer network scenarios and one four-peer scenario. Measured run Hip horizontal position in Godot remained within 0.001 mm of center. Blender evaluated-mesh checks found the minimum approximately 4 mm above ground in 12 samples each of idle, walk, run and carry walk. This does not guarantee foot planting on slopes or at every interpolated frame.

Recorded the main sequence and eight carrying directions through the actual renderer at 60 fps for approximately 16 seconds each and inspected transition frame sequences. Evidence is stored in `artifacts/motion-before.avi`, `motion-after.mp4`, `motion-directions.mp4`, `motion-after.json`, `floor-after.log` and `motion-full.log`. These are controlled visual reviews; human free-play evaluation remains separate. The original Tripo gait style remains; slope foot IK and hand IK adapting to individual parcel sizes are not implemented.

The Windows package is 0.9.2, with protocol 11 unchanged. Connect using the same 0.9.2 build. Editable sources and the reproducible builder are preserved in local Git.

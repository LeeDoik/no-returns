# Force-driven grabbing experiment plan

[한국어](08-force-grab.ko.md)

Reference Human: Fall Flat's public physics-puzzle and object-manipulation behavior without claiming to reproduce undisclosed internal constants. Sources: https://curvegames.com/our-games/human-fall-flat/ and https://nobrakesgames.itch.io/human . API: https://docs.godotengine.org/en/stable/classes/class_rigidbody3d.html .

1. Build a lab-only two-hand spring grip. Keep parcels dynamic without forcing position or rotation. Dampen velocity at each attachment point and cap force per hand.
2. Mouse buttons operate hands independently; looking up/down changes lift height. E toggles both hands; F applies a shove whose velocity change depends on mass. Retain the old method as a panel comparison mode.
3. Approximate reaction through horizontal worker motion. The worker remains a CharacterBody; full-body ragdolls, wall hanging and networking are outside this implementation. Force lines mark experimental attachment points, not actual hand bones.
4. Test no pickup teleport, preserved release velocity, force limits, dynamic falling, wall collisions and mass differences; inspect actual rendered output. Leave production unchanged.

## Delivered behavior and controls

Launch with the same `03_물리 실험실.cmd`. Force mode is the default; turn off the panel switch to compare legacy carrying. Press Tab to control the worker and approach a box. E attaches both hands or releases all. Hold left/right mouse buttons to attach the corresponding hand and release the button to let go. Look up to lift and down to lower. F releases and adds a shove impulse. Legacy mode also retains left-click throwing.

Defaults are 180 N/m spring stiffness, 18 Ns/m hand damping and 90 N maximum per hand. Two hands can supply at most 180 N, insufficient to suspend a 30 kg box, while a 3 kg box can be lifted. The force cap does not automatically scale with mass. A hand releases if attachment error exceeds 2.8 m. Off-center forces generate rotation without forcing box orientation. Throw impulse is `(forward + up × 0.35) × setting × 1.5` N·s, distinct from the legacy mode's speed setting. Ordinary release does not overwrite linear or angular velocity.

Horizontal reaction uses a virtual 25 kg worker mass, a 3 m/s speed cap and exponential decay rate 2 per second. This is approximate control combined with the existing character movement, not full momentum conservation or full-body simulation. Two-joint rotational arm IK aims at actual attachment points without stretching arm lengths; unreachable targets can leave a visible hand gap. Wrist orientation, finger contact and anatomical joint limits need further tuning. Yellow/teal lines show error between target and actual attachment points.

Verification passed: unchanged pickup position, unchanged release velocity, force cap, lifting 3 kg, inability to suspend 30 kg, and no traversal through a wall when pulling toward its far side. The basic lab checks also passed. Rendered a six-second two-hand → one-hand → throw sequence and inspected sampled frames. Evidence is `artifacts/force-grab.avi`, `force-grab.mp4` and `grab-integration-output.log`. Production models, physics and map settings were untouched. This does not constitute the original game's full-body stumbling, hanging or climbing.

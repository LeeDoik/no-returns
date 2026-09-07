# Physics review fixes

[한국어](physics-fix-report.ko.md)

Version 0.1 · 2026-09-07 · Completed within the first prototype.

Two P2 findings are fixed in `scripts/main.gd` and `scripts/depot.gd`:

- The throw preview now sweeps the actual 0.8 m cargo box through discrete physics steps from the cargo's current release position. Gravity comes from the project settings and cargo gravity scale. Linear/angular air damping explicitly use REPLACE with zero damping; collision rotation is locked. The ring indicates the **first contact**, is lifted 0.08 m from the contacting face, and aligns with its normal. It is hidden if no contact is found within three simulated seconds. Bounce and ground friction remain physical; the ring does not predict the final resting position.
- Held cargo sweeps its complete box toward the hand each host tick, stopping before static geometry with a 0.005 m query margin. Turning no longer rotates its collision box. Drop/throw preserves that validated location instead of teleporting to the hand again. If the player origin is more than 2.4 m from the cargo, holding ends at the existing location instead of dragging the cargo over a long distance. This uses the same distance reference as pickup, so valid pickups behind or beside the player remain held.

`tests/test_physics.gd` loads the real depot scene and simulates the actual rigid body. Before production changes, it failed five checks: held divider penetration, post-drop wrong-side position, distant holding, and floor/wall marker accuracy. Baseline marker errors were 3.5278 m on the floor and 0.2494 m at the back wall. The final test additionally checks each carried frame for full-shape overlap, turns immediately before dropping to detect release teleports, and exercises five throw directions/obstacles.

| Actual first-contact scenario | Measured marker error | Required maximum |
| --- | ---: | ---: |
| Floor, straight throw | 0.1734 m | 0.20 m |
| Floor, oblique throw | 0.1734 m | 0.20 m |
| Back wall | 0.0340 m | 0.20 m |
| Side wall | 0.0260 m | 0.20 m |
| Near divider | 0.0600 m | 0.20 m |

The comparison uses the actual body's center at its first `body_entered` signal, offset to the contacting face plus the ring's 0.08 m lift. The 0.20 m tolerance includes the discrete contact-reporting tick. These results cover GodotPhysics3D at the project's 60 Hz with the current static, axis-aligned depot geometry. They do not establish accuracy for moving obstacles, custom gravity areas, different physics engines, or network interpolation/latency. Free-flight rotation and air resistance are deliberately simplified for this first prototype.

A follow-up review caught a range regression: comparing cargo to the hand could cancel a pickup that was valid relative to the player. New real-scene cases place cargo 2.3 m behind and beside the player (vertical offset 0.36 m, total distance about 2.328 m). Before correction, both cases failed the ownership assertion on the first carry tick. The final pickup fixtures are clear of the divider so they also check that unblocked cargo reaches the hand. Changing the carry range check to the player origin made both cases pass while preserving divider and distant-release checks.

Run in PowerShell from the project root:

```powershell
& .tools/godot/Godot_v4.7.2-stable_win64_console.exe --headless --path . --log-file physics-green.log --script tests/test_physics.gd -- --role=physics
```

Result: `PHYSICS PASS: 0 failure(s)`, exit 0. Cargo rules tests and headless editor import also exited 0, with no script errors. This environment still emitted the known root certificate-store error and, for editor import, the `user://` profiler-directory error. Separate transport tests remain documented in the [prototype guide](01-first-playable.en.md).

API references: [Godot shape sweeps and contact queries](https://docs.godotengine.org/en/stable/classes/class_physicsdirectspacestate3d.html), [RigidBody3D damping and rotation controls](https://docs.godotengine.org/en/stable/classes/class_rigidbody3d.html).

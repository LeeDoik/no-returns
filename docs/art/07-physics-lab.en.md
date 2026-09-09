# Physics laboratory implementation plan

[한국어](07-physics-lab.ko.md)

> Latest revision: two-hand force grabbing is now the default. [Updated controls, physics and validation](08-force-grab.en.md) supersede the initial implementation below. Switch off force mode to compare legacy carrying.

Reuse production workers and parcels in a separate single-player test map. Provide drop, ramp, six-box stack and narrow corridor stations. Tune mass, friction, restitution, gravity, angular damping and throw speed at runtime; reset all objects for comparisons. Save only to user://physics-lab.json, with no automatic production changes. Display the physics backend and tick rate without changing them. Gravity tuning affects test parcels only; worker gravity stays identical to production.

Sequence: dedicated scene and controls; tuning panel and save/load; drop/reset/parameter checks; rendered review. Do not modify production code. This is a measurement tool for the current carrying implementation before redesign, not an online physics validation environment.

## Launch and controls

Run `03_물리 실험실.cmd` at the project root. In the editor, open `scenes/physics_lab.tscn` and run the current scene. Tab or Escape toggles the panel and character controls. WASD moves, mouse looks, Space jumps, E picks up/drops, left click throws, 1–4 teleports to stations, and R resets all trials. Confirm typed numeric values with Enter.

The panel tunes mass (kg), parcel/floor friction, parcel restitution, parcel gravity (m/s²), angular damping and horizontal throw speed (m/s). Vertical throw speed and worker gravity retain production values. Change one parameter at a time and restart trials. Loading saved values also resets the scene. Applying values to production requires a separate code change and regression checks.

The rendered session reports GodotPhysics3D at 60 Hz. Jolt comparison is not provided yet. As in production, held parcels stop free physics simulation; changing mass does not automatically create carrying weight. Compare mass through pushing and collisions. Free-fall speed under identical gravity does not depend on mass.

Verification: ten parcels created, 7 kg mass applied, falling observed over 45 physics frames, initial positions/velocities restored. The actual renderer showed the panel and all four stations. Saved settings are written into Godot's user-data directory, with failures reported in the panel. Human free-play, long-duration stack stability and online testing remain separate evaluations.

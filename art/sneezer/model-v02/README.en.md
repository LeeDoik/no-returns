# Sneezer — Dimensional version v0.2

[한국어](README.ko.md)

Status: reviewable 3D model. Game integration, collision, breath effects, and final optimization remain incomplete.

Includes [Blender source](sneezer.blend) and [animated GLB](sneezer.glb). Source timeline frames 1, 30, 48, and 90 show idle, warning, release, and recovery. The GLB animation is named `SneezeCycle`.

Retains v0.1's angular style and cardboard texture, adding shallow eye, eyebrow, and tooth relief, an approximately 10.5cm mouth cavity, and 8mm lid thickness. Angular cheeks protrude approximately 4.4cm in the warning state only. Cheeks appear through state switching rather than continuous inflation. The model totals 1339 triangles across all expression states. Facial features are thin dimensional pixel geometry rather than a texture.

Actual renders of all three states: [idle](previews/idle.png), [warning](previews/warning.png), [sneeze](previews/sneeze.png). The exported GLB was reimported to check exclusive facial states and lid rotation. [Import checks](import-validation.json) and [GLB structure](glb-validation.json) are retained. In-game appearance and performance remain unverified.

The earlier model is preserved in `../model-v01`. No other session's game files or open Blender scene were modified. Rebuild order: `build_model.py` in Blender, `finalize_glb.py` in Python, and `verify_import.py` in Blender. Generation depends on the v0.1 Blender source.

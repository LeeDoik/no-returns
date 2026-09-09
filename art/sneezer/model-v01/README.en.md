# Sneezer — 3D model v0.1

[한국어](README.ko.md)

Status: reviewable 3D model completed. Godot integration, breath effects, collision, and final optimization are incomplete. The user-approved concept is [reference-informed PS1 v0.2](../concepts/sneezer-states-ps1-v02.png). Only the video's graphics informed the asset; its horror, characters, and setting were not adopted.

## Files and use

- [sneezer.blend](sneezer.blend): editable Blender source. Timeline frames 1, 30, 48, and 90 show idle, warning, sneeze, and recovery. Play the timeline to inspect facial switching, squash, and lid motion.
- [sneezer.glb](sneezer.glb): game export with embedded texture. Animation name: `SneezeCycle`. 30fps, frames 1–90; the actual key span is approximately 2.97 seconds.
- `sneezer-idle.glb`, `sneezer-warning.glb`, `sneezer-sneeze.glb`: static state exports. Unused facial states remain at zero scale and can be removed during final optimization.
- [Idle](previews/idle.png), [warning](previews/warning.png), [sneeze](previews/sneeze.png), [GLB reimport verification](previews/glb-reimport-sneeze.png): actual Blender renders. Preview lights and ground are excluded from GLB exports.

## Construction and scope

Modeled directly in Blender. Facial features use thin pixel-shaped faces immediately in front of the box surface instead of sculpted features. This version does not use a facial texture atlas. Body, lids, tape, label, and three facial states remain separately editable. Animation uses rigid object transforms with no skeleton. All facial variants together total 267 triangles. Rest dimensions are approximately 0.8 × 0.72 × 0.76m, with origin at the bottom center. Front is -Y in Blender and +Z in glTF; align the model when placing it in the game.

Cardboard was newly generated with the built-in GPT Image tool. The file is [cardboard.png](textures/cardboard.png), with an actual resolution of 1254 × 1254. It has coarse pixel-like detail; it is not an actual 64–128px texture or verified PS1 hardware asset. The image is packed into the source and embedded in the GLB. Stains and paper fibers are simpler than in the concept.

## Verification

Rendered and visually reviewed all three states. Reimported the GLB into a scene cleared of model objects, checked that exactly one facial state is active at each sampled time, checked lid rotation, and rendered the imported result. Evidence: [model metrics](build-report.json), [GLB structure](glb-validation.json), [reimport checks](import-validation.json). Readability, materials, performance, and synchronization with gameplay rules have not yet been checked in an actual Godot play session.

No game code, maps, scenes owned by the other session, or open Blender window were modified. A separate background Blender process created this asset.

## Rebuild

Run `build_model.py` in background Blender, then run `finalize_glb.py` with the bundled Python. Finally run `verify_import.py` in Blender. Running the build script alone produces multiple GLB animation clips, so also run the finalization step.

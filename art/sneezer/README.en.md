# Sneezer — Design concept v0.2

[한국어](README.ko.md)

Status: proposal for review. Final design, 3D model, and game integration are incomplete.

## v0.2 — Absurd comic expressions

The [revised concept](concepts/sneezer-states-v02.png) adds vacant half-closed eyes, visible front teeth, and a crumpling box with a lifting lid during the sneeze. This proposal increases the comedy as requested by the user. The built-in image generation tool edited v0.1; the [exact generation prompt](concepts/generation-v02.json) is saved. Visual review shows distinguishable states, but tooth shapes are not fully consistent between states and should be unified during modeling. The v0.1 record is preserved below.

The [concept sheet](concepts/sneezer-states-v01.png) shows the idle, pre-sneeze, and sneeze-release states from left to right. It was made with the built-in image generation tool. The exact English generation prompt is in the [generation record](concepts/generation-v01.json).

The brown cardboard box, paper tape, large eyes and eyebrows, and small nose remain consistent. Squinting eyes and inflated cheeks signal the sneeze warning; an open mouth and pale blue breath convey release. There are no arms or legs.

Visual review: the box and tape placement are consistent across the three states, and the expressions are distinguishable. The cheeks and mouth have complex volume that should be simplified for the game model. The breath effect on the right is close to the image edge. This image is not a texture or facial sprite ready for direct use.

Next step: after design review, create the model and expression deformations in a separate Blender file, then check readability at the actual gameplay camera distance. No code, maps, or scenes owned by the other session were modified.

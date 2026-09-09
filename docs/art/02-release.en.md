# 0.9.0 Art integration guide

[한국어](02-release.ko.md)

> Documentation update: versions and values here belong to this file's original context. Check the [documentation home](../README.en.md) first for current rules and outstanding work.

## September 9 worker animation revision

The worker now uses a 41-bone, 13-clip model. The 24-bone walk model and performance figures below describe the initial 0.9.0 release, not a measurement of the replacement. The current editable worker source is `art/tripo-01/worker-animation.blend`. [Motion integration and remaining review](04-animation-integration.en.md).

## Run and inspect

Run `NO_RETURNS.exe` in the Windows package. In the development folder use `00_게임 실행.cmd`, `PLAY.cmd` for the latest source, or `EDIT.cmd` for the editor. Close older instances first. Co-op participants must use the same 0.9.1 build and saved map. Protocol 11 and the map identity check remain in place. The [carrying, physics and crew-color revision](05-carry-physics.en.md) supersedes the original art record below.

Inspect the four parcels and worker in solo practice. Sneezer raises its eyes during warning, shuts them during the burst and opens its lids. Clinger suction cups and Hopper shoes distinguish their silhouettes. Six cushion cells deflate and the spring plate launches upward. The eighteen-sheet flight and reset remain intact. Campaign Packrat uses a new model with moving paws and tail.

## Produced in this change

- Higgsfield: worker, Packrat and facility reference images; cardboard, painted steel and concrete surface sources; textured worker and rat 3D models.
- Blender 5.2.1 LTS: scale and orientation cleanup, removal of an unrelated controller mesh, preservation of the worker's 24 bones and walk animation, and a new six-bone paw/tail rig for Packrat.
- Blender kit: Standard, Sneezer, Clinger and Hopper parcels, workbench, shelf, conveyor module, dispatch port, return spring, packing cushion and pendant. There are thirteen runtime models including the characters.
- Map: new art on seven workbenches, eight shelves, eight conveyor modules, two dispatch ports and existing lamps. Floor materials and subtle wall relief were added. The paths, transforms, dimensions and enabled states of all 171 colliders match the previous version.
- Rendering: distance-filtered textures and 4× multisample antialiasing reduce shimmer and jagged edges. Art follows existing authoritative gameplay and replicated state.

Images were requested with GPT Image 2 at `high / 4k`. Actual square outputs are **2880×2880**; the facility reference is **3840×2160**. Square images were neither labeled as 4096 nor upscaled. Surface sources were made seamless and used to derive normal, roughness and height maps. The current game uses base color and subtle normals; the remaining maps are retained for later material tuning. Nano Banana Pro generation was unavailable under the account plan. No subscription was changed.

## Editable files

`art/release-01/source/postal-kit.blend` contains the facility and parcel sources. `worker.blend` and `packrat.blend` contain character sources. Original GLBs and generation IDs are retained under the production folder's `source` and `reports`. Earlier `art/sneezer` sources were preserved.

The game reads `assets/art/release-01`. Exporting a replacement GLB with the same name triggers Godot reimport. Move `Art_` nodes in `scenes/maps/shipping_shrine.tscn` to position workbenches, shelves and dispatch ports. Adjust the existing editable blocks separately when changing collision. Moving art alone does not move its collider.

Reactive props retain their `kind`, trigger range and reset time properties. Their generated models are previews: do not duplicate them into saved children. `apply_postal_art.gd` and `refine_postal_art.gd` are already-applied, one-time tools, not regular play or build steps. The map is not regenerated automatically. The entire production folder is excluded from distribution.

## Validation and remaining release work

The art-specific check covers face transitions, lids, six cushion cells, spring plate motion, character rigs, planted feet at rest and orphan nodes after teardown. Existing co-op, collision, delivery, paperwork and online regressions are used alongside actual-renderer captures. Results are retained in `artifacts`.

Rigid parts sharing a material and pivot were batched, and shadows were removed from thin printed surfaces. In a 180-frame receiving-room sample on an RTX 4070 Laptop at 1152×720 with four workers, median frame time fell from 24.8ms to 16.4ms and the 95th percentile from 27.8ms to 17.6ms. This short sample on one PC does not establish worst-case whole-map or low-end performance. `batch_postal_art.gd` and `light_postal_art.gd` are also already-applied, one-time optimization and lighting tools.

This is a **development build transitioning to production art**, not a finished commercial release with every background prop replaced. Some walls, props and effects retain prototype geometry. Worker hand contact and rat foot sliding still warrant refinement across varied play situations. External playtesting, low-end performance measurement, Steam invitations, store preparation and account review are not marked complete. Reference images are distinguished from actual game captures and must not be used as game screenshots.

# FLATBED 3D production and review

[한국어](ship-production.ko.md)

2026-09-14 · **First integrated model / quality-review candidate.** Generated the approved A in Tripo and assembled the cabin and entrance mechanism in Blender. Current Unity play scenes and game code were not changed. This does not represent final release approval or 4-player play validation.

## Deliverables

- [Integrated Blender source](../../art/ship-flatbed-01/Flatbed_Integrated.blend)
- [Integrated GLB](../../art/ship-flatbed-01/Flatbed_Integrated.glb) · [Integrated FBX](../../art/ship-flatbed-01/Flatbed_Integrated.fbx)
- [Original Tripo FBX](../../art/ship-flatbed-01/source/NR_Flatbed_Tripo_Source.fbx)
- [Exterior render](../../art/ship-flatbed-01/review/integrated-front.png) · [Open rear](../../art/ship-flatbed-01/review/integrated-rear.png) · [Interior render](../../art/ship-flatbed-01/review/integrated-interior.png)
- [Geometry inspection](../../art/ship-flatbed-01/integrated-validation.json) · [Reimport inspection](../../art/ship-flatbed-01/roundtrip-validation.json)

## Production and actual changes

1. Built exterior/interior structural geometry in common Blender coordinates.
2. The first Tripo single-image input used the entire 4-view sheet and produced four combined craft; it was rejected. Prepared the same A as a single perspective and regenerated it. Reference preparation used the built-in image generation tool.
3. Generated Tripo Smart Mesh and texture, then downloaded FBX. Preserve the source. Generation ID: c1864948-b96d-483d-b31d-c00dfca0bba9. Tripo usage was 55 + 65 + 20 = 140 credits; the observed remaining balance was 750.
4. Automatic Boolean cuts produced unwanted caps and were rejected. Planar splits cleared the cabin and rear entrance; original window faces received glass material.
5. A 6.4m interior protruded through the hull. The integrated model scales it by 0.68 to 4.352m. The earlier 47.04 square metre area and 6.4m interior layout calculations do not apply to this model.
6. Separated the 3m ramp from the 2.4m-tall door. Draft keyframes move doors sideways and stow the ramp below the floor. Blender frame 1 is closed and 40 is open. FBX is a static open state; GLB includes animation.
7. Distributed 4 seats along the sides and reused 4 approved Tripo racks. Consoles and some cabin finishes remain simple production geometry; they are not evaluated as equally complete as the exterior.

## Review and outstanding work

Rendered exterior, rear and interior from the same model; directly identified and corrected duplicated generation, exterior intrusion and Boolean caps. Checked missing UVs and invalid coordinates. Corridor and reimport results are recorded in the linked JSON. Automated checks do not validate carrying feel or fun.

- [x] Generate/download a single Tripo hull and preserve its source.
- [x] Integrate cabin, doors and ramp in Blender and inspect repeated renders.
- [ ] Final user quality approval of full window transmission, carrying visibility and finish boundaries.
- [ ] Validate interference over the complete door/ramp motion and floor contacts.
- [ ] Finish interior surfaces, console screens and equipment sockets to release quality.
- [ ] Validate Unity 4-player carrying, collisions, performance and material parity.

## Reproduction

[build.py](../../art/ship-flatbed-01/build.py) creates the initial dimensional structure; [integrate.py](../../art/ship-flatbed-01/integrate.py) applies current integration changes. Run them in order with Blender 5.2.1. [verify.py](../../art/ship-flatbed-01/verify.py) reimports and checks the integrated file. Use GLB or FBX for game import and preserve independent door/ramp objects. Play-scene integration was not completed in this task.

[Consistency review S01–S08](ship-review.en.md) · [Earlier interior proposal](ship-interior.en.md).


### Window-sill correction

The actual exterior sill required revising the single-level floor proposal. Raise the cockpit floor from 0.6m to 1.2m, with 3 steps of 0.2m rise each. Cargo floor remains at 0.6m. Render the cockpit at floor 1.2m + eye height 1.57m = 2.77m. This production candidate supersedes the earlier common-floor proposal. Carrying over the steps needs Unity validation.


Before publishing, run [finish.py](../../art/ship-flatbed-01/finish.py) to bake the shared 512px cabin paint material and remove reference mannequins from exports. Reproduction order is build → integrate → finish → verify. Cabin paint is a procedural Blender material, not an edit of an external image.


Final automated checks: 48,906 triangles matched before/after GLB reimport, 0 missing UVs, 0 invalid coordinates, all 6 cargo-aisle ray samples passed. Point/line samples do not guarantee whole-body or cargo-volume collision clearance. Bilingual document link, checkbox and numeric parity checks passed.

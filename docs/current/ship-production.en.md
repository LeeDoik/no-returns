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


## 2026-09-14 — Angular exterior candidate

User feedback: the exterior is too rounded. [Angular Blender](../../art/ship-flatbed-01/Flatbed_Angular.blend) · [GLB](../../art/ship-flatbed-01/Flatbed_Angular.glb) · [FBX](../../art/ship-flatbed-01/Flatbed_Angular.fbx) · [Front](../../art/ship-flatbed-01/review/angular-front.png) · [Rear](../../art/ship-flatbed-01/review/angular-rear.png).

[angular.py](../../art/ship-flatbed-01/angular.py) reads the integrated file and applies limited 12-degree face dissolution and flat shading only to the hull. Glass material boundaries and open boundaries are not dissolved. Hull triangles change from 10,962 to 10,562; the complete GLB has 48,506 triangles, matching reimport. There are 0 new vertex positions; this does not redesign the outer dimensions. The candidate preserves the basic silhouette while exposing face boundaries, rather than fully replacing curved corners with rectangular corners. Cabin and door/ramp structures were not edited. Original Integrated files remain available.

[Validation](../../art/ship-flatbed-01/angular-validation.json): 0 missing UVs and 0 invalid coordinates. Directly compared front and rear renders using the same camera and lighting. Unity integration, actual controls and user art approval remain incomplete. No additional Tripo credits were used.


## 2026-09-14 — Interior design 03 visual proposal

![Interior design 03](../../art/ship-concepts-01/interior-design-03.png)

Created an interior image proposal using the angular exterior and existing cabin render. **This is not a new 3D implementation and awaits user selection.** Used the built-in image generation tool; no Tripo credits were used.

- One board compares the rear-to-front overview, cockpit consoles and crew/equipment area.
- Unify off-white angular metal panels, dark flooring, 4 orange seats and restrained cyan screens. Propose cargo racks and tie-down/floor rails, left beacon/right baton docks, ceiling-edge cables, rear hazard markings and handles.
- Propose a short incline instead of the existing stairs. The initial generation appeared to end against the console and was held. Split consoles left/right to reveal an upper standing work area.
- Review: visually confirmed 4 seats, 3 windshield panes, a clear central aisle, and console/equipment placement. Screen copy on the sheet is illustrative, not approved game copy.
- Unverified: incline length/angle/rise, actual furniture/body fit within roughly 4.35m width, headroom and exterior window coordinates, equipment reach. Perspective imagery is not measurement evidence. The 3D model still retains the existing stairs.

[Initial generation — held for console access issue](../../art/ship-concepts-01/interior-design-03-draft.png). After image selection, revalidate structural dimensions and apply them to the model.

## 2026-09-14 — Interior design 04 and rear entrance review

![Interior design 04](../../art/ship-concepts-01/interior-design-04.png)

At the user's request, removed the interior ramp and floor step and placed a large recessed contract and route display centrally below the front windshield. This supersedes the ramp proposal in design 03. The side auxiliary screens, 4 seats, equipment docks and cargo racks remain. Visually confirmed a flat aisle and large display in the image. Screen copy is illustrative. **Only the concept image changed; the actual 3D model retains its stairs.** Window sill height and sightlines, equipment reach and human-scale fit need renewed validation when changing the model.

Reviewed the [open rear entrance](../../art/ship-flatbed-01/review/angular-rear.png). A central door and exterior boarding ramp exist between the engines. Removing the interior ramp does not remove this exterior ramp. The [structural source](../../art/ship-flatbed-01/build.py) specifies a door 3m wide and 2.4m high; the [hull cut](../../art/ship-flatbed-01/integrate.py) spans 3.06m in width and 2.30m in height. These are design and cutting dimensions, not measured final clearances. The central rear engine geometry visually overhangs the entrance; full employee and cargo volume clearance through the frame, hull and engine, open-door interference and ground connection remain unverified. Render and source inspection alone do not guarantee unobstructed passage. No rear structure changes or Unity tests were performed.

## 2026-09-14 — Rear view from the cockpit

The [reverse-angle image](../../art/ship-concepts-01/interior-design-04-rear.png) visualizes the open rear exit from the cockpit using design 04 materials and flat floor. Visually confirmed the baton on image left, beacon on image right, 4 seats, a central aisle and an exterior boarding ramp. The large central display is behind the camera. This generated image does not establish exact rack depth order or numbered seat positions as a reverse render of the same model. Entrance height, engine interference and cargo clearance remain unverified; this concept does not close those issues. No code, model or Unity changes.

## 2026-09-14 — Interior production approach

Added [interior production basis, parts and gates](ship-interior-pipeline.en.md). Based on design 04, use Blender for dimensioned structure, individual production/cleanup for necessary major parts, and Unity for functional assembly. Current work is source inspection and preparation; no new model or game integration.

## 2026-09-14 — Interior structure trial 01 progress

2026-09-14 latest: [Structure trial 03](ship-interior-trial.en.md). Cabin height 3.12m with Space empty-hand jumping. No head collision at 9 jump positions; 3 passage routes and Windows build passed. Obtain human spatial review before art production through the [new pipeline](art-structure-first.en.md).

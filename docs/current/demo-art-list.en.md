# CINDER DEPOT — one-cycle demo and 3D asset list

[한국어](demo-art-list.ko.md)

Current on 2026-10-03: build the [employee boot repair and first deformation rig](03-guides.en.md#2026-10-03--employee-boot-repair-and-first-deformation-rig). The ACT01 candidate has 4,765 vertices, 4,888 faces, 9,118 triangles, a 4K texture, 1.8m height and 53 bones. Split 2 boot junction edges and adjust pad rims/weights. Complete Blender pose trials and FBX reimport; extreme-pose polish, Unity Avatar, idle/walk, actual cargo contact and user quality remain pending. ACT02 arms have not been derived.

The proposed [initial topology settings](03-guides.en.md#2026-10-03--initial-employee-topology-settings) for employee ACT01 are Quad/5,000 faces (approximately 10,000 triangles if all faces are quads). Set the final budget after the first deformation/game checks and derive ACT02 arms from the same source. These are not measured or completed model-production values.

## 2026-10-03 — Deliver individual view images

At the user's request, add [3 separate front/side/back PNGs](03-guides.en.md#2026-10-03--separate-employee-view-images). Prepare the front as a single-character T pose; distinguish completed image generation from appearance approval and Tripo/rig/game validation. ACT01/ACT02 models remain incomplete; retain game 0.9.4 and protocol 13.

## 2026-10-03 — Player appearance concept generated

Add [employee concept 02, original and generation record](03-guides.en.md#2026-10-03--employee-image-concept-02), an appearance-review image with front/back T poses, a side view and team-color swatches. Update only the not-yet-generated image status from 2026-10-02 below; ACT01/ACT02 models, rigs, motions and user appearance approval remain incomplete. Retain game 0.9.4 and protocol 13.

## 2026-10-02 — Proposed player model and animation workflow

This is **research and a recommendation** for the user's GPT Image concept → Tripo model workflow. The current game is [0.9.4, protocol 13](cinder-suppression.en.md); this task generated or integrated no image, model, rig or clip. Current integration specifications take precedence over historical demo status below. Retain ACT01's 1 full-body model with 4 team colors and ACT02's first-person arms/hands.

Recommended order: **GPT Image production reference → Tripo appearance generation → Blender mesh inspection/necessary cleanup → Mixamo rig/basic motions → Blender custom-motion adjustment → Unity integration**. Rigging attaches bones and deformation weights to a model; animation moves those bones. Automatic rigging does not complete gameplay-state integration.

- Prepare a symmetrical full-body T pose on a simple background, with arms clear of the torso and legs apart. Match proportions/gloves/boots across front, side and back views, separating actual input images to suit the selected Tripo input mode. Do not generate the character holding cargo or a baton. The existing employee sheet is an unapproved reference; obtain approval for the new appearance before 3D production.
- In Blender, inspect shoulder/elbow/knee deformation topology, fingers/gloves, units, normals and UVs. Do not finalize the generated mesh unchanged; correct the areas with deformation problems.
- Use Mixamo as the default rigging route. If an existing Tripo rig passes Unity Humanoid and deformation checks, retain it and retarget basic motions. The official Tripo API's `mixamo` option specifies compatible bone names, not a verified Unity result. Keep one canonical skeleton; do not auto-rig the same model twice. Prefer FBX for Unity import.

| Required action | Recommended production method |
|---|---|
| Idle, forward/back/side movement, empty-handed jump | Select Mixamo candidates and match actual movement speeds/transitions. Test playback-speed adjustment for quiet movement first. |
| Parcel/beacon pickup, carrying and placement | Blender upper-body base pose + Unity hand placement (IK). Combine with locomotion and provide object-specific grip targets. |
| Baton use | Adjust a short arm motion to the current immediate strike/recovery presentation. Do not display it while carrying cargo/a beacon. |
| Down, rescue and getting up | Start with a fixed down pose, short entry/recovery and a rescue hold motion. Current rescue is holding E for 2.5 seconds, not carrying/dragging teammates. |

Current [CarryRoom](../../NoReturns/Assets/_NoReturns/Runtime/CarryLab/CarryRoom.cs) uses a 1.8m collider, 1.57m eye height and placeholder box-based bodies, hiding the local full body. Existing CharacterController/host state owns movement, so propose **in-place motions with `Apply Root Motion` disabled** as the integration baseline. Drive presentation from actual movement and carrying/down/rescue state; the model must not own movement, collision or cargo ownership. Follow existing [baton presentation](../../NoReturns/Assets/_NoReturns/Runtime/CarryLab/BatonVisual.cs) and [rescue rules](../../NoReturns/Assets/_NoReturns/Runtime/CarryLab/CarryThreat.cs), without introducing new gameplay adjudication through animation events.

Derive ACT02's arms/gloves from the same full-body source and adjust motions for the camera. Initially retain the validated full Humanoid skeleton/Avatar and separate only the visible mesh. Check the full body seen by teammates and local first-person arms separately; all 4 team colors share the model, rig and motions. Validate built-in Humanoid IK first, without installing a new rigging package.

Current cargo distance is 0.75–1.6m and rotation is adjustable. Rigidly following grip targets at long distances/large rotations can stretch or twist arms. **Do not assume both hands can maintain contact across the entire range.** First establish reachable grips, then evaluate reducing contact correction and blending to the base pose outside that range. Do not parent cargo to hand bones or arbitrarily reduce existing controls.

The first validation slice is **1 character, idle, movement and parcel carrying**. Check shoulder/elbow deformation, hand reach, first-person visibility and remote full-body presentation before completing baton, rescue and jump motions. Exact clip count, automatic rigging success and visual quality remain unconfirmed. Follow the [production backlog](04-backlog.en.md) and [validation criteria](05-validation.en.md).

Official capabilities checked on 2026-10-02: [Adobe Mixamo — humanoid rigging/mesh requirements](https://helpx.adobe.com/ph_en/creative-cloud/faq/mixamo-faq.html), [Tripo Auto Rig API — bone names/FBX output](https://developers.tripo3d.ai/en/docs/animations-rig), [Unity Humanoid retargeting](https://docs.unity3d.com/6000.0/Documentation/Manual/Retargeting.html), [Unity IK](https://docs.unity3d.com/6000.0/Documentation/Manual/InverseKinematics.html), [Root Motion setting](https://docs.unity3d.com/6000.0/Documentation/ScriptReference/Animator-applyRootMotion.html). Distinguish official capabilities from the project recommendations above. When importing external clips, verify provenance, usage terms and terms for public redistribution of source assets.

## 2026-10-01 — Cinder delivery, receipt and return settlement

[CINDER-DELIVERY-01 usage, coordinates and evidence](four-player.en.md#2026-10-01--cinder-delivery-receipt-and-return-settlement). In separate `CinderFourPlayerTest`, connect ship E preparation/arrival → BAY 04 floor delivery → CRT receipt E collection → all crew aboard/E return/420 CR settlement → next shift. Retain version 0.9.1, protocol 10 and TCP 27842. Reuse the existing delivery ledger, parcel, CRT, KO/EN screen and label tooling. Preserve the source environment scene; supersede older unconnected-delivery/receipt statements below only within this test scope. Default launch is delivery; `start --map-only` and `check` retain the movement test. Listener/baton/suppression/beacon/save and four-human/other-PC/performance validation remain incomplete. Actual automated evidence is in the [validation record](../validation/cinder-delivery-01.json).

## Priority for current Cinder production — 2026-10-01

Current environment-asset work follows the [first 8 warehouse-unit brief](cinder-asset-prep.en.md). The 44 units, R 10/N 34 labels and CSV below describe the 0.9.1 demo survey from 2026-09-13, not current Cinder completion or reuse-approval counts. Prepare the first batch again as new production candidates, including a new sign surface unit. Preserve the existing list, CSV and historical integration evidence.

2026-09-13 · Audit/proposal against 0.9.1. Opened and inspected the source image after removing its leading path slash. The user targets the reference space and a complete demo cycle. This is a production plan, not individual appearance approval, completed production or release-quality certification.

## Demo scope

Prepare aboard → select route/auto-arrive → carry 1 sealed parcel → choose sheltered detour/risky shortcut → respond to Listener/optionally inspect clue → deliver → collect receipt → return/settle → buy and physically pick up beacon → prepare next contract. End with the purchased tool visibly aboard and available for the next departure; completing a second delivery is optional.

Propose 12~18 minutes for first success and 7~10 minutes on site. Current suppression thresholds of 90/135/180 seconds and intrusion at 188 seconds do not fit this unchanged; tune after measuring routes. No values change here. Retain indirect light/audio/relay cues instead of a countdown.

Focus on 1 destination, 1 sealed cargo type, 1 Listener type, 1 outer creature type, starting baton/purchasable beacon and 1 focal clue. Preserve the existing 2 clues while staging 1 as the first curiosity hook. Exclude free flight, whole-planet exploration, procedural generation, new weapons and large shop expansion.

## Production quantity

44 production units: 10 existing models for reuse/improvement review and 34 proposed new units. These are not placement/color-variant counts. Kits may contain connector pieces, so this is not an FBX-file or individual-mesh count. Review duplicates/facility candidates are not counted as additional finished assets. Selected FBX presence and integration documents were checked, not every mesh/UV re-audited. Current employees/creatures use placeholder geometry.

P0: core space/function integration. P1: route/facility density finishing. P2: small atmosphere props. All are demo candidates; validate P0 first. R means existing model for reuse/improvement review; N means proposed new production.

| ID | Category | Production unit | Status | Priority | Requirement |
|---|---|---|---|---|---|
| ENV01 | Structure | Straight wall | R | P0 | Back, top and seams |
| ENV02 | Structure | Wall corner | R | P0 | Inner/outer corner assembly |
| ENV03 | Structure | Standard doorway | R | P0 | Cargo clearance |
| ENV04 | Structure | Floor tile | R | P0 | Improve seams and repetition |
| ENV05 | Structure | Wall end cap | N | P0 | Cover exposed ends |
| ENV06 | Structure | T junction | N | P1 | Consistent branch thickness |
| ENV07 | Structure | Ceiling/beam module | N | P0 | Finish first-person overhead view |
| ENV08 | Structure | Freight doorway | N | P1 | Bay 04 entrance silhouette |
| ENV09 | Structure | Shutter leaf | N | P1 | Separate frame and travel axis |
| ENV10 | Structure | Stair/ramp module | N | P1 | Step height and cargo collision |
| ENV11 | Structure | Railing | N | P1 | Edges and machinery boundaries |
| FAC01 | Facility | Work lamp | R | P0 | Separate emissive face and light |
| FAC02 | Facility | Cargo rack | R | P0 | Empty configuration and gap collision |
| FAC03 | Facility | Receipt terminal | R | P0 | Retain CRT and receipt slot |
| FAC04 | Facility | Suppressor body | N | P0 | Parts for 4-stage light/vibration |
| FAC05 | Facility | Relay post | N | P0 | Outer-entry boundary cues |
| FAC06 | Facility | Red warning beacon | N | P0 | Unity-controlled flashing |
| FAC07 | Facility | Pipe/cable kit | N | P1 | Straight/bent pieces sharing a profile |
| FAC08 | Facility | Vent/service panel | N | P1 | Wall density and variation |
| PRP01 | Props | Sealed parcel | R | P0 | Front/back, visibility and grip |
| PRP02 | Props | Lure beacon | R | P0 | Ship purchase spawn and pickup |
| PRP03 | Props | Display-equipped baton | R | P0 | Repaired shell, charge screen and tip |
| PRP04 | Props | Static storage crate | N | P1 | Distinguish from deliverable cargo |
| PRP05 | Props | Industrial drum | N | P2 | Cover/density; no destruction feature |
| PRP06 | Props | Pallet | N | P1 | Stack height and simple collision |
| PRP07 | Props | Office workbench | N | P1 | Recently used workstation |
| PRP08 | Props | Chair | N | P2 | Abandoned seat |
| PRP09 | Props | Cup | N | P2 | Steam uses separate VFX |
| PRP10 | Props | Document/log bundle | N | P1 | Flat mesh and text texture |
| SHP01 | Ship | Ship hull | N | P0 | Return anchor and entrance |
| SHP02 | Ship | Landing leg | N | P1 | Repeat one master mesh |
| SHP03 | Ship | Boarding ramp | N | P0 | Cargo and 4-player entry clearance |
| SHP04 | Ship | Cargo cabin shell | N | P0 | Support interior with facility kit |
| SHP05 | Ship | Route/supply console | N | P0 | Shared route/shop/report screen |
| ACT01 | Characters | Employee full body | N | P0 | One model with 4 team colors |
| ACT02 | Characters | First-person arms/hands | N | P0 | Match full-body gloves/sleeves |
| ACT03 | Characters | Listener | N | P0 | Investigate/windup/stun silhouette |
| ACT04 | Characters | Brown A outer creature | N | P0 | Keep selected A; exclude discarded white creature |
| EXT01 | Exterior | Site ground | N | P0 | Replace flat-color exterior ground |
| EXT02 | Exterior | Large cliff rock | N | P0 | Boundary and outer silhouette |
| EXT03 | Exterior | Medium rock | N | P1 | Rotate/scale to hide joins |
| EXT04 | Exterior | Rubble cluster | N | P2 | Off-route dressing without extra collision |
| EXT05 | Exterior | Outer barrier gate | N | P0 | Intrusion location and suppression failure |
| EXT06 | Exterior | Distant facility silhouette | N | P2 | Non-explorable backdrop |

## Mapping the 7 reference zones

| Reference | Assets to assemble | Play purpose |
|---|---|---|
| 01 landing/return | SHP01~05, ENV04/07, FAC01 | Readable preparation, purchase and return flow |
| 02 storage detour | ENV01~07, FAC01/02, PRP04/06 | Longer, occluded but cargo-safe route |
| 03 sorting yard | ENV08~11, FAC02, PRP04~06, ACT03 | Observe/lure/stun Listener and choose shortcut |
| 04 reception | FAC03, ENV08/09, PRP01 | Floor placement → CRT response → receipt from slot |
| 05 side office | PRP07~10, FAC08, SHP05 material variant | Recent use in an empty facility; console silhouette can be reused |
| 06 suppression | FAC04~08 | Visual/audio anchor for 4 stages in the same space |
| 07 outer entry | EXT01~06, FAC05/06, ACT04 | Spatial advance warning of boundary danger |

Teal/ochre route arrows and numbers are explanatory graphics, not in-game 3D assets. Use architecture, signage and lighting to convey routes. Prioritize first-person doorways, corners, ceilings and backs over a layout that only looks good overhead.

## Work beyond models

- Materials: shared painted metal, worn edges, flooring and rock families. Keep warm work lights, cold CRTs and red alarms consistent. Do not mix unrelated generated textures unchanged.
- 2D/surfaces: reception floor marking, Bay 04 signs, cargo labels, dirt/trace decals, receipt face, route/shop/report screens and employee face display. Do not duplicate a model for every state.
- Animation: employee idle/move/carry/down/rescue; first-person pickup/drop/baton/rescue; Listener locomotion/investigation/windup/attack/stun; outer entry/pursuit/attack. Define clips and grip contacts before modeling; finalize exact clip count after rig validation.
- VFX/audio: baton electricity, suppression transitions/shutdown, warning lights, reception scan/printer, engine/footsteps/creature warning/ambience. Fog, grading, low-resolution presentation and lighting are separate work.
- Collision/technical: floor-centered pivots, real units, simple doorway/pallet collision, hand/cargo sockets, separate emissive and CRT material regions. Use material/animation variants for 4 crew colors and moving parts.

These are not included in the 44 model production units. They and consistent placement are required to achieve the reference quality.

## Existing code and remaining implementation

| Area | Current evidence | Demo work remaining |
|---|---|---|
| 4-player carry/rescue/delivery | 0.9.0~0.9.1 real-process checks | Repeat after art integration and test with 4 humans |
| Contract/return/purchase/save | CarryMission, CarryRoom, CarrySave | Natural ship-console onboarding and first-purchase finish |
| Pay | Standard 300+120=420, beacon 120 | Verify 300 remaining after purchase, restart restore and next-departure use |
| Higher risk | Existing risk contract after first success | Explain risk/choice without adding another destination |
| Creatures/suppression | CarryThreat, CarrySuppression | Actual rigs/audio, route-measured timings and readable attack cues |
| Map | Current 32×44m experiment and facility kit | Reassemble reference zones for first-person carrying |
| Failure | Undelivered abort/all-down recovery/disconnect rules | Failure/retry presentation and disconnect-abuse review |

Pay and timing above are current code values, not a new economy. Present the first purchase as tool-license expansion, without adding a stat-upgrade tree. Use separate test saves for first-time checks, never reset the user's actual progress. Do not finalize the mystery identity or ending.

## Production order and completion gates

1. Connect ship → short corridor → reception using the existing 10 models and core P0 structure, then complete a delivery. Prepare character/creature production without blocking route validation on assets.
2. Finish a small reference section of ceilings, wall caps, floor, lights and rock. Establish its material density and palette as the facility standard.
3. Connect detour, yard, suppressor and outer entry; integrate employee, arms, Listener and brown A into real controls.
4. Add office traces, rack contents, decals and audio. Do not blindly fill passages with cargo-blocking props.
5. Verify fresh-save delivery → receipt → return → purchase → next preparation, plus retry, rejoin and progress restoration after restart.

New designs follow production image → user appearance approval → 3D production. Approval of this list is not blanket approval of 34 new appearances. Regenerate existing models only for confirmed defects; try local dimension/UV/material/pivot fixes first.

- [ ] Per model: front/back/sides/underside, normals/UV/texture paths, units/pivots/simple collision.
- [ ] Reference section: first-person cargo/weapon visibility, 4-player crossing, no door/ramp/rack snagging.
- [ ] Play: unprompted delivery/receipt/return/first purchase and meaningful safe/risky route choice.
- [ ] Failure: no progression lock across down/rescue/all-down recovery/disconnect/next shift.
- [ ] Automated: state/duplicate pay/ownership/save/build regressions pass; separate external 4-player/Steam evidence.
- [ ] Human: independently record feel/readability/tension/replay intent. Select target PC and measure frames, memory and visible-scene load.

## Sources and validation scope

[CSV list](demo-art-list.csv) · [Facility integration](psx-tripo-kit-01.en.md) · [Reception props](psx-props-01.en.md) · [Beacon](psx-beacon-01.en.md) · [Baton](next-equipment.en.md) · [Four-player](four-player.en.md) · [HUD](crew-hud.en.md).

[Contract/pay code](../../NoReturns/Assets/_NoReturns/Runtime/CarryLab/CarryMission.cs) · [Suppression code](../../NoReturns/Assets/_NoReturns/Runtime/CarryLab/CarrySuppression.cs) · [Creature code](../../NoReturns/Assets/_NoReturns/Runtime/CarryLab/CarryThreat.cs).

This task inspected the reference image, Selected model files, current code and documentation, then authored the list and bilingual documents. No code, model, map, timing, save or executable changed; no new gameplay test was run.

[Ship exterior — A selected](ship-concepts.en.md). Comparison concepts before 3D production.

[Interior revision 02 — front glazing and dimensional calculations](ship-interior.en.md). Proposed replacement for the held interior draft; 3D validation pending.

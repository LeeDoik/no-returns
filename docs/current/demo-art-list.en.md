# CINDER DEPOT — one-cycle demo and 3D asset list

[한국어](demo-art-list.ko.md)

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

# Map, art and technical production guide

[한국어](03-guides.ko.md)

## 2026-10-03 — Employee Blender inspection and working copy

Preserve the user-supplied FBX/JPG and import them in Blender 5.2.2 LTS. [Editable working copy](../../art/player-employee-01/prepared/NR_Employee_01.blend) · [Originals, hashes and provenance](../../art/player-employee-01/provenance.json) · [Measured inspection](../../art/player-employee-01/validation.json) · [Reproduction script](../../art/player-employee-01/inspect.py). This supersedes older file-not-received/import-unverified statements below only within the verified scope. ACT01 rigging/game integration and ACT02 arms remain incomplete.

| Actual FBX inspection | Result |
|---|---|
| Mesh / material / UV | 1 each, 4,759 vertices |
| Faces | 4,888: 4,230 quads + 658 triangles |
| Triangulated count | 9,118. Preserve mixed source topology; do not triangulate the whole mesh. |
| Texture | 1 Base Color JPG at 4096×4096, linked and packed into the working copy |
| Skeleton / weights | None |
| Original / working height | Approximately 0.9126m → 1.8m, uniform factor approximately 1.9724 |
| Connectivity | 21 connected components, 437 boundary edges, 1 edge shared by 3 faces, 1 inconsistent-winding edge |

Bake import rotation/scale into only the working mesh and place its origin at the floor center, with object rotation 0 and scale 1. The 1.8m working target matches the [current player controller](../../NoReturns/Assets/_NoReturns/Runtime/CarryLab/CarryRoom.cs). Preserve UVs, face connectivity, materials and image pixels. Original FBX/JPG hashes match; leave the supplied Downloads folder untouched.

[Front](../../art/player-employee-01/review/front.png) · [Back](../../art/player-employee-01/review/back.png) · [Three-quarter](../../art/player-employee-01/review/three-quarter.png) · [Hand close-up](../../art/player-employee-01/review/hand-top.png) · [Body topology](../../art/player-employee-01/review/wire-front.png) · [Hand topology](../../art/player-employee-01/review/wire-hand.png) are 6 actual Blender renders. Inspect worn ivory/orange workwear, a black visor, a back without a protruding backpack and finger shapes. The hand close-up shows gaps between four fingers and a distinct thumb shape, but does not verify gripping or joint deformation. Topology images show actual source polygon edges; remove the diagnostic mesh copy from the saved working file.

Boundary edges include joins of separate gloves, boots, helmet and other parts; do not weld them all. Both exception edges lie on the positive-X boot; record exact vertices, adjacent faces and coordinates in the inspection JSON. **Next: inspect/repair the boot exceptions → canonical rig → shoulder/elbow/knee and hand-grip trials**. Static Quad/texture checks do not establish rigging readiness or performance.

Reproduce from the repository root: `blender --background --factory-startup --disable-autoexec --python-exit-code 1 --python art/player-employee-01/inspect.py`. Pass 8 checks covering source integrity, unchanged topology counts, finite coordinates/UVs, no zero-area faces, height, floor origin, packed textures and reopening the saved `.blend`. `game-dev` is not on PATH; use installed Blender without claiming that CLI's package admission checks. Actual provider settings/job ID/usage-term evidence, user appearance approval, rigging/Unity/performance remain unverified. No new paid generation or game code/scene changes; retain game 0.9.4 and protocol 13.

## 2026-10-03 — Transfer the generated model to Blender

These are the handoff instructions issued after the generation report. The actual FBX/textures have since been received; use the Blender inspection above for current results. Recommend **FBX (.fbx) with its accompanying textures** for mesh editing.

- Select FBX export and the Blender preset if shown. Include textures if that option exists; preserve external texture files and folder structure. Deliver the complete ZIP if supplied as an archive.
- Export the prepared mesh as it is. Disable Triangulate if offered for this editable handoff; do not run additional remeshing/face-count changes just to change format. Confirm original Quad preservation after import rather than inferring it from the format alone.
- Extract the ZIP, then import the model in Blender through `File → Import → FBX (.fbx)`. Check textures in Material Preview, orientation and scale, then save the editable source as `.blend`. Preserve the original FBX/texture package.

Checked on 2026-10-03: [Tripo's official DCC handoff guide](https://www.tripo3d.ai/help/features/how-to-export-and-import-to-dcc-tools) documents Blender FBX import and character quad/skeleton/texture-reference transfer. Do not assume the Blender preset in the [conversion API](https://developers.tripo3d.ai/en/docs/models-convert) exactly matches options shown in Studio. Actual import/material linking are verified above; joint deformation and Unity integration remain incomplete.

## 2026-10-03 — Initial employee topology settings

This is a **production recommendation** in response to the user's settings question. Topology is the connectivity of mesh faces and edges. These are pre-generation first-sample recommendations; the received model measurements appear in the Blender inspection above.

| Item | First trial setting |
|---|---|
| Input | [Single front T pose](../art/space-concepts/employee-02-front.png). Use the lowered-arm side image only as a shape reference. |
| Method | Use the available Smart Mesh/Smart Low Poly or Retopology controls to preserve and clean the shape. Names/ranges vary by version. |
| Topology | **Quad**. Retain a quad-dominant editable source. |
| Polygon Count | Start at **5,000 quad faces**. An all-quad mesh becomes approximately 10,000 triangles after triangulation. Distinguish units if the UI reports triangles, and measure output Faces/Tris separately. |
| Rigging stage | Establish the canonical rig after shape/joint topology cleanup. |
| Delivery | Preserve the editable source; deliver FBX after checking triangulation/deformation of the final game mesh. |

This is a starting value for 1 full-body model, not a PSX hardware constraint or a performance guarantee. Tune low-resolution surfaces and angular silhouettes separately. Represent grime, seams and small wrinkles on the surface rather than modeling all of them; allocate faces to silhouette and bending regions.

Recommended sequence: **generate from the front image → preserve the source → refine quad topology/face count → Blender inspection/local repair → UV/texture checks and reprojection if needed → rigging → deformation trial → Unity verification**. Do not repeatedly remesh geometry that already passes. A Quad label or successful automatic operation does not establish animation readiness.

- Shoulders/armpits: provide face flow around the arm into the torso, and check armpit collapse when raising the arms.
- Elbows/knees: review an initial **3 circumferential loops**, one at the bend and one on each side. This is not an absolute rule; adjust using volume during 90-degree bends and the parcel-carry pose.
- Hands/gloves: first check that thumbs/fingers are not fused to each other or the torso. Test close-up first-person hands in actual gripping poses. Increased face count alone does not establish repair of fused fingers.
- Helmet/visor/rigid buckles: retain silhouettes and appropriate bone weights rather than adding dense bending topology. Check gaps/intersections between separate parts; automatic segmentation is not required.

Official sources checked on 2026-10-03: [Tripo Studio — Quad/Triangle and Polygon Count](https://www.tripo3d.ai/blog/tripo-studio-tutorial-english), [Tripo Smart Mesh](https://www.tripo3d.ai/features/smart-mesh), [Tripo Retopology API](https://developers.tripo3d.ai/en/docs/mesh-decimate). Do not assume identical supported ranges across Studio features and API algorithms. [Blender remeshing/retopology](https://docs.blender.org/manual/en/5.0/modeling/meshes/retopology.html) distinguishes automatic quad remeshing from final deformation-oriented face flow; [triangulation](https://docs.blender.org/manual/en/5.0/modeling/meshes/editing/face/triangulate_faces.html) splits quads/polygons into triangles. The 5,000/10,000 and joint-loop criteria above are project recommendations, not provider guarantees.

## 2026-10-03 — Separate employee view images

At the user's request for separate pictures, generate each view individually with the built-in image editing tool using concept 02 as the reference. All are **1254×1254 PNGs**, each containing 1 full-body employee with no text or color swatches. [Front](../art/space-concepts/employee-02-front.png) · [Side](../art/space-concepts/employee-02-side.png) · [Back](../art/space-concepts/employee-02-back.png) · [3 exact prompts and generation method](../art/space-concepts/employee-02-views.request.json).

Visually inspect front/back T poses, a left-facing lowered-arm side view, helmet, workwear, identification colors, gloves/boots and full-body framing. Preserve the original sheet; generated edits do not retain pixel-identical grime/seams. The side pose differs, so this is not a validated same-pose multi-view input set. Propose the front image first for a Tripo single-image trial. Appearance approval, cross-view consistency, 3D generation and rig/game validation remain incomplete.

## 2026-10-03 — Employee image concept 02

![Employee concept 02 — front, side and back](../art/space-concepts/employee-02.png)

At the user's request, use the built-in image generation tool to create 1 new concept sheet with the previous employee sheet as an appearance reference. Preserve the original 1774×887 PNG, 1,669,938 bytes, together with the [exact generation prompt, reference image and method](../art/space-concepts/employee-02.request.json). Do not overwrite `employee-01.png`.

Show the same employee in front/back T poses and a side view with lowered arms to expose torso thickness. Visually inspect ivory workwear, faded orange identification panels, black visor/gloves/boots, dirty seams, chipped paint, abrasion and a simple back silhouette. The 4 orange/teal/mustard/violet patches below are color swatches, not separate models or finished team variants.

**This is an appearance-review proposal awaiting user approval.** Finger structure, cross-view seam/proportion consistency and rig deformation remain unverified. Do not feed this multi-figure board directly to Tripo; after appearance selection, prepare a single-character front T pose and any required individual views of the same design. This task ran no Tripo job, model/animation generation or Unity integration/execution. Follow the [subsequent production workflow](demo-art-list.en.md#2026-10-02--proposed-player-model-and-animation-workflow).

## 2026-10-02 — Player animation preparation

[ACT01 full body/ACT02 first-person arms recommendation](demo-art-list.en.md#2026-10-02--proposed-player-model-and-animation-workflow). Propose GPT Image → Tripo → mesh inspection → Mixamo basic motions/rigging → Blender custom-motion adjustment → Unity integration. First check 1 character with idle/movement/parcel carrying, sharing one rig across 4 team colors. Validate hand reach with cargo rotation/distance. This is research and a production proposal; image/model generation, gameplay integration and quality approval remain pending.

## 2026-10-02 — Cinder suppression and outer creature 0.9.4

[Current rules, production and running](cinder-suppression.en.md). Connect existing 90/135/180-second stages, signal audio and relative work-light dimming to 4 current suppressors and the boundary. Connect east entry, obstacle routing and pursuit for the outer creature 8 seconds after shutdown. Preserve sky/fog/sun, source art and collision. Default launch is delivery + Listener + suppression/outer; retain `--delivery-only`/`--map-only` regression. Version 0.9.4, protocol 13, TCP 27842. Pass 60 actual four-process checks for purchased-beacon distraction, hazardous return, shared down/ship safety, emergency recovery and next shift. [Validation record](../validation/cinder-suppression-0.9.4.json). Supersede older unconnected-suppression/outer statements below within this scope. Human quality, final creatures/audio, clues/progression saving and other-environment/performance checks remain pending.

The historical `artifacts/` paths identify local evidence from the original run. These files are no longer retained here and are not included in the public repository. Historical passes are distinct from current revalidation.

## 2026-10-02 — Repository and local file cleanup

[Cleanup scope and retention policy](repo-hygiene.en.md). Remove 51 unreferenced Unity review copies/duplicate Smart Wall files (44,900,041 bytes), 115,418,532 bytes of stale local tests/logs/caches and 246 LFS cached objects (approximately 122MB reported). Preserve production sources, current assets, historical documents/commits and 8 existing ship material edits. Retain game 0.9.3 / protocol 12. Pass dependencies across all Unity Assets, LFS HEAD integrity and post-cleanup Mac build with 0 errors / 4 warnings. Mark 142 historical local-evidence links as unretained provenance; all 272 documents pass link/language/checkbox checks. [Per-file evidence](../validation/repo-cleanup-2026-10-02.json).

## 2026-10-02 — Cinder Listener, baton and rescue 0.9.3

[Current rules, running and production checks](cinder-listener.en.md). Connect one Listener's actual floor/obstacle grid movement, noise investigation, warning/attack, existing empty-hand left-click baton, hold-E rescue and all-down ship recovery to default Cinder delivery. Preserve normal 420 CR payment and source art scene. Judge safety using current Cinder ship coordinates; do not instantiate old suppression/outer/clue/save objects. Existing beacon pulses also attract Listener investigation. Keep peaceful delivery regression via `--delivery-only` and movement tests via `--map-only`. Version 0.9.3, protocol 12, TCP 27842. Supersede older unconnected-Cinder-Listener/baton/rescue statements below only within this scope. Suppression/outer creature, clues, progression saving and human quality remain pending. [Actual validation scope](../validation/cinder-listener-0.9.3.json).

## 2026-10-01 — Controls, ship and purchase UI 0.9.2

[Current controls and running](controls-ui.en.md). E targeted use/ship terminal, left-click ground placement, hold right click to rotate parcel, wheel 0.75–1.6m reach, Q immediate release. Separate departure/return/purchase into native buttons showing boarding count, wallet, disabled reasons and zero-pay return confirmation. Esc settings provide 13 button rebindings, sensitivity, FOV, language/defaults. Menus block gameplay inputs while the world continues. Connect Cinder 120 CR beacon purchase/shared carrying/two 8-second signals. Version 0.9.2, protocol 11, TCP 27842. Preserve source map/delivery pay. Supersede E automatic progression/Esc shop and unconnected-Cinder-beacon statements below within this scope. Listener/baton/suppression/save integration and human quality assessment remain. [Actual validation scope](../validation/controls-ui-0.9.2.json).

## 2026-10-01 — Cinder delivery, receipt and return settlement

[CINDER-DELIVERY-01 usage, coordinates and evidence](four-player.en.md#2026-10-01--cinder-delivery-receipt-and-return-settlement). In separate `CinderFourPlayerTest`, connect ship E preparation/arrival → BAY 04 floor delivery → CRT receipt E collection → all crew aboard/E return/420 CR settlement → next shift. Retain version 0.9.1, protocol 10 and TCP 27842. Reuse the existing delivery ledger, parcel, CRT, KO/EN screen and label tooling. Preserve the source environment scene; supersede older unconnected-delivery/receipt statements below only within this test scope. Default launch is delivery; `start --map-only` and `check` retain the movement test. Listener/baton/suppression/beacon/save and four-human/other-PC/performance validation remain incomplete. Actual automated evidence is in the [validation record](../validation/cinder-delivery-01.json).

## 2026-10-01 — Run/regenerate the Cinder four-player test

Follow [four-player usage, commands and limits](four-player.en.md#2026-10-01--cinder-four-player-map-test-environment). Save source art edits in `CinderCompactSiteReview`. `python3 tools/cinder_four_player.py build` copies the source into a separate test scene, removes the single employee/test parcel and attaches the existing four-player runtime. Manual test-scene edits are overwritten by the next build. Do not use the older source-regenerating Blockout Build menu for this task.

Mac `07_Play_Cinder_4P.command`/`08_Stop_Cinder_4P.command` start/stop four windows; `check` validates actual-process joining, carrying, passage traversal and reconnect. TCP 27842, 30fps cap/50Hz physics/250m camera, version 0.9.1/protocol 10. Build with the Editor open, Play stopped and scene saved. Confirm completion with the native receipt. **Mac build: 0 errors/7 existing warnings; pass 13 actual four-process checks, 7 existing rescue-rule conditions and 7 regular-mode defaults. Inspect 2 native 800×500 Korean HUD captures for crew 4/4, E/Q controls, reticle and team colors; also verify 4 manual processes/3 connections and shutdown.** Add no new packages, online server, Steam or delivery/AI integration.

## 2026-10-01 — Open sky and zone boundaries

[Current modification and checks](cinder-compact-site.en.md#2026-10-01--open-sky-and-zone-boundaries). Both modification stages are saved in the current scene. Reproduce in order: `Polish Cinder Scenery and Suppressor Visuals` → `Open Cinder Sky and Define Zones`; each stops with unsaved edits, Play or its existing root. Manage native assets through the [production source](../../NoReturns/Assets/_NoReturns/Editor/CinderBackgroundBuild.cs). Use `Validate Cinder Exterior Background`; prop/architecture/compact-site validation menus also route to current background validation. Preserve original `ArchitectureYard.asset`, using a new mesh with only selected cover triangles removed. Retain both original background state evidence and the current state after ceiling adjustment. No new ground colliders, lights, textures, external assets, dependencies or paid generation. Run checks from the repository root after saving/stopping Play.

```bash
source ~/.unity/env
unity command eval --code 'NoReturns.Editor.CinderBackgroundBuild.Validate(); return true;' --project-path "$PWD/NoReturns" --caller plugin --skill unity-cli --format json
unity command run_script --file "$PWD/tools/unity_checks/CinderBackgroundRenderCheck.cs" --entry CinderBackgroundRenderCheck.Main --project-path "$PWD/NoReturns" --caller plugin --skill unity-cli --format json
unity command editor_play --project-path "$PWD/NoReturns" --caller plugin --skill unity-cli --format json
unity command run_script --file "$PWD/tools/unity_checks/CinderCarryEdgeCheck.cs" --entry CinderCarryEdgeCheck.Main --timeout_ms 180000 --project-path "$PWD/NoReturns" --caller plugin --skill unity-cli --format json
unity command editor_stop --project-path "$PWD/NoReturns" --caller plugin --skill unity-cli --format json
```


## 2026-10-01 — Exterior background production/render checks

Follow [current background, assets and commands](cinder-compact-site.en.md#2026-10-01--rocky-territory-and-industrial-background-outside-the-field). Reuse the existing Shape mesh tool and aged industrial atlas; produce 8 native meshes, 2 materials and 1 native 128×128 mineral texture. 69 placements, 4,624 triangles, unit scale 1 and 0 new Colliders/Lights. Orient repeating terrain/rock UVs by face and check finite values. `Add Cinder Exterior Background` stops with unsaved changes, Play or an existing root. Use `Validate Cinder Exterior Background`, routed `Validate Cinder Site Props`, stopped-Editor `CinderBackgroundRenderCheck.Main` and the shared Play carrying check. Current evidence uses `background-`; preserve earlier `architecture-`/`sky-`/`props-`/`site-` records. Shared mesh replacement uses native setters on the existing Mesh to preserve GUIDs while updating actual rendering. Track the deterministic texture source/native asset; no external assets, dependencies or paid generation. Structure/sky-stage prefixes below are historical evidence.

## 2026-10-01 — Building-form module production/placement

Follow [current structural modules, dimensions, checks and running](cinder-compact-site.en.md#2026-10-01--varied-building-outlines-and-structural-modules). Reuse the existing mesh tool/aged atlas to produce 9 structural module types and supporting surfaces as 11 native Unity meshes. Unit placement scale 1; polygon roofs retain 1.2m UV-tile density. The 2 replaced ground outlines use static MeshColliders; elevated structures/canopies leave passage headroom. `Add Varied Cinder Architecture` stops with unsaved edits, Play or an existing root. Check using `Validate Varied Cinder Architecture`, routed `Validate Cinder Site Props` and the shared Play carrying check, recording `architecture-`. Current sky preservation compares actual valid-pose counts rather than a fixed 246. Preserve earlier `sky-`/`props-`/`site-` records. No new textures, external assets or dependencies.

## 2026-10-01 — Current Cinder asset-production preparation

Follow [current whole-map prop production/checks](cinder-compact-site.en.md). Reuse the approved aged atlas, rack and freight/CRT models; create only the 8 needed prop meshes natively in Unity. No new textures, external assets or dependencies. Preserve original structure, 159 body positions, 94 segments and 17 four-body lanes. Place shelves/pallets near walls, cabinet backs flush against walls, and pipes/vents/equipment on high walls/roofs. Avoid rear gaps that fit only bodies but not rotating cargo. Six labels use builtin-font physical world-space UGUI; 46 freight and 3 CRT visuals are static. Keep saved roofs, hiding/restoring them only for cutaway captures. `Add Cinder Site Props` does not overwrite existing prop groups/unsaved edits. Store `Validate Cinder Site Props` structure results and pre-sky Play carrying evidence under `props-`. Next: prop-quality review and BAY 04 receipt facilities/gameplay-coordinate integration.

Follow the current [sky preset, values and reapplication guide](cinder-compact-site.en.md#2026-10-01--sky-and-distant-atmosphere). Use builtin `Skybox/Cubemap`, a 64×64 pixel 6-face RGBA32 cubemap and 35–115m Linear fog. Run `Apply Cinder Dusk Sky` after saving/stopping Play; it resets manual sky tuning to the preset. Current Play carrying/captures write `sky-`; preserve earlier `props-` evidence. Retain all 40 local lights and directly review final danger signals for fog occlusion.

0.9.1: [Cooperative HUD and evidence](crew-hud.en.md). Reorganized gameplay instructions and four-player states. Human readability assessment remains outstanding.

## Current 0.9.0 — four-player cooperation

[Current rules, launch and validation](four-player.en.md). Supports 1 host and up to 3 clients joining during preparation. This supersedes historical two-player limits and unimplemented four-player statements below. Existing E/Q, baton, delivery and receipt collection rules remain. Steam registration is deferred at user request; other-PC, internet and human four-player fun validation remain outstanding.


0.8.14: [Display implementation policy and current audit](display-systems.en.md).

0.8.13: [Integrated display / 내장 화면](next-equipment.en.md).

0.8.12: [Continuous arc; integrated-display concept pending](next-equipment.en.md).

0.8.11: [LMB + charge display](next-equipment.en.md).

0.8.10: [G 충격봉 / G baton](next-equipment.en.md).

0.8.9: [Game integration — 0.8.9](psx-beacon-01.en.md).

0.8.8: [E/Q controls and carried beacon](controls-beacon.en.md) supersedes previous keys and remote deployment.

0.8.7: replaced floating text with UI textures on the actual CRT surface. [Receipt terminal and occlusion checks](receipt-terminal.en.md).

0.8.6: original CRT/slot alignment, duplicate recorder removed, payment requires receipt collection and return. [Current receipt rules](receipt-terminal.en.md) supersede immediate-payment descriptions.

0.8.5: replaced the reception bench with floor markings and connected terminal reactions. See [Receipt terminal](receipt-terminal.en.md) for current rules and verification status; this supersedes raised-bench descriptions.


0.8.4: two approved parcel/receipt models produced and visually integrated. Dynamic terminal state remains follow-up work. [Record](psx-props-01.en.md).


0.8.3: six selected facilities integrated and Windows build completed. MCP bounds/collider checks passed for six modules; actual east-corridor and north-detour screens inspected. Existing wall/rack colliders and carrying rules remain. Full floor finish, lighting, pattern repetition, frame performance and human feel remain open. [Record](psx-tripo-kit-01.en.md).


Current 0.8.2: fixed downed-host departure recovery, low-step routing/foot height, simultaneous F transitions, terminal collision, ship attack protection, the west rack slit and join-refusal feedback. Unreproduced target oscillation and the standing-host departure-abort policy remain design reviews. [Findings and evidence](review-fixes.en.md).

Added in 0.8.1: after entry, the outer creature automatically pursues crew across the entire map. Downed/aboard employees are excluded; pursuit resumes after beacon distraction. Walls require detours but do not block detection. [Rules/validation](space-play-07.en.md).

Added in 0.8.0: expanded the map to 32×44m with an east corridor, north detour and west storage wing. Four suppression stages and delayed outer-creature entry are implemented. Values, geometry and creature visuals remain experimental, not final art. [SPACE-PLAY-07](space-play-07.en.md).

Added in 0.7.0: I clue inspection and the Tab shared field log. Records reset on next arrival and are not restored after exit; wallet/license saving remains. [Clue specification/validation](space-play-06.en.md).

As of 0.6.0, host progression saves restore wallet, beacon license and successful-delivery count; shifts restart at ship preparation. Earlier session-only/no-save notes describe the state through 0.5.0. [Save rules and validation](space-play-05.en.md).

2026-09-12 · SPACE-01

## Using the project

For development on macOS, follow the [Mac environment and direct CLI commands](macos-development.en.md). The `.cmd` and PowerShell instructions below are for Windows.

Open the fresh Unity project with [01_Open_Project.cmd](../../01_Open_Project.cmd). The project is `NoReturns/`; code, scenes and assets belong under `Assets/_NoReturns/`. Edit scenes, prefabs and materials through Unity Editor or official MCP. Bootstrap is currently an empty starting point.

`powershell -NoProfile -File tools/unity.ps1 check` validates foundation settings. `setup` configures the initial foundation; it does not generate a game or replace authored scenes. Check connectivity with `python tools/unity_mcp.py editor_status`. MCP is not the game's multiplayer server.

Keep the existing root Git repository. Version `.meta` files with assets and exclude Library, Temp, Logs, UserSettings and builds. No remote repository was created or pushed. Record editor-version or render-pipeline changes as separate decisions.

## PSX art pipeline

The visual basis is worn space-delivery industry and unexplained sites. Review every asset against a shared palette, texture density, edges/silhouettes, lighting and UI readability. Do not use previous toy-themed resources.

Sequence: representative scene and employee/cargo/facility concepts → user image approval → model/texture production → scale, collision, animation and license checks → joint review inside Unity.

Separate low-resolution world presentation from readable UI. Choose texture filtering, internal render resolution, palette and dithering/wobble intensity through visual comparison rather than freezing final numerical specifications now. Fog must not conceal delivery signs or creature warnings. PSX shaders and models have not yet been produced.

## Before authoring sites and creatures

First block out receipt points, safe entry/extraction routes, observation positions, readable danger clues and tool-dependent solutions. Specify creature detection, transitions, failure, recovery and online authority. Verify delivery is possible in solo and cooperation before filling the scene with finished art.

[Design authority](01-overview.en.md) · [Current implementation](02-spec.en.md)

## Unapproved visual proposal

The generated [SPACE-ART-01 concept board](../art/space-concepts/concept-01.png) visualizes spacecraft preparation, site delivery and a receipt clue. It is not gameplay capture; appearance, colors, creature scale and symbols are review candidates. Do not produce models from this image before user approval.

## SPACE-ART-02 — visual style reference update

At the user’s request, the [official Mouthwashing Steam page](https://store.steampowered.com/app/2475490/Mouthwashing/) is the visual style reference. [Concept 02](../art/space-concepts/concept-02.png) is a generated review image retaining delivery, spacecraft, creatures and scene composition while introducing yellowed panels, lavender fog, red shadows and fluorescent lighting. The existing board was supplied to the built-in image editing tool. The core edit instruction preserves composition and parcel carrying while shifting toward low-poly forms, low-resolution surfaces and flat lighting. Some dense surface noise remains in the result; final texture density is undecided. Reference-game characters and narrative were not introduced. The reference choice is confirmed, but model production approval for this image is still pending.

## SPACE-ART-03 — response equipment visual proposal

The user responded positively to concept 02 and requested combat equipment imagery. This does not imply blanket art or model production approval. The [equipment board](../art/space-concepts/equipment-01.png) is an unapproved proposal in the same style. Shock Baton depicts close-range interruption, Pressure Caster creates distance, and Decoy Beacon diverts attention for a teammate’s delivery. Actual effects, damage, resources, misfires and friendly effects remain undecided, as does combat emphasis.

The built-in image generation tool received concept 02 as a style reference. The core instruction requests distinct tools and cooperative use scenes sharing yellowed industrial housings, burgundy grips and low-poly rendering. Visually checked silhouettes, colors and usage examples. No functional validation or model production was performed.

## SPACE-ART-04 — sequential gameplay mockups

[Exploration](../art/space-concepts/gameplay-01.png) → [Creature encounter](../art/space-concepts/gameplay-02.png) → [Delivery complete](../art/space-concepts/gameplay-03.png). Generated images retain the site, camera and HUD layout; these are not gameplay captures. Used concept 02 with the built-in image tool for the first frame, then edited subsequent frames sequentially. The core instruction shows carrying, interrupting a creature while a teammate delivers, and returning after receipt in the same space.

SUIT/STAMINA, E/LMB/TAB prompts, crew display, bay identifier, parcel count and combat effects are visual proposals rather than approved rules or implementation. Visually checked composition, objective changes and identification colors. Generation limitations leave a distant creature silhouette in the final frame and shift some background cargo. This is not evidence of frame-accurate state agreement. Appearance approval, in-game HUD readability, control feel and online validation remain pending.

## SPACE-ART-05 — employee sheet

The unapproved [employee sheet](../art/space-concepts/employee-01.png) includes front, side and back views; orange, teal, mustard and violet identification colors; and carry, braced-tool and walking-carry poses. The existing gameplay mockup was supplied to the built-in image tool as an appearance reference. The instruction describes the same employee from multiple directions with broad color planes, low-resolution surfaces and readable hand/parcel relationships. The back is suit fabric with a flat insignia, without a protruding board or backpack.

Visually reviewed directional forms, identification colors and usage poses. The caster in the action pose differs from the equipment board and is not an approved equipment reference. Generated proportions and seam agreement across views need further review before modeling; rigging, animation and clipping prevention remain unverified. No model production before employee appearance approval.

## SPACE-ART-06 — LED expression sheet

The unapproved [expression sheet](../art/space-concepts/expressions-02.png) shows neutral, happy, surprised, annoyed, middle finger (FLIP OFF) and error expressions using amber square dots on the same employee helmet’s black visor. Supplied the employee sheet as an appearance reference to the built-in image tool. The core instruction keeps helmet, lighting and composition consistent while changing only the visor expression. Team identification remains on uniform colors.

Visually checked distinct expressions, common helmet form and emissive color. Pixel grid, brightness and readability at actual distances are neither finalized nor validated. Combining manual selection and brief automatic reactions with manual priority is a design proposal; triggers, duration, precedence and online synchronization rules remain undecided. Visualization only, with no code, model or animation changes. No model production before image approval.

At the user’s request, replaced HELP with a middle-finger dot pictogram. Preserved the previous sheet as history. Built-in image editing targeted the lower-center visor and label; visually checked the meaning and placement of other expressions. Generated material pixels are not exactly identical to the source.


## SPACE-ART-07 — first creature behavior sheet

The unapproved [behavior sheet](../art/space-concepts/creature-01-behavior.png) references the long-limbed creature in the existing encounter mockup. Its working name is THE LISTENER, proposed as a sound-reactive warehouse wanderer. It compares roam → listen → approach → wind-up → sweep → recoil/search poses. This is not a final state-transition diagram or animation specification.

While roaming, the head and arms hang low; hearing a sound makes it stop and turn its head. It leans forward on approach and pulls one arm clearly backward before attacking. It follows through on a broad sweep, while a pressure blast causes loss of balance and renewed searching as candidate responses. Quiet movement, sound diversion and pushback are proposed counters; detection distance, attack timing/range/damage, recovery conditions, solo solutions and online authority remain undecided.

Supplied gameplay-02 as an appearance reference to the built-in image tool and requested distinct full-body poses and counterplay cues for the same creature. Visually checked pose differentiation, arm wind-up, distraction and recoil. The image-added 1.8 m/2.4 m sizes and footer slogan are generator suggestions, not approved specifications or store copy. The decoy appearance does not replace the existing equipment sheet. Appearance/motion approval, rigging and actual gameplay readability remain unverified; no code or models were produced.

## SPACE-ART-08 — spacecraft interior layout proposal

The unapproved [interior sheet](../art/space-concepts/ship-interior-01.png) combines a single-deck plan and interior perspectives in both directions. It places a route/contract terminal forward, a gear rack at mid-left and folding seats at mid-right, cargo bays on both aft sides, a central aisle and an aft ramp. Left/right are defined facing the bow. Route selection followed by automatic travel and landing remains the rule.

Equipment and cargo sit along walls to support preparation by 4 players, with a central passage for employees carrying parcels. Aisle width, cargo capacity and seat count are not final specifications. Arrows indicate circulation, not a one-way rule.

Supplied concept 02 to the built-in image tool as a style reference and requested plan, forward and reverse views of one space. Visually checked major functional zones, aisle and reversed left/right relationships. Some prop placement and seat depictions differ across views, so this is not an exact construction drawing. Generator-added company slogans are not approved game copy. Actual dimensions, collision, camera, 4-player circulation and loading remain untested; no code, map or model changes.

## SPACE-ART-09 — route terminal UI proposal

The unapproved [route terminal image](../art/space-concepts/route-terminal-02.png) compares destinations, a route map and delivery conditions side by side. Amber selection/warning accents contrast with the dark low-resolution bitmap UI. Visually checked CINDER DEPOT agrees across the list, selected map node and detail title. The screen presents cargo, handling, receipt, pay, known hazards and incomplete reports. CONFIRM ROUTE proposes confirming a route selection; it does not finalize immediate departure or crew readiness rules. Automatic travel/landing remains confirmed.

Supplied the ship interior sheet to the built-in image tool and requested a screen-focused three-column UI with legible text and consistent destination selection. RELAY 04, CINDER DEPOT, DEEP ARRAY, 240 CR, routes, lock conditions, handling conditions and wall slogans are review examples, not approved content, economy or copy. Actual interaction, Korean display, other resolutions, in-game viewing distance and cooperative selection synchronization remain untested. No code, UI scene or model changes.

Following feedback that the terminal was too clean, revised it with chunky bitmap type, square nodes, stair-stepped routes, dithering and a flat selection bar. Retained destinations, pay and automatic travel information. Used built-in image editing and kept the original for comparison. ESC/ENTER hints are unapproved control proposals. Visually checked the changes and retained information; actual rendering resolution and font specifications are not finalized.

## SPACE-ART-10 — CINDER DEPOT first-site layout

The unapproved [circulation and perspective sheet](../art/space-concepts/cinder-depot-layout-01.png) proposes the temporary first site requested by the user. Building corners and cargo separate the southwest landing/return point from the northeast BAY 04 receipt station. A solid teal line marks the long detour around the west warehouse and north corridor; a dashed amber line marks the risky shortcut through the central Listener area. A north-side office is an optional branch revealing recent-use traces.

Right-hand A shows sound diversion supporting cargo transport, B an unmanned receipt terminal, and C an office with a chair, mug and printed receipt. These clues propose recent use without finalizing a narrative answer. Both routes are intended for return travel; arrows do not define one-way rules.

Supplied gameplay-01 to the built-in image tool as a style reference and requested landing, two routes, receipt, clues and key-site perspectives. Visually checked major locations, route differentiation and A/B/C correspondence. The generated map retains stairs on the detour, thresholds and some unclear connections; it does not validate carrying feasibility or shortcut travel-time benefits. The safe detour needs greybox validation with ramps and sufficient openings for transport without tools or jumping. Dimensions, speeds, creature detection range and beacon appearance are not finalized. No code, map or model changes.

## SPACE-ART-11 — integrated suppression visualization

The [integrated sheet](../art/space-concepts/cinder-depot-suppression-01.png) combines delivery routes, landing, receipt, optional office, suppression coverage, outer threats, active/lost conditions and indirect stage changes. Supplied the prior site map to the built-in image tool and requested coverage, perimeter invasion and number-free state cues in the same space.

Lamp patterns, sound and creature approach communicate condition instead of an exact countdown. STABLE, UNSTABLE, CRITICAL and OFFLINE are candidate status labels, not a confirmed permanent HUD. Map boundaries, routes, invasion arrows and sound-wave icons are explanatory overlays. Continued decay after delivery is proposed, with receipt still operational after shutdown. Outer-creature appearance and pylon count remain unapproved.

Visually checked major areas and state comparisons. Some generated invasion arrows point outward and FIELD LOST employees are not fully retreating; do not use these as final navigation/behavior drawings. Intended behavior is entry from outside after shutdown with a chance to escape following warnings. No code, map or model changes. Audio, readability, carrying, online agreement and survivability have not been tested in play.

## SPACE-ART-12 — cosmic horror outer-threat candidates

The user requested outer-creature candidates, then steered toward cosmic horror. Retained the [initial animal-like comparison](../art/space-concepts/outer-threats-01.png) and explored new forms in the [latest comparison](../art/space-concepts/outer-threats-03.png). A THE ABSENCE emphasizes central empty darkness, B THE INVERSION an inverted folded body, and C THE INTERVAL apparent gaps between body sections. These are alternatives for one outer threat, not three confirmed species. Names, scale, material, behavior and model production remain unapproved.

Used the built-in image tool with the suppression sheet for initial candidates, then edited that result toward abnormal negative space, folding and segmentation. Visually checked the three silhouettes and distant appearances in a shared environment. Rock-like surfaces and thin connections on C remain; the images do not validate fear or physically impossible form. Evaluate motion, partial visibility, sound and sightlines together in play. No code or model changes.

Re-edited the latest sheet following the request for more grotesque forms. Reduced rocky surfaces and introduced pale folded hide, a smaller suspended body inside A, twisted support limbs on B, and empty body cavities with connecting tissue on C. Used built-in image editing and retained prior sheets. Visually checked form changes; actual fear response remains untested. The user subsequently selected A. Revised appearance and model production remain subject to review.

## SPACE-ART-13 — discarded THE ABSENCE

The white THE ABSENCE production sheet is cancelled. Previous material is history only; THE STRIDER below is current.

## SPACE-ART-14 — replaced by THE STRIDER

The brown A THE STRIDER from the user-specified initial comparison is the current selection. White THE ABSENCE and its production proposal are discarded. The [new production sheet and guide](../art/space-concepts/strider-production.en.md) describe quadruped views, rig concepts, contact and proposed motions. No actual model or rig exists yet.

## SPACE-ART-15 — cargo, receipt station and clues

The unapproved [integrated prop sheet](../art/space-concepts/cargo-receipt-clues-01.png) connects sealed cargo, receipt shelf/terminal, recent-use traces and receipt clues. Supplied the existing receipt scene to the built-in image tool and requested matching parcel markings, devices and records with a set-down → confirm → take-receipt sequence.

Cargo features handgrips, seal, orientation marks and a destination label. The proposed terminal changes from PLACE PARCEL to ACCEPTED and prints a receipt. Physically collecting the paper is not established as mandatory for delivery success. Mystery candidates are a warm mug in an empty office, a blank recipient field on a new receipt, and parcel code NR-041 matching an old slip. Sample code, content and clues are for review and do not establish a time loop or corporate conspiracy. Delivery should remain possible without noticing clues.

Visually checked repeated codes, shelf/terminal states, grips and seal. Some small labels and detailed placements are not fully consistent across generated views. Actual interaction, carrying, online agreement, clue readability and fun remain untested; no code or model changes.

## SPACE-ART-16 — Equipment shop and shift report

Created the [purchase/upgrade screen](../art/space-concepts/equipment-shop-01.png) and [return settlement screen](../art/space-concepts/shift-report-01.png) in the existing PSX route-terminal style: coarse bitmap text, restricted palette, CRT surface and English game copy. Both screens connect previous 180 + shift pay 360 = 540 CR, then 240 CR after buying the caster. Upgrading is a separate transaction unlocked after buying the license.

[Pay, failure and equipment economy draft](economy.en.md) provides proposed amounts and loss rules. Generated with the built-in image tool; visually checked amounts and selection states. Appearance awaits user approval; actual UI readability, input, controller and accessibility remain unverified. No models or game code changed. Do not produce models without image approval.

## SPACE-ART-17 — Gameplay framing and HUD concept

Created the [carrying/encounter comparison](../art/space-concepts/gameplay-hud-01.png). Left shows cargo carrying; right shows stopping upon seeing THE LISTENER. Used employee, earlier gameplay, LISTENER and cargo sheets as references with the built-in image generation tool. This is not a Unity capture.

Generation brief: preserve the same orange employee and sealed cargo, third-person over-shoulder camera, bent CINDER DEPOT service lanes, teal teammate identification, PSX low-resolution materials and bitmap text, and matching minimal HUD placement. Show only objective, crew, SUIT, set-down and stowed caster charge; exclude exact timer, minimap and enemy health bars. Right shows observing LISTENER beyond cover.

Two-handed carrying is intended, with the weapon marked STOWED. The SUIT bar, charge blocks, E input and MOSS teammate name are UI proposals, not finalized health, use counts or key bindings. Visually checked camera direction, main text, teammate color and creature silhouette across both scenes. Cargo clearance at the right-side barrier, obscured hand contact and small-text readability at actual display size require in-game checks. A still image does not validate clipping, controls, stealth detection or tension.

Appearance awaits user approval. No models or game code changed. Suppression-stage comparison, rescue, equipment effects and continuous-delivery sheets remain queued.

## SPACE-ART-18 — First-person situation comparison

Created the [first-person sheet](../art/space-concepts/gameplay-first-person-01.png) with the built-in image generation tool. The third-person camera in SPACE-ART-17 above is superseded history.

Generation brief: use existing employee, equipment, cargo and gameplay HUD images as appearance references but move the camera to employee eye level. Preserve PSX materials and CINDER DEPOT lanes across 4 scenes: empty-handed exploration, two-handed cargo, baton readiness and compressed-air push. No own head/back; show gloves and sleeves only as needed. No simultaneous cargo/weapon carrying; exclude numeric timers and enemy health bars.

Cargo sits low with hands on side handles to preserve forward visibility. Readied/active equipment appears lower right; teammates retain full bodies, team colors and LEDs. Keep indirect suppression cues; weapon effects depict pushing an ordinary LISTENER, not killing outer creatures. Adjustable FOV, restrained head bob and separation from physics shake are implementation proposals with undecided values.

Visual check: all four scenes are first person, cargo and weapons are separate, and main UI labels/equipment forms are readable. In exploration, OPEN appears on a door away from the center dot; do not use this as finalized interaction range/focus. Cargo carried by the teammate in the pressure scene is obscured and does not prove carrying success. Actual floor visibility, handle contact, wall clipping, gas range, text at different display sizes and discomfort remain unverified. E/LMB inputs, SUIT and charge blocks are proposals, not finalized bindings or values. Appearance awaits approval; no models or game code changed.

## SPACE-ART-19 — Same-location suppression stages

The [4-stage sheet](../art/space-concepts/suppression-stages-01.png) compares stable, unstable, critical and offline from the same eye-level first-person composition. Used the existing first-person sheet, STRIDER production sheet and integrated suppression sheet as references with the built-in image generation tool. The generation brief preserves geometry/camera and distinguishes stages through indicators, utility lighting and outer-creature approach without a numeric timer.

| Stage | Proposed visual cue | Proposed sound |
|---|---|---|
| Stable | All indicator windows lit, utility lights active, distant outer silhouette | Even machine hum |
| Unstable | Some indicators dark, vent smoke, exterior approach | Broken operating rhythm |
| Critical | Only red indicator remains, emergency lighting/sparks, creature at perimeter | Strained operating pulses |
| Offline | Device indicators dark, STRIDER enters, emergency lights remain | Machine hum stops; footsteps emerge |

Stage headings and AUDIO text are editorial annotations, not persistent HUD, actual audio or implemented subtitles. The receipt terminal READY and SHIP passage remain available after shutdown. Stage durations, exact boundary and pursuit speed are not finalized. No outer-creature killing or unwarned instant death is introduced.

Visually checked main geometry, lit-indicator count, emergency lighting and entry direction. The generated image retains a rear ridge silhouette while adding a closer creature, so it does not accurately depict one individual's continuous movement. It does not finalize creature count. The middle stable indicator is amber, so it is not an all-green color specification. Small perspective/prop differences do not constitute a validated same-camera render. Stage recognition, color accessibility, sound, escape feasibility and fear await play validation. Image approval pending; no game code, models or audio changed.

## SPACE-ART-20 — Downed, rescue and emergency return

The [rescue sheet](../art/space-concepts/rescue-extraction-01.png) connects discovery, cargo set-down, assisted walking and onboard return confirmation in first person. Used existing first-person, employee and ship-interior references with the built-in image generation tool. The generation brief preserves orange rescuer, teal downed employee MOSS and mustard guard ROOK roles, showing occupied hands and rescue order in PSX style.

This is a 3-person example, not a minimum crew rule. Assisted walking is a rescue-method candidate, not recovery of independent control for the downed employee. The rescuer first sets cargo down and supports the employee without using equipment. Another employee secures the route. A teammate brought aboard alive should count as returned under the economy draft, without implying automatic departure or instant healing. Abandoned undelivered cargo is not automatically recovered/delivered. Preserve the economy proposal that an undelivered return earns no delivery pay.

DOWNED, ASSIST, SUPPORTING MOSS, LOWER, CONFIRM RETURN, E input and status bars are UI proposals. Actual rescue duration, speed, interruption, death transitions and onboard recovery conditions remain undecided. Lit LED eyes suggest life but do not establish a new approved expression specification.

Visually checked teal-employee continuity, cargo set-down, empty-handed support and onboard crew confirmation. The support scene shows a hand near the upper arm, not complete proof of weight support or arm connection. RETURN TO SHIP remains visible aboard; implementation must update the objective after boarding. Hand/wall clipping, stairs/slopes, smaller-crew rescue, synchronization and urgency remain unverified. Image approval pending. No game code, models or animations changed.

## SPACE-ART-21 — Equipment before/after comparison

The [equipment comparison sheet](../art/space-concepts/equipment-before-after-01.png) compares before/after use of the baton, Pressure Caster and Decoy Beacon in first person. Used existing equipment, first-person and LISTENER behavior references with the built-in image tool. The generation brief preserves framing/location within each row and differentiates effects through stance, distance and attention direction while retaining PSX materials and employee/equipment appearances.

| Equipment | Intended before → after | Limits/validation targets |
|---|---|---|
| Shock Baton | Close attack preparation → contact briefly interrupts attack | Approach risk, post-use opening, repeated stunlock potential |
| Pressure Caster | Creature occupies route → push back to open a carrying gap | Range, gas use, wall collision, effects on other objects |
| Decoy Beacon | Watches delivery route → redirects attention toward beacon | Sound detection, competing noises, duration |

The target is an ordinary LISTENER, not a STRIDER kill or guaranteed equal effects on all species. Propose temporary opportunities for delivery/evasion rather than permanent incapacitation. Inputs, values, effect durations and charges remain undecided. Beacon light indicates activation; it does not establish light as the attraction mechanism.

Visually checked close contact, recoil pose, beacon illumination and changed creature direction. The gas cloud obscures distance, so it does not establish push magnitude. Beacon position/equipment details and obscured teammate cargo vary; do not use this as a precise continuous scene or carrying-success proof. Actual sound, gas, collisions, range, online results and cooperative fun remain unverified. User image approval pending. No game code, models, animations or audio changed.

## SPACE-ART-22 — Continuous first delivery

The [first-delivery sheet](../art/space-concepts/first-delivery-01.png) links arrival, carrying, waiting, acceptance, clues and return in first person. Used existing first-person, cargo/receipt/clue and ship sheets with the built-in image tool. The generation brief preserves the same orange employee hands, teal teammate, sealed NR-041 parcel and PSX style while changing cargo state and objective after delivery.

| Scene | Intent |
|---|---|
| ARRIVAL | Leave via ramp after automatic landing; no piloting |
| CARRY | Carry cargo through bent passages using physical signs |
| WAIT FOR IT | Observe and wait for LISTENER to pass; a solution without purchased equipment |
| ACCEPTED | Set cargo on shelf and confirm acceptance; objective changes to return |
| RECENT USE | Optional office: discover matching code on new receipt and old slip, plus recent-use traces |
| RETURN | Leave delivered cargo behind and confirm teammate boarding/return aboard ship |

This is a normal two-employee delivery example, not a finalized crew count, mandatory first-shift events or fixed progression order. Clue discovery is optional and does not explain the mystery. Suppression continues to decay after acceptance without forcing shutdown/chase every run. Do not grant purchased equipment or make physically collecting the receipt a new delivery-success requirement.

Visual check: verified matching code/main cargo appearance, two-handed carrying, post-acceptance return objective and new/old paperwork connection. LISTENER rear view/movement direction is not fully clear, so it is not evidence of actual AI routing. Teammate orientation and threshold placement in the return scene do not clearly establish completed boarding; implementation must verify all crew inside before confirmation. Background office/ship boxes should not be interpreted as duplicates of the delivered parcel. Lighting changes alone do not validate suppression-stage recognition. Actual map connectivity, travel time, carrying, acceptance, online behavior, clue comprehension and fun remain unverified. User image approval pending. No game code, models or animations changed.

[First-person carrying production/launch guide](carry-test.en.md): Unity MCP, Windows build and two-process checks passed. See the guide for launch, junction paths and limitations.

## SPACE-PLAY-02

[View-relative carrying and delivery-mode specification/validation](space-play-02.en.md). Added vertical-look carrying, route selection, receipt, full-crew return and session pay. Actual map, persistence and the full shop remain. The hazard experiment is separated in SPACE-PLAY-03 below. User feedback: the previous carrying build runs, but cargo does not follow vertical view.

## Guidance language — 0.3.1

Added Korean by default, the 한국어 / English menu toggle and persistence of the local preference. [Usage and production guide](carry-test.en.md). Language selection is separate from online game state. Results are recorded in the language checks (`artifacts/language/latest.json`).

## SPACE-PLAY-03 — LISTENER/rescue experiment (0.4.0)

[Listener specification, launch and validation](space-play-03.en.md). A separate hazard mode implements sound investigation, attack warning, shove, down, hold-R revival and all-down emergency recovery. Carrying/delivery modes remain. Next work includes the actual CINDER DEPOT map and alternate routes, suppression, deeper equipment reinvestment and human fear/cooperation evaluation. Final models, carrying employees, death, persistence, 4 players and Steam connectivity are not complete.


## SPACE-PLAY-04 — Supply/risk contracts (0.5.0)

[Purchase/use specification](space-play-04.en.md). Listener mode now implements session beacon-license purchase, shared deployment and risk-contract selection. Persistence, the full shop, more destinations and final art remain. Use existing launchers 06/07. Two-process progression checks passed 24, existing rescue checks 21, and language checks 9. Human feel, economy balance and rendered readability of the new supply panel are unverified.

## Save-file management

Normal builds use `%USERPROFILE%\AppData\LocalLow\NO RETURNS\NO RETURNS\progression-v1.json`, with the previous backup at `.bak`. Company/product names come from [project settings](../../NoReturns/ProjectSettings/ProjectSettings.asset). Clients do not modify their own files. A save-failure notice means current progress has not reached disk. Read errors preserve originals and block room creation until resolved. Older builds do not overwrite newer save formats.

[Proposed Codex/Claude Code collaboration](collaboration.en.md) — roles, boundaries and initial review prompt. No configuration or task dispatch has been performed.


## SPACE-ART-23 — First-area PSX environment modules

Added the [CINDER DEPOT entrance and 6-asset sheet](../art/space-concepts/environment-kit-01.png) and [production/review record](../art/space-concepts/environment-kit-01.en.md). Generated with the built-in image tool using the existing overall concept, ship and first-person sheets. This pending-approval reference compares consistency across wall, corner, door frame, floor, lamp and cargo rack. It is not the exact current map layout or a game capture. No models, materials or game integration were produced; the build remains 0.8.2. The production record preserves the actual prompt and limitations in floor mottling density and connection dimensions.


## SPACE-ART-24 — CINDER DEPOT overall overhead concept

[Overhead image](../art/space-concepts/cinder-depot-overhead-01.png) · [Generation/edit prompts](../art/space-concepts/cinder-depot-overhead-01.prompt.txt)

Following the user's request, review the whole layout before producing individual modules. Used the recent environment kit and earlier site layout as references for a built-in generated overhead roof-cutaway concept. Labels identify 01 ship/return, 02 west storage detour, 03 central sorting yard, 04 receiving bay, 05 optional office clue, 06 east suppression equipment and 07 outer entry. Solid teal marks the longer sheltered detour; ochre dashes mark the shorter route past the normal creature. Both converge on the same receiving bay. The teal route does not guarantee safety after outer-creature entry.

An additional image edit clarified the initially ambiguous east maintenance lane and outer bridge connections. Visually checked the arrangement and palette, but detailed start-arrow continuity, threshold traversal, exact carrying widths and physical reachability of every corridor remain unverified. Check a planar connectivity layout before constructing the actual map. Floor mottling density also requires adjustment in the real-time view.

This is a layout proposal rather than an exact reconstruction or measured drawing of the current 32×44m test map. No photogrammetry, mesh, model or game integration was performed. Preserve the 0.8.2 executable and existing images. Overall layout and individual module approvals remain separately pending. Updated Korean/English documentation and checked links.


## SPACE-ART-25 — Tripo PSX kit


[Git version control](version-control.en.md)

[Steam private testing registration and checklist](steam-testing.en.md) — 2026-09-13

0.8.15: [Baton shell restoration](baton-mesh-fix.en.md).

2026-09-13: [Complete demo cycle and 44 art production units](demo-art-list.en.md). Production proposal for the user goal, not approval of new appearances/timing or completed production.

2026-09-14: [FLATBED interior workflow](ship-interior-pipeline.en.md) — boundaries and reuse rules for structure, individual parts and Unity functional assembly.

2026-09-15 latest: [Structure trial 05](ship-interior-trial.en.md). Interior-first orbital post office: central inspection, left sealed lockers, right dispatch desk and rear folded seats. Devices are structural mockups; human spatial review and exterior art remain pending.


2026-09-16: [HTML layout study: 18 spaces, loops and emergency exit](map-study.en.md). Unity integration and human feel testing remain pending. Ship exterior production is deferred.


2026-09-16 / MAP-STUDY-03: Added west loading yard, east service yard, southern outdoor route and inside-only exit to HTML. Connectivity, delivery cycle and outdoor return automation passed; browser visuals inspected. Unity, online and human fun testing not performed. [MAP-STUDY-03](map-study.en.md).


2026-09-16: [MAP-STUDY-04](map-study.en.md) — Exterior expansion 04: enlarged the proposed overall extent to 120×96m. Preserved interior coordinates and widened the west antenna area, east service yard, south freight yard and fuel equipment area. Exterior obstacles allow movement around multiple sides. Antenna/fuel areas are currently labels and collision obstacles, with no new interactions. Automated travel to 5 additional exterior destinations and existing delivery/exit/return checks passed. Browser rendering confirmed with zero console errors. First-person feel, danger balance and Unity integration remain unverified.


2026-09-16 [MAP-STUDY-05](map-study.en.md): Added the northern exterior maintenance area to complete a four-sided perimeter loop. Proposed extent: 120×112m. Preserve separation between interior and exterior, connected only through the existing west entrance and east emergency door. Full exterior circuit and existing delivery/door automated checks passed; browser rendering and zero console errors confirmed. Unity integration and human feel remain unverified.


2026-09-16: [EXIT-RELOCATION](map-study.en.md). Moved the emergency exit to the right wall of receiving at the user-marked location. Closed the former cooling exit. Inside-only E unlock remains. Automated delivery/receipt/return, new gate unlock, old exit blockage and perimeter circuit checks passed. No Unity changes; human feel unverified.


2026-09-16: [Concept-based interior/exterior zoning](concept-zoning.en.md) / CONCEPT-ZONING-01.


2026-09-17: [CINDER-BLOCKOUT-01 — Unity primitive layout trial](cinder-blockout.en.md). A 108×86.4m validation scale, 5 enterable buildings, 3 loops and reused ship interior trial 05. Separate single-player spatial experiment; enemy pursuit, networking and delivery settlement are not connected. 41 physical passage checks and Editor rendering inspected; Windows build succeeded. Human controls, cargo clearance and cooperative enjoyment remain unverified.

# PSX facility modules — Tripo production record

[한국어](psx-tripo-kit-01.ko.md)

2026-09-13 · SPACE-ART-25 · In production. Game version 0.8.2 remains unchanged.

## 2026-09-13 — In-game integration 0.8.3

After the user resolved Unity, MCP reported ready. FacilityArtImport creates Resources/PSXKit01 prefabs for six selections with URP Lit materials and Point-filtered, mipmapped 512 textures. CarryWorld replaces wall/rack visuals while retaining their existing colliders. Walls use roughly 3m sections; eight floor panels, the east doorway, four lamps and a corner reinforcement inside the rear boundary are added. Models fit collider bounds, so proportions may differ from review sources. The original floor and routes remain; no new complex mesh collisions are used.

[Runtime integration](../../NoReturns/Assets/_NoReturns/Runtime/CarryLab/FacilityArt.cs) · [Import](../../NoReturns/Assets/_NoReturns/Editor/FacilityArtImport.cs). Earlier Unity blockers are historical. Screen, build and route results are recorded under the latest validation below. Human feel/fun validation remains separate.

## 2026-09-13 — Current status: generation stopped at 18, six selected

The user replaced the credit-exhaustion plan with a stop at 18 Smart Mesh candidates. Each of the six approved facility images produced A/B/C variants. Upload permission was resolved after the user changed the setting. Starting balance 2715, spent 1400, remaining balance observed in Tripo 1315; no further generation. The first two generations were free with 20 for texture; the other 16 cost 65 for generation and 20 for texture. Earlier blocked-upload and 2715-balance entries below are historical.

| Facility | Selection | Visual review reason |
|---|---|---|
| Wall | B | More orderly top than A's protrusions, readable stripe |
| Corner | C | Clear inner vertical frame at the connection |
| Door | B | More orderly top than A's rear protrusion, open doorway |
| Floor | B | Relatively simple panel divisions and outline |
| Lamp | C | Simple outline and readable light window |
| Rack | A | Consistent closed crates rather than an open-cargo variation |

Selection is an operator judgment from Blender comparison renders, not final user visual approval. Rack crates remain combined decoration. All 18 archives passed ZIP integrity and FBX mesh/UV checks. Early exports missing UVs were replaced with downloads after texturing completed. Sources and unselected candidates are preserved.

The selected six have bottom-center pivots, normalized normals, and 512×512 Base Color textures with nearest sampling. Loose-element checks removed 0 elements. Original textures remain preserved. Review-scale proposals are wall/floor width 3m, corner height 3m, doorway height 3.2m, lamp width 1.2m, and rack height 2.4m. Proportions were preserved, so modular connection dimensions are not yet fitted. Source Quad polygon counts are not treated as triangle counts; actual triangle counts and dimensions are in selection.json. Further reduction for repeated placement remains open.

- [x] Download and mesh/UV checks for 18 candidates.
- [x] FBX reimport, preserved triangle counts, UV and texture-file checks for six selections.
- [x] Visual review of actual comparison and selected-model renders.
- [ ] Unity Editor import, material setup, modular dimensions and doorway traversal checks.
- [ ] Collision, first-person readability, performance and actual cooperative play checks.

Six selected FBXs and six PNGs were staged in a separate Selected folder. The Unity Software Terms window issue remains unresolved, so Editor integration, compilation and a new build were not performed. Existing code, scenes, collisions and executable 0.8.2 remain unchanged. Blender checks and renders do not replace game validation.

[Generation and cost record](../../art/psx-kit-01/smart/generation-manifest.json) · [18-candidate checks](../../art/psx-kit-01/smart/validation.json) · [Selection metrics](../../art/psx-kit-01/selected/selection.json) · [Reimport checks](../../art/psx-kit-01/selected/roundtrip-validation.json) · [Selected render](../../art/psx-kit-01/selected/selected-review.png) · [Unity files](../../NoReturns/Assets/_NoReturns/Art/PSXKit01/Selected)

## Historical production records

## Scope and decision

Following the user's request to start art production and use Tripo, produce the 6 facility modules from the [environment sheet](../art/space-concepts/environment-kit-01.png). This production decision follows the earlier pending-approval record. It does not approve new character or creature designs. The overall map concept is not a dimensionally finalized plan.

Retain ivory panels, oxidized orange bands and dark frames. The [input PNG folder](../../art/psx-kit-01/input) contains image-generated isolated reconstructions of the existing modules, not lossless crops. Boxes on the cargo rack are static decoration, not gameplay cargo.

## Source generation evidence

Generated through Tripo Studio with Private, HD Model v3.1, Geometry & Texture. Neither 8K nor generation in parts was used. Each cost 55 credits, totaling 330; the balance changed from 3075 to 2745. Face counts below are web viewer readings, not local file inspections.

| Module | Tripo source | Faces |
|---|---|---:|
| Wall | [Model](https://studio.tripo3d.ai/workspace/generate/0ba9796d-0695-4c74-a6be-f6ab4c42b677) | 1952086 |
| Corner | [Model](https://studio.tripo3d.ai/workspace/generate/10648c39-40be-4df7-bea2-6dc0a0c882e9) | 1979820 |
| Door frame | [Model](https://studio.tripo3d.ai/workspace/generate/4fec9eb2-181d-43fc-a0cd-de6c161b6b95) | 1884644 |
| Floor | [Model](https://studio.tripo3d.ai/workspace/generate/840ea99c-b67a-4d12-bcd9-b2318b917d72) | 1946442 |
| Lamp | [Model](https://studio.tripo3d.ai/workspace/generate/fc4afe04-14bc-437c-b5e4-d1aaba016f45) | 1934640 |
| Cargo rack | [Model](https://studio.tripo3d.ai/workspace/generate/e0f41def-43eb-4804-873a-42b005bba0d8) | 1867433 |

## Remaining production and validation

Sources have excessive face counts for real-time PSX modules. Requested wall retopology with Triangle and a 1500-face target; the web viewer confirmed 1500 faces and 1574 vertices. An additional 5 credits brought the balance to 2740. Local mesh and silhouette validation remain pending, and this does not finalize budgets for other modules.

- [x] Saved 6 inputs locally and confirmed completion of 6 Tripo sources in the UI.
- [ ] Download GLB and inspect local meshes and materials.
- [ ] Review reduced silhouettes, UVs, doorway opening and rack gaps.
- [ ] Prepare Unity dimensions, pivots, textures and repeating connections.
- [ ] Check first-person readability and performance in a separate showroom.
- [ ] After user visual review, apply to the gameplay map and verify carrying and collision regressions.

Attempted wall GLB export in the in-app browser, but no download event occurred within 20 seconds and no local file was confirmed. Continuing through Chrome was blocked by a timed-out computer-tool app approval; the user was asked whether the approval prompt was visible. Download and game integration are not complete. Game code, scenes, colliders and executable remain unchanged. Automated documentation checks do not establish art quality or human play validation.

## 2026-09-13 — Chrome download verified

The dedicated Chrome browser connection successfully downloaded the wall GLB. It contains 4904828 bytes, GLB 2, 1500 triangles, 1 mesh, 1 material, 3 embedded images and no external URIs. Blender 5.2.1 imported it and confirmed 1500 faces with exit 0. No additional credits were used. The other 5 downloads, Unity integration and real-time visual validation remain pending. Earlier download-blocked records describe historical attempts and no longer apply to this wall download.

[GLB](../../art/psx-kit-01/tripo-source/NR_Wall_Tripo_1500.glb) · [JSON](../../art/psx-kit-01/tripo-source/wall-download-validation.json)

## 2026-09-13 — Facility production batch

The user authorized spending the remaining credits on facility-module quality and variations. The balance started at 2740; retopology for the other 5 modules used 25, leaving 2715. Exhausting the balance remains incomplete. New generation awaits Chrome file-upload permission. Provided the instruction to allow file URL access for the ChatGPT browser extension; no permission was changed automatically. Unity reports Software Terms waiting, but that window is absent from the computer-tool window inventory and the user also cannot see it. No terms acceptance, compilation or Editor import was performed.

Downloaded the other 5 dense sources and 5 reduced models. Including the existing wall, reduced counts are Wall 1500, Corner 2500, Door 3000, Floor 1000, Lamp 800 and Rack 4000 faces. Copied 11 GLBs into the project and recorded headers, lengths, face counts and SHA256. Blender 5.2.1 imported all 6 reduced meshes, exported 6 review FBXs, extracted textures and rendered the real meshes. Visually confirmed the doorway opening and consistent facility colors; connection dimensions, final grime density, collision fit and real-time performance remain unverified. Rack cargo is inseparable decoration. Original 4k textures are retained; final PSX resolution is undecided. Staged review files under Unity Assets without changing the existing map or executable 0.8.2.

[Download manifest](../../art/psx-kit-01/tripo-source/download-manifest.json) · [FBX and render](../../art/psx-kit-01/prepared) · [Unity review assets](../../NoReturns/Assets/_NoReturns/Art/PSXKit01/Review)


## 2026-09-13 — Upload permission retry

After the user reported granting permission, fileChooser.setFiles still returned Not allowed, including after reloading the page. Explained the distinction between Chrome control permission and extension file URL access and requested confirmation of that setting. No additional generation or credit use occurred; the last confirmed balance is 2715. All 6 review FBXs passed Blender reimport checks for expected face counts and UV presence. Unity import, final visual approval and the credit-exhaustion objective remain incomplete.

## 2026-09-13 — Unity terms window and licensing connection investigation

Reinvestigated the invisible Software Terms report. Unity was absent from the Windows window list and the CLI returned STATUS_NO_INSTANCES. Reopening the project displayed its startup window, followed by a Connection Lost dialog explicitly reporting a disconnected Unity Licensing Client. Editor.log records connection refusal and a 60.01-second timeout; the licensing log records failure to acquire the global mutex because another instance was running. Unaccepted terms alone are not a confirmed root cause.

Attempted Retry and a licensing helper restart. The termination command returned an error, so a successful restart is not claimed. The startup window appeared, but editor ready, asset import and compilation remain unverified. No terms button was clicked directly and no consent settings were edited. No game code, scenes or executable changes. Check bilingual records, document links and checkbox alignment. The current blocker is unverified licensing connection recovery.

## 2026-09-13 — Unity startup blocker recovery verified

On the requested retry, confirmed licensing mutex contention and connection refusal again. Terminated the unresponsive Unity.Licensing.Client PID 18392 specifically and started a new helper. Also terminated this project's stalled startup editor PIDs 45992 and 46472 specifically, then reopened the project. Did not terminate all Unity processes or delete license files.

Unity 6000.6.0f1 PID 44980 connected as ready. The editor_status command confirmed compiling=false and domainReloadInProgress=false. Visually inspected and foregrounded the Windows editor. The current scene is Untitled and play mode is stopped. No terms or Connection Lost window appeared in this launch, and entitlement resolution succeeded. The previous startup blocker is now recovered. The initial terms-window cause and recurrence remain unverified.

No game code, scene or executable 0.8.2 changes. General editor asset refresh completed, but individual import quality for the 6 PSX modules, gameplay and builds were not tested. No consent settings were edited directly. Checked bilingual documents, links and checkbox alignment.


Integration review caught the overwritten FBX root scale of 100; a parent object now preserves source transforms. Fresh collider coordinates are read after Physics.SyncTransforms. The default skybox is removed in favor of the existing dark camera background.


0.8.3: six selected facilities integrated and Windows build completed. MCP bounds/collider checks passed for six modules; actual east-corridor and north-detour screens inspected. Existing wall/rack colliders and carrying rules remain. Full floor finish, lighting, pattern repetition, frame performance and human feel remain open.

[Runtime corridor](../../artifacts/space-play-07-ui/run-20260913-165535/east-corridor.png) · [Runtime detour](../../artifacts/space-play-07-ui/run-20260913-165535/north-detour.png) · [Bounds checks](../../artifacts/space-foundation/art-fit-result.json)


The final 0.8.3 build passed 17 automated carrying regressions using two Windows host/client processes, including ownership contention, view tracking, drop, disconnect release and wall collision. Human feel/fun and Internet latency were not tested. Documentation checks passed for 204 entries.

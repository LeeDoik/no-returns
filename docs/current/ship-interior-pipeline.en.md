# FLATBED interior production workflow

[한국어](ship-interior-pipeline.ko.md)

2026-09-14 latest: [Structure → play approval → art production](art-structure-first.en.md) is the default for all assets. Retain the component methods/checks below and distinguish historical investigation from current status.

## Approach

Use **dimensioned Blender structure + reusable individual parts + Unity functional assembly**. The user requested choosing a better approach and proceeding; recommend this as the production basis for existing design 04. Do not generate the whole interior from one image as a single mesh. Also avoid separately generating every screw and panel.

| Approach | Assessment for this project |
|---|---|
| Generate the entire interior from one image | Useful for exploring atmosphere, but hidden faces, openings, dimensions and editability require renewed checks. Not selected for the final playable space. |
| Generate every element separately and assemble in Blender | Easy replacement, but thickness, edges and texture scale can differ between parts, increasing cleanup. |
| Build structure directly and create only necessary individual assets | Controls passage dimensions and reuses representative parts. Keep final functional assembly in Unity so model re-export does not remove interactions. Recommended approach. |

## Current evidence and first issues

- References are [design 04](../../art/ship-concepts-01/interior-design-04.png) and its [reverse view](../../art/ship-concepts-01/interior-design-04-rear.png). They are not precision forward/reverse drawings. Where placement conflicts, use the forward-facing design 04 layout as the basis and render both directions from one 3D layout.
- The interior width of 4.352m in the [production record](ship-production.en.md) and [integration script](../../art/ship-flatbed-01/integrate.py) is a starting point. Measure clear aisle width separately after furniture placement.
- The actual model has a cargo floor at 0.6m, a cockpit floor at 1.2m and stairs. This differs from the flat-floor concept. Review sill height, eye height and ceiling together rather than simply removing flooring. Do not arbitrarily restore stairs or an interior ramp.
- Retain the rear door and exterior boarding ramp. The existing door design of 3m × 2.4m and hull cut of 3.06m × 2.30m are not clear passage dimensions. Validate the central engine, frame, open door and cargo together. If moving engines or substantially changing windows is necessary, visualize the changed exterior for review first.
- Preserve Angular/Integrated models and create a separate interior candidate. Use actual runtime character and cargo collision dimensions for checks rather than human proportions inferred from images.

## Production units

These are part families, not placement counts, performance budgets or completed asset counts.

| Unit | Production and reuse | Keep separate |
|---|---|---|
| Cabin shell, floor and ceiling | Build in Blender to fit the hull | Shell, collision and inspection visibility groups |
| Straight walls, corners and ceiling ribs | Repeat modules with common profiles | Fixed structure by zone |
| Floor rails and anchors | Reuse representative modules; represent small grooves in materials | Collision only for shapes needed by carrying |
| Rear frame, doors and exterior ramp | Revise and reuse existing structure | Door leaves and ramp movement axes |
| Large central console | Build precise screen opening and housing in Blender | Housing, screen surface and interaction position |
| Side auxiliary consoles | Share material and button design with central console | Auxiliary screen surfaces |
| Folding crew seat | Create 1 representative asset and place 4 | Back, seat pan and number marking |
| Cargo racks and restraints | Review existing approved racks first | Shelves, cargo positions and restraint visuals |
| Beacon dock | Reuse existing beacon; custom-fit the dock | Equipment mounting position and status light |
| Baton dock | Reuse existing baton; custom-fit the dock | Equipment mounting position and status light |
| Lights, wiring and vents | Use a small set of representative parts and materials | Emissive surfaces and wiring paths |
| Signs, numbers and wear | Unify through shared textures and separate markings | Meaningful location-specific wear and warnings |

Use Tripo where a draft is useful for a major part with an approved reference image. Build dimension-critical walls, floors, frames and screen openings in Blender. Clean Tripo scale, geometry, UVs and materials before assembly. Preserve the image-review rule when a new design beyond the approved appearance is needed.

## Blender and Unity boundary

- Keep a common-coordinate assembly review scene in Blender; render forward, rear, exterior and sections from the same model. Repeated parts reuse the same source. Avoid pipes and decorations with no visual connection or purpose.
- Track source `.blend`, explicitly exported `.fbx`, textures and production scripts. Do not merge everything into a single mesh; preserve moving parts and screen surfaces. Default to individual reusable part exports and zone-based fixed shell exports.
- In Unity, organize structure, furniture, screens, equipment and doors as nested prefabs beneath the ship. Models provide appearance; prefabs own collision, interaction, lighting and online-state wiring. Check these connections after reimport.
- The central display should use a separate screen surface with dynamic UI rather than text baked into the housing texture. Inspect and reuse the game's existing display implementation, checking language switching, occlusion and legibility.
- Share materials for off-white paint, orange accents, graphite flooring and cyan screens. Do not make unique high-resolution textures for every screw. Establish PSX appearance through consistent pixel density and restrained detail; do not conceal modeling defects with dark lighting.

## Sequence and gates

1. **Structure trial:** Build plain shell, flat floor, central console and rear exit. Check window sightlines, headroom, equipment interaction positions and actual carried-cargo volume. Test basic entry in Unity first.
2. **Representative section:** Bring only the central console and adjacent wall/floor to final-candidate quality. Compare texture, text size, edges and lighting at actual first-person distance. Apply that quality standard to remaining parts.
3. **Complete assembly:** Add seats, racks, docks, lights and doors, then render both interior directions and rear exterior from the same model. Check equipment/cargo access and the full door/ramp motion range.
4. **Game validation:** Connect entry, carrying, screens, equipment pickup and door state to gameplay. Check state agreement in actual multiple processes, then test 4-player entry, passing and performance.

- [ ] Flat floor, windows and headroom coexist inside the actual hull.
- [ ] Employees and carried cargo pass the rear entrance, open doors and ramp.
- [ ] Visually review the representative section with the actual first-person camera.
- [ ] Preserve materials, screens, collision and interactions after model reimport.
- [ ] Verify multiple-process state agreement and 4-player entry/passing.

Automated dimension, UV, missing-data and round-trip checks provide structural evidence. Record handling, legibility and crampedness separately through actual play. This task only inspected documents and sources; all gates remain incomplete.

## References

[Unity prefabs](https://docs.unity3d.com/6000.0/Documentation/Manual/Prefabs.html) · [Unity model file formats](https://docs.unity3d.com/6000.0/Documentation/Manual/3D-formats.html) · [Blender FBX](https://docs.blender.org/manual/en/latest/files/import_export/fbx.html). Official documentation supports capabilities, not this project's quality or completed integration.

## 2026-09-14 — Tripo in-app browser check

Prefer the Codex in-app browser over Chrome following the user's preference. Accessed the authenticated workspace, balance 750, existing ship model and GLB/FBX export menus. Tested both export formats on the existing model, but each download event timed out after 20 seconds and no new file was confirmed in Downloads. Creation-interface control is verified; new generation, upload and completed file saving are not. Do not infer permanent lack of browser support. No generation credits were spent and existing files were preserved. Download completion is the next validation item.

## 2026-09-14 — Download confirmed through filesystem polling

This section updates the previously unconfirmed local-save status. Exported the existing ship as FBX in the in-app browser and confirmed a new ZIP in Downloads. Browser events were not the success criterion. The exact cause of earlier failures remains undetermined; this records successful downloading with the current filename and attempt.

- File: NR_Flatbed_IAB_Check_20260914.zip in Windows Downloads, 4,856,994 bytes.
- Polled every 2 seconds; size and modification time matched for 2 consecutive comparisons, with no new partial download remaining. Completion was detected after approximately 4.05 seconds.
- ZIP CRC passed. Also verified binary FBX and JPEG texture signatures. Blender reimport and new art quality review are outside this task.
- SHA256: 25ce888b4483c25de862561a18b5b947148cac51f0d12265e9e261a1f0b32423.
- No new generation or credit spending. Kept the download in Downloads without overwriting production sources.

### Reusable procedure

Run [watch_download.py](../../tools/watch_download.py) from the Windows terminal. Set a task-specific export name in Tripo and save a directory snapshot before clicking download.

~~~powershell
python tools/watch_download.py --directory "$env:USERPROFILE/Downloads" --prefix NR_Asset_Check --state "$env:TEMP/nr-download-state.json" --prepare
~~~

Immediately after clicking Export in the in-app browser, run:

~~~powershell
python tools/watch_download.py --directory "$env:USERPROFILE/Downloads" --prefix NR_Asset_Check --state "$env:TEMP/nr-download-state.json" --timeout 120 --interval 2
~~~

Change directory if the actual save folder differs. Existing files are excluded; only new files with the specified prefix are candidates. New .crdownload or .part files block success. Stable ZIPs receive CRC and model-entry checks; GLBs receive header version and total-length checks. The tool currently accepts ZIP or GLB, not standalone FBX. If no completion is confirmed within 120 seconds, return TIMEOUT and exit code 1 without recording success. Do not commit temporary snapshots or download logs.

Automated cases passed for ignoring existing files, blocking partial downloads and accepting a new stable ZIP. Directory polling verifies saved downloads; it does not automatically fix site errors or different save locations.

2026-09-14 latest: [Structure trial 03](ship-interior-trial.en.md). Cabin height 3.12m with Space empty-hand jumping. No head collision at 9 jump positions; 3 passage routes and Windows build passed. Obtain human spatial review before art production through the [new pipeline](art-structure-first.en.md).

# FLATBED interior structure trial 01

[한국어](ship-interior-trial.ko.md)

2026-09-14 · **Blender production and sampled geometry checks complete / Unity validation blocked.** This is a spatial trial, not final art. Existing exterior, main game scene and build are preserved.

## Plan and outcome

1. Inspected the existing Angular hull and runtime employee/cargo dimensions.
2. Created a separate candidate with a flat deck, central display console, auxiliary consoles, 4 seats, rack/dock placeholders and rear doors/ramp.
3. Rendered forward, rear, windshield and rear exterior views of the same model; checked sampled passage and GLB round-trip import.
4. Prepared isolated Unity scene generation, passage checks and build code, but execution stopped at license validation. Compilation, scene generation and executable creation are not complete.

## Deliverables

- [Blender source](../../art/ship-flatbed-01/interior-blockout/Flatbed_InteriorTrial.blend) · [FBX](../../art/ship-flatbed-01/interior-blockout/Flatbed_InteriorTrial.fbx) · [GLB](../../art/ship-flatbed-01/interior-blockout/Flatbed_InteriorTrial.glb)
- [Forward interior](../../art/ship-flatbed-01/interior-blockout/forward.png) · [Rear interior](../../art/ship-flatbed-01/interior-blockout/rear.png) · [Windshield view](../../art/ship-flatbed-01/interior-blockout/window.png) · [Rear exterior](../../art/ship-flatbed-01/interior-blockout/exterior-rear.png)
- [Generation script](../../art/ship-flatbed-01/interior-blockout.py) · [Independent round-trip check](../../art/ship-flatbed-01/verify-interior-blockout.py) · [Geometry results](../../art/ship-flatbed-01/interior-blockout/validation.json) · [Round-trip results](../../art/ship-flatbed-01/interior-blockout/roundtrip.json)

## Dimensional decisions

The previous fabricated ceiling underside was 2.75m high, while sampled central hull roof heights were approximately 3.30–3.48m. See the [probe script](../../art/ship-flatbed-01/probe-interior.py) and [object bounds](../../art/ship-flatbed-01/interior-probe.json). Use that space without deforming the exterior: new ceiling underside 3.12m, uniform cabin floor 1.00m. This is not simply deleting stairs above the previous low cargo deck. Cabin height is 2.12m and windshield eye elevation is 2.57m. These are candidate trial values, not final release dimensions.

Used employee height 1.8m, radius 0.34m, eye height 1.57m and cargo size 0.8×0.65×0.65m from the [current carrying code](../../NoReturns/Assets/_NoReturns/Runtime/CarryLab/CarryRoom.cs). Rear frame clearance is approximately 1.86m above the deck, lower than the cabin. The exterior ramp spans a horizontal run of 3m and rise of 1m. This is a separate open structural candidate, not a final mechanism reusing existing door/ramp animation.

## Verified scope and remaining issues

- Exterior vertex, face and material-index fingerprints match before/after. Existing Angular/Integrated files were not overwritten.
- Employee capsule sphere samples on center and side passage lines found no collision samples. A conservative enclosing sphere for cargo also found no central passage collisions. These checks do not guarantee floor support, sliding, actual rotating cargo or every continuous position.
- Side horizontal eye rays at the cockpit do not meet opaque hull faces. The central ray meets the existing window frame. The render also shows that central frame; do not mark it resolved.
- GLB round-trip triangles match at 12,711; missing UVs 0, nonfinite coordinates 0. Screen surface and exterior ramp remain independent objects.
- Walls, seats and racks are plain structural parts, not the approved concept's final finish, handles, wiring or texture density. Display text is a render example rather than dynamic gameplay UI.

## Unity preparation and blocker

Added [scene generation/build code](../../NoReturns/Assets/_NoReturns/Editor/ShipInteriorTrialBuild.cs) and [trial controls](../../NoReturns/Assets/_NoReturns/Runtime/ShipInteriorTrial.cs). Stored a dedicated FBX and stable .meta identifiers. Intended scene: `Assets/_NoReturns/Scenes/ShipInteriorTrial.unity`; intended executable: `builds/ShipInteriorTrial/NoReturns-InteriorTrial.exe`. **Neither output has been generated yet.** Existing CarryRoom and main game build settings are unchanged.

Intended controls are WASD/mouse, E pick up, Q drop, F1 language switch and Esc release cursor. This independent trial does not replace production carrying code or include online tests. Automated checks are intended to move a real CharacterController along 3 passage lines. Unity code, material conversion, collision and controls have not been compiled or executed yet.

Failure evidence: Unity exited with code 198 and `No valid Unity Editor license found` before import. Automatic approval review rejected a user-environment retry because the license prerequisite had not been shown resolved. Subsequent read-only diagnostics found a user login but a stale session; license listing failed to connect to the client. No workaround execution was attempted. Confirm an active license and normal Editor launch in Unity Hub before resuming:

~~~powershell
unity run NoReturns --timeout 600 -- -executeMethod NoReturns.Editor.ShipInteriorTrialBuild.Build -logFile "$env:TEMP/nr-interior-unity.log"
~~~

- [x] Separate structural model and opposite views rendered from the same model.
- [x] Geometry samples and file round-trip checks.
- [ ] Unity compilation, import and actual collision passage checks.
- [ ] Executable generation and human carrying-feel review.
- [ ] User quality review of central window-frame obstruction and low rear entrance.
- [ ] Final art, door animation, dynamic display and 4-player online integration.

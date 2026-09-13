# Approved D representative art trio — game integration

[한국어](10-approved-d.ko.md)

> **Historical record · retired/superseded 2026-09-12.** The [PSX space-delivery design](../current/01-overview.en.md) is current. Implementation, launch, art and values below are historical, not current status. Deleted files remain path records only.


2026-09-10 · D art revision of development build 0.9.4. Design approved on 2026-09-09. This is not release-quality certification for the whole game.

## Applied result

- Worker: retained the supplied Tripo GLB's 41-joint skin and retargeted the existing 18 clips to its proportions. The supplied file contained no animations. Preserve durations in seconds and export linear keys at 60fps. Keep bone lengths and solve carrying wrist targets at body-relative X ±0.40m, forward 0.44m, height 1.00m. These are wrist targets, not measurements validating the entire parcel contact surface.
- Team identification: put a workwear-region mask into COLOR_0 alpha. The dye shader combines this region with color selection to protect faces, gloves and shoes. The first render dyed gloves; the next exposed an incorrect color-channel export that also dyed faces. Explicitly exporting CrewMask corrected this, as verified in the four-team render.
- Sneezer: applied D's cardboard, coral tape, front teeth, three expressions and two hinged lids. Preserve existing sneeze-state and lid-control names. 20,624 triangles, 23 mesh surfaces and one embedded cardboard image.
- Conveyor: authored D's sage frame, charcoal belt, signage and supports. 21,057 triangles and 11 mesh surfaces. The main map uses four 2m modules recessed into the floor, with X scale 1.8 and floor-relative Y -0.874m. Preserve the existing transport zone, lever, collision floor and movement rules; supports sit below the floor. Existing moving stripes indicate operation. Signage and buttons are raised by 0.25m for visibility. The full standalone model remains in authoring sources and the review scene.

## Sources and reproduction

Production folder (`../../art/quality-trio-01/README.en.md`; retired file), integration plan (`../../art/quality-trio-01/integration-plan.en.md`; retired file).

Worker input is `art/quality-trio-01/source/worker-tripo-rigged.glb`; the preserved previous motion source is `worker-before-d.glb` in the same directory. `retarget_worker.py` produces `worker-motion.blend` and `models/worker.glb`; `worker_team_mask.py` supplies the mask. `build_props.py` produces parcel/facility source files and GLBs. Run Blender with `--factory-startup -b --python` to avoid loading user extensions. An initial inspection encountered extension log-path permission errors and was rerun with factory startup.

The game uses worker/sneezer/conveyor_module.glb in `assets/art/release-01/`. `tools/apply_d_conveyor.gd` is a one-time authoring tool that saves only conveyor art changes; it does not run on game startup. Movement rules are unchanged. Preserve the approved image, Tripo input and Blender sources; never store secret keys.

## Validation and remaining work

- [x] Checked evaluated Blender mesh samples across all 18 clips. Minimum Z is approximately +0.004m or higher; carrying wrist target error is below 0.0001m. Evidence: `art/quality-trio-01/models/worker-motion-report.json`. This does not guarantee slope grounding or actual finger contact.
- [x] Generated 962 frames at 60fps of continuous Godot movement/carrying/turning/throwing/jumping and visually reviewed samples. `artifacts/trio-motion.avi`, `motion-d-trio-*.png`.
- [x] Visually reviewed a 241-frame four-team/three-expression render and the main-map conveyor placement. `artifacts/d-trio.avi`, `d-trio-*.png`, `d-map-conveyor.png`.
- [x] Existing 41 behavior checks, 10 two-player scenarios and the four-player scenario passed. The new D structure, animation duration, mask and transport integration check passed separately. Evidence: `artifacts/trio-suite.log`, `trio-final-art-test.log`.
- [x] Final Windows packaging, startup smoke check and 180-frame launch with the actual OpenGL renderer passed. `artifacts/trio-build-final.log`, `trio-packed-render.log`. The environment still emits a certificate-store warning; exit code was 0 with no game script errors.
- [ ] Multiple devices, external networks, extended human co-op, slope foot placement, source gait style, whole-map art consistency and final commercial-use review.

Automated checks and sampled render review are separate from user gameplay quality approval. ANI-01 and ART-01 remain open. Retain 0.9.4 and protocol 12; use matching game/map revisions for co-op testing.


Review follow-up: fixed the new arrow pointing opposite the default transport direction and ignoring reversal. Centered its geometry; scripts/conveyor.gd now reflects the actual direction. Added initial/reversed alignment checks and reran the D art, conveyor and postal-art checks successfully.

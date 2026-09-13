# FLATBED interior 02 — aligned with front glazing

[한국어](ship-interior.ko.md)

2026-09-13 · Interior proposal matching exterior A and its front-glazing revision. Dimensions below are pre-modeling design assumptions, not measurements from the image. No code/model/game-build changes.

![Interior 02](../../art/ship-concepts-01/interior-glazing-aligned-02.png)

## Layout

Single deck: rear ramp → entry → cargo → preparation → forward cockpit. Three-pane front glazing forms the actual cockpit exterior wall; place a low route/supply console beneath it. Remove the old draft’s forward non-habitable compartment. Propose machinery below floor and in external engine pods. Retain automatic travel/landing.

Facing forward, beacon station is left and baton rack right. Two seats per side, 4 total, with clearance planned while folded. Secure cargo along both sides and leave center clear. No sleeping cabin, kitchen or extra deck.

## Dimensional calculation

| Item | Proposal/calculation |
|---|---|
| Closed hull | Length 11.2m × maximum width 10.8m; excludes deployed ramp |
| Width composition | Central exterior 7.2 + side engine pods 1.8 each = 10.8m |
| Main interior width | Central exterior 7.2 - side structural allowances 0.4 each = 6.4m |
| Interior length | Entry 1.4 + cargo 3.0 + prep 1.2 + cockpit 2.0 = 7.6m |
| Nose taper | Width 6.4 → 4.8m over forward 2.0m |
| Unfurnished area | 6.4×5.6 + (6.4+4.8)/2×2.0 = 47.04 square meters |
| Cargo-zone width | Rack 0.8 + access 1.4 + aisle 2.0 + access 1.4 + rack 0.8 = 6.4m |
| Height | Target clear height 2.5m; validate head clearance under nose slope |
| Rear hatch | Width 3.0m × height 2.4m |
| Ramp | Assume length 3.0m, rise 0.6m; about 11.54 degrees, horizontal extension about 2.94m |
| Two employee collision widths | 0.68×2=1.36m; difference from 2.0m aisle is 0.64m |

47.04 square meters is plan area before furniture and sloped-wall intrusion. Structural allowance of 0.4m reserves services/shell space. Length differences alone do not prove components fit; verify glazing height, nose slope and hatch hinges within one 3D block. The image is a layout/atmosphere study, not exact CAD projection or collision evidence. Validate using this document’s values rather than dimension-line placement or image perspective.

## Validation status

Checked length/width sums, tapered area and ramp triangle calculation. Visually reviewed forward glass/console, rear ramp and equipment placement in both directions. Corrected the old 7.6m interior-width typo to 6.4m. Actual mesh measurement and control tests remain undone.

- [ ] Align exterior/interior glass, floor, ceiling, nose and ramp in one block.
- [ ] Verify outside visibility and console obstruction at actual eye height.
- [ ] Test 4-player entry and carrying/passing/placing a 0.8×0.65×0.65m parcel.
- [ ] Verify access with seats unfolded and equipment removed.
- [ ] Finalize production structure after user interior appearance review.

[Exterior selection](ship-concepts.en.md) · [Demo inventory](demo-art-list.en.md) · [Employee/carrying code](../../NoReturns/Assets/_NoReturns/Runtime/CarryLab/CarryRoom.cs).

2026-09-13 review: [Exterior/interior consistency S01~S08](ship-review.en.md). Retain appearance direction; production-structure validation remains incomplete. Dimensional sums do not prove assemblability.

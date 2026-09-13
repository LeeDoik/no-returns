# The Shipping Shrine — map concept and implementation plan

[한국어](2026-09-07-shipping-shrine.ko.md)

> **Historical record · retired/superseded 2026-09-12.** The [PSX space-delivery design](../../current/01-overview.en.md) is current. Implementation, launch, art and values below are historical, not current status. Deleted files remain path records only.


> Documentation update: versions and values here belong to this file's original context. Check the [documentation home](../../README.en.md) first for current rules and outstanding work.

## Direction

For 0.7.2, replace the ordinary night warehouse theme with **THE SHIPPING SHRINE**. A delivery company treats its job with absurd reverence. Original cardboard idols and paperwork stamps provide workplace satire without real religious figures or symbols. Proceed under the user's concept-change request and standing instruction to work without incremental approvals.

A giant kitchen implies cooking systems and an amusement park implies expensive rides. The shrine keeps contracts, packages and A/B delivery immediately understandable.

## Visible requirements

- A huge cardboard boss above the sorting wall: crown, tie, vacant eyes and paperwork stamp. Visible from entry.
- Northern A/B dispatch: enormous hungry faces, mouths and tongue-shaped floor pads. Different colors/expressions with large A/B labels.
- Eastern Packrat nest: cheese crown and employee-of-the-month panel. Rewarding the thief is the joke.
- Intake becomes offering reception; the belt is the promotion track. Deadpan nonsense on company signs. English originals and complete Korean equivalents appear in-game.
- Purple walls, warm cream/gold, teal/pink altars change the silhouette and palette. Keep raised lighting, avoid white-out and flashing effects.

## Scope and rules

Retain 32 × 36 m dimensions, wall and low divider, both carrying routes, dispatch volumes, belt/rat behavior and contract numbers. Keep new decoration out of routes and bay approach space. Mouth animation neither damages workers nor changes dispatch. This is not a new obstacle system or final commercial art. All shapes use Godot primitives, with no new external asset dependencies.

## Implementation and verification

1. Isolate faces, boss, signs and small looping motion in `scripts/shrine_decor.gd`. Instantiate from `depot.gd` and retheme base materials/names.
2. Refresh title copy and version; decoration follows English/Korean switching.
3. Render overhead, entry third-person, altars and rat corner; fix obstruction and brightness.
4. Run existing map route/throw/dispatch, conveyor, Packrat and campaign checks. Verify the exported pack includes decoration and launches.
5. Refresh stable 0.7 executable/ZIP paths; save local Git v0.7.2. Preserve application name so settings/records remain.


## Implementation result

Implemented in 0.7.2. Actual OpenGL Korean/English menus, entry third-person view, overhead, A/B altars and employee corner were inspected in `artifacts/shrine-*.png`. Initial overbrightness was reduced: ambient 0.34, directional 0.52 and point lights 0.4–0.55 at 6.5 m. `test_night_depot`, `test_conveyor`, `test_packrat` and `test_campaign` passed; logs are `artifacts/shrine-*.log`. Windows export/launch and 23 packed gameplay modules were verified. The isolated executable/pack rendered the theme at `build/pack-probe-0.7/artifacts/world.png`. Existing Steam, external-network and independent fun-testing gates remain unfinished.

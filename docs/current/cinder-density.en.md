# CINDER facility density expansion

[한국어](cinder-density.ko.md)

2026-09-21 · CINDER-DENSE-02 · Primitive-based playable layout

## Purpose and launch

The user found the previous demo too open: routes/enemies were visible and tension was missing. The compression proposal was withdrawn. **The 108×86.4m ground and ship dimensions remain while facility interiors and outdoor connections are populated.** [Play_Cinder_Demo.cmd](../../Play_Cinder_Demo.cmd) launches the updated Windows demo. Source `CinderDepotBlockout.unity` and the ship trial remain. Only the current delivery demo scene was revised.

## Spatial composition

| Facility | Interior functions and connections |
|---|---|
| A sorting warehouse | Receiving, sorting, records and dispatch spaces. North/south entrances, east/west service entrances, staggered internal doors |
| West checkpoint | Staff entry, lockers and security room. Links the landing and west alley |
| West returns annex | Processing, archive and separated work spaces. North detour and east exit |
| North workshop | Machine work and maintenance spaces. Links the northern perimeter and side route |
| Dispatch office | Central cross corridor and office/archive spaces on either side. Entrances on all four sides |
| BAY 04 | Reception lobby, inspection room and separated storage. North/south entrances and side service entrances. Floor reception/receipt coordinates retained |
| C plant room | Pump, power and work chambers. Staggered internal doors and east/west exits |
| South bulk storage | Three linked storage compartments. North/south and east/west loading access |
| East cooling station | Cooling, control and utility compartments. Long indoor alternative route |

The central freight court contains cargo islands and additional screening stacks, loading canopies, service conduits, facade columns and roof exhausts. Perimeter maintenance screens have offset openings to interrupt straight, fully visible travel. Authored coordinates reproduce the facility; this is not random prop scattering. Door openings are 3.2m wide and 3.3m high with a 4m ceiling underside. Operable doors, rooftop play and final art are outside this change.

Signs use a depth-tested text shader. Text showing through walls or appearing mirrored from behind was corrected. Roof-hidden overviews are review views; roofs remain in the actual game.

## Listeners and preserved rules

One listener occupies A warehouse, B central freight court and C plant surroundings respectively. Each has independent movement, sound targets and attack warnings; only the navigation graph is shared. Nearby sounds may alert multiple entities. Cargo sounds, beacons, calls and footsteps are evaluated from each location; baton hits check range, facing and occlusion. Complete sound insulation or final inter-zone balance is not claimed.

Shared down state, 2.5-second rescue, four-second rescue protection, six-second baton cooldown and three-second all-down recovery update once. Three enemies do not triple the timers. The host decides A/B/C state and peers display those results. Cinder protocol is **12**, while the previous small trial remains 10. Previous Cinder protocol 11 builds cannot join this version.

Economy, suppression timing, E/Q, receipt collection and outer-creature rules remain. Quiet movement can avoid enemies on the longer lower-risk route, while the central shortcut intersects patrols and sound. Human play must determine whether this layout delivers the intended tension.

## Authoring and validation

`Populate Dense Cinder Facility` in [CinderDenseBuild.cs](../../NoReturns/Assets/_NoReturns/Editor/CinderDenseBuild.cs) rebuilds the current demo's facility group. It overwrites direct edits: save another version or incorporate edits into generation code. Ground, boundaries and ship remain. `Create Cinder Delivery Demo` now also applies this dense layout. Build uses the saved scene.

[CinderDenseValidation.cs](../../NoReturns/Assets/_NoReturns/Editor/CinderDenseValidation.cs) checks connectivity of 29 interior samples, four obstructed long sightlines, routes for a 1.14m-wide clearance probe and actual CharacterController delivery travel. It does not guarantee every cargo rotation or human control feel. [Real delivery automation](../../tools/test_cinder_dense.py) uses the exported `artifacts/cinder-dense/routes.json`. Unlike the empty-handed route check, carrying inputs look down 25 degrees and follow doorway centres, facing the next corridor before sidestepping through corners.

[CinderThreatValidation.cs](../../NoReturns/Assets/_NoReturns/Editor/CinderThreatValidation.cs) checks shared state/timers, baton contact with B and the legacy single-listener regression. `ValidatePatrols` accelerates 300 seconds of simulation over actual collider navigation to check travel and every patrol node. This is not real-time network play. The [two-process hazard test](../../tools/test_cinder_dense_hazards.py) separately checks B listener warning/down/rescue.

See the [validation checklist](05-validation.en.md) for current passes, failures and unknowns. Raw evidence and captures live locally under `artifacts/cinder-dense/`; builds/logs are excluded from Git.

## Next human play

Have two people divide carrying and distraction, trying both the central shortcut and the longer indoor detour. Record whether a building entered during pursuit offers another exit, whether tighter passages make cargo handling frustrating, and whether signs help locate the destination. Check whether occlusion causes unfair surprise attacks. If tension remains insufficient, adjust patrol intersections, sound ranges and contract locations rather than substituting map compression.

Carrying automation adds clearance waypoints around jambs, columns and corners of the empty-handed route. It does not establish that the raw grid path is safe for every parcel orientation.

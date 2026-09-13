# Edit the map without code

[한국어](map-editing.ko.md)

> **Historical record · retired/superseded 2026-09-12.** The [PSX space-delivery design](../current/01-overview.en.md) is current. Implementation, launch, art and values below are historical, not current status. Deleted files remain path records only.


> Documentation update: versions and values here belong to this file's original context. Check the [documentation home](../README.en.md) first for current rules and outstanding work.

For Godot 4.7.2 and NO RETURNS 0.7.8 · September 8, 2026.

## First edit: move the central low wall

1. Close the game and double-click EDIT_MAP.cmd (`../../EDIT_MAP.cmd`; retired file) in the project folder. Godot opens `shipping_shrine.tscn`. Select **3D** at the top.
2. In the left **Scene** tree, select `ShippingShrine → Geometry → LowDivider`. This is the central low wall.
3. Expand **Inspector → Node3D → Transform → Position** on the right. Change X from `1` to `3` and press Enter. The wall moves 2 m to the right. Y is height; Z is depth. You can also drag the colored axis arrows in the 3D view.
4. Select **Solid1** under LowDivider. The upper **Dimensions** X/Y/Z fields control width/height/depth. Changing them updates both the visible wall and collision. To rest on the floor, set Position Y to half its height. Use Dimensions instead of Scale for wall and floor size.
5. Press **Ctrl+S** to save, then **F5** to run the complete game. Start a solo campaign and check the wall position and collision. **F6** runs only the map scene, without workers or the game menu.
6. Return to editing and use **Ctrl+Z** to undo, or change the values again and save. Later runs use the saved scene; runtime generation does not overwrite it.

Start with one wall. `scenes/maps/shipping_shrine.tscn` is the map source. No code editing is necessary. Edit this source scene directly instead of its instance inside `main.tscn`.

## What should I select?

| Scene tree location | Edit | Gameplay effect |
| --- | --- | --- |
| `Geometry/Floor` | Dimensions, position, color | Floor size and collision |
| Walls under `Geometry/OuterWalls` | Position, Dimensions, color | Boundary walls and collision |
| `Geometry/SortingWall`, `LowDivider` | Parent position, child wall Dimensions | Central walls and collision |
| `Geometry/Shelves`, `WorkTables` | Move each rack/table parent | Grouped appearance and collision |
| `Gameplay/DispatchA`, `DispatchB` | Move parent, rotate around Y | Face/tongue and delivery detection |
| `Gameplay/Conveyor` | Move parent, rotate around Y | Belt location, carrying direction and lever |
| `Gameplay/Conveyor/Lever` | Position | Lever appearance and F interaction location |
| `Gameplay/PackratTerritory` | Move parent | Nest, start, return, activity zone and patrol points |
| `Gameplay/WorkerSpawns/Worker1…4` | Position | Worker start and return positions |
| `Gameplay/CargoSpawns/Cargo1…4` | Position | Four cargo start and reset positions |
| `Environment` | Sun, CeilingLight, WorldEnvironment | Directional/ceiling lights, background and ambient light |
| `Decoration` | Position, rotation, appearance | Background presentation |

Moving only `HungryFace` moves only the face. Select the **DispatchA/B parent** to move delivery detection too. Move the **PackratTerritory parent** for the rat nest. Keep bays and belts horizontal and rotate around Y. This is not an editor for sloped belts, an additional bay C or multiple belts.

## Add walls and change colors

Find `scenes/pieces/solid_block.tscn` in the lower-left **FileSystem** dock. Drag it onto `Geometry` in the Scene tree to add a wall. Edit the new SolidBlock's Position, Dimensions and Color in the Inspector. Its default size is 2×2×0.5 m. Select an existing block and press **Ctrl+D** to duplicate it. This block includes collision.

Importing a decorative mesh alone does not automatically create collision. Reuse this block for walls, obstacles and platforms initially. A duplicated block's size and color change independently. For multi-part objects such as racks, move their parent and inspect the constituent parts when resizing.

## Delivery zones, rat behavior and map expansion

- Delivery volume: edit `DispatchA/B → DeliveryZone → CollisionShape3D → Shape → Size`. The transparent editor shape is the detection volume. Delivery occurs when a package's center enters it. Enlarging the tongue appearance alone does not enlarge detection.
- Rat: `PackratTerritory/StartPoint` is its initial position; `ReturnPoint` is where stolen cargo is dropped. Keep the return point clear of walls and nest decorations. `PatrolPoints/Point1…4` are visited in tree order. `ActivityZone/CollisionShape3D/Shape/Size` controls the movement/search region. Packrat appears from contract 2. Its current AI has no complex wall-routing pathfinder, so connect waypoints through open passages.
- Spawns: default worker Y is `0.05`; cargo Y is `0.55`. Adjust both if floor height changes, and avoid overlapping workers or packages.
- Expansion: enlarge Floor Dimensions and move OuterWalls first. Then adjust `PlayableBounds/CollisionShape3D/Shape/Size` and position to include the new area. Cargo outside these bounds resets. The default floor is 48×60 m, with some extra margin in the detection bounds. Adjust lights, spawns and patrol points to match.
- Belt: default size is 2.2×6 m. Prefer moving/rotating the whole object. Changing length requires matching its bed, stripes and TransportZone; the stripe animation length remains in code.

## Signs and lighting

Select a sign's Label3D and edit both **English / Korean** fields near the top of the Inspector. **Text** is generated display content; editing only that field is overwritten. The editor displays Korean, while the game uses the selected language. Keep English as the source for new production copy.

Raise a ceiling light's Position Y or reduce its Light Energy to reduce glare. The default ceiling lights are 6.5 m high. Expanding only the floor may leave dark areas. Small face/boss animations run only in the game and use the saved placement as their starting pose.

## Play your saved map with friends

**Ctrl+S → F5** tests the latest source map. An existing EXE does not update automatically. Close game windows and double-click BUILD.cmd (`../../BUILD.cmd`; retired file). Successful checks display the new EXE and ZIP paths. This uses Python, Godot and export templates already prepared on this PC. For another PC, see [build handoff](../steam/07-build-handoff.en.md).

Send the new `build/NO_RETURNS_0.7_Windows.zip` to friends and have everyone extract the same ZIP. Do not mix source runs with exported builds. Network protocol 9 compares fingerprints of the saved map and dependent resources, rejecting a mismatch. Automatic map downloads and live editing replication are not supported.

## Troubleshooting

- Placement unchanged in game: you may have edited another scene or not saved. Save `shipping_shrine.tscn` and restart the running game. For the EXE, also run BUILD.cmd.
- No workers: use F5 instead of F6, then start a campaign from the menu.
- Walking through walls: check whether the object is decoration without collision and use SolidBlock. Change Dimensions rather than only enlarging its child mesh.
- Rat stuck: inspect the routes between patrol and return points. It does not automatically find complex routes around walls yet.
- Yellow warning or runtime error: do not rename/delete required items such as `Gameplay`, bays, belt or spawns. The root provides warnings for missing required items. Undo the recent edit with Ctrl+Z and save.

Before large changes, copy the scene outside the project as a backup or record it in Git. See [local version control and recovery](version-control.en.md). Avoid resetting unrelated project files.

## Implementation and validation

The runtime-generated map is now a saved PackedScene. Appearance and gameplay detection read the same scene. The previous generator remains only for one-time conversion and older tests; normal gameplay does not use it. Automated checks cover translated/rotated bays and belts, save/reload, worker/cargo spawn integration, actual delivery earnings, matched block mesh/collision dimensions and instance independence, and rejection of online guests with another map.

The [official Godot PackedScene reference](https://docs.godotengine.org/en/4.3/classes/class_packedscene.html) describes scene saving, instantiation and ownership-based persistence. See the [conversion plan](../superpowers/plans/2026-09-07-editable-map.en.md).


## 0.7.4 Expanded map and new mechanisms

The floor is now **48×60 m**, **2.5 times** the previous area. Dispatch is at the northern end, X±15, Z-43. After the existing intake and rat territory, gather or pass packages at the central relay lounge, then choose a route.

| Route | Behavior and choice |
| --- | --- |
| Left: Scenic Overtime | A permanently open bypass for carrying without mechanism timing. |
| Center: Unpaid Doorman | Stand on either yellow plate or place a free package on it to open the door. It stays open for 6 seconds after leaving, allowing solo passage. Workers/packages inside prevent closure. |
| Right: Express Air Mail | 5 seconds idle, 1.5 seconds amber warning, 2.5 seconds cyan gust. Pushes grounded workers and free cargo in the arrow direction. Send a package ahead for another worker to catch. Held, creature-claimed and attached cargo are not independently pushed. |

No new key is required: simply stand on a plate. Sidestep the airflow or wait for it to end; airborne workers do not receive the grounded wind drift. Existing F conveyor reversal and R rat horn remain. New contracts reset the gate and gust clock; lobby/results do not advance them. Contracts remain 240 seconds. Tune difficulty and fun after human play over the longer routes.

To edit, select `Gameplay/RouteChallenges`. Change Gate Hold Seconds, Wind Idle/Warning/Burst Seconds and Worker/Cargo Wind Speed in the Inspector. Move the **Gate parent** to move its door, plates, anti-crush zone and signs together. Move or rotate the **AirMail parent** around Y to change its airflow volume, direction and signs together. To move an individual plate, select Gate/PlateFront or PlateBack. Adjust WindZone/CollisionShape3D Shape/Size for the airflow region.

`Gate/Status` and `AirMail/Status` are dynamic runtime labels. Edit English/Korean on the other signs. Duplicating additional gates or gust mechanisms is not supported yet: this structure edits the placement/settings of the existing pair. Preserve Door/CollisionShape3D, Clearance and PlateFront/Back names under Gate.

Rebuild edited executables with BUILD.cmd. **Protocol 9** requires everyone to run the same 0.7.4 ZIP. [Expansion design and validation](../superpowers/plans/2026-09-08-shrine-expansion.en.md).


## 0.7.5 Winding alleys and cross-links

The map stays 48×60 m, replacing the three parallel lanes. The left S alley passes the angled entry, outside the lost-property wall, then inside the northern screen. The right route enters behind a screen, travels sideways through the airflow and turns inward at the northern corner. Cross-links at Z-26 before the gate and Z-30 behind it allow mid-route changes. The central pressure gate remains.

AirMail is now at (15,0,-24), Y rotation 90 degrees. Wind blows toward the central cross-link in -X. Wait out or avoid the gust to reach the outer bypass. Use arrows and colored lines to read corners. Intake, dispatch, rat territory, mechanism timing and contract duration remain unchanged.

Move grouped new walls under `Geometry/WindingAlleys`. Edit child SolidBlock Dimensions/Color for size/color. LeftLoop, RightLoop, FrontCrossLink and RearCrossLink under `DesignRoutes` are design/test references, not forced navigation. Update reference points after moving walls. Moving only a waypoint does not automatically move walls.

[Full change plan](../superpowers/plans/2026-09-08-winding-shrine.en.md). Use the same 0.7.6 ZIP. Protocol remains 9; saved-map fingerprints prevent mixing old layouts.


## 0.7.5 Editing interior props

Added 29 reception/packing/lounge/archive clusters, 96 shelf parcels, 18 wall posters and overhead pipes/pennants. Move furniture groups under `Geometry/InteriorDressing`. Shelf stock follows its rack under `Geometry/Shelves`. Pipes are under `Decoration/InteriorOverhead`; posters under `Decoration/WallPosters`. Edit both English/Korean sign fields.

Floor furniture and stacked parcels are fixed collision props. Decorative vending cabinets and stamps do not provide new E interactions. They are separate from deliverable cargo. After moving large furniture, use F5 to check corners, plates and airflow clearance.

## 0.7.6 Editing the new rooms

Under `Geometry/EnclosedRooms`, select `IntakeRoom`, `LostArchive`, `DoormanRoom`, `AirMailRoom` or `DispatchA/B`. Child walls and roofs expose Dimensions and Position. Furniture remains under `Geometry/InteriorDressing`. Moving a room parent does not move existing furniture, delivery zones, plates or wind devices; move related groups together.

The west intake hatch is between `HatchSill` and `HatchHeader`. Its opening is 3m wide and 1.15m high, from 1.8 to 2.95m above the floor. THROW/CATCH floor marks match the actual default throw arc. Re-test throws after changing hatch height or dimensions. The doorman roof is at 8.2m to clear the raised gate. Standard partial roofs are at 5.7m. Fill lights are the corresponding roof-name Light nodes.

## 0.7.7 Art direction pass

Edit wall finishes under each wall’s `SurfaceFinish` and roof battens under `RoofBattens`. These are decorative and have no colliders. Moving a wall moves its finish, but changing Dimensions requires manually adjusting finish lengths/positions. A sign’s `SignBacking` is its panel; resize it when changing text length or font size. Old hidden signs remain in the scene and can be restored using Visibility > Visible.

Subtle wall/floor grain uses a saved NoiseTexture2D. Keep its contrast low. The migration tool has already been applied and is never re-run by normal play or BUILD.cmd.

## 0.7.8 Editing reception and worktop props

Edit counter details at `Geometry/LowDivider/ReceptionCounter`, and each furniture group's new work surface at its `AuthoredWorktop` under `Geometry/InteriorDressing`. Wall portraits, the clock and loading paint are under `Decoration/AuthoredIntake`. Moving a parent moves its attached details.

Select a SolidBlock and adjust **Edge Bevel** to control edge chamfering. Zero restores the original box shape. **Dimensions** updates both mesh and collision size; **Color** changes the color. Collider corners remain square. After resizing a furniture body, manually adjust its worktop, drawers and attached props.

Invoices, tape, scales, printers and stamps are decorative. The stopped office clock does not display contract time. Hidden old boss geometry and mats remain in the scene. The new `tools/author_intake.gd` migration has already been applied once and does not run during gameplay or builds. Share the same rebuilt 0.7.8 package after editing.

## Editing the 0.8.0 sorting line and reactive props

`Geometry/SortingLine` contains the split wall, parcel header, 0.6 m sorting lip and launch line. `Gameplay/Conveyor` is the eight-metre belt at (0,0,-11). To change its length, adjust both the `Bed` mesh and `TransportZone/CollisionShape3D`. Direction, lever and stripes use the belt transform. Keep both staff doors accessible because workers cannot use the parcel opening.

`Gameplay/PackratTerritory` is at (6,0,-18). `PaperNest` is the visible desk/paper nest; `StartPoint` is the spawn; `ReturnPoint` is retrieved-cargo placement; `PatrolPoints` define patrol order; `ActivityZone` bounds target searches. Leave cargo-sized clearance at the entrance and retrieval position.

Eight groups under `Gameplay/Reactions` contain 24 props between intake and the final handoff. `Paperwork` contains 13 replacements for existing documents. Select a prop, duplicate with Ctrl+D, then move or rotate it around Y. Inspector **Kind** offers Packing cushion, Return spring, Carton tower and Paper stack. **Trigger Radius** controls entry range; **Effect Radius** controls impulse range; **Lift Speed** controls upward velocity; **Reset Seconds** controls recovery time. Paper only responds to sneezes and creates no impulse, so the first three numbers do not affect paper.

Place floor props at origin Y=0 and documents just above the desk surface. Springs push slightly along local -Z: aim opposite the blue axis toward the receiving area. Keep the default scale. Type-specific geometry appears in the editor; do not edit generated Preview children directly. Verify restoration after restarting a shift.

`connect_sorting_line.gd`, `populate_reactive_props.gd` and `animate_paperwork.gd` are already-applied, one-time migrations. Do not rerun them while editing. BUILD.cmd packages the saved map without automatic rearrangement. Protocol 10 requires everyone to play the same new ZIP.

# NO RETURNS

[한국어](README.md)

> The packages are alive. Ship them anyway.

**Current version: Shipping Shrine playtest 0.7.7 · September 8, 2026**

A Godot third-person delivery game for solo play or 2–4 cooperating workers. Complete three contracts in an absurd shrine that receives parcels as offerings, buy equipment with shared earnings, relay packages and recover cargo stolen by Packrat. The target is a Windows Steam release; this is a development build using primitive art.

## Run now

**Double-click [NO_RETURNS.exe](build/NO_RETURNS_0.7/NO_RETURNS.exe) → START SOLO CAMPAIGN.** Close older windows first. No editor is required. Share the [Windows ZIP](build/NO_RETURNS_0.7_Windows.zip) with friends and extract the entire archive.

In the development project use [PLAY.cmd](PLAY.cmd), [PLAY_TWO.cmd](PLAY_TWO.cmd) for two local windows, or [PLAY_FOUR.cmd](PLAY_FOUR.cmd) for four. One worker hosts the campaign; others enter its address, join, and wait for the host to start. The same-PC address is `127.0.0.1`. [EDIT.cmd](EDIT.cmd) opens the editor.

## What changed in 0.7

- A complete run of three sequential contracts. Base duration is 240 seconds each, with quotas increasing by crew size and stage.
- Correct delivery earns 10 credits, relay adds 5, contract success adds 20. Spend the shared bank on boots, time extensions and a horn upgrade, then ready unanimously for the next contract.
- A relay requires another worker to catch in flight within three seconds and at least 3 m from release. Wrong deliveries, repeated pickups and self-catches cannot farm bonuses.
- Packrat steals cargo in contracts 2–3. Respond to its warning, use R to scare it, or recover the package at its nest.
- Staged control hints, H help, contract/bank/readiness/equipment results, Korean/English UI, volume/FOV/fullscreen settings and local completed-run records.
- Incompatible guests are rejected; campaign and creature state replicate separately. The host judges gameplay rules and rewards.

The existing 32 × 36 m Night Shift Depot, two bypasses, right-lane reversible conveyor, four cargo types, Q pings and three-minute practice remain. Lights are raised to 6.5 m, and the belt previously between partitions is on the right carrying route.

WASD moves, mouse looks, Space jumps, E picks up/catches/puts down, left click throws, Q pings, F reverses the belt, R horns, H opens help and Esc opens the menu. Match cargo A/B labels to dispatch bays.

## Design and guides

| Korean | English |
| --- | --- |
| [33-section detailed design](docs/superpowers/specs/2026-09-07-release-design.ko.md) | **[Detailed design](docs/superpowers/specs/2026-09-07-release-design.en.md)** |
| [0.7 full play guide](docs/prototype/07-contracts.ko.md) | [Full play guide](docs/prototype/07-contracts.en.md) |
| [0.7 implementation and evidence](docs/superpowers/plans/2026-09-07-release-polish.ko.md) | [Implementation and evidence](docs/superpowers/plans/2026-09-07-release-polish.en.md) |
| [Roadmap and enrollment](docs/steam/01-release-roadmap.ko.md) | [Roadmap](docs/steam/01-release-roadmap.md) |
| [Store draft](docs/steam/02-store-page.ko.md) | [Store draft](docs/steam/02-store-page.en.md) |
| [Assets and trailer](docs/steam/03-assets-and-trailer.ko.md) | [Assets and trailer](docs/steam/03-assets-and-trailer.en.md) |
| [Online and build plan](docs/steam/04-online-and-build-plan.ko.md) | [Online plan](docs/steam/04-online-and-build-plan.md) |
| [Release checklist](docs/steam/05-release-checklist.ko.md) | [Checklist](docs/steam/05-release-checklist.md) |
| [Creator kit](docs/steam/06-creator-kit.ko.md) | [Creator kit](docs/steam/06-creator-kit.en.md) |
| [Windows build and Steam handoff](docs/steam/07-build-handoff.ko.md) | [Build handoff](docs/steam/07-build-handoff.en.md) |

The detailed design separates official reference facts from our interpretation. It covers flow, crew-scaled values, rewards/equipment, relay validation, creature states, server ownership, saves, UI/audio, 18 QA areas, independent tester experiments and release gates. Earlier plans/version documents remain historical records; the new design and implementation ledger control conflicting 0.7 values.

## Validation and remaining release work

Twenty-four behavior tests, eight real two-process scenarios, the four-process scenario and the SteamPipe generator test passed. Coverage includes campaign completion, shared purchases/readiness, theft/horn rescue, actual-flight relay, settled-floor cargo transport to the nest, first-launch settings and duplicate/late-join completion records. Worst tested motion snapshot is 1,280 bytes; separate campaign metadata stays ≤2,048 and creature state ≤256 bytes.

**This is not a Steam release.** Steamworks enrollment and real App/Depot IDs are unavailable. Current transport is direct-address ENet with the host PC as server; Steam friend invites and relay are absent. Separate PCs/external networks, target-hardware performance, independent friend-group fun testing, final art/store imagery/trailer/rights review, Steam installation and Valve review remain open. Automated passes do not establish fun or internet connection quality.

## Version control

Local Git retains the 0.5/0.6 baselines. This change is identified by `v0.7.7`; no GitHub remote is connected. See [version history and recovery](docs/development/version-control.en.md) and [documentation rules](AGENTS.md).


## 0.7.1 Packrat fix

Fixed ignoring nearby visible cargo: search within 6 m in the right lane, approach, then warn within 1.4 m before stealing. Active from contract 2. Existing 0.7 executable/ZIP paths now contain the fix; the settings folder is unchanged. [Fix record](docs/development/2026-09-07-packrat-search.en.md).


## 0.7.2 The Shipping Shrine

A giant cardboard boss watches over the rethemed shrine. A/B bays are hungry faces, and the rat nest is an employee-of-the-month display. Intake receives offerings and the belt is the path to promotion. Added bilingual deadpan signs, purple/gold surroundings and small decorative motion. Both carrying routes and delivery rules remain. [Concept and implementation record](docs/superpowers/plans/2026-09-07-shipping-shrine.en.md).


## 0.7.3 Visually editable map

Double-click **[EDIT_MAP.cmd](EDIT_MAP.cmd)** to open the editable map. Place walls, lights, bays, the belt, rat territory and spawns, then Ctrl+S → F5 to test. The wall prefab keeps mesh and collision dimensions aligned. **[Map editing guide](docs/development/map-editing.en.md)** covers your first wall move through map expansion.

Before sharing with friends, rebuild using **[BUILD.cmd](BUILD.cmd)** and use the same ZIP. Protocol 8 rejects different maps. The saved map is not regenerated at runtime.


## 0.7.4 A larger Shipping Shrine

Expanded to **48×60 m, 2.5 times the previous area**. Carry to the farther dispatch courtyard and stage or relay packages at the lounge. The left route is always open; the central shortcut uses worker/package pressure plates; the right express route warns before gusts push cargo and workers. Six seconds of gate grace allow solo passage.

Edit the new mechanisms' placement, direction, timing and strength in Godot. See the [editing guide](docs/development/map-editing.en.md) and [expansion design/validation](docs/superpowers/plans/2026-09-08-shrine-expansion.en.md). Protocol 9 requires all friends to use the same new ZIP.


## 0.7.5 Winding alleys

The left route is an S alley; the right turns through transverse airflow. Cross-links before/behind the gate allow mid-route changes. Replaced straight guidance with turning arrows/colored lines and screens that reveal spaces around corners. New walls and reference waypoints remain editable. [Change plan and validation](docs/superpowers/plans/2026-09-08-winding-shrine.en.md).

Furnished the interior with 29 reception/packing/lounge/archive clusters, 96 shelf parcels, 18 wall posters and pipes/pennants. Props remain editable; fixed furniture has collision.

## 0.7.6 Enclosed rooms and parcel hatch

Added walls and partial roofs to intake, lost-property archive, doorman waiting room, air-mail passage and A/B dispatch rooms. Doors and corners interrupt sightlines while side bypasses and both gate cross-links remain open. Throw toward the west intake hatch from the **THROW** floor spot; a teammate on the outside **CATCH** spot can press E for a relay attempt. Workers cannot fit through the hatch.

[0.7.6](docs/superpowers/plans/2026-09-08-enclosed-shrine.en.md).

## 0.7.7 Art direction pass

Reworked backgrounds around worn cream, green-gray and concrete tones. Reduced visible map labels from 104 to 38 and mounted essential guidance on backing panels. Added kick plates, panel joints, roof battens and subtle materials. Workers now wear work clothes, caps, gloves, boots and employee numbers, with alternating arm/leg motion. Refined cargo colors and distant text. Controls and delivery rules are unchanged.

[0.7.7](docs/superpowers/plans/2026-09-08-art-direction.en.md).

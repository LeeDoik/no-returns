# NO RETURNS

[한국어](README.md)

> The packages are alive. Ship them anyway.

**Current version: Cooperative Contracts playtest 0.7 · September 7, 2026**

A Godot third-person delivery game for solo play or 2–4 cooperating workers. Complete three contracts in one depot, buy equipment with shared earnings, relay packages and recover cargo stolen by Packrat. The target is a Windows Steam release; this is a development build using primitive art.

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

Twenty-one behavior tests, six real two-process scenarios, the four-process scenario and the SteamPipe generator test passed. Coverage includes campaign completion, shared purchases/readiness, theft/horn rescue, actual-flight relay, settled-floor cargo transport to the nest, first-launch settings and duplicate/late-join completion records. Worst tested motion snapshot is 1,280 bytes; separate campaign metadata stays ≤2,048 and creature state ≤256 bytes.

**This is not a Steam release.** Steamworks enrollment and real App/Depot IDs are unavailable. Current transport is direct-address ENet with the host PC as server; Steam friend invites and relay are absent. Separate PCs/external networks, target-hardware performance, independent friend-group fun testing, final art/store imagery/trailer/rights review, Steam installation and Valve review remain open. Automated passes do not establish fun or internet connection quality.

## Version control

Local Git retains the 0.5/0.6 baselines. This change is identified by `v0.7.0`; no GitHub remote is connected. See [version history and recovery](docs/development/version-control.en.md) and [documentation rules](AGENTS.md).

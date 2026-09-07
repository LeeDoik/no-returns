# NO RETURNS

[한국어](README.md)

> The packages are alive. Ship them anyway.

**Current version: Night Depot playtest 0.6 · September 7, 2026**

A Godot 3D cooperative game where 2–4 workers ship living packages. Primitive visuals support rule validation; Windows on Steam is the release target. English is the source for game copy, with Korean as the default UI and documentation entry point.

## Play now

**Double-click [NO_RETURNS.exe](build/NO_RETURNS_0.6/NO_RETURNS.exe) → Practice Alone.** Close previous windows first. No editor installation is required. Share the complete [Windows ZIP](build/NO_RETURNS_0.6_Windows.zip) with testers.

In the development project use [PLAY.cmd](PLAY.cmd), [PLAY_TWO.cmd](PLAY_TWO.cmd) for two local instances, or [PLAY_FOUR.cmd](PLAY_FOUR.cmd) for four. Once the crew count is ready, start the shift in the host window. [EDIT.cmd](EDIT.cmd) opens Godot.

## Implemented in this slice

- 32 × 36 m night depot with a high central sorting partition and permanently open side carrying routes. Single intake-to-dispatch throws are blocked.
- F-reversible conveyor, shelves, shutters, lighting and a Packrat nest placeholder. Creature AI is not implemented.
- Standard, Sneezer, Clinger and Hopper cargo. Holding a Hopper pauses its cycle; aim and put it down to cross the low divider.
- Initial A shipments followed by balanced A/B destination variations. The three-minute quota is 5/6/8/10 for the respective crew sizes.
- Q pings, results, and a 60-second bonus shift after unanimous agreement. Bonus failure does not undo base success.
- Korean/English UI, saved cue sound/mouse sensitivity/language settings, Windows executable and packaging automation.

WASD moves, mouse looks, Space jumps, E picks up/catches/puts down, left click throws, Q pings, F reverses the conveyor, and Esc opens the menu. Match the cargo's **A / B label**.

## Guides and release preparation

| English | Korean |
| --- | --- |
| [0.6 play guide](docs/prototype/06-night-depot.en.md) | [플레이 안내](docs/prototype/06-night-depot.ko.md) |
| [Map plan and completion record](docs/superpowers/plans/2026-09-07-night-depot.en.md) | [구현 계획](docs/superpowers/plans/2026-09-07-night-depot.ko.md) |
| [Design](docs/superpowers/specs/2026-09-06-no-returns-design.md) | [기획서](docs/superpowers/specs/2026-09-06-no-returns-design.ko.md) |
| [Equipment and Packrat design notes](docs/superpowers/specs/2026-09-07-depot-creature-design.en.md) | [창고 설비·포장쥐 제안](docs/superpowers/specs/2026-09-07-depot-creature-design.ko.md) |
| [Roadmap and enrollment](docs/steam/01-release-roadmap.md) | [로드맵](docs/steam/01-release-roadmap.ko.md) |
| [Store draft](docs/steam/02-store-page.en.md) | [상점 초안](docs/steam/02-store-page.ko.md) |
| [Assets and trailer](docs/steam/03-assets-and-trailer.en.md) | [이미지·트레일러](docs/steam/03-assets-and-trailer.ko.md) |
| [Online and build plan](docs/steam/04-online-and-build-plan.md) | [온라인 계획](docs/steam/04-online-and-build-plan.ko.md) |
| [Release checklist](docs/steam/05-release-checklist.md) | [체크리스트](docs/steam/05-release-checklist.ko.md) |
| [Creator and tester kit](docs/steam/06-creator-kit.en.md) | [스트리머 자료](docs/steam/06-creator-kit.ko.md) |
| [Windows build and Steam handoff](docs/steam/07-build-handoff.en.md) | [빌드 인계](docs/steam/07-build-handoff.ko.md) |

## Evidence and remaining scope

Seventeen behavior tests, four real two-process scenarios, the four-process scenario and the SteamPipe generator test passed. Checks cover worker/held-cargo routes to both bay centers, intake-throw first contact, expanded-boundary recovery, conveyor range/facing/line-of-sight/cooldown and guest reversal. The worst tested snapshot is 1,280 bytes, equal to the current limit. Actual rendering verified Korean/English menus and lever prompts, plus overhead, third-person and dispatch/nest views. Windows export and executable smoke checks passed; all 17 gameplay scripts are packed, and practice rendered in a separate folder containing only the executable and game pack.

**This is not a Steam release.** Steamworks enrollment and a real App ID are unavailable. Current transport is direct-IP ENet with one player's PC hosting; Steam invitations and relay are absent. Separate-PC/external-network tests, actual Steam account/install tests, final art, trailer, store publication and review remain incomplete. No enrollment, payment or publication was performed on the creator's behalf. Prepared documents and the preview-only SteamPipe generator support the next work.

Automated success does not validate fun. Play with friends and assess whether accidents are readable, roles emerge and another shift is appealing. The 0.1–0.5 documents remain historical records. Full Korean/English documents are maintained under the [documentation rules](AGENTS.md).

## Version control

Use local Git with `v0.5.0` as the first baseline. Record this 0.6 update separately; no GitHub remote is connected. See the [history and recovery guide](docs/development/version-control.en.md).

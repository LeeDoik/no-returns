# NO RETURNS — Night Depot playtest 0.6

[한국어](06-night-depot.ko.md)

> Documentation update: versions and values here belong to this file's original context. Check the [documentation home](../README.en.md) first for current rules and outstanding work.

September 7, 2026. Windows x64, Godot 4.7.2. Primitive-art development playtest, not a Steam release candidate. English is the original game-copy language; Korean is the default UI.

## Start

In the packaged Windows folder, double-click **NO_RETURNS.exe**. In the development project, use **PLAY.cmd**; **PLAY_TWO.cmd** and **PLAY_FOUR.cmd** launch two or four local instances. Close old game windows first. Choose Practice Alone, or have one player choose Host and others enter the host address. Wait for the crew count, then the host starts. Two to four workers are supported; solo is a practice mode.

The package contains the executable, its `.pck` game data, both guides, third-party notices and SHA256 manifest. Keep them together. No editor installation is required for the exported package. It is unsigned; Windows may display a publisher/reputation warning. Use only the build received from your trusted project source.

## Controls

| Input | Action |
| --- | --- |
| WASD / mouse | Move / look |
| Space | Jump |
| E | Pick up, catch nearby cargo, or put down |
| Left click | Throw held cargo toward the contact marker |
| Q | Mark nearby visible cargo in front, or a point ahead |
| F | Reverse the nearby conveyor lever |
| Esc | Menu; online simulation continues |

Change language on the title menu. Toggle cue sounds on the title or pause menu. Cycle mouse sensitivity from 0.60× to 2.00× in 0.25× steps in the pause menu. These preferences save locally. There is no camera shake. Normal arrows and A / B labels supplement colors.

## Shipments and goals

Match each package's A or B label to the dispatch bay. First cargo destinations are A. After a successful shipment, the returning replacement receives a destination from a host-seeded shuffled bag containing two A and two B entries. The four fixed cargo types remain available; the game varies destination order rather than changing their identities. There are never more than four cargo objects. An ordinary carrying route around the divider always exists.

Wrong-bay or out-of-bounds cargo returns after 1.2 seconds without scoring or changing its destination. A successful shipment scores once. Recovery also breaks attachments and cancels pending behavior. The timer is 180 seconds; targets are **5 / 6 / 8 / 10** for **1 / 2 / 3 / 4** workers. These are initial tuning values, not validated difficulty ratings. The result's shipment seed can be replayed by launching with `-- --seed=NUMBER`; it reproduces destinations, not every physics outcome or Sneezer interval.

## The four packages

- **Standard:** ordinary carry-and-throw cargo.
- **Sneezer:** a 1.5-second warning precedes a short forward blast. It pushes workers and cargo, releases held cargo, and breaks sticky links. The worker carrying the Sneezer is exempt from its own blast. Watch the nose and directional warning. Subsequent calm intervals are 6–9 seconds.
- **Clinger:** sticks to one worker or package within 0.95 m for up to 5 seconds. Attached workers move at 70% speed. Sneeze to detach; detachment has a 2-second cooldown. A linked pair delivers together only when both destinations match the dispatch bay; the mismatched partner returns without a score. No attachment chains.
- **Hopper:** orange cargo rests for 4 grounded seconds, squashes for a 1-second warning, then jumps forward at 2.8 m/s with 6.5 m/s upward speed. Holding pauses its cycle; airborne motion does not trigger another jump. Landing starts another rest. Aim while holding, put it down before the low divider, and let it jump across. Its arrow indicates direction, not a guaranteed landing point.

The ordinary throw marker predicts the first static contact; subsequent cargo behavior can change the final landing position. The Hopper's jump and a Clinger attachment can combine, provided the attachment does not collide with an obstacle.

## Pings and overtime

Q creates one shared four-second marker with the sender's worker number. A sender can refresh it once per second. A newer accepted marker replaces the old one. Cargo markers follow their selected cargo; location markers remain at the chosen point. This is a small coordination tool, not voice chat.

At quota completion, the result shows deliveries, target, elapsed time and shipment seed. Everyone in the current crew must select **Overtime?** before a single 60-second bonus shift begins. Its additional targets are **3 / 4 / 5 / 6** for **1 / 2 / 3 / 4** workers. Base success remains secured even if the bonus times out. The host can instead restart; this resets score, votes, destinations and timers. Overtime cannot be chained.

## Connection limits

One player's game is the authoritative server; no hosted service is provisioned. Development transport is ENet UDP port **27842**. Use `127.0.0.1` for the same PC, the host's LAN address for a local-network test. Internet direct-IP hosting may require router port forwarding and firewall configuration. Steam invitation, NAT relay, host migration and mid-shift joins are not implemented. Do not promise easy internet play from the local test result.

A departing guest releases held cargo and returns the remaining group to the lobby; the host restarts with the available crew. Host departure ends the session for everyone. Join attempts time out after 10 seconds. Use identical 0.6 builds in a party. Localhost multi-process tests do not establish real inter-region latency or separate-network connectivity.

## Night Shift Depot

The map measures 32 × 36 m. Starting at intake, walk around either side of the central 3.4 m sorting partition to reach northern dispatch bays A / B. Bay centers are (-10, -22) and (10, -22), over 20 m from intake cargo. The layout blocks a single intake-to-dispatch throw while both carrying routes remain permanently open. Throw strength is unchanged. The low practice divider remains available.

The floor conveyor on the right bypass moves workers and grounded cargo at 2 m/s. Face the lever and press **F** within 2.4 m to reverse it. Walls block interaction; the shared cooldown is 0.5 seconds. It does not directly push held, attached or airborne cargo. Each new shift resets it toward dispatch. Online simulation is authoritative on the host and shares the direction with guests.

Shelves, sorting tables, dispatch shutters, lighting and lane markings are prototype primitives. The eastern Packrat nest is a location placeholder; creature AI and cargo theft are not implemented. Pressure-plate doors and launchers are also unimplemented. Before further maps or art, evaluate the two routes, relay throws and conveyor mishaps.

The existing 180-second duration and player-count quotas remain unchanged. Difficulty for the larger layout needs real cooperative play. Record whether delivery takes too long, teammates are easy to locate and both routes are useful.

## Validation and remaining work

Seventeen behavior tests, four real two-process scenarios, the four-process scenario and the SteamPipe generator test passed. Checks cover worker/held-cargo routes to both bay centers, intake-throw first contact, expanded-boundary recovery, conveyor range/facing/line-of-sight/cooldown and guest reversal. The worst tested snapshot is 1,280 bytes, equal to the current limit. Actual rendering verified Korean/English menus and lever prompts, plus overhead, third-person and dispatch/nest views. Windows export and executable smoke checks passed; all 17 gameplay scripts are packed, and practice rendered in a separate folder containing only the executable and game pack. Separate PCs/external networks, Steam, final art, trailers, public store publication and Valve review remain unverified or unfinished. Steamworks registration and actual application IDs are unavailable; no upload, payment or release has been performed. Passing automated checks does not establish fun.

## 0.6 layout and lighting revision

Raised lights from approximately 3.2 m to 6.5 m and reduced energy from 3.4–5.0 to 1.0–1.4. Moved the belt out of the space between two partitions onto the right bypass. Its center is (10, -10), length 6 m, spanning Z -7 to -13 to carry packages past the sorting wall. The lever is at (12, -9). The left route remains free of belt movement. The existing 0.6 executable and ZIP are refreshed with this layout; all party members must use the refreshed build.

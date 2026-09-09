# NO RETURNS — Expanded playtest 0.5

[한국어](05-expanded.ko.md)

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
| Esc | Menu; online simulation continues |

Change language on the title menu. Toggle cue sounds on the title or pause menu. Cycle mouse sensitivity from 0.50× to 2.00× in 0.25× steps in the pause menu. These preferences save locally. There is no camera shake. Normal arrows and A / B labels supplement colors.

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

A departing guest releases held cargo and returns the remaining group to the lobby; the host restarts with the available crew. Host departure ends the session for everyone. Join attempts time out after 10 seconds. Use identical 0.5 builds in a party. Localhost multi-process tests do not establish real inter-region latency or separate-network connectivity.

## Validation and remaining work

Automated checks cover ownership, throws and marker contact, both dispatch bays, recovery, all four package behaviors, seeded destinations, crew quotas, pings, voting, bonus timeout and restart. Two- and four-process tests exercise replicated ownership, Sneezer/Clinger interactions, roster changes and disconnections. All 15 behavior checks, four two-process scenarios and the four-process scenario passed. Expanded-network checks confirm held Hopper pause and hop, B destination/delivery, guest ping, and unanimous overtime with score 6 / target 10 on host and guest. The largest deliberately populated snapshot test was 1,272 bytes (budget 1,280). Actual Korean/English menus/results, settings and Hopper cues were inspected. The exported executable rendered practice from a separate folder containing only the executable and game pack; 15 compiled gameplay scripts were audited in the pack. The SteamPipe generator test also passed. The sandbox emits certificate-store and user-directory/cache warnings; these are recorded separately from script failures. Real separate-machine and Steam tests remain open.

Steamworks enrollment, a real App ID, compatible Steam transport integration, two real accounts on separate PCs/networks, final art, trailer, store publication and Valve review remain open. No payment, upload or release has been performed. Ask testers whether they can explain accidents, recover quickly and find useful combinations; automated success cannot answer whether the game is fun.

## 0.5 fix — held cargo facing

The package model and labels now rotate with the carrier. The Sneezer face inherits that rotation once and matches the warning direction. Regression coverage checks all four cargo types, replicated guest presentation, retained facing after throwing and reset facing on recovery. The Windows executable and ZIP at the existing paths are updated. Close old game windows and relaunch.

## 0.5 fix — worker/cargo collision

Workers and cargo now collide. Workers cannot walk through floor cargo, and flying cargo or a coworker's held cargo also makes contact. Only the carrier ignores their own held crate; put-down, throw and recovery clear that exception. Carry-position queries exclude the carrier too, keeping pickup and movement usable. Attached Clingers retain their existing attachment-follow behavior and restore worker collisions on detachment. Collision/carry/release regressions and existing two-/four-process tests passed; the executable and ZIP are updated.

# NO RETURNS — Cooperative Contracts playtest 0.7

[한국어](07-contracts.ko.md)

September 7, 2026 · Windows x64 · Godot 4.7.2. A development build using primitive art, not a commercial release candidate or Steam-approved build. English is the source language for game copy; Korean is the default UI.

## Start

Extract the whole archive, double-click **NO_RETURNS.exe**, and select **START SOLO CAMPAIGN**. Deliver each package to the bay matching its A/B label. In the development project use PLAY.cmd, or PLAY_TWO.cmd / PLAY_FOUR.cmd for two/four local windows. Close older versions first.

For friends, one player selects **HOST CO-OP CAMPAIGN** and others join using the host address. Check the worker count, then the host starts. You can finish the campaign alone or cooperate with 2–4 workers. Short practice uses the original three-minute rules.

Keep the executable, .pck game data, both guides, third-party notices and SHA256.json together. No editor installation is required.

## Controls and settings

| Input | Action |
| --- | --- |
| WASD / mouse | Move / look |
| Space | Jump |
| E | Pick up, catch nearby airborne cargo, or put down |
| Left click | Throw held cargo |
| Q | Mark a point ahead or nearby cargo for the crew for four seconds |
| F | Face the conveyor lever within 2.4 m to reverse it |
| R | Scare a Packrat within 4 m in contracts 2–3 |
| H | Open cargo and cooperation help |
| Esc | Menu and settings |

Opening help or menus does not stop the online timer or other workers. Your movement input stops. Select Korean/English on the title menu. Settings include cue sounds, master volume, mouse sensitivity, FOV (60–90 degrees) and fullscreen. Defaults are 80% volume and 72-degree FOV. Preferences save on this PC. There is no camera shake; A/B text and directional arrows supplement color.

## Three contracts make one run

Each contract starts with 240 seconds. Quotas depend on crew size and stage.

| Workers | Contract 1 | Contract 2 | Contract 3 |
| --- | --- | --- | --- |
| 1 | 5 | 7 | 9 |
| 2 | 6 | 8 | 10 |
| 3 | 8 | 10 | 12 |
| 4 | 10 | 12 | 14 |

Success banks 10 credits per correct delivery, 5 extra per relay delivery, and a 20-credit completion bonus. Failed attempts bank no temporary earnings. Keep the bank and upgrades from earlier successful contracts and retry the same stage. Contract 3 success completes the run; there is no fourth contract.

Contract 1 has no Packrat; contracts 2–3 have one. All four cargo types and the same map remain available, with higher quotas and an added interruption later. Numbers and expected playtime are provisional tuning; difficulty and fun still need real crews.

## Earn together and choose equipment

Only the host can spend the shared bank between successful contracts. Benefits apply to every worker.

| Upgrade | Price / cap | Effect |
| --- | --- | --- |
| Work boots | 20 / 2 levels | +8% movement per level, maximum +16% |
| Time extension | 25 / 2 levels | +20 seconds from the next contract, maximum 280 seconds |
| Airhorn kit | 20 / 1 level | Horn cooldown 8 → 6 seconds |

Everyone must ready up before the next contract. A purchase clears readiness: review the new setup and ready again. Buying is disabled after final completion. A new run resets the bank and equipment; there are no permanent gameplay upgrades.

## Deliveries and relay catches

Initial destinations are A. After successful dispatch, the host draws the replacement destination from a shuffled bag of two A and two B entries. Standard, Sneezer, Clinger and Hopper remain the four circulating packages. Wrong-bay or out-of-bounds cargo returns after 1.2 seconds without scoring or changing destination.

One worker deliberately throws and **another presses E to catch it in the air within three seconds and at least 3 m from the release position**. Deliver that package correctly to earn 5 extra credits. Self-catches, ground pickups, repeated pickups, wrong bays and recovery cannot create extra rewards. A qualifying relay awards its bonus only once for that delivery.

The throw marker predicts first contact with static scenery. It does not guarantee where later bounces or cargo behavior will land. Replay the shipment seed from the result using `-- --seed=NUMBER`; this reproduces destination order, not every physics outcome or Sneezer interval.

## Use the four packages

- **Standard:** straightforward carrying and relay practice.
- **Sneezer:** a 1.5-second warning precedes a forward blast that pushes workers and cargo and breaks attachments. Its own carrier is exempt from its push. Subsequent calm intervals last 6–9 seconds.
- **Clinger:** attaches to one worker or package within 0.95 m for up to five seconds. Attached workers move at 70% speed. A sneeze detaches it, followed by a two-second cooldown. A pair dispatches together only if both destinations match; a mismatched partner returns without scoring. No attachment chains.
- **Hopper:** rests on the ground for four seconds, warns for one, then jumps forward at 2.8 m/s and upward at 6.5 m/s. Holding pauses the cycle. Aim and place it before the low divider.

Roles are not locked classes. Switch between intake, throwing, receiving and chasing the rat as needed. Worker numbers on Q markers help locate teammates. There is no built-in voice chat.

## Depot and Packrat

The Shipping Shrine measures 32 × 36 m. Carry around either side of the 3.4 m central sorting wall to northern A/B dispatch. The right bypass has a 6 m floor belt moving workers and grounded packages at 2 m/s. Face its lever and press F to reverse; walls block interaction. Shared cooldown is 0.5 seconds. The left bypass is a belt-free carrying route. Lights are 6.5 m high.

Packrat spots visible free grounded cargo within 6 m in the right lane and approaches it. Within 1.4 m it warns for 0.9 seconds before stealing. A worker pickup during approach cancels pursuit. It does not pursue cargo behind walls or in the left lane. It never steals worker-held or attached cargo. It moves at 2.7 m/s, carries toward the eastern nest and drops there. Walls or obstacles cause a safe drop. Returned cargo receives eight seconds of protection from theft.

R works within 4 m and clear sight. The rat drops cargo and flees for three seconds. Base per-worker cooldown is eight seconds; a miss also consumes it, so get close first. Ending, restarting or disconnecting releases creature ownership. There is no health, attack or death system.

## Connection and records

The host PC is the server, using direct-address ENet UDP **27842**. Use `127.0.0.1` on one PC or the host LAN address on the same network. Direct internet hosting may require router/firewall configuration. Steam friend invites/relay, host migration and mid-shift joins are not implemented. Everyone must use the same 0.7 build. Incompatible versions are rejected before roster admission.

Guest departure returns the remaining crew to the lobby. The host restarts the active contract while keeping bank/upgrades from earlier successes. Departure immediately after success prepares the next contract. Host departure ends everyone's connection.

Completed records live in `run_profile.cfg`, and preferences in `preferences.cfg`, inside this PC's Godot user-data folder for `NO RETURNS — Prototype 0.7`. Records contain completed runs, best deliveries/relays and gross earned credits; duplicate completion messages do not increment them. Guests record only runs in which they observed all three active contracts. Failure or abandonment is not completion. There is no automatic settings migration from older version folders, mid-run saving, Cloud or online leaderboard.

## Short practice

Practice retains 180 seconds and quotas of 5/6/8/10 for 1/2/3/4 workers. Unanimous agreement permits one 60-second bonus with extra quotas of 3/4/5/6. Bonus failure preserves base success. Campaign bank, upgrades, Packrat and completed-run records do not apply to practice.

## Remaining release checks

This is a development playtest with a complete cooperative run. Steamworks enrollment and actual App/Depot IDs, Steam invite/relay integration, separate PCs/external networks, target-hardware performance, independent newcomer/friend-group fun testing, final art/store imagery/trailer/rights review, Steam installation and Valve review remain open. Automated passes do not complete those gates.

When reporting issues, include build 0.7, worker count, contract stage, shipment seed, triggering actions, expected result and actual result. Observe whether newcomers know what to do, relay catches help, theft causes are readable and the crew wants another run.


## 0.7.1 fix

Added active cargo search and approach. Patrol waypoints moved to the lane center to avoid the nest. If an approach makes no progress for 1.25 seconds, the target is ignored for three seconds before another search. The in-game version is 0.7.1; existing executable/ZIP paths and the user-settings folder remain stable.


## 0.7.2 map retheme

The Shipping Shrine adds a huge cardboard boss, hungry A/B altars and Packrat as employee of the month. Deliver matching cargo to the A/B tongue-shaped pads. Small mouth motion is decorative, with no damage or timed-delivery requirement. Dimensions, routes, theft and contract rules remain. Existing executable paths now contain 0.7.2.


## 0.7.3 Map editing support

The map is now a saved scene editable directly in Godot. Moving bays, belt, nest and spawns also updates gameplay locations. Protocol 8 rejects guests with different maps. After editing, rebuild and have everyone use the same ZIP. Existing EXE/ZIP paths remain; the game version is 0.7.3.

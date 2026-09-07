# NO RETURNS — Clinger play guide

[한국어](03-clinger.ko.md)

**Historical 0.3 record. The current project runs [0.4 with 2–4 workers](04-four-players.en.md).** Two-player limits and packet measurements below describe 0.3.

Prototype 0.3 · 2026-09-07 · Windows · Godot 4.7.2

**Clinger joins Standard and Sneezer.** It sticks to a worker or another crate; a sneeze can break the bond. This tests the original idea that troublesome cargo can also be a cooperative tool. The user reported that 0.2 works; that is not yet an external multiplayer or fun assessment.

## Start playing

Close older game windows, double-click [PLAY.cmd](../../PLAY.cmd), and choose **Practice Alone**. Use the language button if needed. Find the lime crate with raised sticky pads at the right-hand intake, marked **CLINGER / A**.

Walk into it after its initial one-second grace period. It sticks for five seconds and your normal movement becomes 70% speed; movement and jumping remain available. A connection and countdown identify the bond. Wait for it to fall off, or have a coworker aim the Sneezer at the attached crate to remove it early. The Clinger receives the sneeze's push as it detaches.

E picks up a free crate or puts down your held crate; left-click throws it. A held Clinger does not acquire targets. An attached Clinger cannot be picked up directly. During the two-second reattachment cooldown it can be picked up normally. WASD moves, the mouse looks/aims, Space jumps, and Esc opens the menu. Time and cargo behavior continue while the menu is open. Korean/English and synthesized-sound mute controls remain available; settings are not saved across launches.

All three types go to **A**. The quota remains five deliveries in three minutes; B returns cargo without scoring. If the Clinger is attached to a crate when that crate is successfully dispatched, both count once and return to their own intake spots. Carrying the target crate therefore lets one worker transport two packages while still using one held slot.

## Two short experiments

Run [PLAY_TWO.cmd](../../PLAY_TWO.cmd). Wait for two workers in the host window and press **Start Shift**. Switch between windows on one PC to control each worker; two keyboards on that PC do not control separate players simultaneously.

1. **Rescue a coworker.** Let the Clinger stick to the other worker. Pick up the Sneezer, aim its nose at the attached crate and watch the warning. The sneeze releases the bond and may also shove the coworker. Was the attempted rescue useful, funny or merely annoying?
2. **Bundle a delivery.** Carry the Clinger beside the Standard crate, put it down within contact range, then step away so the crate becomes its nearest target. Pick up the Standard crate and carry both to A before the five-second bond expires. Both should count once. If time runs out, the Clinger drops and can be recovered.

Solo practice supports attaching to yourself and bundling crates. Human cooperative playtesting is still needed to judge whether these interactions are worth expanding. Continue to use primitive art until that is clearer.

## Rules and recovery

| Rule | Current value or behavior |
| --- | --- |
| Active types | Standard, Sneezer, Clinger; one of each |
| Attachment | Nearest eligible worker chest or other crate center within 0.95m, with clear line of sight |
| Duration | 5 seconds |
| Worker movement | 70% normal speed while attached; control remains available |
| Reattachment cooldown | 2 seconds after release; 1 second at spawn/recovery |
| Target limit | One worker or one non-Clinger crate; no attachment chains |
| Following | Keeps the contact offset; solid walls or a target move over 2m break the bond |
| Sneeze | Existing 1.5-second warning; impact on the attached Clinger releases and pushes it |
| Return | 1.2 seconds after delivery or recovery |

Target loss, wrong-bay/lost target recovery, shift end, leaving and restart remove the bond. Wrong-bay/lost target recovery does not award a bundled delivery. The collision sweep allows 1mm per face for resting-contact tolerance, while preventing meaningful wall crossings. Swelling and sticky pads are visual; the base collision box stays unchanged.

The host decides attachment, release, movement and score. Guests display replicated state. Cargo wire data now uses fixed arrays to keep three crates within the existing packet budget; both participants must run the same version. This is still **two-player ENet on UDP 27842**, verified with separate processes on this PC. Other-PC LAN, Internet/NAT, packet loss, latency, Steam lobbies/invitations and four-player sessions remain unverified or unimplemented. [Runtime restoration](01-first-playable.en.md#6-edit-and-restore-the-runtime) and [Steam preparation](../steam/05-release-checklist.md) remain separate guides.

## Verification

Run from the project folder:

```powershell
python tools/run_tests.py
.\.tools\godot\Godot_v4.7.2-stable_win64_console.exe --headless --path . --log-file final-import.log --editor --quit
```

The final suite includes eight individual scripts and three real host/guest pairs, with bounded timeouts. They cover existing ownership/physics/shift/Sneezer behavior, compact serialization, attachment-controller behavior, actual Clinger lifecycle, and two-process attachment/rescue. Test ports are 27943, 27944 and 27945. Logs are in `artifacts/`.

The controller test first failed with the module absent, then passed. Additional floor-contact tests exposed an immediate-detach problem and passed after the 1mm sweep tolerance fix. Actual main-scene tests verify attached pickup rejection, moving a carried bundle, two one-time deliveries, recovery clearing slowdown, and restart/lobby cancellation. Original nearest-Standard tests place the new Clinger away from their fixture so they still exercise the original crate rather than accidentally selecting the nearer new one.

The two-process Clinger test verifies matching target/countdown, followed motion, slowdown, sneeze release, restored speed and disconnection. The largest observed Clinger-test snapshot was **992 bytes**, under the **1280-byte budget**; the three-type round-trip fixture measured **976 bytes**. These are measurements of the exercised states, not universal network guarantees. Code review found no confirmed new P1/P2 issues; the noted lifecycle test gap was covered by the added main-scene integration test.

The behavior suite and editor import returned 0 without script errors. Existing sandbox certificate and `user://` directory/cache errors still appeared and are recorded separately from gameplay failures. Actual Korean/English menus and attachment/rescue captures were inspected; audio listening, long sessions, human fun and remote connectivity still need testing.

Local renderer captures: [attached](../../artifacts/clinger-attached.png), [rescue warning](../../artifacts/clinger-rescue-warning.png), [released](../../artifacts/clinger-released.png), [Korean menu](../../artifacts/clinger-menu-ko.png), [English menu](../../artifacts/clinger-menu-en.png). These ignored inspection artifacts can be recreated with graphical `tests/capture_clinger.gd`. The [implementation plan](../superpowers/plans/2026-09-07-clinger.en.md) records scope. No new engine, purchased asset, Steam registration, public export or upload was performed.

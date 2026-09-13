# NO RETURNS — Sneezer play guide

[한국어](02-sneezer.ko.md)

> **Historical record · retired/superseded 2026-09-12.** The [PSX space-delivery design](../current/01-overview.en.md) is current. Implementation, launch, art and values below are historical, not current status. Deleted files remain path records only.


> Documentation update: versions and values here belong to this file's original context. Check the [documentation home](../README.en.md) first for current rules and outstanding work.

**Historical 0.2 record. The current project runs [0.3 with Clinger](03-clinger.en.md).** The launchers run the latest code; the two-cargo count, packet measurement and future-work list below describe 0.2.

Prototype 0.2 · 2026-09-07 · Windows · Godot 4.7.2

**Sneezer cargo is playable alongside the Standard crate.** The purpose of this version is to test whether aiming a troublesome package creates understandable, recoverable accidents between teammates. Solo practice and two-player development connections are available. Streaming appeal still needs human playtesting.

## 1. Start here

1. Close any older game windows. Double-click PLAY.cmd (`../../PLAY.cmd`; retired file) in the project folder.
2. Select **혼자 연습하기 / PRACTICE ALONE**. The language button switches between Korean and English; Korean remains the default.
3. Find the lavender crate with a pink nose on the left side of intake. Move close and press **E** to pick it up. If two crates are nearby, E selects the nearest available reachable one. Each worker can hold one crate.
4. Turn with the mouse. While carried, the nose follows your horizontal aim. Put it down with **E** or throw it with **left-click**; it keeps its last direction after release.
5. When it swells, watch the **countdown and yellow cone on the floor**. After 1.5 seconds, it sneezes forward. Other workers in range are shoved and drop their cargo. Other crates in range are pushed. The Sneezer and its carrier are immune to their own burst.
6. Deliver either crate to the mint **A** bay. They contribute to the same five-delivery quota within three minutes. Each crate returns to its own intake position after about 1.2 seconds. B returns cargo without awarding a delivery.

The launchers use the portable runtime already prepared in this folder; no Steam login is needed. They run the editable project, not a standalone Steam distribution. EDIT.cmd (`../../EDIT.cmd`; retired file) opens the editor; F5 runs the project. If the runtime is missing, use the restoration instructions in [the first guide, section 6](01-first-playable.en.md#6-edit-and-restore-the-runtime).

## 2. Controls and cues

| Input or cue | Meaning |
| --- | --- |
| WASD / mouse / Space | Move / look / jump |
| E | Pick up or catch the nearest reachable free crate; put down your current crate |
| Left-click | Throw the held crate with fixed forward/upward force |
| Esc | Open or close the menu and release mouse capture; the shift and sneezes continue |
| CUES: ON / OFF | Toggle synthesized warning and sneeze sounds in the main or pause menu |
| Swelling, squinting, countdown, floor cone | A directional sneeze is approaching; move out of the cone or behind solid depot geometry |
| Puff and ACHOO! | The burst has occurred; recover any dropped cargo |

Pitch moves the camera; throws and sneezes use horizontal facing. A carried crate retains the first-contact throw marker from 0.1. It predicts the first collision, not where the crate will finally stop or what a later sneeze will do. Moving targets and network latency can change the actual contact. Swelling only changes the visual mesh, not the collision box.

All danger cues remain visible with sound muted. Sound and language choices last only for the current process. No final audio assets or voice chat were added; the short functional cues are synthesized in code.

## 3. Try a cooperative accident

Run PLAY_TWO.cmd (`../../PLAY_TWO.cmd`; retired file), wait for **2 / 2 CREW** in the host window, then press **START SHIFT** there. One PC uses two separate game windows; switch focus to control each worker. These are not simultaneous controls for two people on one keyboard.

Try three short experiments:

1. One worker holds the Sneezer and aims toward the other worker carrying the Standard crate. Observe the warning, shove, dropped crate and recovery.
2. Turn the nose away before the warning expires, or use the solid divider as cover. Check whether avoiding the accident feels intentional and readable.
3. Put down the Sneezer facing the Standard crate, then wait for the burst. See whether using it to move cargo helps delivery or creates more work.

For each attempt, note whether the warning was noticed, the direction made sense, the accident was amusing, and recovery was quick enough. Solo practice can demonstrate crate pushing; a second participant is needed to judge cooperative comedy. Values below can be tuned after this playtest.

## 4. Current tuning and connection limits

| Rule | Prototype value |
| --- | --- |
| Cargo / workers | Standard and Sneezer / solo practice or two connected workers |
| Shift | Three minutes; five shared deliveries to A |
| Calm interval | First interval 6 seconds; subsequent intervals randomly 6–9 seconds |
| Windup / burst | 1.5 seconds / 0.35 seconds |
| Sneeze reach | 4.5m horizontal range; 100-degree cone; at most 1.6m height difference |
| Worker push | Adds 7m/s horizontally, capped at 12m/s; upward speed at least 3.8m/s; 0.5-second lean |
| Push recovery | Horizontal push decays at 14m/s²; ordinary movement remains available |
| Other cargo push | Adds 8m/s forward and 4m/s up; total speed capped at 16m/s |
| Pickup / cargo recovery | 2.4m / 1.2 seconds |

The host chooses timing and computes ownership, hits, physics and score. Guests display authoritative state at 20 snapshots per second. Sequence IDs scoped to an authority/shift prevent repeated snapshots from replaying a burst, including when joining a different host. Recovery removes hidden cargo from collision until it returns. Leaving a session, finishing a shift and restarting cancel pending effects.

Normal connections use **ENet / UDP 27842**. Automated testing has verified two processes on this PC. Other-PC LAN, Internet/NAT, latency and packet-loss conditions remain unverified. Steam lobbies, friend invitations, an App ID and a Steam export are not implemented. Both players must use the same prototype version. Guest departure returns the host to waiting; host departure returns the guest to the menu. No host migration or mid-shift joining is provided.

## 5. Verification and known limits

Run the following in the project folder:

```powershell
python tools/run_tests.py
.\.tools\godot\Godot_v4.7.2-stable_win64_console.exe --headless --path . --log-file final-import.log --editor --quit
```

The final full run passed all five individual scripts and both host/guest process pairs; the runner returned 0. The editor import returned 0 without script errors. The connection tests use UDP **27943** and **27944**, separate from play. Logs are under `artifacts/`.

- Cargo-rule, original carry/throw physics and shift-flow checks remain passing. The five static throw-marker cases stayed within the 0.20m tolerance.
- Sneeze clock and cone rules cover warning timing, one transition per update, one burst, random bounds, cancellation and invalid inputs.
- Actual-scene checks cover two simultaneous holders, moving pickup near the distance limit, Sneezer delivery into the shared score, dropped/pushed cargo and workers, carrier immunity, wall shielding, recovery and restart.
- Regression tests first reproduced cross-host missing effects and hidden recovery collisions, then passed after their fixes. Duplicate snapshots produce one cue; both host and guest restore collision on recovery.
- Two real process pairs verify original delivery/disconnect/rejoin behavior plus guest-held Sneezer warning, matching hit/event results, actual pushed movement and no repeated burst. The largest serialized snapshot observed was **1,180 bytes**, below the **1,280-byte test budget**; the earlier ENet MTU warning is gone in the final run. This measurement covers the exercised two-player states.
- Code review found two P2 issues; both were fixed and the follow-up review found no remaining P1/P2 in its reviewed scope.
- The actual Compatibility renderer produced Korean/English menus, windup, burst and muted pause screens. Text and layout were inspected. This is visual inspection, not a listening test or human fun assessment.

The sandbox logged existing root-certificate and `user://` cache/profiler-directory errors. These were separate from GDScript failures and did not prevent the checks or image capture. Audio synthesis and mute paths ran, but listening quality and comfortable levels on other devices still need user testing.

Actual scene captures: windup (`../../artifacts/sneezer-windup.png`; retired file), burst (`../../artifacts/sneezer-burst.png`; retired file), Korean menu (`../../artifacts/sneezer-menu-ko.png`; retired file), English menu (`../../artifacts/sneezer-menu-en.png`; retired file), muted pause (`../../artifacts/sneezer-pause-muted.png`; retired file). These local inspection files are ignored by git. The capture fixture is `tests/capture_sneezer.gd` and must use the graphical renderer.

Clinger/Hopper cargo, four players, Steam transport, saved settings, gamepads, final art/audio, commercial performance targets and public redistribution remain future work. Steam release preparation stays in the [release checklist](../steam/05-release-checklist.md). The [0.2 implementation plan](../superpowers/plans/2026-09-07-sneezer.en.md) records the approved scope; the [rules report](sneeze-rules-report.en.md) records its initial failure/success evidence.

# NO RETURNS — two-to-four-player guide

[한국어](04-four-players.ko.md)

> **Historical record · retired/superseded 2026-09-12.** The [PSX space-delivery design](../current/01-overview.en.md) is current. Implementation, launch, art and values below are historical, not current status. Deleted files remain path records only.


> Documentation update: versions and values here belong to this file's original context. Check the [documentation home](../README.en.md) first for current rules and outstanding work.

Prototype 0.4 · 2026-09-07 · Windows · Godot 4.7.2

Hosted rooms now support **2–4 workers**. Solo practice, Standard/Sneezer/Clinger cargo, five shared deliveries and the three-minute shift remain unchanged. This step tests the same interactions with more coworkers; it does not establish final four-player balance.

## Play

1. Close older game windows and return any existing host to the menu.
2. Double-click PLAY_FOUR.cmd (`../../PLAY_FOUR.cmd`; retired file). It opens one host and three guest windows, each 640 × 400. Move or resize them to fit your display.
3. In the host window, wait for **4 / 4 WORKERS**, then select **START SHIFT**. You can also start a hosted shift with two or three workers; the host alone cannot start.
4. Switch focus between windows to control each worker. This is a development launcher for separate processes, not simultaneous local multiplayer on one keyboard. Maximize a window if its text is too small.

PLAY_TWO.cmd (`../../PLAY_TWO.cmd`; retired file) still opens two windows. PLAY.cmd (`../../PLAY.cmd`; retired file) opens the menu and solo practice. For manual joining, one player selects **HOST / 2–4 WORKERS** and others enter the host address; the same-PC address is `127.0.0.1`. Everyone must run the same version. Regular play uses UDP **27842**.

WASD moves, the mouse looks/aims, Space jumps, E picks up/puts down, left-click throws, and Esc opens the menu. Time continues while the menu is open. The language and sound buttons remain available. Cargo rules are described in the [Clinger guide](03-clinger.en.md); its two-player limitation is historical. Runtime restoration remains in [the first guide](01-first-playable.en.md#6-edit-and-restore-the-runtime).

## What changed

| Feature | Current behavior |
| --- | --- |
| Crew | Host plus up to three guests |
| Identity | Stable numbers 01–04 with yellow, mint, lavender and pink outfits |
| Spawn | Separate positions for each slot |
| Guest departure | Current shift stops and everyone returns to waiting; held cargo is released and attachments are cancelled |
| Restart | Host may start again with 2–4 workers; quota and cargo reset |
| Rejoining | Uses a vacant slot without renumbering remaining workers |
| Host departure | Session ends; guests return to the menu with a reason |
| Joining limits | No mid-shift joining or host migration; the server accepts at most three guests |

Try having one worker carry the Sneezer, another carry Standard, and a third get stuck to Clinger. The fourth can receive or recover dropped cargo. Check whether having more people helps coordination or just crowds the space. No extra cargo or quota scaling was added in this version.

## Server and validation scope

The host's game process is also the authoritative server. It computes movement, physics, ownership, attachment, sneeze hits and score. Guests send inputs and display replicated results. The current engine runs physics at 60 Hz and publishes state at 20 Hz. Worker poses now use compact arrays alongside cargo arrays to keep state below the existing 1280-byte test budget. There is no separate rented server or Steam transport.

The full test entry point is:

```powershell
python tools/run_tests.py
```

It runs nine individual scripts, three two-process cases and one four-process case. Four-player logs are `artifacts/four-network-{host,guest1,guest2,guest3}.log`; its isolated runner is `python tools/run_four_test.py`. Test ports 27943–27947 are separate from normal play.

The roster test first failed before slots existed, then passed for 2/3/4 start rules, unique slots/spawns, vacant-slot reuse, fifth-worker rejection in the scene registry and state size. Existing regressions still cover cargo, physics, shift flow, Sneezer, Clinger and two-player reconnection.

The real four-process case verifies the same roster and slots on all clients, ownership of two different cargo items, Clinger attachment, a sneeze hitting two workers and releasing cargo, shared delivery, a carrier leaving, three-player restart and host departure. Its initial recorded maximum serialized state was **1036 bytes**. The state-body budget excludes transport framing. Code review found no P1/P2 issues. Vacant-slot reuse is checked locally; an actual reconnect into that vacant slot is not part of the four-process test. Full-room network rejection is configured through ENet capacity but is not a separate fifth-process test.

Editor import and actual-renderer Korean/English menu, lobby and four-worker captures are also checked. The sandbox can report the existing certificate-store and `user://` cache/profiler directory errors; distinguish these from script errors. Local screenshots: four workers (`../../artifacts/four-workers.png`; retired file), lobby (`../../artifacts/four-lobby.png`; retired file), Korean menu (`../../artifacts/four-menu-ko.png`; retired file), English menu (`../../artifacts/four-menu-en.png`; retired file). These ignored inspection artifacts can be recreated using graphical `tests/capture_four.gd`.

Successful local process tests do not establish other-PC LAN, Internet/NAT, latency, long-session performance or human enjoyment. Steam lobbies, invitations, App ID setup and distribution remain pending in the [release checklist](../steam/05-release-checklist.md). The [0.4 plan](../superpowers/plans/2026-09-07-four-players.en.md) records this step's scope.

Final verification: `python tools/run_tests.py` and editor import both returned 0; no script or MTU errors were found in the final logs.

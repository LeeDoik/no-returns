# NO RETURNS — first prototype guide

[한국어](01-first-playable.ko.md)

Version 0.1 · 2026-09-07 · Windows · Godot 4.7.2

**Historical 0.1 record. The current project runs [prototype 0.2 with Sneezer cargo](02-sneezer.en.md).** Launchers, basic controls and runtime restoration below remain useful; one-crate/audio limitations and the next experiment describe 0.1. The current test runner also covers Sneezer and uses an additional UDP 27944 process pair.

This is the **first development prototype for third-person controls and basic shared-cargo delivery**. Practice alone or connect two instances on one PC. Living special cargo and Steam friend invites come next. Unlike the final design's 2–4 players and 10–15-minute shifts, this experiment supports solo practice or two connected players, five deliveries and a three-minute timer.

## 1. Play now

1. Double-click [PLAY.cmd](../../PLAY.cmd) in the project folder.
2. On the `NO RETURNS` screen, select **PRACTICE ALONE**. The default UI is Korean; use **English / 한국어** to switch.
3. Find the yellow worker and the crate at intake. Press **E** nearby to pick it up.
4. Carry it to the mint-colored **A bay** and press **E** to put it down, or left-click to throw it. A crate arriving inside the bay increments the score.
5. A new crate appears at intake. Ship five within three minutes to complete the shift. Select **ANOTHER SHIFT** to restart.

A portable Godot runtime is already present in this folder. No separate engine installation or Steam login is required. `PLAY.cmd` runs the project with the portable editor binary. This is not yet a standalone Windows export or Steam release build.

## 2. Controls

| Input | Action |
| --- | --- |
| WASD | Camera-relative movement |
| Mouse | Rotate the view |
| Space | Jump |
| E | Pick up / catch nearby cargo; put it down while holding |
| Left click | Throw held cargo |
| Esc | Toggle menu and release the mouse |
| English / 한국어 on the menu | Switch UI language; Korean is the default |

Throws currently use a fixed strength and upward angle. Mouse yaw sets throw direction; vertical mouse movement only changes the camera. A marker appears while holding cargo to indicate predicted first contact. It does not guarantee the final resting position after rolling or bouncing.

Opening the menu or switching to another window stops your own input. The shift timer continues. Choose **BACK TO WORK** when returning, including when switching between two test windows.

## 3. Connect two instances on one PC

1. Leave any existing hosted game. Only one host may use the default port.
2. Run [PLAY_TWO.cmd](../../PLAY_TWO.cmd). It opens a host window and a guest window.
3. Confirm **CREW READY** and **2 / 2 WORKERS** in the host window.
4. Select **START SHIFT** in the host window.
5. Alternate between windows to check movement, pickup and throwing. The guest is the mint-colored worker. This is not simultaneous two-player input using one keyboard.

For manual setup, launch `PLAY.cmd` twice. Choose **HOST / 2 WORKERS** in the first window, and leave the address as `127.0.0.1` before choosing **JOIN DEPOT** in the second.

The host decides final positions, cargo ownership and score. A departing guest releases held cargo and returns the host to the lobby. Another guest may join before a new shift. A departing host returns the guest to the menu with a reason. Joining a running shift is unsupported.

## 4. Other PCs and current Steam scope

The development transport is **ENet / UDP 27842 / two players maximum**. To attempt a LAN test, put the same project and runtime on both PCs and enter the host's LAN address on the guest. Windows Firewall or network policy may block connection. Success on another PC has not yet been verified.

Two real processes on one PC have been tested. Different Internet networks, NAT, Steam lobbies, friend invites, the overlay and actual Steam App ID integration remain unverified or unimplemented. Address entry is a development interface, not a confirmed global-release joining flow. The Steam stages in the [online and build plan](../steam/04-online-and-build-plan.md) remain incomplete.

## 5. Troubleshooting

| Symptom | Check |
| --- | --- |
| Godot runtime missing | Follow the runtime restoration procedure below |
| Could not host | Leave the previous host using the same port |
| Could not connect | Confirm that a host exists and the address is correct; use 127.0.0.1 on one PC |
| Connecting screen | Failure appears within ten seconds; leaving to the menu also cancels |
| Mouse captured | Press Esc |
| Worker does not move | Confirm the shift has started and select Back to Work if the menu is open |
| Crate disappears | Check intake about 1.2 seconds after delivery or return |
| Bay B does not score | The destination is A; B returns the crate without scoring |

## 6. Edit and restore the runtime

Run [EDIT.cmd](../../EDIT.cmd) to open the project in Godot. **F6 runs the selected scene**, while **F5 runs the project**. The entry scene is `scenes/main.tscn`; depot geometry and UI are generated by GDScript, so the edit viewport does not preview every generated object.

If you received source files only or `.tools` is missing, run this in PowerShell from the project folder:

```powershell
powershell -NoProfile -ExecutionPolicy Bypass -File tools/setup_godot.ps1
```

This downloads the official **Godot 4.7.2 standard Windows x64** runtime into the project. It performs no system-wide installation or permanent execution-policy change. The version-pinned archive is checked against its official SHA512 value before extraction. After the ready message, run `PLAY.cmd`. The download is approximately 86MB; the extracted executable is approximately 181MB.

The verified engine version is `4.7.2.stable.official.ed1daf0bf`. An `_sc_` file keeps editor settings and caches in the portable folder. See [Godot's path documentation](https://docs.godotengine.org/en/stable/tutorials/io/data_paths.html) for actual user-data locations and platform behavior.

Official sources: [Windows download](https://godotengine.org/download/windows/), [versioned binaries and checksums](https://github.com/godotengine/godot-builds/releases/tag/4.7.2-stable), [Godot license](https://github.com/godotengine/godot/blob/4.7.2-stable/LICENSE.txt), [third-party copyright notices](https://github.com/godotengine/godot/blob/4.7.2-stable/COPYRIGHT.txt). Before public redistribution, include the notices required by the actual engine and bundled components and make them accessible to users. This folder is for local development execution; no public distribution bundle was produced.

## 7. Automated verification

Run from the project folder. Python is needed for this test runner, not for normal play.

```powershell
python tools/run_tests.py
```

The runner exits with code 0 only when cargo rules, physics regressions, shift flow and the two-process connection test pass. Failure or timeout produces a nonzero exit code. Detailed logs are saved under `artifacts/`. The two-process test uses UDP **27943**, separate from regular play.

Verified behavior:

- Six cargo-rule groups and 45 assertions: contested pickup, invalid coordinates/distance/IDs, owner-only release, duplicate/wrong/held delivery rejection, reset boundaries and disconnection.
- Two real Godot processes: joining, host-simulated guest movement, pickup and throw observed by both sides, foreign throw rejection and shared delivery score.
- Guest departure while holding releases cargo and returns the host to waiting; rejoining works; host departure provides the guest with a reason.
- Wrong-bay and lost-cargo recovery in the actual depot, five-delivery success, timeout and restart from both outcomes, and invalid movement rejection.
- Carry/drop collisions at the divider and first-contact marker accuracy in five throw conditions, using a 0.20m tolerance in the current static depot.
- Menu and depot captured with the actual Compatibility renderer and inspected for Korean text and layout.

Physics regression and review results are recorded in the [physics fix report](physics-fix-report.en.md); original cargo-rule failure and success evidence is in the [rules test report](cargo-rules-report.en.md). These results do not establish performance on other PCs, Internet latency, long sessions or commercial minimum specifications.

## 8. Current limits and next experiment

There is one standard crate. Sneezing and sticky cargo, four players, Steam integration, audio, gamepad support, saved settings, sensitivity adjustment, substantial latency compensation, final art and a standalone export are not implemented. English source copy and Korean UI/documentation are maintained together. Keep the final store description distinct from this small experiment's implemented scope.

Next, **add one Sneezer cargo type and test whether helping a teammate leads to funny accidents**. First check whether this version's movement, camera, pickup and throw are understandable; then add a visibly telegraphed sneeze that pushes other cargo. The game's broader streaming appeal should be judged after that interaction exists.

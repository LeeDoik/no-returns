# NR-LOOP-01 — small delivery loop launch and validation

[한국어](22-slapstick-loop.ko.md)

> **Historical record · retired/superseded 2026-09-12.** The [PSX space-delivery design](01-overview.en.md) is current. Implementation, launch, art and values below are historical, not current status. Deleted files remain path records only.


2026-09-12 · Unity experiment implemented. The [approved handoff scope](21-slapstick-loop-handoff.en.md) is connected into a playable game. This does not establish release quality or human fun validation.

## Launch and controls

Run 07_NoReturns_Loop.cmd (`../../07_NoReturns_Loop.cmd`; retired file). The existing 06 launcher remains CarryLab. The new player is `build/loop/Windows/NO_RETURNS_LOOP.exe`. Copy the entire Windows folder to another PC.

- Solo starts a single-player run.
- 2 players: one selects Host / Begin; the other enters the host PC address and selects Join address. After connection, the host selects Host / Begin again. Use `127.0.0.1` for same-PC testing.
- Direct connection uses TCP 27831. Separate PCs need a reachable network and permission to communicate on this port. Steam invitations and internet relay are not included.
- State exists only during the current process and disappears when it exits.

| Control | Action |
|---|---|
| WASD / mouse / Space | Move / look / jump |
| E | Pick up or put down nearby cargo, load or unload the trolley, or take or release its handle; HUD shows the current action |
| Hold and release left mouse | Charge and throw |
| Esc / right mouse | Release cursor / resume camera control |
| B near truck | Buy trolley |
| R near truck | Recover undelivered cargo and trolley |
| F | Return your worker, leaving held cargo at the scene |
| Enter near truck | Host settles; unfinished count is shown |
| Results | Host retries with the same or a new seed; return to menu |

## Current rules

Regular A and B at 30 each → purchase trolley for 60 → sneeze A and B at 90 each → settle at truck. Normal completion yields gross 240, investment 60, balance 180. Premium price and sneeze risk appear before buying. Prices and the 8–12 minute observation target are experimental; there is no forced time limit.

Matching cargo ID and destination, released hand ownership, and 1 second inside the receipt area commit payment once. Loaded trolley deliveries also count. Wrong addresses, repeat deliveries and repeat purchases neither pay nor charge again. Premium cargo dispatches sequentially. Settlement stops progress and preserves confirmed income. Restart resets wallet, deliveries, workers, trolley, cargo and sneeze timers.

Sneezing activates at first pickup or loading. Intervals are 8–12 seconds, with 1.2 seconds of facial change and `AH... AH...` text before a temporary release sound. The cone checks 3m forward, a 45 degree half-angle and wall occlusion. Initial external velocity is 3m/s horizontally and 1.5m/s upward; workers accumulate and damp it. A hit releases hand cargo but preserves trolley loading joints. Recovery and dropping do not reset active timers. Delivered cargo is inactive. A dedicated inhalation sound remains to be produced.

The trolley uses a dynamic chassis and a joint with limited forces for one parcel. It follows worker target position and velocity while retaining world contacts. Wide-route trolley walking speed is 2.8m/s versus the existing 1.6m/s hand carrying speed. The physics test measured 5.432m over 2 seconds with the load retained. This straight automated measurement does not establish whole-delivery time savings.

## Authoring and preservation

Open the separate scene (`../../unity/NoReturns/Assets/_Project/SlapstickLoop/Scenes/SlapstickLoop.unity`; retired file) in Unity. The fixed yard is 48×50m with a wide detour and narrow alley toward A and a low step and trolley ramp at B. Edit and save its walls, destinations, truck and workers directly. Runtime values live in LoopWorld (`../../unity/NoReturns/Assets/_Project/SlapstickLoop/Runtime/LoopWorld.cs`; retired file), economy in LoopRules (`../../unity/NoReturns/Assets/_Project/SlapstickLoop/Runtime/LoopRules.cs`; retired file), and trolley behavior in LoopTrolley (`../../unity/NoReturns/Assets/_Project/SlapstickLoop/Runtime/LoopTrolley.cs`; retired file). Region count and map size are not final release commitments.

LoopBuilder (`../../unity/NoReturns/Assets/_Project/SlapstickLoop/Editor/LoopBuilder.cs`; retired file) creates the scene or opens an existing file. `Polish scene` corrects initial signs and truck placement for this experiment; do not arbitrarily rerun it after user editing. Building explicitly selects only the new scene and preserves default build scene settings.

CarryLab, Godot, existing art and unrelated uncommitted changes remain. Shared WorkerController gained optional external input, impulses and individual recovery; CargoBody gained optional automatic recovery and explicit initial-position capture. Existing default behavior remains. There are no new regions, final art, combat, full ragdolls or persistent saves.

## Networking and limitations

LoopWire (`../../unity/NoReturns/Assets/_Project/SlapstickLoop/Runtime/LoopWire.cs`; retired file) uses direct .NET TCP. Managed APIs in the existing Unity environment provide this Windows 2-player experiment without another package or service account. The choice follows [Unity .NET support](https://docs.unity3d.com/6000.0/Documentation/Manual/dotnet-profile-support.html) and [Microsoft socket APIs](https://learn.microsoft.com/en-us/dotnet/api/system.net.sockets?view=netframework-4.8.1), and was verified in the actual Windows player.

The host determines physics, deliveries, wallet, purchases, sneezes and settlement. Clients send input requests and receive state at 20Hz. Solo uses the same rules. Actions accumulate until the physics update to avoid losing quick inputs. Client departure releases ownership and lets the host continue solo; host departure explains the event and returns the client to the menu. Late joining, reconnection and host migration are unsupported.

Clients currently display state without prediction or interpolation, leaving latency feel and motion smoothness as outstanding work. Transport tests and game behavior tests are separate and do not guarantee release networking quality.

## Tests and evidence

- `powershell -NoProfile -File tools/loop.ps1 test`: exit 0. Economy 28 checks, existing CarryLab 24 checks, new integration 18 checks, trolley 6 checks and physics API verification. Log (`../../artifacts/loop/test.log`; retired file).
- `powershell -NoProfile -File tools/loop.ps1 build`: Windows build succeeded, exit 0. Log (`../../artifacts/loop/build.log`; retired file).
- `python tools/run_loop_probe.py`: solo and independent host/client players on the same PC. Solo (`../../artifacts/loop/solo.json`; retired file), host (`../../artifacts/loop/host.json`; retired file), client (`../../artifacts/loop/client.json`; retired file).
- Across two processes, a client pickup request acquired ownership, then sneeze impulse, displacement and client recovery were verified. Both sides agreed on deliveries, purchase and settlement values.
- The same run also verified restart resets on both sides and client menu return after host departure. JSON economy values preserve the completed run before restart; lifecycle results use separate fields. Player settlement screenshot (`../../artifacts/loop/solo.png`; retired file), client gameplay screenshot (`../../artifacts/loop/client.png`; retired file).
- Automated tests place workers and parcels for their purpose. They are not human navigation runs; some sneeze hit tests call the release directly.
- `powershell -NoProfile -File tools/test_loop_wire.ps1`: isolated TCP connection, bidirectional messages, disconnection and message-limit checks.

## Remaining validation and decisions

- [ ] Separate-PC direct connection, actual latency and interrupted-network validation.
- [ ] 2 human players carrying a trolley through the detour, alley and step over extended play, retrying the same and new seeds.
- [ ] Measure first-purchase time, perceived trolley benefit, and per-delivery carrying and recovery time.
- [ ] Observe whether players read warnings, understand accidents, change route or role because of a partner, and voluntarily want another run.
- [ ] Tune natural sneeze inhalation audio and warning clarity, plus client motion interpolation.

Human validation has not run. Use these observations to tune warning, trolley and routes before adding equipment or regions. Apply the [handoff observation criteria](21-slapstick-loop-handoff.en.md).

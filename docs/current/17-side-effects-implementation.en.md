# SIDE EFFECTS first cooperative prototype implementation record

[한국어](17-side-effects-implementation.ko.md)

> **Historical record · retired/superseded 2026-09-12.** The [PSX space-delivery design](01-overview.en.md) is current. Implementation, launch, art and values below are historical, not current status. Deleted files remain path records only.


> Archived record · Discontinued 2026-09-12. Not a current launch guide or implementation specification. Links to removed files point to the archive containing their originals. [Unity 메인 / Mainline](18-unity-mainline.en.md).

2026-09-11 · SE-PROT-01 · Revision 2 · First prototype implementation and automated checks complete. Human validation and release readiness remain pending.

This record connects the approved short laboratory scope in the [handoff](16-side-effects-handoff.en.md) to current code. It separates automated rules, actual multi-process connections, rendered inspection, and human fun validation; this document alone does not declare final passage of the gates.

## Launch and controls

Run 07_SIDE_EFFECTS.cmd (`../archive/side-effects-2026-09-12.zip`; retired file). The dedicated output is SIDE_EFFECTS.exe (`../archive/side-effects-2026-09-12.zip`; retired file), and the saved scene is SideEffectsLab.unity (`../archive/side-effects-2026-09-12.zip`; retired file). Select HOST or F1, then enter the address and select JOIN in another window. On the same PC use `127.0.0.1` or F2. The default UDP port is 7778, supporting up to 4 participants including the host.

After connecting, click RESUME CONTROLS or the game space to capture the mouse. Esc releases it; the recapture click does not attack. Controls are WASD movement, mouse look, Space jump, Shift dodge, left click basic attack, right click current tool, hold E rescue/switch, Q help request, and F toggle dragging a downed ally. In practice use 1/2/3 for staff/shield/hammer or release the cursor with Esc to use buttons. Free swapping during combat is unavailable.

When the host releases the cursor with Esc and selects BEGIN EXPEDITION, all connected participants are moved into the combat room and enemies spawn immediately. There is no entry trigger checking whether everyone walked into the second space, and no ready vote. A host alone can also start. Rewards offer KEEP CURRENT or SWAP TOOL, including for downed participants. A downed host can select RESTART EXPEDITION from results or a wipe.

## Current behavior

The flow in SeLoop.cs (`../archive/side-effects-2026-09-12.zip`; retired file) is practice 0 → combat 1 → reward 2 → traversal 3 → results 4, with wipe 5 when everyone is down during combat or traversal. Traversal starts after every current participant chooses a reward.

- Practice contains walls, a ramp, a ledge, and metal/non-metal props.
- Combat in SeCombat.cs (`../archive/side-effects-2026-09-12.zip`; retired file) includes 2 metal melee knights and 1 ranged enemy: 2 types and 3 enemies. Telegraphs and cover are provided; defeating all enemies offers rewards.
- The central gap in SeLabBuilder.cs (`../archive/side-effects-2026-09-12.zip`; retired file) is 4.5m. The launch platform spans z44–50, the landing platform z54.5–64, and the right walking bypass x8–11 and z48–57. The central shortcut combines jumping with a staff impulse from behind.
- Activate the switch at `(0, 0.1, 59)` with E, then any living participant within 2m of exit `(0, 0.1, 63)` completes the team run. Everyone need not gather at the exit; downed allies remain in the results team state.
- A new session selects a seed from 1–999999. The `--se-seed` launch argument fixes it. Restart retains the seed so the team can learn under the same conditions. The host chooses the reward candidate and gust variant; perfectly identical physics trajectories are not guaranteed.
- Gust traversal accelerates living participants in z49–58 rightward at 7m/s² during seconds 6–8 of each 8-second cycle. The HUD warns during the preceding 1 second.

Falling returns a participant to the last safe position in a downed state. Attacks are blocked while crawling and help requests remain available. Holding E to rescue requires range and line of sight; leaving cancels progress. Entering practice, combat, or traversal resets health, down state, external velocity, rescue progress, and shield. Restart also resets tools and contribution records. BOOST-ASSISTED SWITCHES counts switch activations within 4 seconds of a genuinely new staff impulse; it does not infer player intention or fun.

## Tuning and tools

The code source for all values below is SeTuning.cs (`../archive/side-effects-2026-09-12.zip`; retired file), with serialized settings in SeLabTuning.asset (`../archive/side-effects-2026-09-12.zip`; retired file). All fields have been saved in the asset. Units are distance m, speed m/s, time s, and angle °; half-angles apply on either side of forward. These are initial experimental values, not release balance.

| Setting | Value |
|---|---|
| Maximum health / movement / crawling | 100 / 5.5 / 1.2 |
| Jump height / gravity | 1.3 / -24m/s² |
| Dodge speed / duration / cooldown | 10 / 0.18 / 0.9 |
| Exponential external-velocity drag / fall threshold y | 3s⁻¹ / -8 |
| Staff range / half-angle / healing | 6 / 35 / 30 |
| Staff horizontal impulse / upward impulse / cooldown | 9 / 3.8 / 0.8 |
| Repeated allied push protection | 0.65 |
| Rescue range / hold duration / revived health | 2.2 / 2 / 35 |
| Drag range / speed | 2.5 / 3 |
| Basic attack range / damage / cooldown | 2.3 / 20 / 0.35 |
| Hammer range / half-angle / damage | 3 / 55 / 50 |
| Hammer enemy impulse / user recoil / lift / cooldown | 13 / 12 / 3 / 1.1 |
| Magnet range / half-angle / pulling acceleration | 6 / 60 / 12m/s² |
| Shield duration / cooldown / blocking half-angle | 1.2 / 2 / 60 |
| Enemy telegraph / movement speed / damage | 0.9 / 2.2 / 18 |

The staff pushes full-health allies; during protection, healing remains available while additional pushing is blocked. The magnet shield blocks frontal attacks and pulls only metal enemies/props, excluding targets behind walls. The hammer gives the user backward recoil separately from its forward hit. Input and external velocity are separate so movement input does not erase recoil. Basic attacks and tools currently share one cooldown state.

## Networking and production path

The official Unity Transport 6.6.0 is recorded in manifest.json (`../../unity/NoReturns/Packages/manifest.json`; retired file). ProjectVersion.txt (`../../unity/NoReturns/ProjectSettings/ProjectVersion.txt`; retired file) records the Unity version. SeTransport.cs (`../archive/side-effects-2026-09-12.zip`; retired file) provides reliable sequenced transmission, fragmentation, and experimental delay/loss. SeGame.cs (`../archive/side-effects-2026-09-12.zip`; retired file) checks sender ownership, sequence, epoch, finite input, down state, and cooldown. The host determines the tool, range, occlusion, and actual targets. Clients cannot specify hits or health.

There is no client prediction or interpolation. Clients apply host results directly. The transmission threshold is 0.05s, but the Fixed Timestep (`../../unity/NoReturns/ProjectSettings/TimeManager.asset`; retired file) is 0.02s and the accumulator resets to 0 after sending. The normal interval is approximately 0.06s, or 16.7Hz; do not report 20Hz. `maxSnapshotStep` is the maximum displacement between consecutive received positions, not prediction error or correction magnitude. Controls under delay need separate human inspection.

Host departure displays a session-ended message; there is no host migration. Participant departure clears input, sequence, dragging, reward, and boost markers, and vacant IDs 1–3 are reused. Reconnect using F1/F2 or the connection buttons. Implementation of these boundaries and validation in the latest complete executable remain distinct.

[unity_mcp.py](../../tools/unity_mcp.py) is a local IPC bridge forwarding `initialize`, `tools/list`, and `tools/call` to the official Unity CLI stdio MCP server. It is separate from the game's multiplayer server. MCP handles scene creation/inspection; the CLI handles isolated checks/builds. No additional account, matchmaking, Steam, or persistent saves are included. SIDE EFFECTS/Create Lab creates the separate scene, and SIDE EFFECTS/Build Windows builds an explicit scene list into dedicated output. CarryLab, existing executables, and existing build-scene settings are preserved.

## Checks and evidence

Run from the repository root. side-effects.ps1 (`../archive/side-effects-2026-09-12.zip`; retired file) requires the official Unity CLI and corresponding Editor. Do not close the user's Editor without saving.

```powershell
powershell -ExecutionPolicy Bypass -File tools/side-effects.ps1 rules
powershell -ExecutionPolicy Bypass -File tools/side-effects.ps1 combat
powershell -ExecutionPolicy Bypass -File tools/side-effects.ps1 traversal
powershell -ExecutionPolicy Bypass -File tools/side-effects.ps1 build
python tools/run_side_effects_tests.py
python tools/run_side_effects_tests.py --players 2 --delay 50 --loss 1
python tools/run_side_effects_tests.py --players 4 --delay 50 --loss 1 --loop --seed 7341
```

The network harness (`../archive/side-effects-2026-09-12.zip`; retired file) launches actual Windows processes with `-batchmode -nographics`. `--delay 50` is sender-side delay, giving a nominal 100ms round trip when applied in both directions. `--loss 1` is 1% outgoing packet loss, not a release pass threshold. The default test seed is 7341. Per-process JSON/logs and summary.json record exit codes and final state agreement. RTT below is the mean of ping responses across reconnects; frame and processing time add to nominal delay.

| Check | Current result and limit |
|---|---|
| A rules / CarryLab regression | MCP and final batch rules (`../archive/side-effects-2026-09-12.zip`; retired file) passed, including drag/wall/range / existing 24 checks passed. Regression log (`../archive/side-effects-2026-09-12.zip`; retired file) |
| B actual 2 processes, normal conditions | Passed on A/B build. Summary (`../archive/side-effects-2026-09-12.zip`; retired file) |
| B actual 2 processes, 50ms, 1% | Passed on A/B build. Summary (`../archive/side-effects-2026-09-12.zip`; retired file) |
| B actual 4 processes, 50ms, 1% | Passed on A/B build. Summary (`../archive/side-effects-2026-09-12.zip`; retired file) |
| C combat rules | Passed. combat-tests.log (`../archive/side-effects-2026-09-12.zip`; retired file) |
| Actual saved-scene 4.5m traversal comparison | Passed: ordinary walking/basic jumping fall; jump+staff lands at z56.165 with health 100 and 1 impulse; right-side walking reaches switch/exit. traversal.log (`../archive/side-effects-2026-09-12.zip`; retired file) |
| Latest complete Windows build | Generated and launched. Build log (`../archive/side-effects-2026-09-12.zip`; retired file) |
| Latest 2/4-process full loop | Passed normal and 50ms/1% conditions. See latest results below |
| Visuals / humans / separate PC | Host/client combat and traversal images inspected / actual human completion, fun, and separate-PC validation unverified |

B checks bidirectional healing/pushing, duplicates/range, down/revival at partial health, full-health pushing, and healing during protection. `--loop` checks reward, switch, results, restart, wipe, and other progression, but uses test fixtures to remove enemies and move players to objective positions. Actual combat damage-rule tests, saved-scene movement-physics tests, and a human completing combat through the exit provide distinct evidence.

Save a rendered sample by passing `--se-capture <absolute PNG path> --se-capture-after 4` to the executable. `--se-capture-only` exits after saving. Do not combine it with graphics-disabled options. This captures the rendered camera/HUD, not human control validation.

## Human play procedure and remaining work

1. Let two people experience their first healing push in practice with minimal explanation, then explain the accident and expected landing location.
2. Swap roles and agree on jump timing, firing direction, and landing target for the second attempt. Record whether the first mistake changed the plan.
3. Compare the central shortcut with the right walking bypass, and gust/no-gust variants of the same space. Distinguish intentional reuse from simply retrying.
4. Experience down, dragging, and rescue in both directions; ask whether rescue feels like waiting or repetitive work. Check results with a downed host, wipe restart, departure, and rejoining.
5. Complete combat→reward→traversal→results through actual inputs. Keep human explanations, observations, and video separate from automated logs.

Current art is temporary primitive geometry: navy/teal backgrounds, mint healing, amber hazard patterns, numbers, hats, capes, large gloves, and distinct tool shapes. Wall-clipped previews and active-shield indicators are also not final art. Human fun, separate-PC networking, controls under delay, and release quality remain unverified.


## Final multi-process results

Every run uses actual Windows player processes on the same PC. All cases agreed on final health, down state, tool, impulse count, rescues and position; all 19 host scenario checks passed. They include received out-of-range requests, cancellation after rescue progress, exit without the switch, downed reward selection/host restart, and actual disconnect/rejoin.

| Players | Send delay / loss | Client mean RTT(ms) | Maximum motion snapshot step(m) | Result |
|---|---|---|---|---|
| 2 | 0ms / 0% | 55.8 | 0.493 | PASS (`../archive/side-effects-2026-09-12.zip`; retired file) |
| 2 | 50ms / 1% | 122.8 | 0.427 | PASS (`../archive/side-effects-2026-09-12.zip`; retired file) |
| 4 | 0ms / 0% | 50.0, 51.3, 50.0 | 0.458 | PASS (`../archive/side-effects-2026-09-12.zip`; retired file) |
| 4 | 50ms / 1% | 135.2, 133.4, 118.0 | 0.493 | PASS (`../archive/side-effects-2026-09-12.zip`; retired file) |

The position metric is not prediction correction. It measures actual displacement between consecutive authoritative snapshots during the automated motion phases, excluding fixture relocation, room transitions and terminal freezing. Client input prediction/resimulation is absent; latency response remains a P1 follow-up.

The same saved floor produced x=-4.000 in calm conditions and x=-2.380 under the gust. Aggregate verification (`../archive/side-effects-2026-09-12.zip`; retired file), traversal host (`../archive/side-effects-2026-09-12.zip`; retired file), traversal client (`../archive/side-effects-2026-09-12.zip`; retired file), combat image (`../archive/side-effects-2026-09-12.zip`; retired file). These are scripted connection/stage-placement samples, not footage of humans completing the game.

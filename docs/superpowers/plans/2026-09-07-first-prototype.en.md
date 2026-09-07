# First playable foundation — implementation plan

[한국어](2026-09-07-first-prototype.ko.md)

Version 0.1 · 2026-09-07 · Scope: first development prototype.

**Goal:** Run a third-person graybox depot on Windows, connect two instances, and carry, drop, throw, catch, and dispatch one shared crate.

**Architecture:** One host simulates players and crate physics. Guests send movement and interaction requests; the host validates identity, ownership and distance. Replicated snapshots drive guest presentation. Cargo ownership and one-time delivery rules live in a separately tested model. ENet is the temporary development transport behind the session boundary.

**Tech stack:** Godot 4.7.2 standard, GDScript, Compatibility renderer, ENet. Portable runtime stays under `.tools/godot/`. No external art or paid services.

**Global constraints:** Korean and English documents remain complete pairs. Global game copy is English; a Korean UI option is provided for this prototype. Exactly two players including the host; solo practice is a separate mode. No Steam connectivity claim. One shared standard crate, one correct bay A, one incorrect bay B, short recovery and restart. No permanent failure or inventory. Host departure returns the guest to the menu with a reason; guest departure releases the crate and pauses the shift. Reject nonfinite movement and invalid ownership. Never ship or invent a Steam App ID. The current repository is an uncommitted project scaffold; preserve existing files and work in the provided workspace.

Agentic workers may use subagent-driven-development for the independent cargo-rules task; the integrating worker executes the remaining dependent tasks locally. Keep reports bilingual and do not create commits without a repository base or manufacture a new user task.

## 1. Cargo rules and behavioral tests

Create `scripts/cargo_rules.gd` extending `RefCounted`, and `tests/test_cargo_rules.gd` extending `SceneTree`.

Contract:

```gdscript
var holder_id: int = 0
var delivered: bool = false
var score: int = 0
const PICKUP_DISTANCE: float = 2.4
func try_pickup(peer_id: int, player_position: Vector3, cargo_position: Vector3) -> bool
func try_release(peer_id: int) -> bool
func try_dispatch(dock_id: int) -> bool
func remove_player(peer_id: int) -> void
func reset_crate() -> void
func reset_shift() -> void
```

Positive peer IDs may pick up a free, undelivered crate within 2.4 meters. Nonfinite coordinates fail. Only the holder may release. Dispatch only an unheld, undelivered crate to dock 1; increment score once. Removing the holder releases the crate. Resetting the crate clears holder/delivered but preserves score; resetting the shift also clears score. A reset represents a new crate, so that crate may score once again. The world owns connection membership and contact detection.

First run meaningful assertions against the missing module and observe the intentional missing-feature failure, then implement and re-run. Cover contested pickup, distance and invalid vectors, foreign release, held/wrong/duplicate delivery, disconnect, and reset boundaries. Command: `.tools/godot/Godot_v4.7.2-stable_win64_console.exe --headless --path . --script tests/test_cargo_rules.gd`; expect exit 0 and a PASS summary after implementation.

## 2. Playable third-person scene

Create `project.godot`, `scenes/main.tscn`, `scripts/main.gd`, `scripts/depot.gd`, `scripts/worker.gd`, and `scripts/interface.gd`.

The depot provides a floor, walls, center divider, intake/recovery spot and labeled bays A/B made of primitive meshes. Workers use CharacterBody3D with stable camera-relative movement, collisions, jump and orbit camera collision. The host moves the RigidBody3D crate while free; holding freezes it at the carrier's hand anchor. Interact is forgiving proximity pickup/catch or putdown; throw uses a fixed upward arc along aim, with a landing marker. Clamp transport inputs and expire stale movement. Recovery returns lost or wrongly dispatched cargo to intake. A short five-delivery, three-minute training shift is an explicit prototype value, not the final 10–15-minute shift. Completing or timing out offers restart; solo practice uses the same rules.

The UI provides Practice, Host, Join, address entry, language switch, controls, status, score/time, error messages and a pause/leave menu. Holding Escape releases the mouse via a normal menu action; pausing locally stops input, not the host simulation. Buttons must remain usable after disconnect or a failed connection. Global English strings and Korean equivalents live in `scripts/copy.gd`.

## 3. Two-instance transport and integration evidence

Create `scripts/session.gd` and `tests/test_network.gd`. ENet uses UDP port 27842 and one guest slot by default. Join timeout is 10 seconds. All network RPCs have a stable node path. The host authorizes actions using the remote sender ID, never a supplied player ID. The host owns movement integration, score, quota, timer, crate ownership and crate physics. Guests do not send position, score or crate ownership. Reliable actions and unreliable ordered snapshots are separate. Joining during a shift is rejected; a guest can join a waiting lobby. Host starts once both workers are present. Solo practice requires no socket.

Automated integration uses two real Godot processes and a dedicated port. Verify connection, guest movement visible on host, guest pickup/release seen on both peers, one delivery on both peers and guest disconnect cleanup. Include a negative non-holder action. Give all tests timeouts and nonzero failure codes. This proves local ENet behavior only; remote machine, NAT, Steam overlay/invites and release quality remain unverified.

## 4. Run, inspect, and hand off

Create ASCII Windows launchers `PLAY.cmd`, `PLAY_TWO.cmd`, `EDIT.cmd`; resolve paths relative to the launchers. `PLAY_TWO.cmd` opens two interactive windows only when the user runs it. Runtime download and official SHA512 verification are recorded in the guide. Keep downloaded editor archives, runtime, caches, screenshots and test logs out of source control. Do not label an editor-backed launcher a standalone Steam export.

Create complete `docs/prototype/01-first-playable.en.md` and `.ko.md` with exact run/edit/test steps, controls, expected feedback, evidence, limitations and the next experiment. Update both READMEs with the playable entry point and honest Steam status.

Validation: import all scripts with `--headless --editor --quit`; run cargo rules and two-process integration; launch the actual scene with the Compatibility renderer, capture viewport screenshots and inspect menu and depot; check new document links and language pairs. Resolve script errors and review findings before completion. Native Windows export templates, other hardware, controller support, latency compensation and commercial release testing are outside this checkpoint.

## Sources and status

- [Official Windows download](https://godotengine.org/download/windows/): runtime 4.7.2 selected; downloaded checksum and executable version must be verified locally.
- [Godot high-level multiplayer](https://docs.godotengine.org/en/stable/tutorials/networking/high_level_multiplayer.html): ENet setup and host RPC authority.
- All tasks above are planned at document creation. The paired prototype guide records completed evidence; unchecked release requirements remain unchanged.

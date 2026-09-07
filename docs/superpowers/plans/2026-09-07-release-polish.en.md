# Cooperative contract campaign implementation plan

[한국어](2026-09-07-release-polish.ko.md)

Target: 0.7 playable development build, not a Steam-approved release. Apply writing-plans and subagent-driven-development. The user authorized detailed design and implementation without incremental approval. Existing 0.6 remains recoverable in local Git. No paid assets, remote publication or account actions.

## Design and boundaries

Create a complete three-contract run in the existing depot. Main menu offers solo campaign, cooperative campaign hosting, join, and the existing short practice. Preserve existing practice/host test APIs unless an explicit campaign argument is passed. Campaign stages use 240 seconds and the original crew quota plus 0/2/4. Four cargo types remain available. After each successful contract, bank 10 credits per delivery, 5 extra for a qualifying team relay and 20 completion credits. Failed attempts bank nothing. The host buys shared upgrades only between successful contracts: boots (20, +8% movement, cap 2), overtime permit (25, +20 seconds per contract, cap 2), airhorn kit (20, reduces horn cooldown from 8 to 6 seconds, cap 1). Crew readiness votes start the next contract; three successful contracts end the run. No permanent gameplay advantage. Save only completed-run records locally.

Team relay: player A deliberately throws; another player catches in flight within 3 seconds and at least 3 m from release; package delivered to its correct bay earns one relay bonus. No bonus on repeated pickup, self-catch, ground pickup, wrong delivery, recovery or duplicate dispatch. Reset provenance on recovery. Announce successful relay and dispatch with short local synthesized sounds, obeying mute.

Packrat appears only in campaign contracts 2–3. One creature, obvious 0.9-second steal tell, free grounded cargo only, fixed patrol on the right lane and a reachable nest. Never steals held/attached cargo. R airhorn within 4 m and clear sight forces a drop and 3-second flee; 8-second per-worker cooldown (6 upgraded). No damage/death. Stolen cargo auto-drops at nest and cannot be stolen again for 8 seconds; drop on end/restart/disconnect. Host simulates; guests present snapshots.

## Ownership and interfaces

- Root: `scripts/main.gd`, `session.gd`, `contracts.gd`, `run_profile.gd`, `feedback.gd`, relay provenance in `cargo.gd`, integration tests, all paired documentation/build/export work.
- Creature implementer: ONLY `scripts/packrat.gd`, `tests/test_packrat.gd`. No other edits. API `reset(cargos: Dictionary)`, `start(stage: int)`, `step(delta, workers, cargos)`, `scare(worker, cargos)->bool`, `snapshot()->Array`, `apply_snapshot(data:Array)`, `stop(cargos)`, `status_key()->String`. Root calls only host step; no per-frame AI on guests. Creature cargo claim field `cargo.creature_held: bool` supplied by root; ordinary crate stays visible but frozen, collides only with static scenery while stolen. Root excludes claimed cargo from ordinary steps, pickup, conveyor, clinger acquisition and sneeze cargo push. Packrat must restore masks/layers/freeze on every drop. Root exposes `main.packrat`.
- Presentation implementer: ONLY `scripts/interface.gd`, NEW `scripts/campaign_copy.gd`, NEW `tests/capture_campaign.gd`, plus `scripts/preferences.gd`/`tests/test_preferences.gd` for fullscreen, master volume and FOV. Preserve existing interface references/helpers and existing state keys/tests. Do not edit main/copy/depot. New command actions: `campaign`, `host_campaign`, `practice`, `host`, `join`, `next_contract`, `buy_boots`, `buy_time`, `buy_horn`, `help`, `quit`; existing commands remain. `state.campaign` is optional Dictionary with enabled, stage(0-based), credits, earned, deliveries, relays, boots, time_upgrade, horn, finished, ready, voted, best_runs; `state.creature` optional localization key; `state.horn_left` float; `state.help` bool; `state.lesson` int 0..4; `state.progress` string. Render fallback defaults when absent. Root handles commands and help input state; UI emits the help command once. Campaign result replaces overtime control but preserves legacy controls. Settings use static Preferences.volume (0..1), fov(60..90), fullscreen(bool), apply_settings(). Save backward-compatible defaults and obey Preferences.enabled for test isolation.
- Motion packets stay <=1280 serialized bytes. Add separate authoritative reliable session metadata snapshots on change, and separate compact unreliable creature state (<=256 bytes), rather than expanding the full motion snapshot. Session version handshake must reject mismatched protocol before roster admission. Root owns these changes and packet tests.

## Work and evidence ledger

- [x] Full paired design with source-to-design distinctions, loops, numerical rules, world/creature/UI/audio/network details, QA criteria and release gates.
- [x] Contract rules and relay provenance; save validation and no duplicated reward; gameplay integration.
- [x] Packrat state machine and recovery tests; integrate R and reliable reset behavior.
- [x] Onboarding, readable campaign UI, settings and feedback; inspect actual Korean/English renders.
- [x] Protocol admission, split replication, real campaign host/guest tests, existing suite, export smoke and clean-folder rendering.
- [x] Paired guides/README/build paths, release status, local Git commit and reproducible Windows ZIP.

External release gates remain: Steamworks enrollment/application IDs, actual Steam transport integration and invites, two-PC WAN/latency checks, target-hardware profiling, independent cooperative playtesting, final licensed art/audio/store/trailer and Valve review. No claims that these are complete.


## Final development evidence — September 7, 2026

Target identifier: local Git `v0.7.0`. Full design: [33-section specification](../specs/2026-09-07-release-design.en.md); player guide: [0.7 contracts](../../prototype/07-contracts.en.md).

- `python tools/run_tests.py`: 21 behavior scripts, six host/guest scenarios and a four-process scenario passed. Log: `artifacts/full-07-tests.log`. The campaign fixture uses bounded repeated readiness/horn input so the deliberate 150 ms action rate limit does not discard its sole request.
- New physical tests cover actual airborne relay travel/catch, a real box settling for 30 physics frames before continuous nest transport, full-box obstacle checks and restoration. Floor contact tolerance prevents immediate false drops. Campaign integration verifies reward anti-farming, three-stage bank/upgrades/votes, final records, late-join exclusion and one-toggle help.
- `python -m unittest discover -s tests -p test_steampipe.py`: passed. No Steam upload or login.
- `python tools/build_windows.py`: Windows release export, packed smoke, six-file allowlisted archive and SHA256 manifest. `tests/audit_pack.gd`: 22 compiled gameplay scripts present.
- Actual OpenGL captures: `artifacts/campaign-menu-ko.png`, `campaign-menu-en.png`, `campaign-help-ko.png`, `campaign-help-en.png`, `campaign-settings-ko.png`, `campaign-contract-ko.png`, `campaign-contract-en.png`, `campaign-contract-guest-ko.png`. UI captures use constructed display states; they are not human playtests. A single bilingual system font prevents layout shifts after language changes.
- The executable and pack alone render a real solo campaign in `build/pack-probe-0.7/artifacts/world.png`; log `build/pack-probe-0.7/packed-world.log`. This proves local portable loading on this machine, not a second-machine installation.
- Review found and resolved first-launch volume, unearned late-join records, missing live tutorial text, help toggling, Korean horn formatting, grounded creature carry and guest body freeze issues. Final relevant campaign regression passed after UI changes.
- Restricted execution reports OS certificate-store and shader-cache/user-data permission warnings. Gameplay tests report no script errors; local ConfigFile persistence is verified using isolated test paths. Normal-user installation/persistence on another PC remains external QA.

Final transport: protocol 7; host-only admission; motion 20 Hz ≤1280 bytes, creature 10 Hz ≤256 bytes, campaign metadata checked at 5 Hz/on transitions and sent reliably only when changed (≤2048 bytes). No Steam relay/invites or host migration. Packrat final tuning: 2.7 m/s, theft radius 1.4 m, warning 0.9 s, horn 4 m / 8 s (6 upgraded), flee 3 s, returned-cargo protection 8 s. Credits and equipment reset for new runs; record format 1 keeps aggregates and last completed ID, with no older-folder migration.

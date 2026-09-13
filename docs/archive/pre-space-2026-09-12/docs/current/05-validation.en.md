# 5. Testing and release checklist

[한국어](05-validation.ko.md)

> **Historical record · retired/superseded 2026-09-12.** The [PSX space-delivery design](../../../../current/01-overview.en.md) is current. Implementation, launch, art and values below are historical, not current status. Deleted files remain path records only.


Main development is now NO RETURNS Unity. Godot specifications and checks below describe the preserved game, not completed Unity functionality. 현재 기준 / Current baseline (`18-unity-mainline.en.md`; retired file).

Reviewed: 2026-09-09 · Game 0.9.4 · Code baseline: this reactive-prop revision.


Checked items mean only the version-specific evidence scope stated below. Version 0.9.3 is historical; new checks for this 0.9.4 revision are separated below.

## Verified development baseline — 0.9.3

- [x] Recorded passes for 39 behavior checks, 10 two-peer scenarios and one four-peer scenario. `artifacts/animation-polish-suite.log`.
- [x] Recorded Windows export/package execution checks. `artifacts/packed-smoke-output.log`, `artifacts/polish-packed-render.log`.
- [x] A 16-second, 60 fps motion capture and sample review. `artifacts/motion-polish.mp4`.
- [x] Legacy lab carrying restoration test. `artifacts/lab-revert-test.log`.

Evidence lives locally in `artifacts/` and is not tracked by Git. Recheck evidence if missing or overwritten by another version. Automated checks and sampled review do not replace long human sessions. System-certificate warnings occurred, so do not claim warning-free logs.

## Checks by change type

| Change | Required checks |
|---|---|
| Documentation | Language counterparts, reciprocal links, paths, versions, numbers and status parity |
| Rules/values | Relevant behavior tests and comparison against intended rule changes |
| Physics/carrying/animation | Reproduce failure then pass after fix; inspect continuous motion and contact |
| Map | Spawns, boundaries, collisions, camera, routes, delivery, packrat and real cooperative routing |
| Networking | Relevant two/four-peer cases, version/map mismatch, duplication, latency and departure |
| Distribution | Export, included files, hashes, package execution and updated user guides |

Documentation check: `python tools/check_docs.py`. Full suite: `python tools/run_tests.py`. Packaging: `python tools/build_windows.py` or `BUILD.cmd`. Do not hide failures by weakening thresholds. Record command, build, date, pass/fail and limitations.

## Outstanding release gates

Current ECO-01 arithmetic is first four-player success 120 ≥ all maximum upgrades 110. This is a static calculation, not a new game-execution result. Economic changes require checking crew-specific income/purchases, choices in both shop windows and solo progression. Word content/structure in the completed worksheet and analysis (`07-mda.en.md`; retired file) was checked, but visual review remains incomplete because no page renderer is available.

For human fun evaluation, use the MDA observation questions (`07-mda.en.md`; retired file) to record spontaneous cooperation, transport mastery, route discovery, recovery and retries. These remain analytical hypotheses, not new play results or completion evidence.

- [ ] Separate Windows devices/networks with 2/3/4 players.
- [ ] Onboarding, completion and replay review by a new four-player group.
- [ ] Performance measurements and minimum/recommended specifications for the latest build.
- [ ] Final asset rights, generated-content disclosure and licenses.
- [ ] Actual Steam account/app/invite/install/store/build-review status.
- [ ] Copy, screenshots, video, pricing and support contact matching real functionality.
- [ ] User approval to release and verification of public availability/installation.

See the release task list (`../steam/05-release-checklist.md`; retired file) and roadmap (`../steam/01-release-roadmap.md`; retired file) for detailed procedures. Recheck fees, waiting periods and review requirements against official Steamworks documentation when executing them. This document is not a fresh platform-policy audit or approval evidence.

## 0.9.4 spring/impact validation — results and limits

- [x] Inspect the shipped GLB's merged spring/guide-pin mesh and continuously sample plate thickness and support endpoints.
- [x] Check strength-dependent paper speed, world-space direction under a rotated parent, rigid-body carton travel and reset.
- [x] Record 241 frames at 60 fps using the actual renderer and review compression, launch and scattering samples. `artifacts/reactive-fix.mp4`.
- [x] Confirm passing results for 41 behavior, 10 two-peer and one four-peer checks. One two-peer check passed on retest after a timeout.
- [x] Rebuild the Windows package, pass the execution smoke check and run 180 frames with the real graphics renderer. `artifacts/reactive-build.log`, `artifacts/reactive-packed-render.log`.

The first full run failed the existing 2,048-byte campaign metadata check after adding impact data. Compressed transmission with bounded decompression corrected it, and that check then passed. Final full log: `artifacts/reactive-fix-suite-final.log`. Decorative cartons collide with the map/debris and do not physically push workers/delivery cargo. Identical final debris positions, external networking and extended human co-op quality remain unverified.

Obtained passing results for 41 behavior checks, 10 two-peer scenarios and one four-peer scenario. The final throw-animation network check timed out once in the full run and passed on isolated retest; repeat-run stability needs further observation. Windows build/smoke checks and 180-frame packaged execution with the real renderer also passed. Evidence: `artifacts/reactive-fix-suite-final.log`, `artifacts/reactive-animation-network-retest.log`, `artifacts/reactive-four-final.log`, `artifacts/reactive-build.log`, `artifacts/reactive-packed-render.log`.


D validation: existing 41 behavior checks, 10 two-player scenarios, the four-player scenario and the separate D art check passed. See the D validation record (`../art/10-approved-d.en.md`; retired file) for sampled rendering and completed Windows package verification.

## Expansion design revision 2 — documentation and future validation · 2026-09-10

This task reviews the expansion design (`08-expansion-directions.en.md`; retired file) for language parity, paths, boundaries against current code, and duration/economy arithmetic. It does not rerun game tests, rendering, builds or human play. A documentation pass does not satisfy the feature checks below.

- [ ] Check carrying, receipt, recovery and routes without special parcels across 320 base configurations and 100 seeds.
- [ ] Check expedition payouts, prices, 4 slots, saves, duplicate events, reconnect and host departure.
- [ ] Record actual solo/two-player/three-player runs once each and 2 shifts each for 2 newcomer four-player groups.
- [ ] Review uneventful carrying/waiting segments exceeding 20 seconds and compare actual timing against basic 25–30 and optional-inclusive 30–40-minute targets.

No new evidence establishes fun, performance, external-network or release readiness.

## EXP-01 implementation · 2026-09-10

Passed 31 state/storage checks, map API, 1.8m carrying-clearance grid connectivity and blocked direct sightlines, physical receipt/wrong destination/early return/next shift. Actual separate-process local ENet two/four-player receipt/reward/return agreement passed. Evidence: python tools/run_expedition_tests.py, artifacts/expedition*-output.log. GPU menu/board/play/map samples: artifacts/expedition-render.log and expedition-*.png. Recaptured after lighting/spawn-view adjustments. Tests use real 0.4-second receipt with no shortened threshold. After initial network timeout, switched output collection to files and passed rerun. Environment certificate/shader-cache warnings remain. Human fun, external networking, endurance and forced shutdown during saving remain unverified.

Play guide and limits (`../prototype/10-expedition.en.md`; retired file).

Final package validation: Windows export/basic launch passed; --expedition ran the packaged GPU build for 120 frames, exit 0 (artifacts/expedition-packed-render.log). Verified latest expedition guides, SHA256 and ZIP integrity. Documentation checks passed for 30 entries. test_expedition_boot.gd also passed direct-launch-to-main-menu return without repeated auto-entry. No script/scene errors beyond the existing certificate-store warning.


## Unity development environment · 2026-09-11

Editor 6000.6.0f1, CLI 1.0.0-beta.9, URP 17.6.0, Pipeline 0.6.0-exp.1. Isolated project creation, Hub registration, C# compilation, development scene creation, environment validation and Windows development build succeeded (exit 0). CLI read-back confirmed the open Editor's ready state and the Development scene objects. A 30-second player smoke run confirmed D3D11 and PhysX initialization and process survival before the test process was stopped. Visual quality, user controls, game migration, networking and release readiness were not validated.

Evidence: `artifacts/unity/setup.log`, `build.log`, `player.log`, `hierarchy.json`. Initial sandbox license connection denial was resolved through normal user execution. The URP template assigned only per-quality renderers, failing the default-renderer check; explicitly assigning the PC default renderer fixed the check. Mono thread/debugger shutdown warnings remain, but final initialization and build exited 0. Documentation links, language counterparts and new environment-document numbers were checked.

Environment guide (`14-unity-environment.en.md`; retired file).


## UNITY-02 · Movement, carrying and official MCP · 2026-09-11

Installed 13 official Unity skills, registered and revalidated Codex's Unity MCP path, and listed 149 tools. This conversation used actual tools/call requests through a stdio MCP runner. MCP created, saved and read back CarryLab, entered play mode and captured the HUD. Existing Development and the Godot game were preserved.

Unity 0.2.0 adds movement, a 3rd-person camera, jumping, physical pickup, dropping, charged throwing, fall/manual recovery, HUD and a separate launch button. All 24 physics checks passed. Reduced carrying travel was reproduced and fixed with grip-velocity compensation. TMP import timeouts were resolved by restoring 37 original resources from the installed package with GUIDs preserved while the Editor was closed. The HUD-menu error on an empty scene was corrected by explicitly opening CarryLab and revalidating.

Final Windows build exited 0. A 25-second player run confirmed PhysX initialization, process survival and no exceptions. The test process was forcibly stopped, so this is not graceful-exit validation. Bilingual number parity and documentation links passed. Evidence: `artifacts/unity/test.log`, `build.log`, `carry-player.log`, `carry-hierarchy.json`, `carry-lab.png`. Complete human control feel, final animation, networking and release readiness remain unverified. Current specification (`15-unity-controls.en.md`; retired file).


2026-09-12 · New Unity delivery experiment: NR-LOOP-01 launch and validation (`22-slapstick-loop.en.md`; retired file) — use that document for implementation, automated evidence and outstanding human validation.

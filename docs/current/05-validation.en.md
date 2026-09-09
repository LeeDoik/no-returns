# 5. Testing and release checklist

[한국어](05-validation.ko.md)

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

Current ECO-01 arithmetic is first four-player success 120 ≥ all maximum upgrades 110. This is a static calculation, not a new game-execution result. Economic changes require checking crew-specific income/purchases, choices in both shop windows and solo progression. Word content/structure in the [completed worksheet and analysis](07-mda.en.md) was checked, but visual review remains incomplete because no page renderer is available.

For human fun evaluation, use the [MDA observation questions](07-mda.en.md) to record spontaneous cooperation, transport mastery, route discovery, recovery and retries. These remain analytical hypotheses, not new play results or completion evidence.

- [ ] Separate Windows devices/networks with 2/3/4 players.
- [ ] Onboarding, completion and replay review by a new four-player group.
- [ ] Performance measurements and minimum/recommended specifications for the latest build.
- [ ] Final asset rights, generated-content disclosure and licenses.
- [ ] Actual Steam account/app/invite/install/store/build-review status.
- [ ] Copy, screenshots, video, pricing and support contact matching real functionality.
- [ ] User approval to release and verification of public availability/installation.

See the [release task list](../steam/05-release-checklist.md) and [roadmap](../steam/01-release-roadmap.md) for detailed procedures. Recheck fees, waiting periods and review requirements against official Steamworks documentation when executing them. This document is not a fresh platform-policy audit or approval evidence.

## 0.9.4 spring/impact validation — results and limits

- [x] Inspect the shipped GLB's merged spring/guide-pin mesh and continuously sample plate thickness and support endpoints.
- [x] Check strength-dependent paper speed, world-space direction under a rotated parent, rigid-body carton travel and reset.
- [x] Record 241 frames at 60 fps using the actual renderer and review compression, launch and scattering samples. `artifacts/reactive-fix.mp4`.
- [x] Confirm passing results for 41 behavior, 10 two-peer and one four-peer checks. One two-peer check passed on retest after a timeout.
- [x] Rebuild the Windows package, pass the execution smoke check and run 180 frames with the real graphics renderer. `artifacts/reactive-build.log`, `artifacts/reactive-packed-render.log`.

The first full run failed the existing 2,048-byte campaign metadata check after adding impact data. Compressed transmission with bounded decompression corrected it, and that check then passed. Final full log: `artifacts/reactive-fix-suite-final.log`. Decorative cartons collide with the map/debris and do not physically push workers/delivery cargo. Identical final debris positions, external networking and extended human co-op quality remain unverified.

Obtained passing results for 41 behavior checks, 10 two-peer scenarios and one four-peer scenario. The final throw-animation network check timed out once in the full run and passed on isolated retest; repeat-run stability needs further observation. Windows build/smoke checks and 180-frame packaged execution with the real renderer also passed. Evidence: `artifacts/reactive-fix-suite-final.log`, `artifacts/reactive-animation-network-retest.log`, `artifacts/reactive-four-final.log`, `artifacts/reactive-build.log`, `artifacts/reactive-packed-render.log`.

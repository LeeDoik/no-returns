# 5. Testing and release checklist

[한국어](05-validation.ko.md)

Reviewed: 2026-09-09 · Game 0.9.3 · Code baseline `1d8fbde` (update when behavior changes).


Checked items mean only the evidence scope stated below. This documentation reorganization did not rerun game tests.

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

- [ ] Separate Windows devices/networks with 2/3/4 players.
- [ ] Onboarding, completion and replay review by a new four-player group.
- [ ] Performance measurements and minimum/recommended specifications for the latest build.
- [ ] Final asset rights, generated-content disclosure and licenses.
- [ ] Actual Steam account/app/invite/install/store/build-review status.
- [ ] Copy, screenshots, video, pricing and support contact matching real functionality.
- [ ] User approval to release and verification of public availability/installation.

See the [release task list](../steam/05-release-checklist.md) and [roadmap](../steam/01-release-roadmap.md) for detailed procedures. Recheck fees, waiting periods and review requirements against official Steamworks documentation when executing them. This document is not a fresh platform-policy audit or approval evidence.

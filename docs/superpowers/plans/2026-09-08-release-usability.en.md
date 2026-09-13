# Release usability audit and fixes · 0.7.9

[한국어](2026-09-08-release-usability.ko.md)

> **Historical record · retired/superseded 2026-09-12.** The [PSX space-delivery design](../../current/01-overview.en.md) is current. Implementation, launch, art and values below are historical, not current status. Deleted files remain path records only.


> Documentation update: versions and values here belong to this file's original context. Check the [documentation home](../../README.en.md) first for current rules and outstanding work.

## Goal and direction

Audit first launch, settings, in-game menus, exit, network connection and saving for a Steam Windows release. Compared with adding only visuals or rewriting the whole game, prioritize actual player flows and defects in the existing game. Preserve cooperative delivery and the directly editable map. Follow the user's instruction to implement, validate and package without per-step approvals.

## Tasks

- [x] Inspect screens and reproduce failures: Korean/English menu, manual and settings layout, solo pause, admission timeout and independent code-review findings.
- [x] Menus and settings: expose settings before and during play, scrollable settings/manual, viewport fitting, clear keyboard focus, leave/quit confirmation and return. Use `interface.gd`, new `settings_panel.gd` and `release_copy.gd`.
- [x] Controls and environment: persist key rebinding with conflict checks/cancel, prompts reflecting actual bindings, sensitivity/volume/FOV sliders, invert Y, VSync and frame cap. Update `input_bindings.gd`, `preferences.gd`, `worker.gd` and copy.
- [x] Pause and connection: freeze solo physics/time while blocking only local input online. Do not auto-resume after focus loss. Bound connection admission and reproduce/fix game-flow defects confirmed by review.
- [x] Run behavior regressions, two/four-process network checks, actual Korean/English renders, Windows export, packed launch and content audit. Preserve map editing.
- [x] Update both language guides and release checklist using actual evidence; record local main and v0.7.9.

## Release assessment

This pass does not declare Steam release completion. Actual Steamworks registration and App/Depot IDs, Steam invitations/relay implementation and two-PC validation, target-hardware performance, external playtests, final art/rights/store/review remain separate unfinished work. Store claims must match the shipping build, and both the store and build require Valve review. [Official review process](https://partner.steamgames.com/doc/store/review_process), [overlay and pause](https://partner.steamgames.com/doc/features/overlay). Checked: 2026-09-08.

## Validation record

- Reproduced defects first. Solo time/physics, settings access, admission timeout and completed-record retries failed before fixes and passed afterward. A damaged preference value type caused a script error before the fix and passed default recovery afterward.
- Covered host next-contract/overtime starts, guest shift start, canceling exit over settings, closing the window during key capture and keyboard-focus transition from connecting to a ready lobby.
- Final full suite: 30 behavior checks, 8 two-player scenarios and 1 four-player scenario passed. One SteamPipe preview-configuration generation test also passed. Network tests use separate processes on one PC, not Steam or an external network.
- Inspected actual Korean/English menu/settings/manual/result/pause/exit renders, the bottom of the control-settings scroll and 1024×768 / 1600×900 layouts. Bilingual UI geometry and prevention of background keyboard focus under modals passed as well.
- Fixed independently reviewed admission waiting, record retry and contract-transition/nested-menu defects; the affected paths were reviewed again.

Source logs: `artifacts/release-verified-suite.log`, `artifacts/release-resize.log`. Windows build validation is recorded below. The user's separate `project.godot` changes and `art/` work are excluded from this change.

Windows export and packed launch passed. Verified 32 compiled runtime scripts and saved-map identity in the package. ZIP integrity and the SHA256 manifest match the distributed files. Executable: `build/NO_RETURNS_0.7/NO_RETURNS.exe`; shareable ZIP: `build/NO_RETURNS_0.7_Windows.zip`. Local baseline: `v0.7.9`.

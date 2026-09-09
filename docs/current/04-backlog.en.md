# 4. Open work and bug backlog

[한국어](04-backlog.ko.md)

## ECO-01 — Sustaining rewards and equipment choices

P1·Structure confirmed by code/arithmetic; experienced impact unverified. The minimum first-contract success payout for four players is 120 credits, exceeding the 110 cost of every maximum upgrade, so equipment progression can finish in the first shop. [Analysis and completed worksheet](07-mda.en.md). Price/availability changes remain proposals, not implementation. Completion criteria: compare income/purchase paths by crew size; if adjusted, check solo viability and choices in both shop windows, and record reasons for choices in actual co-op play. Retain current specification values until rules change.

Reviewed: 2026-09-09 · Game 0.9.4 · Code baseline: this reactive-prop revision.


P0 blocks execution/release; P1 affects the core experience; P2 is follow-up improvement. Do not label unreproduced reports as confirmed bugs. Development owns items unless assigned otherwise; account/publication approvals require user involvement. Closing an item requires evidence and a fix version.

| ID | Priority and state | Work | Completion criterion |
|---|---|---|---|
| REL-01 | P0 · no completion evidence | Confirm Steam onboarding, app creation and store/build submission state | Record actual App ID and approval/progress status without personal data |
| NET-01 | P1 · not implemented | Steam invites and Internet connection flow | Invite/join/leave/restart across devices on different networks |
| MAP-01 | P1 · user quality concern | Redesign map routes, spaces and interactions | Record route choices, confusion and replay response from a new four-player group; retest revisions |
| PHY-01 | P1 · quality work needed | Weight and wall behavior of fixed carrying | Review pickup, rotation, narrow passages and dropping with the user in lab and production |
| ANI-01 | P1 · partially improved | Source gait style, hand contact and slope planting | Separate clipping/sliding/transition issues using continuous footage and live play; resolve key cases |
| QA-01 | P1 · unverified | Long sessions, separate PCs and latency | Report 2/3/4-player completion, disconnect/host departure/restart and latency behavior |
| PERF-01 | P1 · unverified | Performance/specifications with current models | Measure FPS, frame times and memory on target low-end devices; do not reuse obsolete measurements |
| ART-01 | P1 · review required | Visual consistency, provenance and commercial-use terms | Verify final asset inventory, provenance/rights and scene readability |
| UX-01 | P2 · review required | Onboarding, non-color identification and silent play | New players understand controls, objectives and failure reasons without explanation |

## Resolved and withdrawn history

- Run Hip-origin mismatch: fixed/measured in 0.9.2; directional carry clips added.
- Single-zero apex misclassification, directional boundaries and stationary carry-turn footwork: improved/regression-tested in 0.9.3. This does not close overall animation quality.
- Force-driven two-hand grabbing: cancelled by the user after experimentation. `4b7bd1c` was reverted by `9ac45d6`. Do not treat it as scheduled for reintroduction.

## New issue format

ID / affected version / reproduction / expected behavior / actual behavior / frequency / video-log / impact / priority / state / owner / fix version and retest. Preserve user concerns while distinguishing them from reproduced evidence. Scope changes also update the [overview](01-overview.en.md).

## PROP-01 / PROP-02 — 0.9.4 revision

Fixed reproduced spring penetration and paper/carton responses that did not account adequately for direction/strength. Related behavior/network checks passed. The existing throw-network timeout and successful retest are recorded in the [validation document](05-validation.en.md). Frame-exact decorative-debris replication is outside scope.

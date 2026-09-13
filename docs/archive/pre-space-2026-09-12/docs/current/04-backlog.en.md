# 4. Open work and bug backlog

[한국어](04-backlog.ko.md)

> **Historical record · retired/superseded 2026-09-12.** The [PSX space-delivery design](../../../../current/01-overview.en.md) is current. Implementation, launch, art and values below are historical, not current status. Deleted files remain path records only.


Main development is now NO RETURNS Unity. Godot specifications and checks below describe the preserved game, not completed Unity functionality. 현재 기준 / Current baseline (`18-unity-mainline.en.md`; retired file).

## ECO-01 — Sustaining rewards and equipment choices

P1·Structure confirmed by code/arithmetic; experienced impact unverified. The minimum first-contract success payout for four players is 120 credits, exceeding the 110 cost of every maximum upgrade, so equipment progression can finish in the first shop. Analysis and completed worksheet (`07-mda.en.md`; retired file). Price/availability changes remain proposals, not implementation. Completion criteria: compare income/purchase paths by crew size; if adjusted, check solo viability and choices in both shop windows, and record reasons for choices in actual co-op play. Retain current specification values until rules change.

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


D trio integration and validation are recorded in the work record (`../art/10-approved-d.en.md`; retired file). ANI-01 and ART-01 have partial progress; slopes, human co-op, whole-map styling and rights review remain open.

## Expedition expansion — approved direction, implementation pending · 2026-09-10

Expansion design revision 2 (`08-expansion-directions.en.md`; retired file). P1 below ranks work within the expedition; it does not close or postpone existing release blockers. Preserve current game values and ANI-01, ART-01 and ECO-01 status.

| ID | Status | Work and completion criteria |
|---|---|---|
| EXP-01 | P1 · first playable implemented / human review pending | Fixed-layout truck preparation → 3 contracts → immediate payment → return; separate existing modes, no duplicate payout |
| EXP-02 | P1 · proposed / not implemented | 3 zones with safe/optional routes, 5 contracts and parcel tool uses; solo–4-player receipt without mandatory tools |
| EXP-03 | P1 · proposed / not implemented | 3 tools, 4 slots, company unlocks and price table; validate first purchase, second-shift choice and persistence |
| EXP-04 | P1 · proposed / not implemented | Rescue, safe recovery, online state and departure; preserve contracts and prevent duplicates/repeat payment |
| EXP-05 | P1 · proposed / unverified | 8 zone combinations, 320 base configurations, 100 seeds and human tests; record observations and revisions |

Measure damage thresholds, facility adhesion stability and trolley handling in their experiments. Paid truck progression and further regions require later design and are outside the present tool price table.

## EXP-01 implementation · 2026-09-10

Completed EXP-01 first playable and automated integration. EXP-02 only partially implements fixed-blockout connectivity; rooftops/device solutions pending. EXP-03 tools/purchases absent. EXP-04 partially implements storage/departure, not new rescue/safe sockets. EXP-05 random combinations/human fun review pending. Do not close existing release blockers.

Play guide and limits (`../prototype/10-expedition.en.md`; retired file).


## UNITY-01 · 2026-09-11

P1 · Follow-up implementation pending. An isolated Unity development environment is configured. UNITY-02 implements the first movement, camera and carrying lab. Final animation, networking, maps, saving, Steam migration and actual control-feel review remain pending. Do not mark full migration or release readiness complete. Unity project and CLI environment (`14-unity-environment.en.md`; retired file).


2026-09-12 · New Unity delivery experiment: NR-LOOP-01 launch and validation (`22-slapstick-loop.en.md`; retired file) — use that document for implementation, automated evidence and outstanding human validation.

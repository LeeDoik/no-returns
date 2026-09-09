# Plan → implement → review development loop

[한국어](06-iteration-loop.ko.md)

Status: proposed operating process · 2026-09-09 · baseline build 0.9.3. Writing this document does not start an automation, scheduled task or authorize release.

## Iteration unit

One iteration resolves one observable player problem. Planning records the problem, player impact, reproduction scene, exclusions and acceptance criteria first. Example: ANI-01 hand/parcel mismatch during carrying turns. Do not combine map expansion and physics redesign into that iteration. Do not reintroduce the cancelled force-grab experiment.

Implementation preserves the comparison baseline and a Git recovery point, then makes the smallest coherent change. Do not change independent variables together in one experiment. Produce a playable result and update relevant bilingual specifications, guides, backlog and work log.

Review has three layers: automated rules/collision/network checks; actual continuous play or footage of the same before/after scene; and human assessment of feel, comprehension and fun. Re-read acceptance criteria and seek counterexamples during review. Automated passes do not establish fun or release approval.

## Review outcomes and next actions

- Technical failure: fix the same problem and rerun affected checks. Do not lower acceptance criteria merely to pass.
- Technical pass with worse feel: revise or revert, rather than declaring improvement. Record the outcome.
- Technical pass without human feedback: retain technically verified/feel unverified status. Independent work can continue.
- Acceptance criteria met: close only the verified scope and select the next priority. Run full regressions for shared-behavior changes and integration.
- Two attempts with the same hypothesis and no evidence of improvement trigger reassessment of cause and approach before more constant tuning. This is a proposed review threshold, not an impossibility verdict.

## Two iteration cadences

Small iterations address one problem. Integration play after every 3–5 small iterations is proposed. Integration covers launch→join→contract→reward→next contract→exit and checks cumulative side effects and new-group responses. Risky shared changes trigger immediate integration review regardless of count.

Do not require user approval at every stage. Continue planning, implementation, technical review and documentation within approved scope. Do not claim to replace human fun assessment. For scope changes, account actions, publishing/release or other separately authorized decisions, prepare concrete reviewable results before requesting approval. Contacting other people or recruiting testers requires permission.

## Proposed sequence for this project

1. ANI-01: review animation/contact with existing physics unchanged. Start with pickup→turn→stop→drop in a narrow passage as the comparison scene.
2. MAP-01: validate detours, shortcuts and teammate help in one area through roughly ten minutes of play before expanding the whole map. Ten minutes is a proposed test scope, not a committed content target.
3. QA-01/NET-01/PERF-01: verify separate PCs/networks, invites, completion/restart and low-end performance. Investigate structural risks early alongside prior stages.
4. ART-01/UX-01: finalize consistent art, sound and guidance against validated gameplay.
5. REL-01: check account/app/store preparation early, but base the actual release decision on the [release checklist](05-validation.en.md).

## Exit criteria

Release is not determined by feature counts or a completion percentage. It requires a fixed agreed small scope, resolved execution/progression blockers, target-device performance criteria, real external co-op validation, newcomer comprehension/fun review, and rights/store/build-review/support readiness. Define performance targets and acceptable unresolved issues before testing. These conditions are not all satisfied today.

Keep iteration reports short: problem / change / evidence / verdict (pass, fail, unverified) / limitations / next problem. Link defects to the [backlog](04-backlog.en.md), actual behavior to the [specification](02-spec.en.md) and history to the [work log](../archive/change-log.en.md).

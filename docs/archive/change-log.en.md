# Work log

[한국어](change-log.ko.md)

## 2026-09-09 — Documentation reorganization

Game baseline 0.9.3 / code 1d8fbde. Created six-part current documentation, code-sourced values, priorities/states/completion criteria and evidence boundaries. Preserved historical paths with current-reference notices and archived the former README. Required bilingual documentation updates and work logging for every task in AGENTS.md. Game code, map, physics and uncommitted user settings were untouched. Passed local-link and bilingual checkbox checks for 20 current/archive entry documents and compared key values against code. Validation command: `python tools/check_docs.py`. Refer to prior 0.9.3 game-test evidence rather than rerunning tests for this task.

## Future entry format

Date / task and version / reason / actual changes / updated documents / verification and evidence / outstanding limits / reversion status. If code is unchanged, state that and record investigation conclusions.

## 2026-09-09 — Proposed development loop

Reviewed the documentation home, backlog and validation baseline; wrote bilingual guidance for problem-sized iterations, three review layers, pass/fail/unverified outcomes, integration cadence and release exit criteria. No automation or gameplay changes. Priorities/cadence are proposals; fun, performance and external co-op remain unverified. Validate links and bilingual checkbox states with `python tools/check_docs.py`.

## 2026-09-09 — 0.9.4 spring and impact responses

Reproduced user reports and corrected plate/merged-support contact. Revised paper/carton responses using incoming direction, speed/mass and impact point. Protocol 12; focused checks passed and rendered samples reviewed; integration results recorded below. The revision guide distinguishes decorative rigid bodies from authoritative gameplay. User project.godot changes and separate art work were untouched.

Obtained passing results for 41 behavior checks, 10 two-peer scenarios and one four-peer scenario. The final throw-animation network check timed out once in the full run and passed on isolated retest; repeat-run stability needs further observation. Windows build/smoke checks and 180-frame packaged execution with the real renderer also passed. Evidence: `artifacts/reactive-fix-suite-final.log`, `artifacts/reactive-animation-network-retest.log`, `artifacts/reactive-four-final.log`, `artifacts/reactive-build.log`, `artifacts/reactive-packed-render.log`.

# Proposed Codex and Claude Code collaboration

[한국어](collaboration.ko.md)

2026-09-13 · Task-allocation proposal for current version 0.8.1. This investigation did not install/connect Claude, change permissions, dispatch work, commit Git changes or create a worktree.

## Initial roles

Initially, let Codex retain core-code integration, Unity MCP, builds and executable validation because it holds the current implementation context, and let Claude Code start with independent review. This is not a claim about model superiority. Authors and reviewers can swap per feature, while one person remains responsible for integration. The user judges actual controls, fear, fun and art approval.

| Priority | Work for Claude | Deliverable |
|---|---|---|
| 1 | Independent review of outer pursuit, carrying, down, rescue and settlement | File/line, trigger, impact, reproduction status and minimal fix proposal |
| 2 | Missing test design and separate validation tools | Edge cases and execution evidence for occluded pursuit, target changes, disconnects and duplicate pay |
| 3 | Economy and pressure analysis | Hypotheses/comparisons for delivery time, escape margin, purchase effects and possible exploits |
| 4 | Isolated authoring tool or equipment experiment | Tool/prototype in separate files/scenes with integration instructions |

Economic analysis must not turn test values into release commitments. Code review alone does not establish reproduction or human fun. Preserve the existing art rule requiring image approval before model creation.

## Work that must not overlap in this project

- Do not let two sessions edit the same file simultaneously. Define allowed files, interfaces to preserve, deliverables and acceptance criteria per task.
- The initial review reads game sources and shared documents and reports in chat. If saving a review, use separate Korean/English files. The integrator updates shared current specifications and change logs.
- Only one task at a time controls the Unity Editor/MCP, build output and actual-player tests. [CarryRoom](../../NoReturns/Assets/_NoReturns/Runtime/CarryLab/CarryRoom.cs) currently uses TCP port 27841. Separate code directories alone do not prevent tests competing for that port.
- The [MCP wrapper](../../tools/unity_mcp.py) uses the NoReturnsUnity alias when it matches the project path. In another working directory, verify the actual connected project and output path. Do not accidentally edit the main checkout through its existing editor.
- Before concurrent implementation, organize the current state into a baseline commit and create a separate worktree from that commit. Current game sources include untracked Git files at the time of this investigation, so cloning the existing HEAD alone may omit the latest implementation. No commits or moves were performed here.

Claude Code supports worktree sessions with separate files and branches. The installed version and environment still need checking. [Official worktree documentation](https://code.claude.com/docs/en/worktrees), checked 2026-09-13.

## First assignment prompt

The following can be given to Claude Code. Writing this document does not dispatch the task.

> Independently review NO RETURNS 0.8.1. First read AGENTS.md, docs/README.en.md, docs/current/02-spec.en.md and docs/current/space-play-07.en.md. Do not edit game code, scenes, settings or shared documents, and do not start Unity MCP, builds or game processes.
>
> Review global outer-creature acquisition, wall detours and target changes; down, rescue and ship safety; delivery rewards and duplicate payments; and disconnect state handling. Do not label normal behavior a bug based on speculation. For each finding, give file/line, trigger, gameplay impact, actual reproduction status, minimal fix direction and required checks. Identify gaps in already-passing automated tests. Separate the code you inspected from checks you did not run, and report highest-priority findings first in Korean.

The integrator checks the findings against code and reproduction conditions, then implements fixes, validates and updates documents. A subsequent independent-development assignment also specifies file ownership and Unity access order.

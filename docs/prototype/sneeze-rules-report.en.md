# Sneeze rules implementation report

[한국어](sneeze-rules-report.ko.md)

> **Historical record · retired/superseded 2026-09-12.** The [PSX space-delivery design](../current/01-overview.en.md) is current. Implementation, launch, art and values below are historical, not current status. Deleted files remain path records only.


> Documentation update: versions and values here belong to this file's original context. Check the [documentation home](../README.en.md) first for current rules and outstanding work.

Version 0.2 · 2026-09-07 · Status: implemented and behaviorally verified.

## Scope

`scripts/sneeze_rules.gd` implements the isolated `RefCounted` Sneezer clock and targeting model from Task 1 of the 0.2 plan. `tests/test_sneeze_rules.gd` is a standalone `SceneTree` runner with nine behavioral groups and 67 assertion calls.

The clock starts with a six-second calm, advances through a 1.5-second windup and a 0.35-second burst, then selects a seeded calm interval from 6–9 seconds. Each `advance` call crosses at most one transition. Only the windup-to-burst transition returns `true` and increments the monotonic event ID. Reset cancels the current phase, restores the initial six-second calm and preserves the event ID. Zero, negative, NaN and infinite deltas leave state unchanged.

The static target filter uses normalized horizontal facing, horizontal distance up to 4.5 meters, a separate vertical difference up to 1.6 meters and a 50-degree inclusive cone half-angle. It rejects nonfinite samples, zero horizontal facing and targets that overlap the source in the XZ plane.

## Test evidence

RED was run before the production module existed:

```powershell
.tools/godot/Godot_v4.7.2-stable_win64_console.exe --headless --path . --log-file "C:\Users\LeeDoik\Documents\ChatGPT\게임제작_1\artifacts\sneeze-rules-red.log" --script tests/test_sneeze_rules.gd
```

Result: exit code 1. Godot reported `Preload file "res://scripts/sneeze_rules.gd" does not exist` and failed to load the test script. This was the intended missing-feature failure.

GREEN was run after implementation:

```powershell
.tools/godot/Godot_v4.7.2-stable_win64_console.exe --headless --path . --log-file "C:\Users\LeeDoik\Documents\ChatGPT\게임제작_1\artifacts\sneeze-rules-green.log" --script tests/test_sneeze_rules.gd
```

Result: exit code 0 with `PASS: sneeze rules behavioral tests` on Godot `4.7.2.stable.official.ed1daf0bf`.

Covered boundaries: exact and just-before clock transitions, no event during windup, one event at burst start, oversized frame limiting, seeded interval bounds and reproducibility, reset before and after a burst, finite-delta validation, forward/behind/side targets, exact and exceeded angle/range/height boundaries, vertical aim removal, overlapping XZ positions and nonfinite geometry samples.

## Observations

Float comparisons admit values equal to timing and geometry limits within Godot's approximate-float tolerance. This keeps the documented boundaries inclusive while rejecting the tested values 4.5001 meters, 1.6001 meters and 50.1 degrees.

Godot printed `Failed to read the root certificate store` after the passing offline test. It did not change the exit code or assertions. This isolated report does not claim scene, collision shielding, impulse, rendering, audio, network or multiplayer integration coverage.

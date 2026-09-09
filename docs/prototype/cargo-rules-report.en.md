# Cargo rules implementation report

[한국어](cargo-rules-report.ko.md)

> Documentation update: versions and values here belong to this file's original context. Check the [documentation home](../README.en.md) first for current rules and outstanding work.

Version 0.1 · 2026-09-07 · Status: implemented and behaviorally verified.

## Scope

`scripts/cargo_rules.gd` implements the isolated `RefCounted` cargo state model from Section 1 of the first prototype plan. `tests/test_cargo_rules.gd` is a standalone `SceneTree` runner with six behavioral groups and 45 assertions.

The model accepts pickup only for positive peer IDs, finite positions and a free, undelivered crate within 2.4 meters. It enforces holder-only release, dock 1 dispatch while free, one score per crate, holder cleanup on disconnect, crate reset with score preservation, and full shift reset. Connection membership and world contact detection remain outside this model as specified.

## Test evidence

RED was run before the production module existed:

```powershell
.tools/godot/Godot_v4.7.2-stable_win64_console.exe --headless --path . --log-file artifacts/cargo-rules-red.log --script tests/test_cargo_rules.gd
```

Result: exit code 1. Godot reported `Preload file "res://scripts/cargo_rules.gd" does not exist` and failed to load the test script. This was the intended missing-feature failure.

GREEN was run after implementation:

```powershell
.tools/godot/Godot_v4.7.2-stable_win64_console.exe --headless --path . --log-file cargo-rules-green.log --script tests/test_cargo_rules.gd
```

Result: exit code 0 with `PASS: cargo rules behavioral tests` on Godot `4.7.2.stable.official.ed1daf0bf`.

Covered boundaries: contested pickup, exact and exceeded pickup distance, zero and negative peer IDs, NaN and infinite coordinates, foreign and invalid release, dispatch while held, wrong and invalid docks, duplicate dispatch, pickup after delivery, holder and non-holder disconnect, crate reset, repeat scoring after crate reset, and shift reset.

## Observations

The first post-implementation run exposed float representation at the exact 2.4-meter boundary. The final comparison admits values equal within Godot's approximate-float tolerance while still rejecting 2.4001 meters.

Godot printed `Failed to read the root certificate store` after the passing offline test. It did not change the exit code or cargo-rule assertions. This report does not claim network, scene, transport, or multiplayer integration coverage.

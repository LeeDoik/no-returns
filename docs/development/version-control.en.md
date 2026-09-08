# Local Git version control

[한국어](version-control.ko.md)

September 7, 2026. The creator chose **local Git only**. No GitHub repository or remote connection is created.

## Baseline

Record expanded prototype 0.5 as the first commit and mark it with `v0.5.0`. This identifies the same baseline as the existing 0.5 UI; it does not represent a feature change or a new build. The default branch is `main`. Earlier 0.1–0.4 work had no separate commits, so it is not represented as recoverable Git history. Existing documents remain development records.

The baseline includes four cargo types, A/B destinations, crew quotas, pings, overtime, saved settings, cargo-facing fixes, worker/cargo collisions, Korean/English documents, tests and build tools.

## Tracked files

- Code, scenes, Godot project/export settings and `.gd.uid` identity files.
- Korean/English documents, tests, launch/build tools and license notices.
- `.gitignore`, `.gitattributes` and project working rules.

Exclude Godot cache `.godot/`, downloaded engine/templates `.tools/`, test artifacts `artifacts/`, executable/ZIP output `build/`, temporary files and logs. Environment secrets, export credentials, development Steam App ID files and certificates are also excluded. Rebuild executables using the build tools instead of committing them. Consider Git LFS separately when large original art assets become necessary.

Text is normalized to LF in the repository; Windows `.cmd`/`.bat` launchers use CRLF in the working folder.

## Future workflow

1. Inspect current changes before adding or fixing a feature. Use work branches named `codex/short-task-name`.
2. Update related code and both documentation languages together.
3. Run checks appropriate to the change and inspect the results. The complete game suite is `python tools/run_tests.py`.
4. Review and commit one coherent task. Do not mix unrelated user changes into it.
5. Integrate verified work into `main`. Add a new tag only for a distribution baseline.

Do not move an existing tag to another commit. Use versions such as `v0.5.1` for a patch distribution and `v0.6.0` for the next feature group. Tags identify internal prototypes and do not promise a commercial release.

## Inspect and recover

From the project terminal, use `git status` for uncommitted changes and `git log --oneline --decorate` for history. `git show v0.5.0` displays the first baseline. Committing records a group of changes separately from saving a file. [Official Git explanation](https://git-scm.com/book/en/v2/Getting-Started-About-Version-Control)

To roll back, specify the desired version. Preserve current work first, then use a separate recovery branch or a revert commit. Local history is not backed up to another computer or cloud; external backup is a separate protection against disk failure.

Restoring source into another folder or PC requires preparing Godot first. `tools/setup_godot.ps1` restores the pinned engine; Windows export also requires the matching templates described in the [build handoff](../steam/07-build-handoff.en.md).

## 0.6 night-map history

`v0.6.0` identifies the 32 × 36 m night depot and reversing conveyor update. Implement and verify on `codex/night-depot`, then integrate into `main`. Preserve the previous `v0.5.0` and 0.5 distribution folder. See the [0.6 play guide](../prototype/06-night-depot.en.md).


## 0.7 cooperative contracts record

`v0.7.0` is the development baseline for three contracts, shared bank/equipment, relay rewards, Packrat, help/settings and protocol 7. Validated changes from `codex/cooperative-contracts` are integrated into `main`. The 0.5/0.6 tags and old export folders remain. See [implementation/evidence](../superpowers/plans/2026-09-07-release-polish.en.md) and [play guide](../prototype/07-contracts.en.md). This does not mean Steam release completion.


## 0.7.3 Editable map record

`v0.7.3` adds a saved map scene, solid block prefab, editing/build launchers, placement-driven gameplay and protocol 8 map matching. Validated on `codex/editable-map` and integrated into main. [Map editing guide](map-editing.en.md).


## 0.7.4 Shipping Shrine expansion

Implement and validate the 48×60 m map, pressure shortcut and periodic airflow on `codex/shrine-expansion`, then integrate into main. Preserve `v0.7.4` as the new local baseline. The previous map remains in `v0.7.3` history. [Expansion record](../superpowers/plans/2026-09-08-shrine-expansion.en.md).


## 0.7.5 Winding routes

On `codex/winding-shrine`, replace parallel lanes with an S alley, turning airflow route and cross-links, validate and integrate into main. `v0.7.5` is the new baseline; `v0.7.4` retains the previous layout. [Change record](../superpowers/plans/2026-09-08-winding-shrine.en.md).

## 0.7.6 Enclosed rooms and parcel hatch

Apply room partitions, partial roofs and a parcel relay hatch on `codex/enclosed-shrine`. Integrate into main after validation and retain `v0.7.6` as the local milestone. Preserve the previous layout in `v0.7.5`.

## 0.7.7 Art direction pass

Integrate the validated visual cleanup and worker appearance changes from `codex/art-direction` into main and retain `v0.7.7` as the local milestone. The previous look remains in `v0.7.6`.

## 0.7.8 Focused intake and presentation refinement

`codex/authored-depot` refines reception, role-specific worktops, beveled geometry, carrying poses, labels and delivery audio. After validation, it is integrated into main with local baseline `v0.7.8`. The previous presentation remains at `v0.7.7`. [Implementation and validation](../superpowers/plans/2026-09-08-authored-depot.en.md).

# macOS development environment

[한국어](macos-development.ko.md)

## 2026-10-03 — Current tools and Blender MCP

Follow the [project rules](../../AGENTS.md) to check official releases and update tools used for work. Game 0.9.6 uses the committed [Unity version](../../NoReturns/ProjectSettings/ProjectVersion.txt) and [package manifest](../../NoReturns/Packages/manifest.json). This table supersedes the historical environment records below.

| Tool | Installed and checked this task |
|---|---|
| Unity Editor | 6000.6.0f1 → **6000.6.4f1**, latest stable reported by official `unity releases`. Upgrade the project and check Mac build/play. |
| Unity CLI / Pipeline | **1.0.0-beta.12 / 0.8.0-exp.1**, using `unity self-update` and `unity pipeline upgrade`. Confirm the live Editor server at 127.0.0.1:7800. |
| Blender | **5.2.2 LTS**, installed version matches the [official distribution reference](https://formulae.brew.sh/api/cask/blender.json). |
| Blender MCP | Install [mcp-for-blender](https://github.com/ahujasid/mcp-for-blender) **2.1.3** and check [PyPI release metadata](https://pypi.org/pypi/mcp-for-blender/json). The add-on identifies itself as 1.8/protocol 13, distinct from the package version. |
| uv | **0.12.22**, update and verify against [Homebrew release metadata](https://formulae.brew.sh/api/formula/uv.json). |
| Codex CLI | **0.160.0**, update through the [official installer](https://chatgpt.com/codex/install.sh) and verify the executable version. |
| Codex desktop | Installed **26.930.21537 (12776)**; the app updater offers **26.930.31730 (12947)**. The updater tool is read-only and automation of Codex itself is blocked, so the user must update/restart the app. Do not count the latest app as installed. |

Earlier work used Blender Python batch execution. This task installs/enables the add-on in the currently open Blender, then uses an actual MCP stdio client to call `get_scene_info` and read-only `execute_blender_code`. Read the current employee scene's 5 objects, 53 bones and right-hand information without saving or reopening its unsaved work. The local endpoint is 127.0.0.1:9876. This conversation's default tool list does not automatically include the new MCP, so check it after updating/restarting Codex. A socket response alone is not proof of MCP tool registration.

Reproduction commands for another environment follow. `@latest` resolves the latest package when the connection starts; record the resolved version in task evidence. After updating the add-on, preserve unsaved work and restart Blender or reload the add-on, then check both sides' versions. Keep personal Codex configuration out of the repository.

```sh
uvx mcp-for-blender@latest install-addon
codex mcp add blender --env DISABLE_TELEMETRY=true -- uvx mcp-for-blender@latest
```

Enable Blender MCP in `Preferences > Add-ons`. This Mac registers the absolute `/opt/homebrew/bin/uvx` path and disables telemetry. Read the connected scene, selection and unsaved state before acting; preserve original files. Continue using existing Blender Python scripts for straightforward reproduction and batch validation.

The new Unity's Windows support module/Windows execution, Codex desktop update and default MCP tool exposure after restart remain unverified. Preserve the earlier Unity installation and unsaved Blender file. See [game integration and validation scope](employee-animation.en.md) and the [validation record](../validation/baton-hand-0.9.6.json).

## 2026-10-01 — Cinder delivery, receipt and return settlement

[CINDER-DELIVERY-01 usage, coordinates and evidence](four-player.en.md#2026-10-01--cinder-delivery-receipt-and-return-settlement). In separate `CinderFourPlayerTest`, connect ship E preparation/arrival → BAY 04 floor delivery → CRT receipt E collection → all crew aboard/E return/420 CR settlement → next shift. Retain version 0.9.1, protocol 10 and TCP 27842. Reuse the existing delivery ledger, parcel, CRT, KO/EN screen and label tooling. Preserve the source environment scene; supersede older unconnected-delivery/receipt statements below only within this test scope. Default launch is delivery; `start --map-only` and `check` retain the movement test. Listener/baton/suppression/beacon/save and four-human/other-PC/performance validation remain incomplete. Actual automated evidence is in the [validation record](../validation/cinder-delivery-01.json).

## 2026-10-01 — Current Cinder Mac four-player test

The current environment/art source is `CinderCompactSiteReview`; the derived four-player test is `CinderFourPlayerTest`. [Four-player launch/build/evidence](four-player.en.md#2026-10-01--cinder-four-player-map-test-environment) supersedes the 2026-09-26 target/unverified-build record below. `CinderFourPlayerBuild` preserves the source and builds `builds/CinderFourPlayer/NoReturns.app`. **Mac build: 0 errors/7 existing warnings; pass 13 actual four-process checks, 7 existing rescue-rule conditions and 7 regular-mode defaults. Inspect 2 native 800×500 Korean HUD captures for crew 4/4, E/Q controls, reticle and team colors; also verify 4 manual processes/3 connections and shutdown.** Release signing/notarization, Windows execution, other-PC play, performance and full gameplay remain unverified separately.

2026-09-26. Continue the existing Windows project on an Apple Silicon Mac. The user selected macOS and Windows as targets. This setup does not change game features or Editor/package versions.

Continue development in the `CinderDepotBlockout` scene from [CINDER-BLOCKOUT-01](cinder-blockout.en.md). The user-identified `Play_Cinder_Blockout.cmd` and commit `6f4922a` dated 2026-09-17 point to its Windows player. `CarryRoom`, used for initial environment checks, is an earlier carrying experiment; those checks do not validate the latest map. The existing Windows player was not restored on this Mac because Git excludes `builds/`.

## Components

| Component | Version and purpose |
|---|---|
| Unity Editor | **6000.6.0f1**, Apple Silicon. Match the [project version](../../NoReturns/ProjectSettings/ProjectVersion.txt). |
| Official Unity CLI | **1.0.0-beta.11**. Install Editors, open projects and invoke commands. |
| Unity Pipeline | Retain existing **0.7.0-exp.1**. Connect the running Editor to CLI/MCP. [Package manifest](../../NoReturns/Packages/manifest.json). |
| Git LFS | **3.8.0**. Restore large sources such as models and textures. |
| Build support | The Editor's macOS Mono support plus Windows Build Support (Mono). IL2CPP requires separate setup. |
| Existing tools | Unity Hub, Git, Xcode **27.0** and clang verified. |

Unity supplies C# compilation tools, so a separate .NET SDK or VS Code is not required for this CLI workflow. Configure an IDE debugger or Blender separately when those workflows are needed. Unity `Assets/` currently contains no directly imported `.blend` files or native plugin binaries.

## Open and check the connection

Run at the repository root. `NoReturns/` is the actual Unity project. The first launch takes time to download packages and import assets.

```sh
source "$HOME/.unity/env"
unity --version
unity open ./NoReturns
unity status --project-path "$PWD/NoReturns" --format json
unity command editor_status --caller plugin --skill unity-cli --project-path "$PWD/NoReturns" --format json
unity command open_scene --path Assets/_NoReturns/Scenes/CinderDepotBlockout.unity --caller plugin --skill unity-cli --project-path "$PWD/NoReturns" --format json
unity command --caller plugin --skill unity-cli --project-path "$PWD/NoReturns"
```

Ask Codex for changes in natural language. It edits C#, verifies compilation and behavior, and uses the connected Editor for scenes, prefabs and materials. Discover available commands first. If the connection fails, use `unity pipeline list --format json` to check Safe Mode and package status. CLI connectivity is separate from game multiplayer.

This Mac's Codex MCP configuration is at `.codex/config.toml` in the repository root and excluded through `.git/info/exclude`. It contains personal absolute paths and must not be committed. The CLI remains usable if new MCP tools have not appeared in the current session. Automatic MCP loading in a new session requires a separate check.

## Restore sources and build

On another computer, run `git lfs install --local`, `git lfs pull` and `git lfs fsck` to restore and check assets before opening the Editor. Track `.meta` files with assets; exclude caches, logs, builds and credentials. Windows `.cmd` launchers, `tools/unity.ps1` and `tools/unity_mcp.py` contain Windows paths, so use direct CLI commands on Mac.

These CLI examples are not records of successful game builds. The current [default build-scene list](../../NoReturns/ProjectSettings/EditorBuildSettings.asset) enables only `Bootstrap`. Before producing a playable game, configure the `CinderDepotBlockout` scene in the Editor. Existing `CarryBuild`, `ShipInteriorTrialBuild` and `CinderBlockoutBuild` scripts hard-code Windows and cannot be used unchanged for macOS builds. The existing Cinder Build menu regenerates the scene; do not use it when preserving manual scene edits. Save changes and close this project's Editor first.

```sh
unity build ./NoReturns --target StandaloneOSX --output-path "$PWD/builds/macOS/NO_RETURNS.app"
unity build ./NoReturns --target StandaloneWindows64 --output-path "$PWD/builds/Windows/NO_RETURNS.exe"
```

Actual macOS play, distribution signing/notarization and Windows player testing on Windows are separate checks. Environment setup does not establish release readiness on either platform.

## Korean font and recompilation

The [shared language code](../../NoReturns/Assets/_NoReturns/Runtime/CarryLab/CarryLanguage.cs) loads bundled `NotoSansKR-Regular` instead of system fonts. On this Mac, Unity 6000.6.0f1 failed to convert fonts created by `CreateDynamicFontFromOSFont` through IMGUI's TextCore path. Removed the Windows Malgun Gothic dependency; menus and HUD share the bundled font. The approximately 4.6 MB [upstream OTF](https://github.com/notofonts/noto-cjk/blob/main/Sans/SubsetOTF/KR/NotoSansKR-Regular.otf) is unmodified and accompanied by the [original OFL](../../NoReturns/Assets/_NoReturns/Resources/Fonts/NotoSansKR-LICENSE.txt) and Unity-generated `.meta` files. Git LFS tracks the OTF; it is not installed system-wide.

The test directory and received state in [CarryRoom](../../NoReturns/Assets/_NoReturns/Runtime/CarryLab/CarryRoom.cs) are runtime-only values. Excluded them from serialization so Editor code reload cannot restore `null` as an empty string or empty state. This prevents the empty-path exception and empty rotation-array access seen after recompilation.

The font check needs no separate framework. Stop Play mode in the open Editor and run from the repository root:

```sh
unity command eval_file --file "$PWD/tools/unity_checks/FontCheck.cs" --caller plugin --skill unity-cli --project-path "$PWD/NoReturns" --format json
```

The [check](../../tools/unity_checks/FontCheck.cs) verifies dynamic-font loading, TextCore font-data loading and Korean/Latin/digit glyphs. A successful result is `FONT PASS: NotoSansKR-Regular`.

## Verification in this setup

- [x] Repository cloned; CLI version and PATH in a fresh login shell verified.
- [x] Restored **296** LFS files totaling **852,525,080 bytes**, with zero remaining pointers and a successful `git lfs fsck`.
- [x] Unity account signed in, project registered with Hub and project-specific Codex MCP configured.
- [x] Verified active Unity Personal licensing. The user directly accepted the first-launch Editor terms.
- [x] Editor and Windows Mono module installed and `unity editors verify` passed. Verified the executable's `arm64` architecture and the macOS build-support directory.
- [x] Verified compilation and live CLI connectivity: `ready`, no compilation failure, zero Console errors and 7 existing deprecated-API warnings.
- [x] Passed discovery of 151 CLI commands, C# `eval`, opening/querying the `CarryRoom` scene, and MCP initialization/tool listing/actual `editor_status` invocation.
- [x] Passed bundled-font checks, starting/stopping `CarryRoom` Play mode after recompilation, and visual inspection of the Korean menu. After preserving previous logs and clearing the Console, the new run recorded zero errors and zero warnings.
- [x] Corrected the current target to `CinderDepotBlockout`, opened the existing scene without changes and checked its root hierarchy. Mac Play mode/builds for this scene remain unverified.
- [ ] Verify automatic MCP loading in a new Codex session.
- [ ] Verify macOS/Windows builds and actual gameplay.

Development environment installation and live connection verification are complete. `CinderDepotBlockout` is now open and unmodified. The 7 compiler warnings concern existing object-search APIs in `FacilityArt`, `CarryRoom` and `ShipInteriorTrialBuild` and were not fixed. Gameplay beyond menu startup, game builds and human controls were not tested. A layout issue remains: the help text overlaps the quit button in the small Game view.

During initial import, Unity cleared only the generated runtime list in the [URP global settings](../../NoReturns/Assets/Settings/UniversalRenderPipelineGlobalSettings.asset). The installed URP 17.6.0 `RenderPipelineGraphicsSettingsContainer` clears this list in the Editor and regenerates it during Player builds. Authored settings and scenes remain unchanged. This automatic normalization is included in the record.

Immediately after cloning, Git reported line-ending changes in two Windows launchers, although their raw bytes matched HEAD. Normalized their stored content to the existing `.gitattributes` and verified unchanged command content. `tools/check_docs.py` failed on **142** historical `artifacts/` links excluded from Git. The new documentation introduces no additional failures; do not fabricate historical validation outputs to fill the gaps.

In this CLI version, `install --resume` incorrectly selected an Intel entry for an existing download despite `--architecture arm64`. Stopped that invocation, verified the official Apple Silicon file's size/checksum and installed without `--resume`. Verify the actual architecture when reinstalling as well.

Official references: [Unity CLI](https://docs.unity.com/en-us/unity-cli/use-unity-cli), [Unity Pipeline](https://docs.unity.com/en-us/unity-cli/unity-pipeline/unity-pipeline-package).

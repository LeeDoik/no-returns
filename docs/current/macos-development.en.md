# macOS development environment

[한국어](macos-development.ko.md)

2026-09-26. Continue the existing Windows project on an Apple Silicon Mac. The user selected macOS and Windows as targets. This setup does not change game features or Editor/package versions.

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
unity command --caller plugin --skill unity-cli --project-path "$PWD/NoReturns"
```

Ask Codex for changes in natural language. It edits C#, verifies compilation and behavior, and uses the connected Editor for scenes, prefabs and materials. Discover available commands first. If the connection fails, use `unity pipeline list --format json` to check Safe Mode and package status. CLI connectivity is separate from game multiplayer.

This Mac's Codex MCP configuration is at `.codex/config.toml` in the repository root and excluded through `.git/info/exclude`. It contains personal absolute paths and must not be committed. The CLI remains usable if new MCP tools have not appeared in the current session. Automatic MCP loading in a new session requires a separate check.

## Restore sources and build

On another computer, run `git lfs install --local`, `git lfs pull` and `git lfs fsck` to restore and check assets before opening the Editor. Track `.meta` files with assets; exclude caches, logs, builds and credentials. Windows `.cmd` launchers, `tools/unity.ps1` and `tools/unity_mcp.py` contain Windows paths, so use direct CLI commands on Mac.

These CLI examples are not records of successful game builds. The current [default build-scene list](../../NoReturns/ProjectSettings/EditorBuildSettings.asset) enables only `Bootstrap`. Before producing a playable game, configure the intended scenes, such as `CarryRoom`, in the Editor. Existing `CarryBuild`, `ShipInteriorTrialBuild` and `CinderBlockoutBuild` scripts hard-code Windows and cannot be used unchanged for macOS builds. Save changes and close this project's Editor first.

```sh
unity build ./NoReturns --target StandaloneOSX --output-path "$PWD/builds/macOS/NO_RETURNS.app"
unity build ./NoReturns --target StandaloneWindows64 --output-path "$PWD/builds/Windows/NO_RETURNS.exe"
```

Actual macOS play, distribution signing/notarization and Windows player testing on Windows are separate checks. Environment setup does not establish release readiness on either platform.

## Verification in this setup

- [x] Repository cloned; CLI version and PATH in a fresh login shell verified.
- [x] Restored **296** LFS files totaling **852,525,080 bytes**, with zero remaining pointers and a successful `git lfs fsck`.
- [x] Unity account signed in, project registered with Hub and project-specific Codex MCP configured.
- [ ] Verify an active Unity license.
- [x] Editor and Windows Mono module installed and `unity editors verify` passed. Verified the executable's `arm64` architecture and the macOS build-support directory.
- [ ] Verify project compilation and a CLI connection to the running Editor.
- [ ] Verify automatic MCP loading in a new Codex session.
- [ ] Verify macOS/Windows builds and actual gameplay.

Installation is complete. Unity sign-in works, but there is no active license; Personal versus an existing license selection is pending. After activation, open the project to verify compilation and live CLI connectivity. Installation success is not recorded as successful gameplay.

Immediately after cloning, Git reported line-ending changes in two Windows launchers, although their raw bytes matched HEAD. Normalized their stored content to the existing `.gitattributes` and verified unchanged command content. `tools/check_docs.py` failed on **142** historical `artifacts/` links excluded from Git. The new documentation introduces no additional failures; do not fabricate historical validation outputs to fill the gaps.

In this CLI version, `install --resume` incorrectly selected an Intel entry for an existing download despite `--architecture arm64`. Stopped that invocation, verified the official Apple Silicon file's size/checksum and installed without `--resume`. Verify the actual architecture when reinstalling as well.

Official references: [Unity CLI](https://docs.unity.com/en-us/unity-cli/use-unity-cli), [Unity Pipeline](https://docs.unity.com/en-us/unity-cli/unity-pipeline/unity-pipeline-package).

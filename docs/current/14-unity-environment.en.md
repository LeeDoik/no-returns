# Unity development environment

[한국어](14-unity-environment.ko.md)

> **Historical record · retired/superseded 2026-09-12.** The [PSX space-delivery design](01-overview.en.md) is current. Implementation, launch, art and values below are historical, not current status. Deleted files remain path records only.


2026-09-11 · Environment setup. This does not mean that the existing Godot game has been ported to Unity.

## Start

Double-click 05_Unity.cmd (`../../05_Unity.cmd`; retired file) in the repository root to open `unity/NoReturns`. The project is also registered in Unity Hub. The initial scene is `Assets/_Project/Scenes/Development.unity`, containing only ground, lighting, a camera and a physics reference cube. The current startup scene is CarryLab with movement and carrying implemented. Deliveries and networking are not ported. [Current controls](15-unity-controls.en.md).

## Pinned environment

| Component | Version or setting |
|---|---|
| Editor | 6000.6.0f1 |
| Official Unity CLI | 1.0.0-beta.9 |
| URP | 17.6.0 |
| Input System | 1.19.0 |
| Test Framework | 1.8.0 |
| Unity Pipeline | 0.6.0-exp.1 |
| Development build version | 0.2.0 |
| Target | Windows x64, windowed 1280×720 |

Version sources: ProjectVersion (`../../unity/NoReturns/ProjectSettings/ProjectVersion.txt`; retired file), package manifest (`../../unity/NoReturns/Packages/manifest.json`; retired file). The wrapper uses the user's installed CLI without automatically updating it. `NoReturnsDev` is a temporary company identifier. Unity build versions are separate from Godot versions. The installed Editor is used; no decision on switching to LTS has been made.

## CLI workflow

Run these commands in the repository directory. The wrapper finds the existing CLI without requiring PATH changes.

```powershell
.\05_Unity.cmd open
.\05_Unity.cmd setup
.\05_Unity.cmd check
.\05_Unity.cmd build
.\05_Unity.cmd status
.\05_Unity.cmd commands
```

`open` opens the Editor. `setup` initializes settings and the development scene without overwriting an existing development scene. `check` verifies the scene, URP, product name and build scenes. `build` validates and creates a Windows development build. `status` and `commands` inspect the open Editor's Pipeline connection and available commands. Use `pipeline` to install the connection package.

Before batch operations `setup/check/build`, **save this project's changes and close its Editor**. The wrapper never forcibly closes the Editor. On failure, inspect the corresponding log under `artifacts/unity/`. Retry sandbox license connection denials in the normal user environment. If no license is activated, activation through Hub is required.

Output: `build/unity/Windows/NO_RETURNS.exe`. This executable now runs CarryLab and does not replace the existing game distribution. Implementation: [wrapper](../../tools/unity.ps1), setup, validation and build code (`../../unity/NoReturns/Assets/_Project/Editor/ProjectSetup.cs`; retired file).

## Authoring boundaries and version control

Place new game code under `Assets/_Project/`. URP settings are in `Assets/Settings/`. Move and rename assets through Unity to preserve `.meta` GUIDs. Track `Assets`, `.meta`, the `Packages` manifest/lock and `ProjectSettings` in the existing local Git repository. Exclude Library, Temp, Logs, UserSettings, builds and generated IDE files. No separate repository or remote is created.

`unity/.gdignore` and the Godot export exclusion isolate engine resources. Existing Godot scenes, scripts and assets are preserved. Approved D-direction artwork still needs import, material and rigging validation. The reference cube is not final artwork.

## Validation and next work

Results are recorded in the Unity section of the [validation checklist](05-validation.en.md). Environment checks and gameplay quality validation are separate. Initial movement, camera, pickup and throwing implementation and automated checks are complete; next review actual feel, rapid turns and carrying on ramps. Networking library selection, save compatibility, the full map port and Steam integration remain pending and have not been arbitrarily decided in this setup.

Official references: [Unity CLI](https://docs.unity.com/en-us/unity-cli/use-unity-cli), [Pipeline package](https://docs.unity.com/en-us/unity-production-pipeline/local-tools-cli/unity-pipeline-package). Pin and revalidate versions because these tools are beta/experimental.


## Official skills and MCP · 2026-09-11

At user request, installed 13 skills from the [official Unity skills repository](https://github.com/Unity-Technologies/skills) into the user Codex skills directory. List: `new-unity-project`, `unity-cli`, `unity-package-management`, `build-live-game`, `implement-in-app-purchases`, `levelplay-unity-integration`, `ui`, `ui-uitk`, `ui-ugui`, `ui-imgui`, `validate-urp-render-graph-renderer-feature`, `shader-graph-create-custom-node`, `setup-multiplayer-services`. Installation does not authorize implementing ads, purchases or cloud features. Automatic skill discovery is available from the next turn; relevant instructions were also read directly in this task.

Registered the official Unity CLI `mcp --project-path` server under `mcp_servers.unity` in the user's Codex configuration. Verified that the executable and Korean project path exist and resolve correctly. MCP initialize → tools/list (149 tools) → tools/call (get_scene_hierarchy) round trips succeeded. Evidence: `artifacts/unity/mcp-tools.json`, `mcp-hierarchy.json`. The current conversation's native tool list has not refreshed, so actual MCP requests are sent through the [stdio MCP runner](../../tools/unity_mcp.py). Native tool exposure after restarting Codex still requires a separate check. Distinguish CLI commands from MCP tool calls in reports.

## 2026-09-12

[NO RETURNS Unity](18-unity-mainline.en.md).

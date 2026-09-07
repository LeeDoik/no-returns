# Windows build and Steam handoff

[한국어](07-build-handoff.ko.md)

September 7, 2026 · Prototype 0.6. A local export is distinct from a Steam-uploaded and reviewed release.

## Prepared locally

- `export_presets.cfg` includes gameplay resources (including script preloads), excludes development tests/documents/tools, and exports unsigned Windows x64 executable plus game pack.
- Run `python tools/build_windows.py` from the project root to export and create `build/NO_RETURNS_0.6_Windows.zip`. The archive contains the executable, `.pck`, full Korean/English play guides, official Godot third-party notices and a SHA256 manifest. The allowlist excludes editor caches, test scripts and account data.
- The pinned official Godot 4.7.2 template archive has SHA256 `f298490b8d44d934be425a5a65a51bf15f422428b229a06a6e11d9ffea248011`. Its matching Windows templates were installed under the project's portable editor data. Restore matching templates before rebuilding on another machine. [Official release](https://github.com/godotengine/godot-builds/releases/tag/4.7.2-stable)
- Engine license and third-party notices come from the matching official source release and are retained verbatim. This does not settle the eventual game title or future asset rights. [Godot license](https://github.com/godotengine/godot/blob/4.7.2-stable/LICENSE.txt), [third-party notices](https://github.com/godotengine/godot/blob/4.7.2-stable/COPYRIGHT.txt)

## After obtaining real IDs

Run `python tools/prepare_steampipe.py --app-id YOUR_APP_ID --depot-id YOUR_WINDOWS_DEPOT_ID`, replacing the names with the actual numeric IDs from your Steamworks application. It creates `build/steampipe/app_build.vdf` and `depot_build.vdf` against the local export. Invalid IDs, sample App ID 480, unexpected content and development `steam_appid.txt` files are rejected. The generator cannot establish ownership of a supplied ID.

These configurations have **Preview=1** and no automatic live-branch switch. The generator does not log in, upload, submit or publish. After inspecting the generated mappings and completing the official Steamworks setup, the account owner can use the Steamworks SDK ContentBuilder/SteamCMD flow. Preview validates content without uploading; an actual upload requires deliberately changing the preview configuration and using an authorized account. Keep credentials and Steam Guard codes out of this repository. [SteamPipe uploading](https://partner.steamgames.com/doc/sdk/uploading)

Configure the real Windows depot, access packages and `NO_RETURNS.exe` launch option in Steamworks. Upload a candidate to a private branch, record its build ID, install using an entitled tester, and retest that installed build. Do not submit the current graybox as a finished release.

## Still unavailable

The creator has not registered for Steamworks and no real App ID or depot ID has been supplied. Steam friend invitations and relay transport are not implemented. The current build uses direct-IP ENet. The earlier candidate integration choices remain unvalidated against Godot 4.7.2. Complete an actual two-account, two-machine Steam connection experiment before replacing the current transport or advertising Steam friend-join support.

Account registration, tax/bank approval, payments, Steam transport selection, private Steam installation, separate-network testing, final content, store assets and review remain open. Track them in the [release checklist](05-release-checklist.md). The user authorized continuous local implementation; this record does not mark external account or review gates complete.

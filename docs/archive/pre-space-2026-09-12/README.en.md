# NO RETURNS

[한국어](README.md)

> **Historical record · retired/superseded 2026-09-12.** The [PSX space-delivery design](../../current/01-overview.en.md) is current. Implementation, launch, art and values below are historical, not current status. Deleted files remain path records only.


**Main development: NO RETURNS Unity.** Open the editor with 05_Unity.cmd and launch the game with 06_Unity_Play.cmd. Godot is preserved as source/reference. 현재 개발 상태 / Current status (`docs/current/18-unity-mainline.en.md`; retired file).

A third-person 3D cooperative game about delivering living parcels for 1–4 players. The Unity 0.2.0 carrying lab is the main development baseline. Preserve the existing **Godot 4.7.2 development build 0.9.4**, targeting Windows/Steam. Steam invites and store/build approval are not complete.

## Preserved Godot launch

- 00_게임 실행.cmd (`<00_게임 실행.cmd>`; retired file): run the last package
- 01_맵 편집.cmd (`<01_맵 편집.cmd>`; retired file): edit the map
- 03_물리 실험실.cmd (`<03_물리 실험실.cmd>`; retired file): tune legacy-carrying physics
- PLAY.cmd (`PLAY.cmd`; retired file): run current sources
- BUILD.cmd (`BUILD.cmd`; retired file): rebuild the package
- 02_배포 파일 찾기.cmd (`<02_배포 파일 찾기.cmd>`; retired file): locate the distribution ZIP

Keep the executable and PCK together. `build/NO_RETURNS_0.7` is the retained compatibility path; the actual version is 0.9.4. Cooperative players use the same build/map. Protocol is 12.

## Documentation

Use the **[six-part documentation home](docs/README.en.md)** for current design, rules, production, backlog, validation and history. The former version-by-version README is preserved in the archive (`docs/archive/legacy-readme.en.md`; retired file).

Version control is local Git with no GitHub remote. `art/` holds sources, `assets/` runtime art, `scenes/` and `scripts/` maps/code, `tests/` and `tools/` development tools, and `artifacts/` local validation evidence.

Expedition first playable: launch, controls and scope (`docs/prototype/10-expedition.en.md`; retired file) · EXP-01, 2026-09-10


Unity project and CLI environment (`docs/current/14-unity-environment.en.md`; retired file) · 2026-09-11 · Unity 6000.6.0f1 / CLI 1.0.0-beta.9


Run the Unity carrying lab (`06_Unity_Play.cmd`; retired file) · Unity 0.2.0 · CarryLab (`docs/current/15-unity-controls.en.md`; retired file)


Launch the new delivery loop with 07_NoReturns_Loop.cmd (`07_NoReturns_Loop.cmd`; retired file). NR-LOOP-01 launch and validation (`docs/current/22-slapstick-loop.en.md`; retired file).

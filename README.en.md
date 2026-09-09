# NO RETURNS

[한국어](README.md)

A third-person 3D cooperative game about delivering living parcels for 1–4 players. Current **development build 0.9.4**, Godot 4.7.2, targeting Windows/Steam. Steam invites and store/build approval are not complete.

## Launch

- [00_게임 실행.cmd](<00_게임 실행.cmd>): run the last package
- [01_맵 편집.cmd](<01_맵 편집.cmd>): edit the map
- [03_물리 실험실.cmd](<03_물리 실험실.cmd>): tune legacy-carrying physics
- [PLAY.cmd](PLAY.cmd): run current sources
- [BUILD.cmd](BUILD.cmd): rebuild the package
- [02_배포 파일 찾기.cmd](<02_배포 파일 찾기.cmd>): locate the distribution ZIP

Keep the executable and PCK together. `build/NO_RETURNS_0.7` is the retained compatibility path; the actual version is 0.9.4. Cooperative players use the same build/map. Protocol is 12.

## Documentation

Use the **[six-part documentation home](docs/README.en.md)** for current design, rules, production, backlog, validation and history. The former version-by-version README is preserved in the [archive](docs/archive/legacy-readme.en.md).

Version control is local Git with no GitHub remote. `art/` holds sources, `assets/` runtime art, `scenes/` and `scripts/` maps/code, `tests/` and `tools/` development tools, and `artifacts/` local validation evidence.

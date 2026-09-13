# NO RETURNS — 반품 불가

[English](README.en.md)

> **과거 기록 · 2026-09-12 폐기/대체.** 현재 게임은 [PSX 우주 배송 기획](../../current/01-overview.ko.md)을 따른다. 아래 구현·실행·아트·수치는 과거 자료이며 현재 상태가 아니다. 삭제된 파일은 경로 기록으로만 남긴다.


**메인 개발: NO RETURNS Unity.** 편집기는 05_Unity.cmd, 게임 실행은 06_Unity_Play.cmd. Godot는 원본·참조용으로 보존한다. 현재 개발 상태 / Current status (`docs/current/18-unity-mainline.ko.md`; retired file).

살아 있는 택배를 배송하는 1~4인 3D 협동 게임. Unity 0.2.0 운반 실험실을 메인 개발 기반으로 사용합니다. Godot 4.7.2의 **기존 개발 빌드 0.9.4**는 보존하며 Windows/Steam 출시를 목표로 합니다. Steam 초대·상점/빌드 심사 완료본이 아닙니다.

## 보존된 Godot 실행

- 00_게임 실행.cmd (`<00_게임 실행.cmd>`; retired file): 마지막 배포본 실행
- 01_맵 편집.cmd (`<01_맵 편집.cmd>`; retired file): 맵 편집
- 03_물리 실험실.cmd (`<03_물리 실험실.cmd>`; retired file): 기존 운반 방식의 물리값 시험
- PLAY.cmd (`PLAY.cmd`; retired file): 최신 소스 실행
- BUILD.cmd (`BUILD.cmd`; retired file): 배포본 갱신
- 02_배포 파일 찾기.cmd (`<02_배포 파일 찾기.cmd>`; retired file): 배포 ZIP 위치

실행 파일과 PCK는 함께 보관하세요. `build/NO_RETURNS_0.7`은 기존 호환 경로이며 실제 버전은 0.9.4입니다. 협동 참여자는 같은 빌드·맵을 사용합니다. 프로토콜은 12입니다.

## 문서

**[문서 홈 — 여섯 가지 분류](docs/README.md)**에서 최신 기획·규칙·제작 방법·미완료·검증·과거 기록을 확인하세요. 기존 버전별 README는 과거 기록 (`docs/archive/legacy-readme.ko.md`; retired file)으로 옮겼습니다.

로컬 Git으로 관리하며 GitHub 원격 저장소는 연결하지 않습니다. `art/`는 제작 원본, `assets/`는 실행 아트, `scenes/`·`scripts/`는 맵과 코드, `tests/`·`tools/`는 개발 도구, `artifacts/`는 로컬 검증 기록입니다.

원정 첫 플레이: 실행·조작·범위 (`docs/prototype/10-expedition.ko.md`; retired file) · EXP-01, 2026-09-10


Unity 프로젝트·CLI 개발 환경 (`docs/current/14-unity-environment.ko.md`; retired file) · 2026-09-11 · Unity 6000.6.0f1 / CLI 1.0.0-beta.9


Unity 운반 실험실 실행 (`06_Unity_Play.cmd`; retired file) · Unity 0.2.0 · CarryLab (`docs/current/15-unity-controls.ko.md`; retired file)


새 배송 루프는 07_NoReturns_Loop.cmd (`07_NoReturns_Loop.cmd`; retired file)로 실행합니다. NR-LOOP-01 실행·검증 (`docs/current/22-slapstick-loop.ko.md`; retired file).

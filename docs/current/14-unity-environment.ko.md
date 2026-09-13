# Unity 개발 환경

[English](14-unity-environment.en.md)

> **과거 기록 · 2026-09-12 폐기/대체.** 현재 게임은 [PSX 우주 배송 기획](01-overview.ko.md)을 따른다. 아래 구현·실행·아트·수치는 과거 자료이며 현재 상태가 아니다. 삭제된 파일은 경로 기록으로만 남긴다.


2026-09-11 · 환경 구성. 기존 Godot 게임의 Unity 이식 완료를 의미하지 않는다.

## 시작

저장소 루트의 05_Unity.cmd (`../../05_Unity.cmd`; retired file)를 더블클릭하면 `unity/NoReturns` 프로젝트를 연다. Unity Hub에도 등록한다. 첫 장면은 `Assets/_Project/Scenes/Development.unity`이며 바닥·조명·카메라·물리 참조 큐브만 포함한다. 현재 기본 실행 장면은 CarryLab이며 이동·운반을 구현했다. 배송·온라인은 아직 이식하지 않았다. [현재 조작 명세](15-unity-controls.ko.md).

## 고정한 환경

| 구성 | 버전·설정 |
|---|---|
| Editor | 6000.6.0f1 |
| 공식 Unity CLI | 1.0.0-beta.9 |
| URP | 17.6.0 |
| Input System | 1.19.0 |
| Test Framework | 1.8.0 |
| Unity Pipeline | 0.6.0-exp.1 |
| 개발 빌드 버전 | 0.2.0 |
| 대상 | Windows x64, 창 모드 1280×720 |

버전 근거: ProjectVersion (`../../unity/NoReturns/ProjectSettings/ProjectVersion.txt`; retired file), 패키지 목록 (`../../unity/NoReturns/Packages/manifest.json`; retired file). CLI는 사용자 로컬 설치본을 사용하며 자동 업데이트하지 않는다. `NoReturnsDev`는 임시 회사 식별자다. Unity 빌드 버전은 Godot 버전과 별개다. 설치된 Editor를 사용하며 LTS 전환 결정은 아직 하지 않았다.

## CLI 사용

저장소 폴더에서 다음 명령을 실행한다. PATH 설정 없이 기존 CLI를 자동으로 찾는다.

```powershell
.\05_Unity.cmd open
.\05_Unity.cmd setup
.\05_Unity.cmd check
.\05_Unity.cmd build
.\05_Unity.cmd status
.\05_Unity.cmd commands
```

`open`은 편집기를 연다. `setup`은 설정과 개발 장면을 초기화하며 기존 개발 장면을 덮어쓰지 않는다. `check`는 장면·URP·제품명·빌드 장면을 검사한다. `build`는 검사 후 Windows 개발 빌드를 만든다. `status`와 `commands`는 열린 Editor의 Pipeline 연결과 제공 명령을 확인한다. `pipeline`은 연결 패키지를 설치할 때 사용한다.

배치 작업인 `setup/check/build` 전에는 **이 프로젝트의 변경을 저장하고 Editor를 닫는다**. 실행 도구가 편집기를 강제 종료하지 않는다. 오류가 나면 `artifacts/unity/`의 해당 로그를 확인한다. 샌드박스의 라이선스 연결 거부는 일반 사용자 실행 환경에서 다시 시도한다. 라이선스 자체가 없으면 Hub에서 활성화가 필요하다.

출력: `build/unity/Windows/NO_RETURNS.exe`. 현재 CarryLab 실행 파일이며 기존 게임 배포본을 대체하지 않는다. 구현 근거: [실행 도구](../../tools/unity.ps1), 설정·검사·빌드 코드 (`../../unity/NoReturns/Assets/_Project/Editor/ProjectSetup.cs`; retired file).

## 제작 경계와 버전 관리

새 게임 코드는 `Assets/_Project/`에 둔다. URP 설정은 `Assets/Settings/`에 있다. Unity에서 이동·이름 변경하여 `.meta` GUID를 보존한다. `Assets`, `.meta`, `Packages`의 manifest/lock, `ProjectSettings`를 기존 로컬 Git에서 관리한다. Library·Temp·Logs·UserSettings·빌드·IDE 생성 파일은 제외한다. 별도 저장소나 원격 저장소는 만들지 않는다.

`unity/.gdignore`와 Godot 내보내기 제외 규칙으로 엔진별 리소스를 분리한다. 기존 Godot 씬·스크립트·에셋은 그대로 보존한다. 승인된 D안 아트는 이후 임포트·재질·리깅 검증 대상으로 남긴다. 이번 참조 큐브는 최종 아트가 아니다.

## 검증과 다음 작업

검증 결과는 [출시·검증 목록](05-validation.ko.md)의 Unity 항목에 기록한다. 환경 검사와 게임 플레이 품질 검증은 별개다. 이동·카메라·들기·던지기의 첫 구현과 자동 검사를 마쳤으며, 다음 검토는 실제 조작감·급회전·경사로 운반이다. 네트워크 라이브러리 선택·저장 호환·전체 맵 이식·Steam 통합은 미완료이며 이번 설정에서 임의로 확정하지 않았다.

공식 참고: [Unity CLI](https://docs.unity.com/en-us/unity-cli/use-unity-cli), [Pipeline 패키지](https://docs.unity.com/en-us/unity-production-pipeline/local-tools-cli/unity-pipeline-package). beta/experimental 도구이므로 패키지 버전을 고정하고 변경 시 재검증한다.


## 공식 스킬과 MCP · 2026-09-11

사용자 요청으로 [Unity 공식 스킬 저장소](https://github.com/Unity-Technologies/skills)의 스킬 13개를 사용자 Codex 스킬 폴더에 설치했다. 목록: `new-unity-project`, `unity-cli`, `unity-package-management`, `build-live-game`, `implement-in-app-purchases`, `levelplay-unity-integration`, `ui`, `ui-uitk`, `ui-ugui`, `ui-imgui`, `validate-urp-render-graph-renderer-feature`, `shader-graph-create-custom-node`, `setup-multiplayer-services`. 설치가 광고·결제·클라우드 기능의 구현 승인을 의미하지 않는다. 새 턴부터 자동 스킬 검색 대상이며 이번 작업에서도 관련 지침을 직접 읽었다.

Codex 사용자 설정의 `mcp_servers.unity`에 공식 Unity CLI의 `mcp --project-path` 서버를 등록했다. 실행 파일·한글 프로젝트 경로의 실제 존재와 일치를 확인했다. MCP initialize → tools/list(149개) → tools/call(get_scene_hierarchy) 왕복에 성공했다. 근거: `artifacts/unity/mcp-tools.json`, `mcp-hierarchy.json`. 현재 대화의 기본 도구 목록에는 새 서버가 아직 반영되지 않아 [stdio MCP 실행 도구](../../tools/unity_mcp.py)를 통해 실제 MCP 요청을 사용한다. Codex 재시작 후 기본 도구로 노출되는지는 별도 확인이 필요하다. CLI 명령 호출과 MCP 도구 호출을 구분해 기록한다.

## 2026-09-12

[NO RETURNS Unity](18-unity-mainline.ko.md).

# macOS 개발 환경

[English](macos-development.en.md)

2026-09-26. 기존 Windows 프로젝트를 Apple Silicon Mac에서 이어서 개발한다. 사용자 지정 대상은 macOS와 Windows다. 게임 기능과 에디터·패키지 버전은 이 환경 구성에서 변경하지 않는다.

## 구성

| 구성 요소 | 기준 및 역할 |
|---|---|
| Unity Editor | **6000.6.0f1**, Apple Silicon. [프로젝트 버전](../../NoReturns/ProjectSettings/ProjectVersion.txt)과 일치시킨다. |
| 공식 Unity CLI | **1.0.0-beta.11**. 에디터 설치·프로젝트 실행·명령 호출. |
| Unity Pipeline | 기존 **0.7.0-exp.1** 유지. 열린 에디터와 CLI/MCP를 연결한다. [패키지 목록](../../NoReturns/Packages/manifest.json). |
| Git LFS | **3.8.0**. 모델·텍스처 등 대용량 원본을 복원한다. |
| 빌드 지원 | 에디터의 macOS Mono 지원과 추가 Windows Build Support (Mono). IL2CPP는 별도 구성이다. |
| 기존 도구 | Unity Hub, Git, Xcode **27.0**과 clang 확인. |

Unity가 C# 컴파일 도구를 제공하므로 별도 .NET SDK나 VS Code는 이번 CLI 작업의 필수 설치 항목이 아니다. IDE 디버깅이나 Blender 원본 편집이 필요하면 해당 도구를 별도로 구성한다. 현재 Unity `Assets/`에는 직접 가져오는 `.blend`나 네이티브 플러그인 바이너리가 없다.

## 시작과 연결 확인

저장소 루트에서 실행한다. `NoReturns/`가 실제 Unity 프로젝트다. 첫 실행은 패키지 다운로드와 에셋 가져오기에 시간이 걸린다.

```sh
source "$HOME/.unity/env"
unity --version
unity open ./NoReturns
unity status --project-path "$PWD/NoReturns" --format json
unity command editor_status --caller plugin --skill unity-cli --project-path "$PWD/NoReturns" --format json
unity command --caller plugin --skill unity-cli --project-path "$PWD/NoReturns"
```

Codex에 자연어로 요청하면 C# 수정 후 컴파일과 실제 동작을 확인하고, 씬·프리팹·머티리얼은 연결된 에디터 명령으로 다룬다. 명령 목록을 먼저 조회한다. 연결 실패 시 `unity pipeline list --format json`으로 Safe Mode와 패키지 상태를 확인한다. CLI 연결은 게임 멀티플레이와 별개다.

이 맥의 Codex MCP 설정은 저장소 루트 `.codex/config.toml`에 있으며 `.git/info/exclude`로 제외한다. 절대 경로를 사용하는 개인 설정이므로 커밋하지 않는다. 현재 세션에서 새 MCP 도구가 보이지 않아도 CLI를 직접 사용할 수 있다. 새 세션의 MCP 자동 로딩은 별도 확인이 필요하다.

## 소스 복원과 빌드

다른 컴퓨터에서도 `git lfs install --local`, `git lfs pull`, `git lfs fsck` 순으로 에셋을 복원·검사한 뒤 에디터를 연다. `.meta`는 에셋과 함께 관리하고 캐시·로그·빌드·인증정보는 커밋하지 않는다. Windows용 `.cmd`, `tools/unity.ps1`, `tools/unity_mcp.py`에는 Windows 경로가 있으므로 맥에서는 직접 CLI 명령을 사용한다.

다음은 CLI 사용 예이며 성공한 게임 빌드의 기록이 아니다. 현재 [기본 빌드 장면 목록](../../NoReturns/ProjectSettings/EditorBuildSettings.asset)은 `Bootstrap`만 활성화한다. 실제 게임 실행본을 만들려면 먼저 에디터에서 `CarryRoom` 등 의도한 장면을 포함하는 빌드 설정을 만든다. 기존 `CarryBuild`, `ShipInteriorTrialBuild`, `CinderBlockoutBuild`는 Windows 대상으로 고정돼 있으므로 그대로 macOS 빌드에 사용하지 않는다. 변경을 저장하고 이 프로젝트의 에디터를 닫은 뒤 실행한다.

```sh
unity build ./NoReturns --target StandaloneOSX --output-path "$PWD/builds/macOS/NO_RETURNS.app"
unity build ./NoReturns --target StandaloneWindows64 --output-path "$PWD/builds/Windows/NO_RETURNS.exe"
```

macOS의 실제 플레이·배포 서명·공증과 Windows 실행본의 Windows 기기 검증은 별도다. 환경 설정이 두 플랫폼의 출시 검증 완료를 의미하지 않는다.

## 이번 확인 결과

- [x] 저장소 복제, CLI 버전 및 새 로그인 셸의 PATH 확인.
- [x] LFS 파일 **296개**, **852,525,080바이트** 복원, 남은 포인터 0개 및 `git lfs fsck` 통과.
- [x] Unity 계정 로그인, Hub 프로젝트 등록, 프로젝트 전용 Codex MCP 설정.
- [x] Unity Personal 라이선스 활성 상태 확인. 첫 실행 에디터 약관은 사용자가 직접 동의했다.
- [x] 에디터와 Windows Mono 모듈 설치 및 `unity editors verify` 통과. 실행 파일의 `arm64`와 macOS 빌드 지원 디렉터리 확인.
- [x] 프로젝트 컴파일과 열린 에디터의 CLI 연결 확인. `ready`, 컴파일 실패 없음, 콘솔 오류 0개, 기존 폐기 예정 API 경고 7개.
- [x] CLI 명령 151개 조회, C# `eval`, `CarryRoom` 씬 열기·계층 조회, MCP 초기화·도구 목록·`editor_status` 실제 호출 통과.
- [ ] Codex 새 세션의 MCP 자동 로딩 확인.
- [ ] macOS·Windows 빌드 및 실제 게임 동작 확인.

개발 환경 설치와 실제 연결 검증을 완료했다. `CarryRoom` 씬은 열린 상태이며 변경되지 않았다. 경고 7개는 `FacilityArt`, `CarryRoom`, `ShipInteriorTrialBuild`의 기존 객체 검색 API 사용에서 발생한다. 플레이 모드·게임 빌드·사람 조작감 검증은 수행하지 않았다.

첫 가져오기 때 Unity가 [URP 전역 설정](../../NoReturns/Assets/Settings/UniversalRenderPipelineGlobalSettings.asset)의 생성된 런타임 목록만 비웠다. 설치된 URP 17.6.0의 `RenderPipelineGraphicsSettingsContainer`는 에디터에서 이 목록을 비우고 Player 빌드 중 다시 생성한다. 제작용 설정 목록과 게임 코드·씬은 변경하지 않았다. 이 자동 정규화를 기록에 포함한다.

복제 직후 Windows 실행 파일 2개의 줄바꿈이 Git 변경으로 표시됐지만 원시 바이트는 HEAD와 동일했다. 기존 `.gitattributes`에 맞춰 Git 저장 내용을 정규화했고 명령 내용이 동일함을 검사했다. `tools/check_docs.py`는 Git에서 제외된 과거 `artifacts/` 결과 링크 **142개**가 없어 실패했다. 새 문서로 추가된 실패는 없으며 과거 검증 결과를 새로 만들어 채우지 않는다.

이 CLI 버전에서 `install --resume`은 `--architecture arm64` 지정에도 기존 다운로드를 Intel 항목으로 잘못 선택했다. 해당 실행을 중단하고 공식 Apple Silicon 파일의 크기·체크섬을 검증한 뒤 `--resume` 없이 설치를 완료했다. 에디터를 재설치할 때도 실제 아키텍처를 확인한다.

공식 참고: [Unity CLI](https://docs.unity.com/en-us/unity-cli/use-unity-cli), [Unity Pipeline](https://docs.unity.com/en-us/unity-cli/unity-pipeline/unity-pipeline-package).

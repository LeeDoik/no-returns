# macOS 개발 환경

[English](macos-development.en.md)

## 2026-10-03 — 진압봉 준비·공격·복귀 0.9.9

0.9.9 작업에서 공식 최신 Editor 6000.6.4f1·CLI 1.0.0-beta.12·Pipeline 0.8.0-exp.1과 Blender 5.2.2 LTS·MCP 2.1.3을 다시 확인했다. Blender MCP 직접 조회와 Unity 정적 표본/맥 빌드/화면 없는 플레이 검사를 사용한다. [구현·제작·검증 범위](employee-baton.ko.md).

## 2026-10-03 — 현재 대화의 Blender MCP 연결 확인

현재 대화에 등록된 `get_addon_status`와 `get_scene_info`를 직접 호출했다. 처음에는 Blender가 종료되어 연결에 실패했으며 저장된 직원 리깅 파일을 Blender에서 연 뒤 두 호출 모두 성공했다. Blender 5.2.2 LTS, 애드온 1.8·프로토콜 13 일치(`up_to_date: true`), 텔레메트리 꺼짐과 직원 메시/리그를 포함한 객체 5개를 확인했다. 코드·모델 변경이나 파일 저장은 하지 않았다. 수동 게임 창도 열지 않았다. 이번 확인은 실제 MCP 조회 연결 범위이며 편집·렌더링·게임 동작 재검증은 포함하지 않는다.

## 2026-10-03 — 동료 연습 빌드 0.9.7

[현재 화면 없는 검사·실행 정책](companion-play.ko.md). Pipeline 업데이트 전 파일 경로가 남아 첫 컴파일/빌드가 실패했으나 Editor 재시작 후 해결했다. 현재 맥 빌드 오류 0개·경고 9개이며 앞 작업의 Burst 진입점 경고 5개는 이번 빌드에서 나타나지 않았다. 성능 측정은 하지 않았다.

## 2026-10-03 — 최신 도구와 Blender MCP

작업에 사용하는 도구는 [프로젝트 규칙](../../AGENTS.md)에 따라 공식 배포처의 최신 버전을 확인하고 갱신한다. 게임 0.9.6의 실제 버전 근거는 [Unity 버전](../../NoReturns/ProjectSettings/ProjectVersion.txt)과 [패키지 목록](../../NoReturns/Packages/manifest.json)이다. 이 표가 아래 과거 환경 기록보다 우선한다.

| 도구 | 이번 설치·확인 |
|---|---|
| Unity Editor | 6000.6.0f1 → **6000.6.4f1**, 공식 `unity releases`의 최신 정식판. 프로젝트 업그레이드·Mac 빌드/플레이 검사. |
| Unity CLI / Pipeline | **1.0.0-beta.12 / 0.8.0-exp.1**. `unity self-update`, `unity pipeline upgrade` 사용. 살아 있는 Editor 서버 127.0.0.1:7800 응답 확인. |
| Blender | **5.2.2 LTS**, 설치본과 [공식 배포 연결 정보](https://formulae.brew.sh/api/cask/blender.json)가 일치. |
| Blender MCP | [mcp-for-blender](https://github.com/ahujasid/mcp-for-blender) **2.1.3** 설치. [PyPI 배포 정보](https://pypi.org/pypi/mcp-for-blender/json) 확인. 애드온 자체 표시는 1.8·프로토콜 13이며 패키지 버전과 다르다. |
| uv | **0.12.22**, [Homebrew 배포 정보](https://formulae.brew.sh/api/formula/uv.json) 확인·업데이트. |
| Codex CLI | **0.160.0**, [공식 설치기](https://chatgpt.com/codex/install.sh)로 갱신·실행 버전 확인. |
| Codex 데스크톱 | 설치 **26.930.21537 (12776)**, 앱 업데이트 검사에서 **26.930.31730 (12947)** 제공 확인. 이번 작업 중 앱 업데이트 도구는 조회 전용이고 Codex 자체 UI 자동 조작이 차단되어 있어 사용자의 앱 업데이트/재시작이 남는다. 최신 설치 완료로 간주하지 않는다. |

이전 작업은 Blender Python 배치로 진행했다. 이번에는 애드온을 설치하고 현재 열린 Blender에서 활성화한 뒤 실제 MCP stdio 클라이언트로 `get_scene_info`와 읽기 전용 `execute_blender_code`를 호출했다. 현재 직원 씬의 객체 5개·뼈 53개·오른손 정보를 읽었고 열려 있던 미저장 작업은 저장하거나 다시 열지 않았다. 연결 포트는 로컬 127.0.0.1:9876이다. 설치 당시 대화에는 새 MCP 도구가 보이지 않았지만, 위 후속 확인에서 현재 대화의 직접 호출까지 성공했다. 소켓 응답만으로 MCP 도구 등록 완료를 주장하지 않는다.

다른 환경의 재현 명령은 아래와 같다. `@latest`는 연결 시작 시 최신 패키지를 사용하며, 실제 선택된 버전은 작업 기록에 남긴다. 애드온 업데이트 뒤에는 기존 미저장 작업을 보존하고 Blender를 다시 시작하거나 애드온을 재로딩한 뒤 양쪽 버전을 확인한다. 개인 Codex 설정은 저장소에 넣지 않는다.

```sh
uvx mcp-for-blender@latest install-addon
codex mcp add blender --env DISABLE_TELEMETRY=true -- uvx mcp-for-blender@latest
```

Blender `Preferences > Add-ons`에서 Blender MCP를 활성화한다. 현재 맥에서는 `/opt/homebrew/bin/uvx` 절대 경로를 등록했고 텔레메트리는 끈다. 연결된 씬/선택/미저장 상태를 읽은 뒤 작업하며 원본을 덮어쓰지 않는다. 단순 재현·일괄 검사는 기존 Blender Python 스크립트도 계속 사용한다.

새 Unity의 Windows 지원 모듈·Windows 실행 및 Codex 데스크톱 업데이트 완료 여부는 미확인이다. 기본 MCP 도구 노출·직접 연결은 위 후속 확인에서 검증했다. 이전 Unity 설치와 미저장 Blender 파일은 보존한다. [게임 적용·검증 범위](employee-animation.ko.md)와 [검증 기록](../validation/baton-hand-0.9.6.json)을 따른다.

## 2026-10-01 — Cinder 배송·영수증·귀환 정산

[CINDER-DELIVERY-01 실행·좌표·검증](four-player.ko.md#2026-10-01--cinder-배송영수증귀환-정산). 별도 `CinderFourPlayerTest`에서 우주선 E 준비/도착 → BAY 04 바닥 배송 → CRT 영수증 E 회수 → 전원 탑승·E 귀환·420 CR 정산 → 다음 근무를 연결했다. 버전 0.9.1·프로토콜 10·TCP 27842 유지. 기존 배송 장부·화물·CRT·KO/EN 화면과 표지 도구를 재사용한다. 원본 환경 씬은 그대로이며 아래 이전 단계의 배송/수령 미연결 표기는 이 시험 범위에서 대체된다. 기본 실행은 배송, `start --map-only`와 `check`는 이전 이동 시험이다. 리스너/진압봉/억제/신호기/저장, 사람 네 명·다른 PC·성능은 미완료다. 실제 자동 검사 근거는 [검증 기록](../validation/cinder-delivery-01.json)을 따른다.

## 2026-10-01 — 현재 Cinder 맥 4인 시험

현재 환경/아트 원본은 `CinderCompactSiteReview`, 파생 4인 시험은 `CinderFourPlayerTest`다. [4인 실행·빌드·검증 안내](four-player.ko.md#2026-10-01--cinder-4인-맵-테스트-환경)가 아래 2026-09-26의 대상/빌드 미확인 기록보다 우선한다. `CinderFourPlayerBuild`로 원본을 보존하며 `builds/CinderFourPlayer/NoReturns.app`을 만든다. **맥 빌드 오류 0개·기존 경고 7개, 실제 네 프로세스 자동 검사 13개·기존 구조 규칙 7개·일반 모드 기본값 7개 통과. 800×500 한글 HUD 화면 2개에서 직원 4/4·E/Q 안내·조준점·팀 색을 확인했고 수동 네 프로세스/접속 3개·종료도 확인했다.** 출시에 필요한 서명/공증·Windows 실행·다른 PC·성능·전체 게임 기능은 별도 미확인이다.

2026-09-26. 기존 Windows 프로젝트를 Apple Silicon Mac에서 이어서 개발한다. 사용자 지정 대상은 macOS와 Windows다. 게임 기능과 에디터·패키지 버전은 이 환경 구성에서 변경하지 않는다.

현재 이어서 개발할 대상은 [CINDER-BLOCKOUT-01](cinder-blockout.ko.md)의 `CinderDepotBlockout` 씬이다. 사용자가 지정한 `Play_Cinder_Blockout.cmd`와 2026-09-17 커밋 `6f4922a`가 이 장면의 Windows 실행본을 가리킨다. 초기 환경 검사에 사용한 `CarryRoom`은 이전 운반 실험이며 최신 맵 검증을 대신하지 않는다. `builds/`는 Git 제외 대상이어서 기존 Windows 실행본은 이 맥에 복원되지 않았다.

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
unity command open_scene --path Assets/_NoReturns/Scenes/CinderDepotBlockout.unity --caller plugin --skill unity-cli --project-path "$PWD/NoReturns" --format json
unity command --caller plugin --skill unity-cli --project-path "$PWD/NoReturns"
```

Codex에 자연어로 요청하면 C# 수정 후 컴파일과 실제 동작을 확인하고, 씬·프리팹·머티리얼은 연결된 에디터 명령으로 다룬다. 명령 목록을 먼저 조회한다. 연결 실패 시 `unity pipeline list --format json`으로 Safe Mode와 패키지 상태를 확인한다. CLI 연결은 게임 멀티플레이와 별개다.

이 맥의 Codex MCP 설정은 저장소 루트 `.codex/config.toml`에 있으며 `.git/info/exclude`로 제외한다. 절대 경로를 사용하는 개인 설정이므로 커밋하지 않는다. 현재 세션에서 새 MCP 도구가 보이지 않아도 CLI를 직접 사용할 수 있다. 새 세션의 MCP 자동 로딩은 별도 확인이 필요하다.

## 소스 복원과 빌드

다른 컴퓨터에서도 `git lfs install --local`, `git lfs pull`, `git lfs fsck` 순으로 에셋을 복원·검사한 뒤 에디터를 연다. `.meta`는 에셋과 함께 관리하고 캐시·로그·빌드·인증정보는 커밋하지 않는다. Windows용 `.cmd`, `tools/unity.ps1`, `tools/unity_mcp.py`에는 Windows 경로가 있으므로 맥에서는 직접 CLI 명령을 사용한다.

다음은 CLI 사용 예이며 성공한 게임 빌드의 기록이 아니다. 현재 [기본 빌드 장면 목록](../../NoReturns/ProjectSettings/EditorBuildSettings.asset)은 `Bootstrap`만 활성화한다. 실제 게임 실행본을 만들려면 먼저 에디터에서 `CinderDepotBlockout` 장면을 포함하는 빌드 설정을 만든다. 기존 `CarryBuild`, `ShipInteriorTrialBuild`, `CinderBlockoutBuild`는 Windows 대상으로 고정돼 있으므로 그대로 macOS 빌드에 사용하지 않는다. Cinder의 기존 Build 메뉴는 장면을 재생성하므로 직접 수정한 씬을 보존하려면 사용하지 않는다. 변경을 저장하고 이 프로젝트의 에디터를 닫은 뒤 실행한다.

```sh
unity build ./NoReturns --target StandaloneOSX --output-path "$PWD/builds/macOS/NO_RETURNS.app"
unity build ./NoReturns --target StandaloneWindows64 --output-path "$PWD/builds/Windows/NO_RETURNS.exe"
```

macOS의 실제 플레이·배포 서명·공증과 Windows 실행본의 Windows 기기 검증은 별도다. 환경 설정이 두 플랫폼의 출시 검증 완료를 의미하지 않는다.

## 한국어 글꼴과 재컴파일

[공용 언어 코드](../../NoReturns/Assets/_NoReturns/Runtime/CarryLab/CarryLanguage.cs)는 시스템 글꼴 대신 포함된 `NotoSansKR-Regular`를 불러온다. 이 맥의 Unity 6000.6.0f1에서는 `CreateDynamicFontFromOSFont`로 만든 글꼴이 IMGUI의 TextCore 변환에서 실패했다. Windows 맑은 고딕 의존성을 제거하고 메뉴와 HUD가 같은 글꼴을 사용하게 했다. 약 4.6 MB의 [원본 OTF](https://github.com/notofonts/noto-cjk/blob/main/Sans/SubsetOTF/KR/NotoSansKR-Regular.otf)는 변경하지 않았으며, [OFL 원문](../../NoReturns/Assets/_NoReturns/Resources/Fonts/NotoSansKR-LICENSE.txt)과 Unity가 생성한 `.meta`를 함께 보관한다. OTF는 Git LFS 대상이며 시스템 전체에 설치하지 않는다.

[CarryRoom](../../NoReturns/Assets/_NoReturns/Runtime/CarryLab/CarryRoom.cs)의 테스트 폴더와 수신 상태는 실행 중에만 사용하는 값이다. 에디터 코드 재로딩이 `null`을 빈 문자열·빈 상태로 복원하지 않도록 직렬화에서 제외했다. 재컴파일 뒤 발생하던 빈 경로 예외와 빈 회전 배열 접근을 막는다.

글꼴 검사에는 별도 프레임워크가 필요 없다. 열린 에디터의 Play Mode를 중지하고 저장소 루트에서 실행한다.

```sh
unity command eval_file --file "$PWD/tools/unity_checks/FontCheck.cs" --caller plugin --skill unity-cli --project-path "$PWD/NoReturns" --format json
```

[검사 코드](../../tools/unity_checks/FontCheck.cs)는 동적 글꼴 로딩, TextCore 글꼴 데이터 로딩, 한글·영문·숫자 문자 지원을 확인한다. 성공 결과는 `FONT PASS: NotoSansKR-Regular`다.

## 이번 확인 결과

- [x] 저장소 복제, CLI 버전 및 새 로그인 셸의 PATH 확인.
- [x] LFS 파일 **296개**, **852,525,080바이트** 복원, 남은 포인터 0개 및 `git lfs fsck` 통과.
- [x] Unity 계정 로그인, Hub 프로젝트 등록, 프로젝트 전용 Codex MCP 설정.
- [x] Unity Personal 라이선스 활성 상태 확인. 첫 실행 에디터 약관은 사용자가 직접 동의했다.
- [x] 에디터와 Windows Mono 모듈 설치 및 `unity editors verify` 통과. 실행 파일의 `arm64`와 macOS 빌드 지원 디렉터리 확인.
- [x] 프로젝트 컴파일과 열린 에디터의 CLI 연결 확인. `ready`, 컴파일 실패 없음, 콘솔 오류 0개, 기존 폐기 예정 API 경고 7개.
- [x] CLI 명령 151개 조회, C# `eval`, `CarryRoom` 씬 열기·계층 조회, MCP 초기화·도구 목록·`editor_status` 실제 호출 통과.
- [x] 포함 글꼴 검사, 재컴파일 후 `CarryRoom` Play Mode 시작·중지와 한국어 메뉴 표시 확인. 기존 로그를 보관하고 콘솔을 비운 새 실행 구간에서 오류 0개·경고 0개.
- [x] 최신 작업 대상을 `CinderDepotBlockout`으로 정정하고 기존 씬을 변경 없이 열어 루트 계층 확인. 이 씬의 Mac Play·빌드는 아직 확인하지 않았다.
- [ ] Codex 새 세션의 MCP 자동 로딩 확인.
- [ ] macOS·Windows 빌드 및 실제 게임 동작 확인.

개발 환경 설치와 실제 연결 검증을 완료했다. 현재 `CinderDepotBlockout` 씬을 변경 없이 열어 두었다. 컴파일 경고 7개는 `FacilityArt`, `CarryRoom`, `ShipInteriorTrialBuild`의 기존 객체 검색 API 사용에서 발생하며 수정하지 않았다. 메뉴 시작 확인을 넘는 실제 게임 진행·게임 빌드·사람 조작감 검증은 수행하지 않았다. 작은 Game 뷰에서 안내 문구와 종료 버튼이 겹치는 레이아웃 문제는 남아 있다.

첫 가져오기 때 Unity가 [URP 전역 설정](../../NoReturns/Assets/Settings/UniversalRenderPipelineGlobalSettings.asset)의 생성된 런타임 목록만 비웠다. 설치된 URP 17.6.0의 `RenderPipelineGraphicsSettingsContainer`는 에디터에서 이 목록을 비우고 Player 빌드 중 다시 생성한다. 제작용 설정 목록과 씬은 변경하지 않았다. 이 자동 정규화를 기록에 포함한다.

복제 직후 Windows 실행 파일 2개의 줄바꿈이 Git 변경으로 표시됐지만 원시 바이트는 HEAD와 동일했다. 기존 `.gitattributes`에 맞춰 Git 저장 내용을 정규화했고 명령 내용이 동일함을 검사했다. `tools/check_docs.py`는 Git에서 제외된 과거 `artifacts/` 결과 링크 **142개**가 없어 실패했다. 새 문서로 추가된 실패는 없으며 과거 검증 결과를 새로 만들어 채우지 않는다.

이 CLI 버전에서 `install --resume`은 `--architecture arm64` 지정에도 기존 다운로드를 Intel 항목으로 잘못 선택했다. 해당 실행을 중단하고 공식 Apple Silicon 파일의 크기·체크섬을 검증한 뒤 `--resume` 없이 설치를 완료했다. 에디터를 재설치할 때도 실제 아키텍처를 확인한다.

공식 참고: [Unity CLI](https://docs.unity.com/en-us/unity-cli/use-unity-cli), [Unity Pipeline](https://docs.unity.com/en-us/unity-cli/unity-pipeline/unity-pipeline-package).

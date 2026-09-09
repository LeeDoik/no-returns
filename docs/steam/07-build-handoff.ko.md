# Windows 빌드와 Steam 인계

[English](07-build-handoff.en.md)

> 문서 체계 개정: 이 파일의 버전·수치는 작성 당시 기준입니다. 현재 규칙·미완료 상태는 [문서 홈](../README.md)을 먼저 확인하세요.

2026년 9월 7일 · 프로토타입 0.7. 로컬 내보내기와 Steam 업로드·심사를 완료한 출시는 서로 다릅니다.

## 로컬에서 준비한 것

- `export_presets.cfg`가 스크립트 사전 참조를 포함한 게임 리소스를 포함하고 개발 테스트·문서·도구를 제외하여 서명 없는 Windows x64 실행 파일과 게임 팩을 내보냅니다.
- 프로젝트 루트에서 `python tools/build_windows.py`를 실행하면 내보내기 후 `build/NO_RETURNS_0.7_Windows.zip`을 만듭니다. 실행 파일·`.pck`·전체 한국어/영어 플레이 안내·공식 Godot 제3자 고지·SHA256 목록이 들어갑니다. 포함 허용 목록으로 에디터 캐시·테스트 스크립트·계정 데이터를 제외합니다.
- 고정한 공식 Godot 4.7.2 템플릿 압축파일의 SHA256은 `f298490b8d44d934be425a5a65a51bf15f422428b229a06a6e11d9ffea248011`입니다. 대응 Windows 템플릿을 프로젝트의 포터블 에디터 데이터 아래 설치했습니다. 다른 PC에서 다시 빌드하려면 같은 템플릿을 복원해야 합니다. [공식 릴리스](https://github.com/godotengine/godot-builds/releases/tag/4.7.2-stable)
- 엔진 라이선스와 제3자 고지는 같은 공식 소스 릴리스에서 가져와 원문으로 유지합니다. 이것으로 최종 게임 이름이나 앞으로 사용할 에셋 권리까지 정리되는 것은 아닙니다. [Godot 라이선스](https://github.com/godotengine/godot/blob/4.7.2-stable/LICENSE.txt), [제3자 고지](https://github.com/godotengine/godot/blob/4.7.2-stable/COPYRIGHT.txt)

## 실제 ID를 받은 뒤

`python tools/prepare_steampipe.py --app-id YOUR_APP_ID --depot-id YOUR_WINDOWS_DEPOT_ID`에서 이름 부분을 Steamworks 앱의 실제 숫자 ID로 바꿔 실행합니다. 로컬 내보내기 폴더를 대상으로 `build/steampipe/app_build.vdf`, `depot_build.vdf`가 생성됩니다. 잘못된 ID·샘플 App ID 480·예상 밖 파일·개발용 `steam_appid.txt`는 거부합니다. 생성 도구가 입력한 ID의 소유권까지 확인할 수는 없습니다.

설정은 **Preview=1**이며 자동 공개 브랜치 전환을 포함하지 않습니다. 생성 도구는 로그인·업로드·제출·공개를 하지 않습니다. 생성된 파일 매핑을 검토하고 공식 Steamworks 설정을 마친 뒤 계정 소유자가 Steamworks SDK의 ContentBuilder/SteamCMD 절차를 사용할 수 있습니다. 미리보기는 업로드 없이 콘텐츠를 검사하며 실제 업로드에는 미리보기 설정을 의도적으로 변경하고 권한 있는 계정을 사용해야 합니다. 자격증명과 Steam Guard 코드는 저장소에 넣지 마세요. [SteamPipe 업로드](https://partner.steamgames.com/doc/sdk/uploading)

Steamworks에서 실제 Windows 디포·접근 패키지·`NO_RETURNS.exe` 실행 옵션을 설정합니다. 후보를 비공개 브랜치에 올리고 빌드 ID를 기록한 뒤 접근 권한이 있는 테스터가 설치하여 해당 설치 빌드를 재검증합니다. 현재 기본 도형 프로토타입을 완제품으로 제출하지 마세요.

## 아직 준비할 수 없는 부분

사용자는 Steamworks 미등록 상태이며 실제 App ID·디포 ID가 제공되지 않았습니다. Steam 친구 초대와 중계 통신은 구현하지 않았습니다. 현재 빌드는 직접 주소 방식 ENet을 사용합니다. 기존에 제시한 연동 후보는 Godot 4.7.2 호환성이 검증되지 않았습니다. 통신을 교체하거나 Steam 친구 참가를 홍보하기 전에 실제 계정 두 개·PC 두 대로 연결 실험을 완료해야 합니다.

계정 등록·세금/은행 승인·결제·Steam 통신 선택·Steam 비공개 설치·별도 네트워크 검사·최종 콘텐츠·상점 자료·심사는 남아 있습니다. [출시 체크리스트](05-release-checklist.ko.md)에서 관리합니다. 사용자가 로컬 연속 구현을 승인한 것이며 이 기록은 외부 계정·심사 항목을 완료로 표시하지 않습니다.

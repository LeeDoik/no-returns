# FLATBED 내부 구조 시험 02

[English](ship-interior-trial.en.md)

2026-09-14 · 중앙 창문·화면 수정, Unity 임포트·자동 출입 검사·Windows 빌드 완료. 최종 아트가 아닌 별도 공간 시험이다. 기존 Angular/Integrated 모델, 시험 01 원본과 본 게임 장면·빌드는 보존했다.

## 배치와 결과물

창문의 오른쪽 쏠림을 해결하기 위해 새 후보의 창문 주변 외장을 국소적으로 다시 만들었다. 중앙 유리와 화면을 X=0에 맞추고 좌우 창·기둥을 대칭 배치했다. 콘솔 받침 상단 1.74m, 화면 하단 1.885m로 0.145m 여유를 확보해 화면 가림을 없앴다. 중앙과 좌우 눈높이 시선에서 불투명 외장 장애물은 없었다. 새 후보의 외장 지문은 변경되며 이전 원본은 그대로 보존했다.

- [Blender](../../art/ship-flatbed-01/interior-blockout-02/Flatbed_InteriorTrial.blend) · [FBX](../../art/ship-flatbed-01/interior-blockout-02/Flatbed_InteriorTrial.fbx) · [GLB](../../art/ship-flatbed-01/interior-blockout-02/Flatbed_InteriorTrial.glb)
- [전방](../../art/ship-flatbed-01/interior-blockout-02/forward.png) · [후방](../../art/ship-flatbed-01/interior-blockout-02/rear.png) · [창문](../../art/ship-flatbed-01/interior-blockout-02/window.png)
- [제작 코드](../../art/ship-flatbed-01/interior-blockout.py) · [구조 검사](../../art/ship-flatbed-01/interior-blockout-02/validation.json) · [재가져오기 검사](../../art/ship-flatbed-01/interior-blockout-02/roundtrip.json)
- [Unity 장면](../../NoReturns/Assets/_NoReturns/Scenes/ShipInteriorTrial.unity) · [생성·검사·빌드 코드](../../NoReturns/Assets/_NoReturns/Editor/ShipInteriorTrialBuild.cs) · [시험 조작](../../NoReturns/Assets/_NoReturns/Runtime/ShipInteriorTrial.cs)

로컬 실행 파일은 `builds/ShipInteriorTrial/NoReturns-InteriorTrial.exe`다. 같은 폴더의 데이터와 함께 사용한다. 빌드·로그는 Git에서 제외한다. WASD·마우스, E 들기, Q 놓기, F1 한영 전환, Esc 커서 해제다. 본 게임의 운반 코드나 빌드 설정을 대체하지 않는다.

## 치수·출입 검증

바닥 1.00m, 천장 아래 3.12m, 객실 높이 2.12m, 시점 높이 2.57m다. [현재 운반 코드](../../NoReturns/Assets/_NoReturns/Runtime/CarryLab/CarryRoom.cs)의 직원 높이 1.8m·반지름 0.34m·눈높이 1.57m, 화물 0.8×0.65×0.65m를 기준으로 삼았다. 후방 문틀 여유 높이는 약 1.86m, 외부 발판은 수평 3m·상승 1m다. 출시 확정값이 아닌 시험값이다.

FBX 전방 축 차이를 Unity Y축 180도 회전으로 보정했다. 생성 외장 삼각형을 그대로 보행 충돌체로 쓰면 입구에서 걸렸다. 시각 외장과 충돌체를 분리해 객실·문틀에는 상자, 발판에는 전용 경사 상자를 사용했다. 본 게임 물리 전면 교체가 아니다.

실제 CharacterController.Move 검사에서 X=-0.55, 0, 0.55m 통과선이 모두 Z=2.259m에 도착했다. 직원 표본과 화물을 감싸는 보수적인 구 표본도 충돌 없음이다. GLB 재가져오기 삼각형 13,925개 일치, UV 누락 0, 비정상 좌표 0이다. 이 검사는 사람이 화물을 회전하며 걷는 조작감이나 모든 위치의 통과를 보장하지 않는다.

## Unity 복구와 실행

샌드박스 시도는 종료 코드 198, `No valid Unity Editor license found`로 실패했다. 정상 사용자 환경의 읽기 전용 조회에서 Unity Personal Assigned를 확인한 뒤 에디터를 한 번 열고 기존 인스턴스에 연결했다. 전역 샌드박스 해제·라이선스 파일 삭제·라이선스 프로세스 강제 종료는 하지 않았다. Hub IPC 경고는 남아 있으므로 모든 경고가 사라졌다는 뜻은 아니다.

등록된 MCP는 공식 `unity mcp --project-path ...`인 연결 방식이었다. [실제 MCP 검사](../../tools/unity_mcp_probe.py)의 initialize, tools/list, tools/call editor_status가 성공했고 ready, compiling=false, domainReloadInProgress=false를 확인했다. [작업 도구](../../tools/unity.ps1)의 setup/check/trial은 기존 에디터에만 연결하며 없거나 컴파일·재로드·Play 중이면 실패한다. 배치 에디터를 자동 생성하지 않는다. 동적 eval의 문자열 변환 오류를 피해 등록된 메뉴를 사용한다.

~~~powershell
powershell -NoProfile -File tools/unity.ps1 open
powershell -NoProfile -File tools/unity.ps1 trial
python tools/unity_mcp_probe.py
~~~

open은 필요한 경우 정상 사용자 환경에서 한 번 실행한다. 첫 빌드는 약 111초 만에 성공했지만 CLI 기본 30초 응답 제한을 초과했다. 작업 도구의 대기를 600초로 늘리고 새 build-success.txt를 검사하도록 했다. 메뉴 응답만으로 성공 판정하지 않는다.

## 완료와 미확인

- [x] 같은 모델 렌더에서 중앙 창문과 화면 가림 수정 확인.
- [x] Unity 컴파일·임포트·실제 출입 통과선 3개 성공.
- [x] Windows BuildPipeline 성공, 새 실행 파일과 성공 기록 확인.
- [x] Editor Play 진입과 게임 화면 렌더 확인.
- [ ] 사람의 E 들기·Q 놓기·낮은 입구 운반감 검증.
- [ ] 최종 외장 텍스처 유지·유리·천장 이음새와 과한 조명·PSX 마감.
- [ ] 문 애니메이션·동적 화면·본 게임·4인 온라인 통합.

좌석·벽·랙은 단색 구조 부품이고 화면 글자는 예시다. 로컬 근거는 `artifacts/ship-interior-trial/passage.txt`, `build-success.txt`, `unity-play.png`, `unity-interior.png`와 `NoReturns/Logs/Editor.log`다. 구조 JSON의 unity_playtest=false는 Blender 검사 자체의 범위이며 별도 Unity 결과와 구분한다. 자동 검사·렌더 확인은 사람 조작감·재미 검증이 아니다.

# FLATBED 내부 구조 시험 05

[English](ship-interior-trial.en.md)

2026-09-15 · 내부부터 설계하는 우주 우체국 구조 후보다. 별도 시험 장면·Windows 빌드에 적용했으며 본 게임과 이전 아트 원본은 보존했다. [이전 상태](../archive/ship-interior-trial-03.ko.md).

## 우주 우체국 내부

사용자가 트럭 같은 공간에서 벗어나도록 요청했다. 양쪽 좌석·랙의 반복을 제거하고 우주 화물 검사·밀폐 보관·송장 작업의 서로 다른 구역으로 재배치했다.

- 중앙: 높인 발판 대신 바닥 검사 표시와 상부 스캐너. 통과 폭 3.04m·머리 위 여유 높이 2.75m. 화물을 들고 입구에서 전방까지 이동할 수 있다.
- 왼쪽: 밀폐 반송함과 잠금·상태등. 창 없는 보관함은 미스터리 연출을 검토할 공간이지만 소리·생물·사건은 아직 구현하지 않았다.
- 오른쪽: 송장 출력기 형태, 작업대, 장비 수납과 벽면 화면. 왼쪽의 높은 보관함과 다른 높이·깊이로 구성했다.
- 후방: 벽에 접힌 직원 좌석 4개와 넓은 출입 공간. 좌석 접기·앉기는 아직 없다.
- 전방: 중심선에 맞는 관측창과 공동 항로 화면을 유지한다. 천장 설비 통로가 작업 구역을 연결한다.

모든 새 업무 장치는 구조 모형이다. 검사·정산·보관·구매 기능과 동적 표시를 구현한 것으로 보지 않는다. 현재 플레이 기능은 이동·빈손 점프·상자 들기/놓기다.

객실 바닥 1.00m, 천장 아래 4.12m, 객실 높이 3.12m, 출입구 폭 3.2m·높이 3.12m를 유지한다. 직원 높이 1.8m·반지름 0.34m, 점프 속도 5m/s·중력 18m/s²도 유지한다. 스캐너 아래까지 점프 검사에 포함한다.

제작 스크립트는 기존 외장을 읽지 않고 빈 Blender 장면에서 실내를 먼저 만든다. 현재 외장은 출입·가림 검사를 위한 임시 덮개다. 실내 플레이 승인 → 실내를 감싸는 외장 구조 → 외장 시안 승인 → 아트 제작 순서로 진행한다. 이전 모델과 본 게임은 보존하며 Tripo 생성·비용 사용은 없다.

## 결과물과 재현

[Blender](../../art/ship-flatbed-01/interior-blockout-05/Flatbed_InteriorTrial.blend) · [FBX](../../art/ship-flatbed-01/interior-blockout-05/Flatbed_InteriorTrial.fbx) · [GLB](../../art/ship-flatbed-01/interior-blockout-05/Flatbed_InteriorTrial.glb)

[내부 전방](../../art/ship-flatbed-01/interior-blockout-05/forward.png) · [내부 후방](../../art/ship-flatbed-01/interior-blockout-05/rear.png) · [외부 후방](../../art/ship-flatbed-01/interior-blockout-05/exterior-rear.png) · [외부 전방](../../art/ship-flatbed-01/interior-blockout-05/exterior-front.png)

[구조 생성·치수 검사](../../art/ship-flatbed-01/interior-blockout.py) · [구조 근거](../../art/ship-flatbed-01/interior-blockout-05/validation.json) · [재가져오기](../../art/ship-flatbed-01/interior-blockout-05/roundtrip.json) · [Unity 검사·빌드](../../NoReturns/Assets/_NoReturns/Editor/ShipInteriorTrialBuild.cs) · [조작](../../NoReturns/Assets/_NoReturns/Runtime/ShipInteriorTrial.cs).

실행 파일: `builds/ShipInteriorTrial/NoReturns-InteriorTrial.exe`. WASD·마우스, Space 빈손 점프, E 들기, Q 놓기, F1 한영 전환, Esc 커서 해제. 같은 폴더 데이터를 유지한다. 빌드는 `powershell -NoProfile -File tools/unity.ps1 trial`로 기존 에디터에 연결해 생성한다. 이전 시험 실행 중이면 닫고 새로 켠다.

## 검증 범위

- [x] 고정 실내 부품의 정점을 새 외장 범위와 전방 경사 지붕 경계에 대조해 돌출 0개 확인. 의도적으로 외부에 있는 문짝·발판·외피는 제외한다. 모든 복잡한 메시 간 교차를 검사하는 일반 도구는 아니다.
- [x] 동일 모델의 내부·외부 렌더 검토와 GLB 재가져오기 성공: 삼각형 4,246개, UV 누락 0, 비정상 좌표 0.
- [x] 실제 CharacterController 진입 5경로, 귀환 3경로 통과.
- [x] 문 아래를 포함한 점프 15지점에서 머리 충돌 없음. 실제 상승 약 0.645m.
- [x] Unity 컴파일·임포트와 Windows 빌드 성공. 새 build-success.txt 확인.
- [ ] 사람의 공간감·출입 여유·화물 운반 재검토.
- [ ] 최종 아트·문 동작·외장 마감·본 게임·4인 온라인 통합.

로컬 근거는 `artifacts/ship-interior-trial/passage.txt`, `build-success.txt`, `envelope-blender.log`, `envelope-roundtrip.log`다. 새 구조가 편한지는 자동 검사로 확정하지 않는다. 사용자의 구조 승인을 받은 뒤 [구조 우선 파이프라인](art-structure-first.ko.md)에 따라 아트 제작을 진행한다. 이번 Tripo 생성·크레딧 사용 없음.

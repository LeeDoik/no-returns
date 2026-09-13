# Unity 이동·운반 프로토타입

[English](15-unity-controls.en.md)

> **과거 기록 · 2026-09-12 폐기/대체.** 현재 게임은 [PSX 우주 배송 기획](01-overview.ko.md)을 따른다. 아래 구현·실행·아트·수치는 과거 자료이며 현재 상태가 아니다. 삭제된 파일은 경로 기록으로만 남긴다.


2026-09-11 · UNITY-02 · 승인 범위: 이동·3인칭 카메라·들기·던지기. 구현과 검증 결과는 아래에 갱신한다.

## 설계와 구현 순서

1. 기존 개발 장면을 보존하고 별도의 CarryLab 장면을 만든다. 바닥·벽·경사로·서로 다른 무게의 박스로 조작을 비교한다.
2. CharacterController 이동과 충돌 회피 카메라를 분리한다. WASD, 마우스, Space를 사용한다. 기본 이동 3.2m/s, 운반 이동 1.6m/s, 점프 초기 속도 4.8m/s는 Godot `scripts/worker.gd`를 출발점으로 삼는다.
3. 박스는 Rigidbody 상태를 유지한다. 운반 목표를 향한 제한된 힘과 회전 토크로 추적하며, 멀어지면 놓는다. E로 들기·놓기, 좌클릭을 충전했다 놓으면 던진다. 손 위치 이동만으로 벽을 통과시키지 않는다.
4. 자동 물리 검사로 집기 거리·가림·방향 추적·벽 충돌·해제·무게별 던지기를 확인한다. 실제 조작감과 시각 품질은 별도로 평가한다.
5. Windows 빌드와 한영 명세·가이드·백로그·검증 이력을 갱신한다.

아트는 임시 기본 도형이다. 승인되지 않은 캐릭터 모델을 제작하지 않는다. 이번 단계는 솔로 조작 검증이며 온라인·배송 규칙·정식 애니메이션·전체 맵 이식은 포함하지 않는다.


## 현재 구현 · Unity 0.2.0

CarryLab (`../../unity/NoReturns/Assets/_Project/Scenes/CarryLab.unity`; retired file)은 기존 Development 장면과 분리된 솔로 운반 실험실이다. 시작 화면에 영어 조작 안내·가까운 박스 이름·던지기 충전 표시가 있다. 06_Unity_Play.cmd (`../../06_Unity_Play.cmd`; retired file)는 빌드된 실험실을 실행한다. 05_Unity.cmd (`../../05_Unity.cmd`; retired file)는 Editor를 연다. 장면이 비어 있으면 Project 창에서 CarryLab을 열거나 `NO RETURNS > Create Carry Lab` 메뉴를 사용한다. 기존 CarryLab 파일을 덮어쓰지 않고 연다.

| 조작 | 동작 |
|---|---|
| WASD | 카메라 수평 방향 기준 이동 |
| 마우스 | 3인칭 시점 회전 |
| Space | 지면에서 점프 |
| E | 앞쪽 가까운 박스 들기 / 내려놓기 |
| 좌클릭 누르기·떼기 | 충전 후 던지기 |
| R | 직원과 박스 초기 위치 복원 |
| Esc / 클릭 | 커서 해제 / 조작 복귀 |

## 규칙과 조정 위치

수치는 첫 조작 검증용이며 최종 밸런스가 아니다. Inspector에서 각 컴포넌트를 수정한다.

| 항목 | 현재 값 | 근거 |
|---|---|---|
| 걷기 / 운반 속도 | 3.2 / 1.6m/s | WorkerController (`../../unity/NoReturns/Assets/_Project/Runtime/WorkerController.cs`; retired file) |
| 점프 초기 속도 | 4.8m/s | WorkerController |
| 집기 중심 거리 | 2.1m 이내, 앞쪽, 가림 없음 | CarryMotor (`../../unity/NoReturns/Assets/_Project/Runtime/CarryMotor.cs`; retired file) |
| 운반 힘 상한 | 110N | CarryMotor |
| 손 목표와 박스 이격 해제 | 2.6m 초과 | CarryMotor |
| 충전 시간 / 던지기 충격량 | 0.85초 / 4~11N·s | WorkerController / CarryMotor |
| 카메라 거리 / 충돌 검사 반경 | 4.2m / 0.2m | ThirdPersonCamera (`../../unity/NoReturns/Assets/_Project/Runtime/ThirdPersonCamera.cs`; retired file) |
| 박스 크기 / 질량 | 0.8m / 1·2·4kg | CarryLabBuilder (`../../unity/NoReturns/Assets/_Project/Editor/CarryLabBuilder.cs`; retired file) |
| 실험 바닥 | 24×24m | CarryLabBuilder |
| 낙하 복구 높이 | -8m 아래 | CargoBody (`../../unity/NoReturns/Assets/_Project/Runtime/CargoBody.cs`; retired file), WorkerController |

운반 중 Rigidbody를 고정하거나 Transform으로 이동시키지 않는다. 손의 목표 위치·속도를 이용한 제한된 힘과 회전 토크로 추적하며, 중력·박스와 직원·월드 충돌은 유지한다. 던지기는 동일 충격량이므로 무거운 박스의 속도 증가가 작다. E와 던지기는 소유권을 해제한다. 접근 거리와 가림을 검사하며 단일 박스만 들 수 있다. 카메라는 장애물 앞까지 접근 거리를 줄인다.

CarryHud (`../../unity/NoReturns/Assets/_Project/Runtime/CarryHud.cs`; retired file)는 uGUI/TMP를 사용한다. 입력 장치는 현재 키보드·마우스이며 게임패드와 조작 재설정 UI는 미구현이다. 임시 캡슐에는 정식 팔·손·애니메이션이 없으므로 손 접촉이나 애니메이션 품질을 검증 완료로 표시하지 않는다.

## 검증 결과와 남은 검토

CarryLabTests (`../../unity/NoReturns/Assets/_Project/Editor/CarryLabTests.cs`; retired file)를 `05_Unity.cmd test`로 실행한다. 별도 배치 Editor에서 실제 물리를 진행하는 24개 검사를 통과했다. 범위는 거리·가림·소유권·낙하·회전·벽 충돌·질량별 충격량·카메라 장애물·걷기·점프·운반 이동이다. 평지 운반에서 손 속도 보정 전 이동 저하를 재현하고 수정했다. `artifacts/unity/test.log`가 근거다.

MCP로 장면 생성·저장·재조회·플레이 시작/종료·스크린샷을 수행했다. `artifacts/unity/carry-hierarchy.json`, `carry-lab.png`에 증거가 있다. 화면 표본은 렌더링과 HUD 표시 확인이며 사람의 전체 조작 테스트가 아니다. 급회전·모서리·경사로 장시간 운반, 손/발 애니메이션, 게임패드, 별도 PC, 온라인 상태 일치는 미검증이다. 캐릭터 운동은 CharacterController 기반이며 완전한 물리 캐릭터나 Human Fall Flat 방식이라고 주장하지 않는다.

기본 TMP 가져오기 API가 시간 초과되어 Editor를 닫고 설치 패키지의 원본 리소스 37개를 GUID 보존 상태로 복원했다. 해당 원본과 라이선스를 Git 대상에 포함한다. 이후 새 프로젝트 생성 시 별도의 자동 글꼴 가져오기 창을 실행하지 않는다.

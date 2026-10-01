# CINDER — 4명 너비의 좁은 내부 미로

[English](cinder-maze-layout.en.md)

2026-10-01 · CINDER-MAZE-01 · **요청한 내부 구조 구현·자동 이동/운반 검사 완료 / 사용자 조작감·사람 4인 검토 미완료.** 게임 버전은 바꾸지 않는다.

## 현재 구조

사용자 요청: “사람 한 4명정도가 겨우 지나갈 정도의 너비로 오밀조밀하게, 약간 미로 같이.” 4명이 나란히 지나는 기준으로 해석해 [별도 CinderMazeReview 씬](../../NoReturns/Assets/_NoReturns/Scenes/CinderMazeReview.unity)의 다섯 건물 내부에 칸막이를 추가했다. 기본 유효 폭은 **3.0m**, 벽 중심 간격 3.3m, 벽 두께 0.3m·높이 4m다. 양 끝을 번갈아 돌아야 하는 길에 건물마다 한 곳씩 양쪽 갈림길을 두어 순환할 수 있게 했다. 전체가 하나의 강제 왕복길이 되지 않도록 한다.

[승인한 노후 질감과 기존 외형](cinder-map-appearance.ko.md), 건물 위치·외곽 크기·3.2×3.3m 남북 출입구·천장·조명·선반·우주선과 운반 규칙을 유지한다. **야외 마당과 건물 간 거리는 이번에 축소하지 않았다.** 출입구 주변의 작은 여유 구역은 기본 복도보다 넓다. 원본·회색·창고 외형·5개 건물 외형 씬을 보존한다. 현재 Cinder에는 배송·리스너·진압봉·온라인이 아직 연결되지 않았다.

| 건물 | 칸막이 | 길을 나누는 방향 | 구성 |
|---|---|---|---|
| A WAREHOUSE | 5 | Z | 좌우 꺾임과 가운데 양쪽 우회 |
| SIDE OFFICE | 3 | X | 작은 구역과 중앙 순환 |
| BAY 04 | 4 | Z | 좌우 꺾임과 양쪽 분기 |
| C SERVICE | 5 | Z | 반대 방향에서 시작하는 꺾임·순환 |
| STORAGE | 7 | X | 길쭉한 보관동을 가로지르는 짧은 구역들 |

총 칸막이 Collider 24개, 기존 벽 모듈 157개·1,884 triangles 추가. 모듈은 `Wall_A`, `Wall_Fill030`, `Wall_Fill090`을 스케일 1로 재사용한다. 새 모델·텍스처·재질·게임 시스템은 없다. 칸막이 한 구간마다 BoxCollider 하나를 두고 실제 시각 경계와 맞췄다. 기존 블록아웃 BoxCollider 109개의 설정/좌표는 비교 검사로 보존했다. 수치·경로 근거는 [생성·검사 코드](../../NoReturns/Assets/_NoReturns/Editor/CinderMazeBuild.cs), [Unity 실측](../../art/cinder-kit-01/maze-unity-validation.json)이다.

## 검증과 경계

- [x] 양방향 이동 156개 구간과 3m 내부 직선 통로 19곳의 네 CharacterController 동시 이동 통과. 몸체는 현재 코드의 높이 1.8m·반지름 0.34m·skin 0.035m다. 네 몸체의 배치 폭은 2.81m로 벽 양쪽에 합계 0.19m 여유가 남는다. 다섯 몸체는 간격 없이도 최소 3.4m가 필요하다는 치수 비교이며 5인 플레이 시험은 아니다.
- [x] 이동 가능한 몸체 자세 388곳×8 yaw×3 pitch = 9,312개 정적 운반 자세. 기존 선반 안으로 들어가던 가장자리 샘플을 통로 쪽으로 조정하고 실제 Capsule 검사로 유효성을 확인했다.
- [x] 실제 `CinderBlockoutWalk.Update`로 156개 양방향 구간 운반 통과. 정적 자세와 이동 중 합계 22,196번 화물 침투 검사에서 0건; 허용 깊이 0.00001m. E/W/S/Q·빈손 점프 통과. 구간마다 시작 자세를 API로 설정하고 실제 키 이벤트를 사용한 Mac Editor 검사이며 사람 조작은 아니다.
- [x] 시점 8개·건물별 천장 제거 검토 화면 5개, 총 13개 실제 Play 카메라 촬영. 검토 촬영 때만 천장 Renderer를 숨기고 복원했다. 저장된 씬의 천장은 유지한다.
- [x] 컴파일 오류 0개, 기존 ShipInteriorTrialBuild obsolete 경고 3개. 최종 Play 콘솔 오류/경고 0개. 기존 씬·표면·운반 코드·선내 재질 등 시작 파일 73개 해시 일치; 기존 선내 재질 변경 8개는 커밋에서 제외한다.
- [ ] 사용자 좁은 길·갈림길·운반 회전·반복 벽/조명 품질 검토. 사람 4명이 화물을 든 채 교행하거나 모퉁이를 같이 도는 감각·재미 검증.
- [ ] 야외 연결 구조의 밀도 조정, 수령/억제 설비, Cinder 기능 통합, 성능 측정과 독립 Mac/Windows 빌드.

[이동/4명 통로](../../art/cinder-kit-01/maze-passage-validation.txt), [운반 자세/이동 검사](../../art/cinder-kit-01/maze-carry-edge-validation.json), [파일 보존·검사 기록](../../art/cinder-kit-01/maze-checks.json). 전체 문서 검사는 기존 누락 artifacts 링크 142개로 실패하며 새 실패는 없다.

![창고 내부 구조 — 검토 촬영 때만 천장 제거](../../art/cinder-kit-01/maze-warehouse-cutaway.png)

[화물 운반 모퉁이](../../art/cinder-kit-01/maze-turn-carry.png) · [갈림길 시점](../../art/cinder-kit-01/maze-junction-empty.png) · [사무실](../../art/cinder-kit-01/maze-side-office-cutaway.png) · [BAY 04](../../art/cinder-kit-01/maze-bay-04-cutaway.png) · [설비동](../../art/cinder-kit-01/maze-c-service-cutaway.png) · [보관동](../../art/cinder-kit-01/maze-storage-cutaway.png).

## 실행·제작 순서

Unity에서 `CinderMazeReview`를 열고 Play 한다. 창고 남측 입구에서 시작하며 WASD 이동, 마우스 시야, E 들기, Q 놓기, 빈손 Space 점프다. `NO RETURNS/Trials/Validate Cinder Maze Review`로 구조/통로를 재검사한다. Play에서 [기존 운반 검사 스크립트](../../tools/unity_checks/CinderCarryEdgeCheck.cs)를 Unity CLI `run_script`로 실행한 뒤 Play를 종료한다. 결과는 `maze-` 접두사로 보존한다. `Create Cinder Maze Review`는 기존 파일이 있으면 중단하므로 수동 수정 결과를 덮어쓰지 않는다.

다음은 이 밀도와 길찾기 감각에 대한 직접 검토다. 통로가 맞으면 BAY 04 수령 단말기/바닥 표시와 연결 통로·야외 마감으로 이어간다. 이전 외형 문서의 단말기 우선 순서는 이번 구조 요청으로 뒤로 이동했다.

원시 staged 공백 검사는 Unity가 생성한 씬·meta 빈 필드 후행 공백 4곳으로 실패한다. 코드·문서·근거 검사와 이 자동 생성 공백만 제외한 전체 staged 검사는 통과했다. Unity YAML을 수동 편집하지 않았다.

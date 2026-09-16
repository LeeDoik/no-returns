# CINDER DEPOT 기본 도형 배치 시험

[English](cinder-blockout.en.md)

2026-09-17 · CINDER-BLOCKOUT-01 · 별도 공간 검증 장면

## 실행과 범위

[Play_Cinder_Blockout.cmd](../../Play_Cinder_Blockout.cmd)를 실행한다. 실행 파일은 `builds/CinderBlockout/NoReturns-CinderBlockout.exe`다. 기존 리스너 게임과 우주선 시험 장면을 보존한다. 이번 장면은 한 명이 공간과 화물 운반 폭을 확인하는 실험이다. 계약·배송 판정·영수증·경제·온라인·적 추적은 연결하지 않았다. 빨간 캡슐 3개는 리스너 배치 표식이다.

WASD 이동, 마우스 시야, E 상자 들기, Q 내려놓기, Shift 빈손 달리기, Space 빈손 점프, F1 한국어/영어, Esc 커서 해제, 클릭으로 시야 조작을 재개한다. 운반 중 달리기·점프는 기존 내부 시험처럼 제한한다. 한영 선택은 이 실행 세션에만 유지한다.

## 배치와 실제 수치

[개념도 02](concept-zoning.ko.md)의 건물 위치·야외 우회로·중앙 지름길·A/B/C 순환로를 기본 도형으로 옮겼다. 개념도 1단위를 0.12m로 해석한 **첫 검증 스케일**이며 최종 맵 크기 확정이 아니다. 설비동의 복잡한 외곽선은 직사각형으로 단순화했다. 각 건물 북쪽·남쪽에 열린 출입구를 두었다. 상세 내부 방·문 조작·단서는 아직 없다.

| 항목 | 구현값 |
|---|---|
| 전체 바닥 | 108×86.4m |
| 창고 | 14.4×20.4m |
| 사무실 | 14.4×9m |
| BAY 04 | 13.8×16.8m |
| 설비동 | 12.6×22.2m |
| 보관동 | 28.8×7.2m |
| 건물 천장 하단 / 출입구 | 4m / 폭 3.2m, 높이 3.3m |
| 지붕 통로 바닥 표시 / 지붕 하단 | 폭 3.2m / 4.2m |
| 순환로·지름길 바닥 표시 | 폭 2.4m. 표시 바깥도 보행 가능한 야외 바닥 |
| 직원 / 카메라 | 높이 1.8m, 반경 0.34m / 눈높이 1.57m |
| 걷기 / 빈손 달리기 | 3m/s / 5m/s |
| 점프 속도 / 중력 | 5m/s / 18m/s² |
| 운반 상자 | 0.8×0.65×0.65m |

우주선은 [내부 시험 05](ship-interior-trial.ko.md)의 모델·충돌·조명을 재사용한다. 남서쪽에 배치하고 출입부를 동쪽 출발 동선으로 돌렸다. 외장을 새로 만들지 않았다. 우주선 규격을 건물 크기에 맞추려고 축소하거나 확대하지 않았다. 지붕 통로는 높이와 경로를 확인하는 단순 판이며 지지대·최종 건축 디테일은 생략했다.

## Unity 편집과 재현

장면: [CinderDepotBlockout.unity](../../NoReturns/Assets/_NoReturns/Scenes/CinderDepotBlockout.unity). `Cinder Depot editable primitive blockout` 아래 건물·장애물·바닥 표시를 이동하거나 크기를 바꾼다. `Reused Ship Interior Trial 05`는 기존 선내 구조 묶음이다. 기본 도형과 BoxCollider를 사용하므로 Unity에서 바로 편집할 수 있다.

생성·검사·빌드는 Unity의 `NO RETURNS > Trials > Create/Validate/Build Cinder Depot Blockout` 메뉴로 실행한다. **Create와 Build는 생성 스크립트로 장면을 다시 만들므로 직접 편집한 장면을 덮어쓴다.** 직접 수정한 안은 다른 이름으로 저장하거나 [생성 코드](../../NoReturns/Assets/_NoReturns/Editor/CinderBlockoutBuild.cs)에 반영한다. 단독 Validate는 현재 장면을 검사한다. 출력 루트는 기존 `CarryWorkspace.txt`가 있으면 해당 경로를 사용한다.

수치 근거: 위 생성 코드와 [이동 코드](../../NoReturns/Assets/_NoReturns/Runtime/CinderBlockoutWalk.cs). 기존 우주선 재질의 작업 시작 전 변경 8개는 이번 변경에 포함하지 않는다.

## 검증과 다음 플레이 확인

- [x] Unity MCP로 컴파일·장면 생성.
- [x] 실제 CharacterController 이동 검사 41개 통과: 건물 양쪽 출입 5, 순환로 변 12, 우주선 왕복 2, 우회로·지름길 구간 22.
- [x] Unity 장면 전체 화면 및 Play Mode 시작 시점 렌더 확인. 카메라 1개, 상자 크기 확인.
- [ ] 사람의 E/Q 운반 조작감, 점프 여유, 반복 이동 피로, 유인 순환의 재미.
- [ ] 새 맵의 적 추적·온라인·완성 배송 사이클.

자동 통로 검사는 빈손 직원의 물리 이동 검사다. 모든 방향의 화물 회전·운반 충돌이나 실제 협동 플레이를 통과한 것으로 해석하지 않는다. 통로 통과가 건물 용도의 재미를 보장하지 않는다.

로컬 검증 근거: `artifacts/cinder-blockout/passage.txt`, `overview.png`, `spawn.png`. 자동 검사와 사람의 체감 평가는 분리한다.

- [x] 별도 Windows 빌드 성공. 근거: artifacts/cinder-blockout/build-success.txt 및 Unity BuildReport 성공.


Windows 실행 창에서도 한국어 HUD·상자·건물 표시를 확인했다. 시작 예외는 없지만 기존 URP DepthOfField/Panini 후처리 셰이더 제거 경고가 남는다. 언어 전환 및 E/Q 전체 조작의 사람 검증은 남겨 둔다.

# CINDER — 억제 범위 내부의 전체 맵 배치

[English](cinder-compact-site.en.md)

2026-10-01 · CINDER-COMPACT-SITE-01 · **전체 구역 밀도 수정·자동 이동/운반 검사 완료 / 사용자 품질·사람 4인 검토 미완료.** 게임 버전은 바꾸지 않는다.

## 수정한 범위

사용자 정정: “내가 말한건 전체 맵을 말한거야. 억제기 내부 공간을 오밀조밀하게 구성해줘. 컨셉 사진처럼.” 기존 작업은 건물 실내 복도만 좁히는 잘못된 해석이었다. [컨셉의 억제 범위](../art/space-concepts/cinder-depot-suppression-01.png)와 [전체 상면 배치](../art/space-concepts/cinder-depot-overhead-01.png)를 확인하고, **억제 범위 안의 건물·야외 골목·설비/보관 덩어리·우주선 착륙 구역을 함께 재배치**했다. 이번 기준은 [CinderCompactSiteReview](../../NoReturns/Assets/_NoReturns/Scenes/CinderCompactSiteReview.unity)다. 잘못 해석한 [실내 미로](cinder-maze-layout.ko.md)는 현재 씬에 넣지 않으며 과거 기록으로만 보존한다.

억제 범위 시각 표시는 53.55×65.4m, 시설 외곽 동선의 기본 범위는 47.55×59.4m다. 원래 108×86.4m 검토 바닥은 주변 배경과 추락 방지용으로 남긴다. 개별 건물 크기를 줄이지 않고 위치를 가까이 모은다. 남서쪽 착륙장에서 북쪽을 향하도록 기존 우주선을 옮겼다. 억제 범위의 녹색 선과 네 기둥은 **경계/위치 표식**이며 억제 작동·외곽 생물 제한 기능은 미연결이다.

| 건물 | 크기 X×Z (m), 유지 | 새 중심 X,Z (m) |
|---|---|---|
| A WAREHOUSE | 14.4×20.4 | -14.1, 1.8 |
| SIDE OFFICE | 14.4×9 | -14.1, 19.8 |
| BAY 04 | 13.8×16.8 | 12.75, 15.9 |
| C SERVICE | 12.6×22.2 | 13.35, -6.9 |
| STORAGE | 28.8×7.2 | 5.25, -24.9 |

빈 마당을 북쪽 관제 별동, L자 전력 구역, 서쪽 보관 베이, 중앙 설비섬과 동쪽 설비 덩어리로 나눈다. 6개 닫힌 보조 볼륨으로 구성하며 **새 출입 가능 실내나 상호작용 설비를 구현한 것은 아니다**. 건물 사이 기본 골목 폭 3m, 일부 어긋난 골목 3.15m, 기존 출입구/새 경계 출입구는 3.2×3.3m다. 창고 외곽·중앙 설비섬·동쪽 설비동·남쪽 보관동에 순환로 4개가 연결된다. 긴 서쪽/북쪽 지붕 경로 94m와 꺾이는 중앙 지름길 79.1m가 같은 BAY 04 중앙에 도달한다. 실제 수령 판정 위치가 연결된 것은 아니다.

## 제작과 충돌

[승인한 노후 외형](cinder-map-appearance.ko.md)을 재사용한다. 벽·모서리·끝·문틀·작업등의 기존 키트 메시 배치 383개를 추가하며 여기에는 새 작업등 16개가 포함된다. 벽 높이/지붕 하단 4m·두께 0.3m를 유지하며 배치 스케일은 1이다. 빈 바닥/보조 지붕은 기존 `Floor_A`·`Ceiling_A`의 실제 UV를 샘플링해 1.2m 타일 밀도로 조합한 native Unity 메시다. 건물 바닥 영역을 빼고 가장자리 타일을 잘라 기존 아틀라스 밀도와 치수를 맞춘다. 게임용 바닥·보조 지붕·통로 지붕은 저장된 씬에서 유지한다. 외부 모델·새 생성 이미지·의존성을 추가하지 않는다. 경계 표시용 Unlit 재질 1개와 재현 가능한 메시 asset을 별도 보관한다.

기존 건물 BoxCollider 50개와 우주선의 **로컬 형상/상태**를 보존한 채 전체 위치/우주선 회전을 바꾼다. 새 배치에 맞지 않는 옛 길 바닥/지붕·적재 장애물은 대체했다. 원래 블록아웃 그룹에 남은 BoxCollider는 59개(건물 50, 억제기 표식 4, 검토 바닥 1, 바깥 추락 방지 4)다. 새 시설 그룹의 Collider는 22개다. 기존 109개 좌표를 그대로 유지했다는 이전 외형/미로 검증은 이번 재배치에 적용하지 않는다. 선반·우주선 내부와 기존 씬 파일을 보존한다. 재검사에서 설비 모서리의 화물 0.025m 침투를 발견했다. native BoxCast가 스치듯 통과하는 모서리를 놓쳤으므로 [공통 운반 코드](../../NoReturns/Assets/_NoReturns/Runtime/CinderBlockoutWalk.cs)의 `TrialCargoPose.Position`의 기존 침투 분리를 최종 위치에도 적용했다. Cinder·선내 운반이 같은 함수를 사용한다. 해당 자세를 정적 검사에 고정해 재현 가능한 회귀 검사로 남긴다. 속도·키·들기/놓기 규칙은 유지한다.

수치·재생성: [배치/검사 코드](../../NoReturns/Assets/_NoReturns/Editor/CinderCompactSiteBuild.cs), [native 표면 메시](../../NoReturns/Assets/_NoReturns/Art/CinderCompactSite01/CompactTileSurfaces.asset), [실측](../../art/cinder-kit-01/site-unity-validation.json), [보존한 로컬 충돌 기준](../../art/cinder-kit-01/site-local-collision-baseline.json). `NO RETURNS/Trials/Create Compact Cinder Site Review`는 기존 파일이 있으면 중단한다. `CreateScene(true)`는 의도적인 재생성용이며 현재 씬을 저장·Play 종료 후에만 사용한다. 수동 수정이 있으면 먼저 보존한다. Unity YAML을 수동 편집하지 않는다.

## 검증·실행·남은 작업

- [x] 실제 양방향 이동 94개 구간(배송 2경로·순환 4개·건물 남북 출입·연결길·우주선 경사로) 통과. 같은 BAY 04로 연결되는 두 동선을 확인했다.
- [x] 골목 17곳의 벽 사이 최소 폭 3/3.15m 실측과 네 CharacterController 동시 이동 통과. 높이 1.8m·반지름 0.34m·skin 0.035m, 나란히 배치한 네 몸체 폭 2.81m. 문 열림 구간은 폭이 넓어지므로 세 지점 중 벽이 마주 보는 최소 폭을 측정한다.
- [x] 이동 가능한 자세 159곳×8 yaw×3 pitch = 3,816개 정적 운반 자세와 실제 `CinderBlockoutWalk.Update` 운반 94개 구간. 총 침투 검사 237,148회에서 겹침 0건; 경계 접촉 57건, 허용 깊이 0.00001m. E/W/S/Q·빈손 점프 통과. 구간마다 API로 시작 자세를 설정한 키 이벤트 검사이며 사람 조작은 아니다.
- [x] 기존 건물/우주선 로컬 충돌, 표면 UV·스케일, 시작 파일 75개 중 의도적으로 수정한 공통 운반 코드 1개를 제외한 74개 해시 보존 확인. 기존 선내 재질 변경 8개는 커밋 제외. 컴파일 오류 0개·기존 obsolete 경고 8회(중복 제외 6개), 최종 Play 콘솔 오류/경고 0개.
- [x] 실제 카메라 10개 화면 촬영·검토. 전체 상면 촬영 때만 천장/보조 지붕을 숨기고 복원했다. 현재 씬의 지붕과 바닥은 유지한다.
- [ ] 사용자 전체 구역 밀도·길찾기·긴 골목/분기 시야·컨셉 방향 확인, 사람 4인 교행과 화물 회전 감각.
- [ ] 보조 볼륨의 최종 설비/적재물 외형, 경계 바깥 암석/환경, 수령 단말기와 AI/배송/억제/진압봉/온라인의 새 좌표 통합·성능·독립 빌드.

[이동·네 몸체 검사](../../art/cinder-kit-01/site-passage-validation.txt), [운반 검사](../../art/cinder-kit-01/site-carry-edge-validation.json), [보존·검사 기록](../../art/cinder-kit-01/site-checks.json). 전체 문서는 기존 artifacts 링크 누락 142개로 실패하며 새 실패는 없다.

원시 staged 공백 검사는 Unity가 생성한 씬·메시·재질·meta의 빈 필드 후행 공백 30곳으로 실패한다. 코드·문서·근거 공백 검사와 이 생성 후행 공백만 제외한 전체 staged 검사는 통과했다. 공백 검사를 위해 Unity YAML을 수동 수정하지 않았다.

![억제 범위 안의 전체 배치 — 지붕 제거는 검토 촬영용](../../art/cinder-kit-01/site-field-cutaway.png)

[서쪽 지붕 경로](../../art/cinder-kit-01/site-west-covered.png) · [중앙 분기](../../art/cinder-kit-01/site-central-junction.png) · [중앙 운반 골목](../../art/cinder-kit-01/site-central-carry.png) · [지붕 포함 전체 모습](../../art/cinder-kit-01/site-overview.png).

Unity에서 `CinderCompactSiteReview`를 열고 Play 한다. 우주선 경사로 앞에서 시작하며 WASD 이동, 마우스 시야, E 들기, Q 놓기, 빈손 Space 점프다. `Validate Compact Cinder Site Review`로 재검사하고, Play에서 [운반 검사](../../tools/unity_checks/CinderCarryEdgeCheck.cs)를 Unity CLI `run_script`로 실행한 뒤 종료한다. 결과는 `site-` 접두사로 보존한다. 다음은 전체 구조 직접 검토 후 수령 설비와 기능 좌표 통합이다.

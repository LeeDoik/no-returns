# CINDER — 승인한 노후 외형의 건물 확장

[English](cinder-map-appearance.en.md)

**현재 기준은 [전체 맵과 소품이 있는 CinderCompactSiteReview](cinder-compact-site.ko.md)다.** 전체 맵 방향 사용자 승인 후 기존 노후 아틀라스·선반·화물/CRT를 재사용해 소품 묶음 47개를 배치했다. 아래는 재배치/소품 전 공통 외형 기록이며 새 Collider 0개·기존 109개 월드 좌표 보존·이전 경로 통과 수치를 현재 소품 씬에 승계하지 않는다. 현재 구조·소품 검사와 품질 미완료 상태는 연결된 최신 기록을 따른다.

2026-10-01 · CINDER-MAP-APPEARANCE-01 · **창고 노후 질감 사용자 승인 / 건물 5개 공통 외형 적용·자동 검사 완료 / 확장 결과의 사용자 검토 미완료.** 게임 버전 변경이나 전체 맵 완성 선언이 아니다.

## 범위와 기준

사용자가 “그래 이런 느낌으로 맵을 구성하자. 다음으로 넘어가자”라고 [노후 창고 외형](cinder-asset-prep.ko.md#2026-10-01--노후-질감-수정)을 맵의 스타일 기준으로 승인했다. 누런 패널·벗겨진 적갈색 띠·녹과 때·어두운 문틀·따뜻한 작업등을 기존 사무실·BAY 04·설비동·보관동으로 확장했다. 창고 검토 씬을 보존한 별도 [CinderMapAppearanceReview](../../NoReturns/Assets/_NoReturns/Scenes/CinderMapAppearanceReview.unity)에서 작업했다. 스타일 승인을 전체 동선·시야·재미·출시 품질 승인으로 확대하지 않는다.

건물 위치·크기·출입구·충돌·우주선과 운반 런타임은 유지한다. 야외 지면, 연결 통로의 바닥/지붕, 장애물, 억제 설비 표식과 우주선 마감은 기존 시험 외형이다. 수령·영수증·리스너·진압봉·온라인을 새 맵에 연결하지 않았다. 이번 확장은 건물의 바닥·벽·문틀·천장·작업등까지다.

| 건물 | 기존 바닥 X×Z (m) | 이번 추가 배치 (등 포함) | 이번 추가 삼각형 |
|---|---|---|---|
| 창고 / A WAREHOUSE | 14.4×20.4 | 기존 530개 구조·7개 표현 유지 | 기존 6,408+216 유지 |
| 사무실 / SIDE OFFICE | 14.4×9 | 282 | 3,480 |
| BAY 04 | 13.8×16.8 | 450 | 5,496 |
| 설비동 / C SERVICE | 12.6×22.2 | 546 | 6,648 |
| 보관동 / STORAGE | 28.8×7.2 | 418 | 5,112 |

추가 합계 1,696개 배치·20,736개 삼각형. 기존 창고와 합친 부품군은 27,360개 삼각형이며 전체 씬/FPS 실측값이 아니다. 각 건물의 양측 출입구 3.2×3.3m·천장 하단 4m를 유지한다. 새 작업등은 건물마다 출입구 외부 2개와 내부 측벽 2개, 총 16개다. 기존 5개와 합쳐 21개이며 색 (1,0.67,0.3)·강도 0.65·범위 5m·그림자 없음은 기존 프리팹 값이다.

근거: [기존 배치 코드](../../NoReturns/Assets/_NoReturns/Editor/CinderBlockoutBuild.cs), [확장 조립·검사 코드](../../NoReturns/Assets/_NoReturns/Editor/CinderMapAppearanceBuild.cs), [Unity 실측](../../art/cinder-kit-01/map-unity-validation.json). 배치는 현재 씬의 원래 바닥 Bounds에서 계산하며 임의로 건물 크기를 조절하지 않는다.

## 보완 부품과 제작

기존 공용 512×512 아틀라스와 재질을 재사용한다. 부품을 배치 스케일로 늘이지 않고 아래 보완형의 형상·UV를 원본에서 잘라 사용한다. 새 이미지 생성·Tripo·외부 모델 도입 없음.

| 이름 (`NR_Cinder_` 접두사) | Unity X×Y×Z (m) | 피벗 |
|---|---|---|
| Floor_Half | 0.6×0.2×1.2 | 상면 중앙 |
| Floor_Quarter | 0.6×0.2×0.6 | 상면 중앙 |
| Ceiling_Half | 0.6×0.3×1.2 | 하부 중앙 |
| Ceiling_Quarter | 0.6×0.3×0.6 | 하부 중앙 |
| Ceiling_Edge_Half | 0.15×0.3×0.6 | 하부 중앙 |
| Wall_Fill005 | 0.05×4×0.3 | 하단 중앙 |
| Wall_Fill030 | 0.3×4×0.3 | 하단 중앙 |
| Wall_Fill065 | 0.65×4×0.3 | 하단 중앙 |

각 12개 삼각형·재질 슬롯 1개·닫힌 면이다. 새 FBX/프리팹 8개는 `Art/CinderMapFills01` / `Prefabs/CinderMapFills01`에 둔다. 기존 14개를 합친 모델/프리팹은 22개다. [Blender 제작 코드](../../art/cinder-kit-01/build_map_fills.py), [원본](../../art/cinder-kit-01/Cinder_Map_Fills.blend), [재가져오기 결과](../../art/cinder-kit-01/map-fill-validation.json)를 보관했다. `blender --background --python art/cinder-kit-01/build_map_fills.py`로 보완형만 재생성한다. Unity 생성 메뉴 `NO RETURNS/Trials/Create Cinder Map Appearance Review`는 기존 파일이 있으면 중단하며 씬 재생성을 위해 수동 편집을 덮어쓰지 않는다. YAML 직접 편집 없음.

작업등의 기존 재질은 `_EMISSION`과 `EmissiveIsBlack` 플래그가 함께 저장돼 URP 재가져오기에서 발광이 꺼질 수 있었다. 공유 제작 코드와 재질에서 `BakedEmissive`로 고치고 재가져오기 후 발광 유지 검사를 추가했다. 색·발광 색·Point Light 값은 바꾸지 않았고 라이트맵을 베이크하지 않았다. 기준 파일 82개 중 이 재질 1개만 수정했고 나머지 81개 해시가 같다. 기존 우주선 재질 변경 8개는 보존하고 커밋에서 제외한다.

## 검증과 직접 확인

- [x] Blender/FBX 보완형 8개 치수·피벗·닫힌 면·UV 범위·재가져오기 UV/삼각형/재질 일치 확인.
- [x] Unity 임포트 8개와 기존 14개, 배치 스케일 1·바닥/지붕 면적·새 Collider 0개 확인. 건물 Collider 50개와 전체 블록아웃 BoxCollider 109개 설정·배치 보존 대조.
- [x] 기존 이동 경로 41개, 창고 통과 6개·점프 3개·중앙 화물 자세 48개 통과.
- [x] Mac Editor Play 98개 위치×8개 yaw×3개 pitch = 2,352개 운반 자세의 겹침 0개, E/W/S/Q·빈손 점프 통과. 접촉 경계 6개·허용 오차 0.00001m. 실제 키/API 자세 검사이며 사람 실시간 조작이 아니다.
- [x] Play 카메라 15개 화면 촬영. 건물별 빈손/운반·창고 입구·전체 배치를 확인했다. 컴파일 오류 0개·마지막 Play 콘솔 오류/경고 0개.
- [ ] 확장한 건물의 사용자 시야·이음새·반복 질감·조명 밀도 검토, 원거리 깜빡임과 성능 측정.
- [ ] 야외·연결 통로·수령 설비·건물별 내부 용도와 표지 제작. 독립 Mac/Windows 빌드·선내 Play·온라인/AI/배송 검증.

[검사·보존 해시](../../art/cinder-kit-01/map-checks.json) · [이동 경로](../../art/cinder-kit-01/map-passage-validation.txt) · [Play 자세/입력](../../art/cinder-kit-01/map-carry-edge-validation.json). 전체 문서 검사는 기존 누락 artifacts 링크 142개로 실패하며 이번 새 실패는 없다.

![건물 5개 외형과 기존 야외 시험 배치](../../art/cinder-kit-01/map-overview.png)

[사무실 빈손](../../art/cinder-kit-01/map-side-office-empty.png) · [BAY 04 운반](../../art/cinder-kit-01/map-bay-04-carry.png) · [설비동 빈손](../../art/cinder-kit-01/map-c-service-empty.png) · [보관동 운반](../../art/cinder-kit-01/map-storage-carry.png). 실제 Play 카메라이며 IMGUI 안내는 포함하지 않는다. 전체 배치 화면만 높은 API 시점과 pitch 40°를 사용한다.

Unity에서 위 맵 씬을 열어 Play한다. 시작은 창고 남측 입구다. WASD 이동·마우스 시야·E 들기·Q 놓기·빈손 Space 점프·F1 한영·Esc 커서 해제. 재검사는 `NoReturns.Editor.CinderMapAppearanceBuild.Validate()`; Play 후 기존 [운반 검사](../../tools/unity_checks/CinderCarryEdgeCheck.cs)를 실행하고 Play를 중지한다. 새 검사와 화면에는 `map-` 접두사를 사용해 창고 근거를 덮어쓰지 않는다.

현재 우선순위는 [억제 범위 내부 전체 배치](cinder-compact-site.ko.md)의 사용자 직접 검토다. 실내만 좁힌 해석을 바로잡아 건물 사이 간격·마당·설비 덩어리를 함께 구성했다. 전체 밀도/길찾기/운반 감각을 확인한 뒤 BAY 04 수령 단말기·바닥 표시와 기존 CarryRoom 기능의 새 좌표 통합으로 이어간다. 외형 배치와 실제 배송 통합 상태를 구분한다.

원시 staged 공백 검사는 Unity가 생성한 씬·프리팹·meta의 빈 필드 후행 공백 106곳으로 실패한다. 코드·문서·근거 검사는 통과했고, 이 자동 생성 후행 공백만 제외한 전체 staged 검사도 통과했다. 공백 검사만을 위해 Unity YAML을 수동 편집하지 않았다.

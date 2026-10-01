# CINDER — 억제 범위 내부의 전체 맵 배치

[English](cinder-compact-site.en.md)

2026-10-01 · CINDER-COMPACT-SITE-01 · **전체 맵 방향 사용자 승인·소품과 황혼 스카이 적용·자동 검사 완료 / 하늘·소품 품질과 사람 4인 검토 미완료.** 게임 버전은 바꾸지 않는다.

## 2026-10-01 — 스카이와 원거리 분위기

사용자 요청 “그래 그럼 이제 스카이 설정하자.”에 따라 같은 `CinderCompactSiteReview`에 보랏빛 황혼·옅은 구름·원거리 안개를 적용했다. [억제 범위 컨셉](../art/space-concepts/cinder-depot-suppression-01.png)과 [컨셉 02](../art/space-concepts/concept-02.png)의 보랏빛 배경/따뜻한 작업등 조합을 참고했다. 기본 `Skybox/Procedural` 시도는 실제 화면의 노란 지평선이 맞지 않아 채택하지 않았다. 최종 하늘은 Unity 기본 `Skybox/Cubemap`과 **64×64 픽셀 6면 RGBA32 큐브맵**이다. 방향 좌표 노이즈와 색 그라데이션을 native 코드로 만들어 정적으로 저장하며 별도 셰이더·외부 이미지·의존성·날씨 시스템은 추가하지 않았다.

| 설정 | 현재 값 |
|---|---|
| 스카이 재질/텍스처 | `Cinder_Dusk_Sky.mat` / `Cinder_Dusk_Cube.asset` |
| 스카이 노출·회전 | 1 / 0° |
| 큐브맵 상단 RGB / 지평선 RGB / 하단 RGB | (0.12,0.09,0.18) / (0.34,0.27,0.38) / (0.24,0.19,0.29) |
| 방향광 강도·회전 X/Y/Z | 0.55 / (24,-30,0)°; Inspector Y=330° |
| 방향광 RGB | (0.84,0.76,0.91) |
| 환경광 | Flat, RGB (0.45,0.40,0.45) |
| 안개 | Linear, RGB (0.34,0.27,0.38), 시작 35m·끝 115m |

가까운 골목·화물·표지는 안개 시작 거리 안에 두고, 먼 시설/배경은 지평선 색에 섞인다. 실내/선내 작업등을 포함한 기존 로컬 광원 40개의 설정·배치를 유지했다. 방향광과 환경광을 낮추어 따뜻한 작업등 대비를 키웠다. 건물·우주선·소품 배치·충돌·운반 런타임 변경은 없다. 새 하늘은 정적 배경이며 억제 단계 변화나 주야 순환과 연결하지 않았다. 경계 밖 지형/크리쳐 모델과 조명 베이크를 완료한 것은 아니다.

근거와 재적용: [스카이 설정·제작·검사 코드](../../NoReturns/Assets/_NoReturns/Editor/CinderSkyBuild.cs), [스카이 재질](../../NoReturns/Assets/_NoReturns/Art/CinderCompactSite01/Cinder_Dusk_Sky.mat), [큐브맵](../../NoReturns/Assets/_NoReturns/Art/CinderCompactSite01/Cinder_Dusk_Cube.asset), [설정 실측](../../art/cinder-kit-01/sky-settings-validation.json). 현재 씬을 저장하고 Play를 종료한 뒤 `NO RETURNS/Trials/Apply Cinder Dusk Sky`를 실행하면 이 프리셋과 큐브맵을 재적용한다. 수동 조정은 Lighting의 Environment, 해당 재질과 `Blockout daylight`에서 한다. 메뉴는 수동 스카이 조정을 프리셋으로 되돌리므로 먼저 필요한 값을 보존한다. 다른 씬/미저장 상태/Play 중에는 중단한다.

검증: 기존 충돌·246개 소품 주변 유효 위치·로컬 광원 보존, 이동 94구간·네 몸체 통로 17곳 통과. Mac Editor Play 운반 자세 5,904개·실제 운반 94구간·침투 검사 25,942회에서 겹침 0건, 경계 접촉 57건, E/W/S/Q·빈손 점프 통과. API 자세/키 이벤트 검사이며 사람 실시간 조작은 아니다. 실제 카메라 18개로 하늘·골목·운반 시야·표지·실내를 검토했고 촬영용 지붕 숨김을 복원했다. 컴파일/스카이 셰이더 오류 0개·기존 obsolete 경고 6종·최종 Play 오류/경고 0개. 시작 파일 313개 중 현재 씬을 제외한 312개 해시 일치, 기존 선내 재질 변경 8개는 커밋 제외. 기존 `props-` 근거를 유지하고 현재 운반/촬영 근거는 저장된 스카이 재질 경로로 구분해 `sky-`에 쓴다.

[이동/네 몸체 검사](../../art/cinder-kit-01/sky-passage-validation.txt) · [운반 검사](../../art/cinder-kit-01/sky-carry-edge-validation.json) · [보존·검사 기록](../../art/cinder-kit-01/sky-checks.json). 전체 문서 검사는 기존 누락 링크 142개로 실패하며 새 실패 없음. 하늘/안개 품질의 사용자 평가·최종 위험 표식 가독성·사람 4인·성능·독립 빌드·게임 기능 연결은 미검증이다. 아래 소품/구조 수치는 스카이 적용 전 기록이다.

![현재 하늘과 골목](../../art/cinder-kit-01/sky-upward.png)

[골목 표지](../../art/cinder-kit-01/sky-office-link.png) · [화물 운반 시야](../../art/cinder-kit-01/sky-central-carry.png) · [실내](../../art/cinder-kit-01/sky-warehouse-racks.png) · [시설 외부와 원거리 안개](../../art/cinder-kit-01/sky-exterior-machinery.png).

원시 staged 공백 검사는 Unity 생성 큐브맵·재질·meta의 빈 필드 후행 공백 10곳으로 실패한다. 코드·문서·근거 공백 검사와 이 생성 후행 공백만 제외한 전체 staged 검사는 통과했다. Git LFS PNG 포인터 18개와 `git lfs fsck --pointers`를 확인했다. 공백 검사만을 위해 native YAML을 수동 편집하지 않았다.

## 2026-10-01 — 구역별 소품 배치

사용자가 “그래 느낌 괜찮네 그러면 이제 이 맵을 더 개선해보자. 소품이나 오브젝트 배치같은거.”라고 전체 맵의 방향을 승인했다. 같은 `CinderCompactSiteReview`에 **소품 묶음 47개**를 추가했다. 건물 위치·크기·3/3.15m 골목·네 순환로·우주선·기존 이동 구간과 운반 런타임을 유지한다. 소품 외형의 사용자 평가와 사람 4인 실제 교행은 아직 하지 않았다.

| 묶음 | 수 | 배치·역할 |
|---|---|---|
| Rack | 5 | 창고·BAY 04·설비동·보관동의 벽 쪽 화물 선반 |
| Pallet | 7 | 창고·BAY 04·보관동·착륙장 적재 화물 |
| Workbench | 5 | 포장·사무·발송·설비 작업대 |
| Cabinet | 4 | 사무실·BAY 04·설비동의 벽에 붙인 전력함 |
| Drums | 3 | 창고·설비동·보관동, 묶음당 드럼통 3개 |
| Roof machinery | 6 | 닫힌 보조 건물 지붕의 설비 |
| Pipes | 6 | 높은 벽면 배관, 묶음당 3m 배관 2개 |
| High wall vent | 5 | 높은 벽면 환기구 |
| Legend | 6 | 출입구·북쪽 순환로의 물리적 시설 표지 |

기존 화물 외형 46개와 CRT 외형 3개(사무실 2, 발송 작업대 1)를 재사용했다. 모두 정적 소품이며 수령 판정·영수증 회수·E/Q 상호작용은 연결하지 않았다. 실제 시험 운반 화물은 기존 것을 유지한다. 표지의 영어 원문은 `A / WAREHOUSE`, `SIDE / OFFICE`, `BAY 04 / DISPATCH`, `C SERVICE / POWER`, `STORAGE / FREIGHT`, `BAY 04 / NORTH LOOP`이며 `/`는 줄바꿈이다.

[소품 제작·배치·검사 코드](../../NoReturns/Assets/_NoReturns/Editor/CinderSitePropsBuild.cs)가 팔레트·전력함·작업대·드럼통·환기구·지붕 설비·배관·표지판의 native 메시 8개를 만든다. [CinderSiteProps01](../../NoReturns/Assets/_NoReturns/Art/CinderSiteProps01/)에 저장하고 기존 노후 아틀라스·선반·Parcel/Receipt 모델을 재사용한다. 새 텍스처·외부 에셋·의존성·폰트는 추가하지 않았다. 표지 글자는 내장 `LegacyRuntime.ttf`와 world-space UGUI를 사용하며 클릭을 받지 않는다. 추가 소품군은 메시 배치 108개·삼각형 205,055개·예약 체적 BoxCollider 56개다. 기존 모델의 삼각형을 포함한 배치 합계이며 FPS나 최종 최적화 예산이 아니다. 배관 중심 높이 3.15/3.55m·환기구 하단 2.9m·지붕 설비 하단 4.3m로 골목 몸체 공간을 비운다.

첫 보완 검사의 전력함 뒤/옆 8개 자세에서 화물 겹침을 발견했다. 전력함 정면을 방 안으로 향하게 하고 뒷면을 벽에 밀착해 몸만 들어갈 수 있는 틈을 없앴다. 기존 운반 코드는 바꾸지 않았다. 최종 246개 몸체 위치×24방향/시선 = **5,904개 운반 자세**, 실제 이동 94구간, native 침투 검사 **407,130회에서 겹침 0건**, 경계 접촉 57건, 허용치 0.00001m. E/W/S/Q·빈손 점프 통과. 소품 양끝·벽/선반 주변을 추가 검사하며 기존 유효 위치 159개·양방향 이동 94구간·네 몸체 통로 17곳은 유지한다. API 위치 설정과 실제 `CinderBlockoutWalk.Update` 키 이벤트 검사이며 사람 실시간 조작은 아니다.

원래 씬의 소품 외 모든 BoxCollider 월드 설정을 대조했다. 시작 파일 159개 중 의도적으로 수정한 현재 씬 1개를 제외한 158개 해시가 같다. 기존 선내 재질 변경 8개는 커밋 제외. 공유 충돌 기록 함수는 씬 루트 Collider의 부모가 없는 경우를 처리하도록 보완했다. 실제 카메라 17개 화면을 검토했고 상면 촬영 때만 지붕/지붕 설비를 숨긴 뒤 복원했다. 컴파일 오류 0개·기존 obsolete 경고 6종·최종 Play 오류/경고 0개. TMP 설정 확인이 불필요한 리소스를 자동 임포트해 해당 작업 생성 폴더만 native API로 제거했으며 최종 소품은 TMP를 쓰지 않는다.

근거: [소품 실측](../../art/cinder-kit-01/props-layout-validation.json), [구조 실측](../../art/cinder-kit-01/props-unity-validation.json), [이동/네 몸체 검사](../../art/cinder-kit-01/props-passage-validation.txt), [운반 검사](../../art/cinder-kit-01/props-carry-edge-validation.json), [보존·이전 실패·화면·검사 기록](../../art/cinder-kit-01/props-checks.json). 전체 문서 검사는 기존 누락 링크 142개로 실패하며 새 실패는 없다. 아래 `site-` 결과는 소품 배치 전 구조 작업의 기록이다.

![화물 선반과 적재물](../../art/cinder-kit-01/props-warehouse-racks.png)

[사무 작업대](../../art/cinder-kit-01/props-office-workbenches.png) · [발송 작업대](../../art/cinder-kit-01/props-bay-dispatch.png) · [시설 표지](../../art/cinder-kit-01/props-facility-sign.png) · [지붕·벽면 설비](../../art/cinder-kit-01/props-exterior-machinery.png) · [소품 배치 상면](../../art/cinder-kit-01/props-field-cutaway.png).

현재 씬에 이미 배치했다. `NO RETURNS/Trials/Add Cinder Site Props`는 Play 중·미저장·다른 씬·기존 소품군이 있으면 중단하여 수동 편집을 보호한다. `Validate Cinder Site Props`로 검사하고 Play에서 [공통 운반 검사](../../tools/unity_checks/CinderCarryEdgeCheck.cs)를 실행한다. 스카이 없는 소품 씬은 `props-`, 현재 스카이 포함 씬은 `sky-` 결과를 만든다. 기존 `site-` 근거는 덮어쓰지 않는다. 다음은 소품 밀도/운반 시야의 직접 평가, BAY 04 수령 시설·기능 좌표 통합이다. 사람 4인·AI/배송/억제/진압봉/온라인·경계 밖 환경·성능·독립 빌드는 미검증이다.

원시 staged 공백 검사는 Unity 생성 씬·메시·meta의 후행 공백 153곳으로 실패한다. 코드·문서·근거와 이 생성 공백만 제외한 전체 검사는 통과했다. 공백 검사만을 위한 native YAML 수동 편집은 하지 않았다.

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
- [x] 사용자 전체 맵 방향 승인.
- [ ] 소품 품질·길찾기·긴 골목/분기 시야와 사람 4인 교행·화물 회전 감각.
- [ ] 보조 볼륨의 최종 설비/적재물 외형, 경계 바깥 암석/환경, 수령 단말기와 AI/배송/억제/진압봉/온라인의 새 좌표 통합·성능·독립 빌드.

[이동·네 몸체 검사](../../art/cinder-kit-01/site-passage-validation.txt), [운반 검사](../../art/cinder-kit-01/site-carry-edge-validation.json), [보존·검사 기록](../../art/cinder-kit-01/site-checks.json). 전체 문서는 기존 artifacts 링크 누락 142개로 실패하며 새 실패는 없다.

원시 staged 공백 검사는 Unity가 생성한 씬·메시·재질·meta의 빈 필드 후행 공백 30곳으로 실패한다. 코드·문서·근거 공백 검사와 이 생성 후행 공백만 제외한 전체 staged 검사는 통과했다. 공백 검사를 위해 Unity YAML을 수동 수정하지 않았다.

![억제 범위 안의 전체 배치 — 지붕 제거는 검토 촬영용](../../art/cinder-kit-01/site-field-cutaway.png)

[서쪽 지붕 경로](../../art/cinder-kit-01/site-west-covered.png) · [중앙 분기](../../art/cinder-kit-01/site-central-junction.png) · [중앙 운반 골목](../../art/cinder-kit-01/site-central-carry.png) · [지붕 포함 전체 모습](../../art/cinder-kit-01/site-overview.png).

Unity에서 `CinderCompactSiteReview`를 열고 Play 한다. 우주선 경사로 앞에서 시작하며 WASD 이동, 마우스 시야, E 들기, Q 놓기, 빈손 Space 점프다. `Validate Compact Cinder Site Review`로 재검사하고, Play에서 [운반 검사](../../tools/unity_checks/CinderCarryEdgeCheck.cs)를 Unity CLI `run_script`로 실행한 뒤 종료한다. 소품 없는 구조 검사는 `site-`, 소품이 있고 스카이가 없는 씬은 `props-`, 현재 스카이 포함 씬은 `sky-` 접두사를 쓴다. 소품 추가 후 검사는 위 항목을 따른다. 다음은 소품 직접 검토 후 수령 설비와 기능 좌표 통합이다.

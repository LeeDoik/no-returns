# CINDER — 억제 범위 내부의 전체 맵 배치

[English](cinder-compact-site.en.md)

## 2026-10-01 — Cinder 4인 이동·운반 시험

[CINDER-4P-01 사용법·구축·검증](four-player.ko.md#2026-10-01--cinder-4인-맵-테스트-환경). 별도 `CinderFourPlayerTest`에 방장 1명+참가자 3명 이동·공유 화물 E/Q·방장 초기화를 연결했다. 원본 `CinderCompactSiteReview`와 아트/물리 배치를 보존한다. TCP 27842·프로토콜 10·버전 0.9.1, 맥 네 창 실행/종료·재빌드/자동 검사 도구를 제공한다. **맥 빌드 오류 0개·기존 경고 7개, 실제 네 프로세스 자동 검사 13개·기존 구조 규칙 7개·일반 모드 기본값 7개 통과. 800×500 한글 HUD 화면 2개에서 직원 4/4·E/Q 안내·조준점·팀 색을 확인했고 수동 네 프로세스/접속 3개·종료도 확인했다.** 배송/리스너/진압봉/억제/수령 기능과 사람 4인·다른 PC·성능 검증은 남아 있다. 아래 이전 환경 작업의 독립 빌드/온라인 미완료 표기는 이 시험의 이동·운반 범위만으로 대체하며 전체 게임 통합은 미완료다.

## 2026-10-01 — 하늘 개방과 구역 경계 정리

사용자가 맵이 지나치게 미로처럼 느껴진다고 해 서쪽·북쪽 외곽 통로 차양 **82.8㎡**를 열었다. 서쪽 3×14.4m, 북쪽 13.2×3m 구간이다. 차양 두 곳의 원래 천장 BoxCollider 2개를 남은 차양에 맞춰 4개로 나누고 열린 부분의 보이지 않는 천장을 없앴다. 지상 통로 폭 3/3.15m·순환로 4개·건물/우주선/소품·조명은 유지했다. 게임 버전 변경 없음.

53.55×65.4m 억제 범위 안의 포장 바닥과 밖의 거친 광물 지면을 구분하고, 평평한 녹슨 경계 띠와 물리 표지 2개를 더했다. 영문 원문은 `FIELD / INTERIOR`(억제 범위 내부), `OUTER / BASIN`(외곽 분지)이다. 네 모서리 억제기 주변에는 **2.4×2.4m 정비 패드 4개**를 놓고 주변 접근 공간을 유지했다. 경계·패드는 시각 표시이며 새 지상 충돌체나 억제 게임 기능을 추가하지 않는다.

검토에서 일정한 암석 줄과 파란 억제기 직육면체를 발견해 함께 수정했다. 기존 암석 59개의 위치·회전·원거리 봉우리/능선 선택을 고정 seed 137로 불규칙하게 바꾼다. 억제기 원래 Renderer 4개만 숨기고 1×5×1m 충돌 범위 안에 녹슨 기둥·기계함·작은 녹색 신호 외형을 더했다. 신호는 기존 Unlit 재질을 재사용하며 새 Light가 아니다. native 메시 4개(기둥/신호/열린 차양 바닥 묶음/경계·패드), 기존 메시·재질·표지 도구를 재사용한다. 배경 69개·4,624삼각형 유지, 억제기 8배치·864삼각형 및 경계/표지 3배치·48삼각형 추가. 원래 지상 충돌·47소품 묶음·26구조 배치·40로컬 광원·스카이·35–115m 안개·런타임은 유지했다.

검증: 열린 차양 삼각형 제거·위쪽 Raycast로 보이지 않는 천장 없음, 억제기 외형이 기존 충돌 범위 안에 있음, 모든 배경 삼각형이 억제 범위 밖에 있음 확인. 이동 94구간·네 몸체 통로 17곳·유효 위치 286개·운반 자세 6,864개·실제 운반 94구간 통과. 침투 검사 274,484회, 겹침 0건·접촉 103건, E/W/S/Q·빈손 점프 통과. 지면 렌더 차이 71,212픽셀(640×360, 기준 10,000), 메시 4개 재import/씬 재열기 통과. 실제 화면 30개를 촬영해 주요 9개를 검토하고 촬영용 지붕 숨김을 복원했다. 컴파일 오류 0개, 기존 Editor 경고 3종, 새 소스 경고 0개, 최종 Play 오류/경고 0개. 시작 파일 609개 중 현재 씬을 제외한 608개 해시 일치; 기존 선내 재질 변경 8개 제외. 기존 native 씬 ID 삭제 0개.

초기 자동 운반 검사 2회에서 들기/점프 입력 실패가 있었다. 검사 준비에 텔레포트한 몸체·화물의 Physics.SyncTransforms와 안정된 프레임 대기를 추가한 뒤 다시 통과했다. 게임 런타임 조작 코드는 변경하지 않았다. 첫 정비 패드 촬영 위치가 우주선 안에 들어간 것을 바깥 위치로 수정했다. 컴파일 재로딩 중 즉시 콘솔 상태 조회 연결이 한 차례 실패했지만 후속 조회는 컴파일 성공을 확인했다. 과거 `background-`/`architecture-`/`sky-`/`props-`/`site-` 근거를 보존하고 현재 검사는 `polish-`에 기록한다. 문서 검사에는 기존 누락 링크 142개만 남으며 새 실패 0개다. raw staged 공백 검사는 native 생성 후행 공백 74곳으로 실패한다; 코드·문서·근거 및 생성 후행 공백만 제외한 전체 검사는 통과했다.

사용자 외형 품질·경계 가독성·실제 네 명 통과/동시 운반·성능·독립 빌드·외곽 생물/억제 단계·Cinder 플레이 시스템 통합은 검증/구현 대기다. 이번 요청을 전체 품질 승인으로 처리하지 않는다.

현재 씬에는 두 수정 단계가 저장되어 있다. `Polish Cinder Scenery and Suppressor Visuals` → `Open Cinder Sky and Define Zones` 순서로 재현하며, 저장하지 않은 씬/Play/기존 해당 root에서는 중단한다. native 에셋은 [제작 코드](../../NoReturns/Assets/_NoReturns/Editor/CinderBackgroundBuild.cs)로 관리한다. 현재 검사는 `Validate Cinder Exterior Background`를 사용하고 소품·건물·조밀한 맵 검사 메뉴도 현재 배경 검사로 연결한다. 원래 `ArchitectureYard.asset`을 보존하고 차양 삼각형만 제거한 새 메시를 사용한다. 원래 배경 보존 근거와 현재 천장 조정 후 근거를 따로 보존한다. 새 지상 충돌체/광원/텍스처/외부 에셋/의존성/유료 생성 없음. 재검사는 저장 후 Play를 멈춘 상태에서 저장소 루트에서 실행한다.

```bash
source ~/.unity/env
unity command eval --code 'NoReturns.Editor.CinderBackgroundBuild.Validate(); return true;' --project-path "$PWD/NoReturns" --caller plugin --skill unity-cli --format json
unity command run_script --file "$PWD/tools/unity_checks/CinderBackgroundRenderCheck.cs" --entry CinderBackgroundRenderCheck.Main --project-path "$PWD/NoReturns" --caller plugin --skill unity-cli --format json
unity command editor_play --project-path "$PWD/NoReturns" --caller plugin --skill unity-cli --format json
unity command run_script --file "$PWD/tools/unity_checks/CinderCarryEdgeCheck.cs" --entry CinderCarryEdgeCheck.Main --timeout_ms 180000 --project-path "$PWD/NoReturns" --caller plugin --skill unity-cli --format json
unity command editor_stop --project-path "$PWD/NoReturns" --caller plugin --skill unity-cli --format json
```

[배치](../../art/cinder-kit-01/polish-layout-validation.json) · [운반](../../art/cinder-kit-01/polish-carry-edge-validation.json) · [렌더](../../art/cinder-kit-01/polish-render-validation.json) · [보존과 검사 기록](../../art/cinder-kit-01/polish-checks.json).

![열린 북쪽 통로](../../art/cinder-kit-01/polish-open-sky-north.png)

![억제기 정비 패드](../../art/cinder-kit-01/polish-suppressor-pad.png)

2026-10-01 · CINDER-COMPACT-SITE-01 / CINDER-ARCHITECTURE-01 / CINDER-BACKGROUND-01 / CINDER-SCENERY-POLISH-01 · **전체 맵 방향 사용자 승인·다양한 건물·외곽 배경 적용·자동 검사 완료 / 현재 외형·시야와 사람 4인 검토 미완료.** 게임 버전은 바꾸지 않는다.

## 2026-10-01 — 억제 범위 밖 암석 지대와 산업 배경

사용자가 앞선 건물 변경을 확인하고 배경 제작을 요청했다. 같은 `CinderCompactSiteReview`에 [억제 범위 컨셉](../art/space-concepts/cinder-depot-suppression-01.png)의 황량한 외곽을 적용했다. **암석 59개·산업 시설 외형 9개·외곽 지면 1개**, 총 배치 69개·삼각형 4,624개다. 가까운 낮은 암석, 뒤쪽 산봉우리, 북동쪽 사일로·정유 시설·가대를 층으로 배치하고 기존 35–115m 안개로 거리를 표현한다. 배경 품질에 대한 사용자 의견은 대기이며, 앞선 “확인”을 사람 조작·성능·전체 품질 승인으로 확대하지 않는다. 게임 버전은 바꾸지 않는다.

| native 에셋 | 제작 기준·배치 |
|---|---|
| Basalt crag / Split ridge / Distant peak / Loose boulder | 4종 · 암석 59개. 기준 높이 8.5/13/23/2.1m, 불규칙 꼭짓점과 지면 아래 밑부분 포함. 실제 경계는 메시로 검사 |
| Barren basin | 300×280m 외곽 지면 1개. 억제 범위 53.55×65.4m 안쪽은 비우고 기존 바닥 유지 |
| Abandoned silo / Distant refinery / Derelict gantry | 신규 3종 · 사일로 3개·정유 시설 2개·가대 1개 |
| Refinery exhaust | 기존 Industrial stack 메시 재사용 3개 |

신규 native 메시 8개·재질 2개·128×128 RGBA32 광물 텍스처 1개를 [제작 소스](../../NoReturns/Assets/_NoReturns/Editor/CinderBackgroundBuild.cs)로 만든다. 텍스처는 고정 seed로 만든 거친 광물/자갈 무늬이며 Repeat·Point·mipmap, 면 방향에 맞춘 0.35회/m UV를 사용한다. 산업 시설은 기존 노후 아틀라스와 메시 도구를 재사용한다. 배치 스케일 1, 신규 Collider/Light 0개, 배경의 그림자 투사는 꺼 둔다. 기존 회색 경계 4개는 Renderer만 숨기고 추락 방지 Collider는 유지한다. 기존 건물·우주선·소품 47묶음·구조 배치 26개·로컬 광원 40개·하늘/안개·런타임은 보존한다. 외곽은 정적 시각 배경이며 신규 이동 구역·외부 생물·억제 기능을 구현하지 않는다. 기존 억제 선은 시각 표식이고 실제 게임 경계가 아니다.

검증: 모든 배경 삼각형의 수평 경계가 억제 범위와 겹치지 않음, 유한 UV·비퇴화 삼각형·위쪽 지면 방향·스케일/지원 셰이더·기존 상태 보존 통과. 이동 94구간·네 몸체 통로 17곳·몸체 위치 286개·운반 자세 6,864개·실제 운반 94구간 통과. 최종 침투 검사 26,902회·겹침 0건·접촉 103건, E/W/S/Q·빈손 점프 통과. 새 메시 8개 재가져오기·씬 재열기 후에도 실제 지면 렌더 비교에서 640×360 화면의 55,914픽셀이 달라짐을 확인했다. [렌더 회귀 검사](../../tools/unity_checks/CinderBackgroundRenderCheck.cs)는 지면 표시/숨김 차이가 10,000픽셀 미만이면 실패한다. 최종 카메라 25개를 촬영하고 주요 8개를 검토했으며 촬영용 상부 숨김을 복원했다. 컴파일/셰이더 오류 0개·관찰한 기존 Editor 경고 3종·새 소스 경고 0개·최종 Play/렌더 검사 콘솔 오류/경고 0개.

초기 지면 방향 오류를 수정했다. 메시 갱신이 데이터에는 반영되지만 실제 렌더에는 남는 문제를 표시/숨김 화면의 동일 해시로 재현했다. 공용 `Shape.Save(replace=true)`를 `CopySerialized` 대신 기존 Mesh의 Clear/SetVertices/SetUVs/SetTriangles·법선/경계 재계산으로 바꾸어 GUID를 보존하면서 렌더 버퍼도 갱신한다. 재가져오기만으로는 해결되지 않았으며 위 렌더 회귀 검사로 수정 결과를 확인했다. 검사 카메라의 targetTexture 해제 순서 오류도 수정했다. 촬영이 자동 추가한 URP 광원 데이터 41개를 제거하고 재실행 뒤 원래 0개 상태와 저장된 씬을 확인했다. 임시 Python 이미지 비교는 PIL 부재로 실패했고 native 픽셀 비교로 대체했다. 시작 파일 485개 중 현재 씬을 제외한 484개 해시를 유지하며 기존 선내 재질 변경 8개는 커밋에서 제외한다.

![현재 외곽 배경](../../art/cinder-kit-01/background-background-panorama.png)

[배치 검사](../../art/cinder-kit-01/background-layout-validation.json) · [운반 검사](../../art/cinder-kit-01/background-carry-edge-validation.json) · [렌더 검사](../../art/cinder-kit-01/background-render-validation.json) · [검사·보존 기록](../../art/cinder-kit-01/background-checks.json).

### 제작·검사 실행

이미 배경 루트 `Cinder exterior background`가 저장되어 있다. `NO RETURNS/Trials/Add Cinder Exterior Background`는 다른 씬·미저장 편집·Play·기존 배경 루트에서 중단한다. 검사 메뉴는 `Validate Cinder Exterior Background`; 기존 `Validate Cinder Site Props`도 현행 배경 검사로 연결했다. 저장 후 Play를 종료하고 아래 명령을 저장소 루트에서 실행한다. 기존 [공통 운반 검사](../../tools/unity_checks/CinderCarryEdgeCheck.cs)는 Play에서 실행하며 현재 배경이 있으면 `background-`에 검사/촬영을 기록한다. 이전 `architecture-`/`sky-`/`props-`/`site-` 근거를 보존한다.

```bash
source ~/.unity/env
unity command eval --code 'NoReturns.Editor.CinderBackgroundBuild.Validate(); return true;' --project-path "$PWD/NoReturns" --caller plugin --skill unity-cli --format json
unity command run_script --file "$PWD/tools/unity_checks/CinderBackgroundRenderCheck.cs" --entry CinderBackgroundRenderCheck.Main --project-path "$PWD/NoReturns" --caller plugin --skill unity-cli --format json
```

전체 문서 검사는 기존 누락 링크 142개로 실패하며 새 실패는 없다. 원시 staged 공백 검사는 Unity 생성 파일의 후행 공백 163곳으로 실패한다. 코드/문서/근거 검사와 이 생성 후행 공백만 제외한 전체 검사는 통과한다. 사용자 배경 밀도/반복 무늬/시설 시야·최종 위험 표식 가독성·사람 4인·성능·독립 빌드·외곽 게임 기능·Cinder 기능 통합은 미확인이다. 아래 구조/스카이/소품 수치는 이전 단계의 근거이며 현행 검사 접두사는 `background-`다.

## 2026-10-01 — 다양한 건물 윤곽과 구조 모듈

사용자는 직사각형 벽 중심인 현재 맵을 컨셉 사진처럼 다양한 건물 형태로 구성해 달라고 요청했다. 같은 `CinderCompactSiteReview`에 **구조 모듈 9종**을 제작·배치했다. [억제 범위 컨셉](../art/space-concepts/cinder-depot-suppression-01.png)과 [상면 컨셉](../art/space-concepts/cinder-depot-overhead-01.png)의 잘린 모서리·꺾인 윤곽·단차를 기준으로 삼았다. 중앙 Sorting island는 모서리를 자른 8각형, North control annex는 한쪽 모서리가 들어간 6꼭짓점 계단형 윤곽으로 실제 지상 외형과 충돌체를 바꿨다. 주요 건물 5개의 실내 바닥·문·벽 충돌과 우주선은 유지하고, 건물별 상부 실루엣과 입구 깊이를 추가했다. 게임 버전은 바꾸지 않는다.

| 모듈 | 크기 X×Y×Z (m) | 배치 수·역할 |
|---|---|---|
| Chamfer utility | 6.3×4.3×7.2 | 1 · 중앙 8각 설비동 |
| Stepped annex | 6.3×4.3×17.1 | 1 · 북쪽 계단형 부속동 |
| Sawtooth roof | 7.2×1.5×6.8 | 6 · 창고 톱니형 지붕 |
| Vault roof | 7.2×1.9×7.2 | 4 · 보관동 아치 지붕 |
| L upper annex | 8×2.1×6 | 1 · 사무동 L자 상부실 |
| Octagonal control tower | 4.8×3.4×4.8 | 1 · BAY 04 팔각 관제실 |
| Raised plant room | 3.9×2.2×5.4 | 3 · 북쪽/설비동의 높은 설비실 |
| Entry hood | 4.6×0.79×1.2 | 6 · 입구 차양, 최저 높이 3.36m |
| Industrial stack | 1.66×4×1.66 | 2 · 설비동 산업용 배기탑 |

모듈 외에 바닥 보충 메시와 기존 야외 바닥/유지 지붕에서 교체 대상 지붕만 제외한 복사본을 더해 **native 메시 에셋 11개**다. 새 루트 `Cinder varied architecture`는 바닥 보충 1개를 포함해 배치 26개·삼각형 4,021개·정적 MeshCollider 25개다. 야외 표면 복사본은 기존 표면 오브젝트가 별도로 사용한다. 최고 높이 8.3m, 배치 스케일 1. 기존 2개 부속동 그룹은 씬에 비활성 상태로 보존하며, 직사각형 충돌체를 새 다각형의 정적 MeshCollider로 교체했다. 새 모서리/후퇴부에는 바닥을 채웠고, 소품 묶음 47개·물리적 표지 6개·로컬 광원 40개·스카이/안개는 유지했다. 북쪽 상부 설비실은 기존 지붕 장비와 겹치지 않게 배치했다.

[제작·배치·검사 코드](../../NoReturns/Assets/_NoReturns/Editor/CinderArchitectureBuild.cs), [재사용 메시 도구](../../NoReturns/Assets/_NoReturns/Editor/CinderSitePropsBuild.cs), [native 에셋 폴더](../../NoReturns/Assets/_NoReturns/Art/CinderArchitecture01/), [실측](../../art/cinder-kit-01/architecture-layout-validation.json). 기존 노후 아틀라스와 재질을 재사용한다. 새 텍스처·외부 모델·의존성·셰이더·유료 생성은 없다. 다각형 상단을 1.2m 타일로 잘라 무늬가 큰 삼각형으로 늘어나는 문제를 수정했고, Chamfer utility 상단 140개 삼각형의 UV 밀도 계산을 통과했다. 새 메시 11개 재가져오기와 저장 씬 재열기 후 검사도 통과했다. native 에셋은 Unity API로 저장하며 YAML을 수동 편집하지 않는다.

이미 현재 씬에 적용되어 있다. 저장·Play 종료 후 `NO RETURNS/Trials/Validate Varied Cinder Architecture`로 검사한다. `Validate Cinder Site Props`도 현재 구조 검사로 연결되며 과거 props- 근거를 덮어쓰지 않는다. Play에서는 [공유 운반 검사](../../tools/unity_checks/CinderCarryEdgeCheck.cs)를 Unity CLI `run_script`로 실행한다. 현재 결과/화면은 `architecture-`, 이전 `sky-`·`props-`·`site-`는 보존한다. `Add Varied Cinder Architecture`는 다른 씬·미저장·Play·기존 구조 루트에서 중단한다. 현재 배치를 재생성할 때는 수동 변경을 먼저 보존한다. 스카이 재적용은 현재 유효 위치 수를 기준으로 보존 검사를 하며 이전 고정값 246개를 요구하지 않는다.

검증: UV 범위·비퇴화 삼각형·정적 충돌 메시의 양수 방향 체적·스케일 확인. 기존 지상 이동 94구간·네 몸체 통로 17곳·3/3.15m 최소 폭·4개 순환로·두 배송 경로를 유지했다. 변경한 모서리/후퇴부 표본을 포함해 몸체 위치 286개×24방향/시선 = 운반 자세 6,864개. Mac Editor Play 실제 운반 94구간·침투 검사 340,268회에서 겹침 0건·접촉 103건, 허용치 0.00001m. E/W/S/Q·빈손 점프 통과. API 자세·실제 Update·키 이벤트 검사이며 사람 실시간 조작은 아니다. 실제 카메라 21개를 촬영하고 주요 8개 시점을 검토했다. 촬영용 지붕/상부실 숨김은 복원했다. 컴파일/스카이 셰이더 오류 0개·이번 Editor 재컴파일에서 기존 obsolete 경고 3종·새 소스 경고 0개·최종 Play 오류/경고 0개. 시작 파일 435개 중 현재 씬을 제외한 434개 해시 일치, 기존 선내 재질 변경 8개는 커밋 제외.

재검사에서 같은 이름의 광원들이 Play 종료 후 다른 순서로 열거되어 보존 검사 1건이 실패했다. 모든 원래 기록의 값이 같음을 집합으로 확인하고 경로+위치의 ordinal 순서로 정렬해 오탐을 수정했다. Python/native 정렬 순서 차이로 1건 추가 재시도한 뒤 최종 재열기 검사 통과. 실제 광원/충돌 변경은 없었다. 최초 CLI timeout 옵션 표기는 `--timeout_ms`로 수정했으며 게임 컴파일 오류가 아니다.

[이동·네 몸체 검사](../../art/cinder-kit-01/architecture-passage-validation.txt) · [운반 검사](../../art/cinder-kit-01/architecture-carry-edge-validation.json) · [보존·검사 기록](../../art/cinder-kit-01/architecture-checks.json). 전체 문서 검사는 기존 누락 링크 142개로 실패하며 새 실패 없음. 건물 형태·차양 시야·길찾기·운반 회전의 사용자 품질, 사람 4인, 성능·독립 빌드·배경 품질·Cinder 게임 기능 연결은 미검증이다. 상부실·배기탑·차양은 정적 환경 에셋이며 새 층 진입/계단/상호작용을 구현하지 않았다. 아래 스카이·소품·구조 수치는 이전 단계 기록이다.

![다양한 건물 실루엣](../../art/cinder-kit-01/architecture-exterior-machinery.png)

[모서리를 자른 설비동](../../art/cinder-kit-01/architecture-sorting-chamfer.png) · [계단형 윤곽](../../art/cinder-kit-01/architecture-annex-step.png) · [입구 차양](../../art/cinder-kit-01/architecture-office-link.png) · [현재 지상 윤곽 상면](../../art/cinder-kit-01/architecture-field-cutaway.png).

원시 staged 공백 검사는 Unity 생성 메시·meta의 빈 필드 후행 공백 201곳으로 실패한다. 코드·문서·근거 공백 검사와 이 생성 후행 공백만 제외한 전체 staged 검사는 통과했다. PNG LFS 포인터 21개와 `git lfs fsck --pointers` 확인 완료. 공백 검사만을 위한 native YAML 수동 편집은 하지 않았다.

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

현재 씬에 이미 배치했다. `NO RETURNS/Trials/Add Cinder Site Props`는 Play 중·미저장·다른 씬·기존 소품군이 있으면 중단하여 수동 편집을 보호한다. `Validate Cinder Site Props`로 검사하고 Play에서 [공통 운반 검사](../../tools/unity_checks/CinderCarryEdgeCheck.cs)를 실행한다. 스카이 없는 소품 씬은 `props-`, 현재 스카이 포함 씬은 `sky-` 결과를 만든다. 기존 `site-` 근거는 덮어쓰지 않는다. 다음은 소품 밀도/운반 시야의 직접 평가, BAY 04 수령 시설·기능 좌표 통합이다. 사람 4인·AI/배송/억제/진압봉/온라인·배경 품질·성능·독립 빌드는 미검증이다.

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

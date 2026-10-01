# CINDER DEPOT — 첫 에셋 제작 준비

[English](cinder-asset-prep.en.md)

2026-10-01 · CINDER-ASSET-PREP-01 · **노후 창고 질감 사용자 승인·건물 5개 공통 외형 확장 및 자동 검사 완료 / 확장 결과의 사용자 검토 미완료.** [현재 맵 범위·검토 씬·다음 제작](cinder-map-appearance.ko.md)을 따른다. 아래는 첫 창고 묶음의 규격·제작·검증 기록이다.

## 먼저 만들 것과 이유

**창고 남측 입구와 입구 안쪽 6m를 첫 검토 구역으로 사용한다.** 반복되는 벽·바닥·천장과 출입구를 함께 볼 수 있어, 한 부품군을 나머지 건물에 넓히기 전에 접합·색·운반 여유를 확인할 수 있다. 현재 창고 내부에는 별도 복도가 없다. 6m는 검토할 길이 제안이며 새 복도 벽을 세우거나 기존 경로를 좁히는 결정이 아니다.

1. **구조 5종:** 바닥 → 직선 벽 → 문틀 → 천장·보 → 코너·끝 마감. 출입구와 공간의 공통 단면을 먼저 맞춘다.
2. **표현 3종:** 작업등 → 표지판 → 빈 화물 선반. 출입구 식별과 사용 흔적을 더하되 선반은 문 앞을 비운다.
3. 기준 구역의 구조 플레이와 외형을 검토한 뒤 건물 5개로 확장한다. 다음 기능 에셋은 수령 단말기·수령 바닥 표시, 억제장치·경고 신호 순이다. 직원·손·리스너·외곽 생물의 리그 제작은 별도 준비 과제로 유지한다.

첫 묶음은 **8개 제작 단위**다. 코너/끝 마감과 천장/보의 변형을 포함하므로 FBX 수·메시 수·배치 수와 같지 않다. [과거 44개 데모 목록](demo-art-list.ko.md)의 R 재사용 표기는 현재 Cinder의 완성 에셋 상태가 아니다. 이 묶음은 새로 제작할 후보이며 기존 Selected 모델은 외형·결함 참고로만 둔다.

## 현재 구조에서 가져올 기준

근거: [저장된 씬](../../NoReturns/Assets/_NoReturns/Scenes/CinderDepotBlockout.unity), [생성 코드](../../NoReturns/Assets/_NoReturns/Editor/CinderBlockoutBuild.cs), [이동·운반 코드](../../NoReturns/Assets/_NoReturns/Runtime/CinderBlockoutWalk.cs). 씬 파일의 창고 하위 Transform과 생성 코드 값을 대조했다. 준비 단계에는 정적 대조만 했으며, 아래 제작 검증에서 별도 검토 씬의 Mac Editor Play를 확인했다.

| 항목 | 현재 시험값 | 제작 시 보존할 것 |
|---|---|---|
| 창고 바닥 | 14.4×20.4m, 상면 Y=0m, 두께 0.2m | 마감이 새 문턱·단차를 만들지 않게 한다. |
| 창고 벽 | 높이 4m, 두께 0.3m | 기존 충돌 영역과 조립 중심선을 유지한다. |
| 남측 입구 중심 | Unity (X,Y,Z)=(-18.6,0,-8.4)m | +Z가 창고 안쪽이다. 위치는 현재 씬 기준이다. |
| 출입구 | 유효 폭 3.2m, 높이 3.3m | 장식·모서리가 개구부 안으로 침범하지 않는다. |
| 천장 | 하단 Y=4m, 지붕 두께 0.3m | 새 보가 머리 위 여유를 줄이지 않게 한다. |
| 직원 | 높이 1.8m, 반경 0.34m | 실제 CharacterController를 구조 검사의 기준으로 쓴다. |
| 시점 | 눈높이 1.57m, FOV 80° | 빈손/화물 운반 시점을 모두 비교한다. |
| 운반 화물 | 0.8×0.65×0.65m | 시점 앞 운반·회전·벽 접근을 실제 조작으로 확인한다. |

2026-10-01 사용자 의견: “창고 확인했어. 뭐 이정도 크기면 괜찮아 보이네”. 창고 크기에 대한 긍정 의견으로 기록하고 현재 바닥 14.4×20.4m를 유지한다. 코드·씬·에셋 치수 변경은 없다. 운반 시야·가장자리 회전·벽 접근·접합면 품질이나 새 외형까지 승인된 것으로 확대하지 않는다.

문·천장·직원·화물 값은 기존 시험값이지 사용자 승인된 최종 치수가 아니다. 자동 통로 검사 41개의 과거 통과를 새 외형·화물 회전·협동 검증으로 승계하지 않는다.

## 부품별 제작 지시

단위는 m, 아래 크기는 **Unity X×Y×Z** 기준이다. Blender는 Z-up, Unity는 Y-up이므로 내보내기/재가져오기에서 축·회전·단위를 확인한다. [Blender 제작 스크립트](../../art/cinder-kit-01/build.py)로 회색 구조 5종과 변형 11개를 만들었다. 표의 이름과 시험 치수를 사용한다. 승인 후 [외형 제작 스크립트](../../art/cinder-kit-01/build_appearance.py)로 공통 표면과 작업등·표지판·빈 2단 선반까지 제작했다.

| 순서 / 대응 ID | 원본 이름 | 시험 크기·피벗 | 형태와 통과 조건 | 삼각형 상한 후보 |
|---|---|---|---|---|
| 1 / ENV04 | `NR_Cinder_Floor_A` | 1.2×0.2×1.2. 피벗은 보행 상면 중앙, 형상은 Y=-0.2~0. | 평평한 상면. 5장을 이어 6m 반복 구간에서 이음새·무늬 크기를 확인한다. | 200 |
| 2 / ENV01 | `NR_Cinder_Wall_A` | 1.2×4×0.3. 하단 중심 피벗. | 앞·뒤·상단을 닫는다. 접합면은 평평하게, 색 띠 높이와 UV 밀도는 공통으로 유지한다. | 400 |
| 3 / ENV03 | `NR_Cinder_DoorFrame_A` | 바깥 3.8×4×0.3, 안쪽 개구부 3.2×3.3. 개구부 바닥 중앙 피벗. | 문턱 없는 프레임. 양측 0.3m와 상부 0.7m 안에서 장식을 만든다. 문짝·개폐 기능은 별도다. | 1,200 |
| 4 / ENV07 | `NR_Cinder_Ceiling_A` | 1.2×0.3×1.2. 하부 면 중앙 피벗, 배치 Y=4. | 하부를 마감한다. 보 변형은 기존 지붕 두께 안에서 먼저 검토하며 하단 Y=4를 보존한다. | 600 |
| 5 / ENV02·05 | `NR_Cinder_Corner_A` / `NR_Cinder_End_A` | 두께 0.3, 높이 4. 코너는 벽 중심선 교차점 하단 피벗. | 0.3×4×0.3 코너 접합부와 0.15×4×0.3 끝 마감을 닫는다. 프레임과 코너 폭을 제외한 전후면 벽은 5.15m=4×1.2+0.35, 측면 벽은 20.1m=16×1.2+0.9로 조립한다. UV를 늘이는 배치 스케일은 쓰지 않는다. | 600 |
| 6 / FAC01 | `NR_Cinder_Lamp_A` | 0.6×0.2×0.18. 벽 부착면 중앙 피벗. | 따뜻한 발광 면과 외장을 분리한다. 실제 Light는 Unity가 소유한다. | 400 |
| 7 / 새 표면 단위 | `NR_Cinder_Sign_A` | 1.2×0.6×0.02. 뒤 부착면 중앙 피벗. | 판과 글자 면을 분리한다. `WAREHOUSE` / 창고와 배송 방향 화살표를 검토용 문구로 사용한다. | 12 |
| 8 / FAC02 | `NR_Cinder_Rack_A` | 2.4×2.4×0.6. 하단 중심 피벗. | 비어 있는 2단 선반. 상자는 별도이며 배송 화물 외형을 장식에 굳히지 않는다. 통로 밖에 배치한다. | 1,600 |

1.2m 그리드는 창고 바닥 14.4/20.4m에 맞는 첫 모듈 제안이다. 모든 건물이나 3.2m 개구부가 이 그리드에 맞는다고 가정하지 않는다. 코너·끝·보 단면은 아래 구현값을 사용하며 표지 방향은 후속 검토다. 삼각형 상한은 **원본/변형 1개당 작업 예산 제안**이며 성능 보장이 아니다. 현재 실측은 문틀 36개, 나머지 원본은 각 12개 삼각형이다. 첫 샘플에서 실루엣이 부족하면 근거를 기록하고 조정한다.

## 공통 표면과 외형 참고

[환경 시트](../art/space-concepts/environment-kit-01.png)와 [시트 검토 기록](../art/space-concepts/environment-kit-01.ko.md)을 스타일 참고로 사용한다. 상아색 패널·적갈색 띠·어두운 프레임·따뜻한 작업등을 맞추고, 노후 질감 요청에 따라 넓은 면에도 변색·녹물·벗겨진 도장을 보이게 하고, 이음새·하단·접촉부에 때와 녹을 더 모은다. 출입구와 화물 윤곽이 먼저 읽혀야 한다. 이 참고 선택은 신규 외형 승인과 다르다.

첫 비교용 표면 예산은 공용 **512×512 BaseColor 1장**, 별도 **256×128 표지 텍스처 1장**, 작업등의 단색 발광 면이다. Point 필터를 출발 후보로 두고 mipmap·원거리 깜빡임·글자 가독성을 실제 시점에서 비교한다. 구조 표면은 공용 재질, 발광 면은 별도 재질로 구성하며 큰 단순 면과 각진 실루엣을 유지한다. 텍스처 크기·픽셀 밀도·발광 강도는 시험 제안이며 최종 규격은 첫 구역 화면 검토 후 정한다.

첫 8종은 **로컬 Blender 직접 제작**으로 준비한다. Tripo 호출과 유료 생성은 이 묶음의 필수 단계가 아니다. 새 구조를 Unity에서 확인하고 사용자 구조 의견을 반영한 뒤, 같은 모델의 정면·후면·측면·윗면·1인칭 화면으로 새 외형을 검토한다. 치수는 모델에서 검사하며 이미지의 글자나 비율로 검증하지 않는다.

## 산출물과 저장 위치

다음 회색 원본·검토 씬은 보존한다. 외형·텍스처·표현 3종의 별도 산출물은 아래 최신 적용 기록에 연결한다.

- `art/cinder-kit-01/`: `build.py`, 구조 원본 `Cinder_Kit_Structure.blend`, UV·재가져오기·치수·Play 검사 결과, 같은 모델의 검토 이미지. 부품별 FBX는 아래 Unity 에셋 경로에 직접 출력한다. 기존 우주선·Selected 원본을 덮어쓰지 않는다.
- `NoReturns/Assets/_NoReturns/Art/CinderKit01/`: 회색 구조 FBX 11개·단색 공용 재질 1개와 Unity가 생성한 `.meta`. 텍스처는 없다.
- `NoReturns/Assets/_NoReturns/Prefabs/CinderKit01/`: 모델 외형을 하위에 둔 검토용 프리팹 11개. 이번 프리팹은 시각 모델만 소유하며 별도 검토 씬이 기존 창고 Collider 10개를 유지한다. 새 Collider·광원·표시 기능을 추가하지 않았다. Editor/CLI로 조립하며 YAML을 직접 수정하지 않는다.
- 원본 이름별로 치수·피벗·삼각형·UV·재질 슬롯·출처·검사 결과·사용자 의견을 연결한다. 로컬 신규 형상·텍스처는 직접 제작 출처를 기록한다. 외부 파일을 도입할 때는 사용 범위와 라이선스를 먼저 확인한다. 기존 시안은 참고 자료다.
- 모델·이미지·텍스처·`.blend`는 기존 `.gitattributes`의 Git LFS, 제작 코드·JSON·한영 문서는 일반 Git으로 관리한다. 빌드·로그·캐시·인증정보·개인 설정은 제외한다.

현재 도구 확인: 로컬 `blender --version`은 **5.2.2 LTS**다. Unity 목표 **6000.6.0f1 / URP 17.6.0**은 [버전 파일](../../NoReturns/ProjectSettings/ProjectVersion.txt)과 [패키지](../../NoReturns/Packages/manifest.json) 기준이다. 현재 PATH에 `game-dev`가 없어 해당 CLI의 검사·정규화·패키지 제작 경로는 사용할 수 없다. 이번 준비에는 기존 로컬 제작 경로를 사용하며 새 설치·생성 서비스로 대체하지 않았다. Blender 모델 제작·FBX 재가져오기·별도 구조 검토 씬의 Mac Editor Play를 확인했다. 독립 Mac/Windows 실행본 빌드는 검사하지 않았다.

## 다음 작업과 통과 조건

첫 창고 묶음의 **구조 5종 회색 원본과 조립 샘플**, 남측 입구·안쪽 6m 검토 지점은 보존한다. 이후 노후 질감 승인에 따라 [건물 5개 공통 외형](cinder-map-appearance.ko.md)을 별도 맵 씬으로 확장했다. 다음은 BAY 04 수령 단말기·바닥 표시 제작과 확장 결과의 사용자 검토다. 기존 Create/Build Cinder 메뉴는 씬을 재생성하므로 수동 아트 검토 씬에서 실행하지 않는다.

| 검토 시점 | 확인할 것 |
|---|---|
| 남측 입구 3m 전방, 눈높이 1.57m | 출입구가 읽히는지, 표지·광원이 동선을 설명하는지 |
| 입구를 화물 들고 통과, 좌우·상하 회전 | 프레임·천장·모서리가 0.8×0.65×0.65m 화물·시야를 침범하는지 |
| 창고 안쪽 6m에서 입구로 되돌아보기 | 벽 후면·천장 하부·끝 마감·바닥 반복 이음새 |
| 선반과 등 추가 후 같은 시점 반복 | 보관 구역과 운반 경로가 구분되고, 밝은 면이 화물 윤곽을 가리지 않는지 |

- [x] 현재 씬·생성/조작 코드·환경 시안과 제작 가이드를 대조했다.
- [x] 우선 8종, 구조 선행 5종의 이름·규격 후보·피벗·표면 예산·검토 위치를 정리했다.
- [x] 구조 원본/FBX 11개의 단위·축·치수·개구부·피벗·UV·닫힌 면·법선·재가져오기를 검사했다.
- [x] 재가져오기에서 크기·재질 슬롯을 확인했다. 원래 창고 Renderer 10개는 비활성화하고 Collider 10개는 유지했다. 새 조립물 Collider는 0개다.
- [x] Unity의 자동 통과/점프·화물 자세 검사와 Mac Editor Play의 E/Q·운반 출입/후진·빈손 점프를 확인했다.
- [x] 사용자가 창고 크기를 확인하고 괜찮다는 의견을 주었다. 현재 14.4×20.4m를 유지한다.
- [ ] 사용자의 화물 시야·가장자리 운반 회전·벽 접근·접합면 품질 의견을 확인한다.
- [x] 실제 구조를 참고한 입구 외형 예상도와 8종 다면 부품 시트를 생성·검토하고 보관했다.
- [x] 사용자가 새 외형 시안의 색·질감·작업등·표지·선반 방향을 승인했다.
- [x] 공통 표면과 표현 3종을 적용하고 빈손·운반 화면 및 자동 검사를 반복했다.
- [x] 사용자가 노후 질감을 맵 기준으로 승인한 뒤 건물 5개 공통 외형을 확장했다. 확장 결과의 사용자 검토는 남아 있다.

온라인·적 AI·배송 판정은 현 Cinder 시험에 미연결이다. 이 구조 구역의 통과로 전체 게임·협동·재미·출시 품질을 완료 처리하지 않는다.

## 2026-10-01 — 운반 가장자리 수정과 외형 시안

[입구 예상도·8종 부품 시트](../art/cinder-appearance-01.ko.md)를 제작·검토했다. 내장 imagegen의 실제 프롬프트와 출처·해시를 연결했으며 시안 생성 당시에는 사용자 승인과 실제 적용 전이었다. 이후 승인·제작 결과는 아래 최신 적용 기록을 따른다. 생성 이미지의 원근·개구부 비율을 실제 치수 검증으로 사용하지 않는다.

운반 검사를 14개 위치×8개 yaw×3개 pitch로 넓혔다. [수정 전 결과](../../art/cinder-kit-01/carry-edge-before.json)는 336개 중 38개에서 문 가장자리·벽 겹침을 보였다. 원인은 겹친 눈 위치에서 BoxCast를 시작하고 최소 0.15m 이동을 강제한 기존 처리였다. [공유 운반 위치 계산](../../NoReturns/Assets/_NoReturns/Runtime/CinderBlockoutWalk.cs)의 `TrialCargoPose.Position`은 화물 높이의 시작점을 최대 4회 분리한 뒤 장애물까지 이동한다. 계산 동안 조회 형상을 활성화하고 레이어 2로 제외하며 원래 Collider·레이어 상태를 `finally`에서 복구한다. 같은 기존 처리를 사용하던 [선내 구조 시험](../../NoReturns/Assets/_NoReturns/Runtime/ShipInteriorTrial.cs)과 [구조 검사](../../NoReturns/Assets/_NoReturns/Editor/CinderStructureBuild.cs)도 이 계산을 사용한다. 실제 운반 위치 검사에서 복사한 옛 수식을 제거했다.

[최종 Play 검사](../../art/cinder-kit-01/carry-edge-validation.json): 336개 겹침 0개, E 들기·W 출입·S 후진·Q 놓기·빈손 Space 점프 통과. [재실행 가능한 검사](../../tools/unity_checks/CinderCarryEdgeCheck.cs)는 키 상태를 Input System에 넣고 실제 `Update`를 호출하며 자세는 API로 정한다. 사람의 실시간 수동 조작이 아니다. 기존 모델 11개·전후 통과 6개·점프 위치 3개·중앙 화물 자세 48개 검사도 재통과했다. [컴파일·콘솔 확인](../../art/cinder-kit-01/appearance-checks.json): 컴파일 오류와 콘솔 오류 0개, 기존 폐기 예정 경고는 남아 있다.

검사에서 출력한 실제 회색 화면: [문 가장자리 운반](../../art/cinder-kit-01/carry-edge-door.png) · [벽 바로 앞 운반](../../art/cinder-kit-01/carry-edge-wall.png). 이음새와 화물 부분 노출을 확인했다. 벽에 바짝 붙으면 화물이 화면 하단 밖으로 내려가므로 이 근접 시야의 사용자 평가는 남아 있다. 저장된 창고·선내 씬과 기존 재질 8개는 바꾸지 않았다. 선내 씬 Play 재검증·독립 Mac/Windows 빌드·성능·온라인/AI/배송은 이번 범위에서 미검증이다.

재검사: `CinderStructureReview`에서 Play를 시작한 뒤 프로젝트 루트에서 `unity command run_script --project-path NoReturns --file ../tools/unity_checks/CinderCarryEdgeCheck.cs --caller plugin --skill unity-cli`를 실행한다. 상대 `--file`은 Unity 프로젝트 `NoReturns/` 기준이며 검사 후 Play를 중지한다. 승인 후 공용 표면·표현 3종을 제작했으며 별도 외형 씬의 재검사는 아래를 따른다.

## 기존 플레이 시스템의 위치

| 씬/코드 | 현재 역할 |
|---|---|
| [CarryRoom](../../NoReturns/Assets/_NoReturns/Scenes/CarryRoom.unity) / [CarryRoom 코드](../../NoReturns/Assets/_NoReturns/Runtime/CarryLab/CarryRoom.cs) | 기존 운반·배송·리스너·진압봉·협동 플레이. 리스너 모드는 기존 Windows [실행 런처](../../06_Play_Listener_Test.cmd)의 `--hazard`로 활성화한다. 일반 Editor Play는 기본 운반 모드다. |
| [CarryThreat](../../NoReturns/Assets/_NoReturns/Runtime/CarryLab/CarryThreat.cs) / [BatonVisual](../../NoReturns/Assets/_NoReturns/Runtime/CarryLab/BatonVisual.cs) / [BatonFeedback](../../NoReturns/Assets/_NoReturns/Runtime/CarryLab/BatonFeedback.cs) | 리스너 이동·추적·공격·저지와 진압봉 외형·충전·피드백 코드가 보존되어 있다. |
| [CarryMission](../../NoReturns/Assets/_NoReturns/Runtime/CarryLab/CarryMission.cs) / [ShipInteriorTrial](../../NoReturns/Assets/_NoReturns/Scenes/ShipInteriorTrial.unity) | 기존 우주선 항로·귀환·정산 로직과 별도 선내 구조 시험. 선내 시험 모델은 Cinder 배치에도 남아 있다. |
| `CinderDepotBlockout` / `CinderStructureReview` | 맵 공간 시험 / 새 회색 에셋 검토. 이전 `CarryRoom`의 적·진압봉·배송 로직은 아직 연결하지 않았다. |

이번 변경은 기존 런타임 코드를 삭제·교체하지 않는다. 리스너의 경로 격자와 우주선/배송 판정 좌표는 기존 시험 맵 기준이므로 Cinder에 연결하는 작업이 별도로 남아 있다. 단순히 에셋 씬을 열거나 `CarryRoom` 컴포넌트를 추가했다고 통합 완료로 보지 않는다. 기존 리스너 모드의 Mac 실행은 이번에 재검사하지 않았다.

## 2026-10-01 — 회색 구조 제작과 검토 방법

[검토 씬](../../NoReturns/Assets/_NoReturns/Scenes/CinderStructureReview.unity)을 열고 Play를 누른다. 남측 입구 3m 전방에서 시작하며 앞 오른쪽에 화물이 있다. WASD 이동, 마우스 시야, E 들기, Q 놓기, 빈손 Space 점프, F1 한영 전환, Esc 커서 해제다. 입구를 통과해 6m 들어간 뒤 돌아보거나 화물을 들고 후진한다. 원본 `CinderDepotBlockout`은 바이트 해시가 작업 시작과 같고, 시작 전 우주선 재질 변경 8개도 보존했다.

[Unity 생성/검사 코드](../../NoReturns/Assets/_NoReturns/Editor/CinderStructureBuild.cs)의 `NO RETURNS/Trials/Create Cinder Structure Review` 메뉴는 현재 씬을 저장하고 Play를 중지한 뒤 실행한다. 이 메뉴는 **검토 씬을 재생성**하므로 수동 변경은 먼저 별도 사본에 보관한다. `Validate Cinder Structure Review` 메뉴로 단위/피벗/UV/재질·통로·점프·화물 자세 검사를 다시 실행할 수 있다. Blender 재생성: `blender --background --python art/cinder-kit-01/build.py`.

5개 제작 단위를 **11개 원본/FBX/프리팹**으로 구현했다: Floor_A, Wall_A, Wall_Fill035, Wall_Fill090, DoorFrame_A, Ceiling_A, Ceiling_Edge, Ceiling_Corner, Beam_A, Corner_A, End_A. 코너는 벽 중심선 교차점의 0.3m 사각 접합부, 끝 마감은 폭 0.15m, 보는 1.2×0.3×0.3m다. 보·끝 마감은 원본/프리팹만 만들었으며 이번 닫힌 창고에는 배치하지 않았다. 천장 가장자리 0.15m 보완형으로 기존 지붕 외곽 14.7×20.7m까지 닫는다. 모든 조립 인스턴스는 배치 스케일 1이다.

[Blender 검사 결과](../../art/cinder-kit-01/validation.json): 11개 치수·피벗·UV·닫힌 메시·양의 체적과 FBX 재가져오기·삼각형/재질 슬롯 일치 통과. 문틀 36개, 나머지는 각 12개 삼각형이다. [Unity 조립 실측](../../art/cinder-kit-01/unity-assembly.json): 시각 인스턴스 530개, 6,408개 삼각형, 기존 창고 Collider 10개 유지, 기존 활성 Renderer 0개, 새 Collider 0개. 이는 메시 수치이며 FPS/최종 성능 검증이 아니다.

[Unity 구조 검사](../../art/cinder-kit-01/unity-validation.txt): 모델 11개, 3개 진입선의 전진/후진 6개, 빈손 점프 위치 3개, 중앙 진입선의 화물 자세 48개 통과. 화물 자세는 4개 위치×4개 yaw×3개 pitch(-80/0/80°)에서 실제 운반 BoxCast 위치를 검사한다. 가장자리 전체 회전·벽 접근의 사용자 조작 검증으로 확대 해석하지 않는다. 첫 통로 검사는 바닥에 놓인 화물 Collider에 막혔으며, 실제 운반처럼 그 Collider를 검사 동안 끄고 복구하도록 수정한 뒤 재생성·재검사를 통과했다.

[Mac Editor Play 검사](../../art/cinder-kit-01/play-validation.json): Input System 키 상태를 API로 넣어 실제 Update의 E 들기, W 운반 출입, S 후진 복귀, Q 놓기, 빈손 Space 점프를 확인했다. 점프 상승 0.634135962m. 기준 카메라 위치·각도는 API로 맞췄으며 사람의 수동 조작 평가가 아니다. 컴파일 오류 0개, 기존 폐기 예정 API 경고 7개, 새 Play 구간 오류/경고 0개. 독립 실행본·온라인·AI·배송·최종 외형·사용자 승인은 미검증이다.

검토 이미지: [동일 원본 부품 시트](../../art/cinder-kit-01/structure-sheet.png), [입구 빈손](../../art/cinder-kit-01/entry-empty.png), [입구 운반](../../art/cinder-kit-01/entry-carry.png), [안쪽 6m에서 뒤돌아본 운반 화면](../../art/cinder-kit-01/inside-rear-carry.png). Unity 이미지는 실제 Play 카메라 렌더이며 IMGUI 안내는 포함하지 않는다. 단색 회색 구조이고 최종 아트 시안은 아니다.

한영 링크·수치·이름·예산·체크 상태와 Git LFS를 검사한다. 전체 문서 검사에는 작업 시작 전부터 로컬에 없는 `artifacts/` 근거 링크 **142개**가 있으며 새 실패는 없다. 빌드·로그·캐시·개인 설정은 커밋하지 않는다. [검증 목록](05-validation.ko.md) · [Git 운영 기준](version-control.ko.md).

원본을 직접 수정하지 않은 Unity 생성 YAML·meta의 빈 필드 후행 공백 때문에 원시 staged `git diff --check`는 147곳을 보고한다. 코드·문서 범위 공백 검사는 통과하며, 후행 공백만 제외한 전체 staged 검사도 통과한다. 생성 파일을 공백 검사만을 위해 수동 편집하지 않았다.

## 2026-10-01 — 승인된 외형의 실제 적용

사용자가 “어 이 느낌으로 가자”라고 [외형 시안](../art/cinder-appearance-01.ko.md)을 승인했다. [승인 출처](../../art/cinder-kit-01/appearance-provenance.json)에 기록했다. 이는 시안의 색·질감·등·표지·선반 방향 승인이다. 실제 적용 화면의 사용자 품질 평가는 별도로 남긴다.

[외형 검토 씬](../../NoReturns/Assets/_NoReturns/Scenes/CinderAppearanceReview.unity)은 회색 씬에서 파생한 별도 사본이다. 창고 크기 14.4×20.4m, 개구부 3.2×3.3m, 천장 하단 4m와 기존 창고 Collider 10개를 유지한다. 창고 전체 530개 시각 인스턴스의 표면을 맞추고 남측 입구와 안쪽 6m에 표현 3종을 배치했다. 다른 건물·새 복도·게임 시스템은 확장하지 않았다. 벽을 뚫고 보이던 기존 블록아웃 TextMesh 12개의 Renderer만 외형 씬에서 숨겼다.

- 원본·재생성: [Blender 제작 코드](../../art/cinder-kit-01/build_appearance.py), [외형 원본](../../art/cinder-kit-01/Cinder_Kit_Appearance.blend), [Unity 가져오기·조립·검사](../../NoReturns/Assets/_NoReturns/Editor/CinderAppearanceBuild.cs). `blender --background --python art/cinder-kit-01/build_appearance.py` 실행 후 Unity의 `NO RETURNS/Trials/Create Cinder Appearance Review`를 사용한다. 기존 외형 씬이 있으면 생성 메뉴는 중단한다. 수동 변경을 별도 씬에 보관한 뒤 명시적으로 `CinderAppearanceBuild.Create(true)`를 호출해야 재생성한다. YAML 직접 편집 없음.
- `NoReturns/Assets/_NoReturns/Art/CinderAppearance01/`: 구조 11개와 표현 3개, 총 FBX 14개; 최초에는 직접 그린 공용 512×512 BaseColor와 256×128 `WAREHOUSE`/화살표 텍스처; URP Lit 재질 3개. 생성 시안을 잘라 텍스처로 사용하지 않았으며 외부 폰트·모델·유료 생성은 없다. 텍스처는 sRGB·Point·mipmap·Clamp·무압축, 금속성 0·Smoothness 0.05다. 최초 벽 띠는 Y=1.10~1.75m, 하단 마모는 Y≈0.22~0.30m에 모았다. 현재 질감은 아래 수정으로 교체했으며 띠 범위는 생성 목표이지 정확한 도장 경계 실측값이 아니다. 수치는 [제작 코드](../../art/cinder-kit-01/build_appearance.py)와 [Unity 검사](../../art/cinder-kit-01/production-unity-validation.json) 기준 시험값이다.
- `NoReturns/Assets/_NoReturns/Prefabs/CinderAppearance01/`: 프리팹 14개. 작업등 0.6×0.2×0.18m, 표지 1.2×0.6×0.02m, 선반 2.4×2.4×0.6m. Unity가 작업등 Point Light 5개를 소유한다: 색 (1,0.67,0.3), 강도 0.65, 범위 5m, 그림자 없음. 표지 화살표는 입구를 향한다. 선반 중심 (-25.2,0,-5.4)m·yaw 90°, 선반판 중심 높이 0.22/1.32m, 화물 없는 2단이다.
- 선반만 보관 체적 전체의 BoxCollider 1개를 갖는다. 선반 내부 상호작용은 없으며 필요할 때 개별 판·기둥 충돌로 바꾼다. 램프·표지·구조 외형은 새 Collider가 없다. [실측](../../art/cinder-kit-01/production-unity-validation.json): 구조 6,408개·표현 216개 삼각형, 표현 배치 7개. FPS·최종 성능 검증이 아니다.

[Blender 검사](../../art/cinder-kit-01/production-validation.json): FBX 14개 단위·치수·피벗·UV·닫힌 면·양의 체적·재가져오기·재질 슬롯 통과. 처음 작업등의 Unity 돌출 방향이 반대로 나와 소품 축 매핑을 수정했다. 표지 글자·화살표 방향을 실제 화면으로 확인했다. [Unity 검사](../../art/cinder-kit-01/production-unity-validation.json): 14개 임포트와 텍스처 설정, 배치·충돌·광원 수 통과. [구조 재검사](../../art/cinder-kit-01/production-structure-validation.txt): 기존 회색 프리팹 11개의 기준 검사와 새 외형 씬의 통과 6개·점프 3개·중앙 화물 자세 48개 통과.

[Mac Editor Play 검사](../../art/cinder-kit-01/production-carry-edge-validation.json): 입구·벽·선반 접근 18개 위치×8개 yaw×3개 pitch, **432개 자세의 실제 겹침 0개**, E/W/S/Q·빈손 점프 통과. OverlapBox 후보 6개는 정확히 맞닿은 경계였고 ComputePenetration의 양의 깊이가 없었다. [검사 코드](../../tools/unity_checks/CinderCarryEdgeCheck.cs)는 후보를 정밀 검사하며 허용 오차는 0.00001m다. 기존 운반 런타임 코드를 바꾸지 않았다. 자동 키/API 자세 검사이며 사람 실시간 수동 플레이가 아니다.

실제 Play 카메라: [입구 빈손](../../art/cinder-kit-01/production-entry-empty.png) · [입구 운반](../../art/cinder-kit-01/production-entry-carry.png) · [안쪽 6m에서 뒤돌아본 운반](../../art/cinder-kit-01/production-inside-rear-carry.png) · [빈 선반](../../art/cinder-kit-01/production-rack-empty.png) · [문 가장자리](../../art/cinder-kit-01/production-carry-edge-door.png) · [벽 근접](../../art/cinder-kit-01/production-carry-edge-wall.png). 캡처 전에 한 프레임 이상 기다려 이전 화물 위치가 찍히던 문제를 해결했다. IMGUI 안내는 포함하지 않는다. 조명이 과한 첫 화면에서 Point 강도를 1.8→0.65로 낮췄다. 밝은 면·화물 윤곽·표지·2단 빈 선반을 확인했으나 실제 사용자 평가, 전 부품 정면/후면/측면/상부 품질 검토, 원거리 무늬 깜빡임·성능은 남아 있다.

재검사: `CinderAppearanceReview`를 열어 `CinderAppearanceBuild.Validate()`와 `CinderStructureBuild.Validate()`를 실행한다. Play 후 `unity command run_script --project-path NoReturns --file ../tools/unity_checks/CinderCarryEdgeCheck.cs --caller plugin --skill unity-cli`를 실행하고 Play를 중지한다. 외형 검사 파일에는 `production-` 접두사를 사용해 회색 근거를 덮어쓰지 않는다. [컴파일·콘솔·보존 해시](../../art/cinder-kit-01/production-checks.json): 컴파일 오류 0개, 마지막 Play 콘솔 오류/경고 0개. 원본 블록아웃·회색 씬·기존 우주선 재질 8개는 시작 해시와 같다. 독립 Mac/Windows 빌드·선내 Play·온라인/AI/배송·전체 게임 품질은 이번에 검사하지 않았다.

## 2026-10-01 — 노후 질감 수정

사용자가 실제 색감이 “너무 깔끔”하다며 승인한 컨셉 사진처럼 더럽고 오래된 느낌을 요청했다. 첫 적용에 대한 수정 의견이며 수정 결과의 품질 승인과 구분한다. 벽의 누런 변색·녹물, 붉은 띠의 도장 벗겨짐, 이음새·하단의 녹과 때, 바닥 마모·오염, 천장 얼룩·표지 테두리 부식을 늘렸다. 조명과 모델 형상은 유지했다.

내장 imagegen으로 기존 아틀라스·표지를 수정하고 승인 입구 시안을 질감 참고로 사용했다. [아틀라스 원본](../../art/cinder-kit-01/aged-atlas-source.png) 1254×1254, [표지 원본](../../art/cinder-kit-01/aged-sign-source.png) 1774×887과 [실제 프롬프트·참고/출력 해시](../../art/cinder-kit-01/aged-texture-provenance.json)를 보관했다. Blender 기본 이미지 크기 변경만으로 게임용 512×512 / 256×128로 정규화했다. UV 구역·텍스처 GUID 유지, 컨셉 사진 자르기·외부 생성 서비스·Tripo 호출 없음.

표면만 재생성: `blender --background --python art/cinder-kit-01/build_appearance.py -- --surfaces-only`. 기존 FBX를 출력하지 않고 텍스처와 패킹된 외형 Blender 원본을 갱신한다. Unity `AssetDatabase.Refresh()` 후 기존 검사 메뉴를 실행한다. 씬 생성 메뉴는 실행하지 않았다. [보존·검사 기록](../../art/cinder-kit-01/aged-checks.json): FBX 14개·프리팹 14개·재질 3개·씬 3개·기존 우주선 재질 변경 8개, 총 44개 파일이 시작 해시와 같다. 크기·개구부·충돌·광원·배치·운반 런타임 변경 없음.

현재 질감으로 Blender 재가져오기 14개, Unity 임포트/표면 설정, 통과 6개·점프 3개·중앙 화물 자세 48개를 통과했다. Mac Editor Play 자동 검사 432개 자세의 겹침 0개, E/W/S/Q·빈손 점프 통과. 현재 컴파일 상태 오류 0개·Play 콘솔 오류/경고 0개. 새 컴파일을 강제하지 않았고 사람의 실시간 조작 검사가 아니다. 실제 카메라 6개 화면을 확인했다. [현재 입구](../../art/cinder-kit-01/production-entry-empty.png) · [현재 선반](../../art/cinder-kit-01/production-rack-empty.png); 첫 적용 비교본은 [입구](../../art/cinder-kit-01/clean-entry-before.png) · [선반](../../art/cinder-kit-01/clean-rack-before.png)에 보존한다.

수정 결과의 사용자 품질 의견·전 부품 다면 검토·원거리 무늬·성능·독립 Mac/Windows 빌드·선내 Play·온라인/AI/배송은 미검증이다. 문서 검사에는 기존 누락 artifacts 링크 142개가 남으며 새 실패는 없다.

2026-10-01 사용자 의견: “그래 이런 느낌으로 맵을 구성하자. 다음으로 넘어가자”. 노후 질감 수정 결과를 맵의 스타일 기준으로 승인했다. 이후 [건물 5개로 공통 외형을 확장](cinder-map-appearance.ko.md)했으며 확장 결과의 사용자 검토는 별도다. 기존 기록의 미승인 상태는 당시 상태로 보존한다.

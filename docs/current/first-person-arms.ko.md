# 1인칭 손·팔 제작과 연결 — 0.9.17

[English](first-person-arms.en.md)

## 신호기 양손 운반 — 0.9.17

기존 신호기 모델을 양손으로 받치는 1인칭 자세를 연결했다. [장비 표시 코드](../../NoReturns/Assets/_NoReturns/Runtime/CarryLab/CarryEquipment.cs)는 본인 운반 중에만 시점 기준 **(0,-0.34,0.58)m**에 장비를 표시하고 시선 회전을 따른다. 손과 같은 Overlay 레이어 31을 사용한다. 장비 크기는 기존 **0.42×0.44×0.34m**, 손 접촉 목표는 중심에서 좌우 **±0.22m**·아래 **0.09m**이며 손목은 **25°** 이내로 제한한다. [손 자세 코드](../../NoReturns/Assets/_NoReturns/Runtime/CarryLab/EmployeeVisual.Carry.cs) · [미리보기](../../art/player-employee-01/unity-review/FirstPerson-Beacon-Carry.png).

호스트의 실제 장비 위치·구매·횟수·배치 판정은 유지한다. 로컬 표시 위치는 네트워크 위치를 덮어쓰지 않는다. 내려놓으면 월드 레이어 0·호스트 위치·수직 회전·충돌체를 복원하며 양손을 숨긴다. 일반 위험 플레이에서는 기존 오른손 진압봉으로 복귀한다. 배송 전용 모드는 기존대로 진압봉을 표시하지 않는다. 다운/구조 중에는 장비 손을 차단한다. 다른 사람이 보는 신호기 위치/전신 자세는 이번 범위에 포함하지 않는다.

[0.9.17 검증 기록](../validation/employee-beacon-0.9.17.json): 신호기 FOV 65/80/100° × 시선 -60/-30/0/30/60° × 방향 0/90/180°의 45개와 놓기 복귀 1개를 추가해 총 **1,508개** Editor 표본 통과. 손목 최대 **21.880886°**, 접촉 목표 오차 최대 **0.000000422m**다. 호스트 위치 보존·로컬 레이어/충돌 차단·배치 후 월드 위치/회전/레이어/충돌 복원도 확인했다. 실제 [URP 신호기 스택 출력](../../art/player-employee-01/unity-review/FirstPerson-Beacon-Stack.png) **73,918픽셀**을 확인했다. 이는 Editor 화면 밖 렌더이며 실제 맵 화면은 아니다. 맥 빌드 오류 0개·기존 경고 7개, 화면 없는 실제 네 클라이언트 배송/구매/운반/배치 **46개** 통과. 사람의 자연스러움, 실제 맵/HUD GPU 화면, 원격 신호기 운반 자세, 모든 손가락/옷 관통은 남는다. 아래 0.9.16·0.9.15 수치는 이전 검증 이력이다.

이전 검증 게임 **0.9.16**·프로토콜 **13**·TCP **27842**. 이번에는 아래 구조 손 표현을 추가했다. 맥 빌드 오류 0개·기존 경고 7개, 화면 없는 실제 클라이언트 검사 53개 통과. 아래 0.9.15 소개는 손목 수정 이력이다.

## 2026-10-04 — 1인칭 손목 꺾임 수정 0.9.15

사용자가 0.9.14 공격 미리보기의 손목 꺾임을 지적해 수정했다. 손바닥과 원통형 손잡이의 축 관계를 바로잡고, 팔꿈치를 옆으로 펼쳐 팔과 손등이 이어지게 했다. 손/팔뚝 방향 사이 각도는 최대 25°로 제한하며 손잡이 접촉을 다시 맞춘다. 봉을 세운 채 전방으로 움직이고 벽 앞에서는 어깨 기준 위치도 뒤로 당긴다. [현재 자세·제작](first-person-arms.ko.md) · [검증 기록](../validation/employee-wrist-0.9.15.json). 게임 **0.9.15**·프로토콜 **13**·TCP **27842**. 이전 자세의 자연스러움은 사용자에게서 문제로 확인됐으며 수정본 품질 승인은 아직 없다. 수동 시험은 종료 상태다.

2026-10-04. 기존 직원 우주복의 장갑·소매를 재사용해 본인 시점 오른손/팔과 상자 운반 양손을 연결했다. [진압봉 동작](employee-baton.ko.md) · [검증 기록](../validation/employee-wrist-0.9.15.json). 게임 **0.9.15**, 프로토콜 **13**, TCP **27842**. [버전 설정](../../NoReturns/ProjectSettings/ProjectSettings.asset). 새 Mixamo 동작·Blender/MCP 편집·이미지/모델 생성 서비스 호출은 없다.

## 로컬 재현

1. 기존 [직원 가져오기](employee-animation.ko.md)로 `EmployeeLocal/Employee.prefab`과 로컬 Idle/Walk를 준비한다.
2. Unity 메뉴 `NO RETURNS/Art/Prepare Local First Person Arms`를 실행한다. [추출 코드](../../NoReturns/Assets/_NoReturns/Editor/FirstPersonArmsBuild.cs)는 각 상완 아래 뼈에 대한 스킨 가중치 합이 0.6 이상인 꼭짓점만으로 구성된 삼각형을 추출한다. 원본 9,118삼각형에서 왼팔 **1,414꼭짓점/1,240삼각형**, 오른팔 **1,452꼭짓점/1,250삼각형**이 생성된다. 원본 뼈/바인드 포즈/재질/텍스처를 유지하며 충돌체는 없다.
3. `Assets/_NoReturns/Resources/EmployeeLocal/FirstPersonArms.prefab`과 두 메시가 로컬에 생성된다. 기존 Mixamo 원본 배포 확인이 끝나지 않아 이 폴더는 계속 공개 Git에서 제외한다. 재현 코드·Unity .meta·검증 기록·검토 PNG만 커밋한다.
4. [빌드 코드](../../NoReturns/Assets/_NoReturns/Editor/CinderFourPlayerBuild.cs)가 팔 프리팹/메시 2개·충돌체 부재를 확인한 뒤 기존 맥 시험 앱을 만든다. 직원 모델을 다시 가져온 뒤에는 팔 추출도 다시 실행한다.

## 화면 표현

[런타임 연결](../../NoReturns/Assets/_NoReturns/Runtime/CarryLab/FirstPersonArms.cs). 자기 전신은 계속 숨기고 두 팔 메시만 별도 표시한다. 빈손 상태에서는 진압봉을 쥔 오른손/소매, 상자 운반 시에는 양손을 표시한다. 놓으면 오른손 진압봉으로 복귀한다. 유효한 구조에서는 아래의 양손 구조 자세를 표시하며 진압봉은 숨긴다. 신호기 운반 시 양손을 표시한다. 다운/비활성에서는 손 표현을 숨긴다.

URP Base 카메라에서 **레이어 31**을 제외하고 Overlay 카메라로 팔과 본인 진압봉만 렌더한다. Overlay는 깊이를 지우며 그림자를 받거나 만들지 않는다. 본인 FOV를 따르고 near **0.015m**, far **3m**다. 어깨 기준 모델 원점은 시점에서 **(0,-1.65,0.05)m**이다. [Unity 공식 카메라 스택 안내](https://docs.unity3d.com/6000.0/Documentation/Manual/urp/camera-stacking.html). 그래픽 장치가 없는 자동 시험에서는 GPU 렌더러/스택을 초기화하지 않고 동일 뼈·손잡이 계산을 수행한다. 실제 스택 구성은 Editor 그래픽 검사로 확인한다.

원통 손잡이 축 회전과 손바닥 방향을 분리해 손바닥 목표에 -90° 보정, 최종 봉에 +90° 보정을 적용한다. 팔꿈치는 옆으로 벌리고, 손/팔뚝 방향 사이 각도를 25° 이내로 제한한 뒤 접촉점을 다시 맞춘다. 최종 봉은 실제 손잡이 위치에 붙는다. 준비/공격 목표 회전은 [공통 곡선](../../NoReturns/Assets/_NoReturns/Runtime/CarryLab/BatonMotion.cs)의 (-12-18a,180,-18+8a)°다. 기존 팔 뼈 길이를 늘리지 않는다. 벽 앞에서는 봉의 시점 기준 깊이만 최소 **0.35**로 당긴다. 벽 앞에서 모델 원점 z도 0.05m에서 -0.15m로 이동해 손목을 접어 넣지 않는다. 상자 운반은 기본 원점을 사용한다. 팔/봉 전체를 축소하지 않는다. 중심 전방 광선 검사이므로 모든 측면 모서리 관통 방지를 보장하지 않는다. 상자 손은 기존 화물 접촉 목표를 사용하며 먼 거리에서는 팔 길이 제한 때문에 접촉 오차가 남을 수 있다.

## 구조 손 표현 — 0.9.16

[연결 코드](../../NoReturns/Assets/_NoReturns/Runtime/CarryLab/BatonVisual.cs)가 호스트의 유효한 구조 진행 시간이 0보다 클 때만 양손을 표시한다. 기존 2m/시야 확보·2.5초 구조·4초 부활 보호 규칙은 유지한다. 기본 손가락 자세를 복원하고 0.18초 동안 화면 아래에서 손을 내민다. 이후 2Hz·최대 0.018m 높이의 엇갈린 보조 움직임을 적용한다. [팔 계산](../../NoReturns/Assets/_NoReturns/Runtime/CarryLab/EmployeeVisual.Carry.cs)은 손목을 25° 이내로 제한한다. 취소/완료 시 기존 오른손 진압봉으로 즉시 복귀하며 다운 시 숨긴다. 별도 회수 블렌드는 없다.

이는 카메라 기준 구조 중 표시이며 쓰러진 동료 몸의 실제 접촉점에 손을 붙이는 동작은 아니다. 원격 전신 구조/다운 애니메이션은 남는다. [진입 0.1초](../../art/player-employee-01/unity-review/FirstPerson-Rescue-Enter.png) · [보조 0.5초](../../art/player-employee-01/unity-review/FirstPerson-Rescue-Assist.png) · [보조 1.15초](../../art/player-employee-01/unity-review/FirstPerson-Rescue-Press.png). 진입 초기에는 손이 화면 아래에 있다. 정적 검토 이미지이며 사람의 품질 승인은 아직 없다.

[0.9.16 검증 기록](../validation/employee-rescue-0.9.16.json): 기존 1,397개와 구조 63개·취소/다운 전환 2개, 총 1,462개 Editor 표본 통과. 구조 손목 최대 23.194618°·손 목표 오차 최대 0.000000486m. 실제 URP 스택 렌더 통과. 아래 0.9.15 수치는 이전 검증 이력이다.

## 검증과 남은 작업 (0.9.15 이력)

[Editor 검사](../../NoReturns/Assets/_NoReturns/Editor/FirstPersonArmsReview.cs): FOV 65/80/100° × 시선 -45/0/45° × 깊이 0.35/0.5/0.65/0.8/1 × 31시점, 상자/숨김 포함 **1,397개**. 손잡이 목표 오차 최대 **0.000000670m**, 뼈 길이 변화 최대 **0.000000596m**. 손/팔뚝 각도 최대 **24.037256°**, 같은 방식으로 재현한 이전 공격 최대 자세는 **119.470131°**였다. 이 수치는 팔뚝과 손등 방향선 사이의 각도다. 양손 운반·숨김·레이어/깊이 스택·유한 메시 확인. [대기](../../art/player-employee-01/unity-review/FirstPerson-Ready.png) · [공격](../../art/player-employee-01/unity-review/FirstPerson-Strike.png) · [회수](../../art/player-employee-01/unity-review/FirstPerson-Return.png) · [양손 운반](../../art/player-employee-01/unity-review/FirstPerson-Carry.png). PNG는 정적 Editor 검토이며 실제 게임 화면/사용자 품질 승인이 아니다.

실제 URP Base/Overlay 스택의 화면 밖 렌더도 통과했다. 검정 배경 외 팔/봉 픽셀 **70,087개**와 [스택 출력](../../art/player-employee-01/unity-review/FirstPerson-Stack-Ready.png)을 확인했다. Editor의 실제 카메라 스택 검사이며 native 게임의 전체 맵/HUD 화면 검사는 아니다. 총 검토 이미지 **5장**이다.

실제 맥 빌드와 화면 없는 클라이언트 검사 결과는 검증 기록을 따른다. 사람의 자연스러움/타격감, 실제 게임 GPU 화면의 스택/조명/HUD 조합, 모든 거리·회전·벽 모서리 관통, Windows/LAN·성능·전신 구조/다운과 원격 신호기 운반 자세는 미완료다. 수동 창은 열지 않았고 [요청 시 시험](companion-play.ko.md) 규칙을 유지한다.

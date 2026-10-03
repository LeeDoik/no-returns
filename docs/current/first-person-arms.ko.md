# 1인칭 손·팔 제작과 연결 — 0.9.15

[English](first-person-arms.en.md)

## 2026-10-04 — 1인칭 손목 꺾임 수정 0.9.15

사용자가 0.9.14 공격 미리보기의 손목 꺾임을 지적해 수정했다. 손바닥과 원통형 손잡이의 축 관계를 바로잡고, 팔꿈치를 옆으로 펼쳐 팔과 손등이 이어지게 했다. 손/팔뚝 방향 사이 각도는 최대 25°로 제한하며 손잡이 접촉을 다시 맞춘다. 봉을 세운 채 전방으로 움직이고 벽 앞에서는 어깨 기준 위치도 뒤로 당긴다. [현재 자세·제작](first-person-arms.ko.md) · [검증 기록](../validation/employee-wrist-0.9.15.json). 게임 **0.9.15**·프로토콜 **13**·TCP **27842**. 이전 자세의 자연스러움은 사용자에게서 문제로 확인됐으며 수정본 품질 승인은 아직 없다. 수동 시험은 종료 상태다.

2026-10-04. 기존 직원 우주복의 장갑·소매를 재사용해 본인 시점 오른손/팔과 상자 운반 양손을 연결했다. [진압봉 동작](employee-baton.ko.md) · [검증 기록](../validation/employee-wrist-0.9.15.json). 게임 **0.9.15**, 프로토콜 **13**, TCP **27842**. [버전 설정](../../NoReturns/ProjectSettings/ProjectSettings.asset). 새 Mixamo 동작·Blender/MCP 편집·이미지/모델 생성 서비스 호출은 없다.

## 로컬 재현

1. 기존 [직원 가져오기](employee-animation.ko.md)로 `EmployeeLocal/Employee.prefab`과 로컬 Idle/Walk를 준비한다.
2. Unity 메뉴 `NO RETURNS/Art/Prepare Local First Person Arms`를 실행한다. [추출 코드](../../NoReturns/Assets/_NoReturns/Editor/FirstPersonArmsBuild.cs)는 각 상완 아래 뼈에 대한 스킨 가중치 합이 0.6 이상인 꼭짓점만으로 구성된 삼각형을 추출한다. 원본 9,118삼각형에서 왼팔 **1,414꼭짓점/1,240삼각형**, 오른팔 **1,452꼭짓점/1,250삼각형**이 생성된다. 원본 뼈/바인드 포즈/재질/텍스처를 유지하며 충돌체는 없다.
3. `Assets/_NoReturns/Resources/EmployeeLocal/FirstPersonArms.prefab`과 두 메시가 로컬에 생성된다. 기존 Mixamo 원본 배포 확인이 끝나지 않아 이 폴더는 계속 공개 Git에서 제외한다. 재현 코드·Unity .meta·검증 기록·검토 PNG만 커밋한다.
4. [빌드 코드](../../NoReturns/Assets/_NoReturns/Editor/CinderFourPlayerBuild.cs)가 팔 프리팹/메시 2개·충돌체 부재를 확인한 뒤 기존 맥 시험 앱을 만든다. 직원 모델을 다시 가져온 뒤에는 팔 추출도 다시 실행한다.

## 화면 표현

[런타임 연결](../../NoReturns/Assets/_NoReturns/Runtime/CarryLab/FirstPersonArms.cs). 자기 전신은 계속 숨기고 두 팔 메시만 별도 표시한다. 빈손 상태에서는 진압봉을 쥔 오른손/소매, 상자 운반 시에는 양손을 표시한다. 놓으면 오른손 진압봉으로 복귀한다. 신호기 운반/다운/유효한 구조/비활성에서는 전용 손 표현을 숨긴다. 신호기·구조 전용 손 자세는 미완료다.

URP Base 카메라에서 **레이어 31**을 제외하고 Overlay 카메라로 팔과 본인 진압봉만 렌더한다. Overlay는 깊이를 지우며 그림자를 받거나 만들지 않는다. 본인 FOV를 따르고 near **0.015m**, far **3m**다. 어깨 기준 모델 원점은 시점에서 **(0,-1.65,0.05)m**이다. [Unity 공식 카메라 스택 안내](https://docs.unity3d.com/6000.0/Documentation/Manual/urp/camera-stacking.html). 그래픽 장치가 없는 자동 시험에서는 GPU 렌더러/스택을 초기화하지 않고 동일 뼈·손잡이 계산을 수행한다. 실제 스택 구성은 Editor 그래픽 검사로 확인한다.

원통 손잡이 축 회전과 손바닥 방향을 분리해 손바닥 목표에 -90° 보정, 최종 봉에 +90° 보정을 적용한다. 팔꿈치는 옆으로 벌리고, 손/팔뚝 방향 사이 각도를 25° 이내로 제한한 뒤 접촉점을 다시 맞춘다. 최종 봉은 실제 손잡이 위치에 붙는다. 준비/공격 목표 회전은 [공통 곡선](../../NoReturns/Assets/_NoReturns/Runtime/CarryLab/BatonMotion.cs)의 (-12-18a,180,-18+8a)°다. 기존 팔 뼈 길이를 늘리지 않는다. 벽 앞에서는 봉의 시점 기준 깊이만 최소 **0.35**로 당긴다. 벽 앞에서 모델 원점 z도 0.05m에서 -0.15m로 이동해 손목을 접어 넣지 않는다. 상자 운반은 기본 원점을 사용한다. 팔/봉 전체를 축소하지 않는다. 중심 전방 광선 검사이므로 모든 측면 모서리 관통 방지를 보장하지 않는다. 상자 손은 기존 화물 접촉 목표를 사용하며 먼 거리에서는 팔 길이 제한 때문에 접촉 오차가 남을 수 있다.

## 검증과 남은 작업

[Editor 검사](../../NoReturns/Assets/_NoReturns/Editor/FirstPersonArmsReview.cs): FOV 65/80/100° × 시선 -45/0/45° × 깊이 0.35/0.5/0.65/0.8/1 × 31시점, 상자/숨김 포함 **1,397개**. 손잡이 목표 오차 최대 **0.000000670m**, 뼈 길이 변화 최대 **0.000000596m**. 손/팔뚝 각도 최대 **24.037256°**, 같은 방식으로 재현한 이전 공격 최대 자세는 **119.470131°**였다. 이 수치는 팔뚝과 손등 방향선 사이의 각도다. 양손 운반·숨김·레이어/깊이 스택·유한 메시 확인. [대기](../../art/player-employee-01/unity-review/FirstPerson-Ready.png) · [공격](../../art/player-employee-01/unity-review/FirstPerson-Strike.png) · [회수](../../art/player-employee-01/unity-review/FirstPerson-Return.png) · [양손 운반](../../art/player-employee-01/unity-review/FirstPerson-Carry.png). PNG는 정적 Editor 검토이며 실제 게임 화면/사용자 품질 승인이 아니다.

실제 URP Base/Overlay 스택의 화면 밖 렌더도 통과했다. 검정 배경 외 팔/봉 픽셀 **70,087개**와 [스택 출력](../../art/player-employee-01/unity-review/FirstPerson-Stack-Ready.png)을 확인했다. Editor의 실제 카메라 스택 검사이며 native 게임의 전체 맵/HUD 화면 검사는 아니다. 총 검토 이미지 **5장**이다.

실제 맥 빌드와 화면 없는 클라이언트 검사 결과는 검증 기록을 따른다. 사람의 자연스러움/타격감, 실제 게임 GPU 화면의 스택/조명/HUD 조합, 모든 거리·회전·벽 모서리 관통, Windows/LAN·성능·전용 구조/신호기 동작은 미확인이다. 수동 창은 열지 않았고 [요청 시 시험](companion-play.ko.md) 규칙을 유지한다.

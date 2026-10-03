# 직원 전신·Idle/Walk — 0.9.5

[English](employee-animation.en.md)

2026-10-03. 사용자가 제공한 직원 모델과 보정 Idle, Walking을 현재 Cinder 플레이어에 연결했다. 임시 도형 외형을 전신 모델로 교체하며 이동·충돌·운반·네트워크 판정은 기존 코드를 사용한다. 게임 **0.9.5**, 프로토콜 **13**, TCP **27842**. 버전 근거는 [PlayerSettings](../../NoReturns/ProjectSettings/ProjectSettings.asset), 구현은 [직원 외형](../../NoReturns/Assets/_NoReturns/Runtime/CarryLab/EmployeeVisual.cs)과 [CarryRoom](../../NoReturns/Assets/_NoReturns/Runtime/CarryLab/CarryRoom.cs)이다.

## 연결과 제작

- [반입 스크립트](../../NoReturns/Assets/_NoReturns/Editor/EmployeeAnimationBuild.cs)가 두 FBX를 각각 `Humanoid / Create From This Model`로 반입한다. 53개 뼈 이름은 같지만 기본 관절 위치가 달라 별도 Avatar로 리타기팅한다. 둘 다 유효한 Humanoid이며 Idle 8.333334초, Walk 1.033333초다.
- 오염된 원본 색상 텍스처와 URP/Lit, 거칠기 표현을 위한 Smoothness 0.18을 적용한다. 원본은 4,765정점·9,118삼각형·높이 1.8m이며 Unity의 UV/노멀 분리 후 정점 수는 10,612다. 팀 색은 전체 재질에 32% 혼합한다.
- 실제 수평 이동 속도에 따라 Idle→Walk 0.12m/s 초과, Walk→Idle 0.08m/s 미만, 전환 0.15초다. 보폭 재생률은 속도/2.2를 0.35–2.2배로 제한한다. Root Motion은 끄고 기존 CharacterController가 위치를 결정한다. 원격 플레이어는 수신·보간된 실제 위치로 같은 전환을 계산한다.
- 각 창에서는 자기 몸을 숨기고 나머지 세 명을 표시한다. 다운 시 동작을 멈추고 기존 전신 회전 표현을 사용한다. 별도 콜라이더를 추가하지 않는다.
- 현재 한 개의 전진 Walk를 옆/뒤 이동에도 재사용한다. 전용 후진·횡이동·운반 손 접촉·진압봉·다운/일어나기·구조·점프 동작과 1인칭 팔은 다음 제작 범위다.

## 로컬 재현과 실행

Mixamo 원본/보정 동작의 공개 소스 재배포 조건은 아직 확정하지 않았다. 해당 FBX와 생성된 Prefab/Controller는 `NoReturns/Assets/_NoReturns/Resources/EmployeeLocal/`에 로컬로 보관하고 Git에서 제외한다. 공개 저장소에는 반입/검사 코드·해시·정적 화면만 포함한다. 게임 빌드는 이 로컬 에셋을 포함하므로 이번 검증용 실행본도 로컬에만 둔다.

1. [앞 단계 보정](03-guides.ko.md#2026-10-03--수령-idle의-상체-자세-보정)으로 `artifacts/employee-idle/corrected/NR_Employee_Idle_Upright.fbx`를 준비한다. `~/Downloads/Walking.fbx`와 저장소의 직원 원본 텍스처도 필요하다. 입력 해시는 [검증 기록](../validation/employee-unity-0.9.5.json)에 기록한다.
2. Unity 메뉴 `NO RETURNS > Art > Prepare Local Employee Animations`를 실행한다. 다른 경로의 파일은 Editor에서 `NoReturns.Editor.EmployeeAnimationBuild.Prepare(idlePath, walkPath)`로 전달할 수 있다. 기존 로컬 생성물은 갱신하지만 별도 원본은 보존한다.
3. `NO RETURNS > Art > Review Local Employee Animations`로 10개 자세 검사와 정적 화면 4장을 만든다. 로컬 기록 위치는 `artifacts/employee-unity/`다. 정적 화면은 Animator로 평가한 메시를 BakeMesh로 그린 것으로 실제 게임 캡처와 구분한다. [Unity BakeMesh 정의](https://docs.unity.com/en-us/engine/6000.3/script-reference/unityengine/skinnedmeshrenderer/bakemesh)에 따라 회전축을 보정해 키를 측정한다.
4. `python3 tools/cinder_four_player.py build`로 빌드한 뒤 [07_Play_Cinder_4P.command](../../07_Play_Cinder_4P.command)를 실행한다. 다른 창의 직원을 보며 움직임을 확인한다. 새 체크아웃에서 로컬 에셋을 준비하지 않으면 Cinder 빌드는 준비 안내 오류로 멈춘다. 에디터 직접 Play에는 기존 임시 도형 대체 표시가 남아 있다.
5. 재검사: `python3 tools/test_employee_animation.py`, `python3 tools/cinder_four_player.py check`, `python3 tools/test_cinder_threat.py`. 같은 포트를 사용하므로 순서대로 실행한다.

## 검증과 남은 품질 검토

[검증 기록](../validation/employee-unity-0.9.5.json). Humanoid 두 개, 10개 표본의 유한 메시·높이 1.768–1.838m·루트 이동 0, 발 움직임을 확인했다. 미리보기의 잘못된 축 측정/정적 스키닝 갱신은 검사 도구에서 수정했으며 모델 크기를 임의로 바꾸지 않았다. 네이티브 맥 빌드 오류 0개·기존 경고 7개. 실제 네 실행본의 외형/전환 10개·운반/재접속 13개·리스너/진압봉/다운/구조 31개, 총 54개 검사를 통과했다. 전체 억제 60개와 배송/정산 UI 전체 검사는 이번에 다시 실행하지 않았다. 경고는 기존 맵 충돌 사전 굽기·Pipeline 런타임 설정 부재·구식 검색 API·미사용 디버그 셰이더 제거다.

사용자 자세/색감 승인, 발 미끄러짐과 반복 이음새의 사람 평가, 전 프레임 옷 관통/화물 접촉, 실제 사람 네 명·다른 PC·Windows·성능 측정은 미완료다. 과거의 직원 Unity 미적용 설명은 이 문서의 구현·검증 범위에서 대체한다.

[Unity 대기 자세](../../art/player-employee-01/unity-review/Idle-1.png) · [걷기 자세](../../art/player-employee-01/unity-review/Walk-3.png) · [실제 게임 화면](../../art/player-employee-01/unity-review/employee-in-game.png).

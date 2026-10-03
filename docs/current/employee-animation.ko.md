# 직원 전신·Idle/Walk — 0.9.15

[English](employee-animation.en.md)

## 2026-10-04 — 1인칭 손목 꺾임 수정 0.9.15

사용자가 0.9.14 공격 미리보기의 손목 꺾임을 지적해 수정했다. 손바닥과 원통형 손잡이의 축 관계를 바로잡고, 팔꿈치를 옆으로 펼쳐 팔과 손등이 이어지게 했다. 손/팔뚝 방향 사이 각도는 최대 25°로 제한하며 손잡이 접촉을 다시 맞춘다. 봉을 세운 채 전방으로 움직이고 벽 앞에서는 어깨 기준 위치도 뒤로 당긴다. [현재 자세·제작](first-person-arms.ko.md) · [검증 기록](../validation/employee-wrist-0.9.15.json). 게임 **0.9.15**·프로토콜 **13**·TCP **27842**. 이전 자세의 자연스러움은 사용자에게서 문제로 확인됐으며 수정본 품질 승인은 아직 없다. 수동 시험은 종료 상태다.

## 2026-10-04 — 역동적인 진압봉·1인칭 손과 팔 0.9.14

사용자 요청으로 짧은 준비·빠른 전개·0.42초 회수의 진압봉 팔/가슴 동작과 본인 오른손/소매, 상자 운반 양손을 구현했다. 즉시 타격·6초 재사용·운반/다운/유효한 구조 차단과 기존 이동을 유지한다. [현재 진압봉](employee-baton.ko.md) · [팔 제작·로컬 재현](first-person-arms.ko.md) · [검증 근거](../validation/employee-first-person-0.9.14.json). 게임 **0.9.14**·프로토콜 **13**·TCP **27842**. 아래 0.9.13의 최초 동작 복원·1인칭 팔 미구현/제안 상태는 당시 기록이며 이 구현 범위에서 대체한다. 전용 신호기/구조 손, 사람 품질·실제 게임 GPU 화면 조합·전체 모서리 관통·Windows/LAN·성능은 남는다. 수동 시험은 종료 상태이고 요청할 때만 기존 두 창 도구로 연다.

## 2026-10-03 — 최초 진압봉 복원·후진/옆걸음 0.9.13

사용자의 요청으로 진압봉을 최초 오른손 부착 버전(0.9.6)의 기본/사용 동작으로 되돌렸다. 0.9.9 이후 팔·가슴 공격 보정을 제거하고 손 부착·봉 자체의 0.5초 회수, 즉시 판정·6초 재사용·운반/다운/구조 숨김을 유지한다. 실제 몸 기준 이동 방향을 이용해 후진·좌우/대각선 옆걸음의 발 궤적을 보정한다. 기존 전진 Walk의 교대/발 높이를 사용하고 상체 시선·루트·물리·네트워크는 유지한다. 점프/착지도 유지한다. 새 Mixamo FBX나 Blender 편집은 없다. [진압봉](employee-baton.ko.md) · [이동](employee-locomotion.ko.md). 수정본 사람 품질·전용 클립·1인칭 팔·Windows/LAN/성능은 미확인이다.


## 2026-10-03 — 양손 상자 운반 자세 0.9.8

기본 운반 거리를 **1.1m → 0.8m**로 당기고 현재 상자의 가까운 면 위쪽에 두 손을 맞춘다. 휠 범위 **0.75–1.6m**와 회전·바닥 배치·소유자·충돌 판정은 유지한다. 대기·걷기 위에 팔 자세를 적용하며 약 **0.167초**에 걸쳐 진입/복귀하고 다운 시 즉시 해제한다. 손목·팔 회전만 바꾸며 뼈 길이·배율·직원 루트는 바꾸지 않는다. 멀거나 크게 기울어진 상자는 도달 한계에서 손과 떨어질 수 있다. 모든 거리의 완전한 접촉이나 손가락 관통 해결로 간주하지 않는다. 진압봉 운반 숨김/놓기 후 복귀와 자기 전신 숨김은 유지한다. 신호기 전용 잡기·공격/점프/구조 전용 동작·1인칭 팔은 이번 범위 밖이다.

구현: [양손 자세](../../NoReturns/Assets/_NoReturns/Runtime/CarryLab/EmployeeVisual.Carry.cs), [게임 연결](../../NoReturns/Assets/_NoReturns/Runtime/CarryLab/CarryRoom.cs), [기본 조작 거리](../../NoReturns/Assets/_NoReturns/Runtime/CarryLab/CarryRoom.Controls.cs). Blender MCP로 저장된 직원 리그의 양팔 길이(각 상완 약 0.228m·전완 약 0.249m)를 조회했으며 Blender 원본은 수정/저장하지 않았다. 실제 게임은 기존 Mixamo Humanoid를 Unity에서 평가하고 손 접촉을 보정한다. 새 애니메이션 FBX나 패키지는 추가하지 않는다. 게임 **0.9.8**, 프로토콜 **13**, TCP **27842**. [버전 근거](../../NoReturns/ProjectSettings/ProjectSettings.asset).

[검증 기록](../validation/employee-carry-0.9.8.json): Unity **6000.6.4f1** 맥 빌드 오류 **0개·경고 7개**. 대기/걷기 × 3거리 × 4회전 × 5시점 **120개** 표본에서 유한 메시·고정 뼈 길이/루트 확인, 놓기/다운 자세 해제 확인. 가까운 정면 0.75/0.8m 표본의 손 기준점 오차 최대 **0.039603m**, 뼈 길이 변화 최대 **0.000000388m**. [표본 검사 코드](../../NoReturns/Assets/_NoReturns/Editor/EmployeeCarryReview.cs). 렌더 4장은 Unity 정적 자세 검토이며 실제 게임 캡처와 구분한다: [대기](../../art/player-employee-01/unity-review/Carry-Idle.png), [걷기](../../art/player-employee-01/unity-review/Carry-Walk.png), [손 근접](../../art/player-employee-01/unity-review/Carry-Hands.png), [최대 도달 제한](../../art/player-employee-01/unity-review/Carry-ReachLimit.png).

화면 없는 실제 네 클라이언트의 [운반 검사](../../tools/test_employee_carry.py) **10개** 통과: 소유권/기본 접촉, 타인 놓기 거부, ±90°/180° 회전, 최대 도달 제한/가까이 복귀, 놓기 자세 해제, 로그 오류 없음. 첫 시도는 착륙장 소품에 상자 회전이 막혀 실패했으며, 기존 통로로 이동해 회전 공간을 확보한 뒤 통과했다. 충돌 판정은 완화하지 않았다. 화면 없는 사용자+자동 동료 회귀 **5묶음**도 통과했다. 수동 게임 창·Editor Play는 열지 않았다. “직접 테스트 해볼게” 요청 시 [사용자+동료](companion-play.ko.md)를 연다. 전 프레임/극단 시선·회전 이음새·1인칭 가림/사람 품질, 배송·위험 전체 회귀, Windows/LAN·성능은 이번에 검증하지 않았다.

## 이전 전신 연결·진압봉 0.9.5–0.9.6 기록

2026-10-03. 사용자가 제공한 직원 모델과 보정 Idle, Walking을 현재 Cinder 플레이어에 연결했다. 임시 도형 외형을 전신 모델로 교체하며 이동·충돌·운반·네트워크 판정은 기존 코드를 사용한다. 최초 연결은 0.9.5이며 이 기록 당시 게임 **0.9.6**, 프로토콜 **13**, TCP **27842**. 버전 근거는 [PlayerSettings](../../NoReturns/ProjectSettings/ProjectSettings.asset), 구현은 [직원 외형](../../NoReturns/Assets/_NoReturns/Runtime/CarryLab/EmployeeVisual.cs)과 [CarryRoom](../../NoReturns/Assets/_NoReturns/Runtime/CarryLab/CarryRoom.cs)이다.

## 2026-10-03 — 진압봉 오른손 부착 0.9.6

당시 버전은 **0.9.6**·프로토콜 **13**·TCP **27842**다. 동료의 진압봉을 몸통 고정 좌표에서 실제 오른손/손가락 뼈 기준 위치로 옮겼다. 대기·걷기에서 손을 따라가고, 손잡이를 쥐도록 손가락을 조정한다. 본인의 1인칭 진압봉 위치·벽 앞 당김·기존 타격 판정은 유지한다. 상자 또는 신호기를 들면 모든 창에서 해당 직원의 진압봉을 숨기고 놓거나 배치하면 다시 표시한다. 다운·구조 중 숨김도 유지한다.

[진압봉 표시 구현](../../NoReturns/Assets/_NoReturns/Runtime/CarryLab/BatonVisual.cs)과 위 직원 외형 코드가 근거다. 반입 뼈의 배율 100을 무기에 곱하지 않고 월드 미터로 손목→중지 밑마디 95% 지점에서 손바닥 방향 0.025m를 더한다. 검지→소지 축 기준 손가락 55°·엄지 30°의 고정 쥐기 자세를 진압봉 표시 때만 적용한다. 별도 타격 전신 동작이나 화물 접촉 IK는 추가하지 않았다.

[개발 도구·Blender MCP 설정](macos-development.ko.md#2026-10-03--최신-도구와-blender-mcp) · [이번 검사 기록](../validation/baton-hand-0.9.6.json). 아래 0.9.5 검증은 이전 기록이며 이번 검증 범위는 위 기록을 따른다. 운반 손 접촉·전용 공격 동작·전 프레임 관통/사용자 품질·1인칭 팔은 미완료다.

- [x] Unity 6000.6.4f1 맥 빌드 오류 0개·경고 14개. 실제 네 실행본 진압봉/억제/배송 69개 + 외형/전환 10개 + 리스너/타격/다운/구조 31개 = **110개** 통과. 대기·걷기 손/전신 표본 4장과 새 빌드 게임 화면을 검토했다.
- [x] 실제 MCP stdio 도구 호출로 Blender 객체 5개·뼈 53개·오른손 정보 확인. 미저장 씬·기존 사용자 수정 보존. 공식 최신 Unity/CLI/Pipeline/uv/Codex CLI 적용, Blender 5.2.2 LTS 최신 일치 확인.
- [ ] Codex 데스크톱 업데이트·재시작 후 기본 MCP 도구 노출, 새 Unity의 Windows 지원/실행, 사람 품질/전 프레임 관통·성능. 업그레이드 중 Pipeline DLL을 찾지 못하는 Burst 오류와 최종 빌드의 Burst 진입점 경고 5개가 관찰됐다. 빌드·110개 게임 검사는 통과했지만 해당 경고의 원인/성능 영향은 미확인이다. 나머지는 맵 충돌 사전 굽기·Pipeline 런타임 설정 부재·구식 검색 API·디버그 셰이더 제거 경고다.

## 연결과 제작

- [반입 스크립트](../../NoReturns/Assets/_NoReturns/Editor/EmployeeAnimationBuild.cs)가 두 FBX를 각각 `Humanoid / Create From This Model`로 반입한다. 53개 뼈 이름은 같지만 기본 관절 위치가 달라 별도 Avatar로 리타기팅한다. 둘 다 유효한 Humanoid이며 Idle 8.333334초, Walk 1.033333초다.
- 오염된 원본 색상 텍스처와 URP/Lit, 거칠기 표현을 위한 Smoothness 0.18을 적용한다. 원본은 4,765정점·9,118삼각형·높이 1.8m이며 Unity의 UV/노멀 분리 후 정점 수는 10,612다. 팀 색은 전체 재질에 32% 혼합한다.
- 실제 수평 이동 속도에 따라 Idle→Walk 0.12m/s 초과, Walk→Idle 0.08m/s 미만, 전환 0.15초다. 보폭 재생률은 속도/2.2를 0.35–2.2배로 제한한다. Root Motion은 끄고 기존 CharacterController가 위치를 결정한다. 원격 플레이어는 수신·보간된 실제 위치로 같은 전환을 계산한다.
- 각 창에서는 자기 몸을 숨기고 나머지 세 명을 표시한다. 다운 시 동작을 멈추고 기존 전신 회전 표현을 사용한다. 별도 콜라이더를 추가하지 않는다.
- 현재 한 개의 전진 Walk를 옆/뒤 이동에도 재사용한다. 전용 후진·횡이동·운반 손 접촉·진압봉 공격 전신 동작·다운/일어나기·구조·점프 동작과 1인칭 팔은 다음 제작 범위다.

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

진압봉 정적 자세: [대기 전신](../../art/player-employee-01/unity-review/Baton-Idle-body.png) · [대기 손](../../art/player-employee-01/unity-review/Baton-Idle-hand.png) · [걷기 전신](../../art/player-employee-01/unity-review/Baton-Walk-body.png) · [걷기 손](../../art/player-employee-01/unity-review/Baton-Walk-hand.png). Unity 6000.6.0f1에서 Animator로 평가한 표본 화면이며 모든 프레임의 관통 검사나 사람 승인을 대체하지 않는다.

[새 Unity 실제 게임 화면](../../art/player-employee-01/unity-review/Baton-in-game.png).

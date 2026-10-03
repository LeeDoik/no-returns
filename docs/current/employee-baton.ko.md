# 직원 진압봉 준비·휘두르기·복귀 — 0.9.9

[English](employee-baton.en.md)

2026-10-03. 사용자가 다음 캐릭터 작업으로 진압봉 공격 동작 진행을 승인했다. 현재 게임 **0.9.9**, 프로토콜 **13**, TCP **27842**. [버전 설정](../../NoReturns/ProjectSettings/ProjectSettings.asset)과 [현재 직원 외형](employee-animation.ko.md)이 근거다.

## 현재 동작

- 빈손 상태에서는 오른팔을 굽혀 진압봉을 세운 준비 자세를 취한다. 대기·걷기 동작 위에 오른팔과 가슴 회전을 적용하며 하체 이동을 유지한다. 준비 자세 진입은 약 **0.1초**다.
- 빈손 좌클릭의 기존 타격 판정은 **즉시** 적용된다. 준비 자세에서 바로 타격 자세로 진입하고 **0–0.12초** 후속 휘두르기 → **0.12–0.25초** 팔 회수 → **0.25–0.55초** 준비 자세 복귀로 이어진다. 클릭 후 타격을 늦추는 별도 선행 모션은 추가하지 않는다. 재사용 대기는 기존 **6초**다.
- 진압봉은 실제 손 기준점과 방향을 그대로 사용한다. 무기에 별도 공격 회전을 더해 손과 분리되는 기존 표현을 제거했다. 본인의 1인칭 무기도 같은 시간 곡선을 사용하며 기존 벽 앞 당김을 유지한다. 1인칭 전신 팔 에셋은 아직 없다.
- 상자·신호기 운반, 다운, 유효한 구조 진행, 비활성/미접속 직원에서는 무기와 공격 자세를 숨긴다. 중단된 공격은 물건을 놓거나 구조를 끝낸 직후 다시 재생하지 않고 준비 자세로 돌아간다.
- 조사 중 유효한 구조 요청과 공격 요청을 동시에 보내면 기존 판정 코드가 공격을 허용할 수 있음을 확인했다. 구조 대상이 2m 안에 있고 보이는 경우에는 구조 요청이 공격보다 우선하도록 수정했다. 빈손 공격의 범위/방향/시야, 리스너 기절 시간과 이동·화물 충돌은 바꾸지 않는다.

## 구현과 제작

[공격 자세/시간 곡선](../../NoReturns/Assets/_NoReturns/Runtime/CarryLab/EmployeeVisual.Baton.cs) · [공통 팔 회전 보정](../../NoReturns/Assets/_NoReturns/Runtime/CarryLab/EmployeeVisual.Carry.cs) · [무기 표시](../../NoReturns/Assets/_NoReturns/Runtime/CarryLab/BatonVisual.cs) · [구조 우선 판정](../../NoReturns/Assets/_NoReturns/Runtime/CarryLab/CarryThreat.cs).

Blender MCP로 현재 직원 리그의 가슴·오른쪽 어깨/팔/손 위치와 저장 상태를 조회했다. Blender 파일을 수정하거나 저장하지 않았다. 게임 안의 동작은 기존 Humanoid 대기/걷기를 평가한 뒤 Unity에서 팔·손목·가슴을 회전하는 방식이다. 별도 Mixamo 클립이나 FBX를 반입하지 않으며 원본, 뼈 길이·배율, 직원 루트, Root Motion과 콜라이더를 보존한다. 목표가 팔 도달 범위 밖이면 팔을 늘리지 않고 도달을 제한하며 무기는 실제 손에 계속 붙는다. 전용 애니메이션 클립으로 교체하는 것은 별도 작업이다.

## 검사와 재현

[검증 기록](../validation/employee-baton-0.9.9.json). 최신 공식 Unity **6000.6.4f1**, CLI **1.0.0-beta.12**, Pipeline **0.8.0-exp.1** 일치 확인. Blender **5.2.2 LTS**, MCP 패키지 **2.1.3**, 애드온 **1.8/프로토콜 13** 일치와 텔레메트리 꺼짐 확인.

Unity Editor에서 `NoReturns.Editor.EmployeeBatonReview.Review()`를 실행하면 [표본 검사 코드](../../NoReturns/Assets/_NoReturns/Editor/EmployeeBatonReview.cs)가 `artifacts/employee-baton/`에 기록과 정적 렌더를 생성한다. 두 대기/걷기 상태의 정규화 0.2 시점, 직원 방향 0°/90°/135°, 준비 자세와 60fps 기준 0–0.55초 **35개** 표본으로 총 **210개** 자세를 검사한다. 손 목표점 오차 최대 **0.067738m**, 뼈 길이 변화 최대 **0.000000358m**이며 정적 손 이동 약 **0.437047m**·가슴 회전 약 **14.993174°**를 확인했다. 손 목표점 오차는 도달 제한으로 생기는 값이며 무기가 손에서 떨어진 거리와 다르다. 유한 메시·고정 루트/뼈 길이·차단/중단 취소·복귀를 확인했고 [기존 운반 검사](../../NoReturns/Assets/_NoReturns/Editor/EmployeeCarryReview.cs) **120개**도 다시 통과했다.

맥 빌드 오류 **0개·경고 7개**, 화면 없는 실제 진압봉 검사 **12개**·운반 검사 **10개**·리스너/타격/다운/구조 검사 **30개**, 총 **52개**를 통과했다. 대기 공격의 팔 회전은 양 클라이언트 약 **55.00°/53.26°**, 걷기 공격은 약 **59.25°/57.59°**로 측정해 단순한 캐릭터 위치 이동과 구분했다. 연속 실행 중 포트가 잠시 남아 리스너 검사가 시작 전에 한 번 거절됐으며 포트 해제 후 30개를 통과했다. 기존 빌드 경고 7개는 검증 기록에 남겼다. 시험 프로세스와 세션은 종료했다.

정적 렌더: [준비](../../art/player-employee-01/unity-review/Baton-Ready.png) · [타격](../../art/player-employee-01/unity-review/Baton-0.png) · [후속 동작](../../art/player-employee-01/unity-review/Baton-7.png) · [회수](../../art/player-employee-01/unity-review/Baton-15.png) · [복귀](../../art/player-employee-01/unity-review/Baton-33.png). 실제 게임 캡처나 사용자 품질 승인으로 간주하지 않는다.

화면 없는 실행 검사는 아래처럼 순서대로 수행한다. [동료 실행 규칙](companion-play.ko.md)에 따라 수동 게임 창은 “직접 테스트 해볼게” 요청 때만 연다.

```sh
python3 tools/cinder_four_player.py build
python3 tools/test_employee_baton.py
python3 tools/test_employee_carry.py
python3 tools/test_cinder_threat.py --headless
```

`--headless` 리스너 검사는 실제 입력·네트워크·판정을 사용하며 화면 캡처만 생략한다. 수동 게임 창과 Editor Play는 열지 않았다. 사용자 타격감·시각 품질, 모든 걷기 시점/옷 관통·1인칭 가림, 배송/억제 전체 회귀, Windows/LAN·성능은 아직 미검증이다. 점프/착지·후진/옆걸음·구조/다운 전용 동작과 1인칭 팔은 미완료다.

# 직원 점프·착지 표현 — 0.9.12

[English](employee-locomotion.en.md)

2026-10-03. 진압봉 수정 후 다음 캐릭터 작업으로 점프·착지를 구현했다. 현재 게임 **0.9.12**, 프로토콜 **13**, TCP **27842**. [설정 근거](../../NoReturns/ProjectSettings/ProjectSettings.asset). 전진 Idle/Walk 두 FBX는 유지하며 전용 Mixamo 점프 클립을 새로 받거나 제작한 것은 아니다. 기존 Humanoid의 다리 회전과 착지 시 골반 위치를 보정하는 표현이다.

## 현재 동작·제작

- 실제 직원 위치 차이에서 수직 속도를 계산하고 발밑 **0.30m** 바닥 검사를 사용한다. 검사 원점은 직원 바닥 기준 **0.20m** 위다. 직원 CharacterController·Rigidbody·트리거를 바닥에서 제외하며 법선 y가 **0.5**를 넘는 면을 사용한다. 바닥이 없거나 상승 속도가 **1m/s**를 넘으면 공중으로 처리한다.
- 공중에서 상승/정점/하강을 표시한다. 상승은 수직 속도 **0.3m/s 초과**, 하강은 **-0.3m/s 미만**, 사이는 정점이다. 다리를 약간 접고 발목을 보정하며 공중 비중이 **0.5**를 넘으면 기존 걷기에서 Idle로 전환한다. 다리 비중은 초당 **12**로 전환한다.
- 공중 상태가 **0.12초** 넘게 이어진 뒤 바닥을 만나면 **0.22초** 착지 표현을 재생한다. 골반을 최대 **0.065m** 내리고 두 관절 다리 보정으로 해당 프레임의 발 위치·방향을 유지해 무릎을 굽혔다 편다. 다운 시 공중/착지 상태를 지우며 큰 위치 이동 **2m 이상**에서도 상태를 초기화한다.
- 직원 루트·뼈 길이/스케일·콜라이더·네트워크·기존 점프 속도와 중력은 바꾸지 않는다. 선내/경사면·서버와 원격 보간 위치에서 같은 표현을 계산한다. 점프 입력·착지 소리·착지 피해를 새로 추가하지 않는다.

[표현 코드](../../NoReturns/Assets/_NoReturns/Runtime/CarryLab/EmployeeVisual.Locomotion.cs) · [위치 기반 속도](../../NoReturns/Assets/_NoReturns/Runtime/CarryLab/EmployeeVisual.cs) · [실행 연결](../../NoReturns/Assets/_NoReturns/Runtime/CarryLab/CarryRoom.cs) · [정적 검사](../../NoReturns/Assets/_NoReturns/Editor/EmployeeLocomotionReview.cs) · [실제 동료 검사](../../tools/test_cinder_companion.py).

## 검증과 남은 작업

[검증 기록](../validation/employee-pulse-locomotion-0.9.12.json). Idle/Walk × 3방향 × 5걷기 시점 × 바닥/상승/정점/하강/착지·복귀 9시점, **270개** 표본에서 유한 메시·루트 보존·뼈 길이·발 접촉·상태 복귀·다운 취소를 확인했다. 뼈 길이 변화 최대 **0.000000507m**, 착지 발 오차 최대 **0.000001566m**, 골반 압축 최대 **0.065000m**다. [상승](../../art/player-employee-01/unity-review/Jump-Rise.png) · [하강](../../art/player-employee-01/unity-review/Jump-Fall.png) · [착지](../../art/player-employee-01/unity-review/Landing.png)는 정적 검토 표본이며 게임 캡처/사람 승인이 아니다.

맥 빌드 오류 **0개·기존 경고 7개**. 실제 화면 없는 사용자/동료 검사에는 기존 7행동·이동/점프 높이·따라오기·운반 숨김/복귀와 양쪽 상승/하강/착지·바닥 복귀를 포함한다. 최초 검사에서는 봇의 Jump 표시가 끝난 후 발생한 착지 비중을 집계하지 못했다. 실제 Landing 상태는 양쪽에서 관찰됐으며, 표시 단계 경계를 넘어 착지까지 집계하도록 수정했다. 실제 통과 수치/추가 리스너 검사 범위는 검증 기록을 따른다.

수동 시험은 종료했고 새 수동 창/Editor Play를 열지 않았다. [요청 시 실행 안내](companion-play.ko.md). 다음 제작은 **후진·옆걸음 전용 움직임**, 이어 **1인칭 팔**이다. 전용 점프 FBX·구조/다운 클립, 급경사/계단·급격한 방향 변경·모든 공중/착지 프레임의 옷 관통, 사람 품질·Windows/LAN·성능은 미검증이다. 현재 표현 구현과 최종 전용 클립 제작을 구분한다.

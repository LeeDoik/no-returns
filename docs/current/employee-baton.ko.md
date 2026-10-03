# 직원 진압봉 기본 자세·짧은 전방 동작 — 0.9.12

[English](employee-baton.en.md)

2026-10-03. 사용자가 0.9.11 직접 시험을 종료하고, 현재 봉을 들고 있는 모습이 어색하다고 보고했다. 0.9.9의 세워 쥔 기본 자세를 복원하고 마법을 발사하듯 손과 봉을 한 번 앞으로 뻗는 표현으로 수정했다. 현재 게임 **0.9.12**, 프로토콜 **13**, TCP **27842**. [버전 설정](../../NoReturns/ProjectSettings/ProjectSettings.asset). 이전 0.9.10의 상시 정면 조준 자세는 취소했다. 수정본의 자연스러움·타격감 승인은 아직 받지 않았다.

## 현재 동작

- 빈손 기본 손 위치는 직원 기준 **(0.30, 1.21, 0.20)m**, 봉 회전은 **(15, -10, -15)°**로 0.9.9 준비 자세와 같다. 준비 자세 진입은 약 **0.1초**다. 봉을 평소부터 수평으로 겨누지 않는다.
- 공격 시 손과 세워 쥔 봉을 **0.20m** 앞으로 뻗는다. **0–0.06초** 빠른 전진 → **0.06–0.10초** 유지 → **0.10–0.30초** 회수다. 측면 휘두르기·봉 방향 전환·가슴 비틀기를 넣지 않는다. 타격 판정은 클릭 시 **즉시**, 재사용 대기는 **6초**로 유지한다. 새 마법/투사체 기능은 없다.
- 기존 실제 손 부착·손가락 잡기·팔 길이 제한을 사용한다. 걷는 동안 팔 도달 제한 때문에 목표와 손 사이의 오차가 남을 수 있으며 뼈를 늘리지 않는다.
- 1인칭은 기존 0.9.9 잡는 위치 **(0.27, -0.34, 0.46)m**와 회전 **(-12, 180, -18)°**를 사용하며 앞으로 **0.18m**만 뻗는다. 벽 깊이 보정은 봉의 전방 축 투영과 **0.04m** 여유를 검사하고 깊이만 당긴다. 최소 배율 **0.35**를 유지한다. 전신 1인칭 팔·모든 모서리 관통 해결은 아직 없다.
- 운반·다운·유효한 구조·비활성/미접속 직원에서 봉과 공격 자세를 차단한다. 중단된 공격을 재생하지 않고 기본 자세로 돌아간다. 구조/공격 동시 입력은 구조 우선이다. 공격 범위·방향·시야·리스너 기절·물리 판정은 유지한다.

## 제작과 검증

[팔 동작](../../NoReturns/Assets/_NoReturns/Runtime/CarryLab/EmployeeVisual.Baton.cs) · [손 부착·1인칭](../../NoReturns/Assets/_NoReturns/Runtime/CarryLab/BatonVisual.cs) · [표본 검사](../../NoReturns/Assets/_NoReturns/Editor/EmployeeBatonReview.cs) · [현재 검증 기록](../validation/employee-pulse-locomotion-0.9.12.json). Unity의 기존 Humanoid 위에서 적용하며 새 FBX/클립·Blender 원본 수정·MCP 편집은 없다. 점프/착지는 [별도 현재 안내](employee-locomotion.ko.md)를 따른다.

대기/걷기 × 방향 0°/90°/135° × 준비 및 60fps 기준 0–0.30초 20시점, **120개** 표본을 통과했다. 손/봉 전진 최대 **0.200000m**, 좌우/상하 경로 오차 최대 **0.021758m**, 봉 방향 변화 **0°**, 가슴 비틀기 **0°**, 목표 오차 최대 **0.041788m**, 뼈 길이 변화 최대 **0.000000358m**다. 목표 오차는 팔 도달 제한이며 무기가 손에서 떨어진 거리가 아니다. 최초 0.24m 전진은 도달 제한으로 경로 오차 0.037240m가 발생해 검사에 실패했다. 전진을 0.20m로 줄인 뒤 같은 검사를 통과했다.

정적 검토: [기본 자세](../../art/player-employee-01/unity-review/Pulse-Ready.png) · [전진](../../art/player-employee-01/unity-review/Pulse-3.png) · [유지](../../art/player-employee-01/unity-review/Pulse-6.png) · [회수](../../art/player-employee-01/unity-review/Pulse-12.png) · [복귀](../../art/player-employee-01/unity-review/Pulse-18.png). 실제 게임 캡처나 사용자 품질 승인이 아니다.

맥 빌드 오류 **0개·기존 경고 7개**와 화면 없는 두 클라이언트 전방 동작/운반 차단 **12개**를 통과했다. 실제 봉의 몸 기준 전진은 **0.153486–0.200003m**, 경로 오차 최대 **0.005647m**, 방향 변화 최대 **0.001645°**였다. 걷기 중 손의 월드 이동에는 직원 이동이 포함되므로 공격 거리로 사용하지 않는다. 추가 동료·리스너 검사 범위는 위 검증 기록에서 확인한다. CLI **1.0.0-beta.12**, Pipeline **0.8.0-exp.1**, Editor **6000.6.4f1**의 공식 최신 배포 일치를 확인했다.

```sh
python3 tools/cinder_four_player.py build
python3 tools/test_employee_baton.py
python3 tools/test_cinder_companion.py
python3 tools/test_cinder_threat.py --headless
```

수동 시험을 종료한 뒤 새 수동 창/Editor Play를 열지 않았다. “직접 테스트 해볼게” 요청 시에만 [현재 실행 규칙](companion-play.ko.md)으로 두 창을 연다. 수정본 사용자 품질·급격한 시선/모서리·배송/억제 전체 회귀·Windows/LAN/성능은 미확인이다. 후진/옆걸음·구조/다운 전용 클립·1인칭 팔은 남아 있다. 이전 [0.9.10 검증](../validation/employee-baton-thrust-0.9.10.json)과 [0.9.9 검증](../validation/employee-baton-0.9.9.json)은 과거 기록이다.

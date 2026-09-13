# SIDE EFFECTS 첫 협동 프로토타입 구현 기록

[English](17-side-effects-implementation.en.md)

> **과거 기록 · 2026-09-12 폐기/대체.** 현재 게임은 [PSX 우주 배송 기획](01-overview.ko.md)을 따른다. 아래 구현·실행·아트·수치는 과거 자료이며 현재 상태가 아니다. 삭제된 파일은 경로 기록으로만 남긴다.


> 보관 기록 · 2026-09-12 개발 중단. 현재 실행·구현 명세가 아니다. 제거된 파일 링크는 원본이 들어 있는 압축본으로 연결한다. [Unity 메인 / Mainline](18-unity-mainline.ko.md).

2026-09-11 · SE-PROT-01 · 개정 2 · 첫 프로토타입 구현·자동 검증 완료. 사람 검증과 출시 준비는 미완료다.

[인수인계서](16-side-effects-handoff.ko.md)의 승인된 짧은 실험실 범위를 현재 코드와 연결한다. 자동 규칙 검사, 실제 여러 프로세스 연결, 렌더 확인, 사람 재미 검증을 구분하며 이 문서만으로 단계 최종 통과를 선언하지 않는다.

## 실행과 조작

07_SIDE_EFFECTS.cmd (`../archive/side-effects-2026-09-12.zip`; retired file)를 실행한다. 전용 결과물은 SIDE_EFFECTS.exe (`../archive/side-effects-2026-09-12.zip`; retired file), 저장 장면은 SideEffectsLab.unity (`../archive/side-effects-2026-09-12.zip`; retired file)다. HOST 또는 F1으로 방을 열고 다른 실행 창에서 주소를 입력하여 JOIN한다. 같은 PC는 `127.0.0.1` 또는 F2, 기본 UDP 포트는 7778이며 호스트 포함 최대 4명이다.

연결 후 RESUME CONTROLS 또는 게임 공간을 클릭해야 마우스가 고정된다. Esc는 커서를 해제하고 재진입 클릭은 공격하지 않는다. WASD 이동, 마우스 시점, Space 점프, Shift 회피, 좌클릭 기본 공격, 우클릭 현재 도구, E 유지 구조·스위치, Q 도움 요청, F 다운 동료 끌기 전환이다. 연습에서는 1/2/3으로 지팡이/방패/망치를 선택하거나 Esc 후 버튼을 누른다. 전투 도중 자유 교체는 없다.

호스트가 Esc로 커서를 해제한 뒤 BEGIN EXPEDITION을 누르면 연결된 참가자 전원을 전투 방으로 옮기고 즉시 적을 생성한다. 전원이 두 번째 공간에 걸어 들어오는지 검사하는 입장 트리거나 준비 투표는 없다. 혼자 호스트여도 시작할 수 있다. 보상은 KEEP CURRENT 또는 SWAP TOOL이며 다운 참가자도 선택할 수 있다. 결과·전멸에서는 다운 호스트도 RESTART EXPEDITION을 누를 수 있다.

## 현재 동작

SeLoop.cs (`../archive/side-effects-2026-09-12.zip`; retired file)의 흐름은 연습 0 → 전투 1 → 보상 2 → 이동 3 → 결과 4이며 전투·이동에서 전원 다운이면 전멸 5다. 현재 참가자가 모두 보상을 선택하면 이동 방으로 진행한다.

- 연습에는 벽·경사로·가장자리와 금속·비금속 소품을 둔다.
- SeCombat.cs (`../archive/side-effects-2026-09-12.zip`; retired file)의 전투는 금속 근접 기사 2명과 원거리 적 1명, 총 2종·3명이다. 공격 예고·엄폐물이 있으며 적을 모두 처치하면 보상을 제시한다.
- SeLabBuilder.cs (`../archive/side-effects-2026-09-12.zip`; retired file)의 중앙 간격은 4.5m다. 출발 발판 z44–50, 착지 발판 z54.5–64, 오른쪽 걷기 우회로 x8–11·z48–57이다. 중앙은 점프와 뒤에서 받는 지팡이 충격을 함께 쓰는 지름길이다.
- 스위치 `(0, 0.1, 59)`에서 E로 작동시킨 뒤 생존자 한 명이 출구 `(0, 0.1, 63)`의 2m 안에 들어오면 팀 결과가 나온다. 전원이 출구에 모일 필요는 없고 다운 동료는 결과의 팀 상태에 남는다.
- 새 세션은 시드를 1–999999에서 고른다. 실행 인자 `--se-seed`로 고정할 수 있다. 재시작은 같은 시드를 유지하여 같은 조건에서 다시 배울 수 있게 한다. 보상 후보와 돌풍 선택은 호스트가 결정하며 물리 궤적의 완전 일치는 보장하지 않는다.
- 돌풍이 있는 이동 방은 8초 주기의 6–8초 동안 z49–58의 생존자를 오른쪽으로 7m/s² 가속한다. HUD는 직전 1초를 경고한다.

낙하는 마지막 안전 지점에서 다운되는 방식이다. 다운 중 공격은 막고 기어가기·도움 요청은 유지한다. E 구조는 거리·가림을 유지해야 하며 이탈하면 취소된다. 연습·전투·이동 진입 시 체력과 다운·외부 속도·구조 진행·방패를 초기화한다. 재시작은 도구·기여 기록도 초기화한다. 결과의 BOOST-ASSISTED SWITCHES는 실제 새 지팡이 충격 뒤 4초 이내의 스위치 작동 횟수이며 플레이어 의도나 재미를 자동 판정하지 않는다.

## 설정값과 도구

아래 전체 값의 코드 근거는 SeTuning.cs (`../archive/side-effects-2026-09-12.zip`; retired file), 저장 설정은 SeLabTuning.asset (`../archive/side-effects-2026-09-12.zip`; retired file)이다. 전체 필드를 자산에 저장했다. 거리 m, 속도 m/s, 시간 s, 각도 °이며 반각은 정면 양쪽에 적용한다. 출시 밸런스가 아닌 초기 실험값이다.

| 설정 | 값 |
|---|---|
| 최대 체력 / 이동 / 기어가기 | 100 / 5.5 / 1.2 |
| 점프 높이 / 중력 | 1.3 / -24m/s² |
| 회피 속도 / 지속 / 재사용 | 10 / 0.18 / 0.9 |
| 외부 속도 지수 감쇠 / 낙하 판정 y | 3s⁻¹ / -8 |
| 지팡이 범위 / 반각 / 회복 | 6 / 35 / 30 |
| 지팡이 수평 충격 / 상승 충격 / 재사용 | 9 / 3.8 / 0.8 |
| 아군 연속 밀치기 보호 | 0.65 |
| 구조 범위 / 유지 / 부활 체력 | 2.2 / 2 / 35 |
| 끌기 범위 / 속도 | 2.5 / 3 |
| 기본 공격 범위 / 피해 / 재사용 | 2.3 / 20 / 0.35 |
| 망치 범위 / 반각 / 피해 | 3 / 55 / 50 |
| 망치 적 충격 / 사용자 반동 / 상승 / 재사용 | 13 / 12 / 3 / 1.1 |
| 자석 범위 / 반각 / 당김 가속 | 6 / 60 / 12m/s² |
| 방패 지속 / 재사용 / 방어 반각 | 1.2 / 2 / 60 |
| 적 공격 예고 / 이동 속도 / 피해 | 0.9 / 2.2 / 18 |

지팡이는 만피 아군도 밀며 연속 보호 중에는 회복을 유지하고 추가 밀치기만 차단한다. 자석 방패는 정면을 방어하면서 금속 적·소품만 끌고 벽 너머 대상은 제외한다. 망치는 전방 타격과 별도로 사용자에게 후방 반동을 준다. 입력 속도와 외부 속도를 분리하여 이동 입력이 반동을 지우지 않는다. 기본 공격과 도구는 현재 같은 쿨다운 상태를 공유한다.

## 네트워크와 제작 경로

manifest.json (`../../unity/NoReturns/Packages/manifest.json`; retired file)의 공식 Unity Transport 6.6.0을 사용한다. Unity 버전 근거는 ProjectVersion.txt (`../../unity/NoReturns/ProjectSettings/ProjectVersion.txt`; retired file)다. SeTransport.cs (`../archive/side-effects-2026-09-12.zip`; retired file)는 신뢰성 있는 순차 전송·분할·시험용 지연·손실을 제공한다. SeGame.cs (`../archive/side-effects-2026-09-12.zip`; retired file)는 발신 소유권·순번·epoch·유한 입력·다운·쿨다운을 검사하고 호스트가 도구·범위·가림·실제 대상을 결정한다. 클라이언트는 명중·체력을 지정하지 않는다.

클라이언트 예측·보간은 없다. 호스트 결과를 그대로 적용한다. 전송 누산 조건은 0.05s지만 Fixed Timestep (`../../unity/NoReturns/ProjectSettings/TimeManager.asset`; retired file)이 0.02s이고 송신 후 누산기를 0으로 만들므로 정상 주기는 약 0.06s·16.7Hz다. 20Hz로 보고하지 않는다. `maxSnapshotStep`는 연속 수신 위치 사이 최대 변위이며 예측 오차·보정량이 아니다. 지연 환경의 조작감은 별도 사람 확인이 필요하다.

호스트 이탈은 종료 안내로 처리하며 호스트 이전은 없다. 참가자 이탈 시 입력·순번·끌기·보상·충격 표식을 정리하고 빈 ID 1–3을 재사용한다. F1/F2 또는 접속 버튼으로 다시 연결한다. 이 경계의 코드 구현과 최신 전체 실행본 검증은 구분한다.

[unity_mcp.py](../../tools/unity_mcp.py)는 공식 Unity CLI stdio MCP 서버에 `initialize`·`tools/list`·`tools/call`을 전달하는 로컬 IPC 중계다. 게임 멀티플레이 서버와 다르다. 장면 생성·확인은 MCP, 격리 검사·빌드는 CLI를 사용한다. 추가 계정·매치메이킹·Steam·영구 저장은 없다. SIDE EFFECTS/Create Lab은 별도 장면을 만들고 SIDE EFFECTS/Build Windows는 명시적인 장면 목록으로 전용 출력에 빌드한다. CarryLab·기존 실행 파일·기존 빌드 장면 설정을 보존한다.

## 검사와 증거

저장소 루트에서 실행한다. side-effects.ps1 (`../archive/side-effects-2026-09-12.zip`; retired file)은 공식 Unity CLI와 해당 Editor가 필요하다. 사용자의 편집기를 저장 없이 종료하지 않는다.

```powershell
powershell -ExecutionPolicy Bypass -File tools/side-effects.ps1 rules
powershell -ExecutionPolicy Bypass -File tools/side-effects.ps1 combat
powershell -ExecutionPolicy Bypass -File tools/side-effects.ps1 traversal
powershell -ExecutionPolicy Bypass -File tools/side-effects.ps1 build
python tools/run_side_effects_tests.py
python tools/run_side_effects_tests.py --players 2 --delay 50 --loss 1
python tools/run_side_effects_tests.py --players 4 --delay 50 --loss 1 --loop --seed 7341
```

네트워크 검사 도구 (`../archive/side-effects-2026-09-12.zip`; retired file)는 실제 Windows 프로세스를 `-batchmode -nographics`로 실행한다. `--delay 50`은 송신 측 지연이며 양방향 적용의 명목 왕복 지연은 100ms다. `--loss 1`은 송신 패킷 손실 1%이며 출시 합격선이 아니다. 검사 기본 시드는 7341이다. 각 프로세스 JSON·로그와 summary.json에 종료 코드·최종 상태 일치를 기록한다. 아래 RTT는 재접속 전후 수신한 ping 응답의 평균이며 명목 지연에 프레임·처리 시간이 더해진다.

| 검사 | 현재 결과와 한계 |
|---|---|
| A 규칙 / CarryLab 회귀 | MCP 및 최종 배치 규칙 검사 (`../archive/side-effects-2026-09-12.zip`; retired file) 통과(끌기·벽·거리 포함) / 기존 24개 통과. 회귀 로그 (`../archive/side-effects-2026-09-12.zip`; retired file) |
| B 실제 2개 프로세스·정상 조건 | A/B 빌드 통과. 요약 (`../archive/side-effects-2026-09-12.zip`; retired file) |
| B 실제 2개 프로세스·50ms·1% | A/B 빌드 통과. 요약 (`../archive/side-effects-2026-09-12.zip`; retired file) |
| B 실제 4개 프로세스·50ms·1% | A/B 빌드 통과. 요약 (`../archive/side-effects-2026-09-12.zip`; retired file) |
| C 전투 규칙 | 통과. combat-tests.log (`../archive/side-effects-2026-09-12.zip`; retired file) |
| 실제 저장 장면의 4.5m 이동 비교 | 통과. 일반 걷기·기본 점프 낙하, 점프+지팡이 착지 z56.165·체력 100·충격 1회, 오른쪽 걷기 스위치·출구 접근. traversal.log (`../archive/side-effects-2026-09-12.zip`; retired file) |
| 최신 전체 기능 Windows 빌드 | 생성·실행 확인 완료. 빌드 로그 (`../archive/side-effects-2026-09-12.zip`; retired file) |
| 최신 2/4개 프로세스 전체 루프 | 정상·50ms·1% 조건 모두 통과. 아래 최신 결과 참조 |
| 화면 / 사람 / 별도 PC | 호스트·클라이언트 전투·이동 화면 확인 / 실제 사람 완주·재미·별도 PC 미검증 |

B는 양방향 회복·밀치기, 중복·범위, 다운·부분 체력 구조, 만피 밀치기·보호 중 회복을 검사한다. `--loop`는 보상·스위치·결과·재시작·전멸 등의 진행을 확인하되 검사 준비 코드로 적을 제거하고 플레이어를 목표 위치로 옮긴다. 실제 전투 피해 규칙 검사, 저장 장면의 물리 이동 검사, 사람의 전투→출구 완주는 각각 별도 증거다.

렌더 표본은 실행 파일에 `--se-capture <절대 PNG 경로> --se-capture-after 4`를 넘겨 저장한다. `--se-capture-only`는 저장 후 종료한다. 그래픽을 끄는 옵션과 함께 쓰지 않는다. 이것은 카메라·HUD 렌더 결과이며 사람 조작 검증은 아니다.

## 사람 플레이 절차와 미완료

1. 두 사람이 연습에서 최소 설명으로 첫 회복 밀치기를 경험하고 사고 원인·착지 예상 위치를 설명한다.
2. 역할을 바꾸어 두 번째 시도의 점프 시점·발사 방향·착지 목표를 합의한다. 첫 실수 뒤 계획이 바뀌었는지 기록한다.
3. 중앙 지름길과 오른쪽 걷기 우회, 동일 공간의 돌풍 유무를 비교한다. 의도적 활용과 단순 재시도를 구분한다.
4. 양방향 다운·끌기·구조를 경험하고 구조가 대기·반복 노동인지 묻는다. 다운 호스트 결과·전멸 재시작·이탈·재접속을 확인한다.
5. 실제 입력으로 전투→보상→이동→결과를 완주한다. 사람 설명·관찰·영상과 자동 로그를 분리한다.

현재 아트는 남색·청록 배경, 민트 회복, 호박색 위험 패턴과 번호·모자·망토·큰 장갑·도구 형태를 쓰는 임시 프리미티브다. 벽에서 잘리는 범위 예고와 활성 방패 표시도 최종 아트가 아니다. 사람 재미, 별도 PC 네트워크, 지연 조건의 조작감, 출시 품질은 미완료다.


## 최종 다중 프로세스 결과

각 실행은 동일 PC의 실제 Windows 플레이어 프로세스다. 아래 모든 경우 최종 체력·다운·도구·충격 횟수·구조·위치가 일치하고, 호스트의 19개 시나리오 검사가 통과했다. 범위 밖 요청 수신, 구조 진행 후 취소, 스위치 없이 출구 접근, 다운 중 보상 선택·방장 재시작, 실제 이탈·재접속을 포함한다.

| 인원 | 송신 지연 / 손실 | 참가자 평균 RTT(ms) | 최대 이동 스냅샷 간격(m) | 결과 |
|---|---|---|---|---|
| 2 | 0ms / 0% | 55.8 | 0.493 | PASS (`../archive/side-effects-2026-09-12.zip`; retired file) |
| 2 | 50ms / 1% | 122.8 | 0.427 | PASS (`../archive/side-effects-2026-09-12.zip`; retired file) |
| 4 | 0ms / 0% | 50.0, 51.3, 50.0 | 0.458 | PASS (`../archive/side-effects-2026-09-12.zip`; retired file) |
| 4 | 50ms / 1% | 135.2, 133.4, 118.0 | 0.493 | PASS (`../archive/side-effects-2026-09-12.zip`; retired file) |

위치 값은 일반적인 입력 예측 보정량이 아니다. 자동 검사의 이동 단계에서 연속 호스트 스냅샷 사이 실제 변위를 측정하고, 준비용 순간이동·방 전환·최종 고정은 제외했다. 클라이언트 입력 예측·재시뮬레이션은 아직 없어 지연된 입력 반응은 P1 후속 작업이다.

같은 저장 맵의 돌풍 비교에서 x=-4.000 정지 조건과 x=-2.380 돌풍 조건을 확인했다. 통합 검사 요약 (`../archive/side-effects-2026-09-12.zip`; retired file), 이동 호스트 화면 (`../archive/side-effects-2026-09-12.zip`; retired file), 이동 참가자 화면 (`../archive/side-effects-2026-09-12.zip`; retired file), 전투 화면 (`../archive/side-effects-2026-09-12.zip`; retired file). 화면은 자동 접속·지정 방 배치 표본이며 실제 사람의 완주 영상이 아니다.

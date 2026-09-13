# NR-LOOP-01 — 작은 배송 루프 실행·검증

[English](22-slapstick-loop.en.md)

> **과거 기록 · 2026-09-12 폐기/대체.** 현재 게임은 [PSX 우주 배송 기획](01-overview.ko.md)을 따른다. 아래 구현·실행·아트·수치는 과거 자료이며 현재 상태가 아니다. 삭제된 파일은 경로 기록으로만 남긴다.


2026-09-12 · Unity 실험 구현. [승인된 인계 범위](21-slapstick-loop-handoff.ko.md)를 실제 게임으로 연결했다. 출시 완성도나 사람의 재미 검증 완료를 뜻하지 않는다.

## 실행과 조작

07_NoReturns_Loop.cmd (`../../07_NoReturns_Loop.cmd`; retired file)를 실행한다. 기존 06 실행 항목은 CarryLab이며 그대로 보존했다. 새 실행본은 `build/loop/Windows/NO_RETURNS_LOOP.exe`이다. 다른 PC에는 해당 Windows 폴더 전체를 복사한다.

- Solo: 혼자 시작한다.
- 2인: 한 명은 Host / Begin을 누르고, 다른 한 명은 방장 PC 주소를 입력해 Join address를 누른다. 연결 확인 후 방장이 Host / Begin을 다시 누른다. 같은 PC 검사 주소는 `127.0.0.1`이다.
- 직접 연결 포트는 TCP 27831이다. 별도 PC는 서로 접근 가능한 네트워크와 해당 포트의 통신 허용이 필요하다. Steam 초대나 인터넷 중계는 포함하지 않는다.
- 저장은 실행 중에만 유지한다. 프로세스를 종료하면 사라진다.

| 조작 | 동작 |
|---|---|
| WASD / 마우스 / Space | 이동 / 시점 / 점프 |
| E | 가까운 화물 들기·놓기, 수레 적재·하역 또는 손잡이 잡기·놓기; 현재 가능한 동작을 HUD에 표시 |
| 좌클릭 누르기·떼기 | 충전 후 던지기 |
| Esc / 우클릭 | 커서 해제 / 시점 조작 복귀 |
| B, 트럭 근처 | 수레 구매 |
| R, 트럭 근처 | 미배송 화물·수레 복구 |
| F | 자신의 직원을 복귀시키고 손의 화물은 현장에 놓기 |
| Enter, 트럭 근처 | 방장이 정산; 미완료 건수 표시 |
| 결과 화면 | 방장이 같은 시드 또는 새 시드로 재도전; 메뉴로 나가기 |

## 현재 규칙

일반 A·B 각 30 → 수레 60 구매 → 재채기 A·B 각 90 → 트럭 정산이다. 정상 완료는 총수입 240, 투자 60, 잔액 180이다. 수레 구매 전 고가 주문의 단가와 재채기 위험을 보여준다. 가격과 8–12분 관찰 목표는 실험값이며 강제 시간 제한은 없다.

화물 ID와 목적지가 맞고 손 소유권이 해제된 상태로 수령 구역에 1초 머무르면 한 번 지급한다. 수레 적재 상태에서도 납품한다. 오배송·반복 납품·반복 구매는 지급하거나 추가 차감하지 않는다. 고가 화물은 순서대로 출고한다. 정산은 진행을 멈추며 이미 확정된 수입을 유지한다. 재시작은 지갑·배송·직원·수레·화물·재채기 타이머를 초기화한다.

재채기는 최초 집기·적재부터 활성화된다. 8–12초 간격, 분출 전 1.2초 얼굴 변화와 `AH... AH...` 문구가 있고 임시 분출음이 난다. 전방 3m, 반각 45도와 벽 가림을 검사한다. 초기 외력은 수평 3m/s, 위 1.5m/s이며 직원은 외력을 누적한 뒤 감쇠한다. 직원 피격 시 손 화물을 놓는다. 수레 적재 연결은 유지한다. 회수나 내려놓기로 활성 타이머를 초기화하지 않는다. 완료 화물은 비활성화된다. 들숨 음향의 별도 제작은 남아 있다.

수레는 동적 차체와 제한된 힘의 연결로 화물 하나를 운반한다. 직원 목표 위치와 이동 속도를 따라가며 월드 충돌을 유지한다. 넓은 통로에서 수레 운반의 직원 속도는 2.8m/s, 손 운반은 기존 1.6m/s이다. 물리 검사에서 수레를 밀며 2초 동안 5.432m 이동했고 적재를 유지했다. 이는 직선 자동 측정으로 전체 배송 시간 절감을 증명하지 않는다.

## 제작 위치와 보존 범위

별도 장면 (`../../unity/NoReturns/Assets/_Project/SlapstickLoop/Scenes/SlapstickLoop.unity`; retired file)을 Unity에서 연다. 고정 배송장은 48×50m이며 A의 넓은 우회로·좁은 골목과 B의 낮은 턱과 수레용 경사로가 있다. 장면의 벽·목적지·트럭·직원을 직접 편집하고 저장할 수 있다. 실제 동작 수치는 LoopWorld (`../../unity/NoReturns/Assets/_Project/SlapstickLoop/Runtime/LoopWorld.cs`; retired file), 경제는 LoopRules (`../../unity/NoReturns/Assets/_Project/SlapstickLoop/Runtime/LoopRules.cs`; retired file), 수레는 LoopTrolley (`../../unity/NoReturns/Assets/_Project/SlapstickLoop/Runtime/LoopTrolley.cs`; retired file)에 있다. 목표 지역 수와 맵 크기는 출시 확정값이 아니다.

LoopBuilder (`../../unity/NoReturns/Assets/_Project/SlapstickLoop/Editor/LoopBuilder.cs`; retired file)는 새 장면을 생성하며 기존 파일이 있으면 연다. `Polish scene`은 이 실험의 초기 표지판·트럭 위치 보정 명령이므로 사용자 편집 뒤 임의로 다시 실행하지 않는다. 빌드는 새 장면만 명시하며 기존 기본 빌드 장면 설정을 바꾸지 않는다.

CarryLab·Godot·기존 아트·다른 미커밋 변경을 유지했다. 공용 WorkerController에는 선택적 외부 입력과 외력·개별 복귀를, CargoBody에는 선택적 자동 복구와 명시적 최초 위치 기록을 추가했다. 기존 기본 동작은 유지한다. 새 지역·최종 아트·전투·전체 래그돌·영구 저장은 없다.

## 온라인 기술과 한계

LoopWire (`../../unity/NoReturns/Assets/_Project/SlapstickLoop/Runtime/LoopWire.cs`; retired file)의 .NET TCP 직접 연결을 사용한다. 기존 Unity 환경의 관리형 .NET API로 Windows 2인 실험을 구성하며 패키지·서비스 계정을 추가하지 않았다. [Unity .NET 지원](https://docs.unity3d.com/6000.0/Documentation/Manual/dotnet-profile-support.html), [Microsoft 소켓 API](https://learn.microsoft.com/en-us/dotnet/api/system.net.sockets?view=netframework-4.8.1)를 근거로 선택했고 실제 Windows 빌드에서 확인했다.

방장은 물리·배송·지갑·구매·재채기·정산을 확정한다. 참가자는 입력 요청을 보내고 20Hz 상태를 받는다. 같은 규칙 코드를 솔로에서도 사용한다. 입력 행동은 물리 갱신까지 누적해 빠른 입력이 유실되지 않게 한다. 참가자 이탈 시 소유권을 풀고 솔로를 이어가며, 방장 이탈 시 안내 후 메뉴로 돌아간다. 중도 참가·재접속·방장 이전은 지원하지 않는다.

현재 참가자는 위치 예측·보간 없이 상태를 표시하므로 지연 환경의 조작감과 움직임 매끄러움은 남은 과제다. TCP 전송 테스트와 게임 동작 검사는 별도이며 출시용 온라인 품질을 보증하지 않는다.

## 검사와 근거

- `powershell -NoProfile -File tools/loop.ps1 test`: 종료 코드 0. 경제 28개, 기존 CarryLab 24개, 새 통합 검사 18개와 수레 검사 6개 및 물리 API 확인. 로그 (`../../artifacts/loop/test.log`; retired file).
- `powershell -NoProfile -File tools/loop.ps1 build`: Windows 빌드 성공, 종료 코드 0. 로그 (`../../artifacts/loop/build.log`; retired file).
- `python tools/run_loop_probe.py`: 솔로 및 같은 PC의 독립 호스트·참가자 실행본 검사. 솔로 (`../../artifacts/loop/solo.json`; retired file), 호스트 (`../../artifacts/loop/host.json`; retired file), 참가자 (`../../artifacts/loop/client.json`; retired file).
- 두 프로세스에서 참가자의 집기 요청으로 소유권을 넘겼고, 재채기 외력·변위·참가자 복귀 요청을 확인했다. 양쪽 배송·구매·정산 값이 일치했다.
- 같은 실행에서 양쪽 재시작 초기화와 방장 종료 후 참가자 메뉴 복귀도 확인했다. JSON의 경제 수치는 재시작 이전 완료 상태를 보존한 값이며, 재시작 결과는 별도 필드다. 실행본 정산 화면 (`../../artifacts/loop/solo.png`; retired file), 참가자 실행 화면 (`../../artifacts/loop/client.png`; retired file).
- 자동 검사는 목적에 맞게 직원·화물 위치를 배치한다. 사람이 전체 경로를 걸은 검사가 아니며, 재채기 피격 검사 일부는 분출을 직접 호출한다.
- `powershell -NoProfile -File tools/test_loop_wire.ps1`: 독립 TCP 연결·양방향 전달·연결 해제·메시지 제한 검사.

## 남은 검증과 다음 판단

- [ ] 서로 다른 PC의 직접 연결과 실제 통신 지연·연결 단절 검증.
- [ ] 2인 손 조작으로 우회로·골목·턱을 오가며 수레를 오래 운반하고 같은 시드·새 시드 재도전.
- [ ] 첫 구매 시간, 수레 구매의 체감 효과, 건당 운반·회수 시간 측정.
- [ ] 예고를 읽고 사고를 이해하는지, 동료 때문에 경로·역할을 바꾸는지, 자발적으로 다시 하고 싶은지 관찰.
- [ ] 자연 발생 재채기의 들숨 소리와 예고 가독성, 참가자 움직임 보간 조정.

사람 검증은 미실행이다. 장비나 지역을 늘리기 전에 위 기록으로 예고·수레·경로를 조정한다. [인계 문서의 관찰 기준](21-slapstick-loop-handoff.ko.md#재미-검증과-완료-조건)을 사용한다.

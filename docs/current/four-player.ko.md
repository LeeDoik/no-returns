# 4인 협동 — 현재 규칙·검증

[English](four-player.en.md)

## 2026-10-03 — 직원 전신·대기/걷기 0.9.5

[현재 연결·로컬 재현·검증 범위](employee-animation.ko.md). 보정 Idle와 Walking을 별도 Humanoid Avatar로 가져와 실제 직원 전신과 이동 속도 기반 전환에 연결했다. 각 창은 자기 몸을 숨기고 팀 색을 입힌 동료 세 명을 표시한다. 게임 0.9.5·프로토콜 13·TCP 27842. 동작 원본과 생성된 Unity 에셋은 로컬 보관하며 공개 Git에는 재현 코드·검사 기록·정적 화면을 둔다. 아래 과거 Unity/대기·걷기 미적용 설명을 이 범위에서 대체한다. 운반 손 접촉·전용 추가 동작·1인칭 팔·사용자 품질/다른 환경 검토는 남아 있다.

## 2026-10-02 — Cinder 억제기·외곽 생물 0.9.4

[현재 규칙·제작·실행 안내](cinder-suppression.ko.md). 현재 억제기 4개와 경계선에 기존 90/135/180초 단계·신호음·작업등 약화를 연결하고, 정지 8초 뒤 외곽 생물의 동쪽 진입·장애물 우회·추적을 연결했다. 하늘/안개/태양·원본 아트·충돌은 보존한다. 기본 실행은 배송+리스너+억제/외곽이며 `--delivery-only`/`--map-only` 회귀는 유지한다. 버전 0.9.4·프로토콜 13·TCP 27842. 구매 신호기 유인과 위험 속 귀환·공용 다운/선내 보호·긴급 회수·다음 근무를 실제 네 실행본 60개 검사로 확인했다. [검증 기록](../validation/cinder-suppression-0.9.4.json). 아래 과거 억제/외곽 미연결 설명을 이 범위에서 대체한다. 사람 품질·최종 생물/음향·단서/진행 저장·다른 환경/성능은 미완료다.

## 2026-10-02 — Cinder 리스너·진압봉·구조 0.9.3

[현재 규칙·실행·제작 검사](cinder-listener.ko.md). 기본 Cinder 배송에 리스너 1개의 실제 바닥/장애물 격자 이동·소음 조사·경고/공격, 기존 빈손 좌클릭 진압봉, E 유지 구조·전원 다운 선내 회수를 연결했다. 정상 배송 420 CR과 원본 아트 씬을 보존한다. 선내 안전을 현재 Cinder 좌표로 판정하고 옛 억제/외곽/단서/저장 객체는 생성하지 않는다. 신호기 신호는 기존 리스너 소음 조사에도 연결된다. `--delivery-only`로 위험 없는 배송 회귀, `--map-only`로 이동 시험을 유지한다. 버전 0.9.3·프로토콜 12·TCP 27842. 아래 과거 Cinder 리스너/진압봉/구조 미연결 상태를 이 범위에서 대체한다. 억제/외곽·단서·진행 저장과 사람 품질 평가는 남아 있다. [실제 검증 범위](../validation/cinder-listener-0.9.3.json).

## 2026-10-01 — 조작·우주선·구매 UI 0.9.2

[현재 조작과 실행 안내](controls-ui.ko.md). E 대상 사용/선내 단말, 좌클릭 바닥 배치, 우클릭 유지 화물 회전, 휠 0.75–1.6m 거리, Q 즉시 놓기. 출발·귀환·구매를 실제 버튼으로 분리하고 탑승 인원·잔액·비활성 사유·0 CR 귀환 확인을 표시한다. Esc 조작 설정에 13개 버튼 재설정·감도·FOV·언어/기본값을 제공한다. 메뉴는 세계를 멈추지 않으며 게임 입력을 차단한다. Cinder 신호기 120 CR 구매·공유 운반·2회/8초 신호를 연결한다. 버전 0.9.2·프로토콜 11·TCP 27842. 기존 원본 맵/배송 보수는 유지한다. 아래의 E 자동 진행·Esc 구매 및 Cinder 신호기 미연결 상태를 이 구현 범위에서 대체한다. 리스너/진압봉/억제/저장 통합과 사람 품질 평가는 남아 있다. [실제 검증 범위](../validation/controls-ui-0.9.2.json).

## 2026-10-01 — Cinder 배송·영수증·귀환 정산

**CINDER-DELIVERY-01 / 구현·자동 검증 범위는 아래 근거 참조.** 배송 장부와 CRT를 현재 맵의 별도 `CinderFourPlayerTest`에 연결했다. 원본 `CinderCompactSiteReview`의 아트/건물/하늘/물리 배치를 보존한다. 이후 재빌드는 원본을 복사하고 기존 수령 모델 1개·BoxCollider 1개·피드백·물리 표지 2개를 더한다. 원래 정적 화물 46개·CRT 3개를 기능 화물로 바꾸지 않는다. 새 모델/텍스처/패키지 없음. 버전 **0.9.1**, 프로토콜 **10**, TCP **27842**, 렌더 상한 **30fps**, 물리 **50Hz**, 카메라 **250m** 유지. [빌드 소스](../../NoReturns/Assets/_NoReturns/Editor/CinderFourPlayerBuild.cs) · [배송 장부](../../NoReturns/Assets/_NoReturns/Runtime/CarryLab/CarryMission.cs).

1. [07_Play_Cinder_4P.command](../../07_Play_Cinder_4P.command)를 실행하면 방장 1명과 참가자 3명이 배송 준비 상태로 시작한다. 우주선 경사로를 올라가 E로 항로를 선택한다. 전원이 실제 선내로 들어온 뒤 다시 E로 도착한다. 비행 연출은 없다.
2. 밖의 밀봉 화물을 E로 들고 BAY 04로 이동한다. 바닥 표시 중심 **(17,0,12.4)m**, 외곽 **3×2m**. 화물 중심 **x=15.95~18.05, z=11.8~13, y=0.2~0.65m**의 열린 범위에서 손을 뗀 채 속도 **0.2m/s 미만**으로 **0.75초** 안정되어야 수령한다. Q로 내려놓는다. 들고 있는 화물은 수령하지 않는다.
3. **0.75초** 인쇄 후 단말 중심 **(14.9,0.8,12.4)m**를 바라보고 **2.4m** 안에서 E로 영수증을 회수한다. 누구나 회수할 수 있는 팀 공용 상태이며 손을 차지하지 않는다. 배송/회수 시점에는 지급하지 않는다.
4. 전원이 선내로 돌아온 뒤 E로 귀환한다. 기본 수령 **300 + 귀환 120 = 420 CR**을 한 번 지급하고 HUD에 내역을 표시한다. 회수 없는 귀환/도중 접속 이탈은 새 보수 0이며 기존 잔액은 유지한다. E로 다음 근무를 준비하면 화물/직원 출발 위치와 영수증 상태를 초기화한다. 종료 후 잔액 복원은 이 모드에 없다.

탑승 범위는 **|x+20.7|<1.5, -30.9<z<-24.8, 0.8<y<3m**이다. 실제 선내 바닥의 높이는 1m이며 경사로/밖에서 누르는 E는 출발/귀환을 진행하지 않는다. 수령 장치를 중앙 이동축 밖에 두어 기존 BAY 04 남북 이동을 유지한다. 원본 아트 씬은 기존 단독 검토를 유지한다. Unity Editor에서 배송을 시험하려면 파생 씬 `CinderFourPlayerTest`를 열고 Play → HOST/JOIN을 선택한다.

```sh
source ~/.unity/env
python3 tools/cinder_four_player.py build
python3 tools/cinder_four_player.py start
python3 tools/cinder_four_player.py stop
python3 tools/test_cinder_delivery.py
# 이전 이동/운반·5번째 거절·재접속 회귀
python3 tools/cinder_four_player.py check
python3 tools/cinder_four_player.py start --map-only
```

수동 실행은 `--test-dir` 없이 실제 키보드/마우스를 받는다. 같은 LAN 네 명은 동일 실행본에서 HOST 1명/JOIN 3명, 방장 주소와 TCP 27842를 사용한다. 한 PC의 네 창은 선택한 한 창만 키보드/마우스를 받는다. [08_Stop_Cinder_4P.command](../../08_Stop_Cinder_4P.command)는 현재 시험 프로세스만 종료한다.

CRT는 기존 유리 마스크에 피드백 루트의 평행이동 `_ScreenOffset`만 적용한다. 같은 크기/방향의 수령기를 옮기는 현재 배치를 지원한다. 크기나 회전을 바꾸면 유리·종이 배출 위치를 다시 실측해야 한다. 기본 좌표의 옛 실험은 offset 0을 유지한다. [표현 소스](../../NoReturns/Assets/_NoReturns/Runtime/CarryLab/ReceiptFeedback.cs) · [셰이더](../../NoReturns/Assets/_NoReturns/Resources/ReceiptUI/ReceiptCRT.shader).

[자동 검사·보존 근거](../validation/cinder-delivery-01.json). `DeliveryRules.Run`은 기존/Cinder 장부 **35개** 조건, `ReceiptSurfaceCheck.Run`은 양쪽 좌표의 앞/뒤/벽 가림 **6개** 조건을 검사한다. 480×320 화면에서 기존 전면 **1,597픽셀**, Cinder 전면 **1,596픽셀**만 바뀌고 후면/벽 가림은 **0픽셀**이다. 실제 네 프로세스 배송 검사는 네 명의 물리적 탑승·직원 1명의 서쪽/북쪽 우회로 운반·귀환, 공유 회수/무보수 귀환/전원 확인/1회 지급/다음 근무 초기화를 확인한다. 원본 씬을 통과한 옛 검사와 파생 씬의 이번 검증을 구분한다. 사람 조작·재미·다른 PC/WAN·Windows 실행·성능·리스너/진압봉/억제/신호기/진행 저장은 미확인이다.

**검증 완료:** 맥 빌드 오류 **0개**·기존 경고 **7개**, 실제 네 프로세스 배송 **23개**, 기존/Cinder 장부 **35개**, CRT 표면/가림 **6개**, BAY 04 네 몸체 통로 위치 **132개**, 기존 구조 규칙 **7개** 통과. KO/EN 수령·종이 배출/회수·420 CR 정산 화면을 확인했다. 기존 이동/운반·거절·재접속 회귀 **13개**도 최종 빌드로 통과했다.

![BAY 04 수령기](../../art/cinder-kit-01/delivery-terminal-ko.png)

![선내 정산](../../art/cinder-kit-01/delivery-ship-report.png)

## 2026-10-01 — Cinder 4인 맵 테스트 환경

**CINDER-4P-01**: 현재 `CinderCompactSiteReview`에서 별도 `CinderFourPlayerTest`를 생성해 기존 방장 권한 `CarryRoom`의 직원 4명·이동·공유 화물 E/Q·초기화를 연결했다. 원본 아트 씬은 유지하며 매 빌드마다 시험 씬을 재생성한다. 일반 실험은 TCP **27841**, Cinder는 **27842**로 분리하고 `cinderReview`가 다른 수신 상태를 거절한다. 프로토콜 **10**·게임 버전 **0.9.1**을 유지한다. 시험 모드는 프레임 상한 **30fps**, 물리 **50Hz**, 카메라 거리 **250m**를 사용하며 성능 측정 결과는 아니다. [코드](../../NoReturns/Assets/_NoReturns/Runtime/CarryLab/CarryRoom.cs) · [생성·빌드](../../NoReturns/Assets/_NoReturns/Editor/CinderFourPlayerBuild.cs).

맥에서 [실행 파일](../../07_Play_Cinder_4P.command)을 더블클릭하면 방장 1명과 참가자 3명을 같은 PC의 `127.0.0.1`로 실행한다. [종료 파일](../../08_Stop_Cinder_4P.command)은 이 도구가 기록한 실행본/세션 식별자가 모두 일치하는 프로세스만 종료한다. 터미널을 닫아도 게임 창은 독립 실행되며 같은 세션을 복원하는 기능은 없다. 종료 후 다시 실행하면 새 방이다. 한 컴퓨터의 키보드·마우스는 선택한 창 하나를 조작하므로 실제 네 명의 조작감 평가는 별도 PC가 필요하다.

```sh
# 저장소 루트. 이 프로젝트의 Unity Editor를 열고 Play를 중지·씬을 저장한다.
source ~/.unity/env
python3 tools/cinder_four_player.py build
python3 tools/cinder_four_player.py start
python3 tools/cinder_four_player.py stop
# 수동 창을 종료한 뒤 실제 네 프로세스 자동 검사
python3 tools/cinder_four_player.py check
```

맥 실행본은 `builds/CinderFourPlayer/NoReturns.app`이다. 빌드는 Editor의 다음 update에서 수행하고 `artifacts/cinder-four-player/build.json`으로 완료·오류를 확인한다. CLI 요청의 5초 제한보다 긴 동기 빌드를 직접 호출하지 않는다. 빌드·개인 로그·세션 PID는 Git 제외이며 검토용 [검증 기록](../validation/cinder-four-player-01.json)만 보존한다. Python 표준 라이브러리만 사용한다. Windows 생성 메뉴/도구 경로도 제공하지만 이번 Windows 빌드·실행은 미검증이다.

현재 조작은 WASD 이동·마우스 시점·E 공유 화물 집기·Q 본인 화물 놓기·Space 빈손 점프·Esc 메뉴·R 방장 초기화다. 주황/청록/보라/노랑 헬멧과 `CREW {0}/4`(직원 {0}/4)로 구분한다. 출발 위치는 우주선 앞, x=-21.7/-19.7m, z=-20.05/-18.65m, y=0.035m다. 화물은 (-20.7, 0.55, -18.5)m에서 시작한다. 다른 사람이 들던 화물을 놓을 수 없다. 다섯 번째 참가자는 거절하고 빈 슬롯은 이탈 후 재사용한다.

같은 LAN에서 사람 4인이 시험하려면 같은 시험 실행본을 각 기기에 전달하고 직접 앱을 열어 한 명이 HOST, 나머지 세 명이 방장 LAN 주소로 JOIN한다. 방장의 TCP 27842 접속을 허용한다. 다른 PC·인터넷·Steam 연결은 이번에 검증하지 않았다. 자동 도구는 같은 PC 전용이다.

**구축·검증 상태: 맥 빌드 오류 0개·기존 경고 7개, 실제 네 프로세스 자동 검사 13개·기존 구조 규칙 7개·일반 모드 기본값 7개 통과. 800×500 한글 HUD 화면 2개에서 직원 4/4·E/Q 안내·조준점·팀 색을 확인했고 수동 네 프로세스/접속 3개·종료도 확인했다.** 배송·수령/영수증·정산·리스너·진압봉·억제 단계·신호기·진행 저장은 Cinder에 미연결이다. `--hazard`/`--delivery`를 주어도 시험 모드는 활성화하지 않으며 기존 정적 리스너 표식은 시험 씬에서 숨긴다. 아래 기존 Windows 협동 게임의 통과 결과를 Cinder 전체 플레이 통과로 승계하지 않는다. 사람 네 명의 재미/교행·동시 화물 회전·성능·장시간 안정성은 남아 있다.

![Cinder host / crew 4/4](../../art/cinder-kit-01/crew-player-0.png)

![Cinder client / crew 4/4](../../art/cinder-kit-01/crew-player-3.png)

2026-09-13 · 0.9.0 구현·자동 검증 완료. Unity·직접 LAN 방장 권한 구조를 유지한다. Steam과 운영자 서버는 범위 밖이다.

- [x] CarryRoom/CarryWire: 방장 슬롯 0, 참가 슬롯 1~3, 연결별 입력·시간초과·수신 슬롯 배정. 슬롯 번호를 입력으로 신뢰하지 않는다. 프로토콜 10으로 구버전과 혼합하지 않는다.
- [x] 직원·장비·HUD: 4개 고정 슬롯과 접속 마스크를 사용하고 서로 다른 팀 색을 적용한다. 빈 슬롯은 보이지 않고 충돌하지 않는다.
- [x] CarryThreat: 접속한 모든 직원에게 공격·쿨타임·다운 상태 적용. 구조는 2m 안의 가장 가까운 다운 동료 한 명이며 대상 변경 시 진행률 초기화. 전원 다운은 접속 중인 직원만 센다.
- [x] 운반·배송: 기존 한 화물·한 신호기 소유권을 유지하고 슬롯 2~3도 동일한 E/Q 규칙을 사용한다. 귀환 조건은 접속 중인 전원. 이탈 시 운반 물건 회수와 기존 근무 중단 규칙 유지.
- [x] 실제 Windows 4개 프로세스로 슬롯 배정·운반·장비·영수증·정산 확인. 추가 접속 거절과 이탈·재접속 확인. 기존 2인 회귀 검사와 구조 로직 검사 수행.
- [x] 한영 현재 명세·체크리스트·변경 이력, 실행본과 Git 업데이트.

자동 입력 검사는 재미·조작감 평가와 분리한다. 다른 PC·WAN·Steam·사람 4인 평가는 완료 처리하지 않는다. 이전 p0/p1 등 검사 필드는 호환 관찰값으로 유지하되 게임의 플레이어 상태는 배열로 읽는다.

## 현재 사용법과 규칙

같은 0.9.0 실행본을 사용한다. 06_Play_Listener_Test.cmd로 실행해 한 명이 방을 만들고 최대 3명이 같은 LAN의 방장 주소로 참가한다. 같은 PC 시험에서는 127.0.0.1을 사용한다. 현재는 방장 포함 최대 4명이며, 근무 준비 단계에서만 신규 참가할 수 있다.

주황·청록·보라·노랑 헬멧으로 슬롯을 구분한다. 빈 슬롯에는 직원과 충돌체를 표시하지 않는다. HUD는 현재 인원/4다. E 집기·상호작용/구조 유지, Q 내려놓기, 왼쪽 클릭 충격봉, Shift 조용히 걷기는 그대로다. 구조 진행률은 구조하는 직원의 값이다. 가장 가까운 구조 대상이 달라지면 누적 시간을 다시 센다.

한 명이라도 우주선 밖이거나 다운 상태면 정상 귀환을 실행하지 않는다. 참가자가 나가면 기존처럼 근무는 중단되고 해당 직원이 들던 화물·신호기를 놓는다. 남은 직원 중 다운된 직원이 있으면 우주선에서 복구한다. 모두 서 있다면 현장 위치를 보존한다. 준비 단계로 돌아오면 빈 슬롯을 새 참가자에게 재배정한다. 방장 이전·근무 중 참가·운영자 서버는 구현하지 않았다.

새 프로토콜은 10이다. 입력 소켓에 배정된 슬롯만 갱신하며 클라이언트는 자신의 슬롯 번호를 지정하지 않는다. 연결별 입력 정지와 시간초과를 적용한다. 호환 관찰용 p0/p1은 남겼지만 실제 전송·표시는 positions/yaws 및 down/rescue/cooldown 배열과 occupiedMask를 따른다.

## 검증 실행

- python tools/test_four_player.py: 4인 장비·배송·정산, 초과 접속과 재접속.
- python tools/test_four_player_rescue.py: 실제 4개 프로세스에서 슬롯 2 다운과 슬롯 3 저지·구조.
- Unity MCP run_script에서 tools/unity_checks/FourCrewRules.cs의 FourCrewRules.Run: 구조 대상·진행률·전원 다운 7개 로직 조건.

모든 실행 검사는 전용 test-dir 저장을 사용하므로 사용자의 실제 진행 저장과 분리된다. 자동 검사 실패 기록은 삭제하지 않는다. 초기 검사에서 동료 충돌, 우주선 E의 귀환 동작, 실제 영수증 위치를 고려해 경로를 조정했고 상태 파일 교체 순간 읽기를 재시도하도록 수정했다.

## 검증 근거와 남은 확인

실제 4개 Windows 프로세스: 장비·배송·정산·접속 31개, 다운·저지·구조 15개 통과. 기존 실제 2개 프로세스: 충격봉·구조·전원 다운 회수 24개, 배송·영수증·정산 30개 통과. Unity MCP의 구조 대상·진행률·접속 마스크 로직 7개 통과. 서로 다른 해상도와 실제 4명 조작감, 장시간 안정성·지연·대역폭은 별도 검증 대상이다.

카메라 렌더 캡처에서 슬롯 3의 1인칭 충격봉과 월드 표시를 확인했다. 숨김 실행의 screen.png는 검게 저장되어 HUD의 시각적 검증 근거로 사용하지 않는다. HUD 인원 수 변경은 코드·전송 상태로 확인했으며 사람 가독성 평가는 남아 있다. 최종 검토에서 현장 기록 목록은 직원 슬롯과 무관한 단서 2개를 유지하도록 수정했다.

근거 코드: [network](../../NoReturns/Assets/_NoReturns/Runtime/CarryLab/CarryRoom.Network.cs), [threat](../../NoReturns/Assets/_NoReturns/Runtime/CarryLab/CarryThreat.cs), [room/HUD](../../NoReturns/Assets/_NoReturns/Runtime/CarryLab/CarryRoom.cs), [four-process test](../../tools/test_four_player.py), [rescue test](../../tools/test_four_player_rescue.py), [Unity rules](../../tools/unity_checks/FourCrewRules.cs).

[보존용 자동 검증 결과](../validation/four-player-0.9.0.json). 4인 구조 및 2인 회귀는 최종 현장 기록 UI 수정 전 0.9.0 빌드에서 수행했다. 현장 기록 목록만 수정한 뒤 Windows 빌드와 4인 전체 흐름 검사를 다시 수행해 31개 모두 통과했다.

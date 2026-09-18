# CINDER DEPOT 배송 데모

[English](cinder-demo.en.md)

2026-09-18 · CINDER-DEMO-01 · 기본 도형 맵과 기존 게임 규칙의 연결

## 실행과 한 사이클

[Play_Cinder_Demo.cmd](../../Play_Cinder_Demo.cmd)를 실행한다. Windows 실행 파일은 `builds/CinderDemo/NoReturns-CinderDemo.exe`다. 방 만들기로 혼자 시작하거나, 출발 전에 같은 빌드의 참가자 최대 3명이 방장의 내부 네트워크 주소로 합류한다. 같은 PC의 추가 실행은 `127.0.0.1`이다. 연결은 기존 TCP 27841이며 Steam 매칭은 아니다.

우주선 안에서 E로 계약 선택과 출발을 진행한다. 화물을 E로 들어 북동쪽 BAY 04로 운반하고, 단말기 옆 바닥 표시 안에 Q로 내려놓는다. 안정된 화물을 검사한 뒤 영수증을 인쇄한다. 단말기를 보고 E로 영수증을 회수하고 전원이 선내로 돌아와 E로 귀환·정산한다. 일반 계약은 배송 300CR와 귀환 120CR, 합계 420CR다. 인쇄만 하거나 영수증을 두고 돌아오면 지급되지 않는다. 정산 후 신호기 해금 120CR를 구매하면 선내에 실제 장비가 생긴다. 다음 근무에도 해금과 남은 잔액을 유지하고 사용량을 2회로 준비한다.

조작: WASD 이동, 마우스 시야, E 상호작용·가까운 동료 구조 길게 누르기, Q 내려놓기, Space 빈손 점프, Shift 조용히 걷기, C 소리 내기, 왼쪽 클릭 충격봉. Esc 메뉴의 보급 선택과 언어 전환을 사용한다. 블록아웃 시험의 Shift 달리기와 달리 이 데모는 기존 협동 게임의 조용히 걷기를 사용한다.

## 적용 범위와 수치

- 별도 [CinderDeliveryDemo.unity](../../NoReturns/Assets/_NoReturns/Scenes/CinderDeliveryDemo.unity). 원본 블록아웃과 선내 시험을 보존한다. 지도 범위 108×86.4m, 기존 선내 크기와 출입 구조를 그대로 사용한다. 외장을 새로 만들지 않았다.
- [CinderDemoLayout.cs](../../NoReturns/Assets/_NoReturns/Runtime/CarryLab/CinderDemoLayout.cs): 선내 4개 스폰, 화물·장비 적재, BAY 04 수령 중심 `(32.7,0,15)`, 선내 귀환 영역. 수령 장치의 원래 화면 재질과 영수증 슬롯을 같은 오프셋으로 이동한다.
- [CarryMission.cs](../../NoReturns/Assets/_NoReturns/Runtime/CarryLab/CarryMission.cs): 손에서 놓은 화물이 0.75초 안정되면 수령, 인쇄 준비 0.75초, 영수증 회수와 전원 승선 후 정산. 가격·보수·실패 규칙은 기존 규칙을 재사용하며 새 경제 설계를 확정한 것이 아니다. 참가자 이탈 시 현재 근무를 중단하는 기존 제한도 유지한다.
- [CarryThreat.cs](../../NoReturns/Assets/_NoReturns/Runtime/CarryLab/CarryThreat.cs): B구역 리스너 1체와 외곽 생물 1체. 벽을 고려한 탐색 범위를 확대하고 시작 전에 경로망을 준비한다. 같은 정적 맵의 다음 근무에서는 경로망을 재사용한다. A/C 추가 리스너는 미구현이다.
- [CarrySuppression.cs](../../NoReturns/Assets/_NoReturns/Runtime/CarryLab/CarrySuppression.cs): 0~360초 안정, 360~480초 불안, 480~600초 임계, 600초 이후 꺼짐, 608초부터 외곽 침입. 모두 현장 시간 기준의 첫 검증값이다. 숫자 카운트다운 대신 신호·조명·문구로 알린다. 블록아웃 억제 기둥 전체가 개별 작동하는 것은 아니다.
- [CarryRoom.Network.cs](../../NoReturns/Assets/_NoReturns/Runtime/CarryLab/CarryRoom.Network.cs): 방장 권한, 준비 중 합류, Cinder 프로토콜 11/기존 시험 10 분리. 다른 맵의 빌드를 섞어 접속할 수 없다.
- 사무실의 유지보수 단서를 옮겼다. 건물 내부 세부 방, 새로운 몬스터, 완성 아트, 추가 화물 종류는 이번 범위가 아니다.

## 제작과 재현

Unity `NO RETURNS > Demo > Create Cinder Delivery Demo`는 원본 블록아웃을 새 데모 경로에 다시 복사하므로 데모의 직접 편집을 덮어쓴다. 직접 수정 전 별도 저장하거나 소스 생성 코드에 반영한다. **Build Cinder Delivery Demo는 현재 저장된 데모 장면을 빌드하고 지도를 재생성하지 않는다.** Validate는 수령·영수증·전원 귀환·420CR 규칙을 검사한다. [CinderDemoBuild.cs](../../NoReturns/Assets/_NoReturns/Editor/CinderDemoBuild.cs)가 재현 소스다.

자동 검사에는 테스트 입력 기능이 포함된 로컬 Windows 빌드를 사용한다. [배송 검사](../../tools/test_cinder_demo.py)는 `--crew 1`, `--crew 2`, `--crew 4`를 지원하며 [위험 검사](../../tools/test_cinder_hazards.py)는 2개 프로세스를 사용한다. 같은 포트를 쓰므로 순서대로 실행한다. 실제 이동 입력으로 서쪽·북쪽 우회로를 왕복하고 순간 이동이나 잔액 주입을 하지 않는다. 여러 명의 검사에서는 방장만 배송하고 나머지는 선내에서 복제 상태와 전원 승선 규칙을 확인하므로, 전원 동시 운반 검증과 다르다.

## 검증과 남은 작업

검증 결과는 [검증 체크리스트](05-validation.ko.md)의 CINDER-DEMO-01 항목을 따른다. 로컬 원본 근거는 `artifacts/cinder-demo/`, `artifacts/cinder-demo-loop/`, `artifacts/cinder-demo-hazards/`에 있다. 빌드·로그·캡처는 Git에 포함하지 않는다.

사람이 확인할 항목: 초행길 수령소 발견, 화물 시야·운반 피로, 동료 유인으로 배송할 이유, 억제 신호의 이해, 구조 후 탈출의 긴장, 10~15분 목표 길이. 목표 시간은 실측 확정이 아니다. 렌더 확인에서 기존 충격봉이 단말기 화면 중앙을 가리는 문제가 보였다. 무기 위치와 단말기 가독성 조정은 후속 작업이다. 외곽 생물의 전체 추격 경로, 모든 출입구의 화물 충돌, 다른 PC·인터넷·Steam, 실제 4인 재미는 미검증이다. 기존 URP DepthOfField/Panini 셰이더 경고와 Unity 구형 API 경고가 남는다. 출시 완료 판정이 아닌 한 사이클 플레이 검증용이다.

[다른 PC 실행·접속 안내](cinder-portable.ko.md)

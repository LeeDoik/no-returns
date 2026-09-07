# NO RETURNS — 반품 불가

[English](README.en.md)

> 택배가 살아 있습니다. 그래도 배송은 해야죠.

**현재 버전: 협동 계약 플레이테스트 0.7 · 2026년 9월 7일**

혼자 또는 2~4명이 살아 있는 택배를 배송하는 Godot 3D 협동 게임입니다. 한 맵에서 세 계약을 완수하고, 공동 수입으로 장비를 고르며, 동료와 릴레이하고 포장쥐에게 빼앗긴 상자를 회수합니다. Steam Windows 출시가 목표이며 현재는 기본 도형을 사용하는 개발 빌드입니다.

## 바로 실행

**[NO_RETURNS.exe](build/NO_RETURNS_0.7/NO_RETURNS.exe)를 두 번 클릭 → 혼자 캠페인 시작.** 이전 창은 닫아 주세요. 에디터 없이 실행됩니다. 친구에게는 [Windows ZIP](build/NO_RETURNS_0.7_Windows.zip)을 전달하고 전체 압축을 풀어 사용하세요.

개발 프로젝트에서는 [PLAY.cmd](PLAY.cmd), 같은 PC의 두 창은 [PLAY_TWO.cmd](PLAY_TWO.cmd), 네 창은 [PLAY_FOUR.cmd](PLAY_FOUR.cmd)를 실행합니다. 한 명이 협동 캠페인 방을 만들고 나머지는 주소를 입력해 참가한 다음 방장이 시작합니다. 같은 PC 주소는 `127.0.0.1`입니다. [EDIT.cmd](EDIT.cmd)는 에디터를 엽니다.

## 0.7에서 달라진 것

- 계약 3개를 순서대로 완수하는 한 판. 계약당 기본 240초이며 인원과 단계에 따라 목표가 늘어납니다.
- 정상 배송 10, 릴레이 추가 5, 계약 성공 20크레딧. 공동 금고로 작업화·시간 연장·경적을 구매하고 전원 준비 후 다음 계약으로 이동합니다.
- 다른 직원이 3초 안에 3m 이상 떨어진 공중에서 받는 릴레이. 잘못된 배송·반복 집기·자기 받기의 추가 보상을 막았습니다.
- 계약 2~3에서 화물을 훔치는 포장쥐. 예고를 보고 대응하거나 R 경적으로 쫓고, 둥지에서 상자를 회수합니다.
- 단계별 조작 안내, H 도움말, 계약·금고·준비·장비 결과 화면, 한·영 UI, 음량·시야각·전체 화면과 로컬 완주 기록.
- 버전이 다른 손님의 입장을 막고 캠페인·크리쳐 정보를 별도로 동기화합니다. 방장이 규칙과 보상을 판정합니다.

기존 32×36m 야간 물류센터, 양쪽 우회로, 오른쪽 역전 컨베이어, 네 화물, Q 위치 알림과 3분 연습도 유지합니다. 조명은 6.5m로 높였고 중앙 벽 사이에 있던 벨트는 오른쪽 운반 통로에 있습니다.

WASD 이동, 마우스 시점, Space 점프, E 집기·받기·놓기, 왼쪽 클릭 던지기, Q 알림, F 벨트 역전, R 경적, H 도움말, Esc 메뉴입니다. A/B 라벨과 같은 배송구에 넣으세요.

## 기획과 안내

| 한국어 | 영어 |
| --- | --- |
| **[33개 항목 상세 기획서](docs/superpowers/specs/2026-09-07-release-design.ko.md)** | [Detailed design](docs/superpowers/specs/2026-09-07-release-design.en.md) |
| [0.7 전체 플레이 안내](docs/prototype/07-contracts.ko.md) | [Full play guide](docs/prototype/07-contracts.en.md) |
| [0.7 구현·검증 기록](docs/superpowers/plans/2026-09-07-release-polish.ko.md) | [Implementation and evidence](docs/superpowers/plans/2026-09-07-release-polish.en.md) |
| [출시 로드맵·계정 등록](docs/steam/01-release-roadmap.ko.md) | [Roadmap](docs/steam/01-release-roadmap.md) |
| [상점 소개 초안](docs/steam/02-store-page.ko.md) | [Store draft](docs/steam/02-store-page.en.md) |
| [이미지·트레일러 계획](docs/steam/03-assets-and-trailer.ko.md) | [Assets and trailer](docs/steam/03-assets-and-trailer.en.md) |
| [온라인·빌드 계획](docs/steam/04-online-and-build-plan.ko.md) | [Online plan](docs/steam/04-online-and-build-plan.md) |
| [출시 체크리스트](docs/steam/05-release-checklist.ko.md) | [Checklist](docs/steam/05-release-checklist.md) |
| [스트리머·테스터 자료](docs/steam/06-creator-kit.ko.md) | [Creator kit](docs/steam/06-creator-kit.en.md) |
| [Windows 빌드·Steam 인계](docs/steam/07-build-handoff.ko.md) | [Build handoff](docs/steam/07-build-handoff.en.md) |

상세 기획서는 참고 게임의 공식 설명과 우리의 해석을 구분합니다. 플레이 흐름, 인원별 수치, 보상·장비, 릴레이 판정, 크리쳐 상태, 서버 소유권, 저장, UI·소리, 18개 영역 QA, 독립 테스터 실험, 출시 조건을 담았습니다. 이전 기획·버전 문서는 당시의 기록이며 충돌하는 0.7 수치는 새 상세 기획과 구현 기록을 따릅니다.

## 검증 상태와 출시까지 남은 것

동작 검사 21개, 실제 두 프로세스 시나리오 6개, 네 프로세스 시나리오, SteamPipe 생성 도구 검사를 통과했습니다. 캠페인 완주·공동 구매·준비 투표·절도와 경적 회수, 실제 비행 릴레이, 바닥에 안착한 화물의 둥지 운반, 첫 실행 설정, 중복/늦은 참가 완주 기록을 확인합니다. 이동 스냅샷은 최대 검사 상태 1,280바이트, 별도 캠페인 정보 ≤2,048, 크리쳐 ≤256바이트입니다.

**Steam 출시는 아직 아닙니다.** Steamworks 미등록·실제 App/Depot ID 미확보 상태입니다. 현재는 방장 PC가 서버인 ENet 직접 주소 방식이고 Steam 친구 초대·중계는 없습니다. 별도 PC/외부 네트워크, 대상 기기 성능, 독립된 친구 그룹 재미 검증, 최종 아트·상점 이미지·트레일러·권리 검토, Steam 설치와 Valve 심사가 남았습니다. 자동 검사 결과를 재미나 인터넷 연결 품질의 증거로 해석하지 않습니다.

## 버전 관리

로컬 Git으로 관리하며 0.5/0.6 기준점을 보존합니다. 이번 변경의 식별자는 `v0.7.0`이며 GitHub 원격 저장소는 연결하지 않습니다. [변경 기록·복구 안내](docs/development/version-control.ko.md), [문서 작성 규칙](AGENTS.md)을 참고하세요.

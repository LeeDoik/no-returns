# NO RETURNS — 반품 불가

[English](README.en.md)

> 택배가 살아 있습니다. 그래도 배송은 해야죠.

**현재 버전: 야간 물류센터 플레이테스트 0.6 · 2026년 9월 7일**

2~4명이 살아 있는 택배를 배송하는 Godot 3D 협동 게임입니다. 기본 도형으로 게임 규칙을 검증하는 단계이며 Steam Windows 출시가 목표입니다. 게임 문구 원문은 영어, 기본 UI와 문서 진입점은 한국어입니다.

## 바로 실행

**[NO_RETURNS.exe](build/NO_RETURNS_0.6/NO_RETURNS.exe)를 두 번 클릭 → 혼자 연습하기.** 기존 창은 먼저 닫아주세요. 에디터 설치 없이 실행할 수 있습니다. 다른 사람에게 전달할 때는 [Windows ZIP](build/NO_RETURNS_0.6_Windows.zip) 전체를 전달합니다.

개발 프로젝트에서는 [PLAY.cmd](PLAY.cmd), 같은 PC의 두 창은 [PLAY_TWO.cmd](PLAY_TWO.cmd), 네 창은 [PLAY_FOUR.cmd](PLAY_FOUR.cmd)를 실행합니다. 직원 수가 모이면 방장 창에서 근무를 시작하세요. [EDIT.cmd](EDIT.cmd)는 Godot 에디터를 엽니다.

## 이번에 구현한 것

- 32 × 36m 야간 물류센터, 높은 중앙 분류대와 항상 열린 양쪽 운반 경로. 입고에서 배송구까지 한 번에 던지는 동선을 막았습니다.
- F로 역전하는 컨베이어, 선반·셔터·조명·포장쥐 둥지 표시. 크리쳐 AI는 아직 없습니다.
- 일반·재채기·접착·점프 화물 네 종류. 점프 상자는 들면 주기가 멈추고, 방향을 맞춰 놓으면 낮은 칸막이를 넘습니다.
- 최초 A 배송 후 균형 잡힌 A/B 목적지 변화. 인원별 목표는 3분 동안 5/6/8/10개입니다.
- Q 위치 알림, 결과 화면, 전원 동의 후 60초 보너스 근무. 보너스 실패로 기본 성공을 잃지 않습니다.
- 한국어/영어 UI, 신호음·마우스 감도·언어 저장, Windows 실행 파일과 배포 자동화.

WASD 이동, 마우스 시점, Space 점프, E 집기·받기·놓기, 왼쪽 클릭 던지기, Q 위치 알림, F 컨베이어 역전, Esc 메뉴입니다. 화물의 **A / B 라벨**에 맞게 배송하세요.

## 안내와 출시 준비

| 한국어 | 영어 |
| --- | --- |
| [0.6 플레이 안내](docs/prototype/06-night-depot.ko.md) | [Play guide](docs/prototype/06-night-depot.en.md) |
| [맵 구현 계획과 완료 기록](docs/superpowers/plans/2026-09-07-night-depot.ko.md) | [Implementation plan](docs/superpowers/plans/2026-09-07-night-depot.en.md) |
| [기획서](docs/superpowers/specs/2026-09-06-no-returns-design.ko.md) | [Design](docs/superpowers/specs/2026-09-06-no-returns-design.md) |
| [창고 설비·포장쥐 기획 메모](docs/superpowers/specs/2026-09-07-depot-creature-design.ko.md) | [Equipment and creature proposals](docs/superpowers/specs/2026-09-07-depot-creature-design.en.md) |
| [출시 로드맵·계정 등록](docs/steam/01-release-roadmap.ko.md) | [Roadmap](docs/steam/01-release-roadmap.md) |
| [상점 소개 초안](docs/steam/02-store-page.ko.md) | [Store draft](docs/steam/02-store-page.en.md) |
| [이미지·트레일러 계획](docs/steam/03-assets-and-trailer.ko.md) | [Assets and trailer](docs/steam/03-assets-and-trailer.en.md) |
| [온라인·빌드 계획](docs/steam/04-online-and-build-plan.ko.md) | [Online plan](docs/steam/04-online-and-build-plan.md) |
| [출시 체크리스트](docs/steam/05-release-checklist.ko.md) | [Checklist](docs/steam/05-release-checklist.md) |
| [스트리머·테스터 자료](docs/steam/06-creator-kit.ko.md) | [Creator kit](docs/steam/06-creator-kit.en.md) |
| [Windows 빌드·Steam 인계](docs/steam/07-build-handoff.ko.md) | [Build handoff](docs/steam/07-build-handoff.en.md) |

## 검증 상태와 남은 범위

동작 검사 17개, 실제 두 프로세스 연결 시나리오 4개, 네 프로세스 시나리오와 SteamPipe 생성 도구 검사를 통과했습니다. 양쪽 배송구 중심까지 직원·운반 화물 경로, 입고 투척의 첫 접촉, 넓어진 경계의 복구, 컨베이어 거리·방향·시야·재사용 대기와 게스트 역전을 확인했습니다. 최대 검사 스냅샷은 1,280바이트로 현재 제한과 같습니다. 한국어·영어 메뉴와 레버 안내, 맵 전체·3인칭·배송구/둥지 화면을 실제 렌더링해 확인했습니다. Windows 내보내기와 실행 검사, 게임 스크립트 17개의 팩 포함 여부, 실행 파일·게임 팩만 둔 별도 폴더의 연습 화면도 확인했습니다.

**Steam 출시는 아직 아닙니다.** Steamworks 미등록·실제 App ID 미확보 상태입니다. 현재 통신은 방장 PC가 서버인 ENet 직접 주소 방식이며 Steam 친구 초대·중계는 없습니다. 별도 PC/외부 네트워크, 실제 Steam 계정·설치, 최종 아트·트레일러·상점 공개·심사는 미완료입니다. 등록·결제·공개를 대신 진행하지 않았습니다. 준비된 문서와 미리보기용 SteamPipe 생성 도구로 다음 작업을 이어갈 수 있습니다.

자동 검사 성공과 재미 검증은 다릅니다. 친구와 해보며 사고 원인이 읽히는지, 역할이 나뉘는지, 다시 해보고 싶은지 확인하세요. 기존 0.1~0.5 문서는 해당 버전의 기록으로 보존했습니다. [문서 작성 규칙](AGENTS.md)에 따라 한·영 문서를 함께 유지합니다.

## 버전 관리

로컬 Git으로 관리하며 첫 기준점은 `v0.5.0`입니다. 이번 0.6은 별도 변경으로 기록합니다. GitHub 원격 저장소는 연결하지 않습니다. [변경 기록·복구 안내](docs/development/version-control.ko.md)를 참고하세요.

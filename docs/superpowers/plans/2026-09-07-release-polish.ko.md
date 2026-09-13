# 협동 계약 캠페인 구현 계획

[English](2026-09-07-release-polish.en.md)

> **과거 기록 · 2026-09-12 폐기/대체.** 현재 게임은 [PSX 우주 배송 기획](../../current/01-overview.ko.md)을 따른다. 아래 구현·실행·아트·수치는 과거 자료이며 현재 상태가 아니다. 삭제된 파일은 경로 기록으로만 남긴다.


> 문서 체계 개정: 이 파일의 버전·수치는 작성 당시 기준입니다. 현재 규칙·미완료 상태는 [문서 홈](../../README.md)을 먼저 확인하세요.

목표: 플레이 가능한 개발 빌드 0.7. Steam 승인 출시본이 아닙니다. writing-plans와 subagent-driven-development를 적용합니다. 사용자는 단계별 승인 없이 상세 기획과 적용을 요청했습니다. 기존 0.6은 로컬 Git으로 복원 가능합니다. 유료 에셋·외부 공개·계정 작업은 하지 않습니다.

## 설계와 범위

기존 물류센터에서 계약 3개로 완결되는 근무를 만듭니다. 메뉴에는 혼자 캠페인, 협동 캠페인 방 만들기, 참가, 기존 짧은 연습을 둡니다. 캠페인 인자를 명시하지 않는 기존 연습/방 만들기 검사 API는 유지합니다. 계약은 240초이며 인원별 기존 목표에 0/2/4개를 더합니다. 화물 네 종류를 유지합니다. 계약 성공 시 배송당 10, 유효한 팀 릴레이당 추가 5, 완료 보상 20 크레딧을 공동 금고에 넣습니다. 실패한 시도는 적립하지 않습니다. 성공한 계약 사이에만 방장이 공동 장비를 삽니다. 작업화(20, 이동 +8%, 최대 2단계), 시간 허가증(25, 계약마다 +20초, 최대 2단계), 경적 키트(20, 재사용 대기 8→6초, 최대 1단계). 전원 준비 투표로 다음 계약을 시작하며 세 계약 성공 시 끝납니다. 영구 능력치는 없고 완료 근무 기록만 로컬 저장합니다.

팀 릴레이: A가 직접 던진 화물을 다른 직원이 3초 이내·투척 지점에서 3m 이상 떨어진 공중에서 받으면, 해당 화물을 올바른 배송구에 보냈을 때 릴레이 보상 1회를 지급합니다. 반복 집기·자기 상자 받기·바닥 집기·오배송·복구·중복 배송에는 보상이 없습니다. 복구 시 투척 이력을 지웁니다. 릴레이와 배송 성공에 짧은 합성 효과음을 넣고 음소거를 지킵니다.

포장쥐는 캠페인 계약 2~3에만 등장합니다. 한 마리, 뚜렷한 0.9초 훔치기 예고, 바닥의 자유 화물만 대상, 오른쪽 통로의 고정 순찰과 회수 가능한 둥지를 사용합니다. 운반·접착 화물은 훔치지 않습니다. R 경적은 4m 이내·시야가 열려 있으면 화물을 놓고 3초 도망가게 합니다. 직원별 재사용 대기는 8초(장비 구입 시 6초)입니다. 피해·죽음은 없습니다. 둥지에서 자동으로 놓고 8초 동안 다시 훔치지 않습니다. 종료·재시작·접속 종료에도 놓습니다. 방장이 계산하고 손님은 수신 상태를 표시합니다.

## 작업 소유권과 연결 규약

- 루트: `scripts/main.gd`, `session.gd`, `contracts.gd`, `run_profile.gd`, `feedback.gd`, `cargo.gd` 릴레이 이력, 통합 검사, 모든 한·영 문서와 빌드/배포.
- 크리쳐 담당: `scripts/packrat.gd`, `tests/test_packrat.gd`만 수정합니다. API는 `reset(cargos: Dictionary)`, `start(stage: int)`, `step(delta, workers, cargos)`, `scare(worker, cargos)->bool`, `snapshot()->Array`, `apply_snapshot(data:Array)`, `stop(cargos)`, `status_key()->String`입니다. 루트가 방장에서만 step을 호출합니다. 루트가 제공하는 화물 점유 필드는 `cargo.creature_held: bool`입니다. 화물은 보이되 정지 상태로 크리쳐를 따르고 정적 배경과만 충돌합니다. 루트는 점유 중 일반 화물 갱신·집기·벨트·접착 대상·재채기 밀기를 제외합니다. 내려놓는 모든 경로에서 충돌 마스크·레이어·freeze를 복구합니다. `main.packrat`으로 접근합니다.
- 표현 담당: `scripts/interface.gd`, 새 `scripts/campaign_copy.gd`, 새 `tests/capture_campaign.gd`, 전체화면/음량/FOV용 `scripts/preferences.gd`와 `tests/test_preferences.gd`만 수정합니다. 기존 interface 참조·헬퍼·state 키·검사는 유지합니다. 새 command는 `campaign`, `host_campaign`, `practice`, `host`, `join`, `next_contract`, `buy_boots`, `buy_time`, `buy_horn`, `help`, `quit`이며 기존 명령도 유지합니다. 선택적 `state.campaign` Dictionary 키: enabled, stage(0부터), credits, earned, deliveries, relays, boots, time_upgrade, horn, finished, ready, voted, best_runs. `state.creature`는 번역 키, `state.horn_left`는 float, `state.help`는 bool, `state.lesson`은 0~4 int, `state.progress`는 문자열입니다. 누락 시 기본값으로 표시합니다. 루트가 command와 도움말 입력 상태를 처리하며 UI는 도움말 명령을 한 번만 보냅니다. 캠페인 결과는 기존 보너스 대신 계약 진행을 표시합니다. 설정은 static Preferences.volume(0~1), fov(60~90), fullscreen(bool), apply_settings()를 사용하고 이전 저장과 호환되는 기본값과 검사 격리를 유지합니다.
- 이동 패킷은 직렬화 기준 1280바이트 이하로 유지합니다. 캠페인 정보는 변경 시 별도의 방장 권한 reliable 메시지, 크리쳐는 별도 compact unreliable 상태(256바이트 이하)로 보냅니다. 세션 버전 확인은 직원 등록 전에 호환되지 않는 버전을 거절해야 합니다. 루트가 구현과 패킷 검사를 담당합니다.

## 작업·검증 기록

- [x] 참고 사실과 자체 설계 구분, 반복 구조, 수치, 월드/크리쳐/UI/소리/통신, QA와 출시 기준을 담은 전체 한·영 상세 기획.
- [x] 계약 규칙과 릴레이 이력, 저장 검증과 중복 보상 방지, 플레이 연결.
- [x] 포장쥐 상태와 복구 검사, R 및 초기화 연동.
- [x] 초보 안내, 읽기 쉬운 캠페인 UI, 설정과 피드백, 실제 한국어/영어 화면 확인.
- [x] 버전 확인, 분리 동기화, 실제 방장/손님 캠페인 검사, 기존 검사, 배포 실행과 별도 폴더 렌더링.
- [x] 한·영 가이드/README/빌드 경로, 출시 상태, 로컬 Git 커밋과 재현 가능한 Windows ZIP.

외부 출시 조건: Steamworks 등록/App ID, 실제 Steam 통신·친구 초대, 별도 PC 인터넷/지연 검사, 목표 하드웨어 성능 검사, 독립된 협동 플레이테스트, 권리를 확보한 최종 아트/소리/상점/트레일러, Valve 심사. 완료했다고 표시하지 않습니다.


## 최종 개발 검증 — 2026년 9월 7일

변경 식별자: 로컬 Git `v0.7.0`. 전체 기획은 [33개 항목 상세 설계](../specs/2026-09-07-release-design.ko.md), 플레이 안내는 [0.7 계약](../../prototype/07-contracts.ko.md)입니다.

- `python tools/run_tests.py`: 동작 검사 21개, 방장/손님 시나리오 6개, 네 프로세스 시나리오 통과. 로그: `artifacts/full-07-tests.log`. 캠페인 검사는 의도된 150ms 입력 제한 때문에 유일한 요청이 사라지지 않도록 준비/경적 요청을 제한된 간격으로 반복합니다.
- 새 물리 검사는 실제 공중 릴레이 이동/받기, 실제 상자를 30 물리 프레임 접지시킨 뒤 둥지까지 연속 운반, 화물 전체 크기 장애물 검사와 복구를 확인합니다. 바닥 접촉 허용치로 즉시 잘못 내려놓는 문제를 고쳤습니다. 캠페인 통합은 보상 악용 방지, 세 단계 금고/장비/투표, 완주 기록, 늦은 참가 제외, 한 번만 작동하는 도움말을 확인합니다.
- `python -m unittest discover -s tests -p test_steampipe.py`: 통과. Steam 로그인·업로드는 하지 않았습니다.
- `python tools/build_windows.py`: Windows release 내보내기, 패키지 실행, 허용한 6개 파일의 ZIP과 SHA256 목록. `tests/audit_pack.gd`: 컴파일된 게임 스크립트 22개 포함.
- 실제 OpenGL 화면: `artifacts/campaign-menu-ko.png`, `campaign-menu-en.png`, `campaign-help-ko.png`, `campaign-help-en.png`, `campaign-settings-ko.png`, `campaign-contract-ko.png`, `campaign-contract-en.png`, `campaign-contract-guest-ko.png`. UI 화면은 구성한 표시 상태이며 사람의 플레이테스트가 아닙니다. 두 언어를 지원하는 같은 시스템 글꼴로 언어 전환 후 위치가 어긋나는 문제를 막았습니다.
- 실행 파일과 게임 팩만 별도로 둔 실제 혼자 캠페인 화면: `build/pack-probe-0.7/artifacts/world.png`. 로그: `build/pack-probe-0.7/packed-world.log`. 이 PC의 독립 실행을 확인한 것이며 다른 PC 설치 검증은 아닙니다.
- 검토에서 첫 실행 음량, 늦은 참가의 부당 완주 기록, 실제 단계 안내 누락, 도움말 토글, 한국어 경적 포맷, 접지 화물 운반, 손님 화물 정지 상태 문제를 찾아 해결했습니다. UI 수정 후 관련 캠페인 회귀도 통과했습니다.
- 제한된 실행 환경에서 OS 인증서 저장소와 셰이더 캐시/사용자 데이터 권한 경고가 발생합니다. 게임 검사에는 스크립트 오류가 없고 ConfigFile 저장은 격리된 검사 경로에서 확인했습니다. 다른 PC의 일반 사용자 설치/저장은 외부 QA에 남깁니다.

최종 통신은 프로토콜 7, 방장 참가 승인, 이동 20Hz ≤1280바이트, 크리쳐 10Hz ≤256바이트, 캠페인 5Hz/전환 시 변경 확인 후 달라졌을 때만 reliable 전송(≤2048바이트)입니다. Steam 중계/초대와 방장 이전은 없습니다. 포장쥐 최종값은 2.7m/s, 절도 반경 1.4m, 예고 0.9초, 경적 4m/8초(개선 시 6초), 도주 3초, 반납 화물 보호 8초입니다. 새 판은 금고·장비를 초기화합니다. 기록 형식 1은 집계와 마지막 완료 ID를 보관하며 이전 버전 폴더 자동 이전은 없습니다.

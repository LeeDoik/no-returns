# 변경 범위에 맞는 빠른 검사 — 0.9.19

[English](quick-testing.en.md)

## 현재 구현 — 개인 장비와 치료품 0.9.25

협동 1칸·솔로 2칸, 출발 전 40 CR 치료품 구매·장착, 현장 교환과 자신/동료 완전 치료를 연결했다. 새 Cinder 세션 자금은 400 CR이다. 미사용 장착분은 세션 내 유지하고 다운 횟수는 치료로 초기화하지 않는다. 게임 0.9.25 / 프로토콜 16 / TCP 27842. [현재 명세·제작·검증 범위](personal-equipment.ko.md).

## 현재 구현 — 여러 배송 0.9.24

인원에 따른 3–6개 실물 화물, 개별 운반·검수 중단/재개·영수증과 구역 묶음 보너스를 구현했다. 게임 0.9.24 / 프로토콜 15 / TCP 27842. 기존 단일 420 CR 배송 설명은 일반 Cinder에서 대체된다. [현재 규칙·제작 근거·남은 작업](multiple-deliveries.ko.md).

기존 `test_cinder_threat.py`와 `test_cinder_suppression.py`는 단일 화물·과거 전원 회복 판정에 묶여 있어 현재 버전에서 창을 열기 전에 중단한다. 전체 위험 회귀의 새 규칙 이행은 미완료다. 현재 배송 검사는 `test_cinder_delivery.py --headless`, 국소 전투·구조 검사는 `cinder_quick_test.py`를 사용하며 서로 대체하지 않는다.

## 2026-10-05 — 방문 장부·부활·철수 0.9.23

Cinder 영수증 420 CR을 미정산 장부에 보관하고 출발 때만 지급한다. 선내 생존자는 미정산 100 CR으로 탈락자 한 명을 10초 뒤 부활시킨다. 누구나 12초 출발을 시작/취소하며 남겨진 동료당 100 CR 공제, 정산 최저 0 CR. 체력·반복 다운/탈락과 부활 중 전멸/이륙 판정을 연결했다. [실제 구현·수치·제한](visit-settlement.ko.md) · [검증](../validation/visit-settlement-0.9.23.json). 게임 0.9.23·프로토콜 14·TCP 27842. 아래 과거 Cinder 전원 탑승 귀환/즉시 정산/일격 다운 규칙을 이 범위에서 대체한다. 새 장비·여러 배송·Cinder 저장/재접속·최종 위험 시간/음향·사람 평가는 남는다.

Editor 규칙 32개, 화면 없는 실제 2인 방문 17개·구조 12개, 화면 없는 실제 4인 배송 52개를 검증했다. 첫 구조 검사 HUD 예외를 수정해 재검사했다. 최종 UI 안내 수정 후 빌드/2인 방문을 다시 검사했다. 사람 화면 평가는 하지 않았다. 새 검사는 `python3 tools/test_cinder_visit.py`; 최신 빌드가 필요하다.

## 기본 원칙

매 작업마다 native 빌드와 배송 전체를 반복하지 않는다. 구현 확인과 사람의 품질 판단을 구분한다. [저장소 작업 규칙](../../AGENTS.md)과 [현재 체크리스트](05-validation.ko.md)를 함께 따른다.

| 변경 | 기본 검사 | 확대 조건 |
|---|---|---|
| 문서 | `python3 tools/check_docs.py` | 코드/에셋 변경 없음이면 종료 |
| 손·팔·외형 | 컴파일 + 관련 Editor 자세/화면 + 문서 | 연결/물리/동기화도 바뀌면 해당 기능 검사 |
| 운반·구조·진압봉 | 해당 빠른 기능 검사 | 실패하거나 공통 동작이 바뀌면 `all` |
| 접속·4인 규칙·배송·저장·큰 시스템 | 관련 기존 전체 검사 | 출시 전에는 필요한 전체 회귀 |
| 직접 플레이 | 사용자 요청 때만 사용자+동료 창 | 자동으로 실행하지 않음 |

예를 들어 동료 신호기 팔 조절은 `EmployeeBeaconReview.Review()`의 80개 표본이면 시작할 수 있다. 모든 1인칭 1,508개나 전체 배송을 자동으로 추가하지 않는다. 컴파일은 `unity command recompile` 후 `recompile_status`의 오류 없는 완료를 확인한다. Editor 전용 검사를 했다면 native/사람 검토는 생략·미확인으로 기록한다. 단순 외형 수정에서는 버전만 올리기 위한 native 빌드도 하지 않는다.

## 빠른 기능 검사

프로젝트 루트에서 실행한다. 기본은 기존 빌드 재사용이며 자동 빌드하지 않는다.

```sh
python3 tools/cinder_quick_test.py beacon
python3 tools/cinder_quick_test.py rescue
python3 tools/cinder_quick_test.py baton
python3 tools/cinder_quick_test.py all
```

현재 Unity 소스/에셋/설정과 실행본이 다르거나 기록이 없으면 **창을 열기 전에 거부**한다. native 검사가 필요한 작업이면 한 번만 명시적으로 재빌드한다.

```sh
python3 tools/cinder_quick_test.py all --build
```

[빌드 지문](../../tools/cinder_build_stamp.py)은 Unity Assets/Packages/ProjectSettings의 내용과 실행 파일 수정 시간을 기록한다. Editor 폴더와 빌더가 매번 재생성하는 CinderFourPlayerTest 씬은 제외한다. URP 전역 설정의 빌드 생성 `m_RuntimeSettings.m_List`만 정규화하며 나머지 실제 설정은 검사한다. 로컬 EmployeeLocal 에셋도 포함한다. 문서·Python 검사 코드 변경만으로 native 빌드를 요구하지 않는다. 원본 파일을 복원/수정하면 지문이 달라질 수 있으므로 기존 기록을 임의로 통과 처리하지 않는다. 기록은 무시되는 `artifacts/cinder-four-player/build-source.json`이다.

## 준비 상태와 실제 검사

[실행 도구](../../tools/cinder_quick_test.py)는 화면 없는 실제 클라이언트 **2개**를 띄우며 봇은 켜지 않는다. `all`은 같은 두 프로세스에서 세 상태를 순서대로 준비한다. 기존 세션이 있으면 중복 실행을 거부하고, 종료 후 포트가 정리 중일 때만 최대 30초 기다린다. 정상/실패 시 실행한 세션을 종료한다.

| 항목 | 준비하는 상태 | 실제 입력으로 검증하는 것 |
|---|---|---|
| 신호기 | 현장·구매 완료·2회 사용·가까운 장비 | E 줍기, 본인/동료 양손, 배치·1회 소비·작동, 봉 복귀 |
| 구조 | 가까운 동료 1명 다운 | 구조 유지·진행·손 표시, 취소, 2.5초 구조·부활·봉 복귀 |
| 진압봉 | 사용 가능·2m 앞 대상 | 실제 타격·기절 공유, 자세 복귀, 재사용 차단 |

준비 단계에서만 위치·진행·다운·적 대기 상태를 주입한다. 적은 처음 60초 동안 준비 상태를 유지하므로 자연 조우·추적 난이도 시험은 아니다. 구매/배송/다운 원인도 생략한다. **행동 결과를 주입하거나 타격·구조·배치 판정을 대체하지 않는다.** 이후 기존 호스트 판정과 네트워크를 사용한다. 실제 구매 가격·배송 전체·자연 AI·4인·LAN/Windows·사람 품질 검증은 기존 전체 검사/직접 플레이 범위다.

[준비 코드](../../NoReturns/Assets/_NoReturns/Runtime/CarryLab/CarryRoom.QuickTest.cs)는 `CARRY_TEST_AUTOMATION` 또는 `UNITY_EDITOR`에서만 활성화한다. Cinder 위험 모드 + `--test-dir` + `--quick-test`가 모두 필요하며 방장만 요청할 수 있다. `fixture` 요청은 로컬 시험 파일에서만 읽고 네트워크 입력에 추가하지 않는다. 참가자는 방장 상태를 초기화할 수 없다. Cinder 시험 폴더를 사용하고 개인 진행 저장을 만들지 않는다. 일반 실행과 요청 시 사용자+동료 실행에는 준비 상태가 적용되지 않는다.

## 결과와 남은 범위

검사 이름·성공/실패·준비 상태별 시간·시작/종료 포함 총시간을 `artifacts/cinder-four-player/quick-latest.json`과 각 실행 폴더에 기록한다. `--build` 사용 시 총시간에는 빌드도 포함된다. 결과 비교에는 같은 조건을 사용한다. [구축 검증](../validation/quick-testing-0.9.19.json). 전체 배송 검사는 이번 환경 구축에서 재실행하지 않는다. 수동 게임 창도 열지 않는다.

[직접 플레이 안내](companion-play.ko.md) · [전체 배송 검사](../../tools/test_cinder_delivery.py) · [전체 위험 검사](../../tools/test_cinder_threat.py). 도구: Unity 6000.6.4f1·CLI 1.0.0-beta.12·Pipeline 0.8.0-exp.1 공식 최신 일치. 게임 0.9.19·프로토콜 13·TCP 27842.

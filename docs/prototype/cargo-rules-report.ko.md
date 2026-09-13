# 화물 규칙 구현 보고서

[English](cargo-rules-report.en.md)

> **과거 기록 · 2026-09-12 폐기/대체.** 현재 게임은 [PSX 우주 배송 기획](../current/01-overview.ko.md)을 따른다. 아래 구현·실행·아트·수치는 과거 자료이며 현재 상태가 아니다. 삭제된 파일은 경로 기록으로만 남긴다.


> 문서 체계 개정: 이 파일의 버전·수치는 작성 당시 기준입니다. 현재 규칙·미완료 상태는 [문서 홈](../README.md)을 먼저 확인하세요.

버전 0.1 · 2026-09-07 · 상태: 구현 및 행동 검증 완료.

## 범위

`scripts/cargo_rules.gd`는 첫 프로토타입 계획 1절의 독립된 `RefCounted` 화물 상태 모델을 구현한다. `tests/test_cargo_rules.gd`는 6개 행동 그룹과 45개 검증을 실행하는 독립형 `SceneTree` 테스트 러너다.

이 모델은 양수인 피어 ID, 유한한 위치, 비어 있고 미배송인 크레이트, 2.4미터 이내 거리 조건을 모두 만족할 때만 픽업을 허용한다. 소유자만 해제할 수 있고, 크레이트를 아무도 들지 않았을 때 도크 1에서만 디스패치하며, 크레이트당 점수는 한 번만 오른다. 소유자 접속 해제 시 크레이트를 놓고, 크레이트 초기화는 점수를 보존하며, 교대 초기화는 점수까지 지운다. 계획대로 접속 멤버십과 월드 접촉 판정은 이 모델의 범위 밖이다.

## 테스트 근거

프로덕션 모듈이 없는 상태에서 먼저 RED를 실행했다.

```powershell
.tools/godot/Godot_v4.7.2-stable_win64_console.exe --headless --path . --log-file artifacts/cargo-rules-red.log --script tests/test_cargo_rules.gd
```

결과: 종료 코드 1. Godot이 `Preload file "res://scripts/cargo_rules.gd" does not exist`를 출력하고 테스트 스크립트를 불러오지 못했다. 의도한 기능 누락 실패였다.

구현 후 GREEN을 실행했다.

```powershell
.tools/godot/Godot_v4.7.2-stable_win64_console.exe --headless --path . --log-file cargo-rules-green.log --script tests/test_cargo_rules.gd
```

결과: Godot `4.7.2.stable.official.ed1daf0bf`에서 종료 코드 0과 `PASS: cargo rules behavioral tests`를 확인했다.

검증 경계: 픽업 경쟁, 정확한 픽업 거리와 초과 거리, 0과 음수 피어 ID, NaN과 무한대 좌표, 타인 및 유효하지 않은 해제, 보유 중 디스패치, 잘못되거나 유효하지 않은 도크, 중복 디스패치, 배송 후 픽업, 소유자와 비소유자의 접속 해제, 크레이트 초기화, 크레이트 초기화 후 재득점, 교대 초기화.

## 관찰 사항

구현 직후 첫 실행에서 정확히 2.4미터인 경계값의 부동소수점 표현 차이가 드러났다. 최종 비교는 Godot의 근사 부동소수점 허용 범위에서 같은 값은 받아들이면서 2.4001미터는 계속 거절한다.

통과한 오프라인 테스트가 끝난 뒤 Godot이 `Failed to read the root certificate store`를 출력했다. 종료 코드와 화물 규칙 검증에는 영향을 주지 않았다. 이 보고서는 네트워크, 씬, 전송 계층 또는 멀티플레이 통합 검증을 주장하지 않는다.

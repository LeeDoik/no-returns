# 재채기 규칙 구현 보고서

[English](sneeze-rules-report.en.md)

> 문서 체계 개정: 이 파일의 버전·수치는 작성 당시 기준입니다. 현재 규칙·미완료 상태는 [문서 홈](../README.md)을 먼저 확인하세요.

버전 0.2 · 2026-09-07 · 상태: 구현 및 행동 검증 완료.

## 범위

`scripts/sneeze_rules.gd`는 0.2 계획 Task 1의 독립된 `RefCounted` 재채기 시계 및 대상 판정 모델을 구현한다. `tests/test_sneeze_rules.gd`는 9개 행동 그룹과 67개 검증 호출을 실행하는 독립형 `SceneTree` 테스트 러너다.

시계는 6초 평온 상태로 시작해 1.5초 전조와 0.35초 분출을 거친 뒤, 시드로 결정되는 6–9초 평온 구간을 선택한다. `advance` 호출 한 번은 전이를 최대 한 번만 통과한다. 전조에서 분출로 전환될 때만 `true`를 반환하고 단조 증가 이벤트 ID를 올린다. 재설정은 현재 단계를 취소하고 최초 6초 평온 상태를 복원하며 이벤트 ID는 보존한다. 0, 음수, NaN, 무한대 델타는 상태를 바꾸지 않는다.

정적 대상 필터는 정규화한 수평 방향, 최대 4.5미터의 수평 거리, 별도로 계산한 최대 1.6미터의 수직 차이, 경계를 포함하는 50도 원뿔 반각을 사용한다. 유한하지 않은 표본, 수평 성분이 0인 방향, XZ 평면에서 원점과 겹쳐 대상 방향을 구할 수 없는 위치는 거절한다.

## 테스트 근거

프로덕션 모듈이 없는 상태에서 먼저 RED를 실행했다.

```powershell
.tools/godot/Godot_v4.7.2-stable_win64_console.exe --headless --path . --log-file "C:\Users\LeeDoik\Documents\ChatGPT\게임제작_1\artifacts\sneeze-rules-red.log" --script tests/test_sneeze_rules.gd
```

결과: 종료 코드 1. Godot이 `Preload file "res://scripts/sneeze_rules.gd" does not exist`를 출력하고 테스트 스크립트를 불러오지 못했다. 의도한 기능 누락 실패였다.

구현 후 GREEN을 실행했다.

```powershell
.tools/godot/Godot_v4.7.2-stable_win64_console.exe --headless --path . --log-file "C:\Users\LeeDoik\Documents\ChatGPT\게임제작_1\artifacts\sneeze-rules-green.log" --script tests/test_sneeze_rules.gd
```

결과: Godot `4.7.2.stable.official.ed1daf0bf`에서 종료 코드 0과 `PASS: sneeze rules behavioral tests`를 확인했다.

검증 경계: 정확한 시계 전이와 전이 직전, 전조 중 이벤트 없음, 분출 시작 시 이벤트 한 번, 매우 긴 프레임의 전이 제한, 시드 기반 구간의 범위와 재현성, 분출 전후 재설정, 유한 델타 검증, 정면·후면·측면 대상, 각도·거리·높이의 정확한 경계와 초과값, 수직 조준 성분 제거, 겹치는 XZ 위치, 유한하지 않은 기하 표본.

## 관찰 사항

부동소수점 비교는 Godot의 근사 부동소수점 허용 범위 안에서 시간 및 기하 경계와 같은 값을 받아들인다. 따라서 문서화한 경계는 포함하면서 테스트한 4.5001미터, 1.6001미터, 50.1도는 거절한다.

통과한 오프라인 테스트가 끝난 뒤 Godot이 `Failed to read the root certificate store`를 출력했다. 종료 코드와 검증에는 영향을 주지 않았다. 이 독립 보고서는 씬, 충돌 차폐, 임펄스, 렌더링, 오디오, 네트워크 또는 멀티플레이 통합 검증을 주장하지 않는다.

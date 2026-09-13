# NO RETURNS Unity 메인 개발 상태

[English](18-unity-mainline.en.md)

> **과거 기록 · 2026-09-12 폐기/대체.** 현재 게임은 [PSX 우주 배송 기획](01-overview.ko.md)을 따른다. 아래 구현·실행·아트·수치는 과거 자료이며 현재 상태가 아니다. 삭제된 파일은 경로 기록으로만 남긴다.


[현재 Manyfast 기획](19-no-returns-manyfast.ko.md) — NO RETURNS 배송 원정 상세 제안과 Unity 구현 경계.

2026-09-12 · 사용자 결정: NO RETURNS를 Unity로 개발하는 것을 메인으로 삼는다. SIDE EFFECTS는 사용자가 재미가 없다고 평가해 개발을 중단했다. 이는 사용자 품질 의견이며 자동 검사 결과로 반박하거나 완료 판정을 유지하지 않는다.

## 현재 기준과 실행

- 메인 프로젝트: `unity/NoReturns`, Unity 6000.6.0f1.
- 기본 장면: CarryLab (`../../unity/NoReturns/Assets/_Project/Scenes/CarryLab.unity`; retired file). 기준 실험실은 이동·카메라·물리 운반·던지기를 제공하며, 별도 NR-LOOP-01 장면에 작은 배송 실험을 추가했다.
- 05_Unity.cmd (`../../05_Unity.cmd`; retired file): Unity 편집기 열기. 06_Unity_Play.cmd (`../../06_Unity_Play.cmd`; retired file): 기존 NO RETURNS Unity 실행본.
- [현재 조작 명세](15-unity-controls.ko.md), [환경·MCP 안내](14-unity-environment.ko.md).
- Godot 원본·아트·배송 기획은 보존한다. Godot의 배송·온라인·맵 전체가 Unity에 이식됐다는 뜻은 아니다.

## 정리한 범위

SIDE EFFECTS 전용 Assets 폴더와 meta, Windows 빌드, 검사 결과 폴더, 실행 항목과 전용 개발 스크립트를 활성 경로에서 제거했다. 사용처가 없어진 com.unity.transport 직접 의존성을 Unity 패키지 관리로 제거했다. NO RETURNS의 향후 온라인 방식은 별도 결정한다. Unity CLI·Pipeline MCP·공유 TMP 및 기존 CarryLab은 유지한다.

중단 실험 압축본 (`../archive/side-effects-2026-09-12.zip`; retired file)에 소스·장면·도구·빌드·검사 근거·원문 문서를 보관했다. 343개 파일의 해시와 ZIP 무결성을 확인했다. 이전 문서 경로와 변경 이력은 보관 자료로 남기며 실행 안내로 사용하지 않는다. 복원은 별도 작업이다.

## 다음 작업과 검증

다음 구현은 NO RETURNS의 운반 감각을 기준으로 배송 목표·화물 특성·협동을 단계적으로 Unity에 연결한다. 이번 정리는 새 배송 기능이나 SIDE EFFECTS 메커니즘 이식을 포함하지 않는다.

정리 후 Unity 배치 컴파일과 CarryLab 회귀 24개가 종료 코드 0으로 통과했다. 문서 48개 검사 및 한영 수치 일치를 확인했다. 검사 로그 (`../../artifacts/unity/test.log`; retired file), 정리 목록 (`../../artifacts/unity/side-effects-cleanup.json`; retired file). 사람 조작감·배송 재미·온라인 출시는 미검증 상태를 유지한다.

## NR-LOOP-01 — 2026-09-12

[작은 병맛 배송 루프 개발 인계](21-slapstick-loop-handoff.ko.md). 별도 배송 실험을 구현했다. [NR-LOOP-01 실행·검증](22-slapstick-loop.ko.md)에서 실행 방법과 실제 검사 범위를 확인한다. CarryLab은 보존한다.

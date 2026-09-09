# 3. 맵·아트·기술 가이드

[English](03-guides.en.md)

검토일: 2026-09-09 · 게임 0.9.4 · 코드 기준: 이번 반응 장치 수정본.


## 실행·편집 진입점

- `00_게임 실행.cmd`: 마지막 Windows 배포본 실행. 최신 소스 변경은 자동 반영되지 않는다.
- `PLAY.cmd`: 현재 소스 실행. `01_맵 편집.cmd` 또는 `EDIT_MAP.cmd`: 맵 편집.
- `03_물리 실험실.cmd`: 독립 시험 맵. `BUILD.cmd`: 배포본 갱신. `02_배포 파일 찾기.cmd`: 배포 위치 확인.

## 맵 제작

원본은 [shipping_shrine.tscn](../../scenes/maps/shipping_shrine.tscn)이다. Geometry는 고정 공간, Gameplay는 배송구·벨트·쥐·시작점, Environment는 조명, Decoration은 장식이다. 맵 단독 F6와 게임 전체 F5의 실행 목적이 다르다.

1. 맵 원본을 열고 대상의 부모 노드를 선택한다. 장치 외형만 옮기면 판정과 분리될 수 있다.
2. 위치·회전은 Transform, 벽 크기는 SolidBlock의 Dimensions로 조정한다. 단순 메시에는 자동으로 충돌이 생기지 않는다.
3. 직원·화물 시작점을 바닥에 맞추고 겹침을 피한다. 공간을 늘리면 바닥·외벽·PlayableBounds·조명·장치 구역을 함께 확인한다.
4. 쥐의 경유점 사이를 실제로 통과할 수 있게 한다. 복잡한 장애물 자동 우회는 보장되지 않는다.
5. 저장 후 실제 캐릭터로 빈손·운반·던지기·카메라 가림을 확인한다. 변경 맵을 친구에게 배포할 때 같은 빌드를 사용한다.

노드별 자세한 조작은 [맵 편집 상세](../development/map-editing.ko.md), 환경 기능은 [반응 장치 기록](../prototype/08-reactive-delivery.ko.md)을 참고한다. 상세 기록의 오래된 예제 좌표를 현재 배치로 단정하지 않는다.

## 아트·애니메이션 제작

`art/`는 제작 원본, `assets/art/release-01/`은 게임용 결과물이다. 직원의 현재 Blender 원본은 `art/worker-motion-02/worker-motion.blend`, 재현 도구는 `rebuild_motion.py`, 실행 모델은 `assets/art/release-01/worker.glb`다. 동작 선택·전환은 `scripts/worker_animation.gd`, 표시 자세는 `scripts/worker.gd`, 아트 연결은 `scripts/postal_art.gd`에서 관리한다.

원본을 보존하고 별도 후보를 만들어 크기·방향·재질·뼈 이름·동작 이름을 확인한다. 기존 출력 모델만 임의 교체하지 않는다. Godot 가져오기 후 관절 수치 검사와 이동·운반·회전·점프·착지·던지기 연속 녹화를 함께 수행한다. 스크린샷 한 장으로 자연스러움을 확정하지 않는다. 승인된 결과만 게임 모델에 반영하고 출처·권리 기록을 갱신한다. [제작 이력](../art/01-production.ko.md), [동작 재제작](../art/06-motion-rebuild.ko.md).

## 기술 구조와 변경 위치

| 변경할 기능 | 기준 코드 |
|---|---|
| 판 진행·입력·판정 연결 | main.gd |
| 직원 이동·충돌·표시 | worker.gd |
| 상자 물리·운반·상태 압축 | cargo.gd, cargo_rules.gd |
| 계약·장비·보상 | contracts.gd |
| 접속·프로토콜 | session.gd |
| 맵 참조·편집 요소 | editable_map.gd, editable_block.gd, map_zone.gd |
| UI·영어 원문/한국어 | interface.gd, copy.gd, campaign_copy.gd |
| 설정·기록 | preferences.gd, run_profile.gd |

위 코드는 모두 `scripts/` 아래에 있다. 방장이 판정하고 손님은 상태를 수신한다. 통신 형식을 바꾸면 프로토콜 변경 여부를 판단하고 모든 참여자를 같은 빌드로 검사한다. 서버 판정과 화면 연출을 혼동하지 않는다.

## 물리 실험실

본편 직원·상자 코드를 재사용하는 단일 사용자 시험이다. Tab/Esc 패널 전환, WASD 이동, Space 점프, E 집기/놓기, 왼쪽 클릭 던지기, 1~4 구역 이동, R 초기화다. 질량·마찰·반발·상자 중력·회전 감쇠·던지기 속도를 바꾸며 한 값씩 초기화 비교한다. [실험실 상세](../art/07-physics-lab.ko.md). 힘 기반 잡기 실험은 현재 제거됐다.

## 작업 완료 절차

관련 코드/씬과 이 문서를 확인 → 변경 → 필요한 검사 → 현재 명세·미완료·검증 문서를 한영 동시 갱신 → 변경 기록 → 로컬 Git 저장 순서다. 문서만 바꾼 작업은 링크·언어·수치 일치 검사를 하고 게임 전체 검사를 반복하지 않는다. 사용자 변경과 관련 없는 파일은 되돌리거나 함께 커밋하지 않는다.

## 반응 장치 아트 변경 시

점프대의 Top/Launch plate와 금속 지지 메시 이름은 표시 코드에서 찾는다. 모델 재내보내기 후 지지부 접촉 검사를 수행한다. 상자 더미 잔해는 배송 화물이 아니다. [0.9.4 제작·검증 상세](../art/09-reactive-impact.ko.md).

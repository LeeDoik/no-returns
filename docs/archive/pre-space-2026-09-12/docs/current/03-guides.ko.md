# 3. 맵·아트·기술 가이드

[English](03-guides.en.md)

> **과거 기록 · 2026-09-12 폐기/대체.** 현재 게임은 [PSX 우주 배송 기획](../../../../current/01-overview.ko.md)을 따른다. 아래 구현·실행·아트·수치는 과거 자료이며 현재 상태가 아니다. 삭제된 파일은 경로 기록으로만 남긴다.


현재 메인 개발은 NO RETURNS Unity다. 아래 Godot 명세·검증은 보존된 기존 게임의 기준이며 Unity 구현 완료를 뜻하지 않는다. 현재 기준 / Current baseline (`18-unity-mainline.ko.md`; retired file).

검토일: 2026-09-09 · 게임 0.9.4 · 코드 기준: 이번 반응 장치 수정본.


## 실행·편집 진입점

- `00_게임 실행.cmd`: 마지막 Windows 배포본 실행. 최신 소스 변경은 자동 반영되지 않는다.
- `PLAY.cmd`: 현재 소스 실행. `01_맵 편집.cmd` 또는 `EDIT_MAP.cmd`: 맵 편집.
- `03_물리 실험실.cmd`: 독립 시험 맵. `BUILD.cmd`: 배포본 갱신. `02_배포 파일 찾기.cmd`: 배포 위치 확인.

## 맵 제작

원본은 shipping_shrine.tscn (`../../scenes/maps/shipping_shrine.tscn`; retired file)이다. Geometry는 고정 공간, Gameplay는 배송구·벨트·쥐·시작점, Environment는 조명, Decoration은 장식이다. 맵 단독 F6와 게임 전체 F5의 실행 목적이 다르다.

1. 맵 원본을 열고 대상의 부모 노드를 선택한다. 장치 외형만 옮기면 판정과 분리될 수 있다.
2. 위치·회전은 Transform, 벽 크기는 SolidBlock의 Dimensions로 조정한다. 단순 메시에는 자동으로 충돌이 생기지 않는다.
3. 직원·화물 시작점을 바닥에 맞추고 겹침을 피한다. 공간을 늘리면 바닥·외벽·PlayableBounds·조명·장치 구역을 함께 확인한다.
4. 쥐의 경유점 사이를 실제로 통과할 수 있게 한다. 복잡한 장애물 자동 우회는 보장되지 않는다.
5. 저장 후 실제 캐릭터로 빈손·운반·던지기·카메라 가림을 확인한다. 변경 맵을 친구에게 배포할 때 같은 빌드를 사용한다.

노드별 자세한 조작은 맵 편집 상세 (`../development/map-editing.ko.md`; retired file), 환경 기능은 반응 장치 기록 (`../prototype/08-reactive-delivery.ko.md`; retired file)을 참고한다. 상세 기록의 오래된 예제 좌표를 현재 배치로 단정하지 않는다.

## 아트·애니메이션 제작

`art/`는 제작 원본, `assets/art/release-01/`은 게임용 결과물이다. 직원의 현재 Blender 원본은 `art/worker-motion-02/worker-motion.blend`, 재현 도구는 `rebuild_motion.py`, 실행 모델은 `assets/art/release-01/worker.glb`다. 동작 선택·전환은 `scripts/worker_animation.gd`, 표시 자세는 `scripts/worker.gd`, 아트 연결은 `scripts/postal_art.gd`에서 관리한다.

원본을 보존하고 별도 후보를 만들어 크기·방향·재질·뼈 이름·동작 이름을 확인한다. 기존 출력 모델만 임의 교체하지 않는다. Godot 가져오기 후 관절 수치 검사와 이동·운반·회전·점프·착지·던지기 연속 녹화를 함께 수행한다. 스크린샷 한 장으로 자연스러움을 확정하지 않는다. 승인된 결과만 게임 모델에 반영하고 출처·권리 기록을 갱신한다. 제작 이력 (`../art/01-production.ko.md`; retired file), 동작 재제작 (`../art/06-motion-rebuild.ko.md`; retired file).

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

본편 직원·상자 코드를 재사용하는 단일 사용자 시험이다. Tab/Esc 패널 전환, WASD 이동, Space 점프, E 집기/놓기, 왼쪽 클릭 던지기, 1~4 구역 이동, R 초기화다. 질량·마찰·반발·상자 중력·회전 감쇠·던지기 속도를 바꾸며 한 값씩 초기화 비교한다. 실험실 상세 (`../art/07-physics-lab.ko.md`; retired file). 힘 기반 잡기 실험은 현재 제거됐다.

## 작업 완료 절차

관련 코드/씬과 이 문서를 확인 → 변경 → 필요한 검사 → 현재 명세·미완료·검증 문서를 한영 동시 갱신 → 변경 기록 → 로컬 Git 저장 순서다. 문서만 바꾼 작업은 링크·언어·수치 일치 검사를 하고 게임 전체 검사를 반복하지 않는다. 사용자 변경과 관련 없는 파일은 되돌리거나 함께 커밋하지 않는다.

## 반응 장치 아트 변경 시

점프대의 Top/Launch plate와 금속 지지 메시 이름은 표시 코드에서 찾는다. 모델 재내보내기 후 지지부 접촉 검사를 수행한다. 상자 더미 잔해는 배송 화물이 아니다. 0.9.4 제작·검증 상세 (`../art/09-reactive-impact.ko.md`; retired file).

## 리소스 목록

게임용 13개 GLB, 외부 이미지 21개와 제작 원본·씬·셰이더 및 코드 생성 요소는 리소스 전체 목록 (`09-resource-inventory.ko.md`; retired file)을 참고한다. 2026-09-09 정적 조사이며 품질·권리 검토 완료를 뜻하지 않는다.

## 에셋 생성 파이프라인 확인 · 2026-09-09

시안·스타일 확인 → 에셋별 초안 제작 → Blender 정리·동작 제작 → GLB 내보내기 → Godot 연결 → 게임 화면·동작 검증 순서다. 모든 에셋이 모든 도구를 거치지는 않는다.

- GPT Image: 초기 재채기 상자 이미지 시안. Higgsfield: release-01 스타일 이미지·재질·캐릭터 메시 제작 경로.
- Blender: 생성 메시 정리와 상자·시설 직접 모델링, 크기·축·재질·동작 정리. 초기 PS1 재채기 상자도 Blender 스크립트로 제작했다.
- Tripo: 직원 리깅·동작 제작 이력. 현재 직원 동작은 Blender에서 다시 제작했으며, 상자·장치 반응은 Godot 코드 연출도 사용한다.
- 원본은 art/, 게임용 결과는 assets/art/release-01/. 초기 art/sneezer는 게임용 재채기 상자와 별도다.
- GLB 재가져오기, 크기·재질·동작 검사 후 게임에서 읽힘·충돌·연속 움직임을 확인한다. 관련 변경은 온라인 검사도 수행한다. 생성 성공과 게임 적용·품질 승인을 구분한다.

근거: 제작 이력 (`../art/01-production.ko.md`; retired file), 동작 재제작 (`../art/06-motion-rebuild.ko.md`; retired file), 리소스 목록 (`09-resource-inventory.ko.md`; retired file). 이번 확인은 기록·원본·코드 조사이며 서비스 연결이나 신규 렌더·플레이 검사는 수행하지 않았다.

## 품질 우선 파이프라인 제안 · 2026-09-09

상태: 사용자에게 추천하는 방식이며 채택·구현 완료가 아니다. GPT Image, Higgsfield, Tripo를 모두 사용할 수 있다는 조건으로 제안한다. 실제 계정 잔액·사용 모델·연결은 이번에 확인하지 않았다.

1. 스타일 기준: 대표 직원·상자·시설을 같은 게임 카메라와 조명에서 비교한다. 이전 PS1 시안과 현재 release-01 방향을 자동으로 혼합하지 않는다. 색상, 실루엣, 텍스처 밀도, 표면 거칠기, 그림자 기준을 먼저 맞춘다.
2. 시안: GPT Image와 Higgsfield 이미지 도구로 주요 에셋의 후보 3~5개를 비교한다. 채택한 한 디자인에서 정면·측면·후면을 파생하고 부품 위치·비율·색을 대조한다. 모순된 여러 뷰보다 일관된 단일 뷰가 낫다. 생성된 회전 영상은 정확한 3D 구조의 증거로 취급하지 않는다.
3. 캐릭터: Tripo로 주요 모델 후보 2~3개를 만들고, 무채색 360도 검수로 실루엣과 구조를 고른다. 필요 부품 분리와 게임용 메시 정리를 거쳐 재질·리깅을 진행한다. 자동 리깅 결과의 관절 변형과 운반 손 접촉은 Blender에서 수정한다. 후보 수는 제작 제안이지 측정된 최적값이 아니다.
4. 상자·시설: Blender에서 뚜껑·입·발판·벨트 등 가동부와 반복 모듈을 직접 구성한다. GPT Image/Higgsfield의 디자인·재질 이미지를 활용한다. 단순 부품까지 무조건 3D 생성에 통과시키지 않는다.
5. 재질: 생성 이미지는 재질 원본 후보로 사용한다. Blender에서 UV, 색 통일, 이음새, 조명이 그림처럼 박힌 흔적을 정리하고 필요 시 게임용 메시로 굽는다. 고해상도 원본을 보존하고 실제 화면에 맞게 해상도를 선택한다. PS1 방향 채택 시 낮은 텍스처 해상도와 거친 음영을 의도적으로 유지한다.
6. 동작·검수: Tripo 동작은 초안으로 사용하고 게임 전용 운반·던지기·재채기는 Blender/Godot에서 조정한다. GLB를 재가져온 뒤 게임 카메라로 읽힘, 바닥·손 접촉, 전환, 여러 캐릭터와 소품이 함께 있을 때의 프레임 시간을 확인한다. 대표 직원·재채기 상자·시설 모듈을 먼저 완성한 후 확장한다.

크레딧은 디자인·구조 후보 비교와 선택한 결과의 재질 개선에 집중한다. 여러 도구를 순서대로 거쳤다는 사실이 품질 우위를 보장하지 않는다. 이 프로젝트용 비교 생성은 아직 수행하지 않았다.

기능 근거: [Tripo 다중 뷰](https://developers.tripo3d.ai/en/docs/generation-multiview-to-model), [Tripo 제작·정리·리깅](https://www.tripo3d.ai/blog/tripo-studio-tutorial-english), [Higgsfield Blender 도구](https://higgsfield.ai/blog/higgsfield-blender-plugin). 위 단계별 배치는 해당 기능과 프로젝트 이력을 바탕으로 한 제작 판단이다.

## 전 에셋 일관성과 이미지 승인 — 사용자 확정 규칙 · 2026-09-09

게임 내 모든 에셋은 공통 색상 역할·형태 언어·텍스처 밀도·거칠기·조명 기준을 따른다. 개별 파일의 디테일보다 같은 게임 화면에서의 일관성을 우선한다. 이미지 시안을 먼저 사용자에게 보여주고, 명시적으로 컨펌받은 이미지만 3D 모델로 제작한다. 이미지 생성 완료나 무응답은 승인으로 간주하지 않는다. 현재 대표 3종은 직원·재채기 상자·컨베이어 시안 (`../../art/quality-trio-01/README.ko.md`; retired file)으로 검토하며 현재 D안이 승인돼 제작·적용됐다.


D안 3종은 사용자 승인 후 제작 중이다. 현재 파일·검증 상태와 API/Studio 구분은 대표 아트 제작 기록 (`../../art/quality-trio-01/README.ko.md`; retired file)을 따른다. 현재 게임 적용과 검증 상태는 D안 적용 명세 (`../art/10-approved-d.ko.md`; retired file)를 따른다.


D안 제작·재가져오기 가이드 (`../art/10-approved-d.ko.md`; retired file) — 새 모델의 팀 색상은 CrewMask를 COLOR_0으로 내보내야 한다.

## 원정 콘텐츠 제작 지침 · 2026-09-10

확장 기획 (`08-expansion-directions.ko.md`; retired file)의 구역·계약·도구 표를 향후 제작 기준으로 사용한다. 배송 신전 원본을 보존하고 별도 원정 맵에서 검증한다. 구역마다 안전 연결, 선택 연결, 수령 소켓, 회수 소켓, 시설 지정 면, 쥐 둥지와 안전 구역을 명시한다. 160×160m는 공간 범위 제안이며 꽉 채울 면적 목표가 아니다. 장식 문서 연출과 경로 차단 판정을 분리하고, 들고 있는 0.8m 화물의 회전·카메라 여유를 확인한다. 새 디자인 이미지는 사용자 승인 뒤 3D 제작하며 D안 색·재질·비율을 유지한다. 아직 저작용 노드나 원정 맵이 추가된 것은 아니다.

## EXP-01 실제 구현 · 2026-09-10

Godot에서 원정 씬 (`../../scenes/maps/toypost_village.tscn`; retired file)의 Ground/Truck/구역 물체를 편집한다. Gameplay의 WorkerSpawns/CargoSpawns/Receipts/TruckBase 및 계약 1~5 이름을 보존한다. 경계 40m·수령 반경 1.8m·트럭 반경 6m는 expedition_map.gd와 함께 검토한다. 초기 저작 도구 build_expedition_map.gd는 재실행 시 씬 편집을 덮으므로 일반 편집에 사용하지 않는다. 런타임 지형 재생성은 없다. 변경 후 python tools/run_expedition_tests.py와 실제 운반·카메라 검토를 수행한다.

플레이 안내와 한계 (`../prototype/10-expedition.ko.md`; retired file).


Unity 프로젝트·CLI 개발 환경 (`14-unity-environment.ko.md`; retired file) · 2026-09-11 · Unity 6000.6.0f1 / CLI 1.0.0-beta.9


Unity 이동·운반 실험실 (`15-unity-controls.ko.md`; retired file) · UNITY-02 / Unity 0.2.0 / 2026-09-11


2026-09-12 · 새 Unity 배송 실험: NR-LOOP-01 실행·검증 (`22-slapstick-loop.ko.md`; retired file) — 구현·자동 검사·사람 검증의 구분은 해당 문서를 따른다.

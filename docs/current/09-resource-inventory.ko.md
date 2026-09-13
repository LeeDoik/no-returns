# 현재 리소스 전체 목록

[English](09-resource-inventory.en.md)

> **과거 기록 · 2026-09-12 폐기/대체.** 현재 게임은 [PSX 우주 배송 기획](01-overview.ko.md)을 따른다. 아래 구현·실행·아트·수치는 과거 자료이며 현재 상태가 아니다. 삭제된 파일은 경로 기록으로만 남긴다.


조사일: 2026-09-09. 코드·파일 정적 조사이며 실행 중 사용 추적이나 렌더 품질 검사는 아닙니다. 게임 코드 변경 없음.

범위: assets/·art/의 모델·이미지·제작 원본, scenes/ 씬, scripts/ 셰이더 및 아래 코드 생성 리소스. 캐시(.import/.godot), 배포본(build/), 검사 캡처(artifacts/), 도구·문서·작업 메타데이터는 에셋 수에서 제외합니다. 개별 파일은 맨 아래에 모두 나열합니다.

34 게임용 폴더 파일, 97 조사 대상 파일.

## 게임용 모델

| 리소스 | 파일 | 메시 | 내장 이미지 | 애니메이션 |
|---|---|---:|---:|---|
| 접착 상자 | clinger.glb (`../../assets/art/release-01/clinger.glb`; retired file) | 21 | 0 | — |
| 컨베이어 모듈 | conveyor_module.glb (`../../assets/art/release-01/conveyor_module.glb`; retired file) | 4 | 0 | — |
| 출고 장치 | dispatch.glb (`../../assets/art/release-01/dispatch.glb`; retired file) | 6 | 0 | — |
| 점프 상자 | hopper.glb (`../../assets/art/release-01/hopper.glb`; retired file) | 22 | 0 | — |
| 포장 완충재 | packing_cushion.glb (`../../assets/art/release-01/packing_cushion.glb`; retired file) | 9 | 0 | — |
| 포장쥐 | packrat.glb (`../../assets/art/release-01/packrat.glb`; retired file) | 1 | 4 | — |
| 천장 조명 | pendant.glb (`../../assets/art/release-01/pendant.glb`; retired file) | 4 | 0 | — |
| 스프링 발판 | return_spring.glb (`../../assets/art/release-01/return_spring.glb`; retired file) | 4 | 0 | — |
| 선반 | shelf.glb (`../../assets/art/release-01/shelf.glb`; retired file) | 3 | 0 | — |
| 재채기 상자 | sneezer.glb (`../../assets/art/release-01/sneezer.glb`; retired file) | 20 | 0 | — |
| 일반 상자 | standard.glb (`../../assets/art/release-01/standard.glb`; retired file) | 9 | 0 | — |
| 작업대 | workbench.glb (`../../assets/art/release-01/workbench.glb`; retired file) | 5 | 0 | — |
| 직원 | worker.glb (`../../assets/art/release-01/worker.glb`; retired file) | 1 | 1 | air_fall, air_rise, carry_air, carry_back, carry_back_left, carry_back_right, carry_forward_left, carry_forward_right, carry_idle, carry_left, carry_right, carry_walk, hit_to_body_01, idle, land, run, throw, walk |

사용 근거: 아트 로더 (`../../scripts/postal_art.gd`; retired file), 직원 (`../../scripts/worker.gd`; retired file), 화물 (`../../scripts/cargo.gd`; retired file), 쥐 (`../../scripts/packrat.gd`; retired file), 반응 소품 (`../../scripts/reactive_prop.gd`; retired file). 시설 모델의 메시가 현재 맵 (`../../scenes/maps/shipping_shrine.tscn`; retired file)에 내장되어 있어 외부 GLB 참조만 검색하면 사용을 놓칩니다. 폴더 존재를 전부 매 프레임 로딩된다는 의미로 해석하지 않습니다.

## 재질·이미지·원본

게임용 외부 이미지는 21개: 골판지·페인트·바닥 각 5종(basecolor, normal, roughness, height, seamless), 직원 이미지 2개, 포장쥐 이미지 4개입니다. cardboard/paint의 basecolor·normal은 아트 로더에서 동적으로 연결하고 paint/floor의 basecolor·normal은 맵에서도 직접 참조합니다. 나머지 보조 맵은 이 조사에서 실행 참조를 확인하지 못했으며 삭제 대상으로 확정하지 않습니다.

art/release-01: 직원·쥐·작업장·골판지·페인트·바닥 시안 6장, postal-kit/worker/packrat Blender 원본과 생성 GLB. art/tripo-01: 직원 T포즈·동작 후보·원본·미리보기. art/worker-motion-02: 현재 직원 동작 Blender 원본과 GLB. art/sneezer: 초기 일반/병맛/PS1 시안 4장, 모델 v0.1/v0.2와 미리보기·텍스처·백업. art/sneezer 모델은 현재 게임의 release-01/sneezer.glb와 별도입니다.

## 파일 외의 코드·씬 리소스

- 맵: 바닥·벽·계단·경사로·칸막이·시설·조명·표지판·출고구·쥐 동선·상호작용 구역. 개별 노드와 재질은 씬에 내장.
- 반응 장치: 완충재, 스프링 발판, 일반 상자 모델 3개를 재사용하는 상자 탑, 코드로 만든 문서 더미·인쇄선·도장.
- 효과: 재채기 예고·바람·방향 표시, 접착/점프 화물 표시, 핑, 장치 반응·잔해. 코드 생성이며 독립 이미지 파일 목록만으로 포착되지 않음.
- UI: 메뉴·HUD·설정·계약·결과·한영 문자열. interface/copy/campaign_copy/release_copy/settings_panel 및 Label3D/SystemFont 기반.
- 오디오: sneeze_cues/feedback/reactive_prop에서 WAV 데이터를 합성. 외부 WAV/OGG/MP3와 TTF/OTF 파일은 조사 대상 소스 경로에 없음. 시스템 글꼴 사용.
- 셰이더: crew_suit.gdshader. 모델 재질과 게임에서 만든 StandardMaterial3D도 별도로 존재.

### 현재 맵 내부 집계

```json
{
  "nodes": {
    "Node3D": 431,
    "WorldEnvironment": 1,
    "DirectionalLight3D": 1,
    "OmniLight3D": 14,
    "MeshInstance3D": 1644,
    "Camera3D": 1,
    "StaticBody3D": 162,
    "CollisionShape3D": 171,
    "Label3D": 122,
    "Area3D": 9,
    "Marker3D": 40
  },
  "subresources": {
    "Environment": 1,
    "StandardMaterial3D": 1508,
    "ArrayMesh": 82,
    "BoxMesh": 1254,
    "Gradient": 1,
    "FastNoiseLite": 1,
    "NoiseTexture2D": 1,
    "BoxShape3D": 171,
    "SystemFont": 110,
    "CylinderMesh": 16,
    "TorusMesh": 5,
    "SphereMesh": 24
  },
  "art_nodes": {
    "Art_pendant": 14,
    "Art_shelf": 8,
    "Art_workbench": 7,
    "Art_dispatch": 2,
    "Art_conveyor_module": 8
  }
}
```

위 숫자는 씬 텍스트의 선언 수이며 실행 시 생성·복제·삭제 후 개체 수가 아닙니다.

## 개별 파일 전체 목록

| 경로 | 종류 | 바이트 |
|---|---|---:|
| assets/art/release-01/clinger.glb (`../../assets/art/release-01/clinger.glb`; retired file) | glb | 638152 |
| assets/art/release-01/conveyor_module.glb (`../../assets/art/release-01/conveyor_module.glb`; retired file) | glb | 317088 |
| assets/art/release-01/dispatch.glb (`../../assets/art/release-01/dispatch.glb`; retired file) | glb | 275120 |
| assets/art/release-01/hopper.glb (`../../assets/art/release-01/hopper.glb`; retired file) | glb | 571452 |
| assets/art/release-01/packing_cushion.glb (`../../assets/art/release-01/packing_cushion.glb`; retired file) | glb | 139460 |
| assets/art/release-01/packrat.glb (`../../assets/art/release-01/packrat.glb`; retired file) | glb | 8279884 |
| assets/art/release-01/packrat_Image_0.jpg (`../../assets/art/release-01/packrat_Image_0.jpg`; retired file) | jpg | 3293919 |
| assets/art/release-01/packrat_Image_1.jpg (`../../assets/art/release-01/packrat_Image_1.jpg`; retired file) | jpg | 1835691 |
| assets/art/release-01/packrat_Image_2.jpg (`../../assets/art/release-01/packrat_Image_2.jpg`; retired file) | jpg | 2191727 |
| assets/art/release-01/packrat_Image_3.jpg (`../../assets/art/release-01/packrat_Image_3.jpg`; retired file) | jpg | 85913 |
| assets/art/release-01/pendant.glb (`../../assets/art/release-01/pendant.glb`; retired file) | glb | 46556 |
| assets/art/release-01/return_spring.glb (`../../assets/art/release-01/return_spring.glb`; retired file) | glb | 306080 |
| assets/art/release-01/shelf.glb (`../../assets/art/release-01/shelf.glb`; retired file) | glb | 306356 |
| assets/art/release-01/sneezer.glb (`../../assets/art/release-01/sneezer.glb`; retired file) | glb | 532228 |
| assets/art/release-01/standard.glb (`../../assets/art/release-01/standard.glb`; retired file) | glb | 224888 |
| assets/art/release-01/textures/cardboard_basecolor.png (`../../assets/art/release-01/textures/cardboard_basecolor.png`; retired file) | png | 15970585 |
| assets/art/release-01/textures/cardboard_height.png (`../../assets/art/release-01/textures/cardboard_height.png`; retired file) | png | 5296492 |
| assets/art/release-01/textures/cardboard_normal.png (`../../assets/art/release-01/textures/cardboard_normal.png`; retired file) | png | 16736327 |
| assets/art/release-01/textures/cardboard_roughness.png (`../../assets/art/release-01/textures/cardboard_roughness.png`; retired file) | png | 7036822 |
| assets/art/release-01/textures/cardboard_seamless.png (`../../assets/art/release-01/textures/cardboard_seamless.png`; retired file) | png | 15970585 |
| assets/art/release-01/textures/floor_basecolor.png (`../../assets/art/release-01/textures/floor_basecolor.png`; retired file) | png | 16267315 |
| assets/art/release-01/textures/floor_height.png (`../../assets/art/release-01/textures/floor_height.png`; retired file) | png | 5186926 |
| assets/art/release-01/textures/floor_normal.png (`../../assets/art/release-01/textures/floor_normal.png`; retired file) | png | 16354810 |
| assets/art/release-01/textures/floor_roughness.png (`../../assets/art/release-01/textures/floor_roughness.png`; retired file) | png | 7181005 |
| assets/art/release-01/textures/floor_seamless.png (`../../assets/art/release-01/textures/floor_seamless.png`; retired file) | png | 16267315 |
| assets/art/release-01/textures/paint_basecolor.png (`../../assets/art/release-01/textures/paint_basecolor.png`; retired file) | png | 10412420 |
| assets/art/release-01/textures/paint_height.png (`../../assets/art/release-01/textures/paint_height.png`; retired file) | png | 4871511 |
| assets/art/release-01/textures/paint_normal.png (`../../assets/art/release-01/textures/paint_normal.png`; retired file) | png | 15530362 |
| assets/art/release-01/textures/paint_roughness.png (`../../assets/art/release-01/textures/paint_roughness.png`; retired file) | png | 5341554 |
| assets/art/release-01/textures/paint_seamless.png (`../../assets/art/release-01/textures/paint_seamless.png`; retired file) | png | 10412420 |
| assets/art/release-01/workbench.glb (`../../assets/art/release-01/workbench.glb`; retired file) | glb | 229788 |
| assets/art/release-01/worker.glb (`../../assets/art/release-01/worker.glb`; retired file) | glb | 9591636 |
| assets/art/release-01/worker_texture_0.png (`../../assets/art/release-01/worker_texture_0.png`; retired file) | png | 7638560 |
| assets/art/release-01/worker_worker-tripo-input_glb_basecolor.png (`../../assets/art/release-01/worker_worker-tripo-input_glb_basecolor.png`; retired file) | png | 7638560 |
| art/release-01/concepts/cardboard.png (`../../art/release-01/concepts/cardboard.png`; retired file) | png | 18931940 |
| art/release-01/concepts/depot.png (`../../art/release-01/concepts/depot.png`; retired file) | png | 12956554 |
| art/release-01/concepts/floor.png (`../../art/release-01/concepts/floor.png`; retired file) | png | 19069742 |
| art/release-01/concepts/packrat.png (`../../art/release-01/concepts/packrat.png`; retired file) | png | 6075376 |
| art/release-01/concepts/paint.png (`../../art/release-01/concepts/paint.png`; retired file) | png | 11184418 |
| art/release-01/concepts/worker.png (`../../art/release-01/concepts/worker.png`; retired file) | png | 5580957 |
| art/release-01/source/packrat-generated.glb (`../../art/release-01/source/packrat-generated.glb`; retired file) | glb | 8045144 |
| art/release-01/source/packrat.blend (`../../art/release-01/source/packrat.blend`; retired file) | blend | 8145510 |
| art/release-01/source/postal-kit.blend (`../../art/release-01/source/postal-kit.blend`; retired file) | blend | 710738 |
| art/release-01/source/worker-generated.glb (`../../art/release-01/source/worker-generated.glb`; retired file) | glb | 8983180 |
| art/release-01/source/worker.blend (`../../art/release-01/source/worker.blend`; retired file) | blend | 9183863 |
| art/sneezer/concepts/sneezer-states-ps1-v01.png (`../../art/sneezer/concepts/sneezer-states-ps1-v01.png`; retired file) | png | 1526885 |
| art/sneezer/concepts/sneezer-states-ps1-v02.png (`../../art/sneezer/concepts/sneezer-states-ps1-v02.png`; retired file) | png | 1396600 |
| art/sneezer/concepts/sneezer-states-v01.png (`../../art/sneezer/concepts/sneezer-states-v01.png`; retired file) | png | 1611205 |
| art/sneezer/concepts/sneezer-states-v02.png (`../../art/sneezer/concepts/sneezer-states-v02.png`; retired file) | png | 1617172 |
| art/sneezer/model-v01/previews/glb-reimport-sneeze.png (`../../art/sneezer/model-v01/previews/glb-reimport-sneeze.png`; retired file) | png | 875915 |
| art/sneezer/model-v01/previews/idle.png (`../../art/sneezer/model-v01/previews/idle.png`; retired file) | png | 870559 |
| art/sneezer/model-v01/previews/sneeze.png (`../../art/sneezer/model-v01/previews/sneeze.png`; retired file) | png | 875720 |
| art/sneezer/model-v01/previews/warning.png (`../../art/sneezer/model-v01/previews/warning.png`; retired file) | png | 869628 |
| art/sneezer/model-v01/sneezer-idle.glb (`../../art/sneezer/model-v01/sneezer-idle.glb`; retired file) | glb | 942512 |
| art/sneezer/model-v01/sneezer-idle_cardboard.png (`../../art/sneezer/model-v01/sneezer-idle_cardboard.png`; retired file) | png | 912480 |
| art/sneezer/model-v01/sneezer-sneeze.glb (`../../art/sneezer/model-v01/sneezer-sneeze.glb`; retired file) | glb | 942512 |
| art/sneezer/model-v01/sneezer-sneeze_cardboard.png (`../../art/sneezer/model-v01/sneezer-sneeze_cardboard.png`; retired file) | png | 912480 |
| art/sneezer/model-v01/sneezer-warning.glb (`../../art/sneezer/model-v01/sneezer-warning.glb`; retired file) | glb | 942512 |
| art/sneezer/model-v01/sneezer-warning_cardboard.png (`../../art/sneezer/model-v01/sneezer-warning_cardboard.png`; retired file) | png | 912480 |
| art/sneezer/model-v01/sneezer.blend (`../../art/sneezer/model-v01/sneezer.blend`; retired file) | blend | 1015798 |
| art/sneezer/model-v01/sneezer.glb (`../../art/sneezer/model-v01/sneezer.glb`; retired file) | glb | 950504 |
| art/sneezer/model-v01/sneezer_cardboard.png (`../../art/sneezer/model-v01/sneezer_cardboard.png`; retired file) | png | 912480 |
| art/sneezer/model-v01/textures/cardboard.png (`../../art/sneezer/model-v01/textures/cardboard.png`; retired file) | png | 912480 |
| art/sneezer/model-v02/previews/glb-reimport-sneeze.png (`../../art/sneezer/model-v02/previews/glb-reimport-sneeze.png`; retired file) | png | 885737 |
| art/sneezer/model-v02/previews/idle.png (`../../art/sneezer/model-v02/previews/idle.png`; retired file) | png | 878331 |
| art/sneezer/model-v02/previews/sneeze.png (`../../art/sneezer/model-v02/previews/sneeze.png`; retired file) | png | 885535 |
| art/sneezer/model-v02/previews/warning.png (`../../art/sneezer/model-v02/previews/warning.png`; retired file) | png | 879853 |
| art/sneezer/model-v02/sneezer.blend (`../../art/sneezer/model-v02/sneezer.blend`; retired file) | blend | 1030197 |
| art/sneezer/model-v02/sneezer.blend1 (`../../art/sneezer/model-v02/sneezer.blend1`; retired file) | blend1 | 1030173 |
| art/sneezer/model-v02/sneezer.glb (`../../art/sneezer/model-v02/sneezer.glb`; retired file) | glb | 1027052 |
| art/sneezer/model-v02/sneezer_cardboard.png (`../../art/sneezer/model-v02/sneezer_cardboard.png`; retired file) | png | 912480 |
| art/sneezer/model-v02/textures/cardboard.png (`../../art/sneezer/model-v02/textures/cardboard.png`; retired file) | png | 912480 |
| art/tripo-01/preview-air_rise.png (`../../art/tripo-01/preview-air_rise.png`; retired file) | png | 613307 |
| art/tripo-01/preview-carry_idle.png (`../../art/tripo-01/preview-carry_idle.png`; retired file) | png | 599032 |
| art/tripo-01/preview-carry_walk.png (`../../art/tripo-01/preview-carry_walk.png`; retired file) | png | 604310 |
| art/tripo-01/preview-idle.png (`../../art/tripo-01/preview-idle.png`; retired file) | png | 605111 |
| art/tripo-01/preview-jump.png (`../../art/tripo-01/preview-jump.png`; retired file) | png | 602158 |
| art/tripo-01/preview-lift_heavy.png (`../../art/tripo-01/preview-lift_heavy.png`; retired file) | png | 595494 |
| art/tripo-01/preview-run.png (`../../art/tripo-01/preview-run.png`; retired file) | png | 598775 |
| art/tripo-01/preview-throw.png (`../../art/tripo-01/preview-throw.png`; retired file) | png | 597642 |
| art/tripo-01/preview-walk.png (`../../art/tripo-01/preview-walk.png`; retired file) | png | 603134 |
| art/tripo-01/worker-animation.blend (`../../art/tripo-01/worker-animation.blend`; retired file) | blend | 11267608 |
| art/tripo-01/worker-candidate.glb (`../../art/tripo-01/worker-candidate.glb`; retired file) | glb | 9685620 |
| art/tripo-01/worker-tpose-angle.png (`../../art/tripo-01/worker-tpose-angle.png`; retired file) | png | 931538 |
| art/tripo-01/worker-tpose-front.png (`../../art/tripo-01/worker-tpose-front.png`; retired file) | png | 937326 |
| art/tripo-01/worker-tripo-input.blend (`../../art/tripo-01/worker-tripo-input.blend`; retired file) | blend | 8766776 |
| art/tripo-01/worker-tripo-input.glb (`../../art/tripo-01/worker-tripo-input.glb`; retired file) | glb | 8469988 |
| art/tripo-01/worker-tripo-seven.glb (`../../art/tripo-01/worker-tripo-seven.glb`; retired file) | glb | 9405548 |
| art/worker-motion-02/worker-motion.blend (`../../art/worker-motion-02/worker-motion.blend`; retired file) | blend | 12395974 |
| art/worker-motion-02/worker-motion.blend1 (`../../art/worker-motion-02/worker-motion.blend1`; retired file) | blend1 | 12345109 |
| art/worker-motion-02/worker.glb (`../../art/worker-motion-02/worker.glb`; retired file) | glb | 9591636 |
| scenes/main.tscn (`../../scenes/main.tscn`; retired file) | tscn | 321 |
| scenes/maps/shipping_shrine.tscn (`../../scenes/maps/shipping_shrine.tscn`; retired file) | tscn | 2889605 |
| scenes/physics_lab.tscn (`../../scenes/physics_lab.tscn`; retired file) | tscn | 171 |
| scenes/pieces/reactive_prop.tscn (`../../scenes/pieces/reactive_prop.tscn`; retired file) | tscn | 176 |
| scenes/pieces/solid_block.tscn (`../../scenes/pieces/solid_block.tscn`; retired file) | tscn | 942 |
| scripts/crew_suit.gdshader (`../../scripts/crew_suit.gdshader`; retired file) | gdshader | 846 |

기계용 목록: [JSON](09-resource-inventory.json). 현재 소스만 조사했으며 설치된 배포본과 동일성, 외부 권리, 최종 품질, 성능은 미확인입니다. 기존 ART-01/PERF-01 상태를 변경하지 않습니다.

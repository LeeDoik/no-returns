# 재채기 상자 — 3D 모델 v0.1

[English](README.en.md)

상태: 검토용 3D 모델 제작 완료. Godot 게임 적용, 바람 이펙트, 충돌 판정, 최종 최적화는 미완료입니다. 사용자 승인 시안은 [레퍼런스 반영 PS1 v0.2](../concepts/sneezer-states-ps1-v02.png)입니다. 영상의 그래픽 스타일만 참고하고 공포·인물·배경 콘셉트는 채택하지 않았습니다.

## 파일과 사용

- [sneezer.blend](sneezer.blend): 편집용 Blender 원본. 타임라인 1은 평상시, 30은 예고, 48은 재채기, 90은 복귀입니다. 재생하면 표정 교체, 몸체 눌림, 뚜껑 움직임을 확인할 수 있습니다.
- [sneezer.glb](sneezer.glb): 텍스처를 포함하는 게임용 내보내기. 애니메이션 이름은 `SneezeCycle`입니다. 30fps, 1~90프레임이며 실제 키 범위는 약 2.97초입니다.
- `sneezer-idle.glb`, `sneezer-warning.glb`, `sneezer-sneeze.glb`: 애니메이션 없는 상태별 내보내기. 사용하지 않는 얼굴은 0 배율로 남아 있으므로 최종 최적화 시 제거할 수 있습니다.
- [평상시](previews/idle.png), [예고](previews/warning.png), [재채기](previews/sneeze.png), [GLB 재가져오기 검증](previews/glb-reimport-sneeze.png): 실제 Blender 렌더입니다. 조명과 바닥은 검토용이며 GLB에 포함하지 않았습니다.

## 구성과 범위

Blender에서 직접 모델링했습니다. 얼굴은 돌출 조형 대신 표면 바로 앞의 얇은 픽셀 면으로 구성합니다. 이번 버전은 얼굴 텍스처 아틀라스가 아닙니다. 몸체·뚜껑·테이프·라벨·세 얼굴 상태를 각각 편집할 수 있습니다. 뼈대 없이 오브젝트 변환으로 애니메이션합니다. 모든 얼굴 상태를 합쳐 267삼각형입니다. 기본 크기는 약 0.8 × 0.72 × 0.76m, 바닥 중심이 원점입니다. Blender 정면은 -Y, glTF 정면은 +Z이며 게임에 배치할 때 방향을 맞춰야 합니다.

골판지는 내장 GPT Image 도구로 새로 생성했습니다. 파일은 [cardboard.png](textures/cardboard.png), 실제 해상도는 1254 × 1254입니다. 굵은 픽셀처럼 보이는 질감이며 실제 64~128px 텍스처나 PS1 하드웨어 사양을 충족한 결과는 아닙니다. 원본에 텍스처를 패킹했고 GLB에도 포함했습니다. 시안보다 얼룩과 종이 섬유 표현은 단순합니다.

## 검증

세 상태를 렌더링하고 육안으로 확인했습니다. GLB를 빈 모델 상태로 다시 가져와 각 시점에 얼굴이 하나만 활성화되는지와 뚜껑 회전을 검사하고 렌더링했습니다. 결과는 [모델 수치](build-report.json), [GLB 구조](glb-validation.json), [재가져오기 검사](import-validation.json)에 있습니다. 실제 Godot 플레이 환경의 가독성·재질·성능·게임 규칙과의 동기화는 아직 확인하지 않았습니다.

다른 세션의 게임 코드·맵·씬과 열려 있는 Blender 창은 수정하지 않았습니다. 별도 백그라운드 Blender로 제작했습니다.

## 재생성

Blender 백그라운드에서 `build_model.py`를 실행한 뒤 번들 Python으로 `finalize_glb.py`를 실행합니다. 마지막으로 Blender에서 `verify_import.py`를 실행합니다. 생성 스크립트만 다시 실행하면 GLB 애니메이션이 여러 클립으로 나뉘므로 정리 단계를 함께 실행해야 합니다.

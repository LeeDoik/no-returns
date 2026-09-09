# 재채기 상자 — 입체 버전 v0.2

[English](README.en.md)

상태: 검토용 3D 모델. 게임 적용·충돌·바람 이펙트·최종 최적화는 미완료입니다.

[Blender 원본](sneezer.blend)과 [애니메이션 GLB](sneezer.glb)를 제공합니다. 원본의 타임라인 1은 평상시, 30은 재채기 예고, 48은 분출, 90은 복귀입니다. GLB 애니메이션은 `SneezeCycle`입니다.

v0.1의 각진 스타일과 골판지 텍스처를 유지하면서 눈·눈썹·앞니에 얕은 돌출, 입에 약 10.5cm 깊이, 뚜껑에 8mm 두께를 추가했습니다. 예고 상태에만 각진 볼이 약 4.4cm 돌출됩니다. 볼은 상태 전환으로 나타나며 연속적으로 부푸는 변형은 아닙니다. 모든 표정 상태를 합친 모델은 1339삼각형입니다. 얼굴은 텍스처가 아니라 얇은 입체 픽셀 면입니다.

세 상태의 실제 렌더: [평상시](previews/idle.png), [예고](previews/warning.png), [재채기](previews/sneeze.png). 내보낸 GLB를 재가져와 표정의 단독 표시와 뚜껑 회전을 검사했습니다. [검사 결과](import-validation.json), [GLB 구조](glb-validation.json)를 보관합니다. 게임 안에서의 표현과 성능은 아직 검증하지 않았습니다.

이전 모델은 `../model-v01`에 보존했습니다. 다른 세션의 게임 파일이나 열려 있는 Blender 장면은 수정하지 않았습니다. 재생성 순서는 Blender의 `build_model.py`, Python의 `finalize_glb.py`, Blender의 `verify_import.py`입니다. 생성은 v0.1의 Blender 원본에 의존합니다.

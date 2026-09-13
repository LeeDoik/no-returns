# FLATBED 3D 제작과 검토

[English](ship-production.en.md)

2026-09-14 · **첫 통합 모델 / 품질 검토 후보.** 승인된 A를 Tripo에서 생성하고 Blender에서 객실과 출입 기구를 조립했다. 현재 Unity 플레이 장면과 게임 코드는 변경하지 않았다. 출시용 최종 승인이나 4인 플레이 검증을 뜻하지 않는다.

## 결과물

- [통합 Blender 원본](../../art/ship-flatbed-01/Flatbed_Integrated.blend)
- [통합 GLB](../../art/ship-flatbed-01/Flatbed_Integrated.glb) · [통합 FBX](../../art/ship-flatbed-01/Flatbed_Integrated.fbx)
- [Tripo 원본 FBX](../../art/ship-flatbed-01/source/NR_Flatbed_Tripo_Source.fbx)
- [외부 렌더](../../art/ship-flatbed-01/review/integrated-front.png) · [후방 열린 상태](../../art/ship-flatbed-01/review/integrated-rear.png) · [내부 렌더](../../art/ship-flatbed-01/review/integrated-interior.png)
- [메시 검사](../../art/ship-flatbed-01/integrated-validation.json) · [재가져오기 검사](../../art/ship-flatbed-01/roundtrip-validation.json)

## 제작과 실제 수정

1. Blender에서 외부·내부 공통 좌표의 구조 모델을 만들었다.
2. 4면 시트를 Tripo 단일 이미지 입력에 넣은 첫 결과는 네 대가 합쳐져 탈락했다. 같은 A를 단일 투시도로 정리해 다시 생성했다. 입력 준비는 내장 이미지 생성 도구를 사용했다.
3. Tripo Smart Mesh와 텍스처를 생성하고 FBX를 다운로드했다. 원본은 보존한다. 생성 ID는 c1864948-b96d-483d-b31d-c00dfca0bba9다. 이번 Tripo 비용은 55 + 65 + 20 = 140크레딧, 잔액은 화면 확인 시 750이었다.
4. 자동 Boolean 절단은 원하지 않는 막힌 면을 만들어 폐기했다. 평면 분할로 내부와 후방 출입구를 정리하고 원래 창 면에 유리 재질을 배정했다.
5. 실내 폭 6.4m는 선체를 뚫고 나왔다. 통합 모델에서는 0.68배인 4.352m로 줄였다. 기존 47.04㎡ 면적과 6.4m 실내 배치 계산을 이 모델에 적용하지 않는다.
6. 3m 램프와 높이 2.4m 문을 별개로 만들었다. 문은 양옆으로, 램프는 바닥 아래로 수납하는 키프레임 초안이다. Blender 프레임 1은 닫힘, 40은 열림이다. FBX는 열린 정적 상태, GLB는 애니메이션 포함이다.
7. 좌석 4개를 양옆에 분산했다. 기존 승인 Tripo 수납장 4개를 재사용했다. 내부 콘솔과 일부 벽 마감은 단순 제작 메시이므로 외부와 동일 수준으로 완성됐다고 평가하지 않는다.

## 검토와 남은 사항

외부·후방·내부를 같은 모델에서 렌더하고, 원본 중복 생성과 외장 침범·막힌 Boolean 면을 직접 확인해 수정했다. UV 누락과 비정상 좌표를 검사했다. 통로 검사와 파일 재가져오기 결과는 연결된 JSON에 기록한다. 자동 검사로 실제 운반 조작이나 재미를 검증하지 않는다.

- [x] Tripo 단일 선체 생성·다운로드 및 원본 보존.
- [x] Blender 객실·문·램프 통합과 반복 렌더 검토.
- [ ] 창 전체의 투과 범위, 운반 시 시야, 마감 경계의 최종 사용자 품질 승인.
- [ ] 문·램프 전체 이동 구간의 간섭과 바닥 접점 검증.
- [ ] 내부 마감·콘솔 화면·장비 소켓의 출시 수준 완성.
- [ ] Unity 4인 운반·충돌·성능·재질 동등성 검증.

## 재제작

[build.py](../../art/ship-flatbed-01/build.py)는 초기 치수 구조를, [integrate.py](../../art/ship-flatbed-01/integrate.py)는 현재 통합 변경을 만든다. Blender 5.2.1에서 순서대로 실행한다. [verify.py](../../art/ship-flatbed-01/verify.py)는 통합 파일을 재가져와 검사한다. 게임에 넣을 때는 GLB 또는 FBX를 이용하고 문·램프를 독립 오브젝트로 유지한다. 이번 작업은 플레이 장면 적용을 완료하지 않았다.

[정합성 검토 S01~S08](ship-review.ko.md) · [이전 내부 제안](ship-interior.ko.md).


### 창턱 높이 수정

실제 외부 창턱이 높아 기존 단층 평면을 수정했다. 조종 구역 바닥은 0.6m에서 1.2m로 높이고, 높이 0.2m씩 3개 계단을 배치한다. 화물 구역 바닥은 0.6m다. 조종 구역 시점은 바닥 1.2m + 눈높이 1.57m = 2.77m로 렌더한다. 기존 전 구역 단층·동일 바닥 제안은 이 제작 후보에서 대체한다. 계단 운반감은 Unity에서 검증해야 한다.


최종 출력 전 [finish.py](../../art/ship-flatbed-01/finish.py)를 실행해 공유 512px 내부 도장 재질을 베이크하고 기준 인체를 내보내기 대상에서 제거한다. 재현 순서는 build → integrate → finish → verify다. 내부 도장은 Blender 절차적 재질이며 외부 이미지를 편집한 결과가 아니다.


최종 자동 검사: GLB 재가져오기 전후 48,906삼각형 일치, UV 누락 0, 비정상 좌표 0, 화물 구역 중앙 통로 표본 6개 모두 통과. 점·선 표본 검사이므로 직원·화물의 전체 부피 충돌을 보장하지 않는다. 한영 문서 링크·체크 상태·수치 대응 검사 통과.

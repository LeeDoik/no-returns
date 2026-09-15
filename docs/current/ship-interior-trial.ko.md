# FLATBED 내부 구조 시험 04

[English](ship-interior-trial.en.md)

2026-09-15 · 사용자가 내부 돌출과 낮은 출입구를 지적해 구조를 다시 맞췄다. 별도 시험 장면·Windows 빌드에 적용했으며 본 게임과 이전 아트 원본은 보존했다. [이전 상태](../archive/ship-interior-trial-03.ko.md).

## 원인과 수정

시험 03은 객실 천장만 충분히 높고 후방 문은 약 1.86m로 남아 있었다. 불규칙한 생성 외장을 늘려도 직사각형 객실을 포함한다는 보장이 없었다. 과거 중앙 통과선 검사만으로 외장 일치와 여유로운 출입까지 확인했다고 볼 수 없다.

새 파이프라인에 맞춰 이번 시험의 시각 외장은 객실에 맞는 단순 구조로 교체했다. 기존 Tripo 외장 원본은 유지하며, 최종 엔진·장식·우주선 외관을 완성했다는 의미가 아니다. 내부 가구·화면과 시점은 유지하고 외벽·지붕·전방 연결·후방 문틀을 같은 기준으로 만들었다.

- 객실 바닥 1.00m, 천장 아래 4.12m, 유효 높이 3.12m.
- 후방 출입구 유효 폭 3.2m, 높이 3.12m. 문짝도 같은 높이로 만들고 개방 위치를 옆으로 옮겼다.
- 외장 폭 5.1m. 출입구와 객실 사이의 바닥·천장 연결을 채웠다. 외부 발판은 수평 길이 3m·높이 1m다.
- 직원 높이 1.8m·반지름 0.34m, 점프 초기 속도 5m/s·중력 18m/s²는 유지했다.

## 결과물과 재현

[Blender](../../art/ship-flatbed-01/interior-blockout-04/Flatbed_InteriorTrial.blend) · [FBX](../../art/ship-flatbed-01/interior-blockout-04/Flatbed_InteriorTrial.fbx) · [GLB](../../art/ship-flatbed-01/interior-blockout-04/Flatbed_InteriorTrial.glb)

[내부 전방](../../art/ship-flatbed-01/interior-blockout-04/forward.png) · [내부 후방](../../art/ship-flatbed-01/interior-blockout-04/rear.png) · [외부 후방](../../art/ship-flatbed-01/interior-blockout-04/exterior-rear.png) · [외부 전방](../../art/ship-flatbed-01/interior-blockout-04/exterior-front.png)

[구조 생성·치수 검사](../../art/ship-flatbed-01/interior-blockout.py) · [구조 근거](../../art/ship-flatbed-01/interior-blockout-04/validation.json) · [재가져오기](../../art/ship-flatbed-01/interior-blockout-04/roundtrip.json) · [Unity 검사·빌드](../../NoReturns/Assets/_NoReturns/Editor/ShipInteriorTrialBuild.cs) · [조작](../../NoReturns/Assets/_NoReturns/Runtime/ShipInteriorTrial.cs).

실행 파일: `builds/ShipInteriorTrial/NoReturns-InteriorTrial.exe`. WASD·마우스, Space 빈손 점프, E 들기, Q 놓기, F1 한영 전환, Esc 커서 해제. 같은 폴더 데이터를 유지한다. 빌드는 `powershell -NoProfile -File tools/unity.ps1 trial`로 기존 에디터에 연결해 생성한다. 이전 시험 실행 중이면 닫고 새로 켠다.

## 검증 범위

- [x] 고정 실내 부품의 정점을 새 외장 범위와 전방 경사 지붕 경계에 대조해 돌출 0개 확인. 의도적으로 외부에 있는 문짝·발판·외피는 제외한다. 모든 복잡한 메시 간 교차를 검사하는 일반 도구는 아니다.
- [x] 동일 모델의 내부·외부 렌더 검토와 GLB 재가져오기 성공: 삼각형 2,255개, UV 누락 0, 비정상 좌표 0.
- [x] 실제 CharacterController 진입 5경로, 귀환 3경로 통과.
- [x] 문 아래를 포함한 점프 12지점에서 머리 충돌 없음. 실제 상승 약 0.645m.
- [x] Unity 컴파일·임포트와 Windows 빌드 성공. 새 build-success.txt 확인.
- [ ] 사람의 공간감·출입 여유·화물 운반 재검토.
- [ ] 최종 아트·문 동작·외장 마감·본 게임·4인 온라인 통합.

로컬 근거는 `artifacts/ship-interior-trial/passage.txt`, `build-success.txt`, `envelope-blender.log`, `envelope-roundtrip.log`다. 새 구조가 편한지는 자동 검사로 확정하지 않는다. 사용자의 구조 승인을 받은 뒤 [구조 우선 파이프라인](art-structure-first.ko.md)에 따라 아트 제작을 진행한다. 이번 Tripo 생성·크레딧 사용 없음.

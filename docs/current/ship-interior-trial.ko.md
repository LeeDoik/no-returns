# FLATBED 내부 구조 시험 01

[English](ship-interior-trial.en.md)

2026-09-14 · **Blender 제작·구조 표본 검사 완료 / Unity 검증 차단.** 최종 아트가 아닌 공간 시험이다. 기존 외장과 본 게임 장면·빌드는 보존했다.

## 작업 계획과 결과

1. 기존 Angular 선체와 런타임 직원·화물 크기를 확인했다.
2. 새 후보에 평평한 바닥, 중앙 화면 콘솔, 보조 콘솔, 좌석 4개, 랙과 거치대 자리, 후방 문·발판을 만들었다.
3. 동일 모델의 전방·후방·창문·후방 외관을 렌더하고 표본 통과 및 GLB 재가져오기를 검사했다.
4. 별도 Unity 시험 장면 생성·출입 검사·빌드 코드를 준비했지만 라이선스 검증에서 실행이 중단됐다. 코드 컴파일·장면 생성·실행 파일 생성은 완료되지 않았다.

## 결과물

- [Blender 원본](../../art/ship-flatbed-01/interior-blockout/Flatbed_InteriorTrial.blend) · [FBX](../../art/ship-flatbed-01/interior-blockout/Flatbed_InteriorTrial.fbx) · [GLB](../../art/ship-flatbed-01/interior-blockout/Flatbed_InteriorTrial.glb)
- [내부 전방](../../art/ship-flatbed-01/interior-blockout/forward.png) · [내부 후방](../../art/ship-flatbed-01/interior-blockout/rear.png) · [창문 시점](../../art/ship-flatbed-01/interior-blockout/window.png) · [후방 외관](../../art/ship-flatbed-01/interior-blockout/exterior-rear.png)
- [제작 스크립트](../../art/ship-flatbed-01/interior-blockout.py) · [독립 재가져오기 검사](../../art/ship-flatbed-01/verify-interior-blockout.py) · [구조 검사 결과](../../art/ship-flatbed-01/interior-blockout/validation.json) · [재가져오기 결과](../../art/ship-flatbed-01/interior-blockout/roundtrip.json)

## 치수 결정

기존 제작용 천장 아래 높이는 2.75m였지만 실제 선체 중앙의 표본 지붕은 약 3.30~3.48m에 있었다. [측정 스크립트](../../art/ship-flatbed-01/probe-interior.py)와 [객체 범위](../../art/ship-flatbed-01/interior-probe.json)를 참고했다. 이 여유를 사용해 외장을 변형하지 않고 새 내부 천장 아래를 3.12m, 객실 전체 바닥을 1.00m로 정했다. 기존 낮은 화물 바닥에 단순히 계단만 제거한 것이 아니다. 객실 높이는 2.12m이고 실제 창문 앞 시점은 2.57m다. 모두 이번 후보의 시험값이며 출시 치수 확정이 아니다.

[현재 운반 코드](../../NoReturns/Assets/_NoReturns/Runtime/CarryLab/CarryRoom.cs)의 직원 높이 1.8m·반지름 0.34m·눈높이 1.57m, 화물 0.8×0.65×0.65m를 검사 기준으로 사용했다. 후방 문틀 아래 높이는 바닥 기준 약 1.86m로 객실보다 낮다. 발판은 외부 수평 길이 3m에 높이 1m를 연결한다. 기존 문·발판 애니메이션을 재사용한 최종 장치가 아니라 열린 상태의 별도 구조 후보다.

## 확인한 것과 남은 문제

- 외장 정점·면·재질 인덱스의 전후 지문이 일치한다. 기존 Angular/Integrated 파일을 덮어쓰지 않았다.
- 중앙과 좌우 통과선에서 직원 캡슐을 구 표본으로 검사했으며 충돌 표본은 없었다. 화물을 감싸는 보수적인 구의 중앙 통과 표본도 충돌이 없었다. 바닥 지지·미끄러짐·실제 회전 화물·연속적인 모든 위치를 보장하는 검사는 아니다.
- 조종석의 좌우 수평 시선은 외장 불투명 면에 막히지 않았다. 중앙 시선은 기존 창틀에 막힌다. 렌더에서도 중앙 창틀이 보이며 이 문제를 해결 완료로 표시하지 않는다.
- GLB 재가져오기 전후 12,711삼각형 일치, UV 누락 0, 비정상 좌표 0. 화면 면과 외부 발판의 독립 객체를 확인했다.
- 벽·좌석·랙은 단색 구조 부품이다. 승인 시안의 마감·손잡이·배선·텍스처 밀도까지 구현한 최종 아트가 아니다. 화면 글자는 렌더용 예시이며 동적 게임 UI가 아니다.

## Unity 준비와 차단 상태

[시험 장면 생성·빌드 코드](../../NoReturns/Assets/_NoReturns/Editor/ShipInteriorTrialBuild.cs)와 [시험 조작 코드](../../NoReturns/Assets/_NoReturns/Runtime/ShipInteriorTrial.cs)를 추가했다. 전용 FBX와 안정적인 .meta 식별자도 보관했다. 생성 예정 장면은 `Assets/_NoReturns/Scenes/ShipInteriorTrial.unity`, 출력 예정 실행 파일은 `builds/ShipInteriorTrial/NoReturns-InteriorTrial.exe`다. **두 결과물은 아직 생성되지 않았다.** 기존 CarryRoom과 본 게임 빌드 설정은 변경하지 않았다.

시험 조작은 WASD·마우스, E 들기, Q 놓기, F1 한영 전환, Esc 커서 해제를 의도한다. 실제 운반 코드를 대체하지 않는 독립 시험 조작이며 온라인 시험은 포함하지 않는다. 자동 검사는 실제 CharacterController의 3개 통과선을 이동할 계획이다. Unity 코드와 재질 변환·충돌·조작은 아직 컴파일 및 실행 검증 전이다.

실패 근거: Unity 실행은 종료 코드 198과 `No valid Unity Editor license found`로 임포트 전 중단됐다. 사용자 환경 재시도는 자동 승인 검토가 “라이선스 해결 근거 없이 같은 빌드 재시도”라는 이유로 거절했다. 이후 읽기 전용 진단에서 사용자 로그인은 존재했지만 세션은 stale, 라이선스 조회는 클라이언트 연결 실패였다. 우회 실행은 하지 않았다. Unity Hub에서 활성 라이선스와 에디터 정상 실행을 확인한 뒤 아래 명령을 재개한다.

~~~powershell
unity run NoReturns --timeout 600 -- -executeMethod NoReturns.Editor.ShipInteriorTrialBuild.Build -logFile "$env:TEMP/nr-interior-unity.log"
~~~

- [x] 별도 구조 모델과 동일 모델 양방향 렌더.
- [x] 구조 표본·파일 재가져오기 검사.
- [ ] Unity 컴파일·임포트·실제 충돌 출입 검사.
- [ ] 실행 파일 생성 및 사람이 조작해 운반감 확인.
- [ ] 창틀 중앙 시야와 낮은 후방 입구의 사용자 품질 검토.
- [ ] 최종 아트 마감·문 애니메이션·동적 화면·4인 온라인 통합.

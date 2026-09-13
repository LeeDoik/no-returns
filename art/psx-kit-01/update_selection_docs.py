from pathlib import Path
import shutil,json
root=Path('art/psx-kit-01');dest=Path('NoReturns/Assets/_NoReturns/Art/PSXKit01/Selected');dest.mkdir(parents=True,exist_ok=True)
for p in (root/'selected').iterdir():
 if p.suffix in ('.fbx','.png') and p.name!='selected-review.png':shutil.copy2(p,dest/p.name)
ko='''## 2026-09-13 — 현재 상태: 18개 생성 종료, 6개 선별

사용자 결정으로 크레딧 소진 계획을 취소하고 Smart Mesh 후보 18개에서 종료했다. 승인된 시설 이미지 6종에서 각각 A/B/C를 생성했다. 업로드 권한 문제는 사용자 설정 후 해결됐다. 시작 2715, 사용 1400, Tripo 화면에서 확인한 잔액 1315이며 추가 생성하지 않는다. 처음 2개는 생성 무료·텍스처 20, 나머지 16개는 생성 65·텍스처 20이었다. 아래 과거 업로드 차단·2715 잔액은 당시 기록이다.

| 시설 | 선별 | 시각 검토 이유 |
|---|---|---|
| Wall | B | A의 위쪽 돌출보다 정돈된 상단과 선명한 띠 |
| Corner | C | 안쪽 수직 프레임이 뚜렷한 연결부 |
| Door | B | A의 뒤쪽 돌출보다 정돈된 상단과 열린 문틀 |
| Floor | B | 상대적으로 단순한 판 분할과 외곽 |
| Lamp | C | 단순한 외곽과 읽기 쉬운 발광 창 |
| Rack | A | 열린 화물 변형 없이 닫힌 상자로 일관성 유지 |

선별은 Blender 비교 렌더에 대한 작업자 판단이며 사용자 최종 외형 승인은 아니다. 선반의 상자는 장식으로 합쳐져 있다. 18개 ZIP 압축 검사와 FBX 메시·UV 검사를 통과했다. 너무 이른 내보내기로 UV가 없던 파일은 텍스처 완료 후 다시 받아 교체했다. 원본 및 미선별 후보는 보존했다.

선별 6종은 바닥 중앙 피벗, 노멀 정리, 512×512 Base Color와 최근접 표시로 준비했다. 느슨한 요소 검사에서 제거된 요소는 0이다. 원본 텍스처는 보존한다. 크기는 검토 제안값으로 벽·바닥 폭 3m, 코너 높이 3m, 문틀 높이 3.2m, 조명 폭 1.2m, 선반 높이 2.4m다. 비례를 유지했으므로 반복 연결 규격은 아직 맞추지 않았다. 원본 Quad 면 수를 삼각형 수로 오인하지 않으며 실제 삼각형 수·전체 치수는 selection.json에 기록했다. 반복 배치용 추가 경량화는 남아 있다.

- [x] 후보 18개 다운로드·메시·UV 검사.
- [x] 선별 6종 FBX 재가져오기·삼각형 수 유지·UV·텍스처 파일 검사.
- [x] 실제 모델 비교 렌더와 선별 렌더 시각 검토.
- [ ] Unity 에디터 가져오기·재질 연결·반복 연결 치수와 문틀 통과 검증.
- [ ] 충돌체·1인칭 가독성·성능·실제 협동 플레이 검증.

선별 FBX 6개와 PNG 6개를 별도 Selected 폴더에 배치했다. Unity Software Terms 창 문제는 해결되지 않아 에디터 적용·컴파일·새 빌드를 수행하지 않았다. 기존 게임 코드·씬·충돌체·실행본 0.8.2는 유지했다. Blender 검사와 렌더는 게임 검증을 대체하지 않는다.

[생성·비용 기록](../../art/psx-kit-01/smart/generation-manifest.json) · [18개 검사](../../art/psx-kit-01/smart/validation.json) · [선별 수치](../../art/psx-kit-01/selected/selection.json) · [재가져오기 검사](../../art/psx-kit-01/selected/roundtrip-validation.json) · [선별 렌더](../../art/psx-kit-01/selected/selected-review.png) · [Unity 파일](../../NoReturns/Assets/_NoReturns/Art/PSXKit01/Selected)
'''
en='''## 2026-09-13 — Current status: generation stopped at 18, six selected

The user replaced the credit-exhaustion plan with a stop at 18 Smart Mesh candidates. Each of the six approved facility images produced A/B/C variants. Upload permission was resolved after the user changed the setting. Starting balance 2715, spent 1400, remaining balance observed in Tripo 1315; no further generation. The first two generations were free with 20 for texture; the other 16 cost 65 for generation and 20 for texture. Earlier blocked-upload and 2715-balance entries below are historical.

| Facility | Selection | Visual review reason |
|---|---|---|
| Wall | B | More orderly top than A's protrusions, readable stripe |
| Corner | C | Clear inner vertical frame at the connection |
| Door | B | More orderly top than A's rear protrusion, open doorway |
| Floor | B | Relatively simple panel divisions and outline |
| Lamp | C | Simple outline and readable light window |
| Rack | A | Consistent closed crates rather than an open-cargo variation |

Selection is an operator judgment from Blender comparison renders, not final user visual approval. Rack crates remain combined decoration. All 18 archives passed ZIP integrity and FBX mesh/UV checks. Early exports missing UVs were replaced with downloads after texturing completed. Sources and unselected candidates are preserved.

The selected six have bottom-center pivots, normalized normals, and 512×512 Base Color textures with nearest sampling. Loose-element checks removed 0 elements. Original textures remain preserved. Review-scale proposals are wall/floor width 3m, corner height 3m, doorway height 3.2m, lamp width 1.2m, and rack height 2.4m. Proportions were preserved, so modular connection dimensions are not yet fitted. Source Quad polygon counts are not treated as triangle counts; actual triangle counts and dimensions are in selection.json. Further reduction for repeated placement remains open.

- [x] Download and mesh/UV checks for 18 candidates.
- [x] FBX reimport, preserved triangle counts, UV and texture-file checks for six selections.
- [x] Visual review of actual comparison and selected-model renders.
- [ ] Unity Editor import, material setup, modular dimensions and doorway traversal checks.
- [ ] Collision, first-person readability, performance and actual cooperative play checks.

Six selected FBXs and six PNGs were staged in a separate Selected folder. The Unity Software Terms window issue remains unresolved, so Editor integration, compilation and a new build were not performed. Existing code, scenes, collisions and executable 0.8.2 remain unchanged. Blender checks and renders do not replace game validation.

[Generation and cost record](../../art/psx-kit-01/smart/generation-manifest.json) · [18-candidate checks](../../art/psx-kit-01/smart/validation.json) · [Selection metrics](../../art/psx-kit-01/selected/selection.json) · [Reimport checks](../../art/psx-kit-01/selected/roundtrip-validation.json) · [Selected render](../../art/psx-kit-01/selected/selected-review.png) · [Unity files](../../NoReturns/Assets/_NoReturns/Art/PSXKit01/Selected)
'''
for lang,body in [('ko',ko),('en',en)]:
 p=Path(f'docs/current/psx-tripo-kit-01.{lang}.md');s=p.read_text(encoding='utf-8');pos=s.index('\n## ');p.write_text(s[:pos]+'\n'+body+'\n## Historical production records\n'+s[pos:],encoding='utf-8')
 short=('SPACE-ART-25 현재: Smart Mesh 후보 18개 생성 종료, 6개 선별·512 텍스처·피벗 정리·Blender 재가져오기 통과. 잔액 1315 보존. 업로드 문제 해결. Unity 약관 창 문제로 에디터 적용·반복 연결·충돌·성능·사람 플레이 검증은 미완료. 기존 0.8.2 유지.' if lang=='ko' else 'SPACE-ART-25 current: Smart Mesh generation stopped at 18; six selected with 512 textures, pivots and passing Blender reimports. Preserve remaining 1315 credits. Upload resolved. Unity terms window still blocks Editor integration; modular fit, collisions, performance and human play remain unverified. Existing 0.8.2 unchanged.')
 for stem in ['01-overview','03-guides','04-backlog','05-validation']:
  p=Path(f'docs/current/{stem}.{lang}.md');s=p.read_text(encoding='utf-8');paras=s.split('\n\n');paras=[x for x in paras if not (('SPACE-ART-25' in x or 'Tripo record' in x or 'Tripo PSX kit' in x) and 'psx-tripo-kit-01.' in x and not x.lstrip().startswith('|'))];s='\n\n'.join(paras);p.write_text(s+'\n\n'+short+f' [Record](psx-tripo-kit-01.{lang}.md).\n',encoding='utf-8')
 p=Path(f'docs/archive/change-log.{lang}.md');p.write_text(p.read_text(encoding='utf-8')+'\n\n## 2026-09-13 — SPACE-ART-25 Smart Mesh selection\n\n'+short+f' [Evidence](../current/psx-tripo-kit-01.{lang}.md).\n',encoding='utf-8')

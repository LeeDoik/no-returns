# 저장소 정리와 파일 보존

[English](repo-hygiene.en.md)

2026-10-02 · 게임 0.9.3 / 프로토콜 12 유지. [파일별 정리 기록](../validation/repo-cleanup-2026-10-02.json).

## 이번 정리

- Unity의 `Assets/_NoReturns/Art/PSXKit01/Review` 24개 모델/텍스처와 해당 .meta/폴더 .meta, Smart Wall 후보 중복본 2개: 총 51개 파일·44,900,041바이트 제거. Unity AssetDatabase로 전체 Assets의 직접 의존성을 검사해 Review 참조가 없음을 확인하고 native API로 제거했다. 모든 바이너리는 [prepared](../../art/psx-kit-01/prepared) 또는 [NR_Wall_Smart_A](../../art/psx-kit-01/smart/NR_Wall_Smart_A)에 바이트가 같은 원본을 보존한다.
- 종료된 시험의 낡은 run 폴더 30개, 65,781,676바이트 Editor 로그, Python 캐시 제거: 로컬 생성 파일 총 115,418,532바이트. 최신 검증된 리스너·배송·이동 실행 기록 3개, [공유 검증 기록](../validation), [검토 이미지](../../art/cinder-kit-01), 현재 실행본은 보존했다. 과거 JSON의 run 경로는 실행 당시 출처이며 영구 파일 보관 약속이 아니다. 아래 새 정책이 이전 변경 기록의 로컬 실패 run 보관 설명을 대체한다.
- 부분 클론의 Git blob 누락으로 LFS prune 사전 검사가 실패했다. 원격에서 일반 Git 객체를 다시 받아 연결 검사 후 재시도했다. LFS prune은 246개 캐시 객체·도구 표시 약 122MB를 제거했고 원격 확인 7개를 보고했다. LFS 원본·커밋 이력은 유지했다.
- Finder `.DS_Store`를 [Git 제외 규칙](../../.gitignore)에 추가했다. 선내 재질의 기존 수정 8개는 내용 해시를 확인해 보존하고 이번 커밋에서 제외한다.

## 계속 유지할 기준

제작 모델·텍스처·Blender 파일·제작 스크립트·Unity .meta·라이선스·과거 문서는 소스다. 파일이 크거나 현재 Cinder에서 사용하지 않는다는 이유만으로 삭제하지 않는다. 선택본의 제작 폴더와 Unity 가져오기 폴더도 목적이 다르므로 유지한다.

`artifacts/`, `builds/`, Unity `Library/Temp/Logs/UserSettings`, Python 캐시는 재생성 가능하며 Git에서 제외한다. 시험 결과 중 재검토에 필요한 JSON은 `docs/validation/`, 이미지는 `art/`에 보존한 뒤 낡은 로컬 run을 정리한다. 진행 중인 Editor 캐시와 실행 중인 플레이어의 파일은 먼저 사용 상태를 확인한다. 무차별 `git clean`, 이력 재작성, 원격 LFS 원본 삭제는 하지 않는다.

새 클론의 현재 파일은 줄지만 GitHub의 과거 LFS 저장 용량은 일반 삭제 커밋으로 줄지 않는다. 이번 작업은 현재 트리와 로컬 캐시 정리이며 과거 제작 이력을 지우지 않는다.

과거 로컬 근거의 끊긴 링크 142개는 원래 경로와 이름을 유지한 출처 표기로 바꿨다. 파일이 현재 미보관임을 한영 문서에서 명시하고 기존 검증 결과·결정·완료 상태는 유지한다. [변환 기록](../validation/repo-doc-links-2026-10-02.json). 문서 링크/언어·체크 상태 검사 272개 문서 통과.

검증 상태와 미확인 범위는 [검증 체크리스트](05-validation.ko.md)와 [변경 기록](../archive/change-log.ko.md)을 따른다. 다음 구현은 [미완료 목록](04-backlog.ko.md)의 Cinder 억제/외곽 연결이다.

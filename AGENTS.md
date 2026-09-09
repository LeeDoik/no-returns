# 문서 작성 규칙 / Documentation rules

## 한국어

- 사용자에게는 한국어로 설명하며 문서의 기본 진입점도 한국어로 둔다.
- 기획서, 작업 계획, 가이드, 체크리스트, 상점 소개 등 사람이 읽는 문서는 한국어와 영어를 항상 함께 작성하고 같은 작업에서 함께 수정한다.
- 글로벌 출시용 게임 문구의 원문은 영어로 유지한다. 한국어 문서는 전체 내용을 검토할 수 있는 대응본으로 만들며 요약본으로 대체하지 않는다.
- 각 문서 상단에 상대 언어 링크를 둔다. 로컬 링크는 가급적 같은 언어 문서로 연결한다.
- README.md는 한국어, README.en.md는 영어 안내로 유지한다. 기존 영어 경로는 보존하고 한국어본에 .ko.md를 붙인다. 새 문서는 기본적으로 .en.md와 .ko.md를 짝으로 만든다.
- 버전, 결정, 수치, 출처, 체크박스 상태, 확정·제안·미완료 구분을 두 언어에서 일치시킨다. 배포용 영문 문구는 필요하면 한국어본에서도 원문을 함께 표시한다.
- 코드, 기계용 설정, 외부 라이선스 원문은 무조건 복제하지 않는다. 이 AGENTS.md는 한 파일에 두 언어를 제공한다.

## English

- Communicate in Korean and make Korean the default documentation entry point.
- Always create and update complete Korean and English versions of human-readable specifications, plans, guides, checklists, store copy, and similar documents in the same change.
- Keep English as the source for global-release game copy. The Korean counterpart must support full review; a summary is insufficient.
- Link to the language counterpart at the top. Prefer same-language local links.
- Keep README.md in Korean and README.en.md in English. Preserve existing English paths and append .ko.md for Korean counterparts. Default to paired .en.md and .ko.md names for new documents.
- Align versions, decisions, numbers, sources, checkbox states, and confirmed/proposed/incomplete distinctions. Retain English production strings alongside Korean explanations when useful.
- Do not automatically duplicate code, machine-readable configuration, or third-party license originals. This AGENTS.md contains both languages.

## 모든 게임 작업의 문서 갱신 / Documentation for every game task

### 한국어

- 앞으로 이 프로젝트의 구현·수정·실험·되돌리기·조사·검토 작업에는 문서 갱신을 포함한다. 별도 요청을 기다리지 않는다.
- 시작 시 `docs/README.md`와 관련 `docs/current/` 문서를 읽고 현재 규칙·범위·미완료 상태를 확인한다. 오래된 계획보다 현재 코드와 최신 명세를 확인한다.
- 변경한 기능은 같은 작업에서 해당 현재 명세, 제작 가이드, 미완료·버그 목록, 테스트·출시 체크리스트의 관련 부분을 한국어와 영어로 함께 갱신한다. 관련 없는 문서를 형식적으로 다시 쓰지 않는다.
- 모든 게임 작업은 `docs/archive/change-log.ko.md`와 `.en.md`에 날짜, 이유, 실제 변경, 검증 근거, 미확인 사항을 기록한다. 조사만 했다면 코드 변경 없음과 결론을 기록한다.
- 되돌리기 때는 취소된 기능을 현재 명세에서 제거하고 상태·가이드·체크리스트를 함께 복원한다. 변경 이력은 삭제하지 않는다.
- 수치·버전은 코드/씬/설정의 근거 경로를 연결한다. 제안, 구현, 검증 완료, 사용자 품질 의견, 미재현 오류를 구분한다. 문서 정리만으로 과거 미완료를 완료 처리하지 않는다.
- 과거 문서 경로는 보존하고 `docs/archive/` 목록에서 찾게 한다. 최신 명세를 찾기 위해 버전별 기록을 역추적하게 하지 않는다.
- 완료 전 링크, 언어 대응, 숫자·버전·체크 상태를 확인한다. 게임 동작을 바꾸면 적절한 게임 검사를 하고, 문서만 바꾸면 문서 검사를 한다. 최종 답변에 중요한 문서 위치와 실제 검증 범위를 남긴다.

### English

- Include documentation in every implementation, fix, experiment, revert, investigation and review for this game project; do not wait for a separate request.
- At task start, read `docs/README.en.md` and relevant `docs/current/` documents for current rules, scope and outstanding work. Check current code/specifications rather than relying on old plans.
- Update affected current specifications, production guides, backlog and validation/release checklist in Korean and English in the same task. Do not rewrite unrelated documents merely for formality.
- Log every game task in both `docs/archive/change-log.ko.md` and `.en.md`, including date, reason, actual changes, validation evidence and unknowns. Investigation-only tasks record no code changes and their conclusions.
- On reversion, remove cancelled features from the current specification and restore affected statuses, guides and checklists. Preserve historical records.
- Link values/versions to their code, scene or settings sources. Distinguish proposals, implementation, verified work, user quality feedback and unreproduced issues. Documentation cleanup alone must not close outstanding work.
- Preserve historical paths and make them discoverable through `docs/archive/`. Readers must not reconstruct current specifications by tracing version histories.
- Before completion, check links, language counterparts, numbers, versions and checkbox states. Behavior changes require appropriate game checks; documentation-only changes require document checks. Report important document locations and actual validation scope in the final response.

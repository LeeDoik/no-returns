# HyperFrames 작업 / HyperFrames work

## 한국어

- 상위 AGENTS.md의 한영 문서·Git 규칙을 따른다. [환경 안내](../../docs/current/hyperframes.ko.md).
- 영상 작업 전 `/hyperframes`와 필요한 도메인 스킬을 읽는다. HTML 수정 전 `/hyperframes-core`를 읽는다.
- `npm ci`로 GSAP를 설치한다. CLI 버전은 package.json에 고정한다.
- `npm run check`로 검사한다. `npm run dev`는 지속 미리보기, `npm run stop`은 종료다.
- 실제 영상은 미리보기 검토 후 렌더링한다. 환경 설치 검증용 기본 샘플은 콘텐츠 승인 대상이 아니다.
- 렌더·캐시·node_modules는 커밋하지 않는다. 생성물로 실제 게임 구현을 주장하지 않는다.

## English

- Follow the parent AGENTS.md bilingual documentation and Git rules. [Setup guide](../../docs/current/hyperframes.en.md).
- Read `/hyperframes` and relevant domain skills before video work; read `/hyperframes-core` before HTML edits.
- Install GSAP with `npm ci`. Keep the CLI pinned in package.json.
- Validate with `npm run check`. `npm run dev` starts persistent preview; `npm run stop` stops it.
- Render actual videos after preview review. The default environment smoke sample is not a content approval deliverable.
- Do not commit renders, caches or node_modules. Generated visuals do not prove gameplay implementation.

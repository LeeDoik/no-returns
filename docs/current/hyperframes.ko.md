# HyperFrames 영상 제작 환경

[English](hyperframes.en.md)

2026-10-03 · 환경 설정·시험 렌더 검증 완료. 실제 트레일러 제작은 미완료.

## 2026-10-03 — 24초 게임 소개 영상

현재 [영상 소스](../../video/hyperframes/index.html)는 2초 설치 시험을 대신하는 24초 소개 영상이다. 영어 제목·한글 설명·자체 산업풍 리듬을 사용하며 배송 → 소음/유인 → 억제장치 → 귀환 메시지를 5개 장면으로 구성했다. [한영 대본·범위](../../video/hyperframes/BRIEF.ko.md), [디자인](../../video/hyperframes/DESIGN.ko.md), [장면 구성](../../video/hyperframes/STORYBOARD.ko.md), [검증 기록](../validation/hyperframes-intro-01.json).

`npm run render -- --quality looks --fps 30 --workers 1 --output renders/no-returns-intro.mp4`로 출력한다. 로컬 결과는 `video/hyperframes/renders/no-returns-intro.mp4`다. 이전 설치 시험은 커밋 `0f8bb4e`에 보존한다. 아래 2초 수치는 설치 시점 기록이며 현재 소스의 길이가 아니다.

- [x] check 오류·경고 0, layout 9개 표본 문제 0, 대비 34/34 통과. 5개 장면 캡처와 실제 출력 5프레임 모음을 시각 확인.
- [x] H.264 1920×1080, 30fps, 720프레임, 24초, 6,091,537바이트. AAC 48kHz 2채널, 평균 -24.2dB·최대 -5.7dB. hardware GPU/drawelement, 렌더 14.8초.
- [x] 원본 게임 스크린샷 3장과 OFL 글꼴·자체 음원/재생성 스크립트·출처를 보존. 대형 바이너리는 LFS, 렌더/진단 결과는 커밋 제외.
- [ ] 사용자 영상·청취 품질 승인, 실제 연속 플레이 녹화와 기존 75초 트레일러는 미완료. 음원은 수치/포맷 검사 범위이며 사람 전 구간 청취 검증은 아니다.

초기 상대 경로와 제목 줄 간격 검사는 수정 후 통과했다. 애니메이션 맵 보조 의존성을 임시 캐시에 준비해 25/27개 tween을 확인했다. 느린 진행선·초기 제목 읽기 정지와 등장 중 충돌 휴리스틱은 장면 확인 및 최종 check와 대조했다. Noto Sans KR 9.9MB는 인라인 상한 2MB를 넘으므로 편집 프로젝트 이동 시 assets/fonts를 함께 보존한다. 로컬 MP4 출력은 정상이다. 게임 코드·씬 변경/Unity 빌드는 없다.

## 설치와 소스

- [공식 HyperFrames](https://github.com/heygen-com/hyperframes)의 CLI 0.8.112를 [package.json](../../video/hyperframes/package.json)에 고정했다. 기존 Node.js 26.9.0, npm 11.19.1을 사용한다. 새 환경은 Node.js 22 이상과 FFmpeg가 필요하다.
- Homebrew로 FFmpeg/FFprobe 9.0.2를 설치했다. 렌더러가 Chrome Headless Shell 152.0.7977.30을 로컬 캐시에 내려받았다.
- Codex 사용자 스킬 디렉터리에 hyperframes, hyperframes-core, hyperframes-cli, hyperframes-animation, hyperframes-audio, hyperframes-creative, hyperframes-registry, hyperframes-studio, hyperframes-keyframes 9개를 설치했다. 다음 대화 턴부터 사용 가능하며 다른 컴퓨터에는 자동 복제되지 않는다. 재설치는 공식 `npx hyperframes skills update`를 사용한다.
- [영상 소스](../../video/hyperframes/index.html)는 1920×1080, 2초의 `HyperFrames Ready` 등장 시험이다. GSAP 3.14.2는 [잠금 파일](../../video/hyperframes/package-lock.json)로 고정하고 로컬 경로에서 읽는다. 원격 글꼴을 사용하지 않는다. CLI 최초 다운로드·npm ci·새 브라우저 설치에는 네트워크가 필요하다.

## 실행

저장소 루트에서 아래 명령을 사용한다. `dev`는 명령 종료 뒤에도 유지되는 Studio를 시작하며 `stop`으로 종료한다. 실제 영상은 Studio에서 내용 검토 후 렌더한다.

```sh
cd video/hyperframes
npm ci
npm run dev
npm run check
npm run render -- --quality draft --fps 30 --workers 1 --output renders/setup-test.mp4
npm run stop
```

[현재 Studio](http://localhost:3002/#project/hyperframes). 포트가 사용 중이면 실행 로그의 실제 주소를 따른다. 출력은 `video/hyperframes/renders/`에 저장한다. 렌더·스냅샷·캐시·node_modules는 Git에서 제외하고 HTML·설정·의존성 잠금 파일만 관리한다. 시험 출력은 재생성 가능하며 저장소에는 포함하지 않는다.

## 검증과 남은 범위

- [x] `npm run check`: lint/runtime/motion 오류·경고 0, layout 9개 표본 문제 0, 대비 5/5 통과.
- [x] 로컬 MP4 렌더 및 ffprobe: H.264, 1920×1080, 30fps, 60프레임, 2초, 65,335바이트. hardware GPU/drawelement 경로, 렌더 약 4.8초(초기 브라우저 다운로드 제외).
- [x] MP4의 1초 프레임을 추출해 문구·중앙 배치를 시각 확인. Studio 지속 실행 상태와 HTTP 200 확인.
- [ ] 음성 인식 whisper-cpp, 로컬 TTS Kokoro, 음악 MusicGen은 미설치·미검증. 필요할 때 설치한다. Docker는 설치되어 있으나 실행하지 않았으며 로컬 렌더에는 불필요하다. 따라서 doctor의 선택 기능 경고는 남아 있다.
- [ ] 실제 게임 촬영·내레이션·음악·한영 자막·75초 트레일러 품질은 미검증. [트레일러 기획](trailer-recruitment.ko.md)의 미완료 상태를 유지한다.

초기 기본 템플릿은 정지 타임라인 때문에 `sweep_static` 검사에 실패했다. 0.6초 등장 효과와 2초 길이로 시험을 구성한 뒤 재검사에 통과했다. 저장소 루트에서 잘못 실행한 `npm run check`는 스크립트 없음으로 실패했으며 위 영상 폴더에서 다시 실행해 통과했다. 게임 코드·씬·버전은 변경하지 않았고 Unity 실행/빌드는 수행하지 않았다.

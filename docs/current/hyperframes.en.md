# HyperFrames video production environment

[한국어](hyperframes.ko.md)

2026-10-03 · Setup and smoke render verified. Actual trailer production remains incomplete.

## 2026-10-03 — 24-second game introduction

The current [composition source](../../video/hyperframes/index.html) replaces the 2-second installation smoke sample with a 24-second introduction. Five scenes use English headlines, Korean explanations and an original industrial pulse score: delivery → noise/distraction → suppression → return. [Bilingual script/scope](../../video/hyperframes/BRIEF.md), [design](../../video/hyperframes/DESIGN.md), [storyboard](../../video/hyperframes/STORYBOARD.md), [validation](../validation/hyperframes-intro-01.json).

Render with `npm run render -- --quality looks --fps 30 --workers 1 --output renders/no-returns-intro.mp4`. The local result is `video/hyperframes/renders/no-returns-intro.mp4`. The installation sample remains in commit `0f8bb4e`. The 2-second figures below describe historical setup, not the current source duration.

- [x] Zero check errors/warnings, zero layout issues across 9 samples, contrast 34/34. Visually inspected five scene captures and a contact sheet of five actual encoded frames.
- [x] H.264 1920×1080, 30fps, 720 frames, 24 seconds, 6,091,537 bytes. AAC 48kHz stereo, mean -24.2dB / peak -5.7dB. Hardware GPU/drawelement, 14.8-second render.
- [x] Preserve three original game stills, OFL fonts, original audio/regeneration script and provenance. Large binaries use LFS; render/diagnostic output is excluded from commits.
- [ ] User visual/listening approval, continuous gameplay capture and the earlier 75-second trailer remain incomplete. Audio was checked numerically/by format, not through human full-length listening.

Initial parent-relative asset paths and headline leading failed checks and passed after correction. Temporary cached helpers enabled an animation map of 25/27 tweens. Slow progress lines, the opening reading hold and entrance collision heuristics were compared with scene inspection and the final check. Noto Sans KR at 9.9MB exceeds the 2MB inline ceiling, so keep assets/fonts when moving the editable project. Local MP4 output is valid. No gameplay code/scene changes or Unity build.

## Installation and sources

- Pin the [official HyperFrames](https://github.com/heygen-com/hyperframes) CLI 0.8.112 in [package.json](../../video/hyperframes/package.json). Use existing Node.js 26.9.0 and npm 11.19.1. A new environment needs Node.js 22 or later and FFmpeg.
- Installed FFmpeg/FFprobe 9.0.2 with Homebrew. The renderer downloaded Chrome Headless Shell 152.0.7977.30 into its local cache.
- Installed nine skills into the Codex user skills directory: hyperframes, hyperframes-core, hyperframes-cli, hyperframes-animation, hyperframes-audio, hyperframes-creative, hyperframes-registry, hyperframes-studio, hyperframes-keyframes. Available from the next conversation turn; not automatically copied to other machines. Reinstall using the official `npx hyperframes skills update` command.
- The [composition source](../../video/hyperframes/index.html) is a 1920×1080, 2-second `HyperFrames Ready` entrance test. GSAP 3.14.2 is pinned in the [lockfile](../../video/hyperframes/package-lock.json) and loaded locally. No remote fonts. Initial CLI download, npm ci and a new browser installation require network access.

## Run

Use these commands from the repository root. `dev` starts Studio persistently beyond the command lifetime; `stop` ends it. Review actual video content in Studio before rendering.

```sh
cd video/hyperframes
npm ci
npm run dev
npm run check
npm run render -- --quality draft --fps 30 --workers 1 --output renders/setup-test.mp4
npm run stop
```

[Current Studio](http://localhost:3002/#project/hyperframes). If the port is occupied, follow the actual URL printed at startup. Outputs go to `video/hyperframes/renders/`. Exclude renders, snapshots, caches and node_modules from Git; track HTML, configuration and dependency locks. The smoke output is reproducible and is not included in the repository.

## Validation and remaining scope

- [x] `npm run check`: zero lint/runtime/motion errors or warnings, zero layout issues across 9 samples, contrast 5/5 passed.
- [x] Local MP4 render and ffprobe: H.264, 1920×1080, 30fps, 60 frames, 2 seconds, 65,335 bytes. Hardware GPU/drawelement path, about 4.8 seconds rendering (excluding initial browser download).
- [x] Extracted the MP4 frame at 1 second and visually checked text/centering. Verified persistent Studio status and HTTP 200.
- [ ] whisper-cpp transcription, local Kokoro TTS and MusicGen music remain uninstalled/unverified. Install when needed. Docker is installed but not running and is unnecessary for local rendering. Optional-feature doctor warnings therefore remain.
- [ ] Actual gameplay capture, narration, music, bilingual captions and 75-second trailer quality remain unverified. Preserve outstanding items in the [trailer proposal](trailer-recruitment.en.md).

The original blank template failed `sweep_static` because its timeline was static. Added a 0.6-second entrance and a 2-second test duration; the rerun passed. An accidental `npm run check` from the repository root failed with a missing script; rerunning in the video directory above passed. No gameplay code, scenes or game versions changed; no Unity run/build was performed.

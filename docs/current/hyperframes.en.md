# HyperFrames video production environment

[한국어](hyperframes.ko.md)

2026-10-03 · Setup and smoke render verified. Actual trailer production remains incomplete.

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

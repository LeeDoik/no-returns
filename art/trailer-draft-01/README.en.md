# Trailer draft 01
[한국어](README.ko.md)

2026-09-17 · TRAILER-DRAFT-01 · Mood/edit review only; not gameplay.

[Korean-subtitled video](NO_RETURNS_Trailer_Draft_01_KO.mp4) · [Contact sheet](contact-sheet.jpg) · [Korean subtitles](captions.ko.srt) · [English subtitles](captions.en.srt)

75 seconds, 1280×720, 24fps. Two six-second shots generated through Higgsfield Kling 3.0 are combined with camera moves over existing concepts and Higgsedit titles/cuts. Temporary English narration uses Windows Microsoft Zira; Korean captions are burned into the picture. Audio combines an original synthesized temporary motif, drone and confirmation chime with generated clip ambience. Final casting/music and actual gameplay capture are not included.

## Structure and sources

- 00–03 recruitment title, 03–08 exterior concept, 08–13 essential-supplies company card.
- 13–18 arrival concept, 18–24 generated delivery movement, 24–28 first-person carry concept.
- 28–33 stable suppression concept, 33–39 generated outer-creature approach.
- 39–45 colleague waiting for a Listener, 45–48 assistance, 48–51 extraction concepts.
- 51–58 acceptance/receipt, 58–62 recent-use clue, 62–67 aboard-ship concepts.
- 67–69 blackout/repeated chime, 69–75 title/tagline.

Generated video totals 12 seconds. The proposed Earth archive film is replaced by an existing exterior concept and company cards. Cooperative distraction and rescue are represented by still concepts and do not demonstrate implemented interactions. Older ship and terminal concepts do not exactly match current Unity models. Generated worker/cargo continuity deviates and requires revision before release use.

[Provenance](provenance.json) records source paths, models, job identifiers and timeline ranges. No footage or music from the reference YouTube video was used. Original concept files were not modified.

## Re-editing

1. make-temp-voice.ps1 regenerates temporary English speech. The recorded WAVs are also retained.
2. Run prepare-media.py with Python 3 and FFmpeg to recreate editorial crops and camera-move clips from original concepts.
3. In an environment with Higgsedit, set NR_TRAILER_MEDIA to this folder's absolute path and run higgsedit build edit.jsx. The picture master is picture.mp4. Production used hosted Higgsedit; installation from the public npm package was not available.
4. python finish.py combines temporary audio and Korean captions into the final MP4. narration.json is the bilingual dialogue/timing source.

Exported video, original generated clips and temporary voices use Git LFS. Intermediate crops can be reconstructed from sources/scripts. Automatic approval rejected uploading an entire source/script ZIP; that upload was not performed. Only the resulting video was exported from the cloud.

## Review scope

Check duration, frames, audio track, complete decoding and peak level. Inspect one-second samples of generated clips and the final contact sheet. Human full playback/listening, mood evaluation and publication rights review remain separate. See the [current validation record](../../docs/current/trailer-draft.en.md).

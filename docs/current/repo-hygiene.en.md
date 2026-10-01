# Repository cleanup and file retention

[한국어](repo-hygiene.ko.md)

2026-10-02 · Retain game 0.9.3 / protocol 12. [Per-file cleanup record](../validation/repo-cleanup-2026-10-02.json).

## This cleanup

- Remove 24 models/textures in Unity `Assets/_NoReturns/Art/PSXKit01/Review`, their .meta files/folder .meta, and 2 duplicate Smart Wall candidate files: 51 files / 44,900,041 bytes total. Check direct dependencies throughout Assets using Unity AssetDatabase, confirming no Review references, then remove via native API. Preserve byte-identical originals for every binary in [prepared](../../art/psx-kit-01/prepared) or [NR_Wall_Smart_A](../../art/psx-kit-01/smart/NR_Wall_Smart_A).
- Remove 30 stale run folders from stopped tests, a 65,781,676-byte Editor log and Python caches: 115,418,532 bytes of local generated files total. Preserve the latest 3 validated Listener/delivery/movement runs, [shared validation records](../validation), [review images](../../art/cinder-kit-01) and current player. Historical JSON run paths identify the original execution location, not permanent file retention. The new policy below supersedes earlier change-log descriptions of retaining local failed runs.
- LFS prune preflight failed on a missing Git blob in the partial clone. Re-fetch ordinary Git objects from origin, check connectivity and retry. LFS prune removed 246 cached objects / approximately 122MB as reported by the tool, reporting 7 remote verifications. Retain LFS sources and commit history.
- Add Finder `.DS_Store` to [Git exclusions](../../.gitignore). Preserve the 8 pre-existing ship material edits by checking content hashes and exclude them from this commit.

## Continuing policy

Production models, textures, Blender files, production scripts, Unity .meta, licenses and historical documents are sources. Do not delete them merely because they are large or not currently used by Cinder. Selection production folders and Unity import folders also serve different purposes and remain.

`artifacts/`, `builds/`, Unity `Library/Temp/Logs/UserSettings` and Python caches are reproducible and Git-excluded. Preserve JSON needed for future review in `docs/validation/` and images in `art/` before clearing stale local runs. Check usage before removing active Editor caches or running-player files. Do not use indiscriminate `git clean`, rewrite history or delete remote LFS sources.

A fresh clone has fewer current files, but an ordinary deletion commit does not reduce historical GitHub LFS storage. This task cleans the current tree and local cache without erasing production history.

Replace 142 broken historical local-evidence hyperlinks with provenance notation retaining original paths and labels. Explicitly mark files as no longer retained in both languages; preserve historical test outcomes, decisions and completion states. [Conversion record](../validation/repo-doc-links-2026-10-02.json). All 272 documents pass link/language/checkbox checks.

Follow the [validation checklist](05-validation.en.md) and [change log](../archive/change-log.en.md) for verification and unknowns. The next implementation is Cinder suppression/outer integration in the [backlog](04-backlog.en.md).

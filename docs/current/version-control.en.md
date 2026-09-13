# Git version control

[ko](version-control.ko.md)

Preserve the current 0.8.14 Unity project, art sources, tools and bilingual documents in Git. Record deletion of the old Godot/temporary projects while preserving historical commits. After appropriate checks and documentation updates, commit completed work and normally push to origin. Exclude builds, artifacts test output, Unity caches, personal paths, credentials and Blender automatic backups. Unity builds and verification tools can be rerun from source. The remote is a private GitHub repository; access expansion or making it public requires a separate request. The baseline commit is an actual snapshot of accumulated uncommitted work; do not fabricate retrospective per-task commits.

GitHub remote: https://github.com/LeeDoik/no-returns (private). Models, Blender sources, textures, audio, video and archive assets use Git LFS according to .gitattributes. Install Git LFS and run git lfs pull after cloning. Historical ordinary Git binaries were not rewritten, preserving history. Credential-pattern scans found 0 matches across 783 historical text objects and current candidate files.

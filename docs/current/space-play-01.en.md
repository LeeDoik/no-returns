# SPACE-PLAY-01 — Two-player carrying implementation plan

[한국어](space-play-01.ko.md)

2026-09-12. Approved scope: first-person movement, cargo pickup/set-down, collisions and a two-player direct-connect Windows build. This is a controls experiment before the complete delivery demo. Preserve Bootstrap and art concepts.

- [x] Create CarryRoom scene and placeholder geometry, employees and cargo; not final art production.
- [x] Implement host-authoritative movement, cargo ownership/collision, client input and replicated state.
- [x] Validate connection, movement, pickup contention, rotation, drop, walls and departure across two processes.
- [x] Produce Windows build, launch instructions and updated bilingual current docs/evidence.

Separate transport, input/simulation and presentation under Runtime/CarryLab. Editor/CarryBuild.cs creates scene/build. tools/test_carry_build.py launches real executables, drives the normal input path through test-only input files and compares state results. TCP direct connection is experimental; Steam invitations, NAT relay, latency compensation and host migration are excluded. Distinguish automated operation from human feel evaluation. Equipment, creatures, settlement and rescue are outside this stage.

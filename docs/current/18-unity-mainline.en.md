# NO RETURNS Unity main development status

[한국어](18-unity-mainline.ko.md)

> **Historical record · retired/superseded 2026-09-12.** The [PSX space-delivery design](01-overview.en.md) is current. Implementation, launch, art and values below are historical, not current status. Deleted files remain path records only.


[Current Manyfast concept](19-no-returns-manyfast.en.md) — NO RETURNS expedition details and Unity implementation boundaries.

2026-09-12 · User decision: develop NO RETURNS in Unity as the main project. SIDE EFFECTS is discontinued because the user found it unfun. This is user quality feedback; automated checks do not override it or justify retaining a completion claim.

## Current baseline and launch

- Main project: `unity/NoReturns`, Unity 6000.6.0f1.
- Default scene: CarryLab (`../../unity/NoReturns/Assets/_Project/Scenes/CarryLab.unity`; retired file). The baseline lab provides movement, camera, physical carrying and throwing. The separate NR-LOOP-01 scene adds the small delivery experiment.
- 05_Unity.cmd (`../../05_Unity.cmd`; retired file): open Unity Editor. 06_Unity_Play.cmd (`../../06_Unity_Play.cmd`; retired file): existing NO RETURNS Unity player.
- [Current controls](15-unity-controls.en.md), [environment and MCP](14-unity-environment.en.md).
- Preserve the Godot source, art and delivery design. This does not mean all Godot delivery, networking and maps have been ported to Unity.

## Cleanup scope

Removed the SIDE EFFECTS Assets folder/meta, Windows build, test-output folder, launch entry and dedicated development scripts from active paths. Removed the unused direct com.unity.transport dependency through Unity package management. The future NO RETURNS networking approach remains a separate decision. Retained Unity CLI, Pipeline MCP, shared TMP and CarryLab.

The retired experiment archive (`../archive/side-effects-2026-09-12.zip`; retired file) contains source, scenes, tools, build, test evidence and original documents. Verified hashes for 343 files and ZIP integrity. Earlier document paths and change history remain archival material rather than launch instructions. Restoration is a separate task.

## Next work and validation

Future implementation will connect delivery objectives, cargo traits and cooperation to Unity incrementally, using NO RETURNS carrying feel as the baseline. This cleanup adds no delivery features and ports no SIDE EFFECTS mechanics.

Post-cleanup Unity batch compilation and all 24 CarryLab regression checks passed with exit code 0. Validated 48 documents and Korean/English numeric parity. Test log (`../../artifacts/unity/test.log`; retired file), cleanup inventory (`../../artifacts/unity/side-effects-cleanup.json`; retired file). Human control feel, delivery fun and online release remain unverified.

## NR-LOOP-01 — 2026-09-12

[Small slapstick delivery loop handoff](21-slapstick-loop-handoff.en.md). The separate delivery experiment is implemented. See [launch and validation](22-slapstick-loop.en.md) for actual evidence and remaining human checks.


[NR-LOOP-01 launch and validation](22-slapstick-loop.en.md)

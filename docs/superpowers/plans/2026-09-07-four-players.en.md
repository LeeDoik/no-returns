# Four-player prototype 0.4 plan

[한국어](2026-09-07-four-players.ko.md)

> Documentation update: versions and values here belong to this file's original context. Check the [documentation home](../../README.en.md) first for current rules and outstanding work.

Continue the approved concept after the three core cargo types. Use the existing runtime and ENet. No Steam, new cargo, purchased assets or final balancing in this step.

- Support 2–4 workers per hosted room, plus unchanged solo practice. Host alone cannot start. Keep three cargo slots, five deliveries and 180 seconds for controlled comparison.
- Assign stable free slots 1–4, distinct colors/numbers and non-overlapping spawn points. Rejoining uses a vacant slot without renumbering remaining workers. Replicate host-assigned slots.
- A guest departure cancels the current shift and returns everyone to waiting; the host may restart with 2–4 remaining workers. No host migration or mid-shift joining. A full room rejects further connections through ENet's capacity limit.
- Compact worker pose arrays retain the 1280-byte state budget with four workers and three crates.
- Add PLAY_FOUR.cmd for four local development windows. Preserve existing launchers and two-player checks.

## Work

- [x] Add failing tests for 2/3/4 start rules, unique slots/spawns, vacant-slot reuse and packet budget; implement session/main/worker/UI changes.
- [x] Add a bounded real four-process test for shared roster/slots, ownership, attachment/Sneezer results, delivery and departure/restart. An independent test implementer may use subagent-driven-development while root integrates code. Review via requesting-code-review.
- [x] Run full regression suite and editor import; inspect actual Korean/English menu and four-worker captures.
- [x] Write complete paired 0.4 guides and update README/Steam current-state references. Keep Internet/Steam and human fun/performance claims unverified.

Completion: full runner and editor import returned 0. Four clients agreed on ownership, attachment, two-worker sneeze hits and delivery; three-worker restart passed. Max recorded state: 1036 bytes. Korean/English renderer captures were inspected. See the [0.4 guide](../../prototype/04-four-players.en.md) for limits.

# SPACE-PLAY-06 — Site clues and shared field log

[한국어](space-play-06.ko.md)

2026-09-13 · 0.7.0 implementation and proposed test design. The mystery's identity and ending remain undecided.

## Play and rules

- Place 2 clues at the east entrance maintenance terminal and west reception recorder in listener mode. Recent use in a closed facility and a receipt predating arrival create questions. These are temporary props, not approved final art.
- Press I to inspect the aimed prop within 2.4m. Check the first collision along the actual view ray, rejecting distance, walls, facing away and downed employees. The second record is locked before delivery.
- Delivery changes the recorder's lamp and ejects paper. The host confirms one employee's discovery and shares it with the partner. Reinspection never adds pay or accumulates duplicate state. Clues are optional for delivery.
- Tab opens the shared field log; Tab, Esc or the close button returns to play. Reading blocks that employee's movement/carry actions while the online world and creature continue. Advise brief inspection in the field and reading aboard.
- Records belong to the current shift. Keep them during report and next-shift preparation, then clear on next arrival. A permanent codex restored after application exit is out of scope. Preserve existing wallet/license/delivery-unlock saves.
- Provide English source copy and Korean counterparts throughout. The panel/props test readability; suppression, deadlines and a new complete map are not added here.

## Acceptance criteria

In two real processes, test remote/facing-away/pre-delivery rejection, client inspection and host record agreement, post-delivery recorder change and second discovery, no duplicate pay, local input blocking with world progress while reading, independent language toggling, post-return reading and next-arrival reset. Automated state checks are separate from human curiosity, fear and text-understanding evaluation.

## Locations and implementation sources

The first terminal is centered at (7.7, 1.35, 0), the second recorder at (-7.8, 1.35, 10.6). After arrival, find the first in the right-hand entrance corridor; after delivery, inspect the recorder by the west wall behind the gold reception bench. Preserve the bench and existing carrying route.

[CarryClues](../../NoReturns/Assets/_NoReturns/Runtime/CarryLab/CarryClues.cs) handles temporary props, view rays, receipt gates and clue bits; [CarryRoom](../../NoReturns/Assets/_NoReturns/Runtime/CarryLab/CarryRoom.cs) handles input, journal and replication; [CarryLanguage](../../NoReturns/Assets/_NoReturns/Runtime/CarryLab/CarryLanguage.cs) supplies bilingual copy. Protocol is 5 and both players use the [0.7.0 build](../../NoReturns/Assets/_NoReturns/Editor/CarryBuild.cs). Save format remains 1 and does not store clue bits.

## Current validation

- Passed 11 [Unity physics/rule checks](../../artifacts/space-play-06/rules-result.json): distance, facing away, wall occlusion, inspection phase, pre-receipt lock, duplicates and reset. Separately confirmed pre-delivery rejection with the ray actually hitting the recorder.
- Passed 23 [two-process checks](../../artifacts/space-play-06/latest.json): client discovery sharing, journal input blocking with world progress, independent language rendering, post-receipt discovery, unchanged 420 CR pay, return reading and next-arrival reset. Capture-file creation is separate from pixel validation.
- Hidden-window full captures were black. A separate visible window produced the [actual Korean journal screen](../../artifacts/space-play-06-ui/run-20260913-020950/host/screen.png); visually checked title, body, wrapping, close and language buttons. This shows the first clue and does not approve all resolutions or the full two-clue layout.
- The first executable check failed because the journal-close request was overwritten by movement input. Fixed the runner to await the closed state without weakening game controls. Preserved the failed evidence.
- Human curiosity, fear, text understanding, feeling exposed while reading in the field and final-art quality are unverified. Suppression and time pressure remain subsequent work.

Use [listener launch](../../06_Play_Listener_Test.cmd) and [same-PC join](../../07_Join_Local_Listener.cmd). Runners are [test_clues_build.py](../../tools/test_clues_build.py) and [check_clue_rules.py](../../tools/check_clue_rules.py).

Existing [rescue regression checks](../../artifacts/space-play-03/latest.json) also passed 21.

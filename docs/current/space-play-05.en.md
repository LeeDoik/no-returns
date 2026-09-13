# SPACE-PLAY-05 — Host progression save

[한국어](space-play-05.ko.md)

2026-09-13 · 0.6.0 implementation. Preserve reinvested pay across restarts in the existing listener experiment. Full campaigns, mid-shift resume and Steam Cloud are out of scope.

## Rules and acceptance criteria

- Save the shared wallet, beacon license and successful-delivery count on the host PC. Automatically save when purchases, receipts or returns change these values. Clients use the host's progression without importing it into their own files.
- Creating a room again starts at ship preparation. Do not restore the selected contract, cargo position, down state or remaining beacon charges. Retain secured receipt pay without adding a return bonus for an unfinished shift. Purchased beacons still refill to 2 shared uses on the next arrival.
- Store progression-v1.json under Unity's persistentDataPath. Automated players use a separate file under each test-dir to avoid touching real-player saves.
- Write and flush a temporary file before replacing the existing file, retaining the previous valid file as .bak. Validate format version 1 and checksum to detect corruption and unsupported versions. Explain that backup recovery can lose the most recent changes. If neither file is readable, preserve the originals and report a room-creation error.
- Display save failures and retry. Do not guarantee lossless recovery from forced termination before a completed save or disk failure. The checksum does not prevent deliberate tampering.
- Memory-only progression from 0.5.0 cannot be recovered retroactively after exit. New saves begin with this build. Carrying and basic delivery experiments retain their session rules.

## Validation plan

Deliver, return and purchase in two real processes, exit, then restart using the same host save. Check wallet/license/risk unlock, no duplicate return bonus, no client save creation and 2 charges on a new arrival. File checks cover round trips, corruption, backups, version rejection and write failure. Separate automated checks from human menu-readability and control-feel evaluation.

## Implementation sources and current validation

[CarrySave](../../NoReturns/Assets/_NoReturns/Runtime/CarryLab/CarrySave.cs) handles saving, backups and format validation; [CarryMission](../../NoReturns/Assets/_NoReturns/Runtime/CarryLab/CarryMission.cs) restores progression only; [CarryRoom](../../NoReturns/Assets/_NoReturns/Runtime/CarryLab/CarryRoom.cs) integrates host loading, autosave and error notices. The [build](../../NoReturns/Assets/_NoReturns/Editor/CarryBuild.cs) is 0.6.0; protocol remains 4.

- [Save-file checks](../../artifacts/space-play-05/rules-result.json): 14 passed through Unity MCP, covering round trips, secured pay, unfinished-shift reset, ignored temporary files, backup recovery/read-back, checksum/future-version rejection and write failure.
- [Executable checks](../../artifacts/space-play-05/latest.json): 35 passed in two Windows processes. Following delivery, purchase and attraction, verified termination/restart, restored 300 CR wallet/license/unlock, client replication, 2 charges on arrival, no duplicate bonus and backup recovery from corrupted primary data. No client save file was created.
- Existing economy rules also passed 17 again. Used isolated test folders instead of actual user saves. Found an omitted `InvalidDataException` in recovery handling, handled it explicitly and reran checks.
- Human save-notice readability/control feel, OS power loss, other PCs, Steam Cloud and 4 players are unverified. A save failure only preserves the previous successful checkpoint; importing the old 0.5.0 memory state is not supported.

Use existing [listener launch](../../06_Play_Listener_Test.cmd) and [same-PC join](../../07_Join_Local_Listener.cmd). File checks use [check_save_rules.py](../../tools/check_save_rules.py); actual players use `python tools/test_progression_build.py --save-restart`.

Existing [rescue regression checks](../../artifacts/space-play-03/latest.json) also passed 21. Documentation links/counterparts passed across 192 documents.

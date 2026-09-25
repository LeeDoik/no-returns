# Remaining work — 2026-09-25

[한국어](remaining-work.ko.md)

Current code baseline: `e27b9a3`, CINDER-DENSE-02. This task reviewed documents, source and existing evidence; it changed no game code and reran no gameplay tests. Historical pending lists are not simply accumulated.

## Confirmed baseline

108×86.4m, nine accessible buildings and three A/B/C listeners. Delivery→receipt collection→all aboard→settlement→beacon purchase→next shift is connected. Existing evidence records 32 full-cycle checks in four processes and warning/down/rescue checks in two processes. Only the host traversed the route in the four-process test; clients checked replication aboard. This does not validate four humans having fun together.

Evidence: [current map](cinder-density.en.md), [play rules](cinder-demo.en.md), [validation](05-validation.en.md), local `artifacts/cinder-dense/summary.json`. Networking currently uses direct TCP, not Steam matchmaking.

## Recommended order — still open

| Order | Remaining work | Next acceptance criterion |
|---|---|---|
| 1 | Two-human first-visit play, patrol intersections, sound detection, pursuit release and detour risk tuning | Record whether distraction actually helps a teammate deliver, walking-only stretches and unfair surprise attacks. Preserve map size |
| 2 | Carrying and readability polish | Natural loaded turns at jambs, columns and corners. Reproduce/fix the reported baton obstruction of the receiver screen. Signs explain destination and return routes |
| 3 | Suppression failure, outer pursuit and post-rescue escape | Verify current 360/480/600-second stages and 608-second intrusion in real time online. Check regional light/sound cues, complete pursuit routes and recovery opportunities |
| 4 | Representative PSX area, animation and audio | Finish one representative area while preserving validated structure. Replace placeholder employee/arms, listener/outer creature presentation and warning sounds. Approve images before 3D production. Preserve the exterior-production hold |
| 5 | Repeat play, progression and mystery | Decide the scope and purpose of purchases/upgrades beyond the beacon and risk contract. Connect the first delivery clue to further curiosity. Historical economy equipment proposals are not implemented features |
| 6 | Other-PC co-op, persistence and reconnect | Use protocol 12 for 2–4 humans acting simultaneously; verify latency, disconnect/rejoin rules and post-return quit/reload saves. Resolve the current disconnect-aborts-shift limitation and production networking/storage architecture |
| 7 | Steam distribution and release polish | After authorization to resume registration, prepare app/test access, implement invite/join, and review install/update, performance, settings, real gameplay footage and distribution rights |

Steam enrollment remains paused by the user. Account status and policy were not freshly checked and registration/payment are not resumed. The September 18 portable package (protocol 11) is incompatible with the current build (12); prepare a fresh package before the next external test.

## Recommended immediate action

Before expanding content, have two humans play the current map once and record encounters, successful distractions, parcel snags, navigation confusion, suppression-cue recognition and completion time. Tune items 1–3 together from those observations, then expand representative-area art. This is a recommended sequence, not authorization or a completion claim for new features.

[Backlog](04-backlog.en.md) · [Art candidates](demo-art-list.en.md) · [Steam hold](steam-testing.en.md)

Git inspection found nine pre-existing material edits and a recovery scene. They are excluded from this investigation commit.

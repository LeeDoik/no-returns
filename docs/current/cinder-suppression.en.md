# Cinder suppression and outer creature — 0.9.4

[한국어](cinder-suppression.ko.md)

2026-10-02 · Implemented; pass 60 actual four-process checks. [Current validation record](../validation/cinder-suppression-0.9.4.json). Supersede 0.9.3's unconnected-Cinder-suppression/outer statements within this scope. Retain delivery, Listener, baton and rescue controls in the [Listener guide](cinder-listener.en.md) and [controls guide](controls-ui.en.md). Final outer art, human quality approval and progression saving are outside this task.

## Gameplay rules

Connect one Listener and one outer creature to default Cinder play. The host advances time during field phases (delivering/received). Waiting aboard or opening menus does not stop time. Report/preparation freezes it; the next arrival resets it. Retain existing test thresholds **90/135/180 seconds** for steady → unstable → failing → off. The outer creature starts moving **8 seconds** after shutdown, **188 seconds** after arrival. These are not final release difficulty values.

Display the 4 existing suppressor signals and field boundary as green → pulsing orange → pulsing red → dark/off. Scale the 40 existing work lights relative to their original intensities by **1 / 1.15÷1.5 / 0.8÷1.5 / 0.45÷1.5**. At shutdown reduce existing ambient light to 0.65 times its original value; restore original brightness during report. Do not change saved sky, fog or sun brightness/direction. Use state text and existing temporary signal audio without a numerical remaining-time HUD.

From the failing stage the outer creature appears at **(29,0,18)m** beyond the east boundary; after grace it pursues crew through the existing east opening. The Listener remains active. The outer creature detects distant/occluded crew but routes around walls and attacks only after distance, line of sight and a **0.9-second warning**. Exclude down/aboard crew from pursuit. Retain **2.2m/s patrol, 3.3m/s pursuit and 0.8-second target refresh**; the baton cannot stun it.

Retain purchased beacon rules: **120 CR license, 2 signals per shift, 8 seconds each and 12m attraction**. Ordinary Listener attraction uses noise investigation. For **1.2 seconds** after the last valid signal, the outer creature follows the signal position instead of crew. Resume crew pursuit after signaling ends. Signals do not cancel warning/recovery actions. Placement aboard does not consume a charge.

Both creatures share the four crew members' down state and post-rescue protection. Outer attacks also release cargo and use existing hold-E rescue/all-down emergency recovery. Preserve **300+120=420 CR** normal receipt return, **300 CR** after beacon purchase, and existing-wallet retention/no new delivery pay on failure. Reset parcel, crew, creatures and beacon charges for the next shift. Cinder wallet/license remain session-only.

## Running and production

[07_Play_Cinder_4P.command](../../07_Play_Cinder_4P.command) or `python3 tools/cinder_four_player.py start`. Mac version **0.9.4**, common protocol **13**, Cinder TCP **27842**. Connect matching players. Keep `--map-only` movement testing and `--delivery-only` peaceful delivery regression. [Stop tool](../../08_Stop_Cinder_4P.command).

Preserve source art scene, models, textures, collision and routes; use the existing derived-scene/build path. [CarrySuppression](../../NoReturns/Assets/_NoReturns/Runtime/CarryLab/CarrySuppression.cs) uses runtime copies of current signal materials and original work-light intensities. Do not overwrite saved shared materials. Cinder still does not instantiate old clues/progression saves.

The Cinder Listener grid in [CarryThreat](../../NoReturns/Assets/_NoReturns/Runtime/CarryLab/CarryThreat.cs) retains **x=-27…25, z=-34…30m**. Extend the outer grid to **x=-53…53, z=-42…42m** inside current 108×86.4m ground/fall guards so escaping onto exterior ground does not prevent pursuit. Reuse the 1m static floor/obstacle grid, 0.32m step limit and ship ramp/deck exclusion. Background rocks remain decorative; add no new collision. Dynamic terrain, moving cargo/crew avoidance and final outer model/animation are outside scope.

## Verification and remaining work

[Native stage/light/path checks](../../tools/unity_checks/CinderSuppressionCheck.cs) confirm 9 time-boundary samples, 5 signals/boundary elements, 40 work lights, 7 outer routes, 1,600 collision steps, 8,056 grid nodes and preserved sky/fog/sun. These rule checks explicitly set time/positions; they are not human play or real elapsed-time checks.

[Actual four-player checks](../../tools/test_cinder_suppression.py) use ordinary movement, parcel controls, purchasing, beacon placement and real time. Verify hazardous delivery/settlement, Listener attraction, stage agreement, advance cues/grace, outer attraction/resumed pursuit, shared down/ship safety, emergency recovery and next shift. Do not inject teleports, downs, credits or time. Pass 60 actual checks; Mac build has 0 errors/7 existing warnings. [Korean failing screen](../../art/cinder-kit-01/suppression-failing-ko.png) · [English shutdown screen](../../art/cinder-kit-01/suppression-off-en.png). Inspect 800×500 warning text and retain 2 LFS PNGs. Also pass 31 existing Listener/rescue, 48 peaceful delivery/UI and 13 movement/network regression checks: 152 actual four-process checks total. All 274 documents pass links/language/checkbox checks; this change passes whitespace checks excluding preserved existing material edits. Keep human quality and other-environment validation separate.

Four-human warning interpretation, carrying/evasion/rescue, fun, appearance/audio/dim-floor readability, other PC/LAN, Windows and performance/long-term stability require separate review. Automated passes do not constitute approval of these qualities.

Launch four manual windows from the final player without test-input folders and verify 3 host TCP connections. Leave them running for user review. Immediately after automated shutdown, the first launch was rejected by an in-use port probe; retry after a brief wait succeeded. This does not verify human input/quality.

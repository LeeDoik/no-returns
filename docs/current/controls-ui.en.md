# Controls, ship and purchase UI — 0.9.4

[한국어](controls-ui.ko.md)

## Current implementation — personal slots and medicine 0.9.25

Implemented 1 co-op/2 solo slots, pre-departure 40 CR medicine purchase/equipping, field swapping and self/teammate full healing. New Cinder sessions start with 400 CR. Retain unused equipped doses within the session; treatment does not reset downs. Game 0.9.25 / protocol 16 / TCP 27842. [Current specification, production and validation scope](personal-equipment.en.md).

## Current implementation — multiple deliveries 0.9.24

Implemented 3–6 physical parcels by crew count, independent carrying, verification pause/resume, receipts and district bundles. Game 0.9.24 / protocol 15 / TCP 27842. This supersedes the single 420 CR delivery for normal Cinder sessions. [Current rules, production evidence and remaining work](multiple-deliveries.en.md).

## 2026-10-05 — visit ledger, revival and departure 0.9.23

Cinder receipts record 420 CR as unbanked earnings, paid only on departure. A living occupant spends 100 unbanked CR to revive one eliminated teammate after 10 seconds. Anyone can start/cancel a 12-second departure; deduct 100 CR per abandoned teammate with a zero floor. Integrate health, repeat downs/elimination and all-eliminated/liftoff during revival. [Implementation, values and limits](visit-settlement.en.md), [validation](../validation/visit-settlement-0.9.23.json). Game 0.9.23, protocol 14, TCP 27842. Supersedes historical Cinder all-aboard return, immediate settlement and one-hit down rules in this scope. New tools, multiple deliveries, Cinder saves/reconnects, final danger timing/audio and human review remain.

## 2026-10-03 — Straight baton thrust 0.9.10

Empty-hand left-click contact remains immediate; perform a fast 0.06s thrust and return by 0.30s. Retain 6s cooldown, rescue priority and carry/down blocking. The first-person tip also points forward; wall clearance changes depth only. [Current behavior and validation scope](employee-baton.en.md).

## 2026-10-03 — Baton readiness, attack and recovery 0.9.9

Empty-hand left click retains immediate contact and displays a 0.55s attack/recovery. Keep the 6s cooldown. A simultaneous valid rescue request takes priority over attacking. [Implementation, production and validation scope](employee-baton.en.md).

## 2026-10-03 — Two-handed parcel carrying 0.9.8

Default parcel reach is 0.8m; retain wheel 0.75–1.6m, right-click rotation, left-click placement and Q release. Hands align with the near face when brought close and clamp at arm length when pushed away. [Implementation, evidence and limits](employee-animation.en.md).

## 2026-10-02 — Cinder suppression and outer creature 0.9.4

[Current rules, production and running](cinder-suppression.en.md). Connect existing 90/135/180-second stages, signal audio and relative work-light dimming to 4 current suppressors and the boundary. Connect east entry, obstacle routing and pursuit for the outer creature 8 seconds after shutdown. Preserve sky/fog/sun, source art and collision. Default launch is delivery + Listener + suppression/outer; retain `--delivery-only`/`--map-only` regression. Version 0.9.4, protocol 13, TCP 27842. Pass 60 actual four-process checks for purchased-beacon distraction, hazardous return, shared down/ship safety, emergency recovery and next shift. [Validation record](../validation/cinder-suppression-0.9.4.json). Supersede older unconnected-suppression/outer statements below within this scope. Human quality, final creatures/audio, clues/progression saving and other-environment/performance checks remain pending.

## 2026-10-02 — Cinder Listener, baton and rescue 0.9.3

[Current rules, running and production checks](cinder-listener.en.md). Connect one Listener's actual floor/obstacle grid movement, noise investigation, warning/attack, existing empty-hand left-click baton, hold-E rescue and all-down ship recovery to default Cinder delivery. Preserve normal 420 CR payment and source art scene. Judge safety using current Cinder ship coordinates; do not instantiate old suppression/outer/clue/save objects. Existing beacon pulses also attract Listener investigation. Keep peaceful delivery regression via `--delivery-only` and movement tests via `--map-only`. Version 0.9.3, protocol 12, TCP 27842. Supersede older unconnected-Cinder-Listener/baton/rescue statements below only within this scope. Suppression/outer creature, clues, progression saving and human quality remain pending. [Actual validation scope](../validation/cinder-listener-0.9.3.json).

2026-10-01 · Implemented in the current Cinder Mac player. Follow the validation record below for verified scope. This supersedes earlier E automatic-departure/Esc-shop instructions.

## Default controls

| Action | Default input |
|---|---|
| Move / look | WASD / mouse |
| Pick up aimed parcel/beacon, collect receipt, inspect terminal | E |
| Rescue downed crew | Hold E beside teammate |
| Place parcel on ground / set down beacon | Left click |
| Rotate carried parcel | Hold right click + mouse |
| Parcel reach | Wheel, 0.75–1.6m |
| Release held item immediately | Q |
| Empty-hand jump / quiet walk / call | Space / Shift / C |
| Empty-hand baton / field log | Left click (including Cinder) / Tab (legacy hazard only) |
| Ship terminal | E aboard, without directly departing/returning |
| Menu/settings | Esc, fixed menu-close key |
| Reset carrying lab | R, host `--map-only` test only |

The parcel placement preview uses green/orange outlines for clear ground/blocked placement. Check the first collision within 2.8m, ground normal, parcel bounds and swept path. Walls, ceilings and obstructed spaces keep the parcel held. Rotation into walls/ground is also rejected. Q quickly releases at the current position. Place the beacon on ground ahead; active beacons cannot be collected. Right click/wheel apply only to the parcel.

Bottom prompts follow the aimed target and rebound keys. Menus/log/rebinding block movement, item, baton and rescue inputs while the world and other employees continue. Mouse sensitivity defaults to 0.12, range 0.04–0.30; FOV defaults to 80, range 65–100. Rebind 13 button actions or restore defaults in Esc settings. Reject duplicate keys; Esc cancels rebinding. Bindings, sensitivity, FOV and language persist in personal PlayerPrefs. Automated four-window sessions do not write personal preferences. The Editor persistence check temporarily saves values while preserving and restoring the original preferences.

## Ship and purchases

E aboard → select route → confirm all crew aboard → **departure button**. Deliver/collect receipt → board together → **return/settlement button** → prepare next shift. Repeated E aboard never changes mission phase. Host controls departure, return, contracts and shared purchases; the server rechecks position, occupancy, phase and wallet. Returning without a receipt displays 0 CR and needs a second click. Preserve normal 300+120=420 CR settlement. Contract changes remain available only in existing hazard mode after the first delivery, during route selection.

The same terminal's equipment card sells the **120 CR** beacon license: one shared device, **two 8-second** uses per shift. Disable purchases for insufficient funds, existing ownership, phases outside preparation/report and clients. Duplicate purchases never charge again. Cinder spawn is **(-20.7,1.23,-29)m**, above the y=1m deck. Physically carry the device into the field; placing aboard consumes no charge. Cinder connects purchases, carrying, signal/audio and Listener investigation. Baton/rescue also work in default Cinder. Suppression/outer creature and progression-save integration remain. Cinder license/wallet do not persist after session exit. Preserve existing hazard-mode host saving.

Use matching uGUI presentation for connection, Esc, settings, ship and log. Use a 1280×720 CanvasScaler, native buttons/input/slider, EventSystem and GraphicRaycaster with mouse and default arrow/Enter navigation. English is production-copy source; include complete Korean counterparts in the same change.

[Korean settings](../../art/cinder-kit-01/controls-settings-ko.png) · [Korean ship/shop](../../art/cinder-kit-01/controls-ship-shop-ko.png) · [English ship/shop](../../art/cinder-kit-01/controls-ship-shop-en.png)

## Running, sources and remaining checks

`python3 tools/cinder_four_player.py build` → `start` launches the latest four Mac windows. `stop` terminates only the owned session processes. [Four-player guide](four-player.en.md) · [Validation record](../validation/controls-ui-0.9.2.json).

[Input](../../NoReturns/Assets/_NoReturns/Runtime/CarryLab/CarryControls.cs) · [Controls/terminal/placement](../../NoReturns/Assets/_NoReturns/Runtime/CarryLab/CarryRoom.Controls.cs) · [Ledger](../../NoReturns/Assets/_NoReturns/Runtime/CarryLab/CarryMission.cs) · [Protocol 12](../../NoReturns/Assets/_NoReturns/Runtime/CarryLab/CarryWire.cs) · [Native UI](../../NoReturns/Assets/_NoReturns/Runtime/CarryLab/CrewHud.Menus.cs) · [Version settings](../../NoReturns/ProjectSettings/ProjectSettings.asset). Retain TCP 27842 and source map placement. Older protocol builds cannot connect.

Previous 0.9.2 validation: Mac build, 0 errors/7 existing warnings. Pass 48 four-process delivery/button/purchase/physical carrying checks, 17 native key/persistence/UI/target-priority/preview checks, 35 ledger checks, 10 HUD behavior/100 bilingual text-height checks. Also pass 13 final four-process movement/connection regression checks. Record 142 pre-existing missing links/0 new failures and four native scene trailing spaces separately.

Automated checks/native screen review do not replace human control preference, rotation feel, readability or fun evaluation. Four humans, other-PC/LAN, latest Windows execution, extended stability and performance remain unverified. Add no new equipment types, inventory or packages.

# CINDER delivery demo implementation plan

[한국어](cinder-demo-plan.ko.md)

2026-09-18 · CINDER-DEMO-01 · User-approved complete-cycle integration

Goal: preserve the existing blockout/interior trials and connect 1–4 player contract, carrying, delivery, receipt, return and supplies in a separate CinderDeliveryDemo scene. Preserve host authority, E/Q and Korean-default/English switching. 10–15 minutes is a human-play target, not a measured result.

- [x] Define ship spawn/cargo/reception anchors in CinderDemoLayout and adapt CarryMission/ReceiptFeedback/CarryEquipment/CarryRoom coordinates. Check both legacy reception and Cinder reception.
- [x] Copy existing blockout into a new scene through CinderDemoBuild, remove trial controls and add CarryRoom. Add creation menu and Windows launcher.
- [x] Adapt CarryThreat navigation/patrol to the map. Validate initial integration with one B-zone Listener and one outer creature; A/C additional entities/balance remain subsequent validation. Proposed suppression transitions are irregular at 360 seconds, critical at 480, offline at 600, intrusion at 608.
- [x] Validate preparation, transport, acceptance, receipt, 420CR return, 120CR purchase and next-shift reset with two real processes. Separately check solo/four crew and hazard-state consistency.
- [x] Update paired current specification, guide, backlog, validation and changelog; commit only task changes and push a dedicated branch.

Initial failing evidence: existing CarryMission does not accept a stable parcel at (32.7,.4,15). Real Unity MCP C# evaluation raised EXPECTED RED. Make that check pass after integration. Automated geometry/network checks do not establish human feel, fun or 10–15 minute duration.

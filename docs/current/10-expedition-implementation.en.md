# First playable expedition implementation plan

[한국어](10-expedition-implementation.ko.md)

> **Historical record · retired/superseded 2026-09-12.** The [PSX space-delivery design](01-overview.en.md) is current. Implementation, launch, art and values below are historical, not current status. Deleted files remain path records only.


2026-09-10

Goal: preserve existing modes and connect preparation, 3 deliveries, immediate base payouts, return and next shift on a separate fixed map.

Architecture: expedition_rules owns contracts/rewards/company storage; expedition_map owns the editable fixed layout; expedition_ui owns bilingual presentation; expedition.gd integrates workers/cargo/ENet. Separate expedition protocol and map identity protect the existing campaign.

- [x] State/storage rules and failure/duplicate payout tests.
- [x] Fixed map, receipt, truck preparation and bilingual UI.
- [x] Input/network/departure/return/repeated-shift integration tests.
- [x] Render review, existing regressions, documentation and Windows packaging.

This is EXP-01. Equipment, quality bonuses, optional jobs, random zones and new rescue remain later stages, not current features.

Evidence: expedition state/map/integration, two/four-player and menu round-trip checks, existing regression logs, actual render samples and Windows package. See [current specification](02-spec.en.md) and [play guide](../prototype/10-expedition.en.md). Human fun, external networking and EXP-02–EXP-05 remain incomplete.

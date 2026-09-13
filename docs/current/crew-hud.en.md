# Cooperative HUD — 0.9.1

[한국어](crew-hud.ko.md)

2026-09-13 · Implemented. Move gameplay-only debug text to uGUI. Preserve menus, shop and journal.

- [x] Left objective/wallet, right 4-color crew states, bottom contextual controls and indirect suppression cues; keep the central aim area clear.
- [x] Per-slot self/down/cargo/beacon/aboard/field status. Do not show distance, creature location or suppression deadline.
- [x] 1280×720 reference CanvasScaler, Korean default/English toggle; hide for menu/journal.
- [x] Actual Unity Canvas checks/captures, Windows build/4-process regression, bilingual records/Git.

Separate automated state/layout checks from human readability/fun. UI copy retains English source and complete Korean counterparts.

## Implementation and evidence

Gameplay HUD now uses a separate uGUI Canvas with Expand scaling at a 1280×720 reference. Crew numbers 01~04 accompany colors, so color is not the only identifier. The local employee is marked YOU. Right-side states read host-replicated occupancy, down, ownership and aboard checks; no position markers or distance tracking are added. Objective/wallet sit at upper left, controls/rescue progress/suppression cues at bottom and reticle at center. Removed creature AI debug labels and numeric HUD baton cooldown; retained the baton embedded display. Save notices remain in the existing Esc menu.

Unity MCP passed 10 state/language/input-blocking/visibility/scaling conditions plus height checks for two languages × 5 mission phases × 10 text elements. An overflowing English objective was found and its font size adjusted to 20. Actual Canvas renders temporarily using camera mode were reviewed at 1280×720 and 1024×768. These are isolated HUD layout evidence, not full-game screenshots or human readability assessment.

Windows 0.9.1 built successfully and 31 real local four-process equipment/connection/delivery/receipt/settlement checks passed. Those 31 checks predate only the final font-size adjustment; all Unity text checks and Windows build passed again after that adjustment. Network, reward and rescue rules are unchanged. Human readability, contrast against field backgrounds, small screens and long error-status copy need follow-up assessment.

[CrewHud.cs](../../NoReturns/Assets/_NoReturns/Runtime/CarryLab/CrewHud.cs) · [Unity checks](../../tools/unity_checks/CrewHudCheck.cs)

[JSON](../validation/crew-hud-0.9.1.json)

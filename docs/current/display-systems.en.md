# Display implementation policy and current audit

[ko](display-systems.ko.md)

0.8.14 · Current implementation audit and changes.

| Element | Current approach and decision |
|---|---|
| Recessed baton gauge | Shader draws pixel bars, color and ready check from charge 0–1. Removed creation and swapping of 13 runtime textures. |
| Reception terminal labels | Retain bilingual textures for 6 fixed states. Fonts can be checked beforehand; textures change only on state/language changes. |
| Reception terminal progress | Add a shader bar below the label texture, driven by host scan progress; hidden outside scanning. |
| Floor markings and suppression lights | Retain attached meshes and material color/emission. No complex UI rendering needed. |
| Baton electricity | Retain LineRenderer. Apply gauge drawing only in screen mode to preserve arc color. |
| Supply, return settlement, settings and HUD | Currently IMGUI menus, not physical device displays; no RenderTexture migration in this change. Release UI overhaul remains separate. |

## Production policy

Use shaders for simple numbers, bars and blinking. Use textures for fixed pixel labels and illustrations. Consider UI rendered into RenderTexture for physical screens containing dynamic sentences, contract lists and menus. World Space Canvas is also a candidate for attached interactive UI, while screens embedded in meshes retain material-based output. No new product screen RenderTexture was introduced. Test-capture RenderTextures are separate from product-screen implementation.

The host determines game state; peers display the same values. Display changes do not alter attack, delivery or settlement rules. Both shaders retain depth testing and depth writing. The terminal CRT mask still depends on existing placement coordinates and requires adjustment if placement changes.

## Validation and limitations

Windows build succeeded. Comparing terminal progress at 25% and 75% changed 212 front-view pixels, 0 rear-view pixels and 0 pixels behind an occluding wall. Confirmed progress stays inside the display. No measured performance improvement or release UI completion is claimed. Human readability, comfort and feel remain separate evaluations.

[CRT validation](../../artifacts/space-foundation/crt-progress-result.json) · [Baton shader](../../NoReturns/Assets/_NoReturns/Resources/BatonDisplay.shader) · [Terminal shader](../../NoReturns/Assets/_NoReturns/Resources/ReceiptUI/ReceiptCRT.shader)

Final validation: passed 11 two-process baton display/recharge checks and 30 delivery/receipt/settlement checks. Terminal front/rear/wall-occlusion render checks passed. Inspected the in-game mid-charge baton display. Bilingual documentation checks passed for 216 documents. Human readability and performance measurements were not performed.

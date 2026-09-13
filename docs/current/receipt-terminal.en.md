# Receipt terminal

0.8.14: [Display implementation policy and current audit](display-systems.en.md).

[한국어](receipt-terminal.ko.md)

2026-09-13 · 0.8.7 · Implemented · verification scope below

## Current rules

Set the parcel inside the 3m wide by 2m deep floor outline centered at (-6,0,9). Acceptance requires its center strictly inside x=-7.05~-4.95, z=8.4~9.6, y=0.2~0.65m, no holder, and speed below 0.2m/s for 0.75 seconds. This does not pay money.

Receipt printing completes after 0.75 seconds. Look at the terminal within 2.4m and press [E] to collect. Either teammate can collect it as shared team inventory. It does not require separate physics carrying or occupy a hand slot. Duplicate collection is rejected. Once everyone is aboard, [F] return/settlement pays standard 300+120=420 or risk 450+180=630 once. Successful delivery count increases then. Returning without collection, emergency recovery or mid-shift disconnect pays nothing for that shift. Previously earned balance remains. Next shift resets receipt state.

## Appearance and production guide

Rotated the terminal model 180 degrees to face the delivery approach. Removed the additional overhead panel and placed KO/EN text inside the original CRT. Screen center (-8.09,1.25,8.765) and slot origin (-7.96,0.84,8.691) were measured on the actual model surface. Paper extends forward from the slot and disappears when collected. Floor outline, scan and audio remain.

Removed the separate Reception recorder and fake paper prop beside it. Investigate its mystery clue by looking at the existing receipt terminal and pressing [I]. The maintenance terminal and both clue texts remain. No new 3D generation.

## Technical and validation

[CarryMission.cs](../../NoReturns/Assets/_NoReturns/Runtime/CarryLab/CarryMission.cs) owns printing, collection and settlement. [ReceiptFeedback.cs](../../NoReturns/Assets/_NoReturns/Runtime/CarryLab/ReceiptFeedback.cs) handles presentation only. Protocol 8 shares collection and print-ready state; both peers must use 0.8.6. The 0.8.5 immediate-payment rule and separate display panel are retired.

The pre-change test reproduced payment immediately on delivery. Verification order: missing receipt, duplicate collection, return and abort rules → two real processes → screen and slot inspection → Windows build and documentation checks. Human handling, listening, fun and internet validation remain separate and incomplete.


## Verification results

Windows 0.8.6 build succeeded. Nine Unity rule checks and 29 delivery/receipt/return checks in two real processes passed. Inspected the built-in CRT and slot ejection in an actual game capture. Automated tests do not replace human handling, listening or fun validation.

[Build](../../artifacts/space-foundation/receipt-build-result.json) · [Rules](../../artifacts/space-foundation/receipt-return-result.json) · [Two processes](../../artifacts/space-play-02/latest.json) · [Screen and slot](../../artifacts/space-play-02/run-20260913-180442/receipt-english.png)

Also passed 24 clue/receipt/settlement checks in two processes with the Listener active. The removed recorder's clue is shared through the main terminal, retained after return and reset on next arrival. Documentation check passed for 208 entries.

[Clue regression](../../artifacts/space-play-06/latest.json)

## CRT surface texture fix — 0.8.7

Removed the separate TextMesh and font material. Six states in Korean and English are rendered into individual 256×256 textures, displayed only on the original CRT glass surface. The case texture remains. ReceiptCRT uses opaque depth writes, depth testing and backface culling so walls or the terminal itself occlude the text. No UI object floats in front of the screen. Delivery, receipt, pay and protocol 8 remain unchanged.

Production: tools/make_receipt_ui.py generates KO/EN state textures. The glass mask in ReceiptUI/ReceiptCRT.shader uses measured coordinates for the current placement; moving or resizing the terminal requires adjusting it. Validation compares two UI states from the same camera. Front pixels must change; rear and wall-occluded views must not change.

[Surface/occlusion test](../../artifacts/space-foundation/crt-validation-result.json) · [Front](../../artifacts/receipt-crt/front-ko.png) · [Wall occlusion](../../artifacts/receipt-crt/blocked-a.png)

0.8.7 final verification: changing UI affected 673 front pixels, 0 rear pixels and 0 wall-occluded pixels. Front changes were confined to CRT text. Windows build, 29 two-process checks and 208 documentation entries passed. Also inspected surface UI in the actual game.

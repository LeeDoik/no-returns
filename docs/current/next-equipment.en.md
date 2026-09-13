# Shock baton — current specification

0.8.14: [Display implementation policy and current audit](display-systems.en.md).

[ko](next-equipment.ko.md)

0.8.13 · Integrated-display model applied.

- Default-issued, automatically held with empty hands, used with left mouse click. Hide while carrying cargo/beacon, down or rescuing. Preserve E/Q.
- Unlimited uses, 6-second cooldown. Listener stun: 2.5m range, 65-degree angle, 3-second duration. Do not extend an existing stun; resist restun for 2 seconds after recovery. The 2-second resistance is a playtest value. Outer creatures are immune.
- Generated a new private Tripo model from the approved reference. 65+20=85 credits, balance 975→890. Preserve source and previous model.
- Removed 357 inner-glass faces and constructed a separate BatonScreen mesh/UV inside the model. Length 0.68m; body texture 512. Removed the old external display assembly.
- Swap 13 textures at 48×96 on the recessed screen material. Amber bars fill; a green check means ready. Display the host cooldown.
- The electrode arc continuously modulates shape/width at 18Hz, thickening for 0.18 seconds after use. Hide it with the weapon. Electricity is cosmetic; the screen indicates readiness.

## Production and validation

Windows build succeeded. Blender front view and FBX roundtrip confirmed 2 body/screen meshes with UVs. Ready and mid-charge game captures show only the recessed display changing without an external assembly. Passed 11 two-process display/recharge checks and 3 Unity logic checks for no stun extension, recovery resistance and expiry. Mouse hardware, human impact feel, 4-player and overall performance validation remain.

## Display implementation options

Currently swaps prepared textures. Alternatives include drawing charge directly in a shader, rendering UI into a RenderTexture applied to the model material, or using a World Space Canvas. This answers the question without changing implementation. A shader is a candidate for a small charge gauge; UI/RenderTexture can be considered for complex terminals.

[Model](../../art/psx-baton-02/selected/validation.json) · [Feedback checks](../../artifacts/baton-feedback/latest.json) · [Resistance checks](../../artifacts/space-foundation/baton-resistance-result.json)

Final validation: all 24 two-process stun/down/rescue/emergency recovery regressions also passed. The 11 display checks and 3 resistance logic checks cover separate scopes.

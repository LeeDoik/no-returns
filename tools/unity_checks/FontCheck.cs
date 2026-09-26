// Run with Unity CLI eval_file in Edit mode; see docs/current/macos-development.en.md.
if (UnityEditor.EditorApplication.isPlaying)
    throw new System.InvalidOperationException("Run the font check in Edit mode.");
NoReturns.CarryLab.CarryLanguage.Initialize(null);
var font = NoReturns.CarryLab.CarryLanguage.Font;
if (font == null || !font.dynamic)
    throw new System.Exception("Bundled dynamic font is missing.");
if (UnityEngine.TextCore.LowLevel.FontEngine.LoadFontFace(font, 20) != UnityEngine.TextCore.LowLevel.FontEngineError.Success)
    throw new System.Exception("Cannot load font face: " + font.name);
foreach (var glyph in "한국어가나다ABC123")
    if (!font.HasCharacter(glyph)) throw new System.Exception("Missing glyph: " + glyph);
return "FONT PASS: " + font.name;

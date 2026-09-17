$ErrorActionPreference = 'Stop'
Add-Type -AssemblyName System.Speech
$trailerSynth = New-Object System.Speech.Synthesis.SpeechSynthesizer
$trailerSynth.SelectVoice('Microsoft Zira Desktop')
$trailerSynth.Rate = -1
$trailerSynth.Volume = 100
$trailerLines = Get-Content (Join-Path $PSScriptRoot 'narration.json') -Raw -Encoding UTF8 | ConvertFrom-Json
for ($trailerIndex = 0; $trailerIndex -lt $trailerLines.Count; $trailerIndex++) {
    if ($trailerLines[$trailerIndex].silent) { continue }
    $trailerSynth.SetOutputToWaveFile((Join-Path $PSScriptRoot ('voice-{0}.wav' -f $trailerIndex)))
    $trailerSynth.Speak($trailerLines[$trailerIndex].en)
    $trailerSynth.SetOutputToNull()
}
$trailerSynth.Dispose()

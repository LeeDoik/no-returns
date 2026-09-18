param([string]$Destination)
$ErrorActionPreference = 'Stop'
$workspace = Split-Path $PSScriptRoot -Parent
$source = Join-Path $workspace 'builds/CinderDemo'
if (-not $Destination) { $Destination = Join-Path ([Environment]::GetFolderPath('Desktop')) ('NO_RETURNS_Cinder_Demo_' + (Get-Date -Format 'yyyyMMdd-HHmmss')) }
if (Test-Path -LiteralPath $Destination) { throw "Destination already exists: $Destination" }
$required = @('NoReturns-CinderDemo.exe','NoReturns-CinderDemo_Data','UnityPlayer.dll','MonoBleedingEdge')
foreach ($name in $required) { if (-not (Test-Path -LiteralPath (Join-Path $source $name))) { throw "Missing runtime: $name" } }
New-Item -ItemType Directory -Path $Destination | Out-Null
$entries = Get-ChildItem -LiteralPath $source | Where-Object { $_.Name -notlike '*BackUpThisFolder_ButDontShipItWithYourGame*' }
$manifest = @()
foreach ($entry in $entries) {
    Copy-Item -LiteralPath $entry.FullName -Destination $Destination -Recurse
    $files = if ($entry.PSIsContainer) { Get-ChildItem -LiteralPath $entry.FullName -Recurse -File } else { @($entry) }
    foreach ($file in $files) {
        $relative = $file.FullName.Substring($source.Length).TrimStart('\','/')
        $copied = Join-Path $Destination $relative
        $hash = (Get-FileHash -LiteralPath $file.FullName -Algorithm SHA256).Hash
        if ((Get-FileHash -LiteralPath $copied -Algorithm SHA256).Hash -ne $hash) { throw "Hash mismatch: $relative" }
        $manifest += [pscustomobject]@{path=$relative;bytes=$file.Length;sha256=$hash}
    }
}
[IO.File]::WriteAllText((Join-Path $Destination 'PLAY.cmd'), "@echo off`r`ncd /d `"%~dp0`"`r`nstart `"`" `"NoReturns-CinderDemo.exe`"`r`n", [Text.Encoding]::ASCII)
Copy-Item -LiteralPath (Join-Path $workspace 'docs/current/cinder-portable.ko.md') -Destination (Join-Path $Destination 'README.ko.md')
Copy-Item -LiteralPath (Join-Path $workspace 'docs/current/cinder-portable.en.md') -Destination (Join-Path $Destination 'README.en.md')
foreach ($language in @('ko','en')) {
    $guide = Join-Path $Destination "README.$language.md"
    [IO.File]::WriteAllText($guide, ([IO.File]::ReadAllText($guide)).Replace('cinder-portable.','README.'), [Text.UTF8Encoding]::new($false))
}
$manifest | ConvertTo-Json -Depth 3 | Set-Content -LiteralPath (Join-Path $Destination 'runtime-manifest.json') -Encoding utf8
[pscustomobject]@{destination=$Destination;runtimeFiles=$manifest.Count;bytes=($manifest | Measure-Object bytes -Sum).Sum;hashes='PASS'} | ConvertTo-Json

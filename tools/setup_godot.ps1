$ErrorActionPreference = 'Stop'
$nrRoot = Split-Path -Parent $PSScriptRoot
$nrDestination = Join-Path $nrRoot '.tools/godot'
$nrArchive = Join-Path $nrDestination 'Godot_v4.7.2-stable_win64.exe.zip'
$nrUrl = 'https://github.com/godotengine/godot-builds/releases/download/4.7.2-stable/Godot_v4.7.2-stable_win64.exe.zip'
$nrExpected = '83decd58fdf67b9d657958a1ae6bf1929c20785315a81effe245874cdc57acb709bf868e00778a96984338c1b29dafdb453c6847747694621c6ecf5da2259993'
New-Item -ItemType Directory -Force -Path $nrDestination | Out-Null
if (-not (Test-Path -LiteralPath $nrArchive)) {
    Invoke-WebRequest -Uri $nrUrl -OutFile $nrArchive
}
$nrActual = (Get-FileHash -LiteralPath $nrArchive -Algorithm SHA512).Hash
if ($nrActual -ine $nrExpected) {
    throw 'Godot archive checksum mismatch. Do not run this archive.'
}
Expand-Archive -LiteralPath $nrArchive -DestinationPath $nrDestination -Force
New-Item -ItemType File -Force -Path (Join-Path $nrDestination '_sc_') | Out-Null
& (Join-Path $nrDestination 'Godot_v4.7.2-stable_win64_console.exe') --version
if ($LASTEXITCODE -ne 0) { throw 'Godot version check failed' }
Write-Host 'Verified Godot 4.7.2 portable runtime. Open PLAY.cmd to play.'

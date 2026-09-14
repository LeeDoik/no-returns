param([ValidateSet('open','setup','check','status','commands','trial')][string]$Action='open')
$ErrorActionPreference='Stop'
$repo=Split-Path $PSScriptRoot -Parent
$project=Join-Path $repo 'NoReturns'
$projectAlias=Join-Path (Split-Path (Split-Path $repo -Parent) -Parent) 'NoReturnsUnity'
if (Test-Path -LiteralPath $projectAlias) {
 $aliasInfo=Get-Item -LiteralPath $projectAlias
 if($aliasInfo.LinkType -eq 'Junction' -and $aliasInfo.Target -eq $project){$project=$projectAlias}
}
$cli=Join-Path $env:LOCALAPPDATA 'Unity\bin\unity.exe'
New-Item -ItemType Directory -Force (Join-Path $repo 'artifacts\space-foundation') | Out-Null
switch($Action) {
 'open' { & $cli open $project }
 'status' { & $cli status --project-path $project --format json }
 'commands' { & $cli command --project-path $project }
 default {
  # Attach only: ordinary project work must not silently spawn a batch Editor.
  $statusText = & $cli command editor_status --project-path $project --format json
  if($LASTEXITCODE -ne 0){Write-Error 'Unity 연결 없음 / No connected Editor. Open the project once in Unity Hub or run tools/unity.ps1 open in the normal user environment.';exit 1}
  $status = $statusText | ConvertFrom-Json
  if(-not $status.success -or $status.data.result.compiling -or $status.data.result.domainReloadInProgress -or $status.data.result.playMode -ne 'stopped'){
   Write-Error 'Unity 준비 대기 / Editor must be ready, compilation complete, and Play mode stopped.';exit 1
  }
  $menu=switch($Action){'trial'{'NO RETURNS/Trials/Build Flatbed Interior Trial'} 'setup'{'NO RETURNS/Configure Project Foundation'} default{'NO RETURNS/Validate Project Foundation'}}
  $started=[DateTime]::UtcNow
  & $cli command menu --path $menu --project-path $project --timeout 600 --format json
  if($LASTEXITCODE -ne 0){exit $LASTEXITCODE}
  if($Action -eq 'trial'){
   $marker=Join-Path $repo 'artifacts/ship-interior-trial/build-success.txt'
   if(-not (Test-Path -LiteralPath $marker) -or (Get-Item -LiteralPath $marker).LastWriteTimeUtc -lt $started){Write-Error '새 빌드 성공 근거 없음 / No fresh build-success marker; inspect the Editor log.';exit 1}
  }
 }
}
exit $LASTEXITCODE

param([ValidateSet('open','setup','check','status','commands')][string]$Action='open')
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
  $method=if($Action -eq 'setup'){'Configure'}else{'Validate'}
  & $cli run $project --timeout 900 -- -executeMethod "NoReturns.Editor.ProjectBootstrap.$method" -logFile (Join-Path $repo "artifacts\space-foundation\$Action.log")
 }
}
exit $LASTEXITCODE

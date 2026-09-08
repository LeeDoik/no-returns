@echo off
setlocal
if not exist "%~dp0build\NO_RETURNS_0.7\NO_RETURNS.exe" goto missing
if not exist "%~dp0build\NO_RETURNS_0.7\NO_RETURNS.pck" goto missing
start "" /D "%~dp0build\NO_RETURNS_0.7" "%~dp0build\NO_RETURNS_0.7\NO_RETURNS.exe" %*
exit /b 0
:missing
echo Game build missing. Run BUILD.cmd first, or PLAY.cmd for the development version.
pause
exit /b 1

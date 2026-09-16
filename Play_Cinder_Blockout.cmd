@echo off
cd /d "%~dp0"
if not exist "builds\CinderBlockout\NoReturns-CinderBlockout.exe" (
  echo Cinder blockout build missing. See docs/current/cinder-blockout.en.md
  pause
  exit /b 1
)
start "" "builds\CinderBlockout\NoReturns-CinderBlockout.exe" -screen-fullscreen 0 -screen-width 1280 -screen-height 720

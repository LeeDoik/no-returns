@echo off
setlocal
cd /d "%~dp0"
if not exist ".tools\godot\Godot_v4.7.2-stable_win64.exe" (
  echo Godot runtime missing. See docs/prototype/01-first-playable.ko.md
  pause
  exit /b 1
)
start "" ".tools\godot\Godot_v4.7.2-stable_win64.exe" --editor --path "%CD%"

@echo off
setlocal
cd /d "%~dp0"
if not exist ".tools\godot\Godot_v4.7.2-stable_win64.exe" (
  echo Godot runtime missing. See docs/prototype/01-first-playable.ko.md
  pause
  exit /b 1
)
start "" ".tools\godot\Godot_v4.7.2-stable_win64.exe" --path "%CD%" --resolution 640x400 --position 30,50 -- --host
timeout /t 1 /nobreak >nul
start "" ".tools\godot\Godot_v4.7.2-stable_win64.exe" --path "%CD%" --resolution 640x400 --position 700,50 -- --join=127.0.0.1
start "" ".tools\godot\Godot_v4.7.2-stable_win64.exe" --path "%CD%" --resolution 640x400 --position 30,500 -- --join=127.0.0.1
start "" ".tools\godot\Godot_v4.7.2-stable_win64.exe" --path "%CD%" --resolution 640x400 --position 700,500 -- --join=127.0.0.1

@echo off
setlocal
cd /d "%~dp0"
start "" ".tools\godot\Godot_v4.7.2-stable_win64.exe" --path "%CD%" res://scenes/physics_lab.tscn

@echo off
setlocal
cd /d "%~dp0"
start "" ".tools\godot\Godot_v4.7.2-stable_win64.exe" --editor --path "%CD%" "res://scenes/maps/shipping_shrine.tscn"

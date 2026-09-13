@echo off
cd /d "%~dp0builds\CarryTest"
start "NO RETURNS" "NoReturns.exe" --hazard --join 127.0.0.1

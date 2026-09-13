@echo off
cd /d "%~dp0builds\CarryTest"
start "NO RETURNS" "NoReturns.exe" --delivery --join 127.0.0.1

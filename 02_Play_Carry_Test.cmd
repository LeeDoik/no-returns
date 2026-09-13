@echo off
cd /d "%~dp0builds\CarryTest"
if not exist NoReturns.exe (
 echo Carry test build is not available yet.
 pause
 exit /b 1
)
start "" NoReturns.exe 

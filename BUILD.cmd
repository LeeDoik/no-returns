@echo off
setlocal
cd /d "%~dp0"
python tools/build_windows.py
if errorlevel 1 (
  echo BUILD FAILED. See artifacts/export-output.log
  pause
  exit /b 1
)
echo EXE: build\NO_RETURNS_0.7\NO_RETURNS.exe
echo ZIP: build\NO_RETURNS_0.7_Windows.zip
pause

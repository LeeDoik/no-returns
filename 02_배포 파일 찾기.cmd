@echo off
if exist "%~dp0build\NO_RETURNS_0.7_Windows.zip" (
  explorer.exe /select,"%~dp0build\NO_RETURNS_0.7_Windows.zip"
) else (
  explorer.exe "%~dp0build"
)

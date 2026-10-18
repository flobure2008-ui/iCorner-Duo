@echo off
cd /d "%~dp0"
if not exist "iCornerDuo.exe" (
    "C:\Windows\Microsoft.NET\Framework64\v4.0.30319\csc.exe" /target:winexe /win32icon:"icon.ico" /out:"iCornerDuo.exe" /r:System.dll /r:System.Drawing.dll /r:System.Windows.Forms.dll "Program.cs"
)
start "" "iCornerDuo.exe"

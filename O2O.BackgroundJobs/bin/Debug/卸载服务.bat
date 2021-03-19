@echo off

echo 正在卸载服务...

cd C:\Windows\Microsoft.NET\Framework\v4.0.30319

InstallUtil.exe /u %~dp0O2O.BackgroundJobs.exe

pause

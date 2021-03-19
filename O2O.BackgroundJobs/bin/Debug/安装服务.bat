@echo off

echo 正在安装服务...

cd C:\Windows\Microsoft.NET\Framework\v4.0.30319

InstallUtil.exe %~dp0O2O.BackgroundJobs.exe

echo 安装完成。

echo ----------------------------------------------------------

net start 外卖后台服务

pause
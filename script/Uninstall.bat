@echo off
chcp 65001
%~dp0Service.exe stop && %~dp0Service.exe uninstall
echo 卸载成功！！
timeout /nobreak /t 3
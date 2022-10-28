@echo off
chcp 65001
.\SrunLogin.exe --config=.\config01.yaml
timeout /nobreak /t 2.5
::pause
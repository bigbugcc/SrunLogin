@echo off
chcp 65001
(
  echo "<service>"
  echo "  <id>SrunLogin</id>"
  echo "  <name>SrunLogin</name>"
  echo "  <description>校园网登录服务</description>"
  echo "  <executable>%~dp0SrunLogin.exe</executable>"
  echo "  <arguments>--config=./config02.yaml</arguments>"
  echo "  <onfailure action="restart" delay="60 sec"/>"
  echo "  <log mode="roll-by-size">"
  echo "    <sizeThreshold>5120</sizeThreshold>"
  echo "    <keepFiles>3</keepFiles>"
  echo "  </log>"
  echo "  <logpath>logs</logpath>"
  echo "</service>"
)> ./Service.txt
del Service.xml
FOR /F "delims=" %%i IN (./Service.txt) DO (
  @echo %%~i >> ./Service.xml
)
del Service.txt
::重新注册服务
%~dp0Service.exe stop && %~dp0Service.exe uninstall
%~dp0Service.exe install && %~dp0Service.exe start
echo 服务执行成功!!!
timeout /nobreak /t 3
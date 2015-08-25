cls
echo off
reg query "HKU\S-1-5-19"
if errorlevel 1 (
   echo Administrative Privileges Required
   pause
   exit /b %errorlevel%
)
SET DIR=%~dp0%
IF NOT EXIST "%DIR%log" MKDIR "%DIR%log"
IISRESET
"%WINDIR%\Microsoft.Net\Framework\v4.0.30319\msbuild.exe" /m /v:n "%DIR%invoicegen.proj" /target:Install /logger:FileLogger,Microsoft.Build.Engine;LogFile=%DIR%log/install.log
pause
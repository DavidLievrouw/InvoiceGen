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
"%PROGRAMFILES(X86)%\MSBuild\14.0\Bin\MsBuild.exe" /m /v:n "%DIR%DavidLievrouw.InvoiceGen.proj" /target:BuildDeployInstall /logger:FileLogger,Microsoft.Build.Engine;LogFile=%DIR%log/builddeployinstall.log
pause
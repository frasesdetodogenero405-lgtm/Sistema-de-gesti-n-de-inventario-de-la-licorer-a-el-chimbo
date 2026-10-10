@echo off
setlocal
cd /d "%~dp0"
start "Licorería El Chimbo - MVC" cmd /k "dotnet run --project SistemaLicoreria.Web\SistemaLicoreria.Web.csproj"
timeout /t 2 /nobreak >nul
start "Licorería El Chimbo - YARP" cmd /k "dotnet run --project SistemaLicoreria.Proxy\SistemaLicoreria.Proxy.csproj"
timeout /t 3 /nobreak >nul
start "" "http://127.0.0.1:5070"
endlocal

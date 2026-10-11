@echo off
setlocal
cd /d "%~dp0"
set "VISUAL_STUDIO=C:\Program Files\Microsoft Visual Studio\2022\Community\Common7\IDE\devenv.exe"
set "SOLUCION=%CD%\Sistema_Licoreria_El_Chimbo\Sistema_Licoreria_El_Chimbo.sln"
set "ASPNETCORE_ENVIRONMENT=Development"

if exist "%VISUAL_STUDIO%" (
    start "Visual Studio - Licoreria El Chimbo" "%VISUAL_STUDIO%" "%SOLUCION%"
) else (
    echo No se encontro Visual Studio 2022 Community en la ruta esperada.
)

netstat -ano | findstr /R /C:":5080 .*LISTENING" >nul
if errorlevel 1 (
    start "Licoreria El Chimbo - MVC" cmd /k "dotnet run --project SistemaLicoreria.Web\SistemaLicoreria.Web.csproj --no-launch-profile --urls http://127.0.0.1:5080"
) else (
    echo MVC ya esta activo en http://127.0.0.1:5080
)

netstat -ano | findstr /R /C:":5070 .*LISTENING" >nul
if errorlevel 1 (
    start "Licoreria El Chimbo - YARP" cmd /k "dotnet run --project SistemaLicoreria.Proxy\SistemaLicoreria.Proxy.csproj --no-launch-profile --urls http://127.0.0.1:5070"
) else (
    echo YARP ya esta activo en http://127.0.0.1:5070
)

echo.
echo Sistema listo. Usa el navegador en http://127.0.0.1:5070
endlocal

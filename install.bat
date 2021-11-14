@echo off
set MOD_NAME=
set SE_PATH=C:\Users\YOUR USERNAME HERE\AppData\Roaming\SpaceEngineers
set SE_INSTALL_PATH=C:\Program Files (x86)\Steam\steamapps\common\SpaceEngineers

if %MOD_NAME% == "" (
    echo "Please set MOD_NAME to the name of your mod"
    exit /b 1
)

if %SE_PATH% == C:\Users\YOUR USERNAME HERE\AppData\Roaming\SpaceEngineers (
    echo "Please set SE_PATH to the path to your Space Engineers AppData directory"
    exit /b 1
)

if ! exist %SE_PATH% (
    echo "%SE_PATH% is not a valid directory."
    exit /b 1
)

cd /D "%~dp0"
if ! exist .\Data\Scripts (
    echo "Performing initial setup..."
    mkdir .\Data\Scripts\%MOD_NAME%
    copy .\setup_files\template.cs .\Data\Scripts\%MOD_NAME%\%MOD_NAME%.cs
    copy .\setup_files\SE_Mod.csproj .\Data\Scripts\%MOD_NAME%
    (Get-Content .\Data\Scripts\%MOD_NAME%\SE_Mod.csproj) -replace "SE_INSTALL_PATH", "%SE_INSTALL_PATH%" | Out-File -encoding ASCII .\Data\Scripts\%MOD_NAME%\SE_Mod.csproj
    cd .\Data\Scripts
    dotnet new sln -n SE_Mod
    dotnet sln add %MOD_NAME%\SE_Mod.csproj
    dotnet restore
    echo "Done. Restart VS Code if you already had the folder open in it."
)
else (
    set INSTALL_PATH=%SE_PATH%\Mods\%MOD_NAME%
    echo "Installing to %INSTALL_PATH%..."
    copy %INSTALL_PATH%\modinfo.sbmi . 1>nul 2>nul
    rd /S /Q %INSTALL_PATH%
    md %INSTALL_PATH%
    copy .\metadata.mod %INSTALL_PATH% 1>nul 2>nul
    copy .\modinfo.sbmi %INSTALL_PATH% 1>nul 2>nul
    copy .\thumb.jpg %INSTALL_PATH% 1>nul 2>nul
    copy .\README* %INSTALL_PATH% 1>nul 2>nul
    robocopy /E /NS /NC /NFL /NDL /NP /NJH /NJS .\Data %INSTALL_PATH%\Data 1>nul
    del /Q %INSTALL_PATH%\Data\Scripts\*.sln 1>nul 2>nul
    del /Q %INSTALL_PATH%\Data\Scripts\%MOD_NAME%\*.csproj 1>nul 2>nul
    rd /S /Q %INSTALL_PATH%\Data\Scripts\%MOD_NAME%\bin 1>nul 2>nul
    rd /S /Q %INSTALL_PATH%\Data\Scripts\%MOD_NAME%\obj 1>nul 2>nul
    echo Installed mod at %INSTALL_PATH%
)

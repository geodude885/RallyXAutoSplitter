@echo off
REM Thunderstore Package Builder for RallyXAutoSplitter

echo Updating version from Version.props...
powershell -ExecutionPolicy Bypass -File "UpdateVersion.ps1"

if errorlevel 1 (
    echo Version update failed!
    pause
    exit /b 1
)

REM Read version from Version.props
for /f "tokens=2 delims=<>" %%a in ('findstr /C:"<ModVersion>" Version.props') do set MOD_VERSION=%%a

echo.
echo Building RallyXAutoSplitter v%MOD_VERSION% in Release mode...
dotnet build RallyXAutoSplitter\RallyXAutoSplitter.csproj -c Release

if errorlevel 1 (
	echo Build failed!
	pause
	exit /b 1
)

echo.
echo Creating Thunderstore package...

REM Clean output folder
if exist ThunderstorePackage rmdir /s /q ThunderstorePackage
mkdir ThunderstorePackage\Mods

REM Copy files
copy "RallyXAutoSplitter\bin\Release\net6.0\RallyXAutoSplitter.dll" "ThunderstorePackage\Mods\" >nul
copy "README.md" "ThunderstorePackage\" >nul
copy "Thunderstore\manifest.json" "ThunderstorePackage\" >nul
copy "Thunderstore\CHANGELOG.md" "ThunderstorePackage\" >nul

REM Generate manifest.json with current version
(
echo {
echo   "name": "RallyXAutoSplitter",
echo   "version_number": "%MOD_VERSION%",
echo   "website_url": "https://github.com/geodude885/RallyXAutoSplitter",
echo   "description": "Automatic LiveSplit integration for RUMBLE race mode - starts timer, splits at track starts, and handles resets",
echo   "dependencies": [
echo     "LavaGang-MelonLoader-0.7.2"
echo   ]
echo }
) > "ThunderstorePackage\manifest.json"

echo [OK] Files copied

REM Check for icon
if exist "Thunderstore\icon.png" (
	copy "Thunderstore\icon.png" "ThunderstorePackage\" >nul
	echo [OK] Icon included
) else (
	echo [WARNING] icon.png not found! Create a 256x256 PNG in the Thunderstore folder
)

REM Create zip
set ZIP_NAME=ACutiePi-RallyXAutoSplitter-%MOD_VERSION%.zip
if exist %ZIP_NAME% del %ZIP_NAME%
powershell -Command "Compress-Archive -Path 'ThunderstorePackage\*' -DestinationPath '%ZIP_NAME%'"

echo.
echo ========================================
echo Package created successfully!
echo ========================================
echo File: %ZIP_NAME%
echo.
echo Next steps:
echo 1. Create icon.png (256x256) if you haven't
echo 2. Upload the zip to https://thunderstore.io/
echo 3. Sign in with GitHub
echo.
echo To update version: Edit Version.props
echo.
pause

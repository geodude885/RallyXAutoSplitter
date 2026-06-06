@echo off
REM Thunderstore Package Builder for RallyXAutoSplitter

echo Building RallyXAutoSplitter in Release mode...
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

REM Check for icon
if exist "Thunderstore\icon.png" (
	copy "Thunderstore\icon.png" "ThunderstorePackage\" >nul
	echo [OK] Icon included
) else (
	echo [WARNING] icon.png not found! Create a 256x256 PNG in the Thunderstore folder
)

REM Create zip
if exist ACutiePi-RallyXAutoSplitter-1.0.0.zip del ACutiePi-RallyXAutoSplitter-1.0.0.zip
powershell -Command "Compress-Archive -Path 'ThunderstorePackage\*' -DestinationPath 'ACutiePi-RallyXAutoSplitter-1.0.0.zip'"

echo.
echo ========================================
echo Package created successfully!
echo ========================================
echo File: ACutiePi-RallyXAutoSplitter-1.0.0.zip
echo.
pause

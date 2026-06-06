# Update Version Script
# Updates all version references from Version.props

param([switch]$Build)

# Read version from Version.props
[xml]$versionProps = Get-Content "Version.props"
$version = $versionProps.Project.PropertyGroup.ModVersion
$author = $versionProps.Project.PropertyGroup.ModAuthor
$modName = $versionProps.Project.PropertyGroup.ModName

Write-Host "Updating version to: $version" -ForegroundColor Cyan
Write-Host "Author: $author" -ForegroundColor Cyan

# Update Mod.cs
$modCs = Get-Content "RallyXAutoSplitter\Mod.cs" -Raw
$modCs = $modCs -replace 'AssemblyVersion\(".*?"\)', "AssemblyVersion(`"$version`")"
$modCs = $modCs -replace 'MelonInfo\(typeof\(RallyXAutoSplitter\.Mod\), ".*?", ".*?", ".*?"\)', "MelonInfo(typeof(RallyXAutoSplitter.Mod), `"$modName`", `"$version`", `"$author`")"
Set-Content "RallyXAutoSplitter\Mod.cs" -Value $modCs -NoNewline
Write-Host "[OK] Updated Mod.cs" -ForegroundColor Green

# Update manifest.json - just replace the version, keep everything else
$manifestPath = "Thunderstore\manifest.json"
$manifestContent = Get-Content $manifestPath -Raw
$manifestContent = $manifestContent -replace '"version_number"\s*:\s*"[^"]*"', "`"version_number`": `"$version`""
Set-Content $manifestPath -Value $manifestContent -NoNewline
Write-Host "[OK] Updated manifest.json" -ForegroundColor Green

Write-Host ""
Write-Host "Version update complete!" -ForegroundColor Green
Write-Host "Remember to update CHANGELOG.md manually" -ForegroundColor Yellow

if ($Build) {
	Write-Host ""
	Write-Host "Building..." -ForegroundColor Cyan
	dotnet build RallyXAutoSplitter\RallyXAutoSplitter.csproj -c Release
}

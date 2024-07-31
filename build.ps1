$projectPath = "src/YanehCheck.EpicGamesUtils.WpfUiApp/YanehCheck.EpicGamesUtils.WpfUiApp.csproj"
$buildsDir = "builds"
$projectName = "DesdemonaToolkit"

# Create builds directory
if (-Not (Test-Path -Path $buildsDir)) {
    New-Item -ItemType Directory -Path $buildsDir
}

$version = (Select-String -Path $projectPath -Pattern "<Version>(.*)</Version>" | ForEach-Object { $_.Matches[0].Groups[1].Value }).Trim()

dotnet publish $projectPath -c Release -o "$buildsDir/$projectName-$version-framework-dependent"

dotnet publish $projectPath -c Release -r win-x64 --self-contained -o "$buildsDir/$projectName-$version-self-contained"

Compress-Archive -Path "$buildsDir/$projectName-$version-framework-dependent/*" -DestinationPath "$buildsDir/$projectName-$version-framework-dependent.zip"
Compress-Archive -Path "$buildsDir/$projectName-$version-self-contained/*" -DestinationPath "$buildsDir/$projectName-$version-self-contained.zip"

Remove-Item -Recurse -Force "$buildsDir/$projectName-$version-framework-dependent"
Remove-Item -Recurse -Force "$buildsDir/$projectName-$version-self-contained"
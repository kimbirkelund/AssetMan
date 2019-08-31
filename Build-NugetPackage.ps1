[CmdletBinding()]
PARAM(
    [string]$PackageVersion,
    [switch]$NoBuild,
    [string]$OutputDirectory = $PWD
)

if (!$PackageVersion)
{
    $PackageVersion = nbgv get-version -v NuGetPackageVersion;
}

$ErrorActionPreference = "Stop";

Push-Location $PSScriptRoot;
try
{
    # Publish CLI tool
    dotnet msbuild "/t:Restore,PublishAll" `
        .\src\AssetMan.Cli\AssetMan.Cli.csproj `
        "/p:BasePublishDir=$(Join-Path $PWD publish)";

    # Build msbuild task project
    if (!$NoBuild)
    {
        dotnet build `
            .\src\AssetMan.Tasks\AssetMan.Tasks.csproj
    }

    # Create nuget package
    dotnet pack `
        .\src\AssetMan.Tasks\AssetMan.Tasks.csproj `
        --no-build --no-restore `
        --output $OutputDirectory `
        "-p:PackageVersion=$PackageVersion" `
        "-p:PublishDir=$(Join-Path $PWD publish)";
}
finally
{
    Pop-Location;
}


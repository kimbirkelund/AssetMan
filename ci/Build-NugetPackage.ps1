[CmdletBinding()]
PARAM(
    [string]$PackageVersion,
    [switch]$NoBuild,
    [string]$OutputDirectory = $PWD,
    [string]$Configuration = "Debug"
)

if (!$PackageVersion)
{
    $PackageVersion = nbgv get-version -v NuGetPackageVersion;
}

$ErrorActionPreference = "Stop";

Push-Location (Split-Path -Parent $PSScriptRoot);
try
{
    .\ci\Clean-ReadmeForNuget.ps1

    # Publish CLI tool
    dotnet msbuild `
    	"-t:Restore,PublishAll" `
        .\src\AssetMan.Cli\AssetMan.Cli.csproj `
        "-p:Configuration=$Configuration" `
        "-p:BasePublishDir=$(Join-Path $PWD publish)";

    # Build msbuild task project
    if (!$NoBuild)
    {
        dotnet build `
            .\src\AssetMan.Tasks\AssetMan.Tasks.csproj `
            --configuration $Configuration
    }

    # Create nuget package
    dotnet pack `
        .\src\AssetMan.Tasks\AssetMan.Tasks.csproj `
        --configuration $Configuration `
        --no-build --no-restore `
        --output $OutputDirectory `
        "-p:PackageVersion=$PackageVersion" `
        "-p:PublishDir=$(Join-Path $PWD publish)";
}
finally
{
    Pop-Location;
}


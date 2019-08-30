[CmdletBinding()]
PARAM()

$ErrorActionPreference = "Stop";

Push-Location $PSScriptRoot;
try
{
    # Publish CLI tool
    msbuild /t:PublishAll `
        .\src\AssetMan.Cli\AssetMan.Cli.csproj `
        "/p:BasePublishDir=$(Join-Path $pwd publish)";

    # Build msbuild task project
    dotnet build `
        .\src\AssetMan.Tasks\AssetMan.Tasks.csproj

    # Create nuget package
    dotnet pack `
        .\src\AssetMan.Tasks\AssetMan.Tasks.csproj `
        --no-build --no-restore `
        --output $PWD `
        "-p:PackageVersion=$(nbgv get-version -v NuGetPackageVersion)" `
        "-p:PublishDir=$(Join-Path $PWD publish)";
}
finally
{
    Pop-Location;
}


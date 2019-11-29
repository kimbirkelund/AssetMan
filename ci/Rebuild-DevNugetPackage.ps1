[CmdletBinding()]
PARAM(
    [switch]$NoBuild
)

$ErrorActionPreference = "Stop";

Push-Location $PSScriptRoot;
try
{
    Remove-Item ~\.nuget\packages\assetman\ -Recurse -ErrorAction SilentlyContinue;
    Remove-Item .\AssetMan.*.nupkg -ErrorAction SilentlyContinue;

    .\Build-NugetPackage.ps1 -PackageVersion 0.0.0 -NoBuild:$NoBuild
}
finally
{
    Pop-Location;
}


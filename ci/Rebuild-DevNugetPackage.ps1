[CmdletBinding()]
PARAM(
    [switch]$NoBuild
)

$ErrorActionPreference = "Stop";

Push-Location (Split-Path -Parent $PSScriptRoot);
try
{
    Remove-Item ~\.nuget\packages\assetman\ -Recurse -ErrorAction SilentlyContinue;
    Remove-Item .\AssetMan.*.nupkg -ErrorAction SilentlyContinue;

    .\ci\Build-NugetPackage.ps1 -PackageVersion 0.0.0 -NoBuild:$NoBuild
}
finally
{
    Pop-Location;
}


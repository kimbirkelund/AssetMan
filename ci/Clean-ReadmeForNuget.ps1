[CmdletBinding()]
PARAM()

$PSCmdlet.MyInvocation.BoundParameters.Keys | ForEach-Object { Write-Verbose "$($PSCmdlet.MyInvocation.MyCommand)-$($_): $($PSCmdlet.MyInvocation.BoundParameters[$_])" }

$ErrorActionPreference = "Stop";
Set-StrictMode -Version "Latest";

Push-Location $PSScriptRoot;
try
{
    $keepLine = $true
    $lines = Get-Content ..\README.md |
        Where-Object {
            if ($_ -eq "<!-- REMOVE FOR NUGET -->")
            {
                $keepLine = $false;
                return $false;
            }
            if ($_ -eq "<!-- /REMOVE FOR NUGET -->")
            {
                $keepLine = $true;
                return $false;
            }

            return $keepLine
        }

    $lines |
        Set-Content ..\README.md

}
finally
{
    Pop-Location;
}


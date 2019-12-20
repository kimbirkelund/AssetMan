[CmdletBinding()]
PARAM(
    [string]$NupkgPath,
    [string]$ProjectFiles
)

$ErrorActionPreference = "Stop";

$NupkgPath = Resolve-Path $NupkgPath;
$ProjectFiles = Get-ChildItem -Recurse $ProjectFiles | Select-Object -ExpandProperty FullName;

if (Test-Path -PathType Container $NupkgPath)
{
    $NupkgPath = Get-ChildItem -Recurse -Path $NupkgPath -Filter "AssetMan.*.nupkg" |
        Sort-Object -Descending |
        Select-Object -First 1;
}

$nupkgVersion = ([regex]"(?<version>\d+\.\d+\.\d+.*)\.nupkg$").Match((Split-Path -Leaf $NupkgPath)).Groups["version"].Value;
Write-Verbose "nupkgVersion: $nupkgVersion";

foreach ($projectFile in $ProjectFiles)
{
    Write-Verbose "projectFile: $projectFile";

    Push-Location (Split-Path -Parent $projectFile);
    try
    {
        $projectXmlNamespace = "http://schemas.microsoft.com/developer/msbuild/2003";

        $projectXml = [xml](Get-Content $projectFile);

        $nsm = [System.Xml.XmlNamespaceManager]::new($projectXml.NameTable);
        $nsm.AddNamespace("p", $projectXmlNamespace);

        $projectNode = $projectXml.SelectSingleNode("p:Project", $nsm);
        foreach ($node in $projectNode.SelectNodes("p:Import[starts-with(@Project, '..\..\AssetMan.Tasks\AssetMan')]", $nsm))
        {
            $projectNode.RemoveChild($node) | Out-Null;
        }

        $itemGroupElement = $projectXml.CreateElement("ItemGroup", $projectXmlNamespace);
        $projectNode.AppendChild($itemGroupElement) | Out-Null;

        $packageReferenceElement = $projectXml.CreateElement("PackageReference", $projectXmlNamespace);
        $itemGroupElement.AppendChild($packageReferenceElement) | Out-Null;
        $packageReferenceElement.SetAttribute("Include", "AssetMan");
        $packageReferenceElement.SetAttribute("Version", $nupkgVersion);

        $projectXml.Save($projectFile);

        $nugetConfigFile = "nuget.config";

        "<configuration/>" | Set-Content $nugetConfigFile;

        nuget sources add -Name local -Source (Split-Path -Parent $NupkgPath) -ConfigFile $nugetConfigFile;
    }
    finally
    {
        Pop-Location;
    }
}

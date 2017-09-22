#
# XmlFiles.ps1
#
# Snippets used to process XML files
Exit

# List NuGet packages within a given source control structure
$packages = @()
Get-ChildItem -Path C:\Source -File -Include packages.config -Recurse | Select-Xml -XPath /packages/package | ForEach-Object {
    foreach($node in $_.Node)
    {
        $package = @{}
        $package.Path = $_.Path
        $package.Id = $node.id
        $package.Version = $node.version
        $package.TargetFramework = $node.targetFramework
        $packages += New-Object -TypeName PSObject -Prop $package
    }
}
$packages | Out-GridView

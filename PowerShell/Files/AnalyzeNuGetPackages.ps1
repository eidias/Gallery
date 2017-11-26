#
# AnalyzeNuGetPackages.ps1
#
# 


function Get-InstalledPackages
{
    [CmdletBinding()]
    param(
        [Parameter(ValueFromPipeline=$true)]
        $Path
    )
    process
    {
        $installedPackages = @()
        Get-ChildItem -Path $Path -File -Include packages.config -Recurse | Select-Xml -XPath /packages/package | ForEach-Object {
            foreach($node in $_.Node)
            {
                $package = @{}
                $package.Path = $_.Path
                $package.Id = $node.id
                $package.Version = $node.version
                $package.TargetFramework = $node.targetFramework
                $installedPackages += New-Object -TypeName PSObject -Prop $package
            }
        }
        return $installedPackages
	}
}

function Get-UpdatablePackages
{
    [CmdletBinding()]
    param(
        [Parameter(ValueFromPipeline=$true)]
        $Path,
        [Parameter()]
        $Source = "http://www.nuget.org/api/v2",
        [Parameter()]
        [switch]$SkipInternal
    )
    process
    {
        Write-Host "Checking for updatable NuGet packages using '$($Source)'."
        $updatablePackages = @()
        Get-InstalledPackages $Path | Sort-Object -Property Id -Unique | ForEach {
            $package = @{}
            $package.Id = $_.Id
            $package.Source = $Source
            $package.CurrentVersion = $_.Version
            $package.AvailableVersion = Find-Package -Name $package.Id –Provider NuGet -Source $Source -ErrorAction SilentlyContinue | Select-Object -ExpandProperty Version
            if($error)
            {
                $error.Clear()
                $package.AvailableVersion = "N/A"
                if($SkipInternal)
                {
                    return
                }
            }
            $updatablePackages += New-Object -TypeName PSObject -Prop $package
            Write-Host "Package '$($package.Id)' -> current version = '$($package.CurrentVersion)', available version = '$($package.AvailableVersion)'." 
        }
        return $updatablePackages
 	}
}

function Create-PackageReport
{
    [CmdletBinding()]
    param(
        [Parameter(ValueFromPipeline=$true)]
        $Path
    )
    process
    {
        $packages = Get-UpdatablePackages $Path -SkipInternal | Select-Object Id, CurrentVersion, AvailableVersion
        $preContent = "<h3>Package status for repository '$(Split-Path $Path -Leaf)' - $($packages.Count) packages as of $(Get-Date -Format dd.MM.yyyy)</h3><br/>"
        return $packages | ConvertTo-Html -PreContent $preContent
    }
}

Create-PackageReport C:\Source | Out-File C:\Temp\UpdatablePackages.html -Encoding utf8

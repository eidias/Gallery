#
# Configure.ps1
#
# Create web and FTP sites incl. advanced configuration examples
Exit

function Add-WebAppPool
{
    [CmdletBinding()]
    param(
        [Parameter(ValueFromPipeline=$true)]
        [string]$name,
        [Parameter()]
        [switch]$useNetCore

    )
    process
    {
        New-WebAppPool -Name $name
        Set-ItemProperty -Path IIS:\AppPools\$name -Name startMode -Value AlwaysRunning
        Set-ItemProperty -Path IIS:\AppPools\$name -Name processModel -Value @{ idleTimeoutAction = Suspend }
        if($useNetCore)
        {
            # Do not use Clear-ItemProperty as this reverts the setting to its default value.
            Set-ItemProperty -Path IIS:\AppPools\$name -Name managedRuntimeVersion -Value ""
        }
	}
}

function Add-WebApplication
{
    [CmdletBinding()]
    param(
        [Parameter(ValueFromPipeline=$true)]
        [string]$physicalPath,
        [Parameter()]
        [string]$applicationPool,
        [Parameter()]
        [string]$site = "Default Web Site"
    )
    process
    {
        $name = Split-Path $physicalPath -Leaf
        New-WebApplication -Name $name -Site $site -PhysicalPath $physicalPath -ApplicationPool $applicationPool
        Set-ItemProperty -path IIS:\Sites\$site\$name -Name preloadEnabled -Value $true
    }
}


# Show all sites that have application preload enabled.
Get-ChildItem -Path IIS:\Sites | Where-Object { $_.Name -like "My Site" } | Get-ItemPropertyValue -Name applicationDefaults.preloadEnabled


# Modify a web configuration setting
Add-WebConfiguration -Filter "/system.webServer/security/authorization" -PSPath "IIS:\Sites\Default FTP Site" -Value @{accessType="Allow";users="";roles="";verbs=""}

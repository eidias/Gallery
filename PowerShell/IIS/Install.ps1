#
# Install.ps1
#
# Install IIS with the common features for different workloads.
Exit

# ASP.NET 4.6 configuration for servers
Install-WindowsFeature NET-Framework-45-ASPNET,
                       Web-Default-Doc,Web-Dir-Browsing,Web-Http-Errors,Web-Static-Content,Web-Http-Redirect,
                       Web-Http-Logging,Web-Request-Monitor,Web-Http-Tracing,
                       Web-Stat-Compression,Web-Dyn-Compression,
                       Web-Filtering,Web-IP-Security,
                       Web-Net-Ext45,Web-AppInit,Web-Asp-Net45,Web-ISAPI-Ext,Web-ISAPI-Filter,Web-WebSockets,
                       Web-Mgmt-Console


# ASP.NET Core Server Hosting Bundle
Start-BitsTransfer https://download.microsoft.com/download/B/1/D/B1D7D5BF-3920-47AA-94BD-7A6E48822F18/DotNetCore.2.0.0-WindowsHosting.exe
Start-Process .\DotNetCore.2.0.0-WindowsHosting.exe -ArgumentList "" -Wait


# ASP.NET 4.6 configuration for typical development workstations
Enable-WindowsOptionalFeature -Online -FeatureName IIS-WebServerRole
Enable-WindowsOptionalFeature -Online -FeatureName IIS-NetFxExtensibility45
Enable-WindowsOptionalFeature -Online -FeatureName IIS-ASPNET45
Enable-WindowsOptionalFeature -Online -FeatureName IIS-ISAPIExtensions
Enable-WindowsOptionalFeature -Online -FeatureName IIS-ISAPIFilter


# Show all available IIS features incl. their current state
Get-WindowsOptionalFeature -Online | Where-Object {$_.FeatureName -like "IIS-*"}


# Install URL Rewrite Module 2.1 
Start-BitsTransfer https://download.microsoft.com/download/D/D/E/DDE57C26-C62C-4C59-A1BB-31D58B36ADA2/rewrite_amd64_en-US.msi
Start-Process msiexec -ArgumentList "/i $location\rewrite_amd64_en-US.msi /qb" -Wait


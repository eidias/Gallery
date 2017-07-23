#
# Install.ps1
#
# Install IIS with the common features for different workloads.
Exit

# ASP.NET 4.6 configuration
Install-WindowsFeature NET-Framework-45-ASPNET,
                       Web-Default-Doc,Web-Dir-Browsing,Web-Http-Errors,Web-Static-Content,Web-Http-Redirect,
                       Web-Http-Logging,Web-Request-Monitor,Web-Http-Tracing,
                       Web-Stat-Compression,Web-Dyn-Compression,
                       Web-Filtering,Web-IP-Security,
                       Web-Net-Ext45,Web-AppInit,Web-Asp-Net45,Web-ISAPI-Ext,Web-ISAPI-Filter,Web-WebSockets,
                       Web-Mgmt-Console


# ASP.NET Core installation

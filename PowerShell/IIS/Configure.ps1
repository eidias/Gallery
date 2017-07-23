#
# Configure.ps1
#
# Create web and FTP sites incl. advanced configuration examples
Exit

# Show all sites that have application preload enabled.
Get-ChildItem -Path IIS:\Sites | Where-Object { $_.Name -like "My Site" } | Get-ItemPropertyValue -Name applicationDefaults.preloadEnabled


# Modify a web configuration setting
Add-WebConfiguration -Filter "/system.webServer/security/authorization" -PSPath "IIS:\Sites\Default FTP Site" -Value @{accessType="Allow";users="";roles="";verbs=""}

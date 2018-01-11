# Installs the Azure PowerShell module from the PowerShell Gallery
# PowerShell Gallery is by default un untrusted source and needs to be trused for the Azure commands to work
# Needs to be run from an elevated session, but only once when the module is installed
# If the NuGet package provider is outdated, you will be asked to allow updating before the module is installed


# Clobbering allows importing modules having command names that already exists in other modules. The last loaded module wins.
Install-Module AzureRM -AllowClobber


#
# Client.ps1
#
# Client related one liners
Exit

# Enable proxy access using the default credentials for the script.
[System.Net.WebRequest]::DefaultWebProxy.Credentials = [System.Net.CredentialCache]::DefaultCredentials


# Adjust the interface metric so that routing over multiple adapters is properly handled.
Get-NetIPInterface -AddressFamily IPv4 | Sort-Object Interfacemetric
Set-NetIPInterface -InterfaceIndex -1 -InterfaceMetric 20


# List all currently connected networks incl. VPN
Get-NetConnectionProfile


# Create and maintain VPN connections
Add-VpnConnection -Name our-office-net -ServerAddress 127.0.0.1 -SplitTunneling -RememberCredential
Add-VpnConnectionTriggerApplication -Name our-office-net –ApplicationID C:\Windows\System32\notepad.exe
Add-VpnConnectionTriggerDnsConfiguration -ConnectionName our-office-net -DnsSuffix my.internal.domain -DnsIPAddress 127.0.0.1
Remove-VpnConnectionTriggerDnsConfiguration -ConnectionName our-office-net -DnsSuffix my.internal.domain


# Google operates public DNS servers under 8.8.8.8 and 8.8.4.4
Resolve-DnsName -NoHostsFile -DnsOnly -Name "www.google.ch" -Type A -Server 8.8.8.8

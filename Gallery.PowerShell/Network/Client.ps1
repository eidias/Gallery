#
# Client.ps1
#
# Client related one liners and their purpose
Exit

# Adjust the interface metric so that routing over multiple adapters is properly handled.
Get-NetIPInterface -AddressFamily IPv4 | Sort-Object Interfacemetric
Set-NetIPInterface -InterfaceIndex -1 -InterfaceMetric 20

# Create and maintain VPN connections
Add-VpnConnection -Name our-office-net -ServerAddress 127.0.0.1 -SplitTunneling -RememberCredential
Add-VpnConnectionTriggerApplication -Name our-office-net –ApplicationID C:\Windows\System32\notepad.exe
Add-VpnConnectionTriggerDnsConfiguration -ConnectionName our-office-net -DnsSuffix my.internal.domain -DnsIPAddress 127.0.0.1
Remove-VpnConnectionTriggerDnsConfiguration -ConnectionName our-office-net -DnsSuffix my.internal.domain

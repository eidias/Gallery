#
# Dns.ps1
#
Exit

#Google operates public DNS servers under 8.8.8.8 and 8.8.4.4
Resolve-DnsName -NoHostsFile -DnsOnly -Name "www.google.ch" -Type A -Server 8.8.8.8



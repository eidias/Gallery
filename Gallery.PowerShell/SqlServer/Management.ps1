#
# Management.ps1
#
# Skeleton to perform tasks on SQL Server using SMO.
Exit

[System.Reflection.Assembly]::LoadWithPartialName("Microsoft.SqlServer.ConnectionInfo")
[System.Reflection.Assembly]::LoadWithPartialName("Microsoft.SqlServer.Smo")
[System.Reflection.Assembly]::LoadWithPartialName("Microsoft.SqlServer.SmoExtended")

$serverConnection = New-Object Microsoft.SqlServer.Management.Common.ServerConnection $env:COMPUTERNAME
$serverConnection.StatementTimeout = 0
$server = New-Object -TypeName Microsoft.SqlServer.Management.Smo.Server $serverConnection

$server.Logins | Where-Object { $_.Name -like "IIS APPPOOL\*" } | Select Name, LoginType | Format-Table

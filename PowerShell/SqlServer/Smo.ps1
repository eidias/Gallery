#
# Smo.ps1
#
# Perform various tasks on SQL Server using Server Management Objects (SMO).
Exit

[System.Reflection.Assembly]::LoadWithPartialName("Microsoft.SqlServer.ConnectionInfo")
[System.Reflection.Assembly]::LoadWithPartialName("Microsoft.SqlServer.Smo")
[System.Reflection.Assembly]::LoadWithPartialName("Microsoft.SqlServer.SmoExtended")

$serverConnection = New-Object Microsoft.SqlServer.Management.Common.ServerConnection $env:COMPUTERNAME
$serverConnection.StatementTimeout = 0
$server = New-Object -TypeName Microsoft.SqlServer.Management.Smo.Server $serverConnection


# Query logins based on various criteria.
$server.Logins | Where-Object { $_.Name -like "IIS APPPOOL\*" } | Select Name, LoginType | Format-Table


# Execute a non-query against the server itself.
$query = @"

IF NOT EXISTS(SELECT principal_id FROM sys.server_principals WHERE name = N'IIS APPPOOL\myapppool')
BEGIN
    CREATE LOGIN [IIS APPPOOL\myapppool] FROM WINDOWS
END
GO

EXEC sp_changedbowner [IIS APPPOOL\myapppool]
GO

"@
$server.ConnectionContext.ExecuteNonQuery($query)


# Execute a query with results.
$database = $server.Databases["master"]
$table = $database.ExecuteWithResults("SELECT * FROM sys.server_principals").Tables[0] | Out-GridView

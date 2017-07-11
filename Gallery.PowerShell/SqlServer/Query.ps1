#
# Query.ps1
#
# Executes a query against the local server SQL Server instance.
Exit

[System.Reflection.Assembly]::LoadWithPartialName("Microsoft.SqlServer.ConnectionInfo")
[System.Reflection.Assembly]::LoadWithPartialName("Microsoft.SqlServer.Smo")
[System.Reflection.Assembly]::LoadWithPartialName("Microsoft.SqlServer.SmoExtended")

$serverConnection = New-Object Microsoft.SqlServer.Management.Common.ServerConnection $env:COMPUTERNAME
$serverConnection.StatementTimeout = 0
$server = New-Object -TypeName Microsoft.SqlServer.Management.Smo.Server $serverConnection

# Execute with results
$database = $server.Databases["master"]
$table = $database.ExecuteWithResults("SELECT * FROM sys.server_principals").Tables[0] | Out-GridView

# Execute non-query
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

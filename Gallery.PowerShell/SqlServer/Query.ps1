#
# Query.ps1
#
# Executes a query against the local server SQL Server instance
# This file can be used to do the initial security configuration of the customer database

[System.Reflection.Assembly]::LoadWithPartialName("Microsoft.SqlServer.ConnectionInfo")
[System.Reflection.Assembly]::LoadWithPartialName("Microsoft.SqlServer.Smo")
[System.Reflection.Assembly]::LoadWithPartialName("Microsoft.SqlServer.SmoExtended")

$serverConnection = New-Object Microsoft.SqlServer.Management.Common.ServerConnection $env:COMPUTERNAME
$serverConnection.StatementTimeout = 0
$server = New-Object -TypeName Microsoft.SqlServer.Management.Smo.Server $serverConnection

function Execute-Query
{
    [CmdletBinding()]
    Param(
        [Parameter(ValueFromPipeline=$true)]
        $query
    )
    Process
    {
        $server.ConnectionContext.ExecuteNonQuery($query)
    }
}

@"

IF NOT EXISTS(SELECT principal_id FROM sys.server_principals WHERE name = N'IIS APPPOOL\myapppool')
BEGIN
    CREATE LOGIN [IIS APPPOOL\myapppool] FROM WINDOWS
END
GO

ALTER DATABASE [mydatabase] SET AUTO_CLOSE OFF
GO

USE [mydatabase]
IF EXISTS (SELECT * FROM sys.database_principals WHERE name = N'IIS APPPOOL\myapppool')
BEGIN
    DROP USER [IIS APPPOOL\myapppool]
END
GO

EXEC sp_changedbowner [IIS APPPOOL\myapppool]
GO

"@ | Execute-Query
#
# Query.ps1
#
# Execute a query against a database without a dependencies to SQL Server Management Objects (SMO)
Exit

function Execute-Command
{
    [CmdletBinding()]
    param(
        [Parameter(ValueFromPipeline=$true)]
        [string]$commandText
    )
    process
    {
        $connectionStringBuilder = New-Object -TypeName System.Data.SqlClient.SqlConnectionStringBuilder
        $connectionStringBuilder.psbase.ApplicationName = "Windows PowerShell ISE"
        $connectionStringBuilder.psbase.DataSource = "(local)"
        $connectionStringBuilder.psbase.InitialCatalog = "master"
        $connectionStringBuilder.psbase.IntegratedSecurity = $true

        $sqlConnection = New-Object -TypeName System.Data.SqlClient.SqlConnection
        $sqlConnection.ConnectionString = $connectionStringBuilder.ConnectionString
        $sqlConnection.Open()

        $sqlCmd = New-Object -TypeName System.Data.SqlClient.SqlCommand
        $sqlCmd.Connection = $sqlConnection
        $sqlCmd.CommandText = $commandText

        $sqlReader = $sqlCmd.ExecuteReader()
        if ($sqlReader.HasRows)
        {
            $dataTable = New-Object -TypeName System.Data.DataTable
            $dataTable.Load($sqlReader);
            return $dataTable
        }
        $sqlReader.Close();
        $sqlConnection.Close()
    }
}

@"
            
SELECT * FROM sys.server_principals

"@ | Execute-Command | Out-GridView

#
# Backup.ps1
#
# Backup all non-system databases to the default backup directory using the database name as filename.
# TODO: Create a function and allow using -Force to also backup system databases
Exit

[System.Reflection.Assembly]::LoadWithPartialName("Microsoft.SqlServer.Smo")
[System.Reflection.Assembly]::LoadWithPartialName("Microsoft.SqlServer.SmoExtended")
[System.Reflection.Assembly]::LoadWithPartialName("Microsoft.SqlServer.ConnectionInfo")

$serverConnection = New-Object Microsoft.SqlServer.Management.Common.ServerConnection $env:COMPUTERNAME
$serverConnection.StatementTimeout = 0
$server = New-Object -TypeName Microsoft.SqlServer.Management.Smo.Server $serverConnection

#Gets the number of the current weekday i.e. 1 for Monday and could be used as part of the backup filename.
$dayofweek = (Get-Date).DayOfWeek.value__

foreach ($database in $server.Databases)
{   
    if($database.IsSystemObject -eq $false)           
    {            
        $backup = New-Object -TypeName Microsoft.SqlServer.Management.Smo.Backup       
        $backup.Action = "Database"
        $backup.Database = $database.Name
        $backup.CopyOnly = $true
        #If you specify only the file name or a relative path when you are backing up to a file, the backup file is put in the default backup directory.
        $backup.Devices.AddDevice($database.Name + ".bak", "File")
        $backup.Initialize = $true 
	    $backup.SqlBackup($server)
    }        
}

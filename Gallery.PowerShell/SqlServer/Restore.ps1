#
# Restore.ps1
#
# Restores a database from file to the local SQL Server incl. relocation of all data files
Exit

[System.Reflection.Assembly]::LoadWithPartialName("Microsoft.SqlServer.ConnectionInfo")
[System.Reflection.Assembly]::LoadWithPartialName("Microsoft.SqlServer.Smo")
[System.Reflection.Assembly]::LoadWithPartialName("Microsoft.SqlServer.SmoExtended")

$serverConnection = New-Object Microsoft.SqlServer.Management.Common.ServerConnection $env:COMPUTERNAME
$serverConnection.StatementTimeout = 0
$server = New-Object -TypeName Microsoft.SqlServer.Management.Smo.Server $serverConnection

function Restore-Database
{
    [CmdletBinding()]
    Param(
        [Parameter(ValueFromPipeline=$true)]
        [string]$backupfileName
    )
    Process
    {
        $restore = New-Object -TypeName Microsoft.SqlServer.Management.Smo.Restore
        $backupDeviceItem = New-Object -TypeName Microsoft.SQLServer.Management.Smo.BackupDeviceItem
        $backupDeviceItem.Name = $backupfileName
        $restore.Devices.Add($backupDeviceItem)
        $databaseName = $restore.ReadBackupHeader($server) | Select-Object -ExpandProperty DatabaseName
        $fileList = $restore.ReadFileList($server)
        foreach ($file in $fileList) 
        {
            $relocateFile = New-Object -TypeName Microsoft.SqlServer.Management.Smo.RelocateFile
            $relocateFile.LogicalFileName = $file.LogicalName
            $physicalName = [System.IO.Path]::GetFileName($file.PhysicalName)
            if ($file.Type -eq "D")
            {
                $relocateFile.PhysicalFileName = Join-Path $server.Settings.DefaultFile $physicalName
            }
            if ($file.Type -eq "L")
            {
                $relocateFile.PhysicalFileName = Join-Path $server.Settings.DefaultLog $physicalName
            }
            if ($file.Type -eq "S")
            {
                $relocateFile.PhysicalFileName = Join-Path $server.Settings.DefaultFile $physicalName
            }
            $relocateFile = $restore.RelocateFiles.Add($relocateFile)
        }
        $restore.Database = $databaseName
        $percentCompleteEventHandler = [Microsoft.SqlServer.Management.Smo.PercentCompleteEventHandler] { 
            Write-Progress -Id 1 -Activity "Restoring database '$databaseName'" -PercentComplete $_.Percent
        }
        $restore.add_PercentComplete($percentCompleteEventHandler)
        $restore.SqlRestore($Server)
        $result = $restore.Devices.Remove($backupDeviceItem)
    }
}

Get-ChildItem $server.Settings.BackupDirectory -Filter "mydatabase.bak" | Restore-Database

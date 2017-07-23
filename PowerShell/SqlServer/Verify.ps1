#
# Verify.ps1
#
# Verify that SQL Server backup files are valid and can be restored
Exit

[System.Reflection.Assembly]::LoadWithPartialName("Microsoft.SqlServer.ConnectionInfo")
[System.Reflection.Assembly]::LoadWithPartialName("Microsoft.SqlServer.Smo")
[System.Reflection.Assembly]::LoadWithPartialName("Microsoft.SqlServer.SmoExtended")

$serverConnection = New-Object Microsoft.SqlServer.Management.Common.ServerConnection $env:COMPUTERNAME
$serverConnection.StatementTimeout = 0
$server = New-Object -TypeName Microsoft.SqlServer.Management.Smo.Server $serverConnection
$restore = New-Object -TypeName Microsoft.SqlServer.Management.Smo.Restore

$backupHeaders = @()
Get-ChildItem $server.Settings.BackupDirectory -Filter *.bak | ForEach {
    $restore.Devices.AddDevice($_.FullName, "File")
    if($restore.SqlVerify($server))
    {
        $backupHeader = $restore.ReadBackupHeader($server)
        $backupHeader | Add-Member -MemberType NoteProperty -Name BackupFile -Value $_.FullName
        $backupHeaders += $backupHeader
    }
    else
    {
        $newName = [System.IO.Path]::ChangeExtension($_.FullName,".nobak")
        Rename-Item -Path $_.FullName -NewName $newName
    }
    $restore.Devices.Clear()
}

$backupHeaders | Select ServerName, DatabaseName, DatabaseCreationDate, BackupSize, BackupStartDate, BackupFinishDate, CompatibilityLevel | Out-GridView

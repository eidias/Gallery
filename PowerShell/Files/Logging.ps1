#
# Logging.ps1
#
# Install Microsoft Log Parser first for the related samples to work
Exit

# Use LogParser to query IIS log files
$query = @"
"SELECT cs(User-Agent) As UserAgent, COUNT(*) as Hits FROM C:\inetpub\logs\logfiles\*
GROUP BY UserAgent 
ORDER BY Hits DESC"
"@
Start-Process "C:\Program Files (x86)\Log Parser 2.2\LogParser.exe" -ArgumentList "-i:W3C $query"


# Use LogParser to query the local eventlog
$query = @"
"SELECT * FROM \\$env:COMPUTERNAME\Application
WHERE Message LIKE '%timeout expired%'"
"@
Start-Process "C:\Program Files (x86)\Log Parser 2.2\LogParser.exe" -ArgumentList "-i:EVT $query"


# Parse IIS log files of the last month and display in grid view without using LogParser
$rows = @()
Get-ChildItem C:\inetpub\logs\logfiles\* -File -Filter *.log | Where-Object -Property LastWriteTime -GT (Get-Date).AddMonths(-1) | ForEach-Object {
    if(!$header)
    {
        $header = (Get-Content -Path $_.FullName -TotalCount 4 | Select -First 1 -Skip 3) -replace '#Fields: ' -split ' '
    }
    $rows += Get-Content $_.FullName | Select-String -Pattern '^#' -NotMatch | ConvertFrom-Csv -Delimiter ' ' -Header $header
}
$rows | Out-GridView


# Parse local eventlog files of the last 7 days and extract everything that has a level of warning or higher.
Get-ChildItem -Path C:\Logs | Where-Object -Property LastWriteTime -GT (Get-Date).AddDays(-7) | ForEach-Object {
    $winEvents += Get-WinEvent -FilterHashTable @{ Path=$file.FullName; Level=1,2,3 } -ErrorAction SilentlyContinue
}
$winEvents | Out-GridView


# Export all local event log files with entries on Sundays (day 0) to evtx files and clear the respective log.
$today = Get-Date -
if((Get-Date).DayOfWeek.value__ -eq 0)
{
    $timestamp = Get-Date -Format yyyyMMdd
    Get-WinEvent -ListLog * | Where-Object { $_.RecordCount } | ForEach-Object {

        $logName = $_.LogName -replace "/", "_"
        Start-Process Wevtutil.exe -ArgumentList "cl $_.LogName /bu:C:\Logs\$logName_$timestamp.evtx" -Wait
    }
}

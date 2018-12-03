#
# Monitoring.ps1
#
# Monitoring clients and servers for various parameters
Exit


# Get the current CPU load
Get-WmiObject -Class win32_processor | Measure-Object -Property LoadPercentage -Average | Select-Object -ExpandProperty Average | Write-Host


# Get the current memory load
$operatingsystem = Get-WmiObject -Class win32_operatingsystem
($operatingsystem.TotalVisibleMemorySize - $operatingsystem.FreePhysicalMemory) / $operatingsystem.TotalVisibleMemorySize * 100 | Write-Host


# Show last 10 errors
Get-EventLog -LogName System -EntryType Error -Newest 10 | Select-Object -ExpandProperty Message | Write-Host


#IIS
Import-Module WebAdministration
Get-ChildItem -Path IIS:\Sites


# Windows update
Get-WULastInstallationDate
Start-WUScan
Install-WUUpdates

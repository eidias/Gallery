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

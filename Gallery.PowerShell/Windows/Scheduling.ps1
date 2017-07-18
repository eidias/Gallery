#
# Scheduling.ps1
#
# Creating and managing scheduled tasks and jobs
Exit

# Create a scheduled job in the standard Windows task manager system.
$trigger = New-ScheduledTaskTrigger -Daily -At "00:00"
$action = New-ScheduledTaskAction -Execute "C:\Temp\MyTask.cmd"
Register-ScheduledTask -TaskName "My Scheduled Task" -Trigger $trigger -User "NT AUTHORITY\SYSTEM" –Action $action


# Register a scheduled that runs every 5 minutes from now on.
Register-ScheduledJob -Name "My Scheduled Job" -FilePath C:\Temp\MyScheduledJob.ps1 -RunEvery 5 -RunNow


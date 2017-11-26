#
# Template.ps1
#
# Structures to use in new PowerShell scripts
Exit

# Array initialization.
$array = @()


# Type initialization
$type = @{ Prop1 = ""; Prop2 = ""}


# Skeleton for a new PowerShell function
function FunctionWithBinding
{
    [CmdletBinding()]
    param(
        [Parameter(ValueFromPipeline=$true)]
        [string]$myValue,
        [Parameter()]
        [switch]$myOption
    )
    # Keywords should be written in lowercase
    process
    {
        }
}


# Use the Windows Powershell log for logging events generated in the context of the scripts.
Write-EventLog -LogName "Windows Powershell" -Source "Powershell" -Message "This is a sample error." -EventId 0 -EntryType Error

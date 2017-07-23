#
# Settings.ps1
#
# Settings and profile related tasks
Exit

# Create a settings structure and store it to a local file.
$settings = @{}
$settings.UserName = "arthur.dent"
$settings.Credential = Get-Credential -UserName $settings.UserName
$settings.TheAnswer = 42

Export-Clixml -InputObject $settings -Path C:\Temp\Settings.clixml


# Read a settings file and access its properties.
$settings = Import-Clixml -Path C:\Temp\Settings.clixml -ErrorAction Stop
$theAnswer = 42



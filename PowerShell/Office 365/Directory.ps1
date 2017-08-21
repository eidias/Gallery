#
# Directory.ps1
#
# Various scripts to handle tasks related to Azure Active Directory
Exit

# Requires Office 2016 or the Microsoft Online Services Sign-In Assistant to be installed -> https://www.microsoft.com/en-US/download/details.aspx?id=41950
Import-Module MSOnline
Connect-MsolService

# Display all properties of a given user
Get-MsolUser -UserPrincipalName user@example.com | Format-List

# Set a users password to never expire
Set-MsolUser -UserPrincipalName user@example.com -PasswordNeverExpires $true

# Set the passwords of all users to never expire
Get-MsolUser | Set-MsolUser -PasswordNeverExpires $true

# Display the details for configured strong authentication
Get-MsolUser -UserPrincipalName user@example.com | Select -Expand StrongAuthenticationUserDetails

# Remove alternate email address and mobile phone number used for recovery
Set-MsolUser –UserPrincipalName user@example.com -AlternateEmailAddresses @()
Set-MsolUser –UserPrincipalName user@example.com -MobilePhone @()

# Ensure any open connections are closed before exiting
Remove-Module MsOnline


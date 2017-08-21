#
# Exchange.ps1
#
# Various scripts to handle Exchange online tasks
Exit

$session = New-PSSession -ConfigurationName Microsoft.Exchange -ConnectionUri https://outlook.office365.com/powershell-liveid -Credential Get-Credential -Authentication Basic -AllowRedirection
Import-PSSession $session

# Modify "SendAs" permission on shared mailbox
Add-MailboxPermission "Support" -User support@example.com -Accessright FullAccess
Add-RecipientPermission info@example.com -AccessRights SendAs -Trustee support@example.com

# Change mailbox language
Set-MailboxRegionalConfiguration -Identity user@example.com -Language EN-EN -LocalizeDefaultFolderName $true


# Ensure any open connections are closed before exiting
Remove-PSSession $session






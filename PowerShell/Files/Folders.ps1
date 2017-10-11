#
# Folders.ps1
#
# Create folders and set ACL
Exit

function Create-FolderWithPermissions
{
    [CmdletBinding()]
    param(
        [Parameter()]
        $path,
		[Parameter()]
        $username
    )
    process
    {
		New-Item -Type Directory -Path $path
		$acl = Get-Acl $path
		$fileSystemAccessRule = New-Object System.Security.AccessControl.FileSystemAccessRule($username, "Modify", "Allow")
		$acl.SetAccessRule($fileSystemAccessRule)
		Set-Acl path $acl
	}
}

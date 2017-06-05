#
# FtpHelper.ps1
#

$typeDefinition = @"
using System;
using System.Net;
public class FtpClient : WebClient
{
    protected override WebRequest GetWebRequest(Uri address)
    {
        FtpWebRequest ftpWebRequest = base.GetWebRequest(address) as FtpWebRequest;
        ftpWebRequest.EnableSsl = true;
        return ftpWebRequest;
    }
}
"@
Add-Type -TypeDefinition $typeDefinition
$ftpClient = New-Object FtpClient
$ftpClient.Credentials = New-Object System.Net.NetworkCredential("user", "password")
Get-ChildItem "source path" -Filter *.zip | Foreach-Object {
	$ftpClient.UploadFile("ftp://ftp.server.name/$_.Name", "STOR", $_.FullName)
}



#
# EncodeBase64.ps1
#
# Encode or decode a file to/from a base64 string
Exit

function Encode-ToBase64
{
    [CmdletBinding()]
    param(
        [Parameter(ValueFromPipeline=$true)]
        $fileItem
    )
    process
    {
        $bytes = Get-Content -Encoding Byte -Path $fileItem.FullName
        return [Convert]::ToBase64String($bytes)
   }
}
Get-ChildItem C:\Temp -Filter *.dll -Recurse | Encode-ToBase64


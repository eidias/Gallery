#
# ComputeFileHash.ps1
#
# Compute a SHA256 hash on a given set of files. For PowerShell 4.0 and above use Get-FileHash instead.
Exit

function Compute-FileHash
{
    [CmdletBinding()]
    Param(
        [Parameter(ValueFromPipeline=$true)]
        $fileItem
    )
    Process
    {
        $cryptoServiceProvider = New-Object -TypeName System.Security.Cryptography.SHA256CryptoServiceProvider
        $stream = [System.IO.File]::OpenRead($fileItem.FullName)
        try
        {
            $hash = $cryptoServiceProvider.ComputeHash($stream)
        }
        finally
        {
            $stream.Dispose()
        }
        return [System.BitConverter]::ToString($hash)
   }
}
Get-ChildItem C:\Temp -Filter *.dll -Recurse | Compute-FileHash

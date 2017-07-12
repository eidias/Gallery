#
# Files.ps1
#
# Mostly one liners for file related tasks
Exit

# Identify libraries that are not digitally signed.
Get-ChildItem C:\Temp -Filter *.dll | Get-AuthenticodeSignature | Where-Object { $_.Status -eq "NotSigned" }

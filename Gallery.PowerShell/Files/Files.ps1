#
# Files.ps1
#
# Mostly one liners for file related tasks
Exit

# Identify libraries that are not digitally signed.
Get-ChildItem C:\Temp -Filter *.dll | Get-AuthenticodeSignature | Where-Object { $_.Status -eq "NotSigned" }


#Displays all assembly versions from a given source path.
Get-ChildItem -Path C:\Temp -Include *.dll, *.exe -Recurse | Select-Object -ExpandProperty VersionInfo


# Handle locked files using handle.exe from sysinternals.
Start-BitsTransfer https://download.sysinternals.com/files/Handle.zip -Destination "$env:USERPROFILE\Downloads"
Start-Process handle64.exe -ArgumentList "-p 123" -Wait


# Find duplicate files recursively using SHA256 hashes and display only the duplicates.
Get-ChildItem C:\Users\h0r41\Documents\Scripts -File -Recurse | Get-FileHash | Group-Object -Property Hash | Where-Object { $_.Count -gt 1 } | ForEach-Object { $_.Group | Select-Object -Skip 1 }


# Display files based on their last access time.
Get-ChildItem C:\Temp -File -Recurse | Sort-Object -Property LastAccessTime -Descending | Select Name, LastAccessTime


# Extract archive files and delete corresponding source file.
Get-ChildItem -Path "$env:USERPROFILE\Downloads" -Filter *.zip -File -Recurse | ForEach { 
    Expand-Archive -Path $_.FullName -DestinationPath C:\Temp -Force
    Remove-Item -Path $_.FullName
}



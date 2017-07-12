#
# Signing.ps1
#
# Script functions related to authenticode signatures
Exit

# Show all certs that can be used for authenticode signing.
Get-ChildItem Cert:\CurrentUser\My -CodeSigningCert

# When signing a PowerShell script it needs to be UTF-8 encoded.
# Use the timestamping server provided by the same CA as the certifcate was issued by.
Set-AuthenticodeSignature -FilePath C:\Temp\Script.ps1 -Certificate Cert:\CurrentUser\My\02FAF3E291435468607857694DF5E45B68851868 -TimestampServer http://timestamp.comodoca.com

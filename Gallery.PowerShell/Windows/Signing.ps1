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


<# Create a self-signed certificate.
- Marks the generated private key as exportable. This allows the private key to be included in the certificate.
- Certificate name. This name must conform to the X.500 standard. The simplest method is to specify the name in double quotes, preceded by CN=; for example, -n "CN=myName".
- Certificate store name that stores the output certificate ("My" exists for current user and for local machine)
- Certificate store location. location can be either "currentuser" (the default) or "localmachine".
- Specifies the signature algorithm. algorithm must be md5, sha1 (the default), sha256, sha384, or sha512.
- CryptoAPI provider name, which must be defined in the registry subkeys of HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Cryptography\Defaults\Provider. 
  -> 24 = "Microsoft Enhanced RSA and AES Cryptographic Provider"
- Inserts a list of comma-separated, enhanced key usage object identifiers (OIDs) into the certificate.
- Specifies the generated key length, in bits.
#>
New-SelfSignedCertificate -DnsName "www.fabrikam.com", "www.contoso.com" -CertStoreLocation Cert:\LocalMachine\My

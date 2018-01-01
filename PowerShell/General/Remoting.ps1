#
# Remoting.ps1
#
# Control remote systems using WinRM over HTTPS
Exit


# Enable WSMan over HTTPS on the server side
New-NetFirewallRule -DisplayName "WinRM HTTPS" -Profile Any -Action Allow -Direction Inbound -Protocol TCP -LocalPort 5986    
$certificate = New-SelfSignedCertificate -DnsName host.example.com -NotAfter (Get-Date).AddYears(5)
New-WSManInstance -ResourceURI winrm/config/Listener -SelectorSet @{Address="*";Transport="HTTPS"} -ValueSet @{CertificateThumbprint="$($certificate.Thumbprint)"}


# Create persistent PS session
$sessionOption = New-PSSessionOption -SkipCACheck
$credential = Get-Credential -Message "Enter credentials to access this system" -UserName MyUser
$session = New-PSSession -ComputerName host.example.com -Credential $credential -UseSSL -SessionOption $sessionOption


# Switch to session on remote system
Enter-PSSession $session


# Copy files to a remote system
Copy-Item -Path C:\Temp\Test.txt -Destination C:\Windows\Temp\Test.txt -ToSession $session


# Copy files from a remote system
Copy-Item -Path C:\Windows\Temp\Test.txt -Destination C:\Temp\Test.txt -FromSession $session


# Disconnect remote session
Exit-PSSession

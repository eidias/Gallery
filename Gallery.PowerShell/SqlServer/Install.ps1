#
# Install.ps1
#
# Download SQL Server installer and perform installation according to a generated config file.
Exit

$location = "$env:USERPROFILE\Downloads"
Set-Location $location


#SQL Server 2016
@"
[OPTIONS]
ACTION="Install"
FEATURES=SQLENGINE
INSTANCENAME="MSSQLSERVER"
SQLCOLLATION="Latin1_General_CI_AS"
SQLSVCACCOUNT="NT Service\MSSQLSERVER"
FILESTREAMSHARENAME="MSSQLSERVER"
FILESTREAMLEVEL="2"
"@ | Out-File -FilePath .\SQLServer2016-SSEI-Expr.ini -Force

Start-BitsTransfer http://download.microsoft.com/download/B/F/2/BF2EDBB8-004D-47F3-AA2B-FEA897591599/SQLServer2016-SSEI-Expr.exe
Start-Process .\SQLServer2016-SSEI-Expr.exe -ArgumentList "/ConfigurationFile=.\SQLServer2016-SSEI-Expr.ini /IAcceptSqlServerLicenseTerms /MediaPath=$location\SQLServer2016-SSEI-Expr" -Wait


#SQL Server 2014 with Service Pack 1
@"
[OPTIONS]
IACCEPTSQLSERVERLICENSETERMS="True"
ACTION="Install"
QUIETSIMPLE="True"
FEATURES="SQLENGINE, REPLICATION, SNAC_SDK"
INSTANCENAME="MSSQLSERVER"
SQLSVCACCOUNT="NT Service\MSSQLSERVER"
USEMICROSOFTUPDATE="True"
FILESTREAMSHARENAME="MSSQLSERVER"
FILESTREAMLEVEL="2"
"@ | Out-File -FilePath .\SQLEXPR_x64_ENU.ini -Force

Start-BitsTransfer https://download.microsoft.com/download/1/5/6/156992E6-F7C7-4E55-833D-249BD2348138/ENU/x64/SQLEXPR_x64_ENU.exe
Start-Process .\SQLEXPR_x64_ENU.exe -ArgumentList "/u /x:$location\SQLEXPR_x64_ENU" -Wait
Start-Process .\SQLEXPR_x64_ENU\setup.exe -ArgumentList "/ConfigurationFile=.\SQLEXPR_x64_ENU.ini" -Wait

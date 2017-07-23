#
# Users.ps1
#
# Manage user accounts on the local machine. For PowerShell 4.0 or later use built-in cmdlets instead.
Exit

$accountOptions = @{ 
    ACCOUNTDISABLE = 2;
    LOCKOUT = 16;
    PASSWD_CANT_CHANGE = 64;
    NORMAL_ACCOUNT = 512;
    DONT_EXPIRE_PASSWD = 65536
}

$adsi = [ADSI]"WinNT://$env:COMPUTERNAME"

function Create-User($userName, $password)
{
    $user = $adsi.Create("user", $userName)
    $user.SetPassword($password)
    $user.UserFlags = $user.UserFlags.Value -bor $accountOptions.PASSWD_CANT_CHANGE 
    $user.UserFlags = $user.UserFlags.Value -bor $accountOptions.DONT_EXPIRE_PASSWD
    $user.SetInfo()
}

function Delete-User($userName)
{
    $adsi.Delete("User",$userName)
}

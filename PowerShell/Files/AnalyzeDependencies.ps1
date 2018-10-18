#
# AnalyzeDependencies.ps1
#
# Check assembly versions and dependencies


# List DLLs with all their dependencies
$dependencies = @()
Get-ChildItem -Path "C:\Users\h0r41\Documents\Visual Studio 2017\Projects\Acme\Acme.Web\bin" -File -Filter *.dll -Recurse | % {
    $assembly  = [Reflection.Assembly]::LoadFile($_.FullName)
    $assembly.GetReferencedAssemblies() | % {
        $dependency = @{}
        $dependency.Assembly = $assembly.FullName
        $dependency.Reference = $_.Name
        $dependency.Version = $_.Version
        $dependencies += New-Object -TypeName PSObject -Prop $dependency

    }
}
$dependencies | Out-GridView

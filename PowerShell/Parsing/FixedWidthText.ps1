#
# FixedWidthText.ps1
#
# Parse fixed length text file from interbank into PSObject Array
Exit

$uri = "http://www.six-interbank-clearing.com/dam/downloads/bc-bank-master/bcbankenstamm"
$encoding = [System.Text.Encoding]::Default

# Define the columns in the format <Name>=<Width>
$columns = [Ordered]@{"A"=7;"B"=9;"C"=11;"D"=27;"E"=60;"F"=35;"G"=35;"H"=10;"I"=35;"J"=18;"K"=25;"L"=12;"M"=14;}

$rows = @()
$rowTemplate = New-Object -TypeName PSObject -Property $columns

$webRequest = [Net.WebRequest]::Create($uri)
$webResponse = $webRequest.GetResponse()
$responseStream = $webResponse.GetResponseStream()
$reader = New-Object IO.StreamReader($responseStream, $encoding)
$content = $reader.ReadToEnd()
$reader.Close()

foreach($line in $content.Split([Environment]::NewLine))
{
    if($line.Length -eq 0)
    {
        continue
    }
    $row = New-Object -TypeName PSObject
    $position = 0
    foreach($property in $rowTemplate.PsObject.Properties)
    {
        $row | Add-Member -MemberType NoteProperty -Name $property.Name -Value $line.Substring($position, $property.Value).Trim()
        $position += $property.Value
    }
    $rows += $row
}
$rows | Out-GridView


#
# Soap.ps1
#
# Call a SOAP webservice method without proxy classes
Exit

$uri = 'http://www.example.org'
$headers = @{'username'='user';'password'='p@ssword';'SOAPAction'='http://www.w3.org/2003/05/soap-envelope'}
$body = '<?xml version="1.0"?>
        <soap:Envelope xmlns:soap="http://www.w3.org/2003/05/soap-envelope" xmlns:m="http://www.example.org/stock/Manikandan">
        <soap:Header>
        </soap:Header>
        <soap:Body>
            <m:GetStockPrice>
            <m:StockName>GOOGLE</m:StockName>
            </m:GetStockPrice>
        </soap:Body>
        </soap:Envelope>'

# Ensure potential server errors are properly handled
$webRequest = Invoke-WebRequest $uri -Method Post -ContentType 'text/xml' -Headers $headers -Body $body

# With "Select -ExpandProperty" the embedded XML is implicitly reparsed -> solves &gt, &lt issues
[xml]$xml = $webRequest | Select -ExpandProperty Content | Out-String

# Use Save() to ensure the resulting XML is properly written (i.e. avoid inconsistent line endings)
$xml.Save('C:\Temp\Response.xml')

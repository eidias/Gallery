#
# Smtp.ps1
#

function Send-MailMessageDirectly
{
	#To prevent directly sent messages being treated as spam, ensure proper SPF records are in place
	$from = "A Sender<server@server.name>"
	$to = "some@one.com"
	$subject = "Test"
	$body = "This is a test message"
	$mxRecords = Resolve-DnsName -Name $to.Split("@")[1] -Type MX
	Send-MailMessage -SmtpServer $mxrecords[0].NameExchange -From $from -To $to -Subject $subject -Body $body -BodyAsHtml -Encoding UTF8
}

function Send-MailMessageThroughRelay
{
	$from = "A Sender<server@server.name>"
	$to = "some@one.com"
	$subject = "Test"
	$body = "This is a test message"
	$credential = New-Object PSCredential "smtpuser", $(ConvertTo-SecureString "password" -AsPlainText -Force)
	Send-MailMessage -SmtpServer mail.server.name -Port 587 -Credential $credential -From $from -To $to -Subject $subject -Body $body -BodyAsHtml -Encoding UTF8
}

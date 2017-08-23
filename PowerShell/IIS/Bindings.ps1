#
# Bindings.ps1
#
# Handle SSL bindings for self hosted web applications -> https://docs.microsoft.com/en-us/dotnet/framework/wcf/feature-details/how-to-configure-a-port-with-an-ssl-certificate
Exit

#To bind an SSL certificate to a port number
Start-Process netsh -ArgumentList "http add sslcert ipport=0.0.0.0:4242 certhash="000000000000000000000000000000000000000000" appid={00000000-0000-0000-0000-000000000000}" -Wait


#To bind an SSL certificate to a port number and support client certificates
Start-Process netsh -ArgumentList "http add sslcert ipport=0.0.0.0:4242 certhash="000000000000000000000000000000000000000000" appid={00000000-0000-0000-0000-000000000000} clientcertnegotiation=enable" -Wait


#To delete an SSL certificate from a port number
Start-Process netsh -ArgumentList "http delete sslcert ipport=0.0.0.0:4242" -Wait

Command to locally run the app: dotnet run --urls=https://localhost:5001/

Kudu: https://documentdataparser.scm.azurewebsites.net/DebugConsole/?shell=powershell
// Muszę poprawić bo to jest do mojego prywatnego resource na azurze

Curl:
curl --location 'https://documentdataextractorapp.azurewebsites.net/api/FileTransfer/upload' \
--header 'Cookie: ARRAffinity=79e06db539acb57119e709978d2cf1da299e8341753d6f6345007fcab3f69bc5; ARRAffinitySameSite=79e06db539acb57119e709978d2cf1da299e8341753d6f6345007fcab3f69bc5; ARRAffinity=79e06db539acb57119e709978d2cf1da299e8341753d6f6345007fcab3f69bc5; ARRAffinitySameSite=79e06db539acb57119e709978d2cf1da299e8341753d6f6345007fcab3f69bc5' \
--form 'YOUR FILE PATH HERE"'


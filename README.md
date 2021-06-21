"# WebAPITest" 

A gateway's handling Web API developed with .Net 5

## CLI

- Build :	dotnet build
- Test :	dotnet test
- Publish:	dotnet publish -o PATH_TO_DESTINATION_FOLDER

## Automated build
You can use build.cmd batch script to download the code from the repo, build it, run test and publish project into a newly created folder called "WebAPITest_published" that will be ready to be deployed

## Production

To change IP:Port in production build add/modify "urls" property in 'appsettings.json' with
"urls":"http://IP:PORT;https://IP:SSLPORT",

Ej:  "urls":"http://0.0.0.0:6001;https://0.0.0.0:6002",

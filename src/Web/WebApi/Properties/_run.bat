title FileService
set fsServerUrl=http://localhost:9000
set configuration=Development
setx ASPNETCORE_ENVIRONMENT "%configuration%"
cd %configuration%
dotnet WebApi.dll --server.urls=%fsServerUrl% -c %configuration%
pause >nul
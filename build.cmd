git clone https://github.com/skullwarriorsergio/WebAPITest.git WebAPITest
cd WebAPITest
::Get the NuGet packages
dotnet restore
timeout 2
::Build projects
dotnet build
timeout 2
::Run my Unit Tests
dotnet test
timeout 2
::Release
dotnet publish -o "..\WebAPITest_published"
timeout 5
cd ..
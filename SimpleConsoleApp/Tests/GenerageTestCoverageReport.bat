@echo off

:: dotnet tool install -g dotnet-coverage
:: dotnet tool install -g dotnet-reportgenerator-globaltool

dotnet-coverage collect -f xml -o coverage.xml dotnet test ..\ConsoleApp1.sln
reportgenerator -reports:coverage.xml -targetdir:coveragereport -assemblyfilters:+ConsoleApp1*;-*Tests* -filefilters:-*Program.cs
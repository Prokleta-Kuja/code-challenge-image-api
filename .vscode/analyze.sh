dotnet sonarscanner begin /k:"image-api" /d:sonar.host.url=http://sonarqube:9000 /d:sonar.cs.opencover.reportsPaths="../*.Tests/TestResults/*/coverage.opencover.xml"
dotnet build ..
dotnet test .. --collect:"XPlat Code Coverage" -- DataCollectionRunSettings.DataCollectors.DataCollector.Configuration.Format=opencover
dotnet sonarscanner end
rm -r ../*.Tests/TestResults/
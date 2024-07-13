FROM mcr.microsoft.com/dotnet/nightly/sdk:9.0-preview-noble AS build

WORKDIR /app

COPY Commands Emoji Enums Helpers Secrets/Config.cs Program.cs temp EmojsonBot.csproj /app/

RUN dotnet restore
RUN dotnet publish -c Release -o release

FROM mcr.microsoft.com/dotnet/nightly/runtime:9.0-preview-noble
WORKDIR /app
COPY --from=build /app/release .

ENTRYPOINT ["dotnet", "EnvironmentVarTest.dll"]

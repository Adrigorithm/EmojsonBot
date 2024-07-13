FROM mcr.microsoft.com/dotnet/nightly/sdk:9.0-preview-noble AS build

WORKDIR /home/ubuntu/app

COPY EmojsonBot/ .

RUN dotnet restore
RUN dotnet publish -c Release -o release

FROM mcr.microsoft.com/dotnet/nightly/runtime:9.0-preview-noble

WORKDIR /home/ubuntu/app

COPY --from=build /home/ubuntu/app/release .

ENTRYPOINT ["dotnet", "EmojsonBot.dll"]

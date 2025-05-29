FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build-stage
WORKDIR /source

COPY . .

WORKDIR /source/NetSubs.ApiService

RUN dotnet restore
RUN dotnet publish -c Release -o /app

RUN dotnet tool install --global dotnet-ef

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime-stage

WORKDIR /app
COPY --from=build-stage /app .

EXPOSE 8080
ENTRYPOINT ["dotnet", "NetSubs.ApiService.dll"]

# build e publish
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["PlantWatering.sln", "./"]
COPY ["PlantWatering.Api/PlantWatering.Api.csproj", "PlantWatering.Api/"]

RUN dotnet restore "PlantWatering.Api/PlantWatering.Api.csproj"

COPY . .

WORKDIR "/src/PlantWatering.Api"
RUN dotnet publish "PlantWatering.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

# config variaveis de ambiente
ENV ASPNETCORE_HTTP_PORTS=8080
ENV ASPNETCORE_ENVIRONMENT=Production

EXPOSE 8080

# copia binarios compilados anteriormente
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "PlantWatering.Api.dll"]

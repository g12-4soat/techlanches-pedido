FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Adapter/Driven/TechLanches.Adapter.AWS.SecretsManager/TechLanches.Adapter.AWS.SecretsManager.csproj", "TechLanches.Adapter.AWS.SecretsManager/"]
COPY ["Adapter/Driven/TechLanches.Adapter.SqlServer/TechLanches.Adapter.SqlServer.csproj", "TechLanches.Adapter.SqlServer/"]
COPY ["Core/TechLanches.Domain/TechLanches.Domain.csproj", "TechLanches.Domain/"]
COPY ["Core/TechLanches.Core/TechLanches.Core.csproj", "TechLanches.Core/"]
COPY ["Core/TechLanches.Application/TechLanches.Application.csproj", "TechLanches.Application/"]
COPY ["TechLanches.Pedido.API/TechLanches.Pedido.API.csproj", "TechLanches.Pedido.API/"]
RUN dotnet restore "TechLanches.Pedido.API/TechLanches.Pedido.API.csproj"
COPY . .
WORKDIR "/src/TechLanches.Pedido.API"
RUN dotnet build "TechLanches.Pedido.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TechLanches.Pedido.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TechLanches.Pedido.API.dll"]
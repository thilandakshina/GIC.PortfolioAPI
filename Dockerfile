# Use the official .NET images
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["src/PortfolioAPI.WebApi/PortfolioAPI.WebApi.csproj", "src/PortfolioAPI.WebApi/"]
COPY ["src/PortfolioAPI.Application/PortfolioAPI.Application.csproj", "src/PortfolioAPI.Application/"]
COPY ["src/PortfolioAPI.Infrastructure/PortfolioAPI.Infrastructure.csproj", "src/PortfolioAPI.Infrastructure/"]
COPY ["src/PortfolioAPI.Domain/PortfolioAPI.Domain.csproj", "src/PortfolioAPI.Domain/"]
RUN dotnet restore "src/PortfolioAPI.WebApi/PortfolioAPI.WebApi.csproj"
COPY . .
WORKDIR "/src/src/PortfolioAPI.WebApi"
RUN dotnet build "PortfolioAPI.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PortfolioAPI.WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PortfolioAPI.WebApi.dll"] 
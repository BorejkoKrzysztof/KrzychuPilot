# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
RUN apt-get update && apt-get install -y netcat-openbsd && rm -rf /var/lib/apt/lists/*
USER $APP_UID
EXPOSE 8080


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["KrzychuPilot.API/KrzychuPilot.API.csproj", "KrzychuPilot.API/"]
COPY ["KrzychuPilot.Application/KrzychuPilot.Application.csproj", "KrzychuPilot.Application/"]
COPY ["KrzychuPilot.Domain/KrzychuPilot.Domain.csproj", "KrzychuPilot.Domain/"]
COPY ["KrzychuPilot.Infrastructure/KrzychuPilot.Infrastructure.csproj", "KrzychuPilot.Infrastructure/"]
RUN dotnet restore "./KrzychuPilot.API/KrzychuPilot.API.csproj"
COPY . .
WORKDIR "/src/KrzychuPilot.API"
RUN dotnet build "./KrzychuPilot.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./KrzychuPilot.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY --chmod=0755 ["KrzychuPilot.API/entrypoint.sh", "/entrypoint.sh"]
ENTRYPOINT ["/entrypoint.sh"]
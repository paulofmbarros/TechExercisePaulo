﻿FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

#Stage 1 - Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["PixelService/PixelService.csproj", "PixelService/"]
RUN dotnet restore "PixelService/PixelService.csproj"
COPY . .
WORKDIR "/src/PixelService"
RUN dotnet build "PixelService.csproj" -c $BUILD_CONFIGURATION -o /app/build

#Stage 2 - Publish
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/build .
ENTRYPOINT ["dotnet", "PixelService.dll"]
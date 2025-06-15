FROM mcr.microsoft.com/dotnet/runtime:9.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["tests/Web.AcceptanceTests/Web.AcceptanceTests.csproj", "tests/Web.AcceptanceTests/"]
RUN dotnet restore "tests/Web.AcceptanceTests/Web.AcceptanceTests.csproj"
COPY . .
WORKDIR "/src/tests/Web.AcceptanceTests"
RUN dotnet build "./Web.AcceptanceTests.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Web.AcceptanceTests.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/playwright/dotnet:v1.52.0-noble
# DOTNET 9 not supported, install it manually
# https://github.com/microsoft/playwright-dotnet/pull/3077#issuecomment-2532370021
RUN curl -sSL https://dot.net/v1/dotnet-install.sh | bash -s -- --install-dir /usr/share/dotnet --channel 9.0
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet","test", "Web.AcceptanceTests.dll"]

ARG VERSION=1.0.0

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /work
COPY *.sln ./
COPY *.props ./
COPY src/*/*.csproj ./
RUN for file in $(ls *.csproj); do mkdir -p ./src/${file%.*}/ && mv $file ./src/${file%.*}/; done
COPY tests/*/*.csproj ./
RUN for file in $(ls *.csproj); do mkdir -p ./tests/${file%.*}/ && mv $file ./tests/${file%.*}/; done
RUN dotnet restore
COPY . .
RUN dotnet build --no-restore -c Release

FROM build AS tests

FROM build AS publish
ARG VERSION
WORKDIR /work/src/Web
RUN dotnet publish "Web.csproj" -c Release -o /app/publish /p:Version=$VERSION

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Web.dll"]

FROM mcr.microsoft.com/dotnet/sdk:8.0-nanoserver-ltsc2022 AS build-env
WORKDIR /App

COPY Directory.Packages.props ./
COPY src/news-mixer/ ./
RUN dotnet restore
RUN dotnet publish code/news-mixer.csproj -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0-nanoserver-ltsc2022
WORKDIR /App
COPY --from=build-env /App/out .
ENTRYPOINT ["dotnet", "news-mixer.dll"]

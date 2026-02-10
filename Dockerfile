# 1. Gunakan SDK versi 10 (Preview/RC/Stable di tahun 2026)
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy file csproj dan restore
COPY ["SiperuBackend.csproj", "./"]
RUN dotnet restore "SiperuBackend.csproj"

# Copy sisa file dan build
COPY . .
RUN dotnet publish "SiperuBackend.csproj" -c Release -o /app/publish

# 2. Gunakan Runtime versi 10 juga
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "SiperuBackend.dll"]
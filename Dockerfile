# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy project files
COPY AppSettingsConverter/AppSettingsConverter.csproj AppSettingsConverter/
RUN dotnet restore AppSettingsConverter/AppSettingsConverter.csproj

# Copy source code and build
COPY AppSettingsConverter/ AppSettingsConverter/
WORKDIR /src/AppSettingsConverter
RUN dotnet build -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/runtime:10.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "AppSettingsConverter.dll"]


# Use the official .NET 8.0 runtime image as the base image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5000
EXPOSE 5001

# Use the official .NET 8.0 SDK image for building
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy the project files
COPY ["LightHouseWebApi/LightHouseWebApi.csproj", "LightHouseWebApi/"]
COPY ["LightHouseApplication/LightHouseApplication.csproj", "LightHouseApplication/"]
COPY ["LightHouseInfrastructure/LightHouseInfrastructure.csproj", "LightHouseInfrastructure/"]
COPY ["LightHouseData/LightHouseData.csproj", "LightHouseData/"]
COPY ["LightHouseDomain/LightHouseDomain.csproj", "LightHouseDomain/"]

# Restore dependencies
RUN dotnet restore "LightHouseWebApi/LightHouseWebApi.csproj"

# Copy the rest of the source code
COPY . .

# Build the application
WORKDIR "/src/LightHouseWebApi"
RUN dotnet build "LightHouseWebApi.csproj" -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish "LightHouseWebApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Final stage/image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Set the entry point
ENTRYPOINT ["dotnet", "LightHouseWebApi.dll"]

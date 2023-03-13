#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore "./oauth2-azure-starter-webapi/oauth2-azure-starter-webapi.csproj" --disable-parallel
RUN dotnet publish "./oauth2-azure-starter-webapi/oauth2-azure-starter-webapi.csproj" -c release -o /app --no-restore

# Serve Stage
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS final
WORKDIR /app
COPY --from=build /app ./

# EXPOSE 80
# EXPOSE 443

ENTRYPOINT ["dotnet", "oauth2-azure-starter-webapi.dll"]
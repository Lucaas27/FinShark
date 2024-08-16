
# This Dockerfile is used to build a container image for the FinShark API.
# It sets up the necessary environment and dependencies to run the API.

# Base image for the container, based on the .NET ASP.NET runtime version 8.0.
# It sets the user to "app" and sets the working directory to /app.
# The container will expose port 7286 for incoming connections.
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 7286

# Build image for the container, based on the .NET SDK version 8.0.
# It sets the build configuration to Release by default.
# The working directory is set to /src.
# The API project is restored and copied into the container.
# The API project is built with the specified build configuration and output directory.
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["api/api.csproj", "api/"]
RUN dotnet restore "api/api.csproj"
COPY . .
WORKDIR "/src/api"
RUN dotnet build "api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Migrations image for running EF Core commands.
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS migrations
WORKDIR /src
COPY ["api/api.csproj", "api/"]
RUN dotnet restore "api/api.csproj"
COPY . .
WORKDIR "/src/api"
RUN dotnet tool install --global dotnet-ef
ENV PATH="$PATH:/root/.dotnet/tools"


# FROM build as migrations
# RUN dotnet tool install --global dotnet-ef
# ENV PATH="$PATH:/root/.dotnet/tools"
# ENTRYPOINT ["dotnet-ef", "database", "update"]

# Publish image for the container, based on the build image.
# It sets the build configuration to Release by default.
# The API project is published with the specified build configuration and output directory.
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final image for the container, based on the base image.
# The working directory is set to /app.
# The published API files are copied into the container.
# The entrypoint is set to run the API using the dotnet command.
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "api.dll"]
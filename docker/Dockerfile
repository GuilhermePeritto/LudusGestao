# Use the official .NET 8.0 runtime as the base image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Use the official .NET 8.0 SDK as the build image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Set environment variables to disable telemetry and first run experience
ENV DOTNET_CLI_TELEMETRY_OPTOUT=1
ENV DOTNET_SKIP_FIRST_TIME_EXPERIENCE=1
ENV DOTNET_NOLOGO=1

# Copy only the necessary files for restore
COPY LudusGestao.sln ./
COPY LudusGestao.API/LudusGestao.API.csproj ./LudusGestao.API/
COPY LudusGestao.Application/LudusGestao.Application.csproj ./LudusGestao.Application/
COPY LudusGestao.Domain/LudusGestao.Domain.csproj ./LudusGestao.Domain/
COPY LudusGestao.Infrastructure/LudusGestao.Infrastructure.csproj ./LudusGestao.Infrastructure/

# Restore packages
RUN dotnet restore

# Copy source code (excluding obj/bin folders via .dockerignore)
COPY . .

# Build the application
RUN dotnet build -c Release --no-restore

# Publish stage
FROM build AS publish
RUN dotnet publish -c Release -o /app/publish --no-restore

# Final stage
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LudusGestao.API.dll"]

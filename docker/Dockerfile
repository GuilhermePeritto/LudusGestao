# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Clear all caches and set clean environment
RUN dotnet nuget locals all --clear
RUN rm -rf /root/.nuget /root/.local /tmp/NuGetScratch*
ENV NUGET_PACKAGES=/src/packages
ENV DOTNET_SKIP_FIRST_TIME_EXPERIENCE=1
ENV DOTNET_CLI_TELEMETRY_OPTOUT=1

# Copy solution and project files
COPY LudusGestao.sln ./
COPY LudusGestao.API/LudusGestao.API.csproj ./LudusGestao.API/
COPY LudusGestao.Application/LudusGestao.Application.csproj ./LudusGestao.Application/
COPY LudusGestao.Domain/LudusGestao.Domain.csproj ./LudusGestao.Domain/
COPY LudusGestao.Infrastructure/LudusGestao.Infrastructure.csproj ./LudusGestao.Infrastructure/

# Restore dependencies with clean environment
RUN dotnet restore --packages /src/packages --verbosity minimal --force

# Copy source code
COPY . .

# Build the application with clean environment
RUN dotnet build -c Release --no-restore --verbosity minimal --no-cache

# Publish stage
FROM build AS publish
RUN dotnet publish LudusGestao.API/LudusGestao.API.csproj -c Release -o /app/publish --no-restore --no-cache

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=publish /app/publish .

# Expose port (Render usa porta 10000)
EXPOSE 10000

# Set environment variable for port
ENV ASPNETCORE_URLS=http://+:10000

# Set entry point
ENTRYPOINT ["dotnet", "LudusGestao.API.dll"]

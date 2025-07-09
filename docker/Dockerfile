# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Clear NuGet cache and set environment
RUN dotnet nuget locals all --clear
ENV NUGET_PACKAGES=/src/packages

# Copy solution and project files
COPY LudusGestao.sln ./
COPY LudusGestao.API/LudusGestao.API.csproj ./LudusGestao.API/
COPY LudusGestao.Application/LudusGestao.Application.csproj ./LudusGestao.Application/
COPY LudusGestao.Domain/LudusGestao.Domain.csproj ./LudusGestao.Domain/
COPY LudusGestao.Infrastructure/LudusGestao.Infrastructure.csproj ./LudusGestao.Infrastructure/

# Restore dependencies with clean environment
RUN dotnet restore --packages /src/packages --verbosity minimal

# Copy source code
COPY . .

# Build the application
RUN dotnet build -c Release --no-restore --verbosity minimal

# Publish stage
FROM build AS publish
RUN dotnet publish LudusGestao.API/LudusGestao.API.csproj -c Release -o /app/publish --no-restore

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

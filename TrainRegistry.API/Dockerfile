# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy all .csproj files first (optimizes Docker layer caching)
COPY ["TrainRegistry.API/TrainRegistry.API.csproj", "TrainRegistry.API/"]
COPY ["TrainRegistry.Application/TrainRegistry.Application.csproj", "TrainRegistry.Application/"]
COPY ["TrainRegistry.Application.Tests/TrainRegistry.Application.Tests.csproj", "TrainRegistry.Application.Tests/"]
COPY ["TrainRegistry.Domain/TrainRegistry.Domain.csproj", "TrainRegistry.Domain/"]
COPY ["TrainRegistry.Infrastructure/TrainRegistry.Infrastructure.csproj", "TrainRegistry.Infrastructure/"]

# Restore dependencies
RUN dotnet restore "TrainRegistry.API/TrainRegistry.API.csproj"

# Copy everything else and build
COPY . .
WORKDIR "/src/TrainRegistry.API"
RUN dotnet publish -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "TrainRegistry.API.dll"]
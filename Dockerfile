# ============================================================
# Stage 1 : Build
# ============================================================
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copier les fichiers projet pour restaurer les dépendances
COPY ["Flight.Api/Flight.Api.csproj", "Flight.Api/"]
COPY ["Flight.Application/Flight.Application.csproj", "Flight.Application/"]
COPY ["Flight.Domain/Flight.Domain.csproj", "Flight.Domain/"]
COPY ["Flight.Infrastructure/Flight.Infrastructure.csproj", "Flight.Infrastructure/"]

# Restaurer les packages NuGet
RUN dotnet restore "Flight.Api/Flight.Api.csproj"

# Copier tout le code source
COPY . .

# Publier l'application en Release
WORKDIR /src/Flight.Api
RUN dotnet publish "Flight.Api.csproj" \
    -c Release \
    -o /app/publish \
    --no-restore

# ============================================================
# Stage 2 : Runtime
# ============================================================
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app

# Installer curl pour le healthcheck Docker
USER root
RUN apt-get update \
    && apt-get install -y curl \
    && rm -rf /var/lib/apt/lists/*

# Créer un utilisateur non-root
RUN groupadd -r appgroup && useradd -r -g appgroup appuser

# Copier l'application publiée
COPY --from=build /app/publish .

# Variables d'environnement par défaut
ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://+:8080

EXPOSE 8080

# Repasser en utilisateur non-root
USER appuser

ENTRYPOINT ["dotnet", "Flight.Api.dll"]
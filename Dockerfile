# ============================================================
# Stage 1 : Build
# ============================================================
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copier les fichiers projet pour restaurer les dépendances
COPY ["src/Flight.Api/Flight.Api.csproj", "src/Flight.Api/"]
COPY ["src/Flight.Application/Flight.Application.csproj", "src/Flight.Application/"]
COPY ["src/Flight.Domain/Flight.Domain.csproj", "src/Flight.Domain/"]
COPY ["src/Flight.Infrastructure/Flight.Infrastructure.csproj", "src/Flight.Infrastructure/"]

# Restaurer les packages NuGet
RUN dotnet restore "src/Flight.Api/Flight.Api.csproj"

# Copier tout le code source
COPY . .

# Publier l'application en Release
WORKDIR /src/src/Flight.Api
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

# Exécution avec utilisateur non privilégié
USER appuser

ENTRYPOINT ["dotnet", "Flight.Api.dll"]
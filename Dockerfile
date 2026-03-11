# ============================================================
# Stage 1 : Build
# ============================================================
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copier les fichiers de projet pour restaurer les dépendances en cache
COPY ["Flight.Api/Flight.Api.csproj",           "Flight.Api/"]
COPY ["Flight.Application/Flight.Application.csproj", "Flight.Application/"]
COPY ["Flight.CrossCutting/Flight.CrossCutting.csproj", "Flight.CrossCutting/"]
COPY ["Flight.Domain/Flight.Domain.csproj",       "Flight.Domain/"]
COPY ["Flight.Domain.Core/Flight.Domain.Core.csproj", "Flight.Domain.Core/"]
COPY ["Flight.Infrastructure/Flight.Infrastructure.csproj", "Flight.Infrastructure/"]
COPY ["Flight.Util/Flight.Util.csproj",           "Flight.Util/"]

RUN dotnet restore "Flight.Api/Flight.Api.csproj"

# Copier tout le code source
COPY . .

# Publier en mode Release
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

# Sécurité : exécuter en tant qu'utilisateur non-root
RUN addgroup --system appgroup && adduser --system --ingroup appgroup appuser
USER appuser

# Variables d'environnement par défaut
ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://+:8080

EXPOSE 8080

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "Flight.Api.dll"]

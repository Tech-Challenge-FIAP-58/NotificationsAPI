# syntax=docker/dockerfile:1

# =========================
# Base (runtime)
# =========================
FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
WORKDIR /app

# Se N�O estiver usando APP_UID no compose/ambiente, deixe comentado
# USER $APP_UID


# =========================
# Build (restore + build)
# =========================
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# -------------------------
# Copia solu��o (se existir)
# -------------------------
# Remova esta linha se n�o houver .sln no projeto
COPY FCG.Notifications.sln ./

# -------------------------
# Copia apenas o csproj (cache de restore)
# -------------------------
COPY src/FCG.Notifications/FCG.Notifications.csproj src/FCG.Notifications/
COPY src/FCG.Notifications.Domain/FCG.Notifications.Domain.csproj src/FCG.Notifications.Domain/
COPY src/FCG.Notifications.Services/FCG.Notifications.Services.csproj src/FCG.Notifications.Services/
COPY src/FCG.Core/FCG.Core.csproj src/FCG.Core/

# -------------------------
# Restore
# -------------------------
RUN dotnet restore src/FCG.Notifications/FCG.Notifications.csproj

# -------------------------
# Copia o restante do c�digo
# -------------------------
COPY . .

# -------------------------
# Build
# -------------------------
RUN dotnet build src/FCG.Notifications/FCG.Notifications.csproj \
    -c $BUILD_CONFIGURATION \
    -o /app/build


# =========================
# Publish
# =========================
FROM build AS publish
ARG BUILD_CONFIGURATION=Release

RUN dotnet publish src/FCG.Notifications/FCG.Notifications.csproj \
    -c $BUILD_CONFIGURATION \
    -o /app/publish \
    /p:UseAppHost=false


# =========================
# Final (runtime)
# =========================
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FCG.Notifications.dll"]

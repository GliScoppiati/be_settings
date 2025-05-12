#!/bin/bash

# =============================================================================
# Script per generare le migrations iniziali dei microservizi .NET
# ✔️ Deve essere eseguito dalla cartella /main
# ✔️ Genera migration solo se la cartella 'Migrations' non esiste
# ✔️ Tutte le migrations saranno in /main/NomeServizio/Migrations
# ⚠️ NON eseguire 'dotnet ef database update':
#    le migration saranno applicate automaticamente via Docker all'avvio
# =============================================================================

set -e

ROOT_DIR=$(pwd)
MIGRATION_NAME="InitialCreate"

# Lista dei microservizi per cui generare la migration
INCLUDE_SERVICES=(
  "AuthService"
  "CocktailImportService"
  "CocktailService"
  "CocktailSubmissionService"
  "FavoriteCocktailsService"
  "SearchHistoryService"
  "UserProfileService"
)
  #"Gateway"
  #"ImageFetcherService"
  #"SearchService"

for service in "${INCLUDE_SERVICES[@]}"; do
  proj_dir="$ROOT_DIR/$service"
  csproj_path="$proj_dir/$service.csproj"
  migrations_dir="$proj_dir/Migrations"

  if [ ! -f "$csproj_path" ]; then
    echo "❌ Progetto non trovato per $service. Skipping."
    continue
  fi

  if [ -d "$migrations_dir" ]; then
    echo "⏭️  Migration già presente per $service. Skipping."
    continue
  fi

  echo "📦 Generating migration for $service..."

  dotnet ef migrations add "$MIGRATION_NAME" \
    --project "$proj_dir" \
    --startup-project "$proj_dir" \
    --output-dir Migrations
done

echo "✅ Tutte le migration mancanti sono state generate."
echo ""
echo "!!!! IMPORTANTE !!!!"
echo "Ambiente setuppato per Docker"
echo "NON ESEGUIRE DOTNET EF UPDATE DATABASE"
echo "Automatico al docker compose up"
echo "!!!! IMPORTANTE !!!!"

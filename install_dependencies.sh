#!/bin/bash
set -euo pipefail

# ================================================================
# Script di setup per ambiente di sviluppo locale su Ubuntu 24.04
# Installa: .NET SDK 8, client PostgreSQL, Docker + Docker Compose
# Aggiunge l'utente al gruppo docker per evitare 'sudo' nei comandi
# Dopo l'esecuzione, riavvia il terminale o esegui 'newgrp docker'
# ================================================================

# Colori per output leggibile
GREEN="\033[1;32m"
YELLOW="\033[1;33m"
NC="\033[0m" # No Color

info() {
  echo -e "${YELLOW}➡ $1${NC}"
}

success() {
  echo -e "${GREEN}✅ $1${NC}"
}

# 1. Aggiorna pacchetti di base
info "Aggiornamento pacchetti di sistema..."
sudo apt update && sudo apt install -y \
  apt-transport-https \
  curl \
  ca-certificates \
  gnupg \
  lsb-release \
  software-properties-common

# 2. Installa .NET SDK 8
info "Installazione .NET SDK 8..."
DOTNET_PKG="packages-microsoft-prod.deb"
wget -q https://packages.microsoft.com/config/ubuntu/24.04/packages-microsoft-prod.deb -O $DOTNET_PKG
sudo dpkg -i $DOTNET_PKG && rm $DOTNET_PKG

sudo apt update
sudo apt install -y dotnet-sdk-8.0 || {
  echo "❌ Errore durante l'installazione di dotnet-sdk-8.0"
  exit 1
}

# 3. Installa PostgreSQL client
info "Installazione client PostgreSQL..."
sudo apt install -y postgresql-client || {
  echo "❌ Errore durante l'installazione del client PostgreSQL"
  exit 1
}

# 4. Installa Docker e plugin Compose v2 dal repository ufficiale Docker
info "Aggiunta del repository ufficiale Docker..."

sudo mkdir -p /etc/apt/keyrings
curl -fsSL https://download.docker.com/linux/ubuntu/gpg | \
  sudo gpg --dearmor -o /etc/apt/keyrings/docker.gpg

echo \
  "deb [arch=$(dpkg --print-architecture) \
  signed-by=/etc/apt/keyrings/docker.gpg] \
  https://download.docker.com/linux/ubuntu $(lsb_release -cs) stable" | \
  sudo tee /etc/apt/sources.list.d/docker.list > /dev/null

info "Aggiornamento pacchetti e installazione Docker + Compose Plugin..."
sudo apt update
sudo apt install -y docker-ce docker-ce-cli containerd.io docker-buildx-plugin docker-compose-plugin || {
  echo "❌ Errore durante l'installazione di Docker"
  exit 1
}

# 5. Aggiunge l'utente corrente al gruppo docker
info "Aggiunta dell'utente '$USER' al gruppo docker..."
sudo usermod -aG docker $USER

success "Installazione completata!"
echo -e "\nℹ️ Riavvia il terminale o esegui ${YELLOW}'newgrp docker'${NC} per usare Docker senza sudo."


# FinDrink – Backend Settings

Questo repository contiene la configurazione e gli script necessari per avviare e gestire l'infrastruttura backend della webapp **FinDrink**.

## 📁 Struttura del repository

- **Servizi principali**:
  - `AuthService`
  - `CocktailService`
  - `CocktailImportService`
  - `CocktailSubmissionService`
  - `FavoriteCocktailsService`
  - `SearchService`
  - `SearchHistoryService`
  - `UserProfileService`
  - `ImageFetcherService`
  - `Gateway`

- **Configurazioni e script**:
  - `docker-compose.yml` – Orchestrazione dei container
  - `start.sh` – Script di avvio dei servizi
  - `install_dependencies.sh` – Installazione delle dipendenze
  - `generate_migrations.sh` – Generazione delle migrazioni
  - `main.sln` – Soluzione principale
  - `.env.example` – File di esempio per le variabili d'ambiente
  - `prometheus.yml`, `loki-config.yaml`, `promtail-config.yaml` – Configurazioni per il monitoraggio e il logging
  - `_Documentations/` – Documentazione tecnica

## 🚀 Avvio rapido

1. **Clonare il repository**:

   ```bash
   git clone https://github.com/GliScoppiati/be_settings.git main
   cd main
   ```

2. **Configurare le variabili d'ambiente**:

   ```bash
   cp .env.example .env
   # Modificare il file .env con i valori appropriati
   ```

3. **Installare le dipendenze**: 

   ```bash
   ./install_dependencies.sh
   ```

4. **Avviare i servizi**:

   ```bash
   ./start.sh
   ```

## 🛠️ Tecnologie utilizzate

- **Docker & Docker Compose** – Per la containerizzazione e l'orchestrazione dei servizi
- **Prometheus & Loki** – Per il monitoraggio e la raccolta dei log
- **Shell scripting** – Per l'automazione delle operazioni

## 📄 Licenza

Questo progetto è rilasciato sotto licenza MIT. Per maggiori dettagli, consultare il file `LICENSE`.

---

Per ulteriori informazioni, consulta la documentazione nella cartella `_Documentations/`.

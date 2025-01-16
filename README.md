# Windows Service in comunicazione con SQL

Applicazione progettata per essere eseguita come **servizio Windows** il quale inserisce in una tabella in SQL i dati di avvio e arresto. Questi dati vengono anche scritti in un file di log

## Caratteristiche
- Apre una connessione al server SQL con la stringa di connessione
- Inserisce i dati nella tabella di SQL nelle colonne Message e LoggedAt ogni 5 secondi per monitorare l'esecuzione periodica del servizio.
- Scrive le stesse informazioni in un file di log per verifica del funzionamento.
- Gestisce automaticamente la creazione della directory e del file di log se non esistono.
- Secondo servizio per cancellare periodicamente le prime 5 righe della tabella.

## Requisiti
- **.NET Framework 4.0 o superiore**
- Accesso sufficiente al file system per creare directory e file.
- **SQL Server Managment**

## Installazione
1. Compila il progetto in **Visual Studio 2022** o una versione compatibile.
2. Usa lo strumento `InstallUtil.exe` per installare i servizi. Ad esempio:
   ```cmd
   cd C:\Windows\Microsoft.NET\Framework\v4.0.30319
   InstallUtil.exe "PercorsoCompleto\ProvaService.exe"
3. Avvia i servizi tramite **Gestione servizi** di Windows o col comando:
   ```cmd
   sc start ProvaService

## Configurazione
**Creare un db e la tabella in SQL**
Creare un nuovo db in SQL e passare il comando per la creazione della tabella:
  ```sql
   CREATE TABLE ServiceLogs (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Message NVARCHAR(MAX),
    LoggedAt DATETIME
  );
  ```

**Stringa di connessione**
Sostituire con i propri dati server e nome del db la stringa 
   ```cmd
   string connectionString = "Server=nome_server;Database=nome_db;Trusted_Connection=True;";
  ```


**Log Directory**
Il servizio crea automaticamente una directory chiamata Logs nella directory base dell'applicazione. I file di log hanno un nome nel formato:
  ```cmd
  ServiceLog_gg_mm_aaaa.txt
  ```
**Modifica dell'intervallo**
L'intervallo di tempo tra le esecuzioni del metodo periodico è configurato a 5 secondi (5000 ms). Puoi modificarlo nel metodo OnStart:

  ```csharp
  timer.Interval = 5000; // Intervallo in millisecondi
  ```
**File di log e db**
Il servizio scrive i seguenti messaggi nei file di log e della tabella del db:

1. **Avvio del servizio:**
  ```css
  Servizio iniziato alle [data e ora]
  ```
2. **Evento periodico:**
```css
Il servizio è stato richiamato alle [data e ora]
```
3. **Arresto del servizio:**
  ```css
  Il servizio si è fermato alle [data e ora]
  ```
## Metodo principale
**Funzionalità principali:**
- OnStart:
  - Avvia il servizio.
  - Configura un timer per eseguire attività periodiche.

- OnElapsedTime:
  - Eseguito ogni 5 secondi. Registra l'ora corrente nel file di log e nella tabella del db.
  - Eseguito ogni 15 secondi. Cancella le prime 5 righe della tabella precedentemente creata.
- OnStop:
  - Ferma il servizio e registra un messaggio nel file di log e nella tabella del db.

## Disinstallazione
Per disinstallare il servizio, utilizza nuovamente InstallUtil.exe:

``` cmd
InstallUtil.exe /u "PercorsoCompleto\ProvaService.exe"
```

## Note
- Per eseguire le operazioni di installazione/disinstallazione, assicurati di avviare il terminale come Amministratore.
- Controlla i file di log nella directory Logs per verificare il corretto funzionamento del servizio.

## Esempio di Output nel File di Log
```
Servizio iniziato alle 14/01/2025 10:00:00
Il servizio è stato richiamato alle 14/01/2025 10:00:05
Il servizio è stato richiamato alle 14/01/2025 10:00:10
Il servizio si è fermato alle 14/01/2025 10:00:15
```

## Template di Output della Tabella

Esempio di come appare la tabella `service_logs` in SQL:

```sql
Id  | Message                         | LoggedAt
----|---------------------------------|---------------------
1   | Il servizio è stato avviato.    | 2025-01-15 10:00:00
2   | Il servizio è stato richiamato. | 2025-01-15 10:05:00
3   | Il servizio è stato fermato.    | 2025-01-15 10:10:00

## Contatti
Per qualsiasi domanda o supporto, contattami: [vg.giannellivaleria@gmail.com].

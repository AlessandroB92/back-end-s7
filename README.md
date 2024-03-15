# In Forno - Applicazione di Gestione Ordini per Pizzeria

Benvenuto nel repository dell'applicazione **In Forno**! Questa applicazione è stata sviluppata utilizzando ASP.NET MVC 5.2.9 e Entity Framework 6.2.0, insieme alla libreria Bootstrap 5.2.3 per garantire un'esperienza utente moderna e responsiva.

## Descrizione

**In Forno** è un'applicazione di gestione ordini dedicata a una pizzeria o attività simile. L'applicazione offre una serie di funzionalità per gestire gli ordini e semplificare il processo di acquisto per i clienti.

## Funzionalità Principali

### Ruolo Admin

- Accesso sicuro con sistema di login.
- Gestione degli articoli del menu: aggiunta, modifica ed eliminazione.
- Visualizzazione della lista degli ordini pendenti e possibilità di evaderli una volta completati.
- Calcolo del totale degli ordini evasi e dell'incasso totale per una data specifica.

### Ruolo Utente

- Accesso sicuro con sistema di login o registrazione.
- Visualizzazione del catalogo di pizze disponibili per l'acquisto.
- Possibilità di aggiungere le pizze desiderate all'ordine, specificando la quantità.
- Gestione del carrello degli acquisti con opzioni per inserire l'indirizzo di consegna, aggiungere note all'ordine ed eliminare gli articoli selezionati.
- Invio dell'ordine completato per la preparazione.

## Prerequisiti

Prima di poter eseguire il progetto localmente, assicurati di avere installato quanto segue:

- [Visual Studio](https://visualstudio.microsoft.com/it/) con supporto per lo sviluppo di applicazioni web ASP.NET.
- [SQL Server](https://www.microsoft.com/it-it/sql-server/sql-server-downloads) o un'altra versione di database supportata da Entity Framework.

## Come Iniziare

1. Clona il repository sul tuo sistema locale.
2. Apri il progetto utilizzando Visual Studio.
3. Configura la stringa di connessione nel file `Web.config` per collegarti al tuo database locale.
4. Esegui il comando `Update-Database` nella Console di Gestione Pacchetti di Visual Studio per creare il database utilizzando Entity Framework Migration.
5. Avvia l'applicazione e inizia ad esplorare le funzionalità offerte.

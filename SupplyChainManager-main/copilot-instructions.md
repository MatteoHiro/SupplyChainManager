# Istruzioni per GitHub Copilot

## Scopo
- Questo repository contiene un'app ASP.NET Core per la gestione della supply chain (inventario, ordini, fornitori, magazzini).
- Fornisci suggerimenti coerenti con lo stile C# moderno e le convenzioni del progetto.

## Stile di codice
- Linguaggio: C# (target .NET 8 / net8.0).
- Convenzioni: PascalCase per classi e metodi; camelCase per parametri e variabili locali.
- Preferire l'uso di `async`/`await` per operazioni I/O-bound.
- Usare dependency injection e progettare componenti testabili.

## Cosa suggerire
- Implementazioni concise e testabili per `Controllers`, `Services`, `Entities`.
- Metodi `async` che rispettino il `CancellationToken` e gestiscano gli errori.
- Test xUnit per controller e servizi; mocking delle dipendenze (es. `Moq`).
- Miglioramenti per validazione dell'input, gestione degli errori e sicurezza.

## Cosa evitare
- Non inserire chiavi segrete, credenziali o dati sensibili nel codice generato.
- Non modificare file generati (`bin/`, `obj/`, file di output) o file di build.
- Non applicare refactor invasivi senza verificare i test o chiedere conferma.

> Eseguire prima il build e i test locali prima di proporre modifiche estese.

## Comandi utili
```powershell
dotnet restore
dotnet build
dotnet test
```

## Struttura rilevante
- `src/Controllers` — API controllers
- `src/Services` — logica di business e interfacce
- `src/Entities` — modelli e DTO
- `tests/` — test xUnit

## Esempi di prompt consigliati
- "Implementa in `InventoryService` il metodo per ridurre la giacenza con controllo negativi e relativo test xUnit."
- "Aggiungi un test xUnit per `InventoryController.AdjustStock` che verifica la logica di riserva e rilascio."

## Linee guida commit
- Commit brevi e descrittivi. Esempio: `Add InventoryService tests` o in italiano `Aggiungi test InventoryService`.

## Note finali
- Se non sei sicuro su cambiamenti ampi (es. modifiche API, refactor globali, rewrite di storia Git), chiedi prima di applicare le modifiche.

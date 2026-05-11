# Istruzioni per la Code Review

## Scopo
- Fornire linee guida chiare e pratiche per revisionare le modifiche al codice di questo repository.

## Prima di aprire una PR
- Esegui i comandi locali: `dotnet restore`, `dotnet build`, `dotnet test`.
- Assicurati che la PR sia focalizzata (evita cambi troppo grandi).
- Aggiorna o aggiungi test automatici per le nuove funzionalità o bugfix.
- Documenta nel corpo della PR: cosa cambia, perché, come testare e issue correlati.

## Checklist rapida per il reviewer
- Correttezza: il codice implementa il comportamento atteso e i test passano.
- Test: sono presenti test unitari/integrati sufficienti; i test sono deterministici.
- API / contratti: le modifiche alle API sono compatibili o documentate come breaking change.
- Error handling: errori gestiti, `null`/edge-case considerati.
- Concurrency & cancellazione: usare `CancellationToken` dove necessario.
- Sicurezza: input sanitization, autorizzazioni, nessuna esposizione di segreti.
- Performance: non introdurre loop costosi o allocazioni inutili senza motivo.
- Logging & diagnostica: informazioni utili senza esporre dati sensibili.
- Manutenibilità: codice leggibile, responsabilità separate, nessuna duplicazione ovvia.
- Dipendenze: evita dipendenze non necessarie; rispetta i layer dell'architettura.
- CI e build: la pipeline deve essere verde; tutti i controlli automatici devono passare.
- Documentazione: aggiornare `README`, commenti o endpoint API se necessario.

## Linee guida per PR e commit
- Titolo PR: sintetico e descrittivo (es. "Aggiungi validazione quantità in InventoryService").
- Corpo PR: motivo, riepilogo delle modifiche, istruzioni per test manuale, riferimenti a issue.
- Commit message: riga di intestazione breve (imperativo presente), corpo opzionale con dettagli.
- Dimensione PR: preferire PR piccoli (es. < 300 LOC); per refactor grandi spezzare in step.
- Branch naming: `feature/descrizione`, `fix/descrizione`, `chore/descrizione`.

## Come commentare (etichetta e tono)
- Sii specifico: indica la riga e la proposta di modifica (es. suggerisci frammenti di codice).
- Usa linguaggio costruttivo e orientato alla soluzione; evita commenti personali.
- Segnala "nit" per questioni minori di stile; usa "Requested changes" per problemi bloccanti.

## Criteri per approvare vs richiedere modifiche
- Approva se: tutti i punti della checklist sono soddisfatti, i test passano e la PR è documentata.
- Richiedi modifiche se: mancano test, ci sono problemi di correttezza, sicurezza o design significativo.

## Esempi comandi utili
```powershell
git checkout -b feature/nome-funzionalita
dotnet restore
dotnet build
dotnet test
```

## Etichette e reviewer
- Aggiungi label `area/...` e `type/...` quando pertinenti (es. `area/inventory`, `type/bug`).
- Seleziona reviewer responsabili dell'area (owners, team lead o autori precedenti del codice).

## Merge e pubblicazione
- Prima di merge: assicurarsi che CI sia verde e che non ci siano conflitti aperti.
- Strategia di merge: preferire `Squash and merge` per piccole PR; per grandi cambi preservare la storia se utile.

## Hotfix e rollback
- Per hotfix in produzione: includere descrizione urgente, test che riproducono il problema e rollback plan.

## Note finali
- Quando in dubbio su decisioni architetturali o cambi che impattano molteplici moduli, coinvolgi i maintainer.
- Mantieni il focus sulla qualità del codice e sull'usabilità del sistema.

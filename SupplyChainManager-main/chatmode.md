# Modalità Chat (ChatMode)

## Scopo
- Fornire linee guida concise per l'uso del Copilot / Chat assistant su questo repository.

## Comportamento dell'assistente in chat
- Risposte brevi e operative: prima una sintesi (1-2 righe), poi i dettagli se necessari.
- Quando proponi modifiche al codice, fornisci patch minimali e chiari passi per applicarle.
- Includi comandi eseguibili (`powershell`) e snippet di codice quando utile.
- Se serve chiarimento, poni 1-2 domande mirate prima di applicare cambi rischiosi.

## Tono e formato
- Professionale, cortese e diretto.
- Usa elenchi puntati per passaggi e checklist.
- Quando proponi un cambiamento, indica il file e il percorso (es. `src/Services/InventoryService.cs`).

## Esempi di prompt efficaci
- "Aggiungi test xUnit per `InventoryController.AdjustStock` e mostra il file di test completo."
- "Correggi il bug in `InventoryService` che permette giacenze negative; proponi un patch."
- "Suggerisci miglioramenti per la validazione dell'input nel controller `ProductsController`."

## Regole operative
- Non includere mai segreti o credenziali nelle risposte.
- Non eseguire commit/push automatici a meno che l'utente non lo richieda esplicitamente.
- Preferisci cambi incrementali e testabili; aggiungi o aggiorna test per nuove funzionalità.

## Comandi utili
```powershell
dotnet restore
dotnet build
dotnet test
```

## Note finali
- Quando una modifica impatta le API pubbliche, segnala chiaramente il breaking change e aggiorna la documentazione.
- Se proporre un refactor ampio, suggerisci prima una PR di prova o spezzare il lavoro in più step.

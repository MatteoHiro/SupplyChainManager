# SupplyChainManager - Istruzioni di Build e Deploy

## Build del Progetto

```bash
# Ripristina i pacchetti
dotnet restore

# Build in modalità Release
dotnet build --configuration Release

# Pubblica l'applicazione
dotnet publish --configuration Release --output ./publish
```

## Setup Database

```bash
# Installa EF Core tools (se non già installato)
dotnet tool install --global dotnet-ef

# Crea la prima migration
dotnet ef migrations add InitialCreate

# Applica le migrations al database
dotnet ef database update

# Per ricreare il database da zero
dotnet ef database drop
dotnet ef database update
```

## Esecuzione in Sviluppo

```bash
# Modalità development con hot reload
dotnet watch run

# Oppure semplicemente
dotnet run
```

L'applicazione sarà disponibile su:
- HTTP: http://localhost:5000
- HTTPS: https://localhost:5001
- Swagger UI: http://localhost:5000 o https://localhost:5001

## Test delle API

### Con cURL
```bash
# Ottieni tutti i prodotti
curl -X GET "http://localhost:5000/api/products" -H "accept: application/json"

# Crea un nuovo fornitore
curl -X POST "http://localhost:5000/api/suppliers" \
  -H "Content-Type: application/json" \
  -d '{
    "companyName": "Nuovo Fornitore SRL",
    "contactName": "Giovanni Verdi",
    "email": "info@nuovofornitore.it",
    "phone": "+39 02 9876543",
    "city": "Torino",
    "country": "Italia",
    "leadTimeDays": 5
  }'
```

### Con PowerShell
```powershell
# Ottieni inventario con stock basso
Invoke-RestMethod -Uri "http://localhost:5000/api/inventory/low-stock" -Method Get

# Crea un nuovo ordine
$order = @{
    supplierId = 1
    expectedDeliveryDate = "2026-02-15"
    notes = "Ordine test"
    orderItems = @(
        @{
            productId = 1
            quantity = 50
            unitPrice = 12.50
        }
    )
} | ConvertTo-Json

Invoke-RestMethod -Uri "http://localhost:5000/api/orders" -Method Post -Body $order -ContentType "application/json"
```

## Troubleshooting

### Errore: "A connection was successfully established with the server..."
- Verifica che SQL Server LocalDB sia installato
- Prova a modificare la connection string in appsettings.json

### Errore: "The name 'DbContext' does not exist..."
- Esegui `dotnet restore`
- Verifica che tutti i pacchetti NuGet siano installati correttamente

### Swagger non si apre
- Verifica che il pacchetto Swashbuckle.AspNetCore sia installato
- Controlla che Startup.cs abbia la configurazione Swagger corretta

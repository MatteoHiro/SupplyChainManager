# 🏭 Supply Chain Management System

![.NET](https://img.shields.io/badge/.NET-6.0-512BD4?logo=dotnet)
![Entity Framework](https://img.shields.io/badge/Entity%20Framework-6.0-512BD4)
![SQL Server](https://img.shields.io/badge/SQL%20Server-LocalDB-CC2927?logo=microsoft-sql-server)
![License](https://img.shields.io/badge/license-MIT-green)

**Sistema professionale di gestione della supply chain** costruito con ASP.NET Core 6.0, Entity Framework Core e SQL Server. Questo progetto dimostra best practices nell'architettura a livelli, pattern repository, e API RESTful design.

## 🎯 Caratteristiche Principali

### Gestione Completa della Supply Chain
- ✅ **Gestione Fornitori** - Anagrafica completa con rating e lead time
- 📦 **Catalogo Prodotti** - SKU, prezzi, livelli di riordino
- 🏪 **Multi-Warehouse** - Gestione di più magazzini con tracking delle location
- 📊 **Inventory Management** - Stock tracking, riserve, disponibilità in tempo reale
- 🛒 **Order Processing** - Workflow completo degli ordini con stati multipli
- 🚚 **Shipment Tracking** - Tracciamento spedizioni con carrier e tracking number

### Funzionalità Avanzate
- 🔔 **Alert Stock Basso** - Monitoraggio automatico livelli di riordino
- 📈 **Analytics Magazzino** - Statistiche utilizzo e capacità
- 🔄 **Gestione Riserve** - Sistema di prenotazione stock per ordini
- 💰 **Calcolo Automatico Totali** - Totali ordini e subtotali articoli
- 📝 **Audit Trail** - Tracking creazione e modifiche con timestamp

## 🏗️ Architettura

```
SupplyChainManager/
├── src/
│   ├── Controllers/          # API RESTful endpoints
│   │   ├── ProductsController.cs
│   │   ├── SuppliersController.cs
│   │   ├── OrdersController.cs
│   │   ├── InventoryController.cs
│   │   └── WarehousesController.cs
│   │
│   ├── Entities/             # Domain Models
│   │   ├── Product.cs
│   │   ├── Supplier.cs
│   │   ├── Order.cs
│   │   ├── OrderItem.cs
│   │   ├── Warehouse.cs
│   │   ├── InventoryItem.cs
│   │   └── Shipment.cs
│   │
│   ├── Services/             # Business Logic Layer
│   │   ├── IInventoryService.cs
│   │   ├── InventoryService.cs
│   │   ├── IOrderService.cs
│   │   ├── OrderService.cs
│   │   ├── ISupplierService.cs
│   │   └── SupplierService.cs
│   │
│   ├── Models/               # Data Access Layer
│   │   └── AppDbContext.cs
│   │
│   ├── Program.cs
│   └── Startup.cs
│
├── appsettings.json
└── my-dotnet-ef-app.csproj
```

## 🚀 Quick Start

### Prerequisiti
- .NET 6.0 SDK
- SQL Server LocalDB (o SQL Server)
- Visual Studio 2022 / VS Code / Rider

### Installazione

1. **Clone il repository**
```bash
git clone https://github.com/tuousername/SupplyChainManager.git
cd SupplyChainManager
```

2. **Ripristina i pacchetti NuGet**
```bash
dotnet restore
```

3. **Configura la connection string** (se necessario)
Modifica `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=SupplyChainDb;Trusted_Connection=True;MultipleActiveResultSets=true"
  }
}
```

4. **Crea il database con le migrations**
```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

5. **Avvia l'applicazione**
```bash
dotnet run
```

6. **Apri Swagger UI**
Naviga su: `https://localhost:5001` o `http://localhost:5000`

## 📚 API Endpoints

### Prodotti
```http
GET    /api/products              # Ottieni tutti i prodotti
GET    /api/products/{id}         # Ottieni prodotto per ID
POST   /api/products              # Crea nuovo prodotto
PUT    /api/products/{id}         # Aggiorna prodotto
DELETE /api/products/{id}         # Elimina prodotto (soft delete)
```

### Fornitori
```http
GET    /api/suppliers             # Ottieni tutti i fornitori
GET    /api/suppliers/active      # Ottieni fornitori attivi
GET    /api/suppliers/{id}        # Ottieni fornitore per ID
GET    /api/suppliers/{id}/products  # Ottieni prodotti del fornitore
POST   /api/suppliers             # Crea nuovo fornitore
PUT    /api/suppliers/{id}        # Aggiorna fornitore
DELETE /api/suppliers/{id}        # Elimina fornitore (soft delete)
```

### Ordini
```http
GET    /api/orders                # Ottieni tutti gli ordini
GET    /api/orders/pending        # Ottieni ordini in sospeso
GET    /api/orders/{id}           # Ottieni ordine per ID
GET    /api/orders/supplier/{id}  # Ottieni ordini per fornitore
GET    /api/orders/{id}/total     # Calcola totale ordine
POST   /api/orders                # Crea nuovo ordine
PUT    /api/orders/{id}           # Aggiorna ordine
PATCH  /api/orders/{id}/status    # Aggiorna stato ordine
POST   /api/orders/{id}/cancel    # Cancella ordine
```

### Inventario
```http
GET    /api/inventory                    # Ottieni tutto l'inventario
GET    /api/inventory/low-stock          # Ottieni articoli con stock basso
GET    /api/inventory/{id}               # Ottieni item per ID
GET    /api/inventory/warehouse/{id}     # Ottieni inventario per magazzino
POST   /api/inventory                    # Aggiungi item inventario
PUT    /api/inventory/{id}               # Aggiorna item
POST   /api/inventory/{id}/adjust        # Regola quantità stock
POST   /api/inventory/reserve            # Riserva stock
POST   /api/inventory/release            # Rilascia stock riservato
```

### Magazzini
```http
GET    /api/warehouses            # Ottieni tutti i magazzini
GET    /api/warehouses/{id}       # Ottieni magazzino per ID
GET    /api/warehouses/{id}/stats # Ottieni statistiche magazzino
POST   /api/warehouses            # Crea nuovo magazzino
PUT    /api/warehouses/{id}       # Aggiorna magazzino
DELETE /api/warehouses/{id}       # Elimina magazzino (soft delete)
```

## 💡 Esempi di Utilizzo

### Creare un Nuovo Ordine
```json
POST /api/orders
{
  "supplierId": 1,
  "expectedDeliveryDate": "2026-02-01",
  "notes": "Ordine urgente",
  "orderItems": [
    {
      "productId": 1,
      "quantity": 100,
      "unitPrice": 12.50
    },
    {
      "productId": 2,
      "quantity": 50,
      "unitPrice": 8.75
    }
  ]
}
```

### Riserva Stock per un Ordine
```json
POST /api/inventory/reserve
{
  "productId": 1,
  "warehouseId": 1,
  "quantity": 100
}
```

### Ottieni Statistiche Magazzino
```http
GET /api/warehouses/1/stats

Response:
{
  "warehouseId": 1,
  "warehouseName": "Magazzino Centrale Milano",
  "totalProducts": 45,
  "totalQuantity": 5420,
  "totalReserved": 320,
  "totalAvailable": 5100,
  "capacityUsed": 5420,
  "capacityRemaining": 4580,
  "utilizationPercentage": 54.2
}
```

## 🛠️ Tecnologie Utilizzate

- **Framework**: ASP.NET Core 6.0
- **ORM**: Entity Framework Core 6.0
- **Database**: SQL Server / LocalDB
- **API Documentation**: Swagger/OpenAPI
- **JSON Serialization**: Newtonsoft.Json
- **Patterns**: Repository Pattern, Dependency Injection, Service Layer

## 📋 Modello Dati

### Entità Principali e Relazioni

```
Supplier (1) ──── (N) Product
    │                   │
    │                   │
   (N)                 (N)
    │                   │
  Order ──── (N) OrderItem
    │                   
   (1)                 
    │                   
 Shipment              

Product (1) ──── (N) InventoryItem (N) ──── (1) Warehouse
```

### Caratteristiche del Modello
- **Soft Delete** su entità principali (IsActive flag)
- **Audit Trail** con CreatedAt e UpdatedAt
- **Calculated Fields** (QuantityAvailable in InventoryItem)
- **Unique Constraints** su SKU, OrderNumber, TrackingNumber
- **Cascade Delete** configurato appropriatamente
- **Index** su foreign keys e campi di ricerca frequente

## 🎓 Concetti Dimostrati

Questo progetto dimostra competenze in:

1. **Clean Architecture** - Separazione in layers (Controllers, Services, Data)
2. **SOLID Principles** - Dependency Injection, Interface Segregation
3. **RESTful API Design** - HTTP verbs appropriati, status codes
4. **Entity Framework Core** - Migrations, Relationships, Fluent API
5. **Async/Await Pattern** - Operazioni asincrone throughout
6. **Business Logic** - Gestione ordini, riserve stock, calcoli
7. **Error Handling** - Validazione e gestione errori appropriata
8. **API Documentation** - Swagger/OpenAPI integration
9. **Data Seeding** - Dati di esempio per testing
10. **Domain-Driven Design** - Entità ricche con business logic

## 📝 Prossimi Sviluppi

- [ ] Autenticazione e Autorizzazione (JWT)
- [ ] Unit Tests e Integration Tests
- [ ] Logging strutturato (Serilog)
- [ ] Cache Layer (Redis)
- [ ] Docker containerization
- [ ] CI/CD Pipeline
- [ ] Frontend Dashboard (React/Angular)
- [ ] Real-time notifications (SignalR)
- [ ] Export reports (PDF/Excel)
- [ ] Multi-tenancy support

## 👨‍💻 Autore

Progetto sviluppato come portfolio per dimostrare competenze nello sviluppo di applicazioni enterprise.

## 📄 Licenza

Questo progetto è rilasciato sotto licenza MIT.

---

⭐ **Realizzato per dimostrare competenze in sviluppo di gestionali enterprise con .NET**

💼 **Ideale per colloqui nel settore supply chain e gestionali aziendali**
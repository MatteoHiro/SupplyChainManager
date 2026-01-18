using SupplyChainManager.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SupplyChainManager.Services
{
    public interface IInventoryService
    {
        Task<IEnumerable<InventoryItem>> GetAllInventoryAsync();
        Task<InventoryItem> GetInventoryByIdAsync(int id);
        Task<IEnumerable<InventoryItem>> GetInventoryByWarehouseAsync(int warehouseId);
        Task<IEnumerable<InventoryItem>> GetLowStockItemsAsync();
        Task<InventoryItem> AddInventoryAsync(InventoryItem item);
        Task<InventoryItem> UpdateInventoryAsync(InventoryItem item);
        Task<bool> AdjustStockAsync(int inventoryId, int quantityChange);
        Task<bool> ReserveStockAsync(int productId, int warehouseId, int quantity);
        Task<bool> ReleaseStockAsync(int productId, int warehouseId, int quantity);
    }
}

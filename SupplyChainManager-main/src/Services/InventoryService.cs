using Microsoft.EntityFrameworkCore;
using SupplyChainManager.Entities;
using SupplyChainManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SupplyChainManager.Services
{
    public class InventoryService : IInventoryService
    {
        private readonly AppDbContext _context;

        public InventoryService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<InventoryItem>> GetAllInventoryAsync()
        {
            return await _context.InventoryItems
                .Include(i => i.Product)
                .Include(i => i.Warehouse)
                .ToListAsync();
        }

        public async Task<InventoryItem> GetInventoryByIdAsync(int id)
        {
            return await _context.InventoryItems
                .Include(i => i.Product)
                .Include(i => i.Warehouse)
                .FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<IEnumerable<InventoryItem>> GetInventoryByWarehouseAsync(int warehouseId)
        {
            return await _context.InventoryItems
                .Include(i => i.Product)
                .Where(i => i.WarehouseId == warehouseId)
                .ToListAsync();
        }

        public async Task<IEnumerable<InventoryItem>> GetLowStockItemsAsync()
        {
            return await _context.InventoryItems
                .Include(i => i.Product)
                .Include(i => i.Warehouse)
                .Where(i => i.QuantityOnHand <= i.Product.ReorderLevel)
                .ToListAsync();
        }

        public async Task<InventoryItem> AddInventoryAsync(InventoryItem item)
        {
            item.LastRestocked = DateTime.UtcNow;
            item.UpdatedAt = DateTime.UtcNow;
            
            _context.InventoryItems.Add(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task<InventoryItem> UpdateInventoryAsync(InventoryItem item)
        {
            item.UpdatedAt = DateTime.UtcNow;
            
            _context.Entry(item).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task<bool> AdjustStockAsync(int inventoryId, int quantityChange)
        {
            var inventory = await _context.InventoryItems.FindAsync(inventoryId);
            if (inventory == null) return false;

            inventory.QuantityOnHand += quantityChange;
            inventory.UpdatedAt = DateTime.UtcNow;

            if (quantityChange > 0)
            {
                inventory.LastRestocked = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ReserveStockAsync(int productId, int warehouseId, int quantity)
        {
            var inventory = await _context.InventoryItems
                .FirstOrDefaultAsync(i => i.ProductId == productId && i.WarehouseId == warehouseId);

            if (inventory == null || inventory.QuantityAvailable < quantity)
                return false;

            inventory.QuantityReserved += quantity;
            inventory.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ReleaseStockAsync(int productId, int warehouseId, int quantity)
        {
            var inventory = await _context.InventoryItems
                .FirstOrDefaultAsync(i => i.ProductId == productId && i.WarehouseId == warehouseId);

            if (inventory == null || inventory.QuantityReserved < quantity)
                return false;

            inventory.QuantityReserved -= quantity;
            inventory.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }
    }
}

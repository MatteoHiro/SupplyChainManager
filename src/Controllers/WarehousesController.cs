using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyDotNetEfApp.Entities;
using MyDotNetEfApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyDotNetEfApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WarehousesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public WarehousesController(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Ottiene tutti i magazzini
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Warehouse>>> GetWarehouses()
        {
            var warehouses = await _context.Warehouses
                .Where(w => w.IsActive)
                .Include(w => w.InventoryItems)
                .ToListAsync();
            return Ok(warehouses);
        }

        /// <summary>
        /// Ottiene un magazzino specifico per ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<Warehouse>> GetWarehouse(int id)
        {
            var warehouse = await _context.Warehouses
                .Include(w => w.InventoryItems)
                    .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(w => w.Id == id);

            if (warehouse == null)
                return NotFound();

            return Ok(warehouse);
        }

        /// <summary>
        /// Crea un nuovo magazzino
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<Warehouse>> CreateWarehouse(Warehouse warehouse)
        {
            warehouse.CreatedAt = DateTime.UtcNow;
            _context.Warehouses.Add(warehouse);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetWarehouse), new { id = warehouse.Id }, warehouse);
        }

        /// <summary>
        /// Aggiorna un magazzino esistente
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateWarehouse(int id, Warehouse warehouse)
        {
            if (id != warehouse.Id)
                return BadRequest();

            _context.Entry(warehouse).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await WarehouseExists(id))
                    return NotFound();
                throw;
            }

            return NoContent();
        }

        /// <summary>
        /// Elimina un magazzino (soft delete)
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWarehouse(int id)
        {
            var warehouse = await _context.Warehouses.FindAsync(id);
            if (warehouse == null)
                return NotFound();

            warehouse.IsActive = false;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Ottiene statistiche su un magazzino
        /// </summary>
        [HttpGet("{id}/stats")]
        public async Task<ActionResult> GetWarehouseStats(int id)
        {
            var warehouse = await _context.Warehouses
                .Include(w => w.InventoryItems)
                    .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(w => w.Id == id);

            if (warehouse == null)
                return NotFound();

            var stats = new
            {
                warehouseId = id,
                warehouseName = warehouse.Name,
                totalProducts = warehouse.InventoryItems.Count,
                totalQuantity = warehouse.InventoryItems.Sum(i => i.QuantityOnHand),
                totalReserved = warehouse.InventoryItems.Sum(i => i.QuantityReserved),
                totalAvailable = warehouse.InventoryItems.Sum(i => i.QuantityAvailable),
                capacityUsed = warehouse.InventoryItems.Sum(i => i.QuantityOnHand),
                capacityRemaining = warehouse.Capacity - warehouse.InventoryItems.Sum(i => i.QuantityOnHand),
                utilizationPercentage = warehouse.Capacity > 0 
                    ? (decimal)warehouse.InventoryItems.Sum(i => i.QuantityOnHand) / warehouse.Capacity * 100 
                    : 0
            };

            return Ok(stats);
        }

        private async Task<bool> WarehouseExists(int id)
        {
            return await _context.Warehouses.AnyAsync(w => w.Id == id);
        }
    }
}

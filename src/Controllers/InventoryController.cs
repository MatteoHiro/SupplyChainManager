using Microsoft.AspNetCore.Mvc;
using SupplyChainManager.Entities;
using SupplyChainManager.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SupplyChainManager.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InventoryController : ControllerBase
    {
        private readonly IInventoryService _inventoryService;

        public InventoryController(IInventoryService inventoryService)
        {
            _inventoryService = inventoryService;
        }

        /// <summary>
        /// Ottiene tutto l'inventario
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<InventoryItem>>> GetInventory()
        {
            var items = await _inventoryService.GetAllInventoryAsync();
            return Ok(items);
        }

        /// <summary>
        /// Ottiene gli articoli con stock basso
        /// </summary>
        [HttpGet("low-stock")]
        public async Task<ActionResult<IEnumerable<InventoryItem>>> GetLowStockItems()
        {
            var items = await _inventoryService.GetLowStockItemsAsync();
            return Ok(items);
        }

        /// <summary>
        /// Ottiene un elemento dell'inventario per ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<InventoryItem>> GetInventoryItem(int id)
        {
            var item = await _inventoryService.GetInventoryByIdAsync(id);

            if (item == null)
                return NotFound();

            return Ok(item);
        }

        /// <summary>
        /// Ottiene l'inventario di un magazzino specifico
        /// </summary>
        [HttpGet("warehouse/{warehouseId}")]
        public async Task<ActionResult<IEnumerable<InventoryItem>>> GetInventoryByWarehouse(int warehouseId)
        {
            var items = await _inventoryService.GetInventoryByWarehouseAsync(warehouseId);
            return Ok(items);
        }

        /// <summary>
        /// Aggiunge un nuovo elemento all'inventario
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<InventoryItem>> CreateInventoryItem(InventoryItem item)
        {
            var created = await _inventoryService.AddInventoryAsync(item);
            return CreatedAtAction(nameof(GetInventoryItem), new { id = created.Id }, created);
        }

        /// <summary>
        /// Aggiorna un elemento dell'inventario
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateInventoryItem(int id, InventoryItem item)
        {
            if (id != item.Id)
                return BadRequest();

            await _inventoryService.UpdateInventoryAsync(item);
            return NoContent();
        }

        /// <summary>
        /// Regola la quantità di stock (può essere positivo o negativo)
        /// </summary>
        [HttpPost("{id}/adjust")]
        public async Task<IActionResult> AdjustStock(int id, [FromBody] int quantityChange)
        {
            var result = await _inventoryService.AdjustStockAsync(id, quantityChange);
            if (!result)
                return NotFound();

            return NoContent();
        }

        /// <summary>
        /// Riserva stock per un ordine
        /// </summary>
        [HttpPost("reserve")]
        public async Task<IActionResult> ReserveStock([FromBody] ReserveStockRequest request)
        {
            var result = await _inventoryService.ReserveStockAsync(
                request.ProductId, 
                request.WarehouseId, 
                request.Quantity);

            if (!result)
                return BadRequest("Stock insufficiente o articolo non trovato");

            return NoContent();
        }

        /// <summary>
        /// Rilascia stock riservato
        /// </summary>
        [HttpPost("release")]
        public async Task<IActionResult> ReleaseStock([FromBody] ReserveStockRequest request)
        {
            var result = await _inventoryService.ReleaseStockAsync(
                request.ProductId, 
                request.WarehouseId, 
                request.Quantity);

            if (!result)
                return BadRequest("Operazione non valida");

            return NoContent();
        }
    }

    public class ReserveStockRequest
    {
        public int ProductId { get; set; }
        public int WarehouseId { get; set; }
        public int Quantity { get; set; }
    }
}

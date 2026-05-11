using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SupplyChainManager.Entities;
using SupplyChainManager.Models;
using SupplyChainManager.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SupplyChainManager.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SuppliersController : ControllerBase
    {
        private readonly ISupplierService _supplierService;

        public SuppliersController(ISupplierService supplierService)
        {
            _supplierService = supplierService;
        }

        /// <summary>
        /// Ottiene tutti i fornitori
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Supplier>>> GetSuppliers()
        {
            var suppliers = await _supplierService.GetAllSuppliersAsync();
            return Ok(suppliers);
        }

        /// <summary>
        /// Ottiene solo i fornitori attivi
        /// </summary>
        [HttpGet("active")]
        public async Task<ActionResult<IEnumerable<Supplier>>> GetActiveSuppliers()
        {
            var suppliers = await _supplierService.GetActiveSuppliersAsync();
            return Ok(suppliers);
        }

        /// <summary>
        /// Ottiene un fornitore specifico per ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<Supplier>> GetSupplier(int id)
        {
            var supplier = await _supplierService.GetSupplierByIdAsync(id);

            if (supplier == null)
                return NotFound();

            return Ok(supplier);
        }

        /// <summary>
        /// Ottiene i prodotti di un fornitore
        /// </summary>
        [HttpGet("{id}/products")]
        public async Task<ActionResult<IEnumerable<Product>>> GetSupplierProducts(int id)
        {
            var products = await _supplierService.GetSupplierProductsAsync(id);
            return Ok(products);
        }

        /// <summary>
        /// Crea un nuovo fornitore
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<Supplier>> CreateSupplier(Supplier supplier)
        {
            var created = await _supplierService.CreateSupplierAsync(supplier);
            return CreatedAtAction(nameof(GetSupplier), new { id = created.Id }, created);
        }

        /// <summary>
        /// Aggiorna un fornitore esistente
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSupplier(int id, Supplier supplier)
        {
            if (id != supplier.Id)
                return BadRequest();

            await _supplierService.UpdateSupplierAsync(supplier);
            return NoContent();
        }

        /// <summary>
        /// Elimina un fornitore (soft delete)
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSupplier(int id)
        {
            var result = await _supplierService.DeleteSupplierAsync(id);
            if (!result)
                return NotFound();

            return NoContent();
        }
    }
}

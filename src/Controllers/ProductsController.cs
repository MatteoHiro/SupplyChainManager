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
    public class ProductsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProductsController(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Ottiene tutti i prodotti
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            var products = await _context.Products
                .Include(p => p.Supplier)
                .Where(p => p.IsActive)
                .ToListAsync();
            return Ok(products);
        }

        /// <summary>
        /// Ottiene un prodotto specifico per ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _context.Products
                .Include(p => p.Supplier)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
                return NotFound();

            return Ok(product);
        }

        /// <summary>
        /// Crea un nuovo prodotto
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct(Product product)
        {
            product.CreatedAt = DateTime.UtcNow;
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
        }

        /// <summary>
        /// Aggiorna un prodotto esistente
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, Product product)
        {
            if (id != product.Id)
                return BadRequest();

            product.UpdatedAt = DateTime.UtcNow;
            _context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await ProductExists(id))
                    return NotFound();
                throw;
            }

            return NoContent();
        }

        /// <summary>
        /// Elimina un prodotto (soft delete)
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return NotFound();

            product.IsActive = false;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private async Task<bool> ProductExists(int id)
        {
            return await _context.Products.AnyAsync(p => p.Id == id);
        }
    }
}

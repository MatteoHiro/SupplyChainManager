using Microsoft.EntityFrameworkCore;
using MyDotNetEfApp.Entities;
using MyDotNetEfApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyDotNetEfApp.Services
{
    public class SupplierService : ISupplierService
    {
        private readonly AppDbContext _context;

        public SupplierService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Supplier>> GetAllSuppliersAsync()
        {
            return await _context.Suppliers
                .Include(s => s.Products)
                .ToListAsync();
        }

        public async Task<Supplier> GetSupplierByIdAsync(int id)
        {
            return await _context.Suppliers
                .Include(s => s.Products)
                .Include(s => s.Orders)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<Supplier> CreateSupplierAsync(Supplier supplier)
        {
            supplier.CreatedAt = DateTime.UtcNow;
            supplier.IsActive = true;
            
            _context.Suppliers.Add(supplier);
            await _context.SaveChangesAsync();
            return supplier;
        }

        public async Task<Supplier> UpdateSupplierAsync(Supplier supplier)
        {
            _context.Entry(supplier).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return supplier;
        }

        public async Task<bool> DeleteSupplierAsync(int id)
        {
            var supplier = await _context.Suppliers.FindAsync(id);
            if (supplier == null) return false;

            // Soft delete
            supplier.IsActive = false;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Supplier>> GetActiveSuppliersAsync()
        {
            return await _context.Suppliers
                .Where(s => s.IsActive)
                .Include(s => s.Products)
                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetSupplierProductsAsync(int supplierId)
        {
            return await _context.Products
                .Where(p => p.SupplierId == supplierId && p.IsActive)
                .ToListAsync();
        }
    }
}

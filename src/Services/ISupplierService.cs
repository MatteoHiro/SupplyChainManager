using MyDotNetEfApp.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyDotNetEfApp.Services
{
    public interface ISupplierService
    {
        Task<IEnumerable<Supplier>> GetAllSuppliersAsync();
        Task<Supplier> GetSupplierByIdAsync(int id);
        Task<Supplier> CreateSupplierAsync(Supplier supplier);
        Task<Supplier> UpdateSupplierAsync(Supplier supplier);
        Task<bool> DeleteSupplierAsync(int id);
        Task<IEnumerable<Supplier>> GetActiveSuppliersAsync();
        Task<IEnumerable<Product>> GetSupplierProductsAsync(int supplierId);
    }
}

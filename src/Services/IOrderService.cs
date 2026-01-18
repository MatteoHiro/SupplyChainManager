using SupplyChainManager.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SupplyChainManager.Services
{
    public interface IOrderService
    {
        Task<IEnumerable<Order>> GetAllOrdersAsync();
        Task<Order> GetOrderByIdAsync(int id);
        Task<Order> CreateOrderAsync(Order order);
        Task<Order> UpdateOrderAsync(Order order);
        Task<bool> UpdateOrderStatusAsync(int orderId, OrderStatus status);
        Task<bool> CancelOrderAsync(int orderId);
        Task<IEnumerable<Order>> GetOrdersBySupplierAsync(int supplierId);
        Task<IEnumerable<Order>> GetPendingOrdersAsync();
        Task<decimal> CalculateOrderTotalAsync(int orderId);
    }
}

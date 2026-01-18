using Microsoft.EntityFrameworkCore;
using MyDotNetEfApp.Entities;
using MyDotNetEfApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyDotNetEfApp.Services
{
    public class OrderService : IOrderService
    {
        private readonly AppDbContext _context;

        public OrderService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            return await _context.Orders
                .Include(o => o.Supplier)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .Include(o => o.Shipment)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();
        }

        public async Task<Order> GetOrderByIdAsync(int id)
        {
            return await _context.Orders
                .Include(o => o.Supplier)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .Include(o => o.Shipment)
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<Order> CreateOrderAsync(Order order)
        {
            order.OrderDate = DateTime.UtcNow;
            order.CreatedAt = DateTime.UtcNow;
            order.Status = OrderStatus.Pending;

            // Generate order number
            var count = await _context.Orders.CountAsync();
            order.OrderNumber = $"ORD-{DateTime.UtcNow:yyyyMMdd}-{(count + 1):D4}";

            // Calculate total
            if (order.OrderItems != null && order.OrderItems.Any())
            {
                foreach (var item in order.OrderItems)
                {
                    item.Subtotal = item.Quantity * item.UnitPrice;
                }
                order.TotalAmount = order.OrderItems.Sum(oi => oi.Subtotal);
            }

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            return order;
        }

        public async Task<Order> UpdateOrderAsync(Order order)
        {
            order.UpdatedAt = DateTime.UtcNow;
            
            _context.Entry(order).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return order;
        }

        public async Task<bool> UpdateOrderStatusAsync(int orderId, OrderStatus status)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null) return false;

            order.Status = status;
            order.UpdatedAt = DateTime.UtcNow;

            if (status == OrderStatus.Delivered)
            {
                order.ActualDeliveryDate = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> CancelOrderAsync(int orderId)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null || order.Status == OrderStatus.Delivered) 
                return false;

            order.Status = OrderStatus.Cancelled;
            order.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Order>> GetOrdersBySupplierAsync(int supplierId)
        {
            return await _context.Orders
                .Include(o => o.OrderItems)
                .Where(o => o.SupplierId == supplierId)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Order>> GetPendingOrdersAsync()
        {
            return await _context.Orders
                .Include(o => o.Supplier)
                .Include(o => o.OrderItems)
                .Where(o => o.Status == OrderStatus.Pending || o.Status == OrderStatus.Confirmed)
                .OrderBy(o => o.OrderDate)
                .ToListAsync();
        }

        public async Task<decimal> CalculateOrderTotalAsync(int orderId)
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null) return 0;

            return order.OrderItems.Sum(oi => oi.Subtotal);
        }
    }
}

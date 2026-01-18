using Microsoft.AspNetCore.Mvc;
using MyDotNetEfApp.Entities;
using MyDotNetEfApp.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyDotNetEfApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        /// <summary>
        /// Ottiene tutti gli ordini
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
        {
            var orders = await _orderService.GetAllOrdersAsync();
            return Ok(orders);
        }

        /// <summary>
        /// Ottiene gli ordini in sospeso
        /// </summary>
        [HttpGet("pending")]
        public async Task<ActionResult<IEnumerable<Order>>> GetPendingOrders()
        {
            var orders = await _orderService.GetPendingOrdersAsync();
            return Ok(orders);
        }

        /// <summary>
        /// Ottiene un ordine specifico per ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(int id)
        {
            var order = await _orderService.GetOrderByIdAsync(id);

            if (order == null)
                return NotFound();

            return Ok(order);
        }

        /// <summary>
        /// Ottiene gli ordini di un fornitore
        /// </summary>
        [HttpGet("supplier/{supplierId}")]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrdersBySupplier(int supplierId)
        {
            var orders = await _orderService.GetOrdersBySupplierAsync(supplierId);
            return Ok(orders);
        }

        /// <summary>
        /// Crea un nuovo ordine
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<Order>> CreateOrder(Order order)
        {
            var created = await _orderService.CreateOrderAsync(order);
            return CreatedAtAction(nameof(GetOrder), new { id = created.Id }, created);
        }

        /// <summary>
        /// Aggiorna un ordine esistente
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder(int id, Order order)
        {
            if (id != order.Id)
                return BadRequest();

            await _orderService.UpdateOrderAsync(order);
            return NoContent();
        }

        /// <summary>
        /// Aggiorna lo stato di un ordine
        /// </summary>
        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateOrderStatus(int id, [FromBody] OrderStatus status)
        {
            var result = await _orderService.UpdateOrderStatusAsync(id, status);
            if (!result)
                return NotFound();

            return NoContent();
        }

        /// <summary>
        /// Cancella un ordine
        /// </summary>
        [HttpPost("{id}/cancel")]
        public async Task<IActionResult> CancelOrder(int id)
        {
            var result = await _orderService.CancelOrderAsync(id);
            if (!result)
                return BadRequest("Impossibile cancellare l'ordine");

            return NoContent();
        }

        /// <summary>
        /// Calcola il totale di un ordine
        /// </summary>
        [HttpGet("{id}/total")]
        public async Task<ActionResult<decimal>> CalculateOrderTotal(int id)
        {
            var total = await _orderService.CalculateOrderTotalAsync(id);
            return Ok(new { orderId = id, total });
        }
    }
}

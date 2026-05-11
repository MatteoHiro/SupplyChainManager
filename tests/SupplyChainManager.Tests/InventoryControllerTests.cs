using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SupplyChainManager.Controllers;
using SupplyChainManager.Entities;
using SupplyChainManager.Services;
using Xunit;

namespace SupplyChainManager.Tests
{
    public class InventoryControllerTests
    {
        [Fact]
        public async Task GetInventory_ReturnsOkWithItems()
        {
            var mockService = new Mock<IInventoryService>();
            var items = new List<InventoryItem>
            {
                new InventoryItem { Id = 1, ProductId = 1, WarehouseId = 1, QuantityOnHand = 10 },
                new InventoryItem { Id = 2, ProductId = 2, WarehouseId = 1, QuantityOnHand = 5 }
            };

            mockService.Setup(s => s.GetAllInventoryAsync()).ReturnsAsync(items);

            var controller = new InventoryController(mockService.Object);

            var actionResult = await controller.GetInventory();

            var ok = Assert.IsType<OkObjectResult>(actionResult.Result);
            var returned = Assert.IsAssignableFrom<IEnumerable<InventoryItem>>(ok.Value);
            Assert.Equal(2, returned.Count());
        }

        [Fact]
        public async Task GetInventoryItem_ReturnsNotFound_WhenMissing()
        {
            var mockService = new Mock<IInventoryService>();
            mockService.Setup(s => s.GetInventoryByIdAsync(1)).ReturnsAsync((InventoryItem)null);

            var controller = new InventoryController(mockService.Object);

            var actionResult = await controller.GetInventoryItem(1);

            Assert.IsType<NotFoundResult>(actionResult.Result);
        }

        [Fact]
        public async Task GetInventoryItem_ReturnsOk_WhenFound()
        {
            var mockService = new Mock<IInventoryService>();
            var item = new InventoryItem { Id = 1, ProductId = 1, WarehouseId = 1, QuantityOnHand = 10 };
            mockService.Setup(s => s.GetInventoryByIdAsync(1)).ReturnsAsync(item);

            var controller = new InventoryController(mockService.Object);

            var actionResult = await controller.GetInventoryItem(1);

            var ok = Assert.IsType<OkObjectResult>(actionResult.Result);
            var returned = Assert.IsType<InventoryItem>(ok.Value);
            Assert.Equal(item.Id, returned.Id);
        }
    }
}

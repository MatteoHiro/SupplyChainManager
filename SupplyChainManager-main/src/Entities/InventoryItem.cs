using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SupplyChainManager.Entities
{
    public class InventoryItem
    {
        [Key]
        public int Id { get; set; }

        public int ProductId { get; set; }

        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }

        public int WarehouseId { get; set; }

        [ForeignKey("WarehouseId")]
        public virtual Warehouse Warehouse { get; set; }

        public int QuantityOnHand { get; set; }

        public int QuantityReserved { get; set; }

        public int QuantityAvailable => QuantityOnHand - QuantityReserved;

        [MaxLength(50)]
        public string Location { get; set; }

        public DateTime LastRestocked { get; set; } = DateTime.UtcNow;

        public DateTime? LastCountDate { get; set; }

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}

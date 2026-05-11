using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SupplyChainManager.Entities
{
    public enum OrderStatus
    {
        Pending,
        Confirmed,
        Processing,
        Shipped,
        Delivered,
        Cancelled
    }

    public class Order
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string OrderNumber { get; set; }

        public int SupplierId { get; set; }

        [ForeignKey("SupplierId")]
        public virtual Supplier Supplier { get; set; }

        public DateTime OrderDate { get; set; } = DateTime.UtcNow;

        public DateTime? ExpectedDeliveryDate { get; set; }

        public DateTime? ActualDeliveryDate { get; set; }

        public OrderStatus Status { get; set; } = OrderStatus.Pending;

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }

        [MaxLength(500)]
        public string Notes { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        // Navigation properties
        public virtual ICollection<OrderItem> OrderItems { get; set; }
        public virtual Shipment Shipment { get; set; }
    }
}

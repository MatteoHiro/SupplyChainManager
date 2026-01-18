using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyDotNetEfApp.Entities
{
    public enum ShipmentStatus
    {
        Preparing,
        InTransit,
        Delivered,
        Delayed,
        Cancelled
    }

    public class Shipment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string TrackingNumber { get; set; }

        public int OrderId { get; set; }

        [ForeignKey("OrderId")]
        public virtual Order Order { get; set; }

        [MaxLength(100)]
        public string Carrier { get; set; }

        public DateTime ShipmentDate { get; set; }

        public DateTime? EstimatedDeliveryDate { get; set; }

        public DateTime? ActualDeliveryDate { get; set; }

        public ShipmentStatus Status { get; set; } = ShipmentStatus.Preparing;

        [Column(TypeName = "decimal(18,2)")]
        public decimal ShippingCost { get; set; }

        [MaxLength(500)]
        public string Notes { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }
    }
}

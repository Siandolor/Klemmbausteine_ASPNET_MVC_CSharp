// ============================================================================
// === File: Purchase.cs
// === Description: Defines the Purchase entity representing a procurement or 
// ===              supply order for building block products, including product 
// ===              linkage, pricing, quantity, and delivery tracking details.
// ============================================================================

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Klemmbausteine.Models
{
    public class Purchase
    {
        // --- Primary key of the purchase record
        [Key]
        public int Id { get; set; }

        // --- Foreign key referencing the related product
        [ForeignKey("Product")]
        public int ProductId { get; set; }

        // --- Navigation property linking to the purchased product
        public Product Product { get; set; } = default!;

        // --- Number of units ordered in the purchase
        [Required]
        public int Quantity { get; set; }

        // --- Unit price at the time of purchase
        [Column(TypeName = "decimal(10,2)")]
        public decimal UnitPrice { get; set; }

        // --- Expected delivery date for the purchase
        [DataType(DataType.Date)]
        public DateTime ExpectedDelivery { get; set; }

        // --- Actual delivery date, if completed
        [DataType(DataType.Date)]
        public DateTime? ActualDelivery { get; set; }

        // --- Read-only property indicating whether the order has been delivered
        public bool Delivered => ActualDelivery.HasValue;
    }
}
